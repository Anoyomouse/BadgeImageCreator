using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BadgeImageCreator
{
	public partial class ColorPicker : UserControl
	{
		public ColorPicker()
		{
			InitializeComponent();
		}

		public event EventHandler ChangeColorEvent;

		private void cmdDialog_Click(object sender, EventArgs e)
		{
			colorDialog1.Color = this.BackColor;
			if (colorDialog1.ShowDialog() == DialogResult.OK)
			{
				this.BackColor = colorDialog1.Color;
			}
		}

		protected override void OnBackColorChanged(EventArgs e)
		{
			base.OnBackColorChanged(e);
			if (this.ChangeColorEvent != null)
			{
				this.ChangeColorEvent(this, e);
			}
		}
	}
}
