using System;
using System.Drawing;

namespace VectorImageEdit.Modules.BasicShapes
{
    [Serializable]
    public class Ellipse : ShapeBase
    {
        //public Ellipse() { }

        public Ellipse(Rectangle region, ShapeStyle style)
            : base(region, 0, style, "Layer - Ellipse")
        {
            Style = style;
        }

        public override void DrawGraphics(Graphics destination)
        {
            using (var brush = Style.CreateBrush())
            using (var pen = Style.CreatePen())
            {
                destination.DrawEllipse(pen, Region);
                destination.FillEllipse(brush, Region);
            }
        }
    }
}
