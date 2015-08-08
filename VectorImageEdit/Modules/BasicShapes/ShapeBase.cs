using System;
using System.Drawing;
using VectorImageEdit.Modules.Layers;

namespace VectorImageEdit.Modules.BasicShapes
{
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
        }

        public ShapeStyle Style { get; set; }
    }
}
