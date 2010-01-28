using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

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
            public int attributeCount;
            public string Attribute1 { get; set; }
            public string Attribute2 { get; set; }
            public string Attribute3 { get; set; }
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

                    stringBlock.attributeCount = FileTools.ByteArrayToInt32(fileData, offset);
                    offset += sizeof(Int32);

                    if (stringBlock.attributeCount > 3)
                    {
                        MessageBox.Show("Unexpected Attribute Count!\n\nCount: " + stringBlock.attributeCount, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                    for (int j = 0; j < stringBlock.attributeCount; j++)
                    {
                        count = FileTools.ByteArrayToInt32(fileData, offset);
                        offset += sizeof(Int32);

                        if (j == 0)
                        {
                            stringBlock.Attribute1 = FileTools.ByteArrayToStringUnicode(fileData, offset);
                        }
                        else if (j == 1)
                        {
                            stringBlock.Attribute2 = FileTools.ByteArrayToStringUnicode(fileData, offset);
                        }
                        else if (j == 2)
                        {
                            stringBlock.Attribute3 = FileTools.ByteArrayToStringUnicode(fileData, offset);
                        }

                        offset += (count + 1) * 2;
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
