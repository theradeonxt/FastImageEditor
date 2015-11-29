﻿using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VectorImageEdit.WindowsFormsBridge;

namespace VectorImageEdit.Views.Main
{
    public partial class AppWindow
    {
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
                foreach (var item in topToolStrip.Items
                    .Cast<object>()
                    .OfType<ToolStripItem>()
                    .Where(stripItem => stripItem.Name.Contains("toolStripColorPreset")))
                {
                    item.Click += listener.ActionPerformed;
                    item.AutoToolTip = true;
                    item.ToolTipText = item.BackColor.ToString();
                }
            }
            catch (ArgumentException) { }
            catch (InvalidCastException) { }
        }
        public void AddShapeItemClickListener(IListener listener)
        {
            try
            {
                foreach (var item in leftToolStrip.Items
                    .Cast<object>()
                    .OfType<ToolStripItem>()
                    .Where(stripItem => stripItem.Name.Contains("Shape")))
                {
                    item.Click += listener.ActionPerformed;
                }
            }
            catch (ArgumentException) { }
            catch (InvalidCastException) { }
        }

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
        public string GetToolstripItemName(object item)
        {
            var toolStripItem = item as ToolStripItem;
            return (toolStripItem != null) ? toolStripItem.Name : "";
        }
        public Color GetToolstripItemBackground(object item)
        {
            var toolStripItem = item as ToolStripItem;
            return (toolStripItem != null) ? toolStripItem.BackColor : Color.Empty;
        }

        private void InitializeTheme()
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

        private class CustomToolStripRenderer : ToolStripProfessionalRenderer
        {
            public CustomToolStripRenderer()
                : base(new CustomLook())
            {
            }
        }
    }
}
