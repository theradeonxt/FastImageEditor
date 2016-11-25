using ImageProcessingNET;
using System;
using System.Drawing;
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
                var file = ev.Data[0];
                var set = self.dataSet;
                var guiSize = self.view.GetSizeOf(sender);

                Image loaded;
                BitmapUtility.ExtractLocalImage(file, out loaded);

                ImageType type = self.view.IsSource(sender) ? ImageType.Source : ImageType.Target;
                switch (type)
                {
                    case ImageType.Source:
                        set.AddItem("SRC", (Bitmap)loaded, guiSize);
                        self.view.SetNewImage(sender, set.Item("SRC", ItemRole.Presentation));
                        self.DisplayParameters(set.Item("SRC", ItemRole.Presentation), type);
                        break;
                    case ImageType.Target:
                        set.AddItem("TAR", (Bitmap)loaded, guiSize);
                        self.view.SetNewImage(sender, set.Item("TAR", ItemRole.Presentation));
                        self.DisplayParameters(set.Item("TAR", ItemRole.Presentation), type);
                        break;
                }
            }
        }

        private class LoadImageClickListener : IActionListener
        {
            public void ActionPerformed(object sender, EventArgs e)
            {
                var ev = (MyFileEventArgs)e;
                var transport = new MyDragDropEventArgs<string[]>(new[] { ev.Data });

                new SourceImageDragDropListener()
                    .ActionPerformed(sender, transport);
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
                var set = self.dataSet;

                if (!set.InputImageCount(2))
                    return;

                float perc = (float)self.view.TrackbarValue / self.view.TrackbarMax;
                self.view.BlendingPercentage = perc;

                using (self.statProcessing.Tracker)
                {
                    ImageProcessingApi.ImageAlphaBlend(set.Item("SRC", ItemRole.Model),
                        set.Item("TAR", ItemRole.Model), set.Item("DST", ItemRole.Model));
                }

                ImageProcessingApi.ImageAlphaBlend(set.Item("SRC", ItemRole.Presentation),
                    set.Item("TAR", ItemRole.Presentation), set.Item("DST", ItemRole.Presentation));

                self.view.ProcessStats = @"Processing[ms] : " + self.statProcessing.LastValue();
                self.view.SetNewImageOutput(set.Item("DST", ItemRole.Presentation));

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
                if (!self.dataSet.InputImageCount(2))
                    return;

                self.InitProcessingTimer();
            }
        }
    }
}
