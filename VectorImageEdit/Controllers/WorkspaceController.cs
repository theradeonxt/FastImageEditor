using System;
using VectorImageEdit.Models;
using VectorImageEdit.Modules;
using VectorImageEdit.Modules.GraphicsCompositing;
using VectorImageEdit.Views.Main;
using VectorImageEdit.WindowsFormsBridge;

namespace VectorImageEdit.Controllers
{
    /// <summary>
    /// Handles user-actions related to the workspace region
    /// </summary>
    class WorkspaceController
    {
        private readonly AppWindow view;
        private readonly WorkspaceModel model;

        public WorkspaceController(AppWindow appView, WorkspaceModel model)
        {
            view = appView;
            this.model = model;

            view.AddWorkspaceMouseDownListener(new WorkspaceMouseDownListener(this));
            view.AddWorkspaceMouseMoveListener(new WorkspaceMouseMoveListener(this));
            view.AddWorkspaceMouseUpListener(new WorkspaceMouseUpListener(this));

            // TODO: These don't belong here
            view.MemoryProgressbarPercentage = BackgroundStatitics.MemoryUsagePercent;
            view.MemoryLabelText = string.Format("Memory Used ({0}%)", BackgroundStatitics.MemoryUsagePercent);
        }

        public void GraphicsUpdateCallback(GraphicsProfiler profiler)
        {
            // TODO: Ensure this is called on the GUI thread
            if (view.InvokeRequired)
            {
                MyMethodInvoker.Invoke(() =>
                {
                    view.GraphicsDebugText = string.Format("ClearFrame: {0}ms{1}RasterizeObjects: {2}ms{1}DrawFrame: {3}ms{1}",
                        profiler.ClearFrameDuration, Environment.NewLine, profiler.RasterizeObjectsDuration, profiler.DrawFrameDuration);
                });
            }
            else
            {
                view.GraphicsDebugText = string.Format("ClearFrame: {0}ms{1}RasterizeObjects: {2}ms{1}DrawFrame: {3}ms{1}",
                        profiler.ClearFrameDuration, Environment.NewLine, profiler.RasterizeObjectsDuration, profiler.DrawFrameDuration);
            }
        }

        private class WorkspaceMouseDownListener : AbstractMouseListener<WorkspaceController>, IMouseListener
        {
            public WorkspaceMouseDownListener(WorkspaceController controller)
                : base(controller)
            {
                Handler = ActionPerformed;
            }

            public void ActionPerformed(object sender, MyMouseEventArgs e)
            {
                Controller.model.MouseDown(e);

                switch (Controller.model.StateHandler.SelectedLayerState)
                {
                    case LayerState.Resizing:
                        Controller.view.SetLayerResizeCursor();
                        break;
                    case LayerState.Moving:
                        Controller.view.SetLayerMoveCursor();
                        break;
                }
            }
        }

        private class WorkspaceMouseMoveListener : AbstractMouseListener<WorkspaceController>, IMouseListener
        {
            public WorkspaceMouseMoveListener(WorkspaceController controller)
                : base(controller)
            {
                Handler = ActionPerformed;
            }

            public void ActionPerformed(object sender, MyMouseEventArgs e)
            {
                LayerState state = Controller.model.MouseMovement(e);
                if (state == LayerState.Selectable)
                {
                    Controller.view.SetLayerMoveCursor();
                }
                else
                {
                    Controller.view.SetDefaultCursor();
                }
            }
        }

        private class WorkspaceMouseUpListener : AbstractMouseListener<WorkspaceController>, IMouseListener
        {
            public WorkspaceMouseUpListener(WorkspaceController controller)
                : base(controller)
            {
                Handler = ActionPerformed;
            }

            public void ActionPerformed(object sender, MyMouseEventArgs e)
            {
                Controller.view.SetDefaultCursor();
                Controller.model.MouseUp(e);
            }
        }
    }
}
