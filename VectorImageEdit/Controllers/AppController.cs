using System;
using System.Windows.Forms;
using VectorImageEdit.Forms;
using VectorImageEdit.Forms.AppWindow;
using VectorImageEdit.Models;

namespace VectorImageEdit.Controllers
{
    class AppController
    {
        private readonly AppWindow _appView;
        private readonly AppModel _appModel;

        public AppController(AppWindow appView)
        {
            _appView = appView;
            _appModel = new AppModel();

            InitializeControllers();
            InitializeAppGlobalData();
        }

        private void InitializeAppGlobalData()
        {
            GlobalModel.Instance.LayerManager = new Modules.Layers.LayerManager(_appView.WorkspaceArea, OnListboxItemsChangedCallback);
            GlobalModel.Instance.Layout = new Modules.Layout(_appView.Size);
        }

        private void InitializeControllers()
        {
            // TODO: make these private members?

            ExternalEventsModel exEvMdl = new ExternalEventsModel();
            // ReSharper disable once UnusedVariable
            ExternalEventsController exEvCtrl = new ExternalEventsController(_appView, exEvMdl);

            WorkspaceModel wsMdl = new WorkspaceModel();
            // ReSharper disable once UnusedVariable
            WorkspaceController wsCtrl = new WorkspaceController(_appView, wsMdl);

            _appView.AddListboxSelectionChangedListener(new LayerListSelectedChangedListener(this));
        }

        private void OnListboxItemsChangedCallback()
        {
            // This will be run un the UI thread, but can be invoked from other threads safely
            var layers = _appModel.GetSortedLayers();
            MethodInvoker del = delegate { _appView.ListboxLayers = layers; };
            _appView.Invoke(del);
        }

        private class LayerListSelectedChangedListener : AbstractListener<AppController>, IListener
        {
            public LayerListSelectedChangedListener(AppController controller)
                : base(controller)
            {
            }

            public void ActionPerformed(object sender, EventArgs e)
            {
                Controller._appModel.LayerListSelect(Controller._appView.ListboxSelectedLayer);
            }
        }
    }
}
