using System;
using System.Drawing;
using System.Windows.Forms;
using VectorImageEdit.Models;

namespace VectorImageEdit.Modules.BasicShapes.Geometries
{
    public enum GeometryPointType
    {
        Movable,
        ResizableNS, ResizableWE, ResizableNESW, ResizableNWSE,
        Rotatable,
    }

    /// <summary>
    /// Represents an editable node for a layer.
    /// </summary>
    [Serializable]
    public class GeometryPoint : IGeometryShape
    {
        public GeometryPoint(Point location, GeometryPointType type)
        {
            // center
            Center = location;
            TopLeft = Center;
            TopLeft.Offset(-Size.Width / 2, -Size.Height / 2);

            Type = type;
        }

        // TODO
        public void Focus(bool state)
        {
            if (state == false)
            {
                Cursor.Current = Cursors.Default;
                return;
            }

            switch (Type)
            {
                case GeometryPointType.Movable:
                    Cursor.Current = Cursors.SizeAll;
                    break;
                case GeometryPointType.ResizableNS:
                    Cursor.Current = Cursors.SizeNS;
                    break;
                case GeometryPointType.ResizableWE:
                    Cursor.Current = Cursors.SizeWE;
                    break;
                case GeometryPointType.ResizableNESW:
                    Cursor.Current = Cursors.SizeNESW;
                    break;
                case GeometryPointType.ResizableNWSE:
                    Cursor.Current = Cursors.SizeNWSE;
                    break;
                case GeometryPointType.Rotatable:
                    Cursor.Current = Cursors.PanNW;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static readonly Size Size = new Size(10, 10);

        public Point TopLeft { get; private set; }

        public Point Center { get; set; }

        public GeometryPointType Type { get; set; }

        public bool Visible { get; set; }

        public void DrawGeometry(Graphics destination)
        {
            if (!Visible) return;

            var pen = AppModel.Instance.GeometryItemPen;
            destination.DrawRectangle(pen, TopLeft.X, TopLeft.Y, Size.Width, Size.Height);
        }

        public void Offset(Point by)
        {
            Center.Offset(by);
            TopLeft.Offset(by);
        }

        public void Move(Point location)
        {
            Center = location;
            TopLeft = location;
            TopLeft.Offset(-Size.Width / 2, -Size.Height / 2);
        }

        public static implicit operator Point(GeometryPoint p)
        {
            return p.Center;
        }
    }
}
