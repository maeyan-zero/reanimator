using System;
using System.Data;
using System.Collections.Generic;
using Revival;

namespace Hellgate
{
    public partial class StringFile
    {
        public String FileName { get; set; }
        public Boolean IntegrityCheck { get; private set; }

        private List<StringBlock> StringsTable;

        public StringFile() { }

        public StringFile(byte[] buffer)
        {
            IntegrityCheck = Read(buffer);
        }

        public bool Read(byte[] buffer)
        {
            int offset = 0;
            StringsTable = new List<StringBlock>();

            StringsHeader stringsHeader = Tools.ByteArrayToStructure<StringsHeader>(buffer, ref offset);

            for (int i = 0; i < stringsHeader.Count; i++)
            {
                StringBlock stringBlock = new StringBlock
                {
                    ReferenceId = Tools.ByteArrayToInt32(buffer, ref offset),
                    Unknown = Tools.ByteArrayToInt32(buffer, ref offset)
                };

                int count = Tools.ByteArrayToInt32(buffer, ref offset);
                stringBlock.StringId = Tools.ByteArrayToStringASCII(buffer, offset);
                offset += count + 1;

                stringBlock.Reserved = Tools.ByteArrayToInt32(buffer, ref offset);

                count = Tools.ByteArrayToInt32(buffer, ref offset);
                stringBlock.String = Tools.ByteArrayToStringUnicode(buffer, offset);
                offset += count;

                stringBlock.AttributeCount = Tools.ByteArrayToInt32(buffer, ref offset);

                if (stringBlock.AttributeCount > MaxAttributes)
                {
                    return false;
                }

                for (int j = 0; j < stringBlock.AttributeCount; j++)
                {
                    count = Tools.ByteArrayToInt32(buffer, ref offset);

                    switch (j)
                    {
                        case 0:
                            stringBlock.Attribute1 = Tools.ByteArrayToStringUnicode(buffer, offset);
                            break;
                        case 1:
                            stringBlock.Attribute2 = Tools.ByteArrayToStringUnicode(buffer, offset);
                            break;
                        case 2:
                            stringBlock.Attribute3 = Tools.ByteArrayToStringUnicode(buffer, offset);
                            break;
                        case 3:
                            stringBlock.Attribute4 = Tools.ByteArrayToStringUnicode(buffer, offset);
                            break;
                    }

                    offset += (count + 1) * 2;
                }

                StringsTable.Add(stringBlock);
            }

            return (offset == buffer.Length);
        }

        public byte[] Create(DataTable dataTable)
        {
            return null;
        }

        public DataTable GetDataTable()
        {
            throw new NotImplementedException();
        }
    }
}
