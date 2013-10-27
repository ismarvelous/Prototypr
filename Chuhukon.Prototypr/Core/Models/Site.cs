using Chuhukon.Prototypr.Core.Base;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;


namespace Chuhukon.Prototypr.Core.Models
{
    public class Site : DynamicObject
    {
        private ISiteRepository Repository;

        public Site(ISiteRepository repository)
        {
            Repository = repository;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = Repository.FindModel(binder.Name);

            //result = string.Format("{{ Site.{0} }}", binder.Name); //default result;
            return true;
        }
    }
}