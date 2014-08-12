using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using AForge.Imaging.Filters;

namespace BadgeImageCreator
{
	public partial class frmFilterSettings : Form
	{
		public frmFilterSettings()
		{
			InitializeComponent();
		}

		private IFilter _filter;
		public IFilter FilterToModify
		{
			get
			{
				return _filter;
			}
			set
			{
				if (_filter != null)
				{
					throw new ArgumentException("Cannot assign a filter to a dialog that's already been configured", "FilterToModify");
				}

				if (value == null)
				{
					throw new ArgumentNullException("Cannot assign a null to the filter configuration", "FilterToModify");
				}

				_filter = value;

				var filterProps = _filter.GetType().GetProperties();
				var baseProps = new HashSet<string>(from x in typeof(AForge.Imaging.Filters.BaseFilter).GetProperties() select x.Name);
				int pos = 0;
				bool bLeft = false;
				foreach (var prop in filterProps)
				{
					if (baseProps.Contains(prop.Name))
					{
						continue;
					}

					if (prop.PropertyType.IsArray)
					{
						// For those weird int[,] things
						continue;
					}

					bLeft = !bLeft;

					Label lblTitle = new Label();
					lblTitle.Name = "lbl" + prop.Name;
					lblTitle.AutoSize = true;
					lblTitle.Text = prop.Name;
					lblTitle.Left = bLeft ? 15 : grpSettings.Width / 2 + 15;
					lblTitle.Top = 15 + pos * lblTitle.Height;
					grpSettings.Controls.Add(lblTitle);

					if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(long) || prop.PropertyType == typeof(double) || prop.PropertyType == typeof(decimal))
					{
						var hsv = new System.Windows.Forms.HScrollBar();
						hsv.Name = "hsv" + prop.Name;

						hsv.Width = 200;
						hsv.Left = bLeft ? (grpSettings.Width / 2) - 10 - hsv.Width : grpSettings.Width - 10 - hsv.Width;
						hsv.Top = lblTitle.Top;

						if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(long))
						{
							if (prop.Name == "Divisor")
							{
								hsv.Minimum = 1;
							}
							else
							{
								hsv.Minimum = 0;
							}

							hsv.Maximum = 500;
							hsv.SmallChange = 1;
							hsv.LargeChange = 10;

							if (prop.PropertyType == typeof(int))
							{
								hsv.Scroll += (o, e) =>
								{
									var sb = (HScrollBar)o;
									prop.SetValue(_filter, sb.Value, null);
									RefreshDest();
								};
							}
							else
							{
								hsv.Scroll += (o, e) =>
								{
									var sb = (HScrollBar)o;
									prop.SetValue(_filter, (long)sb.Value, null);
									RefreshDest();
								};
							}
						}
						else if (prop.PropertyType == typeof(double) || prop.PropertyType == typeof(decimal))
						{
							hsv.Minimum = -100;
							hsv.Maximum = 100;
							hsv.SmallChange = 1;
							hsv.LargeChange = 10;

							if (prop.PropertyType == typeof(double))
							{
								hsv.Scroll += (o, e) =>
								{
									var sb = (HScrollBar)o;
									var nv = (double)sb.Value / 100.0;
									prop.SetValue(_filter, nv, null);
									RefreshDest();
								};
							}
							else
							{
								hsv.Scroll += (o, e) =>
								{
									var sb = (HScrollBar)o;
									var nv = (decimal)sb.Value / 100.0m;
									prop.SetValue(_filter, nv, null);
									RefreshDest();
								};
							}
						}

						grpSettings.Controls.Add(hsv);

						lblTitle.Left = hsv.Left - lblTitle.Width - 5;
					}
					else if (prop.PropertyType == typeof(bool))
					{
						var chkbx = new System.Windows.Forms.CheckBox();
						chkbx.Name = "chkbx" + prop.Name;
						chkbx.AutoSize = true;
						chkbx.Text = "";

						chkbx.Left = bLeft ? (grpSettings.Width / 2) - 20 - chkbx.Width : grpSettings.Width - 20 - chkbx.Width;
						chkbx.Top = lblTitle.Top;

						chkbx.CheckedChanged += (o, e) =>
						{
							var cb = (CheckBox)o;
							prop.SetValue(_filter, cb.Checked, null);
							RefreshDest();
						};

						grpSettings.Controls.Add(chkbx);

						lblTitle.Left = chkbx.Left - lblTitle.Width - 5;
					}

					if (!bLeft)
					{
						pos++;
					}
				}
			}
		}

		public Image BeforeFilter
		{
			set
			{
				pbSource.Image = value;
				RefreshDest();
			}
		}

		public void RefreshDest()
		{
			if (pbSource.Image != null)
			{
				var sourceImage = pbSource.Image as Bitmap;
				if ((this.FilterToModify as BaseFilter).FormatTranslations.ContainsKey(sourceImage.PixelFormat))
				{
					pbDest.Image = this.FilterToModify.Apply(sourceImage);
				}
			}
		}
	}
}
