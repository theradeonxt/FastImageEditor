using System.Drawing;
using VectorImageEdit.Modules.GraphicsCompositing;

namespace VectorImageEdit.Modules.Interfaces
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
        /// This is set by the frame or graphics handler (late initialization). 
        /// In external code you should not rely on this being initialized.
        /// </summary>
        /// <param name="dimensions"> The frame bounds </param>
        void SetFrameParameters(Size dimensions);
    }

    public static class RenderingPolicyFactory
    {
        public static IRenderingPolicy FullUpdate()
        {
            return new FullFrameUpdatePolicy();
        }
        public static IRenderingPolicy MinimalUpdatePolicy(Rectangle invalidatedRegion)
        {
            return new MinimalUpdatePolicy(invalidatedRegion);
        }
    }
}