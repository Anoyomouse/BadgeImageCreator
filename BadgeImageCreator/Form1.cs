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
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

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

			var hf = new HistogramEqualization();

			if (chkHF1.Checked)
				hf.ApplyInPlace(b);

			pbSource.Image = b;

			var greyb = AForge.Imaging.Filters.Grayscale.CommonAlgorithms.BT709.Apply(b);

			if (chkHF2.Checked)
				hf.ApplyInPlace(greyb);

			var bc = new BrightnessCorrection();
			bc.AdjustValue = hsbBrightness.Value;
			bc.ApplyInPlace(greyb);

			var cc = new ContrastCorrection();
			cc.Factor = hsbContrast.Value;
			cc.ApplyInPlace(greyb);


			pbInt.Image = greyb;

			//BaseInPlacePartialFilter filter = new AForge.Imaging.Filters.FloydSteinbergDithering();
			BaseInPlacePartialFilter filter = new AForge.Imaging.Filters.SierraDithering();

			var ditheredb = filter.Apply(greyb);
			pbDest.Image = ditheredb;
		}

		private void DitheringModes()
		{
			List<BaseInPlacePartialFilter> filters = new List<BaseInPlacePartialFilter>();
			
			filters.Add(new BurkesDithering());
			filters.Add(new FloydSteinbergDithering());
			filters.Add(new JarvisJudiceNinkeDithering());
			filters.Add(new SierraDithering());
			filters.Add(new StuckiDithering());
		}

		private void rectangleShape1_Click(object sender, EventArgs e)
		{

		}

		private void pcFullImage_Click(object sender, EventArgs e)
		{

		}

		private int BoxWidth = 4;

		private bool _placed = false;
		private Rectangle _selection;

		private Point _movement_point;

		private BoxState _state;
		private BoxFlags _box_flags;

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

		private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
		{
			// Starting point of the selection:
			if (e.Button == MouseButtons.Left)
			{
				if (!_placed)
				{
					_selection = new Rectangle(new Point(e.X, e.Y), new Size());
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

							_movement_point = new Point(x,y);
							this.Text = string.Format("MouseDown({0}, {1}, {2})", e.X, e.Y, _state);
						}
					}
					else
					{
						_selection = new Rectangle(new Point(e.X, e.Y), new Size());
						_state = BoxState.Placing;
					}
				}
			}

			pcFullImage.Refresh();
		}

		private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
		{
			// Update the actual size of the selection:
			if (_state == BoxState.Placing)
			{
				float ratiow = 176f / 264f;

				_selection.X = Math.Min(e.X, _selection.X);

				_selection.Width = Math.Abs(e.X - _selection.X);
				_selection.Height = (int)(_selection.Width * ratiow);

				// Redraw the picturebox:
				pcFullImage.Refresh();
			}
			else if (_state == BoxState.Moving)
			{
				// Get Delta X of movement
				_selection.X = e.X - _movement_point.X;
				_selection.Y = e.Y - _movement_point.Y;

				pcFullImage.Refresh();
			}
			else if (_state == BoxState.Resizing)
			{
				if ((_box_flags | BoxFlags.Right) == BoxFlags.Right)
				{
					_selection.Width = e.X + _movement_point.X;
				}
				if ((_box_flags | BoxFlags.Bottom) == BoxFlags.Bottom)
				{
					_selection.Width = e.Y + _movement_point.Y;
				}
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

		private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
		{
			if (_state == BoxState.Placing)
			{
				float ratiow = 176f / 264f;

				_selection.Width = e.X - _selection.X;
				_selection.Height = (int)(_selection.Width * ratiow);
			}

			if (_state != BoxState.None)
			{
				cmdProcess.PerformClick();
			}

			if (_state != BoxState.None)
			{
				_state = BoxState.None;
			}

			pcFullImage.Refresh();
		}

		private void pcFullImage_Paint(object sender, PaintEventArgs e)
		{
			// Draw a rectangle displaying the current selection
			Pen pen = Pens.GreenYellow;
			e.Graphics.DrawRectangle(pen, _selection);

			if (_state == BoxState.Moving)
			{
				e.Graphics.DrawRectangle(Pens.LimeGreen, _movement_point.X - 2, _movement_point.Y - 2, 4, 4);
			}
		}

		private void RefreshImage(object sender, EventArgs e)
		{
			cmdProcess.PerformClick();
		}

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
				cmdProcess.PerformClick();
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
