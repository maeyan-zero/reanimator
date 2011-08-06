
namespace MediaWiki.Parser.Operator
{
    partial class GreaterThan : IOperator<int, bool>
    {
        int IOperator<int, bool>.Priority
        {
            get { return 0; }
        }

        public bool Evaluate(params int[] val)
        {
            return val[0] > val[1];
        }
    }

    partial class GreaterThan : IOperator<double, bool>
    {
        public bool Evaluate(params double[] val)
        {
            return val[0] > val[1];
        }


        int IOperator<double, bool>.Priority
        {
            get { return 0; }
        }
    }
}
