using System;
using System.Data;
using System.Text;
using Hellgate;

namespace MediaWiki.Articles
{
    public class Treasure : WikiScript
    {
        public Treasure(FileManager manager)
            : base(manager, "treasure")
        {
        }

        public override string ExportArticle()
        {
            throw new NotImplementedException();
        }

        public override string ExportTableInsertScript()
        {
            SQLTableScript table = new SQLTableScript("id", string.Empty,
                "id INT NOT NULL",
                "loot TEXT"
                );

            var treasure = Manager.GetDataTable("TREASURE");

            foreach (DataRow row in treasure.Rows)
            {
                table.AddRow(
                    row["Index"].ToString(),
                    GetSqlEncapsulatedString(GetTreasureTable((int)row["Index"]))
                    );
            }

            return table.GetFullScript();
        }

        //TODO: somehow make this play nice with the DB wiki extension so it displays properly
        /// <summary>
        /// Prints the drop table of the given entity. The drop table is formatted for use with the TreeView extension.
        /// </summary>
        /// <param name="entity"></param>
        public static string GetTreasureTable(int entity)
        {
            if (entity == -1) return String.Empty;

            var builder = new StringBuilder();
            var depth = 1;
            var treasure = Manager.GetDataTable("TREASURE");
            var row = treasure.Rows[entity];
            var id = (string) row["treasureClass"];
            id = id.Replace(" ", "");
            builder.AppendLine("{{#tree:id=" + id + "|openlevels=1|");
            GetTreasureTableRow(builder, row, ref depth);
            builder.AppendLine("}}");
            return builder.ToString();
        }

        static void GetTreasureTableRow(StringBuilder builder, DataRow row, ref int depth, int weight = -1)
        {
            const int maxItems = 8;
            const char nullEntry = '0';

            var name = (string) row["treasureClass"];
            if (weight != -1) name += " / " + weight;
            var pickType = (int) row["pickType"];
            var pick = String.Empty;

            switch (pickType)
            {
                case -1:
                    pick = "Null";
                    break;
                case 0:
                    pick = "One";
                    break;
                case 1:
                    pick = "Everything";
                    break;
                case 2:
                    pick = "Modifiers Only";
                    break;
                case 3:
                    pick = "Chance";
                    break;
                case 4:
                    pick = "Eliminate One";
                    break;
                case 5:
                    pick = "First Valid";
                    break;
            }

            name = GetFormattedString(name);
            builder.AppendLine(GetWikiListIndentation(depth) + name);
            depth++;

            builder.AppendLine(GetWikiListIndentation(depth) + "Selection: " + pick);
            var drop = 100 - (float) row["noDrop"];
            if (drop != 100) builder.AppendLine(GetWikiListIndentation(depth) + "Chance: " + drop + "%");

            for (var i = 1; i <= maxItems; i++)
            {
                var item = (string)row["item" + i];
                var value = (int)row["value" + i];

                if (item[0] == nullEntry) break;

                PrintDropTableRowItem(builder, item, value, ref depth);
            }

            depth--;
        }

        static void PrintDropTableRowItem(StringBuilder builder, string item, int value, ref int depth)
        {
            const int itemtype = 1;
            const int unittype = 2;
            const int treasure = 3;
            const int quality = 4;
            const int nothing = 7;

            var explode = item.Split(',');
            var type = Int32.Parse(explode[0]);
            var param = Int32.Parse(explode[1]);
            string content = null;

            switch (type)
            {
                case itemtype:
                    var itemTable = Manager.GetDataTable("ITEMS");
                    var itemRow = itemTable.Rows[param];
                    var itemName = (string) itemRow["String_string"];
                    content = GetWikiArticleLink(itemName);
                    break;
                case unittype:
                    var unitTable = Manager.GetDataTable("UNITTYPES");
                    var unitRow = unitTable.Rows[param];
                    var unitName = (string) unitRow["type"];
                    unitName = GetFormattedString(unitName);
                    content = unitName;
                    break;
                case treasure:
                    var treasureTable = Manager.GetDataTable("TREASURE");
                    var treasureRow = treasureTable.Rows[param];
                    GetTreasureTableRow(builder, treasureRow, ref depth, value);
                    break;
                case quality:
                    var qualityTable = Manager.GetDataTable("ITEM_QUALITY");
                    var qualityRow = qualityTable.Rows[param];
                    var qualityName = (string) qualityRow["displayName_string"];
                    content = GetWikiArticleLink(qualityName) + " quaility items are excluded.";
                    break;
                case nothing:
                    content = "Nothing";
                    break;
            }

            if (content == null) return;
            if (type != quality) content += " / " + value;
            builder.AppendLine(GetWikiListIndentation(depth) + content);
        }
    }
}
