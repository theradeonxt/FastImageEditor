using System;
using System.Windows.Forms;

namespace VectorImageEdit.Forms
{
    public interface IListener
    {
        void ActionPerformed(object sender, EventArgs e);
    }

    public interface IDragListener
    {
        void ActionPerformed(object sender, DragEventArgs e);
    }
}
