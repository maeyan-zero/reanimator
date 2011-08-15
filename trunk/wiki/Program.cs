using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Hellgate;
using MediaWiki.Articles;
using MediaWiki.Parser;
using MediaWiki.Parser.Class;

namespace MediaWiki
{
    class Program
    {
        static void Main(string[] args)
        {
            var manager = new FileManager(@"C:\Hellgate");
            manager.BeginAllDatReadAccess();
            manager.LoadTableFiles();
            manager.EndAllDatAccess();

            //PrintClassPlanner(manager);
            //return;
            //var achievements = new Achievements(manager);
            //Debug.Write(achievements.ExportArticle());
            //return;
            //var affixes = manager.GetDataTable("AFFIXES");
            //Evaluator evaluator = new Evaluator();
            //evaluator.Manager = manager;
            //foreach (DataRow affix in affixes.Rows)
            //{
            //    Unit unit = new Unit();
            //    string property1 = affix["property1"] as string;
            //    if (property1 == null || property1.Equals(String.Empty)) continue;
            //    unit.SetStat("level", 30);
            //    evaluator.Unit = unit;

            //    if ((int) affix["Index"] == 134)
            //    {
            //        int breakpoint = 0;
            //        breakpoint++;
            //    }
            //    var result = evaluator.Evaluate(property1);
            //    Debug.WriteLine(affix["Index"] + ": " + result[0]);
            //}

            //var itemUpgrades = new ItemUpgrades(manager);
            //var sqlSchema = itemUpgrades.ExportSchema();
            //var sqlTable = itemUpgrades.ExportTable();
            //File.WriteAllLines("itemUpgrades.sql", new[] { sqlSchema, sqlTable });

            var items = new Items(manager);
            var sqlTable = items.ExportTable();

            //var monsters = new Monsters(manager);
            //sqlSchema = monsters.ExportSchema();
            //sqlTable = monsters.ExportTable();
            //File.WriteAllLines("monsters.sql", new[] { sqlSchema, sqlTable });

            var treasure = new Treasure(manager);
            //sqlSchema = treasure.ExportSchema();
            //sqlTable = treasure.ExportTable();
            Debug.Write(Treasure.GetTreasureTable(641));
            
            //File.WriteAllLines("treasure.sql", new[] { sqlSchema, sqlTable });

            return;
        }

#region CLASSPLANNER

        static void PrintClassPlanner(FileManager manager)
        {
            int bm = 4;
            int guard = 7;
            int mm = 29;
            int eng = 33;
            int summ = 17;
            int evo = 16;
            DataTable table = manager.GetDataTable("SKILLS");
            StringWriter writer = new StringWriter();
            foreach (DataRow row in table.Rows)
            {
                if ((int)row["skillGroup1"] != guard) continue;

                writer.WriteLine("id = " + row["Index"] + ";");
                writer.WriteLine("display = \"" + row["displayName_string"] + "\";");
                String description = (string)row["descriptionString_string"];
                description = description.Replace("\n", "<br/>");
                description = description.Replace("’", "'");
                writer.WriteLine("description = \"" + description + "\";");
                String effect = (string)row["effectString_string"];
                effect = effect.Replace("\n", "<br/>");
                effect = effect.Replace("’", "'");
                writer.WriteLine("effect = \"" + effect + "\";");
                writer.WriteLine("//icon = " + (string)row["largeIcon"] + ";");
                writer.WriteLine("skillTab = " + row["skillTab"] + ";");
                writer.WriteLine("column = " + row["skillPageColumn"] + ";");
                writer.WriteLine("row = " + row["skillPageRow"] + ";");

                writer.WriteLine("powerCost = " + row["powerCost"].ToString() + ";");
                writer.WriteLine("powerCostPerLevel = " + row["powerCostPerLevel"].ToString() + ";");
                writer.WriteLine("coolDown = " + row["coolDown"].ToString() + ";");
                writer.WriteLine("//coolDownPercenteChange = " + row["coolDownPercentChange"] + ";");

                writer.WriteLine("requiredItem = " + row["requiresWeaponUnitType"] + ";");

                String groups = row["skillGroup1"].ToString();
                if ((int)row["skillGroup2"] != -1)
                {
                    groups += ", " + row["skillGroup2"].ToString();
                }
                if ((int)row["skillGroup3"] != -1)
                {
                    groups += ", " + row["skillGroup3"].ToString();
                }
                if ((int)row["skillGroup4"] != -1)
                {
                    groups += ", " + row["skillGroup4"].ToString();
                }
                writer.WriteLine("group = new int[] { " + groups + " };");

                String levels = row["level1"].ToString();
                if ((int)row["level2"] != -1)
                {
                    levels += ", " + row["level2"].ToString();
                }
                if ((int)row["level3"] != -1)
                {
                    levels += ", " + row["level3"].ToString();
                }
                if ((int)row["level4"] != -1)
                {
                    levels += ", " + row["level4"].ToString();
                }
                if ((int)row["level5"] != -1)
                {
                    levels += ", " + row["level5"].ToString();
                }
                if ((int)row["level6"] != -1)
                {
                    levels += ", " + row["level6"].ToString();
                }
                if ((int)row["level7"] != -1)
                {
                    levels += ", " + row["level7"].ToString();
                }
                if ((int)row["level8"] != -1)
                {
                    levels += ", " + row["level8"].ToString();
                }
                if ((int)row["level9"] != -1)
                {
                    levels += ", " + row["level9"].ToString();
                }
                if ((int)row["level10"] != -1)
                {
                    levels += ", " + row["level10"].ToString();
                }
                writer.WriteLine("level = new int[] { " + levels + " };");
                writer.WriteLine("maxLevel = " + row["maxLevel"] + ";");

                if ((int)row["requiredSkills1"] != -1)
                {
                    writer.WriteLine("parent = " + row["requiredSkills1"] + ";");
                    writer.WriteLine("parentLevel = " + row["levelsOfRequiredSkills1"] + ";");
                }

                if ((string)row["skillVar0"] != "")
                {
                    string var = "var = new int[] { ";
                    var += ((string)row["skillVar0"]).Replace(";", "");
                    var += ", ";
                    var += ((string)row["skillVar1"]).Replace(";", "");
                    var += ", ";
                    var += ((string)row["skillVar2"]).Replace(";", "");
                    var += ", ";
                    var += ((string)row["skillVar3"]).Replace(";", "");
                    var += ", ";
                    var += ((string)row["skillVar4"]).Replace(";", "");
                    var += " };";
                    writer.WriteLine(var);
                }

                writer.WriteLine("");
            }
            File.WriteAllText("java.txt", writer.ToString());
            return;
        }
#endregion
    }
}
