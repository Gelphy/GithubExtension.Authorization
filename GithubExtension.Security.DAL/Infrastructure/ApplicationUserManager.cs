using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
