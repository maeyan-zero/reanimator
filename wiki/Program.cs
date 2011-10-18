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

            //TODO: hp/power regen is messed up when displaying the full formula because I guess the itemdisplay formula isn't used

            //note: "display dmg" is ilvl multi*item dmg*dmg% (before "increments" are applied), where dmg% is the first or second argument in dmg_elec(100,100)

            //TODO: weapon ranges
            //TODO: add affix names to properties
            //TODO: sword attack rates
            //TODO: wtf is up with ilvls that don't match any of the listed numbers (maxlevel, fixedlevel, level)?
            
            //long term TODO: assign unit types so "isa" functions (among other things) work

            args = new[] { "ITEMS" };

            //new Items(manager).WriteAllUniqueLegendaryItemPages();
            //return;

            string sqlStatement;
            WikiScript script;
            foreach (string arg in args)
            {                
                switch (arg)
                {
                    case "ACHIEVEMENTS":
                        script = new Achievements(manager);
                        break;
                    case "AFFIXES":
                        script = new Affixes(manager);
                        break;
                    case "BASEWAVES":
                        script = new BaseDefenseWaves(manager);
                        break;
                    case "MONSTERAFFIXES":
                        script = new MonsterAffixes(manager);
                        break;
                    case "ARMORAFFIXES":
                        script = new ArmorAffixes(manager);
                        break;
                    case "ITEMLEVELS":
                        script = new ItemLevels(manager);
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
                    case "ITEMS":
                        script = new Items(manager);
                        break;
                    case "ITEM_QUALITY":
                        script = new ItemQuality(manager);
                        break;
                    case "TREASURE":
                        script = new NewTreasure(manager);
                        break;
                    case "MONSTER_QUALITY":
                        script = new MonsterQuality(manager);
                        break;
                    case "RECIPES":
                        script = new Recipes(manager);
                        break;
                    case "ITEM_SETS":
                        script = new Sets(manager);
                        break;
                    default:
                        throw new Exception("Unknown WikiScript: " + arg);
                }

                sqlStatement = script.ExportTableInsertScript();

                File.WriteAllText(arg.ToLower() + ".sql", sqlStatement);
            }

            return;
        }
    }
}
