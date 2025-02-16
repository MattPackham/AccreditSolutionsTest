namespace AccreditSolutions.Service.Abstract
{
    public interface IEnvironmentVariables
    {
        string GitHubUrl { get; }
        int NumberOfRepositories { get; }
    }
}
