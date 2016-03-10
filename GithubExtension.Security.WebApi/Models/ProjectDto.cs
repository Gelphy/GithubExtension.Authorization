using System.ComponentModel.DataAnnotations.Schema;

namespace GithubExtension.Security.WebApi.Models
{
    [NotMapped]
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}