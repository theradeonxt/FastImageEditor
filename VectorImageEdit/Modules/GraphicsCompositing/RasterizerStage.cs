using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using NLog;
using VectorImageEdit.Modules.BasicShapes;
using VectorImageEdit.Modules.LayerManagement;
using VectorImageEdit.Modules.Utility;

namespace VectorImageEdit.Modules.GraphicsCompositing
{
    /// <summary>
    /// Wrapper for object properties that will reside in the cache
    /// </summary>
    internal class CacheItem
    {
        public CacheItem(Bitmap image, BitmapHelper imageInfo, object tag)
        {
            Image = image;
            ImageInfo = imageInfo;
            Tag = tag;
        }

        public object Tag { get; private set; }
        public Bitmap Image { get; private set; }
        public BitmapHelper ImageInfo { get; private set; }
    }

    /// <summary>
    /// Storage of cached items that can persist across multiple rasterizer stage calls.
    /// Note: access to the cache is thread-safe for scalable performance.
    /// </summary>
    internal class PersistentCache : ConcurrentDictionary<int, CacheItem>
    {
        public void RegisterItem([NotNull] Bitmap raster, int id, object tag)
        {
            BitmapHelper imageInfo = new BitmapHelper(raster);
            CacheItem item = new CacheItem(raster, imageInfo, tag);

            bool status = TryAdd(id, item);
            if (status == false)
            {
                Debug.WriteLine("Cache aldready has item with id {0}", id);
            }
        }
    }

    class RasterizerStage : IRenderingStage
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        // Persistent variables
        private static readonly PersistentCache Cache = new PersistentCache();
        private static readonly List<Layer> UpdateList = new List<Layer>();

        // Reference to object list
        private SortedContainer<Layer> objectCollection;

        private IGraphicsSurface surfaceInfo;

        public RasterizerStage([NotNull]SortedContainer<Layer> objectCollection, IGraphicsSurface surfaceInfo)
        {
            this.objectCollection = objectCollection;
            this.surfaceInfo = surfaceInfo;

            // reserve space in the update list, if needed
            if (UpdateList.Capacity < objectCollection.Count)
            {
                UpdateList.Capacity = objectCollection.Count;
            }

            RasterizeObjects();
        }

        /// <summary>
        /// Unlocks the low-level data of the cached objects, 
        /// so their images can be accessed and/or modified
        /// </summary>
        public void Dispose()
        {
            try
            {
                for (int i = 0; i < objectCollection.Count; i++)
                {
                    int itemID = objectCollection[i].Metadata.Uid;
                    Cache[itemID].ImageInfo.Dispose(); // this unlocks the data
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
            objectCollection = null; // remove reference
            surfaceInfo = null;
            UpdateList.Clear(); // clear update elements
        }

        /// <summary>
        /// Gets an object that allows safe manipulation of low-level
        /// data for the layer with the given ID.
        /// </summary>
        /// <param name="layerId"> The ID of object </param>
        /// <returns> A BitmapHelper cached object </returns>
        public BitmapHelper GetRasterInfo(int layerId)
        {
            return Cache[layerId].ImageInfo;
        }

        private void ParallelLayerRasterizer()
        {
            // Process every shape in parallel and generate their rasterizations
            Parallel.For(0, UpdateList.Count, (i =>
            {
                Layer layer = UpdateList[i];
                // Image objects only need to register their raw image in the cache
                if (layer is Picture)
                {
                    layer.HasChanged = false; // acknowledge the change of property, drawing is performed later
                    Cache.RegisterItem(((Picture)layer).Image, layer.Metadata.Uid, layer);
                    return;
                }

                // Vector shapes need to be drawn and their rasterization is cached
                // for subsequent calls unless the object has changed
                Bitmap rasterized = ImagingHelpers.Allocate(layer.Region.Width, layer.Region.Height);
                using (var gfx = Graphics.FromImage(rasterized))
                {
                    // prepare for drawing
                    ImagingHelpers.GraphicsHighQualityDrawingBlend(gfx);
                    gfx.Clear(Color.FromArgb(0, 0, 0, 0) /* surfaceInfo.GraphicsBackground*/);

                    // the object's draw method will reset the HasChanged property
                    layer.DrawGraphics(gfx);
                }

                Cache.RegisterItem(rasterized, layer.Metadata.Uid, layer);
            }));
        }

        private void RasterizeObjects()
        {
            try
            {
                // 1. Check which items are outdated in the cache and remove them.
                List<CacheItem> cacheValues = Cache.Values.ToList();
                for (int i = 0; i < cacheValues.Count; i++)
                {
                    CacheItem item = cacheValues[i];
                    if (objectCollection.Contains((Layer)item.Tag) == false)
                    {
                        // remove from cache
                        CacheItem temporary;
                        if (Cache.TryRemove(((Layer)item.Tag).Metadata.Uid, out temporary))
                        {
                            // ensure the cached resource is released (ok to call Dispose twice)
                            item.ImageInfo.Dispose();
                            item.Image.Dispose();
                        }
                    }
                }

                // 2. Check which layers have changed and build an update list
                // A change is due to: - new item encountered
                //                     - object itself has changed
                for (int index = 0; index < objectCollection.Count; index++)
                {
                    Layer layer = objectCollection[index];
                    CacheItem temporary;
                    int itemId = layer.Metadata.Uid;
                    if (Cache.TryGetValue(itemId, out temporary) == false)
                    {
                        // a new item exists in the objectCollection, mark for updating
                        UpdateList.Add(layer);
                    }
                    else if (layer.HasChanged)
                    {
                        // item already exists in the cache but needs updating
                        // remove it and mark for update
                        Cache.TryRemove(itemId, out temporary);
                        temporary.ImageInfo.Dispose();
                        UpdateList.Add(layer);
                    }
                }

                // 3. Generate rasterizations of changed objects (Multithreaded)
                ParallelLayerRasterizer();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }
    }
}
