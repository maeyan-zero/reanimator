using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Revival.Common;

namespace Hellgate
{
    public partial class ExcelFile
    {
        public static bool EnableDebug;
        public const String FolderPath = @"excel\";
        public const String Extension = ".txt.cooked";
        public const String ExtensionDeserialised = ".txt";

        #region Excel Types
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct ExcelHeader
        {
            public UInt32 StructureID;
            public Int32 Unknown321;
            public Int32 Unknown322;
            public Int16 Unknown161;
            public Int16 Unknown162;
            public Int16 Unknown163;
            public Int16 Unknown164;
            public Int16 Unknown165;
            public Int16 Unknown166;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct RowHeader
        {
            public Int32 Unknown1;
            public Int32 Unknown2;
            public Int16 VersionMajor;
            public Int16 Reserved1;
            public Int16 VersionMinor;
            public Int16 Reserved2;
        }

        [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
        public class OutputAttribute : Attribute
        {
            public bool IsBitmask { get; set; }
            public uint DefaultBitmask { get; set; }
            public bool IsBool { get; set; }
            public bool IsStringOffset { get; set; }
            public bool IsScript { get; set; }
            public bool IsStringIndex { get; set; }
            public bool IsTableIndex { get; set; }
            public bool IsSecondaryString { get; set; }
            public string TableStringId { get; set; }
            public int SortColumnOrder { get; set; }
            public string SecondarySortColumn { get; set; }
            public Object ConstantValue { get; set; }
            public bool DebugIgnoreConstantCheck { get; set; }
        }

        private abstract class Token
        {
            public const Int32 cxeh = 0x68657863;
            public const Int32 rcsh = 0x68736372;
            public const Int32 tysh = 0x68737974;
            public const Int32 mysh = 0x6873796D;
            public const Int32 dneh = 0x68656E64;
            public const Int32 RcshValue = 4;
            public const Int32 TyshValue = 2;
            public const Int32 DnehValue = 0;
            public const Int32 MyshVersion = 3;
        }

        public abstract class ColumnTypeKeys
        {
            public const String IsFinalData = "IsFinalData";
            public const String IsExtendedProps = "IsExtendedProps";
            public const String IsStringOffset = "IsStringOffset";
            public const String IsStringIndex = "IsStringIndex";
            public const String IsRelationGenerated = "IsRelationGenerated";
            public const String IsTableIndex = "IsTableIndex";
            public const String IsBitmask = "IsBitmask";
            public const String IsBool = "IsBool";
            public const String IsScript = "IsScript";
            public const String IsSecondaryString = "IsSecondaryString";
        }

        private static class IntTableDef
        {
            // t, x, 0
            public static readonly int[] Case01 = new[] { 2, 98, 707 };
            // t, x
            public static readonly int[] Case02 = new[] { 1, 3, 4, 5, 6, 14, 26, 50, 86, 516, 527, 700 };
            // t
            public static readonly int[] Case03 = new[] { 320, 333, 339, 347, 358, 369, 388, 399, 418, 426, 437, 448, 459, 470, 481, 538, 708, 709, 710, 711, 712 };
            // t, x
            public static readonly int[] BitField = new[] { 666, 667, 669, 673, 674, 680, 683, 687, 688 };
        }

        private class ExcelPropertiesScript
        {
            internal class Parameter
            {
                public String Name { get; set; }                // short string name e.g. dmg_elec, dam, dur, etc
                public UInt32 Unknown { get; set; }             // todo: check if is Name string-hash (quite sure it is, but need to check... doesn't really matter either way)
                public UInt32 TypeId { get; set; }              // seen as 0x39, 0x3C, 0x41 - "determines" TypeValues length (more specifically, the paramater type values)
                public Int32[] TypeValues { get; set; }         // at least 1 int specifies the param element:  dmg_elec = 0xFC, dmg_fire = 0xFD
            }

            public List<Parameter> Parameters { get; private set; }
            public byte[] ScriptValues { get; set; }

            public ExcelPropertiesScript()
            {
                Parameters = new List<Parameter>();
            }
        }
        #endregion

        public static OutputAttribute GetExcelOutputAttribute(FieldInfo fieldInfo)
        {
            object[] query = fieldInfo.GetCustomAttributes(typeof(OutputAttribute), true);
            return (query.Length != 0) ? (OutputAttribute)query[0] : null;
        }

        private static String _GetStringId(String filePath, bool isTCv4 = false)
        {
            String stringIdPrepend = isTCv4 ? "_TCv4_" : "";

            // check if the file name is the same as the string id
            String baseStringId = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(filePath)).ToUpper();
            String stringIdAsKey = stringIdPrepend + baseStringId;

            DataFileAttributes dataFileAttributes;
            if (DataFileMap.TryGetValue(stringIdAsKey, out dataFileAttributes))
            {
                if (!dataFileAttributes.IsEmpty && !dataFileAttributes.IsMythos) return stringIdAsKey;
            }

            // file name is different to string id, then we have to loop through and check for name replace elements
            String stringId = null;
            foreach (KeyValuePair<String, DataFileAttributes> keyValuePair in DataFileMap)
            {
                DataFileAttributes dataFileAttribute = keyValuePair.Value;
                if (dataFileAttribute.FileName != baseStringId || dataFileAttribute.IsTCv4 != isTCv4) continue;

                stringId = keyValuePair.Key;
            }

            // if the stringId isn't found from file name checks, but we have it as a key,
            // then we have a file with the same name as an entry with a key that is empty or mythos. So return the original key.
            if (dataFileAttributes != null && stringId == null) return stringIdAsKey;

            return stringId;
        }

        private static bool _CheckToken(byte[] buffer, ref int offset, int token)
        {
            return token == FileTools.ByteArrayToInt32(buffer, ref offset);
        }

        private static bool _CheckToken(byte[] buffer, int offset, int token)
        {
            return token == BitConverter.ToInt32(buffer, offset);
        }

        public int[] ReadScriptTable(int offset)
        {
            if (offset == 0) return null;
            int position = offset;
            int value = FileTools.ByteArrayToInt32(_scriptBuffer, position);

            while (value != 0)
            {
                if (IntTableDef.Case01.Contains(value)) position += (3 * sizeof(int));
                else if (IntTableDef.Case02.Contains(value)) position += (2 * sizeof(int));
                else if (IntTableDef.Case03.Contains(value)) position += (1 * sizeof(int));
                else if (IntTableDef.BitField.Contains(value)) position += (2 * sizeof(int));
                else return null;

                value = FileTools.ByteArrayToInt32(_scriptBuffer, position);
            }

            int length = (position + sizeof(int) - offset) / sizeof(int);

            return FileTools.ByteArrayToInt32Array(_scriptBuffer, ref offset, length);
        }

        public string ReadStringTable(int offset)
        {
            if (_stringBuffer == null) return null;

            return offset == -1 ? String.Empty : FileTools.ByteArrayToStringASCII(_stringBuffer, offset);
        }

        public byte[] ReadStringTableAsBytes(int offset)
        {
            return FileTools.GetDelimintedByteArray(_stringBuffer, ref offset, 0);
        }

        public byte[] ReadExtendedProperties(int index)
        {
            return (_extendedBuffer != null) ? _extendedBuffer[index] : null;
        }

        public string ReadSecondaryStringTable(int index)
        {
            return _secondaryStrings[index];
        }

        private bool _ParsePropertiesScriptTable(byte[] data, ref int offset)
        {
            _rowScripts = new List<ExcelPropertiesScript>();

            while (offset <= data.Length)
            {
                if (_CheckToken(data, offset, Token.dneh)) return true;

                ExcelPropertiesScript excelScript = new ExcelPropertiesScript();
                if (!_ParsePropertiesScript(data, ref offset, ref excelScript)) return false;
                _rowScripts.Add(excelScript);
            }

            return false;
        }

        private static bool _ParsePropertiesScript(byte[] data, ref int offset, ref ExcelPropertiesScript excelScript)
        {
            ExcelPropertiesScript.Parameter parameter = new ExcelPropertiesScript.Parameter();

            // token and version checks
            if (!_CheckToken(data, ref offset, Token.mysh)) return false;
            UInt32 version = FileTools.ByteArrayToUInt32(data, ref offset);
            if (version != Token.MyshVersion) return false;


            // general parameter values
            int charCount = FileTools.ByteArrayToInt32(data, ref offset);
            if (charCount >= 0x1000) return false;
            parameter.Name = FileTools.ByteArrayToStringASCII(data, ref offset, charCount);
            parameter.Unknown = FileTools.ByteArrayToUInt32(data, ref offset);


            // what kind of parameter is it
            parameter.TypeId = FileTools.ByteArrayToUInt32(data, ref offset);
            int paramLength;
            switch (parameter.TypeId)
            {
                case 0x38: // 56 // "nPowerChange" from TCv4 Skills
                    paramLength = 4;
                    break;
                    
                case 0x39: // 57 // oldstats, x, sel, dmgtype, ?
                    paramLength = 4;
                    break;

                case 0x3C: // 60 // dam, ?
                    paramLength = 5;
                    break;

                case 0x41: // 65 // dmg_elec, dmg_fire, etc
                    paramLength = 8;
                    break;

                default:
                    Debug.Assert(false, "Unknown MYSH TypeId = " + parameter.TypeId);
                    return false;
            }
            parameter.TypeValues = FileTools.ByteArrayToInt32Array(data, ref offset, paramLength);

            excelScript.Parameters.Add(parameter);
            if (parameter.TypeId != 0x41) return true; // only 0x41 has paramaters and a script values block following it


            // get remaining parameters
            int paramCount = parameter.TypeValues[5];
            for (int i = 0; i < paramCount; i++)
            {
                if (!_ParsePropertiesScript(data, ref offset, ref excelScript)) return false;
            }


            // the actual script values
            int valuesByteCount = FileTools.ByteArrayToInt32(data, ref offset);
            if (valuesByteCount <= 0) return false;
            excelScript.ScriptValues = new byte[valuesByteCount];
            Buffer.BlockCopy(data, offset, excelScript.ScriptValues, 0, valuesByteCount);
            offset += valuesByteCount;

            return true;
        }

        private void _PropertiesScriptToByteArray(ref byte[] buffer, ref int offset)
        {
            foreach (ExcelPropertiesScript excelScript in _rowScripts)
            {
                foreach (ExcelPropertiesScript.Parameter paramater in excelScript.Parameters)
                {
                    FileTools.WriteToBuffer(ref buffer, ref offset, Token.mysh);
                    FileTools.WriteToBuffer(ref buffer, ref offset, Token.MyshVersion);

                    FileTools.WriteToBuffer(ref buffer, ref offset, (Int32)paramater.Name.Length);
                    FileTools.WriteToBuffer(ref buffer, ref offset, paramater.Name);
                    FileTools.WriteToBuffer(ref buffer, ref offset, paramater.Unknown);
                    FileTools.WriteToBuffer(ref buffer, ref offset, paramater.TypeId);
                    FileTools.WriteToBuffer(ref buffer, ref offset, paramater.TypeValues.ToByteArray());
                }

                if (excelScript.ScriptValues == null) continue;
                FileTools.WriteToBuffer(ref buffer, ref offset, (Int32)excelScript.ScriptValues.Length);
                FileTools.WriteToBuffer(ref buffer, ref offset, excelScript.ScriptValues);
            }
        }

        private UInt32[,] _CreateIndexBitRelations()
        {
            // get our index ranges
            int startIndex, endIndex;
            switch (StringId)
            {
                case "STATES":
                    // states has 10x columns to check from 3 (zero based index)
                    startIndex = 3;
                    endIndex = startIndex + 10;
                    break;

                case "_TCv4_STATES": // not confirmed
                    startIndex = 3;
                    endIndex = startIndex + 10;
                    break;

                case "UNITTYPES":
                    // unittypes has 16x columns to check from 2
                    startIndex = 2;
                    endIndex = startIndex + 15;
                    break;

                case "_TCv4_UNITTYPES": // not confirmed
                    startIndex = 2;
                    endIndex = startIndex + 15;
                    break;

                default:
                    throw new NotSupportedException("Unexpected case in CreateIndexBitRelations(). StringId = " + StringId);
            }


            // need 1 bit for every row; 32 bits per int
            int intCount = (Count >> 5) + 1;
            UInt32[,] indexBitRelations = new UInt32[Count, intCount];
            bool[] isGenerated = new bool[Count];


            // generate binary relation table
            for (int i = 0; i < Count; i++)
            {
                if (isGenerated[i]) continue;

                _GenerateIndexBitRelation(ref indexBitRelations, ref isGenerated, i, startIndex, endIndex);
                isGenerated[i] = true;
            }

            return indexBitRelations;
        }

        private void _GenerateIndexBitRelation(ref uint[,] indexBitRelations, ref bool[] isGenerated, int index, int startIndex, int endIndex)
        {
            int intCount = (Count >> 5) + 1;

            // need column fields
            Object row = Rows[index];
            FieldInfo[] fields = Rows[0].GetType().GetFields();

            // each row has its own bit high
            int intOffset = index >> 5;
            indexBitRelations[index, intOffset] |= (uint)1 << index;

            // check isAX columns
            for (int j = startIndex; j < endIndex; j++)
            {
                int value = (int)fields[j].GetValue(row);
                if (value == -1) continue; // at first -1, no other values have been found (tested)

                intOffset = value >> 5;
                indexBitRelations[index, intOffset] |= (uint)1 << value;

                if (!isGenerated[value])
                {
                    _GenerateIndexBitRelation(ref indexBitRelations, ref isGenerated, value, startIndex, endIndex);
                    isGenerated[value] = true;
                }

                // now we need to | the related row and its relations
                for (int relationIndex = 0; relationIndex < intCount; relationIndex++)
                {
                    indexBitRelations[index, relationIndex] |= indexBitRelations[value, relationIndex];
                }
            }
        }


        private void _DoPrecedenceHack(FieldInfo fieldInfo, OutputAttribute outputAttribute)
        {
            const char dash = '-';
            const char score = '_';
            const char dashReplace = '0';
            const char scoreReplace = '9';


            if (fieldInfo.FieldType == typeof(string))
            {
                foreach (object row in Rows)
                {
                    fieldInfo.SetValue(row, ((string) fieldInfo.GetValue(row)).Replace(dash, dashReplace).Replace(score, scoreReplace));
                }
            }

            if (outputAttribute.IsStringOffset)
            {
                for (int i = 0; i < _stringBuffer.Length; i++)
                {
                    switch (_stringBuffer[i])
                    {
                        case (byte)dash:
                            _stringBuffer[i] = (byte)dashReplace;
                            break;
                        case (byte)score:
                            _stringBuffer[i] = (byte)scoreReplace;
                            break;
                    }
                }
            }

            if (String.IsNullOrEmpty(outputAttribute.SecondarySortColumn)) return;

            FieldInfo fieldInfo2 = DataType.GetField(outputAttribute.SecondarySortColumn);
            if (fieldInfo2.FieldType != typeof(string)) return;

            foreach (object row in Rows)
            {
                fieldInfo.SetValue(row, ((string)fieldInfo.GetValue(row)).Replace(dash, dashReplace).Replace(score, scoreReplace));
            }
        }

        void UndoPrecedenceHack(FieldInfo fieldInfo)
        {
            throw new NotImplementedException();
        }

        IEnumerable<int[]> _GenerateSortedIndexArrays()
        {
            //if (StringId == "SOUNDS")
            //{
            //    int bp = 0;
            //}

            int[][] sortedIndexArrays = new int[4][];
            FieldInfo rowHeaderField = DataType.GetField("header", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (FieldInfo fieldInfo in DataType.GetFields())
            {
                OutputAttribute attribute = GetExcelOutputAttribute(fieldInfo);
                if (attribute == null || attribute.SortColumnOrder == 0) continue;

                // Precedence Hack
                // excel files order special characters differently to convention
                _DoPrecedenceHack(fieldInfo, attribute);

                int pos = attribute.SortColumnOrder - 1;

                    // need to order by string, not string offset
                if (attribute.IsStringOffset)
                {
                    var sortedList = from element in Rows
                                     let rowHeader = (RowHeader)rowHeaderField.GetValue(element)
                                     where (rowHeader.Unknown1 != 0x02 &&
                                            (rowHeader.Unknown2 >= 0x38 && rowHeader.Unknown2 <= 0x3F || rowHeader.Unknown2 == 0x01) &&
                                            (rowHeader.VersionMajor == 0 || rowHeader.VersionMajor == 10))
                                     group element by fieldInfo.GetValue(element) into groupedElements
                                     let elementFirst = groupedElements.First()
                                     orderby ReadStringTable((int)fieldInfo.GetValue(elementFirst))
                                     select Rows.IndexOf(elementFirst);
                    sortedIndexArrays[pos] = sortedList.ToArray();
                }

                    // we don't want '-1' rows for not-present secondary strings
                else if (attribute.IsSecondaryString)
                {
                    var sortedList = from element in Rows
                                     let rowHeader = (RowHeader)rowHeaderField.GetValue(element)
                                     where (rowHeader.Unknown1 != 0x02 &&
                                            (rowHeader.Unknown2 >= 0x38 && rowHeader.Unknown2 <= 0x3F || rowHeader.Unknown2 == 0x01) &&
                                            (rowHeader.VersionMajor == 0 || rowHeader.VersionMajor == 10)) && fieldInfo.GetValue(element).ToString() != "-1"
                                     group element by fieldInfo.GetValue(element) into groupedElements
                                     let elementFirst = groupedElements.First()
                                     orderby fieldInfo.GetValue(elementFirst)
                                     select Rows.IndexOf(elementFirst);
                    sortedIndexArrays[pos] = sortedList.ToArray();
                }

                    // with two column sorting we don't group
                else if (!String.IsNullOrEmpty(attribute.SecondarySortColumn))
                {
                    FieldInfo sortBy2 = DataType.GetField(attribute.SecondarySortColumn);
                    var sortedList = from element in Rows
                                     let rowHeader = (RowHeader)rowHeaderField.GetValue(element)
                                     where (rowHeader.Unknown1 != 0x02 &&
                                            (rowHeader.Unknown2 >= 0x38 && rowHeader.Unknown2 <= 0x3F || rowHeader.Unknown2 == 0x01) &&
                                            (rowHeader.VersionMajor == 0 || rowHeader.VersionMajor == 10))
                                     orderby fieldInfo.GetValue(element), sortBy2.GetValue(element)
                                     select Rows.IndexOf(element);
                    sortedIndexArrays[pos] = sortedList.ToArray();
                }

                    // main sorting
                else
                {
                    var sortedList = from element in Rows
                                     let rowHeader = (RowHeader)rowHeaderField.GetValue(element)
                                     where (rowHeader.Unknown1 != 0x02 &&
                                            (rowHeader.Unknown2 >= 0x38 && rowHeader.Unknown2 <= 0x3F || rowHeader.Unknown2 == 0x01) &&
                                            (rowHeader.VersionMajor == 0 || rowHeader.VersionMajor == 10))
                                     group element by fieldInfo.GetValue(element) into groupedElements
                                     let elementFirst = groupedElements.First()
                                     orderby fieldInfo.GetValue(elementFirst)
                                     select Rows.IndexOf(elementFirst);
                    sortedIndexArrays[pos] = sortedList.ToArray();
                }

                // Remove precedence hack
                // UndoPrecedenceHack(fieldInfo);
            }

            return sortedIndexArrays;
        }

        private UInt32 _GenerateStructureId()
        {
            // this is not finished and probably never will be
            // we need type arrays and all sorts of other bullshit that I can't be bothered finding
            // it's much easier to just use our hard-coded array
            // below is the very quick hard-coded version of the (mostly-completed; see comments) strings_files.txt.cooked StructureId
            throw new NotImplementedException();

            UInt32 finalHash = 0x4C; // total structure byte size (not including header)
            finalHash += 0x20;

            // columns are ordered by name DESC
            KeyValuePair<String, Int32>[] columns = new[]
            {
                new KeyValuePair<String, Int32>("Name", 0),
                new KeyValuePair<String, Int32>("Loaded by Game", 0x44),
                new KeyValuePair<String, Int32>("Is Common", 0x40),
                new KeyValuePair<String, Int32>("Credits File", 0x48)
            };

            for (int i = columns.Length - 1; i >= 0; i--)
            {
                String str = columns[i].Key;
                UInt32 stringHash = Crypt.GetStringHash(str);

                // I think this is the "type"
                Int32 type = 0x0B;                  // Int32 (first 3 are ints)
                if (str == "Name") type = 0x01;     // String ("Name" is a string)
                if (str.Length > 0)
                {
                    byte[] typeBytes = BitConverter.GetBytes(type);
                    UInt32 stringHash2 = Crypt.GetStringHash(typeBytes, stringHash);
                    stringHash = stringHash2;
                }


                Int32 byteStructOffset = columns[i].Value;
                byte[] byteStructOffsetBytes = BitConverter.GetBytes(byteStructOffset);
                if (type != 0x02)
                {
                    UInt32 stringHash2 = Crypt.GetStringHash(byteStructOffsetBytes, stringHash);
                    stringHash = stringHash2;

                    Int32 unknown1 = 4;                 // was 0x04 for first 3
                    if (str == "Name") unknown1 = 1;    // 0x01 for "Name"
                    byte[] unknown1Bytes = BitConverter.GetBytes(unknown1);
                    UInt32 stringHash3 = Crypt.GetStringHash(unknown1Bytes, stringHash);
                    stringHash = stringHash3;
                }

                byte[] unknown0x01 = new byte[] {0x01, 0x00, 0x00, 0x00}; // not sure if this was the same for name - gave up
                UInt32 stringHash4 = Crypt.GetStringHash(unknown0x01, stringHash);
                stringHash = stringHash4;

                Int32 unknown3 = 0;                     // was 0x00 for first 3
                if (str == "Name") unknown3 = 1;        // 0x01 for "Name"
                byte[] unknown3Bytes = BitConverter.GetBytes(unknown3);
                UInt32 stringHash5 = Crypt.GetStringHash(unknown3Bytes, stringHash);
                stringHash = stringHash5;

                int switchType = type - 1;
                switch (switchType)
                {
                    case 0: // String
                        // does 1x more string hash, 0x10 in length (so same as first part of Int32)
                        // sub_140011BB0+217  028 lea     r9, [rbx+20h]
                        break;

                        // and a few more cases as well

                    default: // Int32
                        // sub_140011BB0+306  028 lea     r9, [rbx+20h]
                        byte[] unknown2 = new byte[0x10]; // for our first sets of columns, it was all zeros
                        UInt32 stringHash6 = Crypt.GetStringHash(unknown2, stringHash);
                        stringHash = stringHash6;

                        // sub_140011BB0+345  028 lea     r8, [rbx+30h]
                        byte[] unknown4 = new byte[0x18]; // for our first sets of columns, it was all zeros
                        UInt32 stringHash7 = Crypt.GetStringHash(unknown4, stringHash);
                        stringHash = stringHash7;
                        break;
                }

                finalHash += stringHash;
            }

            return finalHash;
        }
    }
}