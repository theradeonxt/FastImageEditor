using System;
using System.Linq;
using VectorImageEdit.Modules.Layers;

namespace VectorImageEdit.Models
{
    class SceneTreeModel
    {
        public void SelectSceneTreeItem(string itemName)
        {
            if (string.IsNullOrEmpty(itemName) ||
                string.IsNullOrWhiteSpace(itemName)) return;
            try
            {
                LayerManager manager = AppGlobalData.Instance.LayerManager;
                Layer layer = manager.GetSortedLayers()
                    .First(item => item.Metadata.DisplayName == itemName);
                manager.MouseHandler.SelectedLayer = layer;
            }
            catch (InvalidOperationException) { }
            catch (NullReferenceException) { }
        }
    }
}
