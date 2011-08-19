using System;
using System.Data;
using System.IO;
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

            //string testcase = "GetStat666('accuracy_bonus') > 0 && (GetStat666('accuracy_bonus') == GetStat666('strength_bonus') && GetStat666('strength_bonus') == GetStat666('stamina_bonus') && GetStat666('stamina_bonus') == GetStat666('willpower_bonus'));";
            //Evaluator evaluator = new Evaluator();
            //evaluator.Unit = new Item();
            //evaluator.Game3 = new Game3();
            //var result = evaluator.Evaluate(testcase);

            args = new[] { "LEVELSCALING" };

            string sqlStatement;
            WikiScript script;
            foreach (string arg in args)
            {                
                switch (arg)
                {
                    case "AFFIXES":
                        script = new Affixes(manager);
                        break;
                    case "MONSTERS":
                        script = new Monsters(manager);
                        break;
                    case "PVPRANKS":
                        script = new PVPRanks(manager);
                        break;
                    case "LEVELSCALING":
                        script = new LevelScaling(manager);
                        break;
                    default:
                        throw new Exception("Unknown WikiScript: " + arg);
                }

                sqlStatement = script.ExportTableInsertScript();

                File.WriteAllText(arg.ToLower() + ".sql", sqlStatement);
            }

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
