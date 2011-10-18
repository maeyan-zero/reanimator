using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hellgate;
using System.Data;

namespace MediaWiki.Articles
{
    class BaseDefenseWaves:WikiScript
    {
        private enum SpawnPickType
        {
            All = 0,
            One,
            Two
        }

        public BaseDefenseWaves(FileManager manager)
            : base(manager, "basedefense_waves")
        { }

        public override string ExportArticle()
        {
            throw new NotImplementedException();
        }

        public override string ExportTableInsertScript()
        {
            var script = new SQLTableScript("id", string.Empty,
                                           "id INT NOT NULL",
                                           "wave INT NOT NULL",
                                           "monsters TEXT",
                                           "spawntime TEXT NOT NULL");

            var spawnTable = Manager.GetDataTable("SPAWN_CLASS");
            var monsterTable = Manager.GetDataTable("MONSTERS");
            string id, wave, monsters, spawntime;
            foreach (DataRow row in Manager.GetDataTable("DEFENSEGAME_WAVE").Rows)
            {
                if ((int)row["Index"] < 1) continue;

                id = wave = row["Index"].ToString();
                monsters = GetMonsters((int)row["monsterClass"], (int)row["spawnClass"], monsterTable, spawnTable);
                spawntime = GetTimeInSeconds((int)row["spawnTime"]);

                script.AddRow(id, wave, GetSqlString(monsters), GetSqlString(spawntime + " seconds"));
            }

            return script.GetFullScript();
        }

        private string GetMonsters(int monsterClass, int spawnClass, DataTable tblMonsters, DataTable tblSpawns)
        {
            string monsters = string.Empty;
            int spawnIndex, monsterIndex;
            List<WaveSpawn> spawns = new List<WaveSpawn>();
            if (monsterClass >= 0)
            {
                //if there's a boss, there are no other spawns
                monsters = GetWikiArticleLink(tblMonsters.Rows[monsterClass]["String_string"].ToString());
            }
            else if (spawnClass >= 0)
            {
                //no boss, get all the spawns

                //assumptions:
                //-base class is always "all" pick type with listed spawns having a count of 1 and equal weight
                //-all spawns have equal weight
                //-spawns don't have subspawns

                List<WaveSpawn> waveSpawns = new List<WaveSpawn>();
                DataRow spawnRow = tblSpawns.Rows[spawnClass];
                DataRow subspawnRow;
                DataRow monsterRow;
                WaveSpawn spawn;
                MonsterSpawn monster;
                StringBuilder builder = new StringBuilder();
                string name, altName;
                string[] nameParts;
                //go through each spawn of the wave
                for (int s = 1; s < 9; s++)
                {
                    spawnIndex = (int)spawnRow["spawn" + s];
                    if (spawnIndex < 0) break;  //nothing else, stop reading

                    subspawnRow = tblSpawns.Rows[spawnIndex];
                    spawn = new WaveSpawn();
                    spawn.Monsters = new List<MonsterSpawn>();
                    spawn.Pick = (SpawnPickType)subspawnRow["pickType"];
                    //go through each individual monster type in the spawn
                    for (int m = 1; m < 9; m++)
                    {
                        monsterIndex = (int)subspawnRow["spawn" + m];
                        if (monsterIndex < 0) break;

                        monsterRow = tblMonsters.Rows[monsterIndex];

                        monster = new MonsterSpawn();
                        monster.Count = int.Parse(subspawnRow["count" + m].ToString().Replace(";", string.Empty));
                        name = monsterRow["String_string"].ToString();
                        //if there's already a monster of the same name, append the other parts of the internal name to differentiate
                        if (spawn.Monsters.Exists(mon => mon.Name == name))
                        {
                            name += " (";
                            altName = monsterRow["name"].ToString();
                            nameParts = altName.Split('_');
                            //should be more than one part, but check anyway
                            if (nameParts.Length > 1)
                            {
                                for (int p = 1; p < nameParts.Length; p++)
                                {
                                    name += nameParts[p] + " ";
                                }
                                name = name.Trim() + ")";
                            }
                        }
                        monster.Name = name;

                        spawn.Monsters.Add(monster);
                    }
                    waveSpawns.Add(spawn);
                }


                //make the list
                builder.AppendLine("<ul>");
                foreach (WaveSpawn wSpawn in waveSpawns)
                {

                    //display "x of:" if not everything will appear
                    if (wSpawn.Pick != SpawnPickType.All && (int)wSpawn.Pick < wSpawn.Monsters.Count)
                    {
                        builder.AppendLine("<li>");
                        builder.AppendLine(wSpawn.Pick.ToString() + " of:");
                        builder.AppendLine("<ul>");
                        builder.Append(GetMonsterList(wSpawn.Monsters));
                        builder.AppendLine("</ul>");
                        builder.AppendLine("</li>");
                    }
                    else
                        builder.Append(GetMonsterList(wSpawn.Monsters));
                }
                builder.Append("</ul>");

                monsters = builder.ToString();
            }
            else
            {
                //no spawns
                monsters = "None";
            }

            return monsters;
        }

        private string GetMonsterList(IEnumerable<MonsterSpawn> monsters)
        {
            StringBuilder builder = new StringBuilder();

            foreach (MonsterSpawn monster in monsters)
            {
                builder.AppendLine("<li>");
                builder.Append(GetWikiArticleLink(monster.Name));
                if (monster.Count > 1)
                    builder.Append(" (" + monster.Count + ")");
                builder.AppendLine();
                builder.AppendLine("</li>");
            }

            return builder.ToString();
        }

        private class WaveSpawn
        {
            public SpawnPickType Pick { get; set; }
            public List<MonsterSpawn> Monsters { get; set; }
        }

        private class MonsterSpawn
        {
            public string Name { get; set; }
            public int Count { get; set; }
        }
    }
}
