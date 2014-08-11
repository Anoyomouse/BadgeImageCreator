using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using AForge.Imaging;
using AForge.Imaging.Filters;
using System.IO;

namespace BadgeImageCreator
{
	/// <summary>Main form for the application</summary>
	public partial class frmBadge : Form
	{
		private FiltersSequence filterStack;

		/// <summary>Initializes a new instance of the <see cref="frmBadge"/> class.</summary>
		public frmBadge()
		{
			InitializeComponent();

			LoadFilters();
		}

		/// <summary>Loads the filters.</summary>
		private void LoadFilters()
		{
			List<Type> filterList = new List<Type>();
			List<string> notSimpleFilter = new List<string>();

			var baseFilterType = typeof(IFilter);
			var asm = baseFilterType.Assembly;
			foreach (var type in asm.GetTypes())
			{
				if (baseFilterType.IsAssignableFrom(type) ||
					type.IsSubclassOf(baseFilterType))
				{
					var constructor = type.GetConstructor(new Type[] { });

					if (constructor != null)
					{
						filterList.Add(type);
					}
					else
					{
						if (type.IsAbstract ||
							type.IsInterface)
						{
							continue;
						}

						if (type.Name != "TexturedFilter" &&
							type.Name != "ImageWarp")
						{
							var constructors = type.GetConstructors();
							notSimpleFilter.Add(string.Format("{0} - {1}", type.Name, constructors[0].ToString()));
						}
					}
				}
			}

			filterList.Sort(new Comparison<Type>((x, y) => x.Name.CompareTo(y.Name)));

			cmbFilters.Items.Clear();
			foreach(var type in filterList)
			{
				cmbFilters.Items.Add(type);
			}
			cmbFilters.SelectedIndex = 0;
		}

		/// <summary>Loads the old settings from the xml and sets up the program.</summary>
		private void LoadOldSettings()
		{
			var hasSettings = QuickSettings.Get["HasSettings"];
			if (string.IsNullOrWhiteSpace(hasSettings) || !hasSettings.Equals("true", StringComparison.InvariantCultureIgnoreCase))
			{
				return;
			}

			var previousFile = QuickSettings.Get["PreviousFile"];
			if (File.Exists(previousFile))
			{
				var file = new FileInfo(ofdImage.FileName);
				if (file.Exists)
				{
					pcFullImage.Image = System.Drawing.Image.FromFile(file.FullName);
				}
			}

			if (pcFullImage.Image != null)
			{
				_state = BoxState.None;

				int x, y, width, height;

				if (int.TryParse(QuickSettings.Get["BoxX"], out x) && int.TryParse(QuickSettings.Get["BoxY"], out y) &&
					int.TryParse(QuickSettings.Get["BoxWidth"], out width) && int.TryParse(QuickSettings.Get["BoxHeight"], out height))
				{
					_placed = true;
					_selection = new Rectangle(x, y, width, height);
				}
				else
				{
					_placed = false;
				}

				cmdProcess.PerformClick();
			}
		}

		/// <summary>Saves the settings to an xml file so they can be loaded on program start.</summary>
		private void SaveSettings()
		{
			QuickSettings.Get["HasSettings"] = "true";
			if (!string.IsNullOrWhiteSpace(ofdImage.FileName))
			{
				QuickSettings.Get["PreviousFile"] = ofdImage.FileName;
			}

			if (_placed)
			{
				QuickSettings.Get["BoxX"] = _selection.X.ToString();
				QuickSettings.Get["BoxY"] = _selection.Y.ToString();
				QuickSettings.Get["BoxWidth"] = _selection.Width.ToString();
				QuickSettings.Get["BoxHeight"] = _selection.Height.ToString();
			}
		}

		/// <summary>Handles the Click event of the Process button which crops the main image and applies the filter stack to generate the output image.</summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void cmdProcess_Click(object sender, EventArgs e)
		{
			if (!_placed)
			{
				return;
			}

			Bitmap b = new Bitmap(pbSource.Width, pbSource.Height);
			Graphics g = Graphics.FromImage(b);
			g.DrawImage(pcFullImage.Image, new Rectangle(0, 0, pbSource.Width, pbSource.Height), _selection.X, _selection.Y, _selection.Width, _selection.Height, GraphicsUnit.Pixel);
			g.Dispose();

			var sc = new SaturationCorrection();
			sc.AdjustValue = hsbSaturation.Value / 100.0f;
			// apply the filter
			sc.ApplyInPlace(b);

			pbSource.Image = b;

			var greyb = AForge.Imaging.Filters.Grayscale.CommonAlgorithms.BT709.Apply(b);

			var bc = new BrightnessCorrection();
			bc.AdjustValue = hsbBrightness.Value;
			bc.ApplyInPlace(greyb);

			var cc = new ContrastCorrection();
			cc.Factor = hsbContrast.Value;
			cc.ApplyInPlace(greyb);

			var edges = new Edges();
			edges.Divisor = 1;
			edges.Threshold = 300;

			edges.ApplyInPlace(greyb);

			pbInt.Image = greyb;

			//BaseInPlacePartialFilter filter = new AForge.Imaging.Filters.FloydSteinbergDithering();
			BaseInPlacePartialFilter filter = null;

			if (cmbAlgorithm.SelectedItem != null && cmbAlgorithm.SelectedItem is AForge.Imaging.Filters.BaseInPlacePartialFilter)
			{
				filter = cmbAlgorithm.SelectedItem as AForge.Imaging.Filters.BaseInPlacePartialFilter;
			}
			
			if (filter == null)
			{
				filter = new AForge.Imaging.Filters.SierraDithering();
			}

			var ditheredb = filter.Apply(greyb);
			pbDest.Image = ditheredb;
		}

		/// <summary>Populates the dithering modes dropdown with available dithering modes.</summary>
		private void LoadDitheringModes()
		{
			List<BaseInPlacePartialFilter> filters = new List<BaseInPlacePartialFilter>();
			
			filters.Add(new BurkesDithering());
			filters.Add(new FloydSteinbergDithering());
			filters.Add(new JarvisJudiceNinkeDithering());
			filters.Add(new SierraDithering());
			filters.Add(new StuckiDithering());

			cmbAlgorithm.Items.Clear();
			cmbAlgorithm.Items.AddRange(filters.ToArray());
			cmbAlgorithm.SelectedIndex = 0;
		}

		/// <summary>How wide is the resizing "box" around the selection rectangle</summary>
		private int BoxWidth = 4;
		/// <summary>Has the selection rectangle been placed</summary>
		private bool _placed = false;
		/// <summary>The selection rectangle on the main image</summary>
		private Rectangle _selection;
		/// <summary>The movement point used in moving, resizing and placing commands, generally mouse start point</summary>
		private Point _movement_point;
		/// <summary>The box placement/moving/resizing state</summary>
		private BoxState _state;
		/// <summary>The box flags to indicate where the mouse cursor is</summary>
		private BoxFlags _box_flags;

		/// <summary>Gets the box flags in relation to the selection rectangle.</summary>
		/// <param name="x">The mouse x position.</param>
		/// <param name="y">The mouse y position.</param>
		/// <returns></returns>
		private BoxFlags GetFlags(int x, int y)
		{
			BoxFlags resizeMask = BoxFlags.None;

			if ((x > _selection.X - BoxWidth && x < _selection.X + _selection.Width + BoxWidth) &&
				(y > _selection.Y - BoxWidth && y < _selection.Y + _selection.Height + BoxWidth))
			{
				// We're resizing, or moving!
			}
			else
			{
				return BoxFlags.None;
			}

			if ((x > _selection.X - BoxWidth && x < _selection.X + _selection.Width + BoxWidth) &&
				(y > _selection.Y - BoxWidth && y < _selection.Y + BoxWidth))
			{
				resizeMask |= BoxFlags.Top;
			}

			if ((x > _selection.X - BoxWidth && x < _selection.X + BoxWidth) &&
				(y > _selection.Y - BoxWidth && y < _selection.Y + _selection.Height + BoxWidth))
			{
				resizeMask |= BoxFlags.Left;
			}

			if ((x > _selection.X - BoxWidth && x < _selection.X + _selection.Width + BoxWidth) &&
				(y > _selection.Y + _selection.Height - BoxWidth && y < _selection.Y + _selection.Height + BoxWidth))
			{
				resizeMask |= BoxFlags.Bottom;
			}

			if ((x > _selection.X + _selection.Width - BoxWidth && x < _selection.X + _selection.Width + BoxWidth) &&
				(y > _selection.Y - BoxWidth && y < _selection.Y + _selection.Height + BoxWidth))
			{
				resizeMask |= BoxFlags.Right;
			}

			if (resizeMask == BoxFlags.None)
				resizeMask = BoxFlags.All;

			return resizeMask;
		}

		/// <summary>Handles the MouseDown event of the pictureBox1 control to start a resize, placement or movement event.</summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
		private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
		{
			// Starting point of the selection:
			if (e.Button == MouseButtons.Left)
			{
				if (!_placed)
				{
					_selection = new Rectangle(new Point(e.X, e.Y), new Size());
					_movement_point = new Point(e.X, e.Y);
					_placed = true;

					_state = BoxState.Placing;
				}
				else
				{
					if ((e.X > _selection.X - BoxWidth && e.X < _selection.X + _selection.Width + BoxWidth) &&
						(e.Y > _selection.Y - BoxWidth && e.Y < _selection.Y + _selection.Height + BoxWidth))
					{
						var resizeMask = GetFlags(e.X, e.Y);
						if (resizeMask == BoxFlags.All)
						{
							_state = BoxState.Moving;
							_box_flags = resizeMask;
							_movement_point = new Point(e.X - _selection.X, e.Y - _selection.Y);

							this.Text = string.Format("MouseDown({0}, {1}, {2})", e.X, e.Y, _state);
						}
						else if (resizeMask != BoxFlags.None)
						{
							_state = BoxState.Resizing;
							_box_flags = resizeMask;

							int x = e.X, y = e.Y;
							if ((_box_flags | BoxFlags.Right) == BoxFlags.Right)
								x = (_selection.X + _selection.Width) - e.X;
							if ((_box_flags | BoxFlags.Bottom) == BoxFlags.Bottom)
								y = (_selection.Y + _selection.Height) - e.Y;

							_movement_point = new Point(x, y);
							this.Text = string.Format("MouseDown({0}, {1}, {2})", e.X, e.Y, _state);
						}
					}
					else
					{
						_selection = new Rectangle(new Point(e.X, e.Y), new Size());
						_movement_point = new Point(e.X, e.Y);
						_state = BoxState.Placing;
					}
				}
			}

			pcFullImage.Refresh();
		}

		/// <summary>Handles the MouseMove event of the pictureBox1 control to control the resize, placement or movement event.</summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
		private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
		{
			float ratiow = 176f / 264f;
			float ratioh = 264f / 176f;

			// Update the actual size of the selection:
			if (_state == BoxState.Placing)
			{

				_selection.X = Math.Min(e.X, _movement_point.X);
				_selection.Width = Math.Max(e.X, _movement_point.X) - _selection.X;

				if (Math.Min(e.Y, _movement_point.Y) == _movement_point.Y)
				{
					// _movement_point.Y is top, and it will grow down
					_selection.Y = _movement_point.Y;
					_selection.Height = (int)(_selection.Width * ratiow);
				}
				else
				{
					// _movement_point.Y is bottom, and it will grow up

					// Calculate the "top"
					// First we get the actual height of the box - ratio calculated
					_selection.Height = (int)(_selection.Width * ratiow);

					// Now we adjust the "top" so the bottom is at _movement_point.Y
					_selection.Y = _movement_point.Y - _selection.Height;
				}
			}
			else if (_state == BoxState.Moving)
			{
				// Get Delta X of movement
				_selection.X = e.X - _movement_point.X;
				_selection.Y = e.Y - _movement_point.Y;
			}
			else if (_state == BoxState.Resizing)
			{
				// We have a static point - movement_point, where the resize event started!
				if (_box_flags.HasFlag(BoxFlags.Right))
				{
					// We need to make the box go from _selection.X -> e.X
					_selection.Width = e.X - _selection.X;
					if (_box_flags.HasFlag(BoxFlags.Top))
					{
						var bottom = _selection.Y + _selection.Height;
						_selection.Height = (int)(_selection.Width * ratiow);

						_selection.Y = bottom - _selection.Height;
					}
					else
					{
						_selection.Height = (int)(_selection.Width * ratiow);
					}
				}
				else if (_box_flags.HasFlag(BoxFlags.Left))
				{
					// We need the box to go from e.X to _selection.X + _selection.Width
					var right = _selection.X + _selection.Width;
					_selection.X = e.X;
					_selection.Width = right - e.X;

					if (_box_flags.HasFlag(BoxFlags.Bottom))
					{
						// Top Right should be fixed
						_selection.Height = (int)(_selection.Width * ratiow);
					}
					else
					{
						var bottom = _selection.Y + _selection.Height;
						_selection.Height = (int)(_selection.Width * ratiow);

						_selection.Y = bottom - _selection.Height;
					}
				}
				else if (_box_flags.HasFlag(BoxFlags.Bottom))
				{
					// We need the box to go from _selection.Y -> e.Y
					_selection.Height = e.Y - _selection.Y;
					_selection.Width = (int)(_selection.Height * ratioh);
				}
				else if (_box_flags.HasFlag(BoxFlags.Top))
				{
					// We need the box to go from e.Y to _selection.Y + _selection.Height
					// Will feel more "natural" if we make the left X shrink, unless BoxFlags.Right is set
					var bottom = _selection.Y + _selection.Height;
					_selection.Y = e.Y;
					_selection.Height = bottom - e.Y;

					var right = _selection.X + _selection.Width;
					var new_width = (int)(_selection.Height * ratioh);
					_selection.Width = new_width;
					_selection.X = right - new_width;
				}
			}

			if (_state != BoxState.None)
			{
				// Redraw the picturebox:
				pcFullImage.Refresh();
			}

			if (_state == BoxState.None)
			{
				if ((e.X > _selection.X - BoxWidth && e.X < _selection.X + _selection.Width + BoxWidth) &&
					(e.Y > _selection.Y - BoxWidth && e.Y < _selection.Y + _selection.Height + BoxWidth))
				{
					var flags = GetFlags(e.X, e.Y);
					if (flags == BoxFlags.All)
						pcFullImage.Cursor = Cursors.SizeAll;
					else if (flags == BoxFlags.Left || flags == BoxFlags.Right)
						pcFullImage.Cursor = Cursors.SizeWE;
					else if (flags == BoxFlags.Top || flags == BoxFlags.Bottom)
						pcFullImage.Cursor = Cursors.SizeNS;
					else if (flags == (BoxFlags.Top | BoxFlags.Left) || flags == (BoxFlags.Bottom | BoxFlags.Right))
						pcFullImage.Cursor = Cursors.SizeNWSE;
					else if (flags == (BoxFlags.Top | BoxFlags.Right) || flags == (BoxFlags.Bottom | BoxFlags.Left))
						pcFullImage.Cursor = Cursors.SizeNESW;
					else
						pcFullImage.Cursor = Cursors.No;
				}
				else
				{
					pcFullImage.Cursor = System.Windows.Forms.Cursors.Default;
				}
			}
		}

		/// <summary>Handles the MouseUp event of the pictureBox1 control to finsh the resize, placement or movement event.</summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
		private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
		{
			if (_state != BoxState.None)
			{
				cmdProcess.PerformClick();
				SaveSettings();
				_state = BoxState.None;
			}

			pcFullImage.Refresh();
		}

		/// <summary>Handles the Paint event of the pcFullImage control which redraws the selection rectangle.</summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="PaintEventArgs"/> instance containing the event data.</param>
		private void pcFullImage_Paint(object sender, PaintEventArgs e)
		{
			// Draw a rectangle displaying the current selection
			Pen pen = Pens.GreenYellow;
			e.Graphics.DrawRectangle(pen, _selection);

			if (_state != BoxState.None)
			{
				e.Graphics.DrawRectangle(Pens.GreenYellow, _movement_point.X - 2, _movement_point.Y - 2, 4, 4);
				e.Graphics.DrawRectangle(Pens.LimeGreen, _selection.X + _movement_point.X - 2, _selection.Y + _movement_point.Y - 2, 4, 4);
			}
		}

		/// <summary>Refreshes the image.</summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void RefreshImage(object sender, EventArgs e)
		{
			cmdProcess.PerformClick();
		}

		/// <summary>Handles the Click event of the cmdOpen control to open a new image for editing.</summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void cmdOpen_Click(object sender, EventArgs e)
		{
			if (ofdImage.ShowDialog() != DialogResult.OK)
			{
				return;
			}

			var file = new FileInfo(ofdImage.FileName);
			if (file.Exists)
			{
				pcFullImage.Image = System.Drawing.Image.FromFile(file.FullName);
				_placed = false;

				SaveSettings();

				cmdProcess.PerformClick();
			}
		}

		/// <summary>Handles the Load event of the frmBadgeCreator control.</summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void frmBadgeCreator_Load(object sender, EventArgs e)
		{
			lsvFilterStack.Items.Clear();
			filterStack = new FiltersSequence();

			LoadDitheringModes();

			LoadOldSettings();
		}

		/// <summary>
		/// Handles the SelectionChangeCommitted event of the cmbAlgorithm control to select a different dithering method.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void cmbAlgorithm_SelectionChangeCommitted(object sender, EventArgs e)
		{
			cmdProcess.PerformClick();
		}

		/// <summary>Handles the Click event of the pbDest control to save the output image.</summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void pbDest_Click(object sender, EventArgs e)
		{
			if (sfdResult.ShowDialog(this) == DialogResult.OK)
			{
				var file = new FileInfo(sfdResult.FileName);
				if (file.Exists)
				{
					file.Delete();
				}

				pbDest.Image.Save(file.FullName, System.Drawing.Imaging.ImageFormat.Png);
			}
		}

		/// <summary>Saves the image to wif.</summary>
		private void SaveImageToWif()
		{
			sfdResult.DefaultExt = ".WIF";
			if (sfdResult.ShowDialog(this) == DialogResult.OK)
			{
				var file = new FileInfo(sfdResult.FileName);
				if (file.Exists)
				{
					file.Delete();
				}

				Bitmap bitmap = pbDest.Image as Bitmap;
				var fileStream = new BinaryWriter(file.OpenWrite());

				short width = 264, height = 176;

				fileStream.Write(height);
				fileStream.Write(width);
				for (int j = 0; j < height; j ++)
				{
					for (int i = 0; i < width; i += 8)
					{
						byte pixelByte = 0;
						for (int bit_i = 0; bit_i < 8; bit_i ++)
						{
							var bit = bitmap.GetPixel(i + bit_i, j) != Color.White;
							pixelByte |= (byte)((bit ? 1 : 0) << bit_i);
						}
						fileStream.Write(pixelByte);
					}
					fileStream.Flush();
				}
				fileStream.Close();
			}
		}

		/// <summary>Loads the wif image.</summary>
		/// <param name="file">The file.</param>
		/// <returns></returns>
		private Bitmap LoadWifImage(FileInfo file)
		{
			if (file.Exists)
			{
				var fileStream = new BinaryReader(file.OpenRead());

				short width = 264, height = 176;

				height = fileStream.ReadInt16();
				width = fileStream.ReadInt16();

				Bitmap bitmap = new Bitmap(width, height);
				Graphics g = Graphics.FromImage(bitmap);
				g.Clear(Color.White);

				for (int j = 0; j < height; j ++)
				{
					for (int i = 0; i < width; i += 8)
					{
						byte pixelByte = fileStream.ReadByte();
						for (int bit_i = 0; bit_i < 8; bit_i ++)
						{
							bool bit = (pixelByte & (1 << bit_i)) != 0;
							if (bit) {
								bitmap.SetPixel(i + bit_i, j, Color.Black);
							}
						}
					}
				}
				fileStream.Close();

				return bitmap;
			}
			return null;
		}

		/// <summary>Handles the Click event of the cmdPreviewWIF control to prompt to load a WIF image and display the resultant image.</summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void cmdPreviewWIF_Click(object sender, EventArgs e)
		{
			if (ofdWifImage.ShowDialog() != System.Windows.Forms.DialogResult.OK)
			{
				return;
			}

			var fi = new FileInfo(ofdWifImage.FileName);
			if (!fi.Exists)
			{
				return;
			}

			frmPreviewWif frm = new frmPreviewWif();
			frm.PreviewImage = LoadWifImage(fi);
			frm.Show();
		}

		/// <summary>Handles the Click event of the cmdAddFilter control adding a filter to the filter stack.</summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void cmdAddFilter_Click(object sender, EventArgs e)
		{
			Type t = cmbFilters.SelectedItem as Type;

			if (t == null)
			{
				MessageBox.Show(string.Format("Cannot add this type as it's not a type? {0} ({1})", cmbFilters.SelectedItem, cmbFilters.SelectedItem.GetType()));
				return;
			}

			IFilter filter = (IFilter)Activator.CreateInstance(t);
			var settings = new frmFilterSettings();
			settings.FilterToModify = filter;

			var lvi = new ListViewItem(t.Name);
			lvi.Tag = settings;
			lsvFilterStack.Items.Add(lvi);

			filterStack.Add(filter);
		}

		/// <summary>
		/// Handles the SelectedIndexChanged event of the cmbFilters control setting the description text, etc.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void cmbFilters_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		/// <summary>Handles the DoubleClick event of the lsvFilterStack control.</summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void lsvFilterStack_DoubleClick(object sender, EventArgs e)
		{
			if (lsvFilterStack.SelectedItems.Count > 0)
			{
				var item = lsvFilterStack.SelectedItems[0];
				var frm = (item.Tag as frmFilterSettings);
				if (filterStack.Count > 1)
				{
					// Find frm.FilterToModify in filter stack
					// Apply all filters till we get to the filter we're working with
					// Pass source image (modified) into the form so we have a source image

					int pos = 0;
					var startImage = pbSource.Image.Clone() as Bitmap;
					foreach (var filter in filterStack)
					{
						if (filter == frm.FilterToModify)
						{
							break;
						}

						startImage = ((IFilter)filter).Apply(startImage);
						pos++;
					}

					frm.BeforeFilter = startImage;
				}
				else if (filterStack.Count == 1)
				{
					// If this is the only filter then our source is pbSource
					frm.BeforeFilter = pbSource.Image;
				}

				frm.ShowDialog(this);
			}
		}
	}

	public enum BoxState
	{
		None,
		Placing,
		Moving,
		Resizing
	}

	[Flags]
	public enum BoxFlags
	{
		None = 0,
		Top = 0x1,
		Bottom = 0x2,
		Left = 0x4,
		Right = 0x8,
		All = 0xF
	}
}
