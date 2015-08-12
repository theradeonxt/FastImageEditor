using VectorImageEdit.Forms.AppWindow;
using VectorImageEdit.Models;

namespace VectorImageEdit.Controllers
{
    class AppController
    {
        private readonly AppWindow _appView;

        public AppController(AppWindow appView)
        {
            _appView = appView;

            InitializeAppGlobalData();
            InitializeControllers();
        }

        private void InitializeAppGlobalData()
        {
            // TODO: correct these
            GlobalModel.Instance.LayerManager = new Modules.Layers.LayerManager(_appView, null);
            GlobalModel.Instance.Layout = new Modules.Layout(_appView.Size);
        }

        private void InitializeControllers()
        {
            //TODO: make this private members?
            ExternalEventsModel exEvMdl = new ExternalEventsModel();
            // ReSharper disable once UnusedVariable
            ExternalEventsController exEvCtrl = new ExternalEventsController(_appView, exEvMdl);

            WorkspaceModel wsMdl = new WorkspaceModel();
            // ReSharper disable once UnusedVariable
            WorkspaceController wsCtrl = new WorkspaceController(_appView, wsMdl);
        }
    }
}
