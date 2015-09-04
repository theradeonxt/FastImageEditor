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

            _appView.AddListboxSelectionChangedListener(new LayerListSelectedChangedListener(this));
        }

        public void OnListboxItemsChangedCallback()
        {
            // This will be run on the UI thread, but can be invoked from other threads safely
            var layers = _model.GetSortedLayers();
            MethodInvoker del = delegate { _appView.ListboxLayers = layers; };
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
                Controller._model.LayerListSelect(Controller._appView.ListboxSelectedLayer);
            }
        }
    }
}
