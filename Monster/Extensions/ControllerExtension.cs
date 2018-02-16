using System.Net;
using System.Web.Mvc;

namespace Monster.Extensions
{
    internal static class ControllerExtension
    {
        public static ActionResult Ok(this Controller cont, object obj,
            JsonRequestBehavior rb = JsonRequestBehavior.DenyGet)
        {
            return JsonResult(cont, obj, HttpStatusCode.OK, rb);
        }

        public static ActionResult NotFound(this Controller cont, object obj,
            JsonRequestBehavior rb = JsonRequestBehavior.DenyGet)
        {
            return JsonResult(cont, obj, HttpStatusCode.NotFound, rb);
        }

        public static ActionResult InternalServerError(this Controller cont, object obj,
            JsonRequestBehavior rb = JsonRequestBehavior.DenyGet)
        {
            return JsonResult(cont, obj, HttpStatusCode.InternalServerError, rb);
        }

        public static ActionResult BadRequest(this Controller cont, object obj,
            JsonRequestBehavior rb = JsonRequestBehavior.DenyGet)
        {
            return JsonResult(cont, obj, HttpStatusCode.BadRequest, rb);
        }

        private static ActionResult JsonResult(Controller cont, object obj, HttpStatusCode sc, JsonRequestBehavior rb)
        {
            cont.Response.StatusCode = (int)sc;
            return rb == JsonRequestBehavior.AllowGet
                ? new JsonResult { Data = obj, JsonRequestBehavior = JsonRequestBehavior.AllowGet }
                : new JsonResult { Data = obj };
        }

        public static ActionResult Ok(this Controller cont, string str)
        {
            return ContentResult(cont, str, HttpStatusCode.OK);
        }

        public static ActionResult NotFound(this Controller cont, string str)
        {
            return ContentResult(cont, str, HttpStatusCode.NotFound);
        }

        public static ActionResult InternalServerError(this Controller cont, string str)
        {
            return ContentResult(cont, str, HttpStatusCode.InternalServerError);
        }

        public static ActionResult BadRequest(this Controller cont, string str)
        {
            return ContentResult(cont, str, HttpStatusCode.BadRequest);
        }

        private static ActionResult ContentResult(Controller cont, string str, HttpStatusCode sc)
        {
            cont.Response.StatusCode = (int)sc;
            return new ContentResult { Content = str };
        }
    }
}