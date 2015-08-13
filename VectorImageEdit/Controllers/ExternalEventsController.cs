using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using VectorImageEdit.Forms;
using VectorImageEdit.Forms.AppWindow;
using VectorImageEdit.Models;

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
            _appView.AddOpenFileListener(new OpenFileListener(this));
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
                SaveFileDialog dialog = new SaveFileDialog
                {
                    Title = @"Save vector data",
                    Filter = string.Format("Vector data|*{0}", ExternalEventsModel.SupportedVectorExt)
                };
                if (dialog.ShowDialog() != DialogResult.OK) return;

                Controller._model.SaveVectorSerialize(dialog.FileName);
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
                OpenFileDialog dialog = new OpenFileDialog
                {
                    Title = @"Open vector data",
                    Filter = string.Format("Vector data|*{0}", ExternalEventsModel.SupportedVectorExt)
                };
                if (dialog.ShowDialog() != DialogResult.OK) return;

                try
                {
                    Controller._model.OpenVectorDeserialize(dialog.FileName);
                }
                catch (InvalidDataException ex)
                {
                    MessageBox.Show(string.Format("Unable to open the vector data file. ({0})", ex.Message));
                }
                catch (Exception)
                {
                    // TODO: ignored for now; is not recoverable - should be logged somewhere
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
                // Show the user a dialog to select the file format and name
                SaveFileDialog dialog = new SaveFileDialog
                {
                    Title = @"Export Preview",
                    Filter = Controller._model.GetExportsFilter(),
                    FilterIndex = 2  // set the default file type (2 is JPEG)
                };
                if (dialog.ShowDialog() != DialogResult.OK) return;

                try
                {
                    // BUG: always shows the message and no save
                    if (!Controller._model.ExportToFile(dialog.FileName))
                    {
                        MessageBox.Show(@"The selected file type is not a supported image format.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(@"Could not save the workspace image. Detailed Info: " + ex.Message);
                }
            }
        }

        private class OpenFileListener : AbstractListener<ExternalEventsController>, IListener
        {
            public OpenFileListener(ExternalEventsController controller)
                : base(controller)
            {
            }

            public void ActionPerformed(object sender, EventArgs e)
            {
                OpenFileDialog fileDialog = new OpenFileDialog
                {
                    Multiselect = true,
                    Title = @"Open Image(s)",
                    Filter = @"Image Files|*.jpg;*.png;*.tiff;*.bmp"
                };
                if (fileDialog.ShowDialog() != DialogResult.OK) return;
                string[] fileNames = fileDialog.FileNames;

                // TODO: Validate image files OR implement generic handling (able to drag & drop any supported file format!)
                Controller.CreateBackgroundFileTask(fileNames);
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

                // TODO: Validate image files OR implement generic handling (able to drag & drop any supported file format!)
                Controller.CreateBackgroundFileTask(fileNames);
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
