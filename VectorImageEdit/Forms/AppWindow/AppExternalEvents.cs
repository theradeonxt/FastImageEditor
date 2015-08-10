using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VectorImageEdit.Modules;
using VectorImageEdit.Modules.ExportFormats;
using VectorImageEdit.Modules.Layers;

namespace VectorImageEdit.Forms.AppWindow
{
    public partial class AppWindow
    {
        private void openFileMenu_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.Multiselect = true;
            fileDialog.Title = @"Open Image(s)";
            fileDialog.Filter = @"Image Files|*.jpg;*.png;*.tiff;*.bmp";

            DialogResult fileDialogResult = fileDialog.ShowDialog();
            if (fileDialogResult == DialogResult.OK)
            {
                ProgressWindow progress = new ProgressWindow("Loading " + fileDialog.FileNames.Length + " images...", 1, fileDialog.FileNames.Length, 1);
                progress.Show();
                Thread.Sleep(100);

                // TODO: Add BackgroundWorker or a custom implementation with progressbar
                foreach (string fileName in fileDialog.FileNames)
                {
                    Bitmap image = ImageLoader.ScaledSize(fileName, _layoutManager.MaximumSize());
                    Rectangle region = _layoutManager.NewLayerMetrics(image.Size);
                    Layer newPicture = new Picture(image, region, 0);
                    _layerManager.BringToFront(newPicture);
                    _layerManager.Add(newPicture);

                    progress.IncrementProgress();
                    Thread.Sleep(50);
                }

                progress.Dispose();
                Thread.Sleep(100);
            }
        }

        #region DragNDrop

        private void panWorkRegion_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Get a listing of selected files
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                statusProgressBar.Value = 0;
                statusProgressBar.Maximum = files.Length;
                statusActionLabel.Text = @"Loading " + files.Length + @" images...";

                BackgroundWorker bw = new BackgroundWorker { WorkerReportsProgress = true };
                bw.DoWork += (obj, workEvent) =>
                {
                    List<Bitmap> images = new List<Bitmap>();
                    int progress = 0;
                    Parallel.ForEach((string[])workEvent.Argument, fileName =>
                    {
                        Bitmap image = ImageLoader.ScaledSize(fileName, _layoutManager.MaximumSize());
                        images.Add(image);
                        ((BackgroundWorker)obj).ReportProgress(Interlocked.Increment(ref progress));
                    });
                    workEvent.Result = images;
                };
                bw.ProgressChanged += (obj, progressEvent) =>
                {
                    int percentage = progressEvent.ProgressPercentage;
                    statusProgressBar.Value = percentage;
                    statusActionLabel.Text = @"Loading " + percentage + @"/" + files.Length + @" images...";
                };
                bw.RunWorkerCompleted += (obj, completeEvent) =>
                {
                    List<Layer> layers = new List<Layer>();
                    foreach (Bitmap image in (List<Bitmap>)completeEvent.Result)
                    {
                        using (BitmapHelper helper = new BitmapHelper(image))
                        {
                            BackgroundStatitics.CommitImageMemory(helper.SizeBytes);
                            //float memUsage = BackgroundStatitics.ImageMemory * 100.0f / _totalRam;
                            //memoryProgressBar.Value = (int)memUsage;
                        }

                        Rectangle region = _layoutManager.NewLayerMetrics(image.Size);
                        layers.Add(new Picture(image, region, 0));
                    }

                    _layerManager.Add(layers);

                    statusProgressBar.Value = 0;
                    statusActionLabel.Text = @"No action";
                };

                bw.RunWorkerAsync(files);
            }
        }

        private void panWorkRegion_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        #endregion

        /// <summary>
        /// Saves an image containing the current workspace graphical content to a format chosen by the user.
        /// Uses the method to get all available image formats described here: http://stackoverflow.com/a/9176575
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportFileMenu_Click(object sender, EventArgs e)
        {
            // show the user a dialog to select the file format and name
            SaveFileDialog dialog = new SaveFileDialog
            {
                Title = @"Export Preview",
                Filter = ""
            };

            string sep = string.Empty;
            var extCodec = new Dictionary<string, ImageFormat>();

            foreach (var imgCodec in ImageCodecInfo.GetImageEncoders())
            {
                // format the file types in the file dialog and obtain their extensions
                int len = imgCodec.FilenameExtension.Replace("*.", "").Length;
                string ext = imgCodec.FilenameExtension.Substring(1, (len == 3) ? 4 : 5).Replace(";", "");
                string name = imgCodec.CodecName.Substring(8).Replace("Codec", "Files").Trim();
                dialog.Filter = string.Format("{0}{1}{2} ({3})|{3}", dialog.Filter, sep, name, imgCodec.FilenameExtension);
                sep = "|";

                // map the file types to an image format
                extCodec.Add(ext, new ImageFormat(imgCodec.FormatID));
            }

            // set the default file type (2 is JPEG)
            dialog.FilterIndex = 2;

            if (dialog.ShowDialog() != DialogResult.OK) return;

            string fileExtension = Path.GetExtension(dialog.FileName);
            ImageFormat ifmt;

            // validate selected file type
            if (fileExtension != null && extCodec.TryGetValue(fileExtension, out ifmt))
            {
                // TODO: Add BackgroundWorker or a custom implementation with progressbar

                // get the workspace data as an image
                Bitmap preview = _layerManager.GetImagePreview();

                // save the obtained image to the desired image format
                ImageOutput.SaveImage(preview, dialog.FileName, extCodec[fileExtension]);
            }
            else
            {
                MessageBox.Show(@"The selected file type is not a valid image format.");
            }
        }

        private void openVectorMenu_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = @"Open vector data",
                Filter = string.Format("Vector data|*{0}", VectorSerializer.SupportedFileExt)
            };
            if (dialog.ShowDialog() != DialogResult.OK) return;

            VectorSerializer serializer = new VectorSerializer();
            try
            {
                IList layers = serializer.Deserialize(dialog.FileName);
                _layerManager.RemoveAll();
                foreach (Layer layer in layers)
                {
                    _layerManager.Add(layer);
                }
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

        /*
         * Saves all workspace objects to a vector file format
         */
        private void saveVectorMenu_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Title = @"Save vector data",
                Filter = string.Format("Vector data|*{0}", VectorSerializer.SupportedFileExt)
            };
            if (dialog.ShowDialog() != DialogResult.OK) return;

            // TODO: Add BackgroundWorker or a custom implementation with progressbar

            VectorSerializer serializer = new VectorSerializer
            {
                Source = _layerManager.GetLayersList()
            };
            serializer.Serialize(dialog.FileName);
        }

        //***********************************************************
        #region View listeners

        public void AddSaveVectorListener(IActionListener listener)
        {
            saveVectorMenu.Click += listener.ActionPerformed;
        }

        public void AddOpenVectorListener(IActionListener listener)
        {
            openVectorMenu.Click += listener.ActionPerformed;
        }

        public void AddExportImageListener(IActionListener listener)
        {
            exportFileMenu.Click += listener.ActionPerformed;
        }

        public void AddOpenFileListener(IActionListener listener)
        {
            openFileMenu.Click += listener.ActionPerformed;
        }

        public void AddFileDragDropListener(IDragActionListener listener)
        {
            panWorkRegion.DragDrop += listener.ActionPerformed;
        }

        #endregion
        //***********************************************************

        //***********************************************************
        #region View Getters/Setters

        #endregion
        //***********************************************************
    }
}
