using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hellgate;
using System.Data;

namespace MediaWiki.Articles
{
    class NewNewTreasure:WikiScript
    {
        public NewNewTreasure(FileManager manager)
            : base(manager, "treasure")
        { }

        public override string ExportArticle()
        {
            throw new NotImplementedException();
        }

        public override string ExportTableInsertScript()
        {
            var script = new SQLTableScript("id", string.Empty,
                                            "id INT NOT NULL",
                                            "loot TEXT");

            string id, loot;
            foreach (DataRow row in Manager.GetDataTable("TREASURE").Rows)
            {
                id = row["Index"].ToString();

                loot = string.Empty;

                script.AddRow(id, loot);
            }

            return script.GetInsertScript();
        }
    }
}
