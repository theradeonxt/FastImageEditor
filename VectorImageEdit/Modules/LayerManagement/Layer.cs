using System;
using System.Drawing;
using JetBrains.Annotations;

namespace VectorImageEdit.Modules.LayerManagement
{
    /// <summary>
    // 
    /// Layer Module
    ///
    /// - represents the basic drawable graphical object
    /// 
    /// </summary>
    [Serializable]
    public abstract class Layer : IComparable<Layer>, IDisposable
    {
        [NotNull]
        public readonly LayerMetadata Metadata;
        private Rectangle _region;

        protected Layer(Rectangle region, int depthLevel, [NotNull]string displayName)
        {
            if (region != Rectangle.Empty)
            {
                bool validFlag = region.Width >= 0 &&
                                 region.Height >= 0 &&
                                 region.Left < region.Right &&
                                 region.Top < region.Bottom;
            }

            _region = region;
            DepthLevel = depthLevel;
            Transparency = 100;

            Metadata = new LayerMetadata(displayName);
        }

        public Rectangle Region
        {
            get { return _region; }
            set
            {
                //  TODO: Inclusive equal lower than?
                bool validFlag = value.Width >= 0 &&
                                 value.Height >= 0 &&
                                 value.Left < value.Right &&
                                 value.Top < value.Bottom;
                if (validFlag) _region = value;
            }
        }
        public int DepthLevel { get; set; }
        public int Transparency { get; set; }

        int IComparable<Layer>.CompareTo(Layer other)
        {
            return DepthLevel.CompareTo(other.DepthLevel);
        }
        public abstract void Dispose();
        public abstract void DrawGraphics(Graphics destination);
        public void Move(Point newLocation)
        {
            _region.Location = newLocation;
        }
        public void Resize(Size newSize)
        {
            // Debug.Assert(newSize.Width >= 0 && newSize.Height >= 0, "Layer resized with invalid properties");
            _region.Size = newSize;
        }
    }
}
