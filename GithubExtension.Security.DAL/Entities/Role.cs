using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubExtension.Security.DAL.Entities
{
    public class Role : IdentityRole
    {
       //public int Id { get; set; }
       //public string Name { get; set; }

       public ICollection<UserProject> UserProjects { get; set; }

       public Role()
       {
           UserProjects = new HashSet<UserProject>();
       }
    }
}
