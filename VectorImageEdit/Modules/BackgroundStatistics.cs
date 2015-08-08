using System.Threading;

namespace VectorImageEdit.Modules
{
    static class BackgroundStatitics
    {
        static BackgroundStatitics()
        {
            _imageMemory = 0;
        }

        public static long ImageMemory
        {
            get { return Interlocked.Read(ref _imageMemory); }
        }

        public static void CommitImageMemory(long bytes)
        {
            if (bytes <= 0) return;
            Interlocked.Add(ref _imageMemory, bytes);
        }

        private static long _imageMemory;
    }
}
