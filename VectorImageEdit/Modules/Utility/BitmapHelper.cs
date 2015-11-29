using System;
using System.Drawing;
using System.Drawing.Imaging;
using JetBrains.Annotations;

namespace VectorImageEdit.Modules.Utility
{
    class BitmapHelper : IDisposable
    {
        /// <summary>
        /// Creates a helper object to access the bitmap data held by the input image. 
        /// 
        /// Note1: This locks the image data for reading/writing,
        ///        the caller must release this resource to unlock the image data.
        /// Note2: Warning! Use this only in a thread that created the Bitmap.
        /// 
        /// </summary>
        ///// <param name="img"> Input image </param>
        public BitmapHelper([NotNull]Bitmap img)
        {
            BitmapData bmd = img.LockBits(new Rectangle(0, 0, img.Width, img.Height),
                ImageLockMode.ReadWrite,
                img.PixelFormat);

            SourceData = bmd;
            SourceImage = img;
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
                SourceImage.UnlockBits(SourceData);
            }
        }

        /// <summary>
        /// Returns a pointer to the first pixel in the image.
        /// </summary>
        public unsafe byte* Start { get; private set; }

        /// <summary>
        /// Returns a pointer to the last pixel in the image.
        /// </summary>
        public unsafe byte* End { get; private set; }

        /// <summary>
        /// Returns the size of the image pixel.
        /// </summary>
        public uint PixelSize { get; private set; }

        /// <summary>
        /// Returns the (padded) size of an entire row of image pixels.
        /// </summary>
        public uint Stride { get; private set; }

        /// <summary>
        /// Returns the size of memory used to hold the image.
        /// </summary>
        public uint SizeBytes { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        [NotNull]
        private BitmapData SourceData { get; set; }
        [NotNull]
        private Bitmap SourceImage { get; set; }
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
            if (bmd.Stride < 0)
            {
                throw new ArgumentException("UnsupportedImageStride Exception");
            }
            unsafe
            {
                Stride = (uint)Math.Abs(bmd.Stride);
                SizeBytes = (uint)(Stride * bmd.Height);
                Start = (byte*)(void*)bmd.Scan0;
                End = Start + SizeBytes - 1;

                Width = SourceImage.Width;
                Height = SourceImage.Height;
            }
        }
    }
}
