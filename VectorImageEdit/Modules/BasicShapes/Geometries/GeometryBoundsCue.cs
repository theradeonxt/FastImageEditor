using System.Drawing;

namespace VectorImageEdit.Modules.BasicShapes.Geometries
{
    /// <summary>
    /// Shows a geometry cue for a layer's bounding region.
    /// This is usually a bounding rectangle around the layer,
    /// defined by the 4 corners of the layer's region.
    /// </summary>
    public class GeometryBoundsCue : IGeometryShape
    {
        public GeometryBoundsCue(Rectangle region)
        {
            ConstructFrom(region);
        }

        /// <summary>
        /// Gets a GeometryItem array that bounds the layer starting from top-left point,
        /// and in clockwise order.
        /// </summary>
        public GeometryItem[] Items { get; private set; }

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
                for (int i = 0; i < Items.Length; i++) Items[i].Visible = true;
            }
        }

        public void DrawGeometry(Graphics destination)
        {
            if (!Visible) return;
            for (int i = 0; i < Items.Length; i++) Items[i].DrawGeometry(destination);
        }

        /// <summary>
        /// Reconstructs the bounding geometry cue from the given region
        /// </summary>
        public void ConstructFrom(Rectangle region)
        {
            if (Items == null)
            {
                Items = new GeometryItem[4];
                // top
                GeometryPoint gp1 = new GeometryPoint(region.Location, GeometryPointType.ResizableNWSE);
                GeometryPoint gp2 = new GeometryPoint(new Point(region.Right, region.Top), GeometryPointType.ResizableNESW);
                Items[0] = new GeometryItem(gp1, gp2, GeometryItemType.Selection);

                // right
                GeometryPoint gp3 = new GeometryPoint(new Point(region.Right, region.Bottom), GeometryPointType.ResizableNWSE);
                Items[1] = new GeometryItem(gp2, gp3, GeometryItemType.Selection);

                // bottom
                GeometryPoint gp4 = new GeometryPoint(new Point(region.Left, region.Bottom), GeometryPointType.ResizableNESW);
                Items[2] = new GeometryItem(gp4, gp3, GeometryItemType.Selection);

                // left
                Items[3] = new GeometryItem(gp1, gp4, GeometryItemType.Selection);

                Visible = visible;
            }
            else
            {
                Items[0].Points[0].Move(region.Location);
                Items[1].Points[0].Move(new Point(region.Right, region.Top));
                Items[2].Points[1].Move(new Point(region.Right, region.Bottom));
                Items[3].Points[1].Move(new Point(region.Left, region.Bottom));
            }
        }
    }
}
