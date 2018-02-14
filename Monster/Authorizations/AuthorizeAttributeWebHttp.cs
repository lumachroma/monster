using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Monster.Authorizations
{
    public class AuthorizeAttributeWebHttp : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (IsAuthorized(actionContext))
                base.OnAuthorization(actionContext);
            else
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
        }
    }
}