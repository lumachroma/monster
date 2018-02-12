using System;
using System.Collections.Generic;
using FirebaseRepository;

namespace Monster.Models
{
    public class User : IFirebaseEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public List<string> Roles { get; set; } = new List<string>();

        public string ChangedBy { get; set; }
        public DateTime ChangedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Id { get; set; }
        public string WebId { get; set; }
    }
}