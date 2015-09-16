using System.Drawing;
using VectorImageEdit.Modules.BasicShapes;

namespace VectorImageEdit.Modules.Factories
{
    static class ShapeStyleBuilder
    {
        public static ShapeStyle CreateShapeStyle(Color edge, Color fill, float edgeSize)
        {
            var style = new ShapeStyle
            {
                EdgeColor = edge,
                EdgeSize = edgeSize,
                FillColor = fill
            };
            return style;
        }
    }
}
