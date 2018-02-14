using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FirebaseRest;
using FirebaseRest.Models;
using Monster.Firebase;
using Monster.Models;

namespace Monster.Controllers
{
    public class AccountController : Controller
    {
        private readonly string _applicationName;
        private readonly FirebaseDataContext<User> _userModelContext;

        public AccountController()
        {
            _applicationName = ConfigurationManager.AppSettings["ApplicationName"] ?? "Monster";

            var firebaseUrl = ConfigurationManager.AppSettings["FirebaseUrl"] ?? "https://swag-monster.firebaseio.com";
            var firebaseAuth = ConfigurationManager.AppSettings["FirebaseAuth"] ??
                               "MfX7DaAWOUjr0zJ2invYbaX6UceHZ3vrif0VGeL4";
            var firebaseQuery = new FirebaseQuery(new FirebaseClient(firebaseUrl, firebaseAuth));
            _userModelContext = new FirebaseDataContext<User>("Users", firebaseQuery);
        }

        [HttpGet]
        public ActionResult Logoff()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            return Redirect("/");
        }

        [HttpGet]
        public ActionResult Login(bool success = true, string status = "OK", string returnUrl = null)
        {
            ViewBag.Title = "Login";

            ViewBag.Success = success;
            ViewBag.Status = status;
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if (string.IsNullOrEmpty(model.Username))
                return RedirectToAction("Login", "Account",
                    new { success = false, status = "Username cannot be set to null or empty." });
            if (string.IsNullOrEmpty(model.Password))
                return RedirectToAction("Login", "Account",
                    new { success = false, status = "Password cannot be set to null or empty." });

            var users = await _userModelContext.SearchByKeyValueAsync("\"Username\"", $"\"{model.Username}\"");
            if (users.Any())
            {
                var user = users.First().Object;
                if (model.Password == user.Password) //TODO: refactor
                {
                    var identity = new ClaimsIdentity($"{_applicationName}Cookie");

                    identity.AddClaim(new Claim(ClaimTypes.Name, user.Username));
                    identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
                    identity.AddClaim(new Claim(ClaimTypes.StateOrProvince, user.Location));
                    identity.AddClaim(new Claim(ClaimTypes.Country, user.Country));

                    var roles = new List<Claim>();
                    foreach (var role in user.Roles)
                    {
                        var claim = new Claim(ClaimTypes.Role, role);
                        roles.Add(claim);
                    }

                    identity.AddClaims(roles);

                    HttpContext.GetOwinContext().Authentication.SignIn(identity);

                    if (Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
                    return RedirectToAction("Dashboard", "Home");
                }
            }

            return RedirectToAction("Login", "Account", new { success = false, status = "Wrong username or password." });
        }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}