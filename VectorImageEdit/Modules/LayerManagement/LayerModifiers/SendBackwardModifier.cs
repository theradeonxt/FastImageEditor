using VectorImageEdit.Modules.GraphicsCompositing;
using VectorImageEdit.Modules.Interfaces;

namespace VectorImageEdit.Modules.LayerManagement.LayerModifiers
{
    /// <summary>
    /// Sends the layer to the previous level than it's currently on.
    /// </summary>
    class SendBackwardModifier : LayerModifierBase, ILayerModifier
    {
        public SendBackwardModifier(ILayerHandler manager, IGraphicsHandler graphics)
            : base(manager, graphics)
        {
        }

        public void ApplyModifier(Layer layer)
        {
            base.ApplyModifier(() =>
            {
                layer.DepthLevel--;
                HandlerLayers.WorkspaceLayers.Rebuild();
            },
            RenderingPolicy.MinimalUpdatePolicy(layer.Region),
            () => HandlerLayers.WorkspaceLayers.Contains(layer));
        }
    }
}
