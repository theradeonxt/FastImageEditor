namespace VectorImageEdit.Models
{
    /// <summary>
    /// Main Model for the aplication
    /// 
    /// - this is the Model manager for all aplcation modules.
    /// </summary>
    class AppModel
    {
        public WorkspaceModel WorkspaceModel { get; private set; }
        public ExternalEventsModel ExternalModel { get; private set; }
        public MenuItemsModel MenuModel { get; private set; }
        public SceneTreeModel SceneTreeModel { get; private set; }
        public ToolstripItemsModel ToolbarsModel { get; private set; }

        public AppModel()
        {
            ExternalModel = new ExternalEventsModel();
            WorkspaceModel = new WorkspaceModel();
            MenuModel = new MenuItemsModel();
            SceneTreeModel = new SceneTreeModel();
            ToolbarsModel = new ToolstripItemsModel();
        }
    }
}
