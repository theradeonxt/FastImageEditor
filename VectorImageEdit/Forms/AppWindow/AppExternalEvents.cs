using VectorImageEdit.Interfaces;

namespace VectorImageEdit.Forms.AppWindow
{
    public partial class AppWindow
    {
        #region View listeners

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
            panWorkRegion.DragDrop += listener.ActionPerformed;
        }

        public void AddDragEnterListener(IDragListener listener)
        {
            panWorkRegion.DragEnter += listener.ActionPerformed;
        }

        #endregion
    }
}
