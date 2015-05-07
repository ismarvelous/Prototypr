using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Prototypr.Mvc;

namespace Prototypr
{
    public class Application : HttpApplication
    {
        protected virtual void Application_Start()
        {
            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            GlobalFilters.Filters.Add(new HandleErrorAttribute());

            //prototypr root.
            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            RouteTable.Routes.MapRoute(
                "Default",
                "{*path}",
                new { controller = "Prototypr", action = "Index", path = "index" }
            );

            AreaRegistration.RegisterAllAreas();

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new PrototyprViewEngine());

            ControllerBuilder.Current.SetControllerFactory(new PrototyprControllerFactory());
        }
    }
}
