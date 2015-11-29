using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using ImageInterpolation.Properties;

namespace ImageInterpolation
{
    /// <summary>
    /// The main GUI class.
    /// Provides uniform handling for all UI objects and their
    /// interaction with the corresponding model(s) and controller(s).
    /// </summary>
    public partial class GuiView : Form
    {
        const float ProgressIncrement = 0.05f;
        const float ProgressStart = 0.0f;
        const float ProgressMax = 1.0f;
        readonly int _progressIntervals;
        int _iterationCount;

        private Bitmap _sourceImage;
        private Bitmap _targetImage;
        private Bitmap _destImage;

        // Trick to enable flicker-free images and fast resize
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;    // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

        public GuiView()
        {
            InitializeComponent();

            pictureBoxSource.AllowDrop = true;
            pictureBoxTarget.AllowDrop = true;
            pictureBoxSource.DragEnter += Picture_DragEnter;
            pictureBoxSource.DragDrop += Picture_DragDrop;
            pictureBoxTarget.DragEnter += Picture_DragEnter;
            pictureBoxTarget.DragDrop += Picture_DragDrop;

            btnLoadSource.Click += ButtonLoad_Click;
            btnLoadTarget.Click += ButtonLoad_Click;

            const float intervals = (ProgressMax - ProgressStart) / ProgressIncrement;
            _progressIntervals = (int)Math.Ceiling(intervals)+1;

            labelBlendingPercentage.Text = (_iterationCount * ProgressIncrement).ToString();
        }

        // ======================================================================
        // For Blending GuiModel get/set the view data
        // ======================================================================


        // ============================
        // Load images from buttons
        // ============================

        private void ButtonLoad_Click(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() != DialogResult.OK) return;

            if (((Button)sender).Name.ToLower().Contains("source"))
            {
                LoadSourceOrTarget(fileDialog.FileName, pictureBoxSource);
            }
            else if (((Button)sender).Name.ToLower().Contains("target"))
            {
                LoadSourceOrTarget(fileDialog.FileName, pictureBoxTarget);
            }
        }

        // ============================
        // Drag & drop images
        // ============================

        private void Picture_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                LoadSourceOrTarget(files[0], (Control)sender);
            }
        }

        private static void Picture_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy
                : DragDropEffects.None;
        }

        private static void DisposeAndReplace(ref Bitmap old, Bitmap with)
        {
            // Note: This can cause problems when used to modify property objects.
            if (old != null)
            {
                old.Dispose();
            }
            old = with;
        }

        private void LoadSourceOrTarget(string fileName, Control parent)
        {
            var image = Resources.placeholder;
            try
            {
                image = (Bitmap)Image.FromFile(fileName);
                if (_destImage != null)
                {
                    if (image.Width != _destImage.Width || image.Height != _destImage.Height)
                    {
                        _destImage.Dispose();
                        _destImage = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);
                    }
                }
                else
                {
                    _destImage = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);
                }
                
            }
            catch (FileNotFoundException) { }
            catch (ArgumentException) { }

            image = BitmapUtility.ConvertToFormat(image, PixelFormat.Format32bppArgb);
            image = ImageProcessingFramework.ImageOpacity(image, 0.5f);

            var scaled = BitmapUtility.Resize(
                image,
                parent.Size);

            parent.BackgroundImage = scaled;

            if (parent.Name.ToLower().Contains("source"))
            {
                DisposeAndReplace(ref _sourceImage, image);
            }
            else if (parent.Name.ToLower().Contains("target"))
            {
                DisposeAndReplace(ref _targetImage, image);
            }

            DisplayParameters(image, parent);
        }

        private static string[] FormatProperties(Bitmap img)
        {
            var res = new string[3];
            res[0] = img.Width.ToString() + @"x" + img.Height.ToString();
            res[1] = img.PixelFormat.ToString();
            using (var bh = new BitmapHelper(img))
            {
                res[2] = (bh.SizeBytes / 1024 / 1024).ToString("###.##");
            }
            return res;
        }

        private void DisplayParameters(Bitmap img, Control parent)
        {
            string[] prop = FormatProperties(img);
            if (parent.Name.ToLower().Contains("source"))
            {
                labelSrcRes.Text = prop[0];
                labelSrcFmt.Text = prop[1];
                labelSrcSize.Text = prop[2];
            }
            else if (parent.Name.ToLower().Contains("target"))
            {
                labelTarRes.Text = prop[0];
                labelTarFmt.Text = prop[1];
                labelTarSize.Text = prop[2];
            }
            else if (parent.Name.ToLower().Contains("intermediate"))
            {
                labelOutRes.Text = prop[0];
                labelOutFmt.Text = prop[1];
                labelOutSize.Text = prop[2];
            }
        }

        // =========================
        // Handle processing actions
        // =========================

        private void PreProcessing()
        {
            avgProcessing = 0;
            avgRefresh = 0;
            ticks = 0;
            _iterationCount = 0;
            //btnGenerate.Text = @"Stop";
            timer1.Enabled = true;
            //progressBar.Value = 0;
        }

        private void PostProcessing()
        {
            //btnGenerate.Text = @"Generate";
            timer1.Enabled = false;
            //progressBar.Value = 0;

            //MessageBox.Show("AVG PROCESSING: " + (avgProcessing / (double)ticks).ToString("###.###") + " ms.\n" +
            //                "AVG REFRESH: " + (avgRefresh / (double)ticks).ToString("###.###") + " ms.\n");
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (_sourceImage == null || _targetImage == null) return;

            /*if (btnGenerate.Text == @"Generate")
                PreProcessing();
            else
                PostProcessing();*/
        }

        private void ProcessingDone(Bitmap dst)
        {
            //progressBar.Value = (_iterationCount * 100) / _progressIntervals;
            _iterationCount++;

            if (_iterationCount >= _progressIntervals)
            {
                PostProcessing();
            }
            if (pictureBoxIntermediate.BackgroundImage != null)
            {
                //pictureBoxIntermediate.BackgroundImage.Dispose();
            }
            pictureBoxIntermediate.BackgroundImage = dst;
        }

        private long avgProcessing = 0;
        private long avgRefresh = 0;
        private int ticks = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            ticks++;

            var result = ImageProcessingFramework.ImageInterpolate(
                _sourceImage,
                _targetImage,
                ProgressIncrement * _iterationCount);

            labelProcessTime.Text = @"Processing[ms] : " + ImageProcessingFramework.LastOperationDuration;
            avgProcessing += ImageProcessingFramework.LastOperationDuration;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            DisplayParameters(result, pictureBoxIntermediate);
            ProcessingDone(BitmapUtility.Resize(
                result,
                pictureBoxIntermediate.Size,
                ConversionQuality.LowQuality,
                ConversionType.Overwrite));
            sw.Stop();

            labelPostTime.Text = @"Refresh[ms] : " + sw.ElapsedMilliseconds;
            avgRefresh += sw.ElapsedMilliseconds;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (_sourceImage == null || _targetImage == null) return;

            labelBlendingPercentage.Text = ((float)trackBar1.Value / trackBar1.Maximum).ToString();

            ImageProcessingFramework.ImageAlphaBlend(_sourceImage, _targetImage, _destImage);

            labelProcessTime.Text = @"Processing[ms] : " + ImageProcessingFramework.LastOperationDuration;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            DisplayParameters(_destImage, pictureBoxIntermediate);
            ProcessingDone(BitmapUtility.Resize(
                _destImage,
                pictureBoxIntermediate.Size,
                ConversionQuality.LowQuality,
                ConversionType.Copy));
            sw.Stop();

            labelPostTime.Text = @"Refresh[ms] : " + sw.ElapsedMilliseconds;
        }

        // ======================================================================
        // For Filter GuiModel get/set the view data
        // ======================================================================

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
    }
}
