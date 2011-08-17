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
            foreach (DataRow row in itemUpgrade.Rows)
            {
                table.AddRow(
                    row["Index"].ToString(),
                    "+" + row["damageMult"].ToString() + "%",
                    "+" + row["shields"].ToString(),
                    "+" + row["armor"].ToString(),
                    "+" + row["feed"].ToString()
                    );
            }

            return table.GetTableSyntax();
        }

        public override string ExportTableInsertScript()
        {
            TableScript = new SQLTableScript("id", string.Empty,
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
                TableScript.AddRow(
                    row["Index"].ToString(),
                    row["damageMult"].ToString(),
                    row["shields"].ToString(),
                    row["armor"].ToString(),
                    row["feed"].ToString(),
                    row["requiredNanoshardsa"].ToString(),
                    row["successRate"].ToString()
                    );
            }

            return TableScript.GetFullScript();
        }
        [Obsolete("This class is now using ExportTableInsertScript instead")]
        public override string ExportSchema()
        {
            var schema = "CREATE TABLE " + Prefix + "item_upgrades (\n" +
                         "\tid INT NOT NULL,\n" +
                         "\tdamage_mult INT NOT NULL,\n" +
                         "\tshields INT NOT NULL,\n" +
                         "\tarmor INT NOT NULL,\n" +
                         "\tfeed INT NOT NULL,\n" +
                         "\trequired_nanoshards INT NOT NULL,\n" +
                         "\tsuccess_rate INT NOT NULL,\n" +
                         "\tPRIMARY KEY(id)\n" +
                         ");";
            return schema;
        }
        [Obsolete("This class is now using ExportTableInsertScript instead")]
        public override string ExportTable()
        {
            DataTable itemUpgrade = Manager.GetDataTable("ITEMUPGRADE");
            var builder = new StringBuilder();
            builder.AppendLine("INSERT INTO " + Prefix + "item_upgrades");
            builder.AppendLine("VALUES");
            foreach (DataRow row in itemUpgrade.Rows)
            {
                builder.Append("(");
                builder.Append(row["Index"] + ", ");
                builder.Append(row["damageMult"] + ", ");
                builder.Append(row["shields"] + ", ");
                builder.Append(row["armor"] + ", ");
                builder.Append(row["feed"] + ", ");
                builder.Append(row["requiredNanoshardsa"] + ", ");
                builder.Append("\"" + row["successRate"] + "%\"");
                builder.AppendLine("),");
            }
            builder.Remove(builder.Length - 3, 3);
            builder.AppendLine(";");
            return builder.ToString();
        }
    }
}
