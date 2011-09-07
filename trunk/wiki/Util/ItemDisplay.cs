using System;
using System.Collections.Generic;
using System.Data;
using Hellgate;
using Hellgate.Excel;
using MediaWiki.Articles;
using MediaWiki.Parser;
using MediaWiki.Parser.Class;
using System.Linq;

namespace MediaWiki.Util
{
    class ItemDisplay
    {
        public static FileManager Manager { get; set; }
        private static Evaluator Evaluator { get; set; }

        public static string[] GetDisplayStrings(Unit unit, bool inherent = false)
        {
            if (Manager == null) throw new Exception("FileManager Null");

            var strings = new List<string>();
            var itemDisplay = Manager.GetDataTable("ITEMDISPLAY");
            Evaluator = new Evaluator {Unit = unit, Context = new Context(), Manager = Manager};

            DataRow last = null;

            foreach (DataRow row in itemDisplay.Rows)
            {
                if (inherent)
                {
                    if (((int)row["toolTipArea"]) != 20)
                        continue;
                }
                else
                {
                    if (((int)row["toolTipArea"]) != 0 && !row["exampleDescription"].ToString().Contains("thorns"))
                        continue;
                }

                var rule1 = (Display.Rule)row["rule1"];
                var rule2 = (Display.Rule)row["rule2"];
                var rule3 = (Display.Rule)row["rule3"];

                if (rule1 == Display.Rule.Ditto) row["displayCondition1"] = last["displayCondition1"];
                if (rule2 == Display.Rule.Ditto) row["displayCondition2"] = last["displayCondition2"];
                if (rule3 == Display.Rule.Ditto) row["displayCondition3"] = last["displayCondition3"];

                var condition1 = (string) row["displayCondition1"];
                var condition2 = (string) row["displayCondition2"];
                var condition3 = (string) row["displayCondition3"];

                last = row;

                if (IsConditionMet(condition1) == false) continue;
                if (IsConditionMet(condition2) == false) continue;
                if (IsConditionMet(condition3) == false) continue;

                var result = GetDisplayString(row);
                if (String.IsNullOrEmpty(result)) continue;

                if (result.Contains("Thorns"))
                    result = FormatThorns(result);

                strings.Add(result);
            }

            string skill = GetSkillBonus(unit);
            if (!String.IsNullOrWhiteSpace(skill))
                strings.Add(skill);

            return strings.ToArray();
        }

        private static bool IsConditionMet(string condition)
        {
            if (String.IsNullOrEmpty(condition)) return true;

            if (condition == "GetStat666('evade') || (GetStat666('evade', 1) == GetStat666('evade', 2) && GetStat666('evade', 1) != 0);")
                return false;

            var result = Evaluator.Evaluate(condition);
            if (result[0] is int) return (int) result[0] > 0;
            if (result[0] is double) return (double) result[0] > 0;
            if (result[0] is bool) return (bool) result[0];
            return true; // If its a string or anything else - its true
        }

        private static string GetDisplayString(DataRow row)
        {
            if ((int) row["formatString"] == -1) return String.Empty;

            var format = (string) row["formatString_string"];

            for (int i = 1; i <= 4; i++)
            {
                var ctrl = (Display.Ctrl) row["ctrl" + i];
                string script;
                object result;
                string control;

                switch (ctrl)
                {
                    case Display.Ctrl.Script:
                    case Display.Ctrl.ScriptPlusminus: // todo
                        script = (string) row["val" + i];

                        // DEBUG until Alex fixes the decompiler
                        if (script == "GetStat666('evade') + (GetStat666('evade', 1) == GetStat666('evade', 2)) ? GetStat666('evade', 1) : 0;")
                            break;

                        result = ResultAsString(Evaluator.Evaluate(script));
                        if (ctrl == Display.Ctrl.ScriptPlusminus&&result.ToString().Count(ch=>ch=='-')<2) result = "+" + result; // todo lazy
                        format = format.Replace("[string" + i + "]", result.ToString());
                        break;
                    case Display.Ctrl.ParamString:
                        control = (string) row["ctrlStat_string"];
                        result = Evaluator.Unit.GetStatParam(control);
                        string total = string.Empty;
                        //there might be more than one (like skill bonuses), so go through them all
                        string[] ary = (string[])result;
                        for (int e = 0; e < ary.Length; e++)
                        {
                            switch (control)
                            {
                                case "power_cost_pct_skillgroup":
                                case "cooldown_pct_skillgroup":
                                case "damage_percent_skillgroup":
                                    result = GetGroupString(ary[e]);
                                    break;
                                case "skill_level":
                                    result = GetSkillString(ary[e]);
                                    break;
                            }
                            total += format.Replace("[string" + i + "]", result.ToString());
                            if (e < ary.Length - 1)
                                total += "<br />";
                        }
                        format = total;//format.Replace("[string" + i + "]", result.ToString());
                        break;
                    case Display.Ctrl.StatValue:
                        control = (string) row["ctrlStat_string"];
                        result = Evaluator.Unit.GetStatValue(control);
                        object[] array = (object[])result;
                        string[] parts = format.Split(new string[] { "<br />" }, StringSplitOptions.RemoveEmptyEntries);
                        //might be multiple entries, so go through all of them
                        for (int p = 0; p < array.Length; p++)
                        {
                            parts[p] = parts[p].Replace("[string" + i + "]", array[p].ToString());
                            if (p < array.Length - 1)
                                parts[p] += "<br />";
                        }
                        format = parts.Aggregate((r,s)=>r+s);//format.Replace("[string" + i + "]", result.ToString());
                        break;
                    case Display.Ctrl.DivBy10:
                        script = (string)row["val" + i];
                        result = ResultDivBy10(Evaluator.Evaluate(script));
                        format = format.Replace("[string" + i + "]", result.ToString());
                        break;
                    case Display.Ctrl.ScriptNoprint:
                        break;
                }
            }

            return format;
        }

        private static string GetSkillString(string result)
        {
            DataTable skills = Manager.GetDataTable("SKILLS");
            foreach (DataRow row in skills.Rows)
            {
                if ((string)row["skill"] == result)
                {
                    return ((int)row["displayName"] != -1) ? (string)row["displayName_string"] : string.Empty;
                }
            }
            return string.Empty;
        }

        private static string ResultAsString(IList<object> evaluated)
        {
            if (evaluated.Count == 0)
                return string.Empty;

            var stat = evaluated[0];
            if (stat is Parser.Evaluator.Range || stat is int || stat is double)
                return stat.ToString();

            return "(" + evaluated[0].ToString() + ")";
        }

        /// <summary>
        /// The results dont seem to need to be divided by 10.
        /// </summary>
        /// <param name="evaluated"></param>
        /// <returns></returns>
        private static string ResultDivBy10(IList<object> evaluated)
        {
            if (evaluated.Count == 0) return String.Empty;

            var result = evaluated[0];
            if (result is Evaluator.Range)
            {
                double val1 = (double)((Evaluator.Range)result).Start;
                double val2 = (double)((Evaluator.Range)result).End;
                return val1/10 + "~" + val2/10;
            }

            return (Double.Parse(result.ToString())/10).ToString();
        }

        private static string GetGroupString(string stringid)
        {
            DataTable skillGroups = Manager.GetDataTable("SKILLGROUPS");
            foreach (DataRow row in skillGroups.Rows)
            {
                if ((string) row["name"] != stringid) continue;
                DataTable strings = Manager.GetDataTable("Strings_Strings");
                foreach (DataRow srow in strings.Rows)
                {
                    if ((int) srow["ReferenceId"] != (int) row["displayString"]) continue;
                    string group = (string) srow["String"];
                    group = group.Replace("{c:light_gray}", "");
                    group = group.Replace("{\\c}", "");
                    return group;
                }
            }
            return String.Empty;
        }

        private static string FormatThorns(string thorns)
        {
            string element = thorns.Split(' ')[0];
            return "<span class=\"" + element + "\">" + thorns + "</span>";
        }

        internal static string[] GetModSlots(Unit unit)
        {
            var strings = new List<string>();
            var itemSlots = unit.GetStat("item_slots");
            if (itemSlots is int) return strings.ToArray();

            foreach (var slot in (Dictionary<string, object>) itemSlots)
            {
                switch (slot.Key)
                {
                    case "fuel":
                        strings.Add(WikiScript.GetImage("fuel", 15) + " " + slot.Value);
                        break;
                    case "relic":
                        strings.Add(WikiScript.GetImage("relic", 15) + " " + slot.Value);
                        break;
                    case "tech":
                        strings.Add(WikiScript.GetImage("tech", 15) + " " + slot.Value);
                        break;
                    case "battery":
                        strings.Add(WikiScript.GetImage("battery", 15) + " " + slot.Value);
                        break;
                    case "ammo":
                        strings.Add(WikiScript.GetImage("ammo", 15) + " " + slot.Value);
                        break;
                    case "rockets":
                        strings.Add(WikiScript.GetImage("rocket", 15) + " " + slot.Value);
                        break;
                    default:
                        throw new Exception("Unknown mod slot: " + slot.Key);
                }
            }
            return strings.ToArray();
        }

        public static string[] GetFeedCosts(Unit unit)
        {
            var strings = new List<string>();

            string feed = string.Empty;

            var accuracy = unit.GetStat("accuracy_feed");
            if ((accuracy is double && (double)accuracy != 0)
                || accuracy is string || accuracy is Evaluator.Range)
            {
                feed = FormatFeed(accuracy);
                strings.Add(feed + " Acc");
            }

            var strength = unit.GetStat("strength_feed");
            if ((strength is double && (double)strength != 0)
                || strength is string || strength is Evaluator.Range)
            {
                feed = FormatFeed(strength);
                strings.Add(feed + " Str");
            }

            var stamina = unit.GetStat("stamina_feed");
            if ((stamina is double && (double)stamina != 0)
                || stamina is string || stamina is Evaluator.Range)
            {
                feed = FormatFeed(stamina);
                strings.Add(feed + " Stam");
            }

            var willpower = unit.GetStat("willpower_feed");
            if ((willpower is double && (double)willpower != 0)
                || willpower is string || willpower is Evaluator.Range)
            {
                feed = FormatFeed(willpower);
                strings.Add(feed + " Will");
            }

            return strings.ToArray();
        }

        public static string FormatFeed(object feed)
        {
            if (feed is Parser.Evaluator.Range)
                return feed.ToString();

            return "(" + feed + ")";
        }

        public static string GetSkillBonus(Unit unit)
        {
            string bonus = unit.GetStat("skill_bonus").ToString();
            if (String.Compare(bonus, "0") == 0)
                return string.Empty;
            return bonus;
        }
    }
}
