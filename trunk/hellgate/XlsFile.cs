using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

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

            UnknownFlag = (1 << 10),    // 1024         // seen in SOUND_GROUP and WARDROBE_APPEARANCE_GROUP; unknown usage (something relating to null/empty/ignore field? (random guess))
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private class XlsElement
        {
            public String Name;             // element name
            public XlsType Type;            // element type
            public int Offset;              // row offset
            public int Count;               // number of elements
            public int Size;                // byte size of element
            public int ElementIndex;        // the order that the element was defined during initial definition stages
            public XlsFlags Flags;          // type flags
            public int Field1C;
            public Object Default;          // default value
            public int Field24;
            public int Field28;
            public int Field2C;
            public Object Index;            // excel index or flag index depending on XlsType
            public Object IndexUnknown;     // this has something to do with index stuff... Used in strings and excel index types and more
            public int ExcelDefaultQ;       // I think this is the default excel index...
            public int Field3C;
            public int Field40;
            public int Field44;
        }

        private class XlsTable
        {
            public String DefinitionId;             // the id used internally (e.g. "EXCEL_TABLE_DEFINITION" for "EXCELTABLES")
            public int RowSize;                     // number of bytes per row
            public List<XlsElement> Elements = new List<XlsElement>();
            public int ElementCount { get { return Elements.Count; } }
            public XlsElement StringIndexElement;
            public XlsElement RelationalElement;
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

        public static void GenerateExcelTables(String[] source, byte[] exeBytes, String hellgatePath, FileManager.ClientVersions clientVersion)
        {
            _exeBytes = exeBytes;
            Dictionary<String, String> functionNames = new Dictionary<String, String>
                                                           {
                                                               {"sub_6B8751", "XLS_ExcelScript"},
                                                               {"sub_6B8768", "XLS_AssignStringIndex"},
                                                               {"sub_6B878B", "XLS_RelationalElement"}
                                                           };

            Regex argsRegex = new Regex(@"(?:\(.*\))*(\w*)\((?:\(\w* \*\))*(.*)\)");
            Regex pointerRegex = new Regex(@"->(\w*) = (?:\(.*\))?(\w*)");
            List<XlsTable> tables = new List<XlsTable>();

            int lineCount = source.Length;
            for (int i = 0; i < lineCount; i++)
            {
                if (!source[i].Contains("XLS_Key") || source[i].StartsWith("//")) continue;
                if (source[i].Contains("XLS_Key<")) continue; // the function definition
                if (source[i].Contains("XLS_KeyUnknownMemMove")) continue; // just another function
                Debug.Write("\n");

                XlsTable xlsTable = new XlsTable();
                bool inTableDefinition = true;
                i--;
                while (inTableDefinition && i++ < lineCount)
                {
                    Match match = argsRegex.Match(source[i]);
                    String func = match.Groups[1].Value;
                    String[] args = match.Groups[2].Value.Split(new[] { ", " }, StringSplitOptions.None);

                    if (String.IsNullOrEmpty(func))
                    {
                        Match pMatch = pointerRegex.Match(source[i]); // mostly a curiosity thing
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
                                throw new NotImplementedException(varName + " not implemented.");
                        }

                        Debug.WriteLine("xlsTable.{0} = {1}", varName, funcName);
                        continue;
                    }

                    Debug.WriteLine(match);
                    Debug.Assert(!String.IsNullOrEmpty(func));

                    String replaceName;
                    if (functionNames.TryGetValue(func, out replaceName))
                    {
                        func = replaceName;
                    }

                    XlsElement xlsElement = null;
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

                        case "XLS_String": // 1 // void __cdecl XLS_String(XLS_TableDefinition *pTableDefinition, char *szName, int offset, int charCount, int index)
                            Debug.Assert(args.Length == 5);
                            xlsElement = new XlsElement
                            {
                                Name = _GetArg<string>(args, 1),
                                Offset = _GetArg<int>(args, 2),
                                Type = XlsType.String,
                                Count = _GetArg<int>(args, 3),
                                Size = 1,
                                Default = null,
                            };

                            // applicable only to String and StringOffset/StringOffsetDefault
                            // XLS_ElementDefinition *__usercall XLS_Def_SetDefinitionDefault_String<eax>(XLS_ElementDefinition *pElement<eax>, int index, int indexUnknown)
                            _XlsSetDefStringStuff(xlsElement, _GetArg<int>(args, 4), -1); // XLS_Def_SetDefinitionDefault_String(&element, index, -1);
                            break;

                        case "XLS_StringOffset": // 2 // void __usercall XLS_StringOffset(XLS_TableDefinition *pTable, char *szName, int offset<esi>, int unknownBool)
                            Debug.Assert(args.Length == 4);
                            xlsElement = new XlsElement
                            {
                                Name = _GetArg<string>(args, 1),
                                Offset = _GetArg<int>(args, 2),
                                Type = XlsType.StringOffset,
                                Count = 1,
                                Size = 4,
                                Default = null
                            };

                            // applicable only to String and StringOffset/StringOffsetDefault
                            // XLS_ElementDefinition *__usercall XLS_Def_SetDefinitionDefault_String<eax>(XLS_ElementDefinition *pElement<eax>, int unknown1, int indexUnknown)
                            _XlsSetDefStringStuff(xlsElement, _GetArg<int>(args, 3), _GetArg<int>(args, 2)); // XLS_Def_SetDefinitionDefault_String(&element, unknownBool, offset);
                            break;

                        case "XLS_StringOffsetDefault": // 2 // void __cdecl XLS_StringOffsetDefault(XLS_TableDefinition *pTable, char *szName, int offset, int defaultSomething)
                            Debug.Assert(args.Length == 4);
                            xlsElement = new XlsElement
                            {
                                Name = _GetArg<string>(args, 1),
                                Offset = _GetArg<int>(args, 2),
                                Type = XlsType.StringOffset,
                                Count = 1,
                                Size = 4,
                                Default = null
                            };

                            // applicable only to String and StringOffset/StringOffsetDefault
                            // XLS_ElementDefinition *__usercall XLS_Def_SetDefinitionDefault_String<eax>(XLS_ElementDefinition *pElement<eax>, int index, int indexUnknown)
                            _XlsSetDefStringStuff(xlsElement, 0, _GetArg<int>(args, 3)); // XLS_Def_SetDefinitionDefault_String(&element, 0, defaultSomething);
                            break;

                        case "XLS_GroupStyle": // 3 // void __cdecl XLS_GroupStyle(XLS_TableDefinition *pTable, char *szName, int offset)
                            Debug.Assert(args.Length == 3);
                            xlsElement = new XlsElement
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
                            xlsElement = new XlsElement
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
                            xlsElement = new XlsElement
                            {
                                Name = _GetArg<string>(args, 1),
                                Offset = _GetArg<int>(args, 2),
                                Size = (int)(_GetArg<uint>(args, 3) >> 3) + 1,
                                Type = XlsType.Flag,
                                Count = 1,
                                Default = _GetArg<bool>(args, 4),
                                Index = (int)_GetArg<uint>(args, 3)
                            };
                            break;

                        case "XLS_Byte_06": // 6 // void __cdecl XLS_Byte_06(XLS_TableDefinition *pTable, char *szName, int offset) [calls XLS_Byte]
                            Debug.Assert(args.Length == 3);
                            xlsElement = new XlsElement
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
                            xlsElement = new XlsElement
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
                            xlsElement = new XlsElement
                            {
                                Name = _GetArg<string>(args, 1),
                                Type = XlsType.Bool,
                                Offset = _GetArg<int>(args, 2),
                                Count = 1,
                                Size = 4,
                                Default = _GetArg<bool>(args, 3)
                            };
                            break;

                        case "XLS_Float1": // 12 // void __cdecl XLS_Float1(XLS_TableDefinition *pTableDefinition, char *szName, int offset, float defaultValue)
                            Debug.Assert(args.Length == 4);
                            xlsElement = new XlsElement
                            {
                                Name = _GetArg<string>(args, 1),
                                Offset = _GetArg<int>(args, 2),
                                Type = XlsType.Float,
                                Count = 1,
                                Size = 4,
                                Default = _GetArg<float>(args, 3)
                            };
                            break;

                        case "XLS_Float2": // 12 // void __cdecl XLS_Float2(XLS_TableDefinition *pTable, char *szName, int offset)
                            Debug.Assert(args.Length == 3);
                            xlsElement = new XlsElement
                            {
                                Name = _GetArg<string>(args, 1),
                                Offset = _GetArg<int>(args, 2),
                                Type = XlsType.Float,
                                Count = 1,
                                Size = 4,
                                Default = 0.0f,

                                Index = 0.0f,
                                IndexUnknown = 100.0f
                            };
                            break;

                        case "XLS_Byte_0D": // 13 // void __cdecl XLS_Byte_0D(XLS_TableDefinition *pTable, char *szName, int offset) [calls XLS_Byte]
                            Debug.Assert(args.Length == 3);
                            xlsElement = new XlsElement
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
                            xlsElement = new XlsElement
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
                            xlsElement = new XlsElement
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
                            xlsElement = new XlsElement
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
                            xlsElement = new XlsElement
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
                            xlsElement = new XlsElement
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
                            switch (clientVersion)
                            {
                                case FileManager.ClientVersions.SinglePlayer: tableIndex = 0x4E; break;
                                case FileManager.ClientVersions.TestCenter:   tableIndex = 0x4F; break;
                                case FileManager.ClientVersions.Resurrection: tableIndex = 0x52; break;
                            }

                            // applicable only to ExcelIndex and ExcelIndexArray, AiInit, Qualities, and FolderCode
                            // void __usercall XLS_Def_ExcelSomething(XLS_ElementDefinition *pElement<eax>, unsigned int unknown1<edx>, unsigned int tableIndex<esi>, int excelDefaultQ)                            // void __usercall XLS_Def_ExcelSomething(XLS_ElementDefinition *pElement<eax>, unsigned int unknown1<edx>, unsigned int tableIndex<esi>, int excelDefaultQ)
                            _XlsSetDefExcelStuff(xlsElement, 0, tableIndex /*LEVEL_FILE_PATHS*/, -1); // XLS_Def_ExcelSomething(&element, 0, 0x52u, -1);
                            break;

                        case "XLS_Int32Default": // 25 // void __cdecl XLS_Int32Default(XLS_TableDefinition *pTableDefinition, char *szName, int offset) [calls XLS_Int32Array]
                            Debug.Assert(args.Length == 3);
                            xlsElement = new XlsElement
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
                            xlsElement = new XlsElement
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
                            switch (clientVersion)
                            {
                                case FileManager.ClientVersions.SinglePlayer: aiInitIndex = 0x46; break;
                                case FileManager.ClientVersions.TestCenter:   aiInitIndex = 0x47; break;
                                case FileManager.ClientVersions.Resurrection: aiInitIndex = 0x48; break;
                            }

                            // applicable only to ExcelIndex and ExcelIndexArray, AiInit, Qualities, and FolderCode
                            // void __usercall XLS_Def_ExcelSomething(XLS_ElementDefinition *pElement<eax>, unsigned int unknown1<edx>, unsigned int tableIndex<esi>, int excelDefaultQ)                            // void __usercall XLS_Def_ExcelSomething(XLS_ElementDefinition *pElement<eax>, unsigned int unknown1<edx>, unsigned int tableIndex<esi>, int excelDefaultQ)
                            _XlsSetDefExcelStuff(xlsElement, 0, aiInitIndex /*AI_INIT*/, _GetArg<int>(args, 2)); // XLS_Def_ExcelSomething(&element, 0, 0x48u, excelDefaultQ);
                            break;

                        case "XLS_Enum": // 29 // void __usercall XLS_Enum(XLS_TableDefinition *pTable, char *szName, int offset, int enumCount, void *enums<eax>) [calls XLS_EnumArray]
                            Debug.Assert(args.Length == 5);
                            xlsElement = new XlsElement
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

                            _XlsSetEnumStuff(xlsElement, xlsEnum, _GetArg<int>(args, 3));
                            break;

                        case "XLS_EnumArray": // 29 (Enum) or 30 (EnumArray) // void __usercall XLS_EnumArray(XLS_TableDefinition *pTableDefinition, void *enums<eax>, int typeId, char *szName, int offset, int elementCount, int enumCount)
                            Debug.Assert(args.Length == 7);
                            xlsElement = new XlsElement
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

                            _XlsSetEnumStuff(xlsElement, xlsEnumArray, _GetArg<int>(args, 6));
                            break;

                        case "XLS_Int32Array": // 35 // void __cdecl XLS_Int32Array(XLS_TableDefinition *pTableDefinition, int typeId, char *szName, int offset, int elementCount, int defaultValue)
                            Debug.Assert(args.Length == 6);
                            xlsElement = new XlsElement
                            {
                                Name = _GetArg<string>(args, 2),
                                Type = _GetArg<XlsType>(args, 1),
                                Offset = _GetArg<int>(args, 3),
                                Count = _GetArg<int>(args, 4),
                                Size = 4,
                                Default = _GetArg<int>(args, 5),
                            };
                            break;

                        case "XLS_Int32Array2": // 35 // void __cdecl XLS_Int32Array2(XLS_TableDefinition *pTable, char *szName, int offset, int elementCount, int defaultValue) [calls XLS_Int32Array]
                            Debug.Assert(args.Length == 5);
                            xlsElement = new XlsElement
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
                            xlsElement = new XlsElement
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
                            xlsElement = new XlsElement
                            {
                                Name = _GetArg<string>(args, 2),
                                Type = _GetArg<XlsType>(args, 1),
                                Offset = _GetArg<int>(args, 3),
                                Count = _GetArg<int>(args, 4),
                                Size = 4,
                                Default = null,
                                Flags = XlsFlags.ExcelIndex
                            };

                            // applicable only to ExcelIndex and ExcelIndexArray, AiInit, Qualities, and FolderCode
                            // void __usercall XLS_Def_ExcelSomething(XLS_ElementDefinition *pElement<eax>, unsigned int unknown1<edx>, unsigned int tableIndex<esi>, int excelDefaultQ)
                            _XlsSetDefExcelStuff(xlsElement, _GetArg<int>(args, 6), _GetArg<int>(args, 5), -1); // XLS_Def_ExcelSomething(&element, unknown1, tableIndex, -1);
                            break;

                        case "XLS_Qualities": // 45 // void __usercall XLS_Qualities(XLS_TableDefinition *pTable<edi>)
                            Debug.Assert(args.Length == 1);

                            // only found in UNIT_DATA
                            xlsElement = new XlsElement
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
                            switch (clientVersion)
                            {
                                case FileManager.ClientVersions.SinglePlayer: qualitiesIndex = 0x43; xlsElement.Offset = 2576; break;
                                case FileManager.ClientVersions.TestCenter:   qualitiesIndex = 0x44; xlsElement.Offset = 2708; break;
                                case FileManager.ClientVersions.Resurrection: qualitiesIndex = 0x45; xlsElement.Offset = 2260; break;
                            }

                            // applicable only to ExcelIndex and ExcelIndexArray, AiInit, Qualities, and FolderCode
                            // void __usercall XLS_Def_ExcelSomething(XLS_ElementDefinition *pElement<eax>, unsigned int unknown1<edx>, unsigned int tableIndex<esi>, int excelDefaultQ)
                            _XlsSetDefExcelStuff(xlsElement, 0, qualitiesIndex  /*ITEM_QUALITY*/, -1); // XLS_Def_ExcelSomething(&xlsDefinitionObj, 0, 0x45u, -1);

                            break;

                        case "XLS_StringIndex": // 46 // void __usercall XLS_StringIndex(XLS_TableDefinition *pTableDefinition<edi>, char *szName, int offset, int indexType(?))
                            Debug.Assert(args.Length == 4);
                            xlsElement = new XlsElement
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
                            xlsElement = new XlsElement
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
                            xlsElement = new XlsElement
                            {
                                Offset = 0,
                                Name = "file",
                                Type = XlsType.File,
                                Count = 1,
                                Size = 8,
                                ExcelDefaultQ = 0,
                                Index = @"data\palettes\", // this type is used only once for PALETTE_DATA
                                Default = null
                            };
                            break;

                        case "XLS_Script": // 50 // void __cdecl XLS_Script(XLS_TableDefinition *pTable, char *szName, int offset)
                            Debug.Assert(args.Length == 3);
                            xlsElement = new XlsElement
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
                            xlsElement = new XlsElement
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
                            xlsElement = new XlsElement
                            {
                                Offset = 0,
                                Size = 0,
                                Name = _GetArg<string>(args, 1),
                                Type = XlsType.Unknown1,
                                Count = 1,
                                Default = _GetArg<int>(args, 3)
                            };

                            _XlsSetDefAimHeightStuff(xlsElement, _GetArg<int>(args, 2), 0);
                            xlsTable.HasUnknown = true;
                            break;

                        case "XLS_AimHeight": // 53 // void __usercall XLS_AimHeight(XLS_TableDefinition *pTable<edi>)
                            Debug.Assert(args.Length == 1);
                            xlsElement = new XlsElement
                            {
                                Offset = 0,
                                Size = 0,
                                Name = "aim_height",
                                Type = XlsType.AimHeight,
                                Count = 1,
                                Default = 0.0f
                            };

                            int aimHeightIndex = -1;
                            switch (clientVersion)
                            {
                                case FileManager.ClientVersions.SinglePlayer: aimHeightIndex = 266; break;
                                case FileManager.ClientVersions.TestCenter:   aimHeightIndex = 281; break;
                                case FileManager.ClientVersions.Resurrection: aimHeightIndex = 284; break;
                            }

                            _XlsSetDefAimHeightStuff(xlsElement, aimHeightIndex, 0);
                            xlsTable.HasUnknown = true;
                            break;

                        case "XLS_UnitTypeArray": // 54 // void __cdecl XLS_UnitTypeArray(XLS_TableDefinition *pTableDefinition, char *szName, int offset, int elementCount, char *defaultValue)
                            Debug.Assert(args.Length == 5);
                            xlsElement = new XlsElement
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
                            xlsElement = new XlsElement
                            {
                                Name = _GetArg<string>(args, 1),
                                Offset = _GetArg<int>(args, 2),
                                Type = XlsType.MultipleRelations,
                                Size = _GetArg<int>(args, 3),
                                ExcelDefaultQ = 0,
                                //Index = _GetArg<String>(args, 4),
                                Default = null
                            };
                            break;

                        case "XLS_BaseRow": // 56 // void __usercall XLS_BaseRow(XLS_TableDefinition *pTable<esi>, int offset)
                            Debug.Assert(args.Length == 2);
                            xlsElement = new XlsElement
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
                            xlsTable.Elements[xlsTable.ElementCount - 1].Flags |= (XlsFlags)_GetArg<int>(args, 1);
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
                            xlsTable.StringIndexElement = xlsTable.Elements.FirstOrDefault(element => element.Name == _GetArg<string>(args, 1));
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
                            xlsTable.RelationalElement = xlsTable.Elements.FirstOrDefault(element => element.Name == "isa");
                            break;

                        case "XLS_SortColumns": // void __usercall XLS_SortColumns(XLS_TableDefinition *pTable, char *szColumn<edx>, int order, int unknown1, char *szByColumn1, char* szByColumn2)
                            Debug.Assert(args.Length == 6);
                            String columnName = _GetArg<string>(args, 1);
                            int order = _GetArg<int>(args, 2);
                            int unknown1 = _GetArg<int>(args, 3);
                            String byColumn1 = _GetArg<string>(args, 4);
                            String byColumn2 = _GetArg<string>(args, 5);

                            XlsElement sortElement;
                            if (columnName != null && (sortElement = xlsTable.Elements.FirstOrDefault(element => String.Compare(element.Name, columnName, true) == 0)) != null) sortElement.Flags |= XlsFlags.Sort;
                            if (byColumn1 != null && (sortElement = xlsTable.Elements.FirstOrDefault(element => String.Compare(element.Name, byColumn1, true) == 0)) != null) sortElement.Flags |= XlsFlags.Sort;
                            if (byColumn2 != null && (sortElement = xlsTable.Elements.FirstOrDefault(element => String.Compare(element.Name, byColumn2, true) == 0)) != null) sortElement.Flags |= XlsFlags.Sort;
                            break;

                        case "XLS_GenerateStructureId": // Appears at the end of a definition...
                            inTableDefinition = false;
                            break;

                        default:
                            throw new NotImplementedException(func + " not implemented.");
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

                    xlsElement.ElementIndex = xlsTable.Elements.Count;
                    xlsTable.Elements.Add(xlsElement);
                }

                tables.Add(xlsTable);
            }


            // get table details
            Debug.Write("\n");
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

                Match match = argsRegex.Match(line);
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
                Debug.WriteLine("Table[{0}]: {1} ({2}.txt.cooked), u1 = {3}, u2 = {4}", tableIndex, definitionId, fileName, unknown1, unknown2);

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

            Debug.WriteLine("\nLoading excel files...");
            Dictionary<String, uint> excelHashes = new Dictionary<String, uint>();
            FileManager fileManager = new FileManager(hellgatePath, clientVersion);
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

            Debug.WriteLine("\nChecking table hash generation...");
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
                    Debug.WriteLine("Table[{0}]: {1} table not found. Hash = 0x{2:X8}", xlsTable.TableIndex, xlsTable.DefinitionId, xlsTable.TableHash);
                    tablesNotFound++;
                }
                else if (hash == xlsTable.TableHash)
                {
                    Debug.WriteLine("Table[{0}]: {1} -> [0x{2:X8} == 0x{3:X8}]", xlsTable.TableIndex, xlsTable.DefinitionId, xlsTable.TableHash, hash);
                    tablesMatched++;
                }
                else
                {
                    Debug.WriteLine("Table[{0}]: {1} -> [0x{2:X8} != 0x{3:X8}]", xlsTable.TableIndex, xlsTable.DefinitionId, xlsTable.TableHash, hash);
                    Debug.WriteLine("Warning: Table hashes do not match.\n");
                    tablesNotMatched++;
                }
            }

            Debug.WriteLine("{0} tables: {1} not found, {2} hash check matches, {3} hash check fails.", tables.Count, tablesNotFound, tablesMatched, tablesNotMatched);
        }

        private static byte[] _exeBytes;
        private static readonly Regex StringRegex = new Regex("\"(.*)\"");
        private static readonly Regex AddressRegex = new Regex(@"(?:off|dword|byte|word)_(\w*)");

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
                    else if (arg.Contains("off_") || arg.Contains("word_") || arg.Contains("byte_")) // decompiler "errors" (usually 1-3 char length strings, so the decompiler mistakes them for a pointer
                    {
                        val = _GetAsciiStringFromClient(arg);
                    }
                    else if (!String.IsNullOrEmpty(arg)) throw new NotImplementedException("arg is not empty, but no value parsed!");
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
                if (arg.Contains("0x")) // is it hex?
                {
                    val = UInt32.Parse(arg.Replace("u", "").Replace("0x", ""), NumberStyles.HexNumber);
                }
                else
                {
                    val = UInt32.Parse(arg.Replace("u", "")); // uints have a "u" at the end sometimes
                }
            }
            else if (t == typeof(int))
            {
                if (arg.Contains("0x")) // is it hex?
                {
                    val = Int32.Parse(arg.Replace("u", "").Replace("0x", ""), NumberStyles.HexNumber);
                }
                else
                {
                    val = Int32.Parse(arg);
                }
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

        private const int ExeBase = 0x400000;
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

        private static void _XlsSetDefStringStuff(XlsElement xlsElement, int index, int indexUnknown)
        {
            if ((uint)(xlsElement.Type - 1) > 1) return; // if String or StringOffset/StringOffsetDefault

            if (xlsElement.Index != null) return; // if already set, then return

            xlsElement.Index = index;
            xlsElement.IndexUnknown = indexUnknown;
        }

        private static void _XlsSetDefAimHeightStuff(XlsElement xlsElement, int index, int indexUnknown)
        {
            if ((uint)(xlsElement.Type - 52) > 1) return; // if Unknown1 or AimHeight

            if (xlsElement.Index != null) return; // if already set, then return

            xlsElement.Index = index;
            xlsElement.IndexUnknown = indexUnknown;
        }

        private static void _XlsSetDefExcelStuff(XlsElement xlsElement, int unknown1, int tableIndex, int excelDefault)
        {
            if ((int)xlsElement.Type < 18) return;

            if ((int)xlsElement.Type <= 24)
            {
                if (xlsElement.Index == null && xlsElement.IndexUnknown == null && tableIndex < 0x200 && unknown1 < 4)
                {
                    xlsElement.Index = tableIndex;
                    xlsElement.IndexUnknown = unknown1;
                }

                return;
            }

            if ((int)xlsElement.Type != 26)
            {
                if ((int)xlsElement.Type <= 37 || (int)xlsElement.Type > 45) return;

                if (xlsElement.Index == null && xlsElement.IndexUnknown == null && tableIndex < 0x200 && unknown1 < 4)
                {
                    xlsElement.Index = tableIndex;
                    xlsElement.IndexUnknown = unknown1;
                }

                return;
            }

            if (xlsElement.Index == null && xlsElement.IndexUnknown == null && xlsElement.ExcelDefaultQ == 0 && tableIndex < 0x200 && unknown1 < 4)
            {
                xlsElement.Index = tableIndex;
                xlsElement.IndexUnknown = unknown1;
                xlsElement.ExcelDefaultQ = excelDefault;
            }
        }

        private static void _XlsSetEnumStuff(XlsElement xlsElement, Dictionary<String, int> enums, int enumCount)
        {
            if ((uint)(xlsElement.Type - 27) > 3) return;  // Enum, EnumArray and 28, 27 (not defined)

            if (xlsElement.Index != null) return; // if already set, then return

            xlsElement.ExcelDefaultQ = enumCount;
            xlsElement.Index = enums;

            // checks the/for default in here as well, but no point as all it appears to do is flag the element with XlsFlags.Enum - which it already has
        }

        private static void _GenerateTableHash(XlsTable xlsTable, UInt32 scriptsHash)
        {
            bool outputPerElement = false;
            if (xlsTable.DefinitionId == "STATE_DATadsfA")
            {
                outputPerElement = true;
            }

            xlsTable.TableHash = (uint)xlsTable.RowSize + 32;

            xlsTable.Elements = xlsTable.Elements.OrderBy(element => element.Name).ToList();
            foreach (XlsElement xlsElement in xlsTable.Elements)
            {
                bool outputHashSteps = false;
                if (outputPerElement && xlsElement.Name == "UI icon badfck")
                {
                    outputHashSteps = true;
                }

                uint hash = _GenerateElementHash(xlsElement, outputHashSteps);
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

        private static uint _GenerateElementHash(XlsElement xlsElement, bool outputHashSteps = false)
        {
            Int32 type = (int)xlsElement.Type;

            UInt32 hash = 0;
            const char rsinglequote = (char)65533;
            if (xlsElement.Name.IndexOf(rsinglequote) == -1) // if not found
            {
                hash = Crypt.GetStringHash(xlsElement.Name, hash);
            }
            else // the initial export converts the 0x92 (’) value to the Ascii Æ, which String read in then converts it to 65533 (0xFFFD) '�' char
            {    // even if you force/replace before hand all Æ to the proper 0x92 (’) value, it still gets converted to the Unicode value (0x2019) - even if you cast str[i] as a char, and you get 8217 (0x2019)
                foreach (char c in xlsElement.Name)
                {
                    if (c == rsinglequote)
                    {
                        hash = Crypt.GetStringHash(new[] { (byte)0x92 }, hash);
                    }
                    else
                    {
                        hash = Crypt.GetStringHash(new[] { (byte)c }, hash);
                    }
                }
            }
            if (outputHashSteps) Debug.WriteLine("Name: 0x{0:X8} [{1}]", hash, xlsElement.Name);

            hash = Crypt.GetStringHash(BitConverter.GetBytes(type), hash); // 4 bytes
            if (outputHashSteps) Debug.WriteLine("Type: 0x{0:X8} [{1}]", hash, type);


            if (type != 2)
            {
                hash = Crypt.GetStringHash(BitConverter.GetBytes(xlsElement.Offset), hash);
                if (outputHashSteps) Debug.WriteLine("Offset: 0x{0:X8} [{1}]", hash, xlsElement.Offset);

                hash = Crypt.GetStringHash(BitConverter.GetBytes(xlsElement.Size), hash);
                if (outputHashSteps) Debug.WriteLine("Size: 0x{0:X8} [{1}]", hash, xlsElement.Size);
            }


            hash = Crypt.GetStringHash(BitConverter.GetBytes(xlsElement.Count), hash);
            if (outputHashSteps) Debug.WriteLine("Count: 0x{0:X8} [{1}]", hash, xlsElement.Count);

            hash = Crypt.GetStringHash(BitConverter.GetBytes((Int32)xlsElement.Flags), hash);
            if (outputHashSteps) Debug.WriteLine("Flags: 0x{0:X8} [{1}]", hash, xlsElement.Flags);


            if (type <= 30) // 1-30
            {
                if (type >= 27) // 27, 28, 29 (Enum), 30 (EnumArray)
                {
                    if (xlsElement.Default != null)
                    {
                        hash = Crypt.GetStringHash(BitConverter.GetBytes((int)xlsElement.Default), hash);
                        if (outputHashSteps) Debug.WriteLine("Default: 0x{0:X8} [{1}]", hash, xlsElement.Default);
                    }
                }
                else // 1-26
                {
                    if (type == 1 || type == 2) // 1 (String), 2 (StringOffset)
                    {
                        if (xlsElement.Default != null)
                        {
                            hash = Crypt.GetStringHash((String)xlsElement.Default, hash);
                            if (outputHashSteps) Debug.WriteLine("Default: 0x{0:X8} [{1}]", hash, xlsElement.Default);
                        }

                        /*
XLS_GenereateElementHash+91   008 push    eax                             ; baseHash
XLS_GenereateElementHash+92   00C push    18h                             ; 24 bytes (6x Int32)
XLS_GenereateElementHash+94   010 pop     edx
XLS_GenereateElementHash+95   00C lea     ecx, [esi+30h]                  ; index
XLS_GenereateElementHash+98   00C call    CryptBytesHash                  
                         */
                        return _GenerateIndexSegmentHash(xlsElement, hash);
                    }

                    if (type != 3) // GroupStyle
                    { // 3-26
                        if (type > 17 && (type <= 24 || type == 26)) // 18, 19, 20 (ExcelIndex), 21 (FolderCode), 22, 23, 24, 26 (AiInit)
                        {
                            if (xlsElement.Default != null)
                            {
                                hash = Crypt.GetStringHash((String)xlsElement.Default, hash);
                                if (outputHashSteps) Debug.WriteLine("Default: 0x{0:X8} [{1}]", hash, xlsElement.Default);
                            }

                            return _GenerateIndexSegmentHash(xlsElement, hash);
                        }

                        // 3 (GroupStyle), 4 (Flag), 5, 6 (Byte06), 7, 8, 9 (Int32), 10, 11 (Bool), 12 (Float), 13 (Byte0D), 14 (CodeInt32), 15, 16 (CodeInt16), 17 (CodeInt8)
                        hash = _GenerateDefaultSegmentHash(xlsElement, hash);
                        return _GenerateIndexSegmentHash(xlsElement, hash);
                    }

                    // 3 (GroupStyle)
                    return _GenerateDefaultSegmentHash(xlsElement, hash);
                }

                // 27, 28, 29 (Enum), 30 (EnumArray)
                Object index = xlsElement.Index;
                if (index == null) return hash;

                Dictionary<String, int> enums = (Dictionary<String, int>)xlsElement.Index;
                hash = Crypt.GetStringHash(BitConverter.GetBytes(enums.Count), hash);

                foreach (KeyValuePair<String, int> enumVal in enums.OrderBy(kvp => kvp.Key, StringComparer.Ordinal))
                {
                    hash = Crypt.GetStringHash(enumVal.Key, hash);
                    hash = Crypt.GetStringHash(BitConverter.GetBytes(enumVal.Value - 1), hash);
                }

                return hash;
            }

            if (type > 49) // 50+
            {
                int isNotUnitType = type - 54;
                if (isNotUnitType == 0) // 54 (UnitType)
                {
                    if (xlsElement.Default != null)
                    {
                        hash = Crypt.GetStringHash((String)xlsElement.Default, hash);
                        if (outputHashSteps) Debug.WriteLine("Default: 0x{0:X8} [{1}]", hash, xlsElement.Default);
                    }

                    return _GenerateIndexSegmentHash(xlsElement, hash);
                }

                if (isNotUnitType == 1) return hash; // 55 (MultipleRelations)

                // 50 (Script), 51 (MaxSlots), 52 (Unknown1), 53 (AimHeight), 56 (BaseRow)
                hash = _GenerateDefaultSegmentHash(xlsElement, hash);
                return _GenerateIndexSegmentHash(xlsElement, hash);
            }

            if (type < 48) // 31-47
            {
                if (type >= 38 && (type <= 44 || type > 45 && type <= 47)) // 38, 39, 40 (ExcelIndexArray), 41, 42, 43, 44, 46 (StringIndex), 47 (TugboatUnknown)
                {
                    if (xlsElement.Default != null)
                    {
                        hash = Crypt.GetStringHash((String)xlsElement.Default, hash);
                        if (outputHashSteps) Debug.WriteLine("Default: 0x{0:X8} [{1}]", hash, xlsElement.Default);
                    }

                    return _GenerateIndexSegmentHash(xlsElement, hash);
                }

                // 31, 32, 33, 34, 35 (Int32Array), 36 (UseFixedDRLGSeed), 37, 45 (Qualities)
                hash = _GenerateDefaultSegmentHash(xlsElement, hash);
                return _GenerateIndexSegmentHash(xlsElement, hash);
            }

            // 48 (File), 49
            if (xlsElement.Index != null)
            {
                hash = Crypt.GetStringHash((String)xlsElement.Index, hash);
                if (outputHashSteps) Debug.WriteLine("Index: 0x{0:X8} [{1}]", hash, xlsElement.Index);
            }

            return hash;
        }

        private static UInt32 _GenerateDefaultSegmentHash(XlsElement xlsElement, UInt32 hash)
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
            if (xlsElement.Default == null) defaultBytes = new byte[4];
            else if (xlsElement.Type == XlsType.Bool || xlsElement.Type == XlsType.Flag) defaultBytes = new byte[] { (byte)((bool)xlsElement.Default ? 1 : 0), 0, 0, 0 };
            else if (xlsElement.Type == XlsType.Float || xlsElement.Type == XlsType.AimHeight) defaultBytes = BitConverter.GetBytes((float)xlsElement.Default);
            else if (xlsElement.Size == 2) defaultBytes = BitConverter.GetBytes((int)(short)xlsElement.Default);
            else if (xlsElement.Size == 1) defaultBytes = BitConverter.GetBytes((int)(byte)xlsElement.Default);
            else defaultBytes = BitConverter.GetBytes((int)xlsElement.Default);

            hash = Crypt.GetStringHash(defaultBytes, hash);
            hash = Crypt.GetStringHash(BitConverter.GetBytes(xlsElement.Field24), hash);
            hash = Crypt.GetStringHash(BitConverter.GetBytes(xlsElement.Field28), hash);
            return Crypt.GetStringHash(BitConverter.GetBytes(xlsElement.Field2C), hash);
        }

        private static UInt32 _GenerateIndexSegmentHash(XlsElement xlsElement, UInt32 hash)
        {
            /*
XLS_GenereateElementHash+10B  00C push    eax                             ; baseHash
XLS_GenereateElementHash+10C  010 push    18h                             ; 24 bytes (6x Int32)
XLS_GenereateElementHash+10E  014 pop     edx
XLS_GenereateElementHash+10F  010 mov     ecx, edi                        ; index
XLS_GenereateElementHash+111  010 call    CryptBytesHash                  ; Call 
             */

            byte[] indexBytes = null;
            byte[] indexUnknownBytes;
            if (xlsElement.Type == XlsType.Float && xlsElement.IndexUnknown != null)
            {
                indexBytes = BitConverter.GetBytes((float)xlsElement.Index);
                indexUnknownBytes = BitConverter.GetBytes((float)xlsElement.IndexUnknown);
            }
            else if (xlsElement.IndexUnknown == null)
            {
                indexUnknownBytes = new byte[4];
            }
            else
            {
                indexUnknownBytes = BitConverter.GetBytes((int)xlsElement.IndexUnknown);
            }

            if (indexBytes == null)
            {
                indexBytes = BitConverter.GetBytes(xlsElement.Index == null ? 0 : (int)xlsElement.Index);
            }

            hash = Crypt.GetStringHash(indexBytes, hash);
            hash = Crypt.GetStringHash(indexUnknownBytes, hash);
            hash = Crypt.GetStringHash(BitConverter.GetBytes(xlsElement.ExcelDefaultQ), hash);
            hash = Crypt.GetStringHash(BitConverter.GetBytes(xlsElement.Field3C), hash);
            hash = Crypt.GetStringHash(BitConverter.GetBytes(xlsElement.Field40), hash);
            return Crypt.GetStringHash(BitConverter.GetBytes(xlsElement.Field44), hash);
        }

        #endregion
    }
}