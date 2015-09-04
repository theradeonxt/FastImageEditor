using System;
using System.Collections.Generic;

namespace VectorImageEdit.Modules.Utility
{
    /// <summary>
    /// 
    /// SortedContainer Module
    ///
    /// - implements a sorted list of items, using their comparison function
    /// - thread safe collection wrapper over SynchronizedCollection
    /// 
    /// </summary>
    public class SortedContainer<TItem> : SynchronizedCollection<TItem>
        where TItem : IComparable<TItem>
    {
        /// <summary>
        /// Adds the new item keeping the collection sorted
        /// based on the items' comparison method
        /// </summary>
        /// <param name="item"></param>
        public new void Add(TItem item)
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
        /// Ensure item ordering after external modifications;
        /// it's up to caller to handle this
        /// </summary>
        public void Rebuild()
        {
            var copy = new TItem[Count];
            CopyTo(copy, 0);
            Clear();
            foreach (var layer in copy)
            {
                Add(layer);
            }
        }
    }
}
