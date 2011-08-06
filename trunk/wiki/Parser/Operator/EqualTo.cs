
namespace MediaWiki.Parser.Operator
{
    partial class EqualTo : IOperator<int, bool>
    {
        int IOperator<int, bool>.Priority
        {
            get { return 0; }
        }

        public bool Evaluate(params int[] val)
        {
            return val[0] == val[1];
        }
    }

    partial class EqualTo : IOperator<double, bool>
    {
        int IOperator<double, bool>.Priority
        {
            get { return 0; }
        }

        public bool Evaluate(params double[] val)
        {
            return Equals(val[0], val[1]);
        }
    }

    partial class EqualTo : IOperator<bool, bool>
    {
        int IOperator<bool, bool>.Priority
        {
            get { return 0; }
        }

        public bool Evaluate(params bool[] val)
        {
            return val[0] == val[1];
        }
    }
}
