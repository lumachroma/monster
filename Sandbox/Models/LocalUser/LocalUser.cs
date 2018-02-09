using System;
using System.Collections.Generic;
using FirebaseRepository;

namespace Sandbox.Models.LocalUser
{
    public class LocalUser : IFirebaseEntity
    {
        public LocalUser()
        {
            ReferenceNo = Guid.NewGuid().ToString();
            IsActivated = false;
            IsLocked = false;
        }

        public string ReferenceNo { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string Location { get; set; }
        public bool IsActivated { get; set; }
        public bool IsLocked { get; set; }
        public List<ExternalUser> ExternalUsers { get; set; } = new List<ExternalUser>();
        public List<string> Roles { get; set; } = new List<string>();

        public string ChangedBy { get; set; }
        public DateTime ChangedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Id { get; set; }
        public string WebId { get; set; }
    }
}