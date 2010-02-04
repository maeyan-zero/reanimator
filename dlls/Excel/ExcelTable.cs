using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Reflection;
using System.Collections;

namespace Reanimator.Excel
{
    public class BadHeaderFlag : Exception
    {
        public BadHeaderFlag() : base() { }
        public BadHeaderFlag(string message) : base(message) { }
    }

    public abstract class ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        protected struct ExcelHeader
        {
            public Int32 structureId;                       // This is the id used to determine what structure to use to read the table block.
            public Int32 unknown32_1;                       // This is how the game reads this in...
            public Int32 unknown32_2;                       // What they do I don't know, lol.
            public Int16 unknown16_1;
            public Int16 unknown16_2;
            public Int16 unknown16_3;
            public Int16 unknown16_4;
            public Int16 unknown16_5;
            public Int16 unknown16_6;
        };

        protected byte[] excelData;
        protected int offset;
        protected ExcelHeader excelHeader;
        public int StructureId
        {
            get { return excelHeader.structureId; }
        }

        protected byte[] stringsBytes;
        public Hashtable Strings { get; private set; }

        int tableCount;
        protected List<object> tables;
        protected DataGridView dataGridView;

        protected int[] tableIndicies;
        protected List<byte[]> extraIndexData;
        public int[] TableIndicies { get { return tableIndicies; } }

        protected int[][] unknownIndicies;
        public int[] Unknowns1 { get { return unknownIndicies[0]; } }
        public int[] Unknowns2 { get { return unknownIndicies[1]; } }
        public int[] Unknowns3 { get { return unknownIndicies[2]; } }
        public int[] Unknowns4 { get { return unknownIndicies[3]; } }

        protected int rcshValue;
        protected int tyshValue;
        // mysh
        protected int dnehValue;

        public byte[] DataBlock { get; set; }
        public byte[] FinalBytes { get; set; }

        public ExcelTable(byte[] data)
        {
            try
            {
                /* A typical Excel Table file:
                 * 
                 ***** Main Header ***** (28) bytes
                 * flag                                 Int32                                   // Must be 0x68657863 ('cxeh').
                 * structureId                          Int32                                   // This is the id used to determine what structure to use to read the table block.
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
                 * flag                                 Int32                                   // Must be 0x68657863 ('cxeh').
                 * byteCount                            Int32                                   // The number of bytes of the following string block.
                 * bytes                                *****                                   // byteCount in size.
                 * 
                 ***** Tables Block ***** (8 + tableSize * tableCount) bytes
                 * flag                                 Int32                                   // Must be 0x68657863 ('cxeh').
                 * tableCount                           Int32                                   // Count of tables.
                 * {                                                                            // Each table has the same first header 16 byte chunk style.
                 *      unknown                         Int32                                   // Seen as 0x01, 0x02 or 0x03.
                 *      unknown                         Int32                                   // Seen as 0x3C, 0x3E or 0x3F.
                 *      unknown                         Int32                                   // Appears to always be 0xFFFF0000.     // These two are possibly
                 *      unknown                         Int32                                   // Appears to always be 0xFFFF0000.     // 4x Int16
                 *      *****                           *****                                   // Table type dependent; see applicable table structure class file.
                 * }
                 * 
                 ***** Primary Index Block ***** (4 + tableCount * 4) bytes
                 * flag                                 Int32                                   // Must be 0x68657863 ('cxeh').
                 * tableCount                                                                   // This is tableCount from above Table Block (not actually read in just
                 * {                                                                            //                                            here for struct definition)
                 *      index                           Int32                                   // Always seen as 0 -> tableCount - 1, unknown
                 * }                                                                            // if it can be different (do we even care?).
                 * 
                 ***** Secondary Index Block 1 ***** (8 + indexCount * 4) bytes                 // Unsure what these do.
                 * flag                                 Int32                                   // Must be 0x68657863 ('cxeh'). 
                 * indexCount                           Int32
                 * {
                 *      unknownValue                    Int32
                 * }
                 * 
                 ***** Secondary Index Block 2 ***** (8 + indexCount * 4) bytes                 // Unsure what these do.
                 * flag                                 Int32                                   // Must be 0x68657863 ('cxeh'). 
                 * indexCount                           Int32
                 * {
                 *      unknownValue                    Int32
                 * }
                 * 
                 ***** Secondary Index Block 3 ***** (8 + indexCount * 4) bytes                 // Unsure what these do.
                 * flag                                 Int32                                   // Must be 0x68657863 ('cxeh'). 
                 * indexCount                           Int32
                 * {
                 *      unknownValue                    Int32
                 * }
                 * 
                 ***** Secondary Index Block 4 ***** (8 + indexCount * 4) bytes                 // Unsure what these do.
                 * flag                                 Int32                                   // Must be 0x68657863 ('cxeh'). 
                 * indexCount                           Int32
                 * {
                 *      unknownValue                    Int32
                 * }
                 * 
                 ***** Unknown Block *****
                 * flag                                 Int32                                   // Must be 0x68657863 ('cxeh'). 
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
                 * flag                                 Int32                                   // Must be 0x68657863 ('cxeh').
                 * byteCount                            Int32                                   // Bytes of following block.
                 * dataBlock                            *****                                   // Count of byteCount.
                 * 
                 ***** Unknown Block *****
                 * // TODO
                 */


                excelData = data;
                tables = new List<object>();
                Strings = new Hashtable();
                unknownIndicies = new int[4][];
                offset = 0;


                // main header
                int flag = FileTools.ByteArrayTo<Int32>(data, ref offset);
                CheckFlag(flag);

                excelHeader = (ExcelHeader)FileTools.ByteArrayToStructure(data, typeof(ExcelHeader), offset);
                offset += Marshal.SizeOf(typeof(ExcelHeader));


                // strings block
                flag = FileTools.ByteArrayTo<Int32>(data, ref offset);
                CheckFlag(flag);

                int stringsBytesCount = FileTools.ByteArrayTo<Int32>(data, ref offset);
                if (stringsBytesCount != 0)
                {
                    this.stringsBytes = new byte[stringsBytesCount];
                    Buffer.BlockCopy(excelData, offset, stringsBytes, 0, stringsBytesCount);
                    
                    // put into a hash table for easier use later, with the stringsBytes[offset] -> offset as the key
                    // sometimes C# makes things hard  -  C++ char* ftw. x.x
                    for (int i = offset; i < offset + stringsBytesCount; i++)
                    {
                        String s = FileTools.ByteArrayToStringAnsi(excelData, i);
                        Strings.Add(i-offset, s);

                        i += s.Length;
                    }

                    offset += stringsBytesCount;
                }


                // tables block
                flag = FileTools.ByteArrayTo<Int32>(data, ref offset);
                CheckFlag(flag);

                this.tableCount = FileTools.ByteArrayTo<Int32>(data, ref offset);
                ParseTables(data);


                // index block
                flag = FileTools.ByteArrayTo<Int32>(data, ref offset);
                CheckFlag(flag);

                if ((uint)excelHeader.structureId == 0x887988C4) // items, missiles, monsters, objects, players
                {
                    tableIndicies = new int[Count];
                    extraIndexData = new List<byte[]>();

                    for (int i = 0; i < Count; i++)
                    {
                        tableIndicies[i] = FileTools.ByteArrayTo<Int32>(data, ref offset);
                        int size = FileTools.ByteArrayTo<Int32>(data, ref offset);

                        byte[] extra = new byte[size];
                        Buffer.BlockCopy(data, offset, extra, 0, size);
                        offset += size;
                        extraIndexData.Add(extra);
                    }
                }
                else
                {
                    tableIndicies = FileTools.ByteArrayToInt32Array(data, offset, Count);
                    offset += Count * sizeof(Int32);
                }


                // secondary index blocks
                for ( int i = 0; i < 4; i++)
                {
                    flag = FileTools.ByteArrayTo<Int32>(data, ref offset);
                    CheckFlag(flag);

                    int count = FileTools.ByteArrayTo<Int32>(data, ref offset);
                    this.unknownIndicies[i] = FileTools.ByteArrayToInt32Array(data, offset, count);
                    offset += count * sizeof(Int32);
                }
 

                // unknown block
                flag = FileTools.ByteArrayTo<Int32>(data, ref offset);
                CheckFlag(flag);

                flag = FileTools.ByteArrayTo<Int32>(data, ref offset);
                if (flag != 0x00)
                {
                    while (true)
                    {

                        if (CheckFlag(flag, 0x68736372)) // 'rcsh'
                        {
                            this.rcshValue = FileTools.ByteArrayTo<Int32>(data, ref offset);
                            if (this.rcshValue != 0x04)
                            {
                                throw new Exception("this.rcshValue = FileTools.ByteArrayTo<Int32>(data, ref offset);\nif (this.rcshValue != 0x04)");
                            }
                        }
                        else if (CheckFlag(flag, 0x68737974)) // 'tysh'
                        {
                            this.tyshValue = FileTools.ByteArrayTo<Int32>(data, ref offset);
                            if (this.tyshValue != 0x02)
                            {
                                throw new Exception("this.tyshValue = FileTools.ByteArrayTo<Int32>(data, ref offset);\nif (this.tyshValue != 0x02)");
                            }
                        }
                        else if (CheckFlag(flag, 0x6873796D)) // 'mysh'
                        {
                            offset -= 4;
                            ParseMYSHTables(data, ref offset);
                        }
                        else if (CheckFlag(flag, 0x68656E64)) // 'dneh'
                        {
                            this.dnehValue = FileTools.ByteArrayTo<Int32>(data, ref offset);
                            if (this.dnehValue != 0x00)
                            {
                                throw new Exception("this.dnehValue = FileTools.ByteArrayTo<Int32>(data, ref offset);\nif (this.dnehValue != 0x02)");
                            }
                        }
                        else // 'cxeh'  -  starting next block
                        {
                            CheckFlag(flag);
                            offset -= 4;
                            break;
                        }

                        flag = FileTools.ByteArrayTo<Int32>(data, ref offset);
                    }
                }


                // data block
                flag = FileTools.ByteArrayTo<Int32>(data, ref offset);
                CheckFlag(flag);

                int byteCount = FileTools.ByteArrayTo<Int32>(data, ref offset);
                if (byteCount != 0)
                {
                    DataBlock = new byte[byteCount];
                    Buffer.BlockCopy(data, offset, DataBlock, 0, byteCount);
                    offset += byteCount;
                }


                // does it have a final flag chunk?
                flag = FileTools.ByteArrayTo<Int32>(data, ref offset);
                if (flag != 0)
                {
                    CheckFlag(flag);

                    int unknown = FileTools.ByteArrayTo<Int32>(data, ref offset);
                    int blockCount = FileTools.ByteArrayTo<Int32>(data, ref offset);

                    FinalBytes = new byte[blockCount * 128];
                    Buffer.BlockCopy(data, offset, FinalBytes, 0, FinalBytes.Length);
                    offset += FinalBytes.Length;
                }

        /*[StructLayout(LayoutKind.Sequential, Pack = 1)]
        protected struct FinalHeader
        {
            public int flag;
            public int unknown; // was 0x20 ->     0x20 << 2 = 0x80 = 128 byte block count      // untested
            public int blockCount;
        }*/

                if (offset != data.Length)
                {
                    throw new BadHeaderFlag("offset != data.Length");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int Count
        {
            get
            {
                return tableCount;
            }
        }

        private void CheckFlag(int flag)
        {
            if (!CheckFlag(flag, 0x68657863))
            {
                throw new BadHeaderFlag("Unexpected header flag!");
            }
        }

        private bool CheckFlag(int flag, int to)
        {
            return flag == to ? true : false;
        }

        private void ParseMYSHTables(byte[] data, ref int offset)
        {
            int totalAttributeCount = 0;
            int attributeCount = 0;
            int blockCount = 0;

            while (offset < data.Length)
            {
                ////////////// temp fix /////////////////
                int f = BitConverter.ToInt32(data, offset);
                if (CheckFlag(f, 0x68657863))
                {
                    break;
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

        public void ReadTables<T>(byte[] data, ref int offset, int count)
        {
            for (int i = 0; i < count; i++)
            {
                T table = (T)FileTools.ByteArrayToStructure(data, typeof(T), offset);
                offset += Marshal.SizeOf(typeof(T));

                tables.Add(table);
            }
        }

        public String StringId { get; set; }
        public override string ToString()
        {
            return StringId;
        }
    }
}