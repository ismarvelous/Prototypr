using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chuhukon.Prototypr.Mvc
{
    /// <summary>
    /// use the correct view paths.. because there is only one controller, the controller is not part of the viewpath..
    /// </summary>
    public class PrototyprViewEngine : RazorViewEngine
    {
        public PrototyprViewEngine()
        {
            var viewLocations = new List<string> {  
                "~/Views/{0}/index.cshtml",  
                "~/Views/{0}.cshtml",  
            };

            var partials = ConfigurationManager.AppSettings["partialfolders"].Split(';');

            viewLocations.AddRange(partials.Select(partial => string.Format("~/Views/{0}/{1}.cshtml", partial, "{0}")));

#if !DEBUG
            //when not in debug mode, use the default notfound template!
            viewLocations.Add("~/Views/notfound.cshtml");
#endif

            this.PartialViewLocationFormats = viewLocations.ToArray();
            this.ViewLocationFormats = viewLocations.ToArray();
        }

    }
}