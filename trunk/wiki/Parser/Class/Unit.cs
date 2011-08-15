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
	        return !Stats.ContainsKey(label) ? null : Stats[label];
	    }

        public object GetStat(string label1, string label2)
        {
		    if (!Stats.ContainsKey(label1)) return null;
            return !((Dictionary<object, object>) Stats[label1]).ContainsKey(label2) ? null : ((Dictionary<object, object>) Stats[label1])[label2];
        }
    }
}
