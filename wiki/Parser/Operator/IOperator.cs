
namespace MediaWiki.Parser.Operator
{
    public interface IOperator<in TI, out TO>
    {
        int Priority { get; }
	    TO Evaluate(params TI[] val);
    }
}
