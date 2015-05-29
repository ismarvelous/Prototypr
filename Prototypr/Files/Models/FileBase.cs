using System.Collections.Generic;

namespace Prototypr.Files.Models
{
    public abstract class FileBase : Base
    {
        protected string FileExtension { get; set; }

        protected FileBase(string path, IDictionary<string, object> source, IFileDataRepository rep) : 
            base(path, source, rep)
        {

        }

        private string _name;
        /// <summary>
        /// Filename without extesnions, used as property name in collections..
        /// </summary>
        public virtual string Name
        {
            get
            {
                if (_name != null)
                    return _name;

                var fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(Path);
                if (fileNameWithoutExtension != null)
                    _name = fileNameWithoutExtension.ToLower().Replace(" ","_");

                return _name;
            }
        }
    }
}
