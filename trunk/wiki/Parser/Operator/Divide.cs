using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaWiki.Parser.Operator
{
    partial class Divide : IOperator<int, int>
    {
        int IOperator<int, int>.Priority
        {
            get { return 10; }
        }

        public int Evaluate(params int[] val)
        {
            return val[0] / val[1];
        }
    }

    partial class Divide : IOperator<double, double>
    {
        int IOperator<double, double>.Priority
        {
            get { return 10; }
        }

        public double Evaluate(params double[] val)
        {
            return val[0] / val[1];
        }
    }
}
