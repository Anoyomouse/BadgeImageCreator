namespace BadgeImageCreator
{
	partial class Form1
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.cmdProcess = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.pcFullImage = new AForge.Controls.PictureBox();
			this.pbInt = new AForge.Controls.PictureBox();
			this.pbDest = new AForge.Controls.PictureBox();
			this.pbSource = new AForge.Controls.PictureBox();
			this.hsbContrast = new System.Windows.Forms.HScrollBar();
			this.hsbBrightness = new System.Windows.Forms.HScrollBar();
			this.hsbSaturation = new System.Windows.Forms.HScrollBar();
			this.chkHF1 = new System.Windows.Forms.CheckBox();
			this.chkHF2 = new System.Windows.Forms.CheckBox();
			this.cmdOpen = new System.Windows.Forms.Button();
			this.ofdImage = new System.Windows.Forms.OpenFileDialog();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.cmbAlgorithm = new System.Windows.Forms.ComboBox();
			this.sfdResult = new System.Windows.Forms.SaveFileDialog();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pcFullImage)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbInt)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbDest)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbSource)).BeginInit();
			this.SuspendLayout();
			// 
			// cmdProcess
			// 
			this.cmdProcess.Location = new System.Drawing.Point(12, 195);
			this.cmdProcess.Name = "cmdProcess";
			this.cmdProcess.Size = new System.Drawing.Size(104, 23);
			this.cmdProcess.TabIndex = 2;
			this.cmdProcess.Text = "Process";
			this.cmdProcess.UseVisualStyleBackColor = true;
			this.cmdProcess.Click += new System.EventHandler(this.cmdProcess_Click);
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.BackColor = System.Drawing.Color.Transparent;
			this.panel1.Controls.Add(this.pcFullImage);
			this.panel1.Location = new System.Drawing.Point(10, 276);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(811, 439);
			this.panel1.TabIndex = 4;
			// 
			// pcFullImage
			// 
			this.pcFullImage.Cursor = System.Windows.Forms.Cursors.Default;
			this.pcFullImage.Image = ((System.Drawing.Image)(resources.GetObject("pcFullImage.Image")));
			this.pcFullImage.Location = new System.Drawing.Point(0, 0);
			this.pcFullImage.Name = "pcFullImage";
			this.pcFullImage.Size = new System.Drawing.Size(776, 650);
			this.pcFullImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pcFullImage.TabIndex = 3;
			this.pcFullImage.TabStop = false;
			this.pcFullImage.Paint += new System.Windows.Forms.PaintEventHandler(this.pcFullImage_Paint);
			this.pcFullImage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
			this.pcFullImage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
			this.pcFullImage.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
			// 
			// pbInt
			// 
			this.pbInt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pbInt.Image = null;
			this.pbInt.Location = new System.Drawing.Point(281, 12);
			this.pbInt.Name = "pbInt";
			this.pbInt.Size = new System.Drawing.Size(265, 177);
			this.pbInt.TabIndex = 3;
			this.pbInt.TabStop = false;
			// 
			// pbDest
			// 
			this.pbDest.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pbDest.Image = null;
			this.pbDest.Location = new System.Drawing.Point(552, 12);
			this.pbDest.Name = "pbDest";
			this.pbDest.Size = new System.Drawing.Size(265, 177);
			this.pbDest.TabIndex = 3;
			this.pbDest.TabStop = false;
			this.pbDest.Click += new System.EventHandler(this.pbDest_Click);
			// 
			// pbSource
			// 
			this.pbSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pbSource.Image = null;
			this.pbSource.Location = new System.Drawing.Point(10, 12);
			this.pbSource.Name = "pbSource";
			this.pbSource.Size = new System.Drawing.Size(265, 177);
			this.pbSource.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pbSource.TabIndex = 3;
			this.pbSource.TabStop = false;
			// 
			// hsbContrast
			// 
			this.hsbContrast.Location = new System.Drawing.Point(318, 209);
			this.hsbContrast.Maximum = 127;
			this.hsbContrast.Minimum = -127;
			this.hsbContrast.Name = "hsbContrast";
			this.hsbContrast.Size = new System.Drawing.Size(157, 20);
			this.hsbContrast.TabIndex = 5;
			this.hsbContrast.ValueChanged += new System.EventHandler(this.RefreshImage);
			// 
			// hsbBrightness
			// 
			this.hsbBrightness.Location = new System.Drawing.Point(493, 209);
			this.hsbBrightness.Maximum = 255;
			this.hsbBrightness.Minimum = -255;
			this.hsbBrightness.Name = "hsbBrightness";
			this.hsbBrightness.Size = new System.Drawing.Size(157, 20);
			this.hsbBrightness.TabIndex = 6;
			this.hsbBrightness.ValueChanged += new System.EventHandler(this.RefreshImage);
			// 
			// hsbSaturation
			// 
			this.hsbSaturation.Location = new System.Drawing.Point(650, 209);
			this.hsbSaturation.Minimum = -100;
			this.hsbSaturation.Name = "hsbSaturation";
			this.hsbSaturation.Size = new System.Drawing.Size(157, 20);
			this.hsbSaturation.TabIndex = 6;
			this.hsbSaturation.ValueChanged += new System.EventHandler(this.RefreshImage);
			// 
			// chkHF1
			// 
			this.chkHF1.AutoSize = true;
			this.chkHF1.Location = new System.Drawing.Point(135, 199);
			this.chkHF1.Name = "chkHF1";
			this.chkHF1.Size = new System.Drawing.Size(15, 14);
			this.chkHF1.TabIndex = 7;
			this.chkHF1.UseVisualStyleBackColor = true;
			this.chkHF1.CheckedChanged += new System.EventHandler(this.RefreshImage);
			// 
			// chkHF2
			// 
			this.chkHF2.AutoSize = true;
			this.chkHF2.Checked = true;
			this.chkHF2.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkHF2.Location = new System.Drawing.Point(156, 198);
			this.chkHF2.Name = "chkHF2";
			this.chkHF2.Size = new System.Drawing.Size(15, 14);
			this.chkHF2.TabIndex = 8;
			this.chkHF2.UseVisualStyleBackColor = true;
			this.chkHF2.CheckedChanged += new System.EventHandler(this.RefreshImage);
			// 
			// cmdOpen
			// 
			this.cmdOpen.Location = new System.Drawing.Point(213, 195);
			this.cmdOpen.Name = "cmdOpen";
			this.cmdOpen.Size = new System.Drawing.Size(81, 23);
			this.cmdOpen.TabIndex = 9;
			this.cmdOpen.Text = "Open";
			this.cmdOpen.UseVisualStyleBackColor = true;
			this.cmdOpen.Click += new System.EventHandler(this.cmdOpen_Click);
			// 
			// ofdImage
			// 
			this.ofdImage.DefaultExt = "png";
			this.ofdImage.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.gif;*.png|All files|*.*";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(318, 193);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(46, 13);
			this.label1.TabIndex = 10;
			this.label1.Text = "Contrast";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(490, 198);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(56, 13);
			this.label2.TabIndex = 10;
			this.label2.Text = "Brightness";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(653, 196);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(55, 13);
			this.label3.TabIndex = 10;
			this.label3.Text = "Saturation";
			// 
			// cmbAlgorithm
			// 
			this.cmbAlgorithm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbAlgorithm.FormattingEnabled = true;
			this.cmbAlgorithm.Location = new System.Drawing.Point(317, 243);
			this.cmbAlgorithm.Name = "cmbAlgorithm";
			this.cmbAlgorithm.Size = new System.Drawing.Size(157, 21);
			this.cmbAlgorithm.TabIndex = 11;
			this.cmbAlgorithm.SelectionChangeCommitted += new System.EventHandler(this.cmbAlgorithm_SelectionChangeCommitted);
			// 
			// sfdResult
			// 
			this.sfdResult.DefaultExt = "PNG";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(833, 727);
			this.Controls.Add(this.cmbAlgorithm);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cmdOpen);
			this.Controls.Add(this.chkHF2);
			this.Controls.Add(this.chkHF1);
			this.Controls.Add(this.hsbSaturation);
			this.Controls.Add(this.hsbBrightness);
			this.Controls.Add(this.hsbContrast);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.pbInt);
			this.Controls.Add(this.pbDest);
			this.Controls.Add(this.pbSource);
			this.Controls.Add(this.cmdProcess);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pcFullImage)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbInt)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbDest)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button cmdProcess;
		private AForge.Controls.PictureBox pbSource;
		private AForge.Controls.PictureBox pbDest;
		private AForge.Controls.PictureBox pbInt;
		private AForge.Controls.PictureBox pcFullImage;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.HScrollBar hsbContrast;
		private System.Windows.Forms.HScrollBar hsbBrightness;
		private System.Windows.Forms.HScrollBar hsbSaturation;
		private System.Windows.Forms.CheckBox chkHF1;
		private System.Windows.Forms.CheckBox chkHF2;
		private System.Windows.Forms.Button cmdOpen;
		private System.Windows.Forms.OpenFileDialog ofdImage;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cmbAlgorithm;
		private System.Windows.Forms.SaveFileDialog sfdResult;
	}
}

