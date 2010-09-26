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
using SkmDataStructures2;

namespace Reanimator
{
    public partial class ExcelFile : DataFile
    {
        public ExcelHeader FileExcelHeader { get; private set; }
       
        private byte[] _stringsBytes;

        public Hashtable Strings { get; private set; }
        public int[] TableIndicies { get; private set; }
        public byte[] IntPtrData { get; private set; }
        public List<byte[]> AiData { get; private set; }
        public List<byte[]> FinalData { get; private set; }
        public List<String> SecondaryStrings { get; private set; }
        

        /**
         * Updated Members for newer methods
         * */
        private List<String> StringTable { set; get; }
        private List<Int32[]> IntegerTable { get; set; }
        public Boolean IntegrityCheck { get; private set; }
        public UInt32 StructureID { get; private set; }



        private Int32 _rcshValue;
        private Int32 _tyshValue;
        private Int32 _dnehValue;

        private int[][] _sortIndicies;
        public int[] SortIndex1 { get { return _sortIndicies[0]; } }
        public int[] SortIndex2 { get { return _sortIndicies[1]; } }
        public int[] SortIndex3 { get { return _sortIndicies[2]; } }
        public int[] SortIndex4 { get { return _sortIndicies[3]; } }

        // these are only public to pass them through the conversion
        // eventually better methods will be implemented
        public byte[] MyshBytes { get; set; }


        const uint idItems = 0x887988C4;
        const uint idItemsTc = 0xE08E6C41;
        const uint idUnitTypes = 0x1F9DDC98;
        const uint idUintTypesTc = 0x5CE494C9;


        public ExcelFile(String stringId, Type type)
            : base(stringId, type)
        {
            IsExcelFile = true;
        }


        public ExcelFile(byte[] CSVsource)
        {
            IntegrityCheck = false;
            if (CSVsource == null) return;
            if (CSVsource.Length < 64) return;
            int offset = 0;
            int length = CSVsource.Length;
            byte delimiter = (byte)'\t';
            string stringToken = FileTools.ByteArrayToStringASCII(CSVsource, ref offset, 8);
            uint typeToken = Convert.ToUInt32(stringToken, 16);
            int stringCursor = 1;
            int integerCursor = 1;
            bool skipFirstRow = true;
            StringTable = new List<string>();
            IntegerTable = new List<int[]>();

            if (!(DataTypes.Contains(typeToken))) return;
            DataType = (Type)DataTypes[typeToken];

            string[][] tableRows = FileTools.CSVtoStringArray(CSVsource, DataType.GetFields().Count(), delimiter);
            if (tableRows == null) return;
            

            int column = 0;
            for (int row = 0; row < tableRows.Count(); row++)
            {
                Object newRow = new Object();
                foreach (FieldInfo field in DataType.GetFields())
                {
                    if ((skipFirstRow) && (row == 0)) continue;
                    if ((field.IsPrivate)) continue; // build the private fields on file generation ToByteArray() etc
                    string value = tableRows[row][column];
                    ExcelOutputAttribute attribute = GetExcelOutputAttribute(field);

                    if ((attribute.IsStringOffset))
                    {
                        StringTable.Add(value);
                        int position = StringTable.Count;
                        field.SetValue(newRow, position);
                    }
                    else if ((attribute.IsIntOffset))
                    {
                        value = value.Replace("\"", "");
                        string[] splitValue = value.Split(',');
                        int count = splitValue.Length;
                        int[] intValue = new int[count];
                        for (int i = 0; i < count; i++)
                            intValue[i] = int.Parse(splitValue[i]);
                        IntegerTable.Add(intValue);
                        int position = IntegerTable.Count;
                        field.SetValue(newRow, position);
                    }
                    else if (field.FieldType == typeof(String))
                        field.SetValue(newRow, value);
                    else if (field.FieldType == typeof(Int32))
                        field.SetValue(newRow, Int32.Parse(value));
                    else if (field.FieldType == typeof(UInt32))
                        field.SetValue(newRow, UInt32.Parse(value));
                    else if (field.FieldType == typeof(Single))
                        field.SetValue(newRow, Single.Parse(value));
                    else if (field.FieldType == typeof(Int16))
                        field.SetValue(newRow, Int16.Parse(value));
                    else if (field.FieldType == typeof(UInt16))
                        field.SetValue(newRow, UInt16.Parse(value));
                    else if (field.FieldType == typeof(Byte))
                        field.SetValue(newRow, Byte.Parse(value));
                    else if (field.FieldType == typeof(Char))
                        field.SetValue(newRow, Char.Parse(value));
                    else if (field.FieldType == typeof(Int64))
                        field.SetValue(newRow, Int64.Parse(value));
                    else if (field.FieldType == typeof(UInt64))
                        field.SetValue(newRow, UInt64.Parse(value));

                    column++;
                }
                Rows.Add(newRow);
            }
            IntegrityCheck = true;
        }

        public byte[] ToByteArray()
        {
            List<byte> buffer = new List<byte>();
            List<byte> stringTableBuffer = new List<byte>();
            stringTableBuffer.Add((byte)0); // add 1 for internal offsetting
            List<byte> integerTableBuffer = new List<byte>();
            integerTableBuffer.Add((byte)0);// add 1 for internal offsetting
            byte[] cxehToken = BitConverter.GetBytes(FileTokens.StartOfBlock);
            buffer.AddRange(cxehToken);
            
            ExcelHeader header = new ExcelHeader
            {
                StructureId = StructureID,
                Unknown321 = 1,
                Unknown322 = 17,
                Unknown161 = 20,
                Unknown162 = 20,
                Unknown163 = 0,
                Unknown164 = 0,
                Unknown165 = 20,
                Unknown166 = 0
            };
            buffer.AddRange(FileTools.StructureToByteArray(header));
            buffer.AddRange(cxehToken);
            int stringByteCountOffset = buffer.Count; // insert here later
            buffer.AddRange(cxehToken);
            TableHeader tableHeader = new TableHeader
            {
                Unknown1 = 0x03,
                Unknown2 = 0x3F,
                VersionMajor = 0x00,
                Reserved1 = -1,
                VersionMinor = 0x00,
                Reserved2 = -1
            };

            foreach (Object row in Rows)
            {
                foreach (FieldInfo field in DataType.GetFields())
                {
                    if ((field.IsPrivate)) // initialize all private fields
                    {
                        if ((field.FieldType == typeof(TableHeader)))
                            field.SetValue(row, tableHeader);
                        else if (field.FieldType.BaseType == typeof(Array))
                        {
                            Array arrayBase = (Array)field.GetValue(Rows[0]);
                            Array ar = (Array)Activator.CreateInstance(field.FieldType, arrayBase.Length);
                            field.SetValue(row, ar);
                        }
                        else if (field.FieldType == typeof(String))
                            field.SetValue(row, String.Empty);
                        else if (field.FieldType == typeof(Int32))
                            field.SetValue(row, (Int32)(-1));
                        else if (field.FieldType == typeof(UInt32))
                            field.SetValue(row, (UInt32)0);
                        else if (field.FieldType == typeof(Int64))
                            field.SetValue(row, (Int64)(-1));
                        else if (field.FieldType == typeof(UInt64))
                            field.SetValue(row, (UInt64)0);
                        else if (field.FieldType == typeof(Single))
                            field.SetValue(row, (Single)0);
                        else if (field.FieldType == typeof(Int16))
                            field.SetValue(row, (Int16)0);
                        else if (field.FieldType == typeof(UInt16))
                            field.SetValue(row, (UInt16)0);
                        else if (field.FieldType == typeof(Byte))
                            field.SetValue(row, (Byte)0);
                        else if (field.FieldType == typeof(Char))
                            field.SetValue(row, (Char)0);
                    }

                    ExcelOutputAttribute attribute = GetExcelOutputAttribute(field);
                    if ((attribute.IsStringOffset))
                    {
                        int i = (int)field.GetValue(row);
                        string stringValue = StringTable[i];
                        stringTableBuffer.AddRange(FileTools.StringToASCIIByteArray(stringValue));
                        int offset = stringTableBuffer.Count;
                        field.SetValue(row, offset);
                    }
                    if ((attribute.IsIntOffset))
                    {
                        int i = (int)field.GetValue(row);
                        int[] integerValue = IntegerTable[i];
                        integerTableBuffer.AddRange(FileTools.IntArrayToByteArray(integerValue));
                        int offset = integerTableBuffer.Count;
                        field.SetValue(row, offset);
                    }

                    // Check and create any sorting patterns
                    if ((attribute.SortAscendingID != 0) || (attribute.SortDistinctID != 0) || (attribute.SortPostOrderID != 0))
                    {
                        //IEnumerable<int> indiceOrder = Rows.Select(i => i
                    }


                    

                    buffer.AddRange(FileTools.StructureToByteArray(row));
                }
            }

            // Now all the offset strings have been read, merge the arrays
            byte[] stringArrayLength = BitConverter.GetBytes(stringTableBuffer.Count);
            buffer.InsertRange(stringByteCountOffset, stringArrayLength);
            stringByteCountOffset += stringArrayLength.Length;// 4 bytes
            buffer.InsertRange(stringByteCountOffset, stringTableBuffer);

            //todo: secondary strings


            int[] indices = new int[Rows.Count];
            for (int i = 0; i < Rows.Count; i++)
                indices[i] = i;
            buffer.AddRange(cxehToken);
            buffer.AddRange(FileTools.IntArrayToByteArray(indices));

            buffer.AddRange(cxehToken);


            return null;
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

            // main header
            int token = FileTools.ByteArrayToInt32(_data, ref offset);
            CheckExcelFlag(token);

            FileExcelHeader = FileTools.ByteArrayToStructure<ExcelHeader>(_data, ref offset);

            // strings block
            token = FileTools.ByteArrayToInt32(_data, ref offset);
            CheckExcelFlag(token);

            int stringsBytesCount = FileTools.ByteArrayToInt32(data, ref offset);
            if (stringsBytesCount != 0)
            {
                _stringsBytes = new byte[stringsBytesCount];
                Buffer.BlockCopy(_data, offset, _stringsBytes, 0, stringsBytesCount);

                // put into a hash table for easier use later, with the _stringsBytes[offset] -> offset as the key
                // sometimes C# makes things hard  -  C++ char* ftw. x.x
                for (int i = offset; i < offset + stringsBytesCount; i++)
                {
                    String s = FileTools.ByteArrayToStringASCII(_data, i);
                    Strings.Add(i - offset, s);

                    i += s.Length;
                }
                offset += stringsBytesCount;
            }

            // tables block
            token = FileTools.ByteArrayToInt32(_data, ref offset);
            CheckExcelFlag(token);

            Count = FileTools.ByteArrayToInt32(_data, ref offset);
            Object[] rows = new Object[Count];
            for (int i = 0; i < Count; i++)
            {
                rows[i] = FileTools.ByteArrayToStructure(_data, DataType, ref offset);
                
            }
            Rows.AddRange(rows);


            // index block
            token = FileTools.ByteArrayToInt32(_data, ref offset);
            CheckExcelFlag(token);

            // tables of the type items have ai data beside each indice
            if (FileExcelHeader.StructureId == idItems ||
                FileExcelHeader.StructureId == idItemsTc)
            {
                TableIndicies = new int[Count];
                AiData = new List<byte[]>();

                for (int i = 0; i < Count; i++)
                {
                    TableIndicies[i] = FileTools.ByteArrayToInt32(_data, ref offset);
                    int size = FileTools.ByteArrayToInt32(_data, ref offset);
                    byte[] extra = new byte[size];
                    Buffer.BlockCopy(_data, offset, extra, 0, size);
                    offset += size;
                    AiData.Add(extra);
                }
            }
            else
            {
                TableIndicies = FileTools.ByteArrayToInt32Array(_data, ref offset, Count);
            }


            // secondary string block
            token = FileTools.ByteArrayToInt32(_data, ref offset);
            if (!CheckFlag(token, FileTokens.StartOfBlock))
            {
                int stringCount = token;
                for (int i = 0; i < stringCount; i++)
                {
                    int charCount = FileTools.ByteArrayToInt32(_data, ref offset);
                    String str = FileTools.ByteArrayToStringASCII(_data, offset);
                    offset += charCount;
                    SecondaryStrings.Add(str);
                }
            }
            else
            {
                offset -= 4;
            }


            // sort index blocks
            // ascending sorted array positions of existing objects, excludes blanks
            // can contain none or up to 4 arrays
            for (int i = 0; i < _sortIndicies.Length; i++)
            {
                token = FileTools.ByteArrayToInt32(_data, ref offset);
                CheckExcelFlag(token);

                int count = FileTools.ByteArrayToInt32(_data, ref offset);
                _sortIndicies[i] = FileTools.ByteArrayToInt32Array(_data, ref offset, count);
            }


            // token is duplicated here
            token = FileTools.ByteArrayToInt32(_data, ref offset);
            CheckExcelFlag(token);


            // rcsh, tysh, mysh, dneh
            token = FileTools.ByteArrayToInt32(_data, ref offset);
            if (token != 0)
            {
                // rcsh
                _rcshValue = FileTools.ByteArrayToInt32(_data, ref offset); //0x04 const
                token = FileTools.ByteArrayToInt32(_data, ref offset);

                // tysh
                _tyshValue = FileTools.ByteArrayToInt32(_data, ref offset); //0x02 const
                token = FileTools.ByteArrayToInt32(_data, ref offset);

                // mysh - skills and stats only
                // todo: needs parsing.. see _ParseMyshTables method
                if (token == FileTokens.TokenMysh)
                {
                    _ParseMyshTables(_data, ref offset);
                    token = FileTools.ByteArrayToInt32(_data, ref offset);
                }

                // dneh
                _dnehValue = FileTools.ByteArrayToInt32(_data, ref offset); //0x00 const
            }
            else
            {
                int paise = 0;
                paise = paise + 2;
            }


            // intptr data block
            // its wierd the unittypes table ignores this part
            if (FileExcelHeader.StructureId != idUnitTypes &&
                FileExcelHeader.StructureId != idUintTypesTc)
            {
                token = FileTools.ByteArrayToInt32(_data, ref offset);
                if (token != 0)
                {
                    CheckExcelFlag(token);
                    int byteCount = FileTools.ByteArrayToInt32(_data, ref offset);
                    if (byteCount != 0)
                    {
                        IntPtrData = new byte[byteCount];
                        Buffer.BlockCopy(_data, offset, IntPtrData, 0, byteCount);
                        offset += byteCount;
                    }
                }
            }


            // final bytes
            // only exists in unittypes and states
            // todo: needs identifying... what does it mean?
            token = FileTools.ByteArrayToInt32(_data, ref offset);
            if (token != 0)
            {
                CheckExcelFlag(token);

                int byteCount = FileTools.ByteArrayToInt32(_data, ref offset);
                int blockCount = FileTools.ByteArrayToInt32(_data, ref offset);
                if (byteCount != 0) // states & unittypes only
                {
                    byteCount = byteCount << 2;
                    FinalData = new List<byte[]>(blockCount);
                    for (int i = 0; i < blockCount; i++)
                    {
                        byte[] buffer = new byte[byteCount];
                        Buffer.BlockCopy(data, offset, buffer, 0, byteCount);
                        offset += byteCount;
                        FinalData.Add(buffer);
                    }
                }
            }

            // isgood if eof
            return IsGood = offset == data.Length;
        }

        private void CheckExcelFlag(int flag)
        {
            if (!CheckFlag(flag, FileTokens.StartOfBlock))
            {
                throw new Exception("Unexpected header flag!\nStructure ID: " + FileExcelHeader.StructureId);
            }
        }

        public string ParseIntOffset(int index)
        {
            int i;
            int len = 0;
            bool parsing = true;
            
            while (parsing)
            {
                i = FileTools.ByteArrayToInt32(IntPtrData, index + len);
                len += sizeof(int) * 1;
                
                switch (i)
                {
                    case 2: //2,x,0
                    case 98://skills//98,x,0
                        len += sizeof(int) * 2;
                        break;
                    case 1:
                    case 3:
                    case 4:
                    case 5://tcv4 skills
                    case 6://skills
                    case 14:
                    case 26:
                    case 50: //skills. 50,0
                    case 86://tcv4 skills
                    case 516:
                    case 527:
                        

                        len += sizeof(int) * 1;
                        break;
                    case 700:
                        len += sizeof(int) * 1;
                        break;
                    case 320:
                    case 333://tcv4 skills
                    case 339:
                    case 347:
                    case 358: // items
                    case 399: // items
                    case 388: // items
                    case 369: // items
                    case 418: // some tcv4 table
                    case 426: // only on (8)healthpack and (9)powerpack. pick up condition
                    case 437: // only used in skills tabl
                    case 448:
                    case 459: // music condition
                    case 470:
                    case 481:
                    case 538: // affixes
                    case 709:
                    case 711:
                    case 712:
                        break;
                    case 666:
                    case 667:
                    case 669:
                    case 673:
                    case 674:
                    case 680:
                    case 683:
                    case 687:
                    case 688:
                        len += sizeof(int) * 1;
                        break;
                    case 707:
                        len += sizeof(int) * 2;
                        break;
                    case 708: // 26,3,26
                        //len += sizeof(int) * 4;
                        break;
                    case 710: // 26,26,3
                        //len += sizeof(int) * 6;
                        break;
                    case 0:
                    //case 6: // appears twice in skills. weird. it acts as a terminator
                        parsing = false;
                        break;
                    default:
                        parsing = false;
                        Console.WriteLine("Unhandled case: {0}", i);
                        break;
                }
            }

            byte[] buffer = new byte[len];
            Array.Copy(IntPtrData, index, buffer, 0, len);
            return Export.ArrayToCSV(buffer, Config.IntPtrCast, sizeof(int));
        }

        private static bool CheckFlag(int flag, int to)
        {
            return flag == to ? true : false;
        }

        // todo: fix me
        private void _ParseMyshTables(byte[] data, ref int offset)
        {
            byte[] endtoken = new byte[] { 0x64, 0x6E, 0x65, 0x68 };
            int check = 0;

            List<byte> buffer = new List<byte>();

            while (check < 4)
            {
                if (data[offset] == endtoken[check])
                {
                    check++;
                }
                else
                {
                    check = 0;
                }
                buffer.Add(data[offset++]);
            }

            offset = offset - 4;
            buffer.RemoveRange(buffer.Count - 4, 4);
            MyshBytes = buffer.ToArray();
            
            //int totalAttributeCount = 0;
            //int attributeCount = 0;
            //int blockCount = 0;
            //int flagCount = 0;
            //while (offset < data.Length)
            //{
            //    ////////////// temp fix /////////////////
            //    //int f = BitConverter.ToInt32(data, offset);
            //    //if (CheckFlag(f, 0x68657863))
            //    //{
            //    //    //Debug.Write("mysh flagCount = " + flagCount + "\n");
            //    //    break;
            //    //}
            //    ////if (CheckFlag(f, 0x6873796D))
            //    ////{
            //    ////    flagCount++;
            //    ////}
            //    //offset++;
            //    //continue;
            //    ////////////// temp fix /////////////////


            //                    int flag = FileTools.ByteArrayToInt32(data, ref offset);
            //                    if (!CheckFlag(flag, 0x6873796D))
            //                    {
            //                        offset -= 4;
            //                        break;
            //                    }

            //                    int unknown1 = FileTools.ByteArrayToInt32(data, ref offset);
            //                    if (unknown1 == 0x00)
            //                    {
            //                        break;
            //                    }

            //                    int byteCount = FileTools.ByteArrayToInt32(data, ref offset);
            //                    // this is really lazy and I couldn't be bothered trying to figure out how to get a *non-zero terminated* string out of a byte array
            //                    byte[] temp = new byte[byteCount + 1];
            //                    Buffer.BlockCopy(data, offset, temp, 0, byteCount);
            //                    String str = FileTools.ByteArrayToStringASCII(temp, 0);
            //                    offset += byteCount;

            //                    int unknown2 = FileTools.ByteArrayToInt32(data, ref offset);
            //                    int type = FileTools.ByteArrayToInt32(data, ref offset); // 0x39 = single, 0x41 = has properties, 0x3C = property
            //                    int unknown3 = FileTools.ByteArrayToInt32(data, ref offset);  // usually 0x05
            //                    int unknown4 = FileTools.ByteArrayToInt32(data, ref offset);  // always 0x00?
            //                    int unknown5 = FileTools.ByteArrayToInt32(data, ref offset);  // usually 0x04
            //                    int unknown6 = FileTools.ByteArrayToInt32(data, ref offset);  // always 0x00?

            //                    if (type == 0x39)
            //                    {
            //                        continue;
            //                    }

            //                    int id = FileTools.ByteArrayToInt32(data, ref offset);
            //                    attributeCount = FileTools.ByteArrayToInt32(data, ref offset);
            //                    totalAttributeCount = attributeCount;

            //                    if (type == 0x41)
            //                    {
            //                        int attributeCountAgain = FileTools.ByteArrayToInt32(data, ref offset); // ??
            //                    }
            //                    else if (type == 0x3C)
            //                    {
            //                        if (str == "dam")
            //                        {
            //                            blockCount += 2;
            //                        }
            //                        else if (str == "dur")
            //                        {
            //                            blockCount++;
            //                        }

            //                        const int blockSize = 4 * sizeof(Int32);
            //                        if (attributeCount == 0)
            //                        {
            //                            offset += blockCount * blockSize;
            //                            continue;
            //                        }

            //                        attributeCount--;
            //                    }
            //                    else
            //                    {
            //                        throw new NotImplementedException("type not implemented!\ntype = " + type);
            //                    }

            //                    int endInt = FileTools.ByteArrayToInt32(data, ref offset);
            //                    if (endInt != 0x00)
            //                    {
            //                        int breakpoint = 1;
            //                    }
                
            //}
        }

        public override byte[] GenerateFile(DataTable dataTable)
        {
            byte[] buffer = new byte[1024];
            int byteOffset = 0;
            byte[] intBytes = null;
            int intByteCount = 0;

            // File Header
            FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTokens.StartOfBlock);
            FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileExcelHeader);

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
            int col = 1;
            foreach (DataRow dr in dataTable.Rows)
            {
                object table = Activator.CreateInstance(tableType);
                col = 1;
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
                                else if (fieldInfo.FieldType == typeof(Int16))
                                {
                                    fieldInfo.SetValue(table, new Int16());
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
                                byte[] array = Export.CSVtoArray(s, Config.IntPtrCast, sizeof(int));
                                FileTools.WriteToBuffer(ref intBytes, ref intByteCount, array);
                            }
                        }
                    }
                    else
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
                            else if (fieldInfo.FieldType == typeof(UInt32))
                            {
                                fieldInfo.SetValue(table, 0);
                            }
                            else if (fieldInfo.FieldType == typeof(Int32))
                            {
                                fieldInfo.SetValue(table, 0);
                            }
                            else if (fieldInfo.FieldType == typeof(Single))
                            {
                                const Single f = 0;
                                fieldInfo.SetValue(table, f);
                            }
                            else if (fieldInfo.FieldType == typeof(String))
                            {
                                fieldInfo.SetValue(table, String.Empty);
                            }
                            else if (fieldInfo.FieldType == typeof(Int32[]))
                            {
                                object[] attributes = fieldInfo.GetCustomAttributes(typeof(MarshalAsAttribute), false);
                                MarshalAsAttribute marshal = (MarshalAsAttribute)attributes[0];
                                int sizeConst = marshal.SizeConst;
                                int[] i32 = new int[sizeConst];
                                fieldInfo.SetValue(table, i32);
                            }
                            else if (fieldInfo.FieldType == typeof(Int16))
                            {
                                const Int16 i16 = 0;
                                fieldInfo.SetValue(table, i16);
                            }
                            else
                            {
                                ExcelOutputAttribute excelAttribute = GetExcelOutputAttribute(fieldInfo);
                                if (excelAttribute.IsBitmask)
                                {
                                    fieldInfo.SetValue(table, new UInt32());
                                }
                                else
                                {
                                    Debug.Fail("Unhandled default type");
                                }
                            }
                        }
                        else
                        {
                            fieldInfo.SetValue(table, o);
                        }
                    }
                    col++;
                }


                if (AiData != null) // items, missiles, monsters, objects, players
                {
                    String s = dr[col++] as String;
                    byte[] array = null;

                    if (!String.IsNullOrEmpty(s))
                    {
                        array = Export.CSVtoArray(s, "hex", sizeof(byte));
                    }
                    else
                    {
                        string empty = "0A,00,02,06,00,64,DA,00,11,09,00,00,00,19,37,40,04,01,00,00,40,E6,0D,10,51,00,00,00,00";
                        array = Export.CSVtoArray(empty, "hex", sizeof(byte));
                    }

                    if (AiData.Count <= row)
                    {
                        AiData.Add(array);
                    }
                    else
                    {
                        AiData[row] = array;
                    }
                }

                if (FinalData != null)
                {
                    String s = dr[col++] as String;
                    byte[] array = null;

                    if (!String.IsNullOrEmpty(s))
                    {
                        array = Export.CSVtoArray(s, "hex", sizeof(int));
                    }
                    else
                    {
                        throw new Exception("Final Data cell can not be null");
                    }

                    if (FinalData.Count <= row)
                    {
                        FinalData.Add(array);
                    }
                    else
                    {
                        FinalData[row] = array;
                    }
                }

                FileTools.WriteToBuffer(ref buffer, ref byteOffset, table);
                row++;
            }
            #endregion


            // Strings block
            if (stringBytes != null && stringsByteCount > 0)
            {
                FileTools.WriteToBuffer(ref buffer, stringsByteOffset, stringsByteCount);
                FileTools.WriteToBuffer(ref buffer, stringsByteOffset + sizeof(Int32), stringBytes, stringsByteCount, true);
                byteOffset += stringsByteCount;
            }




            // Primary index
            FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTokens.StartOfBlock);
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                FileTools.WriteToBuffer(ref buffer, ref byteOffset, i);

                // Ai data - item types only
                if ((uint)FileExcelHeader.StructureId == idItems ||
                    (uint)FileExcelHeader.StructureId == idItemsTc)
                {
                    FileTools.WriteToBuffer(ref buffer, ref byteOffset, AiData[i].Length);
                    FileTools.WriteToBuffer(ref buffer, ref byteOffset, AiData[i]);
                }
            }


            // Secondary string block
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


            // Generate sort indices
            foreach (DataColumn dc in dataTable.Columns)
            {
                if (!(dc.ExtendedProperties.Contains(ColumnTypeKeys.SortAscendingID)) &&
                    !(dc.ExtendedProperties.Contains(ColumnTypeKeys.SortDistinctID)) && 
                    !(dc.ExtendedProperties.Contains(ColumnTypeKeys.SortPostOrderID)))
                {
                    continue;
                }
                
                int sortId;
                if (dc.ExtendedProperties.ContainsKey(ColumnTypeKeys.SortAscendingID))
                {
                    sortId = (int)dc.ExtendedProperties[ColumnTypeKeys.SortAscendingID] - 1; // 1,2,3,4
                }
                else if (dc.ExtendedProperties.ContainsKey(ColumnTypeKeys.SortDistinctID))
                {
                    sortId = (int)dc.ExtendedProperties[ColumnTypeKeys.SortDistinctID] - 1; // 1,2,3,4
                }
                else
                {
                    sortId = (int)dc.ExtendedProperties[ColumnTypeKeys.SortPostOrderID] - 1; // 1,2,3,4
                }


                string sortColumn = dc.ColumnName;
                string filter = string.Empty;
                int indexCol = 0;
                
                DataTable sortTable = dataTable;
                DataView dataView = sortTable.DefaultView;
                dataView.Sort = sortColumn;
                int defaultRow = (int)dataView[0][0];

                if (dc.DataType != typeof(string))
                {
                    filter = sortColumn + " <> -1";
                    if (dataTable.Columns.Contains("code"))
                    {
                        filter = "(" + filter + " AND code <> 0)";
                    }

                    // only used on wardrobe tables
                    if (dc.ExtendedProperties.Contains(ColumnTypeKeys.ExcludeZero))
                    {
                        filter = "(" + filter + " AND " + sortColumn + "<> 0)";
                    }
                }
                else
                {
                    filter = sortColumn + " <> ''";

                    // precedence hack 
                    foreach (DataRow dr in sortTable.Rows)
                    {
                        dr[sortColumn] = ((string)dr[sortColumn]).Replace("-", "98");
                        dr[sortColumn] = ((string)dr[sortColumn]).Replace("_", "99");
                    }
                }

                if (dc.ExtendedProperties.Contains(ColumnTypeKeys.RequiresDefault))
                {
                    filter += " OR Index = " + defaultRow;
                }

                if (dc.ExtendedProperties.Contains(ColumnTypeKeys.SortColumnTwo))
                {
                    sortColumn += ", " + dc.ExtendedProperties[ColumnTypeKeys.SortColumnTwo];
                }

                dataView.RowFilter = filter;
                dataView.Sort = sortColumn;


                if (dc.ExtendedProperties.Contains(ColumnTypeKeys.SortDistinctID))
                {
                    sortTable = SelectDistinct(sortTable, new string[] { sortColumn });
                    dataView = sortTable.DefaultView;
                }


                // Do post tree transversal using a binary tree structure
                if (dc.ExtendedProperties.Contains(ColumnTypeKeys.SortPostOrderID))
                {
                    BinarySearchTree<int> binaryTree = new BinarySearchTree<int>();
                    foreach (DataRowView dr in dataView)
                    {
                        binaryTree.Add((int)dr[0]);
                    }

                    int[] postOrderArray = binaryTree.Postorder.ToArray();

                    _sortIndicies[sortId] = new int[postOrderArray.Length];
                    for (int i = 0; i < postOrderArray.Length; i++)
                    {
                        _sortIndicies[sortId][i] = postOrderArray[i];
                    }
                
                }

                // Ascending and distinct sorts using the dataiew
                if (dc.ExtendedProperties.Contains(ColumnTypeKeys.SortAscendingID) ||
                    dc.ExtendedProperties.Contains(ColumnTypeKeys.SortDistinctID))
                {
                    _sortIndicies[sortId] = new int[dataView.Count];
                    for (int i = 0; i < dataView.Count; i++)
                    {
                        _sortIndicies[sortId][i] = (int)dataView[i][indexCol];
                    }
                }

                // undo precedence hack 
                if (dc.DataType == typeof(string))
                {
                    foreach (DataRow dr in sortTable.Rows)
                    {
                        dr[sortColumn] = ((string)dr[sortColumn]).Replace("98", "-");
                        dr[sortColumn] = ((string)dr[sortColumn]).Replace("99", "_");
                    }
                }

            }


            // Sort indices
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



            // Rcsh, Tysh, Mysh, Dneh
            if (_rcshValue != 0)
            {
                FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTokens.StartOfBlock);
                FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTokens.TokenRcsh);
                FileTools.WriteToBuffer(ref buffer, ref byteOffset, _rcshValue);
                FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTokens.TokenTysh);
                FileTools.WriteToBuffer(ref buffer, ref byteOffset, _tyshValue);
                if (MyshBytes != null)
                {
                    FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTokens.TokenMysh);
                    FileTools.WriteToBuffer(ref buffer, ref byteOffset, MyshBytes);
                }
                FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTokens.TokenDneh);
                FileTools.WriteToBuffer(ref buffer, ref byteOffset, _dnehValue);
            }


            // todo: fix lazy bug fix below.
            // when intptr types exists but none are defined, a 1 byte array is created
            // i doubt its a problem, but for the sake of completeness and debugging
            // this fix is included
            if (intByteCount == 1) intByteCount = 0;



            //if (FileExcelHeader.StructureId != idUnitTypes &&
            //    FileExcelHeader.StructureId != idUintTypesTc)
            {
                // IntPtr data block
                // UnitTypes ignores this section
                FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTokens.StartOfBlock);
                FileTools.WriteToBuffer(ref buffer, ref byteOffset, intByteCount);
                if (intByteCount > 0)
                {
                    byte[] intByteBuffer = new byte[intByteCount];
                    Buffer.BlockCopy(intBytes, 0, intByteBuffer, 0, intByteCount);
                    FileTools.WriteToBuffer(ref buffer, ref byteOffset, intByteBuffer);
                }
            }


            // Final data block
            int blockCount = FinalData != null? dataTable.Rows.Count : 0;
            int blockSize = FinalData != null ? FinalData[0].Length : 0;
            int bshiftCount = blockSize > 0 ? (blockSize >> 2) : 0;

            FileTools.WriteToBuffer(ref buffer, ref byteOffset, FileTokens.StartOfBlock);
            FileTools.WriteToBuffer(ref buffer, ref byteOffset, bshiftCount);
            FileTools.WriteToBuffer(ref buffer, ref byteOffset, blockCount);
            if (bshiftCount > 0)
            {
                byte[] fbBuffer = new byte[blockCount * blockSize];
                for (int i = 0; i < blockCount; i++)
                {
                    Buffer.BlockCopy(FinalData[i], 0, fbBuffer, i * blockSize, blockSize);
                }
                FileTools.WriteToBuffer(ref buffer, ref byteOffset, fbBuffer);
            }


            // Return
            byte[] returnBuffer = new byte[byteOffset];
            Buffer.BlockCopy(buffer, 0, returnBuffer, 0, byteOffset);

            return returnBuffer;
        }

        public static DataTable ConvertToSinglePlayerVersion(DataTable spDataTable, DataTable tcDataTable)
        {
            spDataTable.Rows.Clear();

            foreach (DataRow tcRow in tcDataTable.Rows)
            {
                DataRow convertedRow = spDataTable.NewRow();
                foreach (DataColumn column in spDataTable.Columns)
                {
                    string columnName = column.ColumnName;
                    if (!tcDataTable.Columns.Contains(columnName))
                    {
                        continue;
                    }
                    if (column.DataType == tcDataTable.Columns[columnName].DataType)
                    {
                        convertedRow[columnName] = tcRow[columnName];
                        continue;
                    }
                    if (column.DataType.BaseType == typeof(Enum))
                    {
                        Type spBitMask = column.DataType;
                        Type tcBitMask = tcDataTable.Columns[columnName].DataType;
                        uint currentMask = (uint)tcRow[columnName];
                        uint convertedMask = 0;

                        for (int i = 0; i < 32; i++)
                        {
                            uint testBit = (uint)1 << i;
                            if ((currentMask & testBit) == 0)
                            {
                                continue;
                            }
                            string bitString = Enum.GetName(tcBitMask, testBit);
                            if (bitString == null)
                            {
                                continue;
                            }
                            if (Enum.IsDefined(spBitMask, bitString))
                            {
                                convertedMask += (uint)Enum.Parse(spBitMask, bitString);
                            }
                        }
                        convertedRow[columnName] = convertedMask;
                        continue;
                    }
                }

                spDataTable.Rows.Add(convertedRow);
            }

            return spDataTable;
        }

        public void DumpExtraIndiceData()
        {
            using (TextWriter stream = new StreamWriter(@"extradump.txt"))
            {
                int i = 0;
                foreach (byte[] array in AiData)
                {
                    stream.WriteLine(Export.ArrayToCSV(array, "hex", sizeof(byte)));
                    i++;
                }
            }
        }

        public static ExcelOutputAttribute GetExcelOutputAttribute(FieldInfo fieldInfo)
        {
            foreach (Attribute attribute in fieldInfo.GetCustomAttributes(typeof(ExcelOutputAttribute), true))
            {
                return attribute as ExcelOutputAttribute;
            }
            return null;
        }

        private static DataTable SelectDistinct(DataTable SourceTable, params string[] FieldNames)
        {
            object[] lastValues;
            DataTable newTable;
            DataRow[] orderedRows;

            if (FieldNames == null || FieldNames.Length == 0)
                throw new ArgumentNullException("FieldNames");

            lastValues = new object[FieldNames.Length];
            newTable = new DataTable();

            foreach (string fieldName in FieldNames)
                newTable.Columns.Add(fieldName, SourceTable.Columns[fieldName].DataType);

            orderedRows = SourceTable.Select("", string.Join(", ", FieldNames));

            foreach (DataRow row in orderedRows)
            {
                if ((int)row[FieldNames[0]] == -1) //lazy. function cant be reused
                {
                    continue; // ignore values -1
                }

                if (!fieldValuesAreEqual(lastValues, row, FieldNames))
                {
                    DataRow newRow = createRowClone(row, newTable.NewRow(), FieldNames);
                    newTable.Rows.Add(newRow);
                    setLastValues(lastValues, row, FieldNames);
                    newRow[0] = row[0];//modified to store index, not distinct value
                }
            }

            return newTable;
        }

        private static bool fieldValuesAreEqual(object[] lastValues, DataRow currentRow, string[] fieldNames)
        {
            bool areEqual = true;

            for (int i = 0; i < fieldNames.Length; i++)
            {
                if (lastValues[i] == null || !lastValues[i].Equals(currentRow[fieldNames[i]]))
                {
                    areEqual = false;
                    break;
                }
            }

            return areEqual;
        }

        private static DataRow createRowClone(DataRow sourceRow, DataRow newRow, string[] fieldNames)
        {
            foreach (string field in fieldNames)
                newRow[field] = sourceRow[field];

            return newRow;
        }

        private static void setLastValues(object[] lastValues, DataRow sourceRow, string[] fieldNames)
        {
            for (int i = 0; i < fieldNames.Length; i++)
                lastValues[i] = sourceRow[fieldNames[i]];
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