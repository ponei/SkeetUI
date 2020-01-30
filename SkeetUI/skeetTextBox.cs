using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SkeetUI
{
    public partial class skeetTextBox : UserControl
    {
        #region parameters
        string skeetText = "";
        int skeetTextMaxLen = 2147483647;
        bool skeetMultiline = false;
        bool skeetWordWrap = false;
        bool skeetPassword = false;
        string skeetPasswordChar = "*";

        [Description("Text that the input has"), Category("SkeetUI - TextBox"), DefaultValue("")]
        public string InputText
        {
            get { return skeetText; }
            set
            {
                skeetText = value;
                inputBox.Text = skeetText;
            }
        }

        [Description("Max text length that the control can allow"), Category("SkeetUI - TextBox"), DefaultValue("")]
        public int InputMaxLength
        {
            get { return skeetTextMaxLen; }
            set
            {
                skeetTextMaxLen = value;
                inputBox.MaxLength = skeetTextMaxLen;
            }
        }

        [Description("If the control will allow multiple lines"), Category("SkeetUI - TextBox"), DefaultValue("")]
        public bool Multiline
        {
            get { return skeetMultiline; }
            set
            {
                skeetMultiline = value;
            }
        }

        [Description("If the control will allow word wrapping"), Category("SkeetUI - TextBox"), DefaultValue("")]
        public bool WordWrap
        {
            get { return skeetWordWrap; }
            set
            {
                skeetWordWrap = value;
            }
        }
        #endregion

        #region dllimports
        [DllImport("user32.dll")]
        static extern bool CreateCaret(IntPtr hWnd, IntPtr hBitmap, int nWidth, int nHeight);
        [DllImport("user32.dll")]
        static extern bool ShowCaret(IntPtr hWnd);
        #endregion

        public skeetTextBox()
        {
            InitializeComponent();
            drawBack();
        }

        IntPtr inputCaret;
        private void drawCaret()
        {
            Bitmap caret = new Bitmap(1, 10);
            using (Graphics graph = Graphics.FromImage(caret))
            {
                Rectangle blank = new Rectangle(0, 0, caret.Width, 2);
                SolidBrush blankbrush = new SolidBrush(Color.FromArgb(24, 24, 24));
                graph.FillRectangle(blankbrush, blank);

                Rectangle ImageSize = new Rectangle(0, 2, caret.Width, caret.Height);
                SolidBrush brush = new SolidBrush(Color.FromArgb(120, 190, 20));
                graph.FillRectangle(brush, ImageSize);
            }
            inputCaret = caret.GetHbitmap();

            CreateCaret(inputBox.Handle, inputCaret, 1, 10);
            ShowCaret(inputBox.Handle);
        }

        private void skeetTextBox_Resize(object sender, EventArgs e)
        {
            onResize();
        }

        private void onResize()
        {
            drawBack();
            inputBox.Width = Width - 9;
            if (skeetMultiline)
            {
                if (Height > 20)
                {
                    int newHei = Height - 20;
                }
                else
                {
                    Height = 20;
                    inputBox.Height = 12;
                }
            }
            else
            {
                Height = 20;
                inputBox.Height = 12;
            }
        }

        private void drawBack()
        {
            Bitmap background = new Bitmap(Width, Height);
            using (Graphics g = Graphics.FromImage(background))
            {
                using (Brush brush = new SolidBrush(Color.FromArgb(15, 15, 15)))
                {
                    Pen cpen = new Pen(brush);
                    g.DrawRectangle(cpen, 0, 0, Width - 1, Height - 1);

                    g.DrawRectangle(cpen, 2, 2, Width - 5, Height - 5);
                }
                using (Brush brush = new SolidBrush(Color.FromArgb(40, 40, 40)))
                {
                    Pen cpen = new Pen(brush);
                    g.DrawRectangle(cpen, 1, 1, Width - 3, Height - 3);
                }
            }
            BackgroundImage = background;
        }

        private void inputBox_SelectionChanged(object sender, EventArgs e)
        {
            //caret update
            drawCaret();

            if (inputBox.SelectionLength > 0)
            {
                inputBox.SelectionLength = 0;
            }
            else if (inputBox.SelectedRtf.Contains("charset") && inputBox.SelectionStart == inputBox.Text.Length)
            {
                inputBox.SelectionStart = 0;
            }
        }



        private void inputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                e.SuppressKeyPress = true;
            }
        }

        private void inputBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                e.SuppressKeyPress = true;
            }
        }

        bool mousedown = false;
        private void inputBox_MouseDown(object sender, MouseEventArgs e)
        {
            //caret update
            drawCaret();

            mousedown = true;
        }

        private void inputBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (mousedown)
            {
                drawCaret();
            }
        }

        private void inputBox_MouseUp(object sender, MouseEventArgs e)
        {
            mousedown = false;
        }
    }
}
