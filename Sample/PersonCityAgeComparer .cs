using BinaryCollection;

namespace Sample;

public class PersonCityAgeComparer : IBinaryComparer<Person>
{
    public int Compare(Person? x, Person? y)
    {
        if (x == null && y == null) return 0;
        if (x == null) return -1;
        if (y == null) return 1;

        int hashX = GetHashCode(x);
        int hashY = GetHashCode(y);

        return hashX.CompareTo(hashY);
    }

    public bool Equals(Person? x, Person? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x == null || y == null) return false;

        return x.Age == y.Age && string.Equals(x.Address.City, y.Address.City, StringComparison.OrdinalIgnoreCase);
    }

    public int GetHashCode(Person obj)
    {
        if (obj == null) return 0;

        return HashCode.Combine(
            obj.Age,
            obj.Address.City.ToUpperInvariant()
        );
    }
}
