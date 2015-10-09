using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using NLog;
using VectorImageEdit.Modules.Layers;
using VectorImageEdit.Modules.Utility;

namespace VectorImageEdit.Modules
{
    class RasterizerStage : IDisposable
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        // Raster-info structures for vector objects and raw image objects
        private readonly IDictionary<int, Tuple<Bitmap, BitmapData>> _vectorRasterInfo;
        private readonly IDictionary<int, Tuple<Bitmap, BitmapData>> _rawImageRasterInfo;

        public RasterizerStage(SortedContainer<Layer> objectCollection)
        {
            _vectorRasterInfo = new Dictionary<int, Tuple<Bitmap, BitmapData>>();
            _rawImageRasterInfo = new Dictionary<int, Tuple<Bitmap, BitmapData>>();

            RasterizeObjects(objectCollection);
        }

        /// <summary>
        /// Disposes the temporary buffers for the vector objects and 
        /// unlocks the data (Bitmap) for all objects
        /// </summary>
        public void Dispose()
        {
            try
            {
                foreach (var item in _vectorRasterInfo)
                {
                    item.Value.Item1.UnlockBits(item.Value.Item2);
                    item.Value.Item1.Dispose();
                }
                _vectorRasterInfo.Clear();
                foreach (var item in _rawImageRasterInfo)
                {
                    item.Value.Item1.UnlockBits(item.Value.Item2);
                }
                _rawImageRasterInfo.Clear();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
        }

        /// <summary>
        /// Gets the raster information structure for the layer object with the given ID
        /// </summary>
        /// <param name="layerId"> The ID of object </param>
        /// <returns> </returns>
        public BitmapData GetRasterInfo(int layerId)
        {
            if (_vectorRasterInfo.ContainsKey(layerId))
            {
                return _vectorRasterInfo[layerId].Item2;
            }
            if (_rawImageRasterInfo.ContainsKey(layerId))
            {
                return _rawImageRasterInfo[layerId].Item2;
            }
            throw new KeyNotFoundException();
        }

        private void RasterizerHandlePictureObject(Picture pictureObj)
        {
            Tuple<Bitmap, BitmapData> tuple = new Tuple<Bitmap, BitmapData>(pictureObj.Image,
                pictureObj.Image.LockBits(
                new Rectangle(0, 0, pictureObj.Image.Width, pictureObj.Image.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb));

            _rawImageRasterInfo.Add(pictureObj.Metadata.Uid, tuple);
        }
        private void RasterizerHandleVectorObject(Layer layerObj)
        {
            var rasterized = ImagingHelpers.Allocate(layerObj.Region.Width, layerObj.Region.Height);
            using (var gfx = Graphics.FromImage(rasterized))
            {
                // rasterize using the object's draw method
                layerObj.DrawGraphics(gfx);
            }

            Tuple<Bitmap, BitmapData> tuple = new Tuple<Bitmap, BitmapData>(rasterized, 
                rasterized.LockBits(
                new Rectangle(0, 0, layerObj.Region.Width, layerObj.Region.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb));

            _vectorRasterInfo.Add(layerObj.Metadata.Uid, tuple);
        }
        private void RasterizeObjects(SortedContainer<Layer> objectCollection)
        {
            try
            {
                // Build rasterization information for every layer inside the scene data
                foreach (Layer layer in objectCollection)
                {
                    if (layer is Picture)
                    {
                        RasterizerHandlePictureObject((Picture)layer);
                    }
                    else
                    {
                        // Only a Vector objec needs rasterizing
                        RasterizerHandleVectorObject(layer);
                    }
                }
            }
            catch (OutOfMemoryException ex)
            {
                _logger.Error(ex.ToString());
            }
            catch (ArgumentException ex)
            {
                _logger.Error(ex.ToString());
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
        }
    }
}
