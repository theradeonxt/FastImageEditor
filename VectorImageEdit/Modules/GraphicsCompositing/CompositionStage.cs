using JetBrains.Annotations;
using System;
using System.Drawing;
using System.Threading.Tasks;
using VectorImageEdit.Modules.LayerManagement;
using VectorImageEdit.Modules.Utility;

namespace VectorImageEdit.Modules.GraphicsCompositing
{
    /// <summary>
    /// Last rendering stage. Draws rasterized objects on the frame buffer.
    /// </summary>
    class CompositionStage : IRenderingStage
    {
        public CompositionStage(SortedContainer<Layer> objectCollection, RasterizerStage rasterizer)
        {
            this.objectCollection = objectCollection;
            this.rasterizer = rasterizer;
        }

        public void Composite([NotNull]IRenderingPolicy renderPolicy, [NotNull]BitmapHelper frameInfo)
        {
            Parallel.For(renderPolicy.ScanlineBegin, renderPolicy.ScanlineEnd, y =>
            {
                ScanlineRenderer(y, renderPolicy.DirtyRegion, frameInfo);
            });
        }

        public void Dispose()
        {
            objectCollection = null;
            rasterizer = null;
        }

        #region Private Members

        private SortedContainer<Layer> objectCollection;
        private RasterizerStage rasterizer;

        private unsafe void ScanlineRenderer(int y, Rectangle dirtyRegion, BitmapHelper frameData)
        {
            byte* currentScanLine = frameData.Start + (y * frameData.Stride);

            // TODO: If top-most layer covering the scanline is 100% opaque
            /*if (objectCollection.Count > 0)
            { 
                Layer last = objectCollection.Last;
                if (last.Transparency == 100 && 
                    (ScanlineIntersects(y, last.Region)))
                {
                    CompositeIfNeeded(y, dirtyRegion, currentScanLine, last, rasterizer);
                    return;
                }
            }*/

            // Do a Painter's algorithm iteration (back->front) on the layers
            for (int index = 0; index < objectCollection.Count; index++)
            {
                Layer layer = objectCollection[index];
                CompositeIfNeeded(y, dirtyRegion, currentScanLine, layer);
            }
        }

        private unsafe void CompositeIfNeeded(int y, Rectangle dirtyRegion, byte* currentScanLine, Layer layer)
        {
            Rectangle objRegion = layer.Region;

            if (!ScanlineIntersects(y, objRegion) ||
                !dirtyRegion.IntersectsWith(objRegion)) return;

            // Find the bounds for the "dirty region" of scanline (clamped inside frame)
            int scanlineLeft = Math.Max(dirtyRegion.Left, Math.Min(objRegion.Left, objRegion.Right));
            int scanlineRight = Math.Min(dirtyRegion.Right, Math.Max(objRegion.Left, objRegion.Right));

            // Access the raw data structure of the rasterization
            BitmapHelper layerInfo = rasterizer.GetRasterInfo(layer.Metadata.Uid);

            // Calculate memory indexes
            byte* src = LayerPixel(objRegion, layerInfo, y);
            byte* dst = FrameBufferPixel(currentScanLine, scanlineLeft, layerInfo);
            uint dirtySizeBytes = DirtySize(scanlineLeft, scanlineRight, layerInfo);

            // Composite scanline with layer data
            ImageProcessingApi.AlphaBlend32bgra_32bgra(src, dst, dst, dirtySizeBytes);
        }

        private uint DirtySize(int leftOffset, int rightOffset, BitmapHelper layerInfo)
        {
            uint dirtySizeBytes = (uint)((rightOffset - leftOffset) * layerInfo.PixelSize); // size of modified scanline region
            return dirtySizeBytes;
        }

        private unsafe byte* LayerPixel(Rectangle layerRegion, BitmapHelper layerInfo, int y)
        {
            byte* src = (layerInfo.Start +                                                              // layer data start
                        (y - layerRegion.Top) * layerInfo.Stride) +                                     // image data row
                        (layerRegion.Left < 0 ? layerInfo.PixelSize * Math.Abs(layerRegion.Left) : 0);  // image data column
            return src;
        }

        private unsafe byte* FrameBufferPixel(byte* currentScanLine, int leftOffset, BitmapHelper layerInfo)
        {
            byte* dst = currentScanLine + (leftOffset) * layerInfo.PixelSize; // frame scanline
            return dst;
        }

        private bool ScanlineIntersects(int y, Rectangle region)
        {
            return y >= region.Top && y <= region.Bottom;
        }

        #endregion
    }
}
