using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chuhukon.Prototypr.Mvc
{
    public class PrototyprViewEngine : RazorViewEngine
    {
        public PrototyprViewEngine()
        {
            var viewLocations =  new[] {  

                //{0} = viewname {1} = controller name
                //both allowed */path.cshtml and */path/index.chtml
                "~/Views/{0}/index.cshtml",  
                "~/Views/{0}.cshtml",  

                //predefined partial view directories..
                "~/Views/components/{0}.cshtml",

                //if no view is found, always show notfound.cshtml
                "~/Views/notfound.cshtml"
            };

            this.PartialViewLocationFormats = viewLocations;
            this.ViewLocationFormats = viewLocations;
        }
    }
}