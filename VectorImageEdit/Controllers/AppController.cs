using System;
using System.Windows.Forms;
using VectorImageEdit.Forms;
using VectorImageEdit.Forms.AppWindow;
using VectorImageEdit.Models;
using VectorImageEdit.Modules;
using VectorImageEdit.Modules.Layers;

namespace VectorImageEdit.Controllers
{
    class AppController
    {
        private readonly AppWindow _appView;
        private readonly AppModel _appModel;

        private WorkspaceModel _workspaceMdl;
        private ExternalEventsModel _externalMdl;
        private MenuItemsModel _menuMdl;

        // ReSharper disable once NotAccessedField.Local
        private WorkspaceController _workspaceCtrl;
        // ReSharper disable once NotAccessedField.Local
        private ExternalEventsController _externalCtrl;
        // ReSharper disable once NotAccessedField.Local
        private MenuItemsController _menuCtrl;

        public AppController(AppWindow appView)
        {
            _appView = appView;
            _appModel = new AppModel();

            InitializeModels();
            InitializeControllers();
            InitializeAppGlobalData();
        }

        private void InitializeModels()
        {
            _externalMdl = new ExternalEventsModel();
            _workspaceMdl = new WorkspaceModel();
            _menuMdl = new MenuItemsModel();
        }

        private void InitializeControllers()
        {
            _externalCtrl = new ExternalEventsController(_appView, _externalMdl);
            _workspaceCtrl = new WorkspaceController(_appView, _workspaceMdl);
            _menuCtrl = new MenuItemsController(_appView, _menuMdl);

            _appView.AddListboxSelectionChangedListener(new LayerListSelectedChangedListener(this));
        }

        private void InitializeAppGlobalData()
        {
            AppGlobalModel.Instance.LayerManager = new LayerManager(_appView.WorkspaceArea, OnListboxItemsChangedCallback);
            AppGlobalModel.Instance.Layout = new Layout(_appView.Size);
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
