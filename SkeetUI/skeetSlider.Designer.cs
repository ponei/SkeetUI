﻿namespace SkeetUI
{
    partial class skeetSlider
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
            this.pnlSlider = new System.Windows.Forms.Panel();
            this.lbTitleShadow = new SkeetUI.transparentLabel();
            this.lbTitle = new SkeetUI.transparentLabel();
            this.pnlSliderBox = new transparentPanel();
            this.lbSliderValue = new SkeetUI.outlineLabel();
            this.pnlSlider.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSlider
            // 
            this.pnlSlider.BackColor = System.Drawing.Color.Transparent;
            this.pnlSlider.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pnlSlider.Controls.Add(this.pnlSliderBox);
            this.pnlSlider.Controls.Add(this.lbSliderValue);
            this.pnlSlider.Location = new System.Drawing.Point(0, 17);
            this.pnlSlider.Name = "pnlSlider";
            this.pnlSlider.Size = new System.Drawing.Size(158, 23);
            this.pnlSlider.TabIndex = 1;
            // 
            // lbTitleShadow
            // 
            this.lbTitleShadow.Font = new System.Drawing.Font("Tahoma", 7.23F);
            this.lbTitleShadow.ForeColor = System.Drawing.Color.Black;
            this.lbTitleShadow.Location = new System.Drawing.Point(16, 2);
            this.lbTitleShadow.Name = "lbTitleShadow";
            this.lbTitleShadow.Size = new System.Drawing.Size(127, 13);
            this.lbTitleShadow.TabIndex = 4;
            this.lbTitleShadow.TabStop = false;
            this.lbTitleShadow.Text = "skeetSlider";
            this.lbTitleShadow.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            // 
            // lbTitle
            // 
            this.lbTitle.Font = new System.Drawing.Font("Tahoma", 7.23F);
            this.lbTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(190)))));
            this.lbTitle.Location = new System.Drawing.Point(15, 1);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(127, 13);
            this.lbTitle.TabIndex = 3;
            this.lbTitle.TabStop = false;
            this.lbTitle.Text = "skeetSlider";
            this.lbTitle.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            // 
            // pnlSliderBox
            // 
            this.pnlSliderBox.BackColor = System.Drawing.Color.Fuchsia;
            this.pnlSliderBox.Location = new System.Drawing.Point(14, 0);
            this.pnlSliderBox.Name = "pnlSliderBox";
            this.pnlSliderBox.Size = new System.Drawing.Size(128, 8);
            this.pnlSliderBox.TabIndex = 6;
            this.pnlSliderBox.Click += new System.EventHandler(this.pnlSliderBox_Click);
            this.pnlSliderBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlSliderBox_MouseMove);
            // 
            // lbSliderValue
            // 
            this.lbSliderValue.AutoSize = true;
            this.lbSliderValue.Font = new System.Drawing.Font("Verdana", 7.2F, System.Drawing.FontStyle.Bold);
            this.lbSliderValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(190)))));
            this.lbSliderValue.Location = new System.Drawing.Point(65, 0);
            this.lbSliderValue.Name = "lbSliderValue";
            this.lbSliderValue.OutlineForeColor = System.Drawing.Color.Black;
            this.lbSliderValue.OutlineWidth = 1.5F;
            this.lbSliderValue.Size = new System.Drawing.Size(37, 12);
            this.lbSliderValue.TabIndex = 5;
            this.lbSliderValue.Text = "1.523";
            this.lbSliderValue.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // skeetSlider
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.Controls.Add(this.lbTitleShadow);
            this.Controls.Add(this.lbTitle);
            this.Controls.Add(this.pnlSlider);
            this.Font = new System.Drawing.Font("Verdana", 7.5F, System.Drawing.FontStyle.Bold);
            this.Name = "skeetSlider";
            this.Size = new System.Drawing.Size(158, 40);
            this.Resize += new System.EventHandler(this.skeetSlider_Resize);
            this.pnlSlider.ResumeLayout(false);
            this.pnlSlider.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnlSlider;
        private transparentLabel lbTitle;
        private transparentLabel lbTitleShadow;
        private outlineLabel lbSliderValue;
        private transparentPanel pnlSliderBox;
    }
}
