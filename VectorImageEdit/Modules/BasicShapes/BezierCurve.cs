using System;
using System.Drawing;
using VectorImageEdit.Modules.BasicShapes.Geometries;

namespace VectorImageEdit.Modules.BasicShapes
{
    [Serializable]
    public class BezierCurve : ShapeBase
    {
        public BezierCurve(Point p0, Point p1, Point p2, Point p3, ShapeStyle style)
            : base(new Rectangle(
                Math.Min(Math.Min(Math.Min(p0.X, p1.X), p2.X), p3.X),
                Math.Min(Math.Min(Math.Min(p0.Y, p1.Y), p2.Y), p3.Y),
                Math.Max(Math.Max(Math.Max(p0.X, p1.X), p2.X), p3.X) - Math.Min(Math.Min(Math.Min(p0.X, p1.X), p2.X), p3.X),
                Math.Max(Math.Max(Math.Max(p0.Y, p1.Y), p2.Y), p3.Y) - Math.Min(Math.Min(Math.Min(p0.Y, p1.Y), p2.Y), p3.Y)),
                0,
                style,
                "Layer - BezierCurve")
        {
            ControlPoints = new[]
            {
                new GeometryPoint(p0, GeometryPointType.Movable),
                new GeometryPoint(p1, GeometryPointType.Movable),
                new GeometryPoint(p2, GeometryPointType.Movable),
                new GeometryPoint(p3, GeometryPointType.Movable)
            };

            controlPointLinks = new GeometryItem[3];
            controlPointLinks[0].Points[0] = ControlPoints[0];
            controlPointLinks[0].Points[1] = ControlPoints[1];
            controlPointLinks[1].Points[0] = ControlPoints[1];
            controlPointLinks[1].Points[1] = ControlPoints[2];
            controlPointLinks[2].Points[0] = ControlPoints[2];
            controlPointLinks[2].Points[1] = ControlPoints[3];

            EditablegeometryCue.AddRange(ControlPoints);
            EditablegeometryCue.AddRange(controlPointLinks);
        }

        /// <summary>
        /// Gets the editable geometry points that define this Bezier curve.
        /// Note: the shape region contains all these points.
        /// </summary>
        public GeometryPoint[] ControlPoints { get; private set; }

        private GeometryItem[] controlPointLinks;

        public override void DrawGraphics(Graphics destination)
        {
            DrawGraphics();
            using (var pen = Style.CreatePen())
            {
                destination.DrawBezier(pen, ControlPoints[0], ControlPoints[1], ControlPoints[2], ControlPoints[3]);
            }

            EditablegeometryCue.DrawGeometry(destination);
        }

        public override void ConstructFrom(Rectangle region)
        {
            throw new NotSupportedException();
        }
    }
}
