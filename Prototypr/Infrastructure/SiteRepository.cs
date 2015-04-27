using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using MarkdownSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Prototypr.Core.Base;
using Prototypr.Core.Models;

namespace Prototypr.Infrastructure
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

            IDataModel model = new DynamicFileDataObject(new ExpandoObject());

            //data path under app_data is equal to url path
            var dataPaths = new string[] { 
                Path.Combine(MapPath, string.Format("{0}.json", path.Replace('/', '\\'))),
                Path.Combine(MapPath, string.Format("{0}.md", path.Replace('/', '\\'))),
                Path.Combine(MapPath, path.Replace('/', '\\'))
            };

            foreach (var dataPath in dataPaths)
            {
                model = GetModel(dataPath);

                if (model != null)
                {
                    if (model.Layout == null)
                        model.Layout = path;

                    break;
                }
            }

            if(model == null)
            {
                model = new DynamicFileDataObject(new ExpandoObject())
                {
                    Layout = path,
                    Url = path
                };
            }
            else if (model is DynamicFileDataObject)
            {
                model.Url = ((dynamic)model).Permalink ?? path;
            }

            return model;
        }

        private IDataModel GetModel(string dataPath)
        {
            var md = new Markdown();
            dynamic model = null;

            if (dataPath.EndsWith(".json") && File.Exists(dataPath)) //does json file exist?
            {
                var source = JsonConvert.DeserializeObject<ExpandoObject>(File.ReadAllText(dataPath), new ExpandoObjectConverter()); //deserialize json object
                model = new DynamicFileDataObject(source);
            }
            else if (dataPath.EndsWith(".md") && File.Exists(dataPath)) //does markdown file exist?
            {
                dynamic source = new ExpandoObject();
                model = new DynamicFileDataObject(source as IDictionary<string, object>);
                model.Content = MvcHtmlString.Create(md.Transform(File.ReadAllText(dataPath), source as IDictionary<string, object>)); //deserialze markdown..
            }
            else if (Directory.Exists(dataPath)) //path is equal the the directory
            {
                //Get all data files
                var dataFiles = Directory.GetFiles(dataPath, "*.*")
                    .Where(f => f.EndsWith(".md") || f.EndsWith(".json"));

                var pages = new List<dynamic>(); //model is IEnumerable of all directory files
                foreach (var filepath in dataFiles)
                {
                    if (filepath.EndsWith("json", StringComparison.InvariantCultureIgnoreCase))
                    {
                        pages.Add(JsonConvert.DeserializeObject(File.ReadAllText(filepath)));
                    }
                    else if (filepath.EndsWith("md", StringComparison.InvariantCultureIgnoreCase))
                    {
                        dynamic obj = new ExpandoObject();
                        obj.Content = MvcHtmlString.Create(md.Transform(File.ReadAllText(filepath), obj as IDictionary<string, object>));
                        pages.Add(obj);
                    }
                }

                model = pages.AsModelCollection(dataPath, this);
                //todo: be sure each item in the collection is an IDataModel!!
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

                if (model != null && model.Permalink != null)
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