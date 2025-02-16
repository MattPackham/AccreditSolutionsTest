using System.Collections.Generic;

namespace AccreditSolutions.Models.ViewModels
{
    public class GitHubUserResultViewModel
    {
        public string Username { get; set; }
        public string AvatarUrl { get; set; }
        public string Location { get; set; }
        public List<GitHubRepositoryViewModel> GitHubRepositories { get; set; }
    }
}