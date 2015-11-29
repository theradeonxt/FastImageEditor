using System.Collections;
using System.Windows.Forms;
using VectorImageEdit.WindowsFormsBridge;

namespace VectorImageEdit.Views.Main
{
    public partial class AppWindow
    {
        public void AddTreeSelectionChangedListener(IListener listener)
        {
            lBoxActiveLayers.SelectedIndexChanged += listener.ActionPerformed;
        }

         /// <summary>
         /// Gets or sets the currently selected item in the scene tree.
         /// For multiple items selected it will return only one of them.
         /// </summary>
        public string SelectedItem
        {
            get { return (lBoxActiveLayers.SelectedItem ?? "").ToString(); }
            set
            {
                if (string.IsNullOrEmpty(value) ||
                    lBoxActiveLayers.FindString(value) == ListBox.NoMatches)
                {
                    // Logger.Warn("SelectedItem tried to be set with invalid parameter.{0}", Environment.StackTrace);
                    return;
                }
                lBoxActiveLayers.SelectedItem = value;
            }
        }

         /// <summary>
         /// Gets or sets the currently selected items in the scene tree.
         /// </summary>
        public IEnumerable SelectedItems
        {
            get { return lBoxActiveLayers.SelectedItems; }
            set
            {
                if (value == null)
                {
                    // Logger.Warn("SelectedItems tried to be set with null parameter.{0}", Environment.StackTrace);
                    return;
                }

                lBoxActiveLayers.ClearSelected();
                int lastIndex = 0;
                foreach (var item in value)
                {
                    lastIndex = lBoxActiveLayers.FindString(item.ToString(), lastIndex);
                    if (lastIndex == ListBox.NoMatches)
                    {
                        lastIndex = 0;
                        continue;
                    }
                    lBoxActiveLayers.SetSelected(lastIndex, true);
                }
            }
        }

         /// <summary>
         /// Gets or sets the items displayed in the scene tree.
        /// Note: This will clear the items not present in the given collection! 
         /// </summary>
        public IEnumerable Items
        {
            get { return lBoxActiveLayers.Items; }
            set
            {
                if (value == null)
                {
                    // Logger.Warn("Items tried to be set with null parameter.{0}", Environment.StackTrace);
                    return;
                }

                lBoxActiveLayers.Items.Clear();
                foreach (var item in value)
                {
                    lBoxActiveLayers.Items.Add(item);
                }
            }
        }
    }
}
