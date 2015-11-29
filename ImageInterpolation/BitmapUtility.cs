using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace ImageInterpolation
{
    /// <summary>
    /// Defines the quality level used for image transformations.
    /// By default, ConversionQuality.LowQuality is used.
    /// </summary>
    public enum ConversionQuality
    {
        /// <summary>
        /// Bicubic interpolation and Antialiasing
        /// </summary>
        HighQuality,
        /// <summary>
        /// Nearest neighbor interpolation and no prefiltering
        /// </summary>
        LowQuality
    };

    /// <summary>
    /// This specifies the relation between input and output image.
    /// </summary>
    public enum ConversionType
    {
        /// <summary>
        /// Output is a new object with raw copy of input data
        /// </summary>
        Copy,
        /// <summary>
        /// Output replaces the input and the input image is disposed
        /// </summary>
        Overwrite
    };

    static class BitmapUtility
    {
        /// <summary>
        ///     Converts the input image to a desired PixelFormat using
        ///     a specified conversion quality and conversion type.
        /// </summary>
        /// <param name="img"> Input image </param>
        /// <param name="format"> Desired PixelFormat </param>
        /// <param name="quality"> Conversion quality </param>
        /// <param name="type"> Conversion type </param>
        /// <returns> Resulting image </returns>

        public static Bitmap ConvertToFormat(
            Bitmap img,
            PixelFormat format,
            ConversionQuality quality = ConversionQuality.LowQuality,
            ConversionType type = ConversionType.Copy)
        {
            if (format == img.PixelFormat) return img;

            var bmp = new Bitmap(img.Width, img.Height, format);
            using (var g = Graphics.FromImage(bmp))
            {
                SetConversionQuality(g, quality);
                g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height));
            }
            SetConversionType(ref img, bmp, type);
            return bmp;
        }

        /// <summary>
        ///     Scales the input image to a desired size using
        ///     a specified conversion quality and conversion type.
        /// </summary>
        /// <param name="img"> Input image </param>
        /// <param name="size"> Output size </param>
        /// <param name="quality"> Conversion quality </param>
        /// <param name="type"> Conversion type </param>
        /// <returns> Resulting image </returns>

        public static Bitmap Resize(
            Bitmap img, Size size,
            ConversionQuality quality = ConversionQuality.LowQuality,
            ConversionType type = ConversionType.Copy)
        {
            if (img.Size == size) return img;

            var scaled = new Bitmap(size.Width, size.Height, img.PixelFormat);
            using (var g = Graphics.FromImage(scaled))
            {
                SetConversionQuality(g, quality);
                g.DrawImage(img, new Rectangle(0, 0, size.Width, size.Height));
            }
            SetConversionType(ref img, scaled, type);
            return scaled;
        }

        /// <summary>
        ///     Creates a copy of the input image using a specified conversion type. 
        /// </summary>
        /// <param name="img"> Input image </param>
        /// <param name="type"> Conversion type </param>
        /// <returns> Resulting image </returns>

        public static Bitmap Copy(Bitmap img, ConversionType type = ConversionType.Copy)
        {
            var bmp = new Bitmap(img.Width, img.Height, img.PixelFormat);
            using (var g = Graphics.FromImage(bmp))
            {
                SetConversionQuality(g, ConversionQuality.LowQuality);
                g.DrawImageUnscaled(img, new Rectangle(0, 0, img.Width, img.Height));
            }
            SetConversionType(ref img, bmp, type);
            return bmp;
        }

        private static void SetConversionQuality(Graphics graphics, ConversionQuality quality)
        {
            switch (quality)
            {
                case ConversionQuality.LowQuality:
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.CompositingQuality = CompositingQuality.HighSpeed;
                    graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                    graphics.SmoothingMode = SmoothingMode.None;
                    break;
                case ConversionQuality.HighQuality:
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    break;
            }
        }

        private static void SetConversionType(ref Bitmap original, Bitmap result, ConversionType type)
        {
            switch (type)
            {
                case ConversionType.Copy:
                    break;
                case ConversionType.Overwrite:
                    original.Dispose();
                    original = result;
                    break;
            }
        }
    }
}
