using System.Collections.Generic;

namespace GithubExtension.Security.DAL.Entities
{
    public class UserRepositoryRole
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public int SecurityRoleId { get; set; }

        public virtual User User { get; set; }
        public virtual Repository Repository { get; set; }
        public virtual SecurityRole SecurityRole { get; set; }
    }
}
