using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chuhukon.Prototypr.Mvc
{
    public class PrototyprViewPage<TModel> : WebViewPage<TModel>
    {
        public SiteHelper<TModel> Site
        {
            get
            {
                return new SiteHelper<TModel>();
            }
        }
        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}