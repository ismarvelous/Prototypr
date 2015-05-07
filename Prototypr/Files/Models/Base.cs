using System.Collections.Generic;
using Prototypr.Core.Models;
using Prototypr.Files.Base;

namespace Prototypr.Files.Models
{
    public abstract class Base : BaseDataModel
    {
        protected readonly string Path;
        protected readonly IFileDataRepository DataRepository;

        protected Base(string dataPath, IDictionary<string, object> source, IFileDataRepository rep)
            : base(source)
        {
            Path = dataPath.Replace(rep.DataPath, string.Empty).ToLower();
            DataRepository = rep;
            Initialize();
        }

        private void Initialize()
        {
            if (Source.ContainsKey("Permalink"))
                Url = Source["Permalink"].ToString();

            if (Url == null)
                Url = Path;

            if (Layout == null)
                Layout = Path;
        }
    }
}