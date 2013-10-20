using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Chuhukon.Markdown.Extensions
{
    public static class Metadata
    {
        /// <summary>
        /// Use MarkdownSharp to transform a markdown file to html, but save the meta data into the given metamodel
        /// </summary>
        /// <param name="?"></param>
        /// <param name="path">path of the markdown file </param>
        /// <param name="metamodel">dictionary to put all meta items into </param>
        /// <param name="linenumber">The linenumber where the actual content starts</param>
        public static string Transform(this MarkdownSharp.Markdown markdown, string text, IDictionary<string, object> metamodel)
        {
            int linenr = 0;
            bool metamode = false;
            StringBuilder markdowndata = new StringBuilder();
            System.IO.StringReader reader = new System.IO.StringReader(text);

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
