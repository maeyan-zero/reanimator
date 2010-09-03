using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Reanimator
{
    public partial class StringsFile : DataFile
    {
        private StringsHeader _stringsHeader;
        public List<StringBlock> StringsTable { get; private set; }

        public StringsFile(String stringId, Type type)
            : base(stringId, type)
        {
            IsStringsFile = true;
        }

        override public String ToString()
        {
            return StringId;
        }

        public override bool ParseData(byte[] data)
        {
            _data = data;
            int offset = 0;
            StringsTable = new List<StringBlock>();

            _stringsHeader = FileTools.ByteArrayToStructure<StringsHeader>(_data, ref offset);

            bool attributeCountMessageShown = false;
            for (int i = 0; i < _stringsHeader.Count; i++)
            {
                StringBlock stringBlock = new StringBlock
                {
                    ReferenceId = FileTools.ByteArrayToInt32(_data, ref offset),
                    Unknown = FileTools.ByteArrayToInt32(_data, ref offset)
                };

                int count = FileTools.ByteArrayToInt32(_data, ref offset);
                stringBlock.StringId = FileTools.ByteArrayToStringASCII(_data, offset);
                offset += count + 1;

                stringBlock.Reserved = FileTools.ByteArrayToInt32(_data, ref offset);

                count = FileTools.ByteArrayToInt32(_data, ref offset);
                stringBlock.String = FileTools.ByteArrayToStringUnicode(_data, offset);
                offset += count;

                stringBlock.AttributeCount = FileTools.ByteArrayToInt32(_data, ref offset);

                if (stringBlock.AttributeCount > MaxAttributes)
                {
                    if (!attributeCountMessageShown)
                    {
                        MessageBox.Show("Unexpected Attribute Count!\n\nCount: \n\nPlease report this message." + stringBlock.AttributeCount, "Warning",
                                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        attributeCountMessageShown = true;
                    }
                }

                for (int j = 0; j < stringBlock.AttributeCount; j++)
                {
                    count = FileTools.ByteArrayToInt32(_data, ref offset);

                    switch (j)
                    {
                        case 0:
                            stringBlock.Attribute1 = FileTools.ByteArrayToStringUnicode(_data, offset);
                            break;
                        case 1:
                            stringBlock.Attribute2 = FileTools.ByteArrayToStringUnicode(_data, offset);
                            break;
                        case 2:
                            stringBlock.Attribute3 = FileTools.ByteArrayToStringUnicode(_data, offset);
                            break;
                        case 3:
                            stringBlock.Attribute4 = FileTools.ByteArrayToStringUnicode(_data, offset);
                            break;
                    }

                    offset += (count + 1) * 2;
                }

                StringsTable.Add(stringBlock);
            }

            if (offset != _data.Length)
            {
                MessageBox.Show("Incomplete file parsing!\n\n" + FilePath, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            IsGood = true;
            return true;
        }

        public  override byte[] GenerateFile(DataTable table)
        {
            /***** Strings File *****
             * FileToken                                    Int32                   0x68667374 ('tsfh').
             * Version                                      Int32                   Only seen as 0x06.
             * Count                                        Int32                   Count of StringBlocks.
             * {
             *      ReferenceId                             Int32                   Global reference ID in-game.
             *      Unknown                                 Int32                   TO BE DETEREMINED
             *      
             *      ByteCount                               Int32                   Byte count of following ASCII string (not including \0 termination).
             *      StringId                                ByteCount + 1           String as ASCII, zero terminated.
             *      
             *      Reserved                                Int32                   Null.
             *      
             *      ByteCount                               Int32                   Byte count of following UNICODE string (include \0 termination).
             *      String                                  ByteCount               String as UNICODE, zero terminated.
             *      
             *      AttributeCount                          Int32                   Count of attributes (0 to 4).
             *      {
             *          CharCount                           Int32                   Count of ASCII characters of following string (not including \0 termination).
             *          AttributeString                     (CharCount + 1) * 2     String as UNICODE, zero terminated.
             *      }
             * }
             */

            byte[] buffer = new byte[1024];
            int offset = 0;
            int lastOffset = 4;

            // write main header first
            StringsHeader stringsHeader = new StringsHeader
            {
                Header = FileTokens.Header,
                Version = Version,
                Count = table.Rows.Count
            };
            FileTools.WriteToBuffer(ref buffer, ref offset, stringsHeader);

            // write string blocks
            int row = 0;
            foreach (DataRow dr in table.Rows)
            {
                String stringId = dr["StringId"] as String;
                if (String.IsNullOrEmpty(stringId)) continue;


                // ReferenceId
                Int32 referenceId = (Int32)dr[0];
                FileTools.WriteToBuffer(ref buffer, ref offset, referenceId);


                // Unknown
                Int32 unknownValue = 0;
                if (row < StringsTable.Count)
                {
                    unknownValue = StringsTable[row].Unknown;
                }
                FileTools.WriteToBuffer(ref buffer, ref offset, unknownValue);


                // StringId
                byte[] stringIdBytes = FileTools.StringToASCIIByteArray(stringId);
                Int32 byteCount = stringIdBytes.Length;
                FileTools.WriteToBuffer(ref buffer, ref offset, byteCount);
                FileTools.WriteToBuffer(ref buffer, ref offset, stringIdBytes);
                offset++; // \0


                // Reserved
                offset += sizeof(Int32);


                // String
                String str = dr["String"] as String;
                byteCount = 2; // \0
                if (String.IsNullOrEmpty(str))
                {
                    FileTools.WriteToBuffer(ref buffer, ref offset, byteCount);
                }
                else
                {
                    byte[] stringBytes = FileTools.StringToUnicodeByteArray(str);
                    byteCount += stringBytes.Length;
                    FileTools.WriteToBuffer(ref buffer, ref offset, byteCount);
                    FileTools.WriteToBuffer(ref buffer, ref offset, stringBytes);
                    
                }
                offset += 2; // \0


                // Attributes
                List<String> attributeStrings = new List<string>();
                for (int i = 0; i < MaxAttributes; i++)
                {
                    String colName = "Attribute" + (i + 1);
                    if (!table.Columns.Contains(colName)) continue;

                    String attribute = dr[colName] as String;
                    if (String.IsNullOrEmpty(attribute)) continue;

                    attributeStrings.Add(attribute);
                }

                Int32 attributeCount = attributeStrings.Count;
                FileTools.WriteToBuffer(ref buffer, ref offset, attributeCount);

                foreach (String s in attributeStrings)
                {
                    Int32 charCount = s.Length;
                    FileTools.WriteToBuffer(ref buffer, ref offset, charCount);
                    byte[] attributeBytes = FileTools.StringToUnicodeByteArray(s);
                    FileTools.WriteToBuffer(ref buffer, offset, attributeBytes);
                    lastOffset = attributeBytes.Length + 2; // \0
                    offset += lastOffset;
                }

                row++;
            }

            //offset -= lastOffset;
            byte[] returnBuffer = new byte[offset];
            Buffer.BlockCopy(buffer, 0, returnBuffer, 0, offset);

            return returnBuffer;
        }
    }
}
