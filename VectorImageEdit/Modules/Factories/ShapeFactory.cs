using System;
using System.Collections.Generic;
using System.Drawing;
using VectorImageEdit.Modules.BasicShapes;

namespace VectorImageEdit.Modules.Factories
{
    internal enum ShapeObject
    {
        Circle,
        Square,
        Rectangle,
        Ellipse,
        Hexagon,
        Star,
        Diamond,
        Line
    };

    static class ShapeFactory
    {
        private static readonly Dictionary<ShapeObject, Func<Size, ShapeStyle, ShapeBase>> ShapeCreationMap;

        static ShapeFactory()
        {
            ShapeCreationMap = new Dictionary<ShapeObject, Func<Size, ShapeStyle, ShapeBase>>();
            ShapeCreationMap.Add(ShapeObject.Circle, (size, style) =>
            {
                Point center = new Point(size.Width / 2, size.Height / 2);
                return new Circle(center, size.Width / 7, style);
            });
            ShapeCreationMap.Add(ShapeObject.Square, (size, style) =>
            {
                Point location = new Point(size.Width / 3, size.Height / 3);
                int side = size.Width / 4;
                return new Square(location, side, style);
            });
            ShapeCreationMap.Add(ShapeObject.Ellipse, (size, style) =>
            {
                Point location = new Point(size.Width / 3, size.Height / 3);
                return new Ellipse(new Rectangle(location.X, location.Y, size.Width / 5, size.Height / 7), style);
            });
            ShapeCreationMap.Add(ShapeObject.Rectangle, (size, style) =>
            {
                Point location = new Point(size.Width / 3, size.Height / 3);
                Rectangle region = new Rectangle(location.X, location.Y, size.Width, size.Height);
                return new SRectangle(region, style); 
            });
            ShapeCreationMap.Add(ShapeObject.Line, (size, style) =>
            {
                Point begin = new Point(size.Width / 4, size.Height / 5);
                Point end = new Point(size.Width / 2, size.Height / 4);
                return new Line(begin, end, style);
            });
        }

        public static ShapeBase CreateShape(ShapeObject type, Size size, ShapeStyle style)
        {
            return ShapeCreationMap[type](size, style);
        }
    }
}
