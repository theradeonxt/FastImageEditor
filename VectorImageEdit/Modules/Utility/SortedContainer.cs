using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace VectorImageEdit.Modules.Utility
{
    // TODO: Investigate thread safety for iteration and lock overhead

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
            /* TODO: wrap the operation to make it atomic?
             * The underlying container "Add" is thread-safe, 
             * but the overall behavior here is not
             * 
             * Note: Locking here sometimes deadlocks...
             * */

            lock (SyncRoot)
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
        /// the caller is responsible for handling this
        /// </summary>
        public void Rebuild()
        {
            // TODO: This looks ugly
            var copy = new TItem[Count];
            CopyTo(copy, 0);
            Clear();
            foreach (var layer in copy)
            {
                Add(layer);
            }
        }

        /// <summary>
        /// Gets the Last item in the collection, 
        /// based on the item relative ordering rules.
        /// If no item exists an ArgumentOutOfRangeException is thrown.
        /// </summary>
        [NotNull]
        public TItem Last
        {
            // TODO: This restricts the underlying container to use List
            get { return base[Count - 1]; }
        }

        [NotNull]
        public TItem First
        {
            get { return base[0]; }
        }
    }
}
