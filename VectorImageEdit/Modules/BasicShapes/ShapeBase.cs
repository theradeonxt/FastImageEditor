using System;
using System.Drawing;
using VectorImageEdit.Modules.LayerManagement;

namespace VectorImageEdit.Modules.BasicShapes
{
    /// <summary>
    /// Base class for every Shape object that is represented in a vector format
    /// </summary>
    [Serializable]
    public abstract class ShapeBase : Layer
    {
        protected ShapeBase(Rectangle region, int depthLevel, ShapeStyle style, string displayName, bool supportsRegionConstruction = false)
            : base(region, depthLevel, displayName)
        {
            Style = style;
            SupportsRegionConstruction = supportsRegionConstruction;
        }

        private ShapeStyle style;
        public ShapeStyle Style
        {
            get { return style; }
            set
            {
                style = value;
                OnPropertyChanged();
            }
        }

        protected void DrawGraphics()
        {
            // acknowledge request for object update
            HasChanged = false;
        }

        public override void Dispose()
        {
            // a vector shape has no disposable resources.
            // note that each concrete shape can still override the Dispose()
            // method if desired.
        }

        /// <summary>
        /// Gets a flag indicating if the shape geometry can be constructed from a region.
        /// </summary>
        public bool SupportsRegionConstruction { get; protected set; }

        /// <summary>
        /// Reconstructs the shape geometry from the given region, if SupportsRegionConstruction is true.
        /// </summary>
        public abstract void ConstructFrom(Rectangle region);
    }
}
