using Newtonsoft.Json;
using Service.Abstract;
using Service.Models;
using System.Collections.Generic;
using System.Linq;
using AccreditSolutionsShared.Classes.Concrete;
using System;

namespace Service.Concrete
{
    public class GitHubService : IGitHubService
    {
        private readonly IWebClient _iWebClient;
        private readonly IEnvironmentVariables _iEnvironmentVariables;

        public GitHubService(IWebClient iWebClient, IEnvironmentVariables iEnvironmentVariables)
        {
            _iWebClient = iWebClient;
            _iEnvironmentVariables = iEnvironmentVariables;
        }

        public GitHubUser RetrieveUser(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentNullException();

            var url = GitHubHelper.BuildUserRetrievalUrl(_iEnvironmentVariables.GitHubUrl, username);
            var jsonResult = _iWebClient.DownloadString(url);
            var gitHubUser = JsonConvert.DeserializeObject<GitHubUser>(jsonResult);

            if (gitHubUser == null)
            {
                return null;
            }

            if (gitHubUser.ReposUrl == null || string.IsNullOrWhiteSpace(gitHubUser.ReposUrl)) return gitHubUser;

            var gitHubRepositories = GetRepositories(gitHubUser.ReposUrl);

            if (gitHubRepositories == null) return gitHubUser;

            gitHubUser.GitHubRepositories = gitHubRepositories
                    .OrderByDescending(x => x.StarCount)
                    .ThenBy(x => x.RepositoryName)
                    .Take(_iEnvironmentVariables.NumberOfRepositories)
                    .ToList();

            return gitHubUser;
        }

        public IList<GitHubRepository> GetRepositories(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException();

            var jsonResult = _iWebClient.DownloadString(url);

            if (string.IsNullOrWhiteSpace(jsonResult)) return null;

            return JsonConvert.DeserializeObject<List<GitHubRepository>>(jsonResult);
        }
    }
}
