using Chuhukon.Prototypr.Core.Base;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace Chuhukon.Prototypr.Core.Models
{
    public class DynamicFileDataObject : DynamicObject, IDataModel
    {
        private IDictionary<string, object> Source;

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


        public string Url { get; set; }

        public string Layout { get; set; }
    }
}