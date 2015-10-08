using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace VectorImageEdit.Modules.ImportExports
{
    class ImageExporter : AbstractExporter<ImageFormat, Bitmap>
    {
        public ImageExporter(string fileName)
            : base(fileName)
        {
            DataSource = Properties.Resources.placeholder;
            ExportParameter = ImageFormat.Jpeg;
        }

        public override bool ExportData()
        {
            bool status = ExportValidator(() =>
            {
                if (ExportParameter.Guid == ImageFormat.Jpeg.Guid)
                {
                    // For JPEG format, set quality to 90 (seems to have best quality & size balance)
                    Encoder myEncoder = Encoder.Quality;
                    EncoderParameters myEncoderParameters = new EncoderParameters(1);
                    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 90L);
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    
                    ImageCodecInfo jpegCodecInfo = ImageCodecInfo.GetImageDecoders()
                        .First(codec => codec.FormatID == ExportParameter.Guid);

                    DataSource.Save(FileName, jpegCodecInfo, myEncoderParameters);
                }
                else
                {
                    // Other image codecs are handled in the same way
                    DataSource.Save(FileName, ExportParameter);
                }
            });
            return status;
        }
    }
}
