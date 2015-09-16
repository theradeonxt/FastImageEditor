using System;
using System.Windows.Forms;
using VectorImageEdit.Forms;
using VectorImageEdit.Forms.AppWindow;
using VectorImageEdit.Models;
using VectorImageEdit.Modules.Factories;

namespace VectorImageEdit.Controllers
{
    class ToolstripItemsController
    {
        private readonly AppWindow _appView;
        private readonly ToolstripItemsModel _model;

        public ToolstripItemsController(AppWindow appView, ToolstripItemsModel model)
        {
            _appView = appView;
            _model = model;

            _appView.ToolbarPrimaryColor = AppGlobalData.Instance.PrimaryColor;
            _appView.ToolbarSecondaryColor = AppGlobalData.Instance.SecondaryColor;
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
                factory.CreateDialog();

                ColorType colorMode = Controller._model.ColorMode;
                if (colorMode == ColorType.PrimaryColor)
                {
                    AppGlobalData.Instance.PrimaryColor = factory.DialogData;
                    Controller._appView.ToolbarPrimaryColor = factory.DialogData;
                    Controller._appView.SetPrimaryColorActive();
                }
                else
                {
                    AppGlobalData.Instance.SecondaryColor = factory.DialogData;
                    Controller._appView.ToolbarSecondaryColor = factory.DialogData;
                    Controller._appView.SetSecondaryColorActive();
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
                try
                {
                    var stripButton = (ToolStripItem)sender;
                    if (stripButton.Name.ToLower().Contains("primary"))
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
                catch (InvalidCastException) { }
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
                try
                {
                    var itemColor = ((ToolStripItem)sender).BackColor;
                    if (Controller._model.ColorMode == ColorType.PrimaryColor)
                        Controller._appView.ToolbarPrimaryColor = itemColor;
                    else
                        Controller._appView.ToolbarSecondaryColor = itemColor;
                }
                catch (InvalidCastException) { }
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
                    var stripItem = (ToolStripItem)sender;
                    string key = stripItem.Name.ToLower()
                        .Replace("toolstrip", "")
                        .Replace("shape", "")
                        .ToLower();
                    Controller._model.CreateNewShape(key);
                }
                catch (ArgumentException) { }
                catch (InvalidCastException) { }
                catch (OutOfMemoryException) { }
            }
        }
    }
}
