using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using VectorImageEdit.Modules;
using VectorImageEdit.Modules.Layers;

namespace VectorImageEdit.Models
{
    /// <summary>
    /// AppGlobalData Module
    /// 
    /// - provides access to unique application data and settings
    /// 
    /// Uses Sigleton pattern and lazy initialization as described here; is Thread-safe.
    /// http://geekswithblogs.net/BlackRabbitCoder/archive/2010/05/19/c-system.lazylttgt-and-the-singleton-design-pattern.aspx
    /// </summary>
    class AppGlobalData
    {
        private AppGlobalData()
        {
            PrimaryColor = Color.OrangeRed;
            SecondaryColor = Color.LightBlue;
            ShapeEdgeSize = 4.0f;

            LayerSelectionPen = new Pen(Brushes.Black, 3) { DashStyle = DashStyle.Dash };

            VectorFileExtension = ".vdata";
        }

        #region Singleton implementation details

        private static readonly Lazy<AppGlobalData> ThisInstance =
            new Lazy<AppGlobalData>(() => new AppGlobalData());

        public static AppGlobalData Instance { get { return ThisInstance.Value; } }

        #endregion

        #region Global data

        // Note: These are initialized by the AppController that has
        //       enough information to create these with the right parameters 
        public LayerManager LayerManager { get; set; }
        public Layout Layout { get; set; }

        #endregion

        #region Global settings

        public Color PrimaryColor { get; set; }
        public Color SecondaryColor { get; set; }
        public float ShapeEdgeSize { get; set; }

        public Pen LayerSelectionPen { get; private set; }

        public string VectorFileExtension { get; private set; }

        #endregion
    }
}
