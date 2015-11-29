using System.Drawing;
using System.Windows.Forms; // TODO: Fix
using VectorImageEdit.Modules.Interfaces;
using VectorImageEdit.Modules.LayerManagement;
using VectorImageEdit.WindowsFormsBridge;

namespace VectorImageEdit.Models
{
    partial class WorkspaceModel
    {
        public WorkspaceModel()
        {
            _noLayer = new Picture(new Bitmap(1, 1), Rectangle.Empty, 0);

            _selectedLayer = _noLayer;
            _currentState = LayerState.Normal;
        }

        public Layer SelectedLayer
        {
            get { return _selectedLayer; }
            set
            {
                //if (!IsNewSelection(value)) return;

                if (_selectedLayer != _noLayer &&
                    _selectedLayer.Metadata.Uid == value.Metadata.Uid) return;

                _selectedLayer = value;

                NotifyGraphicsHandler(RenderingPolicyFactory.MinimalUpdatePolicy(_selectedLayer.Region));
            }
        }

        public void MouseMovement(object sender, MyMouseEventArgs e)
        {
            if (_selectedLayer == _noLayer) return;

            switch (_currentState)
            {
                case LayerState.Moving:
                    {
                        Rectangle oldRegion = _selectedLayer.Region;
                        _selectedLayer.Move(new Point(e.X - _pointOffset.X, e.Y - _pointOffset.Y));

                        // 2-step update: first on old region and then on the new one 
                        NotifyGraphicsHandler(RenderingPolicyFactory.MinimalUpdatePolicy(oldRegion));
                        NotifyGraphicsHandler(RenderingPolicyFactory.MinimalUpdatePolicy(_selectedLayer.Region));

                        break;
                    }
                case LayerState.Resizing:
                    {
                        Rectangle oldRegion = _selectedLayer.Region;
                        _selectedLayer.Resize(new Size(e.X - _pointDown.X, e.Y - _pointDown.Y));

                        // Update the largest invalidated region
                        Rectangle invalidatedRegion = oldRegion.Contains(_selectedLayer.Region)
                            ? oldRegion
                            : _selectedLayer.Region;
                        NotifyGraphicsHandler(RenderingPolicyFactory.MinimalUpdatePolicy(invalidatedRegion));

                        break;
                    }
                case LayerState.Normal:
                    break;
            }
        }
        public void MouseDown(object sender, MyMouseEventArgs e)
        {
            if (e.Button != MyMouseEventArgs.MyMouseButton.Left) return;
            if (CanSelectLayer(e.Location) == false)
            {
                DeselectLayers();
                return;
            }

            // Keep track of initial mouse down
            _pointDown = e.Location;

            Layer layer = FindSelectable(_pointDown);

            // The exterior region is 4 pixels inside from the layer edges 
            Rectangle resizeInterior = layer.Region;
            resizeInterior.Inflate(-4, -4);
            _currentState = resizeInterior.Contains(e.Location) ? LayerState.Moving : LayerState.Resizing;
            
            // Logic to respond to the state 
            switch (_currentState)
            {
                case LayerState.Resizing:
                    // Resize begin
                    // TODO: Remove direct dependency; move Cursor to Controller
                    Cursor.Current = Cursors.SizeAll;
                    break;
                case LayerState.Moving:
                    // Movement begin
                    _pointOffset.X = _pointDown.X - layer.Region.Left;
                    _pointOffset.Y = _pointDown.Y - layer.Region.Top;
                    Cursor.Current = Cursors.Hand;
                    break;
            }
            SelectedLayer = layer;
        }
        public void MouseUp(object sender, MyMouseEventArgs e)
        {
            if (_currentState != LayerState.Normal)
            {
                Cursor.Current = Cursors.Default;

                NotifyGraphicsHandler(RenderingPolicyFactory.MinimalUpdatePolicy(_selectedLayer.Region));
            }
            _currentState = LayerState.Normal;
        }
    }
}
