using System;
using VectorImageEdit.Models;
using VectorImageEdit.Views.Main;
using VectorImageEdit.WindowsFormsBridge;

namespace VectorImageEdit.Controllers
{
    class MenuItemsController
    {
        private readonly AppWindow view;
        private readonly MenuItemsModel model;

        public MenuItemsController(AppWindow view, MenuItemsModel model)
        {
            this.view = view;
            this.model = model;

            this.view.AddContextMenuPropertiesListener(new ContextMenuPropertiesListener(this));
            this.view.AddContextMenuDeleteListener(new ContextMenuDeleteListener(this));
            this.view.AddContextMenuBringFrontListener(new ContextMenuBringFrontListener(this));
            this.view.AddContextMenuSendBackListener(new ContextMenuSendBackListener(this));
            this.view.AddContextMenuSendBackwardsListener(new ContextMenuSendBackwardsListener(this));
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
