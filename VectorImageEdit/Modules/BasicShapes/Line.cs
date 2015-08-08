﻿using System;
using System.Drawing;
using VectorImageEdit.Modules.Layers;

namespace VectorImageEdit.Modules.BasicShapes
{
    [Serializable]
    public class Line : ShapeBase
    {
        //public Line() { }

        public Line(Point begin, Point end, ShapeStyle style)
            : base(new Rectangle(
                Math.Min(begin.X, end.X),
                Math.Min(begin.Y, end.Y),
                Math.Abs(begin.X - end.X),
                Math.Abs(begin.Y - end.Y)), 
                0,
                style,
                "Layer - Line")
        {
            Style = style;
        }

        public override void DrawGraphics(Graphics destination)
        {
            using (var pen = StyleBuilder.PenBuilder(Style))
            {
                destination.DrawLine(pen, Region.Left, Region.Top, Region.Right, Region.Bottom);
            }
        }
    }
}
