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
            this.saveVectorMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.exportFileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.filePropertiesMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.exitMenu = new System.Windows.Forms.ToolStripMenuItem();
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
            this.circleToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.squareToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.diamondToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.triangleToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.hexagon = new System.Windows.Forms.ToolStripButton();
            this.starToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.ovalToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.rectangleToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.topToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.edgecolorToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton8 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton9 = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.fillcolortoolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton10 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton11 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton12 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton13 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton14 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton17 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton15 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton16 = new System.Windows.Forms.ToolStripButton();
            this.toolTipHoverInfo = new System.Windows.Forms.ToolTip(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusActionLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusProgressBar = new System.Windows.Forms.ToolStripProgressBar();
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
            this.lBoxActiveLayers.Location = new System.Drawing.Point(940, 106);
            this.lBoxActiveLayers.Name = "lBoxActiveLayers";
            this.lBoxActiveLayers.Size = new System.Drawing.Size(145, 366);
            this.lBoxActiveLayers.TabIndex = 0;
            // 
            // cmsRightClickMenu
            // 
            this.cmsRightClickMenu.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.cmsRightClickMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsMenuProperties,
            this.cmsMenuDelete});
            this.cmsRightClickMenu.Name = "cmsRightClickMenu";
            this.cmsRightClickMenu.Size = new System.Drawing.Size(128, 48);
            // 
            // cmsMenuProperties
            // 
            this.cmsMenuProperties.Name = "cmsMenuProperties";
            this.cmsMenuProperties.Size = new System.Drawing.Size(127, 22);
            this.cmsMenuProperties.Text = "Properties";
            this.cmsMenuProperties.Click += new System.EventHandler(this.cmsMenuProperties_Click);
            // 
            // cmsMenuDelete
            // 
            this.cmsMenuDelete.Name = "cmsMenuDelete";
            this.cmsMenuDelete.Size = new System.Drawing.Size(127, 22);
            this.cmsMenuDelete.Text = "Delete";
            this.cmsMenuDelete.Click += new System.EventHandler(this.cmsMenuDelete_Click);
            // 
            // menuBarTop
            // 
            this.menuBarTop.AllowItemReorder = true;
            this.menuBarTop.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.menuBarTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.layerToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuBarTop.Location = new System.Drawing.Point(0, 27);
            this.menuBarTop.Name = "menuBarTop";
            this.menuBarTop.Size = new System.Drawing.Size(1092, 24);
            this.menuBarTop.TabIndex = 5;
            this.menuBarTop.Text = "menuStrip1";
            this.menuBarTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Borderless_MouseDown);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFileMenu,
            this.openVectorMenu,
            this.saveVectorMenu,
            this.exportFileMenu,
            this.filePropertiesMenu,
            this.exitMenu});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openFileMenu
            // 
            this.openFileMenu.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.openFileMenu.Name = "openFileMenu";
            this.openFileMenu.Size = new System.Drawing.Size(167, 22);
            this.openFileMenu.Text = "Open Image";
            this.openFileMenu.Click += new System.EventHandler(this.openFileMenu_Click);
            // 
            // openVectorMenu
            // 
            this.openVectorMenu.BackColor = System.Drawing.Color.DimGray;
            this.openVectorMenu.Name = "openVectorMenu";
            this.openVectorMenu.Size = new System.Drawing.Size(167, 22);
            this.openVectorMenu.Text = "Open Vector Data";
            this.openVectorMenu.Click += new System.EventHandler(this.openVectorMenu_Click);
            // 
            // saveVectorMenu
            // 
            this.saveVectorMenu.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.saveVectorMenu.Name = "saveVectorMenu";
            this.saveVectorMenu.Size = new System.Drawing.Size(167, 22);
            this.saveVectorMenu.Text = "Save Vector Data";
            this.saveVectorMenu.Click += new System.EventHandler(this.saveVectorMenu_Click);
            // 
            // exportFileMenu
            // 
            this.exportFileMenu.BackColor = System.Drawing.Color.DimGray;
            this.exportFileMenu.Name = "exportFileMenu";
            this.exportFileMenu.Size = new System.Drawing.Size(167, 22);
            this.exportFileMenu.Text = "Export Image";
            this.exportFileMenu.Click += new System.EventHandler(this.exportFileMenu_Click);
            // 
            // filePropertiesMenu
            // 
            this.filePropertiesMenu.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.filePropertiesMenu.Name = "filePropertiesMenu";
            this.filePropertiesMenu.Size = new System.Drawing.Size(167, 22);
            this.filePropertiesMenu.Text = "Properties";
            // 
            // exitMenu
            // 
            this.exitMenu.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.exitMenu.Name = "exitMenu";
            this.exitMenu.Size = new System.Drawing.Size(167, 22);
            this.exitMenu.Text = "Exit";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // layerToolStripMenuItem
            // 
            this.layerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.layerArrangeMenu,
            this.layerDeleteMenu,
            this.layerPropertiesMenu});
            this.layerToolStripMenuItem.Name = "layerToolStripMenuItem";
            this.layerToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.layerToolStripMenuItem.Text = "Layer";
            // 
            // layerArrangeMenu
            // 
            this.layerArrangeMenu.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.layerArrangeMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.layerBringFrontMenu,
            this.layerSendBackMenu,
            this.layerSendBackwMenu});
            this.layerArrangeMenu.Name = "layerArrangeMenu";
            this.layerArrangeMenu.Size = new System.Drawing.Size(127, 22);
            this.layerArrangeMenu.Text = "Arrange";
            // 
            // layerBringFrontMenu
            // 
            this.layerBringFrontMenu.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.layerBringFrontMenu.Name = "layerBringFrontMenu";
            this.layerBringFrontMenu.Size = new System.Drawing.Size(159, 22);
            this.layerBringFrontMenu.Text = "Bring to Front";
            this.layerBringFrontMenu.Click += new System.EventHandler(this.layerBringFrontMenu_Click);
            // 
            // layerSendBackMenu
            // 
            this.layerSendBackMenu.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.layerSendBackMenu.Name = "layerSendBackMenu";
            this.layerSendBackMenu.Size = new System.Drawing.Size(159, 22);
            this.layerSendBackMenu.Text = "Send to Back";
            this.layerSendBackMenu.Click += new System.EventHandler(this.layerSendBackMenu_Click);
            // 
            // layerSendBackwMenu
            // 
            this.layerSendBackwMenu.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.layerSendBackwMenu.Name = "layerSendBackwMenu";
            this.layerSendBackwMenu.Size = new System.Drawing.Size(159, 22);
            this.layerSendBackwMenu.Text = "Send Backwards";
            this.layerSendBackwMenu.Click += new System.EventHandler(this.layerSendBackwMenu_Click);
            // 
            // layerDeleteMenu
            // 
            this.layerDeleteMenu.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.layerDeleteMenu.Name = "layerDeleteMenu";
            this.layerDeleteMenu.Size = new System.Drawing.Size(127, 22);
            this.layerDeleteMenu.Text = "Delete";
            this.layerDeleteMenu.Click += new System.EventHandler(this.layerDeleteMenu_Click);
            // 
            // layerPropertiesMenu
            // 
            this.layerPropertiesMenu.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.layerPropertiesMenu.Name = "layerPropertiesMenu";
            this.layerPropertiesMenu.Size = new System.Drawing.Size(127, 22);
            this.layerPropertiesMenu.Text = "Properties";
            this.layerPropertiesMenu.Click += new System.EventHandler(this.cmsMenuProperties_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.BackColor = System.Drawing.SystemColors.ControlDarkDark;
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
            this.panWorkRegion.Location = new System.Drawing.Point(29, 106);
            this.panWorkRegion.Name = "panWorkRegion";
            this.panWorkRegion.Size = new System.Drawing.Size(907, 368);
            this.panWorkRegion.TabIndex = 6;
            this.panWorkRegion.SizeChanged += new System.EventHandler(this.panWorkRegion_SizeChanged);
            this.panWorkRegion.DragDrop += new System.Windows.Forms.DragEventHandler(this.panWorkRegion_DragDrop);
            this.panWorkRegion.DragEnter += new System.Windows.Forms.DragEventHandler(this.panWorkRegion_DragEnter);
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(942, 405);
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
            this.treeView1.Size = new System.Drawing.Size(129, 55);
            this.treeView1.TabIndex = 10;
            // 
            // leftToolStrip
            // 
            this.leftToolStrip.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.leftToolStrip.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.circleToolStripButton,
            this.squareToolStripButton,
            this.diamondToolStripButton,
            this.triangleToolStripButton,
            this.hexagon,
            this.starToolStripButton,
            this.ovalToolStripButton,
            this.rectangleToolStripButton,
            this.toolStripButton1});
            this.leftToolStrip.Location = new System.Drawing.Point(0, 51);
            this.leftToolStrip.Margin = new System.Windows.Forms.Padding(16, 0, 0, 0);
            this.leftToolStrip.Name = "leftToolStrip";
            this.leftToolStrip.Size = new System.Drawing.Size(26, 450);
            this.leftToolStrip.TabIndex = 12;
            this.leftToolStrip.Text = "toolStrip2";
            // 
            // circleToolStripButton
            // 
            this.circleToolStripButton.AutoSize = false;
            this.circleToolStripButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.circleToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.circleToolStripButton.Image = global::VectorImageEdit.Properties.Resources.circle;
            this.circleToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.circleToolStripButton.Name = "circleToolStripButton";
            this.circleToolStripButton.Size = new System.Drawing.Size(25, 25);
            this.circleToolStripButton.Text = "toolStripButton1";
            this.circleToolStripButton.ToolTipText = "Circle";
            this.circleToolStripButton.Click += new System.EventHandler(this.btnCircle_Click);
            // 
            // squareToolStripButton
            // 
            this.squareToolStripButton.AutoSize = false;
            this.squareToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.squareToolStripButton.Image = global::VectorImageEdit.Properties.Resources.square;
            this.squareToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.squareToolStripButton.Name = "squareToolStripButton";
            this.squareToolStripButton.Size = new System.Drawing.Size(25, 25);
            this.squareToolStripButton.Text = "toolStripButton2";
            this.squareToolStripButton.ToolTipText = "Square";
            this.squareToolStripButton.Click += new System.EventHandler(this.btnSquare_Click);
            // 
            // diamondToolStripButton
            // 
            this.diamondToolStripButton.AutoSize = false;
            this.diamondToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.diamondToolStripButton.Image = global::VectorImageEdit.Properties.Resources.diamond;
            this.diamondToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.diamondToolStripButton.Name = "diamondToolStripButton";
            this.diamondToolStripButton.Size = new System.Drawing.Size(25, 25);
            this.diamondToolStripButton.Text = "toolStripButton3";
            this.diamondToolStripButton.ToolTipText = "Diamond";
            this.diamondToolStripButton.Click += new System.EventHandler(this.btnDiamond_Click);
            // 
            // triangleToolStripButton
            // 
            this.triangleToolStripButton.AutoSize = false;
            this.triangleToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.triangleToolStripButton.Image = global::VectorImageEdit.Properties.Resources.triangle;
            this.triangleToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.triangleToolStripButton.Name = "triangleToolStripButton";
            this.triangleToolStripButton.Size = new System.Drawing.Size(25, 25);
            this.triangleToolStripButton.Text = "toolStripButton4";
            this.triangleToolStripButton.ToolTipText = "Triangle";
            this.triangleToolStripButton.Click += new System.EventHandler(this.btnTriangle_Click);
            // 
            // hexagon
            // 
            this.hexagon.AutoSize = false;
            this.hexagon.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.hexagon.Image = global::VectorImageEdit.Properties.Resources.hexagon;
            this.hexagon.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.hexagon.Name = "hexagon";
            this.hexagon.Size = new System.Drawing.Size(25, 25);
            this.hexagon.Text = "toolStripButton5";
            this.hexagon.ToolTipText = "Hexagon";
            this.hexagon.Click += new System.EventHandler(this.btnHexagon_Click);
            // 
            // starToolStripButton
            // 
            this.starToolStripButton.AutoSize = false;
            this.starToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.starToolStripButton.Image = global::VectorImageEdit.Properties.Resources.star;
            this.starToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.starToolStripButton.Name = "starToolStripButton";
            this.starToolStripButton.Size = new System.Drawing.Size(25, 25);
            this.starToolStripButton.Text = "toolStripButton6";
            this.starToolStripButton.ToolTipText = "Star";
            this.starToolStripButton.Click += new System.EventHandler(this.btnStar_Click);
            // 
            // ovalToolStripButton
            // 
            this.ovalToolStripButton.AutoSize = false;
            this.ovalToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ovalToolStripButton.Image = global::VectorImageEdit.Properties.Resources.oval;
            this.ovalToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ovalToolStripButton.Name = "ovalToolStripButton";
            this.ovalToolStripButton.Size = new System.Drawing.Size(25, 25);
            this.ovalToolStripButton.Text = "toolStripButton7";
            this.ovalToolStripButton.ToolTipText = "Oval";
            this.ovalToolStripButton.Click += new System.EventHandler(this.btnOval_Click);
            // 
            // rectangleToolStripButton
            // 
            this.rectangleToolStripButton.AutoSize = false;
            this.rectangleToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.rectangleToolStripButton.Image = global::VectorImageEdit.Properties.Resources.rectangle;
            this.rectangleToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.rectangleToolStripButton.Name = "rectangleToolStripButton";
            this.rectangleToolStripButton.Size = new System.Drawing.Size(25, 25);
            this.rectangleToolStripButton.Text = "toolStripButton8";
            this.rectangleToolStripButton.ToolTipText = "Rectangle";
            this.rectangleToolStripButton.Click += new System.EventHandler(this.btnRectangle_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.AutoSize = false;
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::VectorImageEdit.Properties.Resources.line;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(25, 25);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.ToolTipText = "Line";
            this.toolStripButton1.Click += new System.EventHandler(this.btnLine_Click);
            // 
            // topToolStrip
            // 
            this.topToolStrip.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.topToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.edgecolorToolStripButton,
            this.toolStripSeparator3,
            this.toolStripButton2,
            this.toolStripButton3,
            this.toolStripButton4,
            this.toolStripButton5,
            this.toolStripButton6,
            this.toolStripButton7,
            this.toolStripButton8,
            this.toolStripButton9,
            this.toolStripLabel3,
            this.fillcolortoolStripButton,
            this.toolStripSeparator1,
            this.toolStripButton10,
            this.toolStripButton11,
            this.toolStripButton12,
            this.toolStripButton13,
            this.toolStripButton14,
            this.toolStripButton17,
            this.toolStripButton15,
            this.toolStripButton16});
            this.topToolStrip.Location = new System.Drawing.Point(26, 51);
            this.topToolStrip.Name = "topToolStrip";
            this.topToolStrip.Size = new System.Drawing.Size(1066, 25);
            this.topToolStrip.TabIndex = 11;
            this.topToolStrip.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.BackColor = System.Drawing.Color.PaleTurquoise;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(42, 22);
            this.toolStripLabel1.Text = "Color1";
            // 
            // edgecolorToolStripButton
            // 
            this.edgecolorToolStripButton.AutoSize = false;
            this.edgecolorToolStripButton.AutoToolTip = false;
            this.edgecolorToolStripButton.BackColor = System.Drawing.Color.OrangeRed;
            this.edgecolorToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.edgecolorToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.edgecolorToolStripButton.Name = "edgecolorToolStripButton";
            this.edgecolorToolStripButton.Size = new System.Drawing.Size(20, 20);
            this.edgecolorToolStripButton.Text = "toolStripButton2";
            this.edgecolorToolStripButton.Click += new System.EventHandler(this.edgecolorToolStripButton_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.AutoSize = false;
            this.toolStripButton2.AutoToolTip = false;
            this.toolStripButton2.BackColor = System.Drawing.Color.Silver;
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(20, 20);
            this.toolStripButton2.Text = "toolStripButton2";
            this.toolStripButton2.Click += new System.EventHandler(this.color1Pick_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.AutoSize = false;
            this.toolStripButton3.AutoToolTip = false;
            this.toolStripButton3.BackColor = System.Drawing.Color.Red;
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(20, 20);
            this.toolStripButton3.Text = "toolStripButton3";
            this.toolStripButton3.Click += new System.EventHandler(this.color1Pick_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.AutoSize = false;
            this.toolStripButton4.AutoToolTip = false;
            this.toolStripButton4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(20, 20);
            this.toolStripButton4.Text = "toolStripButton4";
            this.toolStripButton4.Click += new System.EventHandler(this.color1Pick_Click);
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.AutoSize = false;
            this.toolStripButton5.AutoToolTip = false;
            this.toolStripButton5.BackColor = System.Drawing.Color.Yellow;
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(20, 20);
            this.toolStripButton5.Text = "toolStripButton5";
            this.toolStripButton5.Click += new System.EventHandler(this.color1Pick_Click);
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.AutoSize = false;
            this.toolStripButton6.AutoToolTip = false;
            this.toolStripButton6.BackColor = System.Drawing.Color.Lime;
            this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new System.Drawing.Size(20, 20);
            this.toolStripButton6.Text = "toolStripButton6";
            this.toolStripButton6.Click += new System.EventHandler(this.color1Pick_Click);
            // 
            // toolStripButton7
            // 
            this.toolStripButton7.AutoSize = false;
            this.toolStripButton7.AutoToolTip = false;
            this.toolStripButton7.BackColor = System.Drawing.Color.Cyan;
            this.toolStripButton7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton7.Name = "toolStripButton7";
            this.toolStripButton7.Size = new System.Drawing.Size(20, 20);
            this.toolStripButton7.Text = "toolStripButton7";
            this.toolStripButton7.Click += new System.EventHandler(this.color1Pick_Click);
            // 
            // toolStripButton8
            // 
            this.toolStripButton8.AutoSize = false;
            this.toolStripButton8.AutoToolTip = false;
            this.toolStripButton8.BackColor = System.Drawing.Color.Blue;
            this.toolStripButton8.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton8.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton8.Name = "toolStripButton8";
            this.toolStripButton8.Size = new System.Drawing.Size(20, 20);
            this.toolStripButton8.Text = "toolStripButton8";
            this.toolStripButton8.Click += new System.EventHandler(this.color1Pick_Click);
            // 
            // toolStripButton9
            // 
            this.toolStripButton9.AutoSize = false;
            this.toolStripButton9.AutoToolTip = false;
            this.toolStripButton9.BackColor = System.Drawing.Color.Magenta;
            this.toolStripButton9.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton9.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton9.Name = "toolStripButton9";
            this.toolStripButton9.Size = new System.Drawing.Size(20, 20);
            this.toolStripButton9.Text = "toolStripButton9";
            this.toolStripButton9.Click += new System.EventHandler(this.color1Pick_Click);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(42, 23);
            this.toolStripLabel3.Text = "Color2";
            // 
            // fillcolortoolStripButton
            // 
            this.fillcolortoolStripButton.AutoSize = false;
            this.fillcolortoolStripButton.AutoToolTip = false;
            this.fillcolortoolStripButton.BackColor = System.Drawing.Color.LightBlue;
            this.fillcolortoolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.fillcolortoolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.fillcolortoolStripButton.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.fillcolortoolStripButton.Name = "fillcolortoolStripButton";
            this.fillcolortoolStripButton.Size = new System.Drawing.Size(20, 20);
            this.fillcolortoolStripButton.Text = "toolStripButton1";
            this.fillcolortoolStripButton.Click += new System.EventHandler(this.fillcolorToolStripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 2);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripButton10
            // 
            this.toolStripButton10.AutoSize = false;
            this.toolStripButton10.AutoToolTip = false;
            this.toolStripButton10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.toolStripButton10.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton10.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton10.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.toolStripButton10.Name = "toolStripButton10";
            this.toolStripButton10.Size = new System.Drawing.Size(20, 20);
            this.toolStripButton10.Text = "toolStripButton10";
            this.toolStripButton10.Click += new System.EventHandler(this.color2Pick_Click);
            // 
            // toolStripButton11
            // 
            this.toolStripButton11.AutoSize = false;
            this.toolStripButton11.AutoToolTip = false;
            this.toolStripButton11.BackColor = System.Drawing.Color.Maroon;
            this.toolStripButton11.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton11.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton11.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.toolStripButton11.Name = "toolStripButton11";
            this.toolStripButton11.Size = new System.Drawing.Size(20, 20);
            this.toolStripButton11.Text = "toolStripButton11";
            this.toolStripButton11.Click += new System.EventHandler(this.color2Pick_Click);
            // 
            // toolStripButton12
            // 
            this.toolStripButton12.AutoSize = false;
            this.toolStripButton12.AutoToolTip = false;
            this.toolStripButton12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.toolStripButton12.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton12.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton12.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.toolStripButton12.Name = "toolStripButton12";
            this.toolStripButton12.Size = new System.Drawing.Size(20, 20);
            this.toolStripButton12.Text = "toolStripButton12";
            this.toolStripButton12.Click += new System.EventHandler(this.color2Pick_Click);
            // 
            // toolStripButton13
            // 
            this.toolStripButton13.AutoSize = false;
            this.toolStripButton13.AutoToolTip = false;
            this.toolStripButton13.BackColor = System.Drawing.Color.Olive;
            this.toolStripButton13.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton13.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton13.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.toolStripButton13.Name = "toolStripButton13";
            this.toolStripButton13.Size = new System.Drawing.Size(20, 20);
            this.toolStripButton13.Text = "toolStripButton13";
            this.toolStripButton13.Click += new System.EventHandler(this.color2Pick_Click);
            // 
            // toolStripButton14
            // 
            this.toolStripButton14.AutoSize = false;
            this.toolStripButton14.AutoToolTip = false;
            this.toolStripButton14.BackColor = System.Drawing.Color.Green;
            this.toolStripButton14.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton14.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton14.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.toolStripButton14.Name = "toolStripButton14";
            this.toolStripButton14.Size = new System.Drawing.Size(20, 20);
            this.toolStripButton14.Text = "toolStripButton14";
            this.toolStripButton14.Click += new System.EventHandler(this.color2Pick_Click);
            // 
            // toolStripButton17
            // 
            this.toolStripButton17.AutoSize = false;
            this.toolStripButton17.AutoToolTip = false;
            this.toolStripButton17.BackColor = System.Drawing.Color.Teal;
            this.toolStripButton17.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton17.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton17.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.toolStripButton17.Name = "toolStripButton17";
            this.toolStripButton17.Size = new System.Drawing.Size(20, 20);
            this.toolStripButton17.Text = "toolStripButton17";
            this.toolStripButton17.Click += new System.EventHandler(this.color2Pick_Click);
            // 
            // toolStripButton15
            // 
            this.toolStripButton15.AutoSize = false;
            this.toolStripButton15.AutoToolTip = false;
            this.toolStripButton15.BackColor = System.Drawing.Color.Navy;
            this.toolStripButton15.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton15.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton15.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.toolStripButton15.Name = "toolStripButton15";
            this.toolStripButton15.Size = new System.Drawing.Size(20, 20);
            this.toolStripButton15.Text = "toolStripButton15";
            this.toolStripButton15.Click += new System.EventHandler(this.color2Pick_Click);
            // 
            // toolStripButton16
            // 
            this.toolStripButton16.AutoSize = false;
            this.toolStripButton16.AutoToolTip = false;
            this.toolStripButton16.BackColor = System.Drawing.Color.Purple;
            this.toolStripButton16.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton16.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton16.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.toolStripButton16.Name = "toolStripButton16";
            this.toolStripButton16.Size = new System.Drawing.Size(20, 20);
            this.toolStripButton16.Text = "toolStripButton16";
            this.toolStripButton16.Click += new System.EventHandler(this.color2Pick_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusActionLabel,
            this.statusProgressBar,
            this.memoryUsedLabel,
            this.memoryProgressBar});
            this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.statusStrip1.Location = new System.Drawing.Point(26, 479);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1066, 22);
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusActionLabel
            // 
            this.statusActionLabel.Name = "statusActionLabel";
            this.statusActionLabel.Size = new System.Drawing.Size(59, 15);
            this.statusActionLabel.Text = "No action";
            // 
            // statusProgressBar
            // 
            this.statusProgressBar.MarqueeAnimationSpeed = 50;
            this.statusProgressBar.Name = "statusProgressBar";
            this.statusProgressBar.Size = new System.Drawing.Size(100, 16);
            this.statusProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            // 
            // memoryUsedLabel
            // 
            this.memoryUsedLabel.Name = "memoryUsedLabel";
            this.memoryUsedLabel.Size = new System.Drawing.Size(78, 15);
            this.memoryUsedLabel.Text = "MemoryUsed";
            // 
            // memoryProgressBar
            // 
            this.memoryProgressBar.Name = "memoryProgressBar";
            this.memoryProgressBar.Size = new System.Drawing.Size(116, 16);
            // 
            // appBarmenuStrip
            // 
            this.appBarmenuStrip.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.appBarmenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeToolStripMenuItem,
            this.maximizeToolStripMenuItem,
            this.minimizetoolStripMenuItem,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.loadingCircleToolStripMenuItem1});
            this.appBarmenuStrip.Location = new System.Drawing.Point(0, 0);
            this.appBarmenuStrip.Name = "appBarmenuStrip";
            this.appBarmenuStrip.Size = new System.Drawing.Size(1092, 27);
            this.appBarmenuStrip.TabIndex = 14;
            this.appBarmenuStrip.Text = "menuStrip1";
            this.appBarmenuStrip.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.appBarmenuStrip_MouseDoubleClick);
            this.appBarmenuStrip.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Borderless_MouseDown);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.closeToolStripMenuItem.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.closeToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closeToolStripMenuItem.ForeColor = System.Drawing.SystemColors.Control;
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(29, 23);
            this.closeToolStripMenuItem.Text = "X";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // maximizeToolStripMenuItem
            // 
            this.maximizeToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.maximizeToolStripMenuItem.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.maximizeToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maximizeToolStripMenuItem.ForeColor = System.Drawing.SystemColors.Control;
            this.maximizeToolStripMenuItem.Name = "maximizeToolStripMenuItem";
            this.maximizeToolStripMenuItem.Size = new System.Drawing.Size(28, 23);
            this.maximizeToolStripMenuItem.Text = "□";
            this.maximizeToolStripMenuItem.Click += new System.EventHandler(this.maximizeToolStripMenuItem_Click);
            // 
            // minimizetoolStripMenuItem
            // 
            this.minimizetoolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.minimizetoolStripMenuItem.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.minimizetoolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.minimizetoolStripMenuItem.ForeColor = System.Drawing.SystemColors.Control;
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
            this.toolStripMenuItem4.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(106, 23);
            this.toolStripMenuItem4.Text = "VectorImageEdit";
            // 
            // loadingCircleToolStripMenuItem1
            // 
            this.loadingCircleToolStripMenuItem1.AutoSize = false;
            // 
            // loadingCircleToolStripMenuItem1
            // 
            this.loadingCircleToolStripMenuItem1.LoadingCircleControl.AccessibleName = "loadingCircleToolStripMenuItem1";
            this.loadingCircleToolStripMenuItem1.LoadingCircleControl.Active = true;
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
            this.panel1.BackColor = System.Drawing.Color.Silver;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.treeView1);
            this.panel1.Controls.Add(this.panWorkRegion);
            this.panel1.Controls.Add(this.topToolStrip);
            this.panel1.Controls.Add(this.statusStrip1);
            this.panel1.Controls.Add(this.leftToolStrip);
            this.panel1.Controls.Add(this.menuBarTop);
            this.panel1.Controls.Add(this.lBoxActiveLayers);
            this.panel1.Controls.Add(this.appBarmenuStrip);
            this.panel1.Location = new System.Drawing.Point(5, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1094, 503);
            this.panel1.TabIndex = 15;
            // 
            // AppWindow
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1105, 513);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MainMenuStrip = this.menuBarTop;
            this.Name = "AppWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Vector Image Editor";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Borderless_MouseDown);
            this.Move += new System.EventHandler(this.AppWindow_Move);
            this.cmsRightClickMenu.ResumeLayout(false);
            this.menuBarTop.ResumeLayout(false);
            this.menuBarTop.PerformLayout();
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
        private ToolStripLabel toolStripLabel1;
        private ToolStripButton fillcolortoolStripButton;
        private ToolStripButton edgecolorToolStripButton;
        private ToolStrip leftToolStrip;
        private ToolStripButton circleToolStripButton;
        private MenuStrip appBarmenuStrip;
        private ToolStripMenuItem closeToolStripMenuItem;
        private ToolStripMenuItem maximizeToolStripMenuItem;
        private ToolStripMenuItem minimizetoolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem toolStripMenuItem4;
        private MRG.Controls.UI.LoadingCircleToolStripMenuItem loadingCircleToolStripMenuItem1;
        private ToolStripButton squareToolStripButton;
        private ToolStripButton diamondToolStripButton;
        private ToolStripButton triangleToolStripButton;
        private ToolStripButton hexagon;
        private ToolStripButton starToolStripButton;
        private ToolStripButton ovalToolStripButton;
        private ToolStripButton rectangleToolStripButton;
        private ToolStripButton toolStripButton1;
        private ToolStripButton toolStripButton2;
        private ToolStripButton toolStripButton3;
        private ToolStripButton toolStripButton4;
        private ToolStripButton toolStripButton5;
        private ToolStripButton toolStripButton6;
        private ToolStripButton toolStripButton7;
        private ToolStripButton toolStripButton8;
        private ToolStripButton toolStripButton9;
        private ToolStripLabel toolStripLabel3;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton toolStripButton10;
        private ToolStripButton toolStripButton11;
        private ToolStripButton toolStripButton12;
        private ToolStripButton toolStripButton13;
        private ToolStripButton toolStripButton14;
        private ToolStripButton toolStripButton17;
        private ToolStripButton toolStripButton15;
        private ToolStripButton toolStripButton16;
        private ToolStripSeparator toolStripSeparator1;
        private Panel panel1;
    }
}

