using System.Web.Mvc;
using System.Web.Routing;

namespace Chuhukon.Example
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",
                "{*path}",
                new { controller = "Prototype", action = "Index", path="index" }
            );
        }
    }
}