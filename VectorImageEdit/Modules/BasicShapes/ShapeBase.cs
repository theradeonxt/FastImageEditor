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
        protected ShapeBase(Rectangle region, int depthLevel, ShapeStyle style, string displayName)
            : base(region, depthLevel, displayName)
        {
            Style = style;
        }

        public abstract override void DrawGraphics(Graphics destination);
        public override void Dispose()
        {
            // Dummy from IDisposable interface
        }

        public ShapeStyle Style { get; set; }
    }
}
