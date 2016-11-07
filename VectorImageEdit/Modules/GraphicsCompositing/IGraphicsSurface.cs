using System.Drawing;

namespace VectorImageEdit.Modules.GraphicsCompositing
{
    public interface IGraphicsSurface
    {
        int GraphicsWidth { get; }
        int GraphicsHeight { get; }
        Color GraphicsBackground { get; }
        Graphics GetGraphics();
    }
}
