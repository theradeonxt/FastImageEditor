using System;
using System.Diagnostics;

namespace VectorImageEdit.Modules
{
    /// <summary> 
    /// Sequential profiling class for the Graphics pipeline
    /// 
    /// Note: Use this only in single-threaded code (uses a shared timer object)
    /// 
    /// </summary>
    public class GraphicsProfiler
    {
        private readonly Stopwatch _timer;

        public GraphicsProfiler()
        {
            _timer = new Stopwatch();
            ClearFrameDuration = RasterizeObjectsDuration = DrawFrameDuration = ResizeFrameDuration = 0;
        }

        public long ClearFrameDuration { get; private set; }
        public long RasterizeObjectsDuration { get; private set; }
        public long DrawFrameDuration { get; private set; }
        public long ResizeFrameDuration { get; private set; }

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
        public void ProfileResizeFrame(Action method)
        {
            ResizeFrameDuration = ProfileInternal(method);
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
