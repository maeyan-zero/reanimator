using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hellgate;
using MediaWiki.Parser;
using MediaWiki.Util;
using System.Data;
using MediaWiki.Parser.Class;

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
                "isBossAffix BOOL NOT NULL",
                "name TEXT NOT NULL",
                "formatted_name TEXT NOT NULL",
                "display_string TEXT NOT NULL"
            );

            var affixes = Manager.GetDataTable("AFFIXES");

            string id, code, magicName, formattedName, displayString;
            string quality, property1;
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
                

                Unit unit = new Item();
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
                displayString = GetSqlString(displayStrings.Aggregate(string.Empty, (current, affix) => current + affix + "<br />"));
                displayString = displayString.Replace(" you get", "");   //display "when hit" instead of "when you get hit"
                //Debug.WriteLine(id + ", " + row["affix"] + ", " + displayString);

                table.AddRow(id, code, Convert.ToInt32(isBoss).ToString(), magicName, formattedName, displayString);
                
            }

            return table.GetFullScript();
        }
    }
}
