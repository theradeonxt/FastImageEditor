using System.Windows.Forms;
using VectorImageEdit.WindowsFormsBridge;

namespace VectorImageEdit.Views.Main
{
    /// <summary>
    /// 
    /// AppWindow Module
    ///
    /// This is the Form for the main application window
    /// 
    /// </summary>
    public partial class AppWindow : Form
    {
        public AppWindow()
        {
            InitializeComponent();
            InitializeTheme();

            pboxWorkspaceRegion.AllowDrop = true;
        }

        public void AddWindowSizeChangedListener(IListener listener)
        {
            pboxWorkspaceRegion.SizeChanged += listener.ActionPerformed;
        }
        public void AddWindowMovedListener(IListener listener)
        {
            Move += listener.ActionPerformed;
        }

        public Control WorkspaceArea
        {
            get { return pboxWorkspaceRegion; }
        }

        // TODO: how to prevent multiple BackgroundWorkers from modifying the label/progressbar?
        // TODO: it's a waste to restrict to only one background task? OR dynamically add action status + progressbar?
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

        public int MemoryProgressbarPercentage
        {
            get { return memoryProgressBar.Value; }
            set
            {
                if (memoryProgressBar.Minimum <= value && value <= memoryProgressBar.Maximum)
                {
                    memoryProgressBar.Value = value;
                }
            }
        }
        public string MemoryLabelText
        {
            get { return memoryUsedLabel.Text; }
            set { memoryUsedLabel.Text = value; }
        }

        public string GraphicsDebugText
        {
            set { labelGraphicsDebug.Text = value; }
        }
    }
}
