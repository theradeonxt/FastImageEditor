using System;
using System.Collections.Generic;
using System.Drawing;
using VectorImageEdit.Modules.Layers;
using VectorImageEdit.Modules.Utility;

namespace VectorImageEdit.Modules
{
    class RasterizerStage : IDisposable
    {
        private readonly SortedContainer<Layer> _objectCollection;
        private readonly IDictionary<int, Bitmap> _rasterMap;

        public RasterizerStage(SortedContainer<Layer> objectCollection)
        {
            _objectCollection = objectCollection;
            _rasterMap = new Dictionary<int, Bitmap>();
            RasterizeObjects();
        }

        private void RasterizeObjects()
        {
            // Build rasterizations for every layer
            foreach (var layer in _objectCollection)
            {
                if (layer is Picture)
                {
                    _rasterMap.Add(layer.Metadata.Uid, ((Picture)layer).Image);
                    continue;
                }
                // Only a Vector object (Shape) needs rasterizing
                try
                {
                    var rasterized = ImagingHelpers.Allocate(layer.Region.Width, layer.Region.Height);
                    using (var gfx = Graphics.FromImage(rasterized))
                    {
                        layer.DrawGraphics(gfx);
                    }
                    _rasterMap.Add(layer.Metadata.Uid, rasterized);
                }
                catch (OutOfMemoryException) { }
                catch (ArgumentException) { }
            }
        }

        public void Dispose()
        {
            foreach (var item in _rasterMap)
            {
                item.Value.Dispose();
            }
            _rasterMap.Clear();
        }
    }
}
