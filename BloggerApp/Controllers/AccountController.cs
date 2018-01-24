using BlogsDataAccess;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using static BloggerApp.Models.ExternalLoginModels;

namespace BloggerApp.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public HttpResponseMessage RegisterUser([FromBody] RegisterExternalBindingModel newUser)
        {
            try
            {
                using (  BloggerAppDBEntities entities = new BloggerAppDBEntities())
                {
                    var id_string = Convert.ToString(newUser.id);
                    int UID = new int();
                    var alreadyRegistered = entities.Oauth_info.FirstOrDefault(d => d.oAuthID == id_string);
                    if (alreadyRegistered == null)
                    {
                        var userToAdd = new User_Details();
                        var authInfo = new Oauth_info();

                        userToAdd.UserName = newUser.UserName;
                        userToAdd.EMAIL = newUser.email;
                        userToAdd.PWD = "0";
                        entities.User_Details.Add(userToAdd);
                        entities.SaveChanges();

                        var externalUser = entities.User_Details.FirstOrDefault(d => d.UserName == newUser.UserName);
                        UID = externalUser.UID;

                        authInfo.oAuthID = Convert.ToString(newUser.id);
                        authInfo.UID = UID;

                        entities.Oauth_info.Add(authInfo);
                        entities.SaveChanges();

                    }
                    else
                    {
                        var externalUser = entities.User_Details.FirstOrDefault(d => d.UserName == newUser.UserName);
                        UID = externalUser.UID;

                    }

                    JObject token = GenerateLocalAccessTokenResponse(newUser.UserName, UID); 

                    var message = Request.CreateResponse(HttpStatusCode.Created, token);
                    message.Headers.Location = new Uri(Request.RequestUri + newUser.UserName.ToString());
                    return message;

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        private JObject GenerateLocalAccessTokenResponse(string userName , int UID)
        {

            var tokenExpiration = TimeSpan.FromDays(1);
            var uid = Convert.ToString(UID);
            ClaimsIdentity identity = new ClaimsIdentity(OAuthDefaults.AuthenticationType);

            identity.AddClaim(new Claim("sub", userName));
            identity.AddClaim(new Claim("uid", uid));

            identity.AddClaim(new Claim(ClaimTypes.Name, userName));
            identity.AddClaim(new Claim("role", "user"));

            var props = new AuthenticationProperties()
            {
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.Add(tokenExpiration),
            };

            var ticket = new AuthenticationTicket(identity, props);

            var accessToken = Startup.OAuthBearerOptions.AccessTokenFormat.Protect(ticket);

            JObject tokenResponse = new JObject(
                                        new JProperty("userName", userName),
                                        new JProperty("access_token", accessToken),
                                        new JProperty("token_type", "bearer"),
                                        new JProperty("expires_in", tokenExpiration.TotalSeconds.ToString()),
                                        new JProperty(".issued", ticket.Properties.IssuedUtc.ToString()),
                                        new JProperty(".expires", ticket.Properties.ExpiresUtc.ToString())
        );

            return tokenResponse;
        }

    }
}
