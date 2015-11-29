using System;
using VectorImageEdit.Models;
using VectorImageEdit.Views.Main;
using VectorImageEdit.WindowsFormsBridge;

namespace VectorImageEdit.Controllers
{
    ///// <summary>
    /// Main application window related actions
    ///// </summary>
    class WindowController
    {
        private readonly AppWindow _appView;
        private readonly WindowModel _model;

        public WindowController(AppWindow appView, WindowModel model)
        {
            _appView = appView;
            _model = model;

            _appView.AddWindowSizeChangedListener(new WindowSizeChangedListener(this));
            _appView.AddWindowMovedListener(new WindowMovedListener(this));
        }

        private class WindowSizeChangedListener : AbstractListener<WindowController>, IListener
        {
            public WindowSizeChangedListener(WindowController controller)
                : base(controller)
            {
            }

            public void ActionPerformed(object sender, EventArgs e)
            {
                Controller._model.WorkspaceResize(Controller._appView.WorkspaceSize);
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
                Controller._model.AppWindowMove();
            }
        }
    }
}
