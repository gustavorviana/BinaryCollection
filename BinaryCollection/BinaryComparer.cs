using System.Collections.Generic;

namespace BinaryCollection
{
    /// <summary>
    /// Provides a default implementation of IBinaryComparer&lt;T&gt; that uses the default comparer and equality comparer for type T.
    /// </summary>
    /// <typeparam name="T">The type of objects to compare.</typeparam>
    public class BinaryComparer<T> : IBinaryComparer<T>
    {
        private readonly IComparer<T> _comparer = Comparer<T>.Default;
        private readonly IEqualityComparer<T> _equalityComparer = EqualityComparer<T>.Default;

        /// <summary>
        /// Gets the default BinaryComparer&lt;T&gt; instance for the type specified by the generic argument.
        /// </summary>
        public static BinaryComparer<T> Default { get; } = new BinaryComparer<T>();

        /// <summary>
        /// Initializes a new instance of the BinaryComparer&lt;T&gt; class.
        /// This constructor is private to enforce the singleton pattern through the Default property.
        /// </summary>
        private BinaryComparer()
        {
        }

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of x and y:
        /// Less than zero: x is less than y.
        /// Zero: x equals y.
        /// Greater than zero: x is greater than y.
        /// </returns>
        public int Compare(T x, T y)
        {
            return _comparer.Compare(x, y);
        }

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>true if the specified objects are equal; otherwise, false.</returns>
        public bool Equals(T x, T y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x == null || y == null) return false;
            return _equalityComparer.Equals(x, y);
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <param name="obj">The object for which a hash code is to be returned.</param>
        /// <returns>A hash code for the specified object, or zero if the object is null.</returns>
        public int GetHashCode(T obj)
        {
            return obj?.GetHashCode() ?? 0;
        }
    }
}