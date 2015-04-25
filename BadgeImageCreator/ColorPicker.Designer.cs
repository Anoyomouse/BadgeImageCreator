namespace BadgeImageCreator
{
	partial class ColorPicker
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.cmdDialog = new System.Windows.Forms.Button();
			this.colorDialog1 = new System.Windows.Forms.ColorDialog();
			this.SuspendLayout();
			// 
			// cmdDialog
			// 
			this.cmdDialog.Dock = System.Windows.Forms.DockStyle.Right;
			this.cmdDialog.Location = new System.Drawing.Point(245, 0);
			this.cmdDialog.Name = "cmdDialog";
			this.cmdDialog.Size = new System.Drawing.Size(25, 25);
			this.cmdDialog.TabIndex = 0;
			this.cmdDialog.Text = "...";
			this.cmdDialog.UseVisualStyleBackColor = true;
			this.cmdDialog.Click += new System.EventHandler(this.cmdDialog_Click);
			// 
			// colorDialog1
			// 
			this.colorDialog1.Color = System.Drawing.Color.White;
			this.colorDialog1.SolidColorOnly = true;
			// 
			// ColorPicker
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.Controls.Add(this.cmdDialog);
			this.Name = "ColorPicker";
			this.Size = new System.Drawing.Size(270, 25);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button cmdDialog;
		private System.Windows.Forms.ColorDialog colorDialog1;
	}
}
