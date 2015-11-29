using System;
using System.Windows.Forms;
using VectorImageEdit.Controllers;
using VectorImageEdit.Views.Main;

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

            var appView = new AppWindow();

            // ReSharper disable once UnusedVariable
            var appController = new AppController(appView);

            Application.Run(appView);
        }
    }
}
