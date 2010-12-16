using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml;
using Hellgate.Excel;
using Revival.Common;
using System.Threading;

namespace Hellgate
{
    public partial class ExcelFile : DataFile
    {
        #region Members
        private byte[] _stringBuffer;
        private byte[] _integerBuffer;
        private byte[] _myshBuffer;
        private byte[][] _extendedBuffer;
        private StringCollection _secondaryStrings;
        private List<ExcelScript> _rowScripts;

        private ExcelHeader _excelFileHeader = new ExcelHeader
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
        #endregion

        /// <summary>
        /// Creates a new ExcelFile object.
        /// </summary>
        /// <param name="buffer">Byte array of the given Excel file object.</param>
        /// <param name="filePath">Path to file being loaded.</param>
        /// <param name="isTCv4">Set to true if the buffer contains TCv4 data.</param>
        public ExcelFile(byte[] buffer, String filePath, bool isTCv4 = false)
        {
            Thread.CurrentThread.CurrentCulture = Common.EnglishUSCulture;

            IsExcelFile = true;

            FilePath = filePath;
            StringId = _GetStringId(filePath, isTCv4);
            if (StringId == null) throw new Exceptions.DataFileStringIdNotFound(filePath);

            Attributes = DataFileMap[StringId];
            if (Attributes.IsEmpty)
            {
                HasIntegrity = true;
                return;
            }

            int peek = FileTools.ByteArrayToInt32(buffer, 0);
            bool isCooked = (peek == Token.cxeh);

            HasIntegrity = isCooked ? ParseData(buffer) : ParseCSV(buffer);

            if (_excelFileHeader.StructureID == 0) // CSV file
            {
                _excelFileHeader.StructureID = DataTables.Where(dt => dt.Key == StringId).FirstOrDefault().Value;
            }
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


            StringId = FileTools.ByteArrayToStringASCII(FileTools.GetDelimintedByteArray(buffer, ref offset, delimiter), 0);
            StringId = StringId.Replace("\"", "");//in case strings embedded


            // Mutate the buffer into a string array
            int colCount = Attributes.HasExtended ? DataType.GetFields().Count() + 2 : DataType.GetFields().Count() + 1;
            string[][] tableRows = FileTools.CSVToStringArray(buffer, colCount, delimiter);
            if ((tableRows == null)) return false;


            // Parse the tableRows
            bool failedParsing = false;
            Rows = new List<Object>();
            const BindingFlags bindingFlags = (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            FieldInfo[] dataFields = DataType.GetFields(bindingFlags);
            bool needOutputAttributes = true;
            OutputAttribute[] outputAttributes = new OutputAttribute[dataFields.Length];
            for (int row = 0; row < tableRows.Count(); row++)
            {
                int col = 0;
                Object rowInstance = Activator.CreateInstance(DataType);
                foreach (FieldInfo fieldInfo in dataFields)
                {
                    // Initialize private fields 
                    if ((fieldInfo.IsPrivate))
                    {
                        if ((fieldInfo.FieldType == typeof(TableHeader)))
                        {
                            string headerString = tableRows[row][col++];
                            TableHeader tableHeader = (TableHeader)FileTools.StringToObject(headerString, ",", typeof(TableHeader));
                            fieldInfo.SetValue(rowInstance, tableHeader);
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
                    if (needOutputAttributes) outputAttributes[col] = GetExcelOutputAttribute(fieldInfo);
                    OutputAttribute attribute = outputAttributes[col];
                    string value = tableRows[row][col++];
                    if (attribute != null)
                    {
                        if (attribute.IsStringOffset)
                        {
                            if (_stringBuffer == null)
                            {
                                _stringBuffer = new byte[1024];
                            }

                            if (String.IsNullOrEmpty(value))
                            {
                                fieldInfo.SetValue(rowInstance, -1);
                                continue;
                            }

                            fieldInfo.SetValue(rowInstance, stringBufferOffset);
                            FileTools.WriteToBuffer(ref _stringBuffer, ref stringBufferOffset, FileTools.StringToASCIIByteArray(value));
                            FileTools.WriteToBuffer(ref _stringBuffer, ref stringBufferOffset, (byte)0x00);
                            continue;
                        }

                        if ((attribute.IsIntOffset))
                        {
                            if ((_integerBuffer == null))
                            {
                                _integerBuffer = new byte[1024];
                                _integerBuffer[0] = 0x00;
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
                            FileTools.WriteToBuffer(ref _integerBuffer, ref integerBufferOffset, FileTools.IntArrayToByteArray(intValue));
                            continue;
                        }

                        if ((attribute.IsSecondaryString))
                        {
                            if ((_secondaryStrings == null))
                            {
                                _secondaryStrings = new StringCollection();
                            }
                            if ((String.IsNullOrEmpty(value)))
                            {
                                fieldInfo.SetValue(rowInstance, -1);
                                continue;
                            }
                            if (!(_secondaryStrings.Contains(value)))
                            {
                                _secondaryStrings.Add(value);
                            }
                            fieldInfo.SetValue(rowInstance, _secondaryStrings.IndexOf(value));
                            continue;
                        }

                        if ((attribute.IsBitmask))
                        {
                            fieldInfo.SetValue(rowInstance, UInt32.Parse(value));
                            continue;
                        }
                    }

                    try
                    {
                        Object objValue = FileTools.StringToObject(value, fieldInfo.FieldType);
                        fieldInfo.SetValue(rowInstance, objValue);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Critical Parsing Error: " + e);
                        failedParsing = true;
                        break;
                    }

                }
                if (failedParsing) break;
                needOutputAttributes = false;

                // For item types, items, missiles, monsters etc
                // This must be a hex byte delimited array
                if ((Attributes.HasExtended))
                {
                    if ((_extendedBuffer == null))
                    {
                        _extendedBuffer = new byte[tableRows.Count()][];
                    }
                    const char split = ',';
                    string value = tableRows[row][col];
                    string[] stringArray = value.Split(split);
                    byte[] byteArray = new byte[stringArray.Length];
                    for (int i = 0; i < byteArray.Length; i++)
                    {
                        byteArray[i] = Byte.Parse(stringArray[i], NumberStyles.HexNumber);
                    }
                    _extendedBuffer[row] = byteArray;
                }

                Rows.Add(rowInstance);
            }

            // resize the integer and string buffers if they were used
            if (_stringBuffer != null) Array.Resize(ref _stringBuffer, stringBufferOffset);
            if (_integerBuffer != null) Array.Resize(ref _integerBuffer, integerBufferOffset);

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
            if (!(CheckToken(buffer, ref offset, Token.cxeh))) return false;
            _excelFileHeader = FileTools.ByteArrayToStructure<ExcelHeader>(buffer, ref offset);
            //ExcelMap = GetTypeMap(StructureId);
            //if ((ExcelMap == null)) return false;

            // Strings Block
            if (!(CheckToken(buffer, ref offset, Token.cxeh))) return false;
            int stringBufferOffset = FileTools.ByteArrayToInt32(buffer, ref offset);
            if (stringBufferOffset != 0)
            {
                _stringBuffer = new byte[stringBufferOffset];
                Buffer.BlockCopy(buffer, offset, _stringBuffer, 0, stringBufferOffset);
                offset += stringBufferOffset;
            }

            // Dataset Block
            if (!(CheckToken(buffer, ref offset, Token.cxeh))) return false;
            int rowCount = FileTools.ByteArrayToInt32(buffer, ref offset);
            Rows = new List<Object>(rowCount);
            for (int i = 0; i < rowCount; i++)
            {
                Rows.Add(FileTools.ByteArrayToStructure(buffer, DataType, ref offset));
            }

            // Primary Indice Block
            if (!CheckToken(buffer, ref offset, Token.cxeh)) return false;
            if (Attributes.HasExtended) // items, objects, missles, players
            {
                _extendedBuffer = new byte[Count][];
                for (int i = 0; i < Count; i++)
                {
                    offset += sizeof(int); // Skip the indice

                    int size = FileTools.ByteArrayToInt32(buffer, ref offset);
                    _extendedBuffer[i] = new byte[size];

                    Buffer.BlockCopy(buffer, offset, _extendedBuffer[i], 0, size);
                    offset += size;
                }
            }
            else
            {
                offset += (Count * sizeof(int)); // do not allocate this array
            }

            // Secondary String Block
            if (!(CheckToken(buffer, offset, Token.cxeh)))
            {
                int stringCount = FileTools.ByteArrayToInt32(buffer, ref offset);
                if (stringCount != 0) _secondaryStrings = new StringCollection();
                for (int i = 0; i < stringCount; i++)
                {
                    int charCount = FileTools.ByteArrayToInt32(buffer, ref offset);
                    _secondaryStrings.Add(FileTools.ByteArrayToStringASCII(buffer, offset));
                    offset += charCount;
                }
            }

            // Sorted Indices
            for (int i = 0; i < 4; i++)
            {
                if (!(CheckToken(buffer, ref offset, Token.cxeh))) return false;
                int count = FileTools.ByteArrayToInt32(buffer, ref offset);
                offset += (count * sizeof(int)); // do not allocate
            }

            // rcsh, tysh, mysh, dneh blocks
            if (!CheckToken(buffer, ref offset, Token.cxeh)) return false;
            if (!CheckToken(buffer, offset, 0))
            {
                if (!CheckToken(buffer, ref offset, Token.rcsh)) return false;
                if (!CheckToken(buffer, ref offset, Token.RcshValue)) return false;

                if (!CheckToken(buffer, ref offset, Token.tysh)) return false;
                if (!CheckToken(buffer, ref offset, Token.TyshValue)) return false;

                if (Attributes.HasScriptTable && !_ParseScriptTable(buffer, ref offset)) return false;

                if (!CheckToken(buffer, ref offset, Token.dneh)) return false;
                if (!CheckToken(buffer, ref offset, Token.DnehValue)) return false;
            }


            // Integer Block
            if ((CheckToken(buffer, ref offset, Token.cxeh)))
            {
                int integerBufferOffset = FileTools.ByteArrayToInt32(buffer, ref offset);
                if (integerBufferOffset != 0)
                {
                    _integerBuffer = new byte[integerBufferOffset];
                    Buffer.BlockCopy(buffer, offset, _integerBuffer, 0, integerBufferOffset);
                    offset += integerBufferOffset;
                }
            }


            // final data block; why is this not allocated? - no need to save? automatically generated when cooked?
            // -> automatically generated via CreateIndexBitRelations() method
            if (!CheckToken(buffer, offset, 0))
            {
                if (!CheckToken(buffer, ref offset, Token.cxeh)) return false;

                int byteCount = FileTools.ByteArrayToInt32(buffer, ref offset);
                int blockCount = FileTools.ByteArrayToInt32(buffer, ref offset);
                if (byteCount != 0)
                {
                    byteCount = byteCount << 2;
                    offset += ((byteCount * blockCount)); // do not allocate
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
            if (dataTable == null) throw new ArgumentNullException();

            byte[] newStringBuffer = null;
            int newStringBufferOffset = 0;
            byte[] newIntegerBuffer = null;
            int newIntegerBufferOffset = 1;
            byte[][] newExtendedBuffer = null;
            StringCollection newSecondaryStrings = null;
            List<object> newTable = new List<object>();

            bool failedParsing = false;
            const BindingFlags bindingFlags = (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            FieldInfo[] dataFields = DataType.GetFields(bindingFlags);

            for (int row = 0; row < dataTable.Rows.Count; row++)
            {
                int col = 1; // Skip the indice column (column 0)
                Object rowInstance = Activator.CreateInstance(DataType);
                foreach (FieldInfo fieldInfo in dataFields)
                {
                    // Initialize private fields 
                    if ((fieldInfo.IsPrivate))
                    {
                        if ((fieldInfo.FieldType == typeof(TableHeader)))
                        {
                            string headerString = (string)dataTable.Rows[row][col++];
                            TableHeader tableHeader = (TableHeader)FileTools.StringToObject(headerString, ",", typeof(TableHeader));
                            fieldInfo.SetValue(rowInstance, tableHeader);
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

                    // Public fields -> these are inside the datatable
                    object value = dataTable.Rows[row][col++];
                    OutputAttribute attribute = GetExcelOutputAttribute(fieldInfo);
                    if (attribute != null)
                    {
                        if (attribute.IsStringOffset)
                        {
                            if (newStringBuffer == null)
                            {
                                newStringBuffer = new byte[1024];
                            }

                            string strValue = value as string;
                            if (strValue == null) return false;

                            if (String.IsNullOrEmpty(strValue))
                            {
                                fieldInfo.SetValue(rowInstance, -1);
                                continue;
                            }

                            fieldInfo.SetValue(rowInstance, newStringBufferOffset);
                            FileTools.WriteToBuffer(ref newStringBuffer, ref newStringBufferOffset, FileTools.StringToASCIIByteArray(strValue));
                            FileTools.WriteToBuffer(ref newStringBuffer, ref newStringBufferOffset, (byte)0x00);
                            continue;
                        }

                        if ((attribute.IsIntOffset))
                        {
                            if ((newIntegerBuffer == null))
                            {
                                newIntegerBuffer = new byte[1024];
                                newIntegerBuffer[0] = 0x00;
                            }

                            string strValue = value as string;
                            if (strValue == null) return false;

                            if (strValue == "0" || String.IsNullOrEmpty(strValue))
                            {
                                fieldInfo.SetValue(rowInstance, 0);
                                continue;
                            }

                            strValue = strValue.Replace("\"", "");
                            string[] splitValue = strValue.Split(',');
                            int count = splitValue.Length;
                            int[] intValue = new int[count];
                            for (int i = 0; i < count; i++)
                            {
                                intValue[i] = int.Parse(splitValue[i]);
                            }
                            fieldInfo.SetValue(rowInstance, newIntegerBufferOffset);
                            FileTools.WriteToBuffer(ref newIntegerBuffer, ref newIntegerBufferOffset, FileTools.IntArrayToByteArray(intValue));
                            continue;
                        }

                        if ((attribute.IsSecondaryString))
                        {
                            if (newSecondaryStrings == null)
                            {
                                newSecondaryStrings = new StringCollection();
                            }

                            string strValue = value as string;
                            if (strValue == null) return false;

                            if (String.IsNullOrEmpty(strValue))
                            {
                                fieldInfo.SetValue(rowInstance, -1);
                                continue;
                            }
                            if (newSecondaryStrings.Contains(strValue) == false)
                            {
                                newSecondaryStrings.Add(strValue);
                            }
                            fieldInfo.SetValue(rowInstance, newSecondaryStrings.IndexOf(strValue));
                            continue;
                        }

                        if (attribute.IsStringIndex || attribute.IsTableIndex)
                        {
                            fieldInfo.SetValue(rowInstance, value);
                            col++; // Skip lookup
                            continue;
                        }
                    }

                    try
                    {
                        fieldInfo.SetValue(rowInstance, value);
                    }
                    catch (Exception e)
                    {
                        ExceptionLogger.LogException(e);
                        Console.WriteLine("Critical Parsing Error: " + e.Message);
                        failedParsing = true;
                        break;
                    }

                }
                if (failedParsing) break;

                // For item types, items, missiles, monsters etc
                // This must be a hex byte delimited array
                if ((Attributes.HasExtended))
                {
                    if (newExtendedBuffer == null)
                    {
                        newExtendedBuffer = new byte[dataTable.Rows.Count][];
                    }
                    const char split = ',';
                    string value = dataTable.Rows[row][col] as string;
                    if (String.IsNullOrEmpty(value))
                    {
                        Console.WriteLine("Error parsing Extended property string.");
                        return false;
                    }
                    string[] stringArray = value.Split(split);
                    byte[] byteArray = new byte[stringArray.Length];
                    for (int i = 0; i < byteArray.Length; i++)
                    {
                        byteArray[i] = Byte.Parse(stringArray[i]);
                    }
                    newExtendedBuffer[row] = byteArray;
                }

                newTable.Add(rowInstance);
            }

            // Parsing Complete, assign new references. These arn't assigned before now incase of a parsing error.
            Rows = newTable;
            _stringBuffer = newStringBuffer;
            _integerBuffer = newIntegerBuffer;
            _extendedBuffer = newExtendedBuffer;
            _secondaryStrings = newSecondaryStrings;

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
            FileTools.WriteToBuffer(ref buffer, ref offset, _excelFileHeader);


            // strings Block
            FileTools.WriteToBuffer(ref buffer, ref offset, Token.cxeh);
            if (_stringBuffer != null && _stringBuffer.Length > 1)
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, _stringBuffer.Length);
                FileTools.WriteToBuffer(ref buffer, ref offset, _stringBuffer);
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


            // primary index
            FileTools.WriteToBuffer(ref buffer, ref offset, Token.cxeh);
            for (int i = 0; i < Rows.Count; i++)
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, i);
                if (!Attributes.HasExtended) continue;

                FileTools.WriteToBuffer(ref buffer, ref offset, _extendedBuffer[i].Length);
                FileTools.WriteToBuffer(ref buffer, ref offset, _extendedBuffer[i]);
            }


            // Secondary Strings
            if (_secondaryStrings != null)
            {
                byte[] secondaryStringBuffer = new byte[1024];
                int secondaryStringBufferOffset = 0;
                foreach (string str in _secondaryStrings)
                {
                    FileTools.WriteToBuffer(ref secondaryStringBuffer, ref secondaryStringBufferOffset, str.Length + 1);
                    FileTools.WriteToBuffer(ref secondaryStringBuffer, ref secondaryStringBufferOffset, FileTools.StringToASCIIByteArray(str));
                    FileTools.WriteToBuffer(ref secondaryStringBuffer, ref secondaryStringBufferOffset, (byte)0x00);
                }
                FileTools.WriteToBuffer(ref buffer, ref offset, _secondaryStrings.Count);
                FileTools.WriteToBuffer(ref buffer, ref offset, secondaryStringBuffer, secondaryStringBufferOffset, false);
            }

            // Generate custom sorts
            int[][] customSorts = CreateSortIndices();
            foreach (int[] intArray in customSorts)
            {
                if (intArray != null)
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
            if (_integerBuffer != null || Attributes.HasScriptTable)
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.cxeh);
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.rcsh);
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.RcshValue);
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.tysh);
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.TyshValue);
                if (Attributes.HasScriptTable)
                {
                    if (_myshBuffer == null)
                    {
                        // No need to use reflection here for 1 mysh table.
                        // Would involve creating an interface etc
                        if (StringId == "SKILLS")
                        {
                            _myshBuffer = Skills.Mysh.data;
                        }
                    }
                    FileTools.WriteToBuffer(ref buffer, ref offset, Token.mysh);
                    FileTools.WriteToBuffer(ref buffer, ref offset, _myshBuffer);
                }
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.dneh);
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.DnehValue);
            }

            // Append the integer array.
            if (_integerBuffer != null && _integerBuffer.Length > 1)
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.cxeh);
                FileTools.WriteToBuffer(ref buffer, ref offset, _integerBuffer.Length);
                FileTools.WriteToBuffer(ref buffer, ref offset, _integerBuffer);
            }
            else
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.cxeh);
                FileTools.WriteToBuffer(ref buffer, ref offset, 0);
            }


            // row-row index bit relations - generated from isA0, isA1, isA2 etc
            // only applicable on the UNITTYPES and STATES tables
            if (Attributes.HasIndexBitRelations)
            {
                int blockSize = (Count >> 5) + 1; // need 1 bit for every row; 32 bits per int - blockSize = no. of Int's
                UInt32[,] indexBitRelations = CreateIndexBitRelations();
                byte[] relationsData = new byte[Count * blockSize * sizeof(UInt32)];
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
            FieldInfo[] dataTypeFields = DataType.GetFields();
            int noCols = dataTypeFields.Count();
            int noRows = Count + 1; // +1 for column headers
            const byte delimiter = (byte)'\t';

            byte[] csvBuffer = new byte[1024];
            int csvOffset = 0;
            int row = 0;


            // Table Header - put stringID in this field
            FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, FileTools.StringToASCIIByteArray(_GetStringId(FilePath)));
            FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, delimiter);
            // Public Field Headers
            foreach (FieldInfo fieldInfo in dataTypeFields)
            {
                FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, FileTools.StringToASCIIByteArray(fieldInfo.Name));
                FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, delimiter);
            }
            // Add extra column for extended properties
            if ((Attributes.HasExtended))
            {
                FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, FileTools.StringToASCIIByteArray("ExtendedProps"));
                FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, delimiter);
            }
            // End of line
            FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, FileTools.StringToASCIIByteArray(Environment.NewLine));


            // Parse each row, resolve buffers if needed
            bool needOutputAttributes = true;
            OutputAttribute[] outputAttributes = new OutputAttribute[noCols];
            foreach (Object rowObject in Rows)
            {
                // Write Table Header
                FieldInfo headerField = DataType.GetField("header", BindingFlags.Instance | BindingFlags.NonPublic);
                TableHeader tableHeader = (TableHeader)headerField.GetValue(rowObject);
                string tableHeaderString = FileTools.ObjectToStringGeneric(tableHeader, ",");
                FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, FileTools.StringToASCIIByteArray(tableHeaderString));

                int col = -1;
                foreach (FieldInfo fieldInfo in dataTypeFields)
                {
                    FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, delimiter);

                    col++;
                    if (needOutputAttributes) outputAttributes[col] = GetExcelOutputAttribute(fieldInfo);
                    OutputAttribute attribute = outputAttributes[col];

                    //if (col == 22 && row == 29)
                    //{
                    //    int bp = 0;
                    //}

                    if (attribute != null)
                    {
                        if ((attribute.IsStringOffset))
                        {
                            int offset = (int)fieldInfo.GetValue(rowObject);
                            if (offset != -1)
                            {
                                byte[] stringBytes = ReadStringTableAsBytes(offset);
                                if (stringBytes == null) continue;

                                FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, stringBytes);
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
                            if (index != -1)
                            {
                                FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, FileTools.StringToASCIIByteArray(_secondaryStrings[index]));
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
                needOutputAttributes = false;

                // Extended Buffer if applies
                if (Attributes.HasExtended)
                {
                    FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, delimiter);
                    FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, FileTools.StringToASCIIByteArray(FileTools.ByteArrayToDelimitedASCIIString(_extendedBuffer[row], ',', typeof(byte))));
                }

                row++;
                if (row != noRows - 1)
                {
                    FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, delimiter);
                    FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, FileTools.StringToASCIIByteArray(Environment.NewLine));
                }
            }

            Array.Resize(ref csvBuffer, csvOffset);
            return csvBuffer;
        }

        /// <summary>
        /// Quick and dirty function to export mysh scripts as xml.
        /// Only applicable to PROPERTIES and SKILLS tables.
        /// </summary>
        /// <returns>Byte array of XML document for writing, or null on error.</returns>
        public byte[] ExportScriptTable()
        {
            if (_rowScripts == null || _rowScripts.Count == 0) return null;

            // this functions is quick and dirty - ignore me
            XmlDocument xmlDocument = new XmlDocument();
            XmlElement mainElement = xmlDocument.CreateElement("ExcelScript");
            xmlDocument.AppendChild(mainElement);

            foreach (ExcelScript excelScript in _rowScripts)
            {
                XmlElement scriptElement = xmlDocument.CreateElement("Script");
                mainElement.AppendChild(scriptElement);

                foreach (ExcelScript.Paramater paramater in excelScript.Paramaters)
                {
                    XmlElement paramElement = xmlDocument.CreateElement("Parameter");
                    scriptElement.AppendChild(paramElement);

                    XmlElement paramName = xmlDocument.CreateElement("Name");
                    paramName.InnerText = paramater.Name;
                    paramElement.AppendChild(paramName);

                    XmlElement paramUnknown = xmlDocument.CreateElement("Unknown");
                    paramUnknown.InnerText = paramater.Unknown.ToString("X8");
                    paramElement.AppendChild(paramUnknown);

                    XmlElement paramTypeId = xmlDocument.CreateElement("TypeId");
                    paramTypeId.InnerText = paramater.TypeId.ToString();
                    paramElement.AppendChild(paramTypeId);

                    XmlElement paramTypeValues = xmlDocument.CreateElement("TypeValues");
                    paramElement.AppendChild(paramTypeValues);

                    // temp
                    String text = String.Empty;
                    for (int i = 0; i < paramater.TypeValues.Length; i++)
                    {
                        text += paramater.TypeValues[i].ToString();
                        if (i < paramater.TypeValues.Length - 1) text += ",";
                    }
                    paramTypeValues.InnerText = text;
                }

                XmlElement scriptValues = xmlDocument.CreateElement("Values");

                if (excelScript.ScriptValues != null)
                {
                    int intCount = excelScript.ScriptValues.Length / 4;
                    String text = String.Empty;
                    int offset = 0;
                    Int32[] intArray = FileTools.ByteArrayToInt32Array(excelScript.ScriptValues, ref offset, intCount);
                    for (int i = 0; i < intArray.Length; i++)
                    {
                        // testing if some of those huge numbers are actually two shorts...
                        //if (Math.Abs(intArray[i]) > 10000)
                        //{
                        //    short s1 = (short)(intArray[i] >> 16);
                        //    short s2 = (short)(intArray[i] & 0xFFFF);
                        //    text += s1 + "," + s2;
                        //    if (i < intArray.Length - 1) text += ",";
                        //}
                        //else
                        //{
                        text += intArray[i].ToString();
                        if (i < intArray.Length - 1) text += ",";
                        //}

                    }
                    scriptValues.InnerText = text;
                }
                scriptElement.AppendChild(scriptValues);
            }

            // being lazy and want as byte array for consistancy
            MemoryStream ms = new MemoryStream();
            xmlDocument.Save(ms);
            byte[] arr = ms.ToArray();
            ms.Close();

            return arr;
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

                    if (!tcDataTable.Columns.Contains(columnName)) continue;
                    if (column.DataType == tcDataTable.Columns[columnName].DataType)
                    {
                        convertedRow[columnName] = tcRow[columnName];
                        continue;
                    }
                    if (column.DataType.BaseType != typeof(Enum)) continue;

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

                spDataTable.Rows.Add(convertedRow);
            }

            return spDataTable;
        }
    }
}
