using ImageProcessingNET;
using System;
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

        private readonly ValueStatistics statProcessing;
        private readonly ImageContainer dataSet;

        private int generateMs = 100;
        private Timer generationTimer;
        private readonly ManualResetEvent processingGuard;

        public BlendingController(ModuleBlendingUi view)
        {
            this.view = view;
            self = this;

            const float intervals = (ProgressMax - ProgressStart) / ProgressIncrement;
            progressIntervals = (int)Math.Ceiling(intervals) + 1;

            dataSet = new ImageContainer
            {
                ContainerFormat = PixelFormat.Format24bppRgb
            };

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

            ImageProcessingApi.ImageInterpolate(dataSet.Item("SRC", ItemRole.Presentation),
                dataSet.Item("TAR", ItemRole.Presentation),
                dataSet.Item("DST", ItemRole.Presentation),
                ProgressIncrement * iterationCount);

            view.ProcessStats = @"Processing[ms] : " + statProcessing.LastValue();
            view.SetNewImageOutput(dataSet.Item("DST", ItemRole.Presentation));
        }

        private void ProcessingDone()
        {
            iterationCount++;
            if (iterationCount >= progressIntervals)
            {
                PostProcessing();
            }
            processingGuard.Reset();
        }

        private void TimerCallback(object args)
        {
            try
            {
                processingGuard.WaitOne();
                processingGuard.Set();

                using (statProcessing.Tracker)
                {
                    ImageProcessingApi.ImageInterpolate(dataSet.Item("SRC", ItemRole.Model),
                        dataSet.Item("TAR", ItemRole.Model),
                        dataSet.Item("DST", ItemRole.Model),
                        ProgressIncrement * iterationCount);
                }

                ProcessingDone();
            }
            finally
            {
                processingGuard.Reset();
            }
        }
    }
}
