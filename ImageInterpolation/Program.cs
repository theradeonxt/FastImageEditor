using System;
using System.Windows.Forms;
using ImageInterpolation.ModuleFilter;
using ImageInterpolation.ModuleImageBlending;

namespace ImageInterpolation
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

            GuiView appWindow = new GuiView();
            FilterController moduleFilter = new FilterController(appWindow);
            BlendingController moduleImageBlending = new BlendingController(appWindow);

            Application.Run(appWindow);
        }
    }
}
