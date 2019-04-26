using System;
using System.Windows.Forms;

public class transparentPanel : Panel
{
    //sources:
    //somewhere on stackoverflow
    protected override CreateParams CreateParams
    {
        get
        {
            CreateParams cp = base.CreateParams;
            cp.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT
            return cp;
        }
    }
    protected override void OnPaintBackground(PaintEventArgs e)
    {
        //base.OnPaintBackground(e);
    }
}