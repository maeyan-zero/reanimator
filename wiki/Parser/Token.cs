
namespace MediaWiki.Parser
{
    public class Token
    {
        public const char Number = 'D';
        public const char Variable = 'V';
        public const char Operator = 'P';
        public const char String = 'S';
        public const char OpenSmooth = '(';
        public const char CloseSmooth = ')';
        public const char OpenCurly = '{';
        public const char CloseCurly = '}';
        public const char Comma = 'Z';
        public const char Function = 'F';
        public const char Space = ' ';
        public const char End = ';';
        public const char Reference = '@';
        public const char Newline = '\n';
        public const char Accessor = 'A';
        public const char Tab = '\t';
        public const char TernaryTrue = '?';
        public const char TernaryFalse = ':';
        public const char If = 'I';
        public const char Else = 'E';
        public const char Range = 'R';
        public const char Formula = 'f';

        public object Symbol { get; private set; }
        public char Mark { get; private set; }

        public Token(object token, char mark)
        {
            Symbol = token;
            Mark = mark;
        }

        public static char GetType(char c)
        {
            if (c >= 48 && c <= 57) return Number;

            switch (c)
            {
                case '+':
                case '-':
                case '*':
                case '/':
                case '<':
                case '>':
                case '&':
                case '=':
                    return Operator;
                case '$':
                    return Variable;
                case '\'':
                    return String;
                case '(':
                    return OpenSmooth;
                case ')':
                    return CloseSmooth;
                case '{':
                    return OpenCurly;
                case '}':
                    return CloseCurly;
                case '\n':
                    return Newline;
                case ',':
                    return Comma;
                case ' ':
                    return Space;
                case ';':
                    return End;
                case '@':
                    return Reference;
                case '\t':
                    return Tab;
                case '?':
                    return TernaryTrue;
                case ':':
                    return TernaryFalse;
                default:
                    return Function;
            }
        }

        public new string ToString()
        {
            return Symbol.ToString();
        }
    }
}
