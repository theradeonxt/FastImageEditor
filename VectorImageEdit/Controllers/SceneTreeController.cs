using System;
using System.Windows.Forms;
using VectorImageEdit.Forms;
using VectorImageEdit.Forms.AppWindow;
using VectorImageEdit.Models;

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
            // This will be run on the UI thread, but can be invoked from other threads safely
            var layers = AppGlobalData.Instance.LayerManager.GetLayers().ToArray();
            MethodInvoker del = delegate { _appView.SceneTreeItems = layers; };
            _appView.Invoke(del);
        }

        private class LayerListSelectedChangedListener : AbstractListener<SceneTreeController>, IListener
        {
            public LayerListSelectedChangedListener(SceneTreeController controller)
                : base(controller)
            {
            }

            public void ActionPerformed(object sender, EventArgs e)
            {
                // TODO: Fix this - trigger selection throws InvalidOperationException
                Controller._model.SelectSceneTreeItem(Controller._appView.SceneTreeSelectedItem);
            }
        }
    }
}
