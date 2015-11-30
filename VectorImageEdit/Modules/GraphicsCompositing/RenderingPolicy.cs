using System;
using System.Drawing;
using VectorImageEdit.Modules.Interfaces;

namespace VectorImageEdit.Modules.GraphicsCompositing
{
    /// <summary>
    /// Encapsulates parameters for an optimized frame update
    /// 
    /// This works by updating the invalidated region of the frame
    /// </summary>
    class MinimalUpdatePolicy : IRenderingPolicy
    {
        public MinimalUpdatePolicy(Rectangle invalidatedRegion)
        {
            DirtyRegion = invalidatedRegion;
        }

        public Rectangle DirtyRegion { get; private set; }
        public int ScanlineBegin { get; private set; }
        public int ScanlineEnd { get; private set; }

        public void SetFrameParameters(Size dimensions)
        {
            DirtyRegion = Rectangle.Intersect(DirtyRegion, new Rectangle(0, 0, dimensions.Width, dimensions.Height));
            ScanlineBegin = Math.Min(dimensions.Height, Math.Max(0, DirtyRegion.Top));
            ScanlineEnd = Math.Min(dimensions.Height, DirtyRegion.Bottom);
        }
    }

    /// <summary>
    /// Encapsulates parameters for a full frame update 
    /// </summary>
    class FullFrameUpdatePolicy : IRenderingPolicy
    {
        public Rectangle DirtyRegion { get; set; }
        public int ScanlineBegin { get; private set; }
        public int ScanlineEnd { get; private set; }

        public void SetFrameParameters(Size dimensions)
        {
            ScanlineBegin = 0;
            ScanlineEnd = dimensions.Height;
            DirtyRegion = new Rectangle(0, 0, dimensions.Width, dimensions.Height);
        }
    }
}
