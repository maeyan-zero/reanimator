using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hellgate;
using System.Data;

namespace MediaWiki.Articles
{
    class Levels : WikiScript
    {
        private DataTable tblMonsters, tblSpawnClass;

        public Levels(FileManager manager)
            : base(manager, "levels")
        {
        }

        public override string ExportArticle()
        {
            throw new NotImplementedException();
        }

        public override string ExportTableInsertScript()
        {
            var script = new SQLTableScript("id", "code",
                                            "id INT NOT NULL",
                                            "code VARCHAR(4) NOT NULL",
                                            "name TEXT NOT NULL",
                                            "previous TEXT",
                                            "next TEXT",
                                            "istown BOOL NOT NULL",
                                            "act TEXT NOT NULL",
                                            "hellrift_chance INT NOT NULL",
                                            "adv_chance INT NOT NULL",
                                            "unique_chance INT NOT NULL",
                                            "level_range TEXT",
                                            "has_cannons BOOL NOT NULL",
                                            "spawns TEXT",
                                            "named_spawn TEXT",
                                            "messenger_killcount INT",
                                            "messenger_type TEXT");

            //two types of "mutant"

            //need to figure out number of spawn points to calculate champion chance

            tblMonsters = Manager.GetDataTable("MONSTERS");
            tblSpawnClass = Manager.GetDataTable("SPAWN_CLASS");
            DataTable TblSublevel = Manager.GetDataTable("SUBLEVEL");
            IEnumerable<DataRow> drlgRows = Manager.GetDataTable("LEVEL_DRLG_CHOICE").AsEnumerable();
            DataRow sublevel, drlg;

            string id, code, name, previous, next, act, isTown, riftChance, monsterLevels, championChance, uniqueChance, uniqueName, messengerType,
                additionalInfo,
                hellriftSpawns,
                spawns;
            int index, monsterLevel, monsterMin, monsterMax, hellriftLevel, messengerKillCount, messengerQuality,
                uniqueIndex;
            foreach (DataRow row in Manager.GetDataTable("LEVEL").Rows)
            {
                if (row["code"].ToString() == "0") continue; // ignore blank rows

                //common info
                index = (int)row["Index"];
                id = index.ToString();
                code = GetSqlString(((int)row["code"]).ToString("X"));
                name = GetSqlString(row["levelDisplayName_string"].ToString());
                previous = GetSqlString(GetWikiArticleLink(row["previousLevel_string"].ToString()));
                next = GetSqlString(GetWikiArticleLink(row["nextLevel_string"].ToString()));
                isTown = row["town"].ToString();


                riftChance = row["hellriftChancePercent"].ToString();
                monsterLevel = (int)row["monsterLevel"];
                monsterMin = (int)row["monsterMinLevelLimit"];
                monsterMax = (int)row["monsterMaxLevelLimit"];

                //check if the levels scale
                if ((int)row["monsterLevelFromActivator"] == 3)
                {
                    //if there's a max, display the range
                    if (monsterMax > 1)
                    {
                        //check for
                        if (monsterMax == 55 && monsterMin == 1)
                            monsterLevels = "Scales";
                        else if (monsterMin < 1)
                            monsterLevels = string.Format("Scales (max {0})", monsterMax);
                        else
                            monsterLevels = string.Format("{0} - {1} (scales)", monsterMin, monsterMax);
                    }
                    else
                        monsterLevels = "Scales";
                }
                else
                    monsterLevels = monsterLevel.ToString();

                uniqueChance = row["uniqueMonsterChancePercent"].ToString();

                act = GetSqlString(GetAct((int)row["act"]));    //make this into an article link

                //get hellrift sublevel spawns
                hellriftLevel = (int)row["hellriftSubLevel1"];
                if (hellriftLevel > 3)
                {
                    sublevel = TblSublevel.Rows[hellriftLevel];
                    hellriftSpawns = GetSpawns((int)sublevel["spawnClass"]);
                }
                
                //get spawns and messenger stuff
                drlg = drlgRows.FirstOrDefault(r => (int)r["levelName"] == index);
                if (drlg != null)
                {
                    //unique monster
                    uniqueIndex = (int)drlg["namedMonsterClass"];
                    if (uniqueIndex > 0)
                        uniqueName = tblMonsters.Rows[uniqueIndex]["String_string"].ToString();

                    //spawns
                    spawns = GetSpawns((int)drlg["spawnClass"]);

                    //messenger
                    messengerKillCount = (int)drlg["eventSpawnMaxKillCount"];
                    if (messengerKillCount > 0)
                    {
                        messengerType = tblMonsters.Rows[(int)drlg["eventSpawnMonsterClass"]]["String_string"].ToString();
                        messengerQuality = (int)drlg["eventSpawnMonsterQuality"];
                    }
                }

                script.AddRow(id, code);
            }

            return script.GetInsertScript();
        }

        private string GetAct(int actNum)
        {
            string act = string.Empty;

            switch (actNum)
            {
                case 0:
                    act = "Act 1";
                    break;
                case 1:
                    act = "Act 2";
                    break;
                case 2:
                    act = "Act 3";
                    break;
                case 3:
                    act = "Act 4";
                    break;
                case 4:
                    act = "Act 5";
                    break;
                case 5:
                    act = "Stonehenge";
                    break;
                case 6:
                    act = "2nd Invasion";
                    break;
                case 7:
                    act = "Abyss";
                    break;
                case 8:
                    act = "Tokyo";
                    break;
            }

            return act;
        }

        private string GetSpawns(int spawnClass)
        {
            string monsters = string.Empty;

            if (spawnClass < 2) return monsters;

            return monsters;
        }
    }
}
