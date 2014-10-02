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
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BadgeImageCreator
{
	public partial class frmPreviewWif : Form
	{
		public frmPreviewWif()
		{
			InitializeComponent();
		}

		internal Image PreviewImage
		{
			set
			{
				pbWifImage.Image = value;
				pbWifImage.Refresh();
			}
		}

		private void cmdClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
