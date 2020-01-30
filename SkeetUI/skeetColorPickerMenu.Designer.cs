namespace SkeetUI
{
    partial class skeetColorPickerMenu
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
            this.SuspendLayout();
            // 
            // skeetColorPickerMenu
            // 
            this.Size = new System.Drawing.Size(172, 172);
            this.Text = "skeetColorPickerMenu";
            this.Click += new System.EventHandler(this.skeetColorPickerMenu_Click);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.skeetColorPickerMenu_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.skeetColorPickerMenu_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.skeetColorPickerMenu_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
    }
}