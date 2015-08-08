using System;
using System.Windows.Forms;

namespace VectorImageEdit.Forms
{
    public partial class ProgressWindow : Form
    {
        private int increment;

        public ProgressWindow(string title, int start, int end, int increment)
        {
            InitializeComponent();

            if (start >= 0 && end > start && increment > 0 && !string.IsNullOrEmpty(title))
            {
                progressBar.Minimum = start;
                progressBar.Maximum = end;
                this.increment = increment;
                Text = title;
            }
        }

        public void IncrementProgress()
        {
            try
            {
                progressBar.Increment(increment);
            }
            catch(InvalidOperationException)
            {
                // this just ensures the progressbar works correctly
            }
        }
    }
}
