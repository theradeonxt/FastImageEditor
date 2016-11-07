using System.Drawing;
using VectorImageEdit.Modules.GraphicsCompositing;
using VectorImageEdit.Modules.Interfaces;

namespace VectorImageEdit.Models
{
    class WindowModel
    {
        public void WorkspaceResize(Size newDimensions)
        {
            ILayerHandler layerMan = AppModel.Instance.LayerManager;
            IGraphicsHandler graphicsMan = (IGraphicsHandler)layerMan;
            graphicsMan.PerformResize();

            graphicsMan.UpdateFrame(layerMan.WorkspaceLayers, RenderingPolicy.FullUpdate());
        }
        public void AppWindowMove()
        {
            // Notes: Under Windows 7, Vista and later with AERO Compositing Enabled
            // it's guaranteed that other windows don't overwrite the application graphics.
            // Under Windows XP this is always required, because of how GDI works.
            AppModel.Instance.LayerManager.RefreshFrame(RenderingPolicy.FullUpdate());
        }
    }
}
