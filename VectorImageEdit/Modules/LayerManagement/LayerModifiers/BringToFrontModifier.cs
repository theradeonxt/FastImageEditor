using VectorImageEdit.Modules.Interfaces;

namespace VectorImageEdit.Modules.LayerManagement.LayerModifiers
{
    /// <summary>
    /// Sends the layer to a level on top of any other layer.
    /// </summary>
    class BringToFrontModifier : LayerModifierBase, ILayerModifier
    {
        public BringToFrontModifier(ILayerHandler manager, IGraphicsHandler graphics)
            : base(manager, graphics)
        {
        }

        public void ApplyModifier(Layer layer)
        {
            base.ApplyModifier(() =>
            {
                layer.DepthLevel = HandlerLayers.WorkspaceLayers.Last.DepthLevel + 1;
                HandlerLayers.WorkspaceLayers.Rebuild();
            },
            RenderingPolicyFactory.MinimalUpdatePolicy(layer.Region),
            () => HandlerLayers.WorkspaceLayers.Contains(layer));
        }
    }
}
