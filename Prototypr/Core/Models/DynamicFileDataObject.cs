using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using Prototypr.Core.Base;

namespace Prototypr.Core.Models
{
    public class DynamicFileDataObject : DynamicObject, IDataModel
    {
        private readonly IDictionary<string, object> Source;

        public DynamicFileDataObject(IDictionary<string, object> source)
        {
            Source = source;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = Source.FirstOrDefault(f => f.Key.Equals(binder.Name)).Value;
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var member = Source.FirstOrDefault(f => f.Key.Equals(binder.Name)).Value;
            if (member == null)
            {
                Source.Add(binder.Name, value);
            }
            else
            {
                Source[binder.Name] = value;
            }

            return true;
        }

        private string _url;
        public string Url
        {
            get
            {
                if (_url == null && Source.ContainsKey("Url"))
                {
                    _url = Source["Url"].ToString();
                }

                return _url;
            }
            set { _url = value; }
        }

        private string _layout;

        public string Layout
        {
            get
            {
                if (_layout == null && Source.ContainsKey("Layout"))
                {
                    _layout = Source["Layout"].ToString();
                }

                return _layout;
            }
            set { _layout = value; }
        }
    }
}