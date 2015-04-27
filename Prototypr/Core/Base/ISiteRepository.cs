using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prototypr.Core.Base
{
    public interface ISiteRepository
    {
        /// <summary>
        /// Find a model based on the given URL path...
        /// Result has to contain the properties (Layout, Url)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        IDataModel FindModel(string path);
        IDictionary<string, string> Permalinks();
    }
}