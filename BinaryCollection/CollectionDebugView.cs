using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BinaryCollection
{
    /// <summary>
    /// Provides a debugger view for collections that displays the items in an array format.
    /// This class is used internally by the debugger to provide a better visualization of collection contents.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    internal sealed class CollectionDebugView<T>
    {
        private readonly ICollection<T> collection;

        /// <summary>
        /// Gets an array representation of the collection items for debugger display.
        /// This property is marked as RootHidden to show the array elements directly in the debugger
        /// instead of showing them nested under a property.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items
        {
            get
            {
                T[] array = new T[collection.Count];
                collection.CopyTo(array, 0);
                return array;
            }
        }

        /// <summary>
        /// Initializes a new instance of the CollectionDebugView&lt;T&gt; class for the specified collection.
        /// </summary>
        /// <param name="collection">The collection to provide a debug view for.</param>
        /// <exception cref="ArgumentNullException">Thrown when collection is null.</exception>
        public CollectionDebugView(ICollection<T> collection)
        {
            this.collection = collection ?? throw new ArgumentNullException(nameof(collection));
        }
    }
}