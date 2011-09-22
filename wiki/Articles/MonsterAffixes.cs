using System;
using System.Data;
using System.Linq;
using Hellgate;
using MediaWiki.Parser;
using MediaWiki.Parser.Class;
using MediaWiki.Util;

namespace MediaWiki.Articles
{
    class MonsterAffixes : WikiScript
    {
        public MonsterAffixes(FileManager manager)
            : base(manager, "monster_affixes")
        {
        }

        public override string ExportArticle()
        {
            throw new NotImplementedException();
        }

        public override string ExportTableInsertScript()
        {
            SQLTableScript table = new SQLTableScript("id", "code",
                "id INT NOT NULL",
                "code VARCHAR(6) NOT NULL",
                //"isBossAffix BOOL NOT NULL",
                "name TEXT NOT NULL",
                "formatted_name TEXT NOT NULL",
                "display_string TEXT NOT NULL"
            );

            var affixes = Manager.GetDataTable("AFFIXES");

            string id, code, magicName, formattedName, displayString;
            string quality, property1;
            object shields;
            bool isBoss = false;

            Evaluator evaluator = new Evaluator();
            ItemDisplay.Manager = Manager;
            evaluator.Manager = Manager;

            foreach (DataRow row in affixes.Rows)
            {
                //don't show affixes that aren't used/implemented
                //if ((int)row["spawn"] == 0) continue;
                isBoss = false;

                magicName = row["magicNameString_string"].ToString();
                if (String.IsNullOrWhiteSpace(magicName)) continue;
                if (row["allowTypes1_string"].ToString() != "monster") continue;
                

                Unit unit = new Monster();
                Game3 game3 = new Game3();
                evaluator.Unit = unit;
                evaluator.Game3 = game3;
                for (int i = 1; i < 7; i++)
                {
                    property1 = row["property" + i].ToString();
                    evaluator.Evaluate(property1);
                }

                String[] displayStrings = ItemDisplay.GetDisplayStrings(unit);

                id = row["Index"].ToString();
                code = GetSqlString(((int)row["code"]).ToString("X"));

                magicName = magicName.Replace("[item]", string.Empty).Trim();
                isBoss = magicName.Contains("Sydonai");
                if (isBoss) continue;   //don't think we need boss titles
                formattedName = String.Format("'''{0}'''", magicName);

                quality = row["affixType1_string"].ToString();
                switch (quality)
                {
                    case "common":
                        //just leave it bold
                        break;
                    case "rare":
                        formattedName = Colorize(formattedName, WikiColors.Rare);
                        break;
                    case "legendary":
                        formattedName = Colorize(formattedName, WikiColors.Legendary);
                        break;
                    case "mythic":
                        formattedName = Colorize(formattedName, WikiColors.Mythic);
                        break;
                    default:
                        throw new NotImplementedException(String.Format("Apparently we need '{0}'?", quality));
                }
                magicName = GetSqlString(magicName);
                formattedName = GetSqlString(formattedName);
                displayString = displayStrings.Aggregate(string.Empty, (current, affix) => current + affix + "<br />");
                displayString = displayString.Replace(" you get", "");   //display "when hit" instead of "when you get hit"
                //manually add shields if needed (never a number)
                shields = unit.GetStat("shield_buffer_max");
                if (!(shields is int))
                    displayString = displayString.Replace("ilevel", shields.ToString());    //hacky, but whatever
                //add procs and skills
                displayString += AddOtherProperties(unit);
                displayString = GetSqlString(displayString);
                //Debug.WriteLine(id + ", " + row["affix"] + ", " + displayString);

                table.AddRow(id, code, /*Convert.ToInt32(isBoss).ToString(),*/ magicName, formattedName, displayString);
                
            }

            return table.GetFullScript();
        }

        private string AddOtherProperties(Unit unit)
        {
            string props = string.Empty;
            int num = 0;

            //procs
            //spawning
            num = (int)unit.GetStat("proc_chance_on_got_killed", "Spawning");
            if (num > 0)
                props += "Spawns additional monsters of its type when killed<br />";
            //cascading
            num = (int)unit.GetStat("proc_chance_on_got_killed", "Cascade");
            if (num > 0)
                props += num + "% chance to spawn two smaller clones when killed<br />";
            //teleporting
            num = (int)unit.GetStat("proc_chance_on_got_hit", "Teleport");
            if (num > 0)
                props += num + "% chance to teleport when hit<br />";
            //bug hive
            num = (int)unit.GetStat("proc_chance_on_got_hit", "BugHive");
            if (num > 0)
                props += num + "% chance to release a Bug Hive when hit<br />";


            //skills
            //regenerating (specific hp replenish doesn't seem to do anything)
            num = (int)unit.GetStat("skill_level", "Monster_Affix_Healing");
            if (num > 0)
                props += "Can fully heal when below 25% health, with a cooldown of 30 seconds<br />";

            //fire "field" (nova) (not sure what eventparam is for)
            num = (int)unit.GetStat("skill_level", "Monster_Affix_Field_Fire");
            if (num > 0)
                props += "Constant Fire Nova that does (" + ((10 + 5 * num) / 100.0) + " * mlevel_damage) in a 5m radius<br />";
            //fire effect on death
            num = (int)unit.GetStat("skill_level", "Monster_Affix_FireDeath");
            if (num > 0)
                props += "When killed, causes a fire field that does (" + ((5 + 5 * num) / 100.0) + " * mlevel_damage) damage in a 10m radius for 5 seconds<br />";

            //physical "field" (nova) (not sure what eventparam is for)
            num = (int)unit.GetStat("skill_level", "Monster_Affix_Field_Physical");
            if (num > 0)
                props += "Constant Physical Nova that does (" + ((10 + 5 * num) / 100.0) + " * mlevel_damage) in a 5m radius<br />";
            //physical effect on death
            num = (int)unit.GetStat("skill_level", "Monster_Affix_PhysicalDeath");
            if (num > 0)
                props += "When killed, causes a physical field that does (" + ((5 + 5 * num) / 100.0) + " * mlevel_damage) damage in a 10m radius for 5 seconds<br />";

            //electric "field" (nova) (not sure what eventparam is for)
            num = (int)unit.GetStat("skill_level", "Monster_Affix_Field_Electric");
            if (num > 0)
                props += "Constant Electric Nova that does (" + ((10 + 5 * num) / 100.0) + " * mlevel_damage) in a 5m radius<br />";
            //electric effect on death
            num = (int)unit.GetStat("skill_level", "Monster_Affix_ElectricDeath");
            if (num > 0)
                props += "When killed, causes an electric field that does (" + ((5 + 5 * num) / 100.0) + " * mlevel_damage) damage in a 10m radius for 5 seconds<br />";

            //spectral "field" (nova) (not sure what eventparam is for)
            num = (int)unit.GetStat("skill_level", "Monster_Affix_Field_Spectral");
            if (num > 0)
                props += "Constant Spectral Nova that does (" + ((10 + 5 * num) / 100.0) + " * mlevel_damage) in a 5m radius<br />";
            //spectral effect on death
            num = (int)unit.GetStat("skill_level", "Monster_Affix_SpectralDeath");
            if (num > 0)
                props += "When killed, causes a spectral field that does (" + ((5 + 5 * num) / 100.0) + " * mlevel_damage) damage in a 10m radius for 5 seconds<br />";

            //toxic "field" (nova) (not sure what eventparam is for)
            num = (int)unit.GetStat("skill_level", "Monster_Affix_Field_Toxic");
            if (num > 0)
                props += "Constant Toxic Nova that does (" + ((10 + 5 * num) / 100.0) + " * mlevel_damage) in a 5m radius<br />";
            //toxic effect on death
            num = (int)unit.GetStat("skill_level", "Monster_Affix_ToxicDeath");
            if (num > 0)
                props += "When killed, causes a toxic field that does (" + ((5 + 5 * num) / 100.0) + " * mlevel_damage) damage in a 10m radius for 5 seconds<br />";

            return props;
        }
    }
}
