using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FirebaseRest;
using FirebaseRest.Models;
using Microsoft.Owin.Security.OAuth;
using Monster.Firebase;
using Monster.Models;

namespace Monster.Authorizations
{
    public class MonsterOAuthAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly FirebaseDataContext<User> _userModelContext;

        public MonsterOAuthAuthorizationServerProvider()
        {
            var firebaseUrl = ConfigurationManager.AppSettings["FirebaseUrl"] ?? "https://swag-monster.firebaseio.com";
            var firebaseAuth = ConfigurationManager.AppSettings["FirebaseAuth"] ??
                               "MfX7DaAWOUjr0zJ2invYbaX6UceHZ3vrif0VGeL4";
            var firebaseQuery = new FirebaseQuery(new FirebaseClient(firebaseUrl, firebaseAuth));
            _userModelContext = new FirebaseDataContext<User>("Users", firebaseQuery);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            if (context.ClientId == null) context.Validated();

            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var users = await _userModelContext.SearchByKeyValueAsync("\"Username\"", $"\"{context.UserName}\"");
            if (users.Any())
            {
                var user = users.First().Object;
                if (context.Password == user.Password) //TODO: refactor
                {
                    var identity = new ClaimsIdentity(context.Options.AuthenticationType);

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
                    context.Validated(identity);
                }
                else
                {
                    context.SetError("invalid_grant", "Provided username and password is incorrect");
                }
            }
            else
            {
                context.SetError("invalid_grant", "Provided username and password is incorrect");
            }
        }
    }
}