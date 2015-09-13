namespace VectorImageEdit.Forms.AppWindow
{
    public partial class AppWindow
    {
        #region View Listeners

        public void AddContextMenuPropertiesListener(IListener listener)
        {
            cmsMenuProperties.Click += listener.ActionPerformed;
            layerPropertiesMenu.Click += listener.ActionPerformed;
        }

        public void AddContextMenuDeleteListener(IListener listener)
        {
            cmsMenuDelete.Click += listener.ActionPerformed;
            layerDeleteMenu.Click += listener.ActionPerformed;
        }

        public void AddContextMenuBringFrontListener(IListener listener)
        {
            layerBringFrontMenu.Click += listener.ActionPerformed;
        }

        public void AddContextMenuSendBackListener(IListener listener)
        {
            layerSendBackMenu.Click += listener.ActionPerformed;
        }

        public void AddContextMenuSendBackwardsListener(IListener listener)
        {
            layerSendBackMenu.Click += listener.ActionPerformed;
        }

        #endregion
    }
}
