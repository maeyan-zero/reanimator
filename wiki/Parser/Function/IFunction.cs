
namespace MediaWiki.Parser.Function
{
    interface IFunction<out T>
    {
        T Execute(params object[] param);
    }
}
