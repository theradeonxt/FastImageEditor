using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using JetBrains.Annotations;
using NLog;
using VectorImageEdit.Modules.LayerManagement;
using VectorImageEdit.Modules.Utility;

namespace VectorImageEdit.Modules.GraphicsCompositing
{
    class RasterizerStage : IDisposable
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        // Raster-info structures for vector objects and raw image objects
        private readonly Dictionary<int, Tuple<Bitmap, BitmapData>> _vectorRasterInfo;
        private readonly Dictionary<int, Tuple<Bitmap, BitmapData>> _pictureRasterInfo;

        public RasterizerStage([NotNull]SortedContainer<Layer> objectCollection)
        {
            _vectorRasterInfo = new Dictionary<int, Tuple<Bitmap, BitmapData>>();
            _pictureRasterInfo = new Dictionary<int, Tuple<Bitmap, BitmapData>>();

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
                foreach (var item in _pictureRasterInfo)
                {
                    item.Value.Item1.UnlockBits(item.Value.Item2);
                }
                _pictureRasterInfo.Clear();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }

        /// <summary>
        /// Gets the raw image data for the layer object with the given ID
        /// </summary>
        /// <param name="layerId"> The ID of object </param>
        /// <returns> A BitmapData with raw image data </returns>
        [NotNull]
        public BitmapData GetRasterInfo(int layerId)
        {
            if (_vectorRasterInfo.ContainsKey(layerId))
            {
                return _vectorRasterInfo[layerId].Item2;
            }
            if (_pictureRasterInfo.ContainsKey(layerId))
            {
                return _pictureRasterInfo[layerId].Item2;
            }
            throw new KeyNotFoundException();
        }

        [NotNull]
        private List<Tuple<Bitmap, int>> ParallelVectorRasterizer([NotNull]IEnumerable<Layer> layerCollection)
        {
            // use thread-safe container for results
            var collection = new ConcurrentBag<Tuple<Bitmap, int>>();

            // process every shape in parallel and generate their rasterizations
            layerCollection.AsParallel()
                .Where(layer => !(layer is Picture))
                .ForAll((layer =>
                {
                    Bitmap rasterized = ImagingHelpers.Allocate(layer.Region.Width, layer.Region.Height);
                    using (var gfx = Graphics.FromImage(rasterized))
                    {
                        ImagingHelpers.GraphicsFastDrawingWithBlending(gfx);
                        gfx.Clear(Color.FromArgb(0, 0, 0, 0));
                        // rasterize using the object's draw method
                        layer.DrawGraphics(gfx);
                    }
                    collection.Add(new Tuple<Bitmap, int>(rasterized, layer.Metadata.Uid));
                }));

            return collection.ToList();
        }
        private void CacheRasterizedObject([NotNull]Bitmap raster, int id, bool isVector = false)
        {
            Tuple<Bitmap, BitmapData> tuple = new Tuple<Bitmap, BitmapData>(raster,
                raster.LockBits(
                new Rectangle(0, 0, raster.Width, raster.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb));
            /* 
             * Warning! Do not use members of the Bitmap object for layer[id]
             * from this point on until the class Dispose method is called. 
             */
            if (isVector) _vectorRasterInfo.Add(id, tuple);
            else _pictureRasterInfo.Add(id, tuple);
        }
        private void RasterizeObjects([NotNull]SortedContainer<Layer> objectCollection)
        {
            try
            {
                // Build rasterization information for every layer inside the scene data
                foreach (Tuple<Bitmap, int> rastInfo in ParallelVectorRasterizer(objectCollection))
                {
                    CacheRasterizedObject(rastInfo.Item1, rastInfo.Item2, true);
                }
                foreach (Layer layer in objectCollection.Where(layer => layer is Picture))
                {
                    CacheRasterizedObject(((Picture)layer).Image, layer.Metadata.Uid);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }
    }
}
