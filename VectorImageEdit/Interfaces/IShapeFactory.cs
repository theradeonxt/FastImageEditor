using System.Drawing;
using VectorImageEdit.Modules.BasicShapes;

namespace VectorImageEdit.Interfaces
{
    interface IShapeFactory
    {
        ShapeBase CreateShape(Size size, ShapeStyle style);
    }
}