using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GithubExtension.Security.WebApi.Exceptions;
using GithubExtension.Security.WebApi.Models;
using Newtonsoft.Json;

namespace GithubExtension.Security.WebApi.Services
{
    // TODO: Create NLog
    public class GithubService : IGithubService
    {
        private readonly HttpClient _httpClient;
        private static readonly Dictionary<string, string> DefaultHeaders = new Dictionary<string, string>()
        {
             //Need to set user-agent to access GitHub API, Using Chrome 48
            { "User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/48.0.2564.116 Safari/537.36" }
        };

        public GithubService()
        {
            _httpClient = new HttpClient();
        }

        // TODO: Ask about async of this method
        public async Task<UserDto> GetUserAsync(string token)
        {
            // TODO: Incapsulate all requestUri
            var requestUri = string.Format("https://api.github.com/user?access_token={0}", token);
            var message = CreateMessage(HttpMethod.Get, requestUri);

            var response = await _httpClient.SendAsync(message);

            if (!response.IsSuccessStatusCode)
                throw new UnsuccessfullRequestException();


            var dto = JsonConvert.DeserializeObject<UserDto>(await response.Content.ReadAsStringAsync());

            return dto;
        }

        public async Task<List<RepositoryDto>> GetReposAsync(string userName, string token)
        {
            //Geting repos for user
            var requestUri = string.Format("https://api.github.com/users/{0}/repos?access_token={1}", userName, token);
            var message = CreateMessage(HttpMethod.Get, requestUri);

            var response = await _httpClient.SendAsync(message);
            if (!response.IsSuccessStatusCode)
                throw new UnsuccessfullRequestException();

            //Todo rewise, move to another controller
            return JsonConvert.DeserializeObject<List<RepositoryDto>>(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Static factory for message creation.
        /// Default message parameters are set here
        /// </summary>
        /// <param name="method">Method of your request</param>
        /// <param name="requestUri">Uri of your request</param>
        /// <returns>HttpRequestMessage</returns>
        private static HttpRequestMessage CreateMessage(HttpMethod method, string requestUri)
        {
            var message = new HttpRequestMessage(method, requestUri);
            foreach (var header in DefaultHeaders)
            {
                message.Headers.Add(header.Key, header.Value);
            }

            return message;
        }
    }
}