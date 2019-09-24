namespace SkeetUI
{
    partial class skeetCheckbox
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
            this.shadowLabel = new SkeetUI.shadowLabel();
            this.SuspendLayout();
            // 
            // shadowLabel
            // 
            this.shadowLabel.AutoSize = true;
            this.shadowLabel.EnableShadow = true;
            this.shadowLabel.Font = new System.Drawing.Font("Tahoma", 7.23F);
            this.shadowLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(190)))));
            this.shadowLabel.Location = new System.Drawing.Point(19, 1);
            this.shadowLabel.Name = "shadowLabel";
            this.shadowLabel.ShadowColor = System.Drawing.Color.Black;
            this.shadowLabel.ShadowOffset = 1;
            this.shadowLabel.Size = new System.Drawing.Size(74, 12);
            this.shadowLabel.TabIndex = 0;
            this.shadowLabel.Text = "skeetCheckbox";
            this.shadowLabel.Click += new System.EventHandler(this.shadowLabel_Click);
            // 
            // skeetCheckbox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Controls.Add(this.shadowLabel);
            this.Name = "skeetCheckbox";
            this.Size = new System.Drawing.Size(91, 16);
            this.Click += new System.EventHandler(this.skeetCheckbox_Click);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private shadowLabel shadowLabel;
    }
}
