using System;
using System.Drawing;
using VectorImageEdit.Modules.Layers;

namespace VectorImageEdit.Modules.BasicShapes
{
    [Serializable]
    public class SRectangle : ShapeBase
    {
        //public SRectangle() { }

        public SRectangle(Rectangle region, ShapeStyle style)
            : base(region, 0, style, "Layer - SRectangle")
        {
            Style = style;
        }

        public override void DrawGraphics(Graphics destination)
        {
            using (var brush = StyleBuilder.BrushBuilder(Style))
            using (var pen = StyleBuilder.PenBuilder(Style))
            {
                destination.DrawRectangle(pen, Region);
                destination.FillRectangle(brush, Region);
            }
        }
    }
}
