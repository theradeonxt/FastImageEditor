using System;
using System.Windows.Forms;

namespace VectorImageEdit.WindowsFormsBridge
{
    public interface IListener
    {
        void ActionPerformed(object sender, EventArgs e);
    }
    public interface IMouseListener
    {
        // Used by controllers who are required to implement it
        void ActionPerformed(object sender, MyMouseEventArgs e);
        // Used by views, internally implemented in abstract listener
        void ActionPerformed(object sender, MouseEventArgs e);
    }
    public interface IDragListener
    {
        // Used by controllers who are required to implement it
        void ActionPerformed(object sender, MyDragEventArgs e);
        // Used by views, internally implemented in abstract listener
        void ActionPerformed(object sender, DragEventArgs e);
    }
}
