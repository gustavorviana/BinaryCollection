using System.Collections.Generic;

namespace BinaryCollection
{
    /// <summary>
    /// Defines methods to support comparison and equality operations for binary search collections.
    /// This interface combines both comparison and equality comparison capabilities required for
    /// maintaining sorted collections with efficient binary search operations.
    /// </summary>
    /// <typeparam name="T">The type of objects to compare.</typeparam>
    public interface IBinaryComparer<T> : IComparer<T>, IEqualityComparer<T>
    {
    }
}