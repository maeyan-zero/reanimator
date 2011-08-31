using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hellgate;
using System.Data;

namespace MediaWiki.Articles
{
    class LevelScaling:WikiScript
    {
        public LevelScaling(FileManager manager)
            : base(manager, "level_scaling")
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
                "level_diff INT NOT NULL",
                "pvm_damage INT NOT NULL",
                "pvm_experience INT NOT NULL",
                "mvp_damage INT NOT NULL",
                "treasure_bonus INT NOT NULL"
                );

            var levelscales = Manager.GetDataTable("LEVEL_SCALING");

            string id, diff, pvmDmg, pvmXp, mvpDmg, treasure;
            foreach (DataRow row in levelscales.Rows)
            {
                id = row["Index"].ToString();
                diff = row["levelDiff"].ToString();
                pvmDmg = row["PlayerAttackMonsterDmg"].ToString();
                pvmXp = row["PlayerAttackMonsterExp"].ToString();
                mvpDmg = row["MonsterAttackPlayerDmg"].ToString();
                treasure = row["PlayerAttackMonsterTreasureBonusPct"].ToString();

                table.AddRow(id, diff, pvmDmg, pvmXp, mvpDmg, treasure);
            }

            return table.GetFullScript();
        }
    }
}
