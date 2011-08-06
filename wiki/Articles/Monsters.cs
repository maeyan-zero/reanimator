using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Hellgate;

namespace MediaWiki.Articles
{
    class Monsters : WikiScript
    {
        public Monsters(FileManager manager) : base(manager)
        {
        }

        public override string ExportArticle()
        {
            throw new NotImplementedException();
        }

        public override string ExportSchema()
        {
            var builder = new StringBuilder();
            builder.AppendLine("CREATE TABLE monsters (");
            builder.Append("\t");
            builder.AppendLine("id INT NOT NULL,");
            builder.Append("\t");
            builder.AppendLine("code VARCHAR(4) NOT NULL,");
            builder.Append("\t");
            builder.AppendLine("name TEXT,");
            builder.Append("\t");
            builder.AppendLine("base TEXT,");
            builder.Append("\t");
            builder.AppendLine("quality TEXT,");
            builder.Append("\t");
            builder.AppendLine("hp_min INT,");
            builder.Append("\t");
            builder.AppendLine("hp_max INT,");
            builder.Append("\t");
            builder.AppendLine("experience INT,");
            builder.Append("\t");
            builder.AppendLine("treasure INT,");
            builder.Append("\t");
            builder.AppendLine("treasure_champion INT,");
            builder.Append("\t");
            builder.AppendLine("treasure_first INT,");
            builder.Append("\t");
            builder.AppendLine("PRIMARY KEY (id),");
            builder.Append("\t");
            builder.AppendLine("INDEX (code)");
            builder.AppendLine(");");
            return builder.ToString();
        }

        public override string ExportTable()
        {
            var monsters = Manager.GetDataTable("MONSTERS");
            var builder = new StringBuilder();
            builder.AppendLine("INSERT INTO monsters");
            builder.AppendLine("VALUES");
            foreach (DataRow row in monsters.Rows)
            {
                builder.Append("\t");
                builder.Append("(");
                builder.Append(row["Index"]);
                builder.Append(",");
                builder.Append(GetSqlEncapsulatedString(((int)row["code"]).ToString("X")));
                builder.Append(",");
                builder.Append(GetSqlEncapsulatedString(row["String_string"] as string));
                builder.Append(",");
                var baseMonster = ((int) row["baseRow"] != -1 ? monsters.Rows[(int) row["baseRow"]]["String_string"] as string : "") ?? string.Empty;
                baseMonster = GetWikiArticleLink(baseMonster);
                baseMonster = GetSqlEncapsulatedString(baseMonster);
                builder.Append(baseMonster);
                builder.Append(",");
                var monsterQuality = (int) row["monsterQuality"] != -1 ? (string) row["monsterQuality_string"] : string.Empty;
                monsterQuality = GetWikiArticleLink(monsterQuality);
                monsterQuality = GetSqlEncapsulatedString(monsterQuality);
                builder.Append(monsterQuality);
                builder.Append(",");
                builder.Append(row["hpMin"]);
                builder.Append(",");
                builder.Append(row["hpMax"]);
                builder.Append(",");
                builder.Append(row["experience"]);
                builder.Append(",");
                builder.Append(row["treasure"]);
                builder.Append(",");
                builder.Append(row["championTreasure"]);
                builder.Append(",");
                builder.Append(row["firstTimeTreasure"]);
                builder.AppendLine("),");
            }
            builder.Remove(builder.Length - 3, 3);
            builder.AppendLine(";");
            return builder.ToString();
        }
    }
}
