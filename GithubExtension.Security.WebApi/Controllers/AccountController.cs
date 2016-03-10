using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using GithubExtension.Security.DAL.Context;
using GithubExtension.Security.DAL.Entities;
using GithubExtension.Security.WebApi.Models;
using GithubExtension.Security.WebApi.Services;
using GithubExtension.Security.WebApi.Converters;

namespace GithubExtension.Security.WebApi.Controllers
{
    [RoutePrefix("api/accounts")]
    public class AccountsController : BaseApiController
    {
        private IGithubService _githubService;
        private SecurityContext context;

        public AccountsController()
        {
            context = new SecurityContext();

            _githubService = new GithubService();
        }

        [Authorize(Roles = "Admin")]
        [Route("users")]
        public IHttpActionResult GetUsers()
        {
            return Ok(ApplicationUserManager.Users.ToList().Select(u => TheModelFactory.Create(u)));
        }

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}", Name = "GetUserById")]
        public async Task<IHttpActionResult> GetUser(string id)
        {
            var user = await ApplicationUserManager.FindByIdAsync(id);

            if (user != null)
            {
                return Ok(TheModelFactory.Create(user));
            }

            return NotFound();
        }

        [Authorize(Roles = "Admin")]
        [Route("user/{username}")]
        public async Task<IHttpActionResult> GetUserByName(string username)
        {
            var user = await ApplicationUserManager.FindByNameAsync(username);

            if (user != null)
            {
                return Ok(TheModelFactory.Create(user));
            }

            return NotFound();
        }

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}/roles")]
        [HttpPut]
        public async Task<IHttpActionResult> AssignRolesToUser([FromUri] string id, [FromBody] string[] rolesToAssign)
        {

            var appUser = await ApplicationUserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            var currentRoles = await ApplicationUserManager.GetRolesAsync(appUser.Id);

            var rolesNotExists = rolesToAssign.Except(SecurityRoleManager.Roles.Select(x => x.Name)).ToArray();

            if (rolesNotExists.Any())
            {

                ModelState.AddModelError("", string.Format("Roles '{0}' does not exixts in the system", string.Join(",", rolesNotExists)));
                return BadRequest(ModelState);
            }

            IdentityResult removeResult = await ApplicationUserManager.RemoveFromRolesAsync(appUser.Id, currentRoles.ToArray());

            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to remove user roles");
                return BadRequest(ModelState);
            }

            IdentityResult addResult = await ApplicationUserManager.AddToRolesAsync(appUser.Id, rolesToAssign);

            if (!addResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to add user roles");
                return BadRequest(ModelState);
            }

            return Ok();
        }

        [Route("create")]
        [HttpPost]
        //[Authorize]
        public async Task<IHttpActionResult> CreateUser(string token)
        {
            UserDto userDto = await _githubService.GetUserAsync(token);


            //TODO: Use converter
            var user = new User()
            {
                //TODO: Get Primary Email from GitHub
                Email = "",
                UserName = userDto.Login,
                Token = token,
                ProviderId = userDto.GitHubId,
                
            };

            IdentityResult addUserResult = await ApplicationUserManager.CreateAsync(user);
            if (!addUserResult.Succeeded)
            {
                return GetErrorResult(addUserResult);
            }

            //Geting repos for user
            List<RepositoryDto> repos = await _githubService.GetReposAsync(user.UserName, token);
            var repositoriesToAdd = repos.Select(r => r.ToEntity()).ToArray();

            context.Repositories.AddOrUpdate(repositoriesToAdd);
            foreach (var repository in repositoriesToAdd)
            {
                user.Repositories = repositoriesToAdd;
            }
            //ApplicationUserManager.AddClaimAsync()
            
            // Change identity user to app user
            Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));
            return Created(locationHeader, TheModelFactory.Create(user));
        }
    }
}