using ImageProcessingNET;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using VectorImageEdit.Modules;
using VectorImageEdit.Modules.BasicShapes;
using VectorImageEdit.Modules.ImportExports;
using VectorImageEdit.Modules.Interfaces;
using VectorImageEdit.Modules.LayerManagement;
using VectorImageEdit.Modules.Utility;

namespace VectorImageEdit.Models
{
    class ExternalEventsModel
    {
        public bool TryExportVector(IDiskResourceExporter<int, IList> exporter)
        {
            exporter.DataSource = AppModel.Instance.LayerManager.LayersList;
            return exporter.ExportData();
        }

        public bool TryExportScenePreview(IDiskResourceExporter<ImageFormat, Bitmap> exporter)
        {
            exporter.DataSource = AppModel.Instance.LayerManager.GetImagePreview();
            string extension = Path.GetExtension(exporter.FileName) ?? AppModel.Instance.DefaultImageFileExtension;
            exporter.ExportParameter = ImagingHelpers.GetImageFormatAssociated(extension);
            return exporter.ExportData();
        }

        public void OpenVectorDeserialize(string fileName)
        {
            VectorSerializer serializer = new VectorSerializer();
            var layers = serializer.Deserialize(fileName);
            AppModel.Instance.LayerManager.RemoveAll();
            foreach (Layer layer in layers)
            {
                AppModel.Instance.LayerManager.Add(layer);
            }
        }

        public List<Bitmap> LoadImageFiles(string[] fileNames, Action<int> onProgressChangedCallback)
        {
            var images = new List<Bitmap>();
            int progress = 0;
            Parallel.ForEach(fileNames, fileName =>
            {
                IGenericResourceImporter<Tuple<Size, ScalingMode>, Bitmap> importer = new ImageImporter();
                importer.ImportParameters = new Tuple<Size, ScalingMode>(AppModel.Instance.Layout.MaximumSize(), ScalingMode.CustomSize);
                Bitmap image = importer.Acquire(fileName);
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
                Rectangle region = AppModel.Instance.Layout.NewLayerMetrics(image.Size);
                layers.Add(new Picture(image, region, 0));
            }
            AppModel.Instance.LayerManager.AddRange(layers);
        }
    }
}
