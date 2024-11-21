/**
 * Copyright (c) David-John Miller AKA Anoyomouse 2014
 *
 * See LICENCE in the project directory for licence information
 **/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using AForge.Imaging.Filters;
using AForge;

namespace BadgeImageCreator
{
	public partial class frmFilterSettings : Form, IDisposable
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

					if (prop.PropertyType == typeof(byte) || prop.PropertyType == typeof(short) || prop.PropertyType == typeof(int) ||
						prop.PropertyType == typeof(long) || prop.PropertyType == typeof(double) || prop.PropertyType == typeof(decimal))
					{
						if (prop.Name == "Channel")
						{
							var ddl = new ComboBox();

							ddl.Name = "lbl" + prop.Name;
							ddl.Text = prop.Name;
							ddl.Left = lblTitle.Left + lblTitle.Width;
							ddl.Top = 15 + pos * lblTitle.Height;
							ddl.Width = 200;

							ddl.DropDownStyle = ComboBoxStyle.DropDownList;

							ddl.Items.Add("R");
							ddl.Items.Add("G");
							ddl.Items.Add("B");
							ddl.Items.Add("A");

							// B G R A
							short val = (short)prop.GetValue(_filter, null);
							if (val == 0) ddl.SelectedItem = "B";
							if (val == 1) ddl.SelectedItem = "G";
							if (val == 2) ddl.SelectedItem = "R";
							if (val == 3) ddl.SelectedItem = "A";

							ddl.SelectedValueChanged += (o,e) =>
							{
								var cb = (ComboBox)o;
								short new_channel = AForge.Imaging.RGB.R;
								switch (cb.SelectedValue as string)
								{
									case "R": new_channel = AForge.Imaging.RGB.R; break;
									case "G": new_channel = AForge.Imaging.RGB.G; break;
									case "B": new_channel = AForge.Imaging.RGB.B; break;
									case "A": new_channel = AForge.Imaging.RGB.A; break;
								}

								prop.SetValue(_filter, new_channel, null);
								RefreshDest();
							};

							grpSettings.Controls.Add(ddl);
						}
						else
						{
							var hsv = new System.Windows.Forms.HScrollBar();
							hsv.Name = "hsv" + prop.Name;

							hsv.Width = 200;
							hsv.Left = bLeft ? (grpSettings.Width / 2) - 10 - hsv.Width : grpSettings.Width - 10 - hsv.Width;
							hsv.Top = lblTitle.Top;

							if (prop.PropertyType == typeof(byte) || prop.PropertyType == typeof(short) ||
								prop.PropertyType == typeof(int) || prop.PropertyType == typeof(long))
							{
								if (prop.Name == "Divisor")
								{
									hsv.Minimum = 1;
								}
								else
								{
									hsv.Minimum = 0;
								}

								if (prop.PropertyType != typeof(byte))
									hsv.Maximum = 500;
								else
									hsv.Maximum = 0xFF;
								hsv.SmallChange = 1;
								hsv.LargeChange = 10;

								if (prop.PropertyType == typeof(int))
								{
									if (prop.GetGetMethod() != null)
									{
										int newValue = (int)prop.GetValue(_filter, null);
										if (newValue > 500)
										{
											hsv.Maximum = newValue;
										}

										hsv.Value = newValue;
									}
									hsv.Scroll += (o, e) =>
									{
										var sb = (HScrollBar)o;
										prop.SetValue(_filter, sb.Value, null);
										RefreshDest();
									};
								}
								else if (prop.PropertyType == typeof(short))
								{
									hsv.Value = (short)prop.GetValue(_filter, null);
									hsv.Scroll += (o, e) =>
									{
										try
										{
											var sb = (HScrollBar)o;
											prop.SetValue(_filter, (short)sb.Value, null);
											RefreshDest();
										}
										catch (Exception ex)
										{
											MessageBox.Show("Error: " + ex.ToString());
										}
									};
								}
								else if (prop.PropertyType == typeof(long))
								{
									hsv.Value = (int)prop.GetValue(_filter, null);
									hsv.Scroll += (o, e) =>
									{
										var sb = (HScrollBar)o;
										prop.SetValue(_filter, (long)sb.Value, null);
										RefreshDest();
									};
								}
								else if (prop.PropertyType == typeof(byte))
								{
									hsv.Value = (byte)prop.GetValue(_filter, null);
									hsv.Scroll += (o, e) =>
									{
										var sb = (HScrollBar)o;
										byte val = (byte)(sb.Value & 0xFF);
										prop.SetValue(_filter, val, null);
										RefreshDest();
									};
								}
							}
							else if (prop.PropertyType == typeof(double) || prop.PropertyType == typeof(decimal))
							{
								hsv.Minimum = -1000;
								hsv.Maximum = 1000;
								hsv.SmallChange = 1;
								hsv.LargeChange = 10;

								if (prop.PropertyType == typeof(double))
								{
									hsv.Value = (int)(((double)prop.GetValue(_filter, null)) * 100);
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
									hsv.Value = (int)(((decimal)prop.GetValue(_filter, null)) * 100);
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
					}
					else if (prop.PropertyType == typeof(IntRange))
					{
						var hsvMin = new System.Windows.Forms.HScrollBar();
						var hsvMax = new System.Windows.Forms.HScrollBar();

						hsvMin.Name = "hsv" + prop.Name;

						hsvMin.Width = 100;
						hsvMin.Left = lblTitle.Left + lblTitle.Width;
						hsvMin.Top = lblTitle.Top;

						hsvMin.Minimum = 0;
						hsvMin.Maximum = 500;
						hsvMin.SmallChange = 1;
						hsvMin.LargeChange = 10;

						if (prop.GetGetMethod() != null)
						{
							IntRange newValue = (IntRange)prop.GetValue(_filter, null);
							hsvMin.Value = newValue.Min;
						}
						hsvMin.Scroll += (o, e) =>
						{
							var sb = (HScrollBar)o;
							int new_val = sb.Value;
							int max_val = hsvMax.Value;

							if (new_val > max_val)
							{
								max_val = new_val;
								hsvMax.Value = max_val;
							}

							this.Text = string.Format("Min: {0} - {1}", new_val, max_val);
							prop.SetValue(_filter, new IntRange(new_val, max_val), null);
							RefreshDest();
						};

						hsvMax.Name = "hsv" + prop.Name;

						hsvMax.Width = 100;
						hsvMax.Left = hsvMin.Left + hsvMin.Width;
						hsvMax.Top = lblTitle.Top;

						hsvMax.Minimum = 0;
						hsvMax.Maximum = 500;
						hsvMax.SmallChange = 1;
						hsvMax.LargeChange = 10;

						if (prop.GetGetMethod() != null)
						{
							IntRange newValue = (IntRange)prop.GetValue(_filter, null);
							hsvMax.Value = newValue.Max;
						}
						hsvMax.Scroll += (o, e) =>
						{
							var sb = (HScrollBar)o;
							int new_val = sb.Value;
							int min_val = hsvMin.Value;

							if (new_val < min_val)
							{
								min_val = new_val;
								hsvMin.Value = min_val;
							}

							this.Text = string.Format("Max: {0} - {1}", min_val, new_val);
							prop.SetValue(_filter, new IntRange(min_val, new_val), null);
							RefreshDest();
						};

						grpSettings.Controls.Add(hsvMin);
						grpSettings.Controls.Add(hsvMax);
					}
					else if (prop.PropertyType == typeof(bool))
					{
						var chkbx = new System.Windows.Forms.CheckBox();
						chkbx.Name = "chkbx" + prop.Name;
						chkbx.AutoSize = true;
						chkbx.Text = "";

						chkbx.Left = bLeft ? (grpSettings.Width / 2) - 20 - chkbx.Width : grpSettings.Width - 20 - chkbx.Width;
						chkbx.Top = lblTitle.Top;

						chkbx.Checked = (bool)prop.GetValue(_filter, null);
						chkbx.CheckedChanged += (o, e) =>
						{
							var cb = (CheckBox)o;
							prop.SetValue(_filter, cb.Checked, null);
							RefreshDest();
						};

						grpSettings.Controls.Add(chkbx);

						lblTitle.Left = chkbx.Left - lblTitle.Width - 5;
					}
					else if (prop.PropertyType == typeof(AForge.Imaging.RGB))
					{
						ColorPicker cpBox = new ColorPicker();
						cpBox.Name = "cpBox" + prop.Name;
						cpBox.Text = prop.PropertyType.Name;
						cpBox.Left = lblTitle.Left + lblTitle.Width + 20;
						cpBox.Top = lblTitle.Top;
						cpBox.Height = lblTitle.Height;
						cpBox.Width = 180;

						var oldValue = (AForge.Imaging.RGB)prop.GetValue(_filter, null);
						cpBox.BackColor = oldValue.Color;

						cpBox.ChangeColorEvent += (o, e) =>
						{
							var cb = (ColorPicker)o;
							Color c = cb.BackColor;
							prop.SetValue(_filter, new AForge.Imaging.RGB(c), null);
							RefreshDest();
						};

						grpSettings.Controls.Add(cpBox);
					}
					else
					{
						Label lblTypeName = new Label();
						lblTypeName.Name = "lbl" + prop.Name + "Type";
						lblTypeName.AutoSize = true;
						lblTypeName.Text = prop.PropertyType.Name;
						lblTypeName.Left = lblTitle.Left + lblTitle.Width + 20;
						lblTypeName.Top = lblTitle.Top;
						grpSettings.Controls.Add(lblTypeName);
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
				if ((this.FilterToModify as IFilterInformation).FormatTranslations.ContainsKey(sourceImage.PixelFormat))
				{
					try
					{
						pbDest.Image = this.FilterToModify.Apply(sourceImage);
						lblErrorText.Text = string.Empty;
					}
					catch (Exception e)
					{
						lblErrorText.Text = e.ToString();
					}
				}
				else
				{
					lblErrorText.Text = "Could not find: " + sourceImage.PixelFormat + " in " + (this.FilterToModify as IFilterInformation).FormatTranslations;
				}
			}
		}

		private void cmdApply_Click(object sender, EventArgs e)
		{
			if (this.ParentForm is frmBadge)
			{
				(this.ParentForm as frmBadge).Redraw();
			}
			this.Hide();
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			if (this.ParentForm is frmBadge)
			{
				(this.ParentForm as frmBadge).Redraw();
			}
			this.Hide();
		}

        private void frmFilterSettings_Load(object sender, EventArgs e)
        {

        }
    }
}
