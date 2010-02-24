using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Collections;
using System.Data;

namespace Reanimator.Excel
{
    public class BadHeaderFlag : Exception
    {
        public BadHeaderFlag(string message) : base(message) { }
    }

    public abstract class ExcelTable : IComparable
    {
        [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
        public class ExcelOutputAttribute : Attribute
        {
            public bool IsStringOffset { get; set; }

            public bool IsIntOffset { get; set; }
            public String[] FieldNames { get; set; }
            public int DefaultIndex { get; set; }

            public bool IsStringId { get; set; }
            public String StringTable { get; set; }
        }

        [AttributeUsage(AttributeTargets.Field)]
        public class ExcelBitmaskAttribute : Attribute
        {
            uint bitmask;
        }

        private abstract class FileTokens
        {
            public const Int32 StartOfBlock = 0x68657863;      // 'cxeh'
            public const Int32 TokenRcsh = 0x68736372;         // 'rcsh'
            public const Int32 TokenTysh = 0x68737974;         // 'tysh'
            public const Int32 TokenMysh = 0x6873796D;         // 'mysh'
            public const Int32 TokenDneh = 0x68656E64;         // 'dneh'
        }

        public abstract class ColumnTypeKeys
        {
            public const String IsStringOffset = "IsStringOffset";
            public const String IsStringId = "IsStringId";
            public const String IsRelationGenerated = "IsRelationGenerated";
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct ExcelHeader
        {
            public Int32 StructureId;                       // This is the id used to determine what structure to use to read the table block.
            public Int32 Unknown321;                       // This is how the game reads this in...
            public Int32 Unknown322;                       // What they do I don't know, lol.
            public Int16 Unknown161;
            public Int16 Unknown162;
            public Int16 Unknown163;
            public Int16 Unknown164;
            public Int16 Unknown165;
            public Int16 Unknown166;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        protected struct TableHeader
        {
            public Int32 Unknown1;
            public Int32 Unknown2;
            public Int16 VersionMajor;
            public Int16 Reserved1;                         // I think...
            public Int16 VersionMinor;
            public Int16 Reserved2;                         // I think...
        }

        private readonly byte[] _excelData;
        protected int offset;
        private ExcelHeader _excelHeader;
        public int StructureId
        {
            get { return _excelHeader.StructureId; }
        }

        private readonly byte[] _stringsBytes;
        public Hashtable Strings { get; private set; }

        protected readonly List<object> tables;

        private readonly int[] _tableIndicies;
        private readonly List<byte[]> _extraIndexData;
        public int[] TableIndicies { get { return _tableIndicies; } }

        public List<String> secondaryStrings;

        private int[][] unknownIndicies;
        public int[] Unknowns1 { get { return unknownIndicies[0]; } }
        public int[] Unknowns2 { get { return unknownIndicies[1]; } }
        public int[] Unknowns3 { get { return unknownIndicies[2]; } }
        public int[] Unknowns4 { get { return unknownIndicies[3]; } }

        private readonly Int32 _rcshValue;
        private readonly Int32 _tyshValue;
        // mysh
        private readonly Int32 _dnehValue;

        private byte[] DataBlock { get; set; }
        private byte[] FinalBytes { get; set; }

        protected ExcelTable(byte[] excelData)
        {
            if (excelData == null)
            {
                IsNull = true;
                return;
            }

            /* Excel Table Structure Layout
                 * 
                 ***** Main Header ***** (28) bytes
                 * token                                Int32                                   // Must be 0x68657863 ('cxeh').
                 * StructureId                          Int32                                   // This is the id used to determine what structure to use to read the table block.
                 * unknown                              Int32                                   // This is how the game reads this in...
                 * unknown                              Int32                                   // What they do I don't know, lol.
                 * unknown                              Int16
                 * unknown                              Int16
                 * unknown                              Int16
                 * unknown                              Int16
                 * unknown                              Int16
                 * unknown                              Int16
                 * 
                 ***** String Block ***** (4 + byteCount) bytes
                 * token                                Int32                                   // Must be 0x68657863 ('cxeh').
                 * byteCount                            Int32                                   // The number of bytes of the following string block.
                 * stringBytes                          byteCount                               // The strings (each one is \0) lumped together as one big block.
                 * 
                 ***** Tables Block ***** (8 + tableSize * tableCount) bytes
                 * token                                Int32                                   // Must be 0x68657863 ('cxeh').
                 * tableCount                           Int32                                   // Count of tables.
                 * {                                                                            // Each table has the same first header 16 byte chunk style.
                 *      unknown                         Int32                                   // Seen as 0x01, 0x02 or 0x03.
                 *      type                            Int32                                   // Seen as 0x30, 0x3C, 0x3E or 0x3F.
                 *      majorVersion                    Int16                                   // Usually 0x00.
                 *      reserved                        Int16                                   // Only seen as 0xFF.
                 *      minorVersion                    Int16                                   // Usually 0x00.
                 *      reserved                        Int16                                   // Only seen as 0xFF.
                 *      *****                           *****                                   // Table type dependent; see applicable table structure class file.
                 * }
                 * 
                 ***** Primary Index Block ***** (4 + tableCount * 4) bytes
                 * token                                Int32                                   // Must be 0x68657863 ('cxeh').
                 * tableCount                                                                   // This is tableCount from above Table Block (not actually read in just
                 * {                                                                            //                                            here for struct definition)
                 *      index                           Int32                                   // Always seen as 0 -> tableCount - 1, unknown
                 * }                                                                            // if it can be different (do we even care?).
                 * 
                 ***** Secondary Index Block 1 ***** (8 + indexCount * 4) bytes                 // Unsure what these do.
                 * token                                Int32                                   // Must be 0x68657863 ('cxeh'). 
                 * indexCount                           Int32                                   // For every table that doesn't have 0x30 as its type,
                 * {                                                                            // has a secondary index.
                 *      unknownValue                    Int32
                 * }
                 * 
                 ***** Secondary Index Block 2 ***** (8 + indexCount * 4) bytes                 // Unsure what these do.
                 * token                                Int32                                   // Must be 0x68657863 ('cxeh'). 
                 * indexCount                           Int32
                 * {
                 *      unknownValue                    Int32
                 * }
                 * 
                 ***** Secondary Index Block 3 ***** (8 + indexCount * 4) bytes                 // Unsure what these do.
                 * token                                Int32                                   // Must be 0x68657863 ('cxeh'). 
                 * indexCount                           Int32
                 * {
                 *      unknownValue                    Int32
                 * }
                 * 
                 ***** Secondary Index Block 4 ***** (8 + indexCount * 4) bytes                 // Unsure what these do.
                 * token                                Int32                                   // Must be 0x68657863 ('cxeh'). 
                 * indexCount                           Int32
                 * {
                 *      unknownValue                    Int32
                 * }
                 * 
                 ***** Unknown Block *****                                                      // The flags in this block are all optional (excluding initial 'cxeh' of course).
                 * token                                Int32                                   // Must be 0x68657863 ('cxeh'). 
                 * 
                 * unknownFlag                          Int32                                   // Can be 0x68736372 ('rcsh').
                 * unknownValue                         Int32                                   // Only seen as 0x04.
                 * 
                 * unknownFlag                          Int32                                   // Can be 0x68737974 ('tysh').
                 * unknownValue                         Int32                                   // Only seen as 0x02.
                 * 
                 ** Repeat Start (Unknown Count) **                                             // It just keeps going until a flag not equal to 'mysh' is encountered I think. // CHECK THIS
                 * propertyFlag                         Int32                                   // Can be 0x6873796D ('mysh').
                 * unknownValue                         Int32                                   // If NOT zero.
                 * {
                 *      byteCount                       Int32                                   // Count of bytes of following string.
                 *      string                          byteCount                               // NOT zero terminated.
                 *      unknown                         Int32                                   // Some weird number.
                 *      type                            Int32                                   // 0x39 = Single, 0x41 = Has Attributes, 0x3C = Attribute.
                 *      unknown                         Int32                                   // Usually 0x05.
                 *      unknown                         Int32                                   // Always 0x00?
                 *      unknown                         Int32                                   // Usually 0x04.
                 *      unknown                         Int32                                   // Always 0x00?
                 *      if (type != 0x39)                                                       // If not Single type.
                 *      {
                 *          id                          Int32                                   // Property Id - 0x00 for type 0x3C.
                 *          attributeCount              Int32
                 *          attributeCount2             Int32                                   // Always the same from what I've seen.
                 *          reserved                    Int32                                   // Assuming some reserved thing - always seen as 0x00.
                 *          if (attributeNumber == attributeCount)                              // As in, only the last attribute has any actual details.
                 *          {                                                                   // The preceeding attributes just finish after the reserved block.
                 *          
                 *          }
                 *      }
                 * }
                 ** Repeat End **
                 * 
                 * unknownFlag                          Int32                                   // Can be 0x68656E64 ('dneh').
                 * unknownValue                         Int32                                   // Only seen as 0x00.
                 * 
                 ***** Data Block *****
                 * token                                Int32                                   // Must be 0x68657863 ('cxeh').
                 * byteCount                            Int32                                   // Bytes of following block.
                 * dataBlock                            byteCount                               // An int[] - refered to in the tables
                 *                                                                              // If StructureId == 0x1F9DDC98, then this block is read in the
                 *                                                                              // same style as the second data block, with the second data
                 *                                                                              // block ommited. (unittypes.txt.cooked)
                 ***** Data Block 2 *****
                 * token                                Int32                                   // Must be 0x68657863 ('cxeh').
                 * byteCount                            Int32                                   // Bytes of following blocks shift left 2.
                 * blockCount                           Int32                                   // Number of blocks to read.
                 * {
                 *      dataBlock                       byteCount << 2                          // I have no idea where they drempt this up, but so far
                 * }                                                                            // it has only been seen in states.txt.cooked
                 * 
                 * 
                 * // just testing stuffs
                 * 0x3F = 0011 1111
                 * 0x3E = 0011 1110
                 * 0x3C = 0011 1100
                 * 0x30 = 0011 0000
                 */


            _excelData = excelData;
            tables = new List<Object>();
            Strings = new Hashtable();
            secondaryStrings = new List<String>();
            unknownIndicies = new int[4][];
            offset = 0;


            // main header
            int token = FileTools.ByteArrayTo<Int32>(excelData, ref offset);
            CheckFlag(token);

            _excelHeader = (ExcelHeader)FileTools.ByteArrayToStructure(excelData, typeof(ExcelHeader), offset);
            offset += Marshal.SizeOf(typeof(ExcelHeader));


            // strings block
            token = FileTools.ByteArrayTo<Int32>(excelData, ref offset);
            CheckFlag(token);

            int stringsBytesCount = FileTools.ByteArrayTo<Int32>(excelData, ref offset);
            if (stringsBytesCount != 0)
            {
                _stringsBytes = new byte[stringsBytesCount];
                Buffer.BlockCopy(_excelData, offset, _stringsBytes, 0, stringsBytesCount);

                // put into a hash table for easier use later, with the _stringsBytes[offset] -> offset as the key
                // sometimes C# makes things hard  -  C++ char* ftw. x.x
                for (int i = offset; i < offset + stringsBytesCount; i++)
                {
                    String s = FileTools.ByteArrayToStringAnsi(_excelData, i);
                    Strings.Add(i - offset, s);

                    i += s.Length;
                }

                offset += stringsBytesCount;
            }


            // tables block
            token = FileTools.ByteArrayTo<Int32>(excelData, ref offset);
            CheckFlag(token);

            Count = FileTools.ByteArrayTo<Int32>(excelData, ref offset);
            ParseTables(excelData);


            // index block
            token = FileTools.ByteArrayTo<Int32>(excelData, ref offset);
            CheckFlag(token);

            Hashtable hashTableUnknown1 = new Hashtable();
            Hashtable hashTableUnknown2 = new Hashtable();

            if ((uint)_excelHeader.StructureId == 0x887988C4) // items, missiles, monsters, objects, players
            {
                _tableIndicies = new int[Count];
                _extraIndexData = new List<byte[]>();

                for (int i = 0; i < Count; i++)
                {
                    _tableIndicies[i] = FileTools.ByteArrayTo<Int32>(excelData, ref offset);
                    int size = FileTools.ByteArrayTo<Int32>(excelData, ref offset);

                    byte[] extra = new byte[size];
                    Buffer.BlockCopy(excelData, offset, extra, 0, size);
                    offset += size;
                    _extraIndexData.Add(extra);
                }
            }
            else
            {
                _tableIndicies = FileTools.ByteArrayToInt32Array(excelData, offset, Count);
                offset += Count * sizeof(Int32);

                if (Count == 0)
                {
                    offset += sizeof(Int32);
                }


                /*
                    foreach (Object table in tables)
                    {
                        Type type = table.GetType();
                        FieldInfo fieldInfo = type.GetField("header", BindingFlags.NonPublic | BindingFlags.Instance);
                        if (fieldInfo != null)
                        {
                            if (fieldInfo.FieldType == typeof(TableHeader))
                            {
                                TableHeader tableHeader = (TableHeader)fieldInfo.GetValue(table);

                                if (!hashTableUnknown1.ContainsKey(tableHeader.Unknown1))
                                {
                                    hashTableUnknown1.Add(tableHeader.Unknown1, 0);
                                }
                                if (!hashTableUnknown2.ContainsKey(tableHeader.Unknown2))
                                {
                                    hashTableUnknown2.Add(tableHeader.Unknown2, 0);
                                }

                                int count = (int)hashTableUnknown1[tableHeader.Unknown1];
                                hashTableUnknown1[tableHeader.Unknown1] = count + 1;
                                count = (int)hashTableUnknown2[tableHeader.Unknown2];
                                hashTableUnknown2[tableHeader.Unknown2] = count + 1;
                            }
                        }
                    }

                    Debug.Write("hashTableUnknown1\n");
                    foreach (Int32 key in hashTableUnknown1.Keys)
                    {
                        Debug.Write("[0x" + key.ToString("X") + "] = 0x" + ((int)hashTableUnknown1[key]).ToString("X") + "(" + ((int)hashTableUnknown1[key]) + ")\n");
                    }
                    Debug.Write("hashTableUnknown2\n");
                    foreach (Int32 key in hashTableUnknown2.Keys)
                    {
                        Debug.Write("[0x" + key.ToString("X") + "] = 0x" + ((int)hashTableUnknown2[key]).ToString("X") + "(" + ((int)hashTableUnknown2[key]) + ")\n");
                    }
                    Debug.Write("\n");
                     * */
            }


            // secondary string block
            token = FileTools.ByteArrayTo<Int32>(excelData, ref offset);
            if (!CheckFlag(token, 0x68657863)) // 'cxeh'
            {
                int stringCount = token;
                for (int i = 0; i < stringCount; i++)
                {
                    int charCount = FileTools.ByteArrayTo<Int32>(excelData, ref offset);
                    String str = FileTools.ByteArrayToStringAnsi(excelData, offset);
                    offset += charCount;
                    secondaryStrings.Add(str);
                }
            }
            else
            {
                offset -= 4;
            }


            // secondary index blocks
            for (int i = 0; i < 4; i++)
            {
                token = FileTools.ByteArrayTo<Int32>(excelData, ref offset);
                CheckFlag(token);

                int count = FileTools.ByteArrayTo<Int32>(excelData, ref offset);
                unknownIndicies[i] = FileTools.ByteArrayToInt32Array(excelData, offset, count);
                offset += count * sizeof(Int32);
            }


            // unknown block
            token = FileTools.ByteArrayTo<Int32>(excelData, ref offset);
            CheckFlag(token);

            token = FileTools.ByteArrayTo<Int32>(excelData, ref offset);
            if (token != 0x00)
            {
                while (true)
                {

                    if (CheckFlag(token, 0x68736372)) // 'rcsh'
                    {
                        _rcshValue = FileTools.ByteArrayTo<Int32>(excelData, ref offset);
                        if (_rcshValue != 0x04)
                        {
                            throw new Exception("_rcshValue = FileTools.ByteArrayTo<Int32>(data, ref offset);\nif (_rcshValue != 0x04)");
                        }
                    }
                    else if (CheckFlag(token, 0x68737974)) // 'tysh'
                    {
                        _tyshValue = FileTools.ByteArrayTo<Int32>(excelData, ref offset);
                        if (_tyshValue != 0x02)
                        {
                            throw new Exception("_tyshValue = FileTools.ByteArrayTo<Int32>(data, ref offset);\nif (_tyshValue != 0x02)");
                        }
                    }
                    else if (CheckFlag(token, 0x6873796D)) // 'mysh'
                    {
                        offset -= 4;
                        ParseMyshTables(excelData, ref offset);
                    }
                    else if (CheckFlag(token, 0x68656E64)) // 'dneh'
                    {
                        _dnehValue = FileTools.ByteArrayTo<Int32>(excelData, ref offset);
                        if (_dnehValue != 0x00)
                        {
                            throw new Exception("_dnehValue = FileTools.ByteArrayTo<Int32>(data, ref offset);\nif (_dnehValue != 0x02)");
                        }
                    }
                    else // 'cxeh'  -  starting next block
                    {
                        CheckFlag(token);
                        offset -= 4;
                        break;
                    }

                    token = FileTools.ByteArrayTo<Int32>(excelData, ref offset);
                }
            }


            // data block
            token = FileTools.ByteArrayTo<Int32>(excelData, ref offset);
            if (token != 0)
            {
                CheckFlag(token);
                int byteCount = FileTools.ByteArrayTo<Int32>(excelData, ref offset);
                if (byteCount != 0)
                {
                    if (_excelHeader.StructureId == 0x1F9DDC98)         // Only seen in unittypes.txt.cooked so far.
                    {                                                       // This block reading method is the same as first seen below in the states.txt.cooked,
                        // but there is no data in the previous block for unittypes.txt.cooked.
                        int blockCount = FileTools.ByteArrayTo<Int32>(excelData, ref offset);
                        byteCount = (byteCount << 2) * blockCount;                              // No idea where they drempt this up,
                    }
                    DataBlock = new byte[byteCount];
                    Buffer.BlockCopy(excelData, offset, DataBlock, 0, byteCount);
                    offset += byteCount;
                }
            }


            // does it have a final flag chunk?
            if (offset != excelData.Length)
            {
                token = FileTools.ByteArrayTo<Int32>(excelData, ref offset);
                if (token != 0)
                {
                    CheckFlag(token);

                    int byteCount = FileTools.ByteArrayTo<Int32>(excelData, ref offset);
                    int blockCount = FileTools.ByteArrayTo<Int32>(excelData, ref offset);
                    byteCount = (byteCount << 2) * blockCount;

                    if (byteCount != 0)        // Only seen in states.txt.cooked so far  -  Of note is that 
                    {                          //           the states file has an above data block as well.
                        FinalBytes = new byte[byteCount];
                        Buffer.BlockCopy(excelData, offset, FinalBytes, 0, FinalBytes.Length);
                        offset += FinalBytes.Length;
                    }
                }
            }

            if (offset != excelData.Length)
            {
                throw new BadHeaderFlag("offset != data.Length");
            }
        }

        public int Count { get; private set; }
        public bool IsNull { get; private set; }

        private static void CheckFlag(int flag)
        {
            if (!CheckFlag(flag, 0x68657863))
            {
                throw new BadHeaderFlag("Unexpected header flag!");
            }
        }

        private static bool CheckFlag(int flag, int to)
        {
            return flag == to ? true : false;
        }

        private static void ParseMyshTables(byte[] data, ref int offset)
        {
            int totalAttributeCount = 0;
            int attributeCount = 0;
            int blockCount = 0;
            int flagCount = 0;
            while (offset < data.Length)
            {
                ////////////// temp fix /////////////////
                int f = BitConverter.ToInt32(data, offset);
                if (CheckFlag(f, 0x68657863))
                {
                    //Debug.Write("mysh flagCount = " + flagCount + "\n");
                    break;
                }
                if (CheckFlag(f, 0x6873796D))
                {
                    flagCount++;
                }
                offset++;
                continue;
                ////////////// temp fix /////////////////


                int flag = FileTools.ByteArrayTo<Int32>(data, ref offset);
                if (!CheckFlag(flag, 0x6873796D))
                {
                    offset -= 4;
                    break;
                }

                int unknown1 = FileTools.ByteArrayTo<Int32>(data, ref offset);
                if (unknown1 == 0x00)
                {
                    break;
                }

                int byteCount = FileTools.ByteArrayTo<Int32>(data, ref offset);
                // this is really lazy and I couldn't be bothered trying to figure out how to get a *non-zero terminated* string out of a byte array
                byte[] temp = new byte[byteCount + 1];
                Buffer.BlockCopy(data, offset, temp, 0, byteCount);
                String str = FileTools.ByteArrayToStringAnsi(temp, 0);
                offset += byteCount;

                int unknown2 = FileTools.ByteArrayTo<Int32>(data, ref offset);
                int type = FileTools.ByteArrayTo<Int32>(data, ref offset); // 0x39 = single, 0x41 = has properties, 0x3C = property
                int unknown3 = FileTools.ByteArrayTo<Int32>(data, ref offset);  // usually 0x05
                int unknown4 = FileTools.ByteArrayTo<Int32>(data, ref offset);  // always 0x00?
                int unknown5 = FileTools.ByteArrayTo<Int32>(data, ref offset);  // usually 0x04
                int unknown6 = FileTools.ByteArrayTo<Int32>(data, ref offset);  // always 0x00?

                if (type == 0x39)
                {
                    continue;
                }

                int id = FileTools.ByteArrayTo<Int32>(data, ref offset);
                attributeCount = FileTools.ByteArrayTo<Int32>(data, ref offset);
                totalAttributeCount = attributeCount;

                if (type == 0x41)
                {
                    int attributeCountAgain = FileTools.ByteArrayTo<Int32>(data, ref offset); // ??
                }
                else if (type == 0x3C)
                {
                    if (str == "dam")
                    {
                        blockCount += 2;
                    }
                    else if (str == "dur")
                    {
                        blockCount++;
                    }

                    const int blockSize = 4 * sizeof(Int32);
                    if (attributeCount == 0)
                    {
                        offset += blockCount * blockSize;
                        continue;
                    }

                    attributeCount--;
                }
                else
                {
                    throw new NotImplementedException("type not implemented!\ntype = " + type);
                }

                int endInt = FileTools.ByteArrayTo<Int32>(data, ref offset);
                if (endInt != 0x00)
                {
                    int breakpoint = 1;
                }
            }

        }

        public object GetTableArray()
        {
            return tables.ToArray();
        }

        protected abstract void ParseTables(byte[] data);

        protected void ReadTables<T>(byte[] data, ref int byteOffset, int count)
        {
            for (int i = 0; i < count; i++)
            {
                T table = (T)FileTools.ByteArrayToStructure(data, typeof(T), byteOffset);
                byteOffset += Marshal.SizeOf(typeof(T));

                tables.Add(table);
            }
        }

        public byte[] GenerateExcelFile(DataSet dataSet)
        {
            DataTable dataTable = dataSet.Tables[StringId];
            if (dataTable == null)
            {
                return null;
            }

            byte[] buffer = new byte[1024];
            int byteOffset = 0;


            // main header
            FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTokens.StartOfBlock);
            FileTools.WriteToBuffer(ref buffer, ref byteOffset, _excelHeader);


            // string block
            FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTokens.StartOfBlock);
            int stringsByteCount = 0;
            int stringsByteOffset = byteOffset;
            byte[] stringBytes = null;
            byteOffset += sizeof(Int32);


            // tables block
            FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTokens.StartOfBlock);
            Int32 tableCount = dataTable.Rows.Count;
            FileTools.WriteToBuffer(ref buffer, ref byteOffset, tableCount);

            Type tableType = tables[0].GetType();
            TableHeader defaultTableHeader;
            defaultTableHeader.Unknown1 = 0x03;
            defaultTableHeader.Unknown2 = 0x3F;
            defaultTableHeader.VersionMajor = 0x00;
            defaultTableHeader.Reserved1 = -1;
            defaultTableHeader.VersionMinor = 0x00;
            defaultTableHeader.Reserved2 = -1;
            int row = 0;
            foreach (DataRow dr in dataTable.Rows)
            {
                object table = Activator.CreateInstance(tableType);
                int col = 1;
                foreach (FieldInfo fieldInfo in tableType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    // if it's private, there wont be a column - try for original or assign default
                    if (fieldInfo.IsPrivate)
                    {
                        if (tables.Count > row)
                        {
                            fieldInfo.SetValue(table, fieldInfo.GetValue(tables[row]));
                        }
                        else
                        {
                            if (fieldInfo.FieldType == typeof(TableHeader))
                            {
                                fieldInfo.SetValue(table, defaultTableHeader);
                            }
                            else
                            {
                                fieldInfo.SetValue(table, 0);
                            }
                        }

                        continue;
                    }


                    // get the applicable column
                    DataColumn dc = dataTable.Columns[col];
                    while (dc != null)
                    {
                        if (dc.ExtendedProperties.Contains(ColumnTypeKeys.IsRelationGenerated))
                        {
                            if ((bool)dc.ExtendedProperties[ColumnTypeKeys.IsRelationGenerated])
                            {
                                col++;
                                dc = dataTable.Columns[col];
                                continue;
                            }
                        }

                        break;
                    }
                    if (dc == null)
                    {
                        break;
                    }


                    // if it's a string offset, add string to string buffer and set value as offset
                    if (dc.ExtendedProperties.Contains(ColumnTypeKeys.IsStringOffset))
                    {
                        if ((bool)dc.ExtendedProperties[ColumnTypeKeys.IsStringOffset])
                        {
                            if (stringBytes == null)
                            {
                                stringBytes = new byte[1024];
                            }

                            String s = dr[dc] as String;
                            if (s == null)
                            {
                                fieldInfo.SetValue(table, -1);
                            }
                            else if (s.Length == 0)
                            {
                                fieldInfo.SetValue(table, -1);
                            }
                            else
                            {
                                fieldInfo.SetValue(table, stringsByteCount);
                                FileTools.WriteToBuffer(ref stringBytes, ref stringsByteCount, FileTools.StringToASCIIByteArray(s));
                                stringsByteCount++; // \0
                            }
                        }
                    }
                    else
                    {
                        Object o = dr[dc];
                        fieldInfo.SetValue(table, o);
                    }
                    col++;
                }

                FileTools.WriteToBuffer(ref buffer, ref byteOffset, table);
                row++;
            }


            // write strings block
            if (stringBytes != null && stringsByteCount > 0)
            {
                FileTools.WriteToBuffer(ref buffer, stringsByteOffset, stringsByteCount);
                FileTools.WriteToBuffer(ref buffer, stringsByteOffset + sizeof(Int32), stringBytes, stringsByteCount, true);
            }


            // primary index block
            FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTokens.StartOfBlock);
            byte[] primaryIndex = new byte[dataTable.Rows.Count * sizeof(Int32)];
            for (Int32 i = 0; i < dataTable.Rows.Count; i++)
            {
                byte[] integer = BitConverter.GetBytes(i);
                Buffer.BlockCopy(integer, 0, primaryIndex, i * sizeof(Int32), sizeof(Int32));
            }
            FileTools.WriteToBuffer(ref buffer, ref byteOffset, primaryIndex);


            // secondary index blocks
            String[] sorts = new[] { "name", "asdf", "code", "group" };
            int secondaryIndexCount = 0;
            foreach (String sortBy in sorts)
            {
                if (!dataTable.Columns.Contains(sortBy))
                {
                    continue;
                }


                Int32 countOfIndicies = 0;
                int countOfIndiciesOffset = byteOffset;
                FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTokens.StartOfBlock);
                FileTools.WriteToBuffer(ref buffer, ref byteOffset, countOfIndicies);


                dataTable.DefaultView.Sort = sortBy;
                DataView dataView = dataTable.DefaultView;
                byte[] secondaryIndex = new byte[dataTable.Rows.Count * sizeof(Int32)];
                foreach (DataRowView dr in dataView)
                {
                    Object value = dr[sortBy];

                    String s = value as String;
                    if (s != null)
                    {
                        if (s.Length == 0)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if ((int)value <= 0)
                        {
                            continue;
                        }
                    }

                    byte[] integer = BitConverter.GetBytes((Int32)dr[0]);
                    Buffer.BlockCopy(integer, 0, secondaryIndex, countOfIndicies * sizeof(Int32), sizeof(Int32));
                    countOfIndicies++;
                }

                FileTools.WriteToBuffer(ref buffer, countOfIndiciesOffset, countOfIndicies);
                FileTools.WriteToBuffer(ref buffer, ref byteOffset, secondaryIndex, countOfIndicies * sizeof(Int32), false);
                secondaryIndexCount++;
            }

            const int zeroValue = 0;
            for ( ; secondaryIndexCount < 4; secondaryIndexCount++)
            {
                FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTokens.StartOfBlock);
                FileTools.WriteToBuffer(ref buffer, ref byteOffset, zeroValue);
            }


            // weird unknown header chunks
            FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTokens.StartOfBlock);
            if (_rcshValue != 0)
            {
                FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTokens.TokenRcsh);
                FileTools.WriteToBuffer(ref buffer, ref byteOffset, _rcshValue);

                FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTokens.TokenTysh);
                FileTools.WriteToBuffer(ref buffer, ref byteOffset, _tyshValue);

                // TODO add _mysh values
                FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTokens.TokenMysh);
                FileTools.WriteToBuffer(ref buffer, ref byteOffset, zeroValue);

                FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTokens.TokenDneh);
                FileTools.WriteToBuffer(ref buffer, ref byteOffset, _dnehValue);
            }


            // data block 1
            FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTokens.StartOfBlock);
            if (DataBlock != null)
            {
                FileTools.WriteToBuffer(ref buffer, ref byteOffset, DataBlock.Length);
                if (DataBlock.Length > 0)
                {
                    FileTools.WriteToBuffer(ref buffer, ref byteOffset, DataBlock);
                }
            }
            else
            {
                FileTools.WriteToBuffer(ref buffer, ref byteOffset, zeroValue);
            }


            // data block 2
            FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTokens.StartOfBlock);
            if (FinalBytes != null)
            {
                FileTools.WriteToBuffer(ref buffer, ref byteOffset, FinalBytes.Length);
                if (FinalBytes.Length > 0)
                {
                    FileTools.WriteToBuffer(ref buffer, ref byteOffset, FinalBytes);
                }
            }
            else
            {
                FileTools.WriteToBuffer(ref buffer, ref byteOffset, zeroValue);
            }
            byteOffset -= sizeof (Int32);


            // return final buffer
            byte[] returnBuffer = new byte[byteOffset];
            Buffer.BlockCopy(buffer, 0, returnBuffer, 0, byteOffset);
            return returnBuffer;
        }

        public String StringId { get; set; }
        public override string ToString()
        {
            return StringId;
        }

        public int CompareTo(Object o)
        {
            return String.Compare(ToString(), o.ToString());
        }
    }
}