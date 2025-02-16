using AccreditSolutionsShared.Classes.Abstract;
using AccreditSolutionsShared.Classes.Concrete;
using Service.Abstract;
using Service.Concrete;
using SimpleInjector;
using System.Web.Mvc;
using SimpleInjector.Integration.Web.Mvc;
using System.Web.Routing;
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