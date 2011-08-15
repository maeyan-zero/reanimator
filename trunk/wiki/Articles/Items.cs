using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        public override string ExportSchema()
        {
            throw new NotImplementedException();
        }

        public override string ExportTable()
        {
            var builder = new StringBuilder();
            var items = Manager.GetDataTable("ITEMS");
            var affixTable = Manager.GetDataTable("AFFIXES");
            var evaluator = new Evaluator();
            var unit = new Unit();
            evaluator.Unit = unit;
            ItemDisplay.Manager = Manager;
            int animater = 1261;
            string affixes = (string) items.Rows[animater]["affix"];
            string[] split = affixes.Split(',');
            foreach (var t in split)
            {
                var affix = int.Parse(t);
                if (affix == -1) break;
                var script = (string) affixTable.Rows[affix]["property1"];
                if (String.IsNullOrEmpty(script)) continue;
                evaluator.Evaluate(script);
            }

            var affixStrings = ItemDisplay.GetDisplayStrings(unit);


            return builder.ToString();
        }
    }
}
