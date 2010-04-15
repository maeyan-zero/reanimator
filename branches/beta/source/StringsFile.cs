using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Data;

namespace Reanimator
{
    public class StringsFile
    {
        // general structures/const stuffs
#pragma warning disable 169, 649
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class StringsHeader
        {
            public Int32 Header;
            public Int32 Version;
            public Int32 Count;
        }
#pragma warning restore 169, 649

        private abstract class FileTokens
        {
            public const Int32 Header = 0x68667374;
        }
        private const Int32 Version = 6;
        private const int MaxAttributes = 4;


        // public access stuffs
        public class StringBlock
        {
            public Int32 ReferenceId { get; set; }
            public Int32 Unknown { get; set; }
            public String StringId { get; set; }
            public Int32 Reserved;
            public String String { get; set; }
            public Int32 AttributeCount;
            public String Attribute1 { get; set; }
            public String Attribute2 { get; set; }
            public String Attribute3 { get; set; }
            public String Attribute4 { get; set; }

            public StringBlock()
            {
                StringId = String.Empty;
                String = String.Empty;
                Attribute1 = String.Empty;
                Attribute2 = String.Empty;
                Attribute3 = String.Empty;
                Attribute4 = String.Empty;
            }
        }

        readonly byte[] _fileData;
        readonly StringsHeader _stringsHeader;
        public List<StringBlock> StringsTable { get; private set; }
        public String Name { get; set; }
        public bool IsGood { get; private set; }
        public String FilePath { get; set; }
        public const String FileExtention = "xls.uni.cooked";

        public StringsFile(byte[] data)
        {
            FilePath = String.Empty;
            IsGood = false;
            _fileData = data;
            int offset = 0;
            StringsTable = new List<StringBlock>();

            _stringsHeader = FileTools.ByteArrayToStructure(_fileData, typeof(StringsHeader), 0) as StringsHeader;
            if (_stringsHeader == null) return;

            offset += Marshal.SizeOf(_stringsHeader);

            bool attributeCountMessageShown = false;
            for (int i = 0; i < _stringsHeader.Count; i++)
            {
                StringBlock stringBlock = new StringBlock();

                stringBlock.ReferenceId = FileTools.ByteArrayToInt32(_fileData, offset);
                offset += sizeof(Int32);

                stringBlock.Unknown = FileTools.ByteArrayToInt32(_fileData, offset);
                offset += sizeof(Int32);

                int count = FileTools.ByteArrayToInt32(_fileData, offset);
                offset += sizeof(Int32);
                stringBlock.StringId = FileTools.ByteArrayToStringAnsi(_fileData, offset);
                offset += count + 1;

                stringBlock.Reserved = FileTools.ByteArrayToInt32(_fileData, offset);
                offset += sizeof(Int32);

                count = FileTools.ByteArrayToInt32(_fileData, offset);
                offset += sizeof(Int32);
                stringBlock.String = FileTools.ByteArrayToStringUnicode(_fileData, offset);
                offset += count;

                stringBlock.AttributeCount = FileTools.ByteArrayToInt32(_fileData, offset);
                offset += sizeof(Int32);

                if (stringBlock.AttributeCount > MaxAttributes)
                {
                    if (!attributeCountMessageShown)
                    {
                        MessageBox.Show("Unexpected Attribute Count!\n\nCount: " + stringBlock.AttributeCount, "Warning",
                                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        attributeCountMessageShown = true;
                    }
                }

                for (int j = 0; j < stringBlock.AttributeCount; j++)
                {
                    count = FileTools.ByteArrayToInt32(_fileData, offset);
                    offset += sizeof(Int32);

                    switch (j)
                    {
                        case 0:
                            stringBlock.Attribute1 = FileTools.ByteArrayToStringUnicode(_fileData, offset);
                            break;
                        case 1:
                            stringBlock.Attribute2 = FileTools.ByteArrayToStringUnicode(_fileData, offset);
                            break;
                        case 2:
                            stringBlock.Attribute3 = FileTools.ByteArrayToStringUnicode(_fileData, offset);
                            break;
                        case 3:
                            stringBlock.Attribute4 = FileTools.ByteArrayToStringUnicode(_fileData, offset);
                            break;
                    }

                    offset += (count + 1) * 2;
                }

                StringsTable.Add(stringBlock);
            }

            if (offset != _fileData.Length)
            {
                MessageBox.Show("Incomplete file parsing!\n\n" + FilePath, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                IsGood = false;
                return;
            }

            IsGood = true;
        }

        public StringBlock[] GetFileTable()
        {
            return StringsTable.ToArray();
        }

        override public String ToString()
        {
            return Name;
        }

        internal byte[] GenerateStringsFile(DataTable table)
        {
            /***** Strings File *****
             * FileToken                                    Int32                   0x68667374 ('tsfh').
             * MajorVersion                                 Int32                   Only seen as 0x06.
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
