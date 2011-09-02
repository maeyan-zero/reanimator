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
                if (id == "13") continue;//core
                if (id == "11") continue;//powerup
                if (id == "4") continue;//mutant
                if (id == "9") continue;//mutant-mod
                if (id == "10") continue;//unique-mod

                code = row["code"].ToString();
                code = GetSqlString(code);

                name = row["quality"].ToString();
                name = GetFormattedString(name);
                name = name.Replace("-M", " Mod");
                name = GetSqlString(name);

                //nameString = row["displayName_string"].ToString();
                //nameString = GetSqlString(nameString);

                rarity = row["rarity"].ToString();
                nightmareRarity = row["nightmareRarity"].ToString();
                hellRarity = row["hellRarity"].ToString();
                vendorRarity = row["vendorRarity"].ToString();
                luckRarity = row["luckRarity"].ToString();
                gamblingRarity = row["gamblingRarity"].ToString();
                successRate = row["successRate"].ToString();
                nanoshardChance = row["extraScrapChance"].ToString();
                qualityLevel = row["qualityLevel"].ToString();
                procChange = row["procChance"].ToString();

                script.AddRow(id, code, name, rarity, nightmareRarity, hellRarity, vendorRarity, luckRarity, gamblingRarity, successRate, nanoshardChance, qualityLevel, procChange);
            }

            return script.GetFullScript();
        }
    }
}
