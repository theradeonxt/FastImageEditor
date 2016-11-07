using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using VectorImageEdit.Modules.BasicShapes.Geometries;

namespace VectorImageEdit.Modules.BasicShapes
{
    [Serializable]
    public sealed class Diamond : ShapeBase
    {
        public Diamond(Rectangle region, ShapeStyle style) :
            base(region, 0, style, "Layer - Diamond", true)
        {
            ConstructFrom(region);
            EditablegeometryCue.AddRange(Edges);
        }

        public GeometryItem[] Edges { get; private set; }

        public override void DrawGraphics(Graphics destination)
        {
            DrawGraphics();

            using (var gfxPath = new GraphicsPath())
            using (var brush = Style.CreateBrush())
            using (var pen = Style.CreatePen())
            {
                gfxPath.Reset();
                gfxPath.AddLine(Edges[0].Points[0], Edges[0].Points[1]);
                gfxPath.AddLine(Edges[1].Points[0], Edges[1].Points[1]);
                gfxPath.AddLine(Edges[2].Points[0], Edges[2].Points[1]);
                gfxPath.AddLine(Edges[3].Points[0], Edges[3].Points[1]);

                destination.FillPath(brush, gfxPath);
                //destination.DrawPath(pen, gfxPath);
            }

            EditablegeometryCue.DrawGeometry(destination);
        }

        public override void ConstructFrom(Rectangle region)
        {
            Point midTop, midBottom, midLeft, midRight;
            MidpointGeometryConstructor.Points(out midTop, out midBottom, out midLeft, out midRight, region);

            if (Edges == null)
            {
                Edges = new GeometryItem[4];

                Edges[0] = new GeometryItem(new GeometryPoint(midTop, GeometryPointType.Movable),
                    new GeometryPoint(midRight, GeometryPointType.Movable),
                    GeometryItemType.Cue);

                Edges[1] = new GeometryItem(Edges[0].Points[1],
                    new GeometryPoint(midBottom, GeometryPointType.Movable),
                    GeometryItemType.Cue);

                Edges[2] = new GeometryItem(Edges[1].Points[1],
                    new GeometryPoint(midLeft, GeometryPointType.Movable),
                    GeometryItemType.Cue);

                Edges[3] = new GeometryItem(Edges[2].Points[1], Edges[0].Points[0],
                    GeometryItemType.Cue);
            }
        }
    }
}
