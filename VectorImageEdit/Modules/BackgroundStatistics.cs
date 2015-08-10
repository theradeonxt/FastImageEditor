using System.Threading;
using Microsoft.VisualBasic.Devices;

namespace VectorImageEdit.Modules
{
    static class BackgroundStatitics
    {
        static BackgroundStatitics()
        {
            _imageMemory = 0;
            CompInfo = new ComputerInfo();
            _availableRam = CompInfo.AvailablePhysicalMemory;
        }

        public static long ImageMemory
        {
            get { return Interlocked.Read(ref _imageMemory); }
        }

        public static int MemoryUsagePercent
        {
            get
            {
                _availableRam = CompInfo.AvailablePhysicalMemory;
                ulong percent = (100UL - (_availableRam * 100UL) / CompInfo.TotalPhysicalMemory);
                return (int) percent;
            }
        }

        public static void CommitImageMemory(long bytes)
        {
            if (bytes <= 0) return;
            Interlocked.Add(ref _imageMemory, bytes);
        }

        private static long _imageMemory;
        private static readonly ComputerInfo CompInfo;
        private static ulong _availableRam;
    }
}
