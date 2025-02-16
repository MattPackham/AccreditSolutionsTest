using AccreditSolutions.Models.ViewModels;
using AccreditSolutions.Service.Models;
using System.Collections.Generic;

namespace AccreditSolutions.Models.Factories
{
    public class GitHubUserResultViewModelFactory : IFactory<GitHubUserResultViewModel>
    {
        private readonly GitHubUser _gitHubUser;
        public GitHubUserResultViewModelFactory(GitHubUser gitHubUser)
        {
            _gitHubUser = gitHubUser;
        }

        public GitHubUserResultViewModel Create()
        {
            var displayGitHubUserViewModel = new GitHubUserResultViewModel
            {
                Username = _gitHubUser.UserName,
                AvatarUrl = _gitHubUser.AvatarUrl,
                Location = _gitHubUser.Location,
                GitHubRepositories = CreateRepositories(_gitHubUser.GitHubRepositories)
            };

            return displayGitHubUserViewModel;
        }

        public List<GitHubRepositoryViewModel> CreateRepositories(IList<GitHubRepository> gitHubRepositories)
        {
            if (gitHubRepositories == null) return null;

            var gitHubRepositoryViewModels = new List<GitHubRepositoryViewModel>();

            foreach (var gitHubRepository in gitHubRepositories)
            {
                gitHubRepositoryViewModels.Add(new GitHubRepositoryViewModel()
                    {
                        RepositoryName = gitHubRepository.RepositoryName,
                        RepositoryUrl = gitHubRepository.RepositoryUrl,
                        StarCount = gitHubRepository.StarCount,
                        Description = gitHubRepository.Description
                });
            }

            return gitHubRepositoryViewModels;
        }
    }
}