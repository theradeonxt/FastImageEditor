﻿using System;
using System.Drawing;

namespace VectorImageEdit.Modules.BasicShapes
{
    [Serializable]
    public class Triangle : ShapeBase
    {
        public Triangle(Rectangle region, int depthLevel, ShapeStyle style, string displayName)
            : base(region, depthLevel, style, displayName)
        {
            throw new NotImplementedException();
        }

        public override void DrawGraphics(Graphics destination)
        {
            throw new NotImplementedException();
        }
    }
}
