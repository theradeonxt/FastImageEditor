using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace VectorImageEdit.Modules
{
    static class ImageOutput
    {
        /// <summary>
        /// Saves the given bitmap to the file specified by filePath,
        /// using specified ImageFormat
        /// </summary>
        /// <param name="image"> Input bitmap </param>
        /// <param name="filePath"> Output file </param>
        /// <param name="format"> Image format to use </param>
        public static void SaveImage(Bitmap image, string filePath, ImageFormat format)
        {
            try
            {
                // ReSharper disable once PossibleUnintendedReferenceComparison
                if (format == ImageFormat.Jpeg)
                {
                    // Set default jpeg quality to 90 (seems to have best quality & size properties)
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
                    image.Save(filePath, format);
                }
            }
            catch (Exception ex)
            {
                // Something unexpected happened
                throw new Exception(string.Format(@"Could not save image to: {0}.{1}", filePath, ex.Message));
            }
        }

        private static ImageCodecInfo GetFormatEncoder(ImageFormat format)
        {
            try
            {
                // Get the encoder info for specified ImageFormat
                return ImageCodecInfo.GetImageDecoders()
                    .First(codec => codec.FormatID == format.Guid);
            }
            catch (Exception)
            {
                throw new ArgumentException("No codec available for the requested ImageFormat");
            }
        }
    }
}
