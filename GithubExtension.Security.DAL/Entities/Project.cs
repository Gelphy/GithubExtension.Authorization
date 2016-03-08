using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubExtension.Security.DAL.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        //public virtual ICollection<UserProject> UserProjects { get; set; }
        public virtual ICollection<Claim> Claims { get; set; }

        public Project()
        {
            Claims = new HashSet<Claim>();
        }
    }
}
