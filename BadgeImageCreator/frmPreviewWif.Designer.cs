/**
 * Copyright (c) David-John Miller AKA Anoyomouse 2014
 *
 * See LICENCE in the project directory for licence information
 **/
namespace BadgeImageCreator
{
	partial class frmPreviewWif
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.pbWifImage = new System.Windows.Forms.PictureBox();
			this.cmdClose = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.pbWifImage)).BeginInit();
			this.SuspendLayout();
			// 
			// pbWifImage
			// 
			this.pbWifImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pbWifImage.Location = new System.Drawing.Point(12, 12);
			this.pbWifImage.Name = "pbWifImage";
			this.pbWifImage.Size = new System.Drawing.Size(265, 177);
			this.pbWifImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pbWifImage.TabIndex = 4;
			this.pbWifImage.TabStop = false;
			// 
			// cmdClose
			// 
			this.cmdClose.Location = new System.Drawing.Point(157, 200);
			this.cmdClose.Name = "cmdClose";
			this.cmdClose.Size = new System.Drawing.Size(120, 32);
			this.cmdClose.TabIndex = 5;
			this.cmdClose.Text = "Close";
			this.cmdClose.UseVisualStyleBackColor = true;
			this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
			// 
			// frmPreviewWif
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 244);
			this.Controls.Add(this.cmdClose);
			this.Controls.Add(this.pbWifImage);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.Name = "frmPreviewWif";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Preview WIF Image";
			((System.ComponentModel.ISupportInitialize)(this.pbWifImage)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox pbWifImage;
		private System.Windows.Forms.Button cmdClose;
	}
}