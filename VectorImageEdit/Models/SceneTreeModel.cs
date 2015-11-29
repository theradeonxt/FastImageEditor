using System;
using System.Linq;
using JetBrains.Annotations;
using VectorImageEdit.Modules.LayerManagement;

namespace VectorImageEdit.Models
{
    class SceneTreeModel
    {
        public void SelectedItemChanged([CanBeNull]string itemName)
        {
            if (string.IsNullOrEmpty(itemName) ||
                string.IsNullOrWhiteSpace(itemName)) return;
            try
            {
                var manager = AppModel.Instance.LayerManager;
                Layer layer = manager.WorkspaceLayers
                    .First(item => item.Metadata.DisplayName == itemName);


            }
            catch (InvalidOperationException) { }
            catch (NullReferenceException) { }

            // FIXME
            // manager.MouseHandler.SelectedLayer = layer;
        }
    }
}
