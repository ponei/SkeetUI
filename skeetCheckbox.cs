using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SkeetUI
{
    public partial class skeetCheckbox : UserControl
    {
        #region parameters
        private string skeetTitle = "skeetCheckbox";

        private bool boxChecked = false;
        private Color checkColor = Color.FromArgb(154, 197, 39);

        [Description("If checkbox is checked"), Category("SkeetUI - Checkbox"), DefaultValue("")]
        public bool Checked
        {
            get { return boxChecked; }
            set
            {
                boxChecked = value;
                drawCheckbox();
            }
        }
        [Description("Color of checkbox when checked"), Category("SkeetUI - Checkbox"), DefaultValue("")]
        public Color ColorChecked
        {
            get { return checkColor; }
            set
            {
                checkColor = value;
                drawCheckbox();
            }
        }
        [Description("Color of checkbox when checked"), Category("SkeetUI - Checkbox"), DefaultValue("")]
        public string CheckBoxTitle
        {
            get { return skeetTitle; }
            set
            {
                skeetTitle = value;
                shadowLabel.Text = skeetTitle;
                Width = 8 + shadowLabel.Width + 13;
                drawCheckbox();
            }
        }
        #endregion

        public skeetCheckbox()
        {
            InitializeComponent();
            DoubleBuffered = true;
            drawCheckbox();
        }

        #region draw
        private void drawCheckbox()
        {
            Bitmap background = new Bitmap(8, 12);
            using (Graphics g = Graphics.FromImage(background))
            {
                Color colorint = Color.FromArgb(75, 75, 75);
                Brush brushint = new SolidBrush(colorint);
                if (boxChecked)
                {
                    colorint = checkColor;
                    brushint = new SolidBrush(colorint);
                }

                float correctionf = -0.02f;
                for (int i = 1; 7 >= i; i++)
                {
                    Pen cpen = new Pen(brushint);
                    g.DrawLine(cpen, 1, i + 4, 6, i + 4);

                    //darker
                    colorint = ChangeColorBrightness(colorint, correctionf);
                    brushint = new SolidBrush(colorint);
                    correctionf -= 0.02f;
                }

                using (Brush brush = new SolidBrush(Color.FromArgb(0, 0, 0)))
                {
                    Pen cpen = new Pen(brush);
                    g.DrawRectangle(cpen, 0, 4, 7, 7);
                }
            }
            BackgroundImage = background;
        }

        #endregion

        private Color ChangeColorBrightness(Color color, float correctionFactor) //https://stackoverflow.com/questions/801406/c-create-a-lighter-darker-color-based-on-a-system-color
        {
            float red = (float)color.R;
            float green = (float)color.G;
            float blue = (float)color.B;

            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }

            return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            // EDIT: ADD AN EXTRA HEIGHT VALIDATION TO AVOID INITIALIZATION PROBLEMS
            // BITWISE 'AND' OPERATION: IF ZERO THEN HEIGHT IS NOT INVOLVED IN THIS OPERATION
            if ((specified & BoundsSpecified.Height) == 0 || height == 16)
            {
                base.SetBoundsCore(x, y, width, 16, specified);
            }
            else
            {
                return; // RETURN WITHOUT DOING ANY RESIZING
            }
        }

        private void skeetCheckbox_Click(object sender, EventArgs e)
        {
            boxChecked = !boxChecked;
            drawCheckbox();
        }

        private void shadowLabel_Click(object sender, EventArgs e)
        {
            boxChecked = !boxChecked;
            drawCheckbox();
        }
    }
}
