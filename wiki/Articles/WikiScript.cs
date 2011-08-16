using System;
using System.Globalization;
using System.Text;
using Hellgate;

namespace MediaWiki.Articles
{
    public abstract class WikiScript
    {
        protected static FileManager Manager;
        
        protected static String Prefix = "";
        protected static string TableName = string.Empty;
        protected static SQLTableScript TableScript = null;
        protected static string FullTableName
        {
            get { return Prefix + TableName; }
        }

        protected WikiScript(FileManager manager, string tableName = "")
        {
            Manager = manager;
            TableName = tableName;
        }

        /// <summary>
        /// Extracts an article to be pasted into the wiki.
        /// </summary>
        /// <returns></returns>
        public abstract string ExportArticle();

        //virtual for now just so it doesn't break everything
        /// <summary>
        /// Exports a SQL script containing the table schema (including recreating the table) and row inserting
        /// </summary>
        /// <returns></returns>
        public virtual string ExportTableInsertScript()
        {
            throw new NotImplementedException();
        }

        public string ExportSQL()
        {
            return ExportSchema() + "\n" + ExportTable();
        }

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

        protected static string GetImage(string name, int pxWidth, bool isThumb = false, string caption = "")
        {
            if (String.IsNullOrWhiteSpace(name)) return string.Empty;
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append("[[File:");
            builder.Append(name);

            if (pxWidth > 0)
                builder.Append("|" + pxWidth);
            if (isThumb)
                builder.Append("|thumb");
            if (!String.IsNullOrWhiteSpace(caption))
                builder.Append("|" + caption);

            builder.Append("]]");
            return builder.ToString();
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

        protected class SQLTableScript
        {
            private int columnCount = 0;
            private StringBuilder tableBuilder = new StringBuilder();
            private StringBuilder insertBuilder = new StringBuilder();

            public SQLTableScript(string primaryColumn, params string[] columns)
            {
                columnCount = columns.Length;

                tableBuilder.AppendLine("DROP TABLE IF EXISTS " + FullTableName + ";");
                tableBuilder.AppendLine("CREATE TABLE " + FullTableName + " (");
                foreach (string column in columns)
                {
                    tableBuilder.Append("\t" + column);
                    tableBuilder.AppendLine(",");
                }
                if (!String.IsNullOrWhiteSpace(primaryColumn))
                    tableBuilder.AppendLine("\tPRIMARY KEY(" + primaryColumn + ")");

                tableBuilder.AppendLine(");");
            }

            public void AddRow(params string[] values)
            {
                if (values.Length != columnCount)
                    throw new ArgumentException("Wrong number of row values");

                if (insertBuilder.Length == 0)
                {
                    insertBuilder.AppendLine("INSERT INTO " + FullTableName);
                    insertBuilder.AppendLine("VALUES");
                }
                else
                    insertBuilder.AppendLine(",");

                insertBuilder.Append("(");
                for (int i = 0; i < values.Length; i++)
                {
                    insertBuilder.Append(values[i]);
                    if (i < values.Length - 1)
                        insertBuilder.Append(", ");
                }
                insertBuilder.Append(")");
            }

            public string GetTableSchema()
            {
                return tableBuilder.ToString();
            }

            public string GetInsertScript()
            {
                if (insertBuilder.Length > 0)
                    insertBuilder.AppendLine(";");

                string script = insertBuilder.ToString();
                insertBuilder.Clear(); //clear so it doesn't take up memory

                return script;
            }

            public string GetFullScript()
            {
                return GetTableSchema() + GetInsertScript();
            }
        }
    }
}
