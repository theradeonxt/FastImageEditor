using System;
using System.Drawing;
using System.Windows.Forms;
using NLog;

namespace VectorImageEdit.WindowsFormsBridge
{
    /// <summary>
    // Common error handling and logging for every concrete dialog factory
    /// </summary>
    internal abstract class DialogFactoryInternal
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected void InternalErrorValidation(Action concreteAction)
        {
            try
            {
                concreteAction();
            }
            catch (InvalidCastException ex)
            {
                Logger.Error("The FileDialog received invalid parameters. {0}", ex.ToString());
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Logger.Error("The FileDialog received invalid parameters. {0}", ex.ToString());
            }
            catch (IndexOutOfRangeException ex)
            {
                Logger.Error("The FileDialog received invalid parameters. {0}", ex.ToString());
            }
            catch (Exception ex)
            {
                Logger.Error("Could not create the FileDialog. {0}", ex.ToString());
            }
        }
    }

    class SaveFileDialogFactory : DialogFactoryInternal, IFormDialogFactory<string>
    {
        public string CreateDialog(params object[] dialogParameters)
        {
            string result = string.Empty;
            InternalErrorValidation(() =>
            {
                SaveFileDialog dialog = new SaveFileDialog();

                if (dialogParameters.Length > 0) dialog.Title = (string)dialogParameters[0];
                if (dialogParameters.Length > 1) dialog.Filter = (string)dialogParameters[1];
                if (dialogParameters.Length > 2) dialog.FilterIndex = (int)dialogParameters[2];
                
                if (dialog.ShowDialog() == DialogResult.OK) result = dialog.FileName;
            });
            return result;
        }
    }
    class OpenFileDialogFactory : DialogFactoryInternal, IFormDialogFactory<string>
    {
        public string CreateDialog(params object[] dialogParameters)
        {
            string result = string.Empty;
            InternalErrorValidation(() =>
            {
                OpenFileDialog dialog = new OpenFileDialog();

                if (dialogParameters.Length > 0) dialog.Title = (string)dialogParameters[0];
                if (dialogParameters.Length > 1) dialog.Filter = (string)dialogParameters[1];
                if (dialogParameters.Length > 2) dialog.FilterIndex = (int)dialogParameters[2];

                if (dialog.ShowDialog() == DialogResult.OK) result = dialog.FileName;
            });
            return result;
        }
    }
    class OpenMultipleFilesDialogFactory : DialogFactoryInternal, IFormDialogFactory<string[]>
    {
        public string[] CreateDialog(params object[] dialogParameters)
        {
            string[] result = { string.Empty };
            InternalErrorValidation(() =>
            {
                OpenFileDialog dialog = new OpenFileDialog();

                if (dialogParameters.Length > 0) dialog.Title = (string)dialogParameters[0];
                if (dialogParameters.Length > 1) dialog.Filter = (string)dialogParameters[1];
                if (dialogParameters.Length > 2) dialog.FilterIndex = (int)dialogParameters[2];
                if (dialogParameters.Length > 3) dialog.Multiselect = (bool)dialogParameters[3];

                if (dialog.ShowDialog() == DialogResult.OK) result = dialog.FileNames;
            });
            return result;
        }
    }
    class ColorDialogFactory : DialogFactoryInternal, IFormDialogFactory<Color>
    {
        public Color CreateDialog(params object[] dialogParameters)
        {
            Color result = Color.Black;
            InternalErrorValidation(() =>
            {
                ColorDialog dialog = new ColorDialog
                {
                    FullOpen = (bool)dialogParameters[0]
                };
                if (dialog.ShowDialog() == DialogResult.OK) result = dialog.Color;
            });
            return result;
        }
    }
}
