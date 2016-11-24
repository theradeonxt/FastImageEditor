using System;
using System.Drawing;
using ImageProcessingNET;

namespace ImageInterpolation.ModuleFilter
{
    partial class FilterController
    {
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
                Image loaded = self.modelData.Source;
                BitmapUtility.ExtractLocalImage(file, out loaded);
            }
        }

        private class ApplyFilterListener : IActionListener
        {
            public void ActionPerformed(object sender, EventArgs e)
            {
                ImageProcessingApi.ConvolutionFilter(self.modelData.Source, self.modelData.Output,
                    self.selectedFilter.Kernel);
            }
        }
    }
}
