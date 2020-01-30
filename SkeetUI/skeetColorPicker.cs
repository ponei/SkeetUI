using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SkeetUI
{
    [DefaultEvent("ForeColorChanged")]
    public partial class skeetColorPicker : Panel
    {
        private Color pickerColor = Color.FromArgb(154, 197, 39); //154, 197, 39
        [Description("Selected color of the picker"), Category("SkeetUI - ColorPicker"), DefaultValue("")]
        public Color selectedColor
        {
            get { return pickerColor; }
            set
            {
                pickerColor = value;
                drawPicker();
                menu.setColor(pickerColor);
                newColor(null, pickerColor);
            }
        }

        private skeetColorPickerMenu menu = new skeetColorPickerMenu(1);



        public skeetColorPicker()
        {
            InitializeComponent();
            skeetColorPickerMenu.ColorChanged += newColor;
            DoubleBuffered = true;
            drawPicker();
        }

        private void newColor(object sender, Color color)
        {
            pickerColor = color;
            drawPicker();
            base.OnForeColorChanged(null);
        }


        #region draw
        private void drawPicker()
        {
            Bitmap background = new Bitmap(Width, Height);
            using (Graphics g = Graphics.FromImage(background))
            {
                using (Brush brush = new SolidBrush(Color.Black))
                {
                    Pen cpen = new Pen(brush);
                    g.DrawRectangle(cpen, 0, 0, background.Width - 1, background.Height - 1);
                }

                Rectangle rc = new Rectangle(1, 1, background.Width - 2, background.Height - 2);
                using (Brush brushgrad = new LinearGradientBrush(rc, pickerColor, ChangeColorBrightness(pickerColor, -0.3f), 90f))
                {
                    g.FillRectangle(brushgrad, rc);
                }
            }

            BackgroundImage = background;
        }

        //yoinked
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
            if ((specified & BoundsSpecified.Height) == 0 || height == 10)
            {
                base.SetBoundsCore(x, y, 18, 10, specified);
            }
            else
            {
                return; // RETURN WITHOUT DOING ANY RESIZING
            }
        }
        #endregion




        bool pickerOpen = false;
        private void skeetColorPicker_Click(object sender, EventArgs e)
        {
            pickerOpen = !pickerOpen;
            if (pickerOpen)
            {
                menu.setColor(pickerColor);
                if (Parent.Parent == null)
                {
                    //form
                    Parent.Controls.Add(menu);

                    int bounds = this.Location.X + menu.Width + 8, menuX;
                    if (bounds > Parent.Width - 8)
                    {
                        int subtr = bounds - (Parent.Width - 8);
                        menuX = this.Location.X - subtr;
                    }
                    else
                    {
                        menuX = this.Location.X;
                    }

                    menu.Location = new Point(menuX, this.Location.Y + Height + 5);
                }
                else
                {
                    Parent.Parent.Controls.Add(menu);

                    int bounds = Parent.Location.X + this.Location.X + menu.Width + 8, menuX;
                    if (bounds > Parent.Parent.Width - 8)
                    {
                        int subtr = bounds - (Parent.Parent.Width - 8);
                        menuX = Parent.Location.X + this.Location.X - subtr;
                    }
                    else
                    {
                        menuX = Parent.Location.X + this.Location.X;
                    }

                    menu.Location = new Point(menuX, Parent.Location.Y + this.Location.Y + Height + 5);
                }
                //menu.Location = new Point(290, 130);
                menu.BringToFront();
                menu.Show();
            }
            else
            {
                menu.Hide();
            }
        }
    }
}
