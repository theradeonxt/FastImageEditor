using System.Drawing;
using System.Windows.Forms;
using VectorImageEdit.WindowsFormsBridge;

namespace VectorImageEdit.Views.Main
{
    public partial class AppWindow
    {
        public Size WorkspaceSize
        {
            get { return pboxWorkspaceRegion.Size; }
        }

        public void AddWorkspaceMouseDownListener(IMouseListener listener)
        {
            WorkspaceArea.MouseDown += listener.ActionPerformed;
        }
        public void AddWorkspaceMouseMoveListener(IMouseListener listener)
        {
            WorkspaceArea.MouseMove += listener.ActionPerformed;
        }
        public void AddWorkspaceMouseUpListener(IMouseListener listener)
        {
            WorkspaceArea.MouseUp += listener.ActionPerformed;
        }

        public void SetLayerResizeCursor()
        {
            Cursor.Current = Cursors.SizeAll;
        }
        public void SetLayerMoveCursor()
        {
            Cursor.Current = Cursors.Hand;
        }
        public void SetDefaultCursor()
        {
            Cursor.Current = Cursors.Default;
        }
    }
}
