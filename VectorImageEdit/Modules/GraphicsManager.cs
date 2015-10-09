using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;
using VectorImageEdit.Models;
using VectorImageEdit.Modules.Layers;
using VectorImageEdit.Modules.Utility;

namespace VectorImageEdit.Modules
{
    // TODO: Fix naming for this
    public enum ClearMode
    {
        UpdateOld,
        NoClear,
        FullUpdate
    }

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
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        //TODO: I was unable to replace this with graphics handle for now
        private readonly Control _formControl;   // reference to the form object used to draw on

        private Bitmap _frame;                   // buffer for the current frame data

        private Graphics _frameGraphics;         // graphics object used to draw directly on the frame
        private Graphics _formGraphics;          // graphics object used to draw directly on the form control

        protected GraphicsManager(Control formControl)
        {
            _formControl = formControl;

            Resize();
        }

        public void Dispose()
        {
            DisposeGraphicsResources();
        }
        public void Resize()
        {
            // Clear resources used by previous frame
            DisposeGraphicsResources();

            // Allocate resources for the new frame
            try
            {
                _formGraphics = _formControl.CreateGraphics();
                _frame = ImagingHelpers.Allocate(_formControl.Width + 1, _formControl.Height + 1);
                //_frameBackup = ImagingHelpers.Allocate(_formControl.Width + 1, _formControl.Height + 1);
                _frameGraphics = Graphics.FromImage(_frame);
                //_frameBackupGraphics = Graphics.FromImage(_frameBackup);

                ImagingHelpers.GraphicsFastDrawing(_formGraphics);
                ImagingHelpers.GraphicsFastDrawing(_frameGraphics);
                //ImagingHelpers.GraphicsFastDrawing(_frameBackupGraphics);
            }
            catch (OutOfMemoryException ex)
            {
                _logger.Error(ex.ToString());
            }
            catch (ArgumentException ex)
            {
                _logger.Error(ex.ToString());
            }
        }
        public void RefreshFrame()
        {
            _formGraphics.DrawImageUnscaled(_frame, 0, 0);
        }
        public Bitmap GetImagePreview()
        {
            try
            {
                Bitmap preview = new Bitmap(_frame);
                return preview;
            }
            catch (OutOfMemoryException ex)
            {
                _logger.Error(ex.ToString());
            }
            return Properties.Resources.placeholder;
        }

        [DllImport("ImageProcessing.dll", CallingConvention = CallingConvention.Cdecl)]
        private static unsafe extern int AlphaBlend32bgra_32bgra(
            byte* source,
            byte* target,
            byte* destination,
            uint sizeBytes
        );

        [SuppressMessage("ReSharper", "AccessToDisposedClosure")]
        public void UpdateFrame(SortedContainer<Layer> objectCollection)
        {
            _frameGraphics.Clear(_formControl.BackColor);

            // Lock the frame data and make sure that at this point 
            // we can access the low-level data of every layer
            using (var frameData = new BitmapHelper(_frame))
            using (var rasterizer = new RasterizerStage(objectCollection))
            {
                int frameWidth = _frame.Width;
                Parallel.For(0, _frame.Height, y =>
                {
                    unsafe
                    {
                        byte* currentScanLine = frameData.Start + (y * frameData.Stride);

                        // Do a Painter's algorithm iteration (back->front) on the layers
                        // to find intersections with the scanline
                        lock (objectCollection.SyncRoot)
                        {
                            foreach (Layer layer in objectCollection)
                            {
                                if (y < layer.Region.Top || y > layer.Region.Bottom) continue;

                                // Find the bounds for the "dirty region" of scanline (clamped inside frame)
                                int boundLeft = Math.Max(0, Math.Min(layer.Region.Left, layer.Region.Right));
                                int boundRight = Math.Min(frameWidth - 1,
                                    Math.Max(layer.Region.Left, layer.Region.Right));

                                BitmapData layerRaw = rasterizer.GetRasterInfo(layer.Metadata.Uid);

                                byte* src = (byte*)(layerRaw.Scan0 + 
                                    (y - layer.Region.Top) * layerRaw.Stride + 
                                    boundLeft * 4);
                                byte* dst = currentScanLine + (boundLeft-1) * frameData.PixelSize;
                                int dirtySizeBytes = (boundRight - boundLeft + 1) * 4;

                                AlphaBlend32bgra_32bgra(src, dst, dst, (uint)dirtySizeBytes);
                            }
                        }
                    }
                });
            }

            RefreshFrame();
        }

        protected void UpdateSelection(Rectangle selectionBorder)
        {
            // This only updates the selection rectangle of objects
            // Draw selection over frame region
            _formGraphics.DrawRectangle(AppGlobalData.Instance.LayerSelectionPen,
                selectionBorder.Left, selectionBorder.Top, selectionBorder.Width - 1, selectionBorder.Height - 1);
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
