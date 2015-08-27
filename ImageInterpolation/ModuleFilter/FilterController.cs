using System;
using System.Drawing;
using System.Linq;

namespace ImageInterpolation.ModuleFilter
{
    sealed class FilterController
    {
        private static GuiView _view;

        public FilterController(GuiView view)
        {
            _view = view;

            BuiltinFilters.Load();

            view.AddKernelTextChangedListener(new KernelTextChangedListener());
            view.AddBuiltinFilterChangedListener(new BuiltinFilterChangedListener());
            view.AddNormalizationChangedListener(new NormalizationChangedListener());

            // initialize combobox items
            foreach (var builtinFilter in Enum.GetValues(typeof(BuiltinKernel)))
            {
                _view.AddFilterOption = ((BuiltinKernel)builtinFilter).ToString();
            }
            foreach (var normalization in Enum.GetValues(typeof(NormalizationType)))
            {
                _view.AddNormalizeOption = ((NormalizationType)normalization).ToString();
            }
        }

        private class KernelTextChangedListener : IActionListener
        {
            public void ActionPerformed(object sender, EventArgs e)
            {
                try
                {
                    Filter filter = FormatFilter.Deserialize(_view.KernelText);
                    if (_view.NormalizeState)
                    {
                        // post processing the kernel if normalization is desired
                        filter.Normalize(ActiveNormalization());
                    }
                    OnSuccessDisplayFilter(filter);
                }
                catch (FormatException)
                {
                    OnErrorAction();
                }
            }
        }

        private class BuiltinFilterChangedListener : IActionListener
        {
            public void ActionPerformed(object sender, EventArgs e)
            {
                // obtain builtin filter for selected item
                BuiltinKernel builtin = (BuiltinKernel)Enum.Parse(typeof(BuiltinKernel), _view.SelectedFilter);
                Filter filter = FilterBuilder.BuiltinFilter(builtin);
                OnSuccessDisplayFilter(filter);
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

        private static NormalizationType ActiveNormalization()
        {
            // get the user selected normalization type
            return Enum.GetValues(typeof(NormalizationType))
                .Cast<NormalizationType>()
                .First(item => item.ToString() == _view.SelectedNormalization);
        }

        private static void OnSuccessDisplayFilter(Filter filter)
        {
            // update filter properties shown
            _view.NormalizeProperty = FormatFilter.NormalizeProperty(filter);
            _view.KernelSizeProperty = FormatFilter.SizeProperty(filter);
            _view.FilterTitleColor = Color.MediumAquamarine;

            // update the filter's kernel only if it's changed
            string filterAsText = FormatFilter.Serialize(filter);
            if (String.Compare(filterAsText, _view.KernelText, StringComparison.Ordinal) != 0)
            {
                _view.KernelText = filterAsText;
            }
        }

        private static void OnErrorAction()
        {
            _view.KernelSizeProperty = "";
            _view.NormalizeProperty = "";
            _view.FilterTitleColor = Color.Salmon;
        }
    }
}
