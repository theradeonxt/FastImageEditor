using ImageInterpolation.ModuleImageBlending;

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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.moduleBlendingUi1 = new ModuleBlendingUi();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.moduleFilterUi1 = new ImageInterpolation.ModuleFilterUi();
            this.labelProcessingTime = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
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
            this.tabPage1.Controls.Add(this.moduleBlendingUi1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1128, 376);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "ModuleBlend";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // moduleBlendingUi1
            // 
            this.moduleBlendingUi1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.moduleBlendingUi1.Location = new System.Drawing.Point(3, 3);
            this.moduleBlendingUi1.Name = "moduleBlendingUi1";
            this.moduleBlendingUi1.Size = new System.Drawing.Size(1122, 370);
            this.moduleBlendingUi1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.moduleFilterUi1);
            this.tabPage2.Controls.Add(this.labelProcessingTime);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1128, 376);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "ModuleFilter";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // moduleFilterUi1
            // 
            this.moduleFilterUi1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.moduleFilterUi1.FilterInputImage = null;
            this.moduleFilterUi1.FilterOutputImage = null;
            this.moduleFilterUi1.KernelText = "";
            this.moduleFilterUi1.Location = new System.Drawing.Point(3, 3);
            this.moduleFilterUi1.Name = "moduleFilterUi1";
            this.moduleFilterUi1.Size = new System.Drawing.Size(1122, 370);
            this.moduleFilterUi1.TabIndex = 3;
            // 
            // labelProcessingTime
            // 
            this.labelProcessingTime.AutoSize = true;
            this.labelProcessingTime.Location = new System.Drawing.Point(912, 326);
            this.labelProcessingTime.Name = "labelProcessingTime";
            this.labelProcessingTime.Size = new System.Drawing.Size(0, 13);
            this.labelProcessingTime.TabIndex = 2;
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
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label labelProcessingTime;
        public ModuleFilterUi moduleFilterUi1;
        public ModuleBlendingUi moduleBlendingUi1;
    }
}

