using System;
using System.Collections.Generic;

namespace Sandbox.Models
{
    public class AddressBook
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
    }
}