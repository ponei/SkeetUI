using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public class tablessControl : TabControl
{
    protected override void WndProc(ref Message m)
    {
        // Hide tabs by trapping the TCM_ADJUSTRECT message
        if (m.Msg == 0x1328 && !DesignMode) m.Result = (IntPtr)1;
        else base.WndProc(ref m);
    }

    [DllImport("uxtheme", ExactSpelling = true)]
    public extern static Int32 DrawThemeParentBackground(IntPtr hWnd, IntPtr hdc, ref Rectangle pRect);

    // use with care, as it may cause strange effects
    protected override CreateParams CreateParams
    {
        get
        {
            CreateParams cp = base.CreateParams;
            cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
            return cp;
        }
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
        IntPtr hdc = e.Graphics.GetHdc();
        Rectangle rect = ClientRectangle;
        DrawThemeParentBackground(this.Handle, hdc, ref rect);
        e.Graphics.ReleaseHdc(hdc);
    }
}