namespace VectorImageEdit.Modules.Layers
{
    public partial class LayerManager
    {
        public void BringToFront(Layer layer)
        {
            if (layer == null) return;

            int count = _activeLayers.Count;
            int last = count - 1;

            // no need for reordering if already on topmost level, or is the only existing layer
            if (count <= 1 || layer.DepthLevel > _activeLayers[last].DepthLevel) return;
            
            // sends the objects to the topmost level,
            // meaning the level after any other object
            layer.DepthLevel = _activeLayers[last].DepthLevel + 1;
            _activeLayers.Rebuild();
            if (_activeLayers.Contains(layer))
            {
                UpdateFrame(_activeLayers);
                UpdateSelection(layer.Region, ClearMode.NoClear);
            }

            // the layer remains unchanged
        }

        public void SendToBack(Layer layer)
        {
            if (layer == null) return;

            int count = _activeLayers.Count;

            // no need for reordering if already on backmost level, or is the only existing layer
            if (count > 1 && layer.DepthLevel >= _activeLayers[0].DepthLevel)
            {
                // sends the objects to the backmost level,
                // meaning the level before any other object
                layer.DepthLevel = _activeLayers[0].DepthLevel - 1;
                _activeLayers.Rebuild();
                UpdateFrame(_activeLayers);
                UpdateSelection(layer.Region, ClearMode.NoClear);
            }

            // the layer remains unchanged
        }

        public void SendBackwards(Layer layer)
        {
            if (layer == null) return;

            int count = _activeLayers.Count;

            // no need for reordering if it's the only existing layer
            if (count > 1)
            {
                // sends the objects to the previous level than it's currently on
                layer.DepthLevel = layer.DepthLevel - 1;
                _activeLayers.Rebuild();
                UpdateFrame(_activeLayers);
                UpdateSelection(layer.Region, ClearMode.NoClear);
            }

            // the layer remains unchanged
        }
    }
}
