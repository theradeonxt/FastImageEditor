using System.Drawing;
using System.Windows.Forms;
using VectorImageEdit.Modules.GraphicsCompositing;
using VectorImageEdit.WindowsFormsBridge;

namespace VectorImageEdit.Views.Main
{
    public partial class AppWindow : IGraphicsSurface
    {
        #region IGraphicsSurface Implementation

        public int GraphicsHeight
        {
            get { return WorkspaceSize.Height; }
        }

        public int GraphicsWidth
        {
            get { return WorkspaceSize.Width; }
        }

        public Size GraphicsSize
        {
            get { return WorkspaceSize; }
        }

        public Color GraphicsBackground
        {
            get { return pboxWorkspaceRegion.BackColor; }
        }

        public Graphics GetGraphics()
        {
            return pboxWorkspaceRegion.CreateGraphics();
        }

        #endregion

        public Size WorkspaceSize
        {
            get { return pboxWorkspaceRegion.Size; }
        }

        public void AddWorkspaceMouseDownListener(IMouseListener listener)
        {
            pboxWorkspaceRegion.MouseDown += listener.ActionPerformed;
        }
        public void AddWorkspaceMouseMoveListener(IMouseListener listener)
        {
            pboxWorkspaceRegion.MouseMove += listener.ActionPerformed;
        }
        public void AddWorkspaceMouseUpListener(IMouseListener listener)
        {
            pboxWorkspaceRegion.MouseUp += listener.ActionPerformed;
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
