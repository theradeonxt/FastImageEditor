using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace ImageInterpolation
{
    public partial class ImagePropertiesUi : UserControl
    {
        public ImagePropertiesUi()
        {
            InitializeComponent();
            SetupUi();
        }

        private void SetupUi()
        {
            dataGridView1.Rows.Add("Resolution [px]", "1");
            dataGridView1.Rows.Add("Pixel Format", "2");
            dataGridView1.Rows.Add("Size [MB]", "3");
        }

        public string ImageTag { set { groupBox2.Text = value; } }
        public Size Resolution
        {
            set { dataGridView1.Rows[0].Cells[1].Value = value.Width + " x " + value.Height; }
        }
        public PixelFormat PixelFormat
        {
            set { dataGridView1.Rows[1].Cells[1].Value = value.ToString(); }
        }
        public float RawSize
        {
            set { dataGridView1.Rows[2].Cells[1].Value = value.ToString("###.##"); }
        }
    }
}
