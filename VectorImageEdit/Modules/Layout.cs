using System;
using System.Drawing;

namespace VectorImageEdit.Modules
{
    /// <summary>
    /// Layout Module
    ///
    /// - gets size & location for new objects
    /// 
    /// </summary>
    class Layout
    {
        private Size _maximumSize;
        private const int Padding = 20;
        private const int MarginX = 20;
        private const int MarginY = 20;
        private readonly Random _random = new Random();

        public Layout(Size maximumBounds)
        {
            SetBounds(maximumBounds);
        }

        /// <summary>
        /// Instruct the layout to use the given size as the maximum bounds
        /// </summary>
        /// <param name="maximumBounds"> New bounds </param>
        public void SetBounds(Size maximumBounds)
        {
            _maximumSize = maximumBounds;
        }

        /// <summary>
        /// Get the largest size that can fit the current layout bounds
        /// </summary>
        /// <returns></returns>
        public Size MaximumSize()
        {
            return _maximumSize;
        }

        /// <summary>
        /// Requests a size that bests fits the layer for this layout.
        /// If the given size exceeds the maximum layout bounds,
        /// it is scaled down randomly and tries to keep aspect ratio.
        /// </summary>
        /// <param name="layerSize"> Original size </param>
        /// <returns> New bounds </returns>
        public Rectangle NewLayerMetrics(Size layerSize)
        {
            int width, height;
            float aspectRatio = (float)layerSize.Width / layerSize.Height;

            // Check if downscaling needed (aspect preserving)
            if (layerSize.Width > _maximumSize.Width ||
                layerSize.Height > _maximumSize.Height)
            {
                width = _random.Next(
                    Math.Abs(_maximumSize.Width - 3 * Padding),
                    Math.Abs(_maximumSize.Width - Padding));
                // TODO: not enough to keep aspect ratio: this height can be > workspace height
                float heightf = width / aspectRatio;
                height = (int)heightf;
            }
            else
            {
                width = Math.Abs(layerSize.Width - Padding);
                float heightf = width / aspectRatio;
                height = (int)heightf;
            }

            // The location is always randomized
            int x = _random.Next(MarginX, MarginX * 3);
            int y = _random.Next(MarginY, MarginY * 3);

            // Ensure it ends up with positive sizes
            if (width <= 0) width = Math.Abs(layerSize.Width);
            if (height <= 0) height = Math.Abs(layerSize.Height);

            return new Rectangle(x, y, width, height);
        }
    }
}
