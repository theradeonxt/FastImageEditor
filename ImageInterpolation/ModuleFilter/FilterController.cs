﻿using System;
using System.Drawing;
using System.Linq;

namespace ImageInterpolation.ModuleFilter
{
    sealed partial class FilterController
    {
        private readonly ModuleFilterUi view;
        private readonly ImageContainer dataSet;
        private Filter selectedFilter;
        private ValueStatistics statProcessing;

        public FilterController(ModuleFilterUi view)
        {
            this.view = view;
            self = this;

            statProcessing = new ValueStatistics();
            dataSet = new ImageContainer { InputCount = 1 };
            BuiltinFilters.Load();

            SetupUi();
        }

        private void SetupUi()
        {
            view.AddKernelTextChangedListener(new KernelTextChangedListener());
            view.AddBuiltinFilterChangedListener(new BuiltinFilterChangedListener());
            view.AddNormalizationChangedListener(new NormalizationChangedListener());
            view.AddLoadSourceListener(new LoadSourceListener());
            view.AddApplyFilterListener(new ApplyFilterListener());

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
            if (string.Compare(filterAsText, view.KernelText, StringComparison.Ordinal) != 0)
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

        private void StoreSelectedFilter(Filter filter)
        {
            selectedFilter = filter;
        }

        private bool HasSelectedFilter
        {
            get { return self.selectedFilter != null; }
        }
    }
}
