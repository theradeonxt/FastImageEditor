using System;
using System.IO;
using System.Windows.Forms;
using VectorImageEdit.Forms;
using VectorImageEdit.Forms.AppWindow;
using VectorImageEdit.Models;

namespace VectorImageEdit.Controllers
{
    class ExternalEventsController
    {
        private AppWindow _appView;
        private readonly ExternalEventsModel _model;

        ExternalEventsController(AppWindow appView, ExternalEventsModel model)
        {
            _appView = appView;
            _model = model;

            _appView.AddSaveVectorListener(new SaveVectorListener(this));
            _appView.AddOpenVectorListener(new OpenVectorListener(this));
            _appView.AddExportImageListener(new ExportFileListener(this));
            _appView.AddOpenFileListener(new OpenFileListener(this));
        }

        private class SaveVectorListener : AbstractListener<ExternalEventsController>, IActionListener
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

        private class OpenVectorListener : AbstractListener<ExternalEventsController>, IActionListener
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
                    // ignored for now
                }
            }
        }

        private class ExportFileListener : AbstractListener<ExternalEventsController>, IActionListener
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

                // TODO: Move validation to export and return a boolean flag
                if (Controller._model.ValidateExportFile(dialog.FileName))
                {
                    Controller._model.ExportToFile(dialog.FileName);
                }
                else
                {
                    MessageBox.Show(@"The selected file type is not a supported image format.");
                }
            }
        }

        private class OpenFileListener : AbstractListener<ExternalEventsController>, IActionListener
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
            }
        }
    }
}
