using System.Security.Claims;
using System.Web.Mvc;

namespace Monster.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize]
        public ActionResult Dashboard()
        {
            ViewBag.Message = "The dashboard page.";

            return View();
        }

        [Authorize]
        public ActionResult Settings()
        {
            ViewBag.Message = "The settings page.";

            return View((User as ClaimsPrincipal)?.Claims);
        }

        public ActionResult Unauthorized()
        {
            ViewBag.Message = "Unauthorized";

            return View();
        }
    }
}