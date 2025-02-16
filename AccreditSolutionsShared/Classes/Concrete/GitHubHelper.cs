using System;

namespace AccreditSolutionsShared.Classes.Concrete
{
    public static class GitHubHelper
    {
        public static string BuildUserRetrievalUrl(string baseUrl, string userName)
        {
            if (string.IsNullOrWhiteSpace(baseUrl) || string.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException();

            return baseUrl + userName;
        }
    }
}
