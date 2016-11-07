using System.Drawing;
using JetBrains.Annotations;
using VectorImageEdit.Modules.BasicShapes.Geometries;

namespace VectorImageEdit.Modules.Utility
{
    static class PointPacker
    {
        /// <summary>
        /// Prepares a layer's geometry points for transforming. 
        /// Aggregates the layer's bounding and editable geometry points into the given array of coordinates.
        /// 
        /// Note: If geometryPoints is null, it will be allocated with enough elements to hold all the layer's geometry points. 
        /// </summary>
        /// <param name="geometryPoints"> A reference to an array which will store the points </param>
        /// <param name="editablegeometryCue"> The layer's editable geometry </param>
        /// <param name="boundingGeometryCue"> The layer's bounding geometry </param>
        public static void Pack(
            [CanBeNull] ref Point[]         geometryPoints,
            [CanBeNull] GeometryEditableCue editablegeometryCue,
            [CanBeNull] GeometryBoundsCue   boundingGeometryCue)
        {
            // allocate points if needed
            if (geometryPoints == null)
            {
                geometryPoints = new Point[(editablegeometryCue == null ? 0 : editablegeometryCue.Points.Count) +
                                           (editablegeometryCue == null ? 0 : editablegeometryCue.Items.Count * 2) +
                                           (boundingGeometryCue == null ? 0 : boundingGeometryCue.Items.Length * 2)];
            }

            int pointCount = 0;
            if (editablegeometryCue != null)
            {
                for (int i = 0; i < editablegeometryCue.Points.Count; i++) geometryPoints[pointCount++] = editablegeometryCue.Points[i];
                for (int i = 0; i < editablegeometryCue.Items.Count; i++)
                {
                    geometryPoints[pointCount++] = editablegeometryCue.Items[i].Points[0];
                    geometryPoints[pointCount++] = editablegeometryCue.Items[i].Points[1];
                }
            }
            if (boundingGeometryCue != null)
            {
                for (int i = 0; i < boundingGeometryCue.Items.Length; i++)
                {
                    geometryPoints[pointCount++] = boundingGeometryCue.Items[i].Points[0];
                    geometryPoints[pointCount++] = boundingGeometryCue.Items[i].Points[1];
                }
            }
        }

        /// <summary>
        /// Copies the layer geometry points after a transformation.
        /// Modifies the editable and bounding geometries with the new point coordinates received.
        /// </summary>
        /// <param name="geometryPoints"> An array containing the points </param>
        /// <param name="editablegeometryCue"> The layer's editable geometry that will be modified </param>
        /// <param name="boundingGeometryCue"> The layer's bounding geometry that will be modified </param>
        public static void Unpack(
            [NotNull]   Point[]             geometryPoints,
            [CanBeNull] GeometryEditableCue editablegeometryCue,
            [CanBeNull] GeometryBoundsCue   boundingGeometryCue)
        {
            int pointCount = 0;
            if (editablegeometryCue != null)
            {

                for (int i = 0; i < editablegeometryCue.Points.Count; i++) editablegeometryCue.Points[i].Move(geometryPoints[pointCount++]);
                for (int i = 0; i < editablegeometryCue.Items.Count; i++)
                {
                    editablegeometryCue.Items[i].Points[0].Move(geometryPoints[pointCount++]);
                    editablegeometryCue.Items[i].Points[1].Move(geometryPoints[pointCount++]);
                }
            }
            if (boundingGeometryCue != null)
            {
                for (int i = 0; i < boundingGeometryCue.Items.Length; i++)
                {
                    boundingGeometryCue.Items[i].Points[0].Move(geometryPoints[pointCount++]);
                    boundingGeometryCue.Items[i].Points[1].Move(geometryPoints[pointCount++]);
                }
            }
        }
    }
}
