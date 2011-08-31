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
    class ItemLevels:WikiScript
    {
        public ItemLevels(FileManager manager)
            : base(manager, "item_levels")
        { }

        public override string ExportArticle()
        {
            throw new NotImplementedException();
        }

        public override string ExportTableInsertScript()
        {
            SQLTableScript table = new SQLTableScript("id", string.Empty,
                "id INT NOT NULL",
                "level INT NOT NULL",
                "sfx_attack INT NOT NULL",
                "sfx_defense INT NOT NULL",
                "feed INT NOT NULL",
                "clvl INT NOT NULL",
                "ilvl_range TEXT NOT NULL",
                "aug_common TEXT NOT NULL",
                "aug_rare TEXT NOT NULL",
                "aug_legendary TEXT NOT NULL",
                "scrap_quantity INT NOT NULL",
                "specialScrap_quantity INT NOT NULL"
            );

            var ilvls = Manager.GetDataTable("ITEM_LEVELS");

            string id, level, sfxAtk, sfxDef, feed, clvl, ilvlRange, augC, augR, augL, scraps, specScraps;

            foreach (DataRow row in ilvls.Rows)
            {
                id = row["Index"].ToString();
                level = row["level"].ToString();
                sfxAtk = row["sfxAttackAbility"].ToString();
                sfxDef = row["sfxDefenceAbility"].ToString();
                feed = row["feed"].ToString();
                clvl = row["levelRequirement"].ToString();
                ilvlRange = GetSqlEncapsulatedString(row["itemLevelMin"].ToString() + "-" + row["itemLevelMax"].ToString());
                augC = GetSqlEncapsulatedString(((int)row["augmentCostCommon"]).ToString("0,0"));
                augR = GetSqlEncapsulatedString(((int)row["augmentCostRare"]).ToString("0,0"));
                augL = GetSqlEncapsulatedString(((int)row["augmentCostLegendary"]).ToString("0,0"));
                scraps = row["scrapUpgradeQuantity"].ToString();
                specScraps = row["specialScrapUpgradeQuantity"].ToString();

                table.AddRow(id, level, sfxAtk, sfxDef, feed, clvl, ilvlRange, augC, augR, augL, scraps, specScraps);
            }

            return table.GetFullScript();
        }
    }
}
