using System;
using System.Web.Mvc;
using Prototypr.Files;

namespace Prototypr.Mvc
{
    public class PrototyprControllerFactory : DefaultControllerFactory
    {
        public override IController CreateController(System.Web.Routing.RequestContext requestContext, string controllerName)
        {
            return controllerName == "Prototypr" ? 
                new Prototypr.Controllers.PrototyprController(new FileDataRepository(AppDomain.CurrentDomain.GetData("DataDirectory").ToString())) : 
                base.CreateController(requestContext, controllerName);
        }
    }
}