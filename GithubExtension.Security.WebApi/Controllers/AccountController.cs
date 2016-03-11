using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using GithubExtension.Security.DAL.Context;
using GithubExtension.Security.DAL.Entities;
using GithubExtension.Security.DAL.Infrastructure;
using GithubExtension.Security.WebApi.Models;
using GithubExtension.Security.WebApi.Services;
using GithubExtension.Security.WebApi.Converters;
using Microsoft.AspNet.Identity.Owin;


namespace GithubExtension.Security.WebApi.Controllers
{
    [RoutePrefix("api/accounts")]
    public class AccountsController : BaseApiController
    {
        private IGithubService _githubService;

        private SecurityContext Context
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Get<SecurityContext>();
            }
        }

        public AccountsController()
        {
            //Context = new SecurityContext();

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
            List<RepositoryDto> repos = await _githubService.GetReposAsync(userDto.Login, token);

            // TODO: ceck exists
            var role = await Context.SecurityRoles.FirstOrDefaultAsync(r => r.Name == "Admin");
            var repositoriesToAdd = repos.Select(r => new UserRepositoryRole() { Repository = r.ToEntity(), SecurityRole = role}).ToList();
            
            //TODO: Use converter
            var user = new User()
            {
                Email = userDto.Email,
                UserName = userDto.Login,
                Token = token,
                ProviderId = userDto.GitHubId,
                UserRepositoryRoles = repositoriesToAdd
            };

            IdentityResult addUserResult = await ApplicationUserManager.CreateAsync(user);
            if (!addUserResult.Succeeded)
            {
                return GetErrorResult(addUserResult);
            }

            
            // Change identity user to app user
            Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));
            return Created(locationHeader, TheModelFactory.Create(user));
        }
    }
}