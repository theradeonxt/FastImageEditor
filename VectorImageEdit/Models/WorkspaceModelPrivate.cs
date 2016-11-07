using System.Drawing;
using JetBrains.Annotations;
using VectorImageEdit.Modules.GraphicsCompositing;
using VectorImageEdit.Modules.LayerManagement;

namespace VectorImageEdit.Models
{
    partial class WorkspaceModel
    {
        private Layer selectedLayer;      // the single object selected with focus
        //private Point pointOffset;        // the mouse offset when used to drag objects
        //private Point pointDown;
        private MovementTracker MoveTracker { get; set; }

        private bool CanSelectLayer(Point location)
        {
            var layerCollection = AppModel.Instance.LayerManager.WorkspaceLayers;
            for (int index = layerCollection.Count - 1; index >= 0; index--)
            {
                if (layerCollection[index].Region.Contains(location)) return true;
            }
            return false;
        }

        private bool CanSelectLayer(Point location, out Layer selectable)
        {
            var layerCollection = AppModel.Instance.LayerManager.WorkspaceLayers;
            for (int index = layerCollection.Count - 1; index >= 0; index--)
            {
                if (layerCollection[index].Region.Contains(location))
                {
                    selectable = layerCollection[index];
                    return true;
                }
            }
            selectable = null;
            return false;
        }

        private bool IsNewSelection([NotNull]Layer layer)
        {
            // A new selection happens if we have a different layer than the one already selected
            return selectedLayer.Metadata.Uid != layer.Metadata.Uid;
        }

        private void DeselectLayers()
        {
            // TODO: Group deselection
            if (selectedLayer != StateHandler.DummySelected)
            {
                NotifyGraphicsHandler(RenderingPolicy.MinimalUpdatePolicy(selectedLayer.Region));
            }
            selectedLayer = StateHandler.DummySelected;
        }

        private void NotifyGraphicsHandler([NotNull]IRenderingPolicy policy)
        {
            var graphicsMan = AppModel.Instance.GraphicsManager;
            var layers = AppModel.Instance.LayerManager.WorkspaceLayers;
            graphicsMan.UpdateFrame(layers, policy);

            if (StateHandler.SelectedLayerState != LayerState.Normal)
            {

                //graphicsMan.UpdateSelection(selectedLayer.Region);
            }
        }
    }
}
