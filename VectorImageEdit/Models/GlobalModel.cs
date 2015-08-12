using System;
using VectorImageEdit.Modules;
using VectorImageEdit.Modules.Layers;

namespace VectorImageEdit.Models
{
    // TODO: Rename to AppModel?

    /// <summary>
    /// GlobalModel Module
    /// 
    /// - provides access to global data
    /// 
    /// Uses Sigleton pattern and lazy initialization as described here; is Thread-safe.
    /// http://geekswithblogs.net/BlackRabbitCoder/archive/2010/05/19/c-system.lazylttgt-and-the-singleton-design-pattern.aspx
    /// </summary>
    class GlobalModel
    {
        //********************************************************
        #region Singleton details
        // ReSharper disable once InconsistentNaming
        private static readonly Lazy<GlobalModel> _instance =
            new Lazy<GlobalModel>(() => new GlobalModel());

        private GlobalModel() { }

        public static GlobalModel Instance { get { return _instance.Value; } }

        #endregion
        //********************************************************

        //********************************************************
        #region Actual global data goes here
        // Note: These should be initialized once at app startup.

        public LayerManager LayerManager { get; set; }
        public Layout Layout { get; set; }

        #endregion
        //********************************************************
    }
}
