using System.Drawing;

namespace VectorImageEdit.Modules.GraphicsCompositing
{
    /// <summary>
    /// Encapsulates parameters for an optimized frame update
    /// 
    /// This works by updating only the invalidated region of the frame
    /// </summary>
    class MinimalUpdate : IRenderingPolicy
    {
        public MinimalUpdate(Rectangle invalidatedRegion, Rectangle oldRegion = default(Rectangle))
        {
            DirtyRegion = invalidatedRegion;
            OldRegion = oldRegion;
        }

        public Rectangle OldRegion { get; set; }
        public Rectangle DirtyRegion { get; private set; }
        public int ScanlineBegin { get; private set; }
        public int ScanlineEnd { get; private set; }

        public void ClipToFrame(Size frameSize)
        {
            DirtyRegion = Rectangle.Intersect(DirtyRegion, new Rectangle(0, 0, frameSize.Width, frameSize.Height));
            ScanlineBegin = DirtyRegion.Top;
            ScanlineEnd = DirtyRegion.Bottom - 1;
        }
    }

    /// <summary>
    /// Encapsulates parameters for a full frame update 
    /// </summary>
    class FullFrameUpdate : IRenderingPolicy
    {
        public Rectangle DirtyRegion { get; set; }
        public int ScanlineBegin { get; private set; }
        public int ScanlineEnd { get; private set; }

        public void ClipToFrame(Size frameSize)
        {
            ScanlineBegin = 0;
            ScanlineEnd = frameSize.Height - 1;
            DirtyRegion = new Rectangle(0, 0, frameSize.Width, frameSize.Height);
        }
    }
}
