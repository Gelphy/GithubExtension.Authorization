using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using GithubExtension.Security.DAL.Entities;
using GithubExtension.Security.WebApi.Models;
using Newtonsoft.Json;


namespace GithubExtension.Security.WebApi.Controllers
{
    [RoutePrefix("api/accounts")]
    public class AccountsController : BaseApiController
    {
        [Authorize(Roles = "Admin")]
        [Route("users")]
        public IHttpActionResult GetUsers()
        {
            return Ok(ApplicationUserManager.Users.ToList().Select(u => this.TheModelFactory.Create(u)));
        }

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}", Name = "GetUserById")]
        public async Task<IHttpActionResult> GetUser(string Id)
        {
            var user = await ApplicationUserManager.FindByIdAsync(Id);

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
        public async Task<IHttpActionResult> CreateUser(string token)
        {
            using (var httpClient = new HttpClient())
            {
                var userRequestUri = string.Format("https://api.github.com/user?access_token={0}", token);
                var message = new HttpRequestMessage(HttpMethod.Get, userRequestUri);

                //Need to set user-agent to access GitHub API, Using Chrome 48
                message.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/48.0.2564.116 Safari/537.36");

                var response = await httpClient.SendAsync(message);
                if (!response.IsSuccessStatusCode)
                    return StatusCode(HttpStatusCode.BadRequest);

                var dto = JsonConvert.DeserializeObject<UserDto>(await response.Content.ReadAsStringAsync());
                //TODO: Use converter
                var user = new User()
                {
                    //TODO: Get Primary Email from GitHub
                    Email = "",
                    UserName = dto.Login,
                    Token = token,
                    ProviderId = dto.GitHubId
                };

                IdentityResult addUserResult = await ApplicationUserManager.CreateAsync(user);
                if (!addUserResult.Succeeded)
                {
                    return GetErrorResult(addUserResult);
                }


                //Geting repos for user
                var userReposRequestUri = string.Format("https://api.github.com/users/{0}/repos?access_token={1}", user.UserName, user.Token);
                message = new HttpRequestMessage(HttpMethod.Get, userReposRequestUri);
                message.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/48.0.2564.116 Safari/537.36");
                response = await httpClient.SendAsync(message);
                if (!response.IsSuccessStatusCode)
                    return StatusCode(HttpStatusCode.BadRequest);

                //Todo rewise, move to another controller
                var projects = JsonConvert.DeserializeObject<List<ProjectDto>>(await response.Content.ReadAsStringAsync());

                // Change identity user to app user
                Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));
                return Created(locationHeader, TheModelFactory.Create(user));
            }
        }
    }
}