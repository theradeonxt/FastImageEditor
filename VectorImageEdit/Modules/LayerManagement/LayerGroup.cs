using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace VectorImageEdit.Modules.LayerManagement
{
    /// <summary>
    /// Defines an object that contains multiple groupped layers
    /// </summary>
    class LayerGroup : Layer, IEnumerable<Layer>
    {
        private readonly List<Layer> children; // group items
        private static int _groupId = 0;

        public LayerGroup()
            : base(Rectangle.Empty, 0, (_groupId++).ToString())
        {
            children = new List<Layer>();
        }

        #region Implementations

        public override void Dispose()
        {
            children.Clear();
        }

        public override void DrawGraphics(Graphics destination)
        {
            for (int i = 0; i < children.Count; i++) children[i].DrawGraphics(destination);
        }

        public IEnumerator<Layer> GetEnumerator()
        {
            return children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public void Add(Layer layer)
        {
            children.Add(layer);
        }

        public void Remove(Layer layer)
        {
            children.Remove(layer);
        }
    }
}
