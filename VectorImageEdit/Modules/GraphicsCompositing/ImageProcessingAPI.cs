using System.Runtime.InteropServices;
using System.Security;

namespace VectorImageEdit.Modules.GraphicsCompositing
{
    /// <summary>
    /// Wrapper for the ImageProcessing library
    /// with imported method definitions  
    /// </summary>
    static class ImageProcessingApi
    {
        [SuppressUnmanagedCodeSecurity]
        [DllImport("ImageProcessing.dll", CallingConvention = CallingConvention.StdCall)]
        public static unsafe extern int OpacityAdjust_32bgra(
            byte* source,
            byte* destination,
            uint sizeBytes,
            float percentage
        );

        [SuppressUnmanagedCodeSecurity]
        [DllImport("ImageProcessing.dll", CallingConvention = CallingConvention.StdCall)]
        public static unsafe extern int AlphaBlend32bgra_32bgra(
            byte* source,
            byte* target,
            byte* destination,
            uint sizeBytes
        );

        [DllImport("ImageProcessing.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int SetMultiThreadingStatus(
            [MarshalAs(UnmanagedType.LPStr)]string moduleName,
            int status
        );
    }
}
