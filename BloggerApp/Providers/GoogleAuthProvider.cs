using Microsoft.Owin.Security.Google;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Security.Claims;

namespace BloggerApp.Providers
{
    public class GoogleAuthProvider : IGoogleOAuth2AuthenticationProvider
    {
        public void ApplyRedirect(GoogleOAuth2ApplyRedirectContext context)
        {
            context.Response.Redirect(context.RedirectUri);
        }

        public Task Authenticated(GoogleOAuth2AuthenticatedContext context)
        {
            context.Identity.AddClaim(new Claim("ExternalAccessToken", context.AccessToken));
            return Task.FromResult<object>(null);
        }

        public Task ReturnEndpoint(GoogleOAuth2ReturnEndpointContext context)
        {
            return Task.FromResult<object>(null);
        }

        Task IGoogleOAuth2AuthenticationProvider.Authenticated(GoogleOAuth2AuthenticatedContext context)
        {
            throw new NotImplementedException();
        }

        Task IGoogleOAuth2AuthenticationProvider.ReturnEndpoint(GoogleOAuth2ReturnEndpointContext context)
        {
            throw new NotImplementedException();
        }
    }
}