using System;
using System.Drawing;
using System.Windows.Forms;
using VectorImageEdit.Models;
using VectorImageEdit.Modules.BasicShapes;

namespace VectorImageEdit.Forms.AppWindow
{
    public partial class AppWindow
    {
        private void edgecolorToolStripButton_Click(object sender, EventArgs e)
        {
            ColorDialog colorPicker = new ColorDialog();
            DialogResult dlg = colorPicker.ShowDialog();
            if (dlg == DialogResult.OK)
            {
                edgecolorToolStripButton.BackColor = colorPicker.Color;
                AppGlobalData.Instance.ShapeEdgeColor = colorPicker.Color;
            }
        }

        private void fillcolorToolStripButton_Click(object sender, EventArgs e)
        {
            ColorDialog colorPicker = new ColorDialog();
            DialogResult dlg = colorPicker.ShowDialog();
            if (dlg != DialogResult.OK) return;
            fillcolortoolStripButton.BackColor = colorPicker.Color;
            AppGlobalData.Instance.ShapeFillColor = colorPicker.Color;
        }

        private void color1Pick_Click(object sender, EventArgs e)
        {
            edgecolorToolStripButton.BackColor = ((ToolStripItem)sender).BackColor;
            AppGlobalData.Instance.ShapeEdgeColor = ((ToolStripItem)sender).BackColor;
        }

        private void color2Pick_Click(object sender, EventArgs e)
        {
            fillcolortoolStripButton.BackColor = ((ToolStripItem)sender).BackColor;
            AppGlobalData.Instance.ShapeFillColor = ((ToolStripItem)sender).BackColor;
        }

        #region Shape toolbar buttons

        // TODO: Remove direct dependency on shapes here in the View and move it to Model

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
