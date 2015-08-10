using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

namespace VectorImageEdit.Modules
{
    static class ImageOutput
    {
        public static void SaveImage(Bitmap image, string filePath, ImageFormat imageType)
        {
            /*
             * Saves the given bitmap to the file specified by filePath,
             * using given format
             */

            try
            {
                // ReSharper disable once PossibleUnintendedReferenceComparison
                if (imageType == ImageFormat.Jpeg)
                {
                    // Set default jpeg quality to 90 (best quality & size)
                    ImageCodecInfo jgpEncoder = GetFormatEncoder(ImageFormat.Jpeg);
                    Encoder myEncoder = Encoder.Quality;
                    EncoderParameters myEncoderParameters = new EncoderParameters(1);
                    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 90L);
                    myEncoderParameters.Param[0] = myEncoderParameter;

                    // Save image as jpeg
                    image.Save(filePath, jgpEncoder, myEncoderParameters);
                }
                else
                {
                    image.Save(filePath, imageType);
                }
            }
            catch (Exception e)
            {
                // TODO: Remove MessageBox here
                // Something unexpected happened
                MessageBox.Show(@"Could not save image to: " + filePath + @"." + Environment.NewLine + e.Message,
                    @"ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static ImageCodecInfo GetFormatEncoder(ImageFormat format)
        {
            // Get the encoder info for a specified ImageFormat
            foreach (var codec in ImageCodecInfo.GetImageDecoders().Where(codec => codec.FormatID == format.Guid))
            {
                return codec;
            }
            throw new ArgumentException("No codec available for the requested ImageFormat");
        }
    }
}
