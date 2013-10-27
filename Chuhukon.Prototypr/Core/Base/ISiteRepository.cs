using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chuhukon.Prototypr.Core.Base
{
    public interface ISiteRepository
    {
        dynamic FindModel(string path);
    }
}