using System;
using VectorImageEdit.Models;
using VectorImageEdit.Views.Main;
using VectorImageEdit.WindowsFormsBridge;

namespace VectorImageEdit.Controllers
{
    class SceneTreeController
    {
        private readonly AppWindow view;
        private readonly SceneTreeModel model;

        public SceneTreeController(AppWindow view, SceneTreeModel model)
        {
            this.view = view;
            this.model = model;

            this.view.AddTreeSelectionChangedListener(new LayerListSelectedChangedListener(this));
        }

        public void OnListboxItemsChangedCallback()
        {
            var layers = AppModel.Instance.LayerManager.LayersList.ToArray();
            MyMethodInvoker.Invoke(() => view.Items = layers);
        }

        private class LayerListSelectedChangedListener : AbstractListener<SceneTreeController>, IListener
        {
            public LayerListSelectedChangedListener(SceneTreeController controller)
                : base(controller)
            {
            }

            public void ActionPerformed(object sender, EventArgs e)
            {
                //Controller._appView.SelectedItem = 

                // TODO: Fix this - trigger selection throws InvalidOperationException
                //Controller._model.SelectedItemChanged(Controller._appView.SelectedItem);
            }
        }
    }
}
