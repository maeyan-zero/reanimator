using System.Collections.Generic;

namespace MediaWiki.Parser.Class
{
    public class Unit
    {
        protected Dictionary<string, object> Stats = new Dictionary<string, object>();

	    public object SetStat(string label, object value)
        {
            if (Stats.ContainsKey(label))
                Stats[label] = value;
            else
		        Stats.Add(label, value);
		    return value;
	    }
	
	    public object SetStat(string label1, string label2, object value)
        {
		    if (Stats.ContainsKey(label1) == false)
			    Stats.Add(label1, new Dictionary<string, object>());
            if (((Dictionary<string, object>) Stats[label1]).ContainsKey(label2))
                ((Dictionary<string, object>) Stats[label1])[label2] = value;
            else
		        ((Dictionary<string, object>) Stats[label1]).Add(label2, value);
		    return value;
	    }

	    public object GetStat(string label)
	    {
	        return !Stats.ContainsKey(label) ? 0 : Stats[label];
	    }

        public object GetStat(string label1, string label2)
        {
		    if (!Stats.ContainsKey(label1)) return 0;
            return !((Dictionary<string, object>) Stats[label1]).ContainsKey(label2) ? 0 : ((Dictionary<string, object>) Stats[label1])[label2];
        }

        public int GetStatCount(string label)
        {
            return !Stats.ContainsKey(label) ? 0 : ((Dictionary<string, object>) Stats[label]).Count;
        }

        public string GetStatParam(string label)
        {
            if (!Stats.ContainsKey(label)) return string.Empty;
            string[] keys = new string[6];
            ((Dictionary<string, object>)Stats[label]).Keys.CopyTo(keys, 0);
            return keys[0];
        }

        public object GetStatValue(string label)
        {
            if (!Stats.ContainsKey(label)) return string.Empty;
            string[] keys = new string[6];
            ((Dictionary<string, object>)Stats[label]).Keys.CopyTo(keys, 0);
            return ((Dictionary<string, object>)Stats[label])[keys[0]];
        }
    }
}
