using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using NLog;
using VectorImageEdit.Models;
using VectorImageEdit.Modules.ImportExports;
using VectorImageEdit.Modules.Utility;
using VectorImageEdit.Views.Main;
using VectorImageEdit.WindowsFormsBridge;

namespace VectorImageEdit.Controllers
{
    /// <summary>
    /// Handles events realating to external data
    /// </summary>
    class ExternalEventsController
    {
        private readonly AppWindow view;
        private readonly ExternalEventsModel model;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ExternalEventsController(AppWindow view, ExternalEventsModel model)
        {
            this.view = view;
            this.model = model;

            this.view.AddSaveVectorListener(new SaveVectorListener(this));
            this.view.AddOpenVectorListener(new OpenVectorListener(this));
            this.view.AddExportFileListener(new ExportScenePreviewListener(this));
            this.view.AddOpenFileListener(new OpenImagesListener(this));
            this.view.AddDragDropFileListener(new DragDropFileListener(this));
            this.view.AddDragEnterListener(new DragEnterListener(this));
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

                string result = factory.CreateDialog("Save Layer Data",
                    string.Format("Vector data|*{0}", AppModel.Instance.VectorFileExtension));

                bool status = Controller.model.TryExportVector(new VectorExporter(result));
                if (!status)
                {
                    MessageBoxFactory.Create("Error",
                        "Unable to save the vector data file." + Environment.NewLine +
                        "(You may find out what happened from the Error Log.)",
                        MessageBoxType.Error);
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

                string result = factory.CreateDialog(@"Open vector data",
                    string.Format("Vector data|*{0}", AppModel.Instance.VectorFileExtension));
                
                try
                {
                    Controller.model.OpenVectorDeserialize(result);
                }
                catch (Exception ex)
                {
                    MessageBoxFactory.Create("Error",
                        "Unable to open the vector data file." + Environment.NewLine +
                        "(You may find out what happened from the Error Log.)",
                        MessageBoxType.Error);

                    Logger.Error(ex.ToString());
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

                string result = factory.CreateDialog(@"Export Preview",
                     ImagingHelpers.GetSupportedImagesFilter(),
                     2);

                bool status = Controller.model.TryExportScenePreview(new ImageExporter(result));
                if (!status)
                {
                    MessageBoxFactory.Create("Error",
                        "Unable to save the Scene Preview." + Environment.NewLine +
                        "(You may find out what happened from the Error Log.)",
                        MessageBoxType.Error);
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

                string[] result = factory.CreateDialog(@"Open Image(s)",
                    ImagingHelpers.GetSupportedImagesFilter());

                try
                {
                    Controller.CreateBackgroundFileTask(result);
                }
                catch (Exception ex)
                {
                    MessageBoxFactory.Create("Error",
                        "Unable to open image(s)." + Environment.NewLine +
                        "(You may find out what happened from the Error Log.)",
                        MessageBoxType.Error);

                    Logger.Error(ex.ToString());
                }
            }
        }
        private class DragDropFileListener : AbstractDragListener<ExternalEventsController>, IDragListener
        {
            public DragDropFileListener(ExternalEventsController controller)
                : base(controller)
            {
                Handler = ActionPerformed;
            }

            public void ActionPerformed(object sender, MyDragEventArgs e)
            {
                if (!e.IsDataAvailable(MyDragEventArgs.FileDrop)) return;
                string[] fileNames = (string[])e.GetData();
                try
                {
                    Controller.CreateBackgroundFileTask(fileNames);
                }
                catch (InvalidOperationException) { }
            }
        }
        private class DragEnterListener : AbstractDragListener<ExternalEventsController>, IDragListener
        {
            public DragEnterListener(ExternalEventsController controller)
                : base(controller)
            {
                Handler = ActionPerformed;
            }

            public void ActionPerformed(object sender, MyDragEventArgs e)
            {
                e.Effect = e.IsDataAvailable(MyDragEventArgs.FileDrop) 
                    ? MyDragEventArgs.MyDragDropEffects.Copy : MyDragEventArgs.MyDragDropEffects.None;
            }
        }

        // TODO: Verify that it doesn't crash even with invalid files
        private void CreateBackgroundFileTask(string[] fileNames)
        {
            using (var bw = new BackgroundWorker {WorkerReportsProgress = true})
            {
                bw.DoWork += (obj, workEvent) =>
                {
                    workEvent.Result = model.LoadImageFiles((string[]) workEvent.Argument, bw.ReportProgress);
                };
                bw.ProgressChanged += (obj, progressEvent) =>
                {
                    int percentage = progressEvent.ProgressPercentage;
                    view.StatusProgressbarPercentage = percentage;
                    view.StatusLabelText = @"Loading " + percentage + @"/" + fileNames.Length + @" images...";
                };
                bw.RunWorkerCompleted += (obj, completeEvent) =>
                {
                    model.LoadImageLayers((List<Bitmap>) completeEvent.Result);
                    view.StatusProgressbarPercentage = 0;
                    view.StatusLabelText = @"No action";
                };
                bw.RunWorkerAsync(fileNames);
            }
        }
    }
}
