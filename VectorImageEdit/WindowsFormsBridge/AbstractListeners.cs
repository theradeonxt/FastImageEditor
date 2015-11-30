using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace VectorImageEdit.WindowsFormsBridge
{
    /// <summary>
    /// Common Base Listener class
    /// 
    /// - contains a reference to parent controller that manages this listener
    /// </summary>
    /// <typeparam name="TController"> Controller typename </typeparam>
    internal abstract class AbstractListener<TController>
    {
        protected readonly TController Controller;

        protected AbstractListener(TController controller)
        {
            Controller = controller;
        }
    }

    /// <summary>
    /// Special mouse eistener, extends the base listener and abstracts the mouse events
    /// </summary>
    /// <typeparam name="TController"></typeparam>
    internal abstract class AbstractMouseListener<TController> : AbstractListener<TController>
    {
        protected delegate void HandlerDelegate(object sender, MyMouseEventArgs e);
        protected HandlerDelegate Handler;

        protected AbstractMouseListener(TController controller)
            : base(controller)
        {
        }

        // Implement action required by view
        public void ActionPerformed(object sender, MouseEventArgs e)
        {
            // Calls our listeners inside controllers
            Handler(sender, MouseEventTranslator.FromWinForms(e));
        }
    }

    /// <summary>
    /// Special drag & drop event listener, extends the base listener and abstracts the drag & drop events
    /// </summary>
    /// <typeparam name="TController"></typeparam>
    internal abstract class AbstractDragListener<TController> : AbstractListener<TController>
    {
        protected delegate void HandlerDelegate(object sender, MyDragEventArgs e);
        protected HandlerDelegate Handler;

        protected AbstractDragListener(TController controller)
            : base(controller)
        {
        }

        // Implement action required by view
        public void ActionPerformed(object sender, DragEventArgs e)
        {
            // Ensure the modifications are applied to DragEventArgs object
            MyDragEventArgs ev = DragEventTranslator.FromWinForms(e);
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;

            // Calls our listeners inside controllers
            Handler(sender, ev);
        }
    }

    /// <summary>
    /// Helper to convert betweeen mouse event argument classes
    /// </summary>
    internal static class MouseEventTranslator
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
    /// Helper to convert betweeen drag & drop event argument classes
    /// </summary>
    internal static class DragEventTranslator
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

    /// <summary>
    /// Wrapper for mouse event arguments
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
    /// Wrapper for drag & drop event arguments
    /// </summary>
    public class MyDragEventArgs
    {
        private readonly object _data;
        private readonly string _dataFormat;
        public const string FileDrop = "FileDrop";
        public MyDragDropEffects Effect { get; set; }
        public enum MyDragDropEffects
        {
            Copy,
            None
        };

        public MyDragEventArgs(string dataFormat, object data)
        {
            _data = data;
            _dataFormat = dataFormat;
        }

        public bool IsDataAvailable(string dataFormat)
        {
            return String.Compare(_dataFormat, dataFormat, StringComparison.Ordinal) == 0;
        }
        public object GetData()
        {
            return _data;
        }
    }
}
