using System;
using System.Drawing;
using System.Drawing.Imaging;
using JetBrains.Annotations;

namespace VectorImageEdit.Modules.Utility
{
    /// <summary>
    /// Helper class to deal with the properties of an image in memory.
    /// Provides low-level memory access to the underlying image, safely,
    /// and methods to query various image parameters.
    /// Note: do not rely on thread-safety. Not handled here.
    /// </summary>
    class BitmapHelper : IDisposable
    {
        /// <summary>
        /// Creates a helper object to access the bitmap data held by the input image. 
        /// 
        /// Notes: 
        ///     - This locks the image data for reading/writing, to unlock the image
        ///         the caller must use a using or Dispose construct.
        ///     - Use this only in the thread that originally created the Bitmap.
        /// 
        /// </summary>
        /// <param name="img"> Input image </param>
        public BitmapHelper([NotNull]Bitmap img)
        {
            BitmapData bmd = img.LockBits(new Rectangle(0, 0, img.Width, img.Height),
                ImageLockMode.ReadWrite,
                img.PixelFormat);

            ImageInfo = bmd;
            ImageData = img;
            ImageParameters(bmd);

            // If the above operations failed, an exception will be handled by the caller;
            // at this point it's safe to assume locking completed.
            Locked = true;
        }

        /// <summary>
        /// Releases the resources used by this instance: the image is unlocked.
        /// </summary>
        public void Dispose()
        {
            if (Locked)
            {
                ImageData.UnlockBits(ImageInfo);
                Locked = false;
            }
        }

        /// <summary>
        /// Returns a pointer to the first pixel in the image.
        /// </summary>
        public unsafe byte* Start { get; private set; }

        /// <summary>
        /// Returns a pointer to the last pixel in the image.
        /// </summary>
        public unsafe byte* Last { get; private set; }

        /// <summary>
        /// Returns the size of the image pixel (bytes taken).
        /// </summary>
        public uint PixelSize { get; private set; }

        /// <summary>
        /// Returns the (padded) size of an entire row of image pixels (bytes).
        /// </summary>
        public uint Stride { get; private set; }

        /// <summary>
        /// Returns the size of memory used to hold the image.
        /// </summary>
        public uint SizeBytes { get; private set; }

        /// <summary>
        /// Gets the image width in pixels.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Gets the image height in pixels.
        /// </summary>
        public int Height { get; private set; }

        #region Private Members

        [NotNull]
        private BitmapData ImageInfo { get; set; }

        [NotNull]
        private Bitmap ImageData { get; set; }

        private bool Locked { get; set; }

        private void ImageParameters([NotNull]BitmapData bmd)
        {
            switch (bmd.PixelFormat)
            {
                case PixelFormat.Format24bppRgb:
                    PixelSize = 3;
                    break;
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppRgb:
                    PixelSize = 4;
                    break;
                default:
                    throw new ArgumentException("UnsupportedImageFormat Exception");
            }
            if (bmd.Stride < 0) // FIXME: Handle negative strides too
            {
                throw new ArgumentException("UnsupportedImageStride Exception");
            }
            unsafe
            {
                Stride = (uint)Math.Abs(bmd.Stride);
                SizeBytes = (uint)(Stride * bmd.Height);
                Start = (byte*)(void*)bmd.Scan0;

                if (SizeBytes == 1) // special case of 1 pixel
                {
                    Last = Start;
                }
                else // general pixel count
                {
                    Last = Start + SizeBytes - 1;
                }

                Width = ImageData.Width;
                Height = ImageData.Height;
            }
        }

        #endregion
    }
}
