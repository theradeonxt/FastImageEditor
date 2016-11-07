using System;

namespace VectorImageEdit.Modules.GraphicsCompositing
{
    /// <summary>
    /// Defines a disposable objects that implements part of a rendering process
    /// </summary>
    interface IRenderingStage : IDisposable
    {
        /// <summary>
        /// Release all resources used by this stage (after it has completed)
        /// </summary>
        new void Dispose();
    }
}
