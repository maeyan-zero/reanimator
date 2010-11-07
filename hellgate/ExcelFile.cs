using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using SkmDataStructures2;
using Revival.Common;

namespace Hellgate
{
    public partial class ExcelFile : DataFile
    {
        #region Members
        byte[] StringBuffer;
        byte[] IntegerBuffer;
        byte[] MyshBuffer; // skills, stats(?), properties
        List<byte[]> ExtendedBuffer; // item types
        List<byte[]> FinalBuffer; // unittypes & states. Generate this eventually
        StringCollection SecondaryStrings;
        TypeMap ExcelMap { get; set; }

        const int RcshValue = 4;
        const int TyshValue = 2;
        const int DnehValue = 0;

        // Special structures
        const uint idItems = 0x887988C4;
        const uint idItemsTc = 0xE08E6C41;
        const uint idUnitTypes = 0x1F9DDC98;
        const uint idUintTypesTc = 0x5CE494C9;

        ExcelHeader ExcelHead = new ExcelHeader
        {
            Unknown321 = 0x01,
            Unknown322 = 0x09,
            Unknown161 = 0x0A,
            Unknown162 = 0x0A,
            Unknown163 = 0x00,
            Unknown164 = 0x00,
            Unknown165 = 0x0A,
            Unknown166 = 0x00
        };

        TableHeader DefaultTableHeader = new TableHeader
        {
            Unknown1 = 0x03,
            Unknown2 = 0x3F,
            VersionMajor = 0,
            Reserved1 = -1,
            VersionMinor = 0,
            Reserved2 = -1
        };
        #endregion

        /// <summary>
        /// Default ExcelFile constructor.
        /// </summary>
        public ExcelFile()
        {
            IsExcelFile = true;
        }

        /// <summary>
        /// Creates a new ExcelFile object.
        /// </summary>
        /// <param name="buffer">Byte array of the given Excel file object.</param>
        public ExcelFile(byte[] buffer)
            : this()
        {
            int peek = FileTools.ByteArrayToInt32(buffer, 0);
            bool isCSV = (!(peek == Token.cxeh));
            IntegrityCheck = ((isCSV)) ? ParseCSV(buffer) : ParseData(buffer);
        }

        /// <summary>
        /// Creates a ExcelFile based on the CSV file.
        /// </summary>
        /// <param name="buffer">The CSV file as a byte array.</param>
        /// <returns>True if the buffer parsed okay.</returns>
        public override bool ParseCSV(byte[] buffer)
        {
            // Pre-checks
            if (buffer == null) return false;
            if (buffer.Length < 64) return false;

            // Initialization
            int offset = 0;
            byte delimiter = (byte)'\t';
            int stringBufferOffset = 1;
            int integerBufferOffset = 1;

            // Determine the data type
            StringID = FileTools.ByteArrayToStringASCII(FileTools.GetDelimintedByteArray(buffer, ref offset, delimiter), 0);
            StringID = StringID.Replace("\"", "");//in case strings embedded
            IEnumerable<KeyValuePair<string, uint>> tokenQuery = DataTables.Where(dt => dt.Key == StringID);
            if (tokenQuery.Count() == 0) return false;
            StructureID = tokenQuery.First().Value;
            if (!(DataTypes.Contains(StructureID))) return false;
            ExcelMap = (TypeMap)DataTypes[StructureID];
            DataType = ExcelMap.DataType;
            ExcelHead.StructureID = StructureID;

            // Mutate the buffer into a string array
            string[][] tableRows = FileTools.CSVtoStringArray(buffer, DataType.GetFields().Count(), delimiter);
            if ((tableRows == null)) return false;

            // Parse the tableRows
            Rows = new List<Object>();
            BindingFlags bindingFlags = (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            for (int row = 0; row < tableRows.Count(); row++)
            {
                int col = 0;
                Object rowInstance = Activator.CreateInstance(DataType);
                foreach (FieldInfo fieldInfo in DataType.GetFields(bindingFlags))
                {
                    // Initialize private fields 
                    if ((fieldInfo.IsPrivate))
                    {
                        if ((fieldInfo.FieldType == typeof(TableHeader)))
                        {
                            fieldInfo.SetValue(rowInstance, DefaultTableHeader);
                            continue;
                        }
                        if ((fieldInfo.FieldType.BaseType == typeof(Array)))
                        {
                            MarshalAsAttribute marshal = (MarshalAsAttribute)fieldInfo.GetCustomAttributes(typeof(MarshalAsAttribute), false).First();
                            Array arrayInstance = (Array)Activator.CreateInstance(fieldInfo.FieldType, marshal.SizeConst);
                            fieldInfo.SetValue(rowInstance, arrayInstance);
                            continue;
                        }
                        if ((fieldInfo.FieldType == typeof(String)))
                        {
                            fieldInfo.SetValue(rowInstance, String.Empty);
                            continue;
                        }
                        continue;
                    }

                    // Parse public fields
                    string value = tableRows[row][col++];
                    OutputAttribute attribute = GetExcelOutputAttribute(fieldInfo);
                    if (!(attribute == null))
                    {
                        if ((attribute.IsStringOffset) || (attribute.IsIntOffset))
                        {
                            if ((value == "0"))
                            {
                                fieldInfo.SetValue(rowInstance, 0);
                                continue;
                            }
                        }

                        if ((attribute.IsStringOffset))
                        {
                            if ((StringBuffer == null))
                            {
                                StringBuffer = new byte[1024];
                            }
                            fieldInfo.SetValue(rowInstance, stringBufferOffset);
                            FileTools.WriteToBuffer(ref StringBuffer, ref stringBufferOffset, FileTools.StringToASCIIByteArray(value));
                            continue;
                        }

                        if ((attribute.IsIntOffset))
                        {
                            if ((IntegerBuffer == null))
                            {
                                IntegerBuffer = new byte[1024];
                            }
                            value = value.Replace("\"", "");
                            string[] splitValue = value.Split(',');
                            int count = splitValue.Length;
                            int[] intValue = new int[count];
                            for (int i = 0; i < count; i++)
                                intValue[i] = int.Parse(splitValue[i]);
                            fieldInfo.SetValue(rowInstance, integerBufferOffset);//todo
                            FileTools.WriteToBuffer(ref IntegerBuffer, ref integerBufferOffset, FileTools.IntArrayToByteArray(intValue));
                            continue;
                        }
                    }

                    Object objValue = FileTools.StringToType(value, fieldInfo.FieldType);
                    fieldInfo.SetValue(rowInstance, objValue);
                }

                Rows.Add(rowInstance);
            }

            if (!(stringBufferOffset == 1))
                Array.Resize<byte>(ref StringBuffer, stringBufferOffset);

            if (!(integerBufferOffset == 1))
                Array.Resize<byte>(ref IntegerBuffer, integerBufferOffset);

            return true;
        }

        /// <summary>
        /// Creates a ExcelFile based on the serialized data source.
        /// </summary>
        /// <param name="buffer">The serialized excel file as a byte array.</param>
        /// <returns>True if the buffer parsed okay.</returns>
        public override bool ParseData(byte[] buffer)
        {
            if ((buffer == null)) return false;
            if ((buffer.Length == 0)) return false;
            int offset = 0;

            // File Header
            if (!(CheckFlag(buffer, ref offset, Token.cxeh))) return false;
            ExcelHeader excelHeader = FileTools.ByteArrayToStructure<ExcelHeader>(buffer, ref offset);
            StructureID = excelHeader.StructureID;
            DataType = (Type)DataTypes[StructureID];
            if ((DataType == null)) return false;

            // Strings Block
            if (!(CheckFlag(buffer, ref offset, Token.cxeh))) return false;
            int StringBufferOffset = FileTools.ByteArrayToInt32(buffer, ref offset);
            if (!(StringBufferOffset == 0))
            {
                StringBuffer = new byte[StringBufferOffset];
                Buffer.BlockCopy(buffer, offset, StringBuffer, 0, StringBufferOffset);
                offset += StringBufferOffset;
            }

            // Dataset Block
            if (!(CheckFlag(buffer, ref offset, Token.cxeh))) return false;
            int rowCount = FileTools.ByteArrayToInt32(buffer, ref offset);
            Rows = new List<Object>(rowCount);
            for (int i = 0; i < rowCount; i++)
            {
                Rows.Add(FileTools.ByteArrayToStructure(buffer, DataType, ref offset));
            }

            // Primary Indice Block
            if (!(CheckFlag(buffer, ref offset, Token.cxeh))) return false;
            if (!(StructureID == idItems || StructureID == idItemsTc))
            {
                offset += (Count * sizeof(int));// do not allocate this array
            }
            else
            {
                ExtendedBuffer = new List<byte[]>(Count);
                for (int i = 0; i < Count; i++)
                {
                    offset += sizeof(int); // Skip the indice
                    int size = FileTools.ByteArrayToInt32(buffer, ref offset);
                    ExtendedBuffer.Add(new byte[size]);
                    Buffer.BlockCopy(buffer, offset, ExtendedBuffer[i], 0, size);
                    offset += size;
                }
            }

            // Secondary String Block
            if (!(CheckFlag(buffer, offset, Token.cxeh)))
            {
                int stringCount = FileTools.ByteArrayToInt32(buffer, ref offset);
                if (!(stringCount == 0)) SecondaryStrings = new StringCollection();
                for (int i = 0; i < stringCount; i++)
                {
                    int charCount = FileTools.ByteArrayToInt32(buffer, ref offset);
                    SecondaryStrings.Add(FileTools.ByteArrayToStringASCII(buffer, offset));
                    offset += charCount;
                }
            }

            // Sorted Indices
            for (int i = 0; i < 4; i++)
            {
                if (!(CheckFlag(buffer, ref offset, Token.cxeh))) return false;
                int count = FileTools.ByteArrayToInt32(buffer, ref offset);
                offset += (count * sizeof(int)); // do not allocate
            }

            // Rcsh, Tysh, Mysh, Dneh block
            if (!(CheckFlag(buffer, ref offset, Token.cxeh))) return false;
            if (!(CheckFlag(buffer, offset, 0)))
            {
                if (!(CheckFlag(buffer, ref offset, Token.rcsh))) return false;
                if (!(CheckFlag(buffer, ref offset, RcshValue))) return false;
                if (!(CheckFlag(buffer, ref offset, Token.tysh))) return false;
                if (!(CheckFlag(buffer, ref offset, TyshValue))) return false;
                if ((CheckFlag(buffer, offset, Token.mysh)))
                {
                    offset += sizeof(int);
                    ParseMyshTable(buffer, ref offset);
                }
                if (!(CheckFlag(buffer, ref offset, Token.dneh))) return false;
                if (!(CheckFlag(buffer, ref offset, DnehValue))) return false;
            }

            // Integer Block
            if (!(StructureID == idUnitTypes) && !(StructureID == idUintTypesTc))
            {
                if ((CheckFlag(buffer, ref offset, Token.cxeh)))
                {
                    int IntegerBufferOffset = FileTools.ByteArrayToInt32(buffer, ref offset);
                    if (!(IntegerBufferOffset == 0))
                    {
                        IntegerBuffer = new byte[IntegerBufferOffset];
                        Buffer.BlockCopy(buffer, offset, IntegerBuffer, 0, IntegerBufferOffset);
                        offset += IntegerBufferOffset;
                    }
                }
            }

            // Final data block
            if (!(CheckFlag(buffer, offset, 0)))
            {
                if (!(CheckFlag(buffer, ref offset, Token.cxeh))) return false;
                int byteCount = FileTools.ByteArrayToInt32(buffer, ref offset);
                int blockCount = FileTools.ByteArrayToInt32(buffer, ref offset);
                if (!(byteCount == 0)) // states & unittypes only
                {
                    byteCount = byteCount << 2;
                    FinalBuffer = new List<byte[]>(blockCount);
                    for (int i = 0; i < blockCount; i++)
                    {
                        FinalBuffer.Add(new byte[byteCount]);
                        Buffer.BlockCopy(buffer, offset, FinalBuffer[i], 0, byteCount);
                        offset += byteCount;
                    }
                }
            }

            return offset == buffer.Length;
        }

        /// <summary>
        /// Creates a ExcelFile based on the DataTable data.
        /// </summary>
        /// <param name="dataTable">The DataTable to read the data from.</param>
        /// <returns>True if the DataTable parsed okay.</returns>
        public override bool ParseDataTable(DataTable dataTable)
        {
            return true;
        }

        /// <summary>
        /// Converts the ExcelFile to a byte array.
        /// </summary>
        /// <returns>The serialized ExcelFile.</returns>
        public override byte[] ToByteArray()
        {
            byte[] buffer = new byte[1024];
            int offset = 0;

            // The Excel File header
            FileTools.WriteToBuffer(ref buffer, ref offset, Token.cxeh);
            FileTools.WriteToBuffer(ref buffer, ref offset, ExcelHead);

            // Strings Block
            FileTools.WriteToBuffer(ref buffer, ref offset, Token.cxeh);
            if (!(StringBuffer == null))
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, StringBuffer.Length);
                FileTools.WriteToBuffer(ref buffer, ref offset, StringBuffer);
            }
            else
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, 0);
            }

            // Dataset Block
            FileTools.WriteToBuffer(ref buffer, ref offset, Token.cxeh);
            FileTools.WriteToBuffer(ref buffer, ref offset, Rows.Count);
            foreach (Object row in Rows)
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, row);
            }

            // Primary Indice
            FileTools.WriteToBuffer(ref buffer, ref offset, Token.cxeh);
            for (int i = 0; i < Rows.Count; i++)
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, i);
            }

            // Generate custom sorts
            int[][] customSorts = new int[4][];
            foreach (FieldInfo fieldInfo in DataType.GetFields())
            {
                OutputAttribute outputAttribute = GetExcelOutputAttribute(fieldInfo);
                if ((outputAttribute == null)) continue;
                if (!(outputAttribute.SortAscendingID == 0))
                {
                    int pos = outputAttribute.SortAscendingID - 1;
                    var sortedList = from element in Rows
                                     orderby fieldInfo.GetValue(element)
                                     select Rows.IndexOf(element);
                    customSorts[pos] = sortedList.ToArray();
                }
            }

            // Write custom sorts
            foreach (int[] intArray in customSorts)
            {                
                if (!(intArray == null))
                {
                    FileTools.WriteToBuffer(ref buffer, ref offset, Token.cxeh);
                    FileTools.WriteToBuffer(ref buffer, ref offset, intArray.Length);
                    FileTools.WriteToBuffer(ref buffer, ref offset, FileTools.IntArrayToByteArray(intArray));
                }
                else
                {
                    FileTools.WriteToBuffer(ref buffer, ref offset, Token.cxeh);
                    FileTools.WriteToBuffer(ref buffer, ref offset, 0);
                }
            }

            // Rcsh, Tysh, Mysh, Dneh
            FileTools.WriteToBuffer(ref buffer, ref offset, Token.cxeh);
            if ((ExcelMap.HasRcsh))
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.rcsh);
                FileTools.WriteToBuffer(ref buffer, ref offset, RcshValue);
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.tysh);
                FileTools.WriteToBuffer(ref buffer, ref offset, TyshValue);
                if ((ExcelMap.HasMysh))
                {
                    FileTools.WriteToBuffer(ref buffer, ref offset, Token.mysh);
                    FileTools.WriteToBuffer(ref buffer, ref offset, MyshBuffer);
                }
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.dneh);
                FileTools.WriteToBuffer(ref buffer, ref offset, DnehValue);
            }
            else
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, 0);
            }

            // Append the integer array.
            if (!(IntegerBuffer == null))
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.cxeh);
                FileTools.WriteToBuffer(ref buffer, ref offset, IntegerBuffer.Length);
                FileTools.WriteToBuffer(ref buffer, ref offset, IntegerBuffer);
            }
            else
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.cxeh);
                FileTools.WriteToBuffer(ref buffer, ref offset, 0);
            }

            // Final data block
            //int blockCount = FinalData != null ? dataTable.Rows.Count : 0;
            //int blockSize = FinalData != null ? FinalData[0].Length : 0;
            //int bshiftCount = blockSize > 0 ? (blockSize >> 2) : 0;

            FileTools.WriteToBuffer(ref buffer, ref offset, Token.cxeh);
            FileTools.WriteToBuffer(ref buffer, ref offset, 0);//bshiftCount
            FileTools.WriteToBuffer(ref buffer, ref offset, 0);//blockCount

            Array.Resize<byte>(ref buffer, offset);
            return buffer;
        }

        /// <summary>
        /// Converts a TestCenter ExcelFile into a SinglePlayer version.
        /// </summary>
        /// <param name="spDataTable">The source SinglePlayer DataTable.</param>
        /// <param name="tcDataTable">The source TestCenter DataTable.</param>
        /// <returns>The converted DataTable.</returns>
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
    }
}
