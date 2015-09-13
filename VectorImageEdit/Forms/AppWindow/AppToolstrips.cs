using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VectorImageEdit.Models;
using VectorImageEdit.Modules.BasicShapes;

namespace VectorImageEdit.Forms.AppWindow
{
    // TODO: Remove direct dependency on shapes here in the View and move it to Model

    public partial class AppWindow
    {
        #region View Listeners

        public void AddSwitchColorClickListener(IListener listener)
        {
            toolstripPrimaryColorTrigger.Click += listener.ActionPerformed;
            toolstripSecondaryColorTrigger.Click += listener.ActionPerformed;
        }
        public void AddCustomColorClickListener(IListener listener)
        {
            toolStripColorPick.Click += listener.ActionPerformed;
        }
        public void AddPresetColorClickListener(IListener listener)
        {
            try
            {
                foreach (var stripItem in topToolStrip.Items
                    .Cast<object>()
                    .OfType<ToolStripItem>()
                    .Where(stripItem => stripItem.Name.Contains("toolStripColorPreset")))
                {
                    stripItem.Click += listener.ActionPerformed;
                }
            }
            catch (ArgumentException) { }
            catch (InvalidCastException) { }
        }

        #endregion

        #region View Getters/Setters

        public Color ToolbarPrimaryColor
        {
            set
            {
                toolstripPrimaryColorPreview.BackColor = value;
                toolstripPrimaryColorPreview.ToolTipText = value.ToString();
            }
        }
        public Color ToolbarSecondaryColor
        {
            set
            {
                toolstripSecondaryColorPreview.BackColor = value;
                toolstripSecondaryColorPreview.ToolTipText = value.ToString();
            }
        }

        public void SetPrimaryColorActive()
        {
            toolstripPrimaryColorTrigger.Checked = true;
            toolstripSecondaryColorTrigger.Checked = false;
        }
        public void SetSecondaryColorActive()
        {
            toolstripSecondaryColorTrigger.Checked = true;
            toolstripPrimaryColorTrigger.Checked = false;
        }

        #endregion
        
        private class CustomToolStripRenderer : ToolStripProfessionalRenderer
        {
            public CustomToolStripRenderer()
                : base(new CustomLook())
            {
            }
        }

        private void InitializeToolbarLook()
        {
            // This arranges the color picker toolbar items; the designer look is corrected here
            topToolStrip.LayoutStyle = ToolStripLayoutStyle.Table;
            var layoutSettings = (topToolStrip.LayoutSettings as TableLayoutSettings);
            if (layoutSettings != null)
            {
                layoutSettings.ColumnCount = 13;
                layoutSettings.RowCount = 2;
            }
            // Use a custom appearance theme for the application toolstrip items
            ToolStripManager.Renderer = new CustomToolStripRenderer();
        }

        #region Shape toolbar buttons

        private void btnCircle_Click(object sender, EventArgs e)
        {
            Point center = new Point(panWorkRegion.Width / 2, panWorkRegion.Height / 2);
            Circle c = new Circle(center, panWorkRegion.Width / 7, StyleBuilder.FromGlobalStyles());
            AppGlobalData.Instance.LayerManager.Add(c);
        }

        private void btnOval_Click(object sender, EventArgs e)
        {
            Point location = new Point(panWorkRegion.Width / 3, panWorkRegion.Height / 3);
            Size size = new Size(panWorkRegion.Width / 5, panWorkRegion.Width / 7);
            Oval o = new Oval(new Rectangle(location, size), StyleBuilder.FromGlobalStyles());
            AppGlobalData.Instance.LayerManager.Add(o);
        }

        private void btnSquare_Click(object sender, EventArgs e)
        {
            Point location = new Point(panWorkRegion.Width / 3, panWorkRegion.Height / 3);
            int side = panWorkRegion.Width / 4;
            Square s = new Square(location, side, StyleBuilder.FromGlobalStyles());
            AppGlobalData.Instance.LayerManager.Add(s);
        }

        private void btnRectangle_Click(object sender, EventArgs e)
        {
            Point location = new Point(panWorkRegion.Width / 3, panWorkRegion.Height / 3);
            int width = panWorkRegion.Width / 4;
            int height = panWorkRegion.Width / 6;
            Rectangle region = new Rectangle(location.X, location.Y, width, height);
            SRectangle r = new SRectangle(region, StyleBuilder.FromGlobalStyles());
            AppGlobalData.Instance.LayerManager.Add(r);
        }

        private void btnDiamond_Click(object sender, EventArgs e)
        {

        }

        private void btnHexagon_Click(object sender, EventArgs e)
        {

        }

        private void btnTriangle_Click(object sender, EventArgs e)
        {

        }

        private void btnStar_Click(object sender, EventArgs e)
        {

        }

        private void btnLine_Click(object sender, EventArgs e)
        {
            Point begin = new Point(panWorkRegion.Width / 4, panWorkRegion.Height / 5);
            Point end = new Point(panWorkRegion.Width / 2, panWorkRegion.Height / 4);
            Line l = new Line(begin, end, StyleBuilder.FromGlobalStyles());
            AppGlobalData.Instance.LayerManager.Add(l);
        }

        #endregion
    }
}
