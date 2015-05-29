using System.Dynamic;

namespace Prototypr.Core
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
