using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using FirebaseRest;
using FirebaseRest.Models;
using Monster.Firebase;
using Monster.Models;

namespace Monster.Controllers
{
    public class AuctionController : Controller
    {
        private readonly FirebaseDataContext<User> _userModelContext;

        public AuctionController()
        {
            var firebaseUrl = ConfigurationManager.AppSettings["FirebaseUrl"] ?? "https://swag-monster.firebaseio.com";
            var firebaseAuth = ConfigurationManager.AppSettings["FirebaseAuth"] ??
                               "MfX7DaAWOUjr0zJ2invYbaX6UceHZ3vrif0VGeL4";
            var firebaseQuery = new FirebaseQuery(new FirebaseClient(firebaseUrl, firebaseAuth));
            _userModelContext = new FirebaseDataContext<User>("Users", firebaseQuery);
        }

        [Authorize]
        public async Task<ActionResult> Dashboard()
        {
            ViewBag.Message = "The dashboard page.";

            var key = ((ClaimsIdentity)User.Identity).Claims
                .Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(c => c.Value)
                .First();

            var admin = await _userModelContext.GetByKeyAsync(key);

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
    }
}