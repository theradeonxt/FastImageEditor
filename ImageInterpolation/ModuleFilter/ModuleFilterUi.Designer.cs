namespace ImageInterpolation
{
    partial class ModuleFilterUi
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
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.labelProcessTime = new System.Windows.Forms.Label();
            this.comboBoxNormalization = new System.Windows.Forms.ComboBox();
            this.comboBoxBuiltinFilters = new System.Windows.Forms.ComboBox();
            this.checkBoxNormalize = new System.Windows.Forms.CheckBox();
            this.tboxNormalizeState = new System.Windows.Forms.TextBox();
            this.tboxKernelSize = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.labelFilterTitle = new System.Windows.Forms.Label();
            this.richTextBoxKernel = new System.Windows.Forms.RichTextBox();
            this.pictureBoxFilterInput = new System.Windows.Forms.PictureBox();
            this.pictureBoxFilterOutput = new System.Windows.Forms.PictureBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnLoadSource = new System.Windows.Forms.Button();
            this.btnApplyKernel = new System.Windows.Forms.Button();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFilterInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFilterOutput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.labelProcessTime);
            this.groupBox5.Controls.Add(this.comboBoxNormalization);
            this.groupBox5.Controls.Add(this.comboBoxBuiltinFilters);
            this.groupBox5.Controls.Add(this.checkBoxNormalize);
            this.groupBox5.Controls.Add(this.tboxNormalizeState);
            this.groupBox5.Controls.Add(this.tboxKernelSize);
            this.groupBox5.Controls.Add(this.label11);
            this.groupBox5.Controls.Add(this.labelFilterTitle);
            this.groupBox5.Controls.Add(this.richTextBoxKernel);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox5.Location = new System.Drawing.Point(0, 0);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(316, 299);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            // 
            // labelProcessTime
            // 
            this.labelProcessTime.AutoSize = true;
            this.labelProcessTime.Location = new System.Drawing.Point(210, 173);
            this.labelProcessTime.Name = "labelProcessTime";
            this.labelProcessTime.Size = new System.Drawing.Size(67, 13);
            this.labelProcessTime.TabIndex = 6;
            this.labelProcessTime.Text = "processTime";
            // 
            // comboBoxNormalization
            // 
            this.comboBoxNormalization.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxNormalization.FormattingEnabled = true;
            this.comboBoxNormalization.Location = new System.Drawing.Point(213, 140);
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
            this.comboBoxBuiltinFilters.Location = new System.Drawing.Point(6, 272);
            this.comboBoxBuiltinFilters.Name = "comboBoxBuiltinFilters";
            this.comboBoxBuiltinFilters.Size = new System.Drawing.Size(201, 21);
            this.comboBoxBuiltinFilters.TabIndex = 4;
            this.comboBoxBuiltinFilters.Text = "Builtin Filters";
            // 
            // checkBoxNormalize
            // 
            this.checkBoxNormalize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxNormalize.Location = new System.Drawing.Point(213, 103);
            this.checkBoxNormalize.Name = "checkBoxNormalize";
            this.checkBoxNormalize.Size = new System.Drawing.Size(97, 31);
            this.checkBoxNormalize.TabIndex = 3;
            this.checkBoxNormalize.Text = "Ensure Normalized";
            this.checkBoxNormalize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxNormalize.UseVisualStyleBackColor = true;
            // 
            // tboxNormalizeState
            // 
            this.tboxNormalizeState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tboxNormalizeState.Location = new System.Drawing.Point(213, 77);
            this.tboxNormalizeState.Name = "tboxNormalizeState";
            this.tboxNormalizeState.ReadOnly = true;
            this.tboxNormalizeState.Size = new System.Drawing.Size(97, 20);
            this.tboxNormalizeState.TabIndex = 2;
            this.tboxNormalizeState.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tboxKernelSize
            // 
            this.tboxKernelSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tboxKernelSize.Location = new System.Drawing.Point(213, 51);
            this.tboxKernelSize.Name = "tboxKernelSize";
            this.tboxKernelSize.ReadOnly = true;
            this.tboxKernelSize.Size = new System.Drawing.Size(97, 20);
            this.tboxKernelSize.TabIndex = 2;
            this.tboxKernelSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.Location = new System.Drawing.Point(213, 16);
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
            this.labelFilterTitle.Size = new System.Drawing.Size(201, 16);
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
            this.richTextBoxKernel.Size = new System.Drawing.Size(201, 231);
            this.richTextBoxKernel.TabIndex = 0;
            this.richTextBoxKernel.Text = "";
            this.richTextBoxKernel.WordWrap = false;
            // 
            // pictureBoxFilterInput
            // 
            this.pictureBoxFilterInput.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBoxFilterInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxFilterInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxFilterInput.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxFilterInput.Name = "pictureBoxFilterInput";
            this.pictureBoxFilterInput.Size = new System.Drawing.Size(266, 299);
            this.pictureBoxFilterInput.TabIndex = 3;
            this.pictureBoxFilterInput.TabStop = false;
            // 
            // pictureBoxFilterOutput
            // 
            this.pictureBoxFilterOutput.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBoxFilterOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxFilterOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxFilterOutput.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxFilterOutput.Name = "pictureBoxFilterOutput";
            this.pictureBoxFilterOutput.Size = new System.Drawing.Size(284, 299);
            this.pictureBoxFilterOutput.TabIndex = 4;
            this.pictureBoxFilterOutput.TabStop = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(316, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnLoadSource);
            this.splitContainer1.Panel1.Controls.Add(this.pictureBoxFilterInput);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnApplyKernel);
            this.splitContainer1.Panel2.Controls.Add(this.pictureBoxFilterOutput);
            this.splitContainer1.Size = new System.Drawing.Size(554, 299);
            this.splitContainer1.SplitterDistance = 266;
            this.splitContainer1.TabIndex = 5;
            // 
            // btnLoadSource
            // 
            this.btnLoadSource.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnLoadSource.Location = new System.Drawing.Point(0, 276);
            this.btnLoadSource.Name = "btnLoadSource";
            this.btnLoadSource.Size = new System.Drawing.Size(266, 23);
            this.btnLoadSource.TabIndex = 4;
            this.btnLoadSource.Text = "Load Image...";
            this.btnLoadSource.UseVisualStyleBackColor = true;
            // 
            // btnApplyKernel
            // 
            this.btnApplyKernel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnApplyKernel.Location = new System.Drawing.Point(0, 276);
            this.btnApplyKernel.Name = "btnApplyKernel";
            this.btnApplyKernel.Size = new System.Drawing.Size(284, 23);
            this.btnApplyKernel.TabIndex = 5;
            this.btnApplyKernel.Text = "Apply Kernel";
            this.btnApplyKernel.UseVisualStyleBackColor = true;
            // 
            // ModuleFilterUi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.groupBox5);
            this.Name = "ModuleFilterUi";
            this.Size = new System.Drawing.Size(870, 299);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFilterInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFilterOutput)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ComboBox comboBoxNormalization;
        private System.Windows.Forms.ComboBox comboBoxBuiltinFilters;
        private System.Windows.Forms.CheckBox checkBoxNormalize;
        private System.Windows.Forms.TextBox tboxNormalizeState;
        private System.Windows.Forms.TextBox tboxKernelSize;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label labelFilterTitle;
        private System.Windows.Forms.RichTextBox richTextBoxKernel;
        private System.Windows.Forms.PictureBox pictureBoxFilterInput;
        private System.Windows.Forms.PictureBox pictureBoxFilterOutput;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnLoadSource;
        private System.Windows.Forms.Button btnApplyKernel;
        private System.Windows.Forms.Label labelProcessTime;

    }
}
