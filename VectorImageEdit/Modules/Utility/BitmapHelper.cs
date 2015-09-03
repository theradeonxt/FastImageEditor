using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace VectorImageEdit.Modules.Utility
{
    class BitmapHelper : IDisposable
    {
        /// <summary>
        /// Creates a helper object to access the bitmap data held by the input image. 
        /// NOTE: this locks the image data for reading/writing,
        /// the caller must release this resource to unlock the image data.
        /// </summary>
        /// <param name="img"> Input image </param>
        public BitmapHelper(Bitmap img)
        {
            var bmd = img.LockBits(new Rectangle(0, 0, img.Width, img.Height),
                ImageLockMode.ReadWrite,
                img.PixelFormat);
            Locked = true;
            ImageParameters(bmd, img);
        }

        public void Dispose()
        {
            if (Locked)
            {
                SourceImage.UnlockBits(SourceData);
            }
        }

        /// <summary>
        /// Return a pointer to the first pixel of the image.
        /// </summary>
        public unsafe byte* Start { get; private set; }

        /// <summary>
        /// Return a pointer to the last pixel of the image.
        /// </summary>
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public unsafe byte* End { get; private set; }

        /// <summary>
        /// Return the size of the image pixel.
        /// </summary>
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public uint PixelSize { get; private set; }

        /// <summary>
        /// Return the (padded) size of an entire row of pixels.
        /// </summary>
        public uint Stride { get; private set; }

        /// <summary>
        /// Return the size of memory used to hold the image.
        /// </summary>
        public uint SizeBytes { get; private set; }

        private BitmapData SourceData { get; set; }
        private Bitmap SourceImage { get; set; }
        private bool Locked { get; set; }

        private void ImageParameters(BitmapData bmd, Bitmap img)
        {
            ImageParameters(bmd);
            SourceData = bmd;
            SourceImage = img;
        }
        private void ImageParameters(BitmapData bmd)
        {
            switch (bmd.PixelFormat)
            {
                case PixelFormat.Format24bppRgb:
                    PixelSize = 3;
                    break;
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
            }
        }
    }
}
