using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SkeetUI
{
    [DefaultEvent("Click")]
    public partial class skeetButton : UserControl
    {
        #region parameters
        private string skeetText = "skeetButton";
        private bool skeetEnabled = true;

        [Description("Text that is drawn in the button"), Category("SkeetUI - Button"), DefaultValue("")]
        public string ButtonText
        {
            get { return skeetText; }
            set
            {
                skeetText = value;
                drawButton();
            }
        }

        [Description("If button is enabled or not. Disables hovering and text gets darker"), Category("SkeetUI - Button"), DefaultValue("")]
        public bool ButtonEnabled
        {
            get { return skeetEnabled; }
            set
            {
                skeetEnabled = value;
                drawButton();
            }
        }
        #endregion

        public skeetButton()
        {
            InitializeComponent();
            DoubleBuffered = true;
            drawButton();
        }

        #region draw
        private void drawButton()
        {
            Bitmap background = new Bitmap(Width, Height);
            using (Graphics g = Graphics.FromImage(background))
            {
                //border out
                using (Brush brush = new SolidBrush(Color.FromArgb(0, 0, 0)))
                {
                    Pen cpen = new Pen(brush);
                    g.DrawRectangle(cpen, 0, 0, Width - 1, Height - 1);
                }

                //border in
                Color inborder = Color.FromArgb(50, 50, 50);
                using (Brush brush = new SolidBrush(inborder))
                {
                    Pen cpen = new Pen(brush);
                    g.DrawRectangle(cpen, 1, 1, Width - 3, Height - 3);
                }

                //background, gradient
                Rectangle rc = new Rectangle(2, 2, Width - 4, Height - 4);
                LinearGradientBrush brushgrad = null;
                if (down)
                {
                    brushgrad = new LinearGradientBrush(rc, Color.FromArgb(30, 30, 30), Color.FromArgb(20, 20, 20), 90F);
                }
                else if (hovering)
                {
                    brushgrad = new LinearGradientBrush(rc, Color.FromArgb(40, 40, 40), Color.FromArgb(30, 30, 30), 90F);
                }
                else
                {
                    brushgrad = new LinearGradientBrush(rc, Color.FromArgb(35, 35, 35), Color.FromArgb(25, 25, 25), 90F);
                }

                g.FillRectangle(brushgrad, rc);


                //text in the middle
                Rectangle rcd = new Rectangle(0, 0, Width, Height);
                Font drawf = new Font("Tahoma", 7, FontStyle.Bold);

                //https://stackoverflow.com/questions/2593675/graphics-drawstring-to-exactly-place-text-on-a-system-label -- reference
                SizeF lSize = g.MeasureString(skeetText, drawf, rcd.Width);
                PointF lPoint = new PointF(rcd.X + (rcd.Width - lSize.Width) / 2, rcd.Y + (rcd.Height - lSize.Height) / 2);

                Brush texto = new SolidBrush(Color.Black);
                g.DrawString(skeetText, drawf, texto, lPoint.X + 1, lPoint.Y + 1);

                texto = new SolidBrush(Color.FromArgb(203, 203, 203));
                if (!skeetEnabled)
                {
                    texto = new SolidBrush(Color.FromArgb(150, 150, 150));
                }
                g.DrawString(skeetText, drawf, texto, lPoint);
            }
            BackgroundImage = background;
        }

        #endregion

        private void skeetButton_Resize(object sender, EventArgs e)
        {
            drawButton();
        }

        bool hovering = false;
        bool down = false;
        private void skeetButton_MouseEnter(object sender, EventArgs e)
        {
            if (!skeetEnabled)
            {
                return;
            }

            hovering = true;
            drawButton();
        }

        private void skeetButton_MouseLeave(object sender, EventArgs e)
        {
            if (!skeetEnabled)
            {
                return;
            }

            hovering = false;
            drawButton();
        }

        protected override void OnClick(EventArgs e)
        {
            if (!skeetEnabled)
            {
                return;
            }
            base.OnClick(e);
        }

        private void skeetButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (!skeetEnabled)
            {
                return;
            }

            down = true;
            drawButton();
        }

        private void skeetButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (!skeetEnabled)
            {
                return;
            }

            down = false;
            drawButton();
        }
    }
}
