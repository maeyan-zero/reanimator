using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Hellgate;
using MediaWiki.Parser;
using MediaWiki.Parser.Class;
using MediaWiki.Util;

namespace MediaWiki.Articles
{
    class Affixes : WikiScript
    {
        public Affixes(FileManager manager) : base(manager,"affixes")
        {
        }

        public override string ExportTableInsertScript()
        {
            SQLTableScript table = new SQLTableScript("id", "code",
                "id INT NOT NULL",
                "code VARCHAR(6) NOT NULL",
                "name TEXT NOT NULL",
                "formatted_name TEXT NOT NULL",
                "display_string TEXT NOT NULL",
                "ilvl_range TEXT NOT NULL",
                "allowed_types TEXT NOT NULL",
                "faction TEXT"
                , "feed_costs TEXT NOT NULL"
            );

            var affixes = Manager.GetDataTable("AFFIXES");

            string id, code, magicName, formattedName, displayString, levelRange, typeList, faction;
            string[] feeds;
            string quality, property1, allowType;
            bool isMonster = false;

            Evaluator evaluator = new Evaluator();
            ItemDisplay.Manager = Manager;
            evaluator.Manager = Manager;

            foreach (DataRow row in affixes.Rows)
            {
                //don't show affixes that aren't used/implemented
                if ((int)row["spawn"] == 0) continue;
                isMonster = false;

                magicName = row["magicNameString_string"].ToString();
                if (String.IsNullOrWhiteSpace(magicName)) continue;

                typeList = string.Empty;
                faction = row["onlyOnItemsRequiringUnitType_string"].ToString();
                if (!String.IsNullOrWhiteSpace(faction))
                    faction = GetSqlEncapsulatedString(faction);

                for (int i = 1; i < 7; i++)
                {
                    allowType = row[String.Format("allowTypes{0}_string", i)].ToString();
                    if (String.IsNullOrWhiteSpace(allowType))
                        break;
                    else if (allowType.CompareTo("monster") == 0)
                    {
                        isMonster = true;
                        break; 
                    }

                    if (!String.IsNullOrWhiteSpace(typeList))
                        typeList += ", ";

                    typeList += GetItemType(allowType);
                }

                if (isMonster) continue;

                typeList = GetSqlEncapsulatedString(typeList);
                levelRange = GetSqlEncapsulatedString(row["minLevel"].ToString() + "-" + row["maxLevel"].ToString());

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
                feeds = ItemDisplay.GetFeedCosts(unit);                

                id = row["Index"].ToString();
                code = GetSqlEncapsulatedString(((int)row["code"]).ToString("X"));

                magicName = magicName.Replace("[item]", string.Empty).Trim();
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
                magicName = GetSqlEncapsulatedString(magicName);
                formattedName = GetSqlEncapsulatedString(formattedName);
                displayString = GetSqlEncapsulatedString(FormatAffixList(displayStrings));

                Debug.WriteLine(id + ", " + row["affix"] + ", " + displayString);

                table.AddRow(id, code, magicName, formattedName, displayString, levelRange, typeList, faction, GetSqlEncapsulatedString(FormatAffixList(feeds)));
            }

            return table.GetFullScript();
        }

        private string GetItemType(string type)
        {
            string itemType = string.Empty;

            switch (type)
            {
                case "cabalist_focus":
                    itemType = "focus item";
                    break;
                case "gun_beam":
                    itemType = "laser weapon";
                    break;
                case "gunammo":
                    itemType = "ammo";
                    break;
                case "legarmor":
                    itemType = "legs";
                    break;
                case "rocketammo":
                    itemType = "rocket";
                    break;
                default:
                    itemType = type;
                    break;
            }

            return itemType;
        }

        private string FormatAffixList(string[] affixes)
        {
            string list = affixes.Aggregate(string.Empty, (current, affix) => current + affix + "<br />").Replace("'", "''");
            return list;
        }

        public override string ExportArticle()
        {
            throw new NotImplementedException();
        }
    }
}
