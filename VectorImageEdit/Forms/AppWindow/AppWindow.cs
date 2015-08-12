using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using VectorImageEdit.Models;
using VectorImageEdit.Modules;
using VectorImageEdit.Modules.BasicShapes;
using VectorImageEdit.Modules.Layers;

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
        //private readonly LayerManager GlobalModel.Instance.LayerManager;

        public AppWindow()
        {
            InitializeComponent();

            edgecolorToolStripButton.BackColor = ShapeStyle.GlobalEdgeColor;
            fillcolortoolStripButton.BackColor = ShapeStyle.GlobalFillColor;

            //GlobalModel.Instance.LayerManager = new LayerManager(panWorkRegion, lBoxActiveLayers);

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

        ////////////////////////////
        //
        // Menu Controls Events
        //
        ////////////////////////////

        private void cmsMenuProperties_Click(object sender, EventArgs e)
        {
            Layer selected = GlobalModel.Instance.LayerManager.MouseHandler.SelectedLayer;
            PropertiesWindow layerProperties = new PropertiesWindow(selected);
            layerProperties.ShowDialog();
        }

        private void cmsMenuDelete_Click(object sender, EventArgs e)
        {
            Layer selected = GlobalModel.Instance.LayerManager.MouseHandler.SelectedLayer;
            GlobalModel.Instance.LayerManager.Remove(selected);
        }

        private void layerDeleteMenu_Click(object sender, EventArgs e)
        {
            Layer selected = GlobalModel.Instance.LayerManager.MouseHandler.SelectedLayer;
            GlobalModel.Instance.LayerManager.Remove(selected);
        }

        private void layerBringFrontMenu_Click(object sender, EventArgs e)
        {
            Layer selected = GlobalModel.Instance.LayerManager.MouseHandler.SelectedLayer;
            GlobalModel.Instance.LayerManager.BringToFront(selected);
        }

        private void layerSendBackMenu_Click(object sender, EventArgs e)
        {
            Layer selected = GlobalModel.Instance.LayerManager.MouseHandler.SelectedLayer;
            GlobalModel.Instance.LayerManager.SendToBack(selected);
        }

        private void layerSendBackwMenu_Click(object sender, EventArgs e)
        {
            Layer selected = GlobalModel.Instance.LayerManager.MouseHandler.SelectedLayer;
            GlobalModel.Instance.LayerManager.SendBackwards(selected);
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

        #endregion

        #region View Getters/Setters

        public string ListboxSelectedLayer
        {
            get { return (string)lBoxActiveLayers.SelectedItem; }
            set
            {
                if (string.IsNullOrEmpty(value)) return;
                lBoxActiveLayers.SelectedItem = value;
            }
        }

        public IEnumerable ListboxSelectedLayers { get { return lBoxActiveLayers.SelectedItems; } }

        #endregion
    }
}
