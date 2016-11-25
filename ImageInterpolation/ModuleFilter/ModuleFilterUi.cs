using System.Drawing;
using System.Windows.Forms;

namespace ImageInterpolation
{
    public partial class ModuleFilterUi : UserControl
    {
        public ModuleFilterUi()
        {
            InitializeComponent();
        }

        public Bitmap FilterInputImage
        {
            get { return (Bitmap)pictureBoxFilterInput.BackgroundImage; }
            set { pictureBoxFilterInput.BackgroundImage = value; }
        }
        public Bitmap FilterOutputImage
        {
            get { return (Bitmap)pictureBoxFilterOutput.BackgroundImage; }
            set { pictureBoxFilterOutput.BackgroundImage = value; }
        }
        public string KernelText
        {
            get { return richTextBoxKernel.Text; }
            set { richTextBoxKernel.Text = value; }
        }
        public bool NormalizeState { get { return checkBoxNormalize.Checked; } }
        public string SelectedNormalization { get { return (string)comboBoxNormalization.SelectedItem; } }
        public string SelectedFilter { get { return (string)comboBoxBuiltinFilters.SelectedItem; } }
        public string NormalizeProperty { set { tboxNormalizeState.Text = value; } }
        public string KernelSizeProperty { set { tboxKernelSize.Text = value; } }
        public string AddFilterOption { set { comboBoxBuiltinFilters.Items.Add(value); } }
        public string AddNormalizeOption { set { comboBoxNormalization.Items.Add(value); } }
        public Color FilterTitleColor { set { labelFilterTitle.BackColor = value; } }

        public Size GetSizeOf(object sender)
        {
            var container = sender as PictureBox;
            if (container != null)
            {
                return container.Size;
            }
            return new Size(1, 1);
        }
        public void SetNewImage(object sender, Bitmap bmp)
        {
            var container = sender as PictureBox;
            if (container != null)
            {
                container.BackgroundImage = bmp;
            }
        }
        public void SetNewImageOutput(Bitmap bmp)
        {
            pictureBoxFilterOutput.BackgroundImage = bmp;
        }

        public void AddKernelTextChangedListener(IActionListener listener)
        {
            richTextBoxKernel.TextChanged += listener.ActionPerformed;
        }

        public void AddBuiltinFilterChangedListener(IActionListener listener)
        {
            comboBoxBuiltinFilters.SelectedIndexChanged += listener.ActionPerformed;
        }

        public void AddNormalizationChangedListener(IActionListener listener)
        {
            comboBoxNormalization.SelectedIndexChanged += listener.ActionPerformed;
        }

        public void AddLoadSourceListener(IActionListener listener)
        {
            btnLoadSource.Click += (sender, e) =>
            {
                using (var fileDialog = new OpenFileDialog())
                {
                    if (fileDialog.ShowDialog() != DialogResult.OK) return;
                    listener.ActionPerformed(pictureBoxFilterInput, new MyFileEventArgs(fileDialog.FileName));
                }
            };
        }

        public void AddApplyFilterListener(IActionListener listener)
        {
            btnApplyKernel.Click += listener.ActionPerformed;
        }
    }
}
