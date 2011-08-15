
using System;
using System.Globalization;
using Hellgate;

namespace MediaWiki.Articles
{
    public abstract class WikiScript
    {
        protected static FileManager Manager;
        protected static String Prefix = "";

        protected WikiScript(FileManager manager)
        {
            Manager = manager;
        }

        /// <summary>
        /// Extracts an article to be pasted into the wiki.
        /// </summary>
        /// <returns></returns>
        public abstract string ExportArticle();

        /// <summary>
        /// Exports a database schema for storing our hellgate data.
        /// </summary>
        /// <returns></returns>
        public abstract string ExportSchema();

        /// <summary>
        /// Exports a sql script to insert all of the data into the generated schema.
        /// </summary>
        /// <returns></returns>
        public abstract string ExportTable();

        /// <summary>
        /// Formats a string as a wikimedia link.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected static string GetWikiArticleLink(string name)
        {
            if (name.Equals("")) return string.Empty;
            return "[[" + name + "]]";
        }

        protected static string GetWikiTab(int depth)
        {
            var tab = "";
            for (var i = 0; i < depth; i++) tab += ":";
            return tab;
        }

        protected static string GetFormattedString(string raw)
        {
            var result = raw.Replace("_", " ");
            result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result);
            return result;
        }

        protected static string GetWikiListIndentation(int depth)
        {
            var result = String.Empty;
            for (var i = 0; i < depth; i++)
            {
                result += "*";
            }
            return result;
        }

        protected static string GetSqlEncapsulatedString(string raw)
        {
            if (raw == null) return string.Empty;
            var output = raw.Replace("\"", "\\\"");
            return "\"" + output + "\"";
        }
    }
}
