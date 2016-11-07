using System;
using System.Drawing;
using System.Linq;
using ImageInterpolation.ModuleImageBlending;

namespace ImageInterpolation.ModuleFilter
{
    sealed partial class FilterController
    {
        private readonly ModuleFilterUi view;
        private static FilterController self;
        private InputOutputParams modelData;
        private InputOutputParams guiData;

        public FilterController(ModuleFilterUi view)
        {
            this.view = view;
            self = this;

            BuiltinFilters.Load();
            SetupUi();
        }

        private void SetupUi()
        {
            view.AddKernelTextChangedListener(new KernelTextChangedListener());
            view.AddBuiltinFilterChangedListener(new BuiltinFilterChangedListener());
            view.AddNormalizationChangedListener(new NormalizationChangedListener());

            // initialize combobox items
            foreach (var builtinFilter in Enum.GetValues(typeof(BuiltinKernel)))
            {
                view.AddFilterOption = ((BuiltinKernel)builtinFilter).ToString();
            }
            foreach (var normalization in Enum.GetValues(typeof(NormalizationType)))
            {
                view.AddNormalizeOption = ((NormalizationType)normalization).ToString();
            }
        }

        private NormalizationType ActiveNormalization()
        {
            // get the user selected normalization type
            return Enum.GetValues(typeof(NormalizationType))
                .Cast<NormalizationType>()
                .First(item => item.ToString() == view.SelectedNormalization);
        }

        private void OnSuccessDisplayFilter(Filter filter)
        {
            // update filter properties shown
            view.NormalizeProperty = FormatFilter.NormalizeProperty(filter);
            view.KernelSizeProperty = FormatFilter.SizeProperty(filter);
            view.FilterTitleColor = Color.MediumAquamarine;

            // update the filter's kernel only if it's changed
            string filterAsText = FormatFilter.Serialize(filter);
            if (String.Compare(filterAsText, view.KernelText, StringComparison.Ordinal) != 0)
            {
                view.KernelText = filterAsText;
            }
        }

        private void OnErrorAction()
        {
            view.KernelSizeProperty = "";
            view.NormalizeProperty = "";
            view.FilterTitleColor = Color.Salmon;
        }
    }
}
