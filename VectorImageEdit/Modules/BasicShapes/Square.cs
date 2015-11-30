using System;
using System.Drawing;

namespace VectorImageEdit.Modules.BasicShapes
{
    [Serializable]
    public class Square : ShapeBase
    {
        public Square(Point location, int side, ShapeStyle style)
            : base(new Rectangle(location.X, location.Y, side, side), 0, style, "Layer - Square")
        {
        }

        public override void DrawGraphics(Graphics destination)
        {
            using (var brush = Style.CreateBrush())
            using (var pen = Style.CreatePen())
            {
                destination.DrawRectangle(pen, Region);
                destination.FillRectangle(brush, Region);
            }
        }
    }
}
