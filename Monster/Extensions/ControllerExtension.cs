using System.Net;
using System.Web.Mvc;

namespace Monster.Extensions
{
    internal static class ControllerExtension
    {
        public static ActionResult Ok(this Controller cont, string str)
        {
            return CreateContentResult(cont, str, HttpStatusCode.OK);
        }

        public static ActionResult NotFound(this Controller cont, string str)
        {
            return CreateContentResult(cont, str, HttpStatusCode.NotFound);
        }

        public static ActionResult InternalServerError(this Controller cont, string str)
        {
            return CreateContentResult(cont, str, HttpStatusCode.InternalServerError);
        }

        public static ActionResult BadRequest(this Controller cont, string str)
        {
            return CreateContentResult(cont, str, HttpStatusCode.BadRequest);
        }

        public static ActionResult Forbidden(this Controller cont, string str)
        {
            return CreateContentResult(cont, str, HttpStatusCode.Forbidden);
        }

        public static bool IsAdministrator(this Controller controller)
        {
            return controller.HttpContext.User.IsInRole("Administrator") ||
                   controller.HttpContext.User.IsInRole("administrator") ||
                   controller.HttpContext.User.IsInRole("Admin") ||
                   controller.HttpContext.User.IsInRole("admin");
        }

        public static string GetCurrentUser(this Controller controller)
        {
            return controller.HttpContext.User.Identity.Name;
        }

        private static ActionResult CreateContentResult(Controller cont, string str, HttpStatusCode sc)
        {
            cont.Response.StatusCode = (int)sc;
            return new ContentResult { Content = str };
        }
    }
}