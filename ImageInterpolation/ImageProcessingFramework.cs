using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ImageInterpolation
{
    static class ImageProcessingFramework
    {
        [DllImport("ImageProcessing.dll", CallingConvention = CallingConvention.Cdecl)]
        private static unsafe extern int Blend24bgr_24bgr(
            byte* source,
            byte* target,
            byte* destination,
            uint sizeBytes,
            float percentage
        );
        [DllImport("ImageProcessing.dll", CallingConvention = CallingConvention.Cdecl)]
        private static unsafe extern int OpacityAdjust_32bgra(
            byte* source,
            byte* destination,
            uint sizeBytes,
            float percentage
        );
        [DllImport("ImageProcessing.dll", CallingConvention = CallingConvention.Cdecl)]
        private static unsafe extern int AlphaBlend32bgra_32bgra(
            byte* source,
            byte* target,
            byte* destination,
            uint sizeBytes
        );

        public static Bitmap ImageInterpolate(Bitmap srcImage1, Bitmap srcImage2, float step)
        {
            var destHeight = Math.Min(srcImage1.Height, srcImage2.Height);
            var destWidth = Math.Min(srcImage1.Width, srcImage2.Width);

            // TODO: maybe don't create Bitmap every time
            var resultImage = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);

            // ensure same-sized inputs
            // srcImage1 = Resize(srcImage1, resultImage.Size);
            // srcImage2 = Resize(srcImage2, resultImage.Size);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Less work for caller: let C++ handle threading
            using (var ibSrc1 = new BitmapHelper(srcImage1))
            using (var ibSrc2 = new BitmapHelper(srcImage2))
            using (var ibDest = new BitmapHelper(resultImage))
            {
                unsafe
                {
                    Blend24bgr_24bgr(ibSrc1.Start, ibSrc2.Start, ibDest.Start, ibDest.SizeBytes, step);
                }
            }

            // More work for caller: let C# run more threads
            // TODO: How to mark here that C++ should not do MT in this case?
            /*unsafe
            {
                BitmapData resultData = resultImage.LockBits(
                    new Rectangle(0, 0, resultImage.Width, resultImage.Height), ImageLockMode.ReadWrite,
                    resultImage.PixelFormat);
                BitmapData src1Data = srcImage1.LockBits(
                    new Rectangle(0, 0, srcImage1.Width, srcImage1.Height), ImageLockMode.ReadOnly,
                    srcImage1.PixelFormat);
                BitmapData src2Data = srcImage2.LockBits(
                    new Rectangle(0, 0, srcImage2.Width, srcImage2.Height), ImageLockMode.ReadOnly,
                    srcImage2.PixelFormat);

                int heightInPixels = resultData.Height;
                uint stride = (uint)Math.Abs(resultData.Stride);

                Parallel.For(0, heightInPixels, y =>
                {
                    byte* currentLineSrc1 = (byte*)src1Data.Scan0 + (y * stride);
                    byte* currentLineSrc2 = (byte*)src2Data.Scan0 + (y * stride);
                    byte* currentLineResult = (byte*)resultData.Scan0 + (y * stride);
                    Blend24bgr_24bgr(currentLineSrc1, currentLineSrc2, currentLineResult, stride, step);
                });

                resultImage.UnlockBits(resultData);
                srcImage1.UnlockBits(src1Data);
                srcImage2.UnlockBits(src2Data);
            }*/

            sw.Stop();
            LastOperationDuration = sw.ElapsedMilliseconds;

            return resultImage;
        }

        public static Bitmap ImageOpacity(Bitmap srcImage, float step)
        {
            // TODO: maybe don't create Bitmap every time
            var resultImage = new Bitmap(srcImage.Width, srcImage.Height, srcImage.PixelFormat);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Less work for caller: let C++ handle threading
            using (var ibSrc1 = new BitmapHelper(srcImage))
            using (var ibDest = new BitmapHelper(resultImage))
            {
                unsafe
                {
                    OpacityAdjust_32bgra(ibSrc1.Start, ibDest.Start, ibDest.SizeBytes, step);
                }
            }

            sw.Stop();
            LastOperationDuration = sw.ElapsedMilliseconds;

            return resultImage;
        }

        public static void ImageAlphaBlend(Bitmap srcImage, Bitmap tarImage, Bitmap refImage)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Less work for caller: let C++ handle threading
            using (var ibSrc = new BitmapHelper(srcImage))
            using (var ibTar = new BitmapHelper(tarImage))
            using (var ibDest = new BitmapHelper(refImage))
            {
                unsafe
                {
                    AlphaBlend32bgra_32bgra(ibSrc.Start, ibTar.Start, ibDest.Start, ibDest.SizeBytes);
                }
            }

            sw.Stop();
            LastOperationDuration = sw.ElapsedMilliseconds;
        }

        // TODO: static member is bad for thread safety
        public static long LastOperationDuration
        {
            get; private set;
        }
    }
}
