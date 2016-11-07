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
        private readonly AppWindow appView;

        private readonly WorkspaceController workspaceController;
        private readonly ExternalEventsController externalController;
        private readonly MenuItemsController menuController;
        private readonly SceneTreeController sceneTreeController;
        private readonly ToolstripItemsController toolbarController;
        private readonly WindowController windowController;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public AppController(AppWindow appView)
        {
            try
            {
                this.appView = appView;
                var appModel = AppModel.Instance;

                // Create application modules
                externalController = new ExternalEventsController(this.appView, appModel.ExternalModel);
                workspaceController = new WorkspaceController(this.appView, appModel.WorkspaceModel);
                menuController = new MenuItemsController(this.appView, appModel.MenuModel);
                sceneTreeController = new SceneTreeController(this.appView, appModel.SceneTreeModel);
                toolbarController = new ToolstripItemsController(this.appView, appModel.ToolbarsModel);
                windowController = new WindowController(this.appView, appModel.AppWindowModel);

                // Create application configuration
                appModel.InitializeSettings(workspaceController.GraphicsUpdateCallback,
                    this.appView, this.appView.Size);

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
