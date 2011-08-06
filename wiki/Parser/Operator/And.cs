
namespace MediaWiki.Parser.Operator
{
    partial class And : IOperator<int, bool>
    {
        int IOperator<int, bool>.Priority
        {
            get { return 0; }
        }

        public bool Evaluate(params int[] val)
        {
            return val[0] > 0 && val[1] > 0;
        }
    }

    partial class And : IOperator<double, bool>
    {
        public bool Evaluate(params double[] val)
        {
            return val[0] > 0 && val[1] > 0;
        }

        int IOperator<double, bool>.Priority
        {
            get { return 0; }
        }
    }

    partial class And : IOperator<bool, bool>
    {
        public bool Evaluate(params bool[] val)
        {
            return val[0] && val[1];
        }

        int IOperator<bool, bool>.Priority
        {
            get { return 0; }
        }
    }
}
