using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VectorImageEdit.Forms
{
    interface IActionListener
    {
        void ActionPerformed(object sender, EventArgs e);
    }

    interface IDragActionListener
    {
        void ActionPerformed(object sender, DragEventArgs e);
    }
}
