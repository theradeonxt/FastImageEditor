using System.Drawing;
using System.Windows.Forms;
using VectorImageEdit.Modules.Utility;

namespace VectorImageEdit.Modules.Layers
{
    // TODO: refactor the 3 events into MVC model: WorkspaceModel
    /// <summary>
    /// 
    /// MouseInteraction Module
    /// 
    /// - implements user interaction functionality with layers
    /// - provides object selection/deselection 
    /// 
    /// </summary>
    public class MouseInteraction
    {
        private enum LayerState
        {
            Moving,     // specifies if an object will be moving
            Resizing,   // specifies if an object will be resizing
            Normal      // no special considerations
        };

        private LayerState _currentState;
        private readonly SortedContainer<Layer> _sortedCollection;
        private readonly ObjectModifiedEvent _onObjectModified;
        private Layer _selectedLayer;                    // the single object selected with focus
        private Point _pointOffset;                      // the mouse offset when used to drag objects
        private Point _pointDown;

        // Callback method when an object has changed to trigger some update if needed
        public delegate void ObjectModifiedEvent(Rectangle objectRegion, ClearMode mode);

        public MouseInteraction(SortedContainer<Layer> sortedCollection, ObjectModifiedEvent onObjectModified)
        {
            _sortedCollection = sortedCollection;
            _onObjectModified = onObjectModified;

            _selectedLayer = null;
            _currentState = LayerState.Normal;
        }

        public void MouseMovement(object sender, MouseEventArgs e)
        {
            if (_selectedLayer == null)
            {
                return;
            }

            switch (_currentState)
            {
                case LayerState.Moving:
                    {
                        Point newPoint = new Point
                        {
                            X = e.Location.X - _pointOffset.X,
                            Y = e.Location.Y - _pointOffset.Y
                        };

                        _selectedLayer.Move(newPoint);
                        _onObjectModified(_selectedLayer.Region, ClearMode.UpdateOld);

                        break;
                    }
                case LayerState.Resizing:
                    {
                        Size newSize = new Size
                        {
                            Width = (e.Location.X - _pointDown.X),
                            Height = (e.Location.Y - _pointDown.Y)
                        };

                        _selectedLayer.Resize(newSize);
                        _onObjectModified(_selectedLayer.Region, ClearMode.UpdateOld);
                        break;
                    }
            }
        }

        public void MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            // Keep track of initial mouse down
            _pointDown = e.Location;

            // Find layer at mouse location
            Layer layer;
            if (!FindLayerAt(e.Location, out layer))
            {
                // No selectable layer, deselect all layers
                DeselectLayers();
                return;
            }

            // The exterior region is 4 pixels inside from the layer edges 
            Rectangle resizeInterior = layer.Region;
            resizeInterior.Inflate(-4, -4);
            _currentState = (resizeInterior.Contains(e.Location) == false) ? LayerState.Resizing : LayerState.Moving;
            if (_currentState == LayerState.Resizing)
            {
                // Resize begin
                Cursor.Current = Cursors.SizeAll;
            }
            else
            {
                // Movement begin
                _pointOffset.X = _pointDown.X - layer.Region.Left;
                _pointOffset.Y = _pointDown.Y - layer.Region.Top;

                Cursor.Current = Cursors.Hand;
            }

            // Mark the object as selected
            SelectedLayer = layer;
        }

        public void MouseUp(object sender, MouseEventArgs e)
        {
            if (_currentState == LayerState.Moving
                || _currentState == LayerState.Resizing)
            {
                // The layer is finished updating so update the graphics
                _onObjectModified(_selectedLayer.Region, ClearMode.FullUpdate);

                Cursor.Current = Cursors.Default;
            }
            _currentState = LayerState.Normal;
        }

        private void DeselectLayers()
        {
            //onObjectModified(null, ClearMode.FullUpdate);
            _selectedLayer = null;
        }

        public Layer SelectedLayer
        {
            get { return _selectedLayer; }
            set
            {
                _selectedLayer = value;
                _onObjectModified(_selectedLayer.Region, ClearMode.UpdateOld);
            }
        }

        private bool FindLayerAt(Point location, out Layer layer)
        {
            // Finds an object which contains a given location
            // If an objects is found, it is guaranteed to be the topmost one (using a sorted list)
            int count = _sortedCollection.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                if (_sortedCollection[i].Region.Contains(location))
                {
                    layer = _sortedCollection[i];
                    return true;
                }
            }
            layer = null;
            return false;
        }
    }
}
