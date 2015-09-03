using System;
using System.Drawing;

namespace VectorImageEdit.Modules.BasicShapes
{
    [Serializable]
    public class ShapeStyle
    {
        public Color EdgeColor { get; set; }
        public Color FillColor { get; set; }
        public float EdgeSize { get; set; }
    }
}
