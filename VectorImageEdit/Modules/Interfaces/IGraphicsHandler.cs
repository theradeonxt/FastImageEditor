using JetBrains.Annotations;
using VectorImageEdit.Modules.LayerManagement;
using VectorImageEdit.Modules.Utility;

namespace VectorImageEdit.Modules.Interfaces
{
    public interface IGraphicsHandler
    {
        /// <summary>
        /// Performs an entire frame update process, including redrawing the new frame
        /// </summary>
        /// <param name="objectCollection"></param>
        /// <param name="renderPolicy"></param>
        void UpdateFrame([NotNull] SortedContainer<Layer> objectCollection,
                         [NotNull] IRenderingPolicy renderPolicy);

        /// <summary>
        /// Performs the resize logic when the area managed by this instance is resized
        /// Note: it does not redraw the frame!
        /// </summary>
        void PerformResize();
    }
}