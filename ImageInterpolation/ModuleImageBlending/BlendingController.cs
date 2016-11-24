using ImageInterpolation.Properties;
using ImageProcessingNET;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using Timer = System.Threading.Timer;

namespace ImageInterpolation.ModuleImageBlending
{
    sealed partial class BlendingController
    {
        private readonly ModuleBlendingUi view;

        const float ProgressIncrement = 0.05f;
        const float ProgressStart = 0.0f;
        const float ProgressMax = 1.0f;
        readonly int progressIntervals;
        int iterationCount;

        private readonly ValueStatistics statProcessing; // statistics for processing model images
        private readonly InputOutputParams modelImages;  // images in data model (full size)
        private readonly InputOutputParams guiImages;    // images shown on gui (prescaled)

        private int generateMs = 100;
        private Timer generationTimer;
        private readonly ManualResetEvent processingGuard;

        public BlendingController(ModuleBlendingUi view)
        {
            this.view = view;
            self = this;

            const float intervals = (ProgressMax - ProgressStart) / ProgressIncrement;
            progressIntervals = (int)Math.Ceiling(intervals) + 1;

            modelImages = new InputOutputParams();
            guiImages = new InputOutputParams();

            statProcessing = new ValueStatistics();
            processingGuard = new ManualResetEvent(false);

            SetupUi();
        }

        private void SetupUi()
        {
            view.AddImageDragDropListener(new SourceImageDragDropListener());
            view.AddLoadImageClickListener(new LoadImageClickListener());
            view.AddTrackbarValueChangedListener(new TrackbarValueChangedListener());
            view.AddProcessingClickedListener(new ProcessingClickedListener());

            view.BlendingPercentage = (int)(iterationCount * ProgressIncrement);
        }

        private Bitmap LoadSourceOrTarget(string fileName, ImageType type = ImageType.DontCare, object sender = null)
        {
            Bitmap loaded;
            try
            {
                loaded = (Bitmap)Image.FromFile(fileName);
                if (modelImages.Output != null)
                {
                    // dispose old output images and allocate new ones according to dimensions
                    if (loaded.Width != modelImages.Output.Width || loaded.Height != modelImages.Output.Height)
                    {
                        modelImages.Output.Dispose();
                        guiImages.Output.Dispose();

                        modelImages.Output = new Bitmap(loaded.Width, loaded.Height, PixelFormat.Format32bppArgb);
                        var size = self.view.GetSizeOf(sender);
                        guiImages.Output = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);
                    }
                }
                else
                {
                    modelImages.Output = new Bitmap(loaded.Width, loaded.Height, PixelFormat.Format32bppArgb);
                    var size = self.view.GetSizeOf(sender);
                    guiImages.Output = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);
                }
                // ensure the correct pixel format of the loaded image
                loaded = BitmapUtility.ConvertToFormat(loaded, PixelFormat.Format32bppArgb, ConversionQuality.HighQuality, ConversionType.Overwrite);

                // replace old source/target images with new ones (discard the old ones)
                if (type == ImageType.Source)
                {
                    DisposeAndReplace(ref modelImages.Source, loaded);
                    DisposeAndReplace(ref guiImages.Source, loaded);
                }
                else if (type == ImageType.Target)
                {
                    DisposeAndReplace(ref modelImages.Target, loaded);
                    DisposeAndReplace(ref guiImages.Target, loaded);
                }
            }
            catch (Exception ex)
            {
                // discard input data, since one of them failed to load
                DisposeAndReplace(ref modelImages.Source, null);
                DisposeAndReplace(ref modelImages.Source, null);
                DisposeAndReplace(ref guiImages.Target, null);
                DisposeAndReplace(ref guiImages.Source, null);

                Debug.WriteLine("[LoadSourceOrTarget] Exception: {0}", ex);
                return Resources.placeholder;
            }
            return loaded;
        }

        private Bitmap ScaleImage(Bitmap modelImage, Size scaledSize)
        {
            var scaled = BitmapUtility.Resize(
                modelImage, scaledSize, ConversionQuality.HighQuality);
            return scaled;
        }

        private void DisposeAndReplace(ref Bitmap old, Bitmap with)
        {
            // Note: This can cause problems when used to modify property objects.
            if (old != null)
            {
                old.Dispose();
            }
            old = with;
        }

        private void DisplayParameters(Bitmap img, ImageType type)
        {
            ImagePropertiesUi propertyObject;
            switch (type)
            {
                case ImageType.Source:
                    propertyObject = self.view.propertySource;
                    break;
                case ImageType.Target:
                    propertyObject = self.view.propertyTarget;
                    break;
                case ImageType.Output:
                    propertyObject = self.view.propertyOutput;
                    break;
                default:
                    return;
            }
            float rawSize;
            using (var bh = new BitmapHelper(img))
            {
                rawSize = bh.SizeBytes / 1024.0f / 1024.0f;
            }
            propertyObject.Resolution = img.Size;
            propertyObject.RawSize = rawSize;
            propertyObject.PixelFormat = img.PixelFormat;
        }

        private void InitProcessingTimer()
        {
            iterationCount = 0;
            statProcessing.Clear();
            generationTimer = new Timer(TimerCallback, null, 50, generateMs);
        }

        private void PostProcessing()
        {
            generationTimer.Dispose();

            guiImages.Output = ImageProcessingApi.ImageInterpolate(
                guiImages.Source, guiImages.Target, ProgressIncrement * iterationCount);

            view.ProcessStats = @"Processing[ms] : " + statProcessing.LastValue();
            view.SetNewImageOutput(guiImages.Output);
        }

        private void ProcessingDone()
        {
            iterationCount++;

            if (iterationCount >= progressIntervals)
            {
                PostProcessing();
            }
            //if (pictureBoxIntermediate.BackgroundImage != null)
            {
                //pictureBoxIntermediate.BackgroundImage.Dispose();
            }
            view.SetNewImageOutput(guiImages.Output);

            processingGuard.Reset();
        }

        private void TimerCallback(object args)
        {
            try
            {
                processingGuard.WaitOne();
                processingGuard.Set();

                modelImages.Output = ImageProcessingApi.ImageInterpolate(
                    modelImages.Source, modelImages.Target, ProgressIncrement * iterationCount);
                statProcessing.Track(ImageProcessingApi.LastOperationDuration);
                ProcessingDone();
            }
            finally
            {
                processingGuard.Reset();
            }
        }
    }
}
