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
        public ItemUpgrades(FileManager manager) : base(manager)
        {
        }

        public override string ExportArticle()
        {
            throw new NotImplementedException();
        }

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
