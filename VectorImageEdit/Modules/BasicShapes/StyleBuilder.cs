using System.Drawing;
using System.Drawing.Drawing2D;
using VectorImageEdit.Models;

namespace VectorImageEdit.Modules.BasicShapes
{
    static class StyleBuilder
    {
        /// <summary>
        /// Obtains a style object using properties defined globally for the application
        /// </summary>
        /// <returns></returns>
        public static ShapeStyle FromGlobalStyles()
        {
            var style = new ShapeStyle
            {
                EdgeColor = AppGlobalData.Instance.ShapeEdgeColor,
                EdgeSize = AppGlobalData.Instance.ShapeEdgeSize,
                FillColor = AppGlobalData.Instance.ShapeFillColor
            };
            return style;
        }

        /// <summary>
        /// Obtains a new Brush from the style parameters
        /// Note: use this in a using construct to auto-dispose!
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        public static Brush BrushBuilder(ShapeStyle style)
        {
            return new HatchBrush(HatchStyle.Cross, style.FillColor, style.FillColor);
        }

        /// <summary>
        /// Obtains a new Pen from the style parameters
        /// Note: use this in a using construct to auto-dispose!
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        public static Pen PenBuilder(ShapeStyle style)
        {
            return new Pen(style.EdgeColor, style.EdgeSize);
        }
    }
}
