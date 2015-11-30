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
        private readonly AppWindow _appView;
        private readonly WorkspaceModel _model;

        public WorkspaceController(AppWindow appView, WorkspaceModel model)
        {
            _appView = appView;
            _model = model;

            _appView.AddWorkspaceMouseDownListener(new WorkspaceMouseDownListener(this));
            _appView.AddWorkspaceMouseMoveListener(new WorkspaceMouseMoveListener(this));
            _appView.AddWorkspaceMouseUpListener(new WorkspaceMouseUpListener(this));

            // TODO: These don't belong here
            _appView.MemoryProgressbarPercentage = BackgroundStatitics.MemoryUsagePercent;
            _appView.MemoryLabelText = string.Format("Memory Used ({0}%)", BackgroundStatitics.MemoryUsagePercent);
        }

        public void GraphicsUpdateCallback(GraphicsProfiler profiler)
        {
            // TODO: Ensure this is called on the GUI thread
            _appView.GraphicsDebugText = string.Format("ClearFrame: {0}ms{1}RasterizeObjects: {2}ms{1}DrawFrame: {3}ms{1}",
                profiler.ClearFrameDuration, Environment.NewLine, profiler.RasterizeObjectsDuration, profiler.DrawFrameDuration);
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
                Controller._model.MouseDown(e);

                switch (Controller._model.SelectedLayerState)
                {
                    case WorkspaceModel.LayerState.Resizing:
                        Controller._appView.SetLayerResizeCursor();
                        break;
                    case WorkspaceModel.LayerState.Moving:
                        Controller._appView.SetLayerMoveCursor();
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
                Controller._model.MouseMovement(e);
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
                Controller._appView.SetDefaultCursor();
                Controller._model.MouseUp(e);
            }
        }
    }
}
