using System;
using NLog;
using VectorImageEdit.Models;
using VectorImageEdit.Views.Main;
using VectorImageEdit.WindowsFormsBridge;

namespace VectorImageEdit.Controllers
{
    class ToolstripItemsController
    {
        private readonly AppWindow _appView;
        private readonly ToolstripItemsModel _model;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ToolstripItemsController(AppWindow appView, ToolstripItemsModel model)
        {
            _appView = appView;
            _model = model;

            _appView.ToolbarPrimaryColor = AppModel.Instance.PrimaryColor;
            _appView.ToolbarSecondaryColor = AppModel.Instance.SecondaryColor;
            _appView.SetPrimaryColorActive();

            _appView.AddSwitchColorClickListener(new SwitchColorClickListener(this));
            _appView.AddCustomColorClickListener(new CustomColorClickListener(this));
            _appView.AddPresetColorClickListener(new PresetColorClickListener(this));
            _appView.AddShapeItemClickListener(new ShapeItemClickListener(this));
        }

        private class CustomColorClickListener : AbstractListener<ToolstripItemsController>, IListener
        {
            public CustomColorClickListener(ToolstripItemsController controller)
                : base(controller)
            {
            }

            public void ActionPerformed(object sender, EventArgs e)
            {
                var factory = new ColorDialogFactory();
                var result = factory.CreateDialog(true);
                var selectedColor = result;

                switch (Controller._model.ColorMode)
                {
                    case ColorType.PrimaryColor:
                        AppModel.Instance.PrimaryColor = selectedColor;
                        Controller._appView.ToolbarPrimaryColor = selectedColor;
                        Controller._appView.SetPrimaryColorActive();
                        break;
                    default:
                        AppModel.Instance.SecondaryColor = selectedColor;
                        Controller._appView.ToolbarSecondaryColor = selectedColor;
                        Controller._appView.SetSecondaryColorActive();
                        break;
                }
            }
        }
        private class SwitchColorClickListener : AbstractListener<ToolstripItemsController>, IListener
        {
            public SwitchColorClickListener(ToolstripItemsController controller)
                : base(controller)
            {
            }

            public void ActionPerformed(object sender, EventArgs e)
            {
                string itemName = Controller._appView.GetToolstripItemName(sender);
                if (itemName.ToLower().Contains("primary"))
                {
                    Controller._model.ColorMode = ColorType.PrimaryColor;
                    Controller._appView.SetPrimaryColorActive();
                }
                else
                {
                    Controller._model.ColorMode = ColorType.SecondaryColor;
                    Controller._appView.SetSecondaryColorActive();
                }
            }
        }
        private class PresetColorClickListener : AbstractListener<ToolstripItemsController>, IListener
        {
            public PresetColorClickListener(ToolstripItemsController controller)
                : base(controller)
            {
            }

            public void ActionPerformed(object sender, EventArgs e)
            {
                var itemColor = Controller._appView.GetToolstripItemBackground(sender);
                if (Controller._model.ColorMode == ColorType.PrimaryColor)
                {
                    Controller._appView.ToolbarPrimaryColor = itemColor;
                    AppModel.Instance.PrimaryColor = itemColor;
                }
                else
                {
                    Controller._appView.ToolbarSecondaryColor = itemColor;
                    AppModel.Instance.SecondaryColor = itemColor;
                }
            }
        }
        private class ShapeItemClickListener : AbstractListener<ToolstripItemsController>, IListener
        {
            public ShapeItemClickListener(ToolstripItemsController controller)
                : base(controller)
            {
            }

            public void ActionPerformed(object sender, EventArgs e)
            {
                try
                {
                    string itemName = Controller._appView.GetToolstripItemName(sender);
                    string shapeName = itemName.ToLower()
                        .Replace("toolstrip", "")
                        .Replace("shape", "")
                        .ToLower();
                    
                    Controller._model.CreateNewShape(shapeName);
                }
                catch (Exception ex)
                {
                    MessageBoxFactory.Create(caption: "Information",
                        text: string.Format("The shape requested could not be created.{0}Detailed information may be available in the LogFile.", Environment.NewLine),
                        boxType: MessageBoxType.Information);

                    Logger.Warn(ex.ToString());
                }
            }
        }
    }
}
