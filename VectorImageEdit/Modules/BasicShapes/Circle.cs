using System;
using System.Drawing;
using VectorImageEdit.Modules.BasicShapes.Geometries;

namespace VectorImageEdit.Modules.BasicShapes
{
    [Serializable]
    public class Circle : ShapeBase
    {
        public Circle(Point center, int radius, ShapeStyle style)
            : base(new Rectangle(center.X - radius, center.Y - radius, 2 * radius, 2 * radius),
                  0, style, "Layer - Circle", true)
        {
            RadiusGeometry = new GeometryItem(
                new GeometryPoint(center, GeometryPointType.Movable),
                new GeometryPoint(new Point(center.X + radius, center.Y), GeometryPointType.Movable),
                GeometryItemType.Cue);

            EditablegeometryCue.Add(RadiusGeometry);
        }

        public GeometryItem RadiusGeometry { get; set; }

        public override void DrawGraphics(Graphics destination)
        {
            DrawGraphics();

            using (var brush = Style.CreateBrush())
            using (var pen = Style.CreatePen())
            {
                destination.DrawEllipse(pen, Region);
                destination.FillEllipse(brush, Region);
            }

            EditablegeometryCue.DrawGeometry(destination);
        }

        public override void ConstructFrom(Rectangle region)
        {
            Point center = region.Location;
            center.Offset(region.Width/2, region.Height/2);
            RadiusGeometry.Points[0].Move(center);

            center.Offset(region.Width/2, 0);
            RadiusGeometry.Points[1].Move(center);
        }
    }
}
