using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.Google;
using Owin;
using System;
using IdentityTemplate.Models;

namespace IdentityTemplate
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            app.UseFacebookAuthentication(
                appId: "625211800923183",
                appSecret: "9ff0c0bb93556fb3c54c3c0261193dab");


            var googleOauth2AuthenticationOptions = new GoogleOAuth2AuthenticationOptions
            {
                ClientId = "977006636134-9dsmh83ajg9ageouq6s7pr80im7koenb.apps.googleusercontent.com",
                ClientSecret = "W2HeYWbzNfH3GyWUMYEjmz1l",
                Provider = new GoogleOAuth2AuthenticationProvider()
                {
                    OnAuthenticated = async context =>
                    {
                        context.Identity.AddClaim(new Claim("picture", context.User.GetValue("picture").ToString()));
                        context.Identity.AddClaim(new Claim("profile", context.User.GetValue("profile").ToString()));

                    }
                }
            };

            googleOauth2AuthenticationOptions.Scope.Add("email");

            app.UseGoogleAuthentication(googleOauth2AuthenticationOptions);

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "977006636134-9dsmh83ajg9ageouq6s7pr80im7koenb.apps.googleusercontent.com",
            //    ClientSecret = "W2HeYWbzNfH3GyWUMYEjmz1l"
            //});
        }
    }
}