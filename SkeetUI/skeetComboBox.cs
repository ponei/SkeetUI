using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace SkeetUI
{
    public partial class skeetComboBox : Panel
    {
        #region paramenters
        private string[] skeetItems = { "skeetItem1", "skeetItem2" };
        private int skeetIndex = 0;



        [Description("Items of the ComboBox"), Category("SkeetUI - ComboBox"), DefaultValue("")]
        public string[] Items
        {
            get { return skeetItems; }
            set
            {
                skeetItems = value;
                while (skeetItems != value) { }
                drawBox(boxOpen);
            }
        }

        [Description("Selected item index"), Category("SkeetUI - ComboBox"), DefaultValue("")]
        public int selectedIndex
        {
            get { return skeetIndex; }
            set
            {
                skeetIndex = value;
                drawBox(boxOpen);
            }
        }

        #endregion
        public skeetComboBox()
        {
            InitializeComponent();
            DoubleBuffered = true;

            originalH = Height;

            drawArrows();
            drawBox(boxOpen);
        }

        #region draw
        Bitmap closedArrow = new Bitmap(5, 4);
        Bitmap openArrow = new Bitmap(5, 4);

        int originalH;

        private void drawArrows()
        {
            using (Graphics g = Graphics.FromImage(closedArrow))
            {
                //first line
                Brush brush = new SolidBrush(Color.FromArgb(0, 0, 0));
                Pen pen = new Pen(brush);
                g.DrawLine(pen, 0, 0, 4, 0);

                //triangle
                brush = new SolidBrush(Color.FromArgb(151, 151, 151));
                Point[] DOWN = new Point[] { new Point(0, 1), new Point(5, 1), new Point(2, 4) };
                g.FillPolygon(brush, DOWN);
            }

            openArrow = (Bitmap)closedArrow.Clone();
            openArrow.RotateFlip(RotateFlipType.Rotate180FlipX);


        }



        private void drawBox(bool status)
        {
            if (status)
            {
                drawBoxOpen();
            }
            else
            {
                drawBoxClosed();
            }
        }

        private void drawBoxClosed()
        {
            Bitmap background = new Bitmap(Width, originalH);
            using (Graphics g = Graphics.FromImage(background))
            {
                //border out
                using (Brush brush = new SolidBrush(Color.FromArgb(10, 10, 10)))
                {
                    Pen cpen = new Pen(brush);
                    g.DrawRectangle(cpen, 0, 0, Width - 1, originalH - 1);
                }

                //gradient in
                Rectangle rc = new Rectangle(1, 1, Width - 2, originalH - 2);
                using (LinearGradientBrush brush = new LinearGradientBrush(rc, Color.FromArgb(31, 31, 31), Color.FromArgb(36, 36, 36), 90F))
                {
                    g.FillRectangle(brush, rc);
                }

                //arrow
                g.CompositingMode = CompositingMode.SourceOver;
                g.DrawImage(closedArrow, new Point(background.Width - 10, (originalH - 2) / 2));

                //text index
                if (skeetIndex <= skeetItems.Length && skeetIndex > -1 && skeetItems.Length != 0)
                {
                    Brush brush = new SolidBrush(Color.FromArgb(151, 151, 151));
                    Font drawf = new Font("Tahoma", 7, FontStyle.Regular);
                    g.DrawString(skeetItems[skeetIndex], drawf, brush, 8, 5);
                }
            }

            BackgroundImage = background;
            Height = originalH;
        }

        private void drawBoxOpen(int selected = -1)
        {
            if (skeetItems.Length > 0)
            {
                Bitmap background = new Bitmap(Width, originalH + (18 * skeetItems.Length));
                using (Graphics g = Graphics.FromImage(background))
                {
                    //border out 1° box
                    using (Brush brush = new SolidBrush(Color.FromArgb(10, 10, 10)))
                    {
                        Pen cpen = new Pen(brush);
                        g.DrawRectangle(cpen, 0, 0, Width - 1, originalH - 1);
                    }

                    //gradient in 1° box
                    Rectangle rc = new Rectangle(1, 1, Width - 2, originalH - 2);
                    using (LinearGradientBrush brush = new LinearGradientBrush(rc, Color.FromArgb(31, 31, 31), Color.FromArgb(36, 36, 36), 90F))
                    {
                        g.FillRectangle(brush, rc);
                    }

                    //border out 2° box
                    using (Brush brush = new SolidBrush(Color.FromArgb(10, 10, 10)))
                    {
                        Pen cpen = new Pen(brush);
                        g.DrawRectangle(cpen, 0, originalH, Width - 1, 18 * skeetItems.Length - 1);
                    }

                    //gradient in 2° box
                    rc = new Rectangle(1, 1 + originalH, Width - 2, 18 * skeetItems.Length - 2);
                    using (LinearGradientBrush brush = new LinearGradientBrush(rc, Color.FromArgb(31, 31, 31), Color.FromArgb(36, 36, 36), 90F))
                    {
                        g.FillRectangle(brush, rc);
                    }

                    //arrow
                    g.CompositingMode = CompositingMode.SourceOver;
                    g.DrawImage(openArrow, new Point(background.Width - 10, (originalH - 2) / 2));

                    //text index
                    if (skeetIndex <= skeetItems.Length && skeetIndex > -1 && skeetItems.Length != 0)
                    {
                        Brush brush = new SolidBrush(Color.FromArgb(151, 151, 151));
                        Font drawf = new Font("Tahoma", 7, FontStyle.Regular);
                        g.DrawString(skeetItems[skeetIndex], drawf, brush, 8, 5);
                    }
                }

                BackgroundImage = background;
                Height = originalH + (18 * skeetItems.Length);
            } else
            {
                drawBoxClosed();
            }
        }


        #endregion

        bool boxOpen = false;
        private void skeetComboBox_Resize(object sender, EventArgs e)
        {
            drawBox(boxOpen);
            //drawBoxClosed();
        }

        private void skeetComboBox_Click(object sender, EventArgs e)
        {
            boxOpen = !boxOpen;
            drawBox(boxOpen);
        }

        private void skeetComboBox_MouseMove(object sender, MouseEventArgs e)
        {

        }
    }
}
