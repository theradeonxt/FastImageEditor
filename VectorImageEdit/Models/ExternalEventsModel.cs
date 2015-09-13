using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using VectorImageEdit.Modules;
using VectorImageEdit.Modules.ImportExports;
using VectorImageEdit.Modules.Layers;
using VectorImageEdit.Modules.Utility;

namespace VectorImageEdit.Models
{
    class ExternalEventsModel
    {
        #region Vector Serialize/Deserialize

        public void SaveVectorSerialize(string fileName)
        {
            VectorSerializer serializer = new VectorSerializer
            {
                Source = AppGlobalData.Instance.LayerManager.GetLayers()
            };
            serializer.Serialize(fileName);
        }

        public void OpenVectorDeserialize(string fileName)
        {
            VectorSerializer serializer = new VectorSerializer();
            var layers = serializer.Deserialize(fileName);
            AppGlobalData.Instance.LayerManager.RemoveAll();
            foreach (Layer layer in layers)
            {
                AppGlobalData.Instance.LayerManager.Add(layer);
            }
        }

        #endregion

        #region Open/Export File(s)

        private readonly Dictionary<string, ImageFormat> _extCodec =
            new Dictionary<string, ImageFormat>();

        /// <summary>
        /// Builds a file-filter from the available image formats and a map from file extensions to image formats.
        /// Uses the method to get all available image formats described here: http://stackoverflow.com/a/9176575
        /// </summary>
        public string GetExportsFilter()
        {
            string filter = string.Empty;
            string sep = string.Empty;

            try
            {
                foreach (var imgCodec in ImageCodecInfo.GetImageEncoders())
                {
                    // format the file types in the file dialog and obtain their extensions
                    int len = imgCodec.FilenameExtension.Replace("*.", "").Length;
                    string ext = imgCodec.FilenameExtension.Substring(1, (len == 3) ? 4 : 5).Replace(";", "");
                    string name = imgCodec.CodecName.Substring(8).Replace("Codec", "Files").Trim();
                    filter = string.Format("{0}{1}{2} ({3})|{3}", filter, sep, name, imgCodec.FilenameExtension);
                    sep = "|";

                    // map the file types to an image format (this is done only once)
                    if (_extCodec.Count != 0) _extCodec.Add(ext, new ImageFormat(imgCodec.FormatID));
                }
            }
            catch (ArgumentException) { }
            catch (FormatException) { }

            return filter;
        }

        private bool ValidateExportFile(string fileName)
        {
            try
            {
                string fileExtension = Path.GetExtension(fileName);
                return !string.IsNullOrEmpty(fileExtension) &&
                    _extCodec.ContainsKey(fileExtension);
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        /// <summary>
        /// Saves an image containing the current workspace graphical content to a format chosen by the user.
        /// </summary>
        /// <param name="fileName"> Output File </param>
        public bool ExportToFile(string fileName)
        {
            if (!ValidateExportFile(fileName)) return false;

            // Get the workspace data as an image
            using (var preview = AppGlobalData.Instance.LayerManager.GetImagePreview())
            {
                string ext = Path.GetExtension(fileName) ?? "jpg";

                // Save the image to the desired file with image format based on the extension
                ImageOutput.SaveImage(preview, fileName, _extCodec[ext]);

                return true;
            }
        }

        public List<Bitmap> LoadImageFiles(string[] fileNames, Action<int> onProgressChangedCallback)
        {
            var images = new List<Bitmap>();
            int progress = 0;
            Parallel.ForEach(fileNames, fileName =>
            {
                Bitmap image = ImageLoader.ScaledSize(fileName, AppGlobalData.Instance.Layout.MaximumSize());
                images.Add(image);
                onProgressChangedCallback(Interlocked.Increment(ref progress));
            });
            return images;
        }

        public void LoadImageLayers(List<Bitmap> imageList)
        {
            var layers = new List<Layer>();
            foreach (Bitmap image in imageList)
            {
                using (var helper = new BitmapHelper(image))
                {
                    BackgroundStatitics.CommitImageMemory(helper.SizeBytes);
                }
                Rectangle region = AppGlobalData.Instance.Layout.NewLayerMetrics(image.Size);
                layers.Add(new Picture(image, region, 0));
            }
            AppGlobalData.Instance.LayerManager.Add(layers);
        }

        #endregion
    }
}
