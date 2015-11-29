using JetBrains.Annotations;
using VectorImageEdit.Modules.LayerManagement;
using VectorImageEdit.Modules.Utility;

namespace VectorImageEdit.Modules.Interfaces
{
    public interface ILayerHandler
    {
        [NotNull]SortedContainer<Layer> WorkspaceLayers { get; }
    }
}