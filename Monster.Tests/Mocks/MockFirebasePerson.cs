using System;
using System.Collections.Generic;
using FirebaseRepository;

namespace Monster.Tests.Mocks
{
    public class MockFirebasePerson : IFirebaseEntity
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public List<string> Nicknames { get; set; } = new List<string>();

        public string ChangedBy { get; set; }
        public DateTime ChangedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Id { get; set; }
        public string WebId { get; set; }
    }
}