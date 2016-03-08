using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GithubExtension.Security.DAL.Entities;

namespace GithubExtension.Security.DAL.Context
{
    // TODO: Check internal
    public class SecurityContext : IdentityDbContext<User>
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Claim> Claims { get; set; }

        public SecurityContext()
            : base("GitHubExtension")
        {

        }

        static SecurityContext()
        {
            Database.SetInitializer<SecurityContext>(new SecurityDbInit());
        }

        public static SecurityContext Create()
        {
            return new SecurityContext();
        }
    }

    public class SecurityDbInit : DropCreateDatabaseIfModelChanges<SecurityContext>
    {
        protected override void Seed(SecurityContext context)
        {
            PerformInitialSetup(context);
            base.Seed(context);
        }
        public void PerformInitialSetup(SecurityContext context)
        {
            // initial configuration will go here
        }
    }

   
}
