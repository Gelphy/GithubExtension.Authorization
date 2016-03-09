using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace GithubExtension.Security.WebApi.Models
{
    
    public class UserDTO
    {

        public string Login { get; set; }

        [JsonProperty(PropertyName = "id")]
        public int GitHubId { get; set; }

        public string Url { get; set; }
    }
}