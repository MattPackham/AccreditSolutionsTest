using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AccreditSolutions.Service.Models
{
    public class GitHubUser
    {

        [JsonProperty("login")]
        public string UserName { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }


        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }


        [JsonProperty("repos_url")]
        public string ReposUrl { get; set; }

    
        [JsonIgnore]
        public IList<GitHubRepository> GitHubRepositories { get; set; }
    }
}
