using System;
using System.Diagnostics;

namespace ImageInterpolation
{
    class TimeStat : IDisposable
    {
        private readonly Stopwatch sw = new Stopwatch();
        private readonly ValueStatistics tracker;

        public TimeStat(ValueStatistics tracker)
        {
            this.tracker = tracker;
            sw.Start();
        }

        public void Dispose()
        {
            sw.Stop();
            tracker.Track(sw.ElapsedMilliseconds);
        }
    }
}
