using System;
using VectorImageEdit.Models;
using VectorImageEdit.Views.Main;
using VectorImageEdit.WindowsFormsBridge;

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
            _appView.AddContextMenuBringFrontListener(new ContextMenuBringFrontListener(this));
            _appView.AddContextMenuSendBackListener(new ContextMenuSendBackListener(this));
            _appView.AddContextMenuSendBackwardsListener(new ContextMenuSendBackwardsListener(this));
        }

        private class ContextMenuPropertiesListener : AbstractListener<MenuItemsController>, IListener
        {
            public ContextMenuPropertiesListener(MenuItemsController controller)
                : base(controller)
            {
            }

            public void ActionPerformed(object sender, EventArgs e)
            {
                //var selected = AppModel.Instance.LayerManager.MouseHandler.SelectedLayer;
                //PropertiesWindow layerProperties = new PropertiesWindow(selected);
                //layerProperties.ShowDialog();
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
                //var selected = AppModel.Instance.LayerManager.MouseHandler.SelectedLayer;
                //AppModel.Instance.LayerManager.Remove(selected);
            }
        }
        private class ContextMenuBringFrontListener : AbstractListener<MenuItemsController>, IListener
        {
            public ContextMenuBringFrontListener(MenuItemsController controller)
                : base(controller)
            {
            }

            public void ActionPerformed(object sender, EventArgs e)
            {
                //var selected = AppModel.Instance.LayerManager.MouseHandler.SelectedLayer;
                //AppModel.Instance.LayerManager.ApplyModifier("BringToFront", selected);
            }
        }
        private class ContextMenuSendBackListener : AbstractListener<MenuItemsController>, IListener
        {
            public ContextMenuSendBackListener(MenuItemsController controller)
                : base(controller)
            {
            }

            public void ActionPerformed(object sender, EventArgs e)
            {
                //var selected = AppModel.Instance.LayerManager.MouseHandler.SelectedLayer;
                //AppModel.Instance.LayerManager.ApplyModifier("SendToBack", selected);
            }
        }
        private class ContextMenuSendBackwardsListener : AbstractListener<MenuItemsController>, IListener
        {
            public ContextMenuSendBackwardsListener(MenuItemsController controller)
                : base(controller)
            {
            }

            public void ActionPerformed(object sender, EventArgs e)
            {
                //var selected = AppModel.Instance.LayerManager.MouseHandler.SelectedLayer;
                //AppModel.Instance.LayerManager.ApplyModifier("SendBackward", selected);
            }
        }
    }
}
