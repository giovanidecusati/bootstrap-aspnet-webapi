using System;
using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Web.Http;

namespace Northwind.WebApi.Providers
{
    internal class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        HttpConfiguration _config;

        public AuthorizationServerProvider(HttpConfiguration config)
        {
            _config = config;
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            using (_config.DependencyResolver.BeginScope())
            {
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new string[] { });

                // IEnumerable<DomainNotification> notifications;
                // AuthService authService = (AuthService)_config.DependencyResolver.GetService(typeof(AuthService));                
                // var login = authService.SingIn(new SignInCommand(context.UserName, context.Password), out notifications);
                var login = new { Id = 0, Email = "" };

                List<string> notifications = new List<string>();

                if (login == null)
                {
                    context.SetError("invalid_grant", JsonConvert.SerializeObject(notifications));
                }
                else
                {
                    var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                    identity.AddClaim(new Claim(ClaimTypes.Sid, login.Id.ToString()));
                    identity.AddClaim(new Claim(ClaimTypes.Name, login.Email));
                    identity.AddClaim(new Claim(ClaimTypes.GivenName, context.UserName));

                    Thread.CurrentPrincipal = new GenericPrincipal(identity, new string[] { "Administrators" });
                    context.Validated(identity);
                }
            }
        }
    }
}