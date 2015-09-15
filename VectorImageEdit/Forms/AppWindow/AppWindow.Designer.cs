using System.ComponentModel;
using System.Windows.Forms;

namespace VectorImageEdit.Forms.AppWindow
{
    partial class AppWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Node1");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Node2");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Node0", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Node3");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AppWindow));
            this.lBoxActiveLayers = new System.Windows.Forms.ListBox();
            this.cmsRightClickMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsMenuProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsMenuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.menuBarTop = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.openVectorMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.saveVectorMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.exportFileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.exitMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.filePropertiesMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.layerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.layerArrangeMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.layerBringFrontMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.layerSendBackMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.layerSendBackwMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.layerDeleteMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.layerPropertiesMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panWorkRegion = new System.Windows.Forms.Panel();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.leftToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolstripCircleShape = new System.Windows.Forms.ToolStripButton();
            this.toolstripSquareShape = new System.Windows.Forms.ToolStripButton();
            this.toolstripDiamondShape = new System.Windows.Forms.ToolStripButton();
            this.toolstripTriangleShape = new System.Windows.Forms.ToolStripButton();
            this.toolstripHexagonShape = new System.Windows.Forms.ToolStripButton();
            this.toolstripStarShape = new System.Windows.Forms.ToolStripButton();
            this.toolstripEllipseShape = new System.Windows.Forms.ToolStripButton();
            this.toolstripRectangleShape = new System.Windows.Forms.ToolStripButton();
            this.toolstripLineShape = new System.Windows.Forms.ToolStripButton();
            this.topToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolstripPrimaryColorTrigger = new System.Windows.Forms.ToolStripButton();
            this.toolstripPrimaryColorPreview = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripColorPreset1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripColorPreset2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripColorPreset3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripColorPreset4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripColorPreset5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripColorPreset6 = new System.Windows.Forms.ToolStripButton();
            this.toolStripColorPreset7 = new System.Windows.Forms.ToolStripButton();
            this.toolStripColorPreset8 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripColorPick = new System.Windows.Forms.ToolStripButton();
            this.toolstripSecondaryColorTrigger = new System.Windows.Forms.ToolStripButton();
            this.toolstripSecondaryColorPreview = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripColorPreset9 = new System.Windows.Forms.ToolStripButton();
            this.toolStripColorPreset10 = new System.Windows.Forms.ToolStripButton();
            this.toolStripColorPreset11 = new System.Windows.Forms.ToolStripButton();
            this.toolStripColorPreset12 = new System.Windows.Forms.ToolStripButton();
            this.toolStripColorPreset13 = new System.Windows.Forms.ToolStripButton();
            this.toolStripColorPreset14 = new System.Windows.Forms.ToolStripButton();
            this.toolStripColorPreset15 = new System.Windows.Forms.ToolStripButton();
            this.toolStripColorPreset16 = new System.Windows.Forms.ToolStripButton();
            this.toolTipHoverInfo = new System.Windows.Forms.ToolTip(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusActionLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.memoryUsedLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.memoryProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.appBarmenuStrip = new System.Windows.Forms.MenuStrip();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.maximizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minimizetoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.loadingCircleToolStripMenuItem1 = new MRG.Controls.UI.LoadingCircleToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmsRightClickMenu.SuspendLayout();
            this.menuBarTop.SuspendLayout();
            this.panWorkRegion.SuspendLayout();
            this.leftToolStrip.SuspendLayout();
            this.topToolStrip.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.appBarmenuStrip.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lBoxActiveLayers
            // 
            this.lBoxActiveLayers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lBoxActiveLayers.BackColor = System.Drawing.Color.Silver;
            this.lBoxActiveLayers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lBoxActiveLayers.ContextMenuStrip = this.cmsRightClickMenu;
            this.lBoxActiveLayers.FormattingEnabled = true;
            this.lBoxActiveLayers.HorizontalScrollbar = true;
            this.lBoxActiveLayers.Location = new System.Drawing.Point(820, 106);
            this.lBoxActiveLayers.Name = "lBoxActiveLayers";
            this.lBoxActiveLayers.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lBoxActiveLayers.Size = new System.Drawing.Size(145, 327);
            this.lBoxActiveLayers.TabIndex = 0;
            // 
            // cmsRightClickMenu
            // 
            this.cmsRightClickMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsMenuProperties,
            this.cmsMenuDelete});
            this.cmsRightClickMenu.Name = "cmsRightClickMenu";
            this.cmsRightClickMenu.Size = new System.Drawing.Size(128, 48);
            // 
            // cmsMenuProperties
            // 
            this.cmsMenuProperties.ForeColor = System.Drawing.Color.White;
            this.cmsMenuProperties.Name = "cmsMenuProperties";
            this.cmsMenuProperties.Size = new System.Drawing.Size(127, 22);
            this.cmsMenuProperties.Text = "Properties";
            // 
            // cmsMenuDelete
            // 
            this.cmsMenuDelete.ForeColor = System.Drawing.Color.White;
            this.cmsMenuDelete.Name = "cmsMenuDelete";
            this.cmsMenuDelete.Size = new System.Drawing.Size(127, 22);
            this.cmsMenuDelete.Text = "Delete";
            // 
            // menuBarTop
            // 
            this.menuBarTop.AllowItemReorder = true;
            this.menuBarTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.layerToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuBarTop.Location = new System.Drawing.Point(0, 27);
            this.menuBarTop.Name = "menuBarTop";
            this.menuBarTop.Size = new System.Drawing.Size(972, 24);
            this.menuBarTop.TabIndex = 5;
            this.menuBarTop.Text = "menuStrip1";
            this.menuBarTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Borderless_MouseDown);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFileMenu,
            this.openVectorMenu,
            this.toolStripSeparator4,
            this.saveVectorMenu,
            this.exportFileMenu,
            this.toolStripSeparator5,
            this.exitMenu,
            this.filePropertiesMenu});
            this.fileToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.fileToolStripMenuItem.Text = "FILE";
            // 
            // openFileMenu
            // 
            this.openFileMenu.ForeColor = System.Drawing.Color.White;
            this.openFileMenu.Image = global::VectorImageEdit.Properties.Resources.Image_File_50;
            this.openFileMenu.Name = "openFileMenu";
            this.openFileMenu.Size = new System.Drawing.Size(167, 22);
            this.openFileMenu.Text = "Open Image";
            // 
            // openVectorMenu
            // 
            this.openVectorMenu.ForeColor = System.Drawing.Color.White;
            this.openVectorMenu.Image = global::VectorImageEdit.Properties.Resources.Open_Folder_50;
            this.openVectorMenu.Name = "openVectorMenu";
            this.openVectorMenu.Size = new System.Drawing.Size(167, 22);
            this.openVectorMenu.Text = "Open Vector Data";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(164, 6);
            // 
            // saveVectorMenu
            // 
            this.saveVectorMenu.ForeColor = System.Drawing.Color.White;
            this.saveVectorMenu.Name = "saveVectorMenu";
            this.saveVectorMenu.Size = new System.Drawing.Size(167, 22);
            this.saveVectorMenu.Text = "Save Vector Data";
            // 
            // exportFileMenu
            // 
            this.exportFileMenu.ForeColor = System.Drawing.Color.White;
            this.exportFileMenu.Name = "exportFileMenu";
            this.exportFileMenu.Size = new System.Drawing.Size(167, 22);
            this.exportFileMenu.Text = "Export Image";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(164, 6);
            // 
            // exitMenu
            // 
            this.exitMenu.ForeColor = System.Drawing.Color.White;
            this.exitMenu.Image = global::VectorImageEdit.Properties.Resources.Shutdown_50;
            this.exitMenu.Name = "exitMenu";
            this.exitMenu.Size = new System.Drawing.Size(167, 22);
            this.exitMenu.Text = "Exit";
            // 
            // filePropertiesMenu
            // 
            this.filePropertiesMenu.ForeColor = System.Drawing.Color.White;
            this.filePropertiesMenu.Name = "filePropertiesMenu";
            this.filePropertiesMenu.Size = new System.Drawing.Size(167, 22);
            this.filePropertiesMenu.Text = "Properties";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.editToolStripMenuItem.Text = "EDIT";
            // 
            // layerToolStripMenuItem
            // 
            this.layerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.layerArrangeMenu,
            this.layerDeleteMenu,
            this.layerPropertiesMenu});
            this.layerToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.layerToolStripMenuItem.Name = "layerToolStripMenuItem";
            this.layerToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.layerToolStripMenuItem.Text = "LAYER";
            // 
            // layerArrangeMenu
            // 
            this.layerArrangeMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.layerBringFrontMenu,
            this.layerSendBackMenu,
            this.layerSendBackwMenu});
            this.layerArrangeMenu.ForeColor = System.Drawing.Color.White;
            this.layerArrangeMenu.Name = "layerArrangeMenu";
            this.layerArrangeMenu.Size = new System.Drawing.Size(127, 22);
            this.layerArrangeMenu.Text = "Arrange";
            // 
            // layerBringFrontMenu
            // 
            this.layerBringFrontMenu.ForeColor = System.Drawing.Color.White;
            this.layerBringFrontMenu.Name = "layerBringFrontMenu";
            this.layerBringFrontMenu.Size = new System.Drawing.Size(159, 22);
            this.layerBringFrontMenu.Text = "Bring to Front";
            // 
            // layerSendBackMenu
            // 
            this.layerSendBackMenu.ForeColor = System.Drawing.Color.White;
            this.layerSendBackMenu.Name = "layerSendBackMenu";
            this.layerSendBackMenu.Size = new System.Drawing.Size(159, 22);
            this.layerSendBackMenu.Text = "Send to Back";
            // 
            // layerSendBackwMenu
            // 
            this.layerSendBackwMenu.ForeColor = System.Drawing.Color.White;
            this.layerSendBackwMenu.Name = "layerSendBackwMenu";
            this.layerSendBackwMenu.Size = new System.Drawing.Size(159, 22);
            this.layerSendBackwMenu.Text = "Send Backwards";
            // 
            // layerDeleteMenu
            // 
            this.layerDeleteMenu.ForeColor = System.Drawing.Color.White;
            this.layerDeleteMenu.Image = global::VectorImageEdit.Properties.Resources.Delete_50;
            this.layerDeleteMenu.Name = "layerDeleteMenu";
            this.layerDeleteMenu.Size = new System.Drawing.Size(127, 22);
            this.layerDeleteMenu.Text = "Delete";
            // 
            // layerPropertiesMenu
            // 
            this.layerPropertiesMenu.ForeColor = System.Drawing.Color.White;
            this.layerPropertiesMenu.Image = global::VectorImageEdit.Properties.Resources.Edit_50;
            this.layerPropertiesMenu.Name = "layerPropertiesMenu";
            this.layerPropertiesMenu.Size = new System.Drawing.Size(127, 22);
            this.layerPropertiesMenu.Text = "Properties";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.helpToolStripMenuItem.Text = "HELP";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // panWorkRegion
            // 
            this.panWorkRegion.AllowDrop = true;
            this.panWorkRegion.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panWorkRegion.BackColor = System.Drawing.Color.Silver;
            this.panWorkRegion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panWorkRegion.ContextMenuStrip = this.cmsRightClickMenu;
            this.panWorkRegion.Controls.Add(this.treeView1);
            this.panWorkRegion.Location = new System.Drawing.Point(29, 106);
            this.panWorkRegion.Name = "panWorkRegion";
            this.panWorkRegion.Size = new System.Drawing.Size(787, 335);
            this.panWorkRegion.TabIndex = 6;
            // 
            // treeView1
            // 
            this.treeView1.BackColor = System.Drawing.Color.Gray;
            this.treeView1.FullRowSelect = true;
            this.treeView1.Indent = 15;
            this.treeView1.LabelEdit = true;
            this.treeView1.Location = new System.Drawing.Point(624, 163);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "Node1";
            treeNode1.Text = "Node1";
            treeNode2.Name = "Node2";
            treeNode2.Text = "Node2";
            treeNode3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            treeNode3.Name = "Node0";
            treeNode3.Text = "Node0";
            treeNode4.BackColor = System.Drawing.Color.Aqua;
            treeNode4.Name = "Node3";
            treeNode4.Text = "Node3";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode3,
            treeNode4});
            this.treeView1.ShowLines = false;
            this.treeView1.ShowNodeToolTips = true;
            this.treeView1.Size = new System.Drawing.Size(129, 141);
            this.treeView1.TabIndex = 10;
            this.treeView1.Visible = false;
            // 
            // leftToolStrip
            // 
            this.leftToolStrip.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolstripCircleShape,
            this.toolstripSquareShape,
            this.toolstripDiamondShape,
            this.toolstripTriangleShape,
            this.toolstripHexagonShape,
            this.toolstripStarShape,
            this.toolstripEllipseShape,
            this.toolstripRectangleShape,
            this.toolstripLineShape});
            this.leftToolStrip.Location = new System.Drawing.Point(0, 51);
            this.leftToolStrip.Margin = new System.Windows.Forms.Padding(16, 0, 0, 0);
            this.leftToolStrip.Name = "leftToolStrip";
            this.leftToolStrip.Size = new System.Drawing.Size(26, 417);
            this.leftToolStrip.TabIndex = 12;
            this.leftToolStrip.Text = "toolStrip2";
            // 
            // toolstripCircleShape
            // 
            this.toolstripCircleShape.AutoSize = false;
            this.toolstripCircleShape.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolstripCircleShape.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolstripCircleShape.Image = global::VectorImageEdit.Properties.Resources.Circled_50;
            this.toolstripCircleShape.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolstripCircleShape.Name = "toolstripCircleShape";
            this.toolstripCircleShape.Size = new System.Drawing.Size(25, 25);
            this.toolstripCircleShape.Text = "toolStripButton1";
            this.toolstripCircleShape.ToolTipText = "Circle";
            // 
            // toolstripSquareShape
            // 
            this.toolstripSquareShape.AutoSize = false;
            this.toolstripSquareShape.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolstripSquareShape.Image = global::VectorImageEdit.Properties.Resources.Rectangle_Stroked_50;
            this.toolstripSquareShape.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolstripSquareShape.Name = "toolstripSquareShape";
            this.toolstripSquareShape.Size = new System.Drawing.Size(25, 25);
            this.toolstripSquareShape.Text = "toolStripButton2";
            this.toolstripSquareShape.ToolTipText = "Square";
            // 
            // toolstripDiamondShape
            // 
            this.toolstripDiamondShape.AutoSize = false;
            this.toolstripDiamondShape.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolstripDiamondShape.Image = global::VectorImageEdit.Properties.Resources.Diamond_50;
            this.toolstripDiamondShape.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolstripDiamondShape.Name = "toolstripDiamondShape";
            this.toolstripDiamondShape.Size = new System.Drawing.Size(25, 25);
            this.toolstripDiamondShape.Text = "toolStripButton3";
            this.toolstripDiamondShape.ToolTipText = "Diamond";
            // 
            // toolstripTriangleShape
            // 
            this.toolstripTriangleShape.AutoSize = false;
            this.toolstripTriangleShape.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolstripTriangleShape.Image = global::VectorImageEdit.Properties.Resources.Triangle_50;
            this.toolstripTriangleShape.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolstripTriangleShape.Name = "toolstripTriangleShape";
            this.toolstripTriangleShape.Size = new System.Drawing.Size(25, 25);
            this.toolstripTriangleShape.Text = "toolStripButton4";
            this.toolstripTriangleShape.ToolTipText = "Triangle";
            // 
            // toolstripHexagonShape
            // 
            this.toolstripHexagonShape.AutoSize = false;
            this.toolstripHexagonShape.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolstripHexagonShape.Image = global::VectorImageEdit.Properties.Resources.Hexagon_50;
            this.toolstripHexagonShape.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolstripHexagonShape.Name = "toolstripHexagonShape";
            this.toolstripHexagonShape.Size = new System.Drawing.Size(25, 25);
            this.toolstripHexagonShape.Text = "toolStripButton5";
            this.toolstripHexagonShape.ToolTipText = "Hexagon";
            // 
            // toolstripStarShape
            // 
            this.toolstripStarShape.AutoSize = false;
            this.toolstripStarShape.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolstripStarShape.Image = global::VectorImageEdit.Properties.Resources.Star_50;
            this.toolstripStarShape.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolstripStarShape.Name = "toolstripStarShape";
            this.toolstripStarShape.Size = new System.Drawing.Size(25, 25);
            this.toolstripStarShape.Text = "toolStripButton6";
            this.toolstripStarShape.ToolTipText = "Star";
            // 
            // toolstripEllipseShape
            // 
            this.toolstripEllipseShape.AutoSize = false;
            this.toolstripEllipseShape.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolstripEllipseShape.Image = global::VectorImageEdit.Properties.Resources.Ellipse_50;
            this.toolstripEllipseShape.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolstripEllipseShape.Name = "toolstripEllipseShape";
            this.toolstripEllipseShape.Size = new System.Drawing.Size(25, 25);
            this.toolstripEllipseShape.Text = "toolStripButton7";
            this.toolstripEllipseShape.ToolTipText = "Ellipse";
            // 
            // toolstripRectangleShape
            // 
            this.toolstripRectangleShape.AutoSize = false;
            this.toolstripRectangleShape.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolstripRectangleShape.Image = global::VectorImageEdit.Properties.Resources.Rectangle_Stroked_50;
            this.toolstripRectangleShape.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolstripRectangleShape.Name = "toolstripRectangleShape";
            this.toolstripRectangleShape.Size = new System.Drawing.Size(25, 25);
            this.toolstripRectangleShape.Text = "toolStripButton8";
            this.toolstripRectangleShape.ToolTipText = "Rectangle";
            // 
            // toolstripLineShape
            // 
            this.toolstripLineShape.AutoSize = false;
            this.toolstripLineShape.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolstripLineShape.Image = global::VectorImageEdit.Properties.Resources.line;
            this.toolstripLineShape.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolstripLineShape.Name = "toolstripLineShape";
            this.toolstripLineShape.Size = new System.Drawing.Size(25, 25);
            this.toolstripLineShape.Text = "toolStripButton1";
            this.toolstripLineShape.ToolTipText = "Line";
            // 
            // topToolStrip
            // 
            this.topToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolstripPrimaryColorTrigger,
            this.toolstripPrimaryColorPreview,
            this.toolStripSeparator3,
            this.toolStripColorPreset1,
            this.toolStripColorPreset2,
            this.toolStripColorPreset3,
            this.toolStripColorPreset4,
            this.toolStripColorPreset5,
            this.toolStripColorPreset6,
            this.toolStripColorPreset7,
            this.toolStripColorPreset8,
            this.toolStripSeparator2,
            this.toolStripColorPick,
            this.toolstripSecondaryColorTrigger,
            this.toolstripSecondaryColorPreview,
            this.toolStripSeparator1,
            this.toolStripColorPreset9,
            this.toolStripColorPreset10,
            this.toolStripColorPreset11,
            this.toolStripColorPreset12,
            this.toolStripColorPreset13,
            this.toolStripColorPreset14,
            this.toolStripColorPreset15,
            this.toolStripColorPreset16});
            this.topToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.topToolStrip.Location = new System.Drawing.Point(26, 51);
            this.topToolStrip.Name = "topToolStrip";
            this.topToolStrip.Size = new System.Drawing.Size(946, 25);
            this.topToolStrip.TabIndex = 11;
            this.topToolStrip.Text = "toolStrip1";
            // 
            // toolstripPrimaryColorTrigger
            // 
            this.toolstripPrimaryColorTrigger.AutoToolTip = false;
            this.toolstripPrimaryColorTrigger.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolstripPrimaryColorTrigger.ForeColor = System.Drawing.Color.White;
            this.toolstripPrimaryColorTrigger.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolstripPrimaryColorTrigger.Name = "toolstripPrimaryColorTrigger";
            this.toolstripPrimaryColorTrigger.Size = new System.Drawing.Size(49, 22);
            this.toolstripPrimaryColorTrigger.Text = "Color 1";
            this.toolstripPrimaryColorTrigger.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolstripPrimaryColorPreview
            // 
            this.toolstripPrimaryColorPreview.AutoSize = false;
            this.toolstripPrimaryColorPreview.BackColor = System.Drawing.Color.OrangeRed;
            this.toolstripPrimaryColorPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolstripPrimaryColorPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolstripPrimaryColorPreview.Name = "toolstripPrimaryColorPreview";
            this.toolstripPrimaryColorPreview.Size = new System.Drawing.Size(20, 20);
            this.toolstripPrimaryColorPreview.Text = "toolStripButton2";
            this.toolstripPrimaryColorPreview.ToolTipText = " ";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripColorPreset1
            // 
            this.toolStripColorPreset1.AutoSize = false;
            this.toolStripColorPreset1.AutoToolTip = false;
            this.toolStripColorPreset1.BackColor = System.Drawing.Color.Gray;
            this.toolStripColorPreset1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripColorPreset1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripColorPreset1.Name = "toolStripColorPreset1";
            this.toolStripColorPreset1.Size = new System.Drawing.Size(20, 20);
            this.toolStripColorPreset1.Text = "toolStripButton2";
            this.toolStripColorPreset1.ToolTipText = " ";
            // 
            // toolStripColorPreset2
            // 
            this.toolStripColorPreset2.AutoSize = false;
            this.toolStripColorPreset2.AutoToolTip = false;
            this.toolStripColorPreset2.BackColor = System.Drawing.Color.Red;
            this.toolStripColorPreset2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripColorPreset2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripColorPreset2.Name = "toolStripColorPreset2";
            this.toolStripColorPreset2.Size = new System.Drawing.Size(20, 20);
            this.toolStripColorPreset2.Text = "toolStripButton3";
            this.toolStripColorPreset2.ToolTipText = " ";
            // 
            // toolStripColorPreset3
            // 
            this.toolStripColorPreset3.AutoSize = false;
            this.toolStripColorPreset3.AutoToolTip = false;
            this.toolStripColorPreset3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.toolStripColorPreset3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripColorPreset3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripColorPreset3.Name = "toolStripColorPreset3";
            this.toolStripColorPreset3.Size = new System.Drawing.Size(20, 20);
            this.toolStripColorPreset3.Text = "toolStripButton4";
            this.toolStripColorPreset3.ToolTipText = " ";
            // 
            // toolStripColorPreset4
            // 
            this.toolStripColorPreset4.AutoSize = false;
            this.toolStripColorPreset4.AutoToolTip = false;
            this.toolStripColorPreset4.BackColor = System.Drawing.Color.Yellow;
            this.toolStripColorPreset4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripColorPreset4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripColorPreset4.Name = "toolStripColorPreset4";
            this.toolStripColorPreset4.Size = new System.Drawing.Size(20, 20);
            this.toolStripColorPreset4.Text = "toolStripButton5";
            this.toolStripColorPreset4.ToolTipText = " ";
            // 
            // toolStripColorPreset5
            // 
            this.toolStripColorPreset5.AutoSize = false;
            this.toolStripColorPreset5.AutoToolTip = false;
            this.toolStripColorPreset5.BackColor = System.Drawing.Color.Lime;
            this.toolStripColorPreset5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripColorPreset5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripColorPreset5.Name = "toolStripColorPreset5";
            this.toolStripColorPreset5.Size = new System.Drawing.Size(20, 20);
            this.toolStripColorPreset5.Text = "toolStripButton6";
            this.toolStripColorPreset5.ToolTipText = " ";
            // 
            // toolStripColorPreset6
            // 
            this.toolStripColorPreset6.AutoSize = false;
            this.toolStripColorPreset6.AutoToolTip = false;
            this.toolStripColorPreset6.BackColor = System.Drawing.Color.Cyan;
            this.toolStripColorPreset6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripColorPreset6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripColorPreset6.Name = "toolStripColorPreset6";
            this.toolStripColorPreset6.Size = new System.Drawing.Size(20, 20);
            this.toolStripColorPreset6.Text = "toolStripButton7";
            this.toolStripColorPreset6.ToolTipText = " ";
            // 
            // toolStripColorPreset7
            // 
            this.toolStripColorPreset7.AutoSize = false;
            this.toolStripColorPreset7.AutoToolTip = false;
            this.toolStripColorPreset7.BackColor = System.Drawing.Color.Blue;
            this.toolStripColorPreset7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripColorPreset7.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripColorPreset7.Name = "toolStripColorPreset7";
            this.toolStripColorPreset7.Size = new System.Drawing.Size(20, 20);
            this.toolStripColorPreset7.Text = "toolStripButton8";
            this.toolStripColorPreset7.ToolTipText = " ";
            // 
            // toolStripColorPreset8
            // 
            this.toolStripColorPreset8.AutoSize = false;
            this.toolStripColorPreset8.AutoToolTip = false;
            this.toolStripColorPreset8.BackColor = System.Drawing.Color.Magenta;
            this.toolStripColorPreset8.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripColorPreset8.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripColorPreset8.Name = "toolStripColorPreset8";
            this.toolStripColorPreset8.Size = new System.Drawing.Size(20, 20);
            this.toolStripColorPreset8.Text = "toolStripButton9";
            this.toolStripColorPreset8.ToolTipText = " ";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 2);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripColorPick
            // 
            this.toolStripColorPick.AutoSize = false;
            this.toolStripColorPick.BackgroundImage = global::VectorImageEdit.Properties.Resources.lines_520432_640;
            this.toolStripColorPick.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolStripColorPick.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripColorPick.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripColorPick.Name = "toolStripColorPick";
            this.toolStripColorPick.Size = new System.Drawing.Size(20, 20);
            this.toolStripColorPick.Text = "toolStripButton18";
            this.toolStripColorPick.ToolTipText = "Customize Colors";
            // 
            // toolstripSecondaryColorTrigger
            // 
            this.toolstripSecondaryColorTrigger.AutoToolTip = false;
            this.toolstripSecondaryColorTrigger.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolstripSecondaryColorTrigger.ForeColor = System.Drawing.Color.White;
            this.toolstripSecondaryColorTrigger.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolstripSecondaryColorTrigger.Name = "toolstripSecondaryColorTrigger";
            this.toolstripSecondaryColorTrigger.Size = new System.Drawing.Size(49, 22);
            this.toolstripSecondaryColorTrigger.Text = "Color 2";
            // 
            // toolstripSecondaryColorPreview
            // 
            this.toolstripSecondaryColorPreview.AutoSize = false;
            this.toolstripSecondaryColorPreview.BackColor = System.Drawing.Color.LightBlue;
            this.toolstripSecondaryColorPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolstripSecondaryColorPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolstripSecondaryColorPreview.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.toolstripSecondaryColorPreview.Name = "toolstripSecondaryColorPreview";
            this.toolstripSecondaryColorPreview.Size = new System.Drawing.Size(20, 20);
            this.toolstripSecondaryColorPreview.Text = "toolStripButton1";
            this.toolstripSecondaryColorPreview.ToolTipText = " ";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 2);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripColorPreset9
            // 
            this.toolStripColorPreset9.AutoSize = false;
            this.toolStripColorPreset9.AutoToolTip = false;
            this.toolStripColorPreset9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.toolStripColorPreset9.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripColorPreset9.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripColorPreset9.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.toolStripColorPreset9.Name = "toolStripColorPreset9";
            this.toolStripColorPreset9.Size = new System.Drawing.Size(20, 20);
            this.toolStripColorPreset9.Text = "toolStripButton10";
            // 
            // toolStripColorPreset10
            // 
            this.toolStripColorPreset10.AutoSize = false;
            this.toolStripColorPreset10.AutoToolTip = false;
            this.toolStripColorPreset10.BackColor = System.Drawing.Color.Maroon;
            this.toolStripColorPreset10.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripColorPreset10.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripColorPreset10.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.toolStripColorPreset10.Name = "toolStripColorPreset10";
            this.toolStripColorPreset10.Size = new System.Drawing.Size(20, 20);
            this.toolStripColorPreset10.Text = "toolStripButton11";
            // 
            // toolStripColorPreset11
            // 
            this.toolStripColorPreset11.AutoSize = false;
            this.toolStripColorPreset11.AutoToolTip = false;
            this.toolStripColorPreset11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.toolStripColorPreset11.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripColorPreset11.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripColorPreset11.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.toolStripColorPreset11.Name = "toolStripColorPreset11";
            this.toolStripColorPreset11.Size = new System.Drawing.Size(20, 20);
            this.toolStripColorPreset11.Text = "toolStripButton12";
            // 
            // toolStripColorPreset12
            // 
            this.toolStripColorPreset12.AutoSize = false;
            this.toolStripColorPreset12.AutoToolTip = false;
            this.toolStripColorPreset12.BackColor = System.Drawing.Color.Olive;
            this.toolStripColorPreset12.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripColorPreset12.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripColorPreset12.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.toolStripColorPreset12.Name = "toolStripColorPreset12";
            this.toolStripColorPreset12.Size = new System.Drawing.Size(20, 20);
            this.toolStripColorPreset12.Text = "toolStripButton13";
            // 
            // toolStripColorPreset13
            // 
            this.toolStripColorPreset13.AutoSize = false;
            this.toolStripColorPreset13.AutoToolTip = false;
            this.toolStripColorPreset13.BackColor = System.Drawing.Color.Green;
            this.toolStripColorPreset13.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripColorPreset13.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripColorPreset13.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.toolStripColorPreset13.Name = "toolStripColorPreset13";
            this.toolStripColorPreset13.Size = new System.Drawing.Size(20, 20);
            this.toolStripColorPreset13.Text = "toolStripButton14";
            // 
            // toolStripColorPreset14
            // 
            this.toolStripColorPreset14.AutoSize = false;
            this.toolStripColorPreset14.AutoToolTip = false;
            this.toolStripColorPreset14.BackColor = System.Drawing.Color.Teal;
            this.toolStripColorPreset14.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripColorPreset14.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripColorPreset14.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.toolStripColorPreset14.Name = "toolStripColorPreset14";
            this.toolStripColorPreset14.Size = new System.Drawing.Size(20, 20);
            this.toolStripColorPreset14.Text = "toolStripButton17";
            // 
            // toolStripColorPreset15
            // 
            this.toolStripColorPreset15.AutoSize = false;
            this.toolStripColorPreset15.AutoToolTip = false;
            this.toolStripColorPreset15.BackColor = System.Drawing.Color.Navy;
            this.toolStripColorPreset15.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripColorPreset15.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripColorPreset15.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.toolStripColorPreset15.Name = "toolStripColorPreset15";
            this.toolStripColorPreset15.Size = new System.Drawing.Size(20, 20);
            this.toolStripColorPreset15.Text = "toolStripButton15";
            // 
            // toolStripColorPreset16
            // 
            this.toolStripColorPreset16.AutoSize = false;
            this.toolStripColorPreset16.AutoToolTip = false;
            this.toolStripColorPreset16.BackColor = System.Drawing.Color.Purple;
            this.toolStripColorPreset16.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripColorPreset16.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripColorPreset16.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.toolStripColorPreset16.Name = "toolStripColorPreset16";
            this.toolStripColorPreset16.Size = new System.Drawing.Size(20, 20);
            this.toolStripColorPreset16.Text = "toolStripButton16";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusActionLabel,
            this.statusProgressBar,
            this.toolStripStatusLabel1,
            this.memoryUsedLabel,
            this.memoryProgressBar});
            this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.statusStrip1.Location = new System.Drawing.Point(26, 446);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            this.statusStrip1.Size = new System.Drawing.Size(946, 22);
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusActionLabel
            // 
            this.statusActionLabel.ForeColor = System.Drawing.Color.White;
            this.statusActionLabel.Name = "statusActionLabel";
            this.statusActionLabel.Size = new System.Drawing.Size(59, 15);
            this.statusActionLabel.Text = "No action";
            // 
            // statusProgressBar
            // 
            this.statusProgressBar.MarqueeAnimationSpeed = 50;
            this.statusProgressBar.Name = "statusProgressBar";
            this.statusProgressBar.Size = new System.Drawing.Size(100, 16);
            this.statusProgressBar.Visible = false;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripStatusLabel1.Image = global::VectorImageEdit.Properties.Resources.Memory_Slot_50;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(16, 16);
            // 
            // memoryUsedLabel
            // 
            this.memoryUsedLabel.ForeColor = System.Drawing.Color.White;
            this.memoryUsedLabel.Name = "memoryUsedLabel";
            this.memoryUsedLabel.Size = new System.Drawing.Size(46, 15);
            this.memoryUsedLabel.Text = "15.2MB";
            // 
            // memoryProgressBar
            // 
            this.memoryProgressBar.Name = "memoryProgressBar";
            this.memoryProgressBar.Size = new System.Drawing.Size(116, 16);
            // 
            // appBarmenuStrip
            // 
            this.appBarmenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeToolStripMenuItem,
            this.maximizeToolStripMenuItem,
            this.minimizetoolStripMenuItem,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.loadingCircleToolStripMenuItem1});
            this.appBarmenuStrip.Location = new System.Drawing.Point(0, 0);
            this.appBarmenuStrip.Name = "appBarmenuStrip";
            this.appBarmenuStrip.Size = new System.Drawing.Size(972, 27);
            this.appBarmenuStrip.TabIndex = 14;
            this.appBarmenuStrip.Text = "menuStrip1";
            this.appBarmenuStrip.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.appBarmenuStrip_MouseDoubleClick);
            this.appBarmenuStrip.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Borderless_MouseDown);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.closeToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closeToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(29, 23);
            this.closeToolStripMenuItem.Text = "X";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // maximizeToolStripMenuItem
            // 
            this.maximizeToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.maximizeToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maximizeToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.maximizeToolStripMenuItem.Name = "maximizeToolStripMenuItem";
            this.maximizeToolStripMenuItem.Size = new System.Drawing.Size(28, 23);
            this.maximizeToolStripMenuItem.Text = "□";
            this.maximizeToolStripMenuItem.Click += new System.EventHandler(this.maximizeToolStripMenuItem_Click);
            // 
            // minimizetoolStripMenuItem
            // 
            this.minimizetoolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.minimizetoolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.minimizetoolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.minimizetoolStripMenuItem.Name = "minimizetoolStripMenuItem";
            this.minimizetoolStripMenuItem.Size = new System.Drawing.Size(25, 23);
            this.minimizetoolStripMenuItem.Text = "_";
            this.minimizetoolStripMenuItem.Click += new System.EventHandler(this.minimizetoolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.AutoSize = false;
            this.toolStripMenuItem3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripMenuItem3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem3.Image")));
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(28, 23);
            this.toolStripMenuItem3.Text = "toolStripMenuItem3";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripMenuItem4.ForeColor = System.Drawing.Color.LightGray;
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(106, 23);
            this.toolStripMenuItem4.Text = "VectorImageEdit";
            // 
            // loadingCircleToolStripMenuItem1
            // 
            this.loadingCircleToolStripMenuItem1.AutoSize = false;
            this.loadingCircleToolStripMenuItem1.BackColor = System.Drawing.Color.Transparent;
            // 
            // loadingCircleToolStripMenuItem1
            // 
            this.loadingCircleToolStripMenuItem1.LoadingCircleControl.AccessibleName = "loadingCircleToolStripMenuItem1";
            this.loadingCircleToolStripMenuItem1.LoadingCircleControl.Active = true;
            this.loadingCircleToolStripMenuItem1.LoadingCircleControl.BackColor = System.Drawing.Color.Transparent;
            this.loadingCircleToolStripMenuItem1.LoadingCircleControl.Color = System.Drawing.Color.White;
            this.loadingCircleToolStripMenuItem1.LoadingCircleControl.InnerCircleRadius = 6;
            this.loadingCircleToolStripMenuItem1.LoadingCircleControl.Location = new System.Drawing.Point(150, 3);
            this.loadingCircleToolStripMenuItem1.LoadingCircleControl.Name = "loadingCircleToolStripMenuItem1";
            this.loadingCircleToolStripMenuItem1.LoadingCircleControl.NumberSpoke = 9;
            this.loadingCircleToolStripMenuItem1.LoadingCircleControl.OuterCircleRadius = 7;
            this.loadingCircleToolStripMenuItem1.LoadingCircleControl.RotationSpeed = 100;
            this.loadingCircleToolStripMenuItem1.LoadingCircleControl.Size = new System.Drawing.Size(20, 20);
            this.loadingCircleToolStripMenuItem1.LoadingCircleControl.SpokeThickness = 4;
            this.loadingCircleToolStripMenuItem1.LoadingCircleControl.StylePreset = MRG.Controls.UI.LoadingCircle.StylePresets.Firefox;
            this.loadingCircleToolStripMenuItem1.LoadingCircleControl.TabIndex = 1;
            this.loadingCircleToolStripMenuItem1.LoadingCircleControl.Text = "loadingCircleToolStripMenuItem1";
            this.loadingCircleToolStripMenuItem1.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
            this.loadingCircleToolStripMenuItem1.Name = "loadingCircleToolStripMenuItem1";
            this.loadingCircleToolStripMenuItem1.Size = new System.Drawing.Size(20, 20);
            this.loadingCircleToolStripMenuItem1.Text = "loadingCircleToolStripMenuItem1";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.SlateGray;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.panWorkRegion);
            this.panel1.Controls.Add(this.topToolStrip);
            this.panel1.Controls.Add(this.statusStrip1);
            this.panel1.Controls.Add(this.leftToolStrip);
            this.panel1.Controls.Add(this.menuBarTop);
            this.panel1.Controls.Add(this.lBoxActiveLayers);
            this.panel1.Controls.Add(this.appBarmenuStrip);
            this.panel1.Location = new System.Drawing.Point(5, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(974, 470);
            this.panel1.TabIndex = 15;
            // 
            // AppWindow
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SlateGray;
            this.ClientSize = new System.Drawing.Size(985, 480);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MainMenuStrip = this.menuBarTop;
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "AppWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Vector Image Editor";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Borderless_MouseDown);
            this.cmsRightClickMenu.ResumeLayout(false);
            this.menuBarTop.ResumeLayout(false);
            this.menuBarTop.PerformLayout();
            this.panWorkRegion.ResumeLayout(false);
            this.leftToolStrip.ResumeLayout(false);
            this.leftToolStrip.PerformLayout();
            this.topToolStrip.ResumeLayout(false);
            this.topToolStrip.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.appBarmenuStrip.ResumeLayout(false);
            this.appBarmenuStrip.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ListBox lBoxActiveLayers;
        private MenuStrip menuBarTop;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem exitMenu;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem openFileMenu;
        private Panel panWorkRegion;
        private ToolStripMenuItem saveVectorMenu;
        private ToolStripMenuItem filePropertiesMenu;
        private ToolStripMenuItem layerToolStripMenuItem;
        private ToolStripMenuItem layerArrangeMenu;
        private ToolStripMenuItem layerBringFrontMenu;
        private ToolStripMenuItem layerSendBackMenu;
        private ToolStripMenuItem layerSendBackwMenu;
        private ToolStripMenuItem layerPropertiesMenu;
        private ContextMenuStrip cmsRightClickMenu;
        private ToolStripMenuItem cmsMenuProperties;
        private ToolStripMenuItem cmsMenuDelete;
        private ToolStripMenuItem layerDeleteMenu;
        private ToolTip toolTipHoverInfo;
        private ToolStripMenuItem exportFileMenu;
        private ToolStripMenuItem openVectorMenu;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel statusActionLabel;
        private ToolStripProgressBar statusProgressBar;
        private ToolStripStatusLabel memoryUsedLabel;
        private ToolStripProgressBar memoryProgressBar;
        private TreeView treeView1;
        private ToolStrip topToolStrip;
        private ToolStripButton toolstripSecondaryColorPreview;
        private ToolStripButton toolstripPrimaryColorPreview;
        private ToolStrip leftToolStrip;
        private ToolStripButton toolstripCircleShape;
        private MenuStrip appBarmenuStrip;
        private ToolStripMenuItem closeToolStripMenuItem;
        private ToolStripMenuItem maximizeToolStripMenuItem;
        private ToolStripMenuItem minimizetoolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem toolStripMenuItem4;
        private MRG.Controls.UI.LoadingCircleToolStripMenuItem loadingCircleToolStripMenuItem1;
        private ToolStripButton toolstripSquareShape;
        private ToolStripButton toolstripDiamondShape;
        private ToolStripButton toolstripTriangleShape;
        private ToolStripButton toolstripHexagonShape;
        private ToolStripButton toolstripStarShape;
        private ToolStripButton toolstripEllipseShape;
        private ToolStripButton toolstripRectangleShape;
        private ToolStripButton toolstripLineShape;
        private ToolStripButton toolStripColorPreset1;
        private ToolStripButton toolStripColorPreset2;
        private ToolStripButton toolStripColorPreset3;
        private ToolStripButton toolStripColorPreset4;
        private ToolStripButton toolStripColorPreset5;
        private ToolStripButton toolStripColorPreset6;
        private ToolStripButton toolStripColorPreset7;
        private ToolStripButton toolStripColorPreset8;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton toolStripColorPreset9;
        private ToolStripButton toolStripColorPreset10;
        private ToolStripButton toolStripColorPreset11;
        private ToolStripButton toolStripColorPreset12;
        private ToolStripButton toolStripColorPreset13;
        private ToolStripButton toolStripColorPreset14;
        private ToolStripButton toolStripColorPreset15;
        private ToolStripButton toolStripColorPreset16;
        private ToolStripSeparator toolStripSeparator1;
        private Panel panel1;
        private ToolStripButton toolStripColorPick;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton toolstripPrimaryColorTrigger;
        private ToolStripButton toolstripSecondaryColorTrigger;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripStatusLabel toolStripStatusLabel1;
    }
}

