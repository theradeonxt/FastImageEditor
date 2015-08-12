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

        #region View Getters/Setters
        
        // TODO: how to prevent multiple BackgroundWorkers from modifying the label/progressbar?
        // TODO: it's a waste to restrict to only one background task?
        public int StatusProgressbarPercentage
        {
            get { return statusProgressBar.Value; }
            set
            {
                if (statusProgressBar.Minimum <= value && value <= statusProgressBar.Maximum)
                {
                    statusProgressBar.Value = value;
                }
            }
        }

        public string StatusLabelText
        {
            get { return statusActionLabel.Text; }
            set { statusActionLabel.Text = value; }
        }

        #endregion
    }
}
