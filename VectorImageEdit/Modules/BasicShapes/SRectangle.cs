using System;
using System.Drawing;

namespace VectorImageEdit.Modules.BasicShapes
{
    [Serializable]
    public class SRectangle : ShapeBase
    {
        public SRectangle(Rectangle region, ShapeStyle style)
            : base(region, 0, style, "Layer - Rectangle")
        {
        }

        public override void DrawGraphics(Graphics destination)
        {
            DrawGraphics();
            using (var brush = Style.CreateBrush())
            using (var pen = Style.CreatePen())
            {
                destination.DrawRectangle(pen, Region);
                destination.FillRectangle(brush, Region);
            }
        }

        public override void ConstructFrom(Rectangle region)
        {
            throw new NotImplementedException();
        }
    }
}
