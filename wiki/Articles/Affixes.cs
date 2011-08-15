using System;
using System.Text;
using Hellgate;

namespace MediaWiki.Articles
{
    class Affixes : WikiScript
    {
        public Affixes(FileManager manager) : base(manager)
        {
        }

        public override string ExportArticle()
        {
            throw new NotImplementedException();
        }

        public override string ExportSchema()
        {
            throw new NotImplementedException();
        }

        public override string ExportTable()
        {
            var builder = new StringBuilder();
            var items = Manager.GetDataTable("ITEMS");
            var affixes = Manager.GetDataTable("AFFIXES");

            return builder.ToString();
        }
    }
}
