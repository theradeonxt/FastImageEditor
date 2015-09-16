using System.Windows.Forms;

namespace VectorImageEdit.Modules.Factories
{
    internal enum MessageType
    {
        None,
        Error,
        Warning,
        Information
    };

    static class MessageBoxFactory
    {
        public static void Create(string caption, string text, MessageType type = MessageType.None)
        {
            MessageBoxIcon icon;
            switch (type)
            {
                case MessageType.Error: 
                    icon = MessageBoxIcon.Error;
                    break;
                case MessageType.Warning:
                    icon = MessageBoxIcon.Information;
                    break;
                case MessageType.Information:
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
