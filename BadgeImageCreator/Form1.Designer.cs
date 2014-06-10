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
			this.pbInt = new AForge.Controls.PictureBox();
			this.pbDest = new AForge.Controls.PictureBox();
			this.pbSource = new AForge.Controls.PictureBox();
			this.pcFullImage = new AForge.Controls.PictureBox();
			this.panel1 = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.pbInt)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbDest)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pcFullImage)).BeginInit();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// cmdProcess
			// 
			this.cmdProcess.Location = new System.Drawing.Point(320, 194);
			this.cmdProcess.Name = "cmdProcess";
			this.cmdProcess.Size = new System.Drawing.Size(104, 23);
			this.cmdProcess.TabIndex = 2;
			this.cmdProcess.Text = "Process";
			this.cmdProcess.UseVisualStyleBackColor = true;
			this.cmdProcess.Click += new System.EventHandler(this.cmdProcess_Click);
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
			// 
			// pbSource
			// 
			this.pbSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pbSource.Image = ((System.Drawing.Image)(resources.GetObject("pbSource.Image")));
			this.pbSource.Location = new System.Drawing.Point(11, 12);
			this.pbSource.Name = "pbSource";
			this.pbSource.Size = new System.Drawing.Size(265, 177);
			this.pbSource.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pbSource.TabIndex = 3;
			this.pbSource.TabStop = false;
			// 
			// pcFullImage
			// 
			this.pcFullImage.Cursor = System.Windows.Forms.Cursors.Default;
			this.pcFullImage.Image = global::BadgeImageCreator.Properties.Resources.Mouse_Flower;
			this.pcFullImage.Location = new System.Drawing.Point(0, 0);
			this.pcFullImage.Name = "pcFullImage";
			this.pcFullImage.Size = new System.Drawing.Size(673, 673);
			this.pcFullImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pcFullImage.TabIndex = 3;
			this.pcFullImage.TabStop = false;
			this.pcFullImage.Click += new System.EventHandler(this.pcFullImage_Click);
			this.pcFullImage.Paint += new System.Windows.Forms.PaintEventHandler(this.pcFullImage_Paint);
			this.pcFullImage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
			this.pcFullImage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
			this.pcFullImage.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.BackColor = System.Drawing.Color.Transparent;
			this.panel1.Controls.Add(this.pcFullImage);
			this.panel1.Location = new System.Drawing.Point(12, 233);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(804, 489);
			this.panel1.TabIndex = 4;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(833, 727);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.pbInt);
			this.Controls.Add(this.pbDest);
			this.Controls.Add(this.pbSource);
			this.Controls.Add(this.cmdProcess);
			this.Name = "Form1";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.pbInt)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbDest)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pcFullImage)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button cmdProcess;
		private AForge.Controls.PictureBox pbSource;
		private AForge.Controls.PictureBox pbDest;
		private AForge.Controls.PictureBox pbInt;
		private AForge.Controls.PictureBox pcFullImage;
		private System.Windows.Forms.Panel panel1;
	}
}

