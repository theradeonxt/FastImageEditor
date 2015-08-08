using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
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
        private Control _formControl;            // reference to the form object used to draw on

        private Bitmap _frame;                   // buffer for the current frame data
        private Bitmap _frameBackup;             // buffer with a copy of the frame data

        private Graphics _frameGraphics;         // graphics object used to draw directly on the frame
        private Graphics _frameBackupGraphics;   // graphics object used to draw directly on frame copy
        private Graphics _formGraphics;          // graphics object used to draw directly on the form control
        private Rectangle _frameBackupRegion;    // the region covered by the selection border

        // TODO: move to global styles
        private readonly Pen _borderStyle;       // the style used to draw the selection border

        public GraphicsManager(Control formControl)
        {
            _formControl = formControl;

            // default aspect of selection border
            _borderStyle = new Pen(Brushes.Black, 3);
            _borderStyle.DashStyle = DashStyle.Dash;

            Resize(formControl);
        }

        ~GraphicsManager()
        {
            ClearResources();
        }

        /////////////////////////////////
        //
        // Additional functionality
        //
        /////////////////////////////////

        public void Resize(Control formControl)
        {
            _formControl = formControl;

            // Clear resources used by previous frame
            ClearResources();

            // Allocate resources for the new frame
            _formGraphics = formControl.CreateGraphics();
            GraphicsLowQ(_formGraphics);
            _frame = new Bitmap(formControl.Width + 1, formControl.Height + 1, _formGraphics);
            _frameBackup = new Bitmap(formControl.Width + 1, formControl.Height + 1, _formGraphics);
            _frameGraphics = Graphics.FromImage(_frame);
            GraphicsLowQ(_frameGraphics);
            _frameBackupGraphics = Graphics.FromImage(_frameBackup);
            GraphicsLowQ(_frameBackupGraphics);
        }

        void GraphicsLowQ(Graphics g)
        {
            g.CompositingQuality = CompositingQuality.HighSpeed;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.SmoothingMode = SmoothingMode.None;
            g.CompositingMode = CompositingMode.SourceCopy;
        }

        public Bitmap GetImagePreview()
        {
            Bitmap prev = new Bitmap(_frame);
            return prev;
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
            // Clears the frame and redraws all the objects
            _frameGraphics.Clear(_formControl.BackColor);

            foreach (Layer layer in objectCollection)
            {
                layer.DrawGraphics(_frameGraphics);
            }
            RefreshFrame();
        }

        public void UpdateSelection(Rectangle selectionBorder, ClearMode mode)
        {
            // This only updates the selection rectangle of objects

            // Restore the previously saved region
            if (mode == ClearMode.UpdateOld)
            {
                _formGraphics.DrawImage(_frameBackup, _frameBackupRegion, _frameBackupRegion, GraphicsUnit.Pixel);
            }

            // Copy the affected region of the frame 
            // to a backup frame
            _frameBackupGraphics.DrawImage(_frame, selectionBorder, selectionBorder, GraphicsUnit.Pixel);

            // Draw selection over frame region
            _formGraphics.DrawRectangle(_borderStyle, selectionBorder.Left, selectionBorder.Top, selectionBorder.Width - 1, selectionBorder.Height - 1);

            _frameBackupRegion = selectionBorder;
            _frameBackupRegion.Inflate(1, 1);
        }

        private void ClearResources()
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
