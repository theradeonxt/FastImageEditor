using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace VectorImageEdit.Models
{
    partial class AppModel
    {
        private static readonly Lazy<AppModel> ThisInstance =
            new Lazy<AppModel>(() => new AppModel());

        private AppModel()
        {
            CreateModels();
            CreateDefaultSettings();
        }

        private void CreateModels()
        {
            ExternalModel = new ExternalEventsModel();
            WorkspaceModel = new WorkspaceModel();
            MenuModel = new MenuItemsModel();
            SceneTreeModel = new SceneTreeModel();
            ToolbarsModel = new ToolstripItemsModel();
            AppWindowModel = new WindowModel();
        }
        private void CreateDefaultSettings()
        {
            PrimaryColor = Color.OrangeRed;
            SecondaryColor = Color.LightBlue;
            ShapeEdgeSize = 4.0f;

            LayerSelectionPen = new Pen(Brushes.DarkBlue, 3) { DashStyle = DashStyle.DashDot };

            VectorFileExtension = ".vdata";
            DefaultImageFileExtension = ".jpg";
        }
    }
}
