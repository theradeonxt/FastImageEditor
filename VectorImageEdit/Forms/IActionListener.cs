using System;
using System.Windows.Forms;

namespace VectorImageEdit.Forms
{
    public interface IActionListener
    {
        void ActionPerformed(object sender, EventArgs e);
    }

    public interface IDragActionListener
    {
        void ActionPerformed(object sender, DragEventArgs e);
    }
}
