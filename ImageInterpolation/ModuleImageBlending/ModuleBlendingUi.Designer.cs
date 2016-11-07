namespace ImageInterpolation.ModuleImageBlending
{
    partial class ModuleBlendingUi
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnStartProcessing = new System.Windows.Forms.Button();
            this.labelBlendingPercentage = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.labelProcessTime = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnLoadSource = new System.Windows.Forms.Button();
            this.pictureBoxSource = new System.Windows.Forms.PictureBox();
            this.btnLoadTarget = new System.Windows.Forms.Button();
            this.pictureBoxTarget = new System.Windows.Forms.PictureBox();
            this.pictureBoxIntermediate = new System.Windows.Forms.PictureBox();
            this.propertyTarget = new ImageInterpolation.ImagePropertiesUi();
            this.propertyOutput = new ImageInterpolation.ImagePropertiesUi();
            this.propertySource = new ImageInterpolation.ImagePropertiesUi();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTarget)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIntermediate)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer3
            // 
            this.splitContainer3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer3.Location = new System.Drawing.Point(3, 253);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.propertyTarget);
            this.splitContainer3.Panel1.Controls.Add(this.propertyOutput);
            this.splitContainer3.Panel1.Controls.Add(this.propertySource);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer3.Size = new System.Drawing.Size(1114, 90);
            this.splitContainer3.SplitterDistance = 743;
            this.splitContainer3.TabIndex = 10;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnStartProcessing);
            this.groupBox1.Controls.Add(this.labelBlendingPercentage);
            this.groupBox1.Controls.Add(this.trackBar1);
            this.groupBox1.Controls.Add(this.labelProcessTime);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(367, 90);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // btnStartProcessing
            // 
            this.btnStartProcessing.Location = new System.Drawing.Point(6, 13);
            this.btnStartProcessing.Name = "btnStartProcessing";
            this.btnStartProcessing.Size = new System.Drawing.Size(75, 23);
            this.btnStartProcessing.TabIndex = 5;
            this.btnStartProcessing.Text = "Run";
            this.btnStartProcessing.UseVisualStyleBackColor = true;
            // 
            // labelBlendingPercentage
            // 
            this.labelBlendingPercentage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelBlendingPercentage.Location = new System.Drawing.Point(256, 69);
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
            this.trackBar1.Location = new System.Drawing.Point(200, 36);
            this.trackBar1.Maximum = 100;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(149, 30);
            this.trackBar1.TabIndex = 0;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // labelProcessTime
            // 
            this.labelProcessTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelProcessTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProcessTime.ForeColor = System.Drawing.Color.Brown;
            this.labelProcessTime.Location = new System.Drawing.Point(200, 11);
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
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
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
            this.splitContainer2.TabIndex = 11;
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
            // btnLoadSource
            // 
            this.btnLoadSource.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnLoadSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadSource.Location = new System.Drawing.Point(0, 222);
            this.btnLoadSource.Name = "btnLoadSource";
            this.btnLoadSource.Size = new System.Drawing.Size(313, 22);
            this.btnLoadSource.TabIndex = 3;
            this.btnLoadSource.Text = "Source Image...";
            this.btnLoadSource.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnLoadSource.UseVisualStyleBackColor = true;
            // 
            // pictureBoxSource
            // 
            this.pictureBoxSource.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBoxSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxSource.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxSource.Name = "pictureBoxSource";
            this.pictureBoxSource.Size = new System.Drawing.Size(313, 244);
            this.pictureBoxSource.TabIndex = 4;
            this.pictureBoxSource.TabStop = false;
            // 
            // btnLoadTarget
            // 
            this.btnLoadTarget.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnLoadTarget.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadTarget.Location = new System.Drawing.Point(0, 222);
            this.btnLoadTarget.Name = "btnLoadTarget";
            this.btnLoadTarget.Size = new System.Drawing.Size(315, 22);
            this.btnLoadTarget.TabIndex = 3;
            this.btnLoadTarget.Text = "Target Image...";
            this.btnLoadTarget.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnLoadTarget.UseVisualStyleBackColor = true;
            // 
            // pictureBoxTarget
            // 
            this.pictureBoxTarget.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBoxTarget.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxTarget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxTarget.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxTarget.Name = "pictureBoxTarget";
            this.pictureBoxTarget.Size = new System.Drawing.Size(315, 244);
            this.pictureBoxTarget.TabIndex = 4;
            this.pictureBoxTarget.TabStop = false;
            // 
            // pictureBoxIntermediate
            // 
            this.pictureBoxIntermediate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBoxIntermediate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxIntermediate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxIntermediate.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxIntermediate.Name = "pictureBoxIntermediate";
            this.pictureBoxIntermediate.Size = new System.Drawing.Size(478, 244);
            this.pictureBoxIntermediate.TabIndex = 4;
            this.pictureBoxIntermediate.TabStop = false;
            // 
            // propertyTarget
            // 
            this.propertyTarget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyTarget.Location = new System.Drawing.Point(249, 0);
            this.propertyTarget.Name = "propertyTarget";
            this.propertyTarget.Size = new System.Drawing.Size(245, 90);
            this.propertyTarget.TabIndex = 6;
            // 
            // propertyOutput
            // 
            this.propertyOutput.Dock = System.Windows.Forms.DockStyle.Right;
            this.propertyOutput.Location = new System.Drawing.Point(494, 0);
            this.propertyOutput.Name = "propertyOutput";
            this.propertyOutput.Size = new System.Drawing.Size(249, 90);
            this.propertyOutput.TabIndex = 5;
            // 
            // propertySource
            // 
            this.propertySource.Dock = System.Windows.Forms.DockStyle.Left;
            this.propertySource.Location = new System.Drawing.Point(0, 0);
            this.propertySource.Name = "propertySource";
            this.propertySource.Size = new System.Drawing.Size(249, 90);
            this.propertySource.TabIndex = 5;
            // 
            // ModuleBlendingUi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.splitContainer3);
            this.Name = "ModuleBlendingUi";
            this.Size = new System.Drawing.Size(1126, 352);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTarget)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIntermediate)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelBlendingPercentage;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label labelProcessTime;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnLoadSource;
        private System.Windows.Forms.PictureBox pictureBoxSource;
        private System.Windows.Forms.Button btnLoadTarget;
        private System.Windows.Forms.PictureBox pictureBoxTarget;
        private System.Windows.Forms.PictureBox pictureBoxIntermediate;
        private System.Windows.Forms.Button btnStartProcessing;
        public ImagePropertiesUi propertySource;
        public ImagePropertiesUi propertyTarget;
        public ImagePropertiesUi propertyOutput;
    }
}
