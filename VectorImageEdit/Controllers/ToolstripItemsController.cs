using System;
using System.Windows.Forms;
using VectorImageEdit.Forms;
using VectorImageEdit.Forms.AppWindow;
using VectorImageEdit.Models;

namespace VectorImageEdit.Controllers
{
    class ToolstripItemsController
    {
        private readonly AppWindow _appView;
        private readonly ToolbarItemsModel _model;

        public ToolstripItemsController(AppWindow appView, ToolbarItemsModel model)
        {
            _appView = appView;
            _model = model;

            _appView.ToolbarPrimaryColor = AppGlobalData.Instance.ShapeEdgeColor;
            _appView.ToolbarSecondaryColor = AppGlobalData.Instance.ShapeFillColor;
            _appView.SetPrimaryColorActive();

            _appView.AddSwitchColorClickListener(new SwitchColorClickListener(this));
            _appView.AddCustomColorClickListener(new CustomColorClickListener(this));
            _appView.AddPresetColorClickListener(new PresetColorClickListener(this));
        }

        private class CustomColorClickListener : AbstractListener<ToolstripItemsController>, IListener
        {
            public CustomColorClickListener(ToolstripItemsController controller)
                : base(controller)
            {
            }

            public void ActionPerformed(object sender, EventArgs e)
            {
                ColorDialog colorPicker = new ColorDialog { FullOpen = true };
                DialogResult dlg = colorPicker.ShowDialog();
                if (dlg != DialogResult.OK) return;

                ColorType colorMode = Controller._model.ColorMode;
                if (colorMode == ColorType.PrimaryColor)
                {
                    AppGlobalData.Instance.ShapeEdgeColor = colorPicker.Color;
                    Controller._appView.ToolbarPrimaryColor = colorPicker.Color;
                    Controller._appView.SetPrimaryColorActive();
                }
                else
                {
                    AppGlobalData.Instance.ShapeFillColor = colorPicker.Color;
                    Controller._appView.ToolbarSecondaryColor = colorPicker.Color;
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
                    var stripButton = (ToolStripButton)sender;
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
                    if (Controller._model.ColorMode == ColorType.PrimaryColor)
                        Controller._appView.ToolbarPrimaryColor = ((ToolStripItem)sender).BackColor;
                    else
                        Controller._appView.ToolbarSecondaryColor = ((ToolStripItem)sender).BackColor;
                }
                catch (InvalidCastException) { }
            }
        }
    }
}
