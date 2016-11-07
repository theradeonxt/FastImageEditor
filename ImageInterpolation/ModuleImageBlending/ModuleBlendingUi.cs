using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace ImageInterpolation.ModuleImageBlending
{
    public partial class ModuleBlendingUi : UserControl
    {
        public ModuleBlendingUi()
        {
            InitializeComponent();

            pictureBoxSource.AllowDrop = true;
            pictureBoxTarget.AllowDrop = true;

            pictureBoxSource.DragEnter += Picture_DragEnter;
            pictureBoxTarget.DragEnter += Picture_DragEnter;

            propertySource.ImageTag = "Source Image";
            propertyTarget.ImageTag = "Target Image";
            propertyOutput.ImageTag = "Output Image";
        }

        #region Properties

        public float BlendingPercentage
        {
            get
            {
                float value;
                float.TryParse(labelBlendingPercentage.Text, out value);
                return value;
            }
            set { labelBlendingPercentage.Text = value.ToString(CultureInfo.InvariantCulture); }
        }
        public int TrackbarValue { get { return trackBar1.Value; } }
        public int TrackbarMax { get { return trackBar1.Maximum; } }
        public string ProcessStats
        {
            set
            {
                labelProcessTime.InvokeIfRequired(s => { s.Text = value; });
            }
        }

        #endregion

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
            pictureBoxIntermediate.BackgroundImage = bmp;
        }
        public Size GetSizeOf(object sender)
        {
            var container = sender as PictureBox;
            if (container != null)
            {
                return container.Size;
            }
            return new Size(1, 1);
        }
        public bool IsSource(object sender)
        {
            var container = sender as PictureBox;
            if (container != null)
            {
                return container.Name == pictureBoxSource.Name;
            }
            return false;
        }

        public void AddImageDragDropListener(IActionListener listener)
        {
            pictureBoxSource.DragDrop += (sender, e) =>
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    var data = e.Data.GetData(DataFormats.FileDrop) as string[];
                    if (data != null && data.Length > 0)
                    {
                        listener.ActionPerformed(sender, new MyDragDropEventArgs<string[]>(data));
                    }
                }
            };
            pictureBoxTarget.DragDrop += (sender, e) =>
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    var data = e.Data.GetData(DataFormats.FileDrop) as string[];
                    if (data != null && data.Length > 0)
                    {
                        listener.ActionPerformed(sender, new MyDragDropEventArgs<string[]>(data));
                    }
                }
            };
        }

        public void AddLoadImageClickListener(IActionListener listener)
        {
            btnLoadSource.Click += (sender, e) =>
            {
                using (var fileDialog = new OpenFileDialog())
                {
                    if (fileDialog.ShowDialog() != DialogResult.OK) return;
                    listener.ActionPerformed(pictureBoxSource, new MyFileEventArgs(fileDialog.FileName));
                }
            };

            btnLoadTarget.Click += (sender, e) =>
            {
                using (var fileDialog = new OpenFileDialog())
                {
                    if (fileDialog.ShowDialog() != DialogResult.OK) return;
                    listener.ActionPerformed(pictureBoxTarget, new MyFileEventArgs(fileDialog.FileName));
                }
            };
        }

        public void AddTrackbarValueChangedListener(IActionListener listener)
        {
            trackBar1.ValueChanged += listener.ActionPerformed;
        }

        public void AddProcessingClickedListener(IActionListener listener)
        {
            btnStartProcessing.Click += listener.ActionPerformed;
        }

        private void Picture_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy
                : DragDropEffects.None;
        }
    }
}
