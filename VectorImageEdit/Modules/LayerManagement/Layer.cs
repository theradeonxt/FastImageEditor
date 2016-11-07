using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;
using VectorImageEdit.Modules.BasicShapes.Geometries;
using VectorImageEdit.Modules.Utility;

namespace VectorImageEdit.Modules.LayerManagement
{
    /// <summary>
    /// 
    /// Layer Module
    ///
    /// - represents the basic drawable scene graphical object
    /// 
    /// </summary>
    [Serializable]
    public abstract class Layer : IComparable<Layer>, IDisposable
    {
        protected Layer(Rectangle region, int depthLevel, [NotNull] string displayName)
        {
            this.region = region;
            DepthLevel = depthLevel;
            Transparency = 100;

            Metadata = new LayerMetadata(displayName);

            boundingGeometryCue = new GeometryBoundsCue(region) { Visible = true };
            editablegeometryCue = new GeometryEditableCue { Visible = true };
        }

        #region Properties

        [NonSerialized, NotNull]
        private GeometryBoundsCue boundingGeometryCue;
        /// <summary>
        /// Gets a bounding geometry for this layer, usually defined by its 4 corners.
        /// </summary>
        public GeometryBoundsCue BoundingGeometryCue
        {
            get { return boundingGeometryCue; }
            private set { boundingGeometryCue = value; }
        }

        [NonSerialized, NotNull]
        private GeometryEditableCue editablegeometryCue;
        /// <summary>
        /// Gets the geometry items that influence the layer's appearance.
        /// These are nodes interactable by the user.
        /// </summary>
        public GeometryEditableCue EditablegeometryCue
        {
            get { return editablegeometryCue; }
            private set { editablegeometryCue = value; }
        }

        private Rectangle region;
        /// <summary>
        /// Gets the scene virtual bounding region occupied by this layer.
        /// </summary>
        public Rectangle Region
        {
            get { return region; }
            private set
            {
                region = value;
                OnPropertyChanged();
            }
        }

        private int depthLevel;
        /// <summary>
        /// Gets or sets the layer depth appearance in the scene.
        /// A higher depth means the layer is shown closer to the front.
        /// </summary>
        public int DepthLevel
        {
            get { return depthLevel; }
            set
            {
                depthLevel = value;
                OnPropertyChanged();
            }
        }

        private int transparency;
        /// <summary>
        /// Gets or sets the opacity level of the layer.
        /// Valid values are between [0-100]. 0 means fully transparent, and 100 fully opaque.
        /// </summary>
        public int Transparency
        {
            get { return transparency; }
            set
            {
                transparency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets an object describing the layer metadata properties.
        /// </summary>
        public LayerMetadata Metadata { get; private set; }

        /// <summary>
        /// Gets or sets a flag indicating that one or more layer properties have changed
        /// and that it needs to be redrawn.
        /// Note: this should not be changed directly, it's handled automatically
        /// and is public only for visibility purposes.
        /// </summary>
        public bool HasChanged { get; set; }

        #endregion

        #region Interfaces/Abstract Methods

        /// <summary>
        /// Compares this layer to a given layer based on their relative depth ordering.
        /// </summary>
        int IComparable<Layer>.CompareTo(Layer other)
        {
            return DepthLevel.CompareTo(other.DepthLevel);
        }

        /// <summary>
        /// Releases all resources used by this layer
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// Called when this layer should be repainted.
        /// </summary>
        /// <param name="destination"> The graphics object where drawing will occur </param>
        public abstract void DrawGraphics(Graphics destination);

        #endregion

        #region Notifications

        /// <summary>
        /// Marks this object as in need for graphical update.
        /// </summary>
        protected void OnPropertyChanged()
        {
            HasChanged = true;
        }

        private enum GeometryTransformType { Editable, Bounding, All }

        [NonSerialized, CanBeNull]
        private Point[] geometryArray; // temporary storage for geometry points (allocated when needed, once)

        /// <summary>
        /// Applies the given transformation matrix to the layer's geometries.
        /// Note: The matrix describes a 2D transform either in 2x2 or extended 3x3 form.
        /// </summary>
        /// <param name="mat"> The transformation matrix </param>
        /// <param name="type"> A filter specifying which layer geometries are affected </param>
        private void TransformGeometry(Matrix mat, GeometryTransformType type = GeometryTransformType.All)
        {
            PointPacker.Pack(ref geometryArray,
                type == GeometryTransformType.Bounding ? null : editablegeometryCue,
                type == GeometryTransformType.Editable ? null : boundingGeometryCue);

            mat.TransformPoints(geometryArray);

            PointPacker.Unpack(geometryArray,
                type == GeometryTransformType.Bounding ? null : editablegeometryCue,
                type == GeometryTransformType.Editable ? null : boundingGeometryCue);
        }

        /// <summary>
        /// Called before a translation operation occurs to recalculate the layer geometry.
        /// </summary>
        /// <param name="moveLocation"> The new top-left location </param>
        private void OnMovement(Point moveLocation)
        {
            Point diff = new Point(moveLocation.X - Region.X, moveLocation.Y - Region.Y);

            // create translation matrix
            Matrix mat = new Matrix();
            mat.Translate(diff.X, diff.Y, MatrixOrder.Append);

            TransformGeometry(mat);
        }

        /// <summary>
        /// Called before a rotation operation occurs to recalculate the layer geometry.
        /// </summary>
        /// <param name="angle"> The rotation angle in degrees, around the layer's center point. </param>
        private void OnRotation(float angle)
        {
            Point center = new Point(region.X + region.Width / 2, region.Y + region.Height / 2);

            // create transform matrix
            Matrix mat = new Matrix();
            // translate to origin
            mat.Translate(-center.X, -center.Y, MatrixOrder.Append);
            // rotation around origin
            mat.Rotate(angle, MatrixOrder.Append);
            // translate back
            mat.Translate(center.X, center.Y, MatrixOrder.Append);

            TransformGeometry(mat);
        }

        /// <summary>
        /// Called before a scaling or resize operation occurs to recalculate the layer's geometry.
        /// </summary>
        /// <param name="newSize"> The new layer size </param>
        private void OnScaling(Size newSize)
        {
            SizeF percentage = new SizeF(newSize.Width - region.Width, newSize.Height - region.Height);

            // create scaling matrix
            Matrix mat = new Matrix();
            mat.Scale(percentage.Width, percentage.Height, MatrixOrder.Append);

            TransformGeometry(mat);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Moves the top-left corner of the layer to a given location.
        /// </summary>
        public void Move(Point newLocation)
        {
            OnMovement(newLocation);
            region.Location = newLocation;
            OnPropertyChanged();
        }

        /// <summary>
        /// Resizes the layer's bounding region to the given Size. 
        /// </summary>
        public void Resize(Size newSize)
        {
            // TODO: Minimum size
            // Debug.Assert(newSize.Width >= 0 && newSize.Height >= 0, "Layer resized with invalid properties");
            OnScaling(newSize);
            region.Size = newSize;
            OnPropertyChanged();
        }

        /// <summary>
        /// Enables only the given geometry to be visible for this layer.
        /// If GeometryState.Disabled is given, all the layer geometry is hidden.
        /// </summary>
        /// <param name="state"> A value of type GeometryState enum </param>
        public void SetGeometryState(GeometryState state)
        {
            switch (state)
            {
                case GeometryState.All:
                    boundingGeometryCue.Visible = true;
                    editablegeometryCue.Visible = true;
                    break;
                case GeometryState.Bounds:
                    boundingGeometryCue.Visible = true;
                    editablegeometryCue.Visible = false;
                    break;
                case GeometryState.Editable:
                    editablegeometryCue.Visible = true;
                    boundingGeometryCue.Visible = false;
                    break;
                case GeometryState.Disabled:
                    editablegeometryCue.Visible = false;
                    boundingGeometryCue.Visible = false;
                    break;
            }
            OnPropertyChanged();
        }

        #endregion
    }
}
