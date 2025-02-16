using Newtonsoft.Json;
using AccreditSolutions.Service.Abstract;
using AccreditSolutions.Service.Models;
using System.Collections.Generic;
using System.Linq;
using AccreditSolutionsShared.Classes.Concrete;
using System;

namespace AccreditSolutions.Service.Concrete
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

        public GitHubUser RetrieveUserWithRepositories(string username)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentNullException();

            var gitHubUser = RetrieveUser(username);

            if (gitHubUser == null) return null;

            if (gitHubUser.ReposUrl == null || string.IsNullOrEmpty(gitHubUser.ReposUrl)) return gitHubUser;

            var gitHubRepositories = RetrieveRepositories(gitHubUser.ReposUrl);

            if (gitHubRepositories == null) return gitHubUser;

            gitHubUser.GitHubRepositories = gitHubRepositories
                    .OrderByDescending(x => x.StarCount)
                    .ThenBy(x => x.RepositoryName)
                    .Take(_iEnvironmentVariables.NumberOfRepositories)
                    .ToList();

            return gitHubUser;
        }

        public GitHubUser RetrieveUser(string username)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentNullException();

            var url = GitHubHelper.BuildUserRetrievalUrl(_iEnvironmentVariables.GitHubUrl, username);
            var jsonResult = _iWebClient.DownloadString(url);

            if (string.IsNullOrEmpty(jsonResult)) return null;

            var gitHubUser = JsonConvert.DeserializeObject<GitHubUser>(jsonResult);

            return gitHubUser;
        }

        public IList<GitHubRepository> RetrieveRepositories(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException();

            var jsonResult = _iWebClient.DownloadString(url);

            if (string.IsNullOrWhiteSpace(jsonResult)) return null;

            return JsonConvert.DeserializeObject<List<GitHubRepository>>(jsonResult);
        }
    }
}
