using System.Web.Mvc;
using System.Web.Routing;

namespace AccreditSolutions
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "CatchAll",
                url: "{*url}",
                defaults: new { controller = "GitHubUser", action = "RetrieveGitHubUser" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "GitHubUser", action = "RetrieveGitHubUser" }
            );
        }
    }
}
