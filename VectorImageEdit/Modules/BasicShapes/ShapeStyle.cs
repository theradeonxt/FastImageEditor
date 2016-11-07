using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace VectorImageEdit.Modules.BasicShapes
{
    [Serializable]
    public class ShapeStyle
    {
        public Color EdgeColor { get; set; }
        public Color FillColor { get; set; }
        public float EdgeSize { get; set; }
        public DashStyle LineDash { get; set; }

        /// <summary>
        /// Obtains a new Brush from the style parameters
        /// Note: use this in a using construct to auto-dispose!
        /// </summary>
        /// <returns> The new Brush </returns>
        public Brush CreateBrush()
        {
            return new HatchBrush(HatchStyle.Cross, FillColor, FillColor);
        }

        /// <summary>
        // Obtains a new Pen from this ShapeStyle instance
        /// Note: use this in a using construct to auto-dispose!
        /// </summary>
        /// <returns> The new Pen </returns>
        public Pen CreatePen()
        {
            return new Pen(EdgeColor, EdgeSize) { DashStyle = LineDash };
        }
    }
}
