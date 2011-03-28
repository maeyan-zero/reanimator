using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using Revival.Common;

namespace Hellgate
{
    public partial class StringsFile : DataFile
    {
        public StringsFile(byte[] buffer, String filePath)
        {
            IsStringsFile = true;

            FilePath = filePath;
            StringId = _GetStringId(filePath);
            if (StringId == null) throw new Exceptions.DataFileStringIdNotFound(filePath);
            Attributes = DataFileMap[StringId];

            Rows = new List<Object>();
            
            int peek = FileTools.ByteArrayToInt32(buffer, 0);
            bool isCSV = (peek != Token.Header);
            HasIntegrity = ((isCSV)) ? ParseCSV(buffer) : ParseData(buffer);
        }

        public override sealed bool ParseData(byte[] buffer)
        {
            if ((buffer == null)) return false;
            int offset = 0;

            StringsHeader stringsHeader = FileTools.ByteArrayToStructure<StringsHeader>(buffer, ref offset);

            for (int i = 0; i < stringsHeader.Count; i++)
            {
                StringBlock stringBlock = new StringBlock
                {
                    ReferenceId = FileTools.ByteArrayToInt32(buffer, ref offset),
                    Unknown = FileTools.ByteArrayToInt32(buffer, ref offset)
                };

                int count = FileTools.ByteArrayToInt32(buffer, ref offset);
                stringBlock.StringId = FileTools.ByteArrayToStringASCII(buffer, offset);
                offset += count + 1;

                stringBlock.Reserved = FileTools.ByteArrayToInt32(buffer, ref offset);

                count = FileTools.ByteArrayToInt32(buffer, ref offset);
                stringBlock.String = FileTools.ByteArrayToStringUnicode(buffer, offset);
                offset += count;

                int attributeCount = FileTools.ByteArrayToInt32(buffer, ref offset);

                for (int j = 0; j < attributeCount; j++)
                {
                    count = FileTools.ByteArrayToInt32(buffer, ref offset);

                    switch (j)
                    {
                        case 0:
                            stringBlock.Attribute1 = FileTools.ByteArrayToStringUnicode(buffer, offset);
                            break;
                        case 1:
                            stringBlock.Attribute2 = FileTools.ByteArrayToStringUnicode(buffer, offset);
                            break;
                        case 2:
                            stringBlock.Attribute3 = FileTools.ByteArrayToStringUnicode(buffer, offset);
                            break;
                        case 3:
                            stringBlock.Attribute4 = FileTools.ByteArrayToStringUnicode(buffer, offset);
                            break;
                    }

                    offset += (count + 1) * 2;
                }

                Rows.Add(stringBlock);
            }

            return HasIntegrity = ((offset == buffer.Length)) ? true : false;
        }

        public override sealed bool ParseCSV(byte[] buffer, FileManager fileManager = null)
        {
            if ((buffer == null)) return false;

            string[][] stringBuffer = FileTools.UnicodeCSVToStringArray(buffer, 0x09, 0x22);
            if ((stringBuffer == null)) return false;

            int row = 0;
            Rows = new List<Object>();
            FieldInfo[] stringFields = typeof(StringBlock).GetFields();
            foreach (string[] bufferRow in stringBuffer)
            {
                if (row == 0)
                {
                    row++;
                    continue;
                }

                int col = 0;
                StringBlock stringBlock = new StringBlock();
                foreach (FieldInfo fieldInfo in stringFields)
                {
                    fieldInfo.SetValue(stringBlock, FileTools.StringToObject(bufferRow[col++], fieldInfo.FieldType));
                }

                Rows.Add(stringBlock);
            }

            return HasIntegrity = true;
        }

        public override bool ParseDataTable(DataTable table, FileManager fileManager = null)
        {
            byte[] buffer = new byte[1024];
            int offset = 0;
            int lastOffset = 4;

            // write main header first
            StringsHeader stringsHeader = new StringsHeader
            {
                Header = Token.Header,
                Version = Version,
                Count = table.Rows.Count
            };
            FileTools.WriteToBuffer(ref buffer, ref offset, stringsHeader);

            // write string blocks
            int row = 0;
            foreach (DataRow dr in table.Rows)
            {
                String stringId = dr["StringID"] as String;
                if (String.IsNullOrEmpty(stringId)) continue;


                // ReferenceId
                Int32 referenceId = (Int32)dr[0];
                FileTools.WriteToBuffer(ref buffer, ref offset, referenceId);


                // Unknown
                Int32 unknownValue = (Int32)dr[1];
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

            return true;
        }

        public override byte[] ToByteArray()
        {
            byte[] buffer = new byte[1024];
            int offset = 0;

            FileTools.WriteToBuffer(ref buffer, ref offset, Token.Header);
            FileTools.WriteToBuffer(ref buffer, ref offset, Version);
            FileTools.WriteToBuffer(ref buffer, ref offset, Count);

            foreach (StringBlock stringBlock in Rows)
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, stringBlock.ReferenceId);
                FileTools.WriteToBuffer(ref buffer, ref offset, stringBlock.Unknown);
                FileTools.WriteToBuffer(ref buffer, ref offset, stringBlock.StringId.Length);
                FileTools.WriteToBuffer(ref buffer, ref offset, FileTools.StringToASCIIByteArray(stringBlock.StringId));
                FileTools.WriteToBuffer(ref buffer, ref offset, (byte)0);
                FileTools.WriteToBuffer(ref buffer, ref offset, stringBlock.Reserved);
                FileTools.WriteToBuffer(ref buffer, ref offset, (stringBlock.String.Length * sizeof(short)) + sizeof(short));
                FileTools.WriteToBuffer(ref buffer, ref offset, FileTools.StringToUnicodeByteArray(stringBlock.String));
                FileTools.WriteToBuffer(ref buffer, ref offset, (short)0);
                int attributeCount = 0;
                {
                    if (!String.IsNullOrEmpty(stringBlock.Attribute1)) attributeCount++;
                    if (!String.IsNullOrEmpty(stringBlock.Attribute2)) attributeCount++;
                    if (!String.IsNullOrEmpty(stringBlock.Attribute3)) attributeCount++;
                    if (!String.IsNullOrEmpty(stringBlock.Attribute4)) attributeCount++;
                }
                FileTools.WriteToBuffer(ref buffer, ref offset, attributeCount);
                for (int i = 1; i <= attributeCount; i++)
                {
                    switch (i)
                    {
                        case 1:
                            FileTools.WriteToBuffer(ref buffer, ref offset, stringBlock.Attribute1.Length);
                            FileTools.WriteToBuffer(ref buffer, ref offset, FileTools.StringToUnicodeByteArray(stringBlock.Attribute1));
                            FileTools.WriteToBuffer(ref buffer, ref offset, (short)0);
                            break;
                        case 2:
                            FileTools.WriteToBuffer(ref buffer, ref offset, stringBlock.Attribute2.Length);
                            FileTools.WriteToBuffer(ref buffer, ref offset, FileTools.StringToUnicodeByteArray(stringBlock.Attribute2));
                            FileTools.WriteToBuffer(ref buffer, ref offset, (short)0);
                            break;
                        case 3:
                            FileTools.WriteToBuffer(ref buffer, ref offset, stringBlock.Attribute3.Length);
                            FileTools.WriteToBuffer(ref buffer, ref offset, FileTools.StringToUnicodeByteArray(stringBlock.Attribute3));
                            FileTools.WriteToBuffer(ref buffer, ref offset, (short)0);
                            break;
                        case 4:
                            FileTools.WriteToBuffer(ref buffer, ref offset, stringBlock.Attribute4.Length);
                            FileTools.WriteToBuffer(ref buffer, ref offset, FileTools.StringToUnicodeByteArray(stringBlock.Attribute4));
                            FileTools.WriteToBuffer(ref buffer, ref offset, (short)0);
                            break;
                    }
                }
            }

            Array.Resize(ref buffer, offset);
            return buffer;
        }

        /// <summary>
        /// Converts the StringsFile to a tab-delimited CSV
        /// </summary>
        /// <returns>The CSV as a byte array.</returns>
        public override byte[] ExportCSV(FileManager fileManager = null)
        {
            StringWriter writer = new StringWriter();

            // hard-code ftw (it's very unlikely we're going to change it, so meh)
            const String fields = "\"id\"	\"fk\"	\"stringid\"	\"u1\"	\"string\"	\"a1\"	\"a2\"	\"a3\"	\"a4\"";
            writer.WriteLine(fields);

            foreach (StringBlock stringBlock in Rows)
            {
                writer.WriteLine(String.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}",
                                               stringBlock.ReferenceId,
                                               stringBlock.Unknown,
                                               EncapsulateString(stringBlock.StringId),
                                               stringBlock.Reserved,
                                               EncapsulateString(stringBlock.String),
                                               EncapsulateString(stringBlock.Attribute1),
                                               EncapsulateString(stringBlock.Attribute2),
                                               EncapsulateString(stringBlock.Attribute3),
                                               EncapsulateString(stringBlock.Attribute4)));
            }

            return writer.ToString().ToUnicodeByteArray();
        }

        public override byte[] ExportSQL(string tablePrefix = "")
        {
            StringWriter stringWriter = new StringWriter();
            string tableName = "strings"; String.Format("{0}{1}", tablePrefix, StringId.ToLower());
            //stringWriter.WriteLine(String.Format("CREATE TABLE {0} (", tableName));
            ////stringWriter.WriteLine("\tid INT NOT NULL AUTO_INCREMENT PRIMARY KEY");
            //stringWriter.WriteLine("\tpk INT,");
            //stringWriter.WriteLine("\tfk INT,");
            //stringWriter.WriteLine("\tstringid VARCHAR(64),");
            //stringWriter.WriteLine("\tu1 INT,");
            //stringWriter.WriteLine("\tstring TEXT CHARACTER SET utf8 COLLATE utf8_unicode_ci,");
            //stringWriter.WriteLine("\ta1 VARCHAR(16),");
            //stringWriter.WriteLine("\ta2 VARCHAR(16),");
            //stringWriter.WriteLine("\ta3 VARCHAR(16),");
            //stringWriter.WriteLine("\ta4 VARCHAR(16)");
            //stringWriter.WriteLine(");");

            stringWriter.WriteLine(String.Format("INSERT INTO {0} VALUES", tableName));
            int rowCount = 0;
            foreach (StringBlock stringBlock in Rows)
            {
                stringWriter.Write("\t(");
                stringWriter.Write(stringBlock.ReferenceId);
                stringWriter.Write(",");
                stringWriter.Write(stringBlock.Unknown);
                stringWriter.Write(",");
                stringWriter.Write(EncapsulateString(stringBlock.StringId));
                stringWriter.Write(",");
                stringWriter.Write(stringBlock.Reserved);
                stringWriter.Write(",");
                stringWriter.Write(EncapsulateString(StringToSQLString(stringBlock.String)));
                stringWriter.Write(",");
                stringWriter.Write(EncapsulateString(stringBlock.Attribute1));
                stringWriter.Write(",");
                stringWriter.Write(EncapsulateString(stringBlock.Attribute2));
                stringWriter.Write(",");
                stringWriter.Write(EncapsulateString(stringBlock.Attribute3));
                stringWriter.Write(",");
                stringWriter.Write(EncapsulateString(stringBlock.Attribute4));
                stringWriter.Write(")");
                stringWriter.WriteLine(rowCount++ < Count - 1 ? "," : ";");
            }

            byte[] buffer = FileTools.StringToUnicodeByteArray(stringWriter.ToString());
            return buffer;
        }
    }
}
