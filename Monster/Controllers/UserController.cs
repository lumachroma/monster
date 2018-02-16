using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;
using FirebaseRest;
using FirebaseRest.Models;
using Monster.Authorizations;
using Monster.Extensions;
using Monster.Firebase;
using Monster.Models;

namespace Monster.Controllers
{
    public class UserController : Controller
    {
        private readonly FirebaseDataContext<User> _userContext;

        public UserController()
        {
            var firebaseUrl = ConfigurationManager.AppSettings["FirebaseUrl"] ?? "https://swag-monster.firebaseio.com";
            var firebaseAuth = ConfigurationManager.AppSettings["FirebaseAuth"] ?? "MfX7DaAWOUjr0zJ2invYbaX6UceHZ3vrif0VGeL4";
            var firebaseQuery = new FirebaseQuery(new FirebaseClient(firebaseUrl, firebaseAuth));
            _userContext = new FirebaseDataContext<User>("Users", firebaseQuery);
        }

        [AuthorizeAttributeWebMvc]
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeAttributeWebMvc]
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
            var results = await _userContext.GetAllAsync();
            return this.Ok(results, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Get(string key)
        {
            var result = await _userContext.GetByKeyAsync(key);
            return null != result
                ? this.Ok(result, JsonRequestBehavior.AllowGet)
                : this.NotFound(new { }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Post(User user)
        {
            var usernameExist = await _userContext.SearchByKeyValueAsync("\"Username\"", $"\"{user.Username}\"");
            if (usernameExist.Any()) return this.BadRequest($"{user.Username} already exist!");

            var emailExist = await _userContext.SearchByKeyValueAsync("\"Email\"", $"\"{user.Email}\"");
            if (emailExist.Any()) return this.BadRequest($"{user.Email} already exist!");

            var hash = Crypto.HashPassword(user.Password);
            user.Password = hash;
            var result = await _userContext.PostAsync(user);
            return null != result ? this.Ok(result) : this.InternalServerError(new { });
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult> Put(string key, User user)
        {
            var emailExist = await _userContext.SearchByKeyValueAsync("\"Email\"", $"\"{user.Email}\"");
            if (emailExist.Any()) return this.BadRequest($"{user.Email} already exist!");

            var result = await _userContext.PutAsync(user, key);
            return null != result ? this.Ok(new FirebaseObject<User>(key, user)) : this.InternalServerError(new { });
        }

        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> Delete(string key, User user)
        {
            await _userContext.DeleteAsync(key);
            return this.Ok(new FirebaseObject<User>(key, user));
        }
    }
}