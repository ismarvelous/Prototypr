using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Chuhukon.Prototypr.Core.Base;
using System.Text;
using System.Dynamic;
using System.IO;
using Chuhukon.Prototypr.Core.Models;


namespace Chuhukon.Prototypr.Infrastructure
{
    /// <summary>
    /// Appdata site repository for markdown and json files..
    /// </summary>
    public class SiteRepository : ISiteRepository
    {
        private string MapPath { get; set; }

        public SiteRepository(string mapPath)
        {
            MapPath = mapPath;
        }

        /// <summary>
        /// Find a model based on the given URL path...
        /// Always contains properties (Layout, Url)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public IDataModel FindModel(string path)
        {
            //first check if this path is a permalink
            var permaLink = Permalinks().FirstOrDefault(l => l.Key.Equals(path));
            if (permaLink.Key != null)
                path = permaLink.Value;

            MarkdownSharp.Markdown md = new MarkdownSharp.Markdown();
            IDataModel model = new DynamicFileDataObject(new ExpandoObject() as IDictionary<string, object>);

            //data path under app_data is equal to url path
            string[] dataPaths = new string[] { 
                Path.Combine(MapPath, string.Format("{0}.json", path.Replace('/', '\\'))),
                Path.Combine(MapPath, string.Format("{0}.md", path.Replace('/', '\\'))),
                Path.Combine(MapPath, path.Replace('/', '\\'))
            };

            foreach (var dataPath in dataPaths)
            {
                model = GetModel(dataPath);

                if (model != null)
                {
                    if (model is IDictionary<String, object> && !((IDictionary<String, object>)model).ContainsKey("Layout"))
                        model.Layout = path;
                    else if (model.Layout == null)
                        model.Layout = path;

                    break;
                }
            }

            if(model == null)
            {
                model = new DynamicFileDataObject(new ExpandoObject() as IDictionary<string, object>);
                model.Layout = path;
                model.Url = path;
            }
            else if (model is DynamicFileDataObject)
            {
                if (((dynamic)model).PermaLink != null)
                    model.Url = ((dynamic)model).Permalink;
                else
                    model.Url = path;
            }

            return model;
        }

        private IDataModel GetModel(string dataPath)
        {
            MarkdownSharp.Markdown md = new MarkdownSharp.Markdown();
            dynamic model = null;

            if (dataPath.EndsWith(".json") && System.IO.File.Exists(dataPath)) //does json file exist?
            {
                var source = JsonConvert.DeserializeObject<ExpandoObject>(System.IO.File.ReadAllText(dataPath), new Newtonsoft.Json.Converters.ExpandoObjectConverter()); //deserialize json object
                model = new DynamicFileDataObject(source as IDictionary<string, object>);
            }
            else if (dataPath.EndsWith(".md") && System.IO.File.Exists(dataPath)) //does markdown file exist?
            {
                dynamic source = new ExpandoObject();
                model = new DynamicFileDataObject(source as IDictionary<string, object>);
                model.Content = System.Web.Mvc.MvcHtmlString.Create(md.Transform(System.IO.File.ReadAllText(dataPath), source as IDictionary<string, object>)); //deserialze markdown..
            }
            else if (System.IO.Directory.Exists(dataPath)) //path is equal the the directory
            {
                //Get all data files
                var dataFiles = System.IO.Directory.GetFiles(dataPath, "*.*")
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

                model = pages.AsModelCollection(dataPath, this);
            }
            else
            {
                return null;
            }

            return model;
        }

        /// <summary>
        /// Search all data files for permalinks
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, string> Permalinks()
        {
            //TODO: global caching!!!
            return Permalinks(MapPath, new Dictionary<string, string>());
        }

        private IDictionary<string, string> Permalinks(string path, IDictionary<string, string> dic)
        {
            //permalink support for .md and .json files only
            foreach (var dataPath in Directory.EnumerateFiles(path))
            {
                dynamic model = GetModel(dataPath);

                if (model != null && model.PermaLink != null)
                {
                    //TODO: calculate correct path...
                    var originalPath = Path.Combine(Path.GetDirectoryName(dataPath), Path.GetFileNameWithoutExtension(dataPath));
                    originalPath = originalPath.Substring(MapPath.Length+1, originalPath.Length - MapPath.Length-1).Replace('\\', '/');

                    dic.Add(model.Permalink, originalPath);
                }
            }

            //search all subdirectories aswell.
            foreach (var dataPath in Directory.EnumerateDirectories(path))
            {
                dic = Permalinks(dataPath, dic);
            }

            return dic;
        }
    }
}