using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace SkeetUI
{
    public class outlineLabel : Label
    {


        //sources:    
        //https://stackoverflow.com/questions/19842722/setting-a-font-with-outline-color-in-c-sharp - label outline
        public outlineLabel()
        {
            SetStyle(ControlStyles.DoubleBuffer, true);

            SetStyle(ControlStyles.ResizeRedraw, true);

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            SetStyle(ControlStyles.UserPaint, true);
            OutlineForeColor = Color.Green;
            OutlineWidth = 1.5f;
        }
        public Color OutlineForeColor { get; set; }
        public float OutlineWidth { get; set; }


        protected override void OnPaint(PaintEventArgs e)
        {
            
            e.Graphics.FillRectangle(new SolidBrush(BackColor), ClientRectangle);
            using (GraphicsPath gp = new GraphicsPath())
            using (Pen outline = new Pen(OutlineForeColor, OutlineWidth)
            { LineJoin = LineJoin.Round })
            using (StringFormat sf = new StringFormat())
            using (Brush foreBrush = new SolidBrush(ForeColor))
            {
                gp.AddString(Text, Font.FontFamily, (int)Font.Style,
                    Font.Size, ClientRectangle, sf);
                e.Graphics.ScaleTransform(1.3f, 1.35f);
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                e.Graphics.DrawPath(outline, gp);
                e.Graphics.FillPath(foreBrush, gp);
            }
            
        }
    }
}
