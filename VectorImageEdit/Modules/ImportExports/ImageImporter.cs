using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace VectorImageEdit.Modules.ImportExports
{
    enum ScalingMode { FullSize, CustomSize }

    class ImageImporter : GenericResourceImporter<Tuple<Size, ScalingMode>, Bitmap>
    {
        public override Bitmap Acquire(string resourcePath)
        {
            Bitmap resultImage = null;
            bool status = InternalErrorValidation(() =>
            {
                switch (ImportParameters.Item2)
                {
                    case ScalingMode.FullSize:
                        resultImage = FullSize(resourcePath);
                        break;
                    case ScalingMode.CustomSize:
                        resultImage = ScaledSize(resourcePath, ImportParameters.Item1);
                        break;
                }
            });
            return resultImage;
        }

        public override Bitmap Acquire(string[] resourcePath)
        {
            throw new NotImplementedException();
        }

        public override bool Validate(string resourcePath)
        {
            throw new NotImplementedException();
        }

        public override bool Validate(string[] resourcePath)
        {
            throw new NotImplementedException();
        }

        // TODO: Fix Overscaling bigger than actual window bounds
        // TODO: Implement cache for downscaled images
        private static Bitmap ScaledSize(string fileName, Size maximumSize)
        {
            Bitmap original = OpenImage(fileName);

            // Resize image if its size is greater than the requested size
            if (original.Size.Width > maximumSize.Width || original.Size.Height > maximumSize.Height)
            {
                // Ensure the same aspect ratio as source image
                float aspectRatio = (float)original.Width / original.Height;
                float heightf = maximumSize.Width / aspectRatio;

                Bitmap scaled = new Bitmap(maximumSize.Width, (int)heightf, PixelFormat.Format32bppArgb);
                using (Graphics g = Graphics.FromImage(scaled))
                {
                    g.DrawImage(original, 0, 0, scaled.Width, scaled.Height);
                }
                original.Dispose();

                return scaled;
            }

            return original;
        }
        private static Bitmap FullSize(string fileName)
        {
            return OpenImage(fileName);
        }
        private static Bitmap OpenImage(string fileName)
        {
            Bitmap loadedBitmap = (Bitmap) Image.FromFile(fileName);
            return loadedBitmap.Clone(new Rectangle(0, 0, loadedBitmap.Width, loadedBitmap.Height), PixelFormat.Format32bppArgb);
        }
    }
}
