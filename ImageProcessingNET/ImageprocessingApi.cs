using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageProcessingNET
{
    public static class ImageProcessingApi
    {
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
                    ImageProcessingWrapper.Blend24bgr_24bgr(ibSrc1.Start, ibSrc2.Start, ibDest.Start, ibDest.SizeBytes, step);
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
                    ImageProcessingWrapper.Blend24bgr_24bgr(currentLineSrc1, currentLineSrc2, currentLineResult, stride, step);
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
                    ImageProcessingWrapper.OpacityAdjust_32bgra(ibSrc1.Start, ibDest.Start, ibDest.SizeBytes, step);
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
                    ImageProcessingWrapper.AlphaBlend32bgra_32bgra(ibSrc.Start, ibTar.Start, ibDest.Start, ibDest.SizeBytes);
                }
            }

            sw.Stop();
            LastOperationDuration = sw.ElapsedMilliseconds;
        }

        public static void ConvolutionFilter(Bitmap srcImage, Bitmap dstImage, float[] kernel)
        {
            StaticCheckFormat(PixelFormat.Format32bppArgb, srcImage, dstImage);
            StaticCheckSameSize(srcImage, dstImage);

            using (var ibSrc = new BitmapHelper(srcImage))
            using (var ibDest = new BitmapHelper(dstImage))
            {
                var stride = ibSrc.Stride;
                var size = ibSrc.SizeBytes;
                var kw = (uint)kernel.GetLength(0);
                var kh = (uint)kernel.Length / kw;
                unsafe
                {
                    ImageProcessingWrapper.ConvFilter_32bgra_ref(ibSrc.Start, ibDest.Start, size, stride, kernel, kw, kh);
                }
            }
        }

        // TODO: static member is bad for thread safety
        public static long LastOperationDuration
        {
            get;
            private set;
        }

        private static void StaticCheckFormat(PixelFormat pixFmt, params Bitmap[] args)
        {
            foreach (var img in args)
            {
                Debug.Assert(img.PixelFormat == pixFmt);
            }
        }

        private static void StaticCheckSameSize(params Bitmap[] args)
        {
            Size oldSz = Size.Empty, newSz = Size.Empty;
            foreach (var img in args)
            {
                newSz = img.Size;
                if (!newSz.IsEmpty && !oldSz.IsEmpty)
                    Debug.Assert(newSz == oldSz);
                oldSz = newSz;
            }
        }
    }
}
