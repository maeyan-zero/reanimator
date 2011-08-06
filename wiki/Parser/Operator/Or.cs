
namespace MediaWiki.Parser.Operator
{
    partial class Or : IOperator<int, bool>
    {
        int IOperator<int, bool>.Priority
        {
            get { return 0; }
        }

        public bool Evaluate(params int[] val)
        {
            return val[0] > 0 || val[1] > 0;
        }
    }

    partial class Or : IOperator<double, bool>
    {
        public bool Evaluate(params double[] val)
        {
            return val[0] > 0 || val[1] > 0;
        }

        int IOperator<double, bool>.Priority
        {
            get { return 0; }
        }
    }

    partial class Or : IOperator<bool, bool>
    {
        public bool Evaluate(params bool[] val)
        {
            return val[0] || val[1];
        }

        int IOperator<bool, bool>.Priority
        {
            get { return 0; }
        }
    }
}
