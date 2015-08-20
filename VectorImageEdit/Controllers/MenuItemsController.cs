using System;
using VectorImageEdit.Forms;
using VectorImageEdit.Forms.AppWindow;
using VectorImageEdit.Models;

namespace VectorImageEdit.Controllers
{
    class MenuItemsController
    {
        private readonly AppWindow _appView;
        private readonly MenuItemsModel _model;

        public MenuItemsController(AppWindow appView, MenuItemsModel model)
        {
            _appView = appView;
            _model = model;

            _appView.AddContextMenuPropertiesListener(new ContextMenuPropertiesListener(this));
            _appView.AddContextMenuDeleteListener(new ContextMenuDeleteListener(this));
        }

        private class ContextMenuPropertiesListener : AbstractListener<MenuItemsController>, IListener
        {
            public ContextMenuPropertiesListener(MenuItemsController controller)
                : base(controller)
            {
            }

            public void ActionPerformed(object sender, EventArgs e)
            {
                var selected = AppGlobalModel.Instance.LayerManager.MouseHandler.SelectedLayer;
                PropertiesWindow layerProperties = new PropertiesWindow(selected);
                layerProperties.ShowDialog();
            }
        }

        private class ContextMenuDeleteListener : AbstractListener<MenuItemsController>, IListener
        {
            public ContextMenuDeleteListener(MenuItemsController controller)
                : base(controller)
            {
            }

            public void ActionPerformed(object sender, EventArgs e)
            {
                var selected = AppGlobalModel.Instance.LayerManager.MouseHandler.SelectedLayer;
                AppGlobalModel.Instance.LayerManager.Remove(selected);
            }
        }
    }
}
