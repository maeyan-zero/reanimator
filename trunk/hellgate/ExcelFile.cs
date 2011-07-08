using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
        private byte[] _scriptBuffer;
        private readonly byte[] _myshBuffer;
        private String[][] _csvTable;

        public Xls.TableCodes TableCode { get; set; }
        public byte[][] StatsBuffer;
        public List<String> SecondaryStrings { get; private set; }
        public byte[] ScriptBuffer { get { return _scriptBuffer; } }
        public int[] ScriptCode { get; private set; }
        public List<ExcelFunction> ExcelFunctions;
        public List<Int32[]> IndexSortArray; // is only available/set when ExcelFile.ExcelDebug = true

        private bool _hasCodeField = false;
        public readonly Dictionary<int, Object> RowFromCode = new Dictionary<int, Object>();
        public readonly Dictionary<Object, int> CodeFromRow = new Dictionary<Object, int>();

        private bool _hasFirstStringField = false;
        public readonly Dictionary<string, Object> RowFromFirstString = new Dictionary<string, Object>();
        public readonly Dictionary<Object, string> FirstStringFromRow = new Dictionary<Object, string>();

        public readonly Dictionary<Object, int> IndexFromRow = new Dictionary<Object, int>();


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

        public ExcelFile(String filePath, FileManager.ClientVersionFlags clientVersion = FileManager.ClientVersionFlags.SinglePlayer)
        {
            Thread.CurrentThread.CurrentCulture = Common.EnglishUSCulture;

            IsExcelFile = true;
            FilePath = filePath;
            StringId = _GetStringId(filePath);
            if (StringId == null) throw new Exceptions.DataFileStringIdNotFound(filePath);

            // get the excel type attributes
            DataFileAttributes dataFileAttributes = null;
            if ((clientVersion & FileManager.ClientVersionFlags.Resurrection) > 0)
            {
                DataFileMapResurrection.TryGetValue(StringId, out dataFileAttributes);
            }
            if (dataFileAttributes == null && (clientVersion & FileManager.ClientVersionFlags.TestCenter) > 0)
            {
                DataFileMapTestCenter.TryGetValue(StringId, out dataFileAttributes);
            }
            if (dataFileAttributes == null)
            {
                DataFileMap.TryGetValue(StringId, out dataFileAttributes);
            }
            Attributes = dataFileAttributes;

            // create field delegators
            FieldInfo[] dataFileFields = Attributes.RowType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            dataFileFields = dataFileFields.OrderBy(f => f.MetadataToken).ToArray(); // order by defined order - GetFields does not guarantee ordering
            Delegator = new ObjectDelegator(dataFileFields);

            // if we're empty, then just return
            if (Attributes.IsEmpty)
            {
                HasIntegrity = true;
                Rows = new List<Object>();
                return;
            }

            // we're using hard-coded mysh script table for SKILLS
            if (StringId == "SKILLS") _myshBuffer = Skills.Mysh.Data;

            // CSV file
            if (_excelFileHeader.StructureID == 0)
            {
                _excelFileHeader.StructureID = Attributes.StructureId;
            }
        }

        /// <summary>
        /// Creates a new ExcelFile object.
        /// </summary>
        /// <param name="buffer">Byte array of the given Excel file object.</param>
        /// <param name="filePath">Path to file being loaded.</param>
        /// <param name="clientVersion">The versions of excel files to try to load from.</param>
        public ExcelFile(byte[] buffer, String filePath, FileManager.ClientVersionFlags clientVersion = FileManager.ClientVersionFlags.SinglePlayer)
        {
            Thread.CurrentThread.CurrentCulture = Common.EnglishUSCulture;

            IsExcelFile = true;
            FilePath = filePath;
            StringId = _GetStringId(filePath);
            if (StringId == null) throw new Exceptions.DataFileStringIdNotFound(filePath);

            // get the excel type attributes
            DataFileAttributes dataFileAttributes = null;
            if ((clientVersion & FileManager.ClientVersionFlags.Resurrection) > 0)
            {
                DataFileMapResurrection.TryGetValue(StringId, out dataFileAttributes);
            }
            if (dataFileAttributes == null && (clientVersion & FileManager.ClientVersionFlags.TestCenter) > 0)
            {
                DataFileMapTestCenter.TryGetValue(StringId, out dataFileAttributes);
            }
            if (dataFileAttributes == null)
            {
                DataFileMap.TryGetValue(StringId, out dataFileAttributes);
            }
            Attributes = dataFileAttributes;

            // if we're empty, then just return
            if (Attributes.IsEmpty)
            {
                HasIntegrity = true;
                Rows = new List<Object>();
                return;
            }

            // create field delegators
            Debug.Assert(Attributes != null && Attributes.RowType != null);
            FieldInfo[] dataFileFields = Attributes.RowType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            dataFileFields = dataFileFields.OrderBy(f => f.MetadataToken).ToArray(); // order by defined order - GetFields does not guarantee ordering
            Delegator = new ObjectDelegator(dataFileFields);


            // parse data
            int peek = FileTools.ByteArrayToInt32(buffer, 0);
            bool isCooked = (peek == Token.cxeh);
            HasIntegrity = isCooked ? ParseData(buffer) : ParseCSV(buffer);

            // we're using hard-coded mysh script table for SKILLS
            if (HasIntegrity && StringId == "SKILLS") _myshBuffer = Skills.Mysh.Data;

            // CSV file
            if (_excelFileHeader.StructureID == 0)
            {
                _excelFileHeader.StructureID = Attributes.StructureId;
            }
        }

        /// <summary>
        /// Converts the Excel file to another Excel type, keeping all but the rows intact.
        /// Used for TCv4 -> SP conversion.
        /// </summary>
        /// <param name="excelFile">An Excel object of type to convert to.</param>
        /// <param name="newRows">The rows to replace the old.</param>
        public void ConvertType(ExcelFile excelFile, IEnumerable<Object> newRows)
        {
            Attributes = excelFile.Attributes;
            _excelFileHeader = excelFile._excelFileHeader;
            Rows = new List<Object>(newRows);
        }

        public void LoadCSV(byte[] csvBytes)
        {
            int offset = 0;
            int colCount = 1;
            while (csvBytes[offset++] != '\n') if (csvBytes[offset] == '\t') colCount++;
            _csvTable = FileTools.CSVToStringArray(csvBytes, colCount, (byte)'\t');
        }

        /// <summary>
        /// Creates a ExcelFile based on the CSV file.
        /// </summary>
        /// <param name="csvBytes">The CSV file as a byte array.</param>
        /// <returns>True if the buffer parsed okay.</returns>
        public bool ParseCSV(byte[] csvBytes)
        {
            return (csvBytes != null && ParseCSV(csvBytes, null));
        }

        public override bool ParseCSV(byte[] csvBytes, FileManager fileManager)
        {
            if (csvBytes == null) return false;
            if (fileManager != null && fileManager.DataFiles.Count == 0) fileManager = null;
            bool result = false;
            try
            {
                result = _ParseCSV(csvBytes, fileManager, null);
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e, true);
            }
            return result;
        }

        public void ParseCSV(FileManager fileManager, Dictionary<String, ExcelFile> csvExcelFiles)
        {
            Debug.Assert(fileManager != null && fileManager.DataFiles.Count != 0);
            Debug.Assert(csvExcelFiles != null && csvExcelFiles.Count != 0);
            Debug.Assert(_csvTable != null);

            _ParseCSV(null, fileManager, csvExcelFiles);
        }


        private bool _ParseCSV(byte[] csvBytes, FileManager fileManager, Dictionary<String, ExcelFile> csvExcelFiles)
        {
            // function setup
            int stringBufferOffset = 0;
            int integerBufferOffset = 1;
            bool isProperties = (StringId == "PROPERTIES" || StringId == "_TCv4_PROPERTIES");
            ObjectDelegator objectDelegator;
            OutputAttribute[] excelAttributes;
            bool needOutputAttributes = true;

            if (fileManager == null || !fileManager.DataFileDelegators.ContainsKey(StringId))
            {
                FieldInfo[] fieldInfos = DataType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                fieldInfos = fieldInfos.OrderBy(f => f.MetadataToken).ToArray(); // order by defined order - GetFields does not guarantee ordering
                objectDelegator = new ObjectDelegator(fieldInfos);
                excelAttributes = new OutputAttribute[fieldInfos.Length];
            }
            else
            {
                objectDelegator = fileManager.DataFileDelegators[StringId];
                excelAttributes = new OutputAttribute[objectDelegator.FieldCount];
            }


            String[][] tableRows;
            if (csvBytes == null)
            {
                tableRows = _csvTable;
            }
            else
            {
                // get columns
                int offset = 0;
                int colCount = 1;
                while (csvBytes[offset++] != '\n') if (csvBytes[offset] == CSVDelimiter) colCount++;
                tableRows = FileTools.CSVToStringArray(csvBytes, colCount, CSVDelimiter);
            }

            int rowCount = tableRows.Length;
            String[] columns = tableRows[0];

            if (isProperties)
            {
                ExcelFunctions = new List<ExcelFunction>();
                _scriptBuffer = new byte[1]; // properties is weird - do this just to ensure 100% byte-for-byte accuracy
            }



            // Parse the tableRows
            bool failedParsing = false;
            Rows = new List<Object>();
            for (int row = 1; row < rowCount; row++)
            {
                int col = -1;
                int csvCol = 0;
                Object rowInstance = Activator.CreateInstance(DataType);
                foreach (ObjectDelegator.FieldDelegate fieldDelegate in objectDelegator)
                {
                    col++;

                    if (needOutputAttributes) excelAttributes[col] = GetExcelAttribute(fieldDelegate.Info);
                    OutputAttribute excelAttribute = excelAttributes[col];


                    // columns not present
                    if (!columns.Contains(fieldDelegate.Name))
                    {
                        // create row header object
                        if (fieldDelegate.FieldType == typeof(RowHeader))
                        {
                            String headerString = tableRows[row][csvCol++];
                            RowHeader rowHeader = (RowHeader)FileTools.StringToObject(headerString, ",", typeof(RowHeader));
                            objectDelegator[fieldDelegate.Name, rowInstance] = rowHeader;
                            continue;
                        }

                        // assign default values
                        MarshalAsAttribute arrayMarshal = null;
                        Array arrayInstance = null;
                        if (fieldDelegate.FieldType.BaseType == typeof(Array))
                        {
                            arrayMarshal = (MarshalAsAttribute)fieldDelegate.Info.GetCustomAttributes(typeof(MarshalAsAttribute), false).First();
                            arrayInstance = (Array)Activator.CreateInstance(fieldDelegate.FieldType, arrayMarshal.SizeConst);
                            objectDelegator[fieldDelegate.Name, rowInstance] = arrayInstance;
                        }
                        else if (fieldDelegate.FieldType == typeof(String))
                        {
                            objectDelegator[fieldDelegate.Name, rowInstance] = String.Empty;
                        }

                        // assign constant non-zero values
                        if (excelAttribute == null || excelAttribute.ConstantValue == null) continue;
                        if (fieldDelegate.FieldType.BaseType == typeof(Array))
                        {
                            Debug.Assert(arrayInstance != null, "arrayInstance == null");
                            Debug.Assert(arrayMarshal != null, "arrayMarshal == null");

                            for (int i = 0; i < arrayMarshal.SizeConst; i++)
                            {
                                arrayInstance.SetValue(excelAttribute.ConstantValue, i);
                            }
                        }
                        else
                        {
                            objectDelegator[fieldDelegate.Name, rowInstance] = excelAttribute.ConstantValue;
                        }

                        continue;
                    }


                    // columns present
                    String value = tableRows[row][csvCol++];

                    if (fieldDelegate.Name == "code")
                    {
                        int code = (StringId == "REGION") ? int.Parse(value) : StringToCode(value);

                        if (fieldDelegate.FieldType == typeof(short))
                        {
                            objectDelegator[fieldDelegate.Name, rowInstance] = (short)code;
                        }
                        else
                        {
                            objectDelegator[fieldDelegate.Name, rowInstance] = code;
                        }

                        continue;
                    }

                    bool isArray = (fieldDelegate.FieldType.BaseType == typeof(Array));
                    bool isEnum = (fieldDelegate.FieldType.BaseType == typeof(Enum));
                    if (excelAttribute != null)
                    {
                        if (excelAttribute.IsTableIndex && fileManager != null)
                        {
                            int arraySize = 1;
                            if (isArray)
                            {
                                MarshalAsAttribute arrayMarshal = (MarshalAsAttribute)fieldDelegate.Info.GetCustomAttributes(typeof(MarshalAsAttribute), false).First();
                                arraySize = arrayMarshal.SizeConst;
                                Debug.Assert(arraySize > 0);
                            }

                            String[] indexStrs = value.Split(new[] { ',' });
                            Int32[] rowIndexValues = new int[arraySize];
                            for (int i = 0; i < arraySize; i++) rowIndexValues[i] = -1;

                            int maxElements = indexStrs.Length;
                            if (maxElements > arraySize)
                            {
                                Debug.WriteLine(String.Format("{0}: Loss of array elements detected. row = {1}, col = {2}.", StringId, row, col));
                                maxElements = arraySize;
                            }

                            for (int i = 0; i < maxElements; i++)
                            {
                                value = indexStrs[i];
                                if (value == "-1") continue;


                                String tableStringId = excelAttribute.TableStringId;
                                bool hasCodeColumn = fileManager.DataTableHasColumn(tableStringId, "code");
                                if (value.Length == 0 && hasCodeColumn) continue;


                                //LEVEL references multiple blank TREASURE row index values - all appear to be empty rows though, so meh...
                                //Debug.Assert(!String.IsNullOrEmpty(value));

                                int isNegative = 1;
                                if (value.Length > 0 && value[0] == '-')
                                {
                                    isNegative = -1;
                                    value = value.Substring(1, value.Length - 1);
                                }

                                int rowIndex = -1;
                                ExcelFile relatedExcel = null;
                                if (csvExcelFiles != null && csvExcelFiles.TryGetValue(tableStringId, out relatedExcel))
                                {
                                    rowIndex = relatedExcel._GetRowIndexFromValue(value, hasCodeColumn ? "code" : null);
                                }

                                if (relatedExcel == null)
                                {
                                    if (hasCodeColumn && value.Length <= 4)
                                    {
                                        int code = StringToCode(value);
                                        rowIndex = fileManager.GetExcelRowIndexFromStringId(tableStringId, code, "code");
                                    }
                                    else
                                    {
                                        rowIndex = fileManager.GetExcelRowIndex(tableStringId, value);
                                    }
                                }

                                rowIndexValues[i] = rowIndex * isNegative;
                            }

                            if (isArray)
                            {
                                objectDelegator[fieldDelegate.Name, rowInstance] = rowIndexValues;
                            }
                            else
                            {
                                objectDelegator[fieldDelegate.Name, rowInstance] = rowIndexValues[0];
                            }

                            continue;
                        }

                        if (excelAttribute.IsStringOffset)
                        {
                            if (_stringBuffer == null) _stringBuffer = new byte[1024];

                            if (String.IsNullOrEmpty(value))
                            {
                                objectDelegator[fieldDelegate.Name, rowInstance] = -1;
                                continue;
                            }

                            objectDelegator[fieldDelegate.Name, rowInstance] = stringBufferOffset;
                            FileTools.WriteToBuffer(ref _stringBuffer, ref stringBufferOffset, FileTools.StringToASCIIByteArray(value));
                            stringBufferOffset++; // \0
                            continue;
                        }

                        if (excelAttribute.IsScript)
                        {
                            if ((fileManager == null && value == "0") || value == "")
                            {
                                objectDelegator[fieldDelegate.Name, rowInstance] = 0;
                                continue;
                            }
                            if (_scriptBuffer == null)
                            {
                                _scriptBuffer = new byte[1024];
                                _scriptBuffer[0] = 0x00;
                            }

                            int[] scriptByteCode;
                            if (fileManager != null)
                            {
                                ExcelScript excelScript = new ExcelScript(fileManager);
                                scriptByteCode = excelScript.Compile(value, null, StringId, row, col, fieldDelegate.Name);
                            }
                            else
                            {
                                string[] splitValue = value.Split(',');
                                int count = splitValue.Length;
                                scriptByteCode = new int[count];
                                for (int i = 0; i < count; i++)
                                {
                                    scriptByteCode[i] = int.Parse(splitValue[i]);
                                }
                            }

                            objectDelegator[fieldDelegate.Name, rowInstance] = integerBufferOffset;
                            FileTools.WriteToBuffer(ref _scriptBuffer, ref integerBufferOffset, scriptByteCode.ToByteArray());
                            continue;
                        }

                        if (excelAttribute.IsSecondaryString)
                        {
                            if (SecondaryStrings == null) SecondaryStrings = new List<String>();

                            if (value == "")
                            {
                                objectDelegator[fieldDelegate.Name, rowInstance] = -1;
                                continue;
                            }
                            if (!SecondaryStrings.Contains(value))
                            {
                                SecondaryStrings.Add(value);
                            }

                            objectDelegator[fieldDelegate.Name, rowInstance] = SecondaryStrings.IndexOf(value);
                            continue;
                        }

                        if (excelAttribute.IsBitmask)
                        {
                            Object enumVal;
                            try
                            {
                                enumVal = Enum.Parse(fieldDelegate.FieldType, value);
                            }
                            catch (Exception)
                            {
                                String[] enumStrings = value.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                String[] enumNames = Enum.GetNames(fieldDelegate.FieldType);
                                String enumString = String.Empty;

                                String enumSeperator = String.Empty;
                                foreach (String enumStr in enumStrings)
                                {
                                    if (!enumNames.Contains(enumStr))
                                    {
                                        Debug.WriteLine(String.Format("{0}: bitfield name '{1}' not found.", StringId, enumStr));
                                        continue;
                                    }

                                    enumString += enumSeperator + enumStr;
                                    enumSeperator = ",";
                                }

                                enumVal = enumString == "" ? 0 : Enum.Parse(fieldDelegate.FieldType, enumString);
                            }

                            objectDelegator[fieldDelegate.Name, rowInstance] = (uint)enumVal;
                            continue;
                        }
                    }

                    try
                    {
                        Object objValue = null;
                        if (isArray)
                        {
                            Debug.Assert(fieldDelegate.FieldType == typeof (Int32[]));

                            objValue = FileTools.StringToArray<Int32>(value, ",");
                        }
                        else if (isEnum)
                        {
                            object enumVal = Enum.Parse(fieldDelegate.FieldType, value);
                            objectDelegator[fieldDelegate.Name, rowInstance] = (uint)enumVal;
                        }
                        else
                        {
                            objValue = FileTools.StringToObject(value, fieldDelegate.FieldType);
                            objectDelegator[fieldDelegate.Name, rowInstance] = objValue;
                        }   
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

                // applicable only for Unit type; items, missiles, monsters, objects, players
                if (Attributes.HasStats)
                {
                    if (StatsBuffer == null) StatsBuffer = new byte[rowCount - 1][];

                    String value = tableRows[row][csvCol];
                    String[] stringArray = value.Split(',');
                    byte[] byteArray = new byte[stringArray.Length];

                    for (int i = 0; i < byteArray.Length; i++)
                    {
                        byteArray[i] = Byte.Parse(stringArray[i]);
                    }
                    StatsBuffer[row - 1] = byteArray;
                }

                // properties has extra Scripts stuffs
                // yea, this is a bit messy, but it's a single table only and mostly done out of curiosity
                if (isProperties)
                {
                    String value = tableRows[row][csvCol];
                    String[] scripts = value.Split('\n');
                    ExcelFunction excelScript = new ExcelFunction();
                    if (scripts.Length > 1)
                    {
                        ExcelFunctions.Add(excelScript);
                    }

                    int i = 0;
                    do
                    {
                        if (scripts.Length == 1) continue;

                        i++;
                        String[] values = scripts[i].Split(',');
                        if (values.Length < 4) continue;


                        // script parameters
                        int typeValuesCount = values.Length - 3;
                        ExcelFunction.Parameter parameter = new ExcelFunction.Parameter
                        {
                            Name = values[0],
                            Unknown = UInt32.Parse(values[1]),
                            TypeId = UInt32.Parse(values[2]),
                            TypeValues = new int[typeValuesCount]
                        };

                        for (int j = 0; j < typeValuesCount; j++)
                        {
                            parameter.TypeValues[j] = Int32.Parse(values[3 + j]);
                        }

                        excelScript.Parameters.Add(parameter);

                    } while (i < scripts.Length - 1 - 2); // -2 for: last line is blank, and line before *might* be script values


                    // last line will be script values if it exists
                    if (i < scripts.Length - 2)
                    {
                        String[] values = scripts[++i].Split(',');
                        int[] scriptValues = new int[values.Length];

                        for (int j = 0; j < values.Length; j++)
                        {
                            scriptValues[j] = Int32.Parse(values[j]);
                        }

                        excelScript.ScriptByteCode = scriptValues.ToByteArray();
                    }
                }

                Rows.Add(rowInstance);
            }

            // resize the integer and string buffers if they were used
            if (_stringBuffer != null) Array.Resize(ref _stringBuffer, stringBufferOffset);
            if (_scriptBuffer != null) Array.Resize(ref _scriptBuffer, integerBufferOffset);

            return HasIntegrity = true;
        }

        /// <summary>
        /// Creates a ExcelFile based on the serialized data source.
        /// [Reading process updated to match how client does it (can now read "empty" files)]
        /// </summary>
        /// <param name="buffer">The serialized excel file as a byte array.</param>
        /// <returns>True if the buffer parsed okay.</returns>
        public override sealed bool ParseData(byte[] buffer)
        {
            if (buffer == null) return false;
            if (buffer.Length == 0) return false;
            int offset = 0;

            // File Header
            if (!_CheckToken(buffer, ref offset, Token.cxeh)) return false;
            _excelFileHeader = FileTools.ByteArrayToStructure<ExcelHeader>(buffer, ref offset);

            // Strings Block
            if (!_CheckToken(buffer, ref offset, Token.cxeh)) return false;
            int stringBufferOffset = FileTools.ByteArrayToInt32(buffer, ref offset);
            if (stringBufferOffset != 0)
            {
                _stringBuffer = new byte[stringBufferOffset];
                Buffer.BlockCopy(buffer, offset, _stringBuffer, 0, stringBufferOffset);
                offset += stringBufferOffset;
            }

            // Dataset Block
            if (!_CheckToken(buffer, ref offset, Token.cxeh)) return false;
            int rowCount = FileTools.ByteArrayToInt32(buffer, ref offset);
            Rows = new List<Object>(rowCount);
            for (int i = 0; i < rowCount; i++)
            {
                Rows.Add(FileTools.ByteArrayToStructure(buffer, DataType, ref offset));
                IndexFromRow.Add(Rows[i], i);

                // Add to RowFromCode Dictionary if Row has "code" column
                FieldInfo field = Attributes.RowType.GetField("code");
                if (field != null)
                {
                    int rowCode;
                    if (field.FieldType == typeof(Int16))
                    {
                        rowCode = (Int16) field.GetValue(Rows[i]);
                    }
                    else if (field.FieldType == typeof(Int32))
                    {
                        rowCode = (Int32) field.GetValue(Rows[i]);
                    }
                    else
                    {
                        rowCode = -1;
                    }

                    if (!RowFromCode.ContainsKey(rowCode))
                    {
                        RowFromCode.Add(rowCode, Rows[i]);
                        CodeFromRow.Add(Rows[i], rowCode);
                        _hasCodeField = true;
                    }
                }

                // Add to RowFromFirstString Dictionary if Row has column with attribute SortColumnOrder = 1 
                FieldInfo[] fields = Attributes.RowType.GetFields();
                for (int fieldIndex = 0; fieldIndex < fields.Count(); fieldIndex++)
                {
                    field = fields[fieldIndex];
                    OutputAttribute attribute = GetExcelAttribute(field);

                    // Check if field has attributes
                    if (attribute != null)
                    {
                        if (attribute.SortColumnOrder == 1) break;
                    }
                    else // No attribute, skip field
                    {
                        field = null;
                    }
                }
                if (field == null) continue;

                int nameOffset;
                String nameString = "";

                // Gets string from string buffer
                if (field.FieldType == typeof(Int32))
                {
                    nameOffset = (Int32)field.GetValue(Rows[i]); // No string buffer, just use "value" for string // todo: GetValue is *very* slow - change to delegates
                    nameString = ReadStringTable(nameOffset) ?? nameOffset.ToString();
                }
                else if (field.FieldType == typeof(String))
                {
                    nameString = (String)field.GetValue(Rows[i]);
                }

                if (!RowFromFirstString.ContainsKey(nameString))
                {
                    RowFromFirstString.Add(nameString, Rows[i]);
                    FirstStringFromRow.Add(Rows[i], nameString);
                    _hasFirstStringField = true;
                }
            }

            if (rowCount > 0)
            {
                // primary index
                if (!_CheckToken(buffer, ref offset, Token.cxeh)) return false;
                if (Attributes.HasStats) // items, objects, missles, players
                {
                    StatsBuffer = new byte[Count][];
                    for (int i = 0; i < Count; i++)
                    {
                        offset += sizeof (int); // Skip the indice

                        int size = FileTools.ByteArrayToInt32(buffer, ref offset);
                        StatsBuffer[i] = new byte[size];

                        Buffer.BlockCopy(buffer, offset, StatsBuffer[i], 0, size);
                        offset += size;
                    }
                }
                else
                {
                    offset += (Count*sizeof (int)); // do not allocate this array
                }

                // secondary strings
                if (!_CheckToken(buffer, offset, Token.cxeh))
                {
                    int stringCount = FileTools.ByteArrayToInt32(buffer, ref offset);
                    if (stringCount != 0) SecondaryStrings = new List<String>();
                    for (int i = 0; i < stringCount; i++)
                    {
                        int charCount = FileTools.ByteArrayToInt32(buffer, ref offset);
                        SecondaryStrings.Add(FileTools.ByteArrayToStringASCII(buffer, offset));
                        offset += charCount;
                    }
                }
            }

            // sort indices
            for (int i = 0; i < 4; i++)
            {
                if (!(_CheckToken(buffer, ref offset, Token.cxeh))) return false;
                int count = FileTools.ByteArrayToInt32(buffer, ref offset);

                if (EnableDebug)
                {
                    if (IndexSortArray == null) IndexSortArray = new List<Int32[]>();
                    IndexSortArray.Add(FileTools.ByteArrayToInt32Array(buffer, ref offset, count));
                }
                else
                {
                    offset += (count * sizeof(int)); // do not allocate
                }
            }

            // rcsh, tysh, mysh, dneh, and scripts blocks
            if (!_CheckToken(buffer, ref offset, Token.cxeh)) return false;
            if (_CheckToken(buffer, ref offset, Token.rcsh))
            {
                if (!_CheckToken(buffer, ref offset, Token.RcshValue)) return false;

                if (!_CheckToken(buffer, ref offset, Token.tysh)) return false;
                if (!_CheckToken(buffer, ref offset, Token.TyshValue)) return false;

                if (Attributes.HasScriptTable && !_ParsePropertiesScriptTable(buffer, ref offset)) return false;

                if (!_CheckToken(buffer, ref offset, Token.dneh)) return false;
                if (!_CheckToken(buffer, ref offset, Token.DnehValue)) return false;

                if (!_CheckToken(buffer, ref offset, Token.cxeh)) return false;
                int scriptsByteCount = FileTools.ByteArrayToInt32(buffer, ref offset);
                if (scriptsByteCount != 0)
                {
                    _scriptBuffer = new byte[scriptsByteCount];
                    Buffer.BlockCopy(buffer, offset, _scriptBuffer, 0, scriptsByteCount);
                    offset += scriptsByteCount;

                    int intCount = scriptsByteCount - 1;
                    Debug.Assert(intCount % 4 == 0);
                    intCount /= 4;

                    ScriptCode = FileTools.ByteArrayToInt32Array(_scriptBuffer, 1, intCount);
                }
            }

            // final data block
            if (!_CheckToken(buffer, ref offset, Token.cxeh)) return false;

            int byteCount = FileTools.ByteArrayToInt32(buffer, ref offset);
            int blockCount = FileTools.ByteArrayToInt32(buffer, ref offset);
            if (byteCount != 0)
            {
                byteCount = byteCount << 2;
                offset += (byteCount * blockCount); // do not allocate
            }

            return HasIntegrity = (offset == buffer.Length);
        }

        /// <summary>
        /// Creates a ExcelFile based on the DataTable data.
        /// </summary>
        /// <param name="dataTable">The DataTable to read the data from.</param>
        /// <returns>True if the DataTable parsed okay.</returns>
        public override bool ParseDataTable(DataTable dataTable, FileManager fileManager)
        {
            if (dataTable == null) throw new ArgumentNullException("dataTable");
            //if (fileManager == null) throw new ArgumentNullException("fileManager");

            byte[] newStringBuffer = null;
            int newStringBufferOffset = 0;
            byte[] newIntegerBuffer = null;
            int newIntegerBufferOffset = 1;
            byte[][] newStatsBuffer = null;
            List<String> newSecondaryStrings = null;
            List<Object> newTable = new List<object>();

            bool failedParsing = false;
            const BindingFlags bindingFlags = (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            FieldInfo[] dataFields = DataType.GetFields(bindingFlags);
            OutputAttribute[] excelAttributes;
            ObjectDelegator objectDelegator;

            if (fileManager == null || !fileManager.DataFileDelegators.ContainsKey(StringId))
            {
                FieldInfo[] fieldInfos = DataType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                objectDelegator = new ObjectDelegator(fieldInfos);
                excelAttributes = new OutputAttribute[fieldInfos.Length];
            }
            else
            {
                objectDelegator = fileManager.DataFileDelegators[StringId];
                excelAttributes = new OutputAttribute[objectDelegator.FieldCount];
            }

            for (int row = 0; row < dataTable.Rows.Count; row++)
            {
                int col = 1; // Skip the indice column (column 0)
                Object rowInstance = Activator.CreateInstance(DataType);
                foreach (ObjectDelegator.FieldDelegate fieldInfo in objectDelegator)
                {
                    // Initialize private fields 
                    if ((fieldInfo.IsPrivate))
                    {
                        if ((fieldInfo.FieldType == typeof(RowHeader)))
                        {
                            string headerString = (string)dataTable.Rows[row][col++];
                            RowHeader tableHeader = (RowHeader)FileTools.StringToObject(headerString, ",", typeof(RowHeader));
                            fieldInfo.SetValue(rowInstance, tableHeader);
                            continue;
                        }
                        if ((fieldInfo.FieldType.BaseType == typeof(Array)))
                        {
                            MarshalAsAttribute marshal = (MarshalAsAttribute)fieldInfo.Info.GetCustomAttributes(typeof(MarshalAsAttribute), false).First();
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
                    Object value = dataTable.Rows[row][col++];
                    OutputAttribute attribute = GetExcelAttribute(fieldInfo.Info);
                    bool isArray = (fieldInfo.FieldType.BaseType == typeof(Array));
                    if (attribute != null)
                    {
                        if (attribute.IsTableIndex)
                        {
                            int arraySize = 1;
                            if (isArray)
                            {
                                MarshalAsAttribute arrayMarshal = (MarshalAsAttribute)fieldInfo.Info.GetCustomAttributes(typeof(MarshalAsAttribute), false).First();
                                arraySize = arrayMarshal.SizeConst;
                                Debug.Assert(arraySize > 0);
                            }

                            string strValue = value.ToString();

                            String[] indexStrs = strValue.Split(new[] { ',' });
                            Int32[] rowIndexValues = new int[arraySize];
                            for (int i = 0; i < arraySize; i++) rowIndexValues[i] = -1;

                            int maxElements = indexStrs.Length;
                            if (maxElements > arraySize)
                            {
                                Debug.WriteLine(String.Format("{0}: Loss of array elements detected. row = {1}, col = {2}.", StringId, row, col));
                                maxElements = arraySize;
                            }

                            for (int i = 0; i < maxElements; i++)
                            {
                                strValue = indexStrs[i];
                                if (strValue == "-1") continue;


                                String tableStringId = attribute.TableStringId;
                                bool hasCodeColumn = fileManager.DataTableHasColumn(tableStringId, "code");
                                if (strValue.Length == 0 && hasCodeColumn) continue;


                                //LEVEL references multiple blank TREASURE row index values - all appear to be empty rows though, so meh...
                                //Debug.Assert(!String.IsNullOrEmpty(value));

                                int isNegative = 1;
                                if (strValue.Length > 0 && strValue[0] == '-')
                                {
                                    isNegative = -1;
                                    strValue = strValue.Substring(1, strValue.Length - 1);
                                }

                                int rowIndex = -1;
                                DataFile relatedDataFile = null;
                                ExcelFile relatedExcel = null;
                                if (fileManager.DataFiles.TryGetValue(tableStringId, out relatedDataFile))
                                {
                                    relatedExcel = relatedDataFile as ExcelFile;
                                    rowIndex = relatedExcel._GetRowIndexFromValue(strValue, hasCodeColumn ? "code" : null);
                                }

                                if (relatedExcel == null)
                                {
                                    if (hasCodeColumn && strValue.Length <= 4)
                                    {
                                        int code = StringToCode(strValue);
                                        rowIndex = fileManager.GetExcelRowIndexFromStringId(tableStringId, code, "code");
                                    }
                                    else
                                    {
                                        rowIndex = fileManager.GetExcelRowIndex(tableStringId, strValue);
                                    }
                                }

                                rowIndexValues[i] = rowIndex * isNegative;
                            }

                            if (isArray)
                            {
                                objectDelegator[fieldInfo.Name, rowInstance] = rowIndexValues;
                            }
                            else
                            {
                                objectDelegator[fieldInfo.Name, rowInstance] = rowIndexValues[0];
                            }

                            col++; // Skip lookup
                            continue;
                        }

                        if (attribute.IsStringIndex)
                        {
                            fieldInfo.SetValue(rowInstance, value);
                            col++; // Skip lookup
                            continue;
                        }

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

                        if ((attribute.IsScript))
                        {
                            String strValue = value as string;

                            if (strValue == null || (fileManager == null && strValue == "0") || strValue == "")
                            {
                                objectDelegator[fieldInfo.Name, rowInstance] = 0;
                                continue;
                            }
                            if (newIntegerBuffer == null)
                            {
                                newIntegerBuffer = new byte[1024];
                                newIntegerBuffer[0] = 0x00;
                            }

                            int[] scriptByteCode;
                            if (fileManager != null)
                            {
                                ExcelScript excelScript = new ExcelScript(fileManager);
                                scriptByteCode = excelScript.Compile(strValue);
                            }
                            else
                            {
                                string[] splitValue = strValue.Split(',');
                                int count = splitValue.Length;
                                scriptByteCode = new int[count];
                                for (int i = 0; i < count; i++)
                                {
                                    scriptByteCode[i] = int.Parse(splitValue[i]);
                                }
                            }

                            objectDelegator[fieldInfo.Name, rowInstance] = newIntegerBufferOffset;
                            FileTools.WriteToBuffer(ref newIntegerBuffer, ref newIntegerBufferOffset, scriptByteCode.ToByteArray());
                            continue;
                        }

                        if (attribute.IsSecondaryString)
                        {
                            if (newSecondaryStrings == null) newSecondaryStrings = new List<String>();

                            String strValue = value as String;
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
                if (Attributes.HasStats)
                {
                    if (newStatsBuffer == null)
                    {
                        newStatsBuffer = new byte[dataTable.Rows.Count][];
                    }
                    const char split = ',';
                    string value = dataTable.Rows[row][col] as string;
                    if (String.IsNullOrEmpty(value))
                    {
                        Console.WriteLine("Error parsing stats string.");
                        return false;
                    }
                    string[] stringArray = value.Split(split);
                    byte[] byteArray = new byte[stringArray.Length];
                    for (int i = 0; i < byteArray.Length; i++)
                    {
                        byteArray[i] = Byte.Parse(stringArray[i]);
                    }
                    newStatsBuffer[row] = byteArray;
                }

                newTable.Add(rowInstance);
            }

            // Parsing Complete, assign new references. These arn't assigned before now incase of a parsing error.
            Rows = newTable;
            _stringBuffer = newStringBuffer;
            _scriptBuffer = newIntegerBuffer;
            StatsBuffer = newStatsBuffer;
            SecondaryStrings = newSecondaryStrings;

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
                if (!Attributes.HasStats) continue;

                FileTools.WriteToBuffer(ref buffer, ref offset, StatsBuffer[i].Length);
                FileTools.WriteToBuffer(ref buffer, ref offset, StatsBuffer[i]);
            }


            // Secondary Strings
            if (SecondaryStrings != null)
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, SecondaryStrings.Count);
                foreach (string str in SecondaryStrings)
                {
                    FileTools.WriteToBuffer(ref buffer, ref offset, str.Length + 1); // +1 for \0
                    FileTools.WriteToBuffer(ref buffer, ref offset, FileTools.StringToASCIIByteArray(str));
                    offset++; // \0
                }
            }


            // Generate custom sorts
            IEnumerable<int[]> customSorts = _GenerateSortedIndexArrays();
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

            // rcsh, tysh, mysh, dneh
            // This section exists when there is a string or integer block or a mysh table
            if (_scriptBuffer != null || Attributes.HasScriptTable)
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.cxeh);
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.rcsh);
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.RcshValue);
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.tysh);
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.TyshValue);
                if (Attributes.HasScriptTable)
                {
                    if (_myshBuffer != null)
                    {
                        FileTools.WriteToBuffer(ref buffer, ref offset, Token.mysh);
                        FileTools.WriteToBuffer(ref buffer, ref offset, _myshBuffer);
                    }
                    else if (ExcelFunctions != null)
                    {
                        _PropertiesScriptToByteArray(ref buffer, ref offset);
                    }
                }
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.dneh);
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.DnehValue);
            }

            // add scripts
            //if (Scripts != null && Scripts.Count > 0)
            //{
            //    Scripts.
            //    foreach (int[] script in Scripts.C)
            //}
            if (_scriptBuffer != null && _scriptBuffer.Length > 0)
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.cxeh);
                FileTools.WriteToBuffer(ref buffer, ref offset, _scriptBuffer.Length);
                FileTools.WriteToBuffer(ref buffer, ref offset, _scriptBuffer);
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
                UInt32[,] indexBitRelations = _CreateIndexBitRelations();
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
        public override byte[] ExportCSV(FileManager fileManager)
        {
            return ExportCSV(fileManager);
        }

        public byte[] ExportCSV(FileManager fileManager, IEnumerable<String> columnNames = null)
        {
            if (Attributes.IsEmpty) return new byte[0];

            //// init stuffs
            byte[] csvBuffer = new byte[1024];
            int csvOffset = 0;
            bool isProperties = (StringId == "PROPERTIES" || StringId == "_TCv4_PROPERTIES");

            ObjectDelegator objectDelegator;
            if (fileManager == null || !fileManager.DataFileDelegators.ContainsKey(StringId))
            {
                objectDelegator = new ObjectDelegator(Attributes.RowType.GetFields());

                FieldInfo headerField = DataType.GetField("header", BindingFlags.Instance | BindingFlags.NonPublic);
                objectDelegator.AddField(headerField);
            }
            else
            {
                objectDelegator = fileManager.DataFileDelegators[StringId];
            }


            //// header row
            List<String> columnsList = new List<String> { StringId };
            foreach (ObjectDelegator.FieldDelegate fieldDelegate in objectDelegator)
            {
                if (columnNames == null)
                {
                    if (!fieldDelegate.IsPublic) continue;
                }
                else if (!columnNames.Contains(fieldDelegate.Name))
                {
                    continue;
                }

                columnsList.Add(fieldDelegate.Name);
            }

            // excel table type-specific columns
            if (Attributes.HasStats) columnsList.Add("Stats");
            if (isProperties) columnsList.Add("Script");

            // column header row
            String[] columns = columnsList.ToArray();
            int colCount = columns.Length;
            int rowCount = Count + 1; // +1 for column headers


            //// csv generation
            String[][] strings = new string[rowCount][];
            strings[0] = columns;
            int col = -1;
            foreach (ObjectDelegator.FieldDelegate fieldDelegate in objectDelegator)
            {
                if (!columns.Contains(fieldDelegate.Name) && fieldDelegate.Name != "header") continue;

                col++;
                OutputAttribute excelAttribute = GetExcelAttribute(fieldDelegate.Info);

                int row = 0;
                foreach (Object rowObject in Rows)
                {
                    String[] rowStr = strings[++row];
                    if (rowStr == null)
                    {
                        rowStr = new String[colCount];
                        strings[row] = rowStr;
                    }

                    if (fieldDelegate.Name == "header")
                    {
                        RowHeader tableHeader = (RowHeader)fieldDelegate.GetValue(rowObject);
                        rowStr[col] = FileTools.ObjectToStringGeneric(tableHeader, ",");
                        continue;
                    }

                    if (fieldDelegate.Name == "code")
                    {
                        int code;
                        if (fieldDelegate.FieldType == typeof(short))
                        {
                            code = (int)(short)fieldDelegate.GetValue(rowObject);
                        }
                        else
                        {
                            code = (int)fieldDelegate.GetValue(rowObject);
                        }

                        if (StringId == "REGION") // can't export region code values as chars due to weird chars
                        {
                            rowStr[col] = "\"" + code + "\"";
                        }
                        else
                        {
                            rowStr[col] = "\"" + CodeToString(code) + "\"";
                        }

                        continue;
                    }

                    bool isArray = (fieldDelegate.FieldType.BaseType == typeof(Array));
                    if (excelAttribute != null)
                    {
                        if (excelAttribute.IsTableIndex && fileManager != null)
                        {
                            int[] indexValues;
                            Object indexObj = fieldDelegate.GetValue(rowObject);
                            if (isArray)
                            {
                                indexValues = (int[])indexObj;
                            }
                            else
                            {
                                indexValues = new[] { (int)fieldDelegate.GetValue(rowObject) };
                            }

                            String[] indexStrs = new String[indexValues.Length];
                            for (int i = 0; i < indexStrs.Length; i++)
                            {
                                if (indexValues[i] == -1) // empty string/no code
                                {
                                    indexStrs[i] = "-1";
                                    continue;
                                }

                                String tableStringId = excelAttribute.TableStringId;
                                String negative = String.Empty;
                                if (indexValues[i] < 0)
                                {
                                    indexValues[i] *= -1;
                                    negative = "-";
                                }

                                String indexStr = null;
                                if (fileManager.DataTableHasColumn(tableStringId, "code"))
                                {
                                    int code = fileManager.GetExcelIntFromStringId(tableStringId, "code", indexValues[i]);
                                    if (code != 0) indexStr = CodeToString(code);
                                }

                                if (indexStr == null)
                                {
                                    indexStr = fileManager.GetExcelRowStringFromStringId(tableStringId, indexValues[i]);
                                }

                                indexStrs[i] = negative + indexStr;
                            }

                            rowStr[col] = "\"" + String.Join(",", indexStrs) + "\"";
                            continue;
                        }

                        if (excelAttribute.IsStringOffset)
                        {
                            int offset = (int)fieldDelegate.GetValue(rowObject);
                            if (offset != -1)
                            {
                                rowStr[col] = ReadStringTable(offset);
                            }
                            continue;
                        }

                        if (excelAttribute.IsScript)
                        {
                            int offset = (int)fieldDelegate.GetValue(rowObject);
                            if ((offset == 0))
                            {
                                FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, FileTools.StringToASCIIByteArray("0"));
                                continue;
                            }
                            int[] buffer = ReadScriptTable(offset);

                            String scriptString = FileTools.ArrayToStringGeneric(buffer, ",");
                            if (fileManager != null)
                            {
                                try
                                {
                                    //String debugScriptString = scriptString;
                                    ExcelScript excelScript = new ExcelScript(fileManager);
                                    scriptString = "\"" + excelScript.Decompile(_scriptBuffer, offset, scriptString, StringId, row, col, fieldDelegate.Name) + "\"";

                                    //ExcelScript recompiledScript = new ExcelScript();
                                    //int[] recompiledBytes = recompiledScript.Compile(scriptString, debugScriptString, StringId, row, col, fieldInfo.Name);

                                    //if (!buffer.SequenceEqual(recompiledBytes))
                                    //{
                                    //    String recompiledBytesString = FileTools.ArrayToStringGeneric(recompiledBytes, ",");
                                    //    ExcelScript reDecompiledScript = new ExcelScript();
                                    //    String scriptString2 = excelScript.Decompile(recompiledBytes.ToByteArray(), 0, debugScriptString, StringId, row, col, fieldInfo.Name);
                                    //    int bp = 0;
                                    //}

                                    //scriptString = "\"" + scriptString + "\"";
                                }
                                catch (Exception e)
                                {
                                    Debug.WriteLine(e.ToString());
                                    scriptString = "ScriptError(" + scriptString + ")";
                                }
                            }

                            rowStr[col] = scriptString;
                            continue;
                        }

                        if (excelAttribute.IsSecondaryString)
                        {
                            int index = (int)fieldDelegate.GetValue(rowObject);
                            if (index != -1)
                            {
                                rowStr[col] = SecondaryStrings[index];
                            }
                            continue;
                        }

                        if (excelAttribute.IsBitmask)
                        {
                            rowStr[col] = "\"" + fieldDelegate.GetValue(rowObject) + "\"";
                            continue;
                        }
                    }

                    Object outValue = fieldDelegate.GetValue(rowObject);
                    if (isArray)
                    {
                        rowStr[col] = ((Array)outValue).ToString(",");
                    }
                    else if (fieldDelegate.FieldType == typeof(float))
                    {
                        rowStr[col] = ((float)outValue).ToString("r");
                    }
                    else if (fieldDelegate.FieldType.BaseType == typeof(Enum))
                    {
                        rowStr[col] = ((UInt32)outValue).ToString();
                    }
                    else
                    {
                        rowStr[col] = outValue.ToString();
                    }
                }
            }

            // stats
            if (Attributes.HasStats)
            {
                col++;
                int row = -1;
                foreach (String[] rowStr in strings)
                {
                    if (row == -1) // columns header row
                    {
                        row++;
                        continue;
                    }

                    rowStr[col] = StatsBuffer[row++].ToString(",");
                }
            }

            // properties scripts
            if (isProperties)
            {
                // not point in doing this
                //if (tableHeader.Unknown1 != 2 || scriptRow == ExcelFunctions.Count - 1) // need 1 extra row for some reason

                col++;
                int row = -1;
                foreach (String[] rowStr in strings)
                {
                    if (row == -1)
                    {
                        row++;
                        continue;
                    }

                    if (row >= ExcelFunctions.Count) break;

                    ExcelFunction excelScript = ExcelFunctions[row++];
                    String excelScriptFunction = String.Empty;
                    foreach (ExcelFunction.Parameter paramater in excelScript.Parameters)
                    {
                        excelScriptFunction += String.Format("\n{0},{1},{2},{3}", paramater.Name, paramater.Unknown, paramater.TypeId, paramater.TypeValues.ToString(","));
                    }

                    if (excelScript.ScriptByteCode != null)
                    {
                        int offset = 0;
                        excelScriptFunction += "\n" + FileTools.ByteArrayToInt32Array(excelScript.ScriptByteCode, ref offset, excelScript.ScriptByteCode.Length / 4).ToString(",") + "\n";
                    }

                    rowStr[col] = excelScriptFunction;
                }
            }


            //// join string arrays and create byte array
            String[] rows = new String[rowCount];
            col = 0;
            foreach (String[] rowStr in strings)
            {
                rows[col] = String.Join("\t", rowStr);
                col++;
            }

            String csvString = String.Join(Environment.NewLine, rows);
            return csvString.ToASCIIByteArray();
        }

        public override byte[] ExportSQL(string tablePrefix = "")
        {
            return ExportSQL(null);
        }

        public byte[] ExportSQL(FileManager fileManager, string tablePrefix = "")
        {
            string[] sqlReserved = new string[] { "group", "condition", "left", "right", "default", "key", "force", "analyze", "kill", "usage", "order" };

            StringWriter stringWriter = new StringWriter();
            string tableName = String.Format("{0}{1}", tablePrefix, StringId.ToLower());

            stringWriter.WriteLine(String.Format("CREATE TABLE {0} (", tableName));
            stringWriter.WriteLine("\tid INT NOT NULL PRIMARY KEY,");

            String columnDec = "\t{0} {1}{2}";

            int colCount = 1;


            ObjectDelegator objectDelegator;
            if (fileManager == null || !fileManager.DataFileDelegators.ContainsKey(StringId))
            {
                objectDelegator = new ObjectDelegator(Attributes.RowType.GetFields());

                FieldInfo headerField = DataType.GetField("header", BindingFlags.Instance | BindingFlags.NonPublic);
                objectDelegator.AddField(headerField);
            }
            else
            {
                objectDelegator = fileManager.DataFileDelegators[StringId];
            }

            int noColumns = objectDelegator.FieldCount - 1; // remove header

            foreach (ObjectDelegator.FieldDelegate field in objectDelegator)
            {
                if (field.Name == "header") continue; // dont want this

                String columnName = field.Name;
                String dataType = String.Empty;
                String formatted = String.Empty;

                if (sqlReserved.Where(str => str == columnName.ToLower()).Any() == true)
                {
                    columnName = "a" + columnName;
                }

                // Special types
                OutputAttribute excelOutput = GetExcelAttribute(field.Info);
                if (excelOutput != null)
                {
                    if (excelOutput.IsScript || excelOutput.IsSecondaryString || excelOutput.IsStringOffset) dataType = "TEXT";
                    else if (excelOutput.IsBitmask) dataType = "BIGINT";
                }

                if (dataType == String.Empty)
                {
                    if (field.FieldType == typeof(int)) dataType = "INT";
                    else if (field.FieldType == typeof(float)) dataType = "DECIMAL";
                    else if (field.FieldType == typeof(byte)) dataType = "TINYINT";
                    else if (field.FieldType == typeof(short)) dataType = "SMALLINT";
                    else if (field.FieldType == typeof(uint) || field.FieldType == typeof(Int64)) dataType = "BIGINT";
                    else if (field.FieldType == typeof(string))
                    {
                        MarshalAsAttribute marshalAs = (MarshalAsAttribute)field.Info.GetCustomAttributes(typeof(MarshalAsAttribute), false).First();
                        dataType = String.Format("VARCHAR({0})", marshalAs.SizeConst);
                    }
                    else if (field.FieldType.BaseType == typeof(Array)) { dataType = "TEXT"; }
                }

                formatted = String.Format(columnDec, columnName, dataType, colCount < noColumns ? "," : String.Empty);
                stringWriter.WriteLine(formatted);
                colCount++;
            }

            stringWriter.WriteLine(");");
            stringWriter.WriteLine("INSERT INTO {0}", tableName);
            stringWriter.WriteLine("VALUES");

            colCount = 1;
            int rowCount = 0;
            int noRows = Rows.Count;
            StringWriter valueString = new StringWriter();

            foreach (Object rowObject in Rows)
            {
                valueString.Write(String.Format("\t({0},", rowCount)); // write the id

                foreach (ObjectDelegator.FieldDelegate field in objectDelegator)
                {
                    if (field.Name == "header") continue; // dont want this

                    OutputAttribute excelOutput = GetExcelAttribute(field.Info);
                    Object objValue = field.GetValue(rowObject);
                    bool valueParsed = false;

                    if (excelOutput != null)
                    {
                        if (excelOutput.IsScript)
                        {
                            int[] scriptTable = ReadScriptTable((int)objValue);
                            if (scriptTable != null)
                            {
                                int offset = (int)objValue;
                                int[] scriptbuffer = ReadScriptTable(offset);

                                String scriptString = FileTools.ArrayToStringGeneric(scriptbuffer, ",");
                                try
                                {
                                    if (fileManager != null)
                                    {
                                        ExcelScript excelScript = new ExcelScript(fileManager);
                                        scriptString = excelScript.Decompile(_scriptBuffer, offset);
                                        scriptString = StringToSQLString(scriptString);
                                        scriptString = EncapsulateString(scriptString);
                                        valueString.Write(scriptString);
                                    }
                                    else
                                    {
                                        valueString.Write(scriptString);
                                    }
                                }
                                catch (Exception e)
                                {
                                    valueString.Write(EncapsulateString(scriptString));
                                    Debug.WriteLine(e.ToString());
                                    scriptString = "ScriptError(" + scriptString + ")";
                                }
                            }
                            else
                            {
                                valueString.Write("\"\"");
                            }
                            valueParsed = true;
                        }
                        else if (excelOutput.IsSecondaryString)
                        {
                            if ((int)objValue != -1)
                            {
                                string secString = ReadSecondaryStringTable((int)objValue);
                                valueString.Write(String.Format("\"{0}\"", secString));
                            }
                            else
                            {
                                valueString.Write("\"\"");
                            }
                            valueParsed = true;
                        }
                        else if (excelOutput.IsStringOffset)
                        {
                            string offString = ReadStringTable((int)objValue);
                            offString = StringToSQLString(offString);
                            offString = String.Format("\"{0}\"", offString);

                            valueString.Write(offString);
                            valueParsed = true;
                        }
                        else if (excelOutput.IsBitmask)
                        {
                            valueString.Write((uint)objValue);
                            valueParsed = true;
                        }
                    }

                    if (valueParsed == false)
                    {
                        if (field.FieldType == typeof(string))
                        {
                            string strValue = EncapsulateString(StringToSQLString((objValue.ToString())));
                            valueString.Write(strValue);
                        }
                        else if (field.FieldType.BaseType == typeof(Array))
                        {
                            string strValue = EncapsulateString(((Array)objValue).ToString(","));
                            valueString.Write(strValue);
                        }
                        else if (field.FieldType == typeof(float))
                        {
                            string strValue = ((float)objValue).ToString("r");
                            valueString.Write(strValue);
                        }
                        else
                        {
                            valueString.Write(objValue);
                        }
                    }

                    if (colCount < noColumns) valueString.Write(",");
                    else valueString.Write(")");
                    colCount++;
                }
                if (rowCount < noRows - 1) valueString.WriteLine(",");
                else valueString.WriteLine(";");
                rowCount++;
                colCount = 1;
            }

            stringWriter.Write(valueString.ToString());

            byte[] buffer = FileTools.StringToASCIIByteArray(stringWriter.ToString());
            return buffer;
        }

        #region Unused

        /// <summary>
        /// Quick and dirty function to export mysh scripts as xml.
        /// Only applicable to PROPERTIES and SKILLS tables.
        /// </summary>
        /// <returns>Byte array of XML document for writing, or null on error.</returns>
        public byte[] ExportScriptTable()
        {
            if (ExcelFunctions == null || ExcelFunctions.Count == 0) return null;

            // this functions is quick and dirty - ignore me
            XmlDocument xmlDocument = new XmlDocument();
            XmlElement mainElement = xmlDocument.CreateElement("ExcelScript");
            xmlDocument.AppendChild(mainElement);

            foreach (ExcelFunction excelScript in ExcelFunctions)
            {
                XmlElement scriptElement = xmlDocument.CreateElement("Script");
                mainElement.AppendChild(scriptElement);

                foreach (ExcelFunction.Parameter paramater in excelScript.Parameters)
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

                if (excelScript.ScriptByteCode != null)
                {
                    int intCount = excelScript.ScriptByteCode.Length / 4;
                    String text = String.Empty;
                    int offset = 0;
                    Int32[] intArray = FileTools.ByteArrayToInt32Array(excelScript.ScriptByteCode, ref offset, intCount);
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

        #endregion
    }
}