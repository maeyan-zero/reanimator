using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator
{
    public class Strings
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct StringsHeader
        {
            public Int32 header;
            public Int32 unknown;
            public Int32 count;
        }

        public struct StringBlock
        {
            public int BlockId { get; set; }
            public int Unknown1 { get; set; }
            public string StringId { get; set; }
            public int Unknown2 { get; set; }
            public string String { get; set; }
            public int UsageFlag { get; set; }
            public string Language { get; set; }
            public string Usage { get; set; }
            public string DefaultString { get; set; }
        }

        byte[] fileData;
        StringsHeader header;
        List<StringBlock> strings;

        public Strings(byte[] data)
        {
            fileData = data;
            int offset = 0;
            strings = new List<StringBlock>();

            try
            {
                header = (StringsHeader)FileTools.ByteArrayToStructure(fileData, typeof(StringsHeader), 0);
                offset += Marshal.SizeOf(header);

                List<StringBlock> stringBlocks = new List<StringBlock>();
                for (int i = 0; i < header.count; i++)
                {
                    StringBlock stringBlock = new StringBlock();

                    stringBlock.BlockId = FileTools.ByteArrayToInt32(fileData, offset);
                    offset += sizeof(Int32);

                    stringBlock.Unknown1 = FileTools.ByteArrayToInt32(fileData, offset);
                    offset += sizeof(Int32);

                    int count = FileTools.ByteArrayToInt32(fileData, offset);
                    offset += sizeof(Int32);
                    stringBlock.StringId = FileTools.ByteArrayToStringAnsi(fileData, offset);
                    offset += count + 1;

                    stringBlock.Unknown2 = FileTools.ByteArrayToInt32(fileData, offset);
                    offset += sizeof(Int32);

                    count = FileTools.ByteArrayToInt32(fileData, offset);
                    offset += sizeof(Int32);
                    stringBlock.String = FileTools.ByteArrayToStringUnicode(fileData, offset);
                    offset += count;

                    if (i >= 0x2eD && i % 20 == 0)
                    //if (offset >= 0x152A0)
                    {
                        int breakpoint = 1;
                    }

                    stringBlock.UsageFlag = FileTools.ByteArrayToInt32(fileData, offset);
                    offset += sizeof(Int32);

                    if (stringBlock.UsageFlag == 0x00) // 0x01 = Plural next, 0x02 = Singular next
                    {
                        count = FileTools.ByteArrayToInt32(fileData, offset);
                        offset += sizeof(Int32);
                        stringBlock.Language = FileTools.ByteArrayToStringUnicode(fileData, offset);
                        offset += (count + 1) * 2;
                    }
                    else
                    {
                        stringBlock.Language = "NOT SET";
                    }

                    count = FileTools.ByteArrayToInt32(fileData, offset);
                    offset += sizeof(Int32);
                    stringBlock.Usage = FileTools.ByteArrayToStringUnicode(fileData, offset);
                    offset += (count + 1) * 2;

                    if (stringBlock.Usage == "Singular")
                    {
                        count = FileTools.ByteArrayToInt32(fileData, offset);
                        offset += sizeof(Int32);
                        stringBlock.DefaultString = FileTools.ByteArrayToStringUnicode(fileData, offset);
                        offset += (count + 1) * 2;
                    }
                    else
                    {
                        stringBlock.DefaultString = "NOT SET";
                    }



                    strings.Add(stringBlock);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public StringBlock[] GetFileTable()
        {
            return strings.ToArray();
        }

        public List<StringBlock> StringsTable
        {
            get { return strings; }
        }
    }
}
