using System.Drawing;
using System.Drawing.Drawing2D;
using VectorImageEdit.Modules.BasicShapes;

namespace VectorImageEdit.Modules.Factories
{
    static class ShapeStyleBuilder
    {
        // FIXME DashStyle parameter
        public static ShapeStyle CreateShapeStyle(Color edge, Color fill, float edgeSize)
        {
            var style = new ShapeStyle
            {
                EdgeColor = edge,
                EdgeSize = edgeSize,
                FillColor = fill/*,
                LineDash = DashStyle.Dash*/
            };
            return style;
        }
    }
}
