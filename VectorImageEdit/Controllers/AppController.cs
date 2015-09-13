using System;
using System.Diagnostics.CodeAnalysis;
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
    [SuppressMessage("ReSharper", "NotAccessedField.Local")]
    [SuppressMessage("ReSharper", "PrivateFieldCanBeConvertedToLocalVariable")]
    class AppController
    {
        private readonly AppWindow _appView;
        private readonly AppModel _appModel;

        private readonly WorkspaceController _workspaceController;
        private readonly ExternalEventsController _externalController;
        private readonly MenuItemsController _menuController;
        private readonly SceneTreeController _sceneTreeController;
        private readonly ToolstripItemsController _toolbarController;

        public AppController(AppWindow appView)
        {
            try
            {
                _appView = appView;
                _appModel = new AppModel();

                _externalController = new ExternalEventsController(_appView, _appModel.ExternalModel);
                _workspaceController = new WorkspaceController(_appView, _appModel.WorkspaceModel);
                _menuController = new MenuItemsController(_appView, _appModel.MenuModel);
                _sceneTreeController = new SceneTreeController(_appView, _appModel.SceneTreeModel);
                _toolbarController = new ToolstripItemsController(_appView, _appModel.ToolbarsModel);

                InitializeAppGlobalData();
            }
            catch (OutOfMemoryException)
            { 
                // TODO: This must be logged and application will forcibly close with user feedback
            }
        }

        private void InitializeAppGlobalData()
        {
            AppGlobalData.Instance.LayerManager = new LayerManager(_appView.WorkspaceArea, _sceneTreeController.OnListboxItemsChangedCallback);
            AppGlobalData.Instance.Layout = new Layout(_appView.Size);
        }
    }
}
