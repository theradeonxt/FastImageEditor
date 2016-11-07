using System.Windows.Forms;

namespace ImageInterpolation
{
    public partial class GuiView : Form
    {
        public GuiView()
        {
            InitializeComponent();
        }

        // Trick to enable flicker-free images and fast resize
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;    // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }
    }
}
