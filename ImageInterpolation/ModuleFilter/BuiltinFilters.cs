using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ImageInterpolation.ModuleFilter
{
    enum BuiltinKernel
    {
        Identity3X3,
        Blur3X3Average, Blur3X3HvAverage,
        Sharpen3X3, Sharpen5X5,
        Emboss3X3,
        EdgeEnhance3X3,
        EdgeDetect3X3, EdgeDetect3X3Any, EdgeDetect5X5Horiz, EdgeDetect5X5Vert, EdgeDetect5X5Deg45,
        MotionBlur9X9Tlbr
    }

    static class BuiltinFilters
    {
        private const string FilterFileExtension = ".txt";
        private const string FilterLocation = "BuiltinFilters";

        private static readonly Dictionary<BuiltinKernel, float[]> Map =
        new Dictionary<BuiltinKernel, float[]>();

        public static float[] Kernel(BuiltinKernel type)
        {
            float[] kernel = Map[BuiltinKernel.Identity3X3];
            Map.TryGetValue(type, out kernel);
            return kernel;
        }

        public static void Load()
        {
            Map.Clear();

            // Load saved standard filters
            Enum.GetValues((typeof(BuiltinKernel))).
               Cast<BuiltinKernel>().ToList().
               ForEach(filterId =>
               {
                   string expectedFile = Path.Combine(FilterLocation, filterId.ToString() + FilterFileExtension);
                   if (File.Exists(expectedFile))
                   {
                       string kernelAsText = File.ReadAllText(expectedFile);
                       Filter filter = FormatFilter.Deserialize(kernelAsText);
                       Map.Add(filterId, filter.Kernel);
                   }
               });

            // Standard no-op filter: hard-coded to be used 
            // as fallback in case loading from files failed
            float[] kernel = { 0, 0, 0, 0, 1, 0, 0, 0, 0 };
            Map.Add(BuiltinKernel.Identity3X3, kernel);
        }
    }
}
