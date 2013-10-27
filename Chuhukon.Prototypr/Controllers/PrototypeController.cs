using Chuhukon.Prototypr.Core.Base;
using Chuhukon.Prototypr.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Chuhukon.Prototypr.Controllers
{
    /// <summary>
    /// Default controller for all prototypes, calculates view to render and gets data from App_data folder.
    /// 1. Is this a data file path? -> return file as model -> return (path - file )+/item" as view (route search: */path/item.cshtml or *path/item/index.cshtml)
    /// 1.2 Does the file contain a layout definition? -> return file as model -> return layout as view (route search: *layout.cshtml or *layout/index.cshtml)
    /// 2. Is this a data directory? -> return  collection of files as model -> return path as view (route search: */path/index.cshtml or */path.cshtml)
    /// 3. No data file or data directory found? -> return default - empty model -> return path as view (route search: */path/index.cshtml or */path.cshtml)
    /// </summary>
    public class PrototypeController : Controller
    {
        private ISiteRepository Repository;

        public PrototypeController(ISiteRepository repository)
        {
            Repository = repository;
        }

        public ActionResult Index()
        {
            if (this.RouteData.Values.ContainsKey("path") && this.RouteData.Values["path"] != null)
            {
                string path = this.RouteData.Values["path"].ToString();

                dynamic model = Repository.FindModel(path);

                //return site and model..
                ViewData.Add("Site", new Site(Repository));

                return View(model.Layout, model); //default: view is equal to url path
            }

            return HttpNotFound();
        }

        

    }

}
