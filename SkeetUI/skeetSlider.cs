using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SkeetUI
{
    public partial class skeetSlider : UserControl
    {
        #region parametros control
        private bool skeetShowTitle = true;
        private bool invertTitleColor = false;
        private string skeetTitle = "skeetSlider";
        private bool skeetShowValue = true;
        private string skeetValueSuffix = "";
        private string skeetValuePrefix = "";
        [Description("Inverts the ForeColor of the labels"), Category("SkeetUI - Texts"), DefaultValue(false)]
        public bool InvertTitleColor
        {
            get { return invertTitleColor; }
            set
            {
                invertTitleColor = value;
                Color shadow = Color.Black;
                Color text = Color.FromArgb(190, 190, 190);
                if (value == true)
                {
                    lbTitle.ForeColor = shadow;
                    lbTitle.ShadowColor = text;
                }
                else
                {
                    lbTitle.ForeColor = text;
                    lbTitle.ShadowColor = shadow;
                }
            }
        }
        [Description("Show title above slider"), Category("SkeetUI - Texts"), DefaultValue(true)]
        public bool ShowTitle { get { return skeetShowTitle; } set { skeetShowTitle = value; lbTitle.Visible = value; } }
        [Description("Text of the title"), Category("SkeetUI - Texts"), DefaultValue("skeetSlider")]
        public string Title
        {
            get { return skeetTitle; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    skeetTitle = value;
                lbTitle.Text = skeetTitle;
            }
        }
        [Description("Show value of the slider beside it"), Category("SkeetUI - Texts"), DefaultValue(true)]
        public bool ShowValue { get { return skeetShowValue; } set { skeetShowValue = value; lbSliderValue.Visible = value; } }
        [Description("Suffix that should get together with the value"), Category("SkeetUI - Texts"), DefaultValue("")]
        public string ValueSuffix
        {
            get { return skeetValueSuffix; }
            set
            {
                skeetValueSuffix = value;
                updateValue();
            }
        }
        [Description("Prefix that should get together with the value"), Category("SkeetUI - Texts"), DefaultValue("")]
        public string ValuePrefix
        {
            get { return skeetValuePrefix; }
            set
            {
                skeetValuePrefix = value;
                updateValue();
            }
        }

        private Color sliderColor = Color.FromArgb(154, 197, 39);
        private Color sliderBack = Color.FromArgb(52, 52, 52);
        private double skeetMax = 2.0;
        private double skeetMin = 1.0;
        private double skeetValue = 1.5;
        private int skeetFormatLen = 2;
        [Description("Color of the slider"), Category("SkeetUI - Slider"), DefaultValue(null)]
        public Color SliderColor { get { return sliderColor; } set { sliderColor = value; updateColor(); } }
        [Description("Color of the background of the slider"), Category("SkeetUI - Slider"), DefaultValue(null)]
        public Color SliderBackgroundColor { get { return sliderBack; } set { sliderBack = value; updateColor(); } }
        [Description("Amount of decimal places that value shows"), Category("SkeetUI - Slider"), DefaultValue(2)]
        public int FormatDecimal { get { return skeetFormatLen; } set { if (value >= 0) skeetFormatLen = value; updateValue(); } }
        [Description("Current slider value"), Category("SkeetUI - Slider"), DefaultValue(1.5)]
        public double Value
        {
            get { return skeetValue; }
            set
            {
                double temp = value;

                if (temp >= skeetMin && skeetMax >= temp)
                {
                    skeetValue = value;
                    updateValue();
                }
                else
                {
                    MessageBox.Show("Value can't be lower than minimum or higher than maximum.");
                }
            }
        }
        [Description("Max value that the slider can reach"), Category("SkeetUI - Slider"), DefaultValue(2.0)]
        public double MaxValue
        {
            get { return skeetMax; }
            set
            {
                double temp = value;

                if (temp >= skeetMin)
                {
                    if (temp >= skeetValue)
                    {
                        skeetMax = value;

                        if (!DesignMode)
                        {
                            skeetValue = value; //temp, else skeetmin bugs out -- skeetvalue is only declared after
                        }

                        updateValue();
                    }
                    else
                    {
                        MessageBox.Show("Current value can't be higher than maximum value (" + skeetValue + ">" + temp + ")");
                    }
                }
                else
                {
                    MessageBox.Show("Max value can't be lower than minimum value (" + temp + ">" + skeetMin + ")");
                }
            }
        }
        [Description("Minimum value that the slider needs"), Category("SkeetUI - Slider"), DefaultValue(1.0)]
        public double MinValue
        {
            get { return skeetMin; }
            set
            {
                double temp = value;

                if (temp <= skeetMax)
                {
                    if (skeetValue >= temp)
                    {
                        skeetMin = value;
                        updateValue();
                    }
                    else
                    {
                        MessageBox.Show("Current value can't be lower than minimum value (" + temp + ">" + skeetValue + ")");
                    }
                }
                else
                {
                    MessageBox.Show("Minimum value can't be higher than maximum value (" + temp + ">" + skeetMax + ")");
                }
            }
        }
        #endregion

        #region visuals - sliders/values/control
        Bitmap sliderback = new Bitmap(1, 6);
        Bitmap slidercolor = new Bitmap(1, 6);

        public static Color changeColorBrightness(Color color, float correctionFactor)
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

        private void updateColor()
        {
            sliderback.SetPixel(0, 0, sliderBack);
            slidercolor.SetPixel(0, 0, sliderColor);
            int y = 1;
            for (float i = 0.02f; 0.10f >= i; i += 0.02f, y++)
            {
                sliderback.SetPixel(0, y, changeColorBrightness(sliderBack, i));
                slidercolor.SetPixel(0, y, changeColorBrightness(sliderColor, -i * 2));
            }
            updateSlider();
        }

        int offset = 0;
        private void updateValue()
        {
            offset = 0;
            string text1 = skeetValuePrefix + string.Format("{0:F" + skeetFormatLen + "}", skeetMax) + skeetValueSuffix;
            Font verdanaBold = new Font("Verdana", 7.2F, FontStyle.Bold);
            Size textSize = TextRenderer.MeasureText(text1, verdanaBold);

            if (textSize.Width / 2 > 15)
            {
                int temp = textSize.Width / 2;
                pnlSliderBox.Location = new Point(temp, pnlSliderBox.Location.Y);
                lbTitle.Location = new Point(temp + 1, lbTitle.Location.Y);
                offset = temp;
            }
            else
            {
                offset = 15;
                pnlSliderBox.Location = new Point(15, 0);
                lbTitle.Location = new Point(15, 1);
            }

            lbSliderValue.Text = skeetValuePrefix + string.Format("{0:F" + skeetFormatLen + "}", skeetValue) + skeetValueSuffix;
            pnlSliderBox.Width = Width - offset * 2;
            updateSlider();
        }

        public skeetSlider()
        {
            InitializeComponent();
            DoubleBuffered = true;
            SetDoubleBuffered(pnlSlider);
            updateColor();
            updateValue();
            MinimumSize = new Size(100, 40);
        }

        void updateSlider()
        {
            double diferenca = skeetMax - skeetMin;
            double coisinha = diferenca / (pnlSliderBox.Width - 2);
            double cords = (skeetValue - skeetMin) / coisinha;

            pnlSlider.BackgroundImage = drawSlider(pnlSliderBox, Convert.ToInt32(cords));
            lbSliderValue.Location = new Point(Convert.ToInt32(cords) + offsetValueSwitch(lbSliderValue.Text.Length), lbSliderValue.Location.Y);
        }

        public static void SetDoubleBuffered(System.Windows.Forms.Control c)
        {
            if (System.Windows.Forms.SystemInformation.TerminalServerSession)
                return;

            System.Reflection.PropertyInfo aProp =
                  typeof(System.Windows.Forms.Control).GetProperty(
                        "DoubleBuffered",
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Instance);

            aProp.SetValue(c, true, null);
        }

        private int offsetValueSwitch(int i)
        {
            switch (i)
            {
                case 1:
                    return 9;
                case 2:
                    return 5;
                case 3:
                    return 3;
                default:
                    return 1;
            }
        }

        private Bitmap drawSlider(Control panel, int x)
        {
            Bitmap background = new Bitmap(panel.Width + offset, panel.Height);
            if (x >= 0 && 3 >= x || -1 >= x)
            {
                x = 3;
            }

            if (x >= background.Width)
            {
                x = background.Width;
            }

            using (Graphics g = Graphics.FromImage(background))
            {
                g.InterpolationMode = InterpolationMode.NearestNeighbor;

                g.DrawImage(sliderback, 1 + offset, 1, (background.Width - offset) * 2 - 4, background.Height - 2);
                int lol = 6;
                if (x >= background.Width / 2) { lol = 2; }
                g.DrawImage(slidercolor, 1 + offset, 1, x * 2 - lol, background.Height - 2);
                using (Brush brush = new SolidBrush(Color.FromArgb(0, 0, 0)))
                {
                    Pen cpen = new Pen(brush);
                    g.DrawRectangle(cpen, offset, 0, background.Width - offset - 1, background.Height - 1);
                }
            }

            return background;
        }
        #endregion

        #region visuals - misc
        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (this.DesignMode)
                base.SetBoundsCore(x, y, width, 40, specified);
            else
                base.SetBoundsCore(x, y, width, height, specified);
        }
        #endregion

        private void skeetSlider_Load(object sender, EventArgs e)
        {
            //nada
        }

        private void pnlClick()
        {
            Point cords = pnlSliderBox.PointToClient(Cursor.Position);
            pnlSlider.BackgroundImage = drawSlider(pnlSliderBox, cords.X);

            lbSliderValue.Location = new Point(cords.X + offsetValueSwitch(lbSliderValue.Text.Length), lbSliderValue.Location.Y);

            double max = skeetMax;
            double min = skeetMin;
            double add = (max - min) / (pnlSliderBox.Width - 2);

            int loc = cords.X;
            if (loc >= pnlSliderBox.Width - 2) { loc = pnlSliderBox.Width - 2; }
            if (loc >= 0)
            {
                double value = min + (loc * add);
                skeetValue = value;
                lbSliderValue.Text = skeetValuePrefix + string.Format("{0:F" + skeetFormatLen + "}", skeetValue) + skeetValueSuffix;
            }
        }

        private void skeetSlider_Resize(object sender, EventArgs e)
        {
            pnlSliderBox.Width = Width - offset * 2;
            pnlSlider.Width = Width;
            updateSlider();
        }

        private void pnlSliderBox_Click(object sender, EventArgs e)
        {
            if (pnlSliderBox.ClientRectangle.Contains(pnlSliderBox.PointToClient(Control.MousePosition)))
            {
                pnlClick();
            }
        }

        private void pnlSliderBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (pnlSliderBox.ClientRectangle.Contains(new Point(pnlSliderBox.PointToClient(Control.MousePosition).X, 0)))
                {
                    pnlClick();
                }
            }
        }
    }
}
