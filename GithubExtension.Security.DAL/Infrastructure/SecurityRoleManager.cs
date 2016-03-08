using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using GithubExtension.Security.DAL.Entities;
using GithubExtension.Security.DAL.Context;


namespace GithubExtension.Security.DAL.Infrastructure
{
    public class SecurityRoleManager : RoleManager<Role>, IDisposable
    {
        public SecurityRoleManager(RoleStore<Role> store)
            : base(store)
        {
        }

        // Check why we need IRoleStore<Role, string>
        public SecurityRoleManager(IRoleStore<Role, string> store)
            : base(store)
        {
        }

        public static SecurityRoleManager Create(
        IdentityFactoryOptions<SecurityRoleManager> options,
        IOwinContext context)
        {
            return new SecurityRoleManager(new
            RoleStore<Role>(context.Get<SecurityContext>()));
        }
    }
}
