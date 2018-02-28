using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.WebPages;
using Monster.Controllers;
using Monster.Models;

namespace Monster.Extensions
{
    internal static class AuctionAppControllerExtension
    {
        internal static void StartTheAuction(this AuctionAppController self, Auction auction)
        {
            auction.Status = "Started";
            auction.Call = 0;
            auction.Amount = auction.ProductPrice;
            auction.Bidder = new Bidder();
            auction.Logs = new List<Log>();
            var auctionLogText = $"Auction started at RM {auction.Amount}.";
            AddLogToAuction($"Admin/{auction.Contact.Name}", auction.Contact.Email, auctionLogText, auction);
        }

        internal static void EndTheAuction(this AuctionAppController self, Auction auction)
        {
            var winner = auction.Bidder;
            auction.Status = "Ended";
            var auctionLogText = $"Auction ended at RM {auction.Amount}.";
            AddLogToAuction($"Admin/{auction.Contact.Name}", auction.Contact.Email, auctionLogText, auction);
            auctionLogText = !winner.Nickname.IsEmpty()
                ? $"Auction won by @{winner.Nickname}({winner.Email}) at RM {auction.Amount}."
                : "Too bad, nobody wins the auction!";
            AddLogToAuction($"Admin/{auction.Contact.Name}", auction.Contact.Email, auctionLogText, auction);
        }

        internal static void CallTheAuction(this AuctionAppController self, Auction auction)
        {
            auction.Call += 1;
            var auctionLogText = $"Call {auction.Call} for RM {auction.Amount}.";
            AddLogToAuction($"Admin/{auction.Contact.Name}", auction.Contact.Email, auctionLogText, auction);
        }

        internal static void BidTheAuction(this AuctionAppController self, AuctionAppModel model, Auction auction)
        {
            var bidder = GetBidder(model.BidderEmail, model.BidderCode, auction.Bidders);
            auction.Call = 0;
            auction.Amount = model.BidAmount;
            auction.Bidder = bidder;
            var auctionLogText = $"Bid for RM {model.BidAmount}.";
            AddLogToAuction(bidder.Nickname, bidder.Email, auctionLogText, auction);
            auctionLogText = $"{bidder.Nickname} bid for RM {model.BidAmount}.";
            AddLogToAuction($"Admin/{auction.Contact.Name}", auction.Contact.Email, auctionLogText, auction);
        }

        internal static bool IsBidder(this AuctionAppController self, string bidderEmail, string bidderCode,
            IEnumerable<Bidder> bidders)
        {
            return IsBidder(bidderEmail, bidderCode, bidders);
        }

        internal static Bidder GetBidder(this AuctionAppController self, string bidderEmail, string bidderCode,
            IEnumerable<Bidder> bidders)
        {
            return GetBidder(bidderEmail, bidderCode, bidders);
        }

        private static bool IsBidder(string bidderEmail, string bidderCode, IEnumerable<Bidder> bidders)
        {
            return bidders.Any(x => x.Email == bidderEmail && x.Code == bidderCode);
        }

        private static Bidder GetBidder(string bidderEmail, string bidderCode, IEnumerable<Bidder> bidders)
        {
            return bidders.FirstOrDefault(x => x.Email == bidderEmail && x.Code == bidderCode);
        }

        private static void AddLogToAuction(string nickname, string email, string text, Auction auction)
        {
            auction.Logs.Add(new Log
            {
                Nickname = nickname,
                Email = email,
                Text = text,
                Timestamp = DateTime.Now,
                WebId = Guid.NewGuid().ToString()
            });
        }
    }
}