namespace Sample
{
    public class Person
    {
        public string FirstName { get; }
        public string LastName { get; }
        public int Age { get; }
        public Address Address { get; }

        public Person(string firstName, string lastName, int age, Address address)
        {
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            Age = age;
            Address = address ?? throw new ArgumentNullException(nameof(address));
        }

        public string FullName => $"{FirstName} {LastName}";

        public override string ToString()
        {
            return $"{FullName}, {Age} years old - {Address.City}, {Address.State}";
        }
    }
}
