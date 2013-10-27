using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Chuhukon.Prototypr.Core.Base;
using System.Text;
using System.Dynamic;
using System.IO;


namespace Chuhukon.Prototypr.Infrastructure
{
    /// <summary>
    /// Appdata site repository for markdown and json files..
    /// </summary>
    public class SiteRepository : ISiteRepository
    {
        private string MapPath { get; set; }

        private string DefaultItemPath(string path)
        {
            string[] route = path.ToString().Split('/');
            StringBuilder viewname = new StringBuilder();
            for (int i = 0; i < route.Length - 1; i++)
            {
                if (i < route.Length - 2)
                    viewname.AppendFormat("{0}/", route[i]);
                else
                    viewname.Append(route[i]);

            }

            return viewname.Append("/item").ToString();
        }

        public SiteRepository(string mapPath)
        {
            MapPath = mapPath;
        }

        public dynamic FindModel(string path)
        {
            MarkdownSharp.Markdown md = new MarkdownSharp.Markdown();
            dynamic model = new ExpandoObject();

            //data path under app_data is equal to url path
            var jsonpath = Path.Combine(MapPath, string.Format("{0}.json", path.Replace('/', '\\')));
            var mdpath = Path.Combine(MapPath, string.Format("{0}.md", path.Replace('/', '\\')));
            var dirpath = Path.Combine(MapPath, path.Replace('/', '\\'));
            

            if (System.IO.File.Exists(jsonpath)) //does json file exist?
            {
                model = JsonConvert.DeserializeObject<ExpandoObject>(System.IO.File.ReadAllText(jsonpath), new Newtonsoft.Json.Converters.ExpandoObjectConverter()); //deserialize json object

                if (!((IDictionary<String, object>)model).ContainsKey("Layout")) //if layout is not specified in json file, use default layout
                    model.Layout = DefaultItemPath(path);
                
            }
            else if (System.IO.File.Exists(mdpath)) //does markdown file exist?
            {
                model.Content = System.Web.Mvc.MvcHtmlString.Create(md.Transform(System.IO.File.ReadAllText(mdpath), model as IDictionary<string, object>)); //deserialze markdown..

                if (!((IDictionary<String, object>)model).ContainsKey("Layout")) //if layout is not specified in markdown file, use default layout
                    model.Layout = DefaultItemPath(path);
            }
            else if (System.IO.Directory.Exists(dirpath)) //path is equal the the directory
            {
                //Get all data files
                var dataFiles = System.IO.Directory.GetFiles(dirpath, "*.*")
                    .Where(f => f.EndsWith(".md") || f.EndsWith(".json"));

                var pages = new List<dynamic>(); //model is IEnumerable of all directory files
                foreach (var filepath in dataFiles)
                {
                    if (filepath.EndsWith("json", StringComparison.InvariantCultureIgnoreCase))
                    {
                        pages.Add(JsonConvert.DeserializeObject(System.IO.File.ReadAllText(filepath)));
                    }
                    else if (filepath.EndsWith("md", StringComparison.InvariantCultureIgnoreCase))
                    {
                        dynamic obj = new ExpandoObject();
                        obj.Content = System.Web.Mvc.MvcHtmlString.Create(md.Transform(System.IO.File.ReadAllText(filepath), obj as IDictionary<string, object>));
                        pages.Add(obj);
                    }
                }

                model = pages.AsModelCollection(path, this);
                model.Layout = path;
            }
            else
            {
                model.Layout = path;
            }

            return model;
            
        }
    }
}