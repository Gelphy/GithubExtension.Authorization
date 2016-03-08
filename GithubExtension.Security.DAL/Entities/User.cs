using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GithubExtension.Security.DAL.Infrastructure;
using Microsoft.AspNet.Identity;



namespace GithubExtension.Security.DAL.Entities
{
    public class User : IdentityUser
    {
        //public int Id { get; set; }
        public string Token { get; set; }
       // public string Login { get; set; }
       // public string Email { get; set; }

        public virtual ICollection<UserProject> UserProjects { get; set; }
        public override ICollection<IdentityUserClaim> Claims
        {
            get
            {
                return base.Claims;
            }
        }
        //public virtual ICollection<Claim>

        public User()
        {
            UserProjects = new HashSet<UserProject>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
