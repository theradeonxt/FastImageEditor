using System;
using VectorImageEdit.Forms;
using VectorImageEdit.Forms.AppWindow;
using VectorImageEdit.Models;

namespace VectorImageEdit.Controllers
{
    class ExternalEventsController
    {
        private AppWindow _appView;
        private ExternalEventsModel _model;

        ExternalEventsController(AppWindow appView, ExternalEventsModel model)
        {
            _appView = appView;
            _model = model;

            _appView.AddSaveVectorListener(new SaveVectorListener());
        }

        private class SaveVectorListener: IActionListener
        {
            SaveVectorListener()
            public void ActionPerformed(object sender, EventArgs e)
            {
                
            }
        }
    }
}
