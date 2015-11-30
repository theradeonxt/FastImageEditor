using System;
using System.Diagnostics;

namespace VectorImageEdit.Modules.GraphicsCompositing
{
    /// <summary> 
    /// Sequential profiling class for the Graphics pipeline
    /// 
    /// This will record the main stages in the frame rendering code. 
    /// Note: Use this only in single-threaded code (uses a shared timer object)
    /// 
    /// </summary>
    public class GraphicsProfiler
    {
        private readonly Stopwatch _timer;

        public GraphicsProfiler()
        {
            _timer = new Stopwatch();
            ClearFrameDuration = RasterizeObjectsDuration = DrawFrameDuration = 0;
        }

        public long ClearFrameDuration { get; private set; }
        public long RasterizeObjectsDuration { get; private set; }
        public long DrawFrameDuration { get; private set; }

        public void ProfileClearFrame(Action method)
        {
            ClearFrameDuration = ProfileInternal(method);
        }
        public void ProfileRasterizeObjects(Action method)
        {
            RasterizeObjectsDuration = ProfileInternal(method);
        }
        public void ProfileDrawFrame(Action method)
        {
            DrawFrameDuration = ProfileInternal(method);
        }

        private long ProfileInternal(Action method)
        {
            _timer.Reset();
            _timer.Start();
            method();
            _timer.Stop();
            return _timer.ElapsedMilliseconds;
        }
    }
}
