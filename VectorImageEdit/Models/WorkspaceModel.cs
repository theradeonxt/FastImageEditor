namespace VectorImageEdit.Models
{
    class WorkspaceModel
    {
        public void WorkspaceResize()
        {
            var layerManager = GlobalModel.Instance.LayerManager;
            layerManager.Resize();
            layerManager.UpdateFrame(layerManager.GetSortedLayers());
        }

        public void AppWindowMove()
        {
            //TODO: might be expensive to refresh every time
            GlobalModel.Instance.LayerManager.RefreshFrame();
        }
    }
}
