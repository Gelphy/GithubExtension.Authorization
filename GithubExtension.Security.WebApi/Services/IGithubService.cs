using System.Collections.Generic;
using System.Threading.Tasks;
using GithubExtension.Security.WebApi.Models;

namespace GithubExtension.Security.WebApi.Services
{
    public interface IGithubService
    {
        Task<UserDto> GetUserAsync(string token);
        Task<List<RepositoryDto>> GetReposAsync(string userName, string token);
        Task<string> GetPrimaryEmailForUser(string token);
        Task<List<CollaboratorDto>> GetCollaboratorsForRepo(string owner, string repository, string token);
    }
}