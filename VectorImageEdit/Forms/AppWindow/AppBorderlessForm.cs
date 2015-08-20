using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace VectorImageEdit.Forms.AppWindow
{
    public partial class AppWindow
    {
        //***********************************************************
        // This gives us the drop shadow behind the borderless form
        #region FormShadowEffect
        // ReSharper disable once InconsistentNaming
        private const int CS_DROPSHADOW = 0x20000;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }
        #endregion
        //***********************************************************

        //***********************************************************
        // This gives us the ability to drag the borderless form to a new location
        #region FormMove
        // ReSharper disable once InconsistentNaming
        const int WM_NCLBUTTONDOWN = 0xA1;
        // ReSharper disable once InconsistentNaming
        const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        private void Borderless_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        #endregion
        //***********************************************************

        //***********************************************************
        // This gives us the ability to resize the borderless from any borders instead of just the lower right corner
        #region FormResize
        protected override void WndProc(ref Message m)
        {
            const int wmNcHitTest = 0x84;
            const int htLeft = 10;
            const int htRight = 11;
            const int htTop = 12;
            const int htTopLeft = 13;
            const int htTopRight = 14;
            const int htBottom = 15;
            const int htBottomLeft = 16;
            const int htBottomRight = 17;
            const int gripOffset = 7;

            if (m.Msg == wmNcHitTest && 
                WindowState == FormWindowState.Normal)  // disable gripper on maximized window
            {
                int x = (int)(m.LParam.ToInt64() & 0xFFFF);
                int y = (int)((m.LParam.ToInt64() & 0xFFFF0000) >> 16);
                Point pt = PointToClient(new Point(x, y));
                Size clientSize = ClientSize;
                
                //allow resize on the lower right corner
                if (pt.X >= clientSize.Width - gripOffset && 
                    pt.Y >= clientSize.Height - gripOffset && 
                    clientSize.Height >= gripOffset)
                {
                    m.Result = (IntPtr)(IsMirrored ? htBottomLeft : htBottomRight);
                    return;
                }
                //allow resize on the lower left corner
                if (pt.X <= gripOffset && 
                    pt.Y >= clientSize.Height - gripOffset && 
                    clientSize.Height >= gripOffset)
                {
                    m.Result = (IntPtr)(IsMirrored ? htBottomRight : htBottomLeft);
                    return;
                }
                //allow resize on the upper right corner
                if (pt.X <= gripOffset && 
                    pt.Y <= gripOffset && 
                    clientSize.Height >= gripOffset)
                {
                    m.Result = (IntPtr)(IsMirrored ? htTopRight : htTopLeft);
                    return;
                }
                //allow resize on the upper left corner
                if (pt.X >= clientSize.Width - gripOffset && 
                    pt.Y <= gripOffset && 
                    clientSize.Height >= gripOffset)
                {
                    m.Result = (IntPtr)(IsMirrored ? htTopLeft : htTopRight);
                    return;
                }
                //allow resize on the top border
                if (pt.Y <= gripOffset && 
                    clientSize.Height >= gripOffset)
                {
                    m.Result = (IntPtr)(htTop);
                    return;
                }
                //allow resize on the bottom border
                if (pt.Y >= clientSize.Height - gripOffset && 
                    clientSize.Height >= gripOffset)
                {
                    m.Result = (IntPtr)(htBottom);
                    return;
                }
                //allow resize on the left border
                if (pt.X <= gripOffset && 
                    clientSize.Height >= gripOffset)
                {
                    m.Result = (IntPtr)(htLeft);
                    return;
                }
                //allow resize on the right border
                if (pt.X >= clientSize.Width - gripOffset && 
                    clientSize.Height >= gripOffset)
                {
                    m.Result = (IntPtr)(htRight);
                    return;
                }
            }
            base.WndProc(ref m);
        }
        #endregion
        //***********************************************************

        //***********************************************************
        // This has actions for close, minimize, maximize buttons on titlebar
        // as well as titlebar click/doubleclick and active inactive status
        #region TitleBar Controls/Clicks
        private void appBarmenuStrip_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                WindowState = (WindowState == FormWindowState.Maximized)
                    ? FormWindowState.Normal
                    : FormWindowState.Maximized;
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Close();
            }
            catch (ObjectDisposedException) { }
            catch (InvalidOperationException) { }
        }

        private void minimizetoolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void maximizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (WindowState)
            {
                case FormWindowState.Maximized:
                    WindowState = FormWindowState.Normal;
                    break;
                case FormWindowState.Normal:
                    WindowState = FormWindowState.Maximized;
                    break;
            }
        }

        #endregion
        //***********************************************************
    }
}
