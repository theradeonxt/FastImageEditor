using System;
using System.Drawing;
using System.Windows.Forms;
using NLog;
using VectorImageEdit.Interfaces;

namespace VectorImageEdit.Modules.Factories
{
    /// <summary>
    /// Common error handling and logging for every concrete dialog factory
    /// </summary>
    internal abstract class DialogFactoryInternal
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected void InternalErrorValidation(Action concreteFactoryCallback)
        {
            try
            {
                concreteFactoryCallback();
            }
            catch (InvalidCastException ex)
            {
                Logger.Error(string.Format(@"The FileDialog received invalid parameters. {0}", ex.StackTrace));
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Logger.Error(string.Format(@"The FileDialog received invalid parameters. {0}", ex.StackTrace));
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format(@"Could not create the FileDialog. {0}", ex.StackTrace));
            }
        }
    }

    class SaveFileDialogFactory : DialogFactoryInternal, IFormDialogFactory<string>
    {
        public Tuple<string> CreateDialog(params object[] dialogParameters)
        {
            string result = string.Empty;
            InternalErrorValidation(() =>
            {
                SaveFileDialog dialog = new SaveFileDialog
                {
                    Title = (string)dialogParameters[0],
                    Filter = (string)dialogParameters[1],
                    FilterIndex = (int)dialogParameters[2]
                };
                if (dialog.ShowDialog() == DialogResult.OK) result = dialog.FileName;
            });
            return new Tuple<string>(result);
        }
    }

    class OpenFileDialogFactory : DialogFactoryInternal, IFormDialogFactory<string>
    {
        public Tuple<string> CreateDialog(params object[] dialogParameters)
        {
            string result = string.Empty;
            InternalErrorValidation(() =>
            {
                OpenFileDialog dialog = new OpenFileDialog
                {
                    Title = (string)dialogParameters[0],
                    Filter = (string)dialogParameters[1],
                    FilterIndex = (int)dialogParameters[2]
                };
                if (dialog.ShowDialog() == DialogResult.OK) result = dialog.FileName;
            });
            return new Tuple<string>(result);
        }
    }

    class OpenMultipleFilesDialogFactory : DialogFactoryInternal, IFormDialogFactory<string[]>
    {
        public Tuple<string[]> CreateDialog(params object[] dialogParameters)
        {
            string[] result = { "" };
            InternalErrorValidation(() =>
            {
                OpenFileDialog dialog = new OpenFileDialog
                {
                    Title = (string)dialogParameters[0],
                    Filter = (string)dialogParameters[1],
                    FilterIndex = (int)dialogParameters[2],
                    Multiselect = (bool)dialogParameters[3]
                };
                if (dialog.ShowDialog() == DialogResult.OK) result = dialog.FileNames;
            });
            return new Tuple<string[]>(result);
        }
    }

    class ColorDialogFactory : DialogFactoryInternal, IFormDialogFactory<Color>
    {
        public Tuple<Color> CreateDialog(params object[] dialogParameters)
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
            return new Tuple<Color>(result);
        }
    }
}
