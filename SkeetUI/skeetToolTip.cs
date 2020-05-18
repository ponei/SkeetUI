using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SkeetUI
{
    public class skeetToolTip : ToolTip
    {
        Font skt = new Font("Verdana", 7);
        public skeetToolTip()
        {
            this.OwnerDraw = true;
            this.Popup += new PopupEventHandler(this.OnPopup);
            this.Draw += new DrawToolTipEventHandler(this.OnDraw);
        }

        private void OnPopup(object sender, PopupEventArgs e) // use this event to set the size of the tool tip
        {
            string text = this.GetToolTip(e.AssociatedControl);
            ttText = text;

            ttFormattedText = wrap(ttText, 30);
            Size textSize = getTextSize(ttFormattedText);

            e.ToolTipSize = new Size(textSize.Width + 10, textSize.Height + 10);
        }

        private string ttText;
        private string ttFormattedText;
        private void OnDraw(object sender, DrawToolTipEventArgs e) // use this event to customise the tool tip
        {
            Graphics g = e.Graphics;

            Brush b = new SolidBrush(Color.FromArgb(10, 10, 10));

            g.FillRectangle(b, e.Bounds);

            Brush bOutline = new SolidBrush(Color.FromArgb(48, 48, 48));
            g.DrawRectangle(new Pen(bOutline, 1), new Rectangle(e.Bounds.X, e.Bounds.Y,
                e.Bounds.Width - 1, e.Bounds.Height - 1));

            Brush bText = new SolidBrush(Color.FromArgb(210, 210, 210));
            g.DrawString(ttFormattedText, new Font(skt, FontStyle.Regular), Brushes.Black,
                new PointF(e.Bounds.X + 6, e.Bounds.Y + 6)); // shadow layer
            g.DrawString(ttFormattedText, new Font(skt, FontStyle.Regular), bText,
                new PointF(e.Bounds.X + 5, e.Bounds.Y + 5)); // top layer

            b.Dispose();
        }

        private Size getTextSize(string text)
        {
            Image fakeImage = new Bitmap(1, 1);
            Graphics graphics = Graphics.FromImage(fakeImage);
            SizeF size = graphics.MeasureString(text, skt);
            return new Size((int)size.Width, (int)size.Height);
        }


        //https://stackoverflow.com/questions/10541124/wrap-text-to-the-next-line-when-it-exceeds-a-certain-length
        private string wrap(string input, int maxCharacters)
        {
            List<string> lines = new List<string>();

            if (!input.Contains(" ") && !input.Contains("\n"))
            {
                int start = 0;
                while (start < input.Length)
                {
                    lines.Add(input.Substring(start, Math.Min(maxCharacters, input.Length - start)));
                    start += maxCharacters;
                }
            }
            else
            {
                string[] paragraphs = input.Split('\n');

                foreach (string paragraph in paragraphs)
                {
                    string[] words = paragraph.Split(' ');

                    string line = "";
                    foreach (string word in words)
                    {
                        if ((line + word).Length > maxCharacters)
                        {
                            lines.Add(line.Trim());
                            line = "";
                        }

                        line += string.Format("{0} ", word);
                    }

                    if (line.Length > 0)
                    {
                        lines.Add(line.Trim());
                    }
                }
            }
            return string.Join("\n", lines);
        }
    }
}
