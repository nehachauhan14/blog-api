using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using BlogsDataAccess;

namespace BloggerApp.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                using (BloggerAppDBEntities entities = new BloggerAppDBEntities())
                {
                    var user = entities.User_Details.FirstOrDefault(d => (d.UserName == context.UserName && d.PWD == context.Password));
                    if (user != null)
                    {
                        var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                        identity.AddClaim(new Claim("sub", context.UserName));
                        identity.AddClaim(new Claim("role", "user"));

                        context.Validated(identity);
                    }
                    else
                    {
                        context.SetError("invalid_grant", "The user name or password is incorrect.");
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                context.SetError("invalid_grant", ex.Message);
                return;
            }


        }
    }
}