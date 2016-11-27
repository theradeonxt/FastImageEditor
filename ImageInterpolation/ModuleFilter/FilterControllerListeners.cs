using ImageProcessingNET;
using System;
using System.Drawing;

namespace ImageInterpolation.ModuleFilter
{
    partial class FilterController
    {
        private static FilterController self;

        private class KernelTextChangedListener : IActionListener
        {
            public void ActionPerformed(object sender, EventArgs e)
            {
                try
                {
                    Filter filter = FormatFilter.Deserialize(self.view.KernelText);
                    if (self.view.NormalizeState)
                    {
                        // post processing the kernel if normalization is desired
                        filter.Normalize(self.ActiveNormalization());
                    }
                    self.OnSuccessDisplayFilter(filter);
                    self.StoreSelectedFilter(filter);
                }
                catch (FormatException)
                {
                    self.OnErrorAction();
                }
            }
        }

        private class BuiltinFilterChangedListener : IActionListener
        {
            public void ActionPerformed(object sender, EventArgs e)
            {
                // obtain builtin filter for selected item
                BuiltinKernel builtin = (BuiltinKernel)Enum.Parse(typeof(BuiltinKernel),
                    self.view.SelectedFilter);
                Filter filter = FilterBuilder.BuiltinFilter(builtin);
                self.OnSuccessDisplayFilter(filter);
                self.StoreSelectedFilter(filter);
            }
        }

        private class NormalizationChangedListener : IActionListener
        {
            public void ActionPerformed(object sender, EventArgs e)
            {
                IActionListener listener = new BuiltinFilterChangedListener();
                listener.ActionPerformed(sender, e);
            }
        }

        private class LoadSourceListener : IActionListener
        {
            public void ActionPerformed(object sender, EventArgs e)
            {
                var ev = (MyFileEventArgs)e;
                var file = ev.Data;
                var set = self.dataSet;
                var guiSize = self.view.GetSizeOf(sender);

                Image loaded;
                BitmapUtility.ExtractLocalImage(file, out loaded);

                set.AddItem("SRC", (Bitmap)loaded, guiSize);
                set.AddOutput(guiSize);

                self.view.SetNewImage(sender, set.Item("SRC", ItemRole.Presentation));
            }
        }

        private class ApplyFilterListener : IActionListener
        {
            public void ActionPerformed(object sender, EventArgs e)
            {
                var set = self.dataSet;

                if (!self.HasSelectedFilter)
                    return;

                ImageProcessingApi.ConvolutionFilter(set.Item("SRC", ItemRole.Model),
                    set.Item("DST", ItemRole.Model),
                    self.selectedFilter.Kernel);

                ImageProcessingApi.ConvolutionFilter(set.Item("SRC", ItemRole.Presentation),
                    set.Item("DST", ItemRole.Presentation),
                    self.selectedFilter.Kernel);

                self.view.SetNewImageOutput(set.Item("DST", ItemRole.Presentation));
            }
        }
    }
}
