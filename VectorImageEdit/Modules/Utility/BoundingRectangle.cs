using System;
using System.Drawing;

namespace VectorImageEdit.Modules.Utility
{
    class BoundingRectangle
    {
        private Rectangle _region;

        public BoundingRectangle(Size initialSize = default(Size), Point initialPosition = default(Point))
        {
            _region = new Rectangle(initialPosition, initialSize);
        }

        /// <summary>
        /// Gets the bounding box values
        /// </summary>
        public Rectangle Region
        {
            get { return _region; }
            set { _region = value; }
        }

        /// <summary>
        /// Enlarges this bounding box instance to also contain the specified region
        /// </summary>
        /// <param name="other"></param>
        public void EnlargeToFit(Rectangle other)
        {
            int minTop = Math.Min(other.Top, Region.Top);
            int maxBottom = Math.Max(other.Bottom, Region.Bottom);
            int minLeft = Math.Min(other.Left, Region.Left);
            int maxRight = Math.Max(other.Right, Region.Right);
            _region.Y = minTop;
            _region.X = minLeft;
            _region.Width = maxRight - minLeft;
            _region.Height = maxBottom - minTop;
        }
    }
}
