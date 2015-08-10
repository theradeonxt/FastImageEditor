using System;
using VectorImageEdit.Modules;
using VectorImageEdit.Modules.Layers;

namespace VectorImageEdit.Models
{
    /// <summary>
    /// GlobalModel Module
    /// 
    /// - contains access to globally used things
    /// 
    /// Uses Sigleton pattern and lazy initialization as described here; is Thread-safe.
    /// http://geekswithblogs.net/BlackRabbitCoder/archive/2010/05/19/c-system.lazylttgt-and-the-singleton-design-pattern.aspx
    /// </summary>
    class GlobalModel
    {
        // ReSharper disable once InconsistentNaming
        private static readonly Lazy<GlobalModel> _instance =
            new Lazy<GlobalModel>(() => new GlobalModel());

        private GlobalModel() { }

        public static GlobalModel Instance { get { return _instance.Value; } }

        //********************************************************
        #region Actual global data goes here.
        // Note: These should be initialized at app startup.

        public LayerManager LayerManager { get; set; }
        public Layout Layout { get; set; }

        #endregion
        //********************************************************
    }
}
