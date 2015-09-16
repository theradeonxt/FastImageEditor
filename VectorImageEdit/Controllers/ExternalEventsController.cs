using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using VectorImageEdit.Forms;
using VectorImageEdit.Forms.AppWindow;
using VectorImageEdit.Models;
using VectorImageEdit.Modules.Factories;

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

            // TODO: have a generic file opener/exporter and handle vector and image formats differently?
            _appView.AddSaveVectorListener(new SaveVectorListener(this));
            _appView.AddOpenVectorListener(new OpenVectorListener(this));
            _appView.AddExportFileListener(new ExportFileListener(this));
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
                var factory = new SaveFileDialogFactory();
                factory.CreateDialog(title: @"Save vector data",
                    filter: string.Format("Vector data|*{0}", AppGlobalData.Instance.VectorFileExtension));
                try
                {
                    Controller._model.SaveVectorSerialize(factory.DialogData);
                }
                catch (Exception ex)
                {
                    MessageBoxFactory.Create(caption: "Error",
                        text: string.Format(@"Unable to save the vector data file. ({0})", ex.Message),
                        type: MessageType.Error);
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
                var factory = new OpenFileDialogFactory();
                factory.CreateDialog(title: @"Open vector data",
                    filter: string.Format("Vector data|*{0}", AppGlobalData.Instance.VectorFileExtension));
                try
                {
                    Controller._model.OpenVectorDeserialize(factory.DialogData);
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

        private class ExportFileListener : AbstractListener<ExternalEventsController>, IListener
        {
            public ExportFileListener(ExternalEventsController controller)
                : base(controller)
            {
            }

            public void ActionPerformed(object sender, EventArgs e)
            {
                var factory = new SaveFileDialogFactory();
                factory.CreateDialog(title: @"Export Preview",
                    filter: Controller._model.GetExportsFilter(),
                    filterIndex: 2);
                try
                {
                    // BUG: Always shows the message and won't save
                    if (!Controller._model.ExportToFile(factory.DialogData))
                    {
                        MessageBoxFactory.Create(caption: "Information", 
                            text: @"The selected file type is not a supported image format.", 
                            type: MessageType.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBoxFactory.Create(caption: "Error",
                        text: string.Format(@"Could not save the workspace image. Detailed Info: {0}", ex.Message),
                        type: MessageType.Error);
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
                var factory = new OpenMultipleFilesDialogFactory();
                factory.CreateDialog(title: @"Open Image(s)",
                    filter: @"Image Files|*.jpg;*.png;*.tiff;*.bmp");
                try
                {
                    Controller.CreateBackgroundFileTask(factory.DialogData);
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
