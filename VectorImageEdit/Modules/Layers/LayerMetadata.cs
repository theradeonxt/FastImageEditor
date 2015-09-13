using System;
using System.Threading;

namespace VectorImageEdit.Modules.Layers
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

        public LayerMetadata(string displayName)
        {
            Uid = Interlocked.Increment(ref _nextId);
            DisplayName = displayName + " [" + Uid + "]";
        }

        public override string ToString()
        {
            return DisplayName;
        }

        public string DisplayName { get; set; }

        public int Uid { get; private set; }
    }
}
