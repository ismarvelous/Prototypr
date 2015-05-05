using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Prototypr.Core.Base;

namespace Prototypr.Core.Models
{
    public class NullDataObject : BaseDataObject
    {
        public NullDataObject(ISiteRepository rep) : base (new ExpandoObject(), rep)
        {
        }

        public override bool IsNull
        {
            get { return true; }
        }
    }
}
