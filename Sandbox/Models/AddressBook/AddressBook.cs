using System;
using System.Collections.Generic;
using FirebaseRepository;

namespace Sandbox.Models.AddressBook
{
    public class AddressBook: IFirebaseEntity
    {
        public AddressBook()
        {
            ContactInformation = new ContactInformation();
            Address = new Address();
            ReferenceNo = Guid.NewGuid().ToString();
        }

        public string ReferenceNo { get; set; }
        public string CompanyName { get; set; }
        public string ContactPerson { get; set; }
        public List<string> Groups { get; } = new List<string>();
        public ContactInformation ContactInformation { get; set; }
        public Address Address { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string UserId { get; set; }

        public string ChangedBy { get; set; }
        public DateTime ChangedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Id { get; set; }
        public string WebId { get; set; }
    }
}