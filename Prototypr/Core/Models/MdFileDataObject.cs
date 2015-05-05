using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.IO;
using System.Text.RegularExpressions;
using MarkdownSharp;
using Prototypr.Core.Base;

namespace Prototypr.Core.Models
{
    public class MdFileDataObject : FileDataObject
    {
        private static readonly Markdown Markdown = new Markdown();

        public MdFileDataObject(string path, ISiteRepository rep) : base(path, (new ExpandoObject()) as IDictionary<string, object>, rep)
        {
            Content = MvcHtmlString.Create(Markdown.Transform(File.ReadAllText(path), Source)); //deserialze markdown..
        }

        public MvcHtmlString Content // reserved property for markdown files.
        {
            get; private set; 
        }

        public override string Url
        {
            get { return base.Url.Replace(".md", string.Empty); }
            set { base.Url = value; }
        }

        public override string Layout
        {
            get { return base.Layout.Replace(".md", string.Empty); }
            set { base.Layout = value; }
        }
    }

    internal static class MarkdownExtensions
    {
        /// <summary>
        /// Use MarkdownSharp to transform a markdown file to html, but save the meta data into the given metamodel
        /// </summary>
        /// <param name="?"></param>
        /// <param name="text"></param>
        /// <param name="metamodel">dictionary to put all meta items into </param>
        /// <param name="markdown"></param>
        internal static string Transform(this Markdown markdown, string text, IDictionary<string, object> metamodel)
        {
            var linenr = 0;
            var metamode = false;
            var markdowndata = new StringBuilder();
            var reader = new System.IO.StringReader(text);

            //TODO: we maybe like to use http://www.aaubry.net/page/YamlDotNet

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
