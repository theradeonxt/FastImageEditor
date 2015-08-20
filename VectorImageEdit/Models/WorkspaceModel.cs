namespace VectorImageEdit.Models
{
    class WorkspaceModel
    {
        public void WorkspaceResize()
        {
            var layerManager = AppGlobalModel.Instance.LayerManager;
            layerManager.Resize();
            layerManager.UpdateFrame(layerManager.GetSortedLayers());
        }

        public void AppWindowMove()
        {
            //TODO: might be expensive to refresh every time
            AppGlobalModel.Instance.LayerManager.RefreshFrame();
        }
    }
}
