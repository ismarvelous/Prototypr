using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Prototypr.Core;
using Prototypr.Core.Models;
using Prototypr.Files.Models;

namespace Prototypr.Files
{
    public interface IFileDataRepository : IDataRepository
    {
        string DataPath { get; }
    }

    /// <summary>
    /// Appdata site repository for markdown and json files..
    /// </summary>
    public class FileDataRepository : IFileDataRepository
    {
        public string DataPath { get; private set; }

        public FileDataRepository(string mapPath)
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
            //var path = url; //default path is equal to url;
            

            ////first check if this url is a permalink, if so find the filepath corresponding with this permalink.
            //var permaLink = Permalinks().FirstOrDefault(l => l.Key.Equals(path)); 
            //if (permaLink.Key != null)
            //    path = permaLink.Value;

            ////data path under app_data is equal to url path
            //var dataPaths = new string[] { 
            //    Path.Combine(DataPath, string.Format("{0}.json", path.Replace('/', '\\'))),
            //    Path.Combine(DataPath, string.Format("{0}.md", path.Replace('/', '\\'))),
            //    Path.Combine(DataPath, path.Replace('/', '\\'))
            //};

            var model = FindAll().FirstOrDefault(l => l.Url.ToLower().Equals(url.ToLower())) ?? new NullDataModel();
            
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

        private IDataModel CreateModel(string dataPath)
        {
            IDataModel model;

            if (dataPath.EndsWith(".json") && File.Exists(dataPath)) //does json file exist?
            {
                model = new JsonFile(dataPath, this);
            }
            else if (dataPath.EndsWith(".md") && File.Exists(dataPath)) //does markdown file exist?
            {
                model = new MdFile(dataPath, this);
            }
            else if (Directory.Exists(dataPath)) //path is equal the the directory
            {
                //Get all data files
                var dataFiles = Directory.GetFiles(dataPath, "*.*")
                    .Where(f => f.EndsWith(".md") || f.EndsWith(".json"));

                var pages = new FileCollection(dataPath, this); //model is IEnumerable of all directory files
                foreach (var filepath in dataFiles)
                {
                    if (filepath.EndsWith(".json", StringComparison.InvariantCultureIgnoreCase))
                    {
                        pages.Add(new JsonFile(filepath, this));
                    }
                    else if (filepath.EndsWith(".md", StringComparison.InvariantCultureIgnoreCase))
                    {

                        pages.Add(new MdFile(filepath, this));
                    }
                }

                model = pages;
            }
            else
            {
                model = new NullDataModel();
            }

            return model;
        }

        /// <summary>
        /// Search all data files for permalinks
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IDataModel> FindAll()
        {
            //TODO: global caching!!!
            return FindAll(DataPath, new List<IDataModel>());
        }

        private List<IDataModel> FindAll(string path, List<IDataModel> list )
        {
            //permalink support for .md and .json files only
            list.AddRange(Directory.EnumerateFiles(path).Select(CreateModel));
            var directories = Directory.EnumerateDirectories(path).ToList();
            list.AddRange(directories.Select(CreateModel));

            //search all subdirectories aswell.
            return directories.Aggregate(list, (current, dataPath) => FindAll(dataPath, current));
        }
    }
}