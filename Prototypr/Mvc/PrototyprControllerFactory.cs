using System;
using System.Web.Mvc;

namespace Prototypr.Mvc
{
    public class PrototyprControllerFactory : DefaultControllerFactory
    {
        public override IController CreateController(System.Web.Routing.RequestContext requestContext, string controllerName)
        {
            return controllerName == "Prototype" ? 
                new Prototypr.Controllers.PrototypeController(new Prototypr.Infrastructure.SiteRepository(AppDomain.CurrentDomain.GetData("DataDirectory").ToString())) : 
                base.CreateController(requestContext, controllerName);
        }
    }
}