using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubExtension.Security.DAL.Entities
{
    public class Claim : IdentityUserClaim
    {
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public virtual ICollection<Collaborator> Collaborators { get; set; }

        public Claim()
        {
            Collaborators = new HashSet<Collaborator>();
        }
    }
}
