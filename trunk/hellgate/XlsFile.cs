using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Hellgate.Excel.SinglePlayer;

namespace Hellgate
{
    public class XlsFile
    {
        #region From Client Generation Functions

        private enum XlsType
        {
            None = 0,
            String = 1,
            StringOffset = 2, // also StringOffsetDefault
            GroupStyle = 3,
            Flag = 4,
            Byte06 = 6,
            Int32 = 9,
            Bool = 11,
            Float = 12, // Float1 and Float2
            Byte0D = 13,
            CodeInt32 = 14,
            CodeInt16 = 16,
            CodeInt8 = 17,
            ExcelIndex = 20,
            FolderCode = 21,
            Int32Default = 25,
            AiInit = 26,
            Enum = 29,
            EnumArray = 30,
            Int32Array = 35,
            UseFixedDRLGSeed = 36,
            FloatArray = 37,
            ExcelIndexArray = 40,
            Qualities = 45,
            StringIndex = 46,
            TugboatUnknown = 47,
            File = 48,
            Script = 50,
            MaxSlots = 51,
            Unknown1 = 52,
            AimHeight = 53,
            UnitType = 54,
            MultipleRelations = 55,
            BaseRow = 56
        }

        private enum XlsFlags
        {
            Sort = (1 << 0),            // 1            // is a sort column
            ExcelIndex = (1 << 1),      // 2
            BaseRow = (1 << 2),         // 4

            Enum = (1 << 4),            // 16

            StringIndex2 = (1 << 6),    // 64           // seen in XLS_StringIndex, when last arg == 2
            StringIndex3 = (1 << 7),    // 128          // seen in XLS_StringIndex, when last arg == 3
            HasNone = (1 << 8),         // 256          // flagged when the enum type has a "none" vale

            //UnknownFlag = (1 << 10),    // 1024         // seen in SOUND_GROUP and WARDROBE_APPEARANCE_GROUP; unknown usage (something relating to null/empty/ignore field? (random guess))
        }

        /* As defined in Client
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private class XlsColumn
        {
            public String Name;             // 00 name
            public XlsType Type;            // 04 type
            public int Offset;              // 08 row offset
            public int Count;               // 0C number of elements
            public int Size;                // 10 byte size of element
            public int ElementIndex;        // 14 the order that the element was defined during initial definition stages
            public XlsFlags Flags;          // 18 type flags
            public int Field1C;             // 1C
            public Object Default;          // 20 default value
            public int Field24;             // 24
            public int Field28;             // 28
            public int Field2C;             // 2C
            public Object Index;            // 30 excel index or flag index depending on XlsType
            public Object IndexUnknown;     // 34 this has something to do with index stuff... Used in strings and excel index types and more
            public int ExcelDefaultQ;       // 38 I think this is the default excel index...
            public int Field3C;             // 3C
            public int Field40;             // 40
            public int Field44;             // 44
        }
         */

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private class XlsColumn
        {
            public String Name;             // 00 name
            public XlsType Type;            // 04 type
            public int Offset;              // 08 row offset
            public int Count;               // 0C number of elements
            public int Size;                // 10 byte size of element
            public int ElementIndex;        // 14 the order that the element was defined during initial definition stages
            public XlsFlags Flags;          // 18 type flags

            public Object Default;          // 20 default value

            public int UnknownIndex;        // 30 unknown index used for types 51 (Unknown1) and 52 (AimHeight)

            public uint FlagIndex;          // 30 flag from start byte offset (i.e. can be > 31)

            public bool StringBool;         // 30 unknown usage - appears to be a bool (only seen as 0 and 1), used for strings types 1 (String), 2 (StringOffset)
            public int String64Offset;      // 34 offset for x64 client usage, usage for strings types as above

            public float FloatMin;          // 30 these are used for type 12 (Float), but more specifically, for XLS_Percent [0.0f, 100.0f], otherwise always zero
            public float FloatMax;          // 34

            public int ExcelIndex;          // 30 the excel table index, must be less than 0x200; used for types 20 (ExcelIndex), 21 (FolderCode), 26 (AiInit), 40 (ExcelIndexArray), 45 (Qualities)
            public int ExcelFlags;          // 34 unknown excel flags (pretty sure they're flags), must be < 4, and only seen as 0, 1, 3; used for types as above
            public int ExcelUnknown;        // 38 unknown (index-like?) values, used only for type 26 (AiInit)

            public String FilePath;         // 30 used for type 48 (File)
        }

        private class XlsTable
        {
            public String DefinitionId;             // the id used internally (e.g. "EXCEL_TABLE_DEFINITION" for "EXCELTABLES")
            public int RowSize;                     // number of bytes per row
            public List<XlsColumn> Columns = new List<XlsColumn>();
            public int ColumnCount { get { return Columns.Count; } }
            public XlsColumn StringIndexColumn;
            public XlsColumn RelationalColumn;
            // 00000018 tableDefUnknownStruct XLS_TableDefUnknownStruct 4 dup(?)
            // 00000098 field_98        dd ?
            // 0000009C field_9C        dd ?
            // 000000A0 field_A0        dd ?
            public int FieldA4;                     // A4 not sure, but seen in UNITMODE_DATA and was being set to 320 (which happened to be the count of the ExcelIndexArrays...) (also found in ITEM_LOOK_GROUP_DATA which has no ExcelIndexArrays however)
            public bool HasStringIndex;             // A8 set in XLS_StringIndex and XLS_TugboatUnknown
            public bool HasUnknown;                 // AC set in XLS_MaxSlots and XLS_Unknown1 (seen only in UNIT_DATA)
            public bool HasScripts;                 // B0 set in XLS_Script
            public bool HasBaseRow;                 // B4 set in XLS_BaseRow
            // 000000B8 field_B8        dd ?
            // 000000BC field_BC        dd ?
            // 000000C0 field_C0        dd ?
            // 000000C4 field_C4        dd ?
            public int FieldC8;                     // C8 not sure; seen only in SOUND_GROUP (== 1)
            public String FuncCC;                   // CC these down to FuncE0 appear to be similar to "OnInit", and "Deconstructor" type function calls
            public String FuncD0;                   // D0
            public String FuncD4;                   // D4
            public String FuncD8;                   // D8
            public String FuncDC;                   // DC seen only in LEVEL_DEFINITION
            public String FuncE0;                   // E0 this one is run when you quit (i.e. a deconstructor type function)
            public uint TableHash;                  // E4 the unique hash identifier of the table and its elements

            public String FileName;
            public int TableIndex;
            public int Unknown1;
            public int Unknown2;
        }

        private static byte[] _exeBytes;
        private static readonly Regex AddressRegex = new Regex(@"(?:off|dword|byte|word)_(\w*)");
        private static readonly Regex ArgsRegex = new Regex(@"(?:\(.*\))*(\w*)\((?:\(\w* \*\))*(.*)\)");
        private static readonly Regex StringRegex = new Regex("\"(.*)\"");
        private static readonly Regex PointerRegex = new Regex(@"->(\w*) = (?:\(.*\))?(\w*)");
        private const int ExeBase = 0x400000;

        public static void GenerateExcelTables()
        {
            // load and generate tables //
            const String singlePlayerSource = @"C:\hellgate_sp_unpacked2.c";
            const String singlePlayerExe = @"D:\Games\Hellgate London\SP_x86\UnPacked2.exe";
            const String singlePlayerInstall = @"D:\Games\Hellgate London";
            FileManager singlePlayerFiles = new FileManager(singlePlayerInstall);

            const String testCenterSource = @"C:\hellgate_mp_dx9_x86_tc.unpacked.c";
            const String testCenterExe = @"D:\Games\Hellgate London\MP_x86\hellgate_mp_dx9_x86.unpacked.exe";
            const String testCenterInstall = singlePlayerInstall;
            FileManager testCenterFiles = new FileManager(testCenterInstall, FileManager.ClientVersions.TestCenter);

            const String resurrectionSource = @"C:\hellgate_res.c";
            const String resurrectionExe = @"D:\Games\Hellgate\MP_x86\hellgate.exe";
            const String resurrectionInstall = @"D:\Games\Hellgate";
            FileManager resurrectionFiles = new FileManager(resurrectionInstall, FileManager.ClientVersions.Resurrection);

            const bool outputLinesParsed = false;
            List<XlsTable> singlePlayerTables = _GenerateExcelTables(File.ReadAllLines(singlePlayerSource), File.ReadAllBytes(singlePlayerExe), singlePlayerFiles, outputLinesParsed);
            List<XlsTable> testCenterTables = _GenerateExcelTables(File.ReadAllLines(testCenterSource), File.ReadAllBytes(testCenterExe), testCenterFiles, outputLinesParsed);
            List<XlsTable> resurrectionTables = _GenerateExcelTables(File.ReadAllLines(resurrectionSource), File.ReadAllBytes(resurrectionExe), resurrectionFiles, outputLinesParsed);
            Debug.WriteLine("Table Counts: SinglePlayer = {0}, TestCenter = {1}, Resurrection = {2}", singlePlayerTables.Count, testCenterTables.Count, resurrectionTables.Count);


            // output table class definitions //
            const String classOutputDir = @"D:\Projects\Hellgate London\Reanimator\trunk\hellgate\Xls";
            Directory.CreateDirectory(classOutputDir);

            List<XlsTable> usedTables = new List<XlsTable>();
            usedTables = _OutputTableClassDefintions(singlePlayerFiles, classOutputDir, singlePlayerTables, usedTables);
            usedTables = _OutputTableClassDefintions(testCenterFiles, classOutputDir, testCenterTables, usedTables);
            usedTables = _OutputTableClassDefintions(resurrectionFiles, classOutputDir, resurrectionTables, usedTables);

        }

        private static List<XlsTable> _OutputTableClassDefintions(FileManager fileManager, String outputDir, List<XlsTable> xlsTables, List<XlsTable> xlsDefined)
        {
            fileManager.BeginAllDatReadAccess();
            fileManager.LoadTableFiles(true);
            fileManager.EndAllDatAccess();

            ExcelFile excelTables = fileManager.GetExcelTableFromCode(Xls.TableCodes.EXCELTABLES);
            Debug.Assert(excelTables != null);

            outputDir = Path.Combine(outputDir, fileManager.ClientVersion.ToString());
            Directory.CreateDirectory(outputDir);

            TextInfo textInfo = Common.EnglishUSCulture.TextInfo;

            foreach (XlsTable xlsTable in xlsTables)
            {
                // do we have a file for it
                String fileName = xlsTable.FileName + ".txt.cooked";
                PackFileEntry packFile = fileManager.GetFirstFileEntryFromName(fileName);
                if (packFile == null)
                {
                    Debug.WriteLine("Warning: Table file not found {0}", fileName);
                    continue;
                }

                // get the string id
                ExcelTablesRow excelRow = (ExcelTablesRow)excelTables.Rows[xlsTable.TableIndex];
                String stringId = excelRow.name;
                String className = textInfo.ToTitleCase(stringId.ToLower());
                className = Regex.Replace(className, @"[^A-Za-z0-9]", "");
                String filePath = Path.Combine(outputDir, className + ".cs");

                if (stringId == "SOUNDS")
                {
                    int bp = 0;
                }

                Dictionary<String, XlsColumn> varNames = new Dictionary<String, XlsColumn>();
                int currOffset = 0;
                StringBuilder vars = new StringBuilder();
                StringBuilder enums = new StringBuilder();
                List<XlsColumn> orderedColumns = xlsTable.Columns.OrderBy(xlsElement => xlsElement.Offset).ToList();
                int columnCount = orderedColumns.Count;
                for (int i = 0; i < columnCount; i++)
                {
                    XlsColumn xlsColumn = orderedColumns[i];

                    // unknowns
                    if (xlsColumn.Type == XlsType.AiInit || // 26
                        xlsColumn.Type == XlsType.Unknown1 || // 52
                        xlsColumn.Type == XlsType.AimHeight || // 53
                        xlsColumn.Type == XlsType.MaxSlots) continue; // 51

                    int colOffset = (xlsColumn.Type == XlsType.StringOffset) ? xlsColumn.String64Offset : xlsColumn.Offset; // StringOffset types are (can be) sizeof(Int64 Ptr) for x64 client usage
                    int gap = colOffset - currOffset;
                    Debug.Assert(gap >= 0);
                    while (gap > 0) // we've got gaps
                    {
                        String gapType;
                        int varSize;
                        switch (gap)
                        {
                            case 1:
                                gapType = "byte";
                                varSize = 1;
                                break;

                            case 2:
                                gapType = "short";
                                varSize = 2;
                                break;

                            default:
                                gapType = "Int32";
                                varSize = 4;
                                break;
                        }

                        String gapName = String.Format("_undefined{0:X2}", currOffset);
                        vars.Append(String.Format("        private {0} {1};\n", gapType, gapName));
                        currOffset += varSize;
                        gap -= varSize;
                    }

                    String varName = textInfo.ToTitleCase(xlsColumn.Name.ToLower());
                    varName = Regex.Replace(varName, @"[^A-Za-z0-9]", "");

                    //if (String.IsNullOrEmpty(varName)) varName = "noname" + currOffset;

                    Debug.Assert(!varNames.ContainsKey(varName));

                    int columnByteCount = (xlsColumn.Size * xlsColumn.Count);
                    String varType;
                    switch (xlsColumn.Type)
                    {
                        case XlsType.String: // 1
                        case XlsType.File: // 48
                            varType = "String";
                            break;

                        case XlsType.StringOffset: // 2
                            varType = "Int64";
                            columnByteCount = 8;
                            break;

                        case XlsType.GroupStyle: // 3
                        case XlsType.Int32: // 9
                        case XlsType.Bool: // 11
                        case XlsType.CodeInt32: // 17
                        case XlsType.ExcelIndex: // 20
                        case XlsType.FolderCode: // 21
                        case XlsType.Int32Default: // 25
                        case XlsType.Qualities: // 45
                        case XlsType.StringIndex: // 46
                        case XlsType.Script: // 50
                        case XlsType.UnitType: // 54
                            varType = "Int32";
                            break;

                        case XlsType.Flag: // 4
                            // need to get all flags at this offset range, and get highest flag value to determine how many Int32's we need
                            int highestFlagIndex = 0;
                            List<XlsColumn> xlsFlags = new List<XlsColumn>();
                            for (int j = i; j < columnCount && orderedColumns[j].Offset == currOffset; j++)
                            {
                                xlsFlags.Add(orderedColumns[j]);
                                if ((Int32)orderedColumns[j].FlagIndex < highestFlagIndex) highestFlagIndex = (Int32)orderedColumns[j].FlagIndex;
                            }

                            // the Flags attribute can at most be 32 bits, so we need to split the flags into Int32 segments
                            // the bytes-used in the client is also done in Int32 chunks, so needed when determined end offset as well
                            const String enumDefinition = @"

    [Flags]
    public enum {0}
    {{
{1}    }}";
                            int flagCount = xlsFlags.Count;
                            int int32Count = (highestFlagIndex >> 5) + 1;
                            int flagsCompleted = 0;
                            xlsFlags = xlsFlags.OrderBy(column => column.FlagIndex).ToList();
                            int lastInt32Index = 0;
                            int lastFlagIndex = -1;
                            String flagName = String.Empty;
                            StringBuilder enumValues = new StringBuilder();
                            for (int f = 0; f < flagCount; f++)
                            {
                                int flagIndex = (int)xlsFlags[f].FlagIndex;
                                int flagInt32Index = flagIndex % 32;
                                int int32Index = (flagIndex >> 5);
                                bool haveFlagsRemaining = (f != flagCount - 1);
                                varName = textInfo.ToTitleCase(xlsFlags[f].Name.ToLower());
                                varName = Regex.Replace(varName, @"[^A-Za-z0-9]", "");
                                if (varName == String.Empty)
                                {
                                    varName = String.Format("Unnamed{0:X2}", flagIndex);
                                }


                                // fill in any gaps
                                String enumValue;
                                while (lastFlagIndex != flagIndex - 1)
                                {
                                    enumValue = String.Format("        Undefined{0:X2} = (1 << {0}) // {1}\n", ++lastFlagIndex, (1 << lastFlagIndex));
                                    enumValues.Append(enumValue);
                                }

                                // add enum value
                                enumValue = String.Format("        {0} = (1 << {1}) // {2}\n", varName, flagInt32Index, (1 << flagInt32Index));
                                enumValues.Append(enumValue);
                                lastFlagIndex++;

                                // we're at the last flag, or a new flag Int32 chunk, so output old enum, and start new enum
                                if (int32Index != lastInt32Index || !haveFlagsRemaining)
                                {
                                    int flagsCount = 0;
                                    while (varNames.ContainsKey(flagName = "Flags" + flagsCount++)) { }

                                    if (lastInt32Index >= 0)
                                    {
                                        String enumDeclaration = String.Format(enumDefinition, flagName, enumValues);
                                        enums.Append(enumDeclaration);
                                        enumValues.Clear();
                                    }

                                    varNames.Add(flagName, xlsColumn);
                                    vars.Append(String.Format("        public {0} {1};\n", flagName, flagName));

                                    lastInt32Index = int32Index;
                                }


                            }

                            currOffset += int32Count * 4;
                            i += flagCount;
                            continue;

                        case XlsType.Byte06: // 6
                        case XlsType.Byte0D: // 13
                        case XlsType.CodeInt8: // 17
                            varType = "byte";
                            break;

                        case XlsType.Float: // 12
                            varType = "float";
                            break;

                        case XlsType.CodeInt16: // 16
                            varType = "Int16";
                            break;

                        case XlsType.Enum: // 29
                            varType = varName;
                            break;

                        case XlsType.EnumArray: // 30
                            varType = varName + "[]";
                            break;

                        case XlsType.Int32Array: // 35
                        case XlsType.ExcelIndexArray: // 40
                            varType = "Int32[]";
                            break;

                        case XlsType.MultipleRelations: // 55
                            columnByteCount = xlsColumn.Size;

                            if (stringId == "SOUNDS") varType = "FileName";
                            else if (stringId == "MUSICCONDITIONS") varType = "Evaluate";
                            else if (stringId == "TREASURE") varType = "Item";
                            else if (stringId == "SPAWN_CLASS") varType = "Pick";
                            else
                            {
                                throw new NotImplementedException(xlsColumn.Type + " type not implemented!");
                            }
                            break;

                        case XlsType.BaseRow: // 56
                            columnByteCount = 4;
                            varType = "Int32";
                            break;

                        default:
                            throw new NotImplementedException(xlsColumn.Type + " type not implemented!");
                    }

                    if (varName == String.Empty) varName = String.Format("Unnamed{0:X2}", xlsColumn.Offset);

                    varNames.Add(varName, xlsColumn);
                    vars.Append(String.Format("        public {0} {1};\n", varType, varName));

                    Debug.Assert(columnByteCount > 0);

                    currOffset += columnByteCount;
                }

                const String baseFile =
@"using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Xls.{0}
{{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class {1} // {4} (0x{4:X}) bytes
    {{
        private RowHeader header;
{2}    }}{3}
}}";
                xlsDefined.Add(xlsTable);
                String classFile = String.Format(baseFile, fileManager.ClientVersion, className, vars, enums, xlsTable.RowSize);
                File.WriteAllText(filePath, classFile);
            }

            return xlsDefined;
        }

        private static List<XlsTable> _GenerateExcelTables(String[] source, byte[] exeBytes, FileManager fileManager, bool outputLinesParsed)
        {
            _exeBytes = exeBytes;
            Dictionary<String, String> functionNames = new Dictionary<String, String>
            {
                {"sub_6B8751", "XLS_ExcelScript"},
                {"sub_6B8768", "XLS_AssignStringIndex"},
                {"sub_6B878B", "XLS_RelationalElement"}
            };

            List<XlsTable> tables = new List<XlsTable>();

            int lineCount = source.Length;
            for (int i = 0; i < lineCount; i++)
            {
                if (!source[i].Contains("XLS_Key") || source[i].StartsWith("//")) continue;
                if (source[i].Contains("XLS_Key<")) continue; // the function definition
                if (source[i].Contains("XLS_KeyUnknownMemMove")) continue; // just another function
                if (outputLinesParsed) Debug.Write("\n");

                XlsTable xlsTable = new XlsTable();
                bool inTableDefinition = true;
                i--;
                while (inTableDefinition && i++ < lineCount)
                {
                    Match match = ArgsRegex.Match(source[i]);
                    String func = match.Groups[1].Value;
                    String[] args = match.Groups[2].Value.Split(new[] { ", " }, StringSplitOptions.None);

                    if (String.IsNullOrEmpty(func))
                    {
                        Match pMatch = PointerRegex.Match(source[i]); // mostly a curiosity thing
                        String varName = pMatch.Groups[1].Value;
                        String funcName = pMatch.Groups[2].Value;

                        switch (varName)
                        {
                            case "field_A4":
                                xlsTable.FieldA4 = Int32.Parse(funcName);
                                break;

                            case "field_C8":
                                xlsTable.FieldC8 = Int32.Parse(funcName);
                                break;

                            case "pFunc_CC":
                                xlsTable.FuncCC = funcName;
                                break;

                            case "pFunc_D0":
                                xlsTable.FuncD0 = funcName;
                                break;

                            case "pFunc_D4":
                                xlsTable.FuncD4 = funcName;
                                break;

                            case "pFunc_D8":
                                xlsTable.FuncD8 = funcName;
                                break;

                            case "pFunc_DC":
                                xlsTable.FuncDC = funcName;
                                break;

                            case "pFunc_E0":
                                xlsTable.FuncE0 = funcName;
                                break;

                            default:
                                throw new Exceptions.UnexpectedErrorException(varName + " not implemented in switch (varName).");
                        }

                        if (outputLinesParsed) Debug.WriteLine("xlsTable.{0} = {1}", varName, funcName);
                        continue;
                    }

                    if (outputLinesParsed) Debug.WriteLine(match);
                    Debug.Assert(!String.IsNullOrEmpty(func));

                    String replaceName;
                    if (functionNames.TryGetValue(func, out replaceName))
                    {
                        func = replaceName;
                    }

                    XlsColumn xlsElement = null;
                    switch (func)
                    {
                        case "XLS_Key": // XLS_TableDefinition *__usercall XLS_Key<eax>(char *szStringId<eax>, int rowSize)
                            Debug.Assert(args.Length == 2);
                            xlsTable.DefinitionId = _GetArg<string>(args, 0);
                            xlsTable.RowSize = _GetArg<int>(args, 1);

                            Debug.Assert(!String.IsNullOrEmpty(xlsTable.DefinitionId));
                            Debug.Assert(xlsTable.RowSize > 0);

                            while (!source[++i].Contains("{")) { } // skip until we're in the table definition if-statement

                            break;

                        case "XLS_String": // 1 // void __cdecl XLS_String(XLS_TableDefinition *pTableDefinition, char *szName, int offset, int charCount, int stringBool)
                            Debug.Assert(args.Length == 5);
                            xlsElement = new XlsColumn
                            {
                                Name = _GetArg<string>(args, 1),
                                Offset = _GetArg<int>(args, 2),
                                Type = XlsType.String,
                                Count = _GetArg<int>(args, 3),
                                Size = 1,
                                Default = null,
                            };

                            // applicable only to types 1 (String) and 2 (StringOffset/StringOffsetDefault)
                            // XLS_ElementDefinition *__usercall XLS_Def_SetDefinitionDefault_String<eax>(XLS_ElementDefinition *pElement<eax>, int bUnknown, int x64Offset)
                            _XlsSetDefStringStuff(xlsElement, _GetArg<int>(args, 4), -1); // XLS_Def_SetDefinitionDefault_String(&element, stringBool, -1);
                            break;

                        case "XLS_StringOffset": // 2 // void __usercall XLS_StringOffset(XLS_TableDefinition *pTable, char *szName, int offset<esi>, int stringBool)
                            Debug.Assert(args.Length == 4);
                            xlsElement = new XlsColumn
                            {
                                Name = _GetArg<string>(args, 1),
                                Offset = _GetArg<int>(args, 2),
                                Type = XlsType.StringOffset,
                                Count = 1,
                                Size = 4,
                                Default = null
                            };

                            // applicable only to types 1 (String) and 2 (StringOffset/StringOffsetDefault)
                            // XLS_ElementDefinition *__usercall XLS_Def_SetDefinitionDefault_String<eax>(XLS_ElementDefinition *pElement<eax>, int bUnknown, int x64Offset)
                            _XlsSetDefStringStuff(xlsElement, _GetArg<int>(args, 3), _GetArg<int>(args, 2)); // XLS_Def_SetDefinitionDefault_String(&element, unknownBool, offset);
                            break;

                        case "XLS_StringOffsetDefault": // 2 // void __cdecl XLS_StringOffsetDefault(XLS_TableDefinition *pTable, char *szName, int x86Offset, int x64Offset)
                            Debug.Assert(args.Length == 4);
                            xlsElement = new XlsColumn
                            {
                                Name = _GetArg<string>(args, 1),
                                Offset = _GetArg<int>(args, 2),
                                Type = XlsType.StringOffset,
                                Count = 1,
                                Size = 4,
                                Default = null
                            };

                            // applicable only to types 1 (String) and 2 (StringOffset/StringOffsetDefault)
                            // XLS_ElementDefinition *__usercall XLS_Def_SetDefinitionDefault_String<eax>(XLS_ElementDefinition *pElement<eax>, int bUnknown, int x64Offset)
                            _XlsSetDefStringStuff(xlsElement, 0, _GetArg<int>(args, 3)); // XLS_Def_SetDefinitionDefault_String(&element, 0, x64Offset);
                            break;

                        case "XLS_GroupStyle": // 3 // void __cdecl XLS_GroupStyle(XLS_TableDefinition *pTable, char *szName, int offset)
                            Debug.Assert(args.Length == 3);
                            xlsElement = new XlsColumn
                            {
                                Name = _GetArg<string>(args, 1),
                                Offset = _GetArg<int>(args, 2),
                                Type = XlsType.GroupStyle,
                                Count = 1,
                                Size = 4,
                                Default = -1,
                            };
                            break;

                        case "XLS_Style": // 3 // void __cdecl XLS_Style(XLS_TableDefinition *pTable) [used in TestCenter client]
                            Debug.Assert(args.Length == 1);
                            xlsElement = new XlsColumn
                            {
                                Name = "Style",
                                Offset = 136,
                                Type = XlsType.GroupStyle,
                                Count = 1,
                                Size = 4,
                                Default = -1,
                            };
                            break;

                        case "XLS_Flag": // 4 // void __usercall XLS_Flag(XLS_TableDefinition *pTable, char *szName, int offset, unsigned int bitIndex<edi>, char defaultValue)
                            Debug.Assert(args.Length == 5);
                            xlsElement = new XlsColumn
                            {
                                Name = _GetArg<string>(args, 1),
                                Offset = _GetArg<int>(args, 2),
                                Size = (_GetArg<int>(args, 3) >> 3) + 1,
                                Type = XlsType.Flag,
                                Count = 1,
                                Default = _GetArg<bool>(args, 4),
                                FlagIndex = _GetArg<uint>(args, 3)
                            };
                            break;

                        case "XLS_Byte_06": // 6 // void __cdecl XLS_Byte_06(XLS_TableDefinition *pTable, char *szName, int offset) [calls XLS_Byte]
                            Debug.Assert(args.Length == 3);
                            xlsElement = new XlsColumn
                            {
                                Name = _GetArg<string>(args, 1),
                                Type = XlsType.Byte06,
                                Offset = _GetArg<int>(args, 2),
                                Count = 1,
                                Size = 1,
                                Default = (byte)0
                            };
                            break;

                        case "XLS_Int32": // 9 // void __cdecl XLS_Int32(XLS_TableDefinition *pTableDefinition, char *szName, int offset, int defaultValue) [calls XLS_Int32Array]
                            Debug.Assert(args.Length == 4);
                            xlsElement = new XlsColumn
                            {
                                Name = _GetArg<string>(args, 1),
                                Type = XlsType.Int32,
                                Offset = _GetArg<int>(args, 2),
                                Count = 1,
                                Size = 4,
                                Default = _GetArg<int>(args, 3)
                            };
                            break;

                        case "XLS_Bool": // 11 // void __cdecl XLS_Bool(XLS_TableDefinition *pTableDefinition, char *szName, int offset, int defaultValue) [calls XLS_Int32Array]
                            Debug.Assert(args.Length == 4);
                            xlsElement = new XlsColumn
                            {
                                Name = _GetArg<string>(args, 1),
                                Type = XlsType.Bool,
                                Offset = _GetArg<int>(args, 2),
                                Count = 1,
                                Size = 4,
                                Default = _GetArg<bool>(args, 3)
                            };
                            break;

                        case "XLS_Float": // 12 // void __cdecl XLS_Float(XLS_TableDefinition *pTableDefinition, char *szName, int offset, float defaultValue)
                            Debug.Assert(args.Length == 4);
                            xlsElement = new XlsColumn
                            {
                                Name = _GetArg<string>(args, 1),
                                Offset = _GetArg<int>(args, 2),
                                Type = XlsType.Float,
                                Count = 1,
                                Size = 4,
                                Default = _GetArg<float>(args, 3)
                            };
                            break;

                        case "XLS_Percent": // 12 // void __cdecl XLS_Percent(XLS_TableDefinition *pTable, char *szName, int offset)
                            Debug.Assert(args.Length == 3);
                            xlsElement = new XlsColumn
                            {
                                Name = _GetArg<string>(args, 1),
                                Offset = _GetArg<int>(args, 2),
                                Type = XlsType.Float,
                                Count = 1,
                                Size = 4,
                                Default = 0.0f,

                                FloatMin = 0.0f,    // this is essentially the "min"
                                FloatMax = 100.0f   // and the "max" (together the range; 0-100% etc)
                            };
                            break;

                        case "XLS_Byte_0D": // 13 // void __cdecl XLS_Byte_0D(XLS_TableDefinition *pTable, char *szName, int offset) [calls XLS_Byte]
                            Debug.Assert(args.Length == 3);
                            xlsElement = new XlsColumn
                            {
                                Name = _GetArg<string>(args, 1),
                                Offset = _GetArg<int>(args, 2),
                                Type = XlsType.Byte0D,
                                Count = 1,
                                Size = 1,
                                Default = (byte)128
                            };
                            break;

                        case "XLS_CodeInt32": // 14 // void __cdecl XLS_CodeInt32(XLS_TableDefinition *pTable, char *szName, int offset)
                            Debug.Assert(args.Length == 3);
                            xlsElement = new XlsColumn
                            {
                                Name = _GetArg<string>(args, 1),
                                Offset = _GetArg<int>(args, 2),
                                Type = XlsType.CodeInt32,
                                Count = 1,
                                Size = 4,
                                Default = 0
                            };
                            break;

                        case "XLS_CodeInt16": // 16 // void __cdecl XLS_CodeInt16(XLS_TableDefinition *pTableDefinition, char *szName, int offset)
                            Debug.Assert(args.Length == 3);
                            xlsElement = new XlsColumn
                            {
                                Name = _GetArg<string>(args, 1),
                                Offset = _GetArg<int>(args, 2),
                                Type = XlsType.CodeInt16,
                                Count = 1,
                                Size = 2,
                                Default = (Int16)0
                            };
                            break;

                        case "XLS_CodeInt8": // 17 // void __cdecl XLS_CodeInt8(XLS_TableDefinition *pTable)
                            Debug.Assert(args.Length == 1);
                            xlsElement = new XlsColumn
                            {
                                Count = 1,
                                Size = 1,
                                Name = "code",
                                Type = XlsType.CodeInt8,
                                Offset = 64, // XLS_CodeInt8 is only use once in table ITEM_LOOK_GROUP_DATA
                                Default = (byte)0
                            };
                            break;

                        case "XLS_ExcelIndex": // 20 // void __usercall XLS_ExcelIndex(XLS_TableDefinition *pTableDefinition<edi>, char *szName, int offset, int tableIndex) [calls XLS_ExcelIndexArray]
                            Debug.Assert(args.Length == 4); // XLS_ExcelIndexArray(pTableDefinition, 20, szName, offset, 1, tableIndex, 0);
                            xlsElement = new XlsColumn
                            {
                                Name = _GetArg<string>(args, 1),
                                Type = XlsType.ExcelIndex,
                                Offset = _GetArg<int>(args, 2),
                                Count = 1,
                                Size = 4,
                                Default = null,
                                Flags = XlsFlags.ExcelIndex,
                            };

                            // applicable only to ExcelIndex and ExcelIndexArray, AiInit, Qualities, and FolderCode
                            // void __usercall XLS_Def_ExcelSomething(XLS_ElementDefinition *pElement<eax>, unsigned int unknown1<edx>, unsigned int tableIndex<esi>, int excelDefaultQ)                            // void __usercall XLS_Def_ExcelSomething(XLS_ElementDefinition *pElement<eax>, unsigned int unknown1<edx>, unsigned int tableIndex<esi>, int excelDefaultQ)
                            _XlsSetDefExcelStuff(xlsElement, 0, _GetArg<int>(args, 3), -1); // XLS_Def_ExcelSomething(&element, unknown1, tableIndex, -1);
                            break;

                        case "XLS_FolderCode": // 21 // void __usercall XLS_FolderCode(XLS_TableDefinition *pTable<edi>, int offset)
                            Debug.Assert(args.Length == 2);
                            xlsElement = new XlsColumn
                            {
                                Offset = _GetArg<int>(args, 1),
                                Name = "FolderCode",
                                Type = XlsType.FolderCode,
                                Count = 1,
                                Size = 4,
                                Default = null,
                                Flags = XlsFlags.ExcelIndex
                            };

                            // LEVEL_FILE_PATHS
                            int tableIndex = -1;
                            switch (fileManager.ClientVersion)
                            {
                                case FileManager.ClientVersions.SinglePlayer: tableIndex = 0x4E; break;
                                case FileManager.ClientVersions.TestCenter: tableIndex = 0x4F; break;
                                case FileManager.ClientVersions.Resurrection: tableIndex = 0x52; break;
                            }

                            // applicable only to ExcelIndex and ExcelIndexArray, AiInit, Qualities, and FolderCode
                            // void __usercall XLS_Def_ExcelSomething(XLS_ElementDefinition *pElement<eax>, unsigned int unknown1<edx>, unsigned int tableIndex<esi>, int excelDefaultQ)                            // void __usercall XLS_Def_ExcelSomething(XLS_ElementDefinition *pElement<eax>, unsigned int unknown1<edx>, unsigned int tableIndex<esi>, int excelDefaultQ)
                            _XlsSetDefExcelStuff(xlsElement, 0, tableIndex /*LEVEL_FILE_PATHS*/, -1); // XLS_Def_ExcelSomething(&element, 0, 0x52u, -1);
                            break;

                        case "XLS_Int32Default": // 25 // void __cdecl XLS_Int32Default(XLS_TableDefinition *pTableDefinition, char *szName, int offset) [calls XLS_Int32Array]
                            Debug.Assert(args.Length == 3);
                            xlsElement = new XlsColumn
                            {
                                Name = _GetArg<string>(args, 1),
                                Offset = _GetArg<int>(args, 2),
                                Type = XlsType.Int32Default,
                                Count = 1,
                                Size = 4,
                                Default = -1
                            };
                            break;

                        case "XLS_AiInit": // 26 // void __usercall XLS_AiInit(XLS_TableDefinition *pTable<edi>, char *szName, int excelDefaultQ)
                            Debug.Assert(args.Length == 3);
                            xlsElement = new XlsColumn
                            {
                                Offset = 0,
                                Size = 0,
                                Name = _GetArg<string>(args, 1),
                                Type = XlsType.AiInit,
                                Count = 1,
                                Default = null,
                                Flags = XlsFlags.ExcelIndex
                            };

                            // AI_INIT
                            int aiInitIndex = -1;
                            switch (fileManager.ClientVersion)
                            {
                                case FileManager.ClientVersions.SinglePlayer: aiInitIndex = 0x46; break;
                                case FileManager.ClientVersions.TestCenter: aiInitIndex = 0x47; break;
                                case FileManager.ClientVersions.Resurrection: aiInitIndex = 0x48; break;
                            }

                            // applicable only to ExcelIndex and ExcelIndexArray, AiInit, Qualities, and FolderCode
                            // void __usercall XLS_Def_ExcelSomething(XLS_ElementDefinition *pElement<eax>, unsigned int unknown1<edx>, unsigned int tableIndex<esi>, int excelDefaultQ)                            // void __usercall XLS_Def_ExcelSomething(XLS_ElementDefinition *pElement<eax>, unsigned int unknown1<edx>, unsigned int tableIndex<esi>, int excelDefaultQ)
                            _XlsSetDefExcelStuff(xlsElement, 0, aiInitIndex /*AI_INIT*/, _GetArg<int>(args, 2)); // XLS_Def_ExcelSomething(&element, 0, 0x48u, excelDefaultQ);
                            break;

                        case "XLS_Enum": // 29 // void __usercall XLS_Enum(XLS_TableDefinition *pTable, char *szName, int offset, int enumCount, void *enums<eax>) [calls XLS_EnumArray]
                            Debug.Assert(args.Length == 5);
                            xlsElement = new XlsColumn
                            {
                                Name = _GetArg<string>(args, 1),
                                Type = XlsType.Enum,
                                Offset = _GetArg<int>(args, 2),
                                Count = 1,
                                Size = 4,
                                Flags = XlsFlags.Enum,
                                Default = null // might have to set this to a proper default (the INDEX of the enum array. i.e. enums[0]... I think...)
                            };

                            Dictionary<String, int> xlsEnum = _XlsRipEnums(args[4], _GetArg<int>(args, 3));
                            Debug.Assert(xlsEnum.Count > 0);
                            if (xlsEnum.ContainsKey("none"))
                            {
                                xlsElement.Flags |= XlsFlags.HasNone;
                            }

                            // void __usercall XLS_Def_EnumStuff(int pEnums<eax>, XLS_ElementDefinition *pElement, int enumCount)
                            //_XlsSetEnumStuff(xlsElement, xlsEnum, _GetArg<int>(args, 3));

                            xlsElement.Default = xlsEnum;
                            // xlsElement.Field38 = _GetArg<int>(args, 3); // enumCount
                            break;

                        case "XLS_EnumArray": // 29 (Enum) or 30 (EnumArray) // void __usercall XLS_EnumArray(XLS_TableDefinition *pTableDefinition, void *enums<eax>, int typeId, char *szName, int offset, int elementCount, int enumCount)
                            Debug.Assert(args.Length == 7);
                            xlsElement = new XlsColumn
                            {
                                Name = _GetArg<string>(args, 3),
                                Type = _GetArg<XlsType>(args, 2),
                                Offset = _GetArg<int>(args, 4),
                                Count = _GetArg<int>(args, 5),
                                Size = 4,
                                Flags = XlsFlags.Enum,
                                Default = null // might have to set this to a proper default (the INDEX of the enum array. i.e. enums[0]... I think...)
                            };

                            Dictionary<String, int> xlsEnumArray = _XlsRipEnums(args[1], _GetArg<int>(args, 6));
                            Debug.Assert(xlsEnumArray.Count > 0);
                            if (xlsEnumArray.ContainsKey("none"))
                            {
                                xlsElement.Flags |= XlsFlags.HasNone;
                            }

                            // void __usercall XLS_Def_EnumStuff(int pEnums<eax>, XLS_ElementDefinition *pElement, int enumCount)
                            //_XlsSetEnumStuff(xlsElement, xlsEnumArray, _GetArg<int>(args, 6));

                            xlsElement.Default = xlsEnumArray;
                            // xlsElement.Field38 = _GetArg<int>(args, 6); // enumCount
                            break;

                        case "XLS_Array": // 35 or 37 // void __cdecl XLS_Int32Array(XLS_TableDefinition *pTableDefinition, int typeId, char *szName, int offset, int elementCount, int defaultValue)
                            Debug.Assert(args.Length == 6);
                            xlsElement = new XlsColumn
                            {
                                Name = _GetArg<string>(args, 2),
                                Type = _GetArg<XlsType>(args, 1),
                                Offset = _GetArg<int>(args, 3),
                                Count = _GetArg<int>(args, 4),
                                Size = 4,
                                Default = _GetArg<int>(args, 5),
                            };

                            switch (xlsElement.Type)
                            {
                                case XlsType.Int32Array:
                                    xlsElement.Default = _GetArg<int>(args, 5);
                                    break;

                                case XlsType.FloatArray:
                                    xlsElement.Default = _GetArg<float>(args, 5);
                                    break;

                                default:
                                    throw new Exceptions.UnexpectedErrorException(xlsElement.Type + " not implemented in XLS_Array.");
                            }
                            break;

                        case "XLS_Int32Array": // 35 // void __cdecl XLS_Int32Array2(XLS_TableDefinition *pTable, char *szName, int offset, int elementCount, int defaultValue) [calls XLS_Int32Array]
                            Debug.Assert(args.Length == 5);
                            xlsElement = new XlsColumn
                            {
                                Name = _GetArg<string>(args, 1),
                                Type = XlsType.Int32Array,
                                Offset = _GetArg<int>(args, 2),
                                Count = _GetArg<int>(args, 3),
                                Size = 4,
                                Default = _GetArg<int>(args, 4),
                            };
                            break;

                        case "XLS_UseFixedDRLGSeed": // 36 // void __cdecl XLS_UseFixedDRLGSeed(XLS_TableDefinition *pTable)
                            Debug.Assert(args.Length == 1);
                            xlsElement = new XlsColumn
                            {
                                Name = "Use Fixed DRLG Seed",
                                Type = XlsType.UseFixedDRLGSeed, // found only in DRLG_DEFINITION
                                Offset = 320,
                                Count = 20,
                                Size = 4,
                                Default = -1,
                            };
                            break;

                        case "XLS_ExcelIndexArray": // 40 // void __usercall XLS_ExcelIndexArray(XLS_TableDefinition *pTable<edi>, int typeId, char *szName, int offset, int count, int tableIndex, int unknown1)
                            Debug.Assert(args.Length == 7);
                            xlsElement = new XlsColumn
                            {
                                Name = _GetArg<string>(args, 2),
                                Type = _GetArg<XlsType>(args, 1),
                                Offset = _GetArg<int>(args, 3),
                                Count = _GetArg<int>(args, 4),
                                Size = 4,
                                Default = null,
                                Flags = XlsFlags.ExcelIndex
                            };

                            //Debug.Assert(_GetArg<int>(args, 6) == 0);
                            //if (_GetArg<int>(args, 6) != 0)
                            //{
                            //    int bp = 0;
                            //}

                            // applicable only to ExcelIndex and ExcelIndexArray, AiInit, Qualities, and FolderCode
                            // void __usercall XLS_Def_ExcelSomething(XLS_ElementDefinition *pElement<eax>, unsigned int unknown1<edx>, unsigned int tableIndex<esi>, int excelDefaultQ)
                            _XlsSetDefExcelStuff(xlsElement, _GetArg<int>(args, 6), _GetArg<int>(args, 5), -1); // XLS_Def_ExcelSomething(&element, unknown1, tableIndex, -1);
                            break;

                        case "XLS_Qualities": // 45 // void __usercall XLS_Qualities(XLS_TableDefinition *pTable<edi>)
                            Debug.Assert(args.Length == 1);

                            // only found in UNIT_DATA
                            xlsElement = new XlsColumn
                            {
                                Name = "qualities",
                                Type = XlsType.Qualities,
                                Count = 64,
                                Size = 8,
                                Default = null,
                                Flags = XlsFlags.ExcelIndex
                            };

                            // ITEM_QUALITY
                            int qualitiesIndex = -1;
                            switch (fileManager.ClientVersion)
                            {
                                case FileManager.ClientVersions.SinglePlayer: qualitiesIndex = 0x43; xlsElement.Offset = 2576; break;
                                case FileManager.ClientVersions.TestCenter: qualitiesIndex = 0x44; xlsElement.Offset = 2708; break;
                                case FileManager.ClientVersions.Resurrection: qualitiesIndex = 0x45; xlsElement.Offset = 2260; break;
                            }

                            // applicable only to ExcelIndex and ExcelIndexArray, AiInit, Qualities, and FolderCode
                            // void __usercall XLS_Def_ExcelSomething(XLS_ElementDefinition *pElement<eax>, unsigned int unknown1<edx>, unsigned int tableIndex<esi>, int excelDefaultQ)
                            _XlsSetDefExcelStuff(xlsElement, 0, qualitiesIndex  /*ITEM_QUALITY*/, -1); // XLS_Def_ExcelSomething(&xlsDefinitionObj, 0, 0x45u, -1);

                            break;

                        case "XLS_StringIndex": // 46 // void __usercall XLS_StringIndex(XLS_TableDefinition *pTableDefinition<edi>, char *szName, int offset, int indexType(?))
                            Debug.Assert(args.Length == 4);
                            xlsElement = new XlsColumn
                            {
                                Name = _GetArg<string>(args, 1),
                                Offset = _GetArg<int>(args, 2),
                                Type = XlsType.StringIndex,
                                Count = 1,
                                Size = 4,
                                Default = null
                            };

                            int indexType = _GetArg<int>(args, 3); // not entirely sure what this is - looks like some sort of type though
                            if (indexType == 2) xlsElement.Flags = XlsFlags.StringIndex2;
                            if (indexType == 3) xlsElement.Flags = XlsFlags.StringIndex3;

                            xlsTable.HasStringIndex = true;
                            break;

                        case "XLS_TugboatUnknown": // 47 // void __usercall XLS_TugboatUnknown(XLS_TableDefinition *pTable<esi>, char *szName, int offset, int elementCount<edi>)
                            Debug.Assert(args.Length == 4);
                            xlsElement = new XlsColumn
                            {
                                Name = _GetArg<string>(args, 1),
                                Offset = _GetArg<int>(args, 2),
                                Type = XlsType.TugboatUnknown, // I think it's just another type of string index - doesn't matter though, only found in unused table (QUEST_TASK_DEFINITION_TUGBOAT)
                                Count = _GetArg<int>(args, 3),
                                Size = 4,
                                Default = null,
                                Flags = XlsFlags.StringIndex3
                            };

                            xlsTable.HasStringIndex = true;
                            break;

                        case "XLS_File": // 48 // void __cdecl XLS_File(XLS_TableDefinition *pTable)
                            Debug.Assert(args.Length == 1);
                            xlsElement = new XlsColumn
                            {
                                Offset = 0,
                                Name = "file",
                                Type = XlsType.File,
                                Count = 1,
                                Size = 8,
                                // Field38 = 0,
                                FilePath = @"data\palettes\", // this type is used only once for PALETTE_DATA
                                Default = null
                            };
                            break;

                        case "XLS_Script": // 50 // void __cdecl XLS_Script(XLS_TableDefinition *pTable, char *szName, int offset)
                            Debug.Assert(args.Length == 3);
                            xlsElement = new XlsColumn
                            {
                                Name = _GetArg<string>(args, 1),
                                Offset = _GetArg<int>(args, 2),
                                Type = XlsType.Script,
                                Count = 1,
                                Size = 4,
                                Default = 0
                            };
                            xlsTable.HasScripts = true;
                            break;

                        case "XLS_MaxSlots": // 51 // void __usercall XLS_MaxSlots(XLS_TableDefinition *pTable<edi>)
                            Debug.Assert(args.Length == 1);
                            xlsElement = new XlsColumn
                            {
                                Offset = 0,
                                Size = 0,
                                Name = "max slots",
                                Type = XlsType.MaxSlots,
                                Count = 1,
                                Default = 0
                            };

                            xlsTable.HasUnknown = true;
                            break;

                        case "XLS_Unknown1": // 52 // void __usercall XLS_Unknown1(XLS_TableDefinition *pTable<edi>, char *szName, int index, int defaultValue)
                            Debug.Assert(args.Length == 4);
                            xlsElement = new XlsColumn
                            {
                                Offset = 0,
                                Size = 0,
                                Name = _GetArg<string>(args, 1),
                                Type = XlsType.Unknown1,
                                Count = 1,
                                Default = _GetArg<int>(args, 3),

                                // _XlsSetDefAimHeightStuff(xlsElement, _GetArg<int>(args, 2), 0);
                                UnknownIndex = _GetArg<int>(args, 2), // Field30
                                // xlsElement.Field34 = 0;
                            };

                            xlsTable.HasUnknown = true;
                            break;

                        case "XLS_AimHeight": // 53 // void __usercall XLS_AimHeight(XLS_TableDefinition *pTable<edi>)
                            Debug.Assert(args.Length == 1);
                            xlsElement = new XlsColumn
                            {
                                Offset = 0,
                                Size = 0,
                                Name = "aim_height",
                                Type = XlsType.AimHeight,
                                Count = 1,
                                Default = 0.0f
                            };

                            int aimHeightIndex = -1;
                            switch (fileManager.ClientVersion)
                            {
                                case FileManager.ClientVersions.SinglePlayer: aimHeightIndex = 266; break;
                                case FileManager.ClientVersions.TestCenter: aimHeightIndex = 281; break;
                                case FileManager.ClientVersions.Resurrection: aimHeightIndex = 284; break;
                            }

                            // _XlsSetDefAimHeightStuff(xlsElement, aimHeightIndex, 0);
                            xlsElement.UnknownIndex = aimHeightIndex; // Field30
                            // xlsElement.Field34 = 0;

                            xlsTable.HasUnknown = true;
                            break;

                        case "XLS_UnitTypeArray": // 54 // void __cdecl XLS_UnitTypeArray(XLS_TableDefinition *pTableDefinition, char *szName, int offset, int elementCount, char *defaultValue)
                            Debug.Assert(args.Length == 5);
                            xlsElement = new XlsColumn
                            {
                                Name = _GetArg<string>(args, 1),
                                Offset = _GetArg<int>(args, 2),
                                Count = _GetArg<int>(args, 3),
                                Type = XlsType.UnitType,
                                Size = 4,
                                Default = _GetArg<string>(args, 4)
                            };
                            break;

                        case "XLS_MultipleRelations": // 55 // void __cdecl XLS_MultipleRelations(XLS_TableDefinition *pTable, char *szName, int offset, int size, void *pFunc)
                            Debug.Assert(args.Length == 5);
                            xlsElement = new XlsColumn
                            {
                                Name = _GetArg<string>(args, 1),
                                Offset = _GetArg<int>(args, 2),
                                Type = XlsType.MultipleRelations,
                                Size = _GetArg<int>(args, 3),
                                // Field38 = 0,
                                // Index = _GetArg<String>(args, 4),
                                Default = null
                            };
                            break;

                        case "XLS_BaseRow": // 56 // void __usercall XLS_BaseRow(XLS_TableDefinition *pTable<esi>, int offset)
                            Debug.Assert(args.Length == 2);
                            xlsElement = new XlsColumn
                            {
                                Offset = _GetArg<int>(args, 1),
                                Size = 4,
                                Flags = XlsFlags.BaseRow, // 4
                                Name = "base row",
                                Type = XlsType.BaseRow, // found in LEVEL_DRLG_CHOICE and UNIT_DATA
                                Default = null
                            };

                            xlsTable.HasBaseRow = true;
                            break;




                        case "XLS_FlagLastElement": // void __usercall XLS_FlagLastElement(XLS_TableDefinition *pTable<edx>, int flag)
                            Debug.Assert(args.Length == 2);
                            xlsTable.Columns[xlsTable.ColumnCount - 1].Flags |= (XlsFlags)_GetArg<int>(args, 1);
                            break;

                        case "XLS_AssignStringIndex": // void __usercall XLS_AssignStringIndex(XLS_TableDefinition *pTable<esi>, char *element<eax>)
                            // not sure what this is for
                            /*
                            Direction Type Address                   Text                                         
                            --------- ---- -------                   ----                                         
                            Up        p    XLS_UNITTYPE_DATA+109     call    XLS_AssignStringIndex
                            Up        p    XLS_SKILLGROUP_DATA+88    call    XLS_AssignStringIndex
                            Up        p    XLS_SKILL_DATA+1710       call    XLS_AssignStringIndex
                            Up        p    XLS_ITEM_QUALITY_DATA+426 call    XLS_AssignStringIndex
                            Up        p    XLS_UNIT_DATA+3659        call    XLS_AssignStringIndex
                             */
                            Debug.Assert(args.Length == 2);
                            xlsTable.StringIndexColumn = xlsTable.Columns.FirstOrDefault(element => element.Name == _GetArg<string>(args, 1));
                            break;

                        case "XLS_ExcelScript": // void __usercall XLS_ExcelScript(XLS_TableDefinition *pTable<eax>, int unknown (count? size?))
                            // pretty sure this is relating to the compiled script code in some excel tables
                            /*
                            Direction Type Address                       Text                                    
                            --------- ---- -------                       ----                                    
                            Up        p    XLS_DefineTables+325          call    XLS_ExcelScript
                            Up        p    XLS_SKILL_DATA+173D           call    XLS_ExcelScript
                            Up        p    XLS_SKILL_DATA+1746           call    XLS_ExcelScript
                            Up        p    XLS_SKILL_DATA+1750           call    XLS_ExcelScript
                            Up        p    XLS_LEVEL_AREA_DEFINITION+A36 call    XLS_ExcelScript
                            Up        p    XLS_LEVEL_AREA_DEFINITION+A43 call    XLS_ExcelScript
                            Up        p    XLS_UNIT_DATA+364C            call    XLS_ExcelScript
                             */
                            Debug.Assert(args.Length == 2);
                            // todo
                            break;

                        case "XLS_RelationalElement": // void __usercall XLS_RelationalElement(XLS_TableDefinition *pTable<esi>)
                            // quite certain this is the bit relational element segment
                            /*
                            Direction Type Address               Text                                         
                            --------- ---- -------               ----                                         
                            Up        p    XLS_UNITTYPE_DATA+10E call    XLS_RelationalElement; Call Procedure
                            Up        p    XLS_STATE_DATA+42E    call    XLS_RelationalElement; Call Procedure
                             */
                            Debug.Assert(args.Length == 1);
                            xlsTable.RelationalColumn = xlsTable.Columns.FirstOrDefault(element => element.Name == "isa");
                            break;

                        case "XLS_SortColumns": // void __usercall XLS_SortColumns(XLS_TableDefinition *pTable, char *szColumn<edx>, int order, int unknown1, char *szByColumn1, char* szByColumn2)
                            Debug.Assert(args.Length == 6);
                            String columnName = _GetArg<string>(args, 1);
                            int order = _GetArg<int>(args, 2);
                            int unknown1 = _GetArg<int>(args, 3);
                            String byColumn1 = _GetArg<string>(args, 4);
                            String byColumn2 = _GetArg<string>(args, 5);

                            XlsColumn sortElement;
                            if (columnName != null && (sortElement = xlsTable.Columns.FirstOrDefault(element => String.Compare(element.Name, columnName, true) == 0)) != null) sortElement.Flags |= XlsFlags.Sort;
                            if (byColumn1 != null && (sortElement = xlsTable.Columns.FirstOrDefault(element => String.Compare(element.Name, byColumn1, true) == 0)) != null) sortElement.Flags |= XlsFlags.Sort;
                            if (byColumn2 != null && (sortElement = xlsTable.Columns.FirstOrDefault(element => String.Compare(element.Name, byColumn2, true) == 0)) != null) sortElement.Flags |= XlsFlags.Sort;
                            break;

                        case "XLS_GenerateStructureId": // Appears at the end of a definition...
                            inTableDefinition = false;
                            break;

                        default:
                            throw new Exceptions.UnexpectedErrorException(func + " not implemented in _GenerateExcelTables.");
                    }

                    if (xlsElement == null) continue;
                    Debug.Assert(xlsElement.Name != null);
                    Debug.Assert(xlsElement.Type != XlsType.None);
                    Debug.Assert(xlsElement.Offset >= 0);

                    if (xlsElement.Type != XlsType.AiInit && xlsElement.Type != XlsType.MaxSlots &&
                        xlsElement.Type != XlsType.Unknown1 && xlsElement.Type != XlsType.AimHeight)
                    {
                        Debug.Assert(xlsElement.Size > 0);
                    }

                    if (xlsElement.Type != XlsType.MultipleRelations && xlsElement.Type != XlsType.BaseRow)
                    {
                        Debug.Assert(xlsElement.Count > 0);
                    }

                    if (xlsElement.Type != XlsType.String && xlsElement.Type != XlsType.StringOffset &&
                        xlsElement.Type != XlsType.File && xlsElement.Type != XlsType.UnitType &&
                        xlsElement.Type != XlsType.MultipleRelations && xlsElement.Type != XlsType.TugboatUnknown &&
                        xlsElement.Type != XlsType.BaseRow && xlsElement.Type != XlsType.Enum &&
                        xlsElement.Type != XlsType.EnumArray && xlsElement.Type != XlsType.ExcelIndex &&
                        xlsElement.Type != XlsType.FolderCode && xlsElement.Type != XlsType.AiInit &&
                        xlsElement.Type != XlsType.StringIndex && xlsElement.Type != XlsType.ExcelIndexArray &&
                        xlsElement.Type != XlsType.Qualities)
                    {
                        Debug.Assert(xlsElement.Default != null);
                    }

                    xlsElement.ElementIndex = xlsTable.Columns.Count;
                    xlsTable.Columns.Add(xlsElement);
                }

                tables.Add(xlsTable);
            }


            // get table details
            _GetTableDetails(source, tables, outputLinesParsed);

            // test hashes
            _TestHashes(fileManager, tables, outputLinesParsed);

            return tables;
        }

        private static void _GetTableDetails(String[] source, List<XlsTable> tables, bool outputLinesParsed)
        {
            if (outputLinesParsed) Debug.Write("\n");
            int lineCount = source.Length;
            for (int i = 0; i < lineCount; i++)
            {
                if (!source[i].Contains("XLS_DefineTable") || source[i].StartsWith("//") || source[i].Contains("XLS_DefineTables")) continue;
                if (source[i].Contains("XLS_DefineTable<")) continue; // the function definition

                String line = source[i];
                int eBrake = 0;
                while (line.Count(c => c == '(') != line.Count(c => c == ')') && eBrake++ < 10 && i < lineCount)
                {
                    line += source[++i];
                }
                Debug.Assert(line.Count(c => c == '(') == line.Count(c => c == ')')); // make sure we didn't brake due to excess line counts

                Match match = ArgsRegex.Match(line);
                String func = match.Groups[1].Value;
                String[] args = match.Groups[2].Value.Split(new[] { ", " }, StringSplitOptions.None);

                // int __usercall XLS_DefineTable<eax>(char *szDefinitionId<eax>, DWORD *excelTables<edi>, unsigned int tableIndex, char *szFileName, int a5, int a6)
                Debug.Assert(func == "XLS_DefineTable");
                Debug.Assert(args.Length == 6);

                String definitionId = _GetArg<string>(args, 0);
                uint tableIndex = _GetArg<uint>(args, 2);
                String fileName = _GetArg<string>(args, 3);
                int unknown1 = _GetArg<int>(args, 4);
                int unknown2 = _GetArg<int>(args, 5);

                Debug.Assert(tableIndex < 0x200);
                if (outputLinesParsed) Debug.WriteLine("Table[{0}]: {1} ({2}.txt.cooked), u1 = {3}, u2 = {4}", tableIndex, definitionId, fileName, unknown1, unknown2);

                XlsTable xlsTable = tables.FirstOrDefault(t => t.DefinitionId == definitionId);
                if (xlsTable == null)
                {
                    Debug.WriteLine("Unknown table id!");
                    continue;
                }

                xlsTable.FileName = fileName;
                xlsTable.TableIndex = (int)tableIndex;
                xlsTable.Unknown1 = unknown1;
                xlsTable.Unknown2 = unknown2;
            }
        }

        private static void _TestHashes(FileManager fileManager, List<XlsTable> tables, bool outputLinesParsed)
        {
            if (outputLinesParsed) Debug.WriteLine("\nLoading excel files...");
            Dictionary<String, uint> excelHashes = new Dictionary<String, uint>();
            fileManager.BeginAllDatReadAccess();
            foreach (PackFileEntry fileEntry in fileManager.FileEntries.Values)
            {
                if (!fileEntry.Name.EndsWith(".txt.cooked") || !fileEntry.Directory.Contains("excel")) continue;

                byte[] excelBytes = fileManager.GetFileBytes(fileEntry, true);
                Debug.Assert(excelBytes != null);

                uint hash = StreamTools.ReadUInt32(excelBytes, 4);

                excelHashes.Add(fileEntry.Name.Replace(".txt.cooked", ""), hash);
            }
            fileManager.EndAllDatAccess();

            // tableHash + scriptsHash = finalTableHash
            const uint scriptsHashSinglePlayer = 0xF3EEE2FE;
            const uint scriptsHashTestCenter = 0x2CEFF7B7;
            const uint scriptsHashResurrection = 0x9A292E9B;

            uint scriptsHash = scriptsHashSinglePlayer;
            if (fileManager.IsVersionResurrection) scriptsHash = scriptsHashResurrection;
            if (fileManager.IsVersionTestCenter) scriptsHash = scriptsHashTestCenter;

            if (outputLinesParsed) Debug.WriteLine("\nChecking table hash generation...");
            int tablesNotFound = 0;
            int tablesMatched = 0;
            int tablesNotMatched = 0;
            foreach (XlsTable xlsTable in tables)
            {
                _GenerateTableHash(xlsTable, scriptsHash);

                uint hash;
                excelHashes.TryGetValue(xlsTable.FileName, out hash);

                if (hash == 0)
                {
                    if (outputLinesParsed) Debug.WriteLine("Table[{0}]: {1} table not found. Hash = 0x{2:X8}", xlsTable.TableIndex, xlsTable.DefinitionId, xlsTable.TableHash);
                    tablesNotFound++;
                }
                else if (hash == xlsTable.TableHash)
                {
                    if (outputLinesParsed) Debug.WriteLine("Table[{0}]: {1} -> [0x{2:X8} == 0x{3:X8}]", xlsTable.TableIndex, xlsTable.DefinitionId, xlsTable.TableHash, hash);
                    tablesMatched++;
                }
                else
                {
                    if (outputLinesParsed) Debug.WriteLine("Table[{0}]: {1} -> [0x{2:X8} != 0x{3:X8}]", xlsTable.TableIndex, xlsTable.DefinitionId, xlsTable.TableHash, hash);
                    if (outputLinesParsed) Debug.WriteLine("Warning: Table hashes do not match.\n");
                    tablesNotMatched++;
                }
            }

            Debug.WriteLine("{0} {1} tables: {2} not found, {3} hash check matches, {4} hash check fails.", tables.Count, fileManager.ClientVersion, tablesNotFound, tablesMatched, tablesNotMatched);
            Debug.Assert(tablesNotMatched == 0);
        }

        private static unsafe T _GetArg<T>(String[] args, int index)
        {
            Object val;
            Type t = typeof(T);
            String arg = args[index];

            if (t == typeof(String))
            {
                val = StringRegex.Match(arg).Groups[1].Value;
                if (String.IsNullOrEmpty(val.ToString()))
                {
                    if (arg.Contains("null_value")) val = String.Empty;
                    else if (arg.Length < 4 && arg.Contains("0")) val = null;
                    else if (arg.IndexOf('_') >= 0) // decompiler "errors" (usually 1-3 char length strings, so the decompiler mistakes them for a pointer
                    {
                        val = _GetAsciiStringFromClient(arg);
                    }
                    else if (!String.IsNullOrEmpty(arg)) throw new Exceptions.UnexpectedErrorException("arg is not empty in _GetArg, but no value parsed!");
                }
            }
            else if (t == typeof(bool))
            {
                val = (Int32.Parse(arg) != 0);
            }
            else if (t.BaseType == typeof(Enum))
            {
                val = Enum.Parse(t, arg);
            }
            else if (t == typeof(uint))
            {
                NumberStyles numberStyle = NumberStyles.Integer;
                if (arg.IndexOf("0x") >= 0)
                {
                    arg = arg.Replace("0x", String.Empty);
                    numberStyle = NumberStyles.HexNumber;
                }

                int uIndex;
                if ((uIndex = arg.IndexOf('u')) >= 0)
                {
                    arg = arg.Remove(uIndex, 1);
                }

                val = UInt32.Parse(arg, numberStyle);
            }
            else if (t == typeof(int))
            {
                NumberStyles numberStyle = NumberStyles.Integer;
                if (arg.IndexOf("0x") >= 0)
                {
                    arg = arg.Replace("0x", String.Empty);
                    numberStyle = NumberStyles.HexNumber;
                }

                int uIndex;
                if ((uIndex = arg.IndexOf('u')) >= 0)
                {
                    arg = arg.Remove(uIndex, 1);
                }

                val = Int32.Parse(arg, numberStyle);
            }
            else if (t == typeof(float))
            {
                if (arg.Contains("off_") || arg.Contains("word_") || arg.Contains("byte_"))
                {
                    val = _GetFloatFromClient(arg);
                }
                else if (arg.Contains("-5.104235503814077e38"))
                {
                    uint hexVal = 0xFFC00000; // ~= -5.1042355e38
                    val = *(float*)&hexVal;
                }
                else
                {
                    val = Single.Parse(arg);
                }
            }
            else
            {
                val = Convert.ChangeType(arg, typeof(T));
            }

            return (T)val;
        }

        private static float _GetFloatFromClient(String offStr)
        {
            String offsetStr = AddressRegex.Match(offStr).Groups[1].Value;
            Debug.Assert(!String.IsNullOrEmpty(offsetStr));
            Debug.Assert(_exeBytes != null);

            int offset = Int32.Parse(offsetStr, NumberStyles.HexNumber) - ExeBase; // hellgate.exe uses 0x400000
            Debug.Assert(offset >= 0 && offset < _exeBytes.Length);

            return StreamTools.ReadFloat(_exeBytes, offset);
        }

        private static String _GetAsciiStringFromClient(String offStr)
        {
            String offsetStr = AddressRegex.Match(offStr).Groups[1].Value;
            Debug.Assert(!String.IsNullOrEmpty(offsetStr));
            Debug.Assert(_exeBytes != null);

            int offset = Int32.Parse(offsetStr, NumberStyles.HexNumber) - ExeBase; // hellgate.exe uses 0x400000
            return _GetStringFromOffset(offset);
        }

        private static String _GetStringFromOffset(int offset)
        {
            Debug.Assert(offset >= 0 && offset < _exeBytes.Length);

            int endOffset; // you'd think ASCII.GetBytes would see the terminator and stop there, or that there'd be an option for it - apparently not
            for (endOffset = offset; endOffset < _exeBytes.Length && _exeBytes[endOffset] != 0x00; endOffset++) { }

            int len = (endOffset - offset);
            return (len == 0) ? String.Empty : Encoding.ASCII.GetString(_exeBytes, offset, len);
        }

        private unsafe static Dictionary<String, int> _XlsRipEnums(String address, int enumCount)
        {
            Dictionary<String, int> enumValues = new Dictionary<String, int>();

            // exe location
            Match addressMatch = AddressRegex.Match(address);
            String offsetStr = addressMatch.Groups[1].Value;
            Debug.Assert(!String.IsNullOrEmpty(offsetStr));
            Debug.Assert(_exeBytes != null);

            int offset = Int32.Parse(offsetStr, NumberStyles.HexNumber) - ExeBase;
            Debug.Assert(offset >= 0 && offset < _exeBytes.Length);

            // get enums
            fixed (byte* pData = _exeBytes)
            {
                for (int i = 0; i < enumCount; i++)
                {
                    int currOffset = offset + i * 8;

                    int enumStrOffset = *(int*)(pData + currOffset);
                    if (enumStrOffset == 0) continue; // null pointer

                    enumStrOffset -= ExeBase;

                    String enumStr = _GetStringFromOffset(enumStrOffset);
                    int enumValue = *(int*)(pData + currOffset + 4);

                    enumValues.Add(enumStr, enumValue);
                }
            }

            return enumValues;
        }

        private static void _XlsSetDefStringStuff(XlsColumn xlsColumn, int stringBool, int offset64)
        {
            Debug.Assert(xlsColumn.Type == XlsType.String || xlsColumn.Type == XlsType.StringOffset);
            Debug.Assert(xlsColumn.StringBool == false);
            Debug.Assert(xlsColumn.String64Offset == 0);
            Debug.Assert(stringBool == 0 || stringBool == 1);

            xlsColumn.StringBool = (stringBool != 0); // field30
            xlsColumn.String64Offset = offset64; // field34; is the x64 offset
        }

        //private static void _XlsSetDefStringStuff(XlsColumn xlsElement, int index, int indexUnknown)
        //{
        //    if ((uint)(xlsElement.Type - 1) > 1) return; // if String or StringOffset/StringOffsetDefault

        //    if (xlsElement.Index != null) return; // if already set, then return

        //    xlsElement.Index = index;
        //    xlsElement.IndexUnknown = indexUnknown; // is the x64 offset
        //}

        //private static void _XlsSetDefAimHeightStuff(XlsColumn xlsElement, int index, int indexUnknown)
        //{
        //    if ((uint)(xlsElement.Type - 52) > 1) return; // if Unknown1 or AimHeight

        //    if (xlsElement.Index != null) return; // if already set, then return

        //    xlsElement.Index = index;
        //    xlsElement.IndexUnknown = indexUnknown;
        //}

        private static void _XlsSetDefExcelStuff(XlsColumn xlsElement, int unknownFlag, int tableIndex, int excelUnknown)
        {
            Debug.Assert(tableIndex < 0x200);
            Debug.Assert(unknownFlag < 4);
            Debug.Assert(xlsElement.ExcelIndex == 0);
            Debug.Assert(xlsElement.ExcelFlags == 0);
            Debug.Assert(xlsElement.ExcelUnknown == 0);

            xlsElement.ExcelIndex = tableIndex;
            xlsElement.ExcelFlags = unknownFlag;

            if (xlsElement.Type == XlsType.AiInit)
            {
                xlsElement.ExcelUnknown = excelUnknown;
            }

            //switch (xlsElement.Type)
            //{
            //    case XlsType.ExcelIndex: // 20
            //    case XlsType.FolderCode: // 21
            //    case XlsType.ExcelIndexArray: // 40
            //    case XlsType.Qualities: // 45
            //        xlsElement.Index = tableIndex;
            //        xlsElement.IndexUnknown = unknownFlag;
            //        return;

            //    case XlsType.AiInit: // 26
            //        xlsElement.Index = tableIndex;
            //        xlsElement.IndexUnknown = unknownFlag;
            //        xlsElement.ExcelDefaultQ = excelDefault;
            //        return;

            //    default:
            //        throw new Exceptions.UnexpectedErrorException(xlsElement.Type + " excel type usage not expected/implemented.");
            //}


            //if ((int)xlsElement.Type < 18) return; // 1-17

            //if ((int)xlsElement.Type <= 24) // 19-24 // 19, 20 (ExcelIndex), 21 (FolderCode), 22, 23, 24
            //{
            //    if (xlsElement.Index == null && xlsElement.IndexUnknown == null && tableIndex < 0x200 && unknownFlag < 4)
            //    {
            //        xlsElement.Index = tableIndex;
            //        xlsElement.IndexUnknown = unknownFlag;
            //    }

            //    return;
            //}

            //if ((int)xlsElement.Type != 26) // (!= AiInit)
            //{ // 25, 27-56
            //    if ((int)xlsElement.Type <= 37 || (int)xlsElement.Type > 45) return;

            //    if (xlsElement.Index == null && xlsElement.IndexUnknown == null && tableIndex < 0x200 && unknownFlag < 4)
            //    {
            //        xlsElement.Index = tableIndex;
            //        xlsElement.IndexUnknown = unknownFlag;
            //    }

            //    return;
            //}

            //// 26 (AiInit)
            //if (xlsElement.Index == null && xlsElement.IndexUnknown == null && xlsElement.ExcelDefaultQ == 0 && tableIndex < 0x200 && unknownFlag < 4)
            //{
            //    xlsElement.Index = tableIndex;
            //    xlsElement.IndexUnknown = unknownFlag;
            //    xlsElement.ExcelDefaultQ = excelDefault;
            //}
        }

        //private static void _XlsSetEnumStuff(XlsColumn xlsElement, Dictionary<String, int> enums, int enumCount)
        //{
        //    if ((uint)(xlsElement.Type - 27) > 3) return;  // Enum, EnumArray and 28, 27 (not defined)

        //    if (xlsElement.Index != null) return; // if already set, then return

        //    xlsElement.ExcelDefaultQ = enumCount;
        //    xlsElement.Index = enums;

        //    // checks the/for default in here as well, but no point as all it appears to do is flag the element with XlsFlags.Enum - which it already has
        //}

        private static void _GenerateTableHash(XlsTable xlsTable, UInt32 scriptsHash)
        {
            bool outputPerElement = false;
            if (xlsTable.DefinitionId == "STATE_DATadsfA")
            {
                outputPerElement = true;
            }

            xlsTable.TableHash = (uint)xlsTable.RowSize + 32;

            xlsTable.Columns = xlsTable.Columns.OrderBy(element => element.Name).ToList();
            foreach (XlsColumn xlsElement in xlsTable.Columns)
            {
                bool outputHashSteps = false;
                if (outputPerElement && xlsElement.Name == "UI icon badfck")
                {
                    outputHashSteps = true;
                }

                uint hash = _GenerateColumnHash(xlsElement, outputHashSteps);
                if (outputPerElement)
                {
                    Debug.WriteLine("Element[{0,-3}]: {1,-45} @ {2,4} hash = 0x{3:X8}\t\tTableHash: 0x{4:X8} -> 0x{5:X8}",
                                    xlsElement.ElementIndex, xlsElement.Name, xlsElement.Offset, hash, xlsTable.TableHash, xlsTable.TableHash + hash);
                }

                xlsTable.TableHash += hash;
            }

            if (xlsTable.HasScripts)
            {
                if (outputPerElement)
                {
                    Debug.WriteLine("Has Scripts: 0x{0:X8} -> 0x{1:X8}", xlsTable.TableHash, xlsTable.TableHash + scriptsHash);
                }
                xlsTable.TableHash += scriptsHash;
            }
        }

        private static uint _GenerateColumnHash(XlsColumn xlsColumn, bool outputHashSteps = false)
        {
            UInt32 hash = _GenerateStringHash(xlsColumn.Name, 0);
            if (outputHashSteps) Debug.WriteLine("Name: 0x{0:X8} [{1}]", hash, xlsColumn.Name);

            hash = Crypt.GetBytesHash(BitConverter.GetBytes((Int32)xlsColumn.Type), hash); // 4 bytes
            if (outputHashSteps) Debug.WriteLine("Type: 0x{0:X8} [{1} ({2})]", hash, (int)xlsColumn.Type, xlsColumn.Type);


            if (xlsColumn.Type != XlsType.StringOffset) // 2
            {
                hash = Crypt.GetBytesHash(BitConverter.GetBytes(xlsColumn.Offset), hash);
                if (outputHashSteps) Debug.WriteLine("Offset: 0x{0:X8} [{1}]", hash, xlsColumn.Offset);

                hash = Crypt.GetBytesHash(BitConverter.GetBytes(xlsColumn.Size), hash);
                if (outputHashSteps) Debug.WriteLine("Size: 0x{0:X8} [{1}]", hash, xlsColumn.Size);
            }


            hash = Crypt.GetBytesHash(BitConverter.GetBytes(xlsColumn.Count), hash);
            if (outputHashSteps) Debug.WriteLine("Count: 0x{0:X8} [{1}]", hash, xlsColumn.Count);

            hash = Crypt.GetBytesHash(BitConverter.GetBytes((Int32)xlsColumn.Flags), hash);
            if (outputHashSteps) Debug.WriteLine("Flags: 0x{0:X8} [{1}]", hash, xlsColumn.Flags);


            switch (xlsColumn.Type)
            {
                case XlsType.String: // 1
                case XlsType.StringOffset: // 2
                case XlsType.ExcelIndex: // 20
                case XlsType.FolderCode: // 21
                case XlsType.AiInit: // 26
                case XlsType.ExcelIndexArray: // 40
                case XlsType.StringIndex: // 46
                case XlsType.TugboatUnknown: // 47
                case XlsType.UnitType: // 54
                    if (xlsColumn.Default != null)
                    {
                        hash = _GenerateStringHash((String)xlsColumn.Default, hash);
                        if (outputHashSteps) Debug.WriteLine("Default: 0x{0:X8} [{1}]", hash, xlsColumn.Default);
                    }
                    return _GenerateIndexSegmentHash(xlsColumn, hash);


                case XlsType.GroupStyle: // 3
                    return _GenerateDefaultSegmentHash(xlsColumn, hash);


                case XlsType.Flag: // 4
                case XlsType.Byte06: // 6
                case XlsType.Int32: // 9
                case XlsType.Bool: // 11
                case XlsType.Float: // 12
                case XlsType.Byte0D: // 13
                case XlsType.CodeInt32: // 15
                case XlsType.CodeInt16: // 16
                case XlsType.CodeInt8: // 17
                case XlsType.Int32Default: // 25
                case XlsType.Int32Array: // 35
                case XlsType.UseFixedDRLGSeed: // 36
                case XlsType.FloatArray: // 37
                case XlsType.Qualities: // 45
                    hash = _GenerateDefaultSegmentHash(xlsColumn, hash);
                    return _GenerateIndexSegmentHash(xlsColumn, hash);


                case XlsType.Enum: // 29
                case XlsType.EnumArray: // 30
                    //if (xlsColumn.Default != null && xlsColumn.Default is String) // enums can't have a default assigned anyways
                    //{
                    //    hash = _GenerateStringHash((String)xlsColumn.Default, hash);
                    //    if (outputHashSteps) Debug.WriteLine("Default: 0x{0:X8} [{1}]", hash, xlsColumn.Default);
                    //}

                    Debug.Assert(xlsColumn.Default != null);
                    Dictionary<String, int> enums = (Dictionary<String, int>)xlsColumn.Default;

                    hash = Crypt.GetBytesHash(BitConverter.GetBytes(enums.Count), hash);
                    foreach (KeyValuePair<String, int> enumVal in enums.OrderBy(kvp => kvp.Key, StringComparer.Ordinal))
                    {
                        hash = Crypt.GetStringHash(enumVal.Key, hash);
                        hash = Crypt.GetBytesHash(BitConverter.GetBytes(enumVal.Value - 1), hash);
                    }
                    return hash;


                case XlsType.Script: // 50
                case XlsType.MaxSlots: // 51
                case XlsType.Unknown1: // 52
                case XlsType.AimHeight: // 53
                case XlsType.BaseRow: // 56
                    hash = _GenerateDefaultSegmentHash(xlsColumn, hash);
                    return _GenerateIndexSegmentHash(xlsColumn, hash);


                case XlsType.File: // 48
                    hash = Crypt.GetStringHash(xlsColumn.FilePath, hash);
                    if (outputHashSteps) Debug.WriteLine("Index: 0x{0:X8} [{1}]", hash, xlsColumn.FilePath);
                    return hash;


                case XlsType.MultipleRelations: // 55
                    return hash;


                default:
                    throw new Exceptions.UnexpectedErrorException(xlsColumn.Type + " excel type usage not expected/implemented.");
            }

            //if (type <= 30) // 1-30
            //{
            //    //if (type >= 27) // 27, 28, 29 (Enum), 30 (EnumArray)
            //    //{
            //    //    if (xlsColumn.Default != null)
            //    //    {
            //    //        hash = Crypt.GeBytesHash(BitConverter.GetBytes((int)xlsColumn.Default), hash);
            //    //        if (outputHashSteps) Debug.WriteLine("Default: 0x{0:X8} [{1}]", hash, xlsColumn.Default);
            //    //    }
            //    //}
            //    //else // 1-26
            //    //{
            //    //                    if (type == 1 || type == 2) // 1 (String), 2 (StringOffset)
            //    //                    {
            //    //                        if (xlsColumn.Default != null)
            //    //                        {
            //    //                            hash = Crypt.GetStringHash((String)xlsColumn.Default, hash);
            //    //                            if (outputHashSteps) Debug.WriteLine("Default: 0x{0:X8} [{1}]", hash, xlsColumn.Default);
            //    //                        }

            //    //                        /*
            //    //XLS_GenereateElementHash+91   008 push    eax                             ; baseHash
            //    //XLS_GenereateElementHash+92   00C push    18h                             ; 24 bytes (6x Int32)
            //    //XLS_GenereateElementHash+94   010 pop     edx
            //    //XLS_GenereateElementHash+95   00C lea     ecx, [esi+30h]                  ; index
            //    //XLS_GenereateElementHash+98   00C call    CryptBytesHash                  
            //    //                         */
            //    //                        return _GenerateIndexSegmentHash(xlsColumn, hash);
            //    //                    }

            //    //if (type != 3) // GroupStyle
            //    //{ // 4-26
            //    //    //if (type > 17 && (type <= 24 || type == 26)) // 18, 19, 20 (ExcelIndex), 21 (FolderCode), 22, 23, 24, 26 (AiInit)
            //    //    //{
            //    //    //    if (xlsColumn.Default != null)
            //    //    //    {
            //    //    //        hash = Crypt.GetStringHash((String)xlsColumn.Default, hash);
            //    //    //        if (outputHashSteps) Debug.WriteLine("Default: 0x{0:X8} [{1}]", hash, xlsColumn.Default);
            //    //    //    }

            //    //    //    return _GenerateIndexSegmentHash(xlsColumn, hash);
            //    //    //}

            //    //    // 4 (Flag), 5, 6 (Byte06), 7, 8, 9 (Int32), 10, 11 (Bool), 12 (Float), 13 (Byte0D), 14 (CodeInt32), 15, 16 (CodeInt16), 17 (CodeInt8), 25 (Int32Default)
            //    //    hash = _GenerateDefaultSegmentHash(xlsColumn, hash);
            //    //    return _GenerateIndexSegmentHash(xlsColumn, hash);
            //    //}

            //    // 3 (GroupStyle)
            //    //return _GenerateDefaultSegmentHash(xlsColumn, hash);
            //    //}

            //    // 27, 28, 29 (Enum), 30 (EnumArray)
            //    //Object index = xlsColumn.Index;
            //    //if (index == null) return hash;

            //    //Dictionary<String, int> enums = (Dictionary<String, int>)xlsColumn.Index;
            //    //hash = Crypt.GeBytesHash(BitConverter.GetBytes(enums.Count), hash);

            //    //foreach (KeyValuePair<String, int> enumVal in enums.OrderBy(kvp => kvp.Key, StringComparer.Ordinal))
            //    //{
            //    //    hash = Crypt.GetStringHash(enumVal.Key, hash);
            //    //    hash = Crypt.GeBytesHash(BitConverter.GetBytes(enumVal.Value - 1), hash);
            //    //}

            //    //return hash;
            //}

            //if (type > 49) // 50+
            //{
            //    //int isNotUnitType = type - 54;
            //    //if (isNotUnitType == 0) // 54 (UnitType)
            //    //{
            //    //    if (xlsColumn.Default != null)
            //    //    {
            //    //        hash = Crypt.GetStringHash((String)xlsColumn.Default, hash);
            //    //        if (outputHashSteps) Debug.WriteLine("Default: 0x{0:X8} [{1}]", hash, xlsColumn.Default);
            //    //    }

            //    //    return _GenerateIndexSegmentHash(xlsColumn, hash);
            //    //}

            //    //if (isNotUnitType == 1) return hash; // 55 (MultipleRelations)

            //    // 50 (Script), 51 (MaxSlots), 52 (Unknown1), 53 (AimHeight), 56 (BaseRow)
            //    //hash = _GenerateDefaultSegmentHash(xlsColumn, hash);
            //    //return _GenerateIndexSegmentHash(xlsColumn, hash);
            //}

            //if (type < 48) // 31-47
            //{
            //    //if (type >= 38 && (type <= 44 || type > 45 && type <= 47)) // 38, 39, 40 (ExcelIndexArray), 41, 42, 43, 44, 46 (StringIndex), 47 (TugboatUnknown)
            //    //{
            //    //    if (xlsColumn.Default != null)
            //    //    {
            //    //        hash = Crypt.GetStringHash((String)xlsColumn.Default, hash);
            //    //        if (outputHashSteps) Debug.WriteLine("Default: 0x{0:X8} [{1}]", hash, xlsColumn.Default);
            //    //    }

            //    //    return _GenerateIndexSegmentHash(xlsColumn, hash);
            //    //}

            //    // 31, 32, 33, 34, 35 (Int32Array), 36 (UseFixedDRLGSeed), 37 (FloatArray), 45 (Qualities)
            //    //hash = _GenerateDefaultSegmentHash(xlsColumn, hash);
            //    //return _GenerateIndexSegmentHash(xlsColumn, hash);
            //}

            //return hash;

            // 48 (File), 49
            //if (xlsColumn.FilePath != null)
            //{
            //    hash = Crypt.GetStringHash(xlsColumn.FilePath, hash);
            //    if (outputHashSteps) Debug.WriteLine("Index: 0x{0:X8} [{1}]", hash, xlsColumn.Index);
            //}

            //return hash;
        }

        private static UInt32 _GenerateStringHash(String str, UInt32 hash)
        {
            const char rsinglequote = (char)65533;
            if (str.IndexOf(rsinglequote) == -1) // if not found
            {
                hash = Crypt.GetStringHash(str, hash);
            }
            else // the initial export converts the 0x92 (’) value to the Ascii Æ, which String read in then converts it to 65533 (0xFFFD) '�' char
            {    // even if you force/replace before hand all Æ to the proper 0x92 (’) value, it still gets converted to the Unicode value (0x2019) - even if you cast str[i] as a char, and you get 8217 (0x2019)
                foreach (char c in str)
                {
                    byte[] bytes = new[] { (byte)c };
                    if (c == rsinglequote)
                    {
                        bytes = new[] { (byte)0x92 };
                    }

                    hash = Crypt.GetBytesHash(bytes, hash);
                }
            }

            return hash;
        }

        private static UInt32 _GenerateDefaultSegmentHash(XlsColumn xlsElement, UInt32 hash)
        {
            // damn C# is a pain in the ass sometimes
            // is it so much to ask for a pinned structure no matter what's in it - I don't want to access the object references damn it
            /*
XLS_GenereateElementHash+FC   008 push    eax                             ; baseHash
XLS_GenereateElementHash+FD   00C push    10h                             ; 16 bytes (4x Int32)
XLS_GenereateElementHash+FF   010 lea     ecx, [esi+20h]                  ; defaultValue
XLS_GenereateElementHash+102  010 pop     edx
             */

            byte[] defaultBytes;
            byte[] zeroBytes = new byte[12]; // 3x Int32 for Field24, Field28 and Field2C which are always zero during hashing

            if (xlsElement.Default == null)
            {
                defaultBytes = new byte[4];
            }
            else
            {
                switch (xlsElement.Type)
                {
                    case XlsType.Flag: // 4
                    case XlsType.Bool: // 11
                        defaultBytes = new byte[] { (byte)((bool)xlsElement.Default ? 1 : 0), 0, 0, 0 };
                        break;

                    case XlsType.Byte06: // 6
                    case XlsType.Byte0D: // 13
                    case XlsType.CodeInt8: // 17
                        defaultBytes = BitConverter.GetBytes((int)(byte)xlsElement.Default);
                        break;

                    case XlsType.Float: // 12
                    case XlsType.FloatArray: // 37
                    case XlsType.AimHeight: // 53
                        defaultBytes = BitConverter.GetBytes((float)xlsElement.Default);
                        break;

                    case XlsType.CodeInt16: // 16
                        defaultBytes = BitConverter.GetBytes((int)(short)xlsElement.Default);
                        break;

                    default:
                        defaultBytes = BitConverter.GetBytes((int)xlsElement.Default);
                        break;
                }
            }

            hash = Crypt.GetBytesHash(defaultBytes, hash);
            return Crypt.GetBytesHash(zeroBytes, hash); // Field24, Field28 and Field2C
        }

        /// <summary>
        /// This function hashes the last 6xInt32 values in the XlsColumn structure (on the client)
        /// </summary>
        /// <param name="xlsColumn">The column to hash.</param>
        /// <param name="hash">The current/base hash.</param>
        /// <returns>The hashed segment onto the current/base hash.</returns>
        private static UInt32 _GenerateIndexSegmentHash(XlsColumn xlsColumn, UInt32 hash)
        {
            /*
XLS_GenereateElementHash+10B  00C push    eax                             ; baseHash
XLS_GenereateElementHash+10C  010 push    18h                             ; 24 bytes (6x Int32)
XLS_GenereateElementHash+10E  014 pop     edx
XLS_GenereateElementHash+10F  010 mov     ecx, edi                        ; index
XLS_GenereateElementHash+111  010 call    CryptBytesHash                  ; Call 
             */

            byte[] field30Bytes;
            byte[] field34Bytes;
            byte[] field38Bytes;
            byte[] fields3Cto44 = new byte[12]; // 3x Int32 for Field3C, Field40 and Field44 which are always zero during hashing

            switch (xlsColumn.Type)
            {
                case XlsType.String: // 1
                case XlsType.StringOffset: // 2
                    field30Bytes = new byte[] { (byte)(xlsColumn.StringBool ? 1 : 0), 0, 0, 0 };
                    field34Bytes = BitConverter.GetBytes(xlsColumn.String64Offset);
                    field38Bytes = new byte[4];
                    break;

                case XlsType.Flag: // 4
                    field30Bytes = BitConverter.GetBytes(xlsColumn.FlagIndex);
                    field34Bytes = new byte[4];
                    field38Bytes = new byte[4];
                    break;

                case XlsType.Float: // 12
                    field30Bytes = BitConverter.GetBytes(xlsColumn.FloatMin);
                    field34Bytes = BitConverter.GetBytes(xlsColumn.FloatMax);
                    field38Bytes = new byte[4];
                    break;

                case XlsType.ExcelIndex: // 20
                case XlsType.FolderCode: // 21
                case XlsType.AiInit: // 26
                case XlsType.ExcelIndexArray: // 40
                case XlsType.Qualities: // 45
                    field30Bytes = BitConverter.GetBytes(xlsColumn.ExcelIndex);
                    field34Bytes = BitConverter.GetBytes(xlsColumn.ExcelFlags);
                    field38Bytes = BitConverter.GetBytes(xlsColumn.ExcelUnknown);
                    break;

                case XlsType.Unknown1:
                case XlsType.AimHeight:
                    field30Bytes = BitConverter.GetBytes(xlsColumn.UnknownIndex);
                    field34Bytes = new byte[4];
                    field38Bytes = new byte[4];
                    break;

                default:
                    return Crypt.GetBytesHash(new byte[24], hash);
            }

            //if (field30Bytes == null)
            //{
            //    //if (xlsColumn.Type == XlsType.Float && xlsColumn.IndexUnknown != null)
            //    //{
            //    //    indexBytes = BitConverter.GetBytes((float) xlsColumn.Index);
            //    //    indexUnknownBytes = BitConverter.GetBytes((float) xlsColumn.IndexUnknown);
            //    //}
            //    //else if (xlsColumn.IndexUnknown == null)
            //    //{
            //    //    field34Bytes = new byte[4];
            //    //}
            //    //else
            //    //{
            //    //    field34Bytes = BitConverter.GetBytes((int) xlsColumn.IndexUnknown);
            //    //}

            //    //if (field30Bytes == null)
            //    //{
            //    //    field30Bytes = BitConverter.GetBytes(xlsColumn.Index == null ? 0 : (int) xlsColumn.Index);
            //    //}

            //    field30Bytes = BitConverter.GetBytes(xlsColumn.Index == null ? 0 : (int)xlsColumn.Index);
            //    field34Bytes = new byte[4];
            //    field38Bytes = new byte[4];
            //}

            hash = Crypt.GetBytesHash(field30Bytes, hash); // Field30
            hash = Crypt.GetBytesHash(field34Bytes, hash); // Field34
            hash = Crypt.GetBytesHash(field38Bytes, hash); // Field38
            return Crypt.GetBytesHash(fields3Cto44, hash); // Field3C, Field40, Field44
        }

        #endregion
    }
}