using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chuhukon.Prototypr.Mvc
{
    public class PrototyprViewEngine : RazorViewEngine
    {
        public PrototyprViewEngine()
        {
            var viewLocations =  new List<string> {  
                "~/Views/{0}/index.cshtml",  
                "~/Views/{0}.cshtml",  
            };

            var partials = ConfigurationManager.AppSettings["partialfolders"].Split(';');

            foreach(var partial in partials)
            {
                viewLocations.Add(string.Format("~/Views/{0}/{1}.cshtml", partial, "{0}"));
            }

            viewLocations.Add("~/Views/notfound.cshtml");

            this.PartialViewLocationFormats = viewLocations.ToArray();
            this.ViewLocationFormats = viewLocations.ToArray();
        }
    }
}