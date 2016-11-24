using System;

namespace ImageInterpolation.ModuleFilter
{
    /// <summary>
    /// Helper Filter Factory
    /// </summary>
    static class FilterBuilder
    {
        /// <summary>
        /// Create a filter object with the given size and kernel values.
        /// </summary>
        /// <param name="size"> Kernel size </param>
        /// <param name="values"> Kernel values </param>
        /// <returns> Filter instance </returns>
        public static Filter CustomFilter(int size, float[] values)
        {
            if (size % 2 == 0 ||
                values.Length % 2 == 0 ||
                values.Length != size)
            {
                throw new FormatException("Invalid kernel dimensions.");
            }
            return new Filter(size, values);
        }

        /// <summary>
        /// Create a filter object with a kernel from the builtin ones.
        /// </summary>
        /// <param name="type"> Builtin kernel desired </param>
        /// <returns> Filter object </returns>
        public static Filter BuiltinFilter(BuiltinKernel type)
        {
            float[] values = BuiltinFilters.Kernel(type);
            Filter filter = CustomFilter(values.Length, values);
            return filter;
        }
    }
}
