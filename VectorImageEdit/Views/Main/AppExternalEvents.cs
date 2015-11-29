using VectorImageEdit.WindowsFormsBridge;

namespace VectorImageEdit.Views.Main
{
    public partial class AppWindow
    {
        public void AddSaveVectorListener(IListener listener)
        {
            saveVectorMenu.Click += listener.ActionPerformed;
        }
        public void AddOpenVectorListener(IListener listener)
        {
            openVectorMenu.Click += listener.ActionPerformed;
        }
        public void AddExportFileListener(IListener listener)
        {
            exportFileMenu.Click += listener.ActionPerformed;
        }
        public void AddOpenFileListener(IListener listener)
        {
            openFileMenu.Click += listener.ActionPerformed;
        }
        public void AddDragDropFileListener(IDragListener listener)
        {
            pboxWorkspaceRegion.DragDrop += listener.ActionPerformed;
        }
        public void AddDragEnterListener(IDragListener listener)
        {
            pboxWorkspaceRegion.DragEnter += listener.ActionPerformed;
        }
    }
}
