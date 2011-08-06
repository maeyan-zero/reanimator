
namespace MediaWiki.Parser.Operator
{
    partial class Plus : IOperator<int, int>
    {
        int IOperator<int, int>.Priority
        {
            get { return 0; }
        }

        public int Evaluate(params int[] val)
        {
            return val[0] + val[1];
        }
    }

    partial class Plus : IOperator<double, double>
    {
        public double Evaluate(params double[] val)
        {
            return val[0] + val[1];
        }

        int IOperator<double, double>.Priority
        {
            get { return 0; }
        }
    }
}
