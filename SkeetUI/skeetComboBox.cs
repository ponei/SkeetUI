using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SkeetUI
{
    public partial class skeetComboBox : Panel
    {
        #region paramenters
        private string[] skeetItems = { };
        private int skeetIndex = 0;
        private Color skeetColor = Color.FromArgb(154, 197, 39);



        [Description("Items of the ComboBox"), Category("SkeetUI - ComboBox"), DefaultValue("")]
        public string[] Items
        {
            get { return skeetItems; }
            set
            {
                skeetItems = value;
                if (skeetIndex >= value.Length)
                {
                    skeetIndex = value.Length - 1;
                }
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

        [Description("Selected item color"), Category("SkeetUI - ComboBox"), DefaultValue("")]
        public Color selectedColor
        {
            get { return skeetColor; }
            set
            {
                skeetColor = value;
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

        private Control parentInternal;
        private int parentIndex = -1;
        private Point locationInternal;

        int change = 1; //1 = close, 2 = open
        private void drawBox(bool status)
        {
            if (status)
            {
                drawBoxOpen();
                //go to the back control to break out of bounds
                if (change == 1 && Parent.Parent != null)
                {
                    change = 2;
                    if (parentIndex == -1)
                    {
                        parentIndex = Parent.Controls.IndexOf(this);
                    }
                    parentInternal = Parent;
                    locationInternal = Location;
                    Parent = Parent.Parent;
                    Location = new Point(Location.X + (parentInternal.Location.X), Location.Y + (parentInternal.Location.Y));
                    Parent.Controls.SetChildIndex(this, 0);
                }
            }
            else
            {
                drawBoxClosed();
                //bring back to original control
                if (change == 2 && parentInternal != null)
                {
                    change = 1;

                    Parent = parentInternal;
                    Parent.Controls.SetChildIndex(this, parentIndex);
                    Location = locationInternal;
                }
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

        private void drawBoxOpen(int over = -1)
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

                    //border out 2° box -- we skip for now, is called at the end

                    //gradient in 2° box
                    rc = new Rectangle(1, 1 + originalH, Width - 2, 18 * skeetItems.Length - 2);
                    using (Brush brush = new SolidBrush(Color.FromArgb(35, 35, 35)))
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

                    //texts
                    if (skeetItems.Length > 0)
                    {
                        Brush preto = new SolidBrush(Color.Black);
                        Brush branco = new SolidBrush(Color.FromArgb(208, 208, 208));
                        Brush colorSelect = new SolidBrush(skeetColor);
                        Brush selectBackground = new SolidBrush(Color.FromArgb(26, 26, 26));
                        Brush hoverBackground = new SolidBrush(Color.FromArgb(32, 32, 32));

                        Font drawr = new Font("Tahoma", 7, FontStyle.Regular);
                        Font drawb = new Font("Tahoma", 7, FontStyle.Bold);

                        for (int i = 0; skeetItems.Length > i; i++)
                        {
                            if (i == skeetIndex)
                            {
                                g.FillRectangle(selectBackground, new Rectangle(1, 21 + (i * 18), Width - 2, 18));
                                g.DrawString(skeetItems[i], drawb, preto, 9, 25 + (i * 18)); //shadow
                                g.DrawString(skeetItems[i], drawb, colorSelect, 8, 24 + (i * 18)); //text
                            }
                            else
                            if (i != skeetIndex && i == over)
                            {
                                g.FillRectangle(hoverBackground, new Rectangle(1, 21 + (i * 18), Width - 2, 18));
                                g.DrawString(skeetItems[i], drawb, preto, 9, 25 + (i * 18)); //shadow
                                g.DrawString(skeetItems[i], drawb, branco, 8, 24 + (i * 18)); //text
                            }
                            else
                            {
                                g.DrawString(skeetItems[i], drawr, preto, 9, 25 + (i * 18)); //shadow
                                g.DrawString(skeetItems[i], drawr, branco, 8, 24 + (i * 18)); //text
                            }

                        }
                    }

                    //border out 2° box -- hide some imperfections
                    using (Brush brush = new SolidBrush(Color.FromArgb(10, 10, 10)))
                    {
                        Pen cpen = new Pen(brush);
                        g.DrawRectangle(cpen, 0, originalH, Width - 1, 18 * skeetItems.Length - 1);
                    }
                }

                BackgroundImage = background;
                Height = originalH + (18 * skeetItems.Length);
            }
            else
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
            MouseEventArgs enew = (MouseEventArgs)e;
            if (enew.Location.Y > 20)
            {
                skeetIndex = (enew.Location.Y - 20) / 18;
                drawBox(boxOpen);
            }
            else
            {
                boxOpen = !boxOpen;
                drawBox(boxOpen);
            }



        }

        private void skeetComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Location.Y > 20)
            {
                int y = (e.Location.Y - 20) / 18;
                drawBoxOpen(y);
            }
        }
    }
}
