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
            return controllerName == "Prototype" ? 
                new Prototypr.Controllers.PrototypeController(new Infrastructure.SiteRepository(AppDomain.CurrentDomain.GetData("DataDirectory").ToString())) : 
                base.CreateController(requestContext, controllerName);
        }
    }
}