using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using JetBrains.Annotations;
using NLog;

namespace VectorImageEdit.Modules.Utility
{
    /// <summary>
    /// Utility class that does trivial GDI+ Image manipulation tasks.
    /// </summary>
    static class ImagingHelpers
    {
        [NotNull]
        private static readonly string ImagesFilter;
        [NotNull]
        private static readonly Bitmap ErrorImage;
        [NotNull]
        private static readonly Dictionary<string, ImageFormat> MapExtToFormat;
        [NotNull]
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        static ImagingHelpers()
        {
            MapExtToFormat = CreateFilterDictionary();
            ImagesFilter = CreateFilterMap();
            ErrorImage = CreateDefaultErrorImage();
        }

        public static void GraphicsFastDrawing([NotNull]Graphics g)
        {
            g.CompositingQuality = CompositingQuality.HighSpeed;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.SmoothingMode = SmoothingMode.None;
            g.CompositingMode = CompositingMode.SourceCopy;
        }
        public static void GraphicsFastDrawingWithBlending([NotNull]Graphics g)
        {
            g.CompositingQuality = CompositingQuality.HighSpeed;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.SmoothingMode = SmoothingMode.None;
            g.CompositingMode = CompositingMode.SourceOver;
        }
        public static void GraphicsFastResizePixelated([NotNull]Graphics g)
        {
            GraphicsFastDrawing(g);
        }
        public static void GraphicsFastResizeBilinear([NotNull]Graphics g)
        {
            g.CompositingQuality = CompositingQuality.HighSpeed;
            g.InterpolationMode = InterpolationMode.Bilinear;
            g.SmoothingMode = SmoothingMode.None;
            g.CompositingMode = CompositingMode.SourceCopy;
        }

        /// <summary>
        /// Creates a new Bitmap image with the specified size using an internal PixelFormat.
        /// </summary>
        /// <param name="width"> Input width </param>
        /// <param name="height"> Input height </param>
        /// <returns> A new Bitmap or an error placeholder </returns>
        [NotNull]
        public static Bitmap Allocate(int width, int height)
        {
            try
            {
                return new Bitmap(width, height, PixelFormat.Format32bppArgb);
            }
            catch (ArgumentException ex)
            {
                Logger.Error("Invalid input arguments [width={0},height={1}]. {2}", width, height, ex);
            }
            catch (OutOfMemoryException ex)
            {
                Logger.Error(ex.ToString());
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
            return ErrorImage;
        }

        /// <summary>
        /// Gets a file filter for the GDI+ supported images.
        /// </summary>
        /// <returns> The filter as string </returns>
        [NotNull]
        public static string GetSupportedImagesFilter()
        {
            return ImagesFilter;
        }

        /// <summary>
        /// Gets the associated ImageFormat for the given file extension.
        /// <returns> The ImageFormat associated or ArgumentException if fileExtension not supported or null. </returns>
        /// </summary>
        /// <param name="fileExtension"> Input file extension </param>
        [NotNull]
        public static ImageFormat GetImageFormatAssociated([CanBeNull]string fileExtension)
        {
            if (!string.IsNullOrEmpty(fileExtension) && MapExtToFormat.ContainsKey(fileExtension))
            {
                return MapExtToFormat[fileExtension];
            }
            throw new ArgumentException();
        }

        [NotNull]
        private static Dictionary<string, ImageFormat> CreateFilterDictionary()
        {
            return new Dictionary<string, ImageFormat>();
        }
        [NotNull]
        private static string CreateFilterMap()
        {
            // Build a file-filter from the available image formats and a map from file extensions to image formats.
            // Uses the method to get all available image formats described here: http: stackoverflow.com/a/9176575
            string filter = string.Empty;
            string sep = string.Empty;
            try
            {
                foreach (var imgCodec in ImageCodecInfo.GetImageEncoders())
                {
                    // format the file types in the file dialog and obtain their extensions
                    int len = imgCodec.FilenameExtension.Replace("*.", "").Length;
                    string ext = imgCodec.FilenameExtension.Substring(1, (len == 3) ? 4 : 5).Replace(";", "");
                    string name = imgCodec.CodecName.Substring(8).Replace("Codec", "Files").Trim();
                    filter = string.Format("{0}{1}{2} ({3})|{3}", filter, sep, name, imgCodec.FilenameExtension);
                    sep = "|";

                    //  map the file types to an image format
                    MapExtToFormat.Add(ext, new ImageFormat(imgCodec.FormatID));
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
            return filter;
        }
        [NotNull]
        private static Bitmap CreateDefaultErrorImage()
        {
            try
            {
                // Initialize an image from the project resources used as placeholder in case of some operation error.
                Bitmap errorImage = Properties.Resources.placeholder;
                return errorImage.Clone(new Rectangle(0, 0, errorImage.Width, errorImage.Height),
                    PixelFormat.Format32bppArgb);
            }
            catch (ArgumentException ex)
            {
                Logger.Warn(ex.ToString());
            }
            catch (OutOfMemoryException ex)
            {
                Logger.Warn(ex.ToString());
            }
            return Properties.Resources.placeholder;
        }
    }
}
