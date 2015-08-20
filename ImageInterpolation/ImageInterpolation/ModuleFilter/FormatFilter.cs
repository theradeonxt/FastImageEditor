using System;
using System.Linq;
using System.Text;

namespace ImageInterpolation.ModuleFilter
{
    static class FormatFilter
    {
        /// <summary>
        /// Converts the kernel of the given filter to its equivalent string representation
        /// </summary>
        /// <param name="filter"> Input filter object </param>
        /// <returns> Filter kernel as string </returns>
        public static String Serialize(Filter filter)
        {
            StringBuilder sb = new StringBuilder();
            for (int p = 1; p <= filter.Size; p++)
            {
                sb.Append(filter.Kernel[p - 1]);
                sb.Append((p % filter.Dimension == 0) ? ";\n" : "; ");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Converts a given string corresponding to a valid filter kernel
        /// to the equivalent filter object containing the kernel.
        /// The filter properties are autodetected based on the kernel
        /// </summary>
        /// <param name="filterAsText"> Filter kernel as string </param>
        /// <returns> Output filter object </returns>
        public static Filter Deserialize(String filterAsText)
        {
            string[] rowSplit = { Environment.NewLine, "\n" };
            string[] rows = filterAsText.Split(rowSplit, StringSplitOptions.RemoveEmptyEntries);
            int dimension = rows.Length;
            int size = dimension * dimension;
            float[] kernel = new float[size];
            int ki = 0;

            foreach (string row in rows)
            {
                string[] valueSplit = { ";" };
                string[] values = row.Trim().Split(valueSplit, StringSplitOptions.RemoveEmptyEntries);
                if (values.Length != dimension)
                {
                    throw new FormatException("Autodetecting kernel dimensions failed.");
                }
                values.ToList().ForEach(v => kernel[ki++] = Convert.ToSingle(v));
            }

            return FilterBuilder.CustomFilter(size, kernel);
        }

        /// <summary>
        /// Return the string-formatted size property of the given filter
        /// </summary>
        /// <param name="filter"> Input filter </param>
        /// <returns> Formatted size property </returns>
        public static string SizeProperty(Filter filter)
        {
            return "Size=[" + filter.Dimension + "]x[" + filter.Dimension + "]";
        }

        /// <summary>
        /// Return the string-formatted normalize property of the given filter
        /// </summary>
        /// <param name="filter"> Input filter </param>
        /// <returns> Formatted normalize property </returns>
        public static string NormalizeProperty(Filter filter)
        {
            return filter.Normalized ? "Is normalized" : "Not normalized";
        }
    }
}
