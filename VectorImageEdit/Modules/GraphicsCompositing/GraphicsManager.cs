using System;
using System.Drawing;
using JetBrains.Annotations;
using NLog;
using VectorImageEdit.Models;
using VectorImageEdit.Modules.LayerManagement;
using VectorImageEdit.Modules.Utility;

namespace VectorImageEdit.Modules.GraphicsCompositing
{
    /// <summary>
    /// Graphics Module
    ///
    /// - controls the graphics rendering process
    /// - redraws a frame using all objects
    /// - handles the frame resizing
    /// 
    /// </summary>
    public class GraphicsManager : IDisposable, IGraphicsHandler
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IGraphicsSurface surfaceInfo;

        private Bitmap frame;                   // buffer for the current frame data
        private Graphics frameGraphics;         // graphics object used to draw directly on the frame data
        private Graphics formGraphics;          // graphics object used to draw directly on the form control

        private readonly GraphicsProfiler profiler;
        private readonly Action<GraphicsProfiler> graphicsUpdateCallback;

        protected GraphicsManager(IGraphicsSurface surfaceInfo, Action<GraphicsProfiler> graphicsUpdateCallback)
        {
            profiler = new GraphicsProfiler();

            this.surfaceInfo = surfaceInfo;
            this.graphicsUpdateCallback = graphicsUpdateCallback;

            PerformResize();

            ImageProcessingApi.SetMultiThreadingStatus("AlphaBlend32bgra_32bgra", 0);
        }

        #region Interface Implementation

        public void Dispose()
        {
            DisposeGraphicsResources();
        }

        public void PerformResize()
        {
            //_profiler.ProfileResizeFrame(() =>
            {
                // Clear resources used by previous frame
                DisposeGraphicsResources();

                try
                {
                    // Allocate resources for the new frame
                    formGraphics = surfaceInfo.GetGraphics();
                    frame = ImagingHelpers.Allocate(surfaceInfo.GraphicsWidth > 0 ? surfaceInfo.GraphicsWidth : 1,
                        surfaceInfo.GraphicsHeight > 0 ? surfaceInfo.GraphicsHeight : 1);
                    frameGraphics = Graphics.FromImage(frame);

                    // Setup drawing parameters
                    ImagingHelpers.GraphicsFastDrawingWithBlending(formGraphics);
                    ImagingHelpers.GraphicsFastDrawing(frameGraphics);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.ToString());
                }
            }
        }
        public void RefreshFrame(IRenderingPolicy renderPolicy)
        {
            formGraphics.DrawImageUnscaledAndClipped(frame, renderPolicy.DirtyRegion);
        }

        public void UpdateFrame(SortedContainer<Layer> objectCollection,
                                IRenderingPolicy renderPolicy)
        {
            // Clip the invalidated region to the size of the frame
            renderPolicy.ClipToFrame(frame.Size);

            // Refreh the invalidated frame/region
            profiler.ProfileClearFrame(() =>
            {
                Color clearColor = Color.FromArgb(255, surfaceInfo.GraphicsBackground);
                using (SolidBrush clearBrush = new SolidBrush(clearColor))
                {
                    /*var localPolicy = renderPolicy as MinimalUpdate;
                    if (localPolicy != null && localPolicy.OldRegion != default(Rectangle))
                    {
                        _frameGraphics.FillRectangle(clearBrush, localPolicy.OldRegion);
                    }*/
                    frameGraphics.FillRectangle(clearBrush, renderPolicy.DirtyRegion);
                }
            });

            profiler.ProfileRasterizeObjects(() =>
            {
                using (var frameInfo = new BitmapHelper(frame)) // lock framebuffer data
                using (var rasterizer = new RasterizerStage(objectCollection, surfaceInfo)) // rasterize all scene objects
                using (var compositor = new CompositionStage(objectCollection, rasterizer))
                {
                    // perform scene composition on needed regions
                    compositor.Composite(renderPolicy, frameInfo);
                }
            });

            // Draw the new completed frame/region on the visible area
            profiler.ProfileDrawFrame(() =>
            {
                RefreshFrame(renderPolicy);
            });

            // Notify interested clients with statistics after frame update
            if (graphicsUpdateCallback != null) graphicsUpdateCallback.BeginInvoke(profiler, null, null);
        }

        public void UpdateSelection(Rectangle selectionBorder)
        {
            // Updates the selection rectangle of objects by drawing the selection over frame region
            formGraphics.DrawRectangle(AppModel.Instance.LayerSelectionPen,
                selectionBorder.Left, selectionBorder.Top, selectionBorder.Width - 1, selectionBorder.Height - 1);
        }

        #endregion

        /// <summary>
        /// Gets a bitmap representation of the workspace at the current moment.
        /// Note: the caller is responsible to release the bitmap resources.
        /// </summary>
        [NotNull]
        public Bitmap GetImagePreview()
        {
            // BUG: Clone image!
            return ImagingHelpers.Allocate(frame.Width, frame.Height);
        }

        private void DisposeGraphicsResources()
        {
            // Resources used by the graphics process (frame buffers, graphics objects) are disposed here
            if (frame != null) frame.Dispose();
            if (frameGraphics != null) frameGraphics.Dispose();
            if (formGraphics != null) formGraphics.Dispose();
        }
    }
}
