using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using Hellgate;
using MediaWiki.Parser;
using MediaWiki.Parser.Class;
using MediaWiki.Util;

namespace MediaWiki.Articles
{
    class Items : WikiScript
    {
        public Items(FileManager manager) : base(manager, "items")
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
                "code VARCHAR(4) NOT NULL",
                "image TEXT",
                "name TEXT",
                "type TEXT",
                "flavor TEXT",
                "damage TEXT",
                "defence TEXT",
                "stats TEXT",
                "mods TEXT",
                "feeds TEXT",
                "level TEXT",
                "inherent TEXT",
                "affixes TEXT"
                );

            ItemDisplay.Manager = Manager;
            var items = Manager.GetDataTable("ITEMS");
            var qualityTable = Manager.GetDataTable("ITEM_QUALITY");

            string id, code, name, type, flavor, quality, image, damage, stats, affixes, modslots, feeds, level, inherent, defence;

            foreach (DataRow item in items.Rows)
            {
                if (item["code"].ToString() == "0") continue; // ignore blank rows
                Debug.WriteLine("Parsing: " + item["Index"] + " (" + item["name"] + ")");

                //if (((int)item["Index"]) != 2680) continue;

                // Dependencies
                quality = ((int) item["itemQuality"]) != -1
                                    ? qualityTable.Rows[(int) item["itemQuality"]]["displayName_string"].ToString()
                                    : string.Empty;

                Unit unit = new Item(); // for sharing stats

                // Guts
                id = GetSqlEncapsulatedString(item["index"].ToString());
                code = GetSqlEncapsulatedString(((int)item["code"]).ToString("X"));
                image = GetSqlEncapsulatedString(GetImage(item["name"] + ".png", 224));

                name = item["String_string"].ToString();
                name = "<div class=\"item_name " + quality.ToLower() + "\">" + name + "</div>";
                name = GetSqlEncapsulatedString(name);

                type = item["typeDescription_string"].ToString();
                type = (type != string.Empty) ? "<div class=\"item_type " + quality.ToLower() + "\">" + quality + " " + type + "</div>" : string.Empty;
                type = GetSqlEncapsulatedString(type);

                flavor = item["flavorText_string"].ToString();
                flavor = flavor.Replace("\n", "<br/>");
                flavor = (flavor != string.Empty) ? "<div class=\"item_flavor\">" + flavor + "</div>" : string.Empty;
                flavor = GetSqlEncapsulatedString(flavor);

                damage = GetDamage(item);
                if (!string.IsNullOrEmpty(damage)) damage = "<div class=\"item_heading\">Damage</div><div class=\"item_damage\">" + damage + "</div>";
                damage = GetSqlEncapsulatedString(damage);

                stats = GetStats(item);
                if (!string.IsNullOrEmpty(stats)) stats = "<div class=\"item_heading\">Stats</div><div class=\"item_stats\">" + stats + "</div>";
                stats = GetSqlEncapsulatedString(stats);

                modslots = GetModSlots(item["props2"].ToString());
                modslots = GetSqlEncapsulatedString(modslots);

                feeds = GetFeeds((int)item["index"]);
                feeds = GetSqlEncapsulatedString(feeds);

                level = item["fixedLevel"].ToString().Replace(";", "");
                level = (!string.IsNullOrEmpty(level)) ? GetItemLevels(level) : string.Empty;
                level = GetSqlEncapsulatedString(level);

                inherent = ConcatStrings(GetInherentAffixes(item));
                if (!string.IsNullOrEmpty(inherent)) inherent = "<div class=\"item_heading\">Inherent Affixes</div><div class=\"item_affixes\">" + inherent + "</div>";
                inherent = GetSqlEncapsulatedString(inherent);

                affixes = ConcatStrings(GetAffixes(item, unit));
                if (!string.IsNullOrEmpty(affixes)) affixes = "<div class=\"item_heading\">Special Affixes</div><div class=\"item_affixes\">" + affixes + "</div>";
                affixes = GetSqlEncapsulatedString(affixes);

                defence = GetDefence(item, unit);
                if (!string.IsNullOrEmpty(defence)) defence = "<div class=\"item_heading\">Defence</div><div class=\"item_defence\">" + defence + "</div>";
                defence = GetSqlEncapsulatedString(defence);

                table.AddRow(id, code, image, name, type, flavor, damage, defence, stats, modslots, feeds, level, inherent, affixes);
            }

            return table.GetFullScript();
        }

        private string GetStats(DataRow item)
        {
            var strings = new List<string>();

            if (((int)item["criticalPct"]) != 0) strings.Add("Critical Chance: " + item["criticalPct"] + "%");
            if (((int)item["criticalMult"]) != 0) strings.Add("Critical Damage: " + item["criticalMult"] + "%");
            if (((int)item["interruptAttackPct"]) != 0) strings.Add("Interrupt Strength: " + item["interruptAttackPct"]);
            if (((int)item["cdTicks"]) != 0) strings.Add("Rate of fire: " + (153600 / (int)item["cdTicks"]) + " shots/min");

            if (((int)item["sfxPhysicalAbilityPct"]) != 0) strings.Add("Stun Attack Strength: " + item["sfxPhysicalAbilityPct"]);
            if (((int)item["sfxPhysicalDefensePct"]) != 0) strings.Add("Stun Defence: " + item["sfxPhysicalDefensePct"]);
            if (((int)item["sfxFireAbilityPct"]) != 0) strings.Add("Ignite Attack Strength: " + item["sfxFireAbilityPct"]);
            if (((int)item["sfxFireDefensePct"]) != 0) strings.Add("Ignite Defence: " + item["sfxFireDefensePct"]);
            if (((int)item["sfxElectricAbilityPct"]) != 0) strings.Add("Shock Attack Strength: " + item["sfxElectricAbilityPct"]);
            if (((int)item["sfxElectricDefensePct"]) != 0) strings.Add("Shock Defence: " + item["sfxElectricDefensePct"]);
            if (((int)item["sfxSpectralAbilityPct"]) != 0) strings.Add("Phase Attack Strength: " + item["sfxSpectralAbilityPct"]);
            if (((int)item["sfxSpectralDefensePct"]) != 0) strings.Add("Phase Defence: " + item["sfxSpectralDefensePct"]);
            if (((int)item["sfxToxicAbilityPct"]) != 0) strings.Add("Poison Attack Strength: " + item["sfxToxicAbilityPct"]);
            if (((int)item["sfxToxicDefensePct"]) != 0) strings.Add("Poison Defence: " + item["sfxToxicDefensePct"]);

            return ConcatStrings(strings);
        }

        private string GetDefence(DataRow item, Unit unit)
        {
            var level = !string.IsNullOrEmpty(item["fixedLevel"].ToString()) ?  Int32.Parse(item["fixedLevel"].ToString().Replace(";", "")) : (int)item["level"];
            var strings = new List<string>();
            if (((int) item["armor"]) != 0)
            {
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
                    strings.Add("Armor: " + range);
                }
                else
                {
                    armor = armor * buffer / 100;
                    strings.Add("Armor: " + armor + Convert.ToInt32(bonus));
                }
            }
            if (((int)item["shields"]) != 0)
            {
                var shields = (int)item["shields"];
                var buffer = (int)Manager.GetDataTable("ITEM_LEVELS").Rows[level]["shields"];
                var bonus = unit.GetStat("shields_bonus");
                if (bonus is Evaluator.Range)
                {
                    shields = shields * buffer / 100;
                    Evaluator.Range range = (Evaluator.Range)bonus + shields;
                    strings.Add("Shields: " + range);
                }
                else
                {
                    shields = shields * buffer / 100;
                    if (bonus.ToString() != "0") shields += Convert.ToInt32(bonus);
                    strings.Add("Shields: " + shields);
                }
            }
            return ConcatStrings(strings);
        }

        private IList<string> GetInherentAffixes(DataRow item, Unit unit = null)
        {
            var evaluator = new Evaluator { Unit = unit ?? new Item(), Manager = Manager, Game3 = new Game3() };
            var scripts = item["props1"].ToString() + item["props2"] + item["props3"] + item["props4"] + item["props5"];

            var level = !string.IsNullOrEmpty(item["fixedLevel"].ToString()) ?  Int32.Parse(item["fixedLevel"].ToString().Replace(";", "")) : (int)item["level"];
            evaluator.Unit.SetStat("level", level);

            evaluator.Evaluate(scripts);
            return ItemDisplay.GetDisplayStrings(evaluator.Unit);
        }

        private string GetDamage(DataRow item)
        {
            if (string.IsNullOrEmpty(item["minBaseDmg"].ToString())) return string.Empty;

            var unit = new Item();
            var evaluator = new Evaluator { Unit = unit, Manager = Manager, Game3 = new Game3() };
            var level = !string.IsNullOrEmpty(item["fixedLevel"].ToString()) ? Int32.Parse(item["fixedLevel"].ToString().Replace(";", "")) : (int)item["level"];
            var dmgMin = Int32.Parse(item["minBaseDmg"].ToString().Replace(";", ""));
            var dmgMax = Int32.Parse(item["maxBaseDmg"].ToString().Replace(";", ""));
            var dmgIncrement = (int) item["dmgIncrement"];
            var radialIncrement = (int) item["radialDmgIncrement"];
            unit.SetStat("damage_min", dmgMin);
            unit.SetStat("damage_max", dmgMax);
            unit.SetStat("level", level);
            unit.SetStat("dmg_increment", dmgIncrement);
            unit.SetStat("radial_increment", radialIncrement);
            var scripts = item["props1"].ToString() + item["props2"] + item["props3"] + item["props4"] + item["props5"];
            evaluator.Evaluate(scripts);

            var builder = new StringBuilder();

            var values = unit.GetStat("dmg_fire");
            if (!(values is int))
            {
                var fireDmg = FormatDirectDmg((Dictionary<string, object>) values, "Fire");
                builder.Append(fireDmg);
            }

            values = unit.GetStat("dmg_spec");
            if (!(values is int))
            {
                var fireDmg = FormatDirectDmg((Dictionary<string, object>)values, "Spectral");
                builder.Append(fireDmg);
            }

            values = unit.GetStat("dmg_phys");
            if (!(values is int))
            {
                var fireDmg = FormatDirectDmg((Dictionary<string, object>)values, "Physical");
                builder.Append(fireDmg);
            }

            values = unit.GetStat("dmg_toxic");
            if (!(values is int))
            {
                var fireDmg = FormatDirectDmg((Dictionary<string, object>)values, "Toxic");
                builder.Append(fireDmg);
            }

            values = unit.GetStat("dmg_elec");
            if (!(values is int))
            {
                var fireDmg = FormatDirectDmg((Dictionary<string, object>)values, "Electricity");
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
                var fireDmg = FormatSplashDmg((Dictionary<string, object>)values, "Toxic");
                builder.Append(fireDmg);
            }

            values = unit.GetStat("rad_elec");
            if (!(values is int))
            {
                var fireDmg = FormatSplashDmg((Dictionary<string, object>)values, "Electricity");
                builder.Append(fireDmg);
            }

            return builder.ToString();
        }

        internal string FormatDirectDmg(Dictionary<string, object> values, string classid)
        {
            var mask = "[classid] Dmg (Direct): [string1]-[string2]";
            var min = values["min"].ToString();
            var max = values["max"].ToString();
            mask = mask.Replace("[classid]", classid);
            mask = mask.Replace("[string1]", min);
            mask = mask.Replace("[string2]", max);
            mask = "<div class=\"" + classid + "\">" + mask + "</div>";
            return mask;
        }

        internal string FormatSplashDmg(Dictionary<string, object> values, string classid)
        {
            var mask = "[classid] Dmg (Splash): [string1]-[string2]/[string3]m";
            var min = values["min"].ToString();
            var max = values["max"].ToString();
            var range = values["range"].ToString();
            mask = mask.Replace("[classid]", classid);
            mask = mask.Replace("[string1]", min);
            mask = mask.Replace("[string2]", max);
            mask = mask.Replace("[string3]", range);
            mask = "<div class=\"" + classid + "\">" + mask + "</div>";
            return mask;
        }

        private string GetItemLevels(string level)
        {
            int itemLevel = int.Parse(level);
            int charLevel = itemLevel - 4 - ((itemLevel)/10);
            string output = "<div class=\"item_heading\">Item Level</div>";
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

            strings.Append("<div class=\"item_heading\">Mods</div>");
            strings.Append("<div class=\"item_mods\">");
            strings.Append("<table>");
            strings.Append("<tr>");
            foreach (var slot in (Dictionary<string, object>)itemSlots)
            {
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
            return strings.ToString();
        }

        private IList<string> GetAffixes(DataRow item, Unit unit)
        {
            var evaluator = new Evaluator {Unit = unit, Manager = Manager, Game3 = new Game3()};
            var table = Manager.GetDataTable("AFFIXES");
            var affixes = item["affix"].ToString();

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

            return ItemDisplay.GetDisplayStrings(evaluator.Unit);
        }

        private string ConcatStrings(IList<string> strings)
        {
            string concat = string.Empty;
            for (int i = 0; i < strings.Count; i++)
            {
                concat += strings[i];
                if (strings.Count != i + 1) concat += "<br/>";
            }
            return concat;
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

            return "<div class=\"item_heading\">Feed</div><div class=\"item_feed\">" + GetCSVString(strings) + "</div>";
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
