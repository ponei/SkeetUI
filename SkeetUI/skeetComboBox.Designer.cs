﻿namespace SkeetUI
{
    partial class skeetComboBox
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
            this.SuspendLayout();
            // 
            // skeetComboBox
            // 
            this.Size = new System.Drawing.Size(127, 20);
            this.Click += new System.EventHandler(this.skeetComboBox_Click);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.skeetComboBox_MouseMove);
            this.Resize += new System.EventHandler(this.skeetComboBox_Resize);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
