using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prototypr.Core.Base;

namespace Prototypr.Core.Models
{
    public abstract class BaseDataObject : DynamicObject, IDataModel
    {
        protected readonly IDictionary<string, object> Source;
        protected readonly ISiteRepository Repository;

        protected BaseDataObject(IDictionary<string, object> source, ISiteRepository repository)
        {
            Source = source;
            Repository = repository;
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

        public virtual string Url { get; set; } //todo: basic permalink logic here

        public virtual string Layout { get; set; } //todo: basic layout logic here..

        public virtual bool IsNull
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
