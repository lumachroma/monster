using System.Collections.Generic;

namespace Monster.Tests.Mocks
{
    public class MockFirebaseFixture
    {
        public readonly List<MockFirebasePerson> PersonList;

        public MockFirebaseFixture()
        {
            PersonList = new List<MockFirebasePerson>
            {
                new MockFirebasePerson
                {
                    Name = "Johnathan Doe",
                    Age = 25,
                    Gender = "Male",
                    Nicknames = new List<string> {"Maveric", "Mav", "John"}
                },
                new MockFirebasePerson
                {
                    Name = "Johanna Doe",
                    Age = 23,
                    Gender = "Female",
                    Nicknames = new List<string> {"Jean", "Joan"}
                },
                new MockFirebasePerson
                {
                    Name = "John Clayton",
                    Age = 35,
                    Gender = "Male",
                    Nicknames = new List<string> {"Tarzan", "John"}
                },
                new MockFirebasePerson
                {
                    Name = "Jane Porter",
                    Age = 32,
                    Gender = "Male",
                    Nicknames = new List<string> {"Jane"}
                }
            };
        }
    }
}