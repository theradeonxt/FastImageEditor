using System;
using System.Diagnostics;
using System.Drawing;

namespace VectorImageEdit.Modules.Layers
{
    /// <summary>
    /// 
    /// Layer Module
    ///
    /// - represents the basic drawable graphical object
    /// 
    /// </summary>
    [Serializable]
    public abstract class Layer : IComparable<Layer>, IDisposable
    {
        public readonly LayerMetadata Metadata;

        // this is shadowed by a property for validation reasons and serialization hiding
        private Rectangle _region;

        /*protected Layer()
        {
        }*/

        protected Layer(Rectangle region, int depthLevel, string displayName)
        {
            Debug.Assert(region.Width >= 0
                && region.Height >= 0
                && region.Left < region.Right
                && region.Top < region.Bottom,
                "Layer created with invalid properties");

            _region = region;
            DepthLevel = depthLevel;

            Metadata = new LayerMetadata(displayName);
        }

        #region Property Getters/Setters

        public Rectangle Region
        {
            get { return _region; }
            set
            {
                Debug.Assert(value.Width >= 0 && value.Height >= 0
                    && value.Left < value.Right && value.Top < value.Bottom, "Layer region set with invalid properties");
                _region = value;
            }
        }
        public int DepthLevel { get; set; }

        #endregion

        #region Interface Implementation

        // Compare two layers based on their depth levels using back-to-front ordering
        int IComparable<Layer>.CompareTo(Layer other)
        {
            return DepthLevel.CompareTo(other.DepthLevel);
        }

        public abstract void Dispose();
        public abstract void DrawGraphics(Graphics destination);

        #endregion

        public void Move(Point newLocation)
        {
            _region.Location = newLocation;
        }

        public void Resize(Size newSize)
        {
            //Debug.Assert(newSize.Width >= 0 && newSize.Height >= 0, "Layer resized with invalid properties");
            _region.Size = newSize;
        }
    }
}
