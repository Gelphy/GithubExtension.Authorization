using GithubExtension.Security.DAL.Entities;
using GithubExtension.Security.WebApi.Models;

namespace GithubExtension.Security.WebApi.Converters
{
    public static class RepositoryConverter
    {
        public static Repository ToEntity(this RepositoryDto repository)
        {
            var repositoryEntity = new Repository()
            {
               Id = repository.Id,
               Name =  repository.Name,
               Url =  repository.Url
            };

            return repositoryEntity;
        }
    }
}
