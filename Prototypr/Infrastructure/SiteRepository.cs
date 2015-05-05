using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Prototypr.Core.Base;
using Prototypr.Core.Models;

namespace Prototypr.Infrastructure
{
    /// <summary>
    /// Appdata site repository for markdown and json files..
    /// </summary>
    public class SiteRepository : ISiteRepository
    {
        public string DataPath { get; private set; }

        public SiteRepository(string mapPath)
        {
            DataPath = mapPath.ToLower(); //core always works with lower case path and filenames
        }

        /// <summary>
        /// Find a model based on the given URL path...
        /// Always contains properties (Layout, Url)
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public IDataModel FindModel(string url)
        {
            var path = url; //default path is equal to url;
            
            //first check if this url is a permalink, if so find the filepath corresponding with this permalink.
            var permaLink = Permalinks().FirstOrDefault(l => l.Key.Equals(path)); 
            if (permaLink.Key != null)
                path = permaLink.Value;

            //data path under app_data is equal to url path
            var dataPaths = new string[] { 
                Path.Combine(DataPath, string.Format("{0}.json", path.Replace('/', '\\'))),
                Path.Combine(DataPath, string.Format("{0}.md", path.Replace('/', '\\'))),
                Path.Combine(DataPath, path.Replace('/', '\\'))
            };

            var model = CreateModel(dataPaths);
            
            if (model.IsNull) //be sure url and layout are filled.
            {
                model.Url = url;
                model.Layout = url;
            }

            return model;
        }

        private IDataModel CreateModel(IEnumerable<string> dataPaths)
        {
            IDataModel model = null; //todo: nullobjects

            foreach (var dataPath in dataPaths)
            {
                model = CreateModel(dataPath);

                if (!model.IsNull) break;
            }

            return model;
        }

        private IDataModel CreateModel(string dataPath) //todo: refactor, create factory in Core, move creating IDataModels as much as possible to core..
        {
            IDataModel model;

            if (dataPath.EndsWith(".json") && File.Exists(dataPath)) //does json file exist?
            {
                model = new JsonFileDataObject(dataPath, this);
            }
            else if (dataPath.EndsWith(".md") && File.Exists(dataPath)) //does markdown file exist?
            {
                model = new MdFileDataObject(dataPath, this);
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
                        pages.Add(new JsonFileDataObject(filepath, this));
                    }
                    else if (filepath.EndsWith("md", StringComparison.InvariantCultureIgnoreCase))
                    {

                        pages.Add(new MdFileDataObject(filepath, this));
                    }
                }

                model = pages.AsModelCollection(dataPath, this);
            }
            else
            {
                model = new NullDataObject(this);
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
            return Permalinks(DataPath, new Dictionary<string, string>());
        }

        private IDictionary<string, string> Permalinks(string path, IDictionary<string, string> dic)
        {
            //permalink support for .md and .json files only
            foreach (var dataPath in Directory.EnumerateFiles(path))
            {
                dynamic model = CreateModel(dataPath);

                if (model != null && model.Permalink != null)
                {
                    //TODO: calculate correct path...
                    var originalPath = Path.Combine(Path.GetDirectoryName(dataPath), Path.GetFileNameWithoutExtension(dataPath));
                    originalPath = originalPath.Substring(DataPath.Length+1, originalPath.Length - DataPath.Length-1).Replace('\\', '/');

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