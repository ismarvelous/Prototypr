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

                IDataModel model = Repository.FindModel(path);

                //return site and model..
                ViewData.Add("Site", new Site(Repository));

                if (!model.Url.Contains(path))
                    return RedirectPermanent(string.Concat("/", model.Url)); //TODO: Implement Urls correctly..
                else
                    return View(model.Layout, model); //default: view is equal to url path

                
            }

            return HttpNotFound();
        }

        

    }

}
