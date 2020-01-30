using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SkeetUI
{
    public class skeetForm : Form
    {
        #region misc/visuals
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        private const int WM_NCLBUTTONDBLCLK = 0x00A3;

        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        private bool m_aeroEnabled;

        private const int CS_DROPSHADOW = 0x00020000;
        private const int WM_NCPAINT = 0x0085;
        private const int WM_ACTIVATEAPP = 0x001C;

        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);
        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]

        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);
        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
            );

        public struct MARGINS
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }
        protected override CreateParams CreateParams
        {
            get
            {
                m_aeroEnabled = CheckAeroEnabled();
                CreateParams cp = base.CreateParams;
                if (!m_aeroEnabled)
                    cp.ClassStyle |= CS_DROPSHADOW; return cp;
            }
        }
        private bool CheckAeroEnabled()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                int enabled = 0; DwmIsCompositionEnabled(ref enabled);
                return (enabled == 1) ? true : false;
            }
            return false;
        }
        protected override void WndProc(ref Message m)
        {
            const int RESIZE_HANDLE_SIZE = 10;
            switch (m.Msg)
            {
                case WM_NCPAINT:
                    if (m_aeroEnabled)
                    {
                        var v = 2;
                        DwmSetWindowAttribute(this.Handle, 2, ref v, 4);
                        MARGINS margins = new MARGINS()
                        {
                            bottomHeight = 1,
                            leftWidth = 0,
                            rightWidth = 0,
                            topHeight = 0
                        }; DwmExtendFrameIntoClientArea(this.Handle, ref margins);
                    }
                    break;
                default: break;
                case 0x0084/*NCHITTEST*/ :

                    base.WndProc(ref m);
                    if ((int)m.Result == 0x01/*HTCLIENT*/)
                    {
                        if (skeetResizable)
                        {
                            Point screenPoint = new Point(m.LParam.ToInt32());
                            Point clientPoint = this.PointToClient(screenPoint);
                            if (clientPoint.Y <= RESIZE_HANDLE_SIZE)
                            {
                                if (clientPoint.X <= RESIZE_HANDLE_SIZE)
                                    m.Result = (IntPtr)13/*HTTOPLEFT*/ ;
                                else if (clientPoint.X < (Size.Width - RESIZE_HANDLE_SIZE))
                                    m.Result = (IntPtr)12/*HTTOP*/ ;
                                else
                                    m.Result = (IntPtr)14/*HTTOPRIGHT*/ ;
                            }
                            else if (clientPoint.Y <= (Size.Height - RESIZE_HANDLE_SIZE))
                            {
                                if (clientPoint.X <= RESIZE_HANDLE_SIZE)
                                    m.Result = (IntPtr)10/*HTLEFT*/ ;
                                else if (clientPoint.X < (Size.Width - RESIZE_HANDLE_SIZE))
                                    m.Result = (IntPtr)2/*HTCAPTION*/ ;
                                else
                                    m.Result = (IntPtr)11/*HTRIGHT*/ ;
                            }
                            else
                            {
                                if (clientPoint.X <= RESIZE_HANDLE_SIZE)
                                    m.Result = (IntPtr)16/*HTBOTTOMLEFT*/ ;
                                else if (clientPoint.X < (Size.Width - RESIZE_HANDLE_SIZE))
                                    m.Result = (IntPtr)15/*HTBOTTOM*/ ;
                                else
                                    m.Result = (IntPtr)17/*HTBOTTOMRIGHT*/ ;
                            }
                        }
                        else
                        {
                            m.Result = (IntPtr)HTCAPTION;
                        }
                    }

                    return;
            }

            if (m.Msg == WM_NCLBUTTONDBLCLK)
            {
                m.Result = IntPtr.Zero;
                return;
            }

            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST && (int)m.Result == HTCLIENT) m.Result = (IntPtr)HTCAPTION;

        }

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        #endregion

        private bool skeetResizable = false;
        private bool skeetGradient = true;
        private Color skeetGradient1 = Color.FromArgb(55, 177, 218);
        private Color skeetGradient2 = Color.FromArgb(204, 91, 184);
        private Color skeetGradient3 = Color.FromArgb(204, 227, 53);

        [Description("If form is resizable"), Category("SkeetUI - Form"), DefaultValue(false)]
        public bool Resizable { get { return skeetResizable; } set { skeetResizable = value; } }
        [Description("If gradient line is drawn"), Category("SkeetUI - Form"), DefaultValue(true)]
        public bool GradientLine { get { return skeetGradient; } set { skeetGradient = value; drawGradient(); drawTheme(); } }
        [Description("First color of the gradient"), Category("SkeetUI - Form"), DefaultValue(null)]
        public Color GradientColor1 { get { return skeetGradient1; } set { skeetGradient1 = value; drawGradient(); drawTheme(); } }
        [Description("Second color of the gradient"), Category("SkeetUI - Form"), DefaultValue(null)]
        public Color GradientColor2 { get { return skeetGradient2; } set { skeetGradient2 = value; drawGradient(); drawTheme(); } }
        [Description("Third color of the gradient"), Category("SkeetUI - Form"), DefaultValue(null)]
        public Color GradientColor3 { get { return skeetGradient3; } set { skeetGradient3 = value; drawGradient(); drawTheme(); } }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            drawTheme();
        }

        public skeetForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            DoubleBuffered = true;
            drawGradient();
            //drawTheme();
        }

        #region draw form
        Bitmap gradientUpline = new Bitmap(3, 1);
        Bitmap gradientDownline = new Bitmap(3, 1);

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

        private void drawGradient()
        {
            gradientUpline.SetPixel(0, 0, skeetGradient1);
            gradientUpline.SetPixel(1, 0, skeetGradient2);
            gradientUpline.SetPixel(2, 0, skeetGradient3);
            gradientDownline.SetPixel(0, 0, changeColorBrightness(skeetGradient1, -0.5f));
            gradientDownline.SetPixel(1, 0, changeColorBrightness(skeetGradient2, -0.5f));
            gradientDownline.SetPixel(2, 0, changeColorBrightness(skeetGradient3, -0.5f));
        }

        private Color offsetColor(int offset)
        {
            return Color.FromArgb(0 + offset, 0 + offset, 0 + offset);
        }

        public void drawTheme()
        {
            Bitmap background = new Bitmap(Width, Height);

            using (Graphics g = Graphics.FromImage(background))
            {
                //background
                g.Clear(offsetColor(22));

                //chainmail
                Bitmap chain = new Bitmap(4, Height);
                //draw thing
                using (Graphics gchain = Graphics.FromImage(chain))
                {
                    Pen penchain = new Pen(new SolidBrush(Color.FromArgb(12, 12, 12)));
                    for (int i = 0; 1 >= i; i++)
                    {
                        bool outborder = false;
                        int ydraw = 1 - (i * 2);
                        int xdraw = 0 + (i * 2);
                        while (!outborder)
                        {
                            if (!(ydraw > chain.Height))
                            {
                                gchain.DrawLine(penchain, xdraw, ydraw, xdraw, ydraw + 2);
                                ydraw += 4;
                            }
                            else
                            {
                                outborder = true;
                            }
                        }
                    }
                }
                //apply to form
                int xform = 0;
                g.CompositingMode = CompositingMode.SourceOver;
                while (Width > xform)
                {
                    g.DrawImage(chain, new Point(xform, 0));
                    xform += 4;
                }

                //gradient line
                g.InterpolationMode = InterpolationMode.Bilinear;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                if (skeetGradient)
                {
                    g.DrawImage(gradientUpline, 7, 7, background.Width - 7, 1);
                    g.DrawImage(gradientDownline, 7, 8, background.Width - 7, 1);
                }

                //border
                g.InterpolationMode = InterpolationMode.Bilinear;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Default;
                Pen cpen = new Pen(new SolidBrush(offsetColor(0)));
                g.DrawRectangle(cpen, 0, 0, background.Width - 1, background.Height - 1);

                cpen = new Pen(new SolidBrush(offsetColor(56)));
                g.DrawRectangle(cpen, 1, 1, background.Width - 3, background.Height - 3);

                cpen = new Pen(new SolidBrush(offsetColor(43)));
                g.DrawRectangle(cpen, 2, 2, background.Width - 5, background.Height - 5);

                cpen = new Pen(new SolidBrush(offsetColor(40)));
                g.DrawRectangle(cpen, 3, 3, background.Width - 7, background.Height - 7);
                g.DrawRectangle(cpen, 4, 4, background.Width - 9, background.Height - 9);

                cpen = new Pen(new SolidBrush(offsetColor(43)));
                g.DrawRectangle(cpen, 5, 5, background.Width - 11, background.Height - 11);

                cpen = new Pen(new SolidBrush(offsetColor(52)));
                g.DrawRectangle(cpen, 6, 6, background.Width - 13, background.Height - 13);

            }

            BackgroundImage = background;
        }
        #endregion

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // skeetForm
            // 
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "skeetForm";
            this.ResumeLayout(false);

        }
    }
}