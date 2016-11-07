using System;
using System.Drawing;

namespace VectorImageEdit.Modules.BasicShapes
{
    [Serializable]
    public class Star : ShapeBase
    {
        public Star(Rectangle region, int depthLevel, ShapeStyle style, string displayName)
            : base(region, depthLevel, style, displayName)
        {
            throw new NotImplementedException();
        }

        public override void DrawGraphics(Graphics destination)
        {
            DrawGraphics();
            throw new NotImplementedException();
        }

        public override void ConstructFrom(Rectangle region)
        {
            throw new NotImplementedException();
        }
    }
}
