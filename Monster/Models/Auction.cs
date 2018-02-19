using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FirebaseRepository;

namespace Monster.Models
{
    public class Auction : IFirebaseEntity
    {
        public Auction()
        {
            Logs = new List<Log>();
            Bidders = new List<Bidder>();
        }

        public string Title { get; set; }
        public string Status { get; set; }
        public string ProductName { get; set; }
        public string ProductImageUrl { get; set; }
        public string ProductUrl { get; set; }
        public string ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal Amount { get; set; }
        public decimal Step { get; set; }
        public int Interval { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime StopDateTime { get; set; }
        public List<Log> Logs { get; set; }
        public List<Bidder> Bidders { get; set; }

        public string ChangedBy { get; set; }
        public DateTime ChangedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Id { get; set; }
        public string WebId { get; set; }
    }

    public class Log
    {
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
        public string WebId { get; set; }
    }

    public class Bidder
    {
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
        public string WebId { get; set; }
    }
}