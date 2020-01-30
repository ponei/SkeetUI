using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SkeetUI
{
    public partial class skeetGroupBox : Panel
    {
        #region parameters
        private string skeetTitle = "skeetGroupBox";

        [Description("Title that is drawn on the box"), Category("SkeetUI - GroupBox"), DefaultValue("")]
        public string TitleText
        {
            get { return skeetTitle; }
            set
            {
                skeetTitle = value;
                drawBox();
            }
        }
        #endregion

        public skeetGroupBox()
        {
            InitializeComponent();
            DoubleBuffered = true;
            drawBox();
        }

        #region draw
        private void drawBox()
        {
            Bitmap background = new Bitmap(Width, Height);
            using (Graphics g = Graphics.FromImage(background))
            {
                //border out
                using (Brush brush = new SolidBrush(Color.FromArgb(0, 0, 0)))
                {
                    Pen cpen = new Pen(brush);
                    g.DrawRectangle(cpen, 0, 3, Width - 1, Height - 4);
                }

                //border in
                using (Brush brush = new SolidBrush(Color.FromArgb(48, 48, 48)))
                {
                    Pen cpen = new Pen(brush);
                    g.DrawRectangle(cpen, 1, 4, Width - 3, Height - 6);
                }

                //background
                using (Brush brush = new SolidBrush(Color.FromArgb(17, 17, 17)))
                {
                    g.FillRectangle(brush, 2, 5, Width - 5, Height - 8);
                }

                if (!string.IsNullOrWhiteSpace(skeetTitle))
                {
                    Font drawf = new Font("Tahoma", 7, FontStyle.Bold);
                    SizeF sizestr = g.MeasureString(skeetTitle, drawf);

                    background.SetPixel(10, 3, Color.Transparent);
                    for (int Xcount = 11; Xcount < sizestr.Width + 15; Xcount++)
                    {
                        background.SetPixel(Xcount, 3, Color.Transparent);
                        background.SetPixel(Xcount, 4, Color.Transparent);
                    }

                    Brush texto = new SolidBrush(Color.Black);
                    g.DrawString(skeetTitle, drawf, texto, 15, 0);
                    texto = new SolidBrush(Color.FromArgb(203, 203, 203));
                    g.DrawString(skeetTitle, drawf, texto, 14, -1);
                }
            }

            BackgroundImage = background;
        }
        #endregion

        private void skeetGroupBox_Resize(object sender, EventArgs e)
        {
            drawBox();
        }
    }
}
