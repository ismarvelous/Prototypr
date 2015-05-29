using System.Dynamic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Prototypr.Core.Models;

namespace Prototypr.Files.Models
{
    public class JsonFile : FileBase
    {
        public JsonFile(string path, IFileDataRepository rep) 
            : base(path, JsonConvert.DeserializeObject<ExpandoObject>(File.ReadAllText(path), 
            new ExpandoObjectConverter()), rep)
        {
            Initialize();
        }

        public void Initialize()
        {
            FileExtension = ".json";
            Url = Url.Replace(FileExtension, string.Empty);
            Layout = Layout.Replace(FileExtension, string.Empty);
        }

        public override bool IsNull
        {
            get { return false; }
        }
    }
}
