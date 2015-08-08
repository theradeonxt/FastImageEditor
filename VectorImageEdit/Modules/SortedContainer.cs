using System;
using System.Collections.Generic;
using VectorImageEdit.Modules.Layers;

namespace VectorImageEdit.Modules
{
    /// <summary>
    /// 
    /// SortedContainer Module
    ///
    /// - holds a sorted list of layers, using their comparison function
    ///
    /// </summary>
    public class SortedContainer : SynchronizedCollection<Layer>
    {
        /// <summary>
        /// Adds the new layer keeping the collection sorted
        /// based on the layers' comparison method
        /// </summary>
        /// <param name="item"></param>
        public new void Add(Layer item)
        {
            // First element is a simple add
            if (Count == 0)
            {
                base.Add(item);
                return;
            }
            // Otherwise find where it should be added
            for (int i = 0; i < Count; i++)
            {
                if (base[i].CompareTo(item) > 0)
                {
                    Insert(i, item);
                    return;
                }
            }
            // At this step it means it should be added at the end
            base.Add(item);
        }

        /// <summary>
        /// Ensure item ordering after external modifications
        ///  Up to caller to handle this
        /// </summary>
        public void Rebuild()
        {
            Layer[] copy = new Layer[Count];
            CopyTo(copy, 0);
            Clear();
            foreach (Layer layer in copy)
            {
                Add(layer);
            }
        }
    }
}
