using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.IO;
using VectorImageEdit.Properties;

namespace VectorImageEdit.Modules.ImportExports
{
    /// <summary>
    /// 
    /// Image Loader Module
    ///
    /// - loads images from disk
    /// - specifies how to load images
    /// 
    /// </summary>
    public static class ImageLoader
    {
        // Image cache to find the path to full image if required
        private static readonly ConcurrentDictionary<string, Bitmap> ImageCache;

        static ImageLoader()
        {
            ImageCache = new ConcurrentDictionary<string, Bitmap>();
        }

        // TODO: Fix Overscaling bigger than actual window bounds
        public static Bitmap ScaledSize(string fileName, Size maximumSize)
        {
            Bitmap original = OpenImage(fileName);

            // Resize image if its size is greater than the requested size
            if (original.Size.Width > maximumSize.Width || original.Size.Height > maximumSize.Height)
            {
                // Ensure the same aspect ratio as source image
                float aspectRatio = (float)original.Width / original.Height;
                float heightf = maximumSize.Width / aspectRatio;

                Bitmap scaled = new Bitmap(maximumSize.Width, (int)heightf);
                using (Graphics g = Graphics.FromImage(scaled))
                {
                    g.DrawImage(original, 0, 0, scaled.Width, scaled.Height);
                }
                original.Dispose();

                // Keep info that this is only a preview(scaled version)
                // of the original image
                if (ImageCache.ContainsKey(fileName) == false)
                {
                    ImageCache.TryAdd(fileName, scaled);
                }

                return scaled;
            }

            return original;
        }

        public static Bitmap FullSize(string fileName)
        {
            return OpenImage(fileName);
        }

        public static Bitmap GetFullImage(string keyId)
        {
            // Obtain the full-sized image of 
            // the cached bitmap with ID = keyID
            Bitmap result = Resources.placeholder;
            if (keyId != null)
            {
                ImageCache.TryGetValue(keyId, out result);
            }
            return result;
        }

        private static Bitmap OpenImage(string fileName)
        {
            Bitmap image = Resources.placeholder;
            try
            {
                image = (Bitmap)Image.FromFile(fileName);
            }
            catch (OutOfMemoryException) { }
            catch (FileNotFoundException) { }
            catch (ArgumentException) { }
            return image;
        }
    }
}
