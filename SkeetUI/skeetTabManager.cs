using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SkeetUI
{
    public partial class skeetTabManager : Panel
    {
        #region parameters
        private tablessTabControl baseTabControl;
        [Description("TabControl that the manager is going to handle"), Category("SkeetUI - TabManager"), DefaultValue("")]
        public tablessTabControl TabControl
        {
            get { return baseTabControl; }
            set
            {
                baseTabControl = value;
                drawControl();
            }
        }

        private string tabLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        [Description("Letters that the manager is going to use to identify tabs"), Category("SkeetUI - TabManager"), DefaultValue("")]
        public string Leters
        {
            get { return tabLetters; }
            set
            {
                tabLetters = value;
                drawControl();
            }
        }

        private bool skeetFont = true;
        [Description("If should use Skeet font (BadCache)"), Category("SkeetUI - TabManager"), DefaultValue("")]
        public bool SkeetFont
        {
            get { return skeetFont; }
            set
            {
                skeetFont = value;
                drawControl();
            }
        }

        #endregion

        #region draw
        Bitmap baseManager;
        private void drawBaseManager()
        {
            Bitmap drawbase = new Bitmap(Width, Height);
            using (Graphics g = Graphics.FromImage(drawbase))
            {
                g.Clear(Color.FromArgb(12, 12, 12));
                using (Brush brush = new SolidBrush(Color.FromArgb(48, 48, 48)))
                {
                    Pen cpen = new Pen(brush);
                    g.DrawLine(cpen, 74, 0, 74, Height);
                }
                using (Brush brush = new SolidBrush(Color.FromArgb(0, 0, 0)))
                {
                    Pen cpen = new Pen(brush);
                    g.DrawLine(cpen, 73, 0, 73, Height);
                }
            }

            baseManager = drawbase;
        }

        Bitmap baseOpcao;
        private void drawBaseOpcao()
        {
            Bitmap drawbase = new Bitmap(75, 79);
            using (Graphics g = Graphics.FromImage(drawbase))
            {
                using (Brush brush = new SolidBrush(Color.FromArgb(48, 48, 48)))
                {
                    Pen cpen = new Pen(brush);
                    g.DrawLine(cpen, 0, 1, 74, 1);
                    g.DrawLine(cpen, 0, 77, 74, 77);
                    g.FillRectangle(brush, 74, 0, 1, 1);
                    g.FillRectangle(brush, 74, 78, 1, 1);
                }

                using (Brush brush = new SolidBrush(Color.FromArgb(0, 0, 0)))
                {
                    Pen cpen = new Pen(brush);
                    g.DrawLine(cpen, 0, 0, 73, 0);
                    g.DrawLine(cpen, 0, 78, 73, 78);
                }


            }

            baseOpcao = drawbase;
        }

        private bool isInit = false;
        private void initFont()
        {
            if (!isInit)
            {
                isInit = true;
                //Select your font from the resources.
                //My font here is "Digireu.ttf"
                int fontLength = Properties.Resources.badcache.Length;

                // create a buffer to read in to
                byte[] fontdata = Properties.Resources.badcache;

                // create an unsafe memory block for the font data
                System.IntPtr data = Marshal.AllocCoTaskMem(fontLength);

                // copy the bytes to the unsafe memory block
                Marshal.Copy(fontdata, 0, data, fontLength);

                // pass the font to the font collection
                pfc.AddMemoryFont(data, fontLength);
            }
        }

        private PrivateFontCollection pfc = new PrivateFontCollection();
        private void drawControl(int index = 0)
        {
            if (baseTabControl != null)
            {
                Font fnt;
                if (skeetFont)
                {
                    fnt = new Font(pfc.Families[0], 26f);
                } else {
                    fnt = this.Font;
                }

                Bitmap background = (Bitmap)baseManager.Clone();
                using (Graphics g = Graphics.FromImage(background))
                {
                    g.CompositingMode = CompositingMode.SourceCopy;
                    int y_add = 79 * index;
                    g.DrawImage(baseOpcao, new Point(0, 20 + y_add));

                    g.CompositingMode = CompositingMode.SourceOver;
                    g.TextRenderingHint = TextRenderingHint.AntiAlias;
                    for (int i = 0; baseTabControl.TabCount > i; i++)
                    {
                        int y = 42 + (i * 79);
                        using (Brush brush = new SolidBrush(Color.FromArgb(0, 0, 0)))
                        {
                            g.DrawString(tabLetters[i].ToString(), fnt, brush, 16, y + 2);
                        }
                        if (i == index)
                        {
                            using (Brush brush = new SolidBrush(Color.FromArgb(210, 210, 210)))
                            {
                                g.DrawString(tabLetters[i].ToString(), fnt, brush, 14, y);
                            }
                        } else
                        {
                            using (Brush brush = new SolidBrush(Color.FromArgb(87, 87, 87)))
                            {
                                g.DrawString(tabLetters[i].ToString(), fnt, brush, 14, y);
                            }
                        }
                    }

                }

                BackgroundImage = background;
            }
            else
            {
                BackgroundImage = baseManager;
            }
        }
        #endregion

        #region events
        private void skeetTabManager_Click(object sender, EventArgs e)
        {
            MouseEventArgs mouse = (MouseEventArgs)e;
            if (mouse.Y >= 20)
            {
                double y = (mouse.Y - 20) / 79;
                int index = (int)Math.Floor(y);
                setTabIndex(index);
            }
        }

        private void setTabIndex(int index)
        {
            if (baseTabControl.TabCount - 1 >= index)
            {
                baseTabControl.SelectedIndex = index;
                drawControl(index);
            }
        }

        #endregion
        public skeetTabManager()
        {
            InitializeComponent();
            drawBaseOpcao();
            initFont();
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            BackColor = Color.Transparent;
            DoubleBuffered = true;
        }

        private void skeetTabManager_Resize(object sender, EventArgs e)
        {
            fixedSize();
            drawBaseManager();
            drawControl();
        }

        private void skeetTabManager_HandleCreated(object sender, EventArgs e)
        {
            drawBaseManager();
            drawControl();
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (this.DesignMode)
                base.SetBoundsCore(x, y, 75, height, specified);
            else
                base.SetBoundsCore(x, y, 75, height, specified);
        }

        private void fixedSize()
        {
            if (Parent != null)
            {
                Height = Parent.Height - 16;

            }
        }

        private void skeetTabManager_Move(object sender, EventArgs e)
        {
            fixedLoc();
            fixedSize();
        }

        private void fixedLoc()
        {
            if (Parent != null)
            {
                Left = 7;
                Top = 9;
            }
        }
    }
}
