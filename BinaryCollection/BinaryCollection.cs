using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BinaryCollection
{
    /// <summary>
    /// Represents a collection that maintains its elements in sorted order using binary search operations.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    public class BinaryCollection<T> : ICollection<T>
    {
        private readonly IBinaryComparer<T> _comparer;
        private readonly List<T> _items;

        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The element at the specified index.</returns>
        public T this[int index] => _items[index];

        /// <summary>
        /// Initializes a new instance of the BinaryCollection class that contains elements copied from the specified collection
        /// and uses the specified comparer to maintain sorted order.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new BinaryCollection.</param>
        /// <param name="comparer">The comparer to use when comparing elements, or null to use the default comparer.</param>
        /// <exception cref="ArgumentNullException">Thrown when collection is null.</exception>
        public BinaryCollection(IEnumerable<T> collection, IBinaryComparer<T> comparer = null)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            _comparer = comparer ?? BinaryComparer<T>.Default;
            _items = new List<T>(collection);
            _items.Sort(comparer);
        }

        /// <summary>
        /// Initializes a new instance of the BinaryCollection class that is empty and uses the specified comparer.
        /// </summary>
        /// <param name="comparer">The comparer to use when comparing elements, or null to use the default comparer.</param>
        public BinaryCollection(IBinaryComparer<T> comparer = null)
        {
            _comparer = comparer ?? BinaryComparer<T>.Default;
            _items = new List<T>();
        }

        /// <summary>
        /// Gets the number of elements contained in the BinaryCollection.
        /// </summary>
        public int Count => _items.Count;

        /// <summary>
        /// Gets a value indicating whether the BinaryCollection is read-only.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Searches for the first element that matches the specified item.
        /// </summary>
        /// <param name="item">The item to search for.</param>
        /// <returns>The first matching element, or the default value of T if no match is found.</returns>
        public T Find(T item)
        {
            return FindAll(item).FirstOrDefault();
        }

        /// <summary>
        /// Searches for all elements that match the specified item.
        /// </summary>
        /// <param name="item">The item to search for.</param>
        /// <returns>An enumerable collection of all matching elements.</returns>
        public IEnumerable<T> FindAll(T item)
        {
            return IndexOfAll(item).Select(index => _items[index]);
        }

        /// <summary>
        /// Removes all duplicate elements from the collection, keeping only the first occurrence of each unique element.
        /// </summary>
        public void RemoveDuplicates()
        {
            if (_items.Count <= 1)
                return;

            int writeIndex = 1;
            for (int readIndex = 1; readIndex < _items.Count; readIndex++)
            {
                if (!IsEquals(_items[readIndex], _items[writeIndex - 1]))
                {
                    if (writeIndex != readIndex)
                        _items[writeIndex] = _items[readIndex];
                    writeIndex++;
                }
            }

            if (writeIndex < _items.Count)
                _items.RemoveRange(writeIndex, _items.Count - writeIndex);
        }

        /// <summary>
        /// Adds an element to the collection while maintaining sorted order.
        /// </summary>
        /// <param name="item">The element to add to the collection.</param>
        public void Add(T item)
        {
            int index = _items.BinarySearch(item, _comparer);
            if (index < 0)
                index = ~index;

            _items.Insert(index, item);
        }

        /// <summary>
        /// Adds the elements of the specified collection to the BinaryCollection and re-sorts the entire collection.
        /// </summary>
        /// <param name="items">The collection whose elements should be added to the BinaryCollection.</param>
        /// <exception cref="ArgumentNullException">Thrown when items is null.</exception>
        public void AddRange(IEnumerable<T> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            _items.AddRange(items);
            _items.Sort(_comparer);
        }

        /// <summary>
        /// Removes the first occurrence of a specific element from the collection.
        /// </summary>
        /// <param name="item">The element to remove from the collection.</param>
        /// <returns>true if item was successfully removed; otherwise, false.</returns>
        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index < 0)
                return false;

            _items.RemoveAt(index);
            return true;
        }

        /// <summary>
        /// Removes the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when index is less than 0 or greater than or equal to Count.</exception>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= _items.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of the collection's range.");

            _items.RemoveAt(index);
        }

        /// <summary>
        /// Determines whether the collection contains a specific element.
        /// </summary>
        /// <param name="item">The element to locate in the collection.</param>
        /// <returns>true if item is found in the collection; otherwise, false.</returns>
        public bool Contains(T item)
        {
            return IndexOf(item) >= 0;
        }

        /// <summary>
        /// Searches for the specified element and returns the zero-based index of the first occurrence.
        /// </summary>
        /// <param name="item">The element to locate in the collection.</param>
        /// <returns>The zero-based index of the first occurrence of item, if found; otherwise, -1.</returns>
        public int IndexOf(T item)
        {
            foreach (var index in IndexOfAll(item))
                return index;

            return -1;
        }

        /// <summary>
        /// Searches for the specified element and returns the zero-based indices of all occurrences.
        /// </summary>
        /// <param name="item">The element to locate in the collection.</param>
        /// <returns>An enumerable collection of zero-based indices where the item occurs.</returns>
        public IEnumerable<int> IndexOfAll(T item)
        {
            int index = _items.BinarySearch(item, _comparer);
            if (index < 0)
                yield break;

            int first = index;
            while (first > 0 && _comparer.Compare(_items[first - 1], item) == 0)
                first--;

            int last = index;
            while (last + 1 < _items.Count && _comparer.Compare(_items[last + 1], item) == 0)
                last++;

            for (int i = first; i <= last; i++)
                if (_comparer.Equals(_items[i], item))
                    yield return i;
        }

        /// <summary>
        /// Determines whether two elements are equal using both comparison and equality checks.
        /// </summary>
        /// <param name="x">The first element to compare.</param>
        /// <param name="y">The second element to compare.</param>
        /// <returns>true if the elements are equal; otherwise, false.</returns>
        private bool IsEquals(T x, T y)
        {
            return _comparer.Compare(x, y) == 0 && _comparer.Equals(x, y);
        }

        /// <summary>
        /// Removes all elements from the collection.
        /// </summary>
        public void Clear()
        {
            _items.Clear();
        }

        /// <summary>
        /// Copies the elements of the collection to an Array, starting at a particular Array index.
        /// </summary>
        /// <param name="array">The one-dimensional Array that is the destination of the elements copied from the collection.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Determines whether the collection uses the specified comparer.
        /// </summary>
        /// <param name="comparer">The comparer to check against.</param>
        /// <returns>true if the collection uses the specified comparer; otherwise, false.</returns>
        public bool IsComparer(IBinaryComparer<T> comparer)
        {
            return _comparer.Equals(comparer);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}