using System;
using System.Collections;
using System.Dynamic;
using System.Linq;
using Prototypr.Core;
using Prototypr.Core.Models;

namespace Prototypr.Files.Models
{
    public class FileCollection : Base, IEnumerable //todo: rename filecollection
    {
        public override bool IsNull
        {
            get { return false; }
        }

        public FileCollection(string path, IFileDataRepository dataRepository)
            : base(path, new ExpandoObject(), dataRepository)
        {
        }

        public IEnumerator GetEnumerator()
        {
            return Source.Where(s => s.Value is IDataModel).Select(kv => kv.Value).GetEnumerator();
        }

        public void Add(FileBase itm)
        {
            Source.Add(itm.Name, itm);
        }
    }
}