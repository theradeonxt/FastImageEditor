using System;
using System.Windows.Forms;
using NLog;

namespace VectorImageEdit.WindowsFormsBridge
{
    /// <summary>
    // Windows Forms dependent method invocation ensuring a 
    /// specified action is executed on the correct thread
    /// </summary>
    static class MyMethodInvoker
    {
        private static Control _parentForm;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void SetInvocationTarget(Control parentForm)
        {
            _parentForm = parentForm;
        }
        public static void Invoke(Action action)
        {
            if (_parentForm == null)
            {
                Logger.Warn("Trying to invoke action without setting a form target.{0}", Environment.StackTrace);
                return;
            }
            MethodInvoker uiDelegateFunction = delegate { action(); };
            _parentForm.Invoke(uiDelegateFunction);
        }
    }
}
