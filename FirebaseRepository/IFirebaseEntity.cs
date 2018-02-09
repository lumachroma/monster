using System;

namespace FirebaseRepository
{
    public interface IFirebaseEntity
    {
        string ChangedBy { get; set; }
        DateTime ChangedDate { get; set; }
        string CreatedBy { get; set; }
        DateTime CreatedDate { get; set; }
        string Id { get; set; }
        string WebId { get; set; }
    }
}