using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class BadHeaderFlag : Exception
    {
        public BadHeaderFlag() : base() {}

        public BadHeaderFlag(string message) : base(message) {}
    }

    public abstract class ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        protected struct ExcelHeader
        {
            public Int32 flag;
            public Int32 fileId;                            // or is this a CRC? Or something else weird like that?
            public Int32 unknown32_1;                       // this is how the game reads this in...
            public Int32 unknown32_2;                       // what they do I don't know, lol.
            public Int16 unknown16_1;
            public Int16 unknown16_2;
            public Int16 unknown16_3;
            public Int16 unknown16_4;
            public Int16 unknown16_5;
            public Int16 unknown16_6;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        protected struct StringHeader
        {
            public Int32 flag;
            public Int32 stringBlockSize;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        protected struct TableHeader
        {
            public Int32 flag;
            public Int32 count;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        protected struct TableIndex
        {
            public int flag;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        protected struct UnknownIndex
        {
            public int flag;
            public int count;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        protected struct UnknownHeader
        {
            public int flag;
            public int unknownFlag1;
            public int unknownCount1;
            public int unknownFlag2;
            public int unknownCount2;
            public int unknownFlag3;
            public int unknownCount3;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        protected struct ByteHeader
        {
            public int flag;
            public int count;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        protected struct FinalHeader
        {
            public int flag;
            public int unknown; // was 0x20 ->     0x20 << 2 = 0x80 = 128 byte block count      // untested
            public int blockCount;
        }

        protected byte[] excelData;
        protected int offset;
        protected ExcelHeader excelHeader;
        protected StringHeader stringHeader;
        protected byte[] strings;
        protected TableHeader tableHeader;
        protected TableIndex tableIndex;
        protected int[] tableIndicies;
        protected UnknownIndex unknownIndex1;
        protected int[] unknowns1;
        protected UnknownIndex unknownIndex2;
        protected int[] unknowns2;
        protected UnknownIndex untestedHeader1;
        protected UnknownIndex untestedHeader2;
        protected UnknownHeader unknownHeader;
        protected ByteHeader byteHeader;
        protected byte[] bytes;
        protected FinalHeader finalHeader;
        protected byte[] finalBytes;

        public ExcelTable(byte[] data)
        {
            excelData = data;
            offset = 0;

            uint crc = Crc32.Compute(data);

            try
            {
                // read in main header
                excelHeader = (ExcelHeader)FileTools.ByteArrayToStructure(data, typeof(ExcelHeader), offset);
                offset += Marshal.SizeOf(typeof(ExcelHeader));
                CheckFlag(excelHeader.flag);


                // read in strings (does not always exist)
                stringHeader = (StringHeader)FileTools.ByteArrayToStructure(excelData, typeof(StringHeader), offset);
                offset += Marshal.SizeOf(typeof(StringHeader));
                CheckFlag(stringHeader.flag);
                if (stringHeader.stringBlockSize != 0)
                {
                    strings = new byte[stringHeader.stringBlockSize];
                    Buffer.BlockCopy(excelData, offset, strings, 0, strings.Length);
                    offset += stringHeader.stringBlockSize;
                }


                // read in tables
                tableHeader = (TableHeader)FileTools.ByteArrayToStructure(data, typeof(TableHeader), offset);
                offset += Marshal.SizeOf(typeof(TableHeader));
                CheckFlag(tableHeader.flag);
                ParseTables(data);


                // read in table index
                tableIndex = (TableIndex)FileTools.ByteArrayToStructure(data, typeof(TableIndex), offset);
                offset += Marshal.SizeOf(typeof(TableIndex));
                CheckFlag(tableIndex.flag);
                tableIndicies = FileTools.ByteArrayToInt32Array(data, offset, Count);
                offset += Count * sizeof(Int32);

                
                // these 4 index chunks are read in in a single loop, each time with the "flag", followed by the "count"... Something like that
                // read in some unknown index
                unknownIndex1 = (UnknownIndex)FileTools.ByteArrayToStructure(data, typeof(UnknownIndex), offset);
                offset += Marshal.SizeOf(typeof(UnknownIndex));
                CheckFlag(unknownIndex1.flag);
                if (unknownIndex1.count != 0)
                {
                    unknowns1 = FileTools.ByteArrayToInt32Array(data, offset, unknownIndex1.count);
                    offset += unknownIndex1.count * sizeof(Int32);
                }


                // read in another unknown index
                unknownIndex2 = (UnknownIndex)FileTools.ByteArrayToStructure(data, typeof(UnknownIndex), offset);
                offset += Marshal.SizeOf(typeof(UnknownIndex));
                CheckFlag(unknownIndex2.flag);
                if (unknownIndex2.count != 0)
                {
                    unknowns2 = FileTools.ByteArrayToInt32Array(data, offset, unknownIndex2.count);
                    offset += unknownIndex2.count * sizeof(Int32);
                }


                // untested header1 - this appears to be read in as above
                untestedHeader1 = (UnknownIndex)FileTools.ByteArrayToStructure(data, typeof(UnknownIndex), offset);
                offset += Marshal.SizeOf(typeof(UnknownIndex));
                CheckFlag(untestedHeader1.flag);
                if (untestedHeader1.count != 0)
                {
                    throw new NotImplementedException("untestedHeader1.count != 0");
                }


                // untested header2 - this appears to be read in as above
                untestedHeader2 = (UnknownIndex)FileTools.ByteArrayToStructure(data, typeof(UnknownIndex), offset);
                offset += Marshal.SizeOf(typeof(UnknownIndex));
                CheckFlag(untestedHeader2.flag);
                if (untestedHeader2.count != 0)
                {
                    throw new NotImplementedException("untestedHeader2.count != 0");
                }


                // untested header3
                int length = sizeof(Int32) * 2;
                unknownHeader = (UnknownHeader)FileTools.ByteArrayToStructure(data, typeof(UnknownHeader), offset, length);
                CheckFlag(unknownHeader.flag);
                if (unknownHeader.unknownFlag1 == 0)
                {
                    offset += length;
                }
                else
                {
                    unknownHeader = (UnknownHeader)FileTools.ByteArrayToStructure(data, typeof(UnknownHeader), offset);
                    offset += Marshal.SizeOf(typeof(UnknownHeader));
                }


                // untested header4
                byteHeader = (ByteHeader)FileTools.ByteArrayToStructure(data, typeof(ByteHeader), offset);
                offset += Marshal.SizeOf(typeof(ByteHeader));
                CheckFlag(byteHeader.flag);
                if (byteHeader.count != 0)
                {
                    bytes = new byte[byteHeader.count];
                    Buffer.BlockCopy(data, offset, bytes, 0, byteHeader.count);
                    offset += byteHeader.count;
                }


                // does it have a final flag chunk?
                finalHeader = (FinalHeader)FileTools.ByteArrayToStructure(data, typeof(FinalHeader), offset, sizeof(Int32));
                if (finalHeader.flag != 0)
                {
                    finalHeader = (FinalHeader)FileTools.ByteArrayToStructure(data, typeof(FinalHeader), offset);
                    offset += Marshal.SizeOf(typeof(FinalHeader));
                    CheckFlag(finalHeader.flag);
                    finalBytes = new byte[finalHeader.blockCount * 128];
                    Buffer.BlockCopy(data, offset, finalBytes, 0, finalBytes.Length);
                    offset += finalBytes.Length;
                }
                else
                {
                    offset += sizeof(Int32);
                }

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
                return tableHeader.count;
            }
        }

        private void CheckFlag(int flag)
        {
            if (flag != 0x68657863)      // 'cxeh'
            {
                throw new BadHeaderFlag("Unexpected header flag!");
            }
        }

        protected abstract void ParseTables(byte[] data);

        public static List<T> ReadTables<T>(byte[] data, ref int offset, int count)
        {
            List<T> tables = new List<T>();

            for (int i = 0; i < count; i++)
            {
                T table = (T)FileTools.ByteArrayToStructure(data, typeof(T), offset);
                offset += Marshal.SizeOf(typeof(T));

                tables.Add(table);
            }

            return tables;
        }
    }
}
