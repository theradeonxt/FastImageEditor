using System;
using VectorImageEdit.Models;
using VectorImageEdit.Views.Main;
using VectorImageEdit.WindowsFormsBridge;

namespace VectorImageEdit.Controllers
{
    /// <summary>
    /// Main application window related actions
    /// </summary>
    class WindowController
    {
        private readonly AppWindow view;
        private readonly WindowModel model;

        public WindowController(AppWindow view, WindowModel model)
        {
            this.view = view;
            this.model = model;

            this.view.AddWindowSizeChangedListener(new WindowSizeChangedListener(this));
            this.view.AddWindowMovedListener(new WindowMovedListener(this));
        }

        private class WindowSizeChangedListener : AbstractListener<WindowController>, IListener
        {
            public WindowSizeChangedListener(WindowController controller)
                : base(controller)
            {
            }

            public void ActionPerformed(object sender, EventArgs e)
            {
                Controller.model.WorkspaceResize(Controller.view.WorkspaceSize);
            }
        }
        private class WindowMovedListener : AbstractListener<WindowController>, IListener
        {
            public WindowMovedListener(WindowController controller)
                : base(controller)
            {
            }

            public void ActionPerformed(object sender, EventArgs e)
            {
                Controller.model.AppWindowMove();
            }
        }
    }
}
