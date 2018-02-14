using System.Web.Mvc;

namespace Monster.Authorizations
{
    public class AuthorizeAttributeWebMvc : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (AuthorizeCore(filterContext.HttpContext))
                base.OnAuthorization(filterContext);
            else
                filterContext.Result = new RedirectResult("/Home/Unauthorized");
        }
    }
}