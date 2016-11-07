using System;
using System.Threading;
using JetBrains.Annotations;

namespace VectorImageEdit.Modules.LayerManagement
{
    /// <summary>
    /// LayerMetadata Info
    /// 
    /// - contains the layer metadata properties such as an UID and DisplayName
    /// </summary>
    [Serializable]
    public class LayerMetadata
    {
        private static int _nextId = -1;

        public LayerMetadata([NotNull]string displayName)
        {
            Uid = Interlocked.Increment(ref _nextId);
            DisplayName = displayName + " [" + Uid + "]";
        }

        /// <summary>
        /// Gets or sets the name associated with the layer.
        /// </summary>
        [NotNull]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets an unique ID for the layer.
        /// </summary>
        public int Uid { get; private set; }
    }
}
