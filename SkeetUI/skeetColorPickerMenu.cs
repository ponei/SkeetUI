using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SkeetUI
{
    public partial class skeetColorPickerMenu : Panel
    {
        public skeetColorPickerMenu(int invoke) //just so it doesn't appear in the toolbox
        {
            InitializeComponent();
            DoubleBuffered = true;
            paletteRC = new Rectangle(0, 0, Width - 24, Height - 24);
            palette = new Bitmap(Width - 24, Height - 24);

            hueanglesRC = new Rectangle(0, 0, 8, Height - 24);
            hueangles = new Bitmap(8, Height - 24);

            brightnessRC = new Rectangle(0, 0, Width - 24, 8);
            brightness = new Bitmap(Width - 24, 8);

            menubase = new Bitmap(Width, Height);
            drawBase();
        }

        #region event
        public delegate void skeetChangeColor(object sender, Color color);
        public static event skeetChangeColor ColorChanged;

        private void ColorChange()
        {
            Color actualC = ColorFromHSV(hue, sat, value);
            ColorChanged?.Invoke(null, actualC);
        }

        #endregion

        #region draw
        Bitmap menubase;
        private void drawBase()
        {
            brushBlack = new LinearGradientBrush(paletteRC, Color.Transparent, Color.Black, 90F);
            using (Graphics g = Graphics.FromImage(menubase))
            {
                //////main
                //border 1
                using (Brush brush = new SolidBrush(Color.Black))
                {
                    Pen cpen = new Pen(brush);
                    g.DrawRectangle(cpen, 0, 0, Width - 1, Height - 1);
                }

                //border 2
                using (Brush brush = new SolidBrush(Color.FromArgb(128, 128, 128)))
                {
                    Pen cpen = new Pen(brush);
                    g.DrawRectangle(cpen, 1, 1, Width - 3, Height - 3);
                }

                //base
                using (Brush brush = new SolidBrush(Color.FromArgb(38, 38, 38)))
                {
                    g.FillRectangle(brush, 2, 2, Width - 4, Height - 4);
                }

                //picker outline
                using (Brush brush = new SolidBrush(Color.Black))
                {
                    Pen cpen = new Pen(brush);
                    g.DrawRectangle(cpen, 5, 5, Width - 23, Height - 23);
                }

                //hue outline
                using (Brush brush = new SolidBrush(Color.Black))
                {
                    Pen cpen = new Pen(brush);
                    g.DrawRectangle(cpen, Width - 15, 5, 9, Height - 23);
                }

                //brightness outline
                using (Brush brush = new SolidBrush(Color.Black))
                {
                    Pen cpen = new Pen(brush);
                    g.DrawRectangle(cpen, 5, Height - 15, Width - 23, 9);
                }
            }

            BackgroundImage = menubase;
        }


        LinearGradientBrush brushPicker;
        LinearGradientBrush brushBlack;

        double hue, sat, value, angleH, angleB;
        public void setColor(Color pickerColor, bool hueChange = true, bool brightChange = true, bool palleteChange = true, bool outHue = true)
        {
            //importantes
            if (outHue)
            {
                ColorToHSV(pickerColor, out hue, out sat, out value);
            }
            else
            {
                ColorToHSV(pickerColor, out sat, out value);
            }

            //MessageBox.Show("" + hue + " " + sat + " " + value);
            angleH = (Height - 28D) * hue;
            angleH = angleH / 360D;

            angleB = (Width - 28D) * value;

            //hue slider
            if (hueChange)
            {
                drawHue(true);
            }

            //bright slider
            if (brightChange)
            {
                drawBrightness(true);
            }

            //hue color to brushes/vars
            Color angleHue = hueangles.GetPixel(1, 1 + (int)angleH);
            brushPicker = new LinearGradientBrush(paletteRC, Color.White, angleHue, 0F);

            //palette
            if (palleteChange)
            {
                drawPallete(true);
            }


        }

        Rectangle paletteRC;
        Bitmap palette;
        private void drawPallete(bool box = false)
        {
            using (Graphics g = Graphics.FromImage(palette))
            {
                g.FillRectangle(brushPicker, paletteRC);
                g.FillRectangle(brushBlack, paletteRC);

            }

            Bitmap palWBox = (Bitmap)palette.Clone();
            if (box)
            {

                Pen penblack = new Pen(brushBlack);
                double x = ((sat * palette.Width));
                double y = palette.Height - (value * palette.Height);


                boxPalleteBounds(x, y, out x, out y);

                using (Graphics g = Graphics.FromImage(palWBox))
                {
                    using (Brush brush = new SolidBrush(Color.Black))
                    {
                        Pen cpen = new Pen(brush);
                        g.DrawRectangle(cpen, Convert.ToSingle(x), Convert.ToSingle(y), 4, 4);
                    }
                }
            }

            switch (box)
            {
                case false:
                    using (Graphics g = Graphics.FromImage(menubase))
                    {
                        g.DrawImage(palette, new Point(6, 6));
                    }
                    break;
                case true:
                    using (Graphics g = Graphics.FromImage(menubase))
                    {
                        g.DrawImage(palWBox, new Point(6, 6));
                    }
                    break;
            }

            BackgroundImage = menubase;
            Refresh();
        }

        private void drawBoxPallete(MouseEventArgs e)
        {
            Bitmap palWBox = (Bitmap)palette.Clone();
            using (Graphics g = Graphics.FromImage(palWBox))
            {
                int x = e.X - 8;
                int y = e.Y - 8;

                boxPalleteBounds(x, y, out x, out y);

                sat = (double)(x + 1) / (palette.Width);
                value = 1D - ((double)y / (palette.Height));

                Color newbrush = ColorFromHSV(hue, sat, value);
                setColor(newbrush, false, true, false, false);

                using (Brush brush = new SolidBrush(Color.Black))
                {
                    Pen cpen = new Pen(brush);
                    g.DrawRectangle(cpen, x, y, 4, 4);
                }
            }

            using (Graphics g = Graphics.FromImage(menubase))
            {
                g.DrawImage(palWBox, new Point(6, 6));
            }

            ColorChange();
            BackgroundImage = menubase;
            Refresh();
        }

        private void boxPalleteBounds(double x, double y, out double xnew, out double ynew)
        {
            if (x > Width - 28)
            {
                xnew = Width - 28;
            }
            else if (0 > x)
            {
                xnew = 0;
            }
            else
            {
                xnew = x;
            }

            if (y > Height - 28)
            {
                ynew = Height - 28;
            }
            else if (0 > y)
            {
                ynew = 0;
            }
            else
            {
                ynew = y;
            }
        }

        private void boxPalleteBounds(int x, int y, out int xnew, out int ynew)
        {
            if (x > Width - 28)
            {
                xnew = Width - 28;
            }
            else if (0 > x)
            {
                xnew = 0;
            }
            else
            {
                xnew = x;
            }

            if (y > Height - 28)
            {
                ynew = Height - 28;
            }
            else if (0 > y)
            {
                ynew = 0;
            }
            else
            {
                ynew = y;
            }
        }

        Rectangle hueanglesRC;
        Bitmap hueangles;
        private void drawHue(bool box = false)
        {
            using (Graphics g = Graphics.FromImage(hueangles))
            {
                LinearGradientBrush rainbow = new LinearGradientBrush(hueanglesRC, Color.Black, Color.Black, 90F, false);
                ColorBlend cb = new ColorBlend();
                cb.Positions = new[] { 0, 1 / 6f, 2 / 6f, 3 / 6f, 4 / 6f, 5 / 6f, 1f };
                cb.Colors = new[] { Color.FromArgb(255, 0, 0), Color.FromArgb(255, 255, 0), Color.FromArgb(0, 255, 0), Color.FromArgb(0, 255, 255), Color.FromArgb(0, 0, 255), Color.FromArgb(255, 0, 255), Color.FromArgb(255, 0, 0) };
                rainbow.InterpolationColors = cb;

                g.FillRectangle(rainbow, hueanglesRC);
            }

            Bitmap hueWBox = (Bitmap)hueangles.Clone();
            if (box)
            {
                using (Graphics g = Graphics.FromImage(hueWBox))
                {
                    Pen penblack = new Pen(brushBlack);

                    using (Brush brush = new SolidBrush(Color.Black))
                    {
                        Pen cpen = new Pen(brush);
                        g.DrawRectangle(cpen, -1, (int)angleH, 9, 3);
                    }
                }
            }

            switch (box)
            {
                case false:
                    using (Graphics g = Graphics.FromImage(menubase))
                    {
                        g.DrawImage(hueangles, new Point(Height - 14, 6));
                    }
                    break;
                case true:
                    using (Graphics g = Graphics.FromImage(menubase))
                    {
                        g.DrawImage(hueWBox, new Point(Height - 14, 6));
                    }
                    break;
            }

            BackgroundImage = menubase;
            Refresh();
        }

        private void drawBoxHue(MouseEventArgs e)
        {
            Bitmap hueWBox = (Bitmap)hueangles.Clone();
            using (Graphics g = Graphics.FromImage(hueWBox))
            {
                Pen penblack = new Pen(brushBlack);

                int y = e.Y - 6;

                boxHueBounds(y, out y);

                int newhue = (360 * y) / hueangles.Height;
                hue = newhue;
                ColorChange();
                //MessageBox.Show("" + hue + " " + sat + " " + value);
                Color newbrush = ColorFromHSV(hue, sat, value);
                setColor(newbrush, false, false);

                using (Brush brush = new SolidBrush(Color.Black))
                {
                    Pen cpen = new Pen(brush);
                    g.DrawRectangle(cpen, -1, y, 9, 3);
                }
            }

            using (Graphics g = Graphics.FromImage(menubase))
            {
                g.DrawImage(hueWBox, new Point(Height - 14, 6));
            }

            BackgroundImage = menubase;
            Refresh();
        }

        private void boxHueBounds(int y, out int ynew)
        {
            if (0 > y)
            {
                ynew = 0;
            }
            else if (y > hueangles.Height - 4)
            {
                ynew = hueangles.Height - 4;
            }
            else
            {
                ynew = y;
            }

        }

        Rectangle brightnessRC;
        Bitmap brightness;
        private void drawBrightness(bool box = false)
        {
            using (Graphics g = Graphics.FromImage(brightness))
            {
                LinearGradientBrush gradient = new LinearGradientBrush(brightnessRC, Color.Black, Color.White, LinearGradientMode.Horizontal);
                g.FillRectangle(gradient, brightnessRC);
            }

            Bitmap brightWBox = (Bitmap)brightness.Clone();
            if (box)
            {
                using (Graphics g = Graphics.FromImage(brightWBox))
                {
                    Pen penblack = new Pen(brushBlack);

                    using (Brush brush = new SolidBrush(Color.Black))
                    {
                        Pen cpen = new Pen(brush);
                        g.DrawRectangle(cpen, (int)angleB, -1, 3, 9);
                    }
                }
            }

            switch (box)
            {
                case false:
                    using (Graphics g = Graphics.FromImage(menubase))
                    {
                        g.DrawImage(brightness, new Point(6, Width - 14));
                    }
                    break;
                case true:
                    using (Graphics g = Graphics.FromImage(menubase))
                    {
                        g.DrawImage(brightWBox, new Point(6, Width - 14));
                    }
                    break;
            }

            BackgroundImage = menubase;
            Refresh();
        }

        private void drawBoxBrightness(MouseEventArgs e)
        {
            Bitmap brightWBox = (Bitmap)brightness.Clone();
            using (Graphics g = Graphics.FromImage(brightWBox))
            {
                Pen penblack = new Pen(brushBlack);

                int x = e.X - 6;

                boxBrightBounds(x, out x);

                double newbright = (double)x / (brightness.Width - 4);
                value = newbright;
                ColorChange();
                //MessageBox.Show("" + hue + " " + sat + " " + value);
                Color newbrush = ColorFromHSV(hue, sat, value);
                setColor(newbrush, false, false, true, false);

                using (Brush brush = new SolidBrush(Color.Black))
                {
                    Pen cpen = new Pen(brush);
                    g.DrawRectangle(cpen, x, -1, 3, 9);
                }
            }

            using (Graphics g = Graphics.FromImage(menubase))
            {
                g.DrawImage(brightWBox, new Point(6, Width - 14));
            }

            BackgroundImage = menubase;
            Refresh();
        }

        private void boxBrightBounds(int x, out int xnew)
        {
            if (0 > x)
            {
                xnew = 0;
            }
            else if (x > brightness.Width - 4)
            {
                xnew = brightness.Width - 4;
            }
            else
            {
                xnew = x;
            }

        }
        #endregion

        #region util
        //https://stackoverflow.com/questions/359612/how-to-change-rgb-color-to-hsv
        public static void ColorToHSV(Color color, out double hue, out double saturation, out double value)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            hue = color.GetHue();
            saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            value = max / 255d;
        }

        public static void ColorToHSV(Color color, out double saturation, out double value)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            value = max / 255d;
        }

        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }
        #endregion


        //events
        private bool mouseDown = false;
        private void skeetColorPickerMenu_MouseMove(object sender, MouseEventArgs e)
        {
            fireControl(e);
        }

        private void skeetColorPickerMenu_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            getSelectedControl(e.X, e.Y);
        }

        private void skeetColorPickerMenu_Click(object sender, EventArgs e)
        {
            fireControl((MouseEventArgs)e);
        }

        private void skeetColorPickerMenu_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
            selectedControl = "none";
        }

        //controls
        private void fireControl(MouseEventArgs e)
        {
            if (mouseDown)
            {
                switch (selectedControl)
                {
                    case "palette":
                        drawBoxPallete(e);
                        break;

                    case "brightness":
                        drawBoxBrightness(e);
                        break;

                    case "hue":
                        drawBoxHue(e);
                        break;
                }
            }
        }

        private string selectedControl = "none";

        private void getSelectedControl(int x, int y)
        {
            if (x >= 5 && Width - 17 >= x && y >= 5 && Height - 17 >= y)
            {
                selectedControl = "palette";
            }

            if (x >= 5 && Width - 17 >= x && y >= Height - 14 && Height - 5 >= y)
            {
                selectedControl = "brightness";
            }

            if (x >= Width - 15 && Width - 6 >= x && y >= 5 && Height - 18 >= y)
            {
                selectedControl = "hue";
            }
        }
    }
}
