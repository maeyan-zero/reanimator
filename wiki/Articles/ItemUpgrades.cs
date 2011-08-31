using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Hellgate;

namespace MediaWiki.Articles
{
    class ItemUpgrades : WikiScript
    {
        public ItemUpgrades(FileManager manager) : base(manager,"item_upgrades")
        {
        }

        public override string ExportArticle()
        {
            WikiTable table = new WikiTable(false, string.Empty, string.Empty,
                "Upgrades",
                "Success Rate",
                "Nanoshards",
                "Damage*",
                "Shields",
                "Armor",
                "Feed Cost**"
                );

            DataTable itemUpgrade = Manager.GetDataTable("ITEMUPGRADE");
            string style = "style=\"text-align:center\"| ";
            foreach (DataRow row in itemUpgrade.Rows)
            {
                table.AddRow(
                    style + row["Index"].ToString(),
                    style + "+" + row["damageMult"].ToString() + "%",
                    style + "+" + row["shields"].ToString(),
                    style + "+" + row["armor"].ToString(),
                    style + "+" + row["feed"].ToString()
                    );
            }

            return table.GetTableSyntax() + "\n\n" +
                @"<nowiki>* This number isn't a direct increase, but is used in a calculation that also takes item level and rarity into account</nowiki><br />
                  <nowiki>** The increased cost is split between each feed on the item</nowiki>";
        }

        public override string ExportTableInsertScript()
        {
            SQLTableScript table = new SQLTableScript("id", string.Empty,
                "id INT NOT NULL",
                "damage_mult INT NOT NULL",
                "shields INT NOT NULL",
                "armor INT NOT NULL",
                "feed INT NOT NULL",
                "required_nanoshards INT NOT NULL",
                "success_rate INT NOT NULL"
                );

            DataTable itemUpgrade = Manager.GetDataTable("ITEMUPGRADE");
            foreach (DataRow row in itemUpgrade.Rows)
            {
                table.AddRow(
                    row["Index"].ToString(),
                    row["damageMult"].ToString(),
                    row["shields"].ToString(),
                    row["armor"].ToString(),
                    row["feed"].ToString(),
                    row["requiredNanoshardsa"].ToString(),
                    row["successRate"].ToString()
                    );
            }

            return table.GetFullScript();
        }
    }
}
