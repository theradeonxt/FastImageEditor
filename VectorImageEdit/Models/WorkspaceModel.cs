using System.Collections.Generic;
using System.Drawing;
using VectorImageEdit.Modules.BasicShapes;
using VectorImageEdit.Modules.BasicShapes.Geometries;
using VectorImageEdit.Modules.GraphicsCompositing;
using VectorImageEdit.Modules.LayerManagement;
using VectorImageEdit.WindowsFormsBridge;

namespace VectorImageEdit.Models
{
    public enum LayerState
    {
        Moving,         // specifies if an object will be moving
        Resizing,       // specifies if an object will be resizing
        Normal,         // no special considerations
        Selectable,     // hover function
        GeometryAction  // affects a geometry point
    };

    class MovementTracker
    {
        public MovementTracker()
        {

        }

        public void BeginTracking(Point location, Point objectOffset)
        {
            PointDown = location;
            PointOffset = PointDown;
            PointOffset.Offset(-objectOffset.X, -objectOffset.Y);
        }

        public Point PointOffset { get; private set; } // the mouse offset when used to drag objects
        public Point PointDown { get; private set; }   // original location where movement began

        public Point GetDelta(Point location)
        {
            Point result = location;
            result.Offset(-PointOffset.X, -PointOffset.Y);
            return result;
        }
    }

    class StateHandler
    {
        public StateHandler()
        {
            DummySelected = new Picture(new Bitmap(1, 1), Rectangle.Empty, 0);

            SelectedLayer = DummySelected;
            SelectedLayerState = LayerState.Normal;
            SelectionGroup = new List<Layer>();
        }

        public LayerState SelectedLayerState { get; set; }

        public Layer SelectedLayer { get; set; }

        public Layer DummySelected { get; set; }

        public GeometryPoint EditingPoint { get; set; }

        public List<Layer> SelectionGroup { get; private set; }

        public bool IsMultipleSelection()
        {
            return SelectionGroup.Count > 1;
        }

        public void ResetState()
        {
            SelectedLayerState = LayerState.Normal;
        }
    }

    partial class WorkspaceModel
    {
        public StateHandler StateHandler { get; private set; }

        public WorkspaceModel()
        {
            StateHandler = new StateHandler();
            MoveTracker = new MovementTracker();
        }

        public Layer SelectedLayer
        {
            get { return selectedLayer; }

            set
            {
                //if (!IsNewSelection(value)) return;

                if (selectedLayer != StateHandler.DummySelected &&
                    selectedLayer.Metadata.Uid == value.Metadata.Uid) return;

                selectedLayer = value;

                NotifyGraphicsHandler(RenderingPolicy.MinimalUpdatePolicy(selectedLayer.Region));
            }
        }

        public LayerState MouseMovement(MyMouseEventArgs e)
        {
            LayerState result = LayerState.Normal;

            bool canSelect = CanSelectLayer(e.Location);
            if (canSelect)
            {
                result = LayerState.Selectable;
            }

            if (selectedLayer == StateHandler.DummySelected &&
                canSelect == false)
            {
                return LayerState.Normal;
            }

            switch (StateHandler.SelectedLayerState)
            {
                case LayerState.Moving:
                    {
                        Rectangle oldRegion = selectedLayer.Region;

                        selectedLayer.Move(MoveTracker.GetDelta(e.Location));

                        // 2-step update: first on old region and then on the new one 
                        //NotifyGraphicsHandler(RenderingPolicy.MinimalUpdatePolicy(oldRegion));
                        NotifyGraphicsHandler(RenderingPolicy.MinimalUpdatePolicy(selectedLayer.Region, oldRegion));

                        break;
                    }
                case LayerState.Resizing:
                    {
                        Rectangle oldRegion = selectedLayer.Region;

                        selectedLayer.Resize((Size)MoveTracker.GetDelta(e.Location));

                        // Update the largest invalidated region
                        Rectangle invalidatedRegion = oldRegion.Contains(selectedLayer.Region)
                            ? oldRegion
                            : selectedLayer.Region;
                        NotifyGraphicsHandler(RenderingPolicy.MinimalUpdatePolicy(invalidatedRegion));

                        break;
                    }
            }

            return result;
        }

        public void MouseDown(MyMouseEventArgs e)
        {
            if (e.Button != MyMouseEventArgs.MyMouseButton.Left) return;

            // Deselect all layers
            if (CanSelectLayer(e.Location) == false)
            {
                DeselectLayers();
                return;
            }

            // Keep track of initial mouse down
            //pointDown = e.Location;

            // Selection test at the mouse position
            Layer layer;
            if (CanSelectLayer(e.Location, out layer) == false) return;

            // The exterior region is 4 pixels inside from the layer Edges 
            Rectangle resizeInterior = layer.Region;
            resizeInterior.Inflate(-4, -4);
            StateHandler.SelectedLayerState = resizeInterior.Contains(e.Location) ? LayerState.Moving : LayerState.Resizing;

            // Logic to respond to the state change
            if (StateHandler.SelectedLayerState == LayerState.Moving)
            {
                // Movement begin
                MoveTracker.BeginTracking(e.Location, layer.Region.Location);
                //pointOffset.X = pointDown.X - layer.Region.Left;
                //pointOffset.Y = pointDown.Y - layer.Region.Top;
            }

            SelectedLayer = layer;
        }

        public void MouseUp(MyMouseEventArgs e)
        {
            if (StateHandler.SelectedLayerState != LayerState.Normal)
            {
                NotifyGraphicsHandler(RenderingPolicy.MinimalUpdatePolicy(selectedLayer.Region));
                StateHandler.SelectedLayerState = LayerState.Normal;
            }
        }
    }
}
