using Prototypr.Core.Base;
using Prototypr.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Prototypr.Files.Base;
using Prototypr.Files.Models;

namespace Prototypr.Controllers
{
    /// <summary>
    /// Default controller for all prototypes, calculates view to render and gets data from App_data folder.
    /// </summary>
    public class PrototyprController : Controller
    {
        protected readonly IFileDataRepository DataRepository;

        public PrototyprController(IFileDataRepository dataRepository)
        {
            DataRepository = dataRepository;
        }

        public ActionResult Index(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                //var path = this.RouteData.Values["path"].ToString();

                var model = DataRepository.FindModel(path);

                //return site and model..
                ViewData.Add("Site", new Site(DataRepository));

                if (model.Url != null && !model.Url.Contains(path))
                    return RedirectPermanent(string.Concat("/", model.Url)); //TODO: Implement Urls correctly..

                //select view //TODO: cleanup this code..
                if (ViewEngines.Engines.FindView(ControllerContext, model.Layout, null).View == null)
                {
                    if (model.Layout.LastIndexOf("/", StringComparison.InvariantCulture) == model.Layout.Length-1)
                        model.Layout = model.Layout.Remove(model.Layout.Length - 1, 1);

                    model.Layout = string.Format("{0}/item",
                        model.Layout.Substring(0,
                            model.Layout.IndexOf("/", StringComparison.Ordinal) > 0
                                ? model.Layout.LastIndexOf("/", StringComparison.InvariantCulture)
                                : model.Layout.Length));
                }

                return View(model.Layout, model);
            }

            return HttpNotFound();
        }
    }
}
