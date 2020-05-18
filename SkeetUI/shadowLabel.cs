using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SkeetUI
{
    public class shadowLabel : Label
    {
        //sources:
        //https://gist.github.com/mjs3339/1dceee0c4d395eaaf01cc06107a8aaf9 - custom label

        public shadowLabel()
        {
            SetStyle(ControlStyles.DoubleBuffer, true);

            SetStyle(ControlStyles.ResizeRedraw, true);

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            SetStyle(ControlStyles.UserPaint, true);
        }


        protected override void OnPaint(PaintEventArgs e)
        {

            e.Graphics.FillRectangle(new SolidBrush(BackColor), ClientRectangle);

            using (Brush foreBrush = new SolidBrush(Color.FromArgb(0, 0, 0)))
            {
                e.Graphics.DrawString(Text, Font, foreBrush, 1, 1);
            }
            using (Brush foreBrush = new SolidBrush(ForeColor))
            {
                e.Graphics.DrawString(Text, Font, foreBrush, 0, 0);
            }




        }
    }
}