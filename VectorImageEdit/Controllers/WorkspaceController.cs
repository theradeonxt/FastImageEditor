﻿using System;
using VectorImageEdit.Forms;
using VectorImageEdit.Forms.AppWindow;
using VectorImageEdit.Interfaces;
using VectorImageEdit.Models;
using VectorImageEdit.Modules;

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

            _appView.AddWorkspaceSizeChangedListener(new WorkspaceSizeChangedListener(this));
            _appView.AddWindowMovedListener(new AppWindowMovedListener(this));

            _appView.MemoryProgressbarPercentage = BackgroundStatitics.MemoryUsagePercent;
            _appView.MemoryLabelText = string.Format("Memory Used ({0}%)", BackgroundStatitics.MemoryUsagePercent.ToString());
        }

        private class WorkspaceSizeChangedListener : AbstractListener<WorkspaceController>, IListener
        {
            public WorkspaceSizeChangedListener(WorkspaceController controller)
                : base(controller)
            {
            }

            public void ActionPerformed(object sender, EventArgs e)
            {
                Controller._model.WorkspaceResize();
            }
        }

        private class AppWindowMovedListener : AbstractListener<WorkspaceController>, IListener
        {
            public AppWindowMovedListener(WorkspaceController controller)
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
