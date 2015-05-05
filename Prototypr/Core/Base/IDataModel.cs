using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototypr.Core.Base
{
    public interface IDataModel : IDynamicMetaObjectProvider
    {
        string Url { get; set; }

        /// <summary>
        /// Layout path, used to select the correct view.
        /// </summary>
        string Layout { get; set; }

        bool IsNull { get; }
    }
}
