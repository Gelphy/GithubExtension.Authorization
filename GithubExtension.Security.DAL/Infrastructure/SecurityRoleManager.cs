using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using GithubExtension.Security.DAL.Context;


namespace GithubExtension.Security.DAL.Infrastructure
{
    public class SecurityRoleManager : RoleManager<IdentityRole>
    {
        //public SecurityRoleManager(RoleStore<Role> store)
        //    : base(store)
        //{
        //}

        // Check why we need IRoleStore<Role, string>
        public SecurityRoleManager(IRoleStore<IdentityRole, string> store)
            : base(store)
        {
        }

        public static SecurityRoleManager Create(
            IdentityFactoryOptions<SecurityRoleManager> options,
        IOwinContext context)
        {
            return new SecurityRoleManager(new
            RoleStore<IdentityRole>(context.Get<SecurityContext>()));
        }
    }
}
