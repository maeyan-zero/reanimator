using System;
using System.Data;
using System.Text;
using Hellgate;
using MediaWiki.Parser;
using MediaWiki.Parser.Class;
using MediaWiki.Util;

namespace MediaWiki.Articles
{
    class Items : WikiScript
    {
        public Items(FileManager manager) : base(manager)
        {
        }

        public override string ExportArticle()
        {
            throw new NotImplementedException();
        }

        public override string ExportTableInsertScript()
        {
            TableScript = new SQLTableScript("id", "code",
                "id INT NOT NULL",
                "code VARCHAR(4) NOT NULL",
                "name TEXT",
                "type TEXT",
                "flavor TEXT",
                "quality TEXT",
                "damage TEXT",
                "affixes TEXT"
                );

            ItemDisplay.Manager = Manager;
            var items = Manager.GetDataTable("ITEMS");
            var qualityTable = Manager.GetDataTable("ITEM_QUALITY");

            string id, code, name, type, flavor, quality, damage, affixes;

            foreach (DataRow item in items.Rows)
            {
                if (item["code"].ToString() == "0") continue; // ignore blank rows

                id = GetSqlEncapsulatedString(item["index"].ToString());
                code = GetSqlEncapsulatedString(((int)item["code"]).ToString("X"));
                name = GetSqlEncapsulatedString(item["String_string"].ToString());
                type = GetSqlEncapsulatedString(item["typeDescription_string"].ToString());
                flavor = GetSqlEncapsulatedString(item["flavorText_string"].ToString());
                quality = GetSqlEncapsulatedString(((int)item["itemQuality"]) != -1
                              ? qualityTable.Rows[(int) item["itemQuality"]]["displayName_string"].ToString()
                              : string.Empty);
                damage = GetSqlEncapsulatedString(item["tooltipDamageString_string"].ToString());
                affixes = GetAffixes(item["affix"].ToString());

                TableScript.AddRow(id, code, name, type, flavor, quality, damage, affixes);
            }

            return TableScript.GetFullScript();
        }

        private string GetAffixes(string affixes)
        {
            var evaluator = new Evaluator { Unit = new Item() };

            var table = Manager.GetDataTable("AFFIXES");

            string[] split = affixes.Split(',');
            foreach (var i in split)
            {
                var affix = int.Parse(i);
                if (affix == -1) break; // no more affixes
                var script = table.Rows[affix]["property1"].ToString();
                if (String.IsNullOrEmpty(script)) continue; // no affix script
                evaluator.Evaluate(script);
            }

            var affixStrings = ItemDisplay.GetDisplayStrings(evaluator.Unit);
            string concat = string.Empty;
            for (int i = 0; i < affixStrings.Length; i++)
            {
                concat += affixStrings[i];
                if (affixStrings.Length != i + 1) concat += "\n";
            }

            return GetSqlEncapsulatedString(concat);
        }
    }
}
