using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Windows.Forms;  // TODO: Fix
using JetBrains.Annotations;
using NLog;
using VectorImageEdit.Models;
using VectorImageEdit.Modules.Interfaces;
using VectorImageEdit.Modules.LayerManagement;
using VectorImageEdit.Modules.Utility;

namespace VectorImageEdit.Modules.GraphicsCompositing
{
    ///// <summary>
    /// Graphics Module
    ///
    /// - controls the graphics rendering process
    /// - redraws a frame using all objects
    /// - handles the frame resizing
    /// 
    ///// </summary>
    public class GraphicsManager : IDisposable, IGraphicsHandler
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        //TODO: I was unable to replace this with graphics handle for now
        private readonly Control _formControl;   // reference to the form object used to draw on

        private Bitmap _frame;                   // buffer for the current frame data
        private Graphics _frameGraphics;         // graphics object used to draw directly on the frame data
        private Graphics _formGraphics;          // graphics object used to draw directly on the form control
        private readonly GraphicsProfiler _profiler;
        private readonly Action<GraphicsProfiler> _graphicsUpdateCallback;

        protected GraphicsManager(Control formControl, Action<GraphicsProfiler> graphicsUpdateCallback)
        {
            _profiler = new GraphicsProfiler();

            _formControl = formControl;
            _graphicsUpdateCallback = graphicsUpdateCallback;

            PerformResize();

            ImageProcessingApi.SetMultiThreadingStatus("AlphaBlend32bgra_32bgra", 0);
        }

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
                     _formGraphics = _formControl.CreateGraphics();
                     _frame = ImagingHelpers.Allocate(_formControl.Width + 1, _formControl.Height + 1);
                     _frameGraphics = Graphics.FromImage(_frame);

                     // Setup drawing parameters
                     ImagingHelpers.GraphicsFastDrawingWithBlending(_formGraphics);
                     ImagingHelpers.GraphicsFastDrawingWithBlending(_frameGraphics);
                 }
                 catch (Exception ex)
                 {
                     Logger.Error(ex.ToString());
                 }
             }
        }
        public void RefreshFrame(IRenderingPolicy renderPolicy)
        {
            _formGraphics.DrawImageUnscaledAndClipped(_frame, renderPolicy.DirtyRegion);
        }

        ///// <summary>
        /// Gets a bitmap representation of the workspace at the current moment.
        ///// </summary>
        [NotNull]
        public Bitmap GetImagePreview()
        {
            return ImagingHelpers.Allocate(_frame.Width, _frame.Height);
        }

        public void UpdateFrame(SortedContainer<Layer> objectCollection,
                                IRenderingPolicy renderPolicy)
        {
            // Clip the invalidated region to the size of the frame
            renderPolicy.SetFrameParameters(_frame.Size);

            _profiler.ProfileClearFrame(() =>
            {
                Color clearColor = Color.FromArgb(255, _formControl.BackColor);
                using (SolidBrush clearBrush = new SolidBrush(clearColor))
                {
                    _frameGraphics.FillRectangle(clearBrush, renderPolicy.DirtyRegion);
                }
            });

            _profiler.ProfileRasterizeObjects(() =>
            {
                // Lock the frame data and make it accessible for low-level access
                using (var frameData = new BitmapHelper(_frame))
                using (var rasterizer = new RasterizerStage(objectCollection))
                {
                    Parallel.For(renderPolicy.ScanlineBegin, renderPolicy.ScanlineEnd, y =>
                    {
                        ParallelScanlineRenderer(y, renderPolicy.DirtyRegion, frameData, rasterizer, objectCollection);
                    });
                }
            });

            // Draw the new completed frame on the visible area
            _profiler.ProfileDrawFrame(() => RefreshFrame(renderPolicy));

            // Notify interested clients with statistics after frame update
            _graphicsUpdateCallback(_profiler);
        }

        protected void UpdateSelection(Rectangle selectionBorder)
        {
            // Updates the selection rectangle of objects by drawing the selection over frame region
            _formGraphics.DrawRectangle(AppModel.Instance.LayerSelectionPen,
                selectionBorder.Left, selectionBorder.Top, selectionBorder.Width - 1, selectionBorder.Height - 1);
        }

        private unsafe void ParallelScanlineRenderer(int y, Rectangle dirtyRegion,
            [NotNull]BitmapHelper frameData,
            [NotNull]RasterizerStage rasterizer,
            [NotNull]SortedContainer<Layer> objectCollection)
        {
            /* 
             * Warning! Do not use the Bitmap objects for the frame or layers directly.
             * They are locked at this point and will throw InvalidOperationException.
             * Instead, use the RasterizerStage class to get access to their bitmaps.
             */

            byte* currentScanLine = frameData.Start + (y * frameData.Stride);

            // Do a Painter's algorithm iteration (back->front) on the layers
            foreach (Layer layer in objectCollection)
            {
                if (!ScanlineIntersects(y, layer.Region) || 
                    !dirtyRegion.IntersectsWith(layer.Region)) continue;

                // Find the bounds for the "dirty region" of scanline (clamped inside frame)
                int boundLeft = Math.Max(dirtyRegion.Left, Math.Min(layer.Region.Left, layer.Region.Right));
                int boundRight = Math.Min(dirtyRegion.Right, Math.Max(layer.Region.Left, layer.Region.Right));

                // Access the raw data structure of the rasterization
                BitmapData layerRaw = rasterizer.GetRasterInfo(layer.Metadata.Uid);

                // Calculate memory indexes
                byte* src = (byte*)(layerRaw.Scan0 + (y - layer.Region.Top) * layerRaw.Stride);
                byte* dst = currentScanLine + (boundLeft) * 4;
                int dirtySizeBytes = (boundRight - boundLeft + 1) * 4;

                // Composite scanline with layer data
                ImageProcessingApi.AlphaBlend32bgra_32bgra(src, dst, dst, (uint)dirtySizeBytes);
            }
        }

        private bool ScanlineIntersects(int y, Rectangle region)
        {
            return y >= region.Top && y <= region.Bottom;
        }

        private void DisposeGraphicsResources()
        {
            // Resources used by the graphics process (frame buffers, graphics objects) are disposed here
            if (_frame != null) _frame.Dispose();
            if (_frameGraphics != null) _frameGraphics.Dispose();
            if (_formGraphics != null) _formGraphics.Dispose();
        }
    }
}
