using Newtonsoft.Json;

namespace AccreditSolutions.Service.Models
{
    public class GitHubRepository
    {
        [JsonProperty("name")]
        public string RepositoryName { get; set; }

        [JsonProperty("html_url")]
        public string RepositoryUrl { get; set; }

        [JsonProperty("stargazers_count")]
        public int StarCount { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
