namespace ImageInterpolation
{
    partial class GuiView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.btnLoadSource = new System.Windows.Forms.Button();
            this.btnLoadTarget = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pictureBoxSource = new System.Windows.Forms.PictureBox();
            this.pictureBoxTarget = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelBlendingPercentage = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.labelPostTime = new System.Windows.Forms.Label();
            this.labelProcessTime = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.pictureBoxIntermediate = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelSrcSize = new System.Windows.Forms.Label();
            this.labelSrcFmt = new System.Windows.Forms.Label();
            this.labelSrcRes = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labelTarSize = new System.Windows.Forms.Label();
            this.labelTarFmt = new System.Windows.Forms.Label();
            this.labelTarRes = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.labelOutSize = new System.Windows.Forms.Label();
            this.labelOutFmt = new System.Windows.Forms.Label();
            this.labelOutRes = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.comboBoxNormalization = new System.Windows.Forms.ComboBox();
            this.comboBoxBuiltinFilters = new System.Windows.Forms.ComboBox();
            this.checkBoxNormalize = new System.Windows.Forms.CheckBox();
            this.tboxNormalizeState = new System.Windows.Forms.TextBox();
            this.tboxKernelSize = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.labelFilterTitle = new System.Windows.Forms.Label();
            this.richTextBoxKernel = new System.Windows.Forms.RichTextBox();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.pictureBoxFilterInput = new System.Windows.Forms.PictureBox();
            this.pictureBoxFilterOutput = new System.Windows.Forms.PictureBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTarget)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIntermediate)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFilterInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFilterOutput)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLoadSource
            // 
            this.btnLoadSource.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnLoadSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadSource.Location = new System.Drawing.Point(0, 222);
            this.btnLoadSource.Name = "btnLoadSource";
            this.btnLoadSource.Size = new System.Drawing.Size(313, 22);
            this.btnLoadSource.TabIndex = 3;
            this.btnLoadSource.Text = "Source Image...";
            this.btnLoadSource.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnLoadSource.UseVisualStyleBackColor = true;
            // 
            // btnLoadTarget
            // 
            this.btnLoadTarget.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnLoadTarget.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadTarget.Location = new System.Drawing.Point(0, 222);
            this.btnLoadTarget.Name = "btnLoadTarget";
            this.btnLoadTarget.Size = new System.Drawing.Size(315, 22);
            this.btnLoadTarget.TabIndex = 3;
            this.btnLoadTarget.Text = "Target Image...";
            this.btnLoadTarget.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnLoadTarget.UseVisualStyleBackColor = true;
            // 
            // timer1
            // 
            this.timer1.Interval = 50;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnLoadSource);
            this.splitContainer1.Panel1.Controls.Add(this.pictureBoxSource);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AllowDrop = true;
            this.splitContainer1.Panel2.Controls.Add(this.btnLoadTarget);
            this.splitContainer1.Panel2.Controls.Add(this.pictureBoxTarget);
            this.splitContainer1.Size = new System.Drawing.Size(632, 244);
            this.splitContainer1.SplitterDistance = 313;
            this.splitContainer1.TabIndex = 4;
            // 
            // pictureBoxSource
            // 
            this.pictureBoxSource.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBoxSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxSource.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxSource.Name = "pictureBoxSource";
            this.pictureBoxSource.Size = new System.Drawing.Size(313, 244);
            this.pictureBoxSource.TabIndex = 4;
            this.pictureBoxSource.TabStop = false;
            // 
            // pictureBoxTarget
            // 
            this.pictureBoxTarget.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBoxTarget.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxTarget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxTarget.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxTarget.Name = "pictureBoxTarget";
            this.pictureBoxTarget.Size = new System.Drawing.Size(315, 244);
            this.pictureBoxTarget.TabIndex = 4;
            this.pictureBoxTarget.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelBlendingPercentage);
            this.groupBox1.Controls.Add(this.trackBar1);
            this.groupBox1.Controls.Add(this.labelPostTime);
            this.groupBox1.Controls.Add(this.labelProcessTime);
            this.groupBox1.Location = new System.Drawing.Point(3, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(474, 99);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // labelBlendingPercentage
            // 
            this.labelBlendingPercentage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelBlendingPercentage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelBlendingPercentage.Location = new System.Drawing.Point(350, 55);
            this.labelBlendingPercentage.Name = "labelBlendingPercentage";
            this.labelBlendingPercentage.Size = new System.Drawing.Size(50, 15);
            this.labelBlendingPercentage.TabIndex = 4;
            this.labelBlendingPercentage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackBar1
            // 
            this.trackBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBar1.AutoSize = false;
            this.trackBar1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.trackBar1.Location = new System.Drawing.Point(319, 40);
            this.trackBar1.Maximum = 100;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(149, 30);
            this.trackBar1.TabIndex = 0;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // labelPostTime
            // 
            this.labelPostTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPostTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelPostTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPostTime.ForeColor = System.Drawing.Color.Brown;
            this.labelPostTime.Location = new System.Drawing.Point(319, 73);
            this.labelPostTime.Name = "labelPostTime";
            this.labelPostTime.Size = new System.Drawing.Size(149, 21);
            this.labelPostTime.TabIndex = 3;
            this.labelPostTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelProcessTime
            // 
            this.labelProcessTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelProcessTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelProcessTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProcessTime.ForeColor = System.Drawing.Color.Brown;
            this.labelProcessTime.Location = new System.Drawing.Point(319, 11);
            this.labelProcessTime.Name = "labelProcessTime";
            this.labelProcessTime.Size = new System.Drawing.Size(149, 21);
            this.labelProcessTime.TabIndex = 3;
            this.labelProcessTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.Location = new System.Drawing.Point(6, 6);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.pictureBoxIntermediate);
            this.splitContainer2.Size = new System.Drawing.Size(1114, 244);
            this.splitContainer2.SplitterDistance = 632;
            this.splitContainer2.TabIndex = 6;
            // 
            // pictureBoxIntermediate
            // 
            this.pictureBoxIntermediate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBoxIntermediate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxIntermediate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxIntermediate.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxIntermediate.Name = "pictureBoxIntermediate";
            this.pictureBoxIntermediate.Size = new System.Drawing.Size(478, 244);
            this.pictureBoxIntermediate.TabIndex = 4;
            this.pictureBoxIntermediate.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.labelSrcSize);
            this.groupBox2.Controls.Add(this.labelSrcFmt);
            this.groupBox2.Controls.Add(this.labelSrcRes);
            this.groupBox2.Location = new System.Drawing.Point(9, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(207, 97);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Source Parameters";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Resolution:";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 21);
            this.label3.TabIndex = 0;
            this.label3.Text = "Size[MB]:";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 21);
            this.label2.TabIndex = 0;
            this.label2.Text = "PixelFormat:";
            // 
            // labelSrcSize
            // 
            this.labelSrcSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSrcSize.Location = new System.Drawing.Point(89, 55);
            this.labelSrcSize.Name = "labelSrcSize";
            this.labelSrcSize.Size = new System.Drawing.Size(112, 18);
            this.labelSrcSize.TabIndex = 0;
            this.labelSrcSize.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelSrcFmt
            // 
            this.labelSrcFmt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSrcFmt.Location = new System.Drawing.Point(89, 34);
            this.labelSrcFmt.Name = "labelSrcFmt";
            this.labelSrcFmt.Size = new System.Drawing.Size(112, 18);
            this.labelSrcFmt.TabIndex = 0;
            this.labelSrcFmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelSrcRes
            // 
            this.labelSrcRes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSrcRes.Location = new System.Drawing.Point(89, 16);
            this.labelSrcRes.Name = "labelSrcRes";
            this.labelSrcRes.Size = new System.Drawing.Size(112, 18);
            this.labelSrcRes.TabIndex = 0;
            this.labelSrcRes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.labelTarSize);
            this.groupBox3.Controls.Add(this.labelTarFmt);
            this.groupBox3.Controls.Add(this.labelTarRes);
            this.groupBox3.Location = new System.Drawing.Point(216, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(207, 97);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Target Parameters";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 18);
            this.label4.TabIndex = 0;
            this.label4.Text = "Resolution:";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(6, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 21);
            this.label5.TabIndex = 0;
            this.label5.Text = "Size[MB]:";
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 34);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 21);
            this.label6.TabIndex = 0;
            this.label6.Text = "PixelFormat:";
            // 
            // labelTarSize
            // 
            this.labelTarSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTarSize.Location = new System.Drawing.Point(90, 55);
            this.labelTarSize.Name = "labelTarSize";
            this.labelTarSize.Size = new System.Drawing.Size(111, 18);
            this.labelTarSize.TabIndex = 0;
            this.labelTarSize.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelTarFmt
            // 
            this.labelTarFmt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTarFmt.Location = new System.Drawing.Point(90, 34);
            this.labelTarFmt.Name = "labelTarFmt";
            this.labelTarFmt.Size = new System.Drawing.Size(111, 18);
            this.labelTarFmt.TabIndex = 0;
            this.labelTarFmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelTarRes
            // 
            this.labelTarRes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTarRes.Location = new System.Drawing.Point(90, 16);
            this.labelTarRes.Name = "labelTarRes";
            this.labelTarRes.Size = new System.Drawing.Size(111, 18);
            this.labelTarRes.TabIndex = 0;
            this.labelTarRes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.labelOutSize);
            this.groupBox4.Controls.Add(this.labelOutFmt);
            this.groupBox4.Controls.Add(this.labelOutRes);
            this.groupBox4.Location = new System.Drawing.Point(423, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(207, 97);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Output Parameters";
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(6, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(78, 18);
            this.label7.TabIndex = 0;
            this.label7.Text = "Resolution:";
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(6, 55);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(78, 21);
            this.label8.TabIndex = 0;
            this.label8.Text = "Size[MB]:";
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(6, 34);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(78, 21);
            this.label9.TabIndex = 0;
            this.label9.Text = "PixelFormat:";
            // 
            // labelOutSize
            // 
            this.labelOutSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOutSize.Location = new System.Drawing.Point(90, 55);
            this.labelOutSize.Name = "labelOutSize";
            this.labelOutSize.Size = new System.Drawing.Size(111, 18);
            this.labelOutSize.TabIndex = 0;
            this.labelOutSize.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelOutFmt
            // 
            this.labelOutFmt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOutFmt.Location = new System.Drawing.Point(90, 34);
            this.labelOutFmt.Name = "labelOutFmt";
            this.labelOutFmt.Size = new System.Drawing.Size(111, 18);
            this.labelOutFmt.TabIndex = 0;
            this.labelOutFmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelOutRes
            // 
            this.labelOutRes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOutRes.Location = new System.Drawing.Point(90, 16);
            this.labelOutRes.Name = "labelOutRes";
            this.labelOutRes.Size = new System.Drawing.Size(111, 18);
            this.labelOutRes.TabIndex = 0;
            this.labelOutRes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1136, 402);
            this.tabControl1.TabIndex = 8;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer3);
            this.tabPage1.Controls.Add(this.splitContainer2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1128, 376);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "ModuleBlend";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer3.Location = new System.Drawing.Point(6, 256);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.groupBox2);
            this.splitContainer3.Panel1.Controls.Add(this.groupBox4);
            this.splitContainer3.Panel1.Controls.Add(this.groupBox3);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer3.Size = new System.Drawing.Size(1114, 99);
            this.splitContainer3.SplitterDistance = 630;
            this.splitContainer3.TabIndex = 9;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox5);
            this.tabPage2.Controls.Add(this.splitContainer4);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1128, 376);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "ModuleFilter";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox5.Controls.Add(this.comboBoxNormalization);
            this.groupBox5.Controls.Add(this.comboBoxBuiltinFilters);
            this.groupBox5.Controls.Add(this.checkBoxNormalize);
            this.groupBox5.Controls.Add(this.tboxNormalizeState);
            this.groupBox5.Controls.Add(this.tboxKernelSize);
            this.groupBox5.Controls.Add(this.label11);
            this.groupBox5.Controls.Add(this.labelFilterTitle);
            this.groupBox5.Controls.Add(this.richTextBoxKernel);
            this.groupBox5.Location = new System.Drawing.Point(6, 6);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(303, 364);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            // 
            // comboBoxNormalization
            // 
            this.comboBoxNormalization.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxNormalization.FormattingEnabled = true;
            this.comboBoxNormalization.Location = new System.Drawing.Point(200, 140);
            this.comboBoxNormalization.Name = "comboBoxNormalization";
            this.comboBoxNormalization.Size = new System.Drawing.Size(97, 21);
            this.comboBoxNormalization.TabIndex = 5;
            this.comboBoxNormalization.Text = "Normalization";
            // 
            // comboBoxBuiltinFilters
            // 
            this.comboBoxBuiltinFilters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxBuiltinFilters.FormattingEnabled = true;
            this.comboBoxBuiltinFilters.Location = new System.Drawing.Point(6, 167);
            this.comboBoxBuiltinFilters.Name = "comboBoxBuiltinFilters";
            this.comboBoxBuiltinFilters.Size = new System.Drawing.Size(188, 21);
            this.comboBoxBuiltinFilters.TabIndex = 4;
            this.comboBoxBuiltinFilters.Text = "Builtin Filters";
            // 
            // checkBoxNormalize
            // 
            this.checkBoxNormalize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxNormalize.Location = new System.Drawing.Point(200, 103);
            this.checkBoxNormalize.Name = "checkBoxNormalize";
            this.checkBoxNormalize.Size = new System.Drawing.Size(97, 31);
            this.checkBoxNormalize.TabIndex = 3;
            this.checkBoxNormalize.Text = "Ensure Normalized";
            this.checkBoxNormalize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.checkBoxNormalize, "Check to always apply the following options to postprocess the filter.");
            this.checkBoxNormalize.UseVisualStyleBackColor = true;
            // 
            // tboxNormalizeState
            // 
            this.tboxNormalizeState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tboxNormalizeState.Location = new System.Drawing.Point(200, 77);
            this.tboxNormalizeState.Name = "tboxNormalizeState";
            this.tboxNormalizeState.ReadOnly = true;
            this.tboxNormalizeState.Size = new System.Drawing.Size(97, 20);
            this.tboxNormalizeState.TabIndex = 2;
            this.tboxNormalizeState.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.tboxNormalizeState, "The filter is normalized if the sum of all elements is one(preserves image bright" +
        "ness). \r\nFilters with sum zero are also treated ok.");
            // 
            // tboxKernelSize
            // 
            this.tboxKernelSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tboxKernelSize.Location = new System.Drawing.Point(200, 51);
            this.tboxKernelSize.Name = "tboxKernelSize";
            this.tboxKernelSize.ReadOnly = true;
            this.tboxKernelSize.Size = new System.Drawing.Size(97, 20);
            this.tboxKernelSize.TabIndex = 2;
            this.tboxKernelSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.Location = new System.Drawing.Point(200, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(97, 32);
            this.label11.TabIndex = 1;
            this.label11.Text = "Kernel Params (autodetected)";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelFilterTitle
            // 
            this.labelFilterTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFilterTitle.Location = new System.Drawing.Point(6, 16);
            this.labelFilterTitle.Name = "labelFilterTitle";
            this.labelFilterTitle.Size = new System.Drawing.Size(188, 16);
            this.labelFilterTitle.TabIndex = 1;
            this.labelFilterTitle.Text = "Filter Kernel";
            this.labelFilterTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // richTextBoxKernel
            // 
            this.richTextBoxKernel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxKernel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxKernel.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.richTextBoxKernel.Location = new System.Drawing.Point(6, 35);
            this.richTextBoxKernel.Name = "richTextBoxKernel";
            this.richTextBoxKernel.Size = new System.Drawing.Size(188, 126);
            this.richTextBoxKernel.TabIndex = 0;
            this.richTextBoxKernel.Text = "";
            this.richTextBoxKernel.WordWrap = false;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer4.Location = new System.Drawing.Point(315, 6);
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.pictureBoxFilterInput);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.pictureBoxFilterOutput);
            this.splitContainer4.Size = new System.Drawing.Size(807, 301);
            this.splitContainer4.SplitterDistance = 391;
            this.splitContainer4.TabIndex = 0;
            // 
            // pictureBoxFilterInput
            // 
            this.pictureBoxFilterInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxFilterInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxFilterInput.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxFilterInput.Name = "pictureBoxFilterInput";
            this.pictureBoxFilterInput.Size = new System.Drawing.Size(391, 301);
            this.pictureBoxFilterInput.TabIndex = 0;
            this.pictureBoxFilterInput.TabStop = false;
            // 
            // pictureBoxFilterOutput
            // 
            this.pictureBoxFilterOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxFilterOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxFilterOutput.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxFilterOutput.Name = "pictureBoxFilterOutput";
            this.pictureBoxFilterOutput.Size = new System.Drawing.Size(412, 301);
            this.pictureBoxFilterOutput.TabIndex = 0;
            this.pictureBoxFilterOutput.TabStop = false;
            // 
            // GuiView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1159, 426);
            this.Controls.Add(this.tabControl1);
            this.DoubleBuffered = true;
            this.Name = "GuiView";
            this.Text = "Form1";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTarget)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIntermediate)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFilterInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFilterOutput)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLoadSource;
        private System.Windows.Forms.Button btnLoadTarget;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelProcessTime;
        private System.Windows.Forms.Label labelPostTime;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelSrcSize;
        private System.Windows.Forms.Label labelSrcFmt;
        private System.Windows.Forms.Label labelSrcRes;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelTarSize;
        private System.Windows.Forms.Label labelTarFmt;
        private System.Windows.Forms.Label labelTarRes;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label labelOutSize;
        private System.Windows.Forms.Label labelOutFmt;
        private System.Windows.Forms.Label labelOutRes;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.PictureBox pictureBoxSource;
        private System.Windows.Forms.PictureBox pictureBoxTarget;
        private System.Windows.Forms.PictureBox pictureBoxIntermediate;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RichTextBox richTextBoxKernel;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.PictureBox pictureBoxFilterInput;
        private System.Windows.Forms.PictureBox pictureBoxFilterOutput;
        private System.Windows.Forms.Label labelFilterTitle;
        private System.Windows.Forms.CheckBox checkBoxNormalize;
        private System.Windows.Forms.TextBox tboxNormalizeState;
        private System.Windows.Forms.TextBox tboxKernelSize;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox comboBoxBuiltinFilters;
        private System.Windows.Forms.ComboBox comboBoxNormalization;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label labelBlendingPercentage;
        private System.Windows.Forms.TrackBar trackBar1;
    }
}

