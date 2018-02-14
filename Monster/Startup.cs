using System;
using System.Configuration;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Monster.Authorizations;
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

            var provider = new MonsterOAuthAuthorizationServerProvider();
            var options = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = provider
            };
            app.UseOAuthAuthorizationServer(options);

            app.UseCors(CorsOptions.AllowAll);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}