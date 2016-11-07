using System;
using System.Threading;

namespace ImageInterpolation.ModuleImageBlending
{
    partial class BlendingController
    {
        private static BlendingController self;

        private class SourceImageDragDropListener : IActionListener
        {
            public void ActionPerformed(object sender, EventArgs e)
            {
                var ev = (MyDragDropEventArgs<string[]>)e;
                var files = ev.Data;

                ImageType type = self.view.IsSource(sender) ? ImageType.Source : ImageType.Target;
                var loaded = self.LoadSourceOrTarget(files[0], type);
                var scaled = self.ScaleImage(loaded, self.view.GetSizeOf(sender));

                self.view.SetNewImage(sender, scaled);
                self.DisplayParameters(loaded, type);
            }
        }

        private class LoadImageClickListener : IActionListener
        {
            public void ActionPerformed(object sender, EventArgs e)
            {
                var ev = (MyFileEventArgs)e;
                var fileName = ev.Data;

                ImageType type = self.view.IsSource(sender) ? ImageType.Source : ImageType.Target;
                var loaded = self.LoadSourceOrTarget(fileName, type);
                var scaled = self.ScaleImage(loaded, self.view.GetSizeOf(sender));

                self.view.SetNewImage(sender, scaled);
                self.DisplayParameters(loaded, type);
            }
        }

        private class TrackbarValueChangedListener : IActionListener
        {
            private long waitForActions;
            private Timer backgroundTimer;

            public TrackbarValueChangedListener()
            {
                backgroundTimer = new Timer(TimerCallback, null, 100, 500);
            }

            public void ActionPerformed(object sender, EventArgs e)
            {
                if (self.modelImages.Source == null || self.modelImages.Target == null)
                    return;

                float perc = (float)self.view.TrackbarValue / self.view.TrackbarMax;
                self.view.BlendingPercentage = perc;

                ImageProcessingFramework.ImageAlphaBlend(self.modelImages.Source, self.modelImages.Target, 
                    self.modelImages.Output);
                self.statProcessing.Track(ImageProcessingFramework.LastOperationDuration);

                ImageProcessingFramework.ImageAlphaBlend(self.guiImages.Source, self.guiImages.Target,
                    self.guiImages.Output);

                self.view.ProcessStats = @"Processing[ms] : " + self.statProcessing.LastValue();
                self.view.SetNewImageOutput(self.guiImages.Output);

                waitForActions = 1;
            }

            private void TimerCallback(object args)
            {
                if (Interlocked.Read(ref waitForActions) == 1)
                {
                    self.statProcessing.Clear();
                    Interlocked.Decrement(ref waitForActions);
                }
            }
        }

        private class ProcessingClickedListener : IActionListener
        {
            public void ActionPerformed(object sender, EventArgs e)
            {
                if (self.modelImages.Source == null || self.modelImages.Target == null)
                    return;

                self.InitProcessingTimer();
            }
        }
    }
}
