using System.Dynamic;

namespace Prototypr.Files.Models
{
    public class Site : DynamicObject
    {
        protected readonly IFileDataRepository DataRepository;

        public Site(IFileDataRepository dataRepository)
        {
            DataRepository = dataRepository;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = DataRepository.FindModel(binder.Name);

            //result = string.Format("{{ Site.{0} }}", binder.Name); //default result;
            return true;
        }
    }
}