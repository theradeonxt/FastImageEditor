using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using VectorImageEdit.Modules;
using VectorImageEdit.Modules.ExportFormats;
using VectorImageEdit.Modules.Layers;

namespace VectorImageEdit.Models
{
    class ExternalEventsModel
    {
        //********************************************************
        #region Vector Serialize/Deserialize

        public void SaveVectorSerialize(string fileName)
        {
            VectorSerializer serializer = new VectorSerializer
            {
                Source = GlobalModel.Instance.LayerManager.GetLayers()
            };
            serializer.Serialize(fileName);
        }

        public void OpenVectorDeserialize(string fileName)
        {
            VectorSerializer serializer = new VectorSerializer();
            IList layers = serializer.Deserialize(fileName);
            GlobalModel.Instance.LayerManager.RemoveAll();
            foreach (Layer layer in layers)
            {
                GlobalModel.Instance.LayerManager.Add(layer);
            }
        }

        public static string SupportedVectorExt { get { return VectorSerializer.SupportedFileExt; } }

        #endregion
        //********************************************************

        //********************************************************
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

            return filter;
        }

        public bool ValidateExportFile(string fileName)
        {
            string fileExtension = null;
            try
            {
                fileExtension = Path.GetExtension(fileName);
            }
            catch (ArgumentException) { }

            return !string.IsNullOrEmpty(fileExtension) &&
                   _extCodec.ContainsKey(fileExtension);
        }

        /// <summary>
        /// Saves an image containing the current workspace graphical content to a format chosen by the user.
        /// </summary>
        /// <param name="fileName"></param>
        public void ExportToFile(string fileName)
        {
            // get the workspace data as an image
            using (var preview = GlobalModel.Instance.LayerManager.GetImagePreview())
            {
                try
                {
                    string ext = Path.GetExtension(fileName);
                    if (ext == null) return;

                    // save the obtained image to the desired file with image format based on the extension
                    ImageOutput.SaveImage(preview, fileName, _extCodec[ext]);
                }
                catch (ArgumentException) { }
            }
        }

        public delegate void ProgressChangedCallback(int progress);

        public IList LoadImageFiles(string[] fileNames, ProgressChangedCallback callback)
        {
            IList images = new List<Bitmap>();
            int progress = 0;
            Parallel.ForEach(fileNames, fileName =>
            {
                Bitmap image = ImageLoader.ScaledSize(fileName, GlobalModel.Instance.Layout.MaximumSize());
                images.Add(image);
                callback(Interlocked.Increment(ref progress));
            });
            return images;
        }

        public void LoadImageLayers(IList imageList)
        {
            List<Layer> layers = new List<Layer>();
            foreach (Bitmap image in imageList)
            {
                using (var helper = new BitmapHelper(image))
                {
                    BackgroundStatitics.CommitImageMemory(helper.SizeBytes);
                }

                Rectangle region = GlobalModel.Instance.Layout.NewLayerMetrics(image.Size);
                layers.Add(new Picture(image, region, 0));
            }

            GlobalModel.Instance.LayerManager.Add(layers);
        }

        #endregion
        //********************************************************
    }
}
