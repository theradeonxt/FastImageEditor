using System;
using VectorImageEdit.Models;
using VectorImageEdit.Views.Main;
using VectorImageEdit.WindowsFormsBridge;

namespace VectorImageEdit.Controllers
{
    class SceneTreeController
    {
        private readonly AppWindow _appView;
        private readonly SceneTreeModel _model;

        public SceneTreeController(AppWindow appView, SceneTreeModel model)
        {
            _appView = appView;
            _model = model;

            _appView.AddTreeSelectionChangedListener(new LayerListSelectedChangedListener(this));
        }

        public void OnListboxItemsChangedCallback()
        {
            var layers = AppModel.Instance.LayerManager.GetLayers().ToArray();
            MyMethodInvoker.Invoke(() => _appView.Items = layers);
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
