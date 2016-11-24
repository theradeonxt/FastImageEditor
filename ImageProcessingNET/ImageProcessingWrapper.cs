using System.Runtime.InteropServices;

namespace ImageProcessingNET
{
    /// <summary>
    /// Wrapper of exported C functions from ImageProcessing.dll
    /// </summary>
    internal static class ImageProcessingWrapper
    {
        [DllImport("ImageProcessing.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe int Blend24bgr_24bgr(
            byte* source,
            byte* target,
            byte* destination,
            uint sizeBytes,
            float percentage
            );

        [DllImport("ImageProcessing.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe int OpacityAdjust_32bgra(
            byte* source,
            byte* destination,
            uint sizeBytes,
            float percentage
            );

        [DllImport("ImageProcessing.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe int AlphaBlend32bgra_32bgra(
            byte* source,
            byte* target,
            byte* destination,
            uint sizeBytes
            );

        [DllImport("ImageProcessing.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe int ConvFilter_32bgra_ref(
            byte* source,
            byte* destination,
            uint sizeBytes,
            uint strideBytes,
            float[] kernel,
            uint kernelWidth,
            uint kernekHeight
            );
    }
}