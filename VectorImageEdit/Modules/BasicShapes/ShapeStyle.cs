using System;
using System.Drawing;

namespace VectorImageEdit.Modules.BasicShapes
{
    [Serializable]
    public class ShapeStyle
    {
        // Global settings used to add new shapes
        private static Color _globalEdgeColor = Color.OrangeRed;
        private static Color _globalFillColor = Color.LightBlue;
        private static float _globalEdgeSize = 4.0f;

        public ShapeStyle() { }

        public ShapeStyle(Color edgeColor, Color fillColor)
        {
            EdgeColor = edgeColor;
            FillColor = fillColor;
        }

        public Color EdgeColor { get; set; }
        public Color FillColor { get; set; }
        public float EdgeSize { get; set; }

        #region Global style properties
        public static Color GlobalEdgeColor
        {
            get { return _globalEdgeColor; }
            set { _globalEdgeColor = value; }
        }

        public static float GlobalEdgeSize
        {
            get { return _globalEdgeSize; }
            set { _globalEdgeSize = value; }
        }

        public static Color GlobalFillColor
        {
            get { return _globalFillColor; }
            set { _globalFillColor = value; }
        }
        #endregion
    }
}
