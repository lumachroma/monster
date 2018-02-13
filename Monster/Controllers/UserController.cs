using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using FirebaseRest;
using FirebaseRest.Models;
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

        public ActionResult Index()
        {
            return View();
        }

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

        [HttpGet]
        public async Task<ActionResult> All()
        {
            var results = await _userContext.GetAllAsync();

            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Get(string key)
        {
            var result = await _userContext.GetByKeyAsync(key);

            if (null != result)
                Response.StatusCode = (int)HttpStatusCode.OK;
            else
                Response.StatusCode = (int)HttpStatusCode.NotFound;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Post(User user)
        {
            var result = await _userContext.PostAsync(user);

            if (null != result)
                Response.StatusCode = (int)HttpStatusCode.OK;
            else
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return Json(result);
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult> Put(string key, User user)
        {
            var result = await _userContext.PutAsync(user, key);

            if (null != result)
                Response.StatusCode = (int)HttpStatusCode.OK;
            else
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return Json(new FirebaseObject<User>(key, user));
        }

        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> Delete(string key, User user)
        {
            await _userContext.DeleteAsync(key);
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(new FirebaseObject<User>(key, user));
        }
    }
}