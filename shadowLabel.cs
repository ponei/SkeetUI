using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace SkeetUI
{
    public class shadowLabel : Label
    {
        //sources:
        //https://gist.github.com/mjs3339/1dceee0c4d395eaaf01cc06107a8aaf9 - custom label

        public enum Angles
        {
            /// <summary>
            ///     Normal drawing direction.
            /// </summary>
            LeftToRight = 0,
            /// <summary>
            ///     Draw text top to bottom as viewed from the left.
            /// </summary>
            TopToBottom = 90,
            /// <summary>
            ///     Draw text from right to left as viewed from above (upside down).
            /// </summary>
            RightToLeft = 180,
            /// <summary>
            ///     Draw text bottom to top  as viewed from the right.
            /// </summary>
            BottomToTop = 270
        }

        private Angles _angle = Angles.LeftToRight;

        private bool _enableShadow;
        private Color _shadowColor = Color.LightGray;
        private int _shadowOffset = 1;

        public shadowLabel()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer, true);
        }
        /// <summary>
        ///     Sets the drawing direction
        /// </summary>
        
        /// <summary>
        ///     Sets of the shadow
        /// </summary>
        [Description("Sets of the shadow")]
        public Color ShadowColor
        {
            get => _shadowColor;
            set
            {
                _shadowColor = value;
                Invalidate();
            }
        }
        /// <summary>
        ///     Sets the offset of the shadow. Positives move right and down, negatives move left and up.
        /// </summary>
        [Description("Sets the offset of the shadow. Positives move right and down, negatives move left and up.")]
        public int ShadowOffset
        {
            get => _shadowOffset;
            set
            {
                _shadowOffset = value;
                Invalidate();
            }
        }
        /// <summary>
        ///     Enables to shadow
        /// </summary>
        [Description("Enables to shadow.")]
        public bool EnableShadow
        {
            get => _enableShadow;
            set
            {
                _enableShadow = value;
                Invalidate();
            }
        }
        /// <summary>
        ///     Enables the Gradient
        /// </summary>
        
        protected override void OnPaint(PaintEventArgs e)
        {
            if (_enableShadow)
            {
                var rc0 = new Rectangle(_shadowOffset, _shadowOffset, Width, Height);
                var b0 = new SolidBrush(Color.FromArgb(255, _shadowColor));
                e.Graphics.DrawString(Text, Font, b0, rc0, ContentAlignmentToStringAlignment(TextAlign));
            }

            var size = e.Graphics.VisibleClipBounds.Size;
            switch (_angle)
            {
                case Angles.LeftToRight:
                    e.Graphics.TranslateTransform(0, 0);
                    e.Graphics.RotateTransform(0);
                    e.Graphics.DrawString(Text, Font, new SolidBrush(Color.FromArgb(255, ForeColor)), new RectangleF(0, 0, size.Width, size.Height), ContentAlignmentToStringAlignment(TextAlign));
                    e.Graphics.ResetTransform();
                    break;
                case Angles.TopToBottom:
                    e.Graphics.TranslateTransform(size.Width, 0);
                    e.Graphics.RotateTransform(90);
                    e.Graphics.DrawString(Text, Font, new SolidBrush(Color.FromArgb(255, ForeColor)), new RectangleF(0, 0, size.Height, size.Width), ContentAlignmentToStringAlignment(TextAlign));
                    e.Graphics.ResetTransform();
                    break;
                case Angles.RightToLeft:
                    e.Graphics.TranslateTransform(size.Width, size.Height);
                    e.Graphics.RotateTransform(180);
                    e.Graphics.DrawString(Text, Font, new SolidBrush(Color.FromArgb(255, ForeColor)), new RectangleF(0, 0, size.Width, size.Height), ContentAlignmentToStringAlignment(TextAlign));
                    e.Graphics.ResetTransform();
                    break;
                case Angles.BottomToTop:
                    e.Graphics.TranslateTransform(0, size.Height);
                    e.Graphics.RotateTransform(270);
                    e.Graphics.DrawString(Text, Font, new SolidBrush(Color.FromArgb(255, ForeColor)), new RectangleF(0, 0, size.Height, size.Width), ContentAlignmentToStringAlignment(TextAlign));
                    e.Graphics.ResetTransform();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private static StringFormat ContentAlignmentToStringAlignment(ContentAlignment ca)
        {
            var format = new StringFormat();
            var l2 = (int)Math.Log((double)ca, 2);
            format.LineAlignment = (StringAlignment)(l2 / 4);
            format.Alignment = (StringAlignment)(l2 % 4);
            return format;
        }
    }
}