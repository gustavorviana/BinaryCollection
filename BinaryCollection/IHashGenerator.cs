using System.Collections.Generic;

namespace BinaryCollection
{
    /// <summary>
    /// Defines a method for generating hash codes using a specific equality comparer.
    /// This interface allows objects to provide custom hash code generation logic
    /// that is consistent with a given equality comparer.
    /// </summary>
    /// <typeparam name="T">The type of objects for which hash codes are generated.</typeparam>
    public interface IHashGenerator<T>
    {
        /// <summary>
        /// Returns a hash code for the current object using the specified equality comparer.
        /// The hash code must be consistent with the equality logic of the provided comparer.
        /// </summary>
        /// <param name="comparer">The equality comparer to use for hash code generation.</param>
        /// <returns>A hash code for the current object that is consistent with the specified comparer.</returns>
        int GetHashCode(IEqualityComparer<T> comparer);
    }
}