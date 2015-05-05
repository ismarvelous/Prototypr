using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using Prototypr.Core.Base;

namespace Prototypr.Core.Models
{
    public abstract class FileDataObject : BaseDataObject
    {
        protected readonly string Path;

        protected FileDataObject(string path, IDictionary<string, object> source, ISiteRepository rep)
            : base(source, rep)
        {
            Path = path.ToLower();
        }

        private string _url;
        public override string Url
        {
            get
            {
                if (_url == null)
                {
                    _url = Source.ContainsKey("Permalink") ? 
                        Source["Permalink"].ToString() : 
                        Path.Replace(Repository.DataPath, string.Empty).Replace("\\", "/");
                }

                if (_url.StartsWith("/")) _url = _url.Substring(1, _url.Length - 1);

                return _url;
            }
            set { _url = value; }
        }

        private string _layout;

        public override string Layout
        {
            get
            {
                if (_layout == null)
                {
                    _layout = Source.ContainsKey("Layout") ? 
                        Source["Layout"].ToString() : 
                        Path.Replace(Repository.DataPath, string.Empty).Replace("\\", "/");
                }

                if (_layout.StartsWith("/")) _layout = _layout.Substring(1, _layout.Length - 1);

                return _layout;
            }
            set { _layout = value; }
        }

        public override bool IsNull
        {
            get { return false; }
        }
    }
}