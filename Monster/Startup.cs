using System.Configuration;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace Monster
{
    public class Startup
    {
        private readonly string _applicationName;

        public Startup()
        {
            _applicationName = ConfigurationManager.AppSettings["ApplicationName"] ?? "Monster";
        }

        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = $"{_applicationName}Cookie",
                LoginPath = new PathString("/Account/Login"),
                CookieName = $".{_applicationName}.Cookie"
            });
        }
    }
}