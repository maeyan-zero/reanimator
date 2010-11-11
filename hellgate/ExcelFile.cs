using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using Revival.Common;

namespace Hellgate
{
    public partial class ExcelFile : DataFile
    {
        #region Members
        byte[] StringBuffer;
        byte[] IntegerBuffer;
        byte[] MyshBuffer;
        byte[][] ExtendedBuffer;
        StringCollection SecondaryStrings;
        TypeMap ExcelMap { get; set; }
        new Type DataType { get { return ExcelMap.DataType; } }
        new UInt32 StructureID { get { return ExcelFileHeader.StructureID; } }

        ExcelHeader ExcelFileHeader = new ExcelHeader
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

        TableHeader ExcelTableHeader = new TableHeader
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
            if (tokenQuery.Count() == 0) return false; // no string association found
            ExcelFileHeader.StructureID = tokenQuery.First().Value;
            if (!(DataTypes.Contains(StructureID))) return false; // no structure definition found
            ExcelMap = (TypeMap)DataTypes[StructureID]; // grab the excel map

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
                            fieldInfo.SetValue(rowInstance, ExcelTableHeader);
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
                    // All public fields must be inside the CSV
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
                            {
                                intValue[i] = int.Parse(splitValue[i]);
                            }
                            fieldInfo.SetValue(rowInstance, integerBufferOffset);
                            FileTools.WriteToBuffer(ref IntegerBuffer, ref integerBufferOffset, FileTools.IntArrayToByteArray(intValue));
                            continue;
                        }

                        if ((attribute.IsSecondaryString))
                        {
                            if ((SecondaryStrings == null))
                            {
                                SecondaryStrings = new StringCollection();
                            }
                            if (!(SecondaryStrings.Contains(value)))
                            {
                                SecondaryStrings.Add(value);
                            }
                            fieldInfo.SetValue(rowInstance, SecondaryStrings.IndexOf(value));
                            continue;
                        }
                    }

                    Object objValue = FileTools.StringToType(value, fieldInfo.FieldType);
                    fieldInfo.SetValue(rowInstance, objValue);
                }

                // For item types, items, missiles, monsters etc
                // This must be a hex byte delimited array
                if ((ExcelMap.HasExtended))
                {
                    if ((ExtendedBuffer == null))
                    {
                        ExtendedBuffer = new byte[tableRows.Count()][];
                    }
                    char split = ',';
                    string value = tableRows[row][col];
                    string[] stringArray = value.Split(split);
                    byte[] byteArray = new byte[stringArray.Length];
                    for (int i = 0; i < byteArray.Length; i++)
                    {
                        byteArray[i] = Byte.Parse(stringArray[i], NumberStyles.HexNumber);
                    }
                    ExtendedBuffer[row] = byteArray;
                }

                Rows.Add(rowInstance);
            }

            if (!(StringBuffer == null))
                Array.Resize<byte>(ref StringBuffer, stringBufferOffset);

            if (!(IntegerBuffer == null))
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
            ExcelFileHeader = FileTools.ByteArrayToStructure<ExcelHeader>(buffer, ref offset);
            ExcelMap = (TypeMap)DataTypes[StructureID];
            if ((ExcelMap == null)) return false;
            StringID = DataTables.Where(dt => dt.Value == ExcelFileHeader.StructureID).First().Key;

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
            if (!(ExcelMap.HasExtended))
            {
                offset += (Count * sizeof(int));// do not allocate this array
            }
            else
            {
                ExtendedBuffer = new byte[Count][];
                for (int i = 0; i < Count; i++)
                {
                    offset += sizeof(int); // Skip the indice
                    int size = FileTools.ByteArrayToInt32(buffer, ref offset);
                    ExtendedBuffer[i] = new byte[size];
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
                if (!(CheckFlag(buffer, ref offset, Token.RcshValue))) return false;
                if (!(CheckFlag(buffer, ref offset, Token.tysh))) return false;
                if (!(CheckFlag(buffer, ref offset, Token.TyshValue))) return false;
                if ((ExcelMap.HasMysh))
                {
                    if (!(CheckFlag(buffer, ref offset, Token.mysh))) return false;
                    ParseMyshTable(buffer, ref offset);
                }
                if (!(CheckFlag(buffer, ref offset, Token.dneh))) return false;
                if (!(CheckFlag(buffer, ref offset, Token.DnehValue))) return false;
            }

            // Integer Block
            // Only the UnitTypes class ignores this section
            if (!(ExcelMap.IgnoresTable))
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
                if (!(byteCount == 0))
                {
                    byteCount = byteCount << 2;
                    offset += ((byteCount * blockCount)); //do not allocate
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
            FileTools.WriteToBuffer(ref buffer, ref offset, ExcelFileHeader);

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
            int[][] customSorts = CreateSortIndices();
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
            // This section exists when there is a string or integer block or a mysh table
            FileTools.WriteToBuffer(ref buffer, ref offset, Token.cxeh);
            if (!(StringBuffer == null) || !(IntegerBuffer == null) || ExcelMap.HasMysh)
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.rcsh);
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.RcshValue);
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.tysh);
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.TyshValue);
                if ((ExcelMap.HasMysh))
                {
                    FileTools.WriteToBuffer(ref buffer, ref offset, Token.mysh);
                    FileTools.WriteToBuffer(ref buffer, ref offset, MyshBuffer);
                }
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.dneh);
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.DnehValue);
            }
            else
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, 0);
            }

            // Append the integer array.
            // Unitypes ignores this section for some reason
            if (!(ExcelMap.IgnoresTable))
            {
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
            }

            // Signature. Used with isa1, isa2, isa3 etc etc.
            // Unittypes, states.
            if ((ExcelMap.HasSignature))
            {
                int blockSize = ((int)(System.Math.Ceiling((double)(Count / Signature.Length))) >> 2);
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.cxeh);
                FileTools.WriteToBuffer(ref buffer, ref offset, blockSize);
                FileTools.WriteToBuffer(ref buffer, ref offset, Count);
                foreach (uint integer in CreateSignature())
                {
                    FileTools.WriteToBuffer(ref buffer, ref offset, integer);
                }
            }
            else
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.cxeh);
                FileTools.WriteToBuffer(ref buffer, ref offset, 0);
                FileTools.WriteToBuffer(ref buffer, ref offset, 0);
            }

            // Resize
            Array.Resize<byte>(ref buffer, offset);
            return buffer;
        }

        /// <summary>
        /// Converts the ExcelFile to a CSV
        /// </summary>
        /// <returns>The CSV as a byte array.</returns>
        public override byte[] ExportCSV()
        {
            int noCols = DataType.GetFields().Count();
            int noRows = Count + 1; // +1 for column headers
            if (ExcelMap.HasExtended) noCols++; // extra column for extended data

            Object[,] csvObject = new Object[Count + 1, noCols];

            int row = 0;
            int col = 0;

            // First dump column headers, replace the first with the table string id
            foreach (FieldInfo fieldInfo in DataType.GetFields())
            {
                csvObject[row, col++] = ((col == 0)) ? StringID : fieldInfo.Name;
            }
            if (ExcelMap.HasExtended) csvObject[row, col] = "ExtendedProps";
            row = 1;

            // Parse each row, resolve buffers if needed
            foreach (Object rowObject in Rows)
            {
                col = 0; // reset
                foreach (FieldInfo fieldInfo in DataType.GetFields())
                {
                    OutputAttribute attribute = GetExcelOutputAttribute(fieldInfo);
                    if (!(attribute == null))
                    {
                        if ((attribute.IsStringOffset))
                        {
                            int offset = (int)fieldInfo.GetValue(rowObject);
                            csvObject[row, col++] = (!(offset == -1)) ? ReadStringTable(offset) : String.Empty;
                            continue;
                        }
                        if ((attribute.IsIntOffset))
                        {
                            int offset = (int)fieldInfo.GetValue(rowObject);
                            if ((offset == 0))
                            {
                                csvObject[row, col++] = 0;
                                continue;
                            }
                            byte[] buffer = FileTools.IntArrayToByteArray(ReadIntegerTable(offset));
                            csvObject[row, col++] = Export.ArrayToCSV(buffer, ',', typeof(int));
                            continue;
                        }
                    }
                    csvObject[row, col++] = fieldInfo.GetValue(rowObject);
                }
                if (ExcelMap.HasExtended)
                {
                    csvObject[row, col] = Export.ArrayToCSV(ExtendedBuffer[row - 1], ',', typeof(byte));
                }
                row++;
            }

            return Export.ObjectToArray(csvObject, '\t');
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
