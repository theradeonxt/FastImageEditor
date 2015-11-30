using System;
using System.Drawing;
using System.Windows.Forms; // TODO: Fix
using VectorImageEdit.Modules;
using VectorImageEdit.Modules.GraphicsCompositing;
using VectorImageEdit.Modules.LayerManagement;

namespace VectorImageEdit.Models
{
    /// <summary>
    /// Main Model for the aplication
    /// 
    /// - manages the models for all application modules
    /// - contains application settings
    /// 
    /// Implements the lazy singleton pattern
    /// </summary>
    partial class AppModel
    {
        public WorkspaceModel WorkspaceModel     { get; private set; }
        public ExternalEventsModel ExternalModel { get; private set; }
        public MenuItemsModel MenuModel          { get; private set; }
        public SceneTreeModel SceneTreeModel     { get; private set; }
        public ToolstripItemsModel ToolbarsModel { get; private set; }
        public WindowModel AppWindowModel        { get; private set; }

        // TODO: Refactor into interfaces?
        public LayerManager LayerManager         { get; private set; }
        public GraphicsManager GraphicsManager   { get; private set; }
        public Layout Layout                     { get; private set; }

        public Color PrimaryColor                { get; set; }
        public Color SecondaryColor              { get; set; }
        public float ShapeEdgeSize               { get; set; }
        public Pen LayerSelectionPen             { get; private set; }
        public string VectorFileExtension        { get; private set; }
        public string DefaultImageFileExtension  { get; private set; }

        public static AppModel Instance { get { return ThisInstance.Value; } }

        public void InitializeSettings(Action<GraphicsProfiler> graphicsCallback, params object[] arguments)
        {
            try
            {
                // The single LayerManager instance
                LayerManager = new LayerManager(
                    (Control)arguments[0],
                    graphicsCallback);

                // The single GraphicsManager instance
                GraphicsManager = LayerManager;

                // The single Layout instance
                Layout = new Layout((Size)arguments[1]);
            }
            catch (InvalidCastException) { }
        }
    }
}
