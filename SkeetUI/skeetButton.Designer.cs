﻿namespace SkeetUI
{
    partial class skeetButton
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
            // skeetButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "skeetButton";
            this.Size = new System.Drawing.Size(139, 40);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.skeetButton_MouseDown);
            this.MouseEnter += new System.EventHandler(this.skeetButton_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.skeetButton_MouseLeave);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.skeetButton_MouseUp);
            this.Resize += new System.EventHandler(this.skeetButton_Resize);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
