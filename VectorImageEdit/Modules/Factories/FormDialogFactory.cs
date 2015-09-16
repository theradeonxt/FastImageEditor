using System;
using System.Drawing;
using System.Windows.Forms;

namespace VectorImageEdit.Modules.Factories
{
    interface IFormDialogFactory<T>
    {
        T DialogData { get; }
    }

    abstract class AbstractDialogFactory<T> : IFormDialogFactory<T>
    {
        protected AbstractDialogFactory()
        {
            DialogData = default(T);
        }

        public T DialogData { get; protected set; }
    }

    class SaveFileDialogFactory : AbstractDialogFactory<string>
    {
        public void CreateDialog(string title, string filter, int filterIndex = 0)
        {
            try
            {
                SaveFileDialog dialog = new SaveFileDialog
                {
                    Title = title,
                    Filter = filter,
                    FilterIndex = filterIndex
                };
                if (dialog.ShowDialog() == DialogResult.OK) DialogData = dialog.FileName;
            }
            catch (ArgumentException) { }
        }
    }

    class OpenFileDialogFactory : AbstractDialogFactory<string>
    {
        public void CreateDialog(string title, string filter, int filterIndex = 0)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog
                {
                    Title = title,
                    Filter = filter,
                    FilterIndex = filterIndex
                };
                if (dialog.ShowDialog() == DialogResult.OK) DialogData = dialog.FileName;
            }
            catch (ArgumentException) { }
        }
    }

    class OpenMultipleFilesDialogFactory : AbstractDialogFactory<string[]>
    {
        public void CreateDialog(string title, string filter, int filterIndex = 0)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog
                {
                    Title = title,
                    Filter = filter,
                    FilterIndex = filterIndex,
                    Multiselect = true
                };
                if (dialog.ShowDialog() == DialogResult.OK) DialogData = dialog.FileNames;
            }
            catch (ArgumentException) { }
        }
    }

    class ColorDialogFactory : AbstractDialogFactory<Color>
    {
        public void CreateDialog(bool fullOpen = true)
        {
            ColorDialog dialog = new ColorDialog
            {
                FullOpen = fullOpen
            };
            if (dialog.ShowDialog() == DialogResult.OK) DialogData = dialog.Color;
        }
    }
}
