using System;
using System.Linq;

namespace ImageInterpolation.ModuleFilter
{
    /// <summary>
    /// Describes how to perform the kernel normalization method
    /// </summary>
    enum NormalizationType
    {
        /// <summary>
        /// Divide kernel by sum of absolute values
        /// </summary>
        AbsSumDivide,
        /// <summary>
        /// Divide kernel by sum of values
        /// </summary>
        SumDivide
    }

    /// <summary>
    /// Filter with a convolution kernel that can be applied to an image.
    /// </summary>
    class Filter
    {
        private const float NormalizeTolerance = 0.01f;

        /// <summary>
        /// Creates a filter with a given size and specified kernel values.
        /// </summary>
        /// <param name="size"> Size of kernel </param>
        /// <param name="f"> Kernel values </param>
        public Filter(int size, float[] f)
        {
            Dimension = (int)Math.Sqrt(size);
            Size = size;
            Kernel = f;
        }

        /// <summary>
        /// Gets or sets the size of the kernel.
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// Gets the horizontal(vertical) dimension of the kernel
        /// </summary>
        public int Dimension { get; private set; }

        /// <summary>
        /// Gets or sets the kernel values
        /// </summary>
        public float[] Kernel { get; private set; }

        /// <summary>
        /// Checks if the kernel is normalized.
        /// Normalization 
        /// </summary>
        public bool Normalized { get { return !NeedNormalize(Kernel.Sum()); } }

        /// <summary>
        /// Multiplies the filter in this instance by the given value.
        /// </summary>
        /// <param name="scalar"> Value to multiply with </param>
        public void Multiply(float scalar)
        {
            for (int i = 0; i < Size; i++) Kernel[i] *= scalar;
        }

        /// <summary>
        /// Perform a normalization round on this filter instance
        /// using the specified normalization method
        /// </summary>
        /// <param name="type"> Normalization method </param>
        public void Normalize(NormalizationType type)
        {
            float factor = 0.0f;
            switch (type)
            {
                case NormalizationType.SumDivide:
                    factor = Kernel.Sum();
                    break;
                case NormalizationType.AbsSumDivide:
                    factor += Kernel.Sum(k => Math.Abs(k));
                    break;
            }
            if (NeedNormalize(factor)) Multiply(1.0f / factor);
        }

        private static bool NeedNormalize(float sum)
        {
            // Prevent division by zero or do nothing if sum is already 1.
            // Make sure to allow a tolerance for close to 0 or 1 values

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (sum == 0.0f) return false;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (sum == 1.0f) return false;
            if (Math.Abs(sum) <= NormalizeTolerance) return false;
            if (Math.Abs(sum - 1.0f) <= NormalizeTolerance) return false;
            return true;
        }
    }
}
