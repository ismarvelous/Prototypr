using MarkdownSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Chuhukon.Markdown.Extensions;

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
        //
        // GET: /Simple/

        private StringBuilder ViewPath(string[] route)
        {
            StringBuilder viewname = new StringBuilder();
            for (int i = 0; i < route.Length - 1; i++) 
            {
                if (i < route.Length - 2)
                    viewname.AppendFormat("{0}/", route[i]);
                else
                    viewname.Append(route[i]);

            }

            return viewname;
        }

        public ActionResult Index()
        {
            if (this.RouteData.Values.ContainsKey("path") && this.RouteData.Values["path"] != null)
            {
                dynamic model = new System.Dynamic.ExpandoObject();
                string path = this.RouteData.Values["path"].ToString();
                string[] route = path.ToString().Split('/');
                string view = path;

                //data path under app_data is equal to url path
                var jsonpath = Path.Combine(Server.MapPath("~/App_Data/"), string.Format("{0}.json", path.Replace('/', '\\')));
                var mdpath = Path.Combine(Server.MapPath("~/App_Data/"), string.Format("{0}.md", path.Replace('/', '\\')));
                var dirpath = Path.Combine(Server.MapPath("~/App_Data/"), path.Replace('/', '\\'));

                //check if path is a data path..
                if (System.IO.File.Exists(jsonpath))
                {
                    var viewname = ViewPath(route);

                    model = JsonConvert.DeserializeObject(System.IO.File.ReadAllText(jsonpath));

                    if (model.Layout != null) //if layout is specified in data file, display using specified layout (view) file
                        view = model.Layout;
                    else
                        view = viewname.Append("/item").ToString();

                }
                else if (System.IO.File.Exists(mdpath))
                {
                    var viewname = ViewPath(route);
                    MarkdownSharp.Markdown md = new MarkdownSharp.Markdown();

                    model.Content = MvcHtmlString.Create(md.Transform(System.IO.File.ReadAllText(mdpath), model as IDictionary<string, object>));

                    if (model.Layout != null) //if layout is specified in markdown file, display using specified layout (view) file
                        view = model.Layout;
                    else
                        view = viewname.Append("/item").ToString();
                }
                else if(System.IO.Directory.Exists(dirpath))
                {
                    var json = System.IO.Directory.GetFiles(dirpath, "*.json");
                    //todo: markdown!

                    var pages = new List<dynamic>();
                    foreach (var content in json)
                    {
                        pages.Add(JsonConvert.DeserializeObject(System.IO.File.ReadAllText(content)));
                    }

                    model = pages.AsEnumerable();
                }

                //default: view is equal to url path
                return View(view, model);
            }

            return HttpNotFound();
        }

        

    }

}
