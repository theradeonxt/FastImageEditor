using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Windows.Forms;
using JetBrains.Annotations;
using NLog;
using VectorImageEdit.Models;
using VectorImageEdit.Modules.Layers;
using VectorImageEdit.Modules.Utility;

namespace VectorImageEdit.Modules
{
    /// <summary>
    /// Graphics Module
    ///
    /// - controls the graphics rendering process
    /// - redraws a frame using all objects
    /// - handles the frame resizing
    /// 
    /// </summary>
    public class GraphicsManager : IDisposable
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

            Resize();
        }

        public void Dispose()
        {
            DisposeGraphicsResources();
        }
        public void Resize()
        {
            _profiler.ProfileResizeFrame(() =>
            {
                // Clear resources used by previous frame
                DisposeGraphicsResources();

                // Allocate resources for the new frame
                try
                {
                    _formGraphics = _formControl.CreateGraphics();
                    _frame = ImagingHelpers.Allocate(_formControl.Width + 1, _formControl.Height + 1);
                    _frameGraphics = Graphics.FromImage(_frame);

                    ImagingHelpers.GraphicsFastDrawingWithBlending(_formGraphics);
                    ImagingHelpers.GraphicsFastDrawingWithBlending(_frameGraphics);
                }
                catch (OutOfMemoryException ex)
                {
                    Logger.Error(ex.ToString());
                }
                catch (ArgumentException ex)
                {
                    Logger.Error(ex.ToString());
                }
            });
        }
        public void RefreshFrame()
        {
            _formGraphics.DrawImageUnscaled(_frame, 0, 0);
        }
        public Bitmap GetImagePreview()
        {
            try
            {
                return ImagingHelpers.Allocate(_frame.Width, _frame.Height);
            }
            catch (OutOfMemoryException ex)
            {
                Logger.Error(ex.ToString());
            }
            return Properties.Resources.placeholder;
        }

        [SuppressMessage("ReSharper", "AccessToDisposedClosure")]
        public void UpdateFrame([NotNull]SortedContainer<Layer> objectCollection)
        {
            _profiler.ProfileClearFrame(() =>
            {
                // Clear frame with the opaque background color of the window
                _frameGraphics.Clear(Color.FromArgb(255, _formControl.BackColor));
            });

            _profiler.ProfileRasterizeObjects(() =>
            {
                // Lock the frame data and make sure that at this point 
                // we can access the low-level data of every layer
                using (var frameData = new BitmapHelper(_frame))
                using (var rasterizer = new RasterizerStage(objectCollection))
                {
                    Parallel.For(0, _frame.Height, y =>
                    {
                        ParallelScanlineRenderer(y, frameData, rasterizer, objectCollection);
                    });
                }
            });

            // Draw the new completed frame on the visible area
            _profiler.ProfileDrawFrame(RefreshFrame);

            // Notify interested clients with statistics after frame update
            //_graphicsUpdateCallback(_profiler);
        }

        protected void UpdateSelection(Rectangle selectionBorder)
        {
            // This only updates the selection rectangle of objects
            // Draw selection over frame region
            _formGraphics.DrawRectangle(AppGlobalData.Instance.LayerSelectionPen,
                selectionBorder.Left, selectionBorder.Top, selectionBorder.Width - 1, selectionBorder.Height - 1);
        }

        private unsafe void ParallelScanlineRenderer(int y,
            [NotNull]BitmapHelper frameData, [NotNull]RasterizerStage rasterizer,
            [NotNull]SortedContainer<Layer> objectCollection)
        {
            /* 
            * Warning! Do not use the Bitmap objects for the frame or layers!
            * They are locked at this point and will throw InvalidOperationException.
            */

            byte* currentScanLine = frameData.Start + (y * frameData.Stride);

            // Do a Painter's algorithm iteration (back->front) on the layers
            // to find intersections with the scanline
            foreach (Layer layer in objectCollection)
            {
                if (y < layer.Region.Top || y > layer.Region.Bottom) continue;

                // Find the bounds for the "dirty region" of scanline (clamped inside frame)
                int boundLeft = Math.Max(0, Math.Min(layer.Region.Left, layer.Region.Right));
                int boundRight = Math.Min(frameData.Width - 1,
                    Math.Max(layer.Region.Left, layer.Region.Right));

                BitmapData layerRaw = rasterizer.GetRasterInfo(layer.Metadata.Uid);

                byte* src = (byte*)(layerRaw.Scan0 + (y - layer.Region.Top) * layerRaw.Stride);
                byte* dst = currentScanLine + (boundLeft) * 4;
                int dirtySizeBytes = (boundRight - boundLeft + 1) * 4;

                ImageProcessingApi.AlphaBlend32bgra_32bgra(src, dst, dst, (uint)dirtySizeBytes);
            }
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
