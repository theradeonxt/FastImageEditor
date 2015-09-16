using System.Drawing;
using VectorImageEdit.Modules.BasicShapes;

namespace VectorImageEdit.Modules.Factories
{
    interface IShapeFactory
    {
        ShapeBase CreateShape(Size size, ShapeStyle style);
    }

    class CircleShapeFactory : IShapeFactory
    {
        public ShapeBase CreateShape(Size size, ShapeStyle style)
        {
            Point center = new Point(size.Width / 2, size.Height / 2);
            return new Circle(center, size.Width / 7, style);
        }
    }

    class SquareShapeFactory : IShapeFactory
    {
        public ShapeBase CreateShape(Size size, ShapeStyle style)
        {
            Point location = new Point(size.Width / 3, size.Height / 3);
            int side = size.Width / 4;
            return new Square(location, side, style);
        }
    }

    class EllipseShapeFactory : IShapeFactory
    {
        public ShapeBase CreateShape(Size size, ShapeStyle style)
        {
            Point location = new Point(size.Width / 3, size.Height / 3);
            return new Ellipse(new Rectangle(location.X, location.Y, size.Width / 5, size.Height / 7), style);
        }
    }

    class RectangleShapeFactory : IShapeFactory
    {
        public ShapeBase CreateShape(Size size, ShapeStyle style)
        {
            Point location = new Point(size.Width / 3, size.Height / 3);
            Rectangle region = new Rectangle(location.X, location.Y, size.Width / 2, size.Height / 2);
            return new SRectangle(region, style);
        }
    }

    class LineShapeFactory : IShapeFactory
    {
        public ShapeBase CreateShape(Size size, ShapeStyle style)
        {
            Point begin = new Point(size.Width / 4, size.Height / 5);
            Point end = new Point(size.Width / 2, size.Height / 4);
            return new Line(begin, end, style);
        }
    }

    class StarShapeFactory : IShapeFactory
    {
        public ShapeBase CreateShape(Size size, ShapeStyle style)
        {
            throw new System.NotImplementedException();
        }
    }

    class HexagonShapeFactory : IShapeFactory
    {
        public ShapeBase CreateShape(Size size, ShapeStyle style)
        {
            throw new System.NotImplementedException();
        }
    }

    class DiamondShapeFactory : IShapeFactory
    {
        public ShapeBase CreateShape(Size size, ShapeStyle style)
        {
            throw new System.NotImplementedException();
        }
    }

    class TriangleShapeFactory : IShapeFactory
    {
        public ShapeBase CreateShape(Size size, ShapeStyle style)
        {
            throw new System.NotImplementedException();
        }
    }
}
