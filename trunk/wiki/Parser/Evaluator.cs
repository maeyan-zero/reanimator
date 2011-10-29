using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Hellgate;
using MediaWiki.Parser.Class;

namespace MediaWiki.Parser
{
    public class Evaluator
    {
        public Unit Unit { get; set; }
        public Game3 Game3 { get; set; }
        public Game4 Game4 { get; set; }
        public Context Context { get; set; }
        public FileManager Manager { get; set; }
        public StatsList StatsList { get; set; }

        private readonly Dictionary<Int32, List<Statement>> _cache = new Dictionary<Int32, List<Statement>>();

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
            //for (var i = 0; i < tokens.Count; i++)
            //{
            //    if (tokens[i].Mark != Token.Variable) continue;

            //    var token = tokens[i];
            //    if (_variables.ContainsKey(token.Symbol.ToString())) continue;

            //    var val = _variables[token.ToString()].GetValue();
            //    token = new Token(val, Token.Number);
            //    tokens.RemoveAt(i);
            //    tokens.Insert(i, token);
            //}
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
                                    if (cur == Token.Function)
                                    {
                                        mark = Token.Function;
                                        buffer.Add(expression[i]);
                                    }
                                    else if (cur == Token.Variable)
                                    {
                                        mark = Token.Variable;
                                    }
                                    else
                                    {
                                        mark = NEW;
                                    }
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
        private object ExecuteTokens(IList<Token> tokens)
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
                Token token;
                if (opos > -1)
                {
                    var op = tokens[opos].ToString();

                    // Check that negation is applied
                    if (op.Equals("-") && (opos == 0 || tokens[opos - 1].Mark != Token.Number))
                    {
                        if (tokens[opos + 1].Mark == Token.Range)
                        {
                            range = (Range) tokens[opos + 1].Symbol;
                            range = new Range(-range.Start, -range.End);
                            tokens.RemoveAt(opos);
                            tokens.RemoveAt(opos);
                            tokens.Insert(opos, new Token(range, Token.Range));
                            RemoveParentheses(tokens, opos);
                            continue;
                        }

                        var num = (Double) tokens[opos + 1].Symbol;
                        num = -num; // inverse
                        tokens.RemoveAt(opos);
                        tokens.RemoveAt(opos);
                        tokens.Insert(opos, new Token(num, Token.Number));
                        RemoveParentheses(tokens, opos);
                        continue;
                    }

                    // Check for logical not
                    if (op.Equals("!"))
                    {
                        if (tokens[opos + 1].Mark == Token.Boolean)
                        {
                            var num = (bool)tokens[opos + 1].Symbol;
                            num = !num;
                            token = new Token(num, Token.Boolean);
                        }
                        else
                        {
                            var num = Double.Parse(tokens[opos + 1].Symbol.ToString());
                            num = num + -(num * 2);
                            token = new Token(num, Token.Number);
                        }
                        
                        tokens.RemoveAt(opos);
                        tokens.RemoveAt(opos);
                        tokens.Insert(opos, token);
                        RemoveParentheses(tokens, opos);
                        continue;
                    }

                    // if both sides are not numbers, then concat into a formulae
                    if (((tokens[opos - 1].Mark == Token.Formula) || (tokens[opos + 1].Mark == Token.Formula)) && !IsLogicalOperator(tokens[opos].Symbol))
                    {
                        var f1 = tokens[opos - 1].ToString();
                        var f2 = tokens[opos].ToString();
                        var f3 = tokens[opos + 1].ToString();
                        tokens.RemoveAt(opos - 1);
                        tokens.RemoveAt(opos - 1);
                        tokens.RemoveAt(opos - 1);
                        var formula = f1 + " " + f2 + " " + f3;
                        if (f1 == "0") formula = f3;
                        if (f3 == "0") formula = f1;
                        tokens.Insert(opos - 1, new Token(formula, Token.Formula));
                        continue;
                    }

                    // its a 2 parameter method
                    var val1 = tokens[opos - 1].Symbol;
                    var val2 = tokens[opos + 1].Symbol;
                    var ptoken = ApplyOperator(op, val1, val2);
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
                        args[i] = tokens[curs].Symbol;

                        curs += 2; // skip current arg and comma
                    }

                    var ftoken = ApplyPointerMethod(var, afunc, args);

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
                        dargs[i] = GetReference(tokens[fcurs].ToString());
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

                token = ApplyFunction(func, dargs);

                // Remove function, replace with result
                var pos = open - 1;
                for (var i = pos; i <= closed; i++) tokens.RemoveAt(pos);
                tokens.Insert(pos, token);

                continue;
            }
            while (!tokens.All(t => (t.Mark == Token.Number || t.Mark == Token.Range || t.Mark == Token.Formula || t.Mark == Token.Boolean))); // Should be able to deduce each expression to 1 result

            var result = tokens[0].Symbol;
            return result;
        }

        private object GetReference(string reference)
        {
            switch (reference)
            {
                case "context":
                    return Context;
                case "game3":
                    return Game3;
                case "game4":
                    return Game4;
                case "unit":
                    return Unit;
                case "statslist":
                    return StatsList;
                default:
                    throw new Exception("Unknown reference: " + reference);
            }
        }

        private static Token ApplyOperator(string symbol, params object[] param)
        {
            // convert params to bool if need be
            if (param[0] is bool && !(param[1] is bool) || !(param[0] is bool) && param[1] is bool)
            {
                if (!(param[0] is bool)) param[0] = param[0].ToString() != "0";
                else param[1] = Double.Parse(param[1].ToString()) > 0;
            }
            else if ((param[0] is string || param[1] is string))
            {
                if (param[0] is string) param[0] = 1;
                if (param[1] is string) param[1] = 1;
            }
            else if ((param[0] is Range || param[1] is Range) && IsLogicalOperator(symbol))
            {
                if (param[0] is Range) param[0] = 1;
                if (param[1] is Range) param[1] = 1;
            }

            bool isBoolean = (param[0] is bool && param[1] is bool);

            object result;
            if (isBoolean)
            {
                var boo1 = (bool)param[0];
                var boo2 = (bool)param[1];

               switch (symbol)
                {
                    case "&&":
                        result = boo1 && boo2;
                        return new Token(result, Token.Boolean);
                    case "||":
                        result = boo1 || boo2;
                        return new Token(result, Token.Boolean);
                    case ">":
                    case "<":
                        return new Token(boo1, Token.Boolean);
                    default:
                        throw new Exception("Invalid operator: " + symbol);
                }
            }

            if (param[0] is Range || param[1] is Range)
            {
                Range range = (param[0] is Range) ? (Range)param[0] : (Range)param[1];
                Double value = (param[0] is Range) ? Double.Parse(param[1].ToString()) : Double.Parse(param[0].ToString());

                switch (symbol)
                {
                    case "+":
                        result = range + value;
                        return new Token(result, Token.Range);
                    case "-":
                        result = range - value;
                        return new Token(result, Token.Range);
                    case "*":
                        result = range * value;
                        return new Token(result, Token.Range);
                    case "/":
                        result = range / value;
                        return new Token(result, Token.Range);
                    default:
                        throw new Exception("Invalid range operator: " + symbol);
                }
            }

            var isDouble = (param[0].ToString().Contains(".") || param[1].ToString().Contains("."));
            var val1 = (isDouble) ? Double.Parse(param[0].ToString()) : Int32.Parse(param[0].ToString());
            var val2 = (isDouble) ? Double.Parse(param[1].ToString()) : Int32.Parse(param[1].ToString());

            switch (symbol)
            {
                case "+":
                    result = val1 + val2;
                    return new Token(result, Token.Number);
                case "-":
                    result = val1 - val2;
                    return new Token(result, Token.Number);
                case "*":
                    result = val1 * val2;
                    return new Token(result, Token.Number);
                case "/":
                    result = val1 / val2;
                    return new Token(result, Token.Number);
                case ">":
                    result = val1 > val2;
                    return new Token(result, Token.Boolean);
                case "<":
                    result = val1 < val2;
                    return new Token(result, Token.Boolean);
                case "==":
                    result = Equals(val1, val2);
                    return new Token(result, Token.Boolean);
                case "!=":
                    result = !Equals(val1, val2);
                    return new Token(result, Token.Boolean);
                case "&&":
                    result = val1 > 0 && val2 > 0;
                    return new Token(result, Token.Boolean);
                case "||":
                    result = val1 > 0 || val2 > 0;
                    return new Token(result, Token.Boolean);
                default:
                    throw new Exception("Invalid operator: " + symbol);
            }
        }

        private Token ApplyPointerMethod(string var, string method, params object[] param)
        {
            switch (var)
            {
                case "unit":
                    switch (method)
                    {
                        case "GetStat666":
                        case "GetStat667":
                            object result;
                            switch (param.Length)
                            {
                                case 1:
                                    result = Unit.GetStat(param[0].ToString());
                                    return StatToToken(param[0].ToString(), result);
                                case 2:
                                    result = Unit.GetStat(param[0].ToString(), param[1].ToString());
                                    return new Token(result, Token.Number);
                                default:
                                    throw new Exception("Illegal number of parameters for Unit->GetStat666");
                            }
                        default:
                            throw new Exception("Unknown method for unit: " + method);
                    }
                default:
                    throw new Exception("unknown pointer: " + var);
            }
        }

        private Token ApplyFunction(string func, params object[] param)
        {
            object result;
            switch (func)
            {
                case "meetsclassreqs":
                    return new Token(true, Token.Boolean);
                case "hasdomname":
                case "is_bindable":
                case "nodrop":
                case "notrade":
                case "is_operatable":
                case "is_usable":
                    return new Token(false, Token.Boolean);

                case "feed_stamina_pct":
                case "affix_feed_stamina_pct":
                    return SetFeed("stamina_feed", param);

                case "feed_willpower_pct":
                case "affix_feed_willpower_pct":
                    return SetFeed("willpower_feed", param);

                case "feed_strength_pct":
                case "affix_feed_strength_pct":
                    return SetFeed("strength_feed", param);

                case "feed_accuracy_pct":
                case "affix_feed_accuracy_pct":
                    return SetFeed("accuracy_feed", param);

                case "dmg_fire":
                case "dmg_spec":
                case "dmg_phys":
                case "dmg_toxic":
                case "dmg_elec":
                    //if it's a monster...assume it's % increase?
                    if (Unit is Monster)                    
                        return SetMonsterDamageBonus(func, param);
                    
                    if ((int)Unit.GetStat("isBeam")==1)
                        return SetBeamDamage(func, param);
                    else if ((int)Unit.GetStat("isHive") == 1)
                        return SetEelDamage(func, param);
                    else
                        return SetDirectDamage(func, param);

                case "rad_fire":
                case "rad_spec":
                case "rad_phys":
                    return SetSplashDamage(func, param);

                case "rad_toxic":
                    if ((int)Unit.GetStat("isHive") == 1)
                        return SetSwarmDamage(func, param);
                    else
                        return SetSplashDamage(func, param);

                case "rad_elec":
                    //if ((int)Unit.GetStat("isHive") == 1)
                        //return SetEelDamage(func, param);
                    //else
                        return SetSplashDamage(func, param);

                case "field_fire":
                case "field_spec":
                case "field_phys":
                case "field_toxic":
                case "field_elec":
                    return SetFieldDamage(func, param);

                case "dot_fire":
                case "dot_spec":
                case "dot_phys":
                case "dot_toxic":
                case "dot_elec":
                    return SetDotDamage(func, param);

                case "ai_change_stop":
                case "ai_change_convert":
                case "ai_change_convert_field":
                case "ai_change_fear":                
                    return new Token(0, Token.Number); // todo
                case "rad_toxic_hive":
                    return SetSwarmDamage(func, param);

                case "basetotal":
                    result = Unit.GetStat(param[1].ToString());
                    return new Token(result, result is Range ? Token.Range : Token.Number);

                case "total":
                    var total = Unit.GetStatCount(param[1].ToString());
                    return new Token(total, Token.Number);

                case "isa":
                    var isa = (param[0].ToString() == param[1].ToString());
                    return new Token(isa, Token.Boolean);

                case "meetsitemreqs":
                    // we arn't going this far, just return true.
                    return new Token(true, Token.Boolean);

                case "use_state_duration":
                    // we arn't going this far, just return 0.
                    return new Token(0, Token.Number);

                case "getStatsOnStateSet":
                    // we arn't going this far, just return false.
                    return new Token(0, Token.Number);

                case "has_use_skill":
                    //todo
                    return new Token(false, Token.Boolean);

                case "buyprice":
                case "sellprice":
                case "getItemStateDuration":
                    return new Token(0, Token.Number);

                case "pct":
                    double result1;
                    double result2;
                    if (!Double.TryParse(param[0].ToString(), out result1) || !Double.TryParse(param[1].ToString(), out result2))
                        return new Token(param[0], Token.Formula);

                    return new Token(result1 * (result2 / 100), Token.Number);

                case "thorns_dmg_toxic_monster":
                    if (param.Length != 1)
                        throw new Exception("Illegal number of parameters for thorns_dmg_phys_monster function.");
                    Unit.SetStat("damage_thorns_min", "toxic", param[0]);
                    return new Token(param[0], Token.Number);

                case "thorns_dmg_elec_monster":
                    if (param.Length != 1)
                        throw new Exception("Illegal number of parameters for thorns_dmg_phys_monster function.");
                    Unit.SetStat("damage_thorns_min", "electricity", param[0]);
                    return new Token(param[0], Token.Number);

                case "thorns_dmg_spec_monster":
                    if (param.Length != 1)
                        throw new Exception("Illegal number of parameters for thorns_dmg_phys_monster function.");
                    Unit.SetStat("damage_thorns_min", "spectral", param[0]);
                    return new Token(param[0], Token.Number);

                case "thorns_dmg_fire_monster":
                    if (param.Length != 1)
                        throw new Exception("Illegal number of parameters for thorns_dmg_phys_monster function.");
                    Unit.SetStat("damage_thorns_min", "fire", param[0]);
                    return new Token(param[0], Token.Number);

                case "thorns_dmg_phys_monster":
                    //Unit.SetStat("thorns_dmg_phys_monster", param[0]);
                    if (param.Length != 1)
                        throw new Exception("Illegal number of parameters for thorns_dmg_phys_monster function.");
                    Unit.SetStat("damage_thorns_min", "physical", param[0]);
                    return new Token(param[0], Token.Number);

                case "monster_level_sfx_attack":
                    if (param.Length != 1)
                        throw new Exception("Illegal number of parameters for monster_level_sfx_attack function.");
                    //Unit.SetStat("monster_level_sfx_attack", param[0]);
                    if (!(param[0] is int)) return new Token("sfx_attack", Token.Formula);

                    int mlevel = (int)param[0];

                    // item level 0 means no level was specified, return a formulae part
                    if (mlevel == 0) return new Token("sfx_attack", Token.Formula);                    

                    // otherwise we can retrieve it.
                    var monsterLevels = Manager.GetDataTable("MONLEVEL");
                    var monsterSfxAttack = (int)monsterLevels.Rows[mlevel]["sfxAttack"];

                    return new Token(monsterSfxAttack, Token.Number);

                case "monster_level_sfx_defense":
                    if (param.Length != 1)
                        throw new Exception("Illegal number of parameters for monster_level_sfx_defense function.");

                    if (!(param[0] is int)) return new Token("sfx_defense", Token.Formula);

                    mlevel = (int)param[0];

                    // item level 0 means no level was specified, return a formulae part
                    if (mlevel == 0) return new Token("sfx_defense", Token.Formula);

                    monsterLevels = Manager.GetDataTable("MONLEVEL");
                    var monsterSfxDefense = (int)monsterLevels.Rows[mlevel]["sfxDefense"];

                    return new Token(monsterSfxDefense, Token.Number);

                case "monster_level_shields":
                    if (param.Length != 1) throw new Exception("Illegal number of parameters for monster_level_shields function.");

                    Unit.SetStat("shields_bonus", param[0]);
                    if ((param[0] is int)) return new Token(param[0], Token.Number);
                    return new Token(param[0].ToString().Replace("ilevel", "mlevel_shield"), Token.Formula);

                case "pickskill":
                    if (param.Length != 3) throw new Exception("Illegal number of parameters for pickskill function.");

                    Unit.SetStat("skill_bonus", "RANDOM SKILL " + param[2]); //I think that's how it works?
                    return new Token("RANDOM SKILL", Token.Number);

                case "pickskillbyunittype":
                    if (param.Length != 4)  throw new Exception("Illegal number of parameters for pickskillbyunittype function.");

                    Unit.SetStat("skill_bonus", "RANDOM " + param[2].ToString().ToUpper() + " SKILL " + param[3]); //maybe?
                    return new Token("RANDOM " + param[2].ToString().ToUpper() + " SKILL", Token.Number);

                case "pickskillbyskillgroup":
                    if (param.Length != 4) throw new Exception("Illegal number of parameters for pickskillbyskillgroup function.");

                    Unit.SetStat("skill_bonus", "RANDOM " + param[2].ToString().ToUpper() + " SKILL " + param[3]); //maybe?
                    return new Token("RANDOM " + param[2] + " SKILL", Token.Number);

                case "thorns_dmg_toxic_item":
                    if (param.Length != 1)
                        throw new Exception("Illegal number of parameters for thorns_dmg_toxic_item function.");

                    Unit.SetStat("damage_thorns_min", "toxic", param[0]);
                    return new Token(param[0], Token.Number);

                case "thorns_dmg_spec_item":
                    if (param.Length != 1)
                        throw new Exception("Illegal number of parameters for thorns_dmg_spec_item function.");

                    Unit.SetStat("damage_thorns_min", "physical", param[0]);
                    return new Token(param[0], Token.Number);

                case "thorns_dmg_elec_item":
                    if (param.Length != 1)
                        throw new Exception("Illegal number of parameters for thorns_dmg_elec_item function.");

                    Unit.SetStat("damage_thorns_min", "electricity", param[0]);
                    return new Token(param[0], Token.Number);

                case "thorns_dmg_fire_item":
                    if (param.Length != 1)
                        throw new Exception("Illegal number of parameters for thorns_dmg_fire_item function.");

                    Unit.SetStat("damage_thorns_min", "fire", param[0]);
                    return new Token(param[0], Token.Number);

                case "thorns_dmg_phys_item":
                    if (param.Length != 1)
                        throw new Exception("Illegal number of parameters for thorns_dmg_phys_item function.");

                    Unit.SetStat("damage_thorns_min", "physical", param[0]);
                    return new Token(param[0], Token.Number);

                case "set_shield_bonus":
                    if (param.Length != 2)
                        throw new Exception("Illegal number of parameters for set_shield_bonus function.");

                    Unit.SetStat("shields_bonus", param[0]);
                    if ((param[0] is int)) return new Token(param[0], Token.Number);
                    return new Token(param[0], Token.Formula);

                case "set_armor_bonus":
                    if (param.Length != 2)
                        throw new Exception("Illegal number of parameters for set_armor_bonus function.");

                    Unit.SetStat("armor_bonus", "all", param[0]);
                    if ((param[0] is int)) return new Token(param[0], Token.Number);
                    return new Token(param[0], Token.Formula);

                case "item_level_sfx_defense":
                    if (param.Length != 1)
                        throw new Exception("Illegal number of parameters for item_level_sfx_defense function.");

                    if (!(param[0] is int)) return new Token("sfx_defense", Token.Formula);

                    var itemLevels = Manager.GetDataTable("ITEM_LEVELS");
                    var level = (int) param[0];
                    var sfxDefence = (int) itemLevels.Rows[level]["sfxDefenceAbility"];
                    return new Token(sfxDefence, Token.Number);

                case "pow_regen_per_min":
                    if (param.Length != 1)
                        throw new Exception("Illegal number of parameters for pow_regen_per_min function.");

                    if (param[0] is Range)
                    {
                        Unit.SetStat("power_regen", (Range) param[0] * 4.26);
                        return new Token(param[0], Token.Range);
                    }

                    if (param[0] is String)
                    {
                        Unit.SetStat("power_regen", param[0]);
                        return new Token(param[0], Token.Formula);
                    }

                    Unit.SetStat("power_regen", Convert.ToInt32((double) param[0] * 4.26));
                    return new Token(param[0], Token.Number);

                case "hp_regen_per_min":
                    if (param.Length != 1)
                        throw new Exception("Illegal number of parameters for hp_regen_per_min function.");

                    if (param[0] is Range)
                    {
                        Unit.SetStat("hp_regen", (Range) param[0] * 4.26);
                        return new Token(param[0], Token.Range);
                    }

                    if (param[0] is String)
                    {
                        Unit.SetStat("hp_regen", param[0]);
                        return new Token(param[0], Token.Formula);
                    }

                    Unit.SetStat("hp_regen", Convert.ToInt32((double) param[0] * 4.26));
                    return new Token(param[0], Token.Number);

                case "all_stats_bonus":
                    if (param.Length != 1)
                        throw new Exception("Illegal number of parameters for all_stats_bonus function.");
                    var bonus = param[0];
                    Unit.SetStat("strength_bonus", bonus);
                    Unit.SetStat("accuracy_bonus", bonus);
                    Unit.SetStat("willpower_bonus", bonus);
                    Unit.SetStat("stamina_bonus", bonus);
                    return new Token(bonus, Token.Number);

                case "item_level_sfx_attack":
                    if (param.Length != 1)
                        throw new Exception("Illegal number of parameters for item_level_sfx_attack function.");

                    if (!(param[0] is int)) return new Token("sfx_attack", Token.Formula);

                    level = (int)param[0];

                    // item level 0 means no level was specified, return a formulae part
                    if (level == 0) return new Token("sfx_attack", Token.Formula);

                    // otherwise we can retrieve it.
                    itemLevels = Manager.GetDataTable("ITEM_LEVELS");
                    var sfxAttack = (int) itemLevels.Rows[level]["sfxAttackAbility"];
                    return new Token(sfxAttack, Token.Number);

                case "rand":
                    return new Token(new Range(Int32.Parse(param[1].ToString()), Int32.Parse(param[2].ToString())), Token.Range);
                
                case "GetStat666":
                case "GetStat667":
                case "GetStat680":
                    switch (param.Length)
                    {
                        case 1:
                            result = Unit.GetStat(param[0].ToString());
                            return StatToToken(param[0].ToString(), result);
                        case 2:
                            result = Unit.GetStat(param[0].ToString(), param[1].ToString());
                            return StatToToken(param[0].ToString(), result);
                        default:
                            throw new Exception("Illegal number of parameters for GetStat666");
                    }

                case "SetStat669":
                case "SetStat673":
                    switch (param.Length)
                    {
                        case 2:
                            result = Unit.SetStat(param[0].ToString(), param[1]);
                            return StatToToken(param[0].ToString(), result);
                        case 3:
                            result = Unit.SetStat(param[0].ToString(), param[1].ToString(), param[2]);
                            return StatToToken(param[0].ToString(), result);
                        default:
                            throw new Exception("Illegal number of parameters for GetStat666");
                    }

                default:
                    throw new Exception("Unknown function: " + func);
            }
        }

        private Token SetMonsterDamageBonus(string dmgType, object[] objects)
        {
            //a bit redundant, but it works and it's only "direct" damage assumed to be an additional percentage
            string element = string.Empty;
            switch (dmgType)
            {
                case "dmg_fire":
                    element = "fire";
                    break;
                case "dmg_spec":
                    element = "spectral";
                    break;
                case "dmg_phys":
                    element = "physical";
                    break;
                case "dmg_toxic":
                    element = "toxic";
                    break;
                case "dmg_elec":
                    element = "electricity";
                    break;
                default:
                    throw new Exception("Invalid element: " + dmgType);
            }
            Unit.SetStat("damage_augmentation", element, objects[0]);
            return new Token(dmgType + ": " + objects[0].ToString(), Token.Formula);
        }

        //this isn't being updated because it's probably never used
        private Token SetDotDamage(string dmgType, object[] objects)
        {
            var damageMin = (int)Unit.GetStat("damage_min");
            var damageMax = (int)Unit.GetStat("damage_max");
            //var dotInc = (int)Unit.GetStat("dot_increment");
            var pct = (double)objects[0];
            var secs = (double)objects[2] / 20;
            var ilevel = (int)Unit.GetStat("level");
            var ilevels = Manager.GetDataTable("ITEM_LEVELS");
            var dmgMulti = (int)ilevels.Rows[ilevel]["baseDamageMultiplyer"];
            var absoluteMin = damageMin * ((dmgMulti * pct) / 10000);
            var absoluteMax = damageMax * ((dmgMulti * pct) / 10000);
            absoluteMin = Round(absoluteMin);
            absoluteMax = Round(absoluteMax);
            Unit.SetStat(dmgType, "min", absoluteMin);
            Unit.SetStat(dmgType, "max", absoluteMax);
            Unit.SetStat(dmgType, "secs", secs);
            return new Token(dmgType + ": " + absoluteMin + "/" + absoluteMax, Token.Formula);
        }

        private Token SetFieldDamage(string dmgType, object[] objects)
        {
            var damageMin = (int)Unit.GetStat("damage_min");
            var damageMax = (int)Unit.GetStat("damage_max");
            var fieldInc = (int)Unit.GetStat("field_increment");
            //get affix bonuses (will cause damage rangeception, eg [9-10]-[13-14])
            var pct = (double)objects[0];
            Range splash = GetSplashRadiusPercent() * ((objects.Length < 3) ? 1 : (double)objects[2] / 100);
            var range = splash.Start == splash.End ? (splash.Start / 10.0).ToString() : "[" + splash.Start / 10.0 + " - " + splash.End / 10.0 + "]"; ;//GetSplashRadiusPercent() * ((objects.Length < 3) ? 1 : (double)objects[2] / 10) / 100.0;
            var time = (double) objects[3]/20;
            var ilevel = (int)Unit.GetStat("level");
            var ilevels = Manager.GetDataTable("ITEM_LEVELS");
            var dmgMulti = (int)ilevels.Rows[ilevel]["baseDamageMultiplyer"];
            Range dmgPercent = GetDamagePercent(dmgType);
            var absoluteMin = dmgPercent * (damageMin * ((dmgMulti * fieldInc * pct * 4) / 1000000)) / 100;
            var absoluteMax = dmgPercent * Math.Max(damageMax * ((dmgMulti * fieldInc * pct * 4) / 1000000), 1) / 100;
            //absoluteMin = Round(absoluteMin);
            //absoluteMax = Round(absoluteMax);
            Unit.SetStat(dmgType, "min", absoluteMin);
            Unit.SetStat(dmgType, "max", absoluteMax);
            Unit.SetStat(dmgType, "range", range);
            Unit.SetStat(dmgType, "secs", time);
            return new Token(dmgType + ": " + absoluteMin + "/" + absoluteMax, Token.Formula);
        }

        private Token SetDirectDamage(string dmgType, object[] objects)
        {
            var damageMin = (int) Unit.GetStat("damage_min");
            var damageMax = (int) Unit.GetStat("damage_max");
            var directInc = (int) Unit.GetStat("dmg_increment");
            //get affix bonuses (will cause damage rangeception, eg [9-10]-[13-14])
            var pct = (double) objects[0];
            var ilevel = (int) Unit.GetStat("level");
            var ilevels = Manager.GetDataTable("ITEM_LEVELS");
            var dmgMulti = (int) ilevels.Rows[ilevel]["baseDamageMultiplyer"];
            Range dmgPercent = GetDamagePercent(dmgType);
            var absoluteMin = dmgPercent * (damageMin * ((dmgMulti * directInc * pct) / 1000000)) / 100;
            var absoluteMax = dmgPercent * Math.Max(damageMax * ((dmgMulti * directInc * pct) / 1000000), 1) / 100;

            //absoluteMin = Round(absoluteMin);
            //absoluteMax = Round(absoluteMax);
            Unit.SetStat(dmgType, "min", absoluteMin);
            Unit.SetStat(dmgType, "max", absoluteMax);
            return new Token(dmgType + ": " + absoluteMin + "/" + absoluteMax, Token.Formula);
        }

        private Token SetSplashDamage(string dmgType, object[] objects)
        {
            var damageMin = (int)Unit.GetStat("damage_min");
            var damageMax = (int)Unit.GetStat("damage_max");
            var directInc = (int)Unit.GetStat("dmg_increment");
            var radialInc = (int)Unit.GetStat("radial_increment");
            //get affix bonuses (will cause damage rangeception, eg [9-10]-[13-14])
            //example: absoluteMax=(((((Range)Unit.GetStat("damage_percent","physical"))+100)*absoluteMax)/100).ToString()
            //percent bonus damage is 100 + GetStat666('damage_percent', 'all') + GetStat666('damage_percent', 'toxic') + GetStat666('damage_augmentation', 'all') + GetStat666('damage_augmentation', 'toxic'))
            //splash damage is splash*(1+bonus/100)
            var pct = (double)objects[0];
            //do splash damage manually since Ranges are ints
            Range splash = GetSplashRadiusPercent() * ((objects.Length < 3) ? 1 : (double)objects[2] / 100);
            var range = splash.Start == splash.End ? (splash.Start / 10.0).ToString() : "[" + splash.Start / 10.0 + " - " + splash.End / 10.0 + "]"; ;//GetSplashRadiusPercent() * ((objects.Length < 3) ? 1 : (double)objects[2] / 10) / 100.0;
            var ilevel = (int)Unit.GetStat("level");
            var ilevels = Manager.GetDataTable("ITEM_LEVELS");
            var dmgMulti = (int)ilevels.Rows[ilevel]["baseDamageMultiplyer"];
            Range dmgPercent = GetDamagePercent(dmgType);
            var absoluteMin = dmgPercent * (damageMin * ((dmgMulti * directInc * radialInc * pct) / 100000000)) / 100;
            var absoluteMax = dmgPercent * Math.Max(damageMax * ((dmgMulti * directInc * radialInc * pct) / 100000000), 1) / 100;
            //absoluteMin = Round(absoluteMin);
            //absoluteMax = Round(absoluteMax);1
            Unit.SetStat(dmgType, "min", absoluteMin);
            Unit.SetStat(dmgType, "max", absoluteMax);
            Unit.SetStat(dmgType, "range", range);
            return new Token(dmgType + ": " + absoluteMin + "/" + absoluteMax, Token.Formula);
        }

        private Token SetBeamDamage(string dmgType, object[] objects)
        {
            var damageMin = (int)Unit.GetStat("damage_min");
            var damageMax = (int)Unit.GetStat("damage_max");
            //var directInc = (int)Unit.GetStat("dmg_increment");
            //get affix bonuses (will cause damage rangeception, eg [9-10]-[13-14])
            var pct = (double)objects[0];
            var ilevel = (int)Unit.GetStat("level");
            var ilevels = Manager.GetDataTable("ITEM_LEVELS");
            var dmgMulti = (int)ilevels.Rows[ilevel]["baseDamageMultiplyer"];
            Range dmgPercent = GetDamagePercent(dmgType);
            var absoluteMin = dmgPercent * (damageMin * ((dmgMulti * pct) / 10000.0)) / 100;
            var absoluteMax = dmgPercent * Math.Max(damageMax * ((dmgMulti * pct) / 10000.0), 1) / 100;
            //absoluteMin = Round(absoluteMin);
            //absoluteMax = Round(absoluteMax);
            Unit.SetStat(dmgType, "min", absoluteMin);
            Unit.SetStat(dmgType, "max", absoluteMax);
            return new Token(dmgType + ": " + absoluteMin + "/" + absoluteMax, Token.Formula);
        }

        private Token SetEelDamage(string dmgType, object[] objects)
        {
            var damageMin = (int)Unit.GetStat("damage_min");
            var damageMax = (int)Unit.GetStat("damage_max");
            var directInc = (int)Unit.GetStat("dmg_increment");
            var radialInc = (int)Unit.GetStat("radial_increment");
            //get affix bonuses (will cause damage rangeception, eg [9-10]-[13-14])
            var pct = (double)objects[0];
            var range = (objects.Length < 3) ? 1 : (double)objects[2] / 10;
            var ilevel = (int)Unit.GetStat("level");
            var ilevels = Manager.GetDataTable("ITEM_LEVELS");
            var dmgMulti = (int)ilevels.Rows[ilevel]["baseDamageMultiplyer"];
            Range dmgPercent = GetDamagePercent(dmgType);
            var absoluteMin = dmgPercent * (damageMin * ((dmgMulti * directInc * radialInc * pct * 10) / 100000000)) / 100;
            var absoluteMax = dmgPercent * Math.Max(damageMax * ((dmgMulti * directInc * radialInc * pct * 10) / 100000000), 1) / 100;
            //absoluteMin = Round(absoluteMin);
            //absoluteMax = Round(absoluteMax);
            Unit.SetStat(dmgType, "min", absoluteMin);
            Unit.SetStat(dmgType, "max", absoluteMax);
            Unit.SetStat(dmgType, "range", range);
            return new Token(dmgType + ": " + absoluteMin + "/" + absoluteMax, Token.Formula);
        }

        //really the same as regular splash, but might as well make the distinction
        private Token SetSwarmDamage(string dmgType, object[] objects)
        {
            return SetSplashDamage(dmgType, objects);
            //if anything changes, we'll just customize this stuff
            /*
            var damageMin = (int)Unit.GetStat("damage_min");
            var damageMax = (int)Unit.GetStat("damage_max");
            var directInc = (int)Unit.GetStat("dmg_increment");
            var radialInc = (int)Unit.GetStat("radial_increment");
            //get affix bonuses (will cause damage rangeception, eg [9-10]-[13-14])
            var pct = (double)objects[0];
            Range splash = GetSplashRadiusPercent() * ((objects.Length < 3) ? 1 : (double)objects[2] / 10);
            var range = splash.Start == splash.End ? (splash.Start / 100.0).ToString() : "[" + splash.Start / 100.0 + " - " + splash.End / 100.0 + "]"; ;//GetSplashRadiusPercent() * ((objects.Length < 3) ? 1 : (double)objects[2] / 10) / 100.0;
            var ilevel = (int)Unit.GetStat("level");
            var ilevels = Manager.GetDataTable("ITEM_LEVELS");
            var dmgMulti = (int)ilevels.Rows[ilevel]["baseDamageMultiplyer"];
            Range dmgPercent = GetDamagePercent(dmgType);
            var absoluteMin = dmgPercent * (damageMin * ((dmgMulti * directInc * radialInc * pct) / 100000000)) / 100;
            var absoluteMax = dmgPercent * Math.Max(damageMax * ((dmgMulti * directInc * radialInc * pct) / 100000000), 1) / 100;
            //absoluteMin = Round(absoluteMin);
            //absoluteMax = Round(absoluteMax);
            Unit.SetStat(dmgType, "min", absoluteMin);
            Unit.SetStat(dmgType, "max", absoluteMax);
            Unit.SetStat(dmgType, "range", range);
            return new Token(dmgType + ": " + absoluteMin + "/" + absoluteMax, Token.Formula);
            */
        }

        private Range GetSplashRadiusPercent()
        {
            return new Range(100, 100) + Unit.GetStat("damage_radius_pct", "all");
        }

        private Range GetDamagePercent(string dmgType)
        {
            //start at 100 and add "increases" and "adds" percentages
            Range percentRange = new Range(100, 100) + Unit.GetStat("damage_percent", "all") + Unit.GetStat("damage_augmentation", "all");
            percentRange += Unit.GetStat("damage_augmentation", "all");

            //determine what the element is and get the increase/add
            string elementType = string.Empty;
            switch (dmgType.Split('_')[1])
            {
                case "fire":
                    elementType = "fire";
                    break;
                case "spec":
                    elementType = "spectral";
                    break;
                case "phys":
                    elementType = "physical";
                    break;
                case "toxic":
                    elementType = "toxic";
                    break;
                case "elec":
                    elementType = "electricity";
                    break;
                default:
                    throw new Exception("'" + dmgType + "' isn't an element");
            }

            percentRange += Unit.GetStat("damage_percent", elementType) + Unit.GetStat("damage_augmentation", elementType);

            return percentRange;
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

        private static int GetPosOperator(IList<Token> tokens, Range r)
        {
            var priority = 15;
            var opPosition = -1;

            for (var i = r.Start; i < r.End; i++)
            {
                if (tokens[i].Mark != Token.Operator) continue;
                //if (tokens[i - 1].Mark != Token.Number || tokens[i + 1].Mark != Token.Number)
                //{
                //    if (opPosition == -1) opPosition = -2;
                //    continue;
                //}
                switch (tokens[i].ToString())
                {
                    case "*":
                    case "/":
                    case "%":
                        if (3 >= priority) continue;
                        priority = 3;
                        break;
                    case "+":
                    case "-":
                    case "!":
                        if (4 >= priority) continue;
                        priority = 4;
                        break;
                    case ">":
                    case "<":
                    case ">=":
                    case "<=":
                        if (6 >= priority) continue;
                        priority = 6;
                        break;
                    case "==":
                    case "!=":
                        if (7 >= priority) continue;
                        priority = 7;
                        break;
                    case "&&":
                        if (13 >= priority) continue;
                        priority = 13;
                        break;
                    case "||":
                        if (14 >= priority) continue;
                        priority = 14;
                        break;
                    default:
                        throw new Exception("Unhandled priority for operator " + tokens[i]);
                }

                opPosition = i;
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

            public override string ToString()
            {
                if (Start == End) return Start.ToString();
                if (Start < 0 && End < 0)
                    return "-[" + (-Start) + " - " + (-End) + "]";

                return "[" + Start + " - " + End + "]";
            }

            public static Range operator +(Range c1, int c2)
            {
                return new Range(c1.Start + c2, c1.End + c2);
            }

            public static Range operator -(Range c1, int c2)
            {
                return new Range(c1.Start - c2, c1.End - c2);
            }

            public static Range operator *(Range c1, int c2)
            {
                return new Range(c1.Start * c2, c1.End * c2);
            }

            public static Range operator /(Range c1, int c2)
            {
                return new Range(c1.Start / c2, c1.End / c2);
            }

            public static Range operator +(int c2, Range c1)
            {
                return new Range(c1.Start + c2, c1.End + c2);
            }

            public static Range operator -(int c2, Range c1)
            {
                return new Range(c1.Start - c2, c1.End - c2);
            }

            public static Range operator *(int c2, Range c1)
            {
                return new Range(c1.Start * c2, c1.End * c2);
            }

            public static Range operator /(int c2, Range c1)
            {
                return new Range(c1.Start / c2, c1.End / c2);
            }


            public static Range operator +(Range c1, double c2)
            {
                return new Range(Convert.ToInt32(c1.Start + c2), Convert.ToInt32(c1.End + c2));
            }

            public static Range operator -(Range c1, double c2)
            {
                return new Range(Convert.ToInt32(c1.Start - c2), Convert.ToInt32(c1.End - c2));
            }

            public static Range operator *(Range c1, double c2)
            {
                return new Range(Convert.ToInt32(c1.Start * c2), Convert.ToInt32(c1.End * c2));
            }

            public static Range operator /(Range c1, double c2)
            {
                return new Range(Convert.ToInt32(c1.Start / c2), Convert.ToInt32(c1.End / c2));
            }

            public static Range operator +(double c2, Range c1)
            {
                return new Range(Convert.ToInt32(c1.Start + c2), Convert.ToInt32(c1.End + c2));
            }

            public static Range operator -(double c2, Range c1)
            {
                return new Range(Convert.ToInt32(c1.Start - c2), Convert.ToInt32(c1.End - c2));
            }

            public static Range operator *(double c2, Range c1)
            {
                return new Range(Convert.ToInt32(c1.Start * c2), Convert.ToInt32(c1.End * c2));
            }

            public static Range operator /(double c2, Range c1)
            {
                return new Range(Convert.ToInt32(c1.Start / c2), Convert.ToInt32(c1.End / c2));
            }


            public static Range operator +(Range c1, Range c2)
            {
                return new Range(c1.Start + c2.Start, c1.End + c2.End);
            }

            public static Range operator -(Range c1, Range c2)
            {
                return new Range(c1.Start - c2.Start, c1.End - c2.End);
            }

            public static Range operator *(Range c1, Range c2)
            {
                return new Range(c1.Start * c2.Start, c1.End * c2.End);
            }

            public static Range operator /(Range c1, Range c2)
            {
                return new Range(c1.Start / c2.Start, c1.End / c2.End);
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

        private Token SetFeed(string label, params object[] param)
        {
            if (Unit.GetStat("level") is Range)
            {
                var levels = Manager.GetDataTable("ITEM_LEVELS");
                var ilevelmin = ((Range)Unit.GetStat("level")).Start;
                var ilevelmax = ((Range)Unit.GetStat("level")).End;
                var ifeedmin = (int)levels.Rows[ilevelmin]["feed"];
                var ifeedmax = (int)levels.Rows[ilevelmax]["feed"];
                ifeedmin = Convert.ToInt32(Round(Convert.ToDouble(ifeedmin) * (((double)param[0]) / 100)));
                ifeedmax = Convert.ToInt32(Round(Convert.ToDouble(ifeedmax) * (((double)param[0]) / 100)));
                var range = new Range(ifeedmin, ifeedmax);
                Unit.SetStat(label, range, true);
                return new Token(range, Token.Range);
            }

            var ilevel = Convert.ToInt32(Unit.GetStat("level"));
            if (ilevel == 0)
            {
                Unit.SetStat(label, "ilevel_feed * " + param[0] + "%", true);
                return new Token("ilevel * " + param[0] + "%", Token.Formula);
            }

            var ilevels = Manager.GetDataTable("ITEM_LEVELS");
            var feed = (int)ilevels.Rows[ilevel]["feed"];
            var ifeed = Round(feed * (((double)param[0]) / 100));
            Unit.SetStat(label, ifeed, true);
            return new Token(feed, Token.Number);
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

        private Token StatToToken(string param, object stat)
        {
            if (stat.ToString() == "0" && param == "level") return new Token("ilevel", Token.Formula); // hack
            if (stat is Range) return new Token(stat, Token.Range);
            int num;
            if (Int32.TryParse(stat.ToString(), out num)) return new Token(stat, Token.Number);
            return new Token(stat, Token.Formula);
        }

        private static bool IsLogicalOperator(object op)
        {
            string sop = op.ToString();
            switch (sop)
            {
                case "<":
                case ">":
                case "!=":
                case "==":
                case "<=":
                case ">=":
                case "&&":
                case "||":
                    return true;
                default:
                    return false;
            }
        }

        private bool IsInteger(object input)
        {
            int num;
            return Int32.TryParse(input.ToString(), out num);
        }

        private double Round(double number)
        {
            return Math.Floor(number);
        }
    }
}
