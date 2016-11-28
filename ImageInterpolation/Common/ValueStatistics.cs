using System;

namespace ImageInterpolation
{
    class ValueStatistics
    {
        private double sum;
        private double last;
        private int elements;

        public ValueStatistics()
        {
            Clear();
        }

        public void Track(double value)
        {
            elements++;
            last = value;
            sum += value;
        }

        public IDisposable Tracker
        {
            get { return new TimeStat(this); }
        }

        public double Average()
        {
            return sum / elements;
        }

        public double LastValue()
        {
            return last;
        }

        public void Clear()
        {
            sum = last = 0.0;
            elements = 0;
        }
    }
}