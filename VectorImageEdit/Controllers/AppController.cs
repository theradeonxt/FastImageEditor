using System;
using NLog;
using VectorImageEdit.Models;
using VectorImageEdit.Views.Main;
using VectorImageEdit.WindowsFormsBridge;

namespace VectorImageEdit.Controllers
{
    /// <summary> 
    /// Main Controller for the application
    /// 
    /// - manages the controllers for all application modules
    /// 
    /// </summary>
    class AppController
    {
        private readonly AppWindow _appView;

        private readonly WorkspaceController _workspaceController;
        private readonly ExternalEventsController _externalController;
        private readonly MenuItemsController _menuController;
        private readonly SceneTreeController _sceneTreeController;
        private readonly ToolstripItemsController _toolbarController;
        private readonly WindowController _windowController;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public AppController(AppWindow appView)
        {
            try
            {
                _appView = appView;
                var appModel = AppModel.Instance;

                // Create application modules
                _externalController = new ExternalEventsController(_appView, appModel.ExternalModel);
                _workspaceController = new WorkspaceController(_appView, appModel.WorkspaceModel);
                _menuController = new MenuItemsController(_appView, appModel.MenuModel);
                _sceneTreeController = new SceneTreeController(_appView, appModel.SceneTreeModel);
                _toolbarController = new ToolstripItemsController(_appView, appModel.ToolbarsModel);
                _windowController = new WindowController(_appView, appModel.AppWindowModel);

                // Create application configuration
                appModel.InitializeSettings(_workspaceController.GraphicsUpdateCallback,
                    _appView.WorkspaceArea, _appView.Size);

                // Redirect UI invocations to the main form control
                MyMethodInvoker.SetInvocationTarget(appView);
            }
            catch (OutOfMemoryException ex)
            {
                MessageBoxFactory.Create(caption: "Critical Error",
                    text: string.Format("Unable to create all application modules{0}Detailed information may be available in the LogFile.", Environment.NewLine),
                    boxType: MessageBoxType.Error);

                Logger.Error(ex.ToString());
            }
        }
    }
}
