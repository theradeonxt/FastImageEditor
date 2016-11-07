using System.Drawing;

namespace VectorImageEdit.Modules.BasicShapes.Geometries
{
    public enum GeometryState
    {
        Disabled, // no geometry is shown
        All,      // all geometries are shown
        Bounds,   // only bounding and/or selection geometry is shown
        Editable  // only editable geometry is shown
    }

    public interface IGeometryShape
    {
        /// <summary>
        /// Gets or sets whether this geometry shape is shown or hidden
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// Draws this geometry on the given Graphics object
        /// </summary>
        void DrawGeometry(Graphics destination);
    }
}
