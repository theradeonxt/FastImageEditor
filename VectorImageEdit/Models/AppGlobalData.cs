using System;
using System.Drawing;
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
            ShapeEdgeColor = Color.OrangeRed;
            ShapeFillColor = Color.LightBlue;
            ShapeEdgeSize = 4.0f;
        }

        #region Singleton implementation details
        private static readonly Lazy<AppGlobalData> ThisInstance =
            new Lazy<AppGlobalData>(() => new AppGlobalData());

        public static AppGlobalData Instance { get { return ThisInstance.Value; } }

        #endregion

        #region Global data

        // Note: These should be initialized once at app startup.
        public LayerManager LayerManager { get; set; }
        public Layout Layout { get; set; }

        #endregion

        #region Global settings

        public Color ShapeEdgeColor { get; set; }
        public Color ShapeFillColor { get; set; }
        public float ShapeEdgeSize { get; set; }

        #endregion
    }
}
