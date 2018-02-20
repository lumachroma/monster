using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using Monster.Authorizations;

namespace Monster.Api
{
    [RoutePrefix("api/user-identity")]
    public class UserIdentityController : ApiController
    {
        [AuthorizeAttributeWebHttp]
        [Route("name")]
        public string GetUserName()
        {
            return HttpContext.Current.User.Identity.Name;
        }

        [AuthorizeAttributeWebHttp]
        [Route("email")]
        public string GetUserEmail()
        {
            var email = ((ClaimsIdentity)HttpContext.Current.User.Identity).Claims
                .Where(c => c.Type == ClaimTypes.Email)
                .Select(c => c.Value)
                .First();
            return email;
        }

        [AuthorizeAttributeWebHttp]
        [Route("roles")]
        public IEnumerable<string> GetRoles()
        {
            var roles = ((ClaimsIdentity)HttpContext.Current.User.Identity).Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value);
            return roles;
        }
    }
}