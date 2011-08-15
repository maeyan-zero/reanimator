
using System;
using System.Collections.Generic;
using System.Data;
using Hellgate;
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
            Evaluator = new Evaluator {Unit = unit};

            foreach (DataRow row in itemDisplay.Rows)
            {
                if ((int)row["toolTipArea"] != 0) continue;
                var condition1 = (string) row["displayCondition1"];
                var condition2 = (string) row["displayCondition2"];
                var condition3 = (string) row["displayCondition3"];
                if (IsConditionMet(condition1) == false) continue;
                if (IsConditionMet(condition2) == false) continue;
                if (IsConditionMet(condition3) == false) continue;
                strings.Add(GetDisplayString(row));
            }

            return strings.ToArray();
        }

        private static bool IsConditionMet(string condition)
        {
            if (String.IsNullOrEmpty(condition)) return true;
            var result = Evaluator.Evaluate(condition);
            return (bool) result[0];
        }

        private static string GetDisplayString(DataRow row)
        {
            var format = (string) row["formatString_string"];
            var val1 = (string) row["val1"];
            var val2 = (string) row["val2"];
            var val3 = (string) row["val3"];
            var val4 = (string) row["val4"];
            if (!String.IsNullOrEmpty(val1))
            {
                var result1 = Evaluator.Evaluate(val1)[0];
                format = format.Replace("[string1]", result1.ToString());
            }
            if (!String.IsNullOrEmpty(val2))
            {
                var result1 = Evaluator.Evaluate(val2)[0];
                format = format.Replace("[string2]", result1.ToString());
            }
            if (!String.IsNullOrEmpty(val3))
            {
                var result1 = Evaluator.Evaluate(val3)[0];
                format = format.Replace("[string3]", result1.ToString());
            }
            if (!String.IsNullOrEmpty(val4))
            {
                var result1 = Evaluator.Evaluate(val4)[0];
                format = format.Replace("[string4]", result1.ToString());
            }
            return format;
        }
    }
}
