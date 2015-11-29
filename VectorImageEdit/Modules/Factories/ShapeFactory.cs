using System.Drawing;
using VectorImageEdit.Modules.BasicShapes;

namespace VectorImageEdit.Modules.Factories
{
    static class DefaultShapeFactory
    {
        public delegate ShapeBase ShapeCreator(Size size, ShapeStyle style);

        public static ShapeBase CreateCircle(Size size, ShapeStyle style)
        {
            Point center = new Point(size.Width / 2, size.Height / 2);
            return new Circle(center, size.Width / 7, style);
        }

        public static ShapeBase CreateSquare(Size size, ShapeStyle style)
        {
            Point location = new Point(size.Width / 3, size.Height / 3);
            int side = size.Width / 4;
            return new Square(location, side, style);
        }

        public static ShapeBase CreateEllipse(Size size, ShapeStyle style)
        {
            Point location = new Point(size.Width / 3, size.Height / 3);
            return new Ellipse(new Rectangle(location.X, location.Y, size.Width / 5, size.Height / 7), style);
        }

        public static ShapeBase CreateRectangle(Size size, ShapeStyle style)
        {
            Point location = new Point(size.Width / 3, size.Height / 3);
            Rectangle region = new Rectangle(location.X, location.Y, size.Width / 2, size.Height / 2);
            return new SRectangle(region, style);
        }

        public static ShapeBase CreateLine(Size size, ShapeStyle style)
        {
            Point begin = new Point(size.Width / 4, size.Height / 5);
            Point end = new Point(size.Width / 2, size.Height / 4);
            return new Line(begin, end, style);
        }

        public static ShapeBase CreateStar(Size size, ShapeStyle style)
        {
            throw new System.NotImplementedException();
        }

        public static ShapeBase CreateHexagon(Size size, ShapeStyle style)
        {
            throw new System.NotImplementedException();
        }

        public static ShapeBase CreateDiamond(Size size, ShapeStyle style)
        {
            throw new System.NotImplementedException();
        }

        public static ShapeBase CreateTriangle(Size size, ShapeStyle style)
        {
            throw new System.NotImplementedException();
        }
    }
}
