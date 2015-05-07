using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Dynamic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using MarkdownSharp;
using Prototypr.Core.Models;
using Prototypr.Files.Base;

namespace Prototypr.Files.Models
{
    public class MdFile : FileBase
    {
        private static readonly Markdown Markdown = new Markdown();

        public MdFile(string path, IFileDataRepository rep) : base(path, Markdown.Transform(new StringReader(File.ReadAllText(path))), rep)
        {
            Initialize();
        }

        public void Initialize()
        {
            FileExtension = ".md";
            Url = Url.Replace(FileExtension, string.Empty);
            Layout = Layout.Replace(FileExtension, string.Empty);
        }

        public override bool IsNull
        {
            get { return false; }
        }
    }

    internal static class MarkdownExtensions
    {
        /// <summary>
        /// Use MarkdownSharp to transform a markdown file to html, but save the meta data into the given metamodel
        /// </summary>
        /// <param name="markdown"></param>
        /// <param name="reader"></param>
        internal static IDictionary<string, object> Transform(this Markdown markdown, TextReader reader)
        {
            IDictionary<string, object> metamodel = new ExpandoObject();

            var linenr = 0;
            var metamode = false;
            var markdowndata = new StringBuilder();

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
                        if (metamodel.ContainsKey(property))
                            metamodel[property] = value;
                        else
                            metamodel.Add(property, value);
                    }
                }
                linenr++;
            }

            metamodel.Add("Content", MvcHtmlString.Create(markdown.Transform(reader.ReadToEnd())));

            return metamodel;
        }
    }
}
