using System.Collections.Generic;
using System.Linq;

namespace MediaWiki.Parser.Class
{
    public class Unit
    {
        protected Dictionary<string, object> Stats = new Dictionary<string, object>();

	    public object SetStat(string label, object value, bool append = false)
        {
            if (Stats.ContainsKey(label))
            {
                if (append)
                {
                    if (Stats[label] is Evaluator.Range && value is Evaluator.Range)
                        Stats[label] = (Evaluator.Range) Stats[label] + (Evaluator.Range) value;
                    if (Stats[label] is Evaluator.Range && value is int)
                        Stats[label] = (Evaluator.Range) Stats[label] + (int) value;
                    if (Stats[label] is Evaluator.Range && value is double)
                        Stats[label] = (Evaluator.Range) Stats[label] + (double) value;
                    if (Stats[label] is int && value is int)
                        Stats[label] = (int) Stats[label] + (int) value;
                    if (Stats[label] is double && value is double)
                        Stats[label] = (double) Stats[label] + (double) value;
                    if (Stats[label] is string)
                        Stats[label] += ";" + value;
                }
                else
                {
                    Stats[label] = value;
                }
            }
            else
                Stats.Add(label, value);
		    return value;
	    }
	
	    public object SetStat(string label1, string label2, object value, bool append = false)
        {
		    if (Stats.ContainsKey(label1) == false)
			    Stats.Add(label1, new Dictionary<string, object>());
            if (((Dictionary<string, object>) Stats[label1]).ContainsKey(label2))
            {
                if (append)
                {
                    if (((Dictionary<string, object>) Stats[label1])[label2] is Evaluator.Range &&
                        value is Evaluator.Range)
                        ((Dictionary<string, object>) Stats[label1])[label2] =
                            (Evaluator.Range) ((Dictionary<string, object>) Stats[label1])[label2] +
                            (Evaluator.Range) value;
                    if (((Dictionary<string, object>) Stats[label1])[label2] is Evaluator.Range && value is int)
                        ((Dictionary<string, object>) Stats[label1])[label2] =
                            (Evaluator.Range) ((Dictionary<string, object>) Stats[label1])[label2] + (int) value;
                    if (((Dictionary<string, object>) Stats[label1])[label2] is Evaluator.Range && value is double)
                        ((Dictionary<string, object>) Stats[label1])[label2] =
                            (Evaluator.Range) ((Dictionary<string, object>) Stats[label1])[label2] + (double) value;
                    if (((Dictionary<string, object>) Stats[label1])[label2] is int && value is int)
                        ((Dictionary<string, object>) Stats[label1])[label2] =
                            (int) ((Dictionary<string, object>) Stats[label1])[label2] + (int) value;
                    if (((Dictionary<string, object>) Stats[label1])[label2] is double && value is double)
                        ((Dictionary<string, object>) Stats[label1])[label2] =
                            (double) ((Dictionary<string, object>) Stats[label1])[label2] + (double) value;
                    if (((Dictionary<string, object>) Stats[label1])[label2] is string)
                        ((Dictionary<string, object>) Stats[label1])[label2] += ";" + value;
                }
                else
                {
                    ((Dictionary<string, object>)Stats[label1])[label2] = value;
                }
            }
            else
		        ((Dictionary<string, object>) Stats[label1]).Add(label2, value);
		    return value;
	    }

	    public object GetStat(string label)
	    {
            if (!Stats.ContainsKey(label))
                return 0;

            var stat = Stats[label];
            //if (label.CompareTo("skill_bonus") != 0 && stat is string)
              //  stat = "(" + stat + ")";

            return stat;
	    }

        public dynamic GetStat(string label1, string label2)
        {
		    if (!Stats.ContainsKey(label1)) return 0;
            if (!(Stats[label1] is Dictionary<string, object>)) return 0; // todo handle GetStat("evasion", 1);
            if (!((Dictionary<string, object>)Stats[label1]).ContainsKey(label2)) return 0;

            var stat = ((Dictionary<string, object>)Stats[label1])[label2];

            return stat;
        }

        public int GetStatCount(string label)
        {
            return !Stats.ContainsKey(label) ? 0 : ((Dictionary<string, object>) Stats[label]).Count;
        }

        public string[] GetStatParam(string label)
        {
            if (!Stats.ContainsKey(label)) return null;// string.Empty;
            string[] keys = new string[6];
            ((Dictionary<string, object>)Stats[label]).Keys.CopyTo(keys, 0);
            return keys.Where(s => s != null).ToArray();
            //return keys[0];
        }

        public object GetStatValue(string label)
        {
            if (!Stats.ContainsKey(label)) return string.Empty;
            string[] keys = new string[6];
            ((Dictionary<string, object>)Stats[label]).Keys.CopyTo(keys, 0);
            return ((Dictionary<string, object>)Stats[label]).Values.ToArray();
            //return ((Dictionary<string, object>)Stats[label])[keys[0]];
        }
    }
}
