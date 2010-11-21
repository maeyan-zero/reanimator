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
        public TypeMap ExcelMap { get; set; }
        new Type DataType { get { return ExcelMap.DataType; } }
        new UInt32 StructureID { get { return ExcelFileHeader.StructureID; } set { ExcelFileHeader.StructureID = value; } }

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

        readonly TableHeader ExcelTableHeader = new TableHeader
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
        public override sealed bool ParseCSV(byte[] buffer)
        {
            // Pre-checks
            if (buffer == null) return false;
            if (buffer.Length < 64) return false;

            // Initialization
            int offset = 0;
            const byte delimiter = (byte)'\t';
            int stringBufferOffset = 0;
            int integerBufferOffset = 1;


            StringID = FileTools.ByteArrayToStringASCII(FileTools.GetDelimintedByteArray(buffer, ref offset, delimiter), 0);
            StringID = StringID.Replace("\"", "");//in case strings embedded

            StructureID = GetStructureID(StringID);
            if ((StructureID == 0)) return false;

            ExcelMap = GetTypeMap(StructureID);
            if ((ExcelMap == null)) return false;


            // Mutate the buffer into a string array
            int colCount = ExcelMap.HasExtended ? DataType.GetFields().Count() + 1 : DataType.GetFields().Count();
            string[][] tableRows = FileTools.CSVtoStringArray(buffer, colCount, delimiter);
            if ((tableRows == null)) return false;


            // Parse the tableRows
            Rows = new List<Object>();
            const BindingFlags bindingFlags = (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
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
                    if (attribute != null)
                    {
                        if (attribute.IsStringOffset)
                        {
                            if (StringBuffer == null)
                            {
                                StringBuffer = new byte[1024];
                            }

                            if (String.IsNullOrEmpty(value))
                            {
                                fieldInfo.SetValue(rowInstance, -1);
                                continue;
                            }

                            fieldInfo.SetValue(rowInstance, stringBufferOffset);
                            FileTools.WriteToBuffer(ref StringBuffer, ref stringBufferOffset, FileTools.StringToASCIIByteArray(value));
                            FileTools.WriteToBuffer(ref StringBuffer, ref stringBufferOffset, (byte)0x00);
                            continue;
                        }

                        if ((attribute.IsIntOffset))
                        {
                            if ((IntegerBuffer == null))
                            {
                                IntegerBuffer = new byte[1024];
                                IntegerBuffer[0] = (byte)0x00;
                            }
                            if ((value == "0"))
                            {
                                fieldInfo.SetValue(rowInstance, 0);
                                continue;
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
                            if ((String.IsNullOrEmpty(value)))
                            {
                                fieldInfo.SetValue(rowInstance, -1);
                                continue;
                            }
                            if (!(SecondaryStrings.Contains(value)))
                            {
                                SecondaryStrings.Add(value);
                            }
                            fieldInfo.SetValue(rowInstance, SecondaryStrings.IndexOf(value));
                            continue;
                        }

                        if ((attribute.IsBitmask))
                        {
                            fieldInfo.SetValue(rowInstance, UInt32.Parse(value));
                            continue;
                        }
                    }

                    Object objValue = FileTools.StringToObject(value, fieldInfo.FieldType);
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

            // Resize the integer and string buffers if they were used.
            if (!(StringBuffer == null))
                Array.Resize(ref StringBuffer, stringBufferOffset);

            if (!(IntegerBuffer == null))
                Array.Resize(ref IntegerBuffer, integerBufferOffset);

            return true;
        }

        /// <summary>
        /// Creates a ExcelFile based on the serialized data source.
        /// </summary>
        /// <param name="buffer">The serialized excel file as a byte array.</param>
        /// <returns>True if the buffer parsed okay.</returns>
        public override sealed bool ParseData(byte[] buffer)
        {
            if ((buffer == null)) return false;
            if ((buffer.Length == 0)) return false;
            int offset = 0;

            // File Header
            if (!(CheckFlag(buffer, ref offset, Token.cxeh))) return false;
            ExcelFileHeader = FileTools.ByteArrayToStructure<ExcelHeader>(buffer, ref offset);
            ExcelMap = GetTypeMap(StructureID);
            if ((ExcelMap == null)) return false;

            // Strings Block
            if (!(CheckFlag(buffer, ref offset, Token.cxeh))) return false;
            int stringBufferOffset = FileTools.ByteArrayToInt32(buffer, ref offset);
            if (stringBufferOffset != 0)
            {
                StringBuffer = new byte[stringBufferOffset];
                Buffer.BlockCopy(buffer, offset, StringBuffer, 0, stringBufferOffset);
                offset += stringBufferOffset;
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
                offset += (Count * sizeof(int)); // do not allocate this array
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
                if (stringCount != 0) SecondaryStrings = new StringCollection();
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
                    int integerBufferOffset = FileTools.ByteArrayToInt32(buffer, ref offset);
                    if (integerBufferOffset != 0)
                    {
                        IntegerBuffer = new byte[integerBufferOffset];
                        Buffer.BlockCopy(buffer, offset, IntegerBuffer, 0, integerBufferOffset);
                        offset += integerBufferOffset;
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

            //DEBUG - these parts arn't finished
            if (ExcelMap.HasMysh) return null;
            // if (ExcelMap.HasSignature) return null;

            // The Excel File header
            FileTools.WriteToBuffer(ref buffer, ref offset, Token.cxeh);
            FileTools.WriteToBuffer(ref buffer, ref offset, ExcelFileHeader);

            // Strings Block
            FileTools.WriteToBuffer(ref buffer, ref offset, Token.cxeh);
            if (!(StringBuffer == null) && (StringBuffer.Length > 1))
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
                if ((ExcelMap.HasExtended))
                {
                    FileTools.WriteToBuffer(ref buffer, ref offset, ExtendedBuffer[i].Length);
                    FileTools.WriteToBuffer(ref buffer, ref offset, ExtendedBuffer[i]);
                }
            }

            // Secondary Strings
            if (!(SecondaryStrings == null))
            {
                byte[] secondaryStringBuffer = new byte[1024];
                int secondaryStringBufferOffset = 0;
                foreach (string str in SecondaryStrings)
                {
                    FileTools.WriteToBuffer(ref secondaryStringBuffer, ref secondaryStringBufferOffset, str.Length + 1);
                    FileTools.WriteToBuffer(ref secondaryStringBuffer, ref secondaryStringBufferOffset, FileTools.StringToASCIIByteArray(str));
                    FileTools.WriteToBuffer(ref secondaryStringBuffer, ref secondaryStringBufferOffset, (byte)0x00);
                }
                FileTools.WriteToBuffer(ref buffer, ref offset, SecondaryStrings.Count);
                FileTools.WriteToBuffer(ref buffer, ref offset, secondaryStringBuffer, secondaryStringBufferOffset, false);
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
            if (!(StringBuffer == null) || !(IntegerBuffer == null) || ExcelMap.HasMysh)
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.cxeh);
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

            // Append the integer array.
            // Unitypes ignores this section for some reason
            if (!(ExcelMap.IgnoresTable))
            {
                if (!(IntegerBuffer == null) && (IntegerBuffer.Length > 1))
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

            // row-row index bit relations - generated from isA0, isA1, isA2 etc
            // only applicable on the UNITTYPES and STATES tables
            if (ExcelMap.HasIndexBitRelations)
            {
                int blockSize = (Count >> 5) + 1; // need 1 bit for every row; 32 bits per int - blockSize = no. of Int's
                UInt32[,] indexBitRelations = CreateIndexBitRelations();
                byte[] relationsData = new byte[Count*blockSize*sizeof (UInt32)];
                Buffer.BlockCopy(indexBitRelations, 0, relationsData, 0, relationsData.Length);

                FileTools.WriteToBuffer(ref buffer, ref offset, Token.cxeh);
                FileTools.WriteToBuffer(ref buffer, ref offset, blockSize);
                FileTools.WriteToBuffer(ref buffer, ref offset, Count);
                FileTools.WriteToBuffer(ref buffer, ref offset, relationsData);
            }
            else
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.cxeh);
                FileTools.WriteToBuffer(ref buffer, ref offset, 0);
                FileTools.WriteToBuffer(ref buffer, ref offset, 0);
            }

            // Resize
            Array.Resize(ref buffer, offset);
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
            byte delimiter = (byte)'\t';

            byte[] csvBuffer = new byte[1024];
            int csvOffset = 0;

            int row = 0;
            int col = 0;
            

            // First dump column headers, replace the first with the table string id
            foreach (FieldInfo fieldInfo in DataType.GetFields())
            {
                FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, ((col == 0)) ?
                    FileTools.StringToASCIIByteArray(StringID) :
                    FileTools.StringToASCIIByteArray(fieldInfo.Name));

                if (!(col == noCols - 1))
                    FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, delimiter);

                col++;
            }
            // Add extra column for extended properties
            if ((ExcelMap.HasExtended))
            {
                FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, delimiter);
                FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, FileTools.StringToASCIIByteArray("ExtendedProps"));
            }
            // End of line
            FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, FileTools.StringToASCIIByteArray(Environment.NewLine));


            // Parse each row, resolve buffers if needed
            foreach (Object rowObject in Rows)
            {
                col = 0; // reset
                foreach (FieldInfo fieldInfo in DataType.GetFields())
                {
                    if (!(col == 0)) FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, delimiter);
                    col++;

                    OutputAttribute attribute = GetExcelOutputAttribute(fieldInfo);
                    if (!(attribute == null))
                    {
                        if ((attribute.IsStringOffset))
                        {
                            int offset = (int)fieldInfo.GetValue(rowObject);
                            if (!(offset == -1))
                            {
                                FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, ReadStringTableAsBytes(offset));
                            }
                            continue;
                        }
                        if ((attribute.IsIntOffset))
                        {
                            int offset = (int)fieldInfo.GetValue(rowObject);
                            if ((offset == 0))
                            {
                                FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, FileTools.StringToASCIIByteArray("0"));
                                continue;
                            }
                            int[] buffer = ReadIntegerTable(offset);
                            FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, FileTools.StringToASCIIByteArray(FileTools.ArrayToStringGeneric(buffer, ",")));
                            continue;
                        }
                        if ((attribute.IsSecondaryString))
                        {
                            int index = (int)fieldInfo.GetValue(rowObject);
                            if (!(index == -1))
                            {
                                FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, FileTools.StringToASCIIByteArray(SecondaryStrings[index]));
                            }
                            continue;
                        }
                        if ((attribute.IsBitmask))
                        {
                            uint uintValue = (uint)fieldInfo.GetValue(rowObject);
                            string stringValue = uintValue.ToString();
                            FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, FileTools.StringToASCIIByteArray(stringValue));
                            continue;
                        }
                    }
                    FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, FileTools.StringToASCIIByteArray(fieldInfo.GetValue(rowObject).ToString()));
                }
                if (ExcelMap.HasExtended)
                {
                    FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, delimiter);
                    FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, FileTools.StringToASCIIByteArray(FileTools.ByteArrayToDelimitedASCIIString(ExtendedBuffer[row], ',', typeof(byte))));
                }
                row++;
                if (!(row == noRows - 1))
                {
                    FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, FileTools.StringToASCIIByteArray(Environment.NewLine));
                }
            }

            Array.Resize(ref csvBuffer, csvOffset);
            return csvBuffer;
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
