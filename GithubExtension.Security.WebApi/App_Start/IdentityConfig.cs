using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using GithubExtension.Security.DAL;
using GithubExtension.Security.DAL.Context;
using GithubExtension.Security.DAL.Entities;
using GithubExtension.Security.DAL.Infrastructure;

namespace GithubExtension.Security.WebApi
{
    public class IdentityConfig
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<SecurityContext>(SecurityContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<SecurityRoleManager>(SecurityRoleManager.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                // Manage login path
                LoginPath = new PathString("/Account/Login"),
            });
        } 

    }
}