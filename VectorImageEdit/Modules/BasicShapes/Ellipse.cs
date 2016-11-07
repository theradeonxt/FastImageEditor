using System;
using System.Drawing;
using VectorImageEdit.Modules.BasicShapes.Geometries;

namespace VectorImageEdit.Modules.BasicShapes
{
    [Serializable]
    public sealed class Ellipse : ShapeBase
    {
        public Ellipse(Rectangle region, ShapeStyle style)
            : base(region, 0, style, "Layer - Ellipse", true)
        {
            ConstructFrom(region);
            EditablegeometryCue.AddRange(Axes);
        }

        public GeometryItem[] Axes { get; private set; }

        public override void DrawGraphics(Graphics destination)
        {
            DrawGraphics();

            using (var brush = Style.CreateBrush())
            using (var pen = Style.CreatePen())
            {
                destination.DrawEllipse(pen, Region);
                destination.FillEllipse(brush, Region);
            }

            EditablegeometryCue.DrawGeometry(destination);
        }

        public override void ConstructFrom(Rectangle region)
        {
            Point midTop, midBottom, midLeft, midRight;
            MidpointGeometryConstructor.Points(out midTop, out midBottom, out midLeft, out midRight, region);

            if (Axes == null)
            {
                Axes = new GeometryItem[2];

                Axes[0] = new GeometryItem(new GeometryPoint(midTop, GeometryPointType.ResizableNS),
                    new GeometryPoint(midBottom, GeometryPointType.ResizableNS),
                    GeometryItemType.Cue);

                Axes[1] = new GeometryItem(new GeometryPoint(midLeft, GeometryPointType.ResizableWE),
                    new GeometryPoint(midRight, GeometryPointType.ResizableWE),
                    GeometryItemType.Cue);
            }
        }
    }
}
