using System.Drawing;
using VectorImageEdit.Models;

namespace VectorImageEdit.Modules.BasicShapes.Geometries
{
    public enum GeometryItemType
    {
        Selection, // the item is used when part of a selection
        Cue        // the item is part of the geometry hint
    }

    /// <summary>
    /// Represents an element defined by multiple editable nodes for a layer.
    /// This can be viewed as a path represented by a number of arbitrary
    /// GeometryPoints, drawn in consecutive pairs.
    /// </summary>
    public class GeometryItem : IGeometryShape
    {
        public GeometryItem(GeometryPoint first, GeometryPoint second, GeometryItemType type)
        {
            Points = new[] { first, second };
            Type = type;
        }

        /// <summary>
        /// Gets the geometry points contained (always of length 2).
        /// </summary>
        public GeometryPoint[] Points { get; private set; }

        /// <summary>
        /// Gets how this item is represented.
        /// </summary>
        public GeometryItemType Type { get; private set; }

        private bool visible;
        public bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                visible = value;
                for (int i = 0; i < Points.Length; i++) Points[i].Visible = value;
            }
        }

        public void DrawGeometry(Graphics destination)
        {
            if (!Visible) return;

            Points[0].DrawGeometry(destination);
            Points[1].DrawGeometry(destination);

            Pen pen = null;
            if (Type == GeometryItemType.Selection)
            {
                pen = AppModel.Instance.LayerSelectionPen;
            }
            else if (Type == GeometryItemType.Cue)
            {
                pen = AppModel.Instance.GeometryItemPen;
            }

            // ReSharper disable once AssignNullToNotNullAttribute
            destination.DrawLine(pen, Points[0], Points[1]);
        }
    }
}
