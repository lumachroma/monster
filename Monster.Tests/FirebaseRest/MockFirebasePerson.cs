using System.Collections.Generic;

namespace Monster.Tests.FirebaseRest
{
    public class MockFirebasePerson
    {
        public List<string> Nicknames;

        public MockFirebasePerson()
        {
            Nicknames = new List<string>();
        }

        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
    }
}