using System;
using System.Windows.Forms;
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
        private readonly LayerManager _layerManager;
        private readonly Layout _layoutManager;

        private readonly ulong _totalRam;

        public AppWindow()
        {
            InitializeComponent();

            edgecolorToolStripButton.BackColor = ShapeStyle.GlobalEdgeColor;
            fillcolortoolStripButton.BackColor = ShapeStyle.GlobalFillColor;

            _layerManager = new LayerManager(panWorkRegion, lBoxActiveLayers);
            _layoutManager = new Layout(panWorkRegion.Size);

            topToolStrip.LayoutStyle = ToolStripLayoutStyle.Table;
            var layoutSettings = (topToolStrip.LayoutSettings as TableLayoutSettings);
            if (layoutSettings != null)
            {
                layoutSettings.ColumnCount = 11;
                layoutSettings.RowCount = 2;
            }

            var cinfo = new Microsoft.VisualBasic.Devices.ComputerInfo();
            _totalRam = cinfo.AvailablePhysicalMemory;
            memoryProgressBar.Value = (int)(100UL - _totalRam * 100UL / cinfo.TotalPhysicalMemory);
            memoryUsedLabel.Text = string.Format("Memory Used ({0}%)", memoryProgressBar.Value.ToString());
        }

        ////////////////////////////
        //
        // Menu Controls Events
        //
        ////////////////////////////

        private void cmsMenuProperties_Click(object sender, EventArgs e)
        {
            Layer selected = _layerManager.MouseHandler.SelectedLayer;
            PropertiesWindow layerProperties = new PropertiesWindow(selected);
            layerProperties.ShowDialog();
        }

        private void cmsMenuDelete_Click(object sender, EventArgs e)
        {
            Layer selected = _layerManager.MouseHandler.SelectedLayer;
            _layerManager.Remove(selected);
        }

        private void layerDeleteMenu_Click(object sender, EventArgs e)
        {
            Layer selected = _layerManager.MouseHandler.SelectedLayer;
            _layerManager.Remove(selected);
        }

        private void layerBringFrontMenu_Click(object sender, EventArgs e)
        {
            Layer selected = _layerManager.MouseHandler.SelectedLayer;
            _layerManager.BringToFront(selected);
        }

        private void layerSendBackMenu_Click(object sender, EventArgs e)
        {
            Layer selected = _layerManager.MouseHandler.SelectedLayer;
            _layerManager.SendToBack(selected);
        }

        private void layerSendBackwMenu_Click(object sender, EventArgs e)
        {
            Layer selected = _layerManager.MouseHandler.SelectedLayer;
            _layerManager.SendBackwards(selected);
        }

        ////////////////////////////
        //
        // Form Modified Events
        //
        ////////////////////////////

        private void panWorkRegion_SizeChanged(object sender, EventArgs e)
        {
            _layerManager.Resize(panWorkRegion);
            _layerManager.UpdateFrame(_layerManager.GetSortedLayers());
        }

        private void AppWindow_Move(object sender, EventArgs e)
        {
            _layerManager.RefreshFrame();
        }

        
    }
}
