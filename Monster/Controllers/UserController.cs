using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;
using FirebaseRest;
using FirebaseRest.Models;
using Monster.Extensions;
using Monster.Firebase;
using Monster.Models;
using Newtonsoft.Json;

namespace Monster.Controllers
{
    public class UserController : Controller
    {
        private readonly FirebaseDataContext<User> _userContext;

        public UserController()
        {
            var firebaseUrl = ConfigurationManager.AppSettings["FirebaseUrl"] ?? "https://swag-monster.firebaseio.com";
            var firebaseAuth = ConfigurationManager.AppSettings["FirebaseAuth"] ??
                               "MfX7DaAWOUjr0zJ2invYbaX6UceHZ3vrif0VGeL4";
            var firebaseQuery = new FirebaseQuery(new FirebaseClient(firebaseUrl, firebaseAuth));
            _userContext = new FirebaseDataContext<User>("Users", firebaseQuery);
        }

        [Authorize]
        public ActionResult Index()
        {
            if (!IsAdministrator()) return RedirectToAction("Unauthorized", "Home");
            return View();
        }

        [Authorize]
        public ActionResult Details(string key)
        {
            if (string.IsNullOrEmpty(key)) return this.BadRequest(string.Empty);
            if (!IsAdministrator()) return RedirectToAction("Unauthorized", "Home");

            ViewBag.Key = key;

            return View();
        }

        [Authorize]
        public ActionResult Create()
        {
            if (!IsAdministrator()) return RedirectToAction("Unauthorized", "Home");
            return View();
        }

        [Authorize]
        public ActionResult Edit(string key)
        {
            if (string.IsNullOrEmpty(key)) return this.BadRequest(string.Empty);
            if (!IsAdministrator()) return RedirectToAction("Unauthorized", "Home");

            ViewBag.Key = key;

            return View();
        }

        [Authorize]
        public ActionResult Remove(string key)
        {
            if (string.IsNullOrEmpty(key)) return this.BadRequest(string.Empty);
            if (!IsAdministrator()) return RedirectToAction("Unauthorized", "Home");

            ViewBag.Key = key;

            return View();
        }

        [Authorize]
        public ActionResult Password(string key)
        {
            if (string.IsNullOrEmpty(key)) return this.BadRequest(string.Empty);
            if (!IsAdministrator()) return RedirectToAction("Unauthorized", "Home");

            ViewBag.Key = key;

            return View();
        }

        [Authorize]
        public ActionResult Roles()
        {
            ViewBag.Message = "The roles page.";

            return View((User as ClaimsPrincipal)?.Claims);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> All()
        {
            var results = await _userContext.GetAllAsync();
            return this.Ok(JsonConvert.SerializeObject(results));
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Get(string key)
        {
            var result = await _userContext.GetByKeyAsync(key);
            return null != result ? this.Ok(JsonConvert.SerializeObject(result)) : this.NotFound($"{key} not found!");
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
            return null != result
                ? this.Ok(JsonConvert.SerializeObject(result))
                : this.InternalServerError(string.Empty);
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult> Put(string key, User user)
        {
            var existing = await _userContext.GetByKeyAsync(key);
            if (null == existing) return this.NotFound($"{key} not found!");

            if (user.Email != existing.Email)
            {
                var emailExist = await _userContext.SearchByKeyValueAsync("\"Email\"", $"\"{user.Email}\"");
                if (emailExist.Any()) return this.BadRequest($"{user.Email} already exist!");
            }

            user.Username = existing.Username;
            user.Password = existing.Password;
            var result = await _userContext.PutAsync(user, key);
            return null != result
                ? this.Ok(JsonConvert.SerializeObject(new FirebaseObject<User>(key, user)))
                : this.InternalServerError(string.Empty);
        }

        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> Delete(string key, User user)
        {
            await _userContext.DeleteAsync(key);
            return this.Ok(JsonConvert.SerializeObject(new FirebaseObject<User>(key, user)));
        }

        [Authorize]
        [HttpPatch]
        public async Task<ActionResult> SetPassword(string key, string password)
        {
            var initial = await _userContext.GetByKeyAsync(key);
            if (null == initial) return this.NotFound($"{key} not found!");

            dynamic user = new ExpandoObject();
            user.Password = Crypto.HashPassword(password);
            var result = await _userContext.PatchAsync(user, key);
            string json = JsonConvert.SerializeObject(result);
            return null != result ? this.Ok(json) : this.InternalServerError(string.Empty);
        }

        private bool IsAdministrator()
        {
            return HttpContext.User.IsInRole("Administrator") ||
                   HttpContext.User.IsInRole("administrator") ||
                   HttpContext.User.IsInRole("Admin") ||
                   HttpContext.User.IsInRole("admin");
        }
    }
}