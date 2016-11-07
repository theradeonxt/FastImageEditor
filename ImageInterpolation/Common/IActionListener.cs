using System;

namespace ImageInterpolation
{
    public interface IActionListener
    {
        void ActionPerformed(object sender, EventArgs e);
    }

    public class MyDragDropEventArgs<T> : EventArgs
    {
        public T Data { get; private set; }

        public MyDragDropEventArgs(T data)
        {
            Data = data;
        }
    }

    public class MyFileEventArgs : EventArgs
    {
        public string Data { get; private set; }

        public MyFileEventArgs(string data)
        {
            Data = data;
        }
    }
}
