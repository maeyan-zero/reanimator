using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Hellgate;
using Hellgate.Excel;
using MediaWiki.Parser;
using MediaWiki.Parser.Class;

namespace MediaWiki.Util
{
    class ItemDisplay
    {
        public static FileManager Manager { get; set; }
        private static Evaluator Evaluator { get; set; }

        public static string[] GetDisplayStrings(Unit unit)
        {
            if (Manager == null) throw new Exception("FileManager Null");

            var strings = new List<string>();
            var itemDisplay = Manager.GetDataTable("ITEMDISPLAY");
            Evaluator = new Evaluator {Unit = unit, Context = new Context()};

            DataRow last = null;

            foreach (DataRow row in itemDisplay.Rows)
            {
                if (((int)row["toolTipArea"]) != 0 && ((int)row["toolTipArea"]) != 20) continue;

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

                strings.Add(result);
            }

            return strings.ToArray();
        }

        private static bool IsConditionMet(string condition)
        {
            if (String.IsNullOrEmpty(condition)) return true;
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
                        if (ctrl == Display.Ctrl.ScriptPlusminus) result = "+" + result; // todo lazy
                        format = format.Replace("[string" + i + "]", result.ToString());
                        break;
                    case Display.Ctrl.ParamString:
                        control = (string) row["ctrlStat_string"];
                        result = Evaluator.Unit.GetStatParam(control);
                        switch (control)
                        {
                            case "power_cost_pct_skillgroup":
                            case "cooldown_pct_skillgroup":
                                result = GetGroupString((string) result);
                                break;
                            case "skill_level":
                                result = GetSkillString((string) result);
                                break;
                        }
                        format = format.Replace("[string" + i + "]", result.ToString());
                        break;
                    case Display.Ctrl.StatValue:
                        control = (string) row["ctrlStat_string"];
                        result = Evaluator.Unit.GetStatValue(control);
                        format = format.Replace("[string" + i + "]", result.ToString());
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
            return (evaluated.Count > 0) ? evaluated[0].ToString() : string.Empty;
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
                double val1 = ((Evaluator.Range) result).Start;
                double val2 = ((Evaluator.Range) result).End;
                return val1/10 + "~" + val2/10;
            }

            return (Double.Parse(result.ToString())/10).ToString();
        }

        private static string GetGroupString(string stringid)
        {
            DataTable skillGroups = Manager.GetDataTable("SKILLGROUPS");
            foreach (DataRow row in skillGroups.Rows)
            {
                if ((string) row["name"] == stringid)
                {
                    DataTable strings = Manager.GetDataTable("Strings_Strings");
                    foreach (DataRow srow in strings.Rows)
                    {
                        if ((int) srow["ReferenceId"] == (int) row["displayString"])
                        {
                            string group = (string) srow["String"];
                            group = group.Replace("{c:light_gray}", "");
                            group = group.Replace("{\\c}", "");
                            return group;
                        }
                    }
                }
            }
            return String.Empty;
        }
    }
}
