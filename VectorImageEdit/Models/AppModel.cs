using System;
using System.Collections;
using System.Linq;
using VectorImageEdit.Modules.Layers;

namespace VectorImageEdit.Models
{
    // TODO: This is only used for object selection, move it to its own section
    class AppModel
    {
        public void LayerListSelect(string itemName)
        {
            if (string.IsNullOrEmpty(itemName) ||
                string.IsNullOrWhiteSpace(itemName)) return;
            try
            {
                LayerManager manager = AppGlobalData.Instance.LayerManager;
                Layer layer = manager.GetSortedLayers()
                    .First(item => item.DisplayName == itemName);
                manager.MouseHandler.SelectedLayer = layer;
            }
            catch (InvalidOperationException) { }
            catch (NullReferenceException) { }
        }

        public IList GetSortedLayers()
        {
            return AppGlobalData.Instance.LayerManager.GetSortedLayers();
        }
    }
}
