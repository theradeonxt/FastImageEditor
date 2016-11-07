using System.Drawing;

namespace VectorImageEdit.Modules.GraphicsCompositing
{
    /// <summary>
    /// Generic parameters that instruct how to perform a frame update
    /// </summary>
    public interface IRenderingPolicy
    {
        Rectangle DirtyRegion { get; }
        int ScanlineBegin { get; }
        int ScanlineEnd { get; }

        /// <summary>
        /// Clips the invalidated region to the size of the frame
        /// </summary>
        /// <param name="frameSize"> The frame dimensions </param>
        void ClipToFrame(Size frameSize);
    }

    public static class RenderingPolicy
    {
        public static IRenderingPolicy FullUpdate()
        {
            return new FullFrameUpdate();
        }
        public static IRenderingPolicy MinimalUpdatePolicy(Rectangle invalidatedRegion, Rectangle oldRegion = default(Rectangle))
        {
            return new MinimalUpdate /*FullFrameUpdatePolicy*/(invalidatedRegion, oldRegion);
        }
    }
}