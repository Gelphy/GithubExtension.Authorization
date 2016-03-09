using Microsoft.AspNet.Identity.EntityFramework;

namespace GithubExtension.Security.DAL.Entities
{
    public class Claim : IdentityUserClaim
    {
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }
    }
}
