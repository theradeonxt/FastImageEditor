using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace VectorImageEdit.WindowsFormsBridge
{
    /// <summary>
    /// Wrapper for mouse event arguments.
    /// </summary>
    public class MyMouseEventArgs
    {
        public enum MyMouseButton
        {
            Left,
            Middle,
            Right,
            None
        };

        public MyMouseEventArgs(int x, int y, MyMouseButton button)
        {
            X = x;
            Y = y;
            Button = button;
            Location = new Point(x, y);
        }

        public int X { get; private set; }
        public int Y { get; private set; }
        public Point Location { get; private set; }
        public MyMouseButton Button { get; private set; }
    }

    /// <summary>
    /// Wrapper for drag & drop event arguments.
    /// </summary>
    public class MyDragEventArgs
    {
        private readonly object data;
        private readonly string dataFormat;
        public const string FileDrop = "FileDrop";
        public MyDragDropEffects Effect { get; set; }
        public enum MyDragDropEffects
        {
            Copy,
            None
        };

        public MyDragEventArgs(string dataFormat, object data)
        {
            this.data = data;
            this.dataFormat = dataFormat;
        }

        public bool IsDataAvailable(string format)
        {
            return String.Compare(dataFormat, format, StringComparison.Ordinal) == 0;
        }
        public object GetData()
        {
            return data;
        }
    }

    /// <summary>
    /// Helper to convert betweeen mouse event argument classes.
    /// </summary>
    static class MouseEventTranslator
    {
        private static readonly Dictionary<MouseButtons, MyMouseEventArgs.MyMouseButton> Map
            = new Dictionary<MouseButtons, MyMouseEventArgs.MyMouseButton>
            {
                {MouseButtons.Left, MyMouseEventArgs.MyMouseButton.Left},
                {MouseButtons.None, MyMouseEventArgs.MyMouseButton.None},
                {MouseButtons.Right, MyMouseEventArgs.MyMouseButton.Right},
                {MouseButtons.Middle, MyMouseEventArgs.MyMouseButton.Middle},
                // Ignored
                {MouseButtons.XButton1, MyMouseEventArgs.MyMouseButton.None},
                {MouseButtons.XButton2, MyMouseEventArgs.MyMouseButton.None}
            };
        public static MyMouseEventArgs FromWinForms(MouseEventArgs e)
        {
            return new MyMouseEventArgs(e.X, e.Y, Map[e.Button]);
        }
    }

    /// <summary>
    /// Helper to convert betweeen drag & drop event argument classes.
    /// </summary>
    static class DragEventTranslator
    {
        public static MyDragEventArgs FromWinForms(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return new MyDragEventArgs(MyDragEventArgs.FileDrop, e.Data.GetData(DataFormats.FileDrop));
            }
            return new MyDragEventArgs(string.Empty, e.Data);
        }
    }
}
