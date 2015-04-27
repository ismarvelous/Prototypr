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
        public static dynamic AsModelCollection<TSource>(this IEnumerable<TSource> files, string path, ISiteRepository repository)
        {
            var collection = new ModelCollection<TSource>(files, path, repository);
            return collection;
        }

        /// <summary>
        /// Use MarkdownSharp to transform a markdown file to html, but save the meta data into the given metamodel
        /// </summary>
        /// <param name="?"></param>
        /// <param name="text"></param>
        /// <param name="metamodel">dictionary to put all meta items into </param>
        /// <param name="markdown"></param>
        public static string Transform(this MarkdownSharp.Markdown markdown, string text, IDictionary<string, object> metamodel)
        {
            var linenr = 0;
            var metamode = false;
            var markdowndata = new StringBuilder();
            var reader = new System.IO.StringReader(text);

            //TODO: use http://www.aaubry.net/page/YamlDotNet

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Equals("---") && linenr == 0)
                {
                    metamode = true;
                }
                else if (line.Equals("---") && linenr > 0)
                {
                    if (!metamode)
                    {
                        markdowndata.Append(line);
                    }

                    break; //stop excecuting!
                }
                else if (metamode)
                {
                    var match = Regex.Match(line, @"^([A-z0-9_ ]+)\:(.*)$");

                    if (match.Success)
                    {
                        var property = match.Groups[1].Value.Replace(" ", string.Empty);
                        var value = match.Groups[2].Value.TrimStart().TrimEnd();

                        //todo: build-in support for boolean, data time etc..
                        metamodel.Add(property, value);
                    }
                }
                linenr++;
            }

            return markdown.Transform(reader.ReadToEnd());
        }
    }
}