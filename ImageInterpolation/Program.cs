using System;
using System.ComponentModel;
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
            FilterController moduleFilter = new FilterController(appWindow.moduleFilterUi1);
            BlendingController moduleImageBlending = new BlendingController(appWindow.moduleBlendingUi1);

            Application.Run(appWindow);
        }
    }

    public static class ControlHelpers
    {
        public static void InvokeIfRequired<T>(this T control, Action<T> action) where T : ISynchronizeInvoke
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new Action(() => action(control)), null);
            }
            else
            {
                action(control);
            }
        }
    }
}
