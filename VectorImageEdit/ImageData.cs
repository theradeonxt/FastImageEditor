using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace VectorImageEdit
{
    /*
     * Helper to store the internal data of a bitmap image
     * used when working at the pixel level
     */

    unsafe sealed class ImageData
    {
        public ImageData(Bitmap image)
        {
            imageSrc = image;
            imageData = imageSrc.LockBits(new Rectangle(0, 0, imageSrc.Width, imageSrc.Height), ImageLockMode.ReadWrite, imageSrc.PixelFormat);

            if (imageData.PixelFormat == PixelFormat.Format24bppRgb)
            {
                pixelOffset = 3;
            }
            else if (imageData.PixelFormat == PixelFormat.Format32bppArgb
                || imageData.PixelFormat == PixelFormat.Format32bppRgb)
            {
                pixelOffset = 4;
            }
            else
            {
                throw new Exception("InvalidImageFormat Exception. Supported formats: 24bpp, 32bpp.");
            }

            stride = Math.Abs(imageData.Stride);
            ptrStart = (byte*)(void*)imageData.Scan0;
            ptrEnd = (byte*)(void*)imageData.Scan0 + stride * imageData.Height;
        }

        ~ImageData()
        {
            imageSrc.UnlockBits(imageData);
        }

        public byte* Start
        {
            get { return ptrStart; }
        }

        public byte* End
        {
            get { return ptrEnd; }
        }

        public int Offset
        {
            get { return pixelOffset; }
        }

        public int Stride
        {
            get { return stride; }
        }

        private int pixelOffset;        // Size of a pixel in channels
        private int stride;             // Width of image in bytes
        private byte* ptrStart;         // Address of first pixel in image
        private byte* ptrEnd;           // Address of last pixel in image
        private Bitmap imageSrc;        // Reference to the given image
        private BitmapData imageData;   // Reference to the given image data
    }
}
