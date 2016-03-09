using GithubExtension.Security.DAL.Context;
using GithubExtension.Security.DAL.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GithubExtension.Security.DAL.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GithubExtension.Security.DAL.Context.SecurityContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SecurityContext context)
        {
            //  This method will be called after migrating to the latest version.

            var manager = new UserManager<User>(new UserStore<User>(new SecurityContext()));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new SecurityContext()));

            var user = new User()
            {
                UserName = "senioroman4uk",
                Email = "senioroman4uk@gmail.com",
                EmailConfirmed = true,
                Token = "72e3a8248d1917f18ee923395384e6ac2d0ea207"

            };

            manager.Create(user, "MySuperP@ss!");

            if (!roleManager.Roles.Any())
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
                roleManager.Create(new IdentityRole { Name = "Developer" });
                roleManager.Create(new IdentityRole { Name = "Reviewer" });
            }

            var adminUser = manager.FindByName("senioroman4uk");

            manager.AddToRoles(adminUser.Id, "Admin");
        }
    }
}
