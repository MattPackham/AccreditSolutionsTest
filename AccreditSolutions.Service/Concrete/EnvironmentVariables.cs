using AccreditSolutions.Service.Abstract;
using System.Collections.Generic;
using System.Configuration;

namespace AccreditSolutions.Service.Concrete
{
    public class EnvironmentVariables : IEnvironmentVariables
    {
        public string GitHubUrl => string.IsNullOrEmpty(ConfigurationManager.AppSettings["GitHubUrl"]) ? throw new KeyNotFoundException() : ConfigurationManager.AppSettings["GitHubUrl"];

        public int NumberOfRepositories => string.IsNullOrEmpty(ConfigurationManager.AppSettings["NumberOfRepositories"]) ? throw new KeyNotFoundException() : int.Parse(ConfigurationManager.AppSettings["NumberOfRepositories"]);
    }
}
