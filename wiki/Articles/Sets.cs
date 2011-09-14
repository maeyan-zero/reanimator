using System;
using System.Collections.Generic;
using System.Data;
using Hellgate;
using MediaWiki.Parser;
using MediaWiki.Parser.Class;
using MediaWiki.Util;

namespace MediaWiki.Articles
{
    class Sets : WikiScript
    {
        public Sets(FileManager manager) : base(manager, "item_sets")
        {
            ItemDisplay.Manager = Manager;
        }

        public override string ExportArticle()
        {
            throw new NotImplementedException();
        }

        public override string ExportTableInsertScript()
        {
            var script = new SQLTableScript("id", "", "id INT", "set3 TEXT", "set5 TEXT", "set7 TEXT", "parts TEXT");
            var table = Manager.GetDataTable("ITEM_SETITEM_GROUPS");
            string id, set3, set5, set7, parts;
            foreach (DataRow row in table.Rows)
            {
                id = row["Index"].ToString();
                set3 = GetAffixes((int) row["setAffix1"]);
                set3 = GetSqlString(set3);
                set5 = GetAffixes((int)row["setAffix2"]);
                set5 = GetSqlString(set5);
                set7 = GetAffixes((int)row["setAffix3"]);
                set7 = GetSqlString(set7);
                parts = GetSetParts((int) row["Index"]);
                parts = GetSqlString(parts);

                script.AddRow(id, set3, set5, set7, parts);
            }
            return script.GetFullScript();
        }

        private string GetSetParts(int setId)
        {
            
            var table = Manager.GetDataTable("ITEMS");
            var list = new List<string>();
            foreach (DataRow row in table.Rows)
            {
                if ((int)row["setGroup"] != setId) continue;
                list.Add("[[" + row["String_string"] + "]]");
            }
            var transform = ConcatStrings(list);
            return transform;
        }

        private string GetAffixes(int i)
        {
            if (i == -1) return string.Empty;
            var table = Manager.GetDataTable("AFFIXES");
            var affix = table.Rows[i]["property1"].ToString();
            var evaluator = new Evaluator { Unit = new Item() };
            evaluator.Evaluate(affix);
            var result = ItemDisplay.GetDisplayStrings(evaluator.Unit);
            var transform = ConcatStrings(result);
            return transform;
        }
    }
}
