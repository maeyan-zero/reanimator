using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Hellgate;

using ClassType = MediaWiki.Util.Treasure.ClassType;

using TreasureClass = MediaWiki.Util.Treasure;

namespace MediaWiki.Articles
{
    public class NewTreasure : WikiScript
    {
        
        private const int ColBuffer = 15;



        public NewTreasure(FileManager manager)
            : base(manager, "treasure")
        {
            TreasureClass.Manager = Manager;
        }

        public override string ExportArticle()
        {
            throw new NotImplementedException();
        }

        public override string ExportTableInsertScript()
        {
            var script = new SQLTableScript("id", string.Empty,
                                            "id NOT NULL",
                                            "loot TEXT");

            string id, loot;

            foreach (DataRow row in Manager.GetDataTable("TREASURE").Rows)
            {
                var treasure = new TreasureClass(row);
                treasure.Simplify();

                id = row["Index"].ToString();
                loot = GetHtmlTable(treasure);

                script.AddRow(id, loot);
            }

            return script.GetInsertScript();
        }

        private string GetHtmlTable(TreasureClass treasure)
        {
            var builder = new StringBuilder();
            var depth = 1;
            const string style = "style=\"vertical-align: top; width: auto; border-collapse: collapse; border: solid 1px black;\"";
            builder.AppendLine("<table border=\"1\" cellpadding=\"3\" " + style + ">");
            GetTableRows(treasure, builder, ref depth);
            builder.AppendLine("</table>");
            return builder.ToString();
        }

        internal void GetTableRows(TreasureClass treasure, StringBuilder builder, ref int depth)
        {
            var colSpan = GetColSpan(depth);
            var rowName = GetFormattedString(treasure.Name);
            var rowItems = GetRowItems(treasure);
            var rowRules = GetRowRules(treasure);
            var hasContent = HasContent(treasure);
            var difficuly = GetDifficultyDropRate(treasure);

            if (depth != 1)
            {
                builder.AppendLine("<tr>");
                //builder.AppendLine("<td colspan=\"" + depth + "\"></td>");
                // colspan=\"" + (colSpan + 1) + "\"
                builder.AppendLine("<td colspan=\"3\" " + GetRowStyle(depth) + "><span " +  GetTitleStyle(depth) + ">" + rowName + "</span>" + ((difficuly != string.Empty) ? "<br/>" + difficuly : "") + "</td>");
                builder.AppendLine("</tr>");
            }

            depth++;

            if (hasContent)
            {
                colSpan = GetColSpan(depth);

                builder.AppendLine("<tr style=\"vertical-align: top;\">");
                builder.AppendLine("<td>");

                // Actual drops
                builder.AppendLine("<ul>");
                foreach (var item in rowItems)
                    builder.AppendLine("<li>" + item.Key + "</li>");
                builder.AppendLine("</ul>");

                builder.AppendLine("</td>");
                builder.AppendLine("<td>");

                // Drop rates
                builder.AppendLine("<ul style=\"list-style: none; text-align: right;\">");
                foreach (var item in rowItems)
                    builder.AppendLine("<li>" + item.Value + "</li>");
                builder.AppendLine("</ul>");

                builder.AppendLine("</td>");
                builder.AppendLine("<td>");

                // Any rules
                builder.AppendLine("<ul>");
                foreach (var rule in rowRules)
                    builder.AppendLine("<li>" + rule + "</li>");
                builder.AppendLine("</ul>");

                builder.AppendLine("</td>");
                builder.AppendLine("</tr>");
            }

            // Get Embedded classes
            foreach (var drop in treasure.Drops)
            {
                if (drop.Type != ClassType.Treasure) continue;
                GetTableRows((TreasureClass) drop.Content, builder, ref depth);
            }

            depth--;
        }

        internal string GetRowStyle(int depth)
        {
            switch (depth)
            {
                case 1:
                    return "style=\"background-color: #3399FF; text-align:center;\"";
                case 2:
                    return "style=\"background-color: #3399FF;\"";
                default:
                    return string.Empty;
            }
        }

        internal string GetTitleStyle(int depth)
        {
            switch (depth)
            {
                case 1:
                    return "style=\"font-size: 1.35em; font-weight: bold;\"";
                case 2:
                    return "style=\"font-size: 1.15em; text-decoration: underline;\"";
                default:
                    return string.Empty;
            }
        }

        internal string GetDifficultyDropRate(TreasureClass treasure)
        {
            if (treasure.NormalChance == -1 && treasure.NightmareChance == -1 && treasure.HellChance == -1)
            {
                if (treasure.Difficulty == 0 && treasure.NoDrop != 100) return "Normal: " + treasure.NoDrop + "%";
                if (treasure.Difficulty == 1 && treasure.NoDrop != 100) return "Nightmare: " + treasure.NoDrop + "%";
                if (treasure.Difficulty == 2 && treasure.NoDrop != 100) return "Hell: " + treasure.NoDrop + "%";

                if (treasure.NoDrop != 100) return treasure.NoDrop + "%";
                return string.Empty;
            }
                

            string difficulty = string.Empty;
            difficulty += "Normal: " + ((treasure.NormalChance != -1) ? treasure.NormalChance : 0) + "%";
            difficulty += ", ";
            difficulty += "Nightmare: " + ((treasure.NightmareChance != -1) ? treasure.NightmareChance : 0) + "%";
            difficulty += ", ";
            difficulty += "Hell: " + ((treasure.HellChance != -1) ? treasure.HellChance : 0) + "%";
            //difficulty = "(" + difficulty + ")";
            return difficulty;
        }

        internal bool HasContent(TreasureClass treasure)
        {
            return treasure.Drops.Any(drop => drop.Type == ClassType.Item || drop.Type == ClassType.Unit || drop.Type == ClassType.Quality);
        }

        internal int GetRowSpan(TreasureClass treasure)
        {
            return treasure.Drops.Count(drop => drop.Type == ClassType.Item || drop.Type == ClassType.Unit);
        }

        internal int GetColSpan(int depth)
        {
            return (ColBuffer - depth);
        }

        internal List<KeyValuePair<string, string>> GetRowItems(TreasureClass treasure)
        {
            var items = new List<KeyValuePair<string, string>>();
            foreach (var drop in treasure.Drops)
            {
                if (drop.Type != ClassType.Item && drop.Type != ClassType.Unit && drop.Type != ClassType.Nothing) continue;
                switch (treasure.PickType)
                {
                    case TreasureClass.PickTypes.IndPercent:
                        items.Add(new KeyValuePair<string, string>(drop.Content.ToString(), drop.Value.ToString("F") + "%"));
                        break;
                    case TreasureClass.PickTypes.All:
                        items.Add(new KeyValuePair<string, string>(drop.Content.ToString(), GetDropChance(drop.Value, treasure.Drops).ToString("F") + "%"));
                        break;
                    default:
                        items.Add(new KeyValuePair<string, string>(drop.Content.ToString(), string.Empty));
                        break;
                }
            }
            return items;
        }

        internal double GetDropChance(int value, IList<TreasureClass.Drop> drops)
        {
            double total = drops.Sum(drop => drop.Value);
            return ((value / total) * 100);
        }

        internal IList<string> GetRowRules(TreasureClass treasure)
        {
            var rules = new List<string>();
            var quality = GetQualityRules(treasure);
            var spawn = GetSpawnConditions(treasure);

            if (quality != string.Empty) rules.Add(quality);
            rules.AddRange(spawn);

            return rules;
        }

        internal string GetQualityRules(TreasureClass treasure)
        {
            var include = new List<string>();
            var exclude = new List<string>();
            foreach (var drop in treasure.Drops)
            {
                if (drop.Type != ClassType.Quality) continue;
                if (drop.Value == 1)
                    exclude.Add(drop.Content.ToString());
                else
                    include.Add(drop.Content.ToString());
            }

            if (include.Count == 0 && exclude.Count == 0) return string.Empty;

            var rules = (include.Count != 0) ?
                "Includes " + GetQualities(include) :
                "Excludes " + GetQualities(exclude);
            return rules + " quality items.";
        }

        internal IList<string> GetSpawnConditions(TreasureClass treasure)
        {
            TreasureClass.Bitmask01 conditions = treasure.SpawnCondition;
            var output = new List<string>();
            if ((conditions & TreasureClass.Bitmask01.BaseOnPlayerLevel) == TreasureClass.Bitmask01.BaseOnPlayerLevel)
                output.Add("Results based on player level.");
            if ((conditions & TreasureClass.Bitmask01.RequiredUsableByOperator) == TreasureClass.Bitmask01.RequiredUsableByOperator)
                output.Add("Result must be useable by the player.");
            if ((conditions & TreasureClass.Bitmask01.StackTreasure) == TreasureClass.Bitmask01.StackTreasure)
                output.Add("Stack results.");

            return output;
        }

        internal string GetQualities(IList<string> list)
        {
            var output = string.Empty;
            for (var i = 0; i < list.Count; i++)
            {
                output += list[i];
                if (i == list.Count - 2 && list.Count != 1)
                {
                    output += " and ";
                    continue;
                }
                if (i < list.Count - 1)
                {
                    output += ", ";
                    continue;
                }
            }
            return output;
        }
    }
}
