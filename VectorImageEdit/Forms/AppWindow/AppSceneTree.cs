using System.Collections;

namespace VectorImageEdit.Forms.AppWindow
{
    public partial class AppWindow
    {
        public void AddTreeSelectionChangedListener(IListener listener)
        {
            lBoxActiveLayers.SelectedIndexChanged += listener.ActionPerformed;
        }

        /// <summary>
        /// Gets or sets the currently selected item in the scene tree
        /// </summary>
        public string SceneTreeSelectedItem
        {
            get { return (lBoxActiveLayers.SelectedItem ?? "").ToString(); }
            set
            {
                if (string.IsNullOrEmpty(value)) return;
                lBoxActiveLayers.SelectedItem = value;
            }
        }

        /// <summary>
        /// Gets or sets the currently selected items in the scene tree
        /// </summary>
        public IEnumerable SceneTreeSelectedItems
        {
            get { return lBoxActiveLayers.SelectedItems; }
            set
            {
                lBoxActiveLayers.ClearSelected();
                foreach (var item in value)
                {
                    int index = lBoxActiveLayers.FindString(item.ToString(), 0);
                    if (index >= 0) lBoxActiveLayers.SetSelected(index, true);
                }
            }
        }

        public IEnumerable SceneTreeItems
        {
            get { return lBoxActiveLayers.Items; }
            set
            {
                lBoxActiveLayers.Items.Clear();
                foreach (var item in value)
                {
                    lBoxActiveLayers.Items.Add(item);
                }
            }
        }
    }
}
