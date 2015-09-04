using System;
using System.Drawing;

namespace VectorImageEdit.Modules.BasicShapes
{
    [Serializable]
    public class Oval : ShapeBase
    {
        //public Oval() { }

        public Oval(Rectangle region, ShapeStyle style)
            : base(region, 0, style, "Layer - Oval")
        {
            Style = style;
        }

        public override void DrawGraphics(Graphics destination)
        {
            using (var brush = StyleBuilder.BrushBuilder(Style))
            using (var pen = StyleBuilder.PenBuilder(Style))
            {
                destination.DrawEllipse(pen, Region);
                destination.FillEllipse(brush, Region);
            }
        }
    }
}
