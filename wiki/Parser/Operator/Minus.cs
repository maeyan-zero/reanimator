using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaWiki.Parser.Operator
{
    partial class Minus : IOperator<int, int>
    {
        int IOperator<int, int>.Priority
        {
            get { return 0; }
        }

        public int Evaluate(params int[] val)
        {
            return val[0] - val[1];
        }
    }

    partial class Minus : IOperator<double, double>
    {
        public double Evaluate(params double[] val)
        {
            return val[0] - val[1];
        }

        int IOperator<double, double>.Priority
        {
            get { return 0; }
        }
    }
}
