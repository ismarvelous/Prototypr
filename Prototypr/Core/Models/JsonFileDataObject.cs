using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Prototypr.Core.Base;

namespace Prototypr.Core.Models
{
    public class JsonFileDataObject : FileDataObject
    {
        public JsonFileDataObject(string path, ISiteRepository rep) 
            : base(path, JsonConvert.DeserializeObject<ExpandoObject>(File.ReadAllText(path), 
            new ExpandoObjectConverter()), rep)
        {
            
        }

        public override string Url
        {
            get { return base.Url.Replace(".json", string.Empty); }
            set { base.Url = value; }
        }

        public override string Layout
        {
            get { return base.Layout.Replace(".json", string.Empty); }
            set { base.Layout = value; }
        }
    }
}
