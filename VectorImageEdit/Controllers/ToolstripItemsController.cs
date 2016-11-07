using System;
using NLog;
using VectorImageEdit.Models;
using VectorImageEdit.Views.Main;
using VectorImageEdit.WindowsFormsBridge;

namespace VectorImageEdit.Controllers
{
    class ToolstripItemsController
    {
        private readonly AppWindow view;
        private readonly ToolstripItemsModel model;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ToolstripItemsController(AppWindow view, ToolstripItemsModel model)
        {
            this.view = view;
            this.model = model;

            this.view.ToolbarPrimaryColor = AppModel.Instance.PrimaryColor;
            this.view.ToolbarSecondaryColor = AppModel.Instance.SecondaryColor;
            this.view.SetPrimaryColorActive();

            this.view.AddSwitchColorClickListener(new SwitchColorClickListener(this));
            this.view.AddCustomColorClickListener(new CustomColorClickListener(this));
            this.view.AddPresetColorClickListener(new PresetColorClickListener(this));
            this.view.AddShapeItemClickListener(new ShapeItemClickListener(this));
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

                switch (Controller.model.ColorMode)
                {
                    case ColorType.PrimaryColor:
                        AppModel.Instance.PrimaryColor = selectedColor;
                        Controller.view.ToolbarPrimaryColor = selectedColor;
                        Controller.view.SetPrimaryColorActive();
                        break;
                    default:
                        AppModel.Instance.SecondaryColor = selectedColor;
                        Controller.view.ToolbarSecondaryColor = selectedColor;
                        Controller.view.SetSecondaryColorActive();
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
                string itemName = Controller.view.GetToolstripItemName(sender);
                if (itemName.ToLower().Contains("primary"))
                {
                    Controller.model.ColorMode = ColorType.PrimaryColor;
                    Controller.view.SetPrimaryColorActive();
                }
                else
                {
                    Controller.model.ColorMode = ColorType.SecondaryColor;
                    Controller.view.SetSecondaryColorActive();
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
                var itemColor = Controller.view.GetToolstripItemBackground(sender);
                if (Controller.model.ColorMode == ColorType.PrimaryColor)
                {
                    Controller.view.ToolbarPrimaryColor = itemColor;
                    AppModel.Instance.PrimaryColor = itemColor;
                }
                else
                {
                    Controller.view.ToolbarSecondaryColor = itemColor;
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
                    string itemName = Controller.view.GetToolstripItemName(sender);
                    string shapeName = itemName.ToLower()
                        .Replace("toolstrip", "")
                        .Replace("shape", "")
                        .ToLower();
                    
                    Controller.model.CreateNewShape(shapeName);
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
