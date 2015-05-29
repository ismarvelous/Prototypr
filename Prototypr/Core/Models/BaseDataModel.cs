using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace Prototypr.Core.Models
{
    public abstract class BaseDataModel : DynamicObject, IDataModel
    {
        public readonly IDictionary<string, object> Source;

        protected BaseDataModel(IDictionary<string, object> source)
        {
            Source = source;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = Source.FirstOrDefault(f => f.Key.Equals(binder.Name, StringComparison.InvariantCultureIgnoreCase)).Value;
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var member = Source.FirstOrDefault(f => f.Key.Equals(binder.Name, StringComparison.InvariantCultureIgnoreCase)).Value;
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

        public string Url
        {
            get
            {
                return Source.ContainsKey("Url") ? 
                    SafePath(Source["Url"].ToString()) : 
                    null;
            }
            set
            {
                if (Source.ContainsKey("Url"))
                    Source["Url"] = SafePath(value);
                else
                    Source.Add("Url", SafePath(value));

            }
        }

        public string Layout
        {
            get
            {
                return Source.ContainsKey("Layout") ? 
                    SafePath(Source["Layout"].ToString()) : 
                    null;
            }
            set
            {
                if (Source.ContainsKey("Layout"))
                    Source["Layout"] = SafePath(value);
                else
                    Source.Add("Layout", SafePath(value));

            }
        }

        private static string SafePath(string path)
        {
            if (path != null)
            {
                var ret = path;
                ret = ret.Replace("\\", "/");

                if (ret.StartsWith("/"))
                    ret = ret.Substring(1, ret.Length - 1);

                if (ret.EndsWith("/"))
                    ret = ret.Substring(0, ret.Length - 1);

                return ret;
            }

            return null;
        }

        public virtual bool IsNull
        {
            get { return false; }
        }
    }
}
