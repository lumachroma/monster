using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using FirebaseRest;
using FirebaseRest.Models;
using Monster.Extensions;
using Monster.Firebase;
using Monster.Models;
using Newtonsoft.Json;

namespace Monster.Controllers
{
    public class AuctionController : Controller
    {
        private readonly AdminControllerQueryEntity<Auction> _adminAuctionQuery;
        private readonly FirebaseDataContext<Auction> _auctionContext;
        private readonly UserControllerQueryEntity<Auction> _userAuctionQuery;

        public AuctionController()
        {
            var firebaseUrl = ConfigurationManager.AppSettings["FirebaseUrl"] ?? "https://swag-monster.firebaseio.com";
            var firebaseAuth = ConfigurationManager.AppSettings["FirebaseAuth"] ??
                               "MfX7DaAWOUjr0zJ2invYbaX6UceHZ3vrif0VGeL4";
            var firebaseQuery = new FirebaseQuery(new FirebaseClient(firebaseUrl, firebaseAuth));
            _auctionContext = new FirebaseDataContext<Auction>("Auctions", firebaseQuery);
            _adminAuctionQuery = new AdminControllerQueryEntity<Auction>(_auctionContext);
            _userAuctionQuery = new UserControllerQueryEntity<Auction>(_auctionContext);
        }

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Details(string key)
        {
            if (string.IsNullOrEmpty(key)) return this.BadRequest(string.Empty);

            ViewBag.Key = key;

            return View();
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        public ActionResult Edit(string key)
        {
            if (string.IsNullOrEmpty(key)) return this.BadRequest(string.Empty);

            ViewBag.Key = key;

            return View();
        }

        [Authorize]
        public ActionResult Remove(string key)
        {
            if (string.IsNullOrEmpty(key)) return this.BadRequest(string.Empty);

            ViewBag.Key = key;

            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> All()
        {
            IReadOnlyCollection<FirebaseObject<Auction>> results;
            if (this.IsAdministrator()) results = await _adminAuctionQuery.All();
            else results = await _userAuctionQuery.All();
            return this.Ok(JsonConvert.SerializeObject(results));
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Some(string key)
        {
            IReadOnlyCollection<FirebaseObject<Auction>> results;
            if (this.IsAdministrator()) results = await _adminAuctionQuery.Some(key);
            else results = await _userAuctionQuery.Some(key);
            return this.Ok(JsonConvert.SerializeObject(results));
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Get(string key)
        {
            Auction result;
            if (this.IsAdministrator()) result = await _adminAuctionQuery.Get(key);
            else result = await _userAuctionQuery.Get(key);
            return null != result
                ? this.Ok(JsonConvert.SerializeObject(result))
                : this.NotFound($"{key} not found!");
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Post(Auction auction)
        {
            FirebaseObject<Auction> result = null;
            if (this.IsAdministrator()) result = await _adminAuctionQuery.Post(auction);
            else result = await _userAuctionQuery.Post(auction);
            return null != result
                ? this.Ok(JsonConvert.SerializeObject(result))
                : this.InternalServerError(string.Empty);
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult> Put(string key, Auction auction)
        {
            var existing = await _auctionContext.GetByKeyAsync(key);
            if (null == existing) return this.NotFound($"{key} not found!");

            Auction result;
            if (this.IsAdministrator()) result = await _adminAuctionQuery.Put(key, auction);
            else result = await _userAuctionQuery.Put(key, auction);
            return null != result
                ? this.Ok(JsonConvert.SerializeObject(new FirebaseObject<Auction>(key, auction)))
                : this.InternalServerError(string.Empty);
        }

        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> Delete(string key, Auction auction)
        {
            var existing = await _auctionContext.GetByKeyAsync(key);
            if (null == existing) return this.NotFound($"{key} not found!");

            if (this.IsAdministrator()) await _adminAuctionQuery.Delete(key);
            else await _userAuctionQuery.Delete(key);
            return this.Ok(JsonConvert.SerializeObject(new FirebaseObject<Auction>(key, auction)));
        }
    }
}