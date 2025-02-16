using Service.Models;
using System.Collections.Generic;

namespace Service.Abstract
{
    public interface IGitHubService
    {
        GitHubUser RetrieveUser(string username);

        IList<GitHubRepository> GetRepositories(string url);
    }
}
