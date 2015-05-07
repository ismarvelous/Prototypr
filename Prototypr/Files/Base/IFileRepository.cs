using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prototypr.Core.Base;

namespace Prototypr.Files.Base
{
    public interface IFileDataRepository : IDataRepository
    {
        string DataPath { get; }
    }
}
