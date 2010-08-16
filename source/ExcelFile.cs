using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using Reanimator.ExcelDefinitions;

namespace Reanimator
{
    public partial class ExcelFile : DataFile
    {
        private readonly bool _excelDebugAll;

        // file vars - order is approx order found in file
        private ExcelHeader _excelHeader;

        private byte[] _stringsBytes;
        public Hashtable Strings { get; private set; }

        public int[] TableIndicies { get; private set; }
        private List<byte[]> _extraIndexData;

        public List<String> SecondaryStrings { get; private set; }

        private Int32 _rcshValue;
        private Int32 _tyshValue;
        // mysh
        private Int32 _dnehValue;

        private int[][] _sortIndicies;
        public int[] SortIndex1 { get { return _sortIndicies[0]; } }
        public int[] SortIndex2 { get { return _sortIndicies[1]; } }
        public int[] SortIndex3 { get { return _sortIndicies[2]; } }
        public int[] SortIndex4 { get { return _sortIndicies[3]; } }

        private byte[] _dataBlock { get; set; }
        public Hashtable DataBlock { get; private set; }
        public bool HasDataBlock { get { return _dataBlock != null ? true : false; } }
        public byte[] FinalBytes { get; set; }

        byte[] _extra = new byte[] {
                        10, 0, 2, 6,
                        0, 100, 218, 0,
                        17, 9, 0, 0,
                        0, 25, 55, 64,
                        4, 1, 0, 0,
                        64, 230, 13, 16,
                        81, 0, 0, 0, 0 };

        // func defs etc
        public ExcelFile(String stringId, Type type)
            : base(stringId, type)
        {
            _excelDebugAll = true;
            IsExcelFile = true;
        }

        override public String ToString()
        {
            return StringId;
        }

        public override string FileExtension
        {
            get { return FileExtention; }
        }

        public override string SaveTitle
        {
            get { return "Excel Cooked"; }
        }

        public override bool ParseData(byte[] data)
        {
            if (data == null) return false;

            #region Excel Table Structure Layout
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
                 ***** Sort Index Block 1 ***** (8 + indexCount * 4) bytes                      // Sort index array
                 * token                                Int32                                   // Must be 0x68657863 ('cxeh'). 
                 * indexCount                           Int32                                   // For every table that doesn't have 0x30 as its type,
                 * {                                                                            // has a secondary index.
                 *      index                           Int32
                 * }
                 * 
                 ***** Sort Index Block 2 ***** (8 + indexCount * 4) bytes                      // Sort index array
                 * token                                Int32                                   // Must be 0x68657863 ('cxeh'). 
                 * indexCount                           Int32
                 * {
                 *      index                           Int32
                 * }
                 * 
                 ***** Sort Index Block 3 ***** (8 + indexCount * 4) bytes                      // Sort index array
                 * token                                Int32                                   // Must be 0x68657863 ('cxeh'). 
                 * indexCount                           Int32
                 * {
                 *      index                           Int32
                 * }
                 * 
                 ***** Sort Index Block 4 ***** (8 + indexCount * 4) bytes                      // Sort index array
                 * token                                Int32                                   // Must be 0x68657863 ('cxeh'). 
                 * indexCount                           Int32
                 * {
                 *      index                           Int32
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
            #endregion

            // init vars
            _data = data;
            Strings = new Hashtable();
            SecondaryStrings = new List<String>();
            _sortIndicies = new int[4][];
            int offset = 0;
            DataBlock = new Hashtable();

            // main header
            int token = FileTools.ByteArrayTo<Int32>(_data, ref offset);
            CheckExcelFlag(token);

            _excelHeader = (ExcelHeader)FileTools.ByteArrayToStructure(_data, typeof(ExcelHeader), offset);
            offset += Marshal.SizeOf(typeof(ExcelHeader));
            if (_excelDebugAll) Debug.Write(String.Format("ExcelHeader: Unknown161 = {0}, Unknown162 = {1}, Unknown163 = {2}, Unknown164 = {3}, Unknown165 = {4}, Unknown166 = {5}, Unknown321 = {6}, Unknown322 = {7}\n", _excelHeader.Unknown161, _excelHeader.Unknown162, _excelHeader.Unknown163, _excelHeader.Unknown164, _excelHeader.Unknown165, _excelHeader.Unknown166, _excelHeader.Unknown321, _excelHeader.Unknown322));


            // strings block
            token = FileTools.ByteArrayTo<Int32>(_data, ref offset);
            CheckExcelFlag(token);

            int stringsBytesCount = FileTools.ByteArrayTo<Int32>(data, ref offset);
            if (stringsBytesCount != 0)
            {
                _stringsBytes = new byte[stringsBytesCount];
                Buffer.BlockCopy(_data, offset, _stringsBytes, 0, stringsBytesCount);

                // put into a hash table for easier use later, with the _stringsBytes[offset] -> offset as the key
                // sometimes C# makes things hard  -  C++ char* ftw. x.x
                for (int i = offset; i < offset + stringsBytesCount; i++)
                {
                    String s = FileTools.ByteArrayToStringAnsi(_data, i);
                    Strings.Add(i - offset, s);

                    i += s.Length;
                }

                offset += stringsBytesCount;
            }


            // tables block
            token = FileTools.ByteArrayTo<Int32>(_data, ref offset);
            CheckExcelFlag(token);

            Count = FileTools.ByteArrayTo<Int32>(_data, ref offset);
            for (int i = 0; i < Count; i++)
            {
                Object table = FileTools.ByteArrayToStructure(data, DataType, offset);
                offset += Marshal.SizeOf(DataType);

                Rows.Add(table);
            }


            // index block
            token = FileTools.ByteArrayTo<Int32>(_data, ref offset);
            CheckExcelFlag(token);

            if ((uint)_excelHeader.StructureId == 0x887988C4) // items, missiles, monsters, objects, players
            {
                TableIndicies = new int[Count];
                _extraIndexData = new List<byte[]>();

                for (int i = 0; i < Count; i++)
                {
                    TableIndicies[i] = FileTools.ByteArrayTo<Int32>(_data, ref offset);
                    int size = FileTools.ByteArrayTo<Int32>(_data, ref offset);
                    byte[] extra = new byte[size];
                    Buffer.BlockCopy(_data, offset, extra, 0, size);
                    offset += size;
                    _extraIndexData.Add(extra);
                }
            }
            else
            {
                TableIndicies = FileTools.ByteArrayToInt32Array(_data, offset, Count);
                offset += Count * sizeof(Int32);

                if (Count == 0)
                {
                    offset += sizeof(Int32);
                }
            }

            // secondary string block
            token = FileTools.ByteArrayTo<Int32>(_data, ref offset);
            if (!CheckFlag(token, 0x68657863)) // 'cxeh'
            {
                int stringCount = token;
                for (int i = 0; i < stringCount; i++)
                {
                    int charCount = FileTools.ByteArrayTo<Int32>(_data, ref offset);
                    String str = FileTools.ByteArrayToStringAnsi(_data, offset);
                    offset += charCount;
                    SecondaryStrings.Add(str);
                }

                if (_excelDebugAll) Debug.Write("Has secondary strings...\n");
            }
            else
            {
                offset -= 4;
            }


            // sort index blocks
            for (int i = 0; i < 4; i++)
            {
                token = FileTools.ByteArrayTo<Int32>(_data, ref offset);
                CheckExcelFlag(token);

                int count = FileTools.ByteArrayTo<Int32>(_data, ref offset);
                _sortIndicies[i] = FileTools.ByteArrayToInt32Array(_data, offset, count);
                offset += count * sizeof(Int32);
            }


            // unknown block
            token = FileTools.ByteArrayTo<Int32>(_data, ref offset);
            CheckExcelFlag(token);

            token = FileTools.ByteArrayTo<Int32>(_data, ref offset);
            if (token != 0x00)
            {
                while (true)
                {
                    if (CheckFlag(token, 0x68736372)) // 'rcsh'
                    {
                        _rcshValue = FileTools.ByteArrayTo<Int32>(_data, ref offset);
                        if (_rcshValue != 0x04)
                        {
                            throw new Exception("_rcshValue = FileTools.ByteArrayTo<Int32>(data, ref offset);\nif (_rcshValue != 0x04)");
                        }
                        if (_excelDebugAll) Debug.Write(String.Format("Has rcsh value = {0}\n", _rcshValue));
                    }
                    else if (CheckFlag(token, 0x68737974)) // 'tysh'
                    {
                        _tyshValue = FileTools.ByteArrayTo<Int32>(_data, ref offset);
                        if (_tyshValue != 0x02)
                        {
                            throw new Exception("_tyshValue = FileTools.ByteArrayTo<Int32>(data, ref offset);\nif (_tyshValue != 0x02)");
                        }
                        if (_excelDebugAll) Debug.Write(String.Format("Has tysh value = {0}\n", _tyshValue));
                    }
                    else if (CheckFlag(token, 0x6873796D)) // 'mysh'
                    {
                        offset -= 4;
                        ParseMyshTables(_data, ref offset);
                        if (_excelDebugAll) Debug.Write(String.Format("Has mysh value = true\n"));
                    }
                    else if (CheckFlag(token, 0x68656E64)) // 'dneh'
                    {
                        _dnehValue = FileTools.ByteArrayTo<Int32>(_data, ref offset);
                        if (_dnehValue != 0x00)
                        {
                            throw new Exception("_dnehValue = FileTools.ByteArrayTo<Int32>(data, ref offset);\nif (_dnehValue != 0x02)");
                        }
                        if (_excelDebugAll) Debug.Write(String.Format("Has dneh value = {0}\n", _dnehValue));
                    }
                    else // 'cxeh'  -  starting next block
                    {
                        CheckExcelFlag(token);
                        offset -= 4;
                        break;
                    }

                    token = FileTools.ByteArrayTo<Int32>(_data, ref offset);
                }
            }


            // data block
            token = FileTools.ByteArrayTo<Int32>(_data, ref offset);
            if (token != 0)
            {
                CheckExcelFlag(token);
                int byteCount = FileTools.ByteArrayTo<Int32>(_data, ref offset);
                if (byteCount != 0)
                {
                    if (_excelHeader.StructureId == 0x1F9DDC98)                                 // Only seen in unittypes.txt.cooked so far.
                    {                                                                           // This block reading method is the same as first seen below in the states.txt.cooked,
                        int blockCount = FileTools.ByteArrayTo<Int32>(_data, ref offset);   // but there is no data in the previous block for unittypes.txt.cooked.
                        byteCount = (byteCount << 2) * blockCount;
                        Debug.Write(String.Format("Has weird block .Length = {0}\n", byteCount));
                    }
                    _dataBlock = new byte[byteCount];
                    Buffer.BlockCopy(_data, offset, _dataBlock, 0, byteCount);
                    offset += byteCount;
                    Debug.Write(String.Format("Has data block .Length = {0}\n", byteCount));
                }
            }


            // does it have a final flag chunk?
            if (offset != _data.Length)
            {
                token = FileTools.ByteArrayTo<Int32>(_data, ref offset);
                if (token != 0)
                {
                    CheckExcelFlag(token);

                    int byteCount = FileTools.ByteArrayTo<Int32>(_data, ref offset);
                    int blockCount = FileTools.ByteArrayTo<Int32>(_data, ref offset);
                    byteCount = (byteCount << 2) * blockCount;

                    if (byteCount != 0)        // Only seen in states.txt.cooked so far  -  Of note is that 
                    {                          //           the states file has an above data block as well.
                        FinalBytes = new byte[byteCount];
                        Buffer.BlockCopy(_data, offset, FinalBytes, 0, FinalBytes.Length);
                        offset += FinalBytes.Length;
                    }
                    if (_excelDebugAll) Debug.Write(String.Format("Has final block .Length = {0}\n", byteCount));
                }
            }

            if (offset != _data.Length)
            {
                throw new Exception("offset != data.Length");
            }

            if (_excelDebugAll) Debug.Write("\n");

            IsGood = true;
            return true;
        }

        private void CheckExcelFlag(int flag)
        {
            if (!CheckFlag(flag, 0x68657863))
            {
                throw new Exception("Unexpected header flag!\nStructure ID: " + _excelHeader.StructureId);
            }
        }

        public void ParseDataBlock(DataTable dt)
        {
            int[] columns;
            List<int> length = new List<int>();
            int last = 1;

            // Retrieve the ordinals of IntOffset columns.
            int count = 0;

            foreach (DataColumn dc in dt.Columns)
            {
                if (dc.ExtendedProperties.Contains(ExcelFile.ColumnTypeKeys.IsIntOffset) && dc.ExtendedProperties.Contains(ExcelFile.ColumnTypeKeys.IntOffsetOrder))
                {
                    int i = (int)dc.ExtendedProperties[ExcelFile.ColumnTypeKeys.IntOffsetOrder];
                    if (i == 0) continue;
                    count++;
                }
            }

            // Retrieve the ordinals of IntOffset columns.
            columns = new int[count];

            foreach (DataColumn dc in dt.Columns)
            {
                if (dc.ExtendedProperties.Contains(ExcelFile.ColumnTypeKeys.IsIntOffset) && dc.ExtendedProperties.Contains(ExcelFile.ColumnTypeKeys.IntOffsetOrder))
                {
                    int i = (int)dc.ExtendedProperties[ExcelFile.ColumnTypeKeys.IntOffsetOrder];
                    if (i == 0) continue;
                    columns[i - 1] = dc.Ordinal;
                }
            }

            // Calculate the length of each cell.
            foreach (DataRow dr in dt.Rows)
            {
                foreach (int c in columns)
                {
                    int val = int.Parse((string)dr[c]);
                    if (val == 0) continue; // Skip if there is no data.
                    if (val == 1) continue; // Skip first cell, only passes once.
                    int i = val - last;
                    length.Add(i);
                    last = val;
                }
            }
            // Get length of last cell.. required.
            int lc = _dataBlock.Length - last;
            length.Add(lc);

            // Populates the grid resolving the IntPtrs
            int cur = 0;

            DataBlock = new Hashtable();

            for (int r = 0; r < dt.Rows.Count; r++)
            {
                // Add the rest of the columns.
                for (int c = 0; c < columns.Length; c++)
                {
                    int val = int.Parse(((string)dt.Rows[r][columns[c]]));
                    if (val == 0) continue; // Skip if there is no data.

                    if (length[cur] < 0) // There are about 5 cases where the length is a negative integer.
                    {                    // Need this while we work out whats going on.
                        cur++;
                        //grid[r + 1, c + 1] = "ERROR";
                        continue;
                    }

                    byte[] buffer = new byte[length[cur]];
                    Array.Copy(_dataBlock, val, buffer, 0, buffer.Length);

                    int[] ibuffer = FileTools.ByteArrayToInt32Array(buffer, 0, buffer.Length / sizeof(int));
                    string csv = "";
                    for (int i = 0; i < ibuffer.Length; i++)
                    {
                        csv += ibuffer[i].ToString();
                        if (i < ibuffer.Length - 1)
                            csv += ",";
                    }

                    DataBlock.Add(val, csv);
                    cur++;
                }
            }
        }

        public string ParseIntOffset(int index)
        {
            int cur = index;
            StringWriter csv = new StringWriter();
            bool parsing = true;
            List<int> ilist = new List<int>();

            while (parsing)
            {
                int icase = FileTools.ByteArrayToInt32(_dataBlock, cur);
                cur += sizeof(int);
                csv.Write(icase.ToString());
                
                switch (icase)
                {
                    case 2:
                        csv.Write(",");
                        csv.Write((uint)FileTools.ByteArrayToInt32(_dataBlock, cur)); // int value between 270-370
                        cur += sizeof(int);
                        csv.Write(",");
                        csv.Write((uint)FileTools.ByteArrayToInt32(_dataBlock, cur)); // 0
                        cur += sizeof(int);
                        csv.Write(",");
                        break;

                    // simple integer
                    case 1:
                    case 3:
                    case 4:
                    case 14:
                    case 26:
                    case 516:
                    case 527:
                    case 700:
                        csv.Write(",");
                        csv.Write((uint)FileTools.ByteArrayToInt32(_dataBlock, cur)); // int value
                        cur += sizeof(int);
                        csv.Write(",");
                        break;

                    // tokens
                    case 320:
                    case 339:
                    case 347:
                    case 358: // items
                    case 399: // items
                    case 388: // items
                    case 369: // items
                    case 426: // only on (8)healthpack and (9)powerpack. pick up condition
                    case 437: // only used in skills tabl
                    case 448:
                    case 470:
                    case 481:
                    
                    case 538: // affixes
                    case 709:
                    case 711:
                    case 712:
                        csv.Write(",");
                        break;

                    // bitmasks
                    case 666:
                    case 667:
                    case 669:
                    case 673:
                    case 674:
                    case 680:
                    case 683:
                    case 687:
                    case 688:
                        csv.Write(",");
                        csv.Write((uint)FileTools.ByteArrayToInt32(_dataBlock, cur)); // int bitmask
                        cur += sizeof(int);
                        csv.Write(",");
                        break;

                    // functions
                    case 707:
                        csv.Write(",");
                        csv.Write((uint)FileTools.ByteArrayToInt32(_dataBlock, cur)); // Boolean? 1/0
                        cur += sizeof(int);
                        csv.Write(",");
                        csv.Write((uint)FileTools.ByteArrayToInt32(_dataBlock, cur));  // 714
                        cur += sizeof(int);
                        csv.Write(",");
                        break;
                    // random range?
                    case 708: // 26,3,26
                        csv.Write(",");
                        csv.Write((uint)FileTools.ByteArrayToInt32(_dataBlock, cur)); // 26
                        cur += sizeof(int);
                        csv.Write(",");
                        csv.Write((uint)FileTools.ByteArrayToInt32(_dataBlock, cur));  // value
                        cur += sizeof(int);
                        csv.Write(",");
                        csv.Write((uint)FileTools.ByteArrayToInt32(_dataBlock, cur)); // 3
                        cur += sizeof(int);
                        csv.Write(",");
                        csv.Write((uint)FileTools.ByteArrayToInt32(_dataBlock, cur));  // value
                        cur += sizeof(int);
                        csv.Write(",");
                        break;
                    case 710: // 26,26,3
                        csv.Write(",");
                        csv.Write((uint)FileTools.ByteArrayToInt32(_dataBlock, cur)); // 26
                        cur += sizeof(int);
                        csv.Write(",");
                        csv.Write((uint)FileTools.ByteArrayToInt32(_dataBlock, cur));  // value
                        cur += sizeof(int);
                        csv.Write(",");
                        csv.Write((uint)FileTools.ByteArrayToInt32(_dataBlock, cur)); // 26
                        cur += sizeof(int);
                        csv.Write(",");
                        csv.Write((uint)FileTools.ByteArrayToInt32(_dataBlock, cur));  // value
                        cur += sizeof(int);
                        csv.Write(",");
                        csv.Write((uint)FileTools.ByteArrayToInt32(_dataBlock, cur)); // 3
                        cur += sizeof(int);
                        csv.Write(",");
                        csv.Write((uint)FileTools.ByteArrayToInt32(_dataBlock, cur)); // value
                        cur += sizeof(int);
                        csv.Write(",");
                        break;
                    case 0:
                    case 6: // appears twice in skills. weird. it acts as a terminator
                        parsing = false;
                        break;
                    default:
                        parsing = false;
                        Console.WriteLine("Unhandled case: {0}", icase);
                        break;
                }
            }

            return csv.ToString();
        }

        private static bool CheckFlag(int flag, int to)
        {
            return flag == to ? true : false;
        }

        // todo: fix me
        private static void ParseMyshTables(byte[] data, ref int offset)
        {
            //int totalAttributeCount = 0;
            //int attributeCount = 0;
            //int blockCount = 0;
            //int flagCount = 0;
            while (offset < data.Length)
            {
                ////////////// temp fix /////////////////
                int f = BitConverter.ToInt32(data, offset);
                if (CheckFlag(f, 0x68657863))
                {
                    //Debug.Write("mysh flagCount = " + flagCount + "\n");
                    break;
                }
                //if (CheckFlag(f, 0x6873796D))
                //{
                //    flagCount++;
                //}
                offset++;
                continue;
                ////////////// temp fix /////////////////
                /*

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
                 */
            }
        }

        public override byte[] GenerateFile(DataTable dataTable)
        {
            const int zeroValue = 0;
            byte[] buffer = new byte[1024];
            int byteOffset = 0;
            byte[] intBytes = null;
            int intByteCount = 0;

            // File Header
            FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTokens.StartOfBlock);
            FileTools.WriteToBuffer(ref buffer, ref byteOffset, _excelHeader);

            // Strings Block
            FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTokens.StartOfBlock);
            int stringsByteCount = 0;
            int stringsByteOffset = byteOffset;
            byte[] stringBytes = null;
            byteOffset += sizeof(Int32);

            // Tables Block
            FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTokens.StartOfBlock);
            FileTools.WriteToBuffer(ref buffer, ref byteOffset, dataTable.Rows.Count);

            Type tableType = Rows[0].GetType();

            TableHeader defaultTableHeader;
            defaultTableHeader.Unknown1 = 0x03;
            defaultTableHeader.Unknown2 = 0x3F;
            defaultTableHeader.VersionMajor = 0x00;
            defaultTableHeader.Reserved1 = -1;
            defaultTableHeader.VersionMinor = 0x00;
            defaultTableHeader.Reserved2 = -1;

            #region Parse DataSet
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
                        // if the row we're on is less than the table count, then we have an already present value
                        if (Rows.Count > row)
                        {
                            // TODO This method means you must *not* delete existing rows. Doing so will mis-align all following rows!
                            fieldInfo.SetValue(table, fieldInfo.GetValue(Rows[row]));
                        }
                        else // must be a new/added row
                        {
                            if (fieldInfo.FieldType == typeof(TableHeader))
                            {
                                fieldInfo.SetValue(table, defaultTableHeader);
                            }
                            else
                            {
                                if (fieldInfo.FieldType.BaseType == typeof(Array))
                                {
                                    Array arrayBase = (Array)fieldInfo.GetValue(Rows[0]);
                                    Array ar = (Array)Activator.CreateInstance(fieldInfo.FieldType, arrayBase.Length);
                                    fieldInfo.SetValue(table, ar);
                                }
                                else if (fieldInfo.FieldType == typeof(String))
                                {
                                    fieldInfo.SetValue(table, String.Empty);
                                }
                                else if (fieldInfo.FieldType == typeof(Int32))
                                {
                                    fieldInfo.SetValue(table, -1);
                                }
                                else
                                {
                                    fieldInfo.SetValue(table, 0);
                                }
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
                    // if its a int offset, convert the csv to a byte array-add it to the buffer and set value as offset
                    else if (dc.ExtendedProperties.Contains(ColumnTypeKeys.IsIntOffset))
                    {
                        if ((bool)dc.ExtendedProperties[ColumnTypeKeys.IsIntOffset])
                        {
                            if (intBytes == null)
                            {
                                byte b = 0;
                                intBytes = new byte[1024];
                                FileTools.WriteToBuffer(ref intBytes, ref intByteCount, b);
                            }

                            String s = dr[dc] as String;
                            if (s == "0" || s.Length == 0)
                            {
                                fieldInfo.SetValue(table, 0);
                            }
                            else
                            {
                                fieldInfo.SetValue(table, intByteCount);
                                string[] explode = s.Split(',');

                                foreach (string part in explode)
                                {
                                    FileTools.WriteToBuffer(ref intBytes, ref intByteCount, uint.Parse(part));
                                }
                            }
                        }
                    }
                    else
                    {
                        if (row < Rows.Count)
                        {
                            Object o = dr[dc];
                            fieldInfo.SetValue(table, o);
                        }
                        else
                        {
                            // TODO this section is just gross and needs to be done properly
                            ExcelOutputAttribute excelOutputAttribute = GetExcelOutputAttribute(fieldInfo);
                            if (excelOutputAttribute == null || !excelOutputAttribute.IsBitmask)
                            {
                                Object o = dr[dc];
                                Type type = o.GetType();
                                if (type == typeof(DBNull))
                                {
                                    if (fieldInfo.FieldType == typeof(byte))
                                    {
                                        const byte b = 0;
                                        fieldInfo.SetValue(table, b);
                                    }
                                    else if (fieldInfo.FieldType == typeof(String))
                                    {
                                        fieldInfo.SetValue(table, String.Empty);
                                    }
                                    else
                                    {
                                        o = 0;
                                        fieldInfo.SetValue(table, o);
                                    }
                                }
                                else
                                {
                                    fieldInfo.SetValue(table, o);
                                }

                            }
                            else
                            {
                                Object o = dr[dc];
                                fieldInfo.SetValue(table, o);
                            }
                        }
                    }
                    col++;
                }

                FileTools.WriteToBuffer(ref buffer, ref byteOffset, table);
                row++;
            }
            #endregion

            // write strings block
            if (stringBytes != null && stringsByteCount > 0)
            {
                FileTools.WriteToBuffer(ref buffer, stringsByteOffset, stringsByteCount);
                FileTools.WriteToBuffer(ref buffer, stringsByteOffset + sizeof(Int32), stringBytes, stringsByteCount, true);
                byteOffset += stringsByteCount;
            }

            // PRIMARY INDEX
            FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTokens.StartOfBlock);

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                FileTools.WriteToBuffer(ref buffer, ref byteOffset, i);

                if ((uint)_excelHeader.StructureId == 0x887988C4) // items, missiles, monsters, objects, players
                {
                    FileTools.WriteToBuffer(ref buffer, ref byteOffset, _extraIndexData[i].Length);
                    FileTools.WriteToBuffer(ref buffer, ref byteOffset, _extraIndexData[i]);
                }
            }

            // secondary string block
            Int32 secondaryStringCount = SecondaryStrings.Count;
            if (secondaryStringCount > 0)
            {
                FileTools.WriteToBuffer(ref buffer, ref byteOffset, secondaryStringCount);

                for (int i = 0; i < secondaryStringCount; i++)
                {
                    String str = SecondaryStrings[i];
                    Int32 strCharCount = str.Length + 1; // +1 for \0
                    FileTools.WriteToBuffer(ref buffer, ref byteOffset, strCharCount);
                    byte[] strBytes = FileTools.StringToASCIIByteArray(str);
                    FileTools.WriteToBuffer(ref buffer, ref byteOffset, strBytes);
                    byteOffset++; // +1 for \0
                }
            }

            // generate and sort the indicies
            foreach (DataColumn dc in dataTable.Columns)
            {
                if (!dc.ExtendedProperties.Contains(ColumnTypeKeys.SortId)) continue;

                int sortId = (int)dc.ExtendedProperties[ColumnTypeKeys.SortId] - 1;
                string ctype = dc.DataType == typeof(string) ? "''" : "0";  // modify the expr depending on data type
                string expr = dc.ColumnName + " <> " + ctype;
                string sort =  dc.ColumnName + " ASC";
                DataRow[] sortedTable = dataTable.Select(expr, sort);

                _sortIndicies[sortId] = new int[sortedTable.Length];

                for (int i = 0; i < sortedTable.Length; i++)
                {
                    _sortIndicies[sortId][i] = (int)sortedTable[i][0];
                }
            }

            // write the indicies to the buffer
            FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTokens.StartOfBlock);
            FileTools.WriteToBuffer(ref buffer, ref byteOffset, SortIndex1.Length);
            FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTools.IntArrayToByteArray(SortIndex1));
            FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTokens.StartOfBlock);
            FileTools.WriteToBuffer(ref buffer, ref byteOffset, SortIndex2.Length);
            FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTools.IntArrayToByteArray(SortIndex2));
            FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTokens.StartOfBlock);
            FileTools.WriteToBuffer(ref buffer, ref byteOffset, SortIndex3.Length);
            FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTools.IntArrayToByteArray(SortIndex3));
            FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTokens.StartOfBlock);
            FileTools.WriteToBuffer(ref buffer, ref byteOffset, SortIndex4.Length);
            FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTools.IntArrayToByteArray(SortIndex4));

            // weird unknown header chunks
            if (_rcshValue != 0)
            {
                FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTokens.StartOfBlock);
                FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTokens.TokenRcsh);
                FileTools.WriteToBuffer(ref buffer, ref byteOffset, _rcshValue);
                FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTokens.TokenTysh);
                FileTools.WriteToBuffer(ref buffer, ref byteOffset, _tyshValue);
                //FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTokens.TokenMysh);
                //FileTools.WriteToBuffer(ref buffer, ref byteOffset, zeroValue);
                FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTokens.TokenDneh);
                FileTools.WriteToBuffer(ref buffer, ref byteOffset, _dnehValue);
            }

            // Data Block 1. Holds the 'IntOffset' data.
            FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTokens.StartOfBlock);
            FileTools.WriteToBuffer(ref buffer, ref byteOffset, intByteCount);
            if (intBytes != null) // TODO. shouldnt have to do this here
            {
                byte[] tmp = new byte[intByteCount];
                Array.Copy(intBytes, tmp, intByteCount);
                FileTools.WriteToBuffer(ref buffer, ref byteOffset, tmp);
            }

            // Data Block 2
            int finalBytesCount = FinalBytes == null ? 0 : FinalBytes.Length;
            FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTokens.StartOfBlock);
            FileTools.WriteToBuffer(ref buffer, ref byteOffset, finalBytesCount);
            if (finalBytesCount > 0)
            {
                FileTools.WriteToBuffer(ref buffer, ref byteOffset, FinalBytes);
            }

            // One last null
            FileTools.WriteToBuffer(ref buffer, ref byteOffset, zeroValue);

            // return final buffer
            byte[] returnBuffer = new byte[byteOffset];
            Buffer.BlockCopy(buffer, 0, returnBuffer, 0, byteOffset);

            return returnBuffer;
        }

        public static ExcelOutputAttribute GetExcelOutputAttribute(FieldInfo fieldInfo)
        {
            foreach (Attribute attribute in fieldInfo.GetCustomAttributes(typeof(ExcelOutputAttribute), true))
            {
                return attribute as ExcelOutputAttribute;
            }

            return null;
        }

        // todo: FIXME
        // figure out how to better do this - was in old Stats class
        public string GetStringFromId(int id)
        {
            foreach (StatsRow statsTable in Rows.Cast<StatsRow>().Where(statsTable => statsTable.code == id))
            {
                return statsTable.stat;
            }

            return "NOT FOUND";
        }
    }
}