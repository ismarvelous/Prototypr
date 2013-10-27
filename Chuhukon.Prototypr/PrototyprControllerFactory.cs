using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Chuhukon.Prototypr
{
    public class PrototyprControllerFactory : DefaultControllerFactory
    {
        public override IController CreateController(System.Web.Routing.RequestContext requestContext, string controllerName)
        {
            if (controllerName == "Prototype") //TODO: use Ninject...
                return new Prototypr.Controllers.PrototypeController(new Chuhukon.Prototypr.Infrastructure.SiteRepository(AppDomain.CurrentDomain.GetData("DataDirectory").ToString()));
            else
                return base.CreateController(requestContext, controllerName);
        }

        public override void ReleaseController(IController controller)
        {
            base.ReleaseController(controller);
        }
    }
}