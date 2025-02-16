using Service.Abstract;
using System.Collections.Generic;
using System.Configuration;

namespace Service.Concrete
{
    public class EnvironmentVariables : IEnvironmentVariables
    {
        public string GitHubUrl => string.IsNullOrEmpty(ConfigurationManager.AppSettings["GitHubUrl"]) ? throw new KeyNotFoundException() : ConfigurationManager.AppSettings["GitHubUrl"];

        public int NumberOfRepositories => string.IsNullOrEmpty(ConfigurationManager.AppSettings["NumberOfRepositories"]) ? throw new KeyNotFoundException() : int.Parse(ConfigurationManager.AppSettings["NumberOfRepositories"]);
    }
}
