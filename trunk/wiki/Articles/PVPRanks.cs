using System;
using System.Data;
using Hellgate;

namespace MediaWiki.Articles
{
    class PVPRanks:WikiScript
    {
        public PVPRanks(FileManager manager)
            : base(manager, "pvp_ranks")
        {
        }

        public override string ExportArticle()
        {
            throw new NotImplementedException();
        }

        public override string ExportTableInsertScript()
        {
            TableScript = new SQLTableScript("id", "code",
                "id INT NOT NULL",
                "code VARCHAR(4) NOT NULL",
                "name TEXT",
                "name_charsheet TEXT",
                "experience_needed INT NOT NULL",
                //"minusxp_enabled BOOL NOT NULL", //not sure what this is
                //"max_rank_player INT",  //eh?
                //"max_percentile INT NOT NULL", //not sure if we want this
                "rank_icon TEXT"
                );

            var ranks = Manager.GetDataTable("PVP_RANKS");

            string id, code, name, charName, xp, maxPct, maxRank, minusXp, icon;
            foreach (DataRow row in ranks.Rows)
            {
                int xpMin = (int)row["pvpExpMin"];
                if (xpMin <= 0) continue;

                id = row["Index"].ToString();
                code = GetSqlEncapsulatedString(((int)row["code"]).ToString("X"));
                name = GetSqlEncapsulatedString(row["rankName"].ToString());
                xp = xpMin.ToString();
                maxPct = row["maxPercentile"].ToString();
                maxRank = row["maxRankPlayer"].ToString();
                minusXp = row["minusExpEnable"].ToString();
                charName = GetSqlEncapsulatedString(row["characterSheetString_string"].ToString());
                icon = GetSqlEncapsulatedString(GetImage(row["characterSheetIcon"].ToString() + ".png"));

                TableScript.AddRow(id, code, name, charName, xp, icon);
            }

            return TableScript.GetFullScript();
        }
    }
}
