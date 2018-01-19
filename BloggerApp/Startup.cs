using BloggerApp.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Builder;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Web.Http;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.Facebook;
using BloggerApp.Facebook;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;

[assembly: OwinStartup(typeof(BloggerApp.Startup))]
namespace BloggerApp
{
    public class Startup
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }
        public static GoogleOAuth2AuthenticationOptions googleAuthOptions { get; private set; }
        public static FacebookAuthenticationOptions facebookAuthOptions { get; private set; }


        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            ConfigureOAuth(app);
            WebApiConfig.Register(config);
            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(config);


        }

        public void ConfigureOAuth(IAppBuilder app)
        {
           app.SetDefaultSignInAsAuthenticationType(DefaultAuthenticationTypes.ExternalCookie);
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new SimpleAuthorizationServerProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            //Configure Google External Login
            googleAuthOptions = new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "145622591194-5saq9er9t7701kqpd6do9ahfe3jsa88k.apps.googleusercontent.com",
                ClientSecret = "oHJ6jGlf77bVwq0b_pXHhNMJ",
                Provider = new GoogleAuthProvider()
            };
            app.UseGoogleAuthentication(googleAuthOptions);

            //Configure Facebook External Login
            facebookAuthOptions = new FacebookAuthenticationOptions()
            {
                AppId = "1556537601049909",
                AppSecret = "c37b4482f4703cb5e1eb47aec15d2d2f",
                Provider = new FacebookAuthProvider()
            };
            app.UseFacebookAuthentication(facebookAuthOptions);


            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "145622591194-5saq9er9t7701kqpd6do9ahfe3jsa88k.apps.googleusercontent.com",
            //    ClientSecret = "oHJ6jGlf77bVwq0b_pXHhNMJ"
            //});

        }

        //var facebookOptions = new FacebookAuthenticationOptions()
        //{
        //    AppId = "160811734413146",
        //    AppSecret = "21f2665e0aed11867fcd8d35e67d6068",
        //    BackchannelHttpHandler = new FacebookBackChannelHandler(),
        //    UserInformationEndpoint = "https://graph.facebook.com/v2.4/me?fi..."
        //};

        //facebookOptions.Scope.Add("email");

        //app.UseFacebookAuthentication(facebookOptions);


    }

}