using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using Hellgate;
using MediaWiki.Parser;
using MediaWiki.Parser.Class;
using MediaWiki.Util;
using System.IO;
using System.Linq;

namespace MediaWiki.Articles
{
    class Items : WikiScript
    {
        private readonly int _height = 19;

        public Items(FileManager manager) : base(manager, "items")
        {
        }

        public override string ExportArticle()
        {
            throw new NotImplementedException();
        }

        public void WriteAllUniqueLegendaryItemPages()
        {
            string quality, name, code, itemType;
            string item;
            var items = Manager.GetDataTable("ITEMS");

            StringBuilder builder = new StringBuilder();

            foreach (DataRow row in items.Rows)
            {
                //make sure it has a base row (spawn isn't an indicator)
                if ((int)row["baseRow"] < 0) continue;
                //test quality
                quality = row["itemQuality_string"].ToString();
                if (quality.CompareTo("unique") != 0 && quality.CompareTo("legendary") != 0 && quality.CompareTo("rare") != 0 && quality.CompareTo("rare-m") != 0) continue;
                //skip rings
                itemType = row["unitType_string"].ToString();
                //if (itemType.CompareTo("trinket_ring") == 0) continue;

                code = ((int)row["code"]).ToString("X");
                name = row["String_string"].ToString();


                item = string.Format("{{{{Item|code={0}}}}}", code);

                builder.AppendLine(string.Format("--{0}--", name));
                builder.AppendLine(item);
                if (quality.CompareTo("unique") == 0)
                    builder.AppendLine(string.Format("[[Category: {0} Items]]", char.ToUpper(quality[0]) + quality.Substring(1)));
                else
                    builder.AppendLine(string.Format("[[Category: {0} Named Items]]", char.ToUpper(quality[0]) + quality.Substring(1)));

                //only list additional categories for uniques
                if (quality.CompareTo("unique") == 0)
                {
                    switch (itemType)
                    {
                        case "hunter_gun1h":
                            builder.AppendLine("[[Category: Unique Hunter Items]]");
                            builder.AppendLine("[[Category: Unique Hunter One-Handed Guns]]");
                            builder.AppendLine("[[Category: Unique Hunter Weapons]]");
                            builder.AppendLine("[[Category: Weapons]]");
                            break;
                        case "hunter_gun2h":
                            builder.AppendLine("[[Category: Unique Hunter Items]]");
                            builder.AppendLine("[[Category: Unique Hunter Two-Handed Guns]]");
                            builder.AppendLine("[[Category: Unique Hunter Weapons]]");
                            builder.AppendLine("[[Category: Weapons]]");
                            break;
                        case "hunter_gun1h_beam":
                            builder.AppendLine("[[Category: Unique Hunter Items]]");
                            builder.AppendLine("[[Category: Unique Hunter One-Handed Guns]]");
                            builder.AppendLine("[[Category: Unique Hunter One-Handed Beam Weapons]]");
                            builder.AppendLine("[[Category: Unique Hunter Weapons]]");
                            builder.AppendLine("[[Category: Weapons]]");
                            break;
                        case "hunter_gun2h_beam":
                            builder.AppendLine("[[Category: Unique Hunter Items]]");
                            builder.AppendLine("[[Category: Unique Hunter Two-Handed Guns]]");
                            builder.AppendLine("[[Category: Unique Hunter Two-Handed Beam Weapons]]");
                            builder.AppendLine("[[Category: Unique Hunter Weapons]]");
                            builder.AppendLine("[[Category: Weapons]]");
                            break;
                        case "templar_gun1h":
                            builder.AppendLine("[[Category: Unique Templar Items]]");
                            builder.AppendLine("[[Category: Unique Templar One-Handed Guns]]");
                            builder.AppendLine("[[Category: Unique Templar Weapons]]");
                            builder.AppendLine("[[Category: Weapons]]");
                            break;
                        case "hunter_sniper2h":
                            builder.AppendLine("[[Category: Unique Hunter Items]]");
                            builder.AppendLine("[[Category: Unique Hunter Two-Handed Guns]]");
                            builder.AppendLine("[[Category: Unique Hunter Sniper Rifles]]");
                            builder.AppendLine("[[Category: Unique Hunter Weapons]]");
                            builder.AppendLine("[[Category: Weapons]]");
                            break;
                        case "quest_hunter":
                            builder.AppendLine("[[Category: Unique Hunter Items]]");
                            builder.AppendLine("[[Category: Quest Items]]");
                            break;
                        case "hunter_gun2h_field":
                            builder.AppendLine("[[Category: Unique Hunter Items]]");
                            builder.AppendLine("[[Category: Unique Hunter Two-Handed Guns]]");
                            builder.AppendLine("[[Category: Unique Hunter Field Weapons]]");
                            builder.AppendLine("[[Category: Unique Hunter Weapons]]");
                            builder.AppendLine("[[Category: Weapons]]");
                            break;
                        case "revealer_1h":
                            builder.AppendLine("[[Category: Unique Templar Items]]");
                            builder.AppendLine("[[Category: Unique Templar One-Handed Guns]]");
                            builder.AppendLine("[[Category: Unique Templar Nova Guns]]");
                            builder.AppendLine("[[Category: Unique Templar Weapons]]");
                            builder.AppendLine("[[Category: Weapons]]");
                            break;
                        case "templar_gun1h_beam":
                            builder.AppendLine("[[Category: Unique Templar Items]]");
                            builder.AppendLine("[[Category: Unique Templar One-Handed Guns]]");
                            builder.AppendLine("[[Category: Unique Templar One-Handed Beam Weapons]]");
                            builder.AppendLine("[[Category: Unique Templar Weapons]]");
                            builder.AppendLine("[[Category: Weapons]]");
                            break;
                        case "sword":
                            builder.AppendLine("[[Category: Unique Templar Items]]");
                            builder.AppendLine("[[Category: Unique Templar One-Handed Swords]]");
                            builder.AppendLine("[[Category: Unique Templar Melee Weapons]]");
                            builder.AppendLine("[[Category: Unique Templar Swords]]");
                            builder.AppendLine("[[Category: Unique Templar Weapons]]");
                            builder.AppendLine("[[Category: Weapons]]");
                            break;
                        case "sword_cricket":
                            builder.AppendLine("[[Category: Unique Templar Items]]");
                            builder.AppendLine("[[Category: Unique Templar Melee Weapons]]");
                            builder.AppendLine("[[Category: Unique Templar Cricket Bats]]");
                            builder.AppendLine("[[Category: Unique Templar Weapons]]");
                            builder.AppendLine("[[Category: Weapons]]");
                            break;
                        case "quest_templar_sword":
                        case "quest_templar_shield":
                        case "quest_templar_2hsword":
                            builder.AppendLine("[[Category: Unique Templar Items]]");
                            builder.AppendLine("[[Category: Quest Items]]");
                            break;
                        case "axe":
                            builder.AppendLine("[[Category: Unique Templar Items]]");
                            builder.AppendLine("[[Category: Unique Templar Melee Weapons]]");
                            builder.AppendLine("[[Category: Unique Templar Axes]]");
                            builder.AppendLine("[[Category: Unique Templar Weapons]]");
                            builder.AppendLine("[[Category: Weapons]]");
                            break;
                        case "2hsword":
                            builder.AppendLine("[[Category: Unique Templar Items]]");
                            builder.AppendLine("[[Category: Unique Templar Two-Handed Swords]]");
                            builder.AppendLine("[[Category: Unique Templar Melee Weapons]]");
                            builder.AppendLine("[[Category: Unique Templar Swords]]");
                            builder.AppendLine("[[Category: Unique Templar Weapons]]");
                            builder.AppendLine("[[Category: Weapons]]");
                            break;
                        case "shield_offensive":
                            builder.AppendLine("[[Category: Unique Templar Items]]");
                            builder.AppendLine("[[Category: Unique Templar Shields]]");
                            break;
                        case "cabalist_focus":
                            builder.AppendLine("[[Category: Unique Cabalist Items]]");
                            builder.AppendLine("[[Category: Unique Cabalist Focus Items]]");
                            builder.AppendLine("[[Category: Unique Cabalist Weapons]]");
                            builder.AppendLine("[[Category: Weapons]]");
                            break;
                        case "quest_cabalist":
                            builder.AppendLine("[[Category: Unique Cabalist Items]]");
                            builder.AppendLine("[[Category: Quest Items]]");
                            break;
                        case "cabalist_gun1h":
                            builder.AppendLine("[[Category: Unique Cabalist Items]]");
                            builder.AppendLine("[[Category: Unique Cabalist One-Handed Guns]]");
                            builder.AppendLine("[[Category: Unique Cabalist Weapons]]");
                            builder.AppendLine("[[Category: Weapons]]");
                            break;
                        case "cabalist_gun2h":
                            builder.AppendLine("[[Category: Unique Cabalist Items]]");
                            builder.AppendLine("[[Category: Unique Cabalist Two-Handed Guns]]");
                            builder.AppendLine("[[Category: Unique Cabalist Weapons]]");
                            builder.AppendLine("[[Category: Weapons]]");
                            break;
                        case "cabalist_gun1h_beam":
                            builder.AppendLine("[[Category: Unique Cabalist Items]]");
                            builder.AppendLine("[[Category: Unique Cabalist One-Handed Guns]]");
                            builder.AppendLine("[[Category: Unique Cabalist One-Handed Beam Weapons]]");
                            builder.AppendLine("[[Category: Unique Cabalist Weapons]]");
                            builder.AppendLine("[[Category: Weapons]]");
                            break;
                        case "cabalist_gun2h_beam":
                            builder.AppendLine("[[Category: Unique Cabalist Items]]");
                            builder.AppendLine("[[Category: Unique Cabalist Two-Handed Guns]]");
                            builder.AppendLine("[[Category: Unique Cabalist Two-Handed Beam Weapons]]");
                            builder.AppendLine("[[Category: Unique Cabalist Weapons]]");
                            builder.AppendLine("[[Category: Weapons]]");
                            break;
                        case "cabalist_gun2h_hive":
                            builder.AppendLine("[[Category: Unique Cabalist Items]]");
                            builder.AppendLine("[[Category: Unique Cabalist Two-Handed Guns]]");
                            builder.AppendLine("[[Category: Unique Cabalist Two-Handed Hive Weapons]]");
                            builder.AppendLine("[[Category: Unique Cabalist Weapons]]");
                            builder.AppendLine("[[Category: Weapons]]");
                            break;
                        case "harp2h":
                            builder.AppendLine("[[Category: Unique Cabalist Items]]");
                            builder.AppendLine("[[Category: Unique Cabalist Two-Handed Guns]]");
                            builder.AppendLine("[[Category: Unique Cabalist Two-Handed HARP Weapons]]");
                            builder.AppendLine("[[Category: Unique Cabalist Weapons]]");
                            builder.AppendLine("[[Category: Weapons]]");
                            break;
                        case "cabalist_helm":
                            builder.AppendLine("[[Category: Unique Cabalist Items]]");
                            builder.AppendLine("[[Category: Unique Cabalist Helms]]");
                            builder.AppendLine("[[Category: Unique Cabalist Armor]]");
                            builder.AppendLine("[[Category: Armor]]");
                            break;
                        case "hunter_helm":
                            builder.AppendLine("[[Category: Unique Hunter Items]]");
                            builder.AppendLine("[[Category: Unique Hunter Helms]]");
                            builder.AppendLine("[[Category: Unique Hunter Armor]]");
                            builder.AppendLine("[[Category: Armor]]");
                            break;
                        case "templar_helm":
                            builder.AppendLine("[[Category: Unique Templar Items]]");
                            builder.AppendLine("[[Category: Unique Templar Helms]]");
                            builder.AppendLine("[[Category: Unique Templar Armor]]");
                            builder.AppendLine("[[Category: Armor]]");
                            break;
                        case "cabalist_belt":
                            builder.AppendLine("[[Category: Unique Cabalist Items]]");
                            builder.AppendLine("[[Category: Unique Cabalist Belts]]");
                            builder.AppendLine("[[Category: Unique Cabalist Armor]]");
                            builder.AppendLine("[[Category: Armor]]");
                            break;
                        case "cabalist_arms":
                            builder.AppendLine("[[Category: Unique Cabalist Items]]");
                            builder.AppendLine("[[Category: Unique Cabalist Arms]]");
                            builder.AppendLine("[[Category: Unique Cabalist Armor]]");
                            builder.AppendLine("[[Category: Armor]]");
                            break;
                        case "cabalist_legarmor":
                            builder.AppendLine("[[Category: Unique Cabalist Items]]");
                            builder.AppendLine("[[Category: Unique Cabalist Leg Armor]]");
                            builder.AppendLine("[[Category: Unique Cabalist Armor]]");
                            builder.AppendLine("[[Category: Armor]]");
                            break;
                        case "cabalist_shoulderpads":
                            builder.AppendLine("[[Category: Unique Cabalist Items]]");
                            builder.AppendLine("[[Category: Unique Cabalist Shoulderpads]]");
                            builder.AppendLine("[[Category: Unique Cabalist Armor]]");
                            builder.AppendLine("[[Category: Armor]]");
                            break;
                        case "cabalist_torso":
                            builder.AppendLine("[[Category: Unique Cabalist Items]]");
                            builder.AppendLine("[[Category: Unique Cabalist Torso Armor]]");
                            builder.AppendLine("[[Category: Unique Cabalist Armor]]");
                            builder.AppendLine("[[Category: Armor]]");
                            break;
                        case "cabalist_boots":
                            builder.AppendLine("[[Category: Unique Cabalist Items]]");
                            builder.AppendLine("[[Category: Unique Cabalist Boots]]");
                            builder.AppendLine("[[Category: Unique Cabalist Armor]]");
                            builder.AppendLine("[[Category: Armor]]");
                            break;
                        case "hunter_legarmor":
                            builder.AppendLine("[[Category: Unique Hunter Items]]");
                            builder.AppendLine("[[Category: Unique Hunter Leg Armor]]");
                            builder.AppendLine("[[Category: Unique Hunter Armor]]");
                            builder.AppendLine("[[Category: Armor]]");
                            break;
                        case "hunter_belt":
                            builder.AppendLine("[[Category: Unique Hunter Items]]");
                            builder.AppendLine("[[Category: Unique Hunter Belts]]");
                            builder.AppendLine("[[Category: Unique Hunter Armor]]");
                            builder.AppendLine("[[Category: Armor]]");
                            break;
                        case "hunter_arms":
                            builder.AppendLine("[[Category: Unique Hunter Items]]");
                            builder.AppendLine("[[Category: Unique Hunter Arms]]");
                            builder.AppendLine("[[Category: Unique Hunter Armor]]");
                            builder.AppendLine("[[Category: Armor]]");
                            break;
                        case "hunter_shoulderpads":
                            builder.AppendLine("[[Category: Unique Hunter Items]]");
                            builder.AppendLine("[[Category: Unique Hunter Shoulderpads]]");
                            builder.AppendLine("[[Category: Unique Hunter Armor]]");
                            builder.AppendLine("[[Category: Armor]]");
                            break;
                        case "hunter_torso":
                            builder.AppendLine("[[Category: Unique Hunter Items]]");
                            builder.AppendLine("[[Category: Unique Hunter Torso Armor]]");
                            builder.AppendLine("[[Category: Unique Hunter Armor]]");
                            builder.AppendLine("[[Category: Armor]]");
                            break;
                        case "hunter_boots":
                            builder.AppendLine("[[Category: Unique Hunter Items]]");
                            builder.AppendLine("[[Category: Unique Hunter Boots]]");
                            builder.AppendLine("[[Category: Unique Hunter Armor]]");
                            builder.AppendLine("[[Category: Armor]]");
                            break;
                        case "templar_belt":
                            builder.AppendLine("[[Category: Unique Templar Items]]");
                            builder.AppendLine("[[Category: Unique Templar Belts]]");
                            builder.AppendLine("[[Category: Unique Templar Armor]]");
                            builder.AppendLine("[[Category: Armor]]");
                            break;
                        case "templar_arms":
                            builder.AppendLine("[[Category: Unique Templar Items]]");
                            builder.AppendLine("[[Category: Unique Templar Arms]]");
                            builder.AppendLine("[[Category: Unique Templar Armor]]");
                            builder.AppendLine("[[Category: Armor]]");
                            break;
                        case "templar_legarmor":
                            builder.AppendLine("[[Category: Unique Templar Items]]");
                            builder.AppendLine("[[Category: Unique Templar Leg Armor]]");
                            builder.AppendLine("[[Category: Unique Templar Armor]]");
                            builder.AppendLine("[[Category: Armor]]");
                            break;
                        case "templar_shoulderpads":
                            builder.AppendLine("[[Category: Unique Templar Items]]");
                            builder.AppendLine("[[Category: Unique Templar Shoulderpads]]");
                            builder.AppendLine("[[Category: Unique Templar Armor]]");
                            builder.AppendLine("[[Category: Armor]]");
                            break;
                        case "templar_torso":
                            builder.AppendLine("[[Category: Unique Templar Items]]");
                            builder.AppendLine("[[Category: Unique Templar Torso Armor]]");
                            builder.AppendLine("[[Category: Unique Templar Armor]]");
                            builder.AppendLine("[[Category: Armor]]");
                            break;
                        case "templar_boots":
                            builder.AppendLine("[[Category: Unique Templar Items]]");
                            builder.AppendLine("[[Category: Unique Templar Boots]]");
                            builder.AppendLine("[[Category: Unique Templar Armor]]");
                            builder.AppendLine("[[Category: Armor]]");
                            break;
                        case "trinket_ring":    //there aren't any implemented, but just so it doesn't throw errors
                            builder.AppendLine("[[Category: Unique Trinkets]]");
                            builder.AppendLine("[[Category: Unique Rings]]");
                            builder.AppendLine("[[Category: Trinkets]]");
                            break;
                        default:
                            throw new NotImplementedException("Unimplemented type: " + itemType);
                    }
                }
                //builder.AppendLine("TYPE: " + itemType);

                //determine categories by item type

                builder.AppendLine("\n");
            }

            File.WriteAllText("itempages.txt", builder.ToString());
        }

        public override string ExportTableInsertScript()
        {
            SQLTableScript table = new SQLTableScript("id", "code",
                "id INT NOT NULL",
                "code VARCHAR(4) NOT NULL",
                "image TEXT",
                "name TEXT",
                "type TEXT",
                "flavor TEXT",
                "dmg_icon TEXT",
                "damage TEXT",
                "defence TEXT",
                "stats TEXT",
                "mods TEXT",
                "feeds TEXT",
                "level TEXT",
                "inherent TEXT",
                "affixes TEXT",
                "quality_id INT",
                "name_raw TEXT",
                "type_raw TEXT",
                "level_raw INT"
                );

            ItemDisplay.Manager = Manager;
            var items = Manager.GetDataTable("ITEMS");
            var qualityTable = Manager.GetDataTable("ITEM_QUALITY");
            var ilvls = Manager.GetDataTable("ITEM_LEVELS");

            string id, code, name, type, flavor, quality, image, damage, stats, affixes, modslots, feeds, level, inherent, defence,
                qualityId, typeRaw, nameRaw, levelRaw, dmgIcon;
            string clvl;

            foreach (DataRow item in items.Rows)
            {
                if (item["code"].ToString() == "0") continue; // ignore blank rows
                Debug.WriteLine("Parsing: " + item["Index"] + " (" + item["name"] + ")");

                //if (((int)item["Index"]) != 1914) continue;

                // Dependencies
                quality = ((int) item["itemQuality"]) != -1
                                    ? qualityTable.Rows[(int) item["itemQuality"]]["displayName_string"].ToString()
                                    : string.Empty;

                Unit unit = new Item(); // for sharing stats

                // Guts
                id = GetSqlString(item["index"].ToString());
                code = GetSqlString(((int)item["code"]).ToString("X"));
                
                qualityId = item["itemQuality"].ToString();

                nameRaw = GetDisplayString(item["String"].ToString());
                name = "<div class=\"item_name " + quality.ToLower() + "\">" + nameRaw + "</div>";
                name = GetSqlString(name);

                //we can toss the base type if needed, or make another column for displaying below the in-game type description
                int baseRow = (int)item["baseRow"];
                if (baseRow >= 0)
                    typeRaw = GetDisplayString(items.Rows[baseRow]["String"].ToString());
                else
                    typeRaw = item["typeDescription_string"].ToString();    //probably fine as it is

                if (nameRaw == "Megan's Fury")//(quality == "Unique" &&item["inventory_string"].ToString().Contains("weapon"))
                { }
                type = (typeRaw != string.Empty) ? "<div class=\"item_type " + quality.ToLower() + "\">" + quality + " " + typeRaw + "</div>" : string.Empty;
                type = GetSqlString(type);

                //image name is whatever the index name thing is
                //image = item["name"].ToString();
                //if it's an armor item, add both the male and female versions side-by-side
                image = GetSqlString(GetImage((baseRow > 0) ? typeRaw.Replace("\"", "'") + ".png" : nameRaw.Replace("\"", "'") + ".png", 224));
                typeRaw = GetSqlString(typeRaw);
                nameRaw = GetSqlString(nameRaw);

                flavor = item["flavorText_string"].ToString();
                flavor = flavor.Replace("\n", "<br/>");
                flavor = (flavor != string.Empty) ? "<div class=\"item_flavor\">" + flavor + "</div>" : string.Empty;
                flavor = GetSqlString(flavor);

                dmgIcon = item["tooltipDamageIcon"].ToString();
                if (!string.IsNullOrEmpty(dmgIcon))
                {
                    dmgIcon = GetImage(dmgIcon + ".png", 50);
                    dmgIcon = "<td class=\"dmg_icon\">" + dmgIcon + "</td>";
                }

                stats = GetStats(item);
                //stats = AddElementThumbs(stats); //<div class=\"item_heading\">Stats</div>
                if (!string.IsNullOrEmpty(stats)) stats = "<div class=\"simple_line\"></div><div class=\"item_stats\">" + stats + "</div>";
                stats = GetSqlString(stats);

                modslots = GetModSlots(item["props2"].ToString());
                modslots = GetSqlString(modslots);

                feeds = GetFeeds((int)item["index"]);
                feeds = GetSqlString(feeds);

                levelRaw = item["fixedLevel"].ToString().Replace(";", "");
                if (levelRaw.Equals("")) levelRaw = item["maxLevel"].ToString();
                if (qualityId == "12") levelRaw = "63";
                level = levelRaw != "0" ? levelRaw : "Scales";

                //ignore clvl for necklaces
                //(not sure if this will always be the case)
                if (((int)item["typeDescription"]) != 3430 && level != "Scales")
                {
                    clvl = ilvls.Rows[int.Parse(level)]["levelRequirement"].ToString();
                    level = (!string.IsNullOrEmpty(level))
                                ? GetItemLevels(level, clvl, (int) item["itemQuality"])
                                : string.Empty;
                }
                level = GetSqlString(level);

                //since the increment stuff is only set when retrieving the damage, the function needs to be called twice
                //once to set the increments (ignore return value), and once to format the damage display after affixes have been set (to include bonuses)

                //first damage
                damage = GetDamage(item, unit);

                //assign affixes
                inherent = ConcatStrings(GetInherentAffixes(item));
                if (!string.IsNullOrEmpty(inherent)) inherent = "<div class=\"item_heading\">Inherent + Hidden Affixes</div><div class=\"item_affixes\">" + inherent + "</div>";
                inherent = GetSqlString(inherent);

                affixes = ConcatStrings(GetAffixes(item, unit));
                if (!string.IsNullOrEmpty(affixes)) affixes = "<div class=\"item_heading\">Special Affixes</div><div class=\"item_affixes\">" + affixes + "</div>";
                affixes = GetSqlString(affixes);

                //second damage
                //I think this is right, since two damage things ended up here
                damage = GetDamage(item, unit);//<div class=\"item_heading\">Damage</div>
                if (!string.IsNullOrEmpty(damage))
                {
                    damage = "<td><div class=\"item_damage\">" + damage + "</div></td>";
                    damage = "<div class=\"simple_line\"></div><table><tr>" + dmgIcon + damage + "</tr></table>";
                }
                damage = GetSqlString(damage);
                dmgIcon = GetSqlString(dmgIcon);

                //damage = GetDamage(item, unit);
                //if (!string.IsNullOrEmpty(damage)) damage = "<div class=\"item_heading\">Damage</div><div class=\"item_damage\">" + damage + "</div>";
                //damage = GetSqlString(damage);

                defence = GetDefence(item, unit);//<div class=\"item_heading\">Defence</div>
                if (!string.IsNullOrEmpty(defence)) defence = "<div class=\"item_defence\">" + defence + "</div>";
                defence = GetSqlString(defence);

                table.AddRow(id, code, image, name, type, flavor, dmgIcon, damage, defence, stats, modslots, feeds, level, inherent, affixes, qualityId, nameRaw, typeRaw, levelRaw);
            }

            return table.GetFullScript();
        }

        private string GetDisplayString(string itemRow)
        {
            //skip invalid entries
            if (string.IsNullOrWhiteSpace(itemRow) || itemRow.CompareTo("-1") == 0)
                return string.Empty;

            var strings = Manager.GetDataTable("Strings_Strings");
            string name = string.Empty;
            foreach (DataRow row in strings.Rows)
            {
                //check if it's a reference row with the default display string
                if (row["ReferenceId"].ToString().CompareTo(itemRow) == 0
                    && (row["Attribute1"].ToString().CompareTo("Default") == 0
                        || row["Attribute2"].ToString().CompareTo("Default") == 0
                        || row["Attribute3"].ToString().CompareTo("Default") == 0
                        || row["Attribute4"].ToString().CompareTo("Default") == 0))
                {
                    name = row["String"].ToString();
                    break;
                }
            }

            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("Name with reference ID " + itemRow + " not found");

            return name;
        }

        private string GetStats(DataRow item)
        {
            var strings = new List<string>();

            var level = !string.IsNullOrEmpty(item["fixedLevel"].ToString()) ?  Int32.Parse(item["fixedLevel"].ToString().Replace(";", "")) : (int)item["level"];
            var itemlevels = Manager.GetDataTable("ITEM_LEVELS");

            int interrupt = (int)itemlevels.Rows[level]["interruptAttack"];
            int sfxAtk = (int)itemlevels.Rows[level]["sfxAttackAbility"];
            int sfxDef = (int)itemlevels.Rows[level]["sfxDefenceAbility"];

            if (((int)item["criticalPct"]) != 0) strings.Add("Critical Chance: " + item["criticalPct"] + "%");
            if (((int)item["criticalMult"]) != 0) strings.Add("Critical Damage: " + item["criticalMult"] + "%");
            if (((int)item["interruptAttackPct"]) != 0) strings.Add("Interrupt Strength: " + (interrupt * (int)item["interruptAttackPct"] / 100));
            if (((int)item["cdTicks"]) != 0)
            {
                string itemType = item["unitType_string"].ToString();
                if (itemType.Contains("beam"))
                    strings.Add("Rate of fire: constant");
                else
                    strings.Add("Rate of fire: " + (153600 / (int)item["cdTicks"]) + " shots/min");
            }

            if (((int)item["sfxPhysicalAbilityPct"]) != 0) strings.Add("Stun Attack Strength: " + (sfxAtk * (int)item["sfxPhysicalAbilityPct"] / 100));
            if (((int)item["sfxPhysicalDefensePct"]) != 0) strings.Add("Stun Defence: " + (sfxDef * (int)item["sfxPhysicalDefensePct"] / 100));
            if (((int)item["sfxFireAbilityPct"]) != 0) strings.Add("Ignite Attack Strength: " + (sfxAtk * (int)item["sfxFireAbilityPct"] / 100));
            if (((int)item["sfxFireDefensePct"]) != 0) strings.Add("Ignite Defence: " + (sfxDef * (int)item["sfxFireDefensePct"] / 100));
            if (((int)item["sfxElectricAbilityPct"]) != 0) strings.Add("Shock Attack Strength: " + (sfxAtk * (int)item["sfxElectricAbilityPct"] / 100));
            if (((int)item["sfxElectricDefensePct"]) != 0) strings.Add("Shock Defence: " + (sfxDef * (int)item["sfxElectricDefensePct"] / 100));
            if (((int)item["sfxSpectralAbilityPct"]) != 0) strings.Add("Phase Attack Strength: " + (sfxAtk * (int)item["sfxSpectralAbilityPct"] / 100));
            if (((int)item["sfxSpectralDefensePct"]) != 0) strings.Add("Phase Defence: " + (sfxDef * (int)item["sfxSpectralDefensePct"] / 100));
            if (((int)item["sfxToxicAbilityPct"]) != 0) strings.Add("Poison Attack Strength: " + (sfxAtk * (int)item["sfxToxicAbilityPct"] / 100));
            if (((int)item["sfxToxicDefensePct"]) != 0) strings.Add("Poison Defence: " + (sfxDef * (int)item["sfxToxicDefensePct"] / 100));

            return ConcatStrings(strings);
        }

        private string GetDefence(DataRow item, Unit unit)
        {
            var level = !string.IsNullOrEmpty(item["fixedLevel"].ToString()) ?  Int32.Parse(item["fixedLevel"].ToString().Replace(";", "")) : (int)item["level"];
            var strings = new List<string>();

            var armor = (int) item["armor"];
            var buffer = (int) Manager.GetDataTable("ITEM_LEVELS").Rows[level]["armor"];
            object bonus;
            if (unit.GetStat("armor_bonus") is int)
                bonus = 0;
            else
                bonus = ((Dictionary<string, object>) unit.GetStat("armor_bonus"))["all"];
            if (bonus is Evaluator.Range)
            {
                armor = armor * buffer / 100;
                Evaluator.Range range = (Evaluator.Range) bonus + armor;
                if (range.ToString() != "0") strings.Add(GetImage("armor.png", _height) + " Armor: " + range);
            }
            else
            {
                armor = armor * buffer / 100;
                if (bonus.ToString() != "0") armor += Convert.ToInt32(bonus);
                if (armor.ToString() != "0") strings.Add(GetImage("armor.png", _height) + " Armor: " + armor);
            }

            var shields = (int)item["shields"];
            buffer = (int)Manager.GetDataTable("ITEM_LEVELS").Rows[level]["shields"];
            bonus = unit.GetStat("shields_bonus");
            if (bonus is Evaluator.Range)
            {
                shields = shields * buffer / 100;
                Evaluator.Range range = (Evaluator.Range)bonus + shields;
                if (range.ToString() != "0") strings.Add(GetImage("shields.png", _height) + " Shields: " + range);
            }
            else
            {
                shields = shields * buffer / 100;
                if (bonus.ToString() != "0") shields += Convert.ToInt32(bonus);
                if (shields.ToString() != "0") strings.Add(GetImage("shields.png", _height) + " Shields: " + shields);
            }

            return ConcatStrings(strings);
        }

        private IList<string> GetInherentAffixes(DataRow item, Unit unit = null)
        {
            var evaluator = new Evaluator { Unit = unit ?? new Item(), Manager = Manager, Game3 = new Game3() };
            var scripts = item["props1"].ToString() + item["props2"] + item["props3"] + item["props4"] + item["props5"];

            //it might be hidden but it's a fraction that's likely rounded to 0, so it's not worth displaying
            scripts = scripts.Replace("SetStat673('hp_regen', 1);", "").Replace("SetStat673('power_regen', 1);", "");

            var level = !string.IsNullOrEmpty(item["fixedLevel"].ToString()) ?  Int32.Parse(item["fixedLevel"].ToString().Replace(";", "")) : (int)item["level"];
            evaluator.Unit.SetStat("level", level);

            evaluator.Evaluate(scripts);
            return ItemDisplay.GetDisplayStrings(evaluator.Unit);
        }

        private string AddElementThumbs(string source)
        {
            source = source.Replace("Ignite", GetImage("Ignite.png", _height) + " Ignite");
            source = source.Replace("Shock", GetImage("Shock.png", _height) + "Shock");
            source = source.Replace("Poison", GetImage("Poison.png", _height) + "Poison");
            source = source.Replace("Phase", GetImage("Phase.png", _height) + "Phase");
            source = source.Replace("Stun", GetImage("Stun.png", _height) + "Stun");
            return source;
        }

        private string GetDamage(DataRow item,Unit unit)
        {
            if (string.IsNullOrEmpty(item["minBaseDmg"].ToString())) return string.Empty;

            //var unit = new Item();
            var evaluator = new Evaluator { Unit = unit, Manager = Manager, Game3 = new Game3() };
            var level = !string.IsNullOrEmpty(item["fixedLevel"].ToString()) ? Int32.Parse(item["fixedLevel"].ToString().Replace(";", "")) : (int)item["level"];
            //if (item["itemQuality"].ToString() == "12") level = 63; //manually correct mythic (although it seems to use a different ilvl anyway)
            var dmgMin = Int32.Parse(item["minBaseDmg"].ToString().Replace(";", ""));
            var dmgMax = Int32.Parse(item["maxBaseDmg"].ToString().Replace(";", ""));
            var dmgIncrement = (int) item["dmgIncrement"];
            var radialIncrement = (int) item["radialDmgIncrement"];
            var fieldIncrement = (int) item["fieldDmgIncrement"];
            //var dotIncrement = (int) item["dotDmgIncrement"]; // doesnt seem to be used
            //custom stuff for beam and hive damage (temporary until the type stuff is redone)
            //beam - direct damage of any element, or HARP
            bool isBeam = item["unitType_string"].ToString().Contains("beam") || item["unitType_string"].ToString().Contains("harp");
            //hive - any hive/funky bug thing weapon, used for eel (direct) and swarm (splash, never used)
            bool isHive = item["unitType_string"].ToString().Contains("hive") || item["unitType_string"].ToString().Contains("cabalist_bug2h_achiev");
            unit.SetStat("isBeam", isBeam);
            unit.SetStat("isHive", isHive);

;           unit.SetStat("damage_min", dmgMin);
            unit.SetStat("damage_max", dmgMax);
            unit.SetStat("level", level);
            unit.SetStat("dmg_increment", dmgIncrement);
            unit.SetStat("radial_increment", radialIncrement);
            unit.SetStat("field_increment", fieldIncrement);

            var scripts = item["props1"].ToString() + item["props2"] + item["props3"] + item["props4"] + item["props5"];
            evaluator.Evaluate(scripts);

            var builder = new StringBuilder();

            var values = unit.GetStat("dmg_fire");
            if (!(values is int))
            {
                string fireDmg;
                if (isBeam)
                    fireDmg = FormatBeamDmg((Dictionary<string, object>)values, "Fire");
                else
                    fireDmg = FormatDirectDmg((Dictionary<string, object>)values, "Fire");
                builder.Append(fireDmg);
            }

            values = unit.GetStat("dmg_spec");
            if (!(values is int))
            {
                string fireDmg;
                if (isBeam)
                    fireDmg = FormatBeamDmg((Dictionary<string, object>)values, "Spectral");
                else
                    fireDmg = FormatDirectDmg((Dictionary<string, object>)values, "Spectral");
                builder.Append(fireDmg);
            }

            values = unit.GetStat("dmg_phys");
            if (!(values is int))
            {
                string fireDmg;
                if (isBeam)
                    fireDmg = FormatBeamDmg((Dictionary<string, object>)values, "Physical");
                else
                    fireDmg = FormatDirectDmg((Dictionary<string, object>)values, "Physical");
                builder.Append(fireDmg);
            }

            values = unit.GetStat("dmg_toxic");
            if (!(values is int))
            {
                string fireDmg;
                if (isBeam)
                    fireDmg = FormatBeamDmg((Dictionary<string, object>)values, "Toxic");
                else
                    fireDmg = FormatDirectDmg((Dictionary<string, object>)values, "Toxic");
                builder.Append(fireDmg);
            }

            values = unit.GetStat("dmg_elec");
            if (!(values is int))
            {
                string fireDmg;
                if (isBeam)
                    fireDmg = FormatBeamDmg((Dictionary<string, object>)values, "Electricity");
                else if (isHive)
                    fireDmg = FormatEelDmg((Dictionary<string, object>)values, "Electricity");  //item display says it uses splash, but IT LIES
                else
                    fireDmg = FormatDirectDmg((Dictionary<string, object>)values, "Electricity");
                builder.Append(fireDmg);
            }

            values = unit.GetStat("rad_fire");
            if (!(values is int))
            {
                var fireDmg = FormatSplashDmg((Dictionary<string, object>)values, "Fire");
                builder.Append(fireDmg);
            }

            values = unit.GetStat("rad_spec");
            if (!(values is int))
            {
                var fireDmg = FormatSplashDmg((Dictionary<string, object>)values, "Spectral");
                builder.Append(fireDmg);
            }

            values = unit.GetStat("rad_phys");
            if (!(values is int))
            {
                var fireDmg = FormatSplashDmg((Dictionary<string, object>)values, "Physical");
                builder.Append(fireDmg);
            }

            values = unit.GetStat("rad_toxic");
            if (!(values is int))
            {
                string fireDmg;
                if (isHive)
                    fireDmg = FormatSwarmDmg((Dictionary<string, object>)values, "Toxic");
                else
                    fireDmg = FormatSplashDmg((Dictionary<string, object>)values, "Toxic");
                builder.Append(fireDmg);
            }

            values = unit.GetStat("rad_elec");
            if (!(values is int))
            {
                string fireDmg;
                //if (isHive)
                    //fireDmg = FormatEelDmg((Dictionary<string, object>)values, "Electricity");
                //else
                    fireDmg = FormatSplashDmg((Dictionary<string, object>)values, "Electricity");
                builder.Append(fireDmg);
            }

            values = unit.GetStat("field_fire");
            if (!(values is int))
            {
                var fireDmg = FormatFieldDmg((Dictionary<string, object>)values, "Fire");
                builder.Append(fireDmg);
            }

            values = unit.GetStat("field_spec");
            if (!(values is int))
            {
                var fireDmg = FormatFieldDmg((Dictionary<string, object>)values, "Spectral");
                builder.Append(fireDmg);
            }

            values = unit.GetStat("field_phys");
            if (!(values is int))
            {
                var fireDmg = FormatFieldDmg((Dictionary<string, object>)values, "Physical");
                builder.Append(fireDmg);
            }

            values = unit.GetStat("field_toxic");
            if (!(values is int))
            {
                var fireDmg = FormatFieldDmg((Dictionary<string, object>)values, "Toxic");
                builder.Append(fireDmg);
            }

            values = unit.GetStat("field_elec");
            if (!(values is int))
            {
                var fireDmg = FormatFieldDmg((Dictionary<string, object>)values, "Electricity");
                builder.Append(fireDmg);
            }

            values = unit.GetStat("dot_fire");
            if (!(values is int))
            {
                var fireDmg = FormatDotDmg((Dictionary<string, object>)values, "Fire");
                builder.Append(fireDmg);
            }

            values = unit.GetStat("dot_spec");
            if (!(values is int))
            {
                var fireDmg = FormatDotDmg((Dictionary<string, object>)values, "Spectral");
                builder.Append(fireDmg);
            }

            values = unit.GetStat("dot_phys");
            if (!(values is int))
            {
                var fireDmg = FormatDotDmg((Dictionary<string, object>)values, "Physical");
                builder.Append(fireDmg);
            }

            values = unit.GetStat("dot_toxic");
            if (!(values is int))
            {
                var fireDmg = FormatDotDmg((Dictionary<string, object>)values, "Toxic");
                builder.Append(fireDmg);
            }

            values = unit.GetStat("dot_elec");
            if (!(values is int))
            {
                var fireDmg = FormatDotDmg((Dictionary<string, object>)values, "Electricity");
                builder.Append(fireDmg);
            }

            values = unit.GetStat("rad_toxic_hive");
            if (!(values is int))
            {
                var fireDmg = FormatSwarmDmg((Dictionary<string, object>)values, "Toxic");
                builder.Append(fireDmg);
            }

            //if there's no damage to display, it must be a focus item that has power damage
            if (builder.Length == 0)
            {
                DataRow lvlRow = Manager.GetDataTable("ITEM_LEVELS").Rows[(int)unit.GetStat("level")];
                builder.Append(string.Format("'''Power''': {0}-{1}",
                    (int)(dmgMin / 100.0 * (int)lvlRow["baseDamageMultiplyer"]),
                    (int)(dmgMax / 100.0 * (int)lvlRow["baseDamageMultiplyer"])
                    ));
            }

            return builder.ToString();
        }

        internal string FormatBeamDmg(Dictionary<string, object> values, string classid)
        {
            var mask = "[classid] Dmg (Beam): [string1]-[string2]/sec";
            var icon = GetImage(classid + "Direct.png", _height);
            var min = values["min"].ToString();
            var max = values["max"].ToString();
            mask = mask.Replace("[icon]", icon);
            mask = mask.Replace("[classid]", classid);
            mask = mask.Replace("[string1]", min);
            mask = mask.Replace("[string2]", max);
            mask = "<div class=\"" + classid + "\">" + mask + "</div>";
            return mask;
        }

        internal string FormatEelDmg(Dictionary<string, object> values, string classid)
        {
            var mask = "[classid] Dmg (Eel): [string1]-[string2]/sec";
            var icon = GetImage(classid + "Direct.png", _height);
            var min = values["min"].ToString();
            var max = values["max"].ToString();
            mask = mask.Replace("[icon]", icon);
            mask = mask.Replace("[classid]", classid);
            mask = mask.Replace("[string1]", min);
            mask = mask.Replace("[string2]", max);
            mask = "<div class=\"" + classid + "\">" + mask + "</div>";
            return mask;
        }

        internal string FormatSwarmDmg(Dictionary<string, object> values, string classid)
        {
            var mask = "[classid] Dmg (Swarm): [string1]-[string2]/sec/[string3]m";
            var icon = GetImage(classid + "Direct.png", _height);
            var min = values["min"].ToString();
            var max = values["max"].ToString();
            var range = values["range"].ToString();
            mask = mask.Replace("[icon]", icon);
            mask = mask.Replace("[classid]", classid);
            mask = mask.Replace("[string1]", min);
            mask = mask.Replace("[string2]", max);
            mask = mask.Replace("[string3]", range);
            mask = "<div class=\"" + classid + "\">" + mask + "</div>";
            return mask;
        }
        
        internal string FormatDotDmg(Dictionary<string, object> values, string classid)
        {
            //var mask = "[icon][classid] Dmg (DoT): [string1]-[string2]/sec for [string3] seconds";
            var mask = "[classid] Dmg (DoT): [string1]-[string2]/sec for [string3] seconds";
            var icon = GetImage(classid + "DoT.png", _height);
            var min = values["min"].ToString();
            var max = values["max"].ToString();
            var secs = values["secs"].ToString();
            mask = mask.Replace("[icon]", icon);
            mask = mask.Replace("[classid]", classid);
            mask = mask.Replace("[string1]", min);
            mask = mask.Replace("[string2]", max);
            mask = mask.Replace("[string3]", secs);
            mask = "<div class=\"" + classid + "\">" + mask + "</div>";
            return mask;
        }

        internal string FormatFieldDmg(Dictionary<string, object> values, string classid)
        {
            //var mask = "[icon][classid] Dmg (Field): [string1]-[string2]/sec/[string4]m for [string3] seconds";
            var mask = "[classid] Dmg (Field): [string1]-[string2]/sec/[string4]m for [string3] seconds";
            var icon = GetImage(classid + "Field.png", _height);
            var min = values["min"].ToString();
            var max = values["max"].ToString();
            var range = values["range"].ToString();
            var secs = values["secs"].ToString();
            mask = mask.Replace("[icon]", icon);
            mask = mask.Replace("[classid]", classid);
            mask = mask.Replace("[string1]", min);
            mask = mask.Replace("[string2]", max);
            mask = mask.Replace("[string4]", range);
            mask = mask.Replace("[string3]", secs);
            mask = "<div class=\"" + classid + "\">" + mask + "</div>";
            return mask;
        }

        internal string FormatDirectDmg(Dictionary<string, object> values, string classid)
        {
            //var mask = "[icon][classid] Dmg (Direct): [string1]-[string2]";
            var mask = "[classid] Dmg (Direct): [string1]-[string2]";
            var icon = GetImage(classid + "Direct.png", _height);
            var min = values["min"].ToString();
            var max = values["max"].ToString();
            mask = mask.Replace("[icon]", icon);
            mask = mask.Replace("[classid]", classid);
            mask = mask.Replace("[string1]", min);
            mask = mask.Replace("[string2]", max);
            mask = "<div class=\"" + classid + "\">" + mask + "</div>";
            return mask;
        }

        internal string FormatSplashDmg(Dictionary<string, object> values, string classid)
        {
            //var mask = "[icon][classid] Dmg (Splash): [string1]-[string2]/[string3]m";
            var mask = "[classid] Dmg (Splash): [string1]-[string2]/[string3]m";
            var icon = GetImage(classid + "Splash.png", _height);
            var min = values["min"].ToString();
            var max = values["max"].ToString();
            var range = values["range"].ToString();
            mask = mask.Replace("[icon]", icon);
            mask = mask.Replace("[classid]", classid);
            mask = mask.Replace("[string1]", min);
            mask = mask.Replace("[string2]", max);
            mask = mask.Replace("[string3]", range);
            mask = "<div class=\"" + classid + "\">" + mask + "</div>";
            return mask;
        }

        private string GetItemLevels(string level, string clvl, int quality)
        {
            int itemLevel = int.Parse(level);
            if (itemLevel <= 1) return string.Empty;
            if (quality == 12) itemLevel = 63;
            int charLevel = int.Parse(clvl);// (quality == 12) ? 55 : itemLevel - 4 - ((itemLevel) / 10);

            string output = "<div class=\"simple_line\"></div>";
            output += "<div class=\"item_level\">Item Level: " + itemLevel;
            if (charLevel > 1) output += "<br/>Requires Character Level: " + charLevel + "</div>";
            return output;
        }

        private string GetModSlots(string modsScript)
        {
            var evaluator = new Evaluator { Unit = new Item() };
            evaluator.Evaluate(modsScript);

            var strings = new StringBuilder();
            var itemSlots = evaluator.Unit.GetStat("item_slots");
            if (itemSlots is int) return strings.ToString();

            var noSlots = string.Empty;
            int slotsAdded = 0;

            strings.Append("<div class=\"simple_line\"></div>");
            strings.Append("<div class=\"item_mods\">");
            strings.Append("<table>");
            strings.Append("<tr>");
            foreach (var slot in (Dictionary<string, object>)itemSlots)
            {
                //skip slots with 0
                if (slot.Value is double && ((double)slot.Value) == 0) continue;

                slotsAdded++;

                //int maxSlot = (slot.Value is Evaluator.Range) ? ((Evaluator.Range) slot.Value).End : Convert.ToInt32(slot.Value);
                strings.Append("<td>");
                //for (int i = 0; i < maxSlot; i++)
                    strings.Append(GetImage(slot.Key + ".png", 48));
                strings.Append("</td>");
                noSlots += "<td>" + slot.Value + "</td>";
            }
            strings.Append("</tr>");
            strings.Append("<tr>" + noSlots + "</tr>");
            strings.Append("</table>");
            strings.Append("</div>");

            //if no slots were added, return blank (might not be able to effectively determine this before the loop)
            if (slotsAdded == 0) return string.Empty;

            return strings.ToString();
        }

        private IList<string> GetAffixes(DataRow item, Unit unit)
        {
            var evaluator = new Evaluator {Unit = unit, Manager = Manager, Game3 = new Game3()};
            var table = Manager.GetDataTable("AFFIXES");
            var affixes = item["affix"].ToString();

            var level = !string.IsNullOrEmpty(item["fixedLevel"].ToString()) ?  Int32.Parse(item["fixedLevel"].ToString().Replace(";", "")) : (int)item["level"];
            if (level == 0) level = 55;
            evaluator.Unit.SetStat("level", level);

            string[] split = affixes.Split(',');
            foreach (var i in split)
            {
                var affix = int.Parse(i);
                if (affix == -1) break; // no more affixes
                var script = table.Rows[affix]["property1"].ToString();
                //weird level-changing property that screws up formulas, skip it (always appears in first property, pretty sure it doesn't affect anything else)
                if (script.Contains("SetStat673('level'"))
                    script = string.Empty;
                script += table.Rows[affix]["property2"].ToString();
                script += table.Rows[affix]["property3"].ToString();
                script += table.Rows[affix]["property4"].ToString();
                script += table.Rows[affix]["property5"].ToString();
                script += table.Rows[affix]["property6"].ToString();
                if (String.IsNullOrEmpty(script)) continue; // no affix script
                evaluator.Evaluate(script);
            }

            return ItemDisplay.GetDisplayStrings(evaluator.Unit);
        }

        private string ArrayToHtmlList(IList<string> strings, string classid)
        {
            if (strings.Count == 0) return string.Empty;

            var builder = new StringBuilder();
            builder.Append("<ul class=\"" + classid + "\">");
            foreach (var str in strings)
            {
                builder.Append("<li>" + str + "</li>");
            }
            builder.Append("</ul>");

            return builder.ToString();
        }

        private string GetFeeds(int itemid)
        {
            Unit unit = new Item();
            var evaluator = new Evaluator { Unit = unit, Manager = Manager };
            var item = Manager.GetDataTable("ITEMS").Rows[itemid];
            var table = Manager.GetDataTable("AFFIXES");
            var affixes = item["affix"].ToString();
            var scripts = item["perLevelProps1"].ToString();
            scripts += item["perLevelProps2"].ToString();

            var level = !string.IsNullOrEmpty(item["fixedLevel"].ToString()) ?  Int32.Parse(item["fixedLevel"].ToString().Replace(";", "")) : (int)item["level"];
            evaluator.Unit.SetStat("level", level);

            string[] split = affixes.Split(',');
            foreach (var i in split)
            {
                var affix = int.Parse(i);
                if (affix == -1) break; // no more affixes
                var script = table.Rows[affix]["property1"].ToString();
                script += table.Rows[affix]["property2"].ToString();
                script += table.Rows[affix]["property3"].ToString();
                script += table.Rows[affix]["property4"].ToString();
                script += table.Rows[affix]["property5"].ToString();
                script += table.Rows[affix]["property6"].ToString();
                if (String.IsNullOrEmpty(script)) continue; // no affix script
                evaluator.Evaluate(script);
            }

            if (scripts != string.Empty) evaluator.Evaluate(scripts);

            var strings = new List<string>();

            string feed = string.Empty;

            var accuracy = unit.GetStat("accuracy_feed");
            if ((accuracy is double && (double)accuracy != 0)
                || accuracy is string || accuracy is Evaluator.Range)
            {
                feed = ItemDisplay.FormatFeed(accuracy);
                strings.Add(feed + " Acc");
            }

            var strength = unit.GetStat("strength_feed");
            if ((strength is double && (double)strength != 0)
                || strength is string || strength is Evaluator.Range)
            {
                feed = ItemDisplay.FormatFeed(strength);
                strings.Add(feed + " Str");
            }

            var stamina = unit.GetStat("stamina_feed");
            if ((stamina is double && (double)stamina != 0)
                || stamina is string || stamina is Evaluator.Range)
            {
                feed = ItemDisplay.FormatFeed(stamina);
                strings.Add(feed + " Stam");
            }

            var willpower = unit.GetStat("willpower_feed");
            if ((willpower is double && (double)willpower != 0)
                || willpower is string || willpower is Evaluator.Range)
            {
                feed = ItemDisplay.FormatFeed(willpower);
                strings.Add(feed + " Will");
            }

            if (strings.Count == 0) return string.Empty;
            return "<div class=\"simple_line\"></div><div class=\"item_feed\">" + GetCSVString(strings) + "</div>";
        }

        public string GetColorCode(int quality)
        {
            switch(quality)
            {
                case 0:
                    return WikiColors.Common;
                case 1:
                case 6:
                    return WikiColors.Enhanced;
                case 2:
                case 7:
                    return WikiColors.Rare;
                case 3:
                case 8:
                    return WikiColors.Legendary;
                case 5:
                case 10:
                    return WikiColors.Unique;
                case 12:
                    return WikiColors.Mythic;
                case 14:
                    return WikiColors.Set;
                default:
                    throw new ArgumentException("Unhandled color code: " + quality);
            }
        }
    }
}
