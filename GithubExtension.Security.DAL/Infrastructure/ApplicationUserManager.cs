using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using GithubExtension.Security.DAL.Entities;
using GithubExtension.Security.DAL.Context;

namespace GithubExtension.Security.DAL.Infrastructure
{
    public class ApplicationUserManager : UserManager<User>
    {
        public ApplicationUserManager(IUserStore<User> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            SecurityContext db = context.Get<SecurityContext>();
            ApplicationUserManager manager = new ApplicationUserManager(new UserStore<User>(db));

            return manager;
        }
    }
}
