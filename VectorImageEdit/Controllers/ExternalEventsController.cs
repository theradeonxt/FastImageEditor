using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using VectorImageEdit.Forms;
using VectorImageEdit.Forms.AppWindow;
using VectorImageEdit.Interfaces;
using VectorImageEdit.Models;
using VectorImageEdit.Modules.Factories;
using VectorImageEdit.Modules.ImportExports;
using VectorImageEdit.Modules.Utility;

namespace VectorImageEdit.Controllers
{
    class ExternalEventsController
    {
        private readonly AppWindow _appView;
        private readonly ExternalEventsModel _model;

        public ExternalEventsController(AppWindow appView, ExternalEventsModel model)
        {
            _appView = appView;
            _model = model;

            _appView.AddSaveVectorListener(new SaveVectorListener(this));
            _appView.AddOpenVectorListener(new OpenVectorListener(this));
            _appView.AddExportFileListener(new ExportScenePreviewListener(this));
            _appView.AddOpenFileListener(new OpenImagesListener(this));
            _appView.AddDragDropFileListener(new DragDropFileListener(this));
            _appView.AddDragEnterListener(new DragEnterListener(this));
        }

        private class SaveVectorListener : AbstractListener<ExternalEventsController>, IListener
        {
            public SaveVectorListener(ExternalEventsController controller)
                : base(controller)
            {
            }

            public void ActionPerformed(object sender, EventArgs e)
            {
                IFormDialogFactory<string> factory = new SaveFileDialogFactory();

                Tuple<string> result = factory.CreateDialog("Save Layer Data",
                    string.Format("Vector data|*{0}", AppGlobalData.Instance.VectorFileExtension));

                bool status = Controller._model.TryExportVector(new VectorExporter(result.Item1));
                if (!status)
                {
                    MessageBoxFactory.Create("Error",
                        "Unable to save the vector data file." + Environment.NewLine +
                        "(You may find out what happened from the Error Log.)",
                        MessageType.Error);
                }
            }
        }

        private class OpenVectorListener : AbstractListener<ExternalEventsController>, IListener
        {
            public OpenVectorListener(ExternalEventsController controller)
                : base(controller)
            {
            }

            public void ActionPerformed(object sender, EventArgs e)
            {
                IFormDialogFactory<string> factory = new OpenFileDialogFactory();

                Tuple<string> result = factory.CreateDialog(@"Open vector data",
                    string.Format("Vector data|*{0}", AppGlobalData.Instance.VectorFileExtension));
                
                try
                {
                    Controller._model.OpenVectorDeserialize(result.Item1);
                }
                catch (InvalidDataException ex)
                {
                    MessageBoxFactory.Create(caption: "Warning",
                        text: string.Format(@"Unable to open the vector data file. ({0})", ex.Message),
                        type: MessageType.Warning);
                }
                catch (Exception ex)
                {
                    MessageBoxFactory.Create(caption: "Error",
                        text: string.Format(@"Unable to open the vector data file. ({0})", ex.Message),
                        type: MessageType.Error);
                }
            }
        }

        private class ExportScenePreviewListener : AbstractListener<ExternalEventsController>, IListener
        {
            public ExportScenePreviewListener(ExternalEventsController controller)
                : base(controller)
            {
            }

            public void ActionPerformed(object sender, EventArgs e)
            {
                SaveFileDialogFactory factory = new SaveFileDialogFactory();

                Tuple<string> result = factory.CreateDialog(@"Export Preview",
                     ImagingHelpers.GetSupportedImagesFilter(),
                     2);

                bool status = Controller._model.TryExportScenePreview(new ImageExporter(result.Item1));
                if (!status)
                {
                    MessageBoxFactory.Create("Error",
                        "Unable to save the Scene Preview." + Environment.NewLine +
                        "(You may find out what happened from the Error Log.)",
                        MessageType.Error);
                }
            }
        }

        private class OpenImagesListener : AbstractListener<ExternalEventsController>, IListener
        {
            public OpenImagesListener(ExternalEventsController controller)
                : base(controller)
            {
            }

            public void ActionPerformed(object sender, EventArgs e)
            {
                IFormDialogFactory<string[]> factory = new OpenMultipleFilesDialogFactory();

                Tuple<string[]> result = factory.CreateDialog(@"Open Image(s)",
                    ImagingHelpers.GetSupportedImagesFilter());
                
                try
                {
                    Controller.CreateBackgroundFileTask(result.Item1);
                }
                catch (InvalidOperationException) { }
            }
        }

        private class DragDropFileListener : AbstractListener<ExternalEventsController>, IDragListener
        {
            public DragDropFileListener(ExternalEventsController controller)
                : base(controller)
            {
            }

            public void ActionPerformed(object sender, DragEventArgs e)
            {
                if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
                string[] fileNames = (string[])e.Data.GetData(DataFormats.FileDrop);
                try
                {
                    Controller.CreateBackgroundFileTask(fileNames);
                }
                catch (InvalidOperationException) { }
            }
        }

        private class DragEnterListener : AbstractListener<ExternalEventsController>, IDragListener
        {
            public DragEnterListener(ExternalEventsController controller)
                : base(controller)
            {
            }

            public void ActionPerformed(object sender, DragEventArgs e)
            {
                e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
            }
        }

        // TODO: Verify that it doesn't crash even with invalid files
        // TODO: Move to Model
        private void CreateBackgroundFileTask(string[] fileNames)
        {
            var bw = new BackgroundWorker { WorkerReportsProgress = true };
            bw.DoWork += (obj, workEvent) =>
            {
                workEvent.Result = _model.LoadImageFiles((string[])workEvent.Argument, bw.ReportProgress);
            };
            bw.ProgressChanged += (obj, progressEvent) =>
            {
                int percentage = progressEvent.ProgressPercentage;
                _appView.StatusProgressbarPercentage = percentage;
                _appView.StatusLabelText = @"Loading " + percentage + @"/" + fileNames.Length + @" images...";
            };
            bw.RunWorkerCompleted += (obj, completeEvent) =>
            {
                _model.LoadImageLayers((List<Bitmap>)completeEvent.Result);
                _appView.StatusProgressbarPercentage = 0;
                _appView.StatusLabelText = @"No action";
            };
            bw.RunWorkerAsync(fileNames);
        }
    }
}
