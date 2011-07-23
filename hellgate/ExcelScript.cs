using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Revival.Common;

namespace Hellgate
{
    public partial class ExcelScript
    {
        #region Members

        // debug members
        public static bool DebugEnabled { get; private set; }
        private static bool _globalDebug;
        private const String DebugRoot = @"C:\excel_script_debug\";
        private const String DebugRootTestCenter = @"C:\excel_script_debug_testcenter\";
        private const String DebugRootResurrection = @"C:\excel_script_debug_resurrection\";
        private const String DebugFormat = "Debug: Row({0}), Col({1}) = '{2}', StringId = '{3}', ScriptByteString = '{4}'";
        private readonly String _debugRoot;
        private bool _debugFormatConditionalByteCounts;
        private String _debugOutput;
        private String _debugScriptByteString;
        private String _debugStringId;
        private String _debugColName;
        private int _debugRow;
        private int _debugCol;

        // excel function stuffs
        private static int _initCallFunctionsCountSinglePlayer;
        private static int _initCallFunctionsTestCenterCount;
        private static int _initCallFunctionsResurrectionCount;
        private static bool _haveExcelFunctions;
        private ExcelFile.ExcelFunction _excelScriptFunction;
        private Function _excelFunction;

        // members for excel handling
        public readonly List<Function> CallFunctions;
        private readonly FileManager _fileManager;
        private readonly ExcelFile _statsTable;
        private readonly ObjectDelegator _statsDelegator;

        // compile/decompile members
        private readonly int _initCallFunctionsCount;
        private String _script;
        public Int32[] ScriptCode { get; private set; }
        private int _offset;
        private int _level;
        private int _startOffset;
        private Stack<StackObject> _stack;
        private List<StackObject> _vars;

        #endregion

        /// <summary>
        /// Initialise private variables and generate excel script functions if required.
        /// </summary>
        public ExcelScript(FileManager fileManager)
        {
            if (fileManager == null) throw new ArgumentNullException("fileManager", "File Manager cannot be null.");
            if (!fileManager.DataFiles.ContainsKey("STATS")) throw new Exceptions.ScriptNotInitialisedException("The supplied file manager did not have a valid Stats table.");

            // these initial counts are required for script compiling when determing if a function is from the .exe or from an Excel table
            if (!_haveExcelFunctions)
            {
                _initCallFunctionsCountSinglePlayer = CallFunctionsSinglePlayer.Count;
                _initCallFunctionsTestCenterCount = CallFunctionsTestCenter.Count;
                _initCallFunctionsResurrectionCount = CallFunctionsResurrection.Count;
            }

            _fileManager = fileManager;
            if (_fileManager.IsVersionResurrection || _fileManager.IsVersionMod)
            {
                CallFunctions = CallFunctionsResurrection;
                _initCallFunctionsCount = _initCallFunctionsResurrectionCount;
                _debugRoot = DebugRootResurrection;
            }
            else if (_fileManager.IsVersionTestCenter)
            {
                CallFunctions = CallFunctionsTestCenter;
                _initCallFunctionsCount = _initCallFunctionsTestCenterCount;
                _debugRoot = DebugRootTestCenter;
            }
            else
            {
                CallFunctions = CallFunctionsSinglePlayer;
                _initCallFunctionsCount = _initCallFunctionsCountSinglePlayer;
                _debugRoot = DebugRoot;
            }

            _statsTable = (ExcelFile)_fileManager.DataFiles["STATS"];
            _statsDelegator = _fileManager.DataFileDelegators["STATS"];
            if (_globalDebug) EnableDebug(true);

            if (CallFunctions.Count == _initCallFunctionsCount) _GenerateExcelScriptFunctions();
        }

        #region Compiler Functions

        /// <summary>
        /// Compiles a script string to its byte code form.
        /// </summary>
        /// <param name="script">The script string to compile to byte code.</param>
        /// <param name="debugScriptByteString">For debugging purposes only.</param>
        /// <param name="debugStringId">For debugging purposes only.</param>
        /// <param name="debugRow">For debugging purposes only.</param>
        /// <param name="debugCol">For debugging purposes only.</param>
        /// <param name="debugColName">For debugging purposes only.</param>
        /// <returns>The compiled script as an int array.</returns>
        public int[] Compile(String script, String debugScriptByteString = null, String debugStringId = null, int debugRow = 0, int debugCol = 0, String debugColName = null)
        {
            if (String.IsNullOrEmpty(script)) throw new ArgumentNullException("script", "Script string cannot be null or empty!");

            _vars = new List<StackObject>();
            _offset = 0;
            _script = script;
            _level = -1;

            if (DebugEnabled)
            {
                _debugScriptByteString = debugScriptByteString;
                _debugStringId = debugStringId;
                _debugRow = debugRow;
                _debugCol = debugCol;
                _debugColName = debugColName;
                _debugOutput = String.Format(DebugFormat, _debugRow, _debugCol, _debugColName, _debugStringId, _debugScriptByteString);
            }

            //if (script == "SetStat673('stamina_feed', (int)(double)GetStat666('strength_bonus'));")
            //{
            //    int bp = 0;
            //}

            return ScriptCode = _Compile('\0', null);
        }

        /// <summary>
        /// Compiles a script string to its byte code form.
        /// </summary>
        /// <param name="terminator">A terminator character to stop at.</param>
        /// <param name="argument">A Client Function Argument to be used as a comparison when applicable.</param>
        /// <param name="withinCondition">Set to True when within a condition statement.</param>
        /// <param name="byteOffset">The current byte offset of the compiled byte code.</param>
        /// <param name="overloadedFunction">Set to true if currently within an overloaded function call stack.</param>
        /// <returns>The compiled script as an int array.</returns>
        private Int32[] _Compile(char terminator, Argument argument, bool withinCondition = false, int byteOffset = 0, bool overloadedFunction = false)
        {
            _level++;
            List<Int32> scriptByteCode = new List<Int32>();
            ScriptOpCodes operatorOpCode = ScriptOpCodes.Return;
            //int bp = 0;
            String excelStr;
            Stack<StackObject> operatorStack = new Stack<StackObject>();

            //if (_debugRow == 739 && _debugCol == 51)
            //{
            //    bp = 0;
            //}

            bool endOfStatement = false;
            bool endOfCondition = false;
            Int32[] contextPtr = null;
            while (_offset < _script.Length && _script[_offset] != terminator)
            {
                _SkipWhite();

                int condOffset;
                switch (_script[_offset])
                {
                    case '(':       // opening paranthesis
                        _offset++;

                        Int32[] paranethesesCodeBytes;
                        if (withinCondition || byteOffset > 0)
                        {
                            paranethesesCodeBytes = _Compile(')', null, false, byteOffset);
                        }
                        else
                        {
                            paranethesesCodeBytes = _Compile(')', null);
                        }

                        scriptByteCode.AddRange(paranethesesCodeBytes);

                        Debug.Assert(_script[_offset] == ')');
                        _offset++;
                        break;

                    case ')':       // closing paranthesis
                        if (_level > 0 && withinCondition || overloadedFunction)
                        {
                            endOfCondition = true;
                            break;
                        }

                        throw new Exceptions.ScriptFormatException("Unexpected closing paranthesis encountered.", _offset);

                    case '@':       // global var
                        _offset++;
                        String globalName = _GetNameStr();

                        if (argument != null)
                        {
                            ArgType argType = _GetArgType(globalName);
                            if (argument.Type != argType) throw new Exceptions.ScriptInvalidArgTypeException(globalName, argument.Type.ToString().ToLower(), _offset - globalName.Length);
                        }

                        String asOpCodeName = "globalvar" + globalName;
                        ScriptOpCodes opCode = _GetScriptOpCode(asOpCodeName);
                        if (opCode == 0) throw new Exceptions.ScriptUnknownVarNameException(globalName, _offset - globalName.Length);

                        scriptByteCode.Add((Int32)opCode);
                        break;

                    case '$':       // context var
                        _offset++;
                        String contextName = _GetNameStr();

                        ContextVariables contextVar = Enum.GetValues(typeof(ContextVariables)).Cast<ContextVariables>().Where(type => type.ToString().ToLower() == contextName).FirstOrDefault();
                        if (contextVar == 0 && contextName != "unit") throw new Exceptions.ScriptUnknownVarNameException(contextName, _offset - contextName.Length);

                        ArgType constextArgType = _GetArgType(contextName);

                        bool isPtr = false;
                        ScriptOpCodes contextOpCode = ScriptOpCodes.Return;
                        if (_script[_offset] == '-' && _script[_offset + 1] == '>') // is ptr
                        {
                            isPtr = true;
                            constextArgType = ArgType.Context;
                            _offset += 2;
                        }

                        if (!isPtr && argument != null && argument.Type != constextArgType) throw new Exceptions.ScriptInvalidArgTypeException(contextName, argument.Type.ToString().ToLower(), _offset - contextName.Length);

                        switch (constextArgType)
                        {
                            case ArgType.Int32:
                                contextOpCode = ScriptOpCodes.PushContextVarInt32;
                                break;

                            case ArgType.Context:
                                contextOpCode = ScriptOpCodes.PushContextVarPtr;
                                break;

                            default:
                                //bp = 0;
                                break;
                        }
                        Debug.Assert((int)contextOpCode != 0);

                        if (isPtr)
                        {
                            contextPtr = new[] { (Int32)contextOpCode, (Int32)contextVar, (Int32)ScriptOpCodes.UsePtrObjectReference };
                        }
                        else
                        {
                            scriptByteCode.AddRange(new[] { (Int32)contextOpCode, (Int32)contextVar });
                        }
                        break;

                    case '\'':      // string
                        if (argument == null || argument.Type != ArgType.ExcelIndex) throw new Exceptions.ScriptFormatException("Unexpected string encountered.", _offset);

                        excelStr = _GetString();
                        int rowIndex = _fileManager.GetExcelRowIndexFromTableIndex(argument.TableIndex, excelStr);
                        if (rowIndex == -1) throw new Exceptions.UnknownExcelStringException(excelStr, _offset);
                        _offset += excelStr.Length + 1;

                        scriptByteCode.AddRange(new[] { (Int32)ScriptOpCodes.Push, rowIndex });
                        break;

                    case ';':       // end of script block
                        if (argument != null) throw new Exceptions.ScriptFormatException("Unexpected ';' encountered within function arguments.", _offset);

                        _CompileOperators(operatorStack, scriptByteCode, 99);

                        endOfStatement = true;
                        _offset++;
                        break;

                    case '\n':      // end of line
                        _offset++;
                        break;

                    case ':':       // 4    0x04
                        _offset++;
                        _SkipWhite();
                        if (_script[_offset] == '[') _offset = _script.IndexOf(']', ++_offset) + 1; // is debug script skip over

                        Int32[] ternaryFalseBytes = _Compile((char)0xFF, null); // continue until ';' or ')'

                        int byteOffsetToEndFalse = (scriptByteCode.Count + ternaryFalseBytes.Length + 2) * 4 + byteOffset; // +2 for 2x codes for TernaryFalse

                        scriptByteCode.AddRange(new[] { (Int32)ScriptOpCodes.TernaryFalse, byteOffsetToEndFalse });
                        scriptByteCode.AddRange(ternaryFalseBytes);
                        break;

                    case '?':       // 14   0x0E
                        _offset++;
                        _SkipWhite();
                        if (_script[_offset] == '[') _offset = _script.IndexOf(']', ++_offset) + 1; // is debug script skip over

                        Int32[] ternaryTrueBytes = _Compile(':', null);

                        int byteOffsetToEndTrue = (scriptByteCode.Count + ternaryTrueBytes.Length + 2) * 4 + byteOffset; // +2 for 2x codes for TernaryTrue
                        if (_script[_offset] == ':') byteOffsetToEndTrue += 8; // +8 for 2x codes for TernaryFalse

                        scriptByteCode.AddRange(new[] { (Int32)ScriptOpCodes.TernaryTrue, byteOffsetToEndTrue });
                        scriptByteCode.AddRange(ternaryTrueBytes);
                        break;

                    case '^':       // 347  0x15B
                        _CompileOperators(operatorStack, scriptByteCode, 4);
                        operatorStack.Push(new StackObject { Value = "^", Precedence = 4, OpCode = ScriptOpCodes.Pow });
                        _offset++;
                        continue;

                    case '*':       // 358  0x166
                        _CompileOperators(operatorStack, scriptByteCode, 5);
                        operatorStack.Push(new StackObject { Value = "*", Precedence = 5, OpCode = ScriptOpCodes.Mult });
                        _offset++;
                        continue;

                    case '/':       // 369  0x171
                        _CompileOperators(operatorStack, scriptByteCode, 5);
                        operatorStack.Push(new StackObject { Value = "/", Precedence = 5, OpCode = ScriptOpCodes.Div });
                        _offset++;
                        continue;

                    case '+':       // 388  0x184
                        _CompileOperators(operatorStack, scriptByteCode, 6);
                        operatorStack.Push(new StackObject { Value = "+", Precedence = 6, OpCode = ScriptOpCodes.Add });
                        _offset++;
                        continue;

                    case '&':       // 516  0x204
                        if (_script[_offset + 1] != '&') throw new NotImplementedException(String.Format("Binary AND operations are not currently implemented. Offset = '{0}'", _offset));
                        _CompileOperators(operatorStack, scriptByteCode, 13);

                        if (withinCondition)
                        {
                            endOfCondition = true;
                            break;
                        }

                        _offset += 2;
                        condOffset = 0;
                        if (byteOffset == 0) condOffset = _GetByteOffset(scriptByteCode) + 8; // +2 for or op code and offset, +1 for end condition

                        Int32[] andByteCode = _Compile('\0', null, true, condOffset);

                        int andEndOffset = (scriptByteCode.Count + andByteCode.Length + 3) * 4 + byteOffset; // +2 for and op code and offset, +1 for end condition

                        scriptByteCode.AddRange(new[] { (Int32)ScriptOpCodes.And, andEndOffset });
                        scriptByteCode.AddRange(andByteCode);
                        scriptByteCode.Add((Int32)ScriptOpCodes.EndCond);
                        break;

                    case '|':       // 527  0x20F
                        if (_script[_offset + 1] != '|') throw new NotImplementedException(String.Format("Binary OR operations are not currently implemented. Offset = '{0}'", _offset));
                        _CompileOperators(operatorStack, scriptByteCode, 14);

                        if (withinCondition)
                        {
                            endOfCondition = true;
                            break;
                        }

                        _offset += 2;
                        condOffset = 0;
                        if (byteOffset == 0) condOffset = _GetByteOffset(scriptByteCode) + 8; // +2 for or op code and offset, +1 for end condition

                        Int32[] orByteCode = _Compile('\0', null, true, condOffset);

                        int orEndOffset = (scriptByteCode.Count + orByteCode.Length + 3) * 4 + byteOffset; // +2 for or op code and offset, +1 for end condition
                        scriptByteCode.AddRange(new[] { (Int32)ScriptOpCodes.Or, orEndOffset });
                        scriptByteCode.AddRange(orByteCode);
                        scriptByteCode.Add((Int32)ScriptOpCodes.EndCond);
                        break;

                    case '[':
                        _offset = _script.IndexOf(']', ++_offset) + 1; // is debug script skip over
                        break;

                    case '-':
                        if (scriptByteCode.Count == 0 || operatorStack.Count > 0)
                        {
                            // is is a negative number
                            if (_IsNumber())
                            {
                                int scriptNum = _GetNumber();

                                scriptByteCode.AddRange(new[] { (Int32)ScriptOpCodes.Push, scriptNum });
                                break;
                            }

                            _CompileOperators(operatorStack, scriptByteCode, 3);    // 320  0x140
                            operatorStack.Push(new StackObject { Value = "-", Precedence = 3, OpCode = ScriptOpCodes.Complement });

                            //operatorOpCode = ScriptOpCodes.Complement;              // 320  0x140
                            _offset++;
                            continue;
                        }

                        _CompileOperators(operatorStack, scriptByteCode, 6);        // 399  0x18F
                        operatorStack.Push(new StackObject { Value = "-", Precedence = 6, OpCode = ScriptOpCodes.Sub });
                        _offset++;

                        continue;

                    case '<':
                        if (_offset + 1 >= _script.Length) throw new Exceptions.ScriptUnexpectedScriptTerminationException();
                        if (_script[_offset + 1] == '=')
                        {
                            _CompileOperators(operatorStack, scriptByteCode, 8);    // 448  0x1C0
                            operatorStack.Push(new StackObject { Value = "<=", Precedence = 8, OpCode = ScriptOpCodes.LessThanOrEqual });
                            _offset += 2;
                            continue;
                        }

                        _CompileOperators(operatorStack, scriptByteCode, 8);        // 426  0x1AA
                        operatorStack.Push(new StackObject { Value = "<", Precedence = 8, OpCode = ScriptOpCodes.LessThan });
                        _offset++;
                        continue;

                    case '>':
                        if (_offset + 1 >= _script.Length) throw new Exceptions.ScriptUnexpectedScriptTerminationException();
                        if (_script[_offset + 1] == '=')
                        {
                            _CompileOperators(operatorStack, scriptByteCode, 8);    // 459  0x1CB
                            operatorStack.Push(new StackObject { Value = ">=", Precedence = 8, OpCode = ScriptOpCodes.GreaterThanOrEqual });
                            _offset += 2;
                            continue;
                        }

                        _CompileOperators(operatorStack, scriptByteCode, 8);        // 437  0x1B5
                        operatorStack.Push(new StackObject { Value = ">", Precedence = 8, OpCode = ScriptOpCodes.GreaterThan });
                        _offset++;
                        continue;

                    case '=':       // 470  0x1D6
                        if (_offset + 1 >= _script.Length) throw new Exceptions.ScriptUnexpectedScriptTerminationException();
                        if (_script[_offset + 1] == '=')
                        {
                            _CompileOperators(operatorStack, scriptByteCode, 9);
                            operatorStack.Push(new StackObject { Value = "==", Precedence = 9, OpCode = ScriptOpCodes.EqualTo });
                            _offset += 2;
                            continue;
                        }

                        throw new Exceptions.ScriptFormatException("Unexpected assignment operator encountered.", _offset);

                    case '!':
                        if (_offset + 1 >= _script.Length) throw new Exceptions.ScriptUnexpectedScriptTerminationException();
                        if (_script[_offset + 1] == '=')
                        {
                            _CompileOperators(operatorStack, scriptByteCode, 9);    // 481  0x1E1
                            operatorStack.Push(new StackObject { Value = "!=", Precedence = 9, OpCode = ScriptOpCodes.NotEqualTo });
                            _offset += 2;
                            continue;
                        }


                        _CompileOperators(operatorStack, scriptByteCode, 3);        // 339  0x153
                        operatorStack.Push(new StackObject { Value = "!", Precedence = 3, OpCode = ScriptOpCodes.Not });
                        _offset++;
                        continue;

                    default:        // must be number, function, or variable

                        // is number
                        if (_IsNumber())
                        {
                            int scriptNum = _GetNumber();

                            scriptByteCode.AddRange(new[] { (Int32)ScriptOpCodes.Push, scriptNum });
                            break;
                        }

                        // is it an if-statement?
                        if (_script[_offset] == 'i' && _script[_offset + 1] == 'f')
                        {
                            if (_offset + 3 >= _script.Length) throw new Exceptions.ScriptUnexpectedScriptTerminationException();
                            if (_script[_offset + 1] == 'f' && (_script[_offset + 2] == ' ' || _script[_offset + 2] == '('))
                            {
                                // conditions block
                                _offset += 2;
                                _SkipWhite();

                                _offset++; // opening parenthesis
                                Int32[] ifBlockByteCode = _Compile(')', null, false, _GetByteOffset(scriptByteCode));
                                scriptByteCode.AddRange(ifBlockByteCode);

                                _offset++; // closing parenthesis
                                _SkipWhite();
                                if (_script[_offset] == '[')
                                {
                                    _offset = _script.IndexOf(']', ++_offset) + 1; // is debug script skip over
                                    _SkipWhite();
                                }

                                // if true block
                                char ifTrueBlockTerminator = ';';
                                if (_script[_offset] == '{')
                                {
                                    ifTrueBlockTerminator = '}';
                                    _offset++;
                                }
                                int ifBlockOffset = _GetByteOffset(scriptByteCode) + byteOffset + 8; // +8 for 2x codes for TernaryTrue
                                Int32[] ifTrueByteCode = _Compile(ifTrueBlockTerminator, null, false, ifBlockOffset);

                                if (ifTrueBlockTerminator == '}')
                                {
                                    if (_script[_offset] != '}') throw new Exceptions.ScriptFormatException("Unexpected end of if-block; expected closing '}'.", _offset);
                                    _offset++;
                                }
                                int ifByteOffsetToEndTrue = ifTrueByteCode.Length * 4 + ifBlockOffset; // +2 for 2x codes for TernaryTrue

                                // do we have else/else-if
                                int startElseCheck = _offset;
                                _SkipWhite();

                                Int32[] ifFalseByteCode = null;
                                if (_offset + 4 < _script.Length && _script[_offset] == 'e')
                                {
                                    String elseStr = _GetNameStr();
                                    if (elseStr == "else")
                                    {
                                        ifByteOffsetToEndTrue += 8; // +8 for 2x codes for TernaryFalse

                                        // todo: we are assuming all else-blocks are within braces... though maybe this would be better practice anyways?
                                        _SkipWhite();
                                        bool isElseIf = false;
                                        if (_script[_offset] == '{') // if-else
                                        {
                                            _offset++;
                                        }
                                        else if (_script[_offset] == 'i') // is-else if (we don't want to increase offset here as we're already at begining of if-statement (unlike above)
                                        {
                                            isElseIf = true; // the if() statement will leave us at the end (_offset = .Length)
                                        }
                                        else // i.e. not opening block and not if-else if block
                                        {
                                            throw new Exceptions.ScriptFormatException("Expected begining of else-block; expected opening '{'.", _offset); 
                                        }

                                        ifFalseByteCode = _Compile('}', null, false, ifByteOffsetToEndTrue);

                                        _SkipWhite();
                                        if (!isElseIf) // can't check end of if-else if block as the if() statement takes us to the end of the script code... possibly should fix this...
                                        {
                                            if (_script[_offset] != '}') throw new Exceptions.ScriptFormatException("Expected end of else-block; expected closing '}'.", _offset);
                                            _offset++;
                                        }
                                    }
                                    else
                                    {
                                        _offset = startElseCheck; // "roll back" our checks
                                    }
                                }

                                scriptByteCode.AddRange(new[] { (Int32)ScriptOpCodes.TernaryTrue, ifByteOffsetToEndTrue });
                                scriptByteCode.AddRange(ifTrueByteCode);

                                if (ifFalseByteCode != null)
                                {
                                    int ifByteOffsetToEndFalse = ifByteOffsetToEndTrue + ifFalseByteCode.Length * 4;
                                    scriptByteCode.AddRange(new[] { (Int32)ScriptOpCodes.TernaryFalse, ifByteOffsetToEndFalse });
                                    scriptByteCode.AddRange(ifFalseByteCode);
                                }
                                break;
                            }
                        }

                        // get our function/var name
                        int nameStrStart = _offset;
                        String nameStr = _GetNameStr();

                        _SkipWhite();

                        int functionStartOffset = _offset;
                        if (_script[_offset] == '(') // we have a function
                        {
                            // is it a stat set/get function?
                            if (nameStr.StartsWith("GetStat") || nameStr.StartsWith("SetStat"))
                            {
                                _CompileStatFunctions(scriptByteCode, nameStr, nameStrStart, functionStartOffset, contextPtr);
                            }
                            else // must be client/excep function
                            {
                                // is it a script error?
                                if (nameStr == "ScriptError") // then we have a int/byte array, already compiled
                                {
                                    int closingParanthesis = _script.IndexOf(')');
                                    String scriptIntArrayStr = _script.Substring(++_offset, (closingParanthesis - _offset));
                                    int[] scriptIntArray = scriptIntArrayStr.ToArray<int>(',');
                                    _offset = closingParanthesis;
                                    return scriptIntArray;
                                }

                                // check client functions
                                Function[] functions = _GetFunctions(nameStr);
                                if (functions.Length == 0) throw new Exceptions.ScriptUnknownFunctionException(nameStr);

                                Function function = functions[0];
                                bool isOverloaded = false;
                                if (functions.Length > 1)
                                {
                                    isOverloaded = true;
                                    function = (from func in functions
                                                orderby func.ArgCount descending
                                                select func).First();
                                }

                                // if we are using standard call functions with tcv4 excel usage, we need to check the function arguments count
                                int ignoreArgs = 0;
                                Function functionTCv4 = null;
                                //if (_forceStandardCallFunctionList && _forceTCv4ExcelUsage)
                                //{
                                //    functionTCv4 = (from func in CallFunctionsTCv4
                                //                    where function.ArgCount <= func.ArgCount && function.Name == func.Name
                                //                    select func).FirstOrDefault();
                                //    Debug.Assert(functionTCv4 != null); // hopefully there are none with removed args, only added...

                                //    if (function.ArgCount < functionTCv4.ArgCount) ignoreArgs = functionTCv4.ArgCount - function.ArgCount;
                                //}

                                int maxArgCount = function.ArgCount;
                                if (maxArgCount == 0) _offset++; // opening parenthesis

                                int argsFound = 0;
                                for (int argIndex = 0; argIndex < maxArgCount + ignoreArgs; argIndex++, argsFound++)
                                {
                                    _offset++; // opening parenthesis and commas


                                    // if TCv4 function has more args than standard, then use TCv4 arg when past standard count (script bytes wont be added anyways)
                                    Argument functionArg = null;
                                    if (argIndex < function.ArgCount)
                                    {
                                        functionArg = function.Args[argIndex];
                                    }
                                    else if (functionTCv4 != null)
                                    {
                                        functionArg = functionTCv4.Args[argIndex];
                                    }
                                    Debug.Assert(functionArg != null);

                                    // if we have an argument with an excel index, and we have a TCv4 function, we need to get the TCv4 excel index instead
                                    if (functionTCv4 != null && functionArg.TableIndex != -1)
                                    {
                                        functionArg = (from arg in functionTCv4.Args
                                                       where arg.Name == functionArg.Name
                                                       select arg).FirstOrDefault();
                                        Debug.Assert(functionArg.TableIndex != -1);
                                    }

                                    // determine applicable argument terminator
                                    char argTerminator = ',';
                                    if (argIndex == maxArgCount + ignoreArgs - 1) argTerminator = ')';

                                    // get argument byte code
                                    Int32[] argByteCode = _Compile(argTerminator, functionArg, false, 0, isOverloaded);
                                    if (argIndex < maxArgCount) scriptByteCode.AddRange(argByteCode);


                                    if (!isOverloaded) continue;
                                    _SkipWhite();
                                    if (_script[_offset] == ')') break;
                                }

                                if (isOverloaded)
                                {
                                    argsFound++;
                                    if (function.ArgCount != argsFound)
                                    {
                                        function = (from func in functions
                                                    where func.ArgCount == argsFound
                                                    select func).First();
                                    }
                                }

                                int functionIndex = CallFunctions.IndexOf(function);
                                if (functionIndex >= _initCallFunctionsCount) // properties etc functions
                                {
                                    scriptByteCode.AddRange(new[] { (Int32)ScriptOpCodes.CallPropery, functionIndex, 0 });
                                }
                                else // standard client functions
                                {
                                    scriptByteCode.AddRange(new[] { (Int32)ScriptOpCodes.Call, functionIndex });
                                }

                                _offset++; // closing parenthesis
                            }
                        }
                        else if (_script[_offset] == ')') // we have a type cast
                        {
                            // casts appear at the end of the arg segment
                            // i.e. (int)(double)GetStat666('strength_bonus') -> 666,176160768,648,598 NOT 598,648,666,176160768
                            // so while I bit messy, it's easier to just loop back and call ourself again and treat as function call essentially
                            ScriptOpCodes typeCast;
                            switch (nameStr)
                            {
                                case "int":
                                    typeCast = ScriptOpCodes.TypeCastDoubleInt; // 598
                                    break;

                                case "double":
                                    typeCast = ScriptOpCodes.TypeCastIntDouble; // 648
                                    break;

                                default:
                                    throw new NotImplementedException("Type cast not implemented: " + nameStr);
                            }

                            _offset++;
                            Int32[] scriptBytes = _Compile(terminator, null);
                            _offset--; // because a type cast begines with an opening paranthesis, it means we'll be returning at a case which *also* increases our offset [ case '(':       // opening paranthesis]
                            scriptByteCode.AddRange(scriptBytes);
                            scriptByteCode.Add((Int32)typeCast);
                        }
                        else // we have a variable
                        {
                            StackObject varObj;

                            // hmmm, we aren't checking if they've tried to define a float/etc... For now, meh...
                            if (nameStr == "int")  // is var allocation
                            {
                                String varName = _GetNameStr();
                                _SkipWhite();

                                if (_script[_offset] != '=') throw new Exceptions.ScriptFormatException("Unexpected end of variable definition code.", _offset);

                                _offset++; // '=' sign
                                _SkipWhite();

                                // ensire we don't already have a var by this name defined
                                if (_GetVar(varName) != null) throw new Exceptions.ScriptVariableAlreadyDefinedException(varName, _offset);
                                Int32[] variableDefCode = _Compile(';', null, false, _GetByteOffset(scriptByteCode), false);

                                int varByteOffset = _vars.Count * 4;
                                scriptByteCode.AddRange(new[] { (Int32)ScriptOpCodes.AllocateVar, varByteOffset });
                                scriptByteCode.AddRange(variableDefCode);
                                scriptByteCode.Add((Int32)ScriptOpCodes.AssignLocalVarInt32);

                                varObj = new StackObject
                                {
                                    Value = varName,
                                    ByteOffset = (uint)varByteOffset
                                };
                                _vars.Add(varObj);
                                break;
                            }
                            if (nameStr == "void")
                            {
                                throw new NotImplementedException(String.Format("void type usage not implemented at offset '{0}'", nameStrStart));
                            }

                            // funciton with too many arguments
                            if (_script[_offset] == ',' && argument != null) throw new Exceptions.ScriptFormatException("Attempted function call with too many arguments encountered.", nameStrStart);

                            // is var usage
                            varObj = _GetVar(nameStr);
                            if (varObj == null) throw new Exceptions.ScriptUnknownVarNameException(nameStr, nameStrStart);

                            scriptByteCode.AddRange(new[] { (Int32)ScriptOpCodes.PushLocalVarInt32, (Int32)varObj.ByteOffset });
                        }
                        break;
                }

                _SkipWhite();

                if (terminator == (char)0xFF) // if in a TernaryFalse segment
                {
                    _SkipWhite();
                    if (_script[_offset] == ';' || _script[_offset] == ')') break;
                }
                if ((withinCondition || overloadedFunction) && (endOfStatement || endOfCondition)) break;

                if (operatorOpCode == 0) continue;
                scriptByteCode.Add((Int32)operatorOpCode);
                operatorOpCode = 0;
            }

            _CompileOperators(operatorStack, scriptByteCode, 99);

            if (_offset == _script.Length && _level == 0) scriptByteCode.Add(0);

            _level--;
            //_singleStatement = false;
            return scriptByteCode.ToArray();
        }

        private static void _CompileOperators(Stack<StackObject> operatorStack, ICollection<Int32> scriptByteCode, int precedence)
        {
            if (operatorStack.Count == 0) return;

            StackObject operatorObj;
            do
            {
                operatorObj = operatorStack.Peek();

                if (operatorObj.Precedence > precedence) continue;

                scriptByteCode.Add((int)operatorObj.OpCode);
                operatorStack.Pop();
            } while (operatorStack.Count > 0 && operatorObj.Precedence <= precedence);


            //if (operatorStack.Count > 1)
            //{
            //    int b2p = 0;
            //}

            while (operatorStack.Count > 0 && precedence == 99)
            {
                operatorObj = operatorStack.Pop();
                scriptByteCode.Add((int)operatorObj.OpCode);
            }
        }

        #endregion

        #region Decompiler Functions

        /// <summary>
        /// Decompiles a script from byte codes to human readable text.
        /// </summary>
        /// <param name="scriptBytes">The script byte-code array.</param>
        /// <param name="offset">The starting offset within the byte code array.</param>
        /// <param name="debugScriptByteString">For debugging purposes only.</param>
        /// <param name="debugStringId">For debugging purposes only.</param>
        /// <param name="debugRow">For debugging purposes only.</param>
        /// <param name="debugCol">For debugging purposes only.</param>
        /// <param name="debugColName">For debugging purposes only.</param>
        /// <returns>Decompiled excel script byte codes as human readable script.</returns>
        public String Decompile(byte[] scriptBytes, int offset, String debugScriptByteString = null, String debugStringId = null, int debugRow = 0, int debugCol = 0, String debugColName = null)
        {
            _vars = new List<StackObject>();
            _stack = new Stack<StackObject>();
            _startOffset = offset;
            _offset = offset;

            if (DebugEnabled)
            {
                _debugScriptByteString = debugScriptByteString;
                _debugStringId = debugStringId;
                _debugRow = debugRow;
                _debugCol = debugCol;
                _debugColName = debugColName;
            }

            return _script = _Decompile(scriptBytes, scriptBytes.Length, 0).Value;
        }

        /// <summary>
        /// Decompiles an excel script function from byte codes to human readable text.
        /// </summary>
        /// <param name="excelScriptFunction">The excel script function to decompile.</param>
        /// <param name="excelFunction">The associated excel function to use for agrument reference.</param>
        /// <param name="debugScriptByteString">For debugging purposes only.</param>
        /// <param name="debugStringId">For debugging purposes only.</param>
        /// <param name="debugRow">For debugging purposes only.</param>
        /// <param name="debugCol">For debugging purposes only.</param>
        /// <param name="debugColName">For debugging purposes only.</param>
        /// <returns>Decompiled excel script byte codes as human readable script.</returns>
        private String _Decompile(ExcelFile.ExcelFunction excelScriptFunction, Function excelFunction, String debugScriptByteString = null, String debugStringId = null, int debugRow = 0, int debugCol = 0, String debugColName = null)
        {
            if (excelScriptFunction == null) throw new ArgumentNullException("excelScriptFunction", "The excel script function cannot be null!");
            if (excelFunction == null) throw new ArgumentNullException("excelFunction", "The excel function cannot be null!");

            _vars = new List<StackObject>();
            _stack = new Stack<StackObject>();
            _excelScriptFunction = excelScriptFunction;
            _excelFunction = excelFunction;
            _startOffset = 0;
            _offset = 0;

            if (DebugEnabled)
            {
                _debugScriptByteString = debugScriptByteString;
                _debugStringId = debugStringId;
                _debugRow = debugRow;
                _debugCol = debugCol;
                _debugColName = debugColName;
            }

            return _script = _Decompile(_excelScriptFunction.ScriptByteCode, _excelScriptFunction.ScriptByteCode.Length, 0).Value;
        }

        /// <summary>
        /// Decompiles a script from byte codes to human readable text.
        /// </summary>
        /// <param name="scriptBytes">The excel script function to decompile.</param>
        /// <param name="maxBytes">The maximum number of bytes to parse.</param>
        /// <param name="ifLevel">The current function stack level relating to if/else blocks (set to 0).</param>
        /// <returns>A stack object with decompiled script and number of bytes read.</returns>
        private StackObject _Decompile(byte[] scriptBytes, int maxBytes, int ifLevel)
        {
            bool debug = (_debugStringId != null) && DebugEnabled;
            bool debugShowParsed = false;
            bool debugOutputParsed = false;
            bool debugOutputFuncWithOpCode = false;
            bool debugScriptParsed = true;
            bool debugOutputBytesRead = false;
            String debugPos = null;

            if (debug)
            {
                _debugFormatConditionalByteCounts = false;
                debugOutputParsed = true;
                debugShowParsed = false;

                String rowName = String.Empty;
                if (_fileManager != null)
                {
                    int colIndex = 0;
                    switch (_debugStringId)
                    {
                        case "DAMAGE_EFFECTS":
                            debugOutputParsed = true;
                            break;

                        case "ITEMDISPLAY":
                            colIndex = 1;
                            debugShowParsed = false;
                            break;

                        case "MUSICCONDITIONS":
                            colIndex = 4;
                            break;
                    }

                    rowName = _fileManager.GetExcelRowStringFromStringId(_debugStringId, _debugRow, colIndex);
                }

                debugPos = String.Format("row({0}): '{1}', col({2}): '{3}', scriptBytes: {4}", _debugRow, rowName, _debugCol, _debugColName, _debugScriptByteString);
                if (debugShowParsed) Debug.WriteLine(debugPos);
            }

            int startStackCount = _stack.Count;
            int infCheck = 0;
            int scriptStartOffset = _offset;
            uint bytesRead = 0;
            bool processStackOnReturn = true;
            try
            {
                while (_offset < scriptStartOffset + maxBytes)
                {
                    ScriptOpCodes opCode = (ScriptOpCodes)FileTools.ByteArrayToUInt32(scriptBytes, ref _offset);
                    String value1;
                    //String value2;
                    uint index;
                    int functionIndex;
                    uint byteOffset;
                    StackObject stackObject1;
                    StackObject stackObject2;
                    int subMaxBytes;

                    //if (_debugRow == 893 && _debugCol == 155 && _offset == scriptStartOffset + 4)
                    //{
                    //    //debugOutputBytesRead = true;
                    //    int bp = 0;
                    //}

                    switch (opCode)
                    {
                        case ScriptOpCodes.Return:                   // 0    0x00
                            bytesRead += (uint)(_offset - scriptStartOffset);
                            return _Return(startStackCount, bytesRead, processStackOnReturn, ifLevel, debugShowParsed);

                        case ScriptOpCodes.CallPropery:              // 2    0x02
                            functionIndex = FileTools.ByteArrayToInt32(scriptBytes, ref _offset);
                            _CallFunction(functionIndex);
                            int nullByte = FileTools.ByteArrayToInt32(scriptBytes, ref _offset);
                            Debug.Assert(nullByte == 0);
                            break;

                        case ScriptOpCodes.Call:                     // 3    0x03
                            functionIndex = FileTools.ByteArrayToInt32(scriptBytes, ref _offset);
                            _CallFunction(functionIndex);
                            break;

                        case ScriptOpCodes.TernaryFalse:             // 4    0x04
                            _CheckStack(1, opCode);

                            int trueStatementCount = _stack.Count - startStackCount;
                            Debug.Assert(trueStatementCount > 0);

                            byteOffset = FileTools.ByteArrayToUInt32(scriptBytes, ref _offset);
                            subMaxBytes = (int)byteOffset - (_offset - _startOffset);
                            stackObject1 = _Decompile(scriptBytes, subMaxBytes, ifLevel + 1);

                            Debug.Assert(stackObject1.StatementCount > 0 || stackObject1.IfLevel > 0);
                            stackObject1.FalseStatements = stackObject1.StatementCount;
                            stackObject1.TrueStatements = trueStatementCount;
                            processStackOnReturn = (trueStatementCount > 1); // if more than 1 true statement, then we need to amalgamte them before returning for processing in the TernaryTrue block

                            if (!String.IsNullOrEmpty(stackObject1.Value)) _stack.Push(stackObject1);
                            break;

                        case ScriptOpCodes.AllocateVar:              // 6    0x06
                            byteOffset = FileTools.ByteArrayToUInt32(scriptBytes, ref _offset);
                            Debug.Assert((byteOffset % 4) == 0);

                            value1 = String.Format(" var{0} = ", byteOffset / 4);
                            _stack.Push(new StackObject { Value = value1, ByteOffset = byteOffset });
                            break;

                        case ScriptOpCodes.Unknown9:                // 9    0x09
                            _CheckStack(1, opCode);

                            stackObject1 = _stack.Pop();
                            stackObject1.Value = String.Format("(Unknown9)({0})", stackObject1.Value);
                            _stack.Push(stackObject1);
                            break;

                        case ScriptOpCodes.TernaryTrue:             // 14   0x0E
                            _CheckStack(1, opCode);

                            byteOffset = FileTools.ByteArrayToUInt32(scriptBytes, ref _offset);
                            subMaxBytes = (int)byteOffset - (_offset - _startOffset);

                            int blockIfLevel = (ifLevel % 2 == 0) ? ifLevel + 1 : 1; // if not multiple of two, we have if inside if
                            stackObject1 = _Decompile(scriptBytes, subMaxBytes, blockIfLevel);
                            if (stackObject1.FalseStatements == -1 && stackObject1.TrueStatements == -1) break; // from is-else blocks

                            String ternaryTrueFormat;
                            String conditionsScript;
                            bool isTernaryIf = false;
                            if (ifLevel > 0)
                            {
                                const String elseIfRel = "else if ({0})\n{{\n{2}\n}}";
                                const String elseIfDebug = "else if ({0})[{1}]\n{{\n{2}\n}}";
                                const String ifRel = "if ({0})\n{{\n{2}\n}}";
                                const String ifDebug = "if ({0})[{1}]\n{{\n{2}\n}}";

                                if (ifLevel == 0)
                                {
                                    ternaryTrueFormat = _debugFormatConditionalByteCounts ? ifDebug : ifRel;
                                }
                                else
                                {
                                    ternaryTrueFormat = _debugFormatConditionalByteCounts ? elseIfDebug : elseIfRel;
                                }

                                String ifElseBlock = String.Empty;
                                int baseIfLevel = 0;
                                int ifLevelsProcessed;
                                for (ifLevelsProcessed = 0; ifLevelsProcessed <= ifLevel; ifLevelsProcessed += 2)
                                {
                                    if (ifLevelsProcessed == 0)
                                    {
                                        stackObject2 = _stack.Pop();
                                    }
                                    else // reverse stack popping order
                                    {
                                        stackObject1 = _stack.Pop();
                                        stackObject2 = _stack.Pop();
                                    }

                                    String addNewLine = "\n";
                                    String addSemiColon = (stackObject1.StatementCount <= 1 && !stackObject1.IsIf) ? ";" : String.Empty;
                                    if (ifLevelsProcessed == ifLevel)
                                    {
                                        baseIfLevel = stackObject2.IfLevel;
                                        addNewLine = String.Empty;
                                        ternaryTrueFormat = _debugFormatConditionalByteCounts ? ifDebug : ifRel;
                                    }

                                    String statementString = stackObject1.Value;
                                    if (!stackObject1.IsIf && stackObject1.StatementCount <= 1)
                                    {
                                        statementString = "\t" + statementString;
                                    }

                                    ifElseBlock = addNewLine + String.Format(ternaryTrueFormat, stackObject2.Value, byteOffset, statementString + addSemiColon) + ifElseBlock;
                                }

                                // if we were in an if() within an if(), then we need to also grab the preceeding statements within the current block
                                while (baseIfLevel > 0 && _stack.Count > 0)
                                {
                                    StackObject preStatementCode = _stack.Pop();

                                    if (preStatementCode.IfLevel != baseIfLevel)
                                    {
                                        _stack.Push(preStatementCode);
                                        break;
                                    }

                                    String addSemiColon = (preStatementCode.IsIf) ? String.Empty : ";";
                                    ifElseBlock = String.Format("{0}{1}\n{2}", preStatementCode.Value, addSemiColon, ifElseBlock);
                                }

                                Debug.Assert(ifLevelsProcessed - 2 == ifLevel);
                                conditionsScript = ifElseBlock;
                            }
                            else if (stackObject1.FalseStatements == -1)
                            {
                                if (stackObject1.TrueStatements == 1)
                                {
                                    const String onlyTrue1Rel = "if ({0}) {2};";
                                    const String onlyTrue1Debug = "if ({0})[{1}] {2};";
                                    ternaryTrueFormat = _debugFormatConditionalByteCounts ? onlyTrue1Debug : onlyTrue1Rel;
                                }
                                else
                                {
                                    const String onlyTrueRel = "if ({0})\n{{\n\t{2}\n}}";
                                    const String onlyTrueDebug = "if ({0})[{1}]\n{{\n\t{2}\n}}";
                                    ternaryTrueFormat = _debugFormatConditionalByteCounts ? onlyTrueDebug : onlyTrueRel;
                                }

                                conditionsScript = String.Format(ternaryTrueFormat, _stack.Pop().Value, byteOffset, stackObject1.Value);
                            }
                            else
                            {
                                StackObject falseObj = _stack.Pop();
                                StackObject trueObj = _stack.Pop();
                                StackObject ifObj = _stack.Pop();

                                if (stackObject1.TrueStatements == 1 && stackObject1.FalseStatements == 1)
                                {
                                    const String true1False1Rel = "({0}) ? {2} : {3}";
                                    const String true1False1Debug = "({0}) ?[{1}] {2} : {3}";
                                    ternaryTrueFormat = _debugFormatConditionalByteCounts ? true1False1Debug : true1False1Rel;
                                    isTernaryIf = true;
                                }
                                else
                                {
                                    const String trueFalseRel = "if ({0})\n{{\n{2}}}\nelse\n{{\n{3}}}";
                                    const String trueFalseDebug = "if ({0})[{1}]\n{{\n{2}}}\nelse\n{{\n{3}}}";
                                    ternaryTrueFormat = _debugFormatConditionalByteCounts ? trueFalseDebug : trueFalseRel;

                                    String[] code = trueObj.Value.Split(new[] { ";", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                                    String codeStr = code.Aggregate(String.Empty, (current, line) => current + ("\t" + line + ";\n"));
                                    trueObj.Value = codeStr;

                                    code = falseObj.Value.Split(new[] { ";", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                                    codeStr = code.Aggregate(String.Empty, (current, line) => current + ("\t" + line + ";\n"));
                                    falseObj.Value = codeStr;
                                }

                                conditionsScript = String.Format(ternaryTrueFormat, ifObj.Value, byteOffset, trueObj.Value, falseObj.Value);
                            }

                            _stack.Push(new StackObject { Value = conditionsScript, IsIf = true, IsTernaryIf = isTernaryIf });
                            break;

                        case ScriptOpCodes.Push:                     // 26   0x1A
                            int value = FileTools.ByteArrayToInt32(scriptBytes, ref _offset);
                            _stack.Push(new StackObject { Value = value.ToString() });
                            break;

                        case ScriptOpCodes.PushLocalVarInt32:        // 50   0x32
                            byteOffset = FileTools.ByteArrayToUInt32(scriptBytes, ref _offset);
                            _PushLocalVar((int)byteOffset, ArgType.Int32);
                            break;

                        case ScriptOpCodes.PushLocalVarPtr:          // 57   0x39
                            byteOffset = FileTools.ByteArrayToUInt32(scriptBytes, ref _offset);
                            _PushLocalVar((int)byteOffset, ArgType.Ptr);
                            break;

                        case ScriptOpCodes.AssignLocalVarInt32:      // 98   0x62
                            _CheckStack(2, opCode);

                            stackObject2 = _stack.Pop();
                            stackObject1 = _stack.Pop();

                            Debug.Assert(stackObject2.Type == ArgType.Int32);
                            stackObject2.ByteOffset = stackObject1.ByteOffset;
                            _vars.Add(stackObject2);

                            value1 = String.Format("int{0}{1}", stackObject1.Value, stackObject2.Value);
                            _stack.Push(new StackObject { Value = value1, IsVarAssign = true, Type = ArgType.Int32, IfLevel = ifLevel });
                            break;

                        case ScriptOpCodes.AssignLocalVarPtr:        // 105   0x69
                            _CheckStack(2, opCode);

                            stackObject2 = _stack.Pop();
                            stackObject1 = _stack.Pop();

                            Debug.Assert(stackObject2.Type == ArgType.Ptr || stackObject2.Type == ArgType.ContextPtr);
                            stackObject2.ByteOffset = stackObject1.ByteOffset;
                            _vars.Add(stackObject2);

                            value1 = String.Format("void*{0}{1}", stackObject1.Value, stackObject2.Value);
                            _stack.Push(new StackObject { Value = value1, IsVarAssign = true, Type = ArgType.Ptr, IfLevel = ifLevel });
                            break;

                        case ScriptOpCodes.Complement:               // 320  0x140
                            _CheckStack(1, opCode);

                            stackObject1 = _stack.Pop();

                            const String complementFormatMany = "-({0})";
                            const String complementFormatSingle = "-{0}";
                            String complementFormat = (stackObject1.StatementCount > 0 || stackObject1.OperatorCount > 0) ? complementFormatMany : complementFormatSingle;

                            stackObject1.Value = String.Format(complementFormat, stackObject1.Value);
                            stackObject1.OpCode = opCode;
                            _stack.Push(stackObject1);
                            break;

                        case ScriptOpCodes.Not:                      // 339  0x153
                            _CheckStack(1, opCode);

                            stackObject1 = _stack.Pop();

                            const String notFormatMany = "!({0})";
                            const String notFormatSingle = "!{0}";
                            String notFormat = (stackObject1.StatementCount > 0 || stackObject1.OperatorCount > 0) ? notFormatMany : notFormatSingle;

                            stackObject1.Value = String.Format(notFormat, stackObject1.Value);
                            stackObject1.OpCode = opCode;
                            _stack.Push(stackObject1);
                            break;

                        case ScriptOpCodes.Pow:                      // 347  0x15B
                            _DoOperator("^", 4, opCode, ifLevel);
                            break;

                        case ScriptOpCodes.Mult:                     // 358  0x166
                            _DoOperator(" * ", 5, opCode, ifLevel);
                            break;

                        case ScriptOpCodes.Div:                      // 369  0x171
                            _DoOperator(" / ", 5, opCode, ifLevel);
                            break;

                        case ScriptOpCodes.Add:                      // 388  0x184
                            _DoOperator(" + ", 6, opCode, ifLevel);
                            break;

                        case ScriptOpCodes.Sub:                      // 399  0x18F
                            _DoOperator(" - ", 6, opCode, ifLevel);
                            break;

                        case ScriptOpCodes.LessThan:                 // 426  0x1AA
                            _DoOperator(" < ", 8, opCode, ifLevel);
                            break;

                        case ScriptOpCodes.GreaterThan:              // 437  0x1B5
                            _DoOperator(" > ", 8, opCode, ifLevel);
                            break;

                        case ScriptOpCodes.LessThanOrEqual:          // 448  0x1C0
                            _DoOperator(" <= ", 8, opCode, ifLevel);
                            break;

                        case ScriptOpCodes.GreaterThanOrEqual:       // 459  0x1CB
                            _DoOperator(" >= ", 8, opCode, ifLevel);
                            break;

                        case ScriptOpCodes.EqualTo:                  // 470  0x1D6
                            _DoOperator(" == ", 9, opCode, ifLevel);
                            break;

                        case ScriptOpCodes.NotEqualTo:               // 481  0x1E1
                            _DoOperator(" != ", 9, opCode, ifLevel);
                            break;

                        case ScriptOpCodes.And:                      // 516  0x204
                            _DecompileCondition(scriptBytes, opCode, ifLevel);
                            break;

                        case ScriptOpCodes.Or:                       // 527  0x20F
                            _DecompileCondition(scriptBytes, opCode, ifLevel);
                            break;

                        case ScriptOpCodes.EndCond:                    // 538  0x21A
                            _CheckStack(2, opCode);

                            stackObject2 = _stack.Pop();
                            stackObject1 = _stack.Pop();

                            const String orFormat = "{0} || {1}";
                            const String andFormat = "{0} && {1}";
                            const String andSingleAndMany = "{0} && ({1})"; // while this doesn't really matter (brackets can be removed) it does produce differing compile byte code, which for debugging I don't want
                            const String orAndFormat = "{0} || ({1})";
                            const String andOrFormat = "{0} && ({1})";

                            String endIfFormat;
                            if (stackObject1.OpCode == ScriptOpCodes.Or && stackObject2.OpCode == ScriptOpCodes.And)
                            {
                                endIfFormat = orAndFormat;
                            }
                            else if (stackObject1.OpCode == ScriptOpCodes.And && stackObject2.OpCode == ScriptOpCodes.Or)
                            {
                                endIfFormat = andOrFormat;
                            }
                            else
                            {
                                endIfFormat = stackObject1.OpCode == ScriptOpCodes.Or ? orFormat : andFormat;

                                if (stackObject2.StatementCount > 0)
                                {
                                    endIfFormat = andSingleAndMany;
                                }
                            }

                            StackObject endCondStackObj = new StackObject
                            {
                                Value = String.Format(endIfFormat, stackObject1.Value, stackObject2.Value),
                                OpCode = stackObject1.OpCode,
                                StatementCount = (stackObject1.StatementCount == -1) ? 1 : stackObject1.StatementCount + 1
                            };
                            _stack.Push(endCondStackObj);
                            break;

                        case ScriptOpCodes.TypeCastDoubleInt:        // 598  0x256
                            _TypeCast("int");
                            break;

                        case ScriptOpCodes.TypeCastIntDouble:        // 648  0x288
                            _TypeCast("double");
                            break;

                        case ScriptOpCodes.GetStat666:               // 666  0x29A
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref _offset);
                            _StatsFunction("GetStat666", index, opCode, false);
                            break;

                        case ScriptOpCodes.GetStat667:               // 667  0x29B
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref _offset);
                            _StatsFunction("GetStat667", index, opCode, false);
                            break;

                        case ScriptOpCodes.SetStat669:               // 669  0x29D
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref _offset);
                            _StatsFunction("SetStat669", index, opCode, true);
                            break;

                        case ScriptOpCodes.SetStat673:               // 673  0x2A1
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref _offset);
                            _StatsFunction("SetStat673", index, opCode, true);
                            break;

                        case ScriptOpCodes.SetStat674:               // 674  0x2A2
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref _offset);
                            _StatsFunction("SetStat674", index, opCode, true);
                            break;

                        case ScriptOpCodes.GetStat680:               // 680  0x2A8
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref _offset);
                            _StatsFunction("GetStat680", index, opCode, false);
                            break;

                        case ScriptOpCodes.SetStat683:               // 683  0x2AB
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref _offset);
                            _StatsFunction("SetStat683", index, opCode, true);
                            break;

                        case ScriptOpCodes.SetStat687:               // 687  0x2AF
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref _offset);
                            _StatsFunction("SetStat687", index, opCode, true);
                            break;

                        case ScriptOpCodes.SetStat688:               // 688  0x2B0
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref _offset);
                            _StatsFunction("SetStat688", index, opCode, true);
                            break;

                        case ScriptOpCodes.PushContextVarInt32:      // 700 0x2BC
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref _offset);

                            value1 = String.Format("${0}", ((ContextVariables)index).ToString().ToLower());
                            _stack.Push(new StackObject { Value = value1, IsFunction = true, Type = ArgType.Int32 });
                            break;

                        case ScriptOpCodes.PushContextVarUInt32:     // 701 0x2BD
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref _offset);
                            _PushContextVariable("PushContextVarUInt32", index);
                            break;

                        case ScriptOpCodes.PushContextVarInt64:      // 702 0x2BE
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref _offset);
                            _PushContextVariable("PushContextVarInt64", index);
                            break;

                        case ScriptOpCodes.PushContextVarUInt64:     // 703 0x2BF
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref _offset);
                            _PushContextVariable("PushContextVarUInt64", index);
                            break;

                        case ScriptOpCodes.PushContextVarFloat:      // 704 0x2C0
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref _offset);
                            _PushContextVariable("PushContextVarFloat", index);
                            break;

                        case ScriptOpCodes.PushContextVarDouble:     // 705 0x2C1
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref _offset);
                            _PushContextVariable("PushContextVarDouble", index);
                            break;

                        case ScriptOpCodes.PushContextVarDouble2:    // 706 0x2C2
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref _offset);
                            _PushContextVariable("PushContextVarDouble2", index);
                            break;

                        case ScriptOpCodes.PushContextVarPtr:        // 707 0x2C3
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref _offset);

                            value1 = String.Format("${0}", ((ContextVariables)index).ToString().ToLower());
                            _stack.Push(new StackObject { Value = value1, IsFunction = true, Type = ArgType.ContextPtr });
                            break;

                        case ScriptOpCodes.GlobalVarGame3:           // 708  0x2C4
                            _stack.Push(new StackObject { Value = "@game3", Type = ArgType.Game3 });
                            break;

                        case ScriptOpCodes.GlobalVarContext:         // 709  0x2C5
                            _stack.Push(new StackObject { Value = "@context", Type = ArgType.Context });
                            break;

                        case ScriptOpCodes.GlobalVarGame4:           // 710  0x2C6
                            _stack.Push(new StackObject { Value = "@game4", Type = ArgType.Game4 });
                            break;

                        case ScriptOpCodes.GlobalVarUnit:            // 711  0x2C7
                            _stack.Push(new StackObject { Value = "@unit", Type = ArgType.Unit });
                            break;

                        case ScriptOpCodes.GlobalVarStatsList:           // 712  0x2C8
                            _stack.Push(new StackObject { Value = "@statslist", Type = ArgType.StatsList });
                            break;

                        case ScriptOpCodes.AssignContextVar:         // 713  0x2C9
                            _CheckStack(2, opCode);

                            stackObject2 = _stack.Pop();
                            stackObject1 = _stack.Pop();
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref _offset);

                            value1 = String.Format("{0} = ({1}){2}", stackObject1.Value, ((ContextVariables)index).ToString().ToLower(), stackObject2.Value);
                            _stack.Push(new StackObject { Value = value1, IsVarAssign = true });
                            break;

                        case ScriptOpCodes.UsePtrObjectReference:    // 714  0x2CA
                            _CheckStack(1, opCode);

                            stackObject1 = _stack.Pop();
                            Debug.Assert(stackObject1.Type == ArgType.ContextPtr);

                            value1 = String.Format("{0}->", stackObject1.Value);
                            _stack.Push(new StackObject { Value = value1, IsVarAssign = true, Type = ArgType.ContextPtr });
                            break;

                        default:
                            Debug.WriteLine(String.Format("Unknown OpCode: {0} at offset {1}", opCode, _offset));
                            throw new Exceptions.ScriptUnknownOpCodeException(opCode.ToString(), _DumpStack(_stack));
                    }

                    infCheck++;
                    if (infCheck >= 1000) throw new Exceptions.ScriptInfiniteCheckException(opCode.ToString(), _DumpStack(_stack));
                }
            }
            catch (Exception e)
            {
                if (debug)
                {
                    String debugOutputPath = String.Format("{0}{1}_scriptdebug.txt", _debugRoot, _debugStringId);
                    String debugOutput = String.Format("{0}\n{1}\n\n\n", debugPos, e);
                    File.AppendAllText(debugOutputPath, debugOutput);
                    debugScriptParsed = false;
                }

                throw;
            }
            finally
            {
                if (((debugOutputParsed && debugScriptParsed) || (debugOutputFuncWithOpCode && debugScriptParsed)) && startStackCount == 0)
                {
                    String debugOutputPath = String.Format("{0}{1}_scriptdebug.txt", _debugRoot, _debugStringId);
                    String debugOutput = String.Format("{0}\n{1}\n\n", debugPos, _script);
                    File.AppendAllText(debugOutputPath, debugOutput);
                }
            }

            bytesRead += (uint)(_offset - scriptStartOffset);
            if (debugOutputBytesRead) Console.WriteLine("Read from {0} to {1} = {2} bytes.", scriptStartOffset, _offset, scriptStartOffset - _offset);
            return _Return(startStackCount, bytesRead, processStackOnReturn, ifLevel, debugShowParsed);
        }

        #endregion
    }
}
