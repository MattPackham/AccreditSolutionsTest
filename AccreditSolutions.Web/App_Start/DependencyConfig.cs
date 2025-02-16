using AccreditSolutionsShared.Classes.Abstract;
using AccreditSolutionsShared.Classes.Concrete;
using AccreditSolutions.Service.Abstract;
using AccreditSolutions.Service.Concrete;
using SimpleInjector;
using System.Web.Mvc;
using SimpleInjector.Integration.Web.Mvc;
using System.Reflection;

namespace AccreditSolutions.App_Start
{
    public static class DependencyConfig
    {
        public static void RegisterDependencies()
        {
            var container = new Container();

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            container.Register<IUsernameValidator, UsernameValidator>();
            container.Register<IEnvironmentVariables, EnvironmentVariables>();
            container.Register<IWebClient, WebClientFacade>();
            container.Register<IGitHubService, GitHubService>();

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
    }
}