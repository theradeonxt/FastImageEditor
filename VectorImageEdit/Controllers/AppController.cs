using System;
using VectorImageEdit.Forms.AppWindow;
using VectorImageEdit.Models;
using VectorImageEdit.Modules;
using VectorImageEdit.Modules.Layers;

namespace VectorImageEdit.Controllers
{
    /// <summary>
    /// Main Controller for the application
    /// 
    /// - is the Controller manager for all aplication modules.
    /// </summary>
    class AppController
    {
        private readonly AppWindow _appView;
        private readonly AppModel _appModel;

        private WorkspaceController WorkspaceController { get; set; }
        private ExternalEventsController ExternalController { get; set; }
        private MenuItemsController MenuController { get; set; }
        private SceneTreeController SceneTreeController { get; set; }

        public AppController(AppWindow appView)
        {
            try
            {
                _appView = appView;
                _appModel = new AppModel();

                ExternalController = new ExternalEventsController(_appView, _appModel.ExternalModel);
                WorkspaceController = new WorkspaceController(_appView, _appModel.WorkspaceModel);
                MenuController = new MenuItemsController(_appView, _appModel.MenuModel);
                SceneTreeController = new SceneTreeController(_appView, _appModel.SceneTreeModel);

                InitializeAppGlobalData();
            }
            catch (OutOfMemoryException)
            { 
                // TODO: This must be logged and application will forcibly close with user feedback
            }
        }

        private void InitializeAppGlobalData()
        {
            AppGlobalData.Instance.LayerManager = new LayerManager(_appView.WorkspaceArea, SceneTreeController.OnListboxItemsChangedCallback);
            AppGlobalData.Instance.Layout = new Layout(_appView.Size);
        }
    }
}
