using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VectorImageEdit.Forms.AppWindow;
using VectorImageEdit.Modules.ExportFormats;
using VectorImageEdit.Modules.Layers;

namespace VectorImageEdit.Models
{
    class ExternalEventsModel
    {
        private AppWindow _appView;
        public ExternalEventsModel(AppWindow appView)
        {
            _appView = appView;
        }

        public void SerializeLayers(string fileName, LayerManager manager)
        {
            VectorSerializer serializer = new VectorSerializer
            {
                Source = manager.GetLayersList()
            };
            serializer.Serialize(fileName);
        }
    }
}
