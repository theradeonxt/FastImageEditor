using VectorImageEdit.Modules.Interfaces;

namespace VectorImageEdit.Modules.LayerManagement.LayerModifiers
{
    /// <summary>
    /// Sends the layer to a level that is behind any other layer.
    /// </summary>
    class SendToBackModifier : LayerModifierBase, ILayerModifier
    {
        public SendToBackModifier(ILayerHandler manager, IGraphicsHandler graphics)
            : base(manager, graphics)
        {
        }

        public void ApplyModifier(Layer layer)
        {
            base.ApplyModifier(() =>
            {
                layer.DepthLevel = HandlerLayers.WorkspaceLayers.First.DepthLevel - 1;
                HandlerLayers.WorkspaceLayers.Rebuild();
            },
            RenderingPolicyFactory.MinimalUpdatePolicy(layer.Region),
            () => HandlerLayers.WorkspaceLayers.Contains(layer));
        }
    }
}
