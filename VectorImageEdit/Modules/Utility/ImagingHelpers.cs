using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using JetBrains.Annotations;

namespace VectorImageEdit.Modules.Utility
{
    static class ImagingHelpers
    {
        private static readonly string ImagesFilter;
        private static readonly Dictionary<string, ImageFormat> MapExtToFormat;

        static ImagingHelpers()
        {
            MapExtToFormat = new Dictionary<string, ImageFormat>();

            // Builds a file-filter from the available image formats and a map from file extensions to image formats.
            // Uses the method to get all available image formats described here: http://stackoverflow.com/a/9176575
            ImagesFilter = "";
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

                    // map the file types to an image format
                    MapExtToFormat.Add(ext, new ImageFormat(imgCodec.FormatID));
                }
            }
            catch (ArgumentException) { }
            catch (FormatException) { }
            ImagesFilter = filter;
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
        /// Creates a new Bitmap object with the specified size using an internal PixelFormat
        /// </summary>
        /// <param name="width"> Input width </param>
        /// <param name="height"> Input height </param>
        /// <returns> Output Bitmap </returns>
        [CanBeNull]
        public static Bitmap Allocate(int width, int height)
        {
            try
            {
                return new Bitmap(width, height, PixelFormat.Format32bppArgb);
            }
            catch (ArgumentException)
            {
                Bitmap errorImage = Properties.Resources.placeholder;
                return errorImage.Clone(new Rectangle(0, 0, errorImage.Width, errorImage.Height),
                    PixelFormat.Format32bppArgb);
            }
        }

        /// <summary>
        /// Gets a file filter for the GDI+ supported images
        /// </summary>
        /// <returns> The filter as string </returns>
        public static string GetSupportedImagesFilter()
        {
            return ImagesFilter;
        }

        /// <summary>
        /// Gets the associated ImageFormat for the given file extension
        /// </summary>
        /// <param name="fileExtension"> Input file extension </param>
        /// <returns> The ImageFormat associated </returns>
        [CanBeNull]
        public static ImageFormat GetImageFormat(string fileExtension)
        {
            if (fileExtension != null && MapExtToFormat.ContainsKey(fileExtension))
            {
                return MapExtToFormat[fileExtension];
            }
            throw new ArgumentException();
        }
    }
}
