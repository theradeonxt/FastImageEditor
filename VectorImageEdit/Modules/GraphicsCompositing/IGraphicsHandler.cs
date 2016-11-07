using System.Drawing;
using JetBrains.Annotations;
using VectorImageEdit.Modules.LayerManagement;
using VectorImageEdit.Modules.Utility;

namespace VectorImageEdit.Modules.GraphicsCompositing
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
        /// Draws a selection cue on the specified area.
        /// </summary>
        /// <param name="selectionBorder"> The area to select </param>
        void UpdateSelection(Rectangle selectionBorder);

        /// <summary>
        /// Performs the resize logic when the area managed by this instance is resized
        /// Note: it does not redraw the frame!
        /// </summary>
        void PerformResize();
    }
}