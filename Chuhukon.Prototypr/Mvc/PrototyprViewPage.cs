using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chuhukon.Prototypr.Mvc
{
    public class PrototyprViewPage<TModel> : WebViewPage<TModel>
    {
        private dynamic _site;

        /// <summary>
        /// Gets the Site item of the associated ViewDataDictionary class
        /// </summary>
        public dynamic Site
        {
            get
            {
                if(_site == null)
                {
                    _site = ViewData["Site"];
                }

                return _site;
            }
        }
        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}