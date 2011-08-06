using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MediaWiki.Parser.Function;
using MediaWiki.Parser.Variable;
using MediaWiki.Parser.Operator;

namespace MediaWiki.Parser
{
    internal class Evaluator
    {
        private readonly Dictionary<String, object> _references =
            new Dictionary<String, object>();
        private readonly Dictionary<String, IFunction<object>> _functions =
            new Dictionary<String, IFunction<object>>();
        private readonly Dictionary<String, IVariable> _variables =
            new Dictionary<String, IVariable>();
        private readonly Dictionary<String, IOperator<object, object>> _operators =
            new Dictionary<String, IOperator<object, object>>();
        private readonly Dictionary<String, Dictionary<String, IFunction<object>>> _functionPointers =
            new Dictionary<String, Dictionary<String, IFunction<object>>>();
        private readonly Dictionary<Int32, List<Statement>> _cache =
            new Dictionary<Int32, List<Statement>>();

        Evaluator()
        {
            _operators.Add("+", (IOperator<object, object>) new Plus());
            _operators.Add("-", (IOperator<object, object>) new Minus());
            _operators.Add("*", (IOperator<object, object>) new Multiply());
            _operators.Add("/", (IOperator<object, object>) new Divide());
            _operators.Add(">", (IOperator<object, object>) new GreaterThan());
            _operators.Add("<", (IOperator<object, object>) new LessThan());
            _operators.Add("&&", (IOperator<object, object>) new And());
            _operators.Add("==", (IOperator<object, object>) new EqualTo());
        }

        public List<object> Evaluate(String expression)
        {
            if (expression.Equals("")) return null;

            List<Statement> statement;
            var hashcode = expression.GetHashCode();

            if (_cache.ContainsKey(hashcode))
            {
                statement = _cache[hashcode];
            }
            else
            {
                var tokens = Tokenizer(expression);
                statement = Statementizer(tokens);
                _cache.Add(hashcode, statement);
            }

            return ExecuteStatements(statement);
        }

        /// <summary>
        /// Resolves any variable types in a List of tokens. Variables must be resolved if the statement is to be executed without error.
        /// </summary>
        /// <param name="tokens"></param>
        private void ResolveVariables(IList<Token> tokens)
        {
            for (var i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].Mark != Token.Variable) continue;

                var token = tokens[i];
                if (_variables.ContainsKey(token.Symbol.ToString())) continue;

                var val = _variables[token.ToString()].GetValue();
                token = new Token(val, Token.Number);
                tokens.RemoveAt(i);
                tokens.Insert(i, token);
            }
        }

        /// <summary>
        /// Takes an expression and turns it into a Vector of tokens.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static List<Token> Tokenizer(String text)
        {
            var expression = text.ToCharArray();
            const char NEW = ' ';

            var len = expression.Length;
            var tokens = new List<Token>();
            var buffer = new List<char>();
            var mark = NEW;

            for (var i = 0; i < len; i++)
            {
                if (mark == NEW)
                {
                    mark = Token.GetType(expression[i]);
                    switch (mark)
                    {
                        case Token.Variable:
                        case Token.String:
                        case Token.Reference:
                            continue;
                        case Token.Space:
                        case Token.Newline:
                        case Token.Tab:
                            mark = NEW;
                            continue;
                        case Token.End:
                        case Token.OpenSmooth:
                        case Token.CloseSmooth:
                        case Token.OpenCurly:
                        case Token.CloseCurly:
                        case Token.Comma:
                        case Token.TernaryTrue:
                        case Token.TernaryFalse:
                            tokens.Add(new Token(expression[i], mark));
                            mark = NEW;
                            continue;
                    }
                }

                switch (mark)
                {
                    case Token.Number:
                        var cur = Token.GetType(expression[i]);

                        if (cur == Token.Number)
                        {
                            buffer.Add(expression[i]);
                        }
                        else
                        {
                            var num = CharListToString(buffer);
                            var val = Double.Parse(num);
                            tokens.Add(new Token(val, mark));
                            buffer.Clear();

                            if (cur == Token.Comma || cur == Token.CloseSmooth || cur == Token.End)
                                tokens.Add(new Token(expression[i], cur));

                            mark = NEW;
                        }

                        continue;
                    case Token.Variable:
                        cur = Token.GetType(expression[i]);

                        if (cur == Token.Function || cur == Token.Number)
                            // a function is just a alpha character, can contain nums
                        {
                            buffer.Add(expression[i]);
                        }
                        else
                        {
                            var val = CharListToString(buffer);
                            tokens.Add(new Token(val, mark));
                            buffer.Clear();

                            switch (cur)
                            {
                                case Token.Comma:
                                case Token.CloseSmooth:
                                case Token.CloseCurly:
                                case Token.End:
                                    tokens.Add(new Token(expression[i], cur));
                                    break;
                                case Token.Operator:
                                    buffer.Add(expression[i]);
                                    mark = Token.Operator;
                                    continue;
                            }

                            mark = NEW;
                        }

                        continue;
                    case Token.Function:
                        cur = Token.GetType(expression[i]);
                        if (cur == Token.Newline) continue; // ignore new lines
                        if (cur == Token.Tab) continue; // ignore tabs
                        if (cur == Token.OpenSmooth || cur == Token.OpenCurly || cur == Token.Space)
                        {
                            var val = CharListToString(buffer);
                            buffer.Clear();

                            // if its a control path, mark it
                            if (val.Equals("if"))
                            {
                                mark = Token.If;
                            }
                            else if (val.Equals("else"))
                            {
                                mark = Token.Else;
                            }

                            tokens.Add(new Token(val, mark));

                            if (cur == Token.OpenSmooth || cur == Token.OpenCurly)
                                tokens.Add(new Token(expression[i], cur)); // Add the parenthesis

                            mark = NEW;
                        }
                        else
                        {
                            buffer.Add(expression[i]);
                        }

                        continue;
                    case Token.Reference:
                        cur = Token.GetType(expression[i]);

                        if (cur != Token.Function && cur != Token.Number)
                        {
                            // Add the reference
                            var val = CharListToString(buffer);
                            tokens.Add(new Token(val, mark));
                            buffer.Clear();

                            // if its not a space, its either a comma or a close parenthesis
                            if (cur != Token.Space) tokens.Add(new Token(expression[i], cur));

                            mark = NEW;
                        }
                        else
                        {
                            buffer.Add(expression[i]);
                        }

                        continue;
                    case Token.String:
                        cur = Token.GetType(expression[i]);

                        if (cur == Token.String)
                        {
                            var val = CharListToString(buffer);
                            tokens.Add(new Token(val, mark));
                            buffer.Clear();

                            mark = NEW;
                        }
                        else
                        {
                            buffer.Add(expression[i]);
                        }

                        continue;
                    case Token.Operator:
                        cur = Token.GetType(expression[i]);

                        switch (cur)
                        {
                            case Token.Operator:
                                buffer.Add(expression[i]);
                                break;
                            case Token.Number:
                                mark = Token.Number;
                                buffer.Add(expression[i]);
                                break;
                            default:
                                var val = CharListToString(buffer);
                                if (val.Equals("->"))
                                {
                                    mark = Token.Accessor;
                                    tokens.Add(new Token(val, mark));

                                    buffer.Clear();
                                    buffer.Add(expression[i]);
                                    mark = Token.Function;
                                }
                                else
                                {
                                    tokens.Add(new Token(val, mark));
                                    buffer.Clear();
                                    if (cur == Token.OpenSmooth) tokens.Add(new Token(expression[i], cur));
                                    mark = NEW;
                                }
                                break;
                        }

                        continue;
                }
            }

            // Some statements dont end with ';' so need to check the buffer is actually empty
            if (buffer.Count != 0)
            {
                var val = CharListToString(buffer);
                Object obj;
                switch (mark)
                {
                    case Token.Number:
                        obj = Double.Parse(val);
                        break;
                    default:
                        obj = val;
                        break;
                }
                tokens.Add(new Token(obj, mark));
            }

            return tokens;
        }

        /// <summary>
        /// Takes a vector of tokens and organises them into logical statements.
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        private static List<Statement> Statementizer(IList<Token> tokens)
        {
            var statements = new List<Statement>();
            var buffer = new List<Token>();
            var stack = new Stack<Token>();

            // Reverse the tokens for left to right purposes with stack
            for (var i = tokens.Count - 1; i >= 0; i--) stack.Push(tokens[i]);

            do
            {
                var context = stack.Pop();

                if (context.Mark == Token.TernaryTrue)
                {
                    statements.Add(StatementizerTernary(stack, buffer));
                    buffer = new List<Token>();
                    continue;
                }

                if (context.Mark == Token.If)
                {
                    statements.Add(StatementizerIfElse(stack));
                    continue;
                }

                if (context.Mark != Token.End)
                {
                    buffer.Add(context);
                }
                else
                {
                    var statement = new Statement(buffer);
                    statements.Add(statement);
                    buffer = new List<Token>();
                }
            } while (stack.Count > 0);

            return statements;
        }

        /// <summary>
        /// When parsing Ternary statements, you will not know ifs ternary until you reach the '?' operator,
        /// therefor pass the tokens you have already removed as the control
        /// You must have already popped the '?' operator, important.
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="controlTokens"></param>
        /// <returns></returns>
        private static ControlPath StatementizerTernary(Stack<Token> tokens, List<Token> controlTokens)
        {
            // Control Paths
            var control = controlTokens;
            var truePath = new List<Statement>();
            var falsePath = new List<Statement>();
            var buffer = new List<Token>();

            do
            {
                var context = tokens.Pop();
                if (context.Mark == Token.TernaryTrue) // Its another Ternary statement - unlikely
                {
                    StatementizerTernary(tokens, buffer);
                    buffer = new List<Token>();
                    continue;
                }

                if (context.Mark == Token.TernaryFalse) // Now we have the true path
                {
                    truePath.Add(new Statement(buffer));
                    buffer = new List<Token>();
                    continue;
                }

                if (context.Mark != Token.End) // The buffer is still being appended too
                {
                    buffer.Add(context);
                }
                else // Its the end of the Ternary statement
                {
                    falsePath.Add(new Statement(buffer));
                    buffer = new List<Token>();
                    break;
                }
            } while (tokens.Count > 0);

            // DEBUG - A ternary statement has been found in the Denounce skill
            // that doesnt end with a semi-colon. Therefor the false path is not being
            // appended. This is done below for the meantime
            if (buffer.Count != 0)
            {
                falsePath.Add(new Statement(buffer));
            }

            return new ControlPath(control, truePath, falsePath);
        }

        /// <summary>
        /// Creates a control path based on If/Else statements. The If statement should already have been popped.
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        private static ControlPath StatementizerIfElse(Stack<Token> tokens)
        {
            // Control Paths
            List<Token> control;
            var truePath = new List<Statement>();
            var falsePath = new List<Statement>();
            var buffer = new List<Token>();

            // Read the Control Statement
            var openControl = 0;
            do
            {
                var context = tokens.Pop();
                if (context.Mark == Token.OpenSmooth) openControl++;

                buffer.Add(context);
                if (context.Mark == Token.CloseSmooth) openControl--;

                if (openControl != 0) continue;

                control = buffer;
                buffer = new List<Token>();
                break;
            } while (true);

            // Read the True statement/s
            var isStatementBlock = false;
            do
            {
                var context = tokens.Pop();
                if (context.Mark == Token.OpenCurly)
                {
                    isStatementBlock = true;
                    continue;
                }

                if (context.Mark == Token.CloseCurly) break;

                if (context.Mark != Token.End)
                {
                    buffer.Add(context);
                }
                else
                {
                    truePath.Add(new Statement(buffer));
                    buffer = new List<Token>();

                    if (isStatementBlock == false) break;
                }
            } while (true);

            // There is not always a false path.
            if (tokens.Count == 0 || tokens.Peek().Mark != Token.Else)
            {
                return new ControlPath(control, truePath, falsePath);
            }

            tokens.Pop();

            // Read the False statement/s
            isStatementBlock = false;
            do
            {
                var context = tokens.Pop();
                if (context.Mark == Token.OpenCurly)
                {
                    isStatementBlock = true;
                    continue;
                }

                if (context.Mark == Token.CloseCurly) break;

                if (context.Mark != Token.End)
                {
                    buffer.Add(context);
                }
                else
                {
                    falsePath.Add(new Statement(buffer));
                    buffer = new List<Token>();

                    if (isStatementBlock == false) break;
                }
            } while (true);

            return new ControlPath(control, truePath, falsePath);
        }

        /// <summary>
        /// Takes a vector of tokens and evaluates it, returning the result.
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        private object ExecuteTokens(List<Token> tokens)
        {
            ResolveVariables(tokens);

            var parenthesisExists = true;
            var accessorExists = true;
            do
            {
                int open;
                int closed;

                // First check for the first closing parenthesis, if any
                if (parenthesisExists)
                {
                    closed = GetPosFirstClosedParenthesis(tokens);
                    open = GetPosOpenParenthesis(tokens, closed);
                    if (open == 0 && closed == 0)
                    {
                        parenthesisExists = false;
                        continue;
                    }
                }
                else
                {
                    closed = tokens.Count;
                    open = 0;
                }

                // See if any operators exist this range
                var range = new Range(open, closed);
                var opos = GetPosOperator(tokens, range);

                // If one does, perform operand method
                if (opos >= 1)
                {
                    var op = tokens[opos].ToString();

                    if (_operators.ContainsKey(op) == false)
                        throw new Exception("Unimplemented Operator: " + op);

                    // Check that negation is applied
                    if (op.Equals("-") && tokens[opos - 1].Mark != Token.Number)
                    {
                        var num = (Double) tokens[opos + 1].Symbol;
                        num = num - (num*2); // inverse
                        tokens.RemoveAt(opos);
                        tokens.RemoveAt(opos);
                        tokens.Insert(opos, new Token(num, Token.Number));
                        RemoveParentheses(tokens, opos);
                        continue;
                    }

                    // its a 2 parameter method
                    var val1 = (Double) tokens[opos - 1].Symbol;
                    var val2 = (Double) tokens[opos + 1].Symbol;
                    var opresult = _operators[op].Evaluate(val1, val2);
                    var ptoken = new Token(opresult, Token.Number);
                    opos--; // move cursor back to the first val
                    tokens.RemoveAt(opos);
                    tokens.RemoveAt(opos);
                    tokens.RemoveAt(opos);
                    tokens.Insert(opos, ptoken);
                    RemoveParentheses(tokens, opos);
                    continue;
                }

                if (accessorExists)
                {
                    var accessorPos = GetAccessorPosition(tokens);
                    if (accessorPos == -1)
                    {
                        accessorExists = false;
                        continue;
                    }

                    var var = (String) tokens[accessorPos - 1].Symbol;
                    var afunc = (String) tokens[accessorPos + 1].Symbol;

                    // Determine number of args
                    var noargs = GetFuncArgCount(tokens, accessorPos + 2);
                    var curs = accessorPos + 3;
                    var args = new Object[noargs];
                    for (var i = 0; i < noargs; i++)
                    {
                        var t = tokens[curs];
                        if (t.Mark == Token.Reference)
                        {
                            if (_references.ContainsKey(tokens[curs].ToString()) == false)
                                throw new Exception("Reference not Found: " + t.ToString());

                            args[i] = _references[tokens[curs].ToString()];
                        }
                        else
                        {
                            args[i] = tokens[curs].Symbol;
                        }

                        curs += 2; // skip current arg and comma
                    }

                    if (!_functionPointers.ContainsKey(var) || !_functionPointers[var].ContainsKey(afunc))
                        throw new Exception("Function Pointer not found: " + var + "->" + afunc);

                    var fresult = _functionPointers[var][afunc].Execute(args);
                    var ftoken = new Token(fresult, Token.Number);

                    var argrem = (noargs > 1) ? ((noargs * 2) - 1) : noargs;
                    var last = accessorPos + 1 + 1 + argrem + 1; // accessor + func + open + args + close
                    accessorPos--;
                    for (var i = accessorPos; i <= last; i++) tokens.RemoveAt(accessorPos);
                    tokens.Insert(accessorPos, ftoken);
                    continue;
                }

                // Its a function, evaluate it
                // Should possibly always be a function at this point
                if (!parenthesisExists) continue;

                // Determine number of args
                var fargs = GetFuncArgCount(tokens, open - 1);
                var fcurs = open + 1;
                var dargs = new Object[fargs];
                for (int i = 0; i < fargs; i++)
                {
                    var t = tokens[fcurs];
                    if (t.Mark == Token.Reference)
                    {
                        dargs[i] = _references[tokens[fcurs].ToString()];
                    }
                    else
                    {
                        dargs[i] = tokens[fcurs].Symbol;
                    }

                    fcurs += 2; // skip current arg and comma
                }

                // If there is only parenthesis and no function call, just remove redundant brackets
                if (open - 1 == - 1 || tokens[open - 1].Mark != Token.Function)
                {
                    tokens.RemoveAt(open);
                    tokens.RemoveAt(closed - 1);
                    continue;
                }

                // Make function call
                var func = (String) tokens[open - 1].Symbol;

                if (!_functions.ContainsKey(func)) throw new Exception("Function not found: " + func);

                object furesult = _functions[func].Execute(dargs);
                var token = new Token(furesult, Token.Number);

                // Remove function, replace with result
                var pos = open - 1;
                for (var i = pos; i <= closed; i++) tokens.RemoveAt(pos);
                tokens.Insert(pos, token);

                continue;
            }
            while (tokens.Count != 1); // Should be able to deduce each expression to 1 result

            var result = tokens[0].Symbol;
            return result;
        }

        /// <summary>
        /// Executes a collection of statements. This is the top level method.
        /// </summary>
        /// <param name="statements"></param>
        /// <returns></returns>
        private List<object> ExecuteStatements(IEnumerable<Statement> statements)
        {
            var result = new List<object>();

            try
            {
                foreach (var statement in statements)
                {
                    if (statement.GetType() == typeof(ControlPath))
                    {
                        var condition = (bool) ExecuteTokens(statement.Tokens);
                        var path = condition ? ((ControlPath) statement).TruePath : ((ControlPath) statement).FalsePath;
                        result.AddRange(ExecuteStatements(path));
                    }
                    else
                    {
                        result.Add(ExecuteTokens(new List<Token>(statement.Tokens)));
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }

            return result;
        }

        private static int GetAccessorPosition(IEnumerable<Token> tokens)
        {
            var i = 0;

            foreach (var token in tokens)
            {
                if (token.Mark == Token.Accessor) return i;
                i++;
            }

            return -1;
        }

        private static int GetFuncArgCount(IList<Token> tokens, int func)
        {
            var count = 0;

            // Check if the function has no arguments
            if (tokens[func + 1].Mark == Token.OpenSmooth && tokens[func + 2].Mark == Token.CloseSmooth)
                return count;

            // Count the number of arguments by counting comma's
            count++; // add one because of the algorithm used
            func += 2; // known position of first argument

            while (true)
            {
                if (tokens[func].Mark == Token.Comma)
                {
                    count++;
                    func++;
                    continue;
                }

                if (tokens[func].Mark == Token.CloseSmooth) break;
                func++;
            }

            return count;
        }

        private static String CharListToString(IEnumerable<char> list)
        {
            return list.Aggregate("", (current, t) => current + t);
        }

        private int GetPosOperator(IList<Token> tokens, Range r)
        {
            var priority = -1;
            var opPosition = -1;

            for (var i = r.Start; i < r.End; i++)
            {
                if (tokens[i].Mark != Token.Operator) continue;

                if (_operators[tokens[i].ToString()].Priority <= priority) continue;

                opPosition = i;
                priority = _operators[tokens[i].ToString()].Priority;

                continue;
            }

            return opPosition;
        }

        private static void RemoveParentheses(IList<Token> tokens, int pos)
        {
            if (pos > 1 && tokens[pos - 2].Mark != 'F'
                && tokens[pos - 1].Mark == '('
                && tokens[pos + 1].Mark == ')' || pos == 1
                && tokens[0].Mark == '('
                && tokens[2].Mark == ')')
            {
                tokens.RemoveAt(pos + 1);
                tokens.RemoveAt(pos - 1);
            }
            return;
        }

        private static int GetPosOpenParenthesis(IList<Token> tokens, int closedParenthesis)
        {
            var i = closedParenthesis - 2;
            while (i >= 0)
            {
                var t = tokens[i];
                if (t.Mark == '(') return i;
                i--;
            }
            return 0;
        }

        private static int GetPosFirstClosedParenthesis(IList<Token> tokens)
        {
            for (var i = 0; i < tokens.Count; i++)
            {
                var t = tokens[i];
                if (t.Mark == ')') return i;
            }

            return 0;
        }

        public class Range
        {
            public int Start { get; private set; }
            public int End { get; private set; }

            public Range(int start, int end)
            {
                Start = start;
                End = end;
            }
        }

        ///
        /// A collection of Tokens forming one executable statement.
        /// 
        public class Statement
        {
            public List<Token> Tokens { get; private set; }

            public Statement(List<Token> tokens)
            {
                Tokens = tokens;
            }
        }

        ///
        /// The result of the control parameter when executed determines if the truePath or falsePath is executed.
        /// 
        public class ControlPath : Statement
        {
            public List<Statement> TruePath { get; private set; }
            public List<Statement> FalsePath { get; private set; }

            public ControlPath(List<Token> control, List<Statement> truePath, List<Statement> falsePath) : base(control)
            {
                TruePath = truePath;
                FalsePath = falsePath;
            }
        }
    }
}
