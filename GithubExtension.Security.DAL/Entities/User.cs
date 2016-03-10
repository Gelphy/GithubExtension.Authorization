﻿using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace GithubExtension.Security.DAL.Entities
{
    public class User : IdentityUser
    {
        public string Token { get; set; }
        public int ProviderId { get; set; }

        public virtual ICollection<Repository> Repositories { get; set; } 

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
