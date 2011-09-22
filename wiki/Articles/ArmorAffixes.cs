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
    class ArmorAffixes : WikiScript
    {
        public ArmorAffixes(FileManager manager)
            : base(manager, "armor_affixes")
        { }

        public override string ExportArticle()
        {
            throw new NotImplementedException();
        }

        public override string ExportTableInsertScript()
        {
            SQLTableScript table = new SQLTableScript("id", "code",
                 "id INT NOT NULL",
                 "code VARCHAR(6) NOT NULL",
                 "name TEXT NOT NULL",
                 "formatted_name TEXT NOT NULL",
                 "display_string TEXT NOT NULL",
                 "ilvl INT NOT NULL",
                 "faction TEXT"
             );

            var affixes = Manager.GetDataTable("AFFIXES");

            string id, code, magicName, formattedName, displayString, ilvl, typeList, faction;
            string property1, type;

            Evaluator evaluator = new Evaluator();
            ItemDisplay.Manager = Manager;
            evaluator.Manager = Manager;

            foreach (DataRow row in affixes.Rows)
            {
                //don't show affixes that aren't used/implemented
                if ((int)row["spawn"] == 0) continue;

                //skip non-armorsets
                magicName = row["setNameString_string"].ToString();
                if (String.IsNullOrWhiteSpace(magicName)) continue;                
                type = row["affixType1_string"].ToString();
                if (type.CompareTo("armorset") != 0) continue;

                faction = row["onlyOnItemsRequiringUnitType_string"].ToString();
                if (!String.IsNullOrWhiteSpace(faction))
                    faction = GetSqlString(faction);

                Unit unit = new Item();
                Game3 game3 = new Game3();
                evaluator.Unit = unit;
                evaluator.Game3 = game3;
                property1 = string.Empty;
                for (int i = 1; i < 7; i++)
                {
                    property1 += row["property" + i].ToString();                    
                }
                //not sure how armor affixes normally work on regular items (random?), 
                //so skip the blank ones and only display the ilvl that produces the effects on uniques of non-standard ilvls
                if (string.IsNullOrWhiteSpace(property1)) continue;

                evaluator.Evaluate(property1);
                String[] displayStrings = ItemDisplay.GetDisplayStrings(unit);

                id = row["Index"].ToString();
                code = GetSqlString(((int)row["code"]).ToString("X"));
                magicName = magicName.Replace("[item]", string.Empty).Trim();
                formattedName = String.Format("'''{0}'''", magicName);
                ilvl = row["itemLevel"].ToString();

                magicName = GetSqlString(magicName);
                formattedName = GetSqlString(formattedName);
                displayString = GetSqlString(displayStrings.Aggregate(string.Empty, (current, affix) => current + affix + "<br />"));

                //Debug.WriteLine(id + ", " + row["affix"] + ", " + displayString);

                table.AddRow(id, code, magicName, formattedName, displayString, ilvl, faction);
            }

            return table.GetFullScript();
        }
    }
}
