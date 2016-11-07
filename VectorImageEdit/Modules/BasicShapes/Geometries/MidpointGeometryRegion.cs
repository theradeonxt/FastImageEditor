using System.Drawing;

namespace VectorImageEdit.Modules.BasicShapes.Geometries
{
    static class MidpointGeometryConstructor
    {
        public static void Points(out Point midTop, out Point midBottom, out Point midLeft, out Point midRight, Rectangle region)
        {
            midTop = region.Location;
            midTop.Offset(region.Width / 2, 0);
            midBottom = midTop;
            midBottom.Offset(0, region.Height);
            midRight = region.Location;
            midRight.Offset(region.Width, region.Height / 2);
            midLeft = midRight;
            midLeft.Offset(-region.Width, 0);
        }
    }
}
