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
    /// <typeparam name="TItem"> The type of item </typeparam>
    public class SortedContainer<TItem> : SynchronizedCollection<TItem>
        where TItem : IComparable<TItem>
    {
        /// <summary>
        /// Adds the new item keeping the collection sorted
        /// </summary>
        /// <param name="item"> The item to add </param>
        public new void Add(TItem item)
        {
            //TODO wrap the operation to make it atomic
            //lock (SyncRoot)
            {
                // if there are no elements, add it to the beginning
                if (Count == 0)
                {
                    base.Add(item);
                    return;
                }
                // otherwise find where it should be added
                for (int i = 0; i < Count; i++)
                {
                    if (base[i].CompareTo(item) > 0)
                    {
                        Insert(i, item);
                        return;
                    }
                }
                // at this step it means it should be added at the end
                base.Add(item);
            }
        }

        /// <summary>
        /// Ensure item (re)ordering after external modifications;
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
