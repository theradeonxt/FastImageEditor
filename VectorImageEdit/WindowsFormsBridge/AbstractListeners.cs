using System.Windows.Forms;

namespace VectorImageEdit.WindowsFormsBridge
{
    /// <summary>
    /// Common Base Listener class.
    /// Contains a reference to parent controller that manages this listener.
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
    /// Special mouse eistener, extends the base listener and abstracts the mouse events.
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
    /// Special drag & drop event listener, extends the base listener and abstracts the drag & drop events.
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
}
