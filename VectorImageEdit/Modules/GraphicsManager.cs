using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;
using VectorImageEdit.Modules.Layers;
using VectorImageEdit.Modules.Utility;

namespace VectorImageEdit.Modules
{
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
    public class GraphicsManager
    {
        // TODO: unable to replace this with graphics handle for now
        private readonly Control _formControl;   // reference to the form object used to draw on

        private Bitmap _frame;                   // buffer for the current frame data
        private Bitmap _frameBackup;             // buffer with a copy of the frame data

        private Graphics _frameGraphics;         // graphics object used to draw directly on the frame
        private Graphics _frameBackupGraphics;   // graphics object used to draw directly on frame copy
        private Graphics _formGraphics;          // graphics object used to draw directly on the form control
        private Rectangle _frameBackupRegion;    // the region covered by the selection border

        // TODO: move to global styles
        private readonly Pen _borderStyle;       // the style used to draw the selection border

        protected GraphicsManager(Control formControl)
        {
            _formControl = formControl;

            // default aspect of selection border
            _borderStyle = new Pen(Brushes.Black, 3) { DashStyle = DashStyle.Dash };

            Resize();
        }

        ~GraphicsManager()
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
                _frameBackup = ImagingHelpers.Allocate(_formControl.Width + 1, _formControl.Height + 1);
                _frameGraphics = Graphics.FromImage(_frame);
                _frameBackupGraphics = Graphics.FromImage(_frameBackup);
            }
            catch (OutOfMemoryException) { }
            catch (ArgumentException) { }

            ImagingHelpers.GraphicsFastDrawing(_formGraphics);
            ImagingHelpers.GraphicsFastDrawing(_frameGraphics);
            ImagingHelpers.GraphicsFastDrawing(_frameBackupGraphics);
        }

        public Bitmap GetImagePreview()
        {
            Bitmap preview = new Bitmap(_frame);
            return preview;
        }

        public void RefreshFrame()
        {
            _formGraphics.DrawImageUnscaled(_frame, 0, 0);
        }

        [SuppressMessage("ReSharper", "AccessToDisposedClosure")]
        public void UpdateFrame(SortedContainer<Layer> objectCollection)
        {
            _frameGraphics.Clear(_formControl.BackColor);

            // Lock the frame data
            using (var frameData = new BitmapHelper(_frame))
            using (var rasterizer = new RasterizerStage(objectCollection))
            {
                Parallel.For(0, _frame.Height, y =>
                {
                    unsafe
                    {
                        byte* currentLine = (byte*)frameData.Start + (y * frameData.Stride);

                        // intersection of layers with current scanline
                        foreach (Layer layer in objectCollection)
                        {
                            if (y < layer.Region.Top || y > layer.Region.Bottom) continue;


                        }
                    }
                });
            }

            RefreshFrame();
        }

        protected void UpdateSelection(Rectangle selectionBorder, ClearMode mode)
        {
            // This only updates the selection rectangle of objects

            // Restore the previously saved region
            if (mode == ClearMode.UpdateOld)
            {
                _formGraphics.DrawImage(_frameBackup, _frameBackupRegion, _frameBackupRegion,
                    GraphicsUnit.Pixel);
            }

            // Copy the affected region of the frame to a backup frame
            _frameBackupGraphics.DrawImage(_frame, selectionBorder, selectionBorder,
                GraphicsUnit.Pixel);

            // Draw selection over frame region
            _formGraphics.DrawRectangle(_borderStyle,
                selectionBorder.Left, selectionBorder.Top, selectionBorder.Width - 1, selectionBorder.Height - 1);

            _frameBackupRegion = selectionBorder;
            _frameBackupRegion.Inflate(1, 1);
        }

        private void DisposeGraphicsResources()
        {
            // Resources used by the graphics process (frame buffers, graphics objects) are disposed here
            if (_frame != null) _frame.Dispose();
            if (_frameBackup != null) _frameBackup.Dispose();
            if (_frameGraphics != null) _frameGraphics.Dispose();
            if (_frameBackupGraphics != null) _frameBackupGraphics.Dispose();
            if (_formGraphics != null) _formGraphics.Dispose();
        }
    }
}
