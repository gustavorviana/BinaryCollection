using BinaryCollection;
using Sample;

Console.WriteLine("=== Person Example with Address ===");

var comparer = new PersonCityAgeComparer();
var people = new BinaryCollection<Person>(comparer);

people.AddRange(
[
    new Person("John", "Smith", 25, new Address("123 Main St", "New York", "NY", "10001")),
    new Person("Jane", "Doe", 30, new Address("456 Oak Ave", "Los Angeles", "CA", "90210")),
    new Person("Bob", "Johnson", 25, new Address("789 Pine St", "New York", "NY", "10002")),
    new Person("Alice", "Williams", 28, new Address("321 Elm St", "Chicago", "IL", "60601")),
    new Person("Charlie", "Brown", 25, new Address("654 Maple Dr", "Chicago", "IL", "60602")),
    new Person("Diana", "Wilson", 30, new Address("987 Cedar Ln", "New York", "NY", "10003")),
    new Person("Eve", "Davis", 25, new Address("147 Birch Way", "Los Angeles", "CA", "90211")),
    new Person("Frank", "Miller", 32, new Address("258 Spruce St", "Chicago", "IL", "60603")),
    new Person("Grace", "Taylor", 25, new Address("369 Willow Ave", "New York", "NY", "10004"))
]);

Console.WriteLine("All people (sorted by city, then age):");
foreach (var person in people)
    Console.WriteLine($"  {person}. Hash: {comparer.GetHashCode(person)}.");

Console.WriteLine();

FindAndPrint(25, "New York");
FindAndPrint(30, "Los Angeles");
FindAndPrint(25, "Chicago");

Console.WriteLine("=== Adding new person ===");
var searchCriteria = new Person("", "", 25, new Address("", "Chicago", "", ""));
var newPerson = new Person("Helen", "Garcia", 25, new Address("555 Oak St", "New York", "NY", "10005"));
people.Add(newPerson);

Console.WriteLine($"Added: {newPerson}");
Console.WriteLine("Updated search for age 25 in New York:");
PrintAll(people.FindAll(searchCriteria));


void FindAndPrint(int age, string city)
{
    Console.WriteLine($"=== Search: Age {age} in {city} ===");
    var searchCriteria = new Person("", "", age, new Address("", city, "", ""));
    var results = people.FindAll(searchCriteria);

    Console.WriteLine($"Found {results.Count()} people aged {age} in {city}:");
    PrintAll(results);
}

void PrintAll(IEnumerable<Person> persons)
{
    foreach (var person in persons)
        Console.WriteLine($"  {person}");

    Console.WriteLine();
}