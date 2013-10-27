﻿using Chuhukon.Prototypr.Core.Base;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace Chuhukon.Prototypr.Core.Models
{
    public class ModelCollection<T> : DynamicObject, IEnumerable<T>
    {
        public string Path { get; private set; }
        public string Layout { get; set; }

        private ISiteRepository Repository { get; set; }
        private IEnumerable<T> Collection;

        public ModelCollection(IEnumerable<T> collection, string path, ISiteRepository repository)
        {
            Collection = collection;
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