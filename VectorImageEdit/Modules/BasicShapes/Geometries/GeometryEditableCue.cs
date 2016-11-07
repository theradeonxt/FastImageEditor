using System.Collections.Generic;
using System.Drawing;

namespace VectorImageEdit.Modules.BasicShapes.Geometries
{
    /// <summary>
    /// Shows geometry items that are user-interactable
    /// </summary>
    public class GeometryEditableCue : IGeometryShape
    {
        public GeometryEditableCue()
        {
            Items = new List<GeometryItem>();
            Points = new List<GeometryPoint>();
        }

        /// <summary>
        /// Gets a list of editable geometry items currently registered.
        /// </summary>
        public List<GeometryItem> Items { get; private set; }

        /// <summary>
        /// Gets a list of editable geometry points currently registered.
        /// </summary>
        public List<GeometryPoint> Points { get; private set; }

        public void Add(GeometryPoint point)
        {
            Points.Add(point);
            Visible = visible;
        }

        public void Add(GeometryItem item)
        {
            Items.Add(item);
            Visible = visible;
        }

        public void AddRange(GeometryPoint[] points)
        {
            Points.AddRange(points);
            Visible = visible;
        }

        public void AddRange(GeometryItem[] items)
        {
            Items.AddRange(items);
            Visible = visible;
        }

        private bool visible;
        public bool Visible
        {
            get { return visible; }
            set
            {
                visible = value;
                for (int i = 0; i < Items.Count; i++) Items[i].Visible = value;
                for (int i = 0; i < Points.Count; i++) Points[i].Visible = value;
            }
        }

        public void DrawGeometry(Graphics destination)
        {
            if (!Visible) return;
            for (int i = 0; i < Items.Count; i++) Items[i].DrawGeometry(destination);
            for (int i = 0; i < Points.Count; i++) Points[i].DrawGeometry(destination);
        }
    }
}
