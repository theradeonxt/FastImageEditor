using System.Drawing;
using System.Linq;
using JetBrains.Annotations;
using VectorImageEdit.Modules.Interfaces;
using VectorImageEdit.Modules.LayerManagement;

namespace VectorImageEdit.Models
{
    partial class WorkspaceModel
    {
        private Layer _selectedLayer;      // the single object selected with focus
        private readonly Layer _noLayer;   // marker for an unspecified layer
        private Point _pointOffset;        // the mouse offset when used to drag objects
        private Point _pointDown;

        [NotNull]
        private Layer FindSelectable(Point location)
        {
            var layers = AppModel.Instance.LayerManager.WorkspaceLayers;
            return layers.Last(item => item.Region.Contains(location));
        }
        private bool CanSelectLayer(Point location)
        {
            var layers = AppModel.Instance.LayerManager.WorkspaceLayers;
            return layers.Any(item => item.Region.Contains(location));
        }
        private bool IsNewSelection([NotNull]Layer layer)
        {
            // A new selection happens if we have a different layer than the one already selected
            return _selectedLayer.Metadata.Uid != layer.Metadata.Uid;
        }
        private void DeselectLayers()
        {
            if (_selectedLayer != _noLayer)
            {
                NotifyGraphicsHandler(RenderingPolicyFactory.MinimalUpdatePolicy(_selectedLayer.Region));
            }
            _selectedLayer = _noLayer;
        }
        private void NotifyGraphicsHandler(IRenderingPolicy policy)
        {
            // TODO: Run a task here to free the UI thread
            var graphicsMan = AppModel.Instance.GraphicsManager;
            var layers = AppModel.Instance.LayerManager.WorkspaceLayers;
            graphicsMan.UpdateFrame(layers, policy);
        }
    }
}
