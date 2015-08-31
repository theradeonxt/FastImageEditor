using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Windows.Forms;
using VectorImageEdit.Modules.BasicShapes;
using VectorImageEdit.Modules.Layers;

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

        /////////////////////////////////
        //
        // Additional functionality
        //
        /////////////////////////////////

        public void Resize()
        {
            // Clear resources used by previous frame
            DisposeGraphicsResources();

            // Allocate resources for the new frame
            _formGraphics = _formControl.CreateGraphics();
            GraphicsLow(_formGraphics);
            _frame = new Bitmap(_formControl.Width + 1, _formControl.Height + 1, _formGraphics);
            _frameBackup = new Bitmap(_formControl.Width + 1, _formControl.Height + 1, _formGraphics);
            _frameGraphics = Graphics.FromImage(_frame);
            GraphicsLow(_frameGraphics);
            _frameBackupGraphics = Graphics.FromImage(_frameBackup);
            GraphicsLow(_frameBackupGraphics);
        }

        private void GraphicsLow(Graphics g)
        {
            g.CompositingQuality = CompositingQuality.HighSpeed;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.SmoothingMode = SmoothingMode.None;
            g.CompositingMode = CompositingMode.SourceCopy;
        }

        public Bitmap GetImagePreview()
        {
            Bitmap preview = new Bitmap(_frame);
            return preview;
        }

        /////////////////////////////////
        //
        // Object Rendering
        //
        /////////////////////////////////

        public void RefreshFrame()
        {
            _formGraphics.DrawImageUnscaled(_frame, 0, 0);
        }

        public void UpdateFrame(SortedContainer objectCollection)
        {
            _frameGraphics.Clear(_formControl.BackColor);

            /*foreach (Layer layer in objectCollection)
            {
                layer.DrawGraphics(_frameGraphics);
            }*/

            var layerMapping = new ConcurrentDictionary<int, Layer>();

            using (BitmapHelper frameData = new BitmapHelper(_frame))
            {
                unsafe
                {
                    Parallel.For(0, _frame.Height, y =>
                    {
                        byte* currentLine = (byte*)frameData.Start + (y * frameData.Stride);

                        // intersection of layers with current scanline
                        foreach (Layer layer in objectCollection)
                        {
                            if (layer is ShapeBase)
                            {
                                if (layerMapping.ContainsKey(layer.Uid)) continue;
                                layer.DrawGraphics(_frameGraphics);
                            }
                            else
                            {

                            }
                        }
                    });
                }
            }

            RefreshFrame();
        }

        protected void UpdateSelection(Rectangle selectionBorder, ClearMode mode)
        {
            // This only updates the selection rectangle of objects

            // Restore the previously saved region
            if (mode == ClearMode.UpdateOld)
            {
                _formGraphics.DrawImage(_frameBackup, _frameBackupRegion, _frameBackupRegion, GraphicsUnit.Pixel);
            }

            // Copy the affected region of the frame to a backup frame
            _frameBackupGraphics.DrawImage(_frame, selectionBorder, selectionBorder, GraphicsUnit.Pixel);

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
