using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Prototypr.Core.Base;
using Prototypr.Core.Models;

namespace Prototypr.Infrastructure
{
    public static class Extensions
    {
        public static dynamic AsModelCollection<TSource>(this IEnumerable<TSource> files, string path,
            ISiteRepository repository)
        {
            var collection = new ModelCollection<TSource>(files, path, repository);
            return collection;
        }
    }
}