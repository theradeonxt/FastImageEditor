using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace VectorImageEdit.Modules.Utility
{
    static class ImagingHelpers
    {
        public static void GraphicsFastDrawing(Graphics g)
        {
            if (g == null) return;
            g.CompositingQuality = CompositingQuality.HighSpeed;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.SmoothingMode = SmoothingMode.None;
            g.CompositingMode = CompositingMode.SourceCopy;
        }

        public static void GraphicsFastDrawingWithBlending(Graphics g)
        {
            if (g == null) return;
            g.CompositingQuality = CompositingQuality.HighSpeed;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.SmoothingMode = SmoothingMode.None;
            g.CompositingMode = CompositingMode.SourceOver;
        }

        public static void GraphicsFastResize(Graphics g)
        {
            if (g == null) return;
            GraphicsFastDrawing(g);
        }

        public static void GraphicsFastResizeBilinear(Graphics g)
        {
            if (g == null) return;
            g.CompositingQuality = CompositingQuality.HighSpeed;
            g.InterpolationMode = InterpolationMode.Bilinear;
            g.SmoothingMode = SmoothingMode.None;
            g.CompositingMode = CompositingMode.SourceCopy;
        }

        /// <summary>
        /// Creates a new Bitmap object with the specified size using an internal PixelFormat
        /// </summary>
        /// <param name="width"> Input width </param>
        /// <param name="height"> Input height </param>
        /// <returns> Output Bitmap </returns>
        public static Bitmap Allocate(int width, int height)
        {
            // TODO: remove the null
            // TODO: Clone might not work if OutOfMemoryException - ensure Properties.Resources.placeholder has the right image format
            try
            {
                return new Bitmap(width, height, PixelFormat.Format32bppArgb);
            }
            catch (OutOfMemoryException)
            {
                Bitmap errorImage = Properties.Resources.placeholder;
                try
                {
                    return errorImage.Clone(new Rectangle(0, 0, errorImage.Width, errorImage.Height),
                        PixelFormat.Format32bppArgb);
                }
                catch (OutOfMemoryException) { }
                catch (ArgumentException) { }
            }
            return null;
        }
    }
}
