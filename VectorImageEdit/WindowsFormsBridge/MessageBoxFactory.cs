using System.Windows.Forms;

namespace VectorImageEdit.WindowsFormsBridge
{
    internal enum MessageBoxType
    {
        None,
        Error,
        Warning,
        Information
    };

    static class MessageBoxFactory
    {
        public static void Create(string caption, string text, MessageBoxType boxType = MessageBoxType.None)
        {
            MessageBoxIcon icon;
            switch (boxType)
            {
                case MessageBoxType.Error: 
                    icon = MessageBoxIcon.Error;
                    break;
                case MessageBoxType.Warning:
                    icon = MessageBoxIcon.Information;
                    break;
                case MessageBoxType.Information:
                    icon = MessageBoxIcon.Warning;
                    break;
                default:
                    icon = MessageBoxIcon.None;
                    break;
            }
            MessageBox.Show(text, caption, MessageBoxButtons.OK, icon);
        }
    }
}
