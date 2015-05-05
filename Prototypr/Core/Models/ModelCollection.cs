using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using Prototypr.Core.Base;

namespace Prototypr.Core.Models
{
    public class ModelCollection<T> : DynamicObject, IEnumerable<T>, IDataModel
    {

        private string _url;
        public virtual string Url
        {
            get { return _url ?? (_url = Path); }
            set { _url = value; }
        }

        private string _layout;

        public virtual string Layout
        {
            get { return _layout ?? (_layout = Path); }
            set { _layout = value; }
        }

        /// <summary>
        /// Directory path
        /// </summary>
        public string Path { get; private set; }


        public bool IsNull
        {
            get { return false; }
        }

        protected readonly ISiteRepository Repository;
        protected readonly IEnumerable<T> Collection; //files.

        public ModelCollection(IEnumerable<T> files, string path, ISiteRepository repository)
        {
            Collection = files;
            Repository = repository;
            Path = path;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = Repository.FindModel(string.Format("{0}/{1}", Path, binder.Name));
            return true;
        }


        public IEnumerator<T> GetEnumerator()
        {
            return Collection.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Collection.GetEnumerator();
        }
    }
}