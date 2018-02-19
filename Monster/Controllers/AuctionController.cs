using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Claims;
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
        private readonly FirebaseDataContext<Auction> _auctionContext;
        private readonly FirebaseDataContext<User> _userContext;

        public AuctionController()
        {
            var firebaseUrl = ConfigurationManager.AppSettings["FirebaseUrl"] ?? "https://swag-monster.firebaseio.com";
            var firebaseAuth = ConfigurationManager.AppSettings["FirebaseAuth"] ??
                               "MfX7DaAWOUjr0zJ2invYbaX6UceHZ3vrif0VGeL4";
            var firebaseQuery = new FirebaseQuery(new FirebaseClient(firebaseUrl, firebaseAuth));
            _userContext = new FirebaseDataContext<User>("Users", firebaseQuery);
            _auctionContext = new FirebaseDataContext<Auction>("Auction", firebaseQuery);
        }

        [Authorize]
        public async Task<ActionResult> Dashboard()
        {
            ViewBag.Message = "The dashboard page.";

            var key = ((ClaimsIdentity)User.Identity).Claims
                .Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(c => c.Value)
                .First();

            var admin = await _userContext.GetByKeyAsync(key);

            return View(new FirebaseObject<User>(key, admin));
        }

        [Authorize]
        public ActionResult Bio(string key)
        {
            if (string.IsNullOrEmpty(key)) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.Key = key;
            ViewBag.Message = "The bio page.";

            return View();
        }

        [Authorize]
        public ActionResult Roles()
        {
            ViewBag.Message = "The roles page.";

            return View((User as ClaimsPrincipal)?.Claims);
        }

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Details(string key)
        {
            if (string.IsNullOrEmpty(key)) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

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
            if (string.IsNullOrEmpty(key)) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.Key = key;

            return View();
        }

        [Authorize]
        public ActionResult Remove(string key)
        {
            if (string.IsNullOrEmpty(key)) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.Key = key;

            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> All()
        {
            var results = await _auctionContext.GetAllAsync();
            return this.Ok(JsonConvert.SerializeObject(results));
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Get(string key)
        {
            var result = await _auctionContext.GetByKeyAsync(key);
            return null != result ? this.Ok(JsonConvert.SerializeObject(result)) : this.NotFound($"{key} not found!");
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Post(Auction auction)
        {
            var result = await _auctionContext.PostAsync(auction);
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

            var result = await _auctionContext.PutAsync(auction, key);
            return null != result
                ? this.Ok(JsonConvert.SerializeObject(new FirebaseObject<Auction>(key, auction)))
                : this.InternalServerError(string.Empty);
        }

        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> Delete(string key, Auction auction)
        {
            await _auctionContext.DeleteAsync(key);
            return this.Ok(JsonConvert.SerializeObject(new FirebaseObject<Auction>(key, auction)));
        }
    }
}