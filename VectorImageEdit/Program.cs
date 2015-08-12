using System;
using System.Windows.Forms;
using VectorImageEdit.Controllers;

namespace VectorImageEdit
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var appWnd = new Forms.AppWindow.AppWindow();
            // ReSharper disable once UnusedVariable
            AppController appCtrl = new AppController(appWnd);

            Application.Run(appWnd);
        }
    }
}
