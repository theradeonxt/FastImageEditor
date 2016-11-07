using System;
using System.Drawing;
using VectorImageEdit.Modules.BasicShapes.Geometries;

namespace VectorImageEdit.Modules.BasicShapes
{
    [Serializable]
    public class Line : ShapeBase
    {
        public Line(Point begin, Point end, ShapeStyle style)
            : base(new Rectangle(
                Math.Min(begin.X, end.X),
                Math.Min(begin.Y, end.Y),
                Math.Abs(begin.X - end.X),
                Math.Abs(begin.Y - end.Y)),
                0,
                style,
                "Layer - Line")
        {
            Begin = new GeometryPoint(begin, GeometryPointType.Movable);
            End = new GeometryPoint(end, GeometryPointType.Movable);

            EditablegeometryCue.Add(Begin);
            EditablegeometryCue.Add(End);
        }

        public GeometryPoint Begin { get; set; }
        public GeometryPoint End { get; set; }

        public override void DrawGraphics(Graphics destination)
        {
            DrawGraphics();
            using (var pen = Style.CreatePen())
            {
                destination.DrawLine(pen, Begin.Center, End.Center);
            }

            EditablegeometryCue.DrawGeometry(destination);
        }

        public override void ConstructFrom(Rectangle region)
        {
            throw new NotSupportedException();
        }
    }
}
