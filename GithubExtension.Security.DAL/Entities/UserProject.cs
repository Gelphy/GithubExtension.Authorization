using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubExtension.Security.DAL.Entities
{
    public class UserProject
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public Guid UserId { get; set; }
        public string Description { get; set; }

        public virtual User User {get; set;}
        public virtual Project Project { get; set; }
        
        public virtual ICollection<Role> Roles { get; set; }

        public UserProject()
        {
            Roles = new HashSet<Role>();
        }
    }
}
