using System;
using System.Drawing;

namespace VectorImageEdit.Modules.BasicShapes
{
    [Serializable]
    public class Circle : ShapeBase
    {
        //public Circle() { }

        public Circle(Point center, int radius, ShapeStyle style)
            : base(new Rectangle(center.X - radius, center.Y - radius, 2 * radius, 2 * radius), 0, style, "Layer - Circle")
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
