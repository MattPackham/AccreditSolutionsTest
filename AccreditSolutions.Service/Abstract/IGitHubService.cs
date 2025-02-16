using AccreditSolutions.Service.Models;
using System.Collections.Generic;

namespace AccreditSolutions.Service.Abstract
{
    public interface IGitHubService
    {
        GitHubUser RetrieveUser(string username);

        IList<GitHubRepository> RetrieveRepositories(string url);

        GitHubUser RetrieveUserWithRepositories(string username);
    }
}
