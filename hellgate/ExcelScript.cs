using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using Hellgate.Excel;
using Revival.Common;

namespace Hellgate
{
    public partial class ExcelScript
    {
        private const String DebugRoot = @"C:\excel_script_debug\";
        private static bool _debug;

        private static FileManager _fileManager;
        private static bool _havePropertiesFunctions;
        private static bool _haveSkillsFunctions;

        private static ExcelFile _statsTable;
        private static ObjectDelegator _statsDelegator;

        private String _script;
        private readonly Stack<StackObject> _stack;
        private readonly StackObject[] _vars;
        private ExcelFile.ExcelFunction _excelScriptFunction;
        private Function _excelFunction;

        public ExcelScript()
        {
            _script = String.Empty;
            _stack = new Stack<StackObject>();
            _vars = new StackObject[10];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="script"></param>
        /// <param name="scriptByteString"></param>
        /// <param name="stringId"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="colName"></param>
        /// <returns></returns>
        public int[] Compile(String script, String scriptByteString = null, String stringId = null, int row = 0, int col = 0, String colName = null)
        {
            int offset = 0;
            return _Compile(script, ref offset, '\0', null, scriptByteString, stringId, row, col, colName);
        }

        private Int32[] _Compile(String script, ref int offset, char terminator, Argument argument, String scriptByteString = null, String stringId = null, int row = 0, int col = 0, String colName = null)
        {
            if (String.IsNullOrEmpty(script)) throw new ArgumentNullException("script", "Script string cannot be null or empty!");

            List<Int32> scriptByteCode = new List<Int32>();
            ScriptOpCodes operatorOpCode = ScriptOpCodes.Return;
            int bp = 0;
            String excelStr;
            int rowIndex;
            Stack<StackObject> operatorStack = new Stack<StackObject>();

            if (row == 16 && col == 150)
            {
                bp = 0;
            }

            while (offset < script.Length && script[offset] != terminator)
            {
                _SkipWhite(script, ref offset);

                switch (script[offset])
                {
                    case '(':       // parantheses
                        offset++;
                        Int32[] paranethesesCodeBytes = _Compile(script, ref offset, ')', null, scriptByteString, stringId, row, col, colName);
                        scriptByteCode.AddRange(paranethesesCodeBytes);

                        Debug.Assert(script[offset] == ')');
                        offset++;
                        break;

                    case '@':       // global var
                        offset++;
                        String globalName = _GetNameStr(script, ref offset);

                        if (argument != null)
                        {
                            ArgType argType = _GetArgType(globalName);
                            if (argument.Type != argType) throw new Exceptions.ScriptInvalidArgTypeException(globalName, argument.Type.ToString().ToLower(), offset - globalName.Length);
                        }

                        String asOpCodeName = "globalvar" + globalName;
                        ScriptOpCodes opCode = _GetScriptOpCode(asOpCodeName);
                        if (opCode == 0) throw new Exceptions.ScriptUnknownVarNameException(globalName, offset - globalName.Length);

                        scriptByteCode.Add((Int32)opCode);
                        break;

                    case '$':       // context var
                        offset++;
                        String contextName = _GetNameStr(script, ref offset);

                        ContextVariables contextVar = Enum.GetValues(typeof(ContextVariables)).Cast<ContextVariables>().Where(type => type.ToString().ToLower() == contextName).FirstOrDefault();
                        if (contextVar == 0 && contextName != "unit") throw new Exceptions.ScriptUnknownVarNameException(contextName, offset - contextName.Length);

                        ArgType constextArgType = _GetArgType(contextName);

                        bool isPtr = false;
                        ScriptOpCodes contextOpCode = ScriptOpCodes.Return;
                        if (script[offset] == '-' && script[offset + 1] == '>') // is ptr
                        {
                            isPtr = true;
                            constextArgType = ArgType.Context;
                            offset += 2;
                        }

                        if (!isPtr && argument != null && argument.Type != constextArgType) throw new Exceptions.ScriptInvalidArgTypeException(contextName, argument.Type.ToString().ToLower(), offset - contextName.Length);

                        switch (constextArgType)
                        {
                            case ArgType.Int32:
                                contextOpCode = ScriptOpCodes.PushContextVarInt32;
                                break;

                            case ArgType.Context:
                                contextOpCode = ScriptOpCodes.PushContextVarPtr;
                                break;

                            default:
                                bp = 0;
                                break;
                        }
                        Debug.Assert((int)contextOpCode != 0);

                        scriptByteCode.AddRange(new[] { (Int32)contextOpCode, (Int32)contextVar });

                        if (isPtr) scriptByteCode.Add((Int32)ScriptOpCodes.UsePtrObjectReference);
                        break;

                    case '\'':      // string
                        if (argument == null || argument.Type != ArgType.ExcelIndex) throw new Exceptions.ScriptFormatException("Unexpected string encountered.", offset);

                        excelStr = _GetString(script, ++offset);
                        rowIndex = _fileManager.GetExcelRowIndexFromTableIndex(argument.TableIndex, excelStr);
                        if (rowIndex == -1) throw new Exceptions.UnknownExcelStringException(excelStr, offset);
                        offset += excelStr.Length + 1;

                        scriptByteCode.AddRange(new[] { (Int32)ScriptOpCodes.Push, rowIndex });
                        break;

                    case ';':       // end of script block
                        Debug.Assert(argument == null);
                        offset++;
                        break;

                    case '\n':      // end of line
                        offset++;
                        break;

                    case ':':       // 4    0x04
                        offset++;
                        _SkipWhite(script, ref offset);
                        if (script[offset] == '[') offset = script.IndexOf(']', ++offset) + 1; // is debug script skip over

                        Int32[] ternaryFalseBytes = _Compile(script, ref offset, (char)0xFF, null, scriptByteString, stringId, row, col, colName);

                        int byteOffsetToEndFalse = (scriptByteCode.Count + ternaryFalseBytes.Length) * 4 + 8; // +8 for 2x codes for TernaryFalse

                        scriptByteCode.AddRange(new[] { (Int32)ScriptOpCodes.TernaryFalse, byteOffsetToEndFalse });
                        scriptByteCode.AddRange(ternaryFalseBytes);
                        break;

                    case '?':       // 14   0x0E
                        offset++;
                        _SkipWhite(script, ref offset);
                        if (script[offset] == '[') offset = script.IndexOf(']', ++offset) + 1; // is debug script skip over

                        Int32[] ternaryTrueBytes = _Compile(script, ref offset, ':', null, scriptByteString, stringId, row, col, colName);

                        int byteOffsetToEndTrue = (scriptByteCode.Count + ternaryTrueBytes.Length) * 4 + 8; // +8 for 2x codes for TernaryTrue
                        if (script[offset] == ':') byteOffsetToEndTrue += 8; // +8 for 2x codes for TernaryFalse

                        scriptByteCode.AddRange(new[] { (Int32)ScriptOpCodes.TernaryTrue, byteOffsetToEndTrue });
                        scriptByteCode.AddRange(ternaryTrueBytes);
                        break;

                    case '!':
                        if (script[offset + 1] == '=')          // 481  0x1E1
                        {
                            operatorStack.Push(new StackObject { Value = "!=", Precedence = 9, ByteOffset = (uint)ScriptOpCodes.NotEqualTo });
                            offset += 2;
                            continue;
                        }

                        operatorOpCode = ScriptOpCodes.Not;     // 339  0x153
                        offset++;
                        continue;

                    case '^':       // 347  0x15B
                        operatorStack.Push(new StackObject { Value = "^", Precedence = 4, ByteOffset = (uint)ScriptOpCodes.Pow });
                        offset++;
                        continue;

                    case '*':       // 358  0x166
                        _CompileOperators(operatorStack, scriptByteCode, 5);
                        operatorStack.Push(new StackObject { Value = "*", Precedence = 5, ByteOffset = (uint)ScriptOpCodes.Mult });
                        offset++;
                        continue;

                    case '/':       // 369  0x171
                        _CompileOperators(operatorStack, scriptByteCode, 5);
                        operatorStack.Push(new StackObject { Value = "/", Precedence = 5, ByteOffset = (uint)ScriptOpCodes.Div });
                        offset++;
                        continue;

                    case '+':       // 388  0x184
                        _CompileOperators(operatorStack, scriptByteCode, 6);
                        operatorStack.Push(new StackObject { Value = "+", Precedence = 6, ByteOffset = (uint)ScriptOpCodes.Add });
                        offset++;
                        continue;

                    case '&':       // 516  0x204
                        if (script[offset + 1] != '&') throw new NotImplementedException(String.Format("Binary AND operations are not currently implemented. Offset = '{0}'", offset));

                        throw new NotImplementedException(String.Format("Conditional && operators are not currently implement. Offset = '{0}'", offset));
                        break;

                    case '|':       // 527  0x20F
                        if (script[offset + 1] != '|') throw new NotImplementedException(String.Format("Binary OR operations are not currently implemented. Offset = '{0}'", offset));

                        throw new NotImplementedException(String.Format("Conditional || operators are not currently implement. Offset = '{0}'", offset));
                        break;

                    case '-':
                        if (scriptByteCode.Count == 0)
                        {
                            // is is a negative number
                            if (_IsNumber(script, offset))
                            {
                                int scriptNum = _GetNumber(script, ref offset);

                                scriptByteCode.AddRange(new[] { (Int32)ScriptOpCodes.Push, scriptNum });
                                break;
                            }

                            operatorOpCode = ScriptOpCodes.Complement;          // 320  0x140
                            offset++;
                            continue;
                        }

                        _CompileOperators(operatorStack, scriptByteCode, 6);    // 399  0x18F
                        operatorStack.Push(new StackObject { Value = "-", Precedence = 6, ByteOffset = (uint)ScriptOpCodes.Sub });
                        offset++;

                        continue;

                    case '<':       // 426  0x1AA
                        if (script[offset + 1] == '=')
                        {
                            bp = 0;
                        }

                        operatorStack.Push(new StackObject { Value = "<", Precedence = 8, ByteOffset = (uint)ScriptOpCodes.LessThan });
                        offset++;
                        continue;

                    case '>':       // 437  0x1B5
                        if (script[offset + 1] == '=')
                        {
                            bp = 0;
                        }

                        operatorStack.Push(new StackObject { Value = ">", Precedence = 8, ByteOffset = (uint)ScriptOpCodes.GreaterThan });
                        offset++;
                        continue;

                    case '=':       // 470  0x1D6
                        if (script[offset + 1] == '=')
                        {
                            operatorStack.Push(new StackObject { Value = "==", Precedence = 9, ByteOffset = (uint)ScriptOpCodes.EqualTo });
                            offset += 2;
                            continue;
                        }

                        bp = 0; // var assignment
                        break;

                    default:        // must be number, function, or variable

                        // is number
                        if (_IsNumber(script, offset))
                        {
                            int scriptNum = _GetNumber(script, ref offset);

                            scriptByteCode.AddRange(new[] { (Int32)ScriptOpCodes.Push, scriptNum });
                            break;
                        }

                        // is it an if-statement?
                        if (script[offset] == 'i')
                        {
                            if (offset + 3 >= script.Length) throw new Exceptions.ScriptUnexpectedScriptTermination();

                            if (script[offset + 1] == 'f' && (script[offset + 2] == ' ' || script[offset + 2] == '('))
                            {
                                // conditions block
                                offset += 2;
                                _SkipWhite(script, ref offset);

                                offset++; // opening parenthesis
                                Int32[] ifBlockByteCode = _Compile(script, ref offset, ')', null, scriptByteString, stringId, row, col, colName);
                                scriptByteCode.AddRange(ifBlockByteCode);

                                offset++; // closing parenthesis
                                _SkipWhite(script, ref offset);


                                // if true block
                                char ifTrueBlockTerminator = ';';
                                if (script[offset] == '{') ifTrueBlockTerminator = '}';
                                Int32[] ifTrueByteCode = _Compile(script, ref offset, ifTrueBlockTerminator, null, scriptByteString, stringId, row, col, colName);

                                int ifByteOffsetToEndTrue = (scriptByteCode.Count + ifTrueByteCode.Length) * 4 + 8; // +8 for 2x codes for TernaryTrue
                                scriptByteCode.AddRange(new[] { (Int32)ScriptOpCodes.TernaryTrue, ifByteOffsetToEndTrue });
                                scriptByteCode.AddRange(ifTrueByteCode);


                                // if false block
                                // todo

                                //if (script[offset] == ':') ifByteOffsetToEndTrue += 8; // +8 for 2x codes for TernaryFalse);

                                break;
                            }
                        }

                        // get our function/var name
                        int funcNameStartOffset = offset;
                        String nameStr = _GetNameStr(script, ref offset);

                        _SkipWhite(script, ref offset);

                        int functionStartOffset = offset;
                        if (script[offset] == '(') // we have a function
                        {
                            // is it a stat set/get function?
                            if (nameStr.StartsWith("GetStat") || nameStr.StartsWith("SetStat"))
                            {
                                ScriptOpCodes funcOpCode = _GetScriptOpCode(nameStr.ToLower(CultureInfo.InvariantCulture));
                                if (funcOpCode == 0) throw new Exceptions.ScriptUnknownVarNameException(nameStr, funcNameStartOffset);

                                offset++;
                                _SkipWhite(script, ref offset);

                                // do we have excel string or row index?
                                Excel.Stats statRow = null;
                                rowIndex = -1;
                                if (script[offset] == '\'')
                                {
                                    excelStr = _GetString(script, ++offset);

                                    foreach (Stats tableRow in _statsTable.Rows)
                                    {
                                        rowIndex++;

                                        if ((String)_statsDelegator["stat"](tableRow) != excelStr) continue;

                                        statRow = tableRow;
                                        break;
                                    }

                                    if (statRow == null) throw new Exceptions.UnknownExcelStringException(excelStr, offset);
                                    offset += excelStr.Length + 1;
                                }
                                else
                                {
                                    rowIndex = _GetNumber(script, ref offset);

                                    if (rowIndex < 0 || rowIndex > _statsTable.Rows.Count) throw new IndexOutOfRangeException(String.Format("Excel row index '{0}' out of range at script offset '{1}'", rowIndex, offset - rowIndex.ToString().Length));

                                    statRow = (Stats)_statsTable.Rows[rowIndex];
                                    excelStr = rowIndex.ToString(); // for exception below
                                }

                                _SkipWhite(script, ref offset);

                                int param1Table = (int)_statsDelegator["param1Table"](statRow);
                                bool isSetStat = (nameStr.StartsWith("Set"));

                                Int32[] callStatFunc = new[] { (Int32)funcOpCode, 0 };
                                if (param1Table == -1)
                                {
                                    int param = rowIndex << 22;
                                    callStatFunc[1] = param;
                                }
                                else
                                {
                                    if (script[offset] != ',') throw new Exceptions.ScriptFunctionArgumentCountException(nameStr, 2, String.Format("\nThe GetStat for stat '{0}' requires an extra parameter at offset '{1}'.", excelStr, offset));

                                    offset++;
                                    _SkipWhite(script, ref offset);

                                    int paramRowIndex = -1;
                                    if (script[offset] == '\'') // excel string
                                    {
                                        String paramStr = _GetString(script, ++offset);
                                        paramRowIndex = _fileManager.GetExcelRowIndexFromTableIndex(param1Table, paramStr);
                                        if (paramRowIndex == -1) throw new Exceptions.UnknownExcelStringException(paramStr, offset);
                                        offset += paramStr.Length + 1;
                                    }
                                    else // row index
                                    {
                                        paramRowIndex = _GetNumber(script, ref offset);
                                        if (paramRowIndex == -1) throw new Exceptions.UnknownExcelStringException(paramRowIndex.ToString(), offset);
                                    }

                                    int param = (rowIndex << 22) | paramRowIndex;
                                    callStatFunc[1] = param;
                                }

                                if (isSetStat)
                                {
                                    if (script[offset] != ',') throw new Exceptions.ScriptFunctionArgumentCountException(nameStr, 2, String.Format("\nThe GetStat for stat '{0}' requires an value to set it to at offset '{1}'.", excelStr, offset));
                                    offset++;

                                    Int32[] getStatArgBytes = _Compile(script, ref offset, ')', null, scriptByteString, stringId, row, col, colName);
                                    scriptByteCode.AddRange(getStatArgBytes);
                                }

                                if (script[offset] != ')') throw new Exceptions.ScriptFormatException(String.Format("Unexpected end of function '{0}' starting at offset '{1}'", nameStr, functionStartOffset), offset);
                                offset++;

                                scriptByteCode.AddRange(callStatFunc);
                            }
                            else // must be client/excep function
                            {
                                // check client functions
                                Function function = _GetFunction(nameStr);
                                if (function == null) throw new Exceptions.ScriptUnknownFunctionException(nameStr);

                                for (int argIndex = 0; argIndex < function.ArgCount; argIndex++)
                                {
                                    offset++;

                                    char argTerminator = ',';
                                    if (argIndex == function.ArgCount - 1) argTerminator = ')';

                                    Int32[] argByteCode = _Compile(script, ref offset, argTerminator, function.Args[argIndex], scriptByteString, stringId, row, col, colName);
                                    scriptByteCode.AddRange(argByteCode);
                                }

                                int functionIndex = CallFunctions.IndexOf(function);
                                if (functionIndex > 251) // properties etc functions
                                {
                                    scriptByteCode.AddRange(new[] { (Int32)ScriptOpCodes.CallPropery, functionIndex, 0 });
                                }
                                else // standard client functions
                                {
                                    scriptByteCode.AddRange(new[] { (Int32)ScriptOpCodes.Call, functionIndex });
                                }

                                offset++;
                            }
                        }
                        else // we have a variable
                        {
                            bp = 0; // todo
                        }
                        break;
                }

                _SkipWhite(script, ref offset);

                if (terminator == (char)0xFF) break;

                if (operatorOpCode == 0) continue;
                scriptByteCode.Add((Int32)operatorOpCode);
                operatorOpCode = 0;
            }

            _CompileOperators(operatorStack, scriptByteCode, 99);

            if (offset == script.Length) scriptByteCode.Add(0);

            return scriptByteCode.ToArray();
        }

        private static void _CompileOperators(Stack<StackObject> operatorStack, ICollection<Int32> scriptByteCode, int precedence)
        {
            if (operatorStack.Count == 0) return;

            int bp = 0;

            StackObject operatorObj = operatorStack.Peek();

            if (operatorObj.Precedence <= precedence)
            {
                scriptByteCode.Add((int)operatorObj.ByteOffset);
                operatorStack.Pop();
            }

            if (operatorStack.Count > 1)
            {
                int b2p = 0;
            }

            while (operatorStack.Count > 0 && precedence == 99)
            {
                operatorObj = operatorStack.Pop();
                scriptByteCode.Add((int)operatorObj.ByteOffset);
            }
        }

        public String GetScript { get { return _script; } }

        /// <summary>
        /// Decompiles a script from byte codes to human readable text.
        /// </summary>
        /// <param name="scriptBytes">The script byte-code array.</param>
        /// <param name="offset">The starting offset within the byte code array.</param>
        /// <param name="scriptString">For debugging purposes only.</param>
        /// <param name="stringId">For debugging purposes only.</param>
        /// <param name="row">For debugging purposes only.</param>
        /// <param name="col">For debugging purposes only.</param>
        /// <param name="colName">For debugging purposes only.</param>
        /// <returns>Decompiled excel script byte codes as human readable script.</returns>
        public String Decompile(byte[] scriptBytes, int offset, String scriptString = null, String stringId = null, int row = 0, int col = 0, String colName = null)
        {
            if (!_havePropertiesFunctions && !_debug) throw new Exceptions.ScriptNotInitialisedException("Properties excel script functions have not been parsed; they must be loaded before any decompiling can be done.\nPlease ensure you have provided a valid FileManager for initialisation.");
            if (!_haveSkillsFunctions && !_debug) throw new Exceptions.ScriptNotInitialisedException("Skills excel script functions have not been parsed; they must be loaded before any decompiling can be done.\nPlease ensure you have provided a valid FileManager for initialisation.");

            return _script = _Decompile(scriptBytes, offset, offset, scriptBytes.Length, scriptString, stringId, row, col, colName).Value;
        }

        /// <summary>
        /// Decompiles an excel script function from byte codes to human readable text.
        /// </summary>
        /// <param name="excelScriptFunction">The excel script function to decompile.</param>
        /// <param name="excelFunction">The associated excel function to use for agrument reference.</param>
        /// <param name="scriptString">For debugging purposes only.</param>
        /// <param name="stringId">For debugging purposes only.</param>
        /// <param name="row">For debugging purposes only.</param>
        /// <param name="col">For debugging purposes only.</param>
        /// <param name="colName">For debugging purposes only.</param>
        /// <returns>Decompiled excel script byte codes as human readable script.</returns>
        private String Decompile(ExcelFile.ExcelFunction excelScriptFunction, Function excelFunction, String scriptString = null, String stringId = null, int row = 0, int col = 0, String colName = null)
        {
            if (excelScriptFunction == null) throw new ArgumentNullException("excelScriptFunction", "The excel script function cannot be null!");
            if (excelFunction == null) throw new ArgumentNullException("excelFunction", "The excel function cannot be null!");

            _excelScriptFunction = excelScriptFunction;
            _excelFunction = excelFunction;
            return _script = _Decompile(_excelScriptFunction.ScriptByteCode, 0, 0, _excelScriptFunction.ScriptByteCode.Length, scriptString, stringId, row, col, colName).Value;
        }

        /// <summary>
        /// Decompiles a script from byte codes to human readable text.
        /// </summary>
        /// <param name="scriptBytes">The excel script function to decompile.</param>
        /// <param name="offset">The starting offset within the byte code array.</param>
        /// <param name="startOffset">The original starting offset within the byte code array (applicable to Ternary operator parsing; set to offset).</param>
        /// <param name="maxBytes">The maximum number of bytes to parse.</param>
        /// <param name="scriptString">For debugging purposes only.</param>
        /// <param name="stringId">For debugging purposes only.</param>
        /// <param name="row">For debugging purposes only.</param>
        /// <param name="col">For debugging purposes only.</param>
        /// <param name="colName">For debugging purposes only.</param>
        /// <returns>A stack object with decompiled script and number of bytes read.</returns>
        private StackObject _Decompile(byte[] scriptBytes, int offset, int startOffset, int maxBytes, String scriptString = null, String stringId = null, int row = 0, int col = 0, String colName = null)
        {
            bool debug = (stringId != null) && _debug;
            bool debugShowParsed = false;
            bool debugOutputParsed = false;
            bool debugOutputFuncWithOpCode = false;
            bool debugScriptParsed = true;
            bool debugFormatConditionalByteCounts = false;
            bool debugOutputBytesRead = false;
            String debugPos = null;

            if (debug)
            {
                debugFormatConditionalByteCounts = true;
                debugOutputParsed = true;
                debugShowParsed = false;

                String rowName = String.Empty;
                if (_fileManager != null)
                {
                    int colIndex = 0;
                    switch (stringId)
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

                    rowName = _fileManager.GetExcelRowStringFromStringId(stringId, row, colIndex);
                }

                debugPos = String.Format("row({0}): '{1}', col({2}): '{3}', scriptBytes: {4}", row, rowName, col, colName, scriptString);
                if (debugShowParsed) Debug.WriteLine(debugPos);
            }

            int startStackCount = _stack.Count;
            int infCheck = 0;
            int scriptStartOffset = offset;
            uint bytesRead = 0;
            bool processStackOnReturn = true;
            try
            {
                while (offset < scriptStartOffset + maxBytes)
                {
                    ScriptOpCodes opCode = (ScriptOpCodes)FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                    String value1;
                    String value2;
                    uint index;
                    int functionIndex;
                    uint unknown;
                    uint byteOffset;
                    StackObject stackObject1;
                    StackObject stackObject2;
                    String argName;
                    int subMaxBytes;

                    if (row == 549 && col == 23)
                    {
                        //debugOutputBytesRead = true;
                        int bp = 0;
                    }

                    switch (opCode)
                    {
                        case ScriptOpCodes.Return:                   // 0    0x00
                            bytesRead += (uint)(offset - scriptStartOffset);
                            return _Return(startStackCount, bytesRead, processStackOnReturn, debugShowParsed);

                        case ScriptOpCodes.CallPropery:              // 2    0x02
                            functionIndex = FileTools.ByteArrayToInt32(scriptBytes, ref offset);
                            _CallFunction(functionIndex);
                            int nullByte = FileTools.ByteArrayToInt32(scriptBytes, ref offset);
                            Debug.Assert(nullByte == 0);
                            break;

                        case ScriptOpCodes.Call:                     // 3    0x03
                            functionIndex = FileTools.ByteArrayToInt32(scriptBytes, ref offset);
                            _CallFunction(functionIndex);
                            break;

                        case ScriptOpCodes.TernaryFalse:             // 4    0x04
                            _CheckStack(1, opCode);

                            int trueStatementCount = _stack.Count - startStackCount;
                            Debug.Assert(trueStatementCount > 0);

                            byteOffset = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                            subMaxBytes = (int)byteOffset - (offset - startOffset);
                            stackObject1 = _Decompile(scriptBytes, offset, startOffset, subMaxBytes, scriptString, stringId, row, col, colName);

                            Debug.Assert(stackObject1.StatementCount > 0);
                            stackObject1.FalseStatements = stackObject1.StatementCount;
                            stackObject1.TrueStatements = trueStatementCount;
                            processStackOnReturn = false;

                            _stack.Push(stackObject1);
                            offset += (int)stackObject1.ByteOffset;
                            break;

                        case ScriptOpCodes.AllocateVar:              // 6    0x06
                            byteOffset = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                            Debug.Assert((byteOffset % 4) == 0);

                            value1 = String.Format(" var{0} = ", byteOffset);
                            _stack.Push(new StackObject { Value = value1, ByteOffset = byteOffset });
                            break;

                        case ScriptOpCodes.TernaryTrue:              // 14    0x0E
                            _CheckStack(1, opCode);

                            byteOffset = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                            subMaxBytes = (int)byteOffset - (offset - startOffset);
                            stackObject1 = _Decompile(scriptBytes, offset, startOffset, subMaxBytes, scriptString, stringId, row, col, colName);
                            Debug.Assert(stackObject1.FalseStatements != -1 || stackObject1.TrueStatements != -1);

                            String ternaryTrueFormat;
                            String conditionsScript;
                            if (stackObject1.FalseStatements == -1)
                            {
                                if (stackObject1.TrueStatements == 1)
                                {
                                    const String onlyTrue1Rel = "if ({0}) {2};";
                                    const String onlyTrue1Debug = "if ({0})[{1}] {2};";
                                    ternaryTrueFormat = debugFormatConditionalByteCounts ? onlyTrue1Debug : onlyTrue1Rel;
                                }
                                else
                                {
                                    const String onlyTrueRel = "if ({0})\n{{\n{2}\n}}";
                                    const String onlyTrueDebug = "if ({0})[{1}]\n{{\n{2}\n}}";
                                    ternaryTrueFormat = debugFormatConditionalByteCounts ? onlyTrueDebug : onlyTrueRel;
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
                                    ternaryTrueFormat = debugFormatConditionalByteCounts ? true1False1Debug : true1False1Rel;
                                }
                                else
                                {
                                    const String trueFalseRel = "if ({0})\n{{\n{2}}}\nelse\n{{\n{3}}}";
                                    const String trueFalseDebug = "if ({0})[{1}]\n{{\n{2}}}\nelse\n{{\n{3}}}";
                                    ternaryTrueFormat = debugFormatConditionalByteCounts ? trueFalseDebug : trueFalseRel;

                                    String[] code = trueObj.Value.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                                    String codeStr = code.Aggregate(String.Empty, (current, line) => current + ("\t" + line + ";\n"));
                                    trueObj.Value = codeStr;

                                    code = falseObj.Value.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                                    codeStr = code.Aggregate(String.Empty, (current, line) => current + ("\t" + line + ";\n"));
                                    falseObj.Value = codeStr;
                                }

                                conditionsScript = String.Format(ternaryTrueFormat, ifObj.Value, byteOffset, trueObj.Value, falseObj.Value);
                            }


                            _stack.Push(new StackObject { Value = conditionsScript, IsIf = true });
                            offset += (int)stackObject1.ByteOffset;
                            break;

                        case ScriptOpCodes.Push:                     // 26   0x1A
                            int value = FileTools.ByteArrayToInt32(scriptBytes, ref offset);
                            _stack.Push(new StackObject { Value = value.ToString() });
                            break;

                        case ScriptOpCodes.PushLocalVarInt32:        // 50   0x32
                            byteOffset = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                            _PushLocalVar(byteOffset, ArgType.Int32);
                            break;

                        case ScriptOpCodes.PushLocalVarPtr:          // 57   0x39
                            byteOffset = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                            _PushLocalVar(byteOffset, ArgType.Ptr);
                            break;

                        case ScriptOpCodes.AssignLocalVarInt32:      // 98   0x62
                            _CheckStack(2, opCode);

                            stackObject2 = _stack.Pop();
                            stackObject1 = _stack.Pop();

                            index = stackObject1.ByteOffset / 4;
                            Debug.Assert(index >= 0 && index < _vars.Length);
                            Debug.Assert(stackObject2.Type == ArgType.Int32);

                            _vars[index] = stackObject2;
                            value1 = String.Format("int{0}{1}", stackObject1.Value, stackObject2.Value);
                            _stack.Push(new StackObject { Value = value1, IsVarAssign = true, Type = ArgType.Int32 });
                            break;

                        case ScriptOpCodes.AssignLocalVarPtr:        // 105   0x69
                            _CheckStack(2, opCode);

                            stackObject2 = _stack.Pop();
                            stackObject1 = _stack.Pop();

                            index = stackObject1.ByteOffset / 4;
                            Debug.Assert(index >= 0 && index < _vars.Length);
                            Debug.Assert(stackObject2.Type == ArgType.Ptr || stackObject2.Type == ArgType.ContextPtr);

                            _vars[index] = stackObject2;
                            value1 = String.Format("void*{0}{1}", stackObject1.Value, stackObject2.Value);
                            _stack.Push(new StackObject { Value = value1, IsVarAssign = true, Type = ArgType.Ptr });
                            break;

                        case ScriptOpCodes.Complement:               // 320  0x140
                            _CheckStack(1, opCode);

                            StackObject toComplementObject = _stack.Pop();
                            if (toComplementObject.Value[0] == '-') // if already negative, remove sign
                            {
                                toComplementObject.Value = toComplementObject.Value.Substring(1, toComplementObject.Value.Length - 1);
                            }
                            else
                            {
                                toComplementObject.Value = "-" + toComplementObject.Value;
                            }

                            _stack.Push(toComplementObject);
                            break;

                        case ScriptOpCodes.Not:                      // 339  0x153
                            _CheckStack(1, opCode);

                            _stack.Push(new StackObject { Value = String.Format("!{0}", _stack.Pop().Value) });
                            break;

                        case ScriptOpCodes.Pow:                      // 347  0x15B
                            _DoOperator("^", 4, opCode);
                            break;

                        case ScriptOpCodes.Mult:                     // 358  0x166
                            _DoOperator(" * ", 5, opCode);
                            break;

                        case ScriptOpCodes.Div:                      // 369  0x171
                            _DoOperator(" / ", 5, opCode);
                            break;

                        case ScriptOpCodes.Add:                      // 388  0x184
                            _DoOperator(" + ", 6, opCode);
                            break;

                        case ScriptOpCodes.Sub:                      // 399  0x18F
                            _DoOperator(" - ", 6, opCode);
                            break;

                        case ScriptOpCodes.LessThan:                 // 426  0x1AA
                            _DoOperator(" < ", 8, opCode);
                            break;

                        case ScriptOpCodes.GreaterThan:              // 437  0x1B5
                            _DoOperator(" > ", 8, opCode);
                            break;

                        case ScriptOpCodes.LessThanOrEqual:          // 448  0x1C0
                            _DoOperator(" <= ", 8, opCode);
                            break;

                        case ScriptOpCodes.GreaterThanOrEqual:       // 459  0x1CB
                            _DoOperator(" >= ", 8, opCode);
                            break;

                        case ScriptOpCodes.EqualTo:                  // 470  0x1D6
                            _DoOperator(" == ", 9, opCode);
                            break;

                        case ScriptOpCodes.NotEqualTo:               // 481  0x1E1
                            _DoOperator(" != ", 9, opCode);
                            break;

                        case ScriptOpCodes.And:                      // 516  0x204
                            _CheckStack(1, opCode);

                            byteOffset = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                            Debug.Assert(byteOffset % 4 == 0 && FileTools.ByteArrayToUInt32(scriptBytes, startOffset + (int)byteOffset - 4) == (uint)ScriptOpCodes.EndIf);

                            stackObject1 = _stack.Pop();
                            stackObject1.OpCode = opCode;
                            _stack.Push(stackObject1);

                            subMaxBytes = (int)byteOffset - (offset - startOffset);
                            stackObject1 = _Decompile(scriptBytes, offset, startOffset, subMaxBytes, scriptString, stringId, row, col, colName);


                            const String andFormatRelease = "{0}";
                            const String andFormatDebug = "{0}[{1}]";
                            String andOutputFormat = debugFormatConditionalByteCounts ? andFormatDebug : andFormatRelease;

                            _stack.Push(new StackObject { Value = String.Format(andOutputFormat, _stack.Pop().Value, byteOffset), OpCode = opCode });
                            offset += (int)stackObject1.ByteOffset;
                            debugOutputFuncWithOpCode = true;
                            break;

                        case ScriptOpCodes.Or:                       // 527  0x20F
                            _CheckStack(1, opCode);

                            byteOffset = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                            Debug.Assert(byteOffset % 4 == 0 && FileTools.ByteArrayToUInt32(scriptBytes, startOffset + (int)byteOffset - 4) == (uint)ScriptOpCodes.EndIf);

                            stackObject1 = _stack.Pop();
                            stackObject1.OpCode = opCode;
                            _stack.Push(stackObject1);

                            subMaxBytes = (int)byteOffset - (offset - startOffset);
                            stackObject1 = _Decompile(scriptBytes, offset, startOffset, subMaxBytes, scriptString, stringId, row, col, colName);

                            const String orFormatRelease = "{0}";
                            const String orFormatDebug = "{0}[{1}]";
                            String orOuputFormat = debugFormatConditionalByteCounts ? orFormatDebug : orFormatRelease;

                            _stack.Push(new StackObject { Value = String.Format(orOuputFormat, _stack.Pop().Value, byteOffset), OpCode = opCode });
                            offset += (int)stackObject1.ByteOffset;
                            debugOutputFuncWithOpCode = true;
                            break;

                        case ScriptOpCodes.EndIf:                    // 538  0x21A
                            _CheckStack(2, opCode);

                            stackObject2 = _stack.Pop();
                            stackObject1 = _stack.Pop();

                            const String orFormat = "{0} || {1}";
                            const String andFormat = "{0} && {1}";
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
                            }

                            _stack.Push(new StackObject { Value = String.Format(endIfFormat, stackObject1.Value, stackObject2.Value), OpCode = stackObject1.OpCode });
                            break;

                        case ScriptOpCodes.GetStat666:               // 666  0x29A
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                            _StatsFunction("GetStat666", index, (uint)ExcelTableCodes.Stats, opCode, false);
                            break;

                        case ScriptOpCodes.GetStat667:               // 667  0x29B
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                            _StatsFunction("GetStat667", index, (uint)ExcelTableCodes.Stats, opCode, false);
                            break;

                        case ScriptOpCodes.SetStat669:               // 669  0x29D
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                            _StatsFunction("SetStat669", index, (uint)ExcelTableCodes.Stats, opCode, true);
                            break;

                        case ScriptOpCodes.SetStat673:               // 673  0x2A1
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                            _StatsFunction("SetStat673", index, (uint)ExcelTableCodes.Stats, opCode, true);
                            break;

                        case ScriptOpCodes.SetStat674:               // 674  0x2A2
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                            _StatsFunction("SetStat674", index, (uint)ExcelTableCodes.Stats, opCode, true);
                            break;

                        case ScriptOpCodes.GetStat680:               // 680  0x2A8
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                            _StatsFunction("GetStat680", index, (uint)ExcelTableCodes.Stats, opCode, false);
                            break;

                        case ScriptOpCodes.SetStat683:               // 683  0x2AB
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                            _StatsFunction("SetStat683", index, (uint)ExcelTableCodes.Stats, opCode, true);
                            break;

                        case ScriptOpCodes.SetStat687:               // 687  0x2AF
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                            _StatsFunction("SetStat687", index, (uint)ExcelTableCodes.Stats, opCode, true);
                            break;

                        case ScriptOpCodes.SetStat688:               // 688  0x2B0
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                            _StatsFunction("SetStat688", index, (uint)ExcelTableCodes.Stats, opCode, true);
                            break;

                        case ScriptOpCodes.PushContextVarInt32:      // 700 0x2BC
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);

                            value1 = String.Format("${0}", ((ContextVariables)index).ToString().ToLower());
                            _stack.Push(new StackObject { Value = value1, IsFunction = true, Type = ArgType.Int32 });
                            break;

                        case ScriptOpCodes.PushContextVarUInt32:     // 701 0x2BD
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                            _PushContextVariable("PushContextVarUInt32", index);
                            break;

                        case ScriptOpCodes.PushContextVarInt64:      // 702 0x2BE
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                            _PushContextVariable("PushContextVarInt64", index);
                            break;

                        case ScriptOpCodes.PushContextVarUInt64:     // 703 0x2BF
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                            _PushContextVariable("PushContextVarUInt64", index);
                            break;

                        case ScriptOpCodes.PushContextVarFloat:      // 704 0x2C0
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                            _PushContextVariable("PushContextVarFloat", index);
                            break;

                        case ScriptOpCodes.PushContextVarDouble:     // 705 0x2C1
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                            _PushContextVariable("PushContextVarDouble", index);
                            break;

                        case ScriptOpCodes.PushContextVarDouble2:    // 706 0x2C2
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                            _PushContextVariable("PushContextVarDouble2", index);
                            break;

                        case ScriptOpCodes.PushContextVarPtr:        // 707 0x2C3
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);

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

                        case ScriptOpCodes.GlobalVarStats:           // 712  0x2C8
                            _stack.Push(new StackObject { Value = "@stats", Type = ArgType.StatsList });
                            break;

                        case ScriptOpCodes.AssignContextVar:         // 713  0x2C9
                            _CheckStack(2, opCode);

                            stackObject2 = _stack.Pop();
                            stackObject1 = _stack.Pop();
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);

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
                    String debugOutputPath = String.Format("{0}{1}_scriptdebug.txt", DebugRoot, stringId);
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
                    //String[] code = _script.Split(new[] { "; ", ";" }, StringSplitOptions.RemoveEmptyEntries);
                    //String codeStr = code.Aggregate(String.Empty, (current, line) => current + ("\t" + line + ";\n"));

                    String debugOutputPath = String.Format("{0}{1}_scriptdebug.txt", DebugRoot, stringId);
                    String debugOutput = String.Format("{0}\n{1}\n\n", debugPos, _script);
                    File.AppendAllText(debugOutputPath, debugOutput);
                }
            }

            bytesRead += (uint)(offset - scriptStartOffset);
            if (debugOutputBytesRead) Console.WriteLine("Read from {0} to {1} = {2} bytes.", scriptStartOffset, offset, scriptStartOffset - offset);
            return _Return(startStackCount, bytesRead, processStackOnReturn, debugShowParsed);
        }
    }
}
