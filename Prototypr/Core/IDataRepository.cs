using System.Collections.Generic;

namespace Prototypr.Core
{
    public interface IDataRepository
    {
        /// <summary>
        /// Find a model based on the given key.
        /// </summary>
        /// <param name="key">unique key with which you can find the IDataModel.</param>
        /// <returns></returns>
        IDataModel FindModel(string key);
        IEnumerable<IDataModel> FindAll();
    }
}