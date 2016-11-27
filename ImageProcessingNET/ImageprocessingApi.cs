using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageProcessingNET
{
    public static class ImageProcessingApi
    {
        public static void ImageInterpolate(Bitmap srcImg, Bitmap tarImg, Bitmap dstImg, float step)
        {
            StaticCheckFormat(PixelFormat.Format24bppRgb, srcImg, tarImg, dstImg);
            StaticCheckSameSize(srcImg, tarImg, dstImg);

            // Less work for caller: let C++ handle threading
            using (var ibSrc = new BitmapHelper(srcImg))
            using (var ibTar = new BitmapHelper(tarImg))
            using (var ibDest = new BitmapHelper(dstImg))
            {
                unsafe
                {
                    ImageProcessingWrapper.Blend24bgr_24bgr(ibSrc.Start, ibTar.Start, ibDest.Start, ibDest.SizeBytes, step);
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
        }

        public static void ImageOpacity(Bitmap srcImg, Bitmap dstImg, float step)
        {
            // Less work for caller: let C++ handle threading
            using (var ibSrc = new BitmapHelper(srcImg))
            using (var ibDest = new BitmapHelper(dstImg))
            {
                unsafe
                {
                    ImageProcessingWrapper.OpacityAdjust_32bgra(ibSrc.Start, ibDest.Start, ibDest.SizeBytes, step);
                }
            }
        }

        public static void ImageAlphaBlend(Bitmap srcImg, Bitmap tarImg, Bitmap dstImg)
        {
            // Less work for caller: let C++ handle threading
            using (var ibSrc = new BitmapHelper(srcImg))
            using (var ibTar = new BitmapHelper(tarImg))
            using (var ibDest = new BitmapHelper(dstImg))
            {
                unsafe
                {
                    ImageProcessingWrapper.AlphaBlend32bgra_32bgra(ibSrc.Start, ibTar.Start, ibDest.Start, ibDest.SizeBytes);
                }
            }
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
                var kw = (uint)Math.Sqrt(kernel.GetLength(0));
                var kh = (uint)kernel.Length / kw;
                unsafe
                {
                    ImageProcessingWrapper.ConvFilter_32bgra(ibSrc.Start, ibDest.Start, size, stride, kernel, kw, kh);
                }
            }
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
