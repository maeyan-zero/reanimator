using System;
using System.Data;
using System.Diagnostics;
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
                "affixes TEXT",
                "mods TEXT",
                "feeds TEXT",
                "level VARCHAR(3)"
                );

            ItemDisplay.Manager = Manager;
            var items = Manager.GetDataTable("ITEMS");
            var qualityTable = Manager.GetDataTable("ITEM_QUALITY");

            string id, code, name, type, flavor, quality, damage, affixes, modslots, feeds, level;

            foreach (DataRow item in items.Rows)
            {
                if (item["code"].ToString() == "0") continue; // ignore blank rows
                //if (item["Index"].ToString() != "1211") continue;

                Debug.WriteLine("Parsing: " + item["Index"] + " (" + item["name"] + ")");

                id = GetSqlEncapsulatedString(item["index"].ToString());
                code = GetSqlEncapsulatedString(((int)item["code"]).ToString("X"));
                name = GetSqlEncapsulatedString(item["String_string"].ToString());
                type = GetSqlEncapsulatedString(item["typeDescription_string"].ToString());
                flavor = GetSqlEncapsulatedString(item["flavorText_string"].ToString());
                quality = GetSqlEncapsulatedString(((int)item["itemQuality"]) != -1
                              ? qualityTable.Rows[(int) item["itemQuality"]]["displayName_string"].ToString()
                              : string.Empty);
                damage = GetSqlEncapsulatedString(item["tooltipDamageString_string"].ToString());
                affixes = GetAffixes((int) item["index"]);
                modslots = GetModSlots(item["props2"].ToString());
                feeds = GetFeeds((int) item["index"]);
                level = GetSqlEncapsulatedString(item["fixedLevel"].ToString().Replace(";", ""));

                TableScript.AddRow(id, code, name, type, flavor, quality, damage, affixes, modslots, feeds, level);
            }

            return TableScript.GetFullScript();
        }

        private string GetModSlots(string modsScript)
        {
            var evaluator = new Evaluator { Unit = new Item() };
            evaluator.Evaluate(modsScript);
            var strings = ItemDisplay.GetModSlots(evaluator.Unit);
            var concat = ConcatStrings(strings);
            return GetSqlEncapsulatedString(concat);
        }

        private string GetAffixes(int itemid)
        {
            var evaluator = new Evaluator {Unit = new Item(), Manager = Manager, Game3 = new Game3()};
            var item = Manager.GetDataTable("ITEMS").Rows[itemid];
            var table = Manager.GetDataTable("AFFIXES");
            var affixes = item["affix"].ToString();
            var scripts = item["props1"].ToString() + item["props2"] + item["props3"] + item["props4"] + item["props5"];

            var level = item["fixedLevel"].ToString();
            level = level.Replace(";", "");
            int ilevel = 0;
            if (!string.IsNullOrEmpty(level))
            {
                ilevel = int.Parse(level);
                evaluator.Unit.SetStat("level", ilevel);
            }

            string[] strings;
            string concat = string.Empty;

            if (scripts != string.Empty)
            {
                evaluator.Evaluate(scripts);
                strings = ItemDisplay.GetDisplayStrings(evaluator.Unit);
                concat = ConcatStrings(strings);
            }

            evaluator.Unit = new Item();
            evaluator.Unit.SetStat("level", ilevel);

            string[] split = affixes.Split(',');
            foreach (var i in split)
            {
                var affix = int.Parse(i);
                if (affix == -1) break; // no more affixes
                var script = table.Rows[affix]["property1"].ToString();
                script += table.Rows[affix]["property2"].ToString();
                script += table.Rows[affix]["property3"].ToString();
                script += table.Rows[affix]["property4"].ToString();
                script += table.Rows[affix]["property5"].ToString();
                script += table.Rows[affix]["property6"].ToString();
                if (String.IsNullOrEmpty(script)) continue; // no affix script
                evaluator.Evaluate(script);
            }

            strings = ItemDisplay.GetDisplayStrings(evaluator.Unit);
            if (strings.Length != 0) concat += "\n";
            concat += ConcatStrings(strings);

            return GetSqlEncapsulatedString(concat);
        }

        private string ConcatStrings(string[] strings)
        {
            string concat = string.Empty;
            for (int i = 0; i < strings.Length; i++)
            {
                concat += strings[i];
                if (strings.Length != i + 1) concat += "\n";
            }
            return concat;
        }

        private string GetFeeds(int itemid)
        {
            var evaluator = new Evaluator { Unit = new Item(), Manager = Manager };
            var item = Manager.GetDataTable("ITEMS").Rows[itemid];
            var table = Manager.GetDataTable("AFFIXES");
            var affixes = item["affix"].ToString();
            var scripts = item["perLevelProps1"].ToString();
            scripts += item["perLevelProps2"].ToString();

            var level = item["fixedLevel"].ToString();
            level = level.Replace(";", "");
            if (!string.IsNullOrEmpty(level))
            {
                var ilevel = int.Parse(level);
                evaluator.Unit.SetStat("level", ilevel);
            }

            string[] split = affixes.Split(',');
            foreach (var i in split)
            {
                var affix = int.Parse(i);
                if (affix == -1) break; // no more affixes
                var script = table.Rows[affix]["property1"].ToString();
                script += table.Rows[affix]["property2"].ToString();
                script += table.Rows[affix]["property3"].ToString();
                script += table.Rows[affix]["property4"].ToString();
                script += table.Rows[affix]["property5"].ToString();
                script += table.Rows[affix]["property6"].ToString();
                if (String.IsNullOrEmpty(script)) continue; // no affix script
                evaluator.Evaluate(script);
            }

            if (scripts != string.Empty) evaluator.Evaluate(scripts);

            var strings = ItemDisplay.GetFeedCosts(evaluator.Unit);
            var concat = ConcatStrings(strings);
            return GetSqlEncapsulatedString(concat);
        }
    }
}
