using System;
using System.Collections;
using System.Windows.Forms;
using VectorImageEdit.Models;
using VectorImageEdit.Modules;

namespace VectorImageEdit.Forms.AppWindow
{
    /// <summary>
    /// 
    /// AppWindow Module
    ///
    /// This is the main application window
    /// 
    /// </summary>
    public partial class AppWindow : Form
    {
        public AppWindow()
        {
            InitializeComponent();

            edgecolorToolStripButton.BackColor = AppGlobalData.Instance.ShapeEdgeColor;
            fillcolortoolStripButton.BackColor = AppGlobalData.Instance.ShapeFillColor;

            // This arranges the color picker toolbar items.
            // Note: the look from designer is corrected here
            topToolStrip.LayoutStyle = ToolStripLayoutStyle.Table;
            var layoutSettings = (topToolStrip.LayoutSettings as TableLayoutSettings);
            if (layoutSettings != null)
            {
                layoutSettings.ColumnCount = 11;
                layoutSettings.RowCount = 2;
            }

            memoryProgressBar.Value = BackgroundStatitics.MemoryUsagePercent;
            memoryUsedLabel.Text = string.Format("Memory Used ({0}%)", memoryProgressBar.Value.ToString());
        }

        #region View Listeners

        public void AddWorkspaceSizeChangedListener(IListener listener)
        {
            panWorkRegion.SizeChanged += listener.ActionPerformed;
        }

        public void AddAppWindowMovedListener(IListener listener)
        {
            Move += listener.ActionPerformed;
        }

        public void AddListboxSelectionChangedListener(IListener listener)
        {
            lBoxActiveLayers.SelectedIndexChanged += listener.ActionPerformed;
        }

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

        #endregion View Listeners

        #region View Getters/Setters

        public Control WorkspaceArea
        {
            get { return panWorkRegion; }
        }

        public string ListboxSelectedLayer
        {
            get
            {
                try
                {
                    return lBoxActiveLayers.SelectedItem.ToString();
                }
                catch (NullReferenceException)
                {
                    return "";
                }
            }
            set
            {
                if (string.IsNullOrEmpty(value)) return;
                lBoxActiveLayers.SelectedItem = value;
            }
        }

        public IEnumerable ListboxSelectedLayers
        {
            get { return lBoxActiveLayers.SelectedItems; }
        }

        public IEnumerable ListboxLayers
        {
            get { return lBoxActiveLayers.Items; }
            set
            {
                lBoxActiveLayers.Items.Clear();
                foreach (var item in value)
                {
                    lBoxActiveLayers.Items.Add(item);
                }
            }
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

        #endregion View Getters/Setters
    }
}
