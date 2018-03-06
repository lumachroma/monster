using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using FirebaseRest;
using FirebaseRest.Models;
using Monster.Extensions;
using Monster.Firebase;
using Monster.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Monster.Controllers
{
    public class AuctionAppController : Controller
    {
        private readonly FirebaseDataContext<Auction> _auctionContext;

        public AuctionAppController()
        {
            var firebaseUrl = ConfigurationManager.AppSettings["FirebaseUrl"] ?? "https://swag-monster.firebaseio.com";
            var firebaseAuth = ConfigurationManager.AppSettings["FirebaseAuth"] ??
                               "MfX7DaAWOUjr0zJ2invYbaX6UceHZ3vrif0VGeL4";
            var firebaseQuery = new FirebaseQuery(new FirebaseClient(firebaseUrl, firebaseAuth));
            _auctionContext = new FirebaseDataContext<Auction>("Auctions", firebaseQuery);
        }

        [Authorize]
        [Route("AuctionApp/Auctioneer/{key}")]
        public ActionResult Auctioneer(string key)
        {
            if (string.IsNullOrEmpty(key)) return this.BadRequest(string.Empty);

            ViewBag.Key = key;
            ViewBag.Message = "The Auctioneer page.";

            return View();
        }

        [Route("AuctionApp/Bidder/{key}")]
        public ActionResult Bidder(string key)
        {
            if (string.IsNullOrEmpty(key)) return this.BadRequest(string.Empty);

            ViewBag.Key = key;
            ViewBag.Message = "The Bidder page.";

            return View();
        }

        [Route("AuctionApp/VerifyBidder/{key}")]
        [HttpPost]
        public async Task<ActionResult> VerifyBidder(string key, AuctionAppModel model)
        {
            var auction = await _auctionContext.GetByKeyAsync(key);
            if (null == auction)
                return this.NotFound($"{key} not found!");
            if (!this.IsBidder(model.BidderEmail, model.BidderCode, auction.Bidders))
                return this.Forbidden($"{model.BidderEmail} not verified!");

            var bidderNickname = this.GetBidder(model.BidderEmail, model.BidderCode, auction.Bidders).Nickname;

            dynamic jObject = new JObject();
            jObject.Key = key;
            jObject.Auth = true;
            jObject.Nickname = bidderNickname;
            return this.Ok((string)JsonConvert.SerializeObject(jObject).ToString());
        }

        [Route("AuctionApp/PerformBid/{key}")]
        [HttpPost]
        public async Task<ActionResult> PerformBid(string key, AuctionAppModel model)
        {
            var auction = await _auctionContext.GetByKeyAsync(key);
            if (null == auction)
                return this.NotFound($"{key} not found!");
            if (!this.IsBidder(model.BidderEmail, model.BidderCode, auction.Bidders))
                return this.Forbidden($"{model.BidderEmail} not verified!");
            if (model.BidderEmail == auction.Bidder.Email && model.BidderCode == auction.Bidder.Code)
                return this.BadRequest($"Invalid bid: {auction.Bidder.Email} is higest bidder!");
            if (auction.Status != "Started")
                return this.BadRequest($"Invalid status: {auction.Status}!");
            if (auction.Call > 3)
                return this.BadRequest($"Invalid call: {auction.Call}!");
            if (model.BidAmount <= auction.Amount)
                return this.BadRequest($"Invalid amount: {auction.Amount}!");

            this.BidTheAuction(model, auction);
            this.CallTheAuction(auction);

            var result = await _auctionContext.PutAsync(auction, key);
            return null != result
                ? this.Ok(JsonConvert.SerializeObject(new FirebaseObject<Auction>(key, auction)))
                : this.InternalServerError(string.Empty);
        }

        [Authorize]
        [Route("AuctionApp/PerformCall/{key}")]
        [HttpPost]
        public async Task<ActionResult> PerformCall(string key)
        {
            var auction = await _auctionContext.GetByKeyAsync(key);
            if (!this.IsAdministrator() && this.GetCurrentUser() != auction.CreatedBy)
                return this.Forbidden("Invalid autioneer");
            if (null == auction)
                return this.NotFound($"{key} not found!");
            if (auction.Status != "Started")
                return this.BadRequest($"Invalid status: {auction.Status}!");
            if (auction.Call > 3)
                return this.BadRequest($"Invalid call: {auction.Call}!");

            if (auction.Call == 3) this.EndTheAuction(auction);
            else this.CallTheAuction(auction);

            var result = await _auctionContext.PutAsync(auction, key);
            return null != result
                ? this.Ok(JsonConvert.SerializeObject(new FirebaseObject<Auction>(key, auction)))
                : this.InternalServerError(string.Empty);
        }

        [Authorize]
        [Route("AuctionApp/PerformStart/{key}")]
        [HttpPost]
        public async Task<ActionResult> PerformStart(string key)
        {
            var auction = await _auctionContext.GetByKeyAsync(key);
            if (!this.IsAdministrator() && this.GetCurrentUser() != auction.CreatedBy)
                return this.Forbidden("Invalid autioneer");
            if (null == auction)
                return this.NotFound($"{key} not found!");
            if (auction.Status != "New")
                return this.BadRequest($"Invalid status: {auction.Status}!");

            this.StartTheAuction(auction);

            var result = await _auctionContext.PutAsync(auction, key);
            return null != result
                ? this.Ok(JsonConvert.SerializeObject(new FirebaseObject<Auction>(key, auction)))
                : this.InternalServerError(string.Empty);
        }
    }

    public class AuctionAppModel
    {
        public string BidderEmail { get; set; }
        public string BidderCode { get; set; }
        public decimal BidAmount { get; set; }
    }
}