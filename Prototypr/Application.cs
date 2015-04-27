using System.Web;
using System.Web.Mvc;
using Prototypr.Mvc;
using PrototyprControllerFactory = Chuhukon.Example.PrototyprControllerFactory;

namespace Prototypr
{
    public class Application : HttpApplication
    {
        protected virtual void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new PrototyprViewEngine());

            ControllerBuilder.Current.SetControllerFactory(new PrototyprControllerFactory());
        }
    }
}
