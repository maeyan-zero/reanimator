using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Hellgate;

namespace MediaWiki.Articles
{
    class ItemQuality : WikiScript
    {
        public ItemQuality(FileManager manager)
            : base(manager, "item_quality")
        {
        }

        public override string ExportArticle()
        {
            throw new NotImplementedException();
        }

        public override string ExportTableInsertScript()
        {
            var script = new SQLTableScript("id", "code",
                                            "id INT",
                                            "code VARCHAR(4)",
                                            "name TEXT",
                                            "name_string TEXT",
                                            "rarity INT",
                                            "nightmare_rarity INT",
                                            "hell_rarity INT",
                                            "vendor_rarity INT",
                                            "luck_rarity INT",
                                            "gambling_rarity INT",
                                            "success_rate INT",
                                            "nanoshard_chance INT",
                                            "quality_level INT",
                                            "proc_chance INT");

            string id,
                   code,
                   name,
                   nameString,
                   rarity,
                   nightmareRarity,
                   hellRarity,
                   vendorRarity,
                   luckRarity,
                   gamblingRarity,
                   successRate,
                   nanoshardChance,
                   qualityLevel,
                   procChange;

            var data = Manager.GetDataTable("ITEM_QUALITY");
            foreach (DataRow row in data.Rows)
            {
                id = row["Index"].ToString();

                name = row["quality"].ToString();
                name = GetSqlString(name);
            }

            return script.GetFullScript();
        }
    }
}
