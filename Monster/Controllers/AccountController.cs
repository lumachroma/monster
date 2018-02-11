using System.Collections.Generic;
using System.Configuration;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Monster.Controllers
{
    public class AccountController : Controller
    {
        private readonly string _applicationName;
        private readonly UserModel _defaultUserAdmin;

        public AccountController()
        {
            _applicationName = ConfigurationManager.AppSettings["ApplicationName"] ?? "Monster";
            _defaultUserAdmin = new UserModel
            {
                Username = "lumachroma",
                Password = "123456qwerty!",
                Email = "lumachroma@outlook.com",
                State = "Kuala Lumpur",
                Country = "Malaysia",
                Roles = new List<string> { "Admin", "Developer", "Geek" }
            }; //TODO: refactor
        }

        [HttpGet]
        public ActionResult Logoff()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            return Redirect("/");
        }

        [HttpGet]
        public ActionResult Login(bool success = true, string status = "OK")
        {
            ViewBag.Title = "Login";

            ViewBag.Success = success;
            ViewBag.Status = status;

            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model, string returnUrl = "/")
        {
            if (string.IsNullOrEmpty(model.Username))
                return RedirectToAction("Login", "Account",
                    new { success = false, status = "Username cannot be set to null or empty." });
            if (string.IsNullOrEmpty(model.Password))
                return RedirectToAction("Login", "Account",
                    new { success = false, status = "Password cannot be set to null or empty." });

            if (model.Username.Equals(_defaultUserAdmin.Username) &&
                model.Password.Equals(_defaultUserAdmin.Password)) //TODO: refactor
            {
                var identity = new ClaimsIdentity($"{_applicationName}Cookie");

                identity.AddClaim(new Claim(ClaimTypes.Name, _defaultUserAdmin.Username));
                identity.AddClaim(new Claim(ClaimTypes.Email, _defaultUserAdmin.Email));
                identity.AddClaim(new Claim(ClaimTypes.StateOrProvince, _defaultUserAdmin.State));
                identity.AddClaim(new Claim(ClaimTypes.Country, _defaultUserAdmin.Country));

                var roles = new List<Claim>();
                foreach (var role in _defaultUserAdmin.Roles)
                {
                    var claim = new Claim(ClaimTypes.Role, role);
                    roles.Add(claim);
                }

                identity.AddClaims(roles);

                HttpContext.GetOwinContext().Authentication.SignIn(identity);

                if (returnUrl == "/") return RedirectToAction("Dashboard", "Home");
                return Redirect(returnUrl);
            }

            return RedirectToAction("Login", "Account", new { success = false, status = "Wrong username or password." });
        }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UserModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}