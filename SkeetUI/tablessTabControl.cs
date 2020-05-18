using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SkeetUI
{
    public class tablessTabControl : TabControl
    {
        protected override void WndProc(ref Message m)
        {
            // Hide tabs by trapping the TCM_ADJUSTRECT message
            if (m.Msg == 0x1328 && !DesignMode) m.Result = (IntPtr)1;
            else base.WndProc(ref m);
        }

        private void draw()
        {
            //chainmail
            Bitmap chain = new Bitmap(4, 4);
            using (Graphics g = Graphics.FromImage(chain))
            {
                g.Clear(Color.FromArgb(22, 22, 22));
                Brush b = new SolidBrush(Color.FromArgb(12, 12, 12));
                int x = this.Location.X % 2;
                g.FillRectangle(b, 0 + x, 0, 1, 2);
                g.FillRectangle(b, 0 + x, 3, 1, 1);
                g.FillRectangle(b, 2 + x, 1, 1, 3);
            }


            foreach (TabPage tb in this.TabPages)
            {
                tb.BackgroundImage = chain;
                tb.BackColor = Color.Transparent;
            }

        }

        protected override void OnPaintBackground(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        protected override void OnMove(EventArgs e)
        {
            base.OnMove(e);
            draw();
        }
    }
}