using System;
using System.Collections;
using System.Linq;
using VectorImageEdit.Modules.Layers;

namespace VectorImageEdit.Models
{
    class AppModel
    {
        public void LayerListSelect(string itemName)
        {
            if (string.IsNullOrEmpty(itemName) ||
                string.IsNullOrWhiteSpace(itemName)) return;
            try
            {
                LayerManager manager = GlobalModel.Instance.LayerManager;
                Layer layer = manager.GetSortedLayers()
                    .First(item => item.DisplayName == itemName);
                manager.MouseHandler.SelectedLayer = layer;
            }
            catch (InvalidOperationException) { }
        }

        public IList GetSortedLayers()
        {
            return GlobalModel.Instance.LayerManager.GetSortedLayers();
        }
    }
}
