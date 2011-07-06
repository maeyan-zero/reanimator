using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Hellgate.Excel;
using Hellgate.Excel.JapaneseBeta;
using Hellgate.Excel.SinglePlayer;
using Revival.Common;

namespace Hellgate
{
    public partial class FileManager : IDisposable
    {
        [Flags]
        public enum ClientVersionFlags
        {
            SinglePlayer = (1 << 0),
            TestCenter = (1 << 1),
            Resurrection = (1 << 2)
        }

        private const String ExcelTablesStringId = "EXCELTABLES";
        private const String SkillsTableStringId = "SKILLS";
        private const String StatsTableStringId = "STATS";
        private const String StatesTableStringId = "STATES";

        public readonly Dictionary<String, ObjectDelegator> DataFileDelegators = new Dictionary<String, ObjectDelegator>();
        public readonly ClientVersionFlags ClientVersion;

        public bool HasIntegrity { get; private set; }
        public bool IsVersionTestCenter { get { return (ClientVersion & ClientVersionFlags.TestCenter) > 0; } }
        public bool IsVersionResurrection { get { return (ClientVersion & ClientVersionFlags.Resurrection) > 0; } }
        public String HellgatePath { get; private set; }
        public String HellgateDataPath { get { return Path.Combine(HellgatePath, Common.DataPath); } }
        public String HellgateDataCommonPath { get { return Path.Combine(HellgatePath, Common.DataCommonPath); } }
        public String Language { get; private set; } // determines which folder to check for the strings files
        public List<PackFile> IndexFiles { get; private set; }
        public Dictionary<ulong, PackFileEntry> FileEntries { get; private set; }
        public SortedDictionary<String, DataFile> DataFiles { get; private set; }
        public DataSet XlsDataSet { get; private set; }

        private List<Xls.TableCodes> _excelIndexToCodeList;
        private Dictionary<Xls.TableCodes, ExcelFile> _excelCodeToTable;

        /// <summary>
        /// Initialize the File Manager by the given Hellgate path.
        /// </summary>
        /// <param name="hellgatePath">Path to the Hellgate installation.</param>
        /// <param name="mpVersion">Set true to initialize only the MP files data.</param>
        public FileManager(String hellgatePath, bool mpVersion = false)
        {
            // what version are we loading from
            ClientVersion = ClientVersionFlags.SinglePlayer;
            if (mpVersion) ClientVersion |= ClientVersionFlags.TestCenter;

            String versionDat = Path.Combine(hellgatePath, "Version.dat");
            if (File.Exists(versionDat)) ClientVersion |= ClientVersionFlags.Resurrection;


            // rest of ctor
            HellgatePath = hellgatePath;
            Language = Config.StringsLanguage;
            Reload();
        }

        /// <summary>
        /// Reinitializes the FileManager. This is useful after a modification has been installed.
        /// </summary>
        public void Reload()
        {
            DataFiles = new SortedDictionary<String, DataFile>();
            IndexFiles = new List<PackFile>();
            FileEntries = new Dictionary<ulong, PackFileEntry>();
            XlsDataSet = new DataSet("xlsDataSet")
            {
                Locale = new CultureInfo("en-us", true),
                RemotingFormat = SerializationFormat.Binary
            };

            HasIntegrity = _LoadFileTable();
            _OrderSiblings();
        }

        private void _OrderSiblings()
        {
            foreach (PackFileEntry fileEntry in FileEntries.Values)
            {
                if (fileEntry.Siblings == null || fileEntry.Siblings.Count == 1) continue;

                fileEntry.Siblings = (from entry in fileEntry.Siblings
                                      orderby entry.FileTime descending
                                      select entry).ToList();
            }
        }

        /// <summary>
        /// Generates of a list of all the files inside the .idx .dat files from the Hellgate path.
        /// </summary>
        /// <returns>Result of the initialization. Occurance of an error will return false.</returns>
        private bool _LoadFileTable()
        {
            if (!Directory.Exists(HellgateDataPath))
            {
                Console.WriteLine(@"Critical Error: HellgateDataPath data\ does not exist!");
                return false;
            }

            List<String> idxPaths = new List<String>();
            string[] query = IsVersionTestCenter ? Common.MPFiles : Common.SPFiles;
            foreach (String fileQuery in query)
            {
                idxPaths.AddRange(Directory.GetFiles(HellgateDataPath, fileQuery).Where(p => p.EndsWith(IndexFile.Extension) || p.EndsWith(HellgatePackFile.Extension)));
            }
            if (idxPaths.Count == 0)
            {
                Console.WriteLine("Error: No index files found at path: " + HellgateDataPath);
                return false;
            }

            foreach (String idxPath in idxPaths)
            {
                HellgateFile hellgateFile;
                String datFullPath;
                if (idxPath.EndsWith(IndexFile.Extension))
                {
                    hellgateFile = new IndexFile(idxPath);
                    datFullPath = idxPath.Replace(IndexFile.Extension, ((IndexFile)hellgateFile).DatExtension);
                }
                else
                {
                    hellgateFile = new HellgatePackFile(idxPath);
                    datFullPath = idxPath.Replace(HellgatePackFile.Extension, ((HellgatePackFile)hellgateFile).DatExtension);
                }

                // if there is no accompanying .dat at all, then ignore .idx
                if (!File.Exists(datFullPath)) continue;


                // read in and parse index
                PackFile packFile = (PackFile)hellgateFile;
                try
                {
                    byte[] fileBytes = File.ReadAllBytes(idxPath);
                    hellgateFile.ParseFileBytes(fileBytes);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Warning: Failed to read in index file: " + idxPath);
                    Debug.WriteLine(ex);
                    continue;
                }

                if (packFile.Count == 0) continue;
                IndexFiles.Add(packFile);
                _LoadIndexFile(packFile);
            }

            return FileEntries.Count != 0;
        }

        /// <summary>
        /// Parses a single index file on the specified path. Checking for accompanying dat file and populating file index.
        /// </summary>
        /// <param name="packFile">The full path of the index file to parse.</param>
        private void _LoadIndexFile(PackFile packFile)
        {
            // loop through index files
            foreach (PackFileEntry currFileEntry in packFile.Files)
            {
                //if (currFileEntry.Name.Contains("bldg_c_station_warp_next_layout.xml.cooked") || currFileEntry.Name.Contains("sku."))
                //{
                //    int bp = 0;
                //}

                ulong pathHash = currFileEntry.PathHash;

                // have we added the file yet
                if (!FileEntries.ContainsKey(pathHash))
                {
                    FileEntries.Add(pathHash, currFileEntry);
                    continue;
                }

                // we haven't added the file, so we need to compare file times and backup states
                PackFileEntry origFileEntry = FileEntries[pathHash];

                // do backup checks first as they'll "override" the FileTime values (i.e. file not found causes game to go to older version)
                // if currFile IS a backup, and orig is NOT, then add to Siblings as game will be loading orig over "backup" anyways
                if (currFileEntry.IsPatchedOut && !origFileEntry.IsPatchedOut)
                {
                    if (origFileEntry.Siblings == null) origFileEntry.Siblings = new List<PackFileEntry>();
                    origFileEntry.Siblings.Add(currFileEntry);

                    continue;
                }

                // if curr is NOT a backup, but orig IS, then we want to update (i.e. don't care about FileTime; as above)
                // OR if orig is older than curr, we also want to update/re-arrange Siblings, etc
                if ((!currFileEntry.IsPatchedOut && origFileEntry.IsPatchedOut) ||
                    origFileEntry.FileTime < currFileEntry.FileTime)
                {
                    // set the Siblings list to the updated FileEntry and null out other
                    if (origFileEntry.Siblings != null)
                    {
                        currFileEntry.Siblings = origFileEntry.Siblings;
                        origFileEntry.Siblings = null;
                    }

                    // add the "orig" (now old) to the curr FileEntry.Siblings list
                    if (currFileEntry.Siblings == null) currFileEntry.Siblings = new List<PackFileEntry>();
                    currFileEntry.Siblings.Add(origFileEntry);
                    FileEntries[pathHash] = currFileEntry;

                    continue;
                }

                // if curr is older (or equal to; hellgate000 has duplicates) than the orig, then add this to the Siblings list (i.e. orig is newer)
                if (origFileEntry.FileTime >= currFileEntry.FileTime)
                {
                    if (origFileEntry.Siblings == null) origFileEntry.Siblings = new List<PackFileEntry>();
                    origFileEntry.Siblings.Add(currFileEntry);

                    continue;
                }

                Debug.Assert(false, "End of 'if (FileEntries.ContainsKey(hash))'", "wtf??\n\nThis shouldn't happen, please report this.");
            }
        }

        /// <summary>
        /// Loads all of the available Excel and Strings files to a hashtable.
        /// </summary>
        /// <returns>Returns true on success.</returns>
        public bool LoadTableFiles(bool ignorePatchedOut = false)
        {
            // want excel files and strings files
            foreach (PackFileEntry fileEntry in
                FileEntries.Values.Where(fileEntry => fileEntry.Name.EndsWith(ExcelFile.Extension) ||
                    (fileEntry.Name.EndsWith(StringsFile.Extention) && fileEntry.Path.Contains(Language))))
            {
                byte[] fileBytes = GetFileBytes(fileEntry, ignorePatchedOut);
                if (fileBytes == null)
                {
                    Debug.WriteLine("Warning: Failed to read file bytes in LoadTableFiles(). FileEntry = " + fileEntry.Name);
                    continue;
                }

                // parse file data
                DataFile dataFile;
                if (fileEntry.Name.EndsWith(ExcelFile.Extension))
                {
                    try
                    {
                        dataFile = new ExcelFile(fileBytes, fileEntry.Path, ClientVersion);
                    }
                    catch (Exceptions.DataFileStringIdNotFound dataFileStringIdNotFound)
                    {
                        Debug.WriteLine(dataFileStringIdNotFound.ToString());
                        ExceptionLogger.LogException(dataFileStringIdNotFound);
                        continue;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                        ExceptionLogger.LogException(ex);
                        Console.WriteLine(String.Format("Critical Error: Failed to load excel file {0} (ClientVersion = {1})", fileEntry.Name, ClientVersion));
                        continue;
                    }
                }
                else
                {
                    //if (MPVersion) continue; // todo: need to add _TCv4_ stringId keys to DataFileMap before we can load them 
                    dataFile = new StringsFile(fileBytes, fileEntry.Path);
                }

                if (!dataFile.HasIntegrity)
                {
                    Console.WriteLine("Error: Failed to load data file: " + dataFile.StringId);
                    continue;
                }
                if (dataFile.StringId == null)
                {
                    Console.WriteLine(String.Format("Error: StringId is null. {0} was not indexed.", fileEntry.Name));
                    continue;
                }

                try
                {
                    DataFiles.Add(dataFile.StringId, dataFile);
                    DataFileDelegators.Add(dataFile.StringId, dataFile.Delegator); // not sure if we need this still...
                }
                catch (Exception e)
                {
                    Console.WriteLine("Critical Error: Cannot add table data file to dictionary: " + dataFile + "\n" + e);
                }
            }

            return true;
        }

        public void ProcessTables()
        {
            // excel tables
            ExcelFile excelTables = (ExcelFile)DataFiles[ExcelTablesStringId];
            foreach (ExcelTablesRow excelTableRow in excelTables.Rows)
            {
                Xls.TableCodes tableCode = (Xls.TableCodes)excelTableRow.code;

                ExcelFile excelTable = GetExcelTableFromCode(tableCode);
                if (excelTable == null) continue;

                excelTable.TableCode = tableCode;
            }

            // stats table
            ExcelFile statsTable = (ExcelFile)DataFiles[StatsTableStringId];
            foreach (StatsRow stat in statsTable.Rows)
            {
                int bitCount = _GetTableRowBitMax(stat.valTable, false);
                if (bitCount != -1) stat.valbits = bitCount;

                bitCount = _GetTableRowBitMax(stat.param1Table, true);
                if (bitCount != -1) stat.param1Bits = bitCount;
                if (stat.param1Bits > 0) stat.ParamCount++;

                bitCount = _GetTableRowBitMax(stat.param2Table, true);
                if (bitCount != -1) stat.param2Bits = bitCount;
                if (stat.param2Bits > 0) stat.ParamCount++;

                bitCount = _GetTableRowBitMax(stat.param3Table, true);
                if (bitCount != -1) stat.param3Bits = bitCount;
                if (stat.param3Bits > 0) stat.ParamCount++;

                bitCount = _GetTableRowBitMax(stat.param4Table, true);
                if (bitCount != -1) stat.param4Bits = bitCount;
                if (stat.param4Bits > 0) stat.ParamCount++;

                if (stat.valTable != -1) stat.ValueExcelTable = GetExcelTableFromIndex(stat.valTable);
                if (stat.param1Table != -1) stat.Param1ExcelTable = GetExcelTableFromIndex(stat.param1Table);
                if (stat.param2Table != -1) stat.Param2ExcelTable = GetExcelTableFromIndex(stat.param2Table);
                if (stat.param3Table != -1) stat.Param3ExcelTable = GetExcelTableFromIndex(stat.param3Table);
                if (stat.param4Table != -1) stat.Param4ExcelTable = GetExcelTableFromIndex(stat.param4Table);
            }

            // level drlg choice table
            ExcelFile levelDrlgChoiceTable = (ExcelFile)DataFiles["LEVEL_DRLG_CHOICE"];
            foreach (LevelDrlgChoiceRow levelDrlgChoice in levelDrlgChoiceTable.Rows)
            {
                if (levelDrlgChoice.nameOffset == -1) continue;

                levelDrlgChoice.name = levelDrlgChoiceTable.ReadStringTable(levelDrlgChoice.nameOffset);
                Debug.Assert(!String.IsNullOrEmpty(levelDrlgChoice.name));
            }
        }

        private int _GetTableRowBitMax(int tableIndex, bool isParam)
        {
            if (tableIndex == -1) return -1;

            ExcelFile valTable = GetExcelTableFromIndex(tableIndex);
            if (valTable == null) return -1;

            int rowCount = valTable.Rows.Count;
            if (isParam) rowCount++;

            return FileTools.GetMaxBits(rowCount);
        }

        /// <summary>
        /// Retrieves a DataFile from the DataFiles list.
        /// </summary>
        /// <param name="stringId">The stringID of the DataFile.</param>
        /// <returns>Matching DataFile if it exists.</returns>
        public DataFile GetDataFile(String stringId)
        {
            if (DataFiles == null) return null;

            DataFile dataFile;
            return (DataFiles.TryGetValue(stringId, out dataFile)) ? dataFile : null;
        }

        /// <summary>
        /// Checks if a data table has a specified column name.
        /// </summary>
        /// <param name="stringId">The StringId of the data table to check.</param>
        /// <param name="colName">The column name to check for.</param>
        /// <returns>True if the table has the column.</returns>
        public bool DataTableHasColumn(String stringId, String colName)
        {
            if (DataFiles == null || !DataFiles.ContainsKey(stringId)) return false;

            ObjectDelegator objectDelegator = DataFileDelegators[stringId];
            return objectDelegator.ContainsGetFieldDelegate(colName);
        }

        #region "row" From First String functions
        // Row from first string
        public object GetRowFromFirstString(Xls.TableCodes tableCode, String firstString)
        {
            ExcelFile table = GetExcelTableFromCode(tableCode);
            Object row;
            return (table.RowFromFirstString.TryGetValue(firstString, out row)) ? row : null;
        }

        // Stat row
        public StatsRow GetStatRowFromName(String name)
        {
            return (StatsRow)GetRowFromFirstString(Xls.TableCodes.STATS, name);
        }

        // State row
        public StatesRow GetStateRowFromName(String name)
        {
            return (StatesRow)GetRowFromFirstString(Xls.TableCodes.STATES, name);
        }

        // Skill row
        public SkillsRow GetSkillRowFromName(String name)
        {
            return (SkillsRow)GetRowFromFirstString(Xls.TableCodes.SKILLS, name);
        }
        #endregion

        #region "row" From Index functions
        // Row from index
        public object GetRowFromIndex(Xls.TableCodes tableCode, Int32 index)
        {
            ExcelFile table = GetExcelTableFromCode(tableCode);
            return index < table.Rows.Count() ? (table.Rows[index]) : null;
        }

        // Stat row from index
        public StatsRow GetStatRowFromIndex(Int32 index)
        {
            return (StatsRow)GetRowFromIndex(Xls.TableCodes.STATS, index);
        }

        // State row from index
        public StatesRow GetStateRowFromIndex(Int32 index)
        {
            return (StatesRow)GetRowFromIndex(Xls.TableCodes.STATES, index);
        }

        // UnitData row from index
        public UnitDataRow GetUnitDataRowFromIndex(Xls.TableCodes tableCode, Int32 index)
        {
            return (UnitDataRow)GetRowFromIndex(tableCode, index);
        }
        #endregion

        #region "row" From Code functions
        // Row from code
        public object GetRowFromCode(Xls.TableCodes tableCode, Int16 code)
        {
            ExcelFile table = GetExcelTableFromCode(tableCode);
            object row;
            return (table.RowFromCode.TryGetValue(code, out row)) ? row : null;
        }

        // Stat row from code
        public StatsRow GetStatRowFromCode(Int16 code)
        {
            return (StatsRow)GetRowFromCode(Xls.TableCodes.STATS, code);
        }

        // UnitData row from code
        public UnitDataRow GetUnitDataRowFromCode(Xls.TableCodes tableCode, Int16 code)
        {
            return (UnitDataRow)GetRowFromCode(tableCode, code);
        }
        #endregion

        #region "rowIndex" From First String functions
        // Row index from first string
        public int GetRowIndexFromFirstString(Xls.TableCodes tableCode, String firstString)
        {
            ExcelFile table = GetExcelTableFromCode(tableCode);
            object row = GetRowFromFirstString(tableCode, firstString);
            if (row != null)
            {
                int index = 0;
                return (table.IndexFromRow.TryGetValue(row, out index)) ? index : -1;
            }
            return -1;
        }

        // Stat row index
        public int GetStatRowIndexFromName(String name)
        {
            return GetRowIndexFromFirstString(Xls.TableCodes.STATS, name);
        }

        // State row index
        public int GetStateRowIndexFromName(String name)
        {
            return GetRowIndexFromFirstString(Xls.TableCodes.STATES, name);
        }

        // Skill row index
        public int GetSkillRowIndexFromName(String name)
        {
            return GetRowIndexFromFirstString(Xls.TableCodes.SKILLS, name);
        }
        #endregion

        #region "rowIndex" From Row functions
        public int GetRowIndexFromRow(Xls.TableCodes tableCode, object row)
        {
            ExcelFile table = GetExcelTableFromCode(tableCode);
            if (row != null)
            {
                int index;
                return (table.IndexFromRow.TryGetValue(row, out index)) ? index : -1;
            }
            return -1;
        }

        public int GetSkillRowIndexFromRow(SkillsRow skillsRow)
        {
            return GetRowIndexFromRow(Xls.TableCodes.SKILLS, skillsRow);
        }
        #endregion

        #region "rowIndex" From Code functions
        // Row index from code
        public int GetRowIndexFromCode(Xls.TableCodes tableCode, Int16 code)
        {
            ExcelFile table = GetExcelTableFromCode(tableCode);
            Object row;
            return (table.RowFromCode.TryGetValue(code, out row)) ? table.IndexFromRow[row] : -1;
        }

        // Stat row index
        public int GetStatRowIndexFromCode(Int16 code)
        {
            return GetRowIndexFromCode(Xls.TableCodes.STATS, code);
        }

        // State row index
        public int GetStateRowIndexFromCode(Int16 code)
        {
            return GetRowIndexFromCode(Xls.TableCodes.STATES, code);
        }

        // Skill row index
        public int GetSkillRowIndexFromCode(Int16 code)
        {
            return GetRowIndexFromCode(Xls.TableCodes.SKILLS, code);
        }
        #endregion

        #region "code" From First String functions
        // code from first string
        public int GetCodeFromFirstString(Xls.TableCodes tableCode, String firstString)
        {
            ExcelFile table = GetExcelTableFromCode(tableCode);
            object row;
            if (table.RowFromFirstString.TryGetValue(firstString, out row))
            {
                int code;
                return (table.CodeFromRow.TryGetValue(row, out code)) ? code : -1;
            }
            return -1;
        }

        // Stat code
        public int GetStatCodeFromName(String name)
        {
            return GetCodeFromFirstString(Xls.TableCodes.STATS, name);
        }

        // State code
        public int GetStateCodeFromName(String name)
        {
            return GetCodeFromFirstString(Xls.TableCodes.STATES, name);
        }

        // Skill code
        public int GetSkillCodeFromName(String name)
        {
            return GetCodeFromFirstString(Xls.TableCodes.SKILLS, name);
        }
        #endregion

        #region "code" From Row functions
        public int GetCodeFromRow(Xls.TableCodes tableCode, object row)
        {
            ExcelFile table = GetExcelTableFromCode(tableCode);
            int code;
            return (table.CodeFromRow.TryGetValue(row, out code)) ? code : -1;
        }
        #endregion

        public Object GetRowFromValue(Xls.TableCodes tableCode, String colName, Object value)
        {
            if (value == null || String.IsNullOrEmpty(colName)) return null;

            ExcelFile excelTable = GetExcelTableFromCode(tableCode);
            if (excelTable == null) return null;

            ObjectDelegator tableDelegate = DataFileDelegators[excelTable.StringId];
            ObjectDelegator.FieldDelegate fieldDelegate = tableDelegate.GetFieldDelegate(colName);
            if (fieldDelegate == null) return null;

            foreach (Object row in excelTable.Rows)
            {
                if (value.Equals(fieldDelegate.GetValue(row))) return row;
            }

            return null;
        }

        /// <summary>
        /// Obtains an int value from an excel table using a StringId, column name, and row index.
        /// Returns 0x4C494146 on fail (will output as 'FAIL').
        /// </summary>
        /// <param name="stringId">An Excel Table StringId.</param>
        /// <param name="colName">The column name to check.</param>
        /// <param name="rowIndex">The row index to obtain the value from.</param>
        /// <returns></returns>
        public int GetExcelIntFromStringId(String stringId, String colName, int rowIndex)
        {
            if (DataFiles == null || !DataFiles.ContainsKey(stringId)) return 0x4C494146;

            ExcelFile excelFile = DataFiles[stringId] as ExcelFile;
            if (excelFile == null) return 0x4C494146;
            if (rowIndex < 0 || rowIndex >= excelFile.Rows.Count) return 0x4C494146;

            ObjectDelegator excelDelegator = DataFileDelegators[stringId];
            Object row = excelFile.Rows[rowIndex];

            Object value = excelDelegator[colName](row);
            if (value.GetType() == typeof(int)) return (int)value;

            return (int)(short)value;
        }

        /// <summary>
        /// Retrieves an Excel Table from its Code value. If it's not loaded, then it will be loaded and returned.
        /// </summary>
        /// <param name="code">The code of the Excel Table to retrieve.</param>
        /// <returns>The Excel Table as a DataTable, or null if not found.</returns>
        public ExcelFile GetExcelTableFromCode(Xls.TableCodes code)
        {
            if (DataFiles == null || DataFiles.ContainsKey(ExcelTablesStringId) == false || code == Xls.TableCodes.Null) return null;

            if (_excelCodeToTable == null)
            {
                ExcelFile excelTables = (ExcelFile)DataFiles[ExcelTablesStringId];
                _excelCodeToTable = new Dictionary<Xls.TableCodes, ExcelFile>();

                foreach (ExcelTablesRow excelTablesRow in excelTables.Rows)
                {
                    DataFile dataFile;
                    if (DataFiles.TryGetValue(excelTablesRow.name, out dataFile))
                    {
                        _excelCodeToTable[(Xls.TableCodes)excelTablesRow.code] = (ExcelFile)dataFile;
                    }
                }
            }

            ExcelFile excelTable;
            return _excelCodeToTable.TryGetValue(code, out excelTable) ? excelTable : null;
        }

        public int GetExcelRowIndexFromTableIndex(int tableIndex, String value)
        {
            uint tableCode = (uint)_GetTableCodeFromTableIndex(tableIndex);
            return GetExcelRowIndex(tableCode, value);
        }

        public String GetExcelRowStringFromTableIndex(int tableIndex, int rowIndex)
        {
            Xls.TableCodes tableCode = _GetTableCodeFromTableIndex(tableIndex);
            return GetExcelRowStringFromRowIndex(tableCode, rowIndex);
        }

        public ExcelFile GetExcelTableFromIndex(int tableIndex)
        {
            if (tableIndex < 0) return null;

            Xls.TableCodes tableCode = _GetTableCodeFromTableIndex(tableIndex);
            return GetExcelTableFromCode(tableCode);
        }

        private Xls.TableCodes _GetTableCodeFromTableIndex(int tableIndex)
        {
            if (tableIndex < 0) return Xls.TableCodes.Null;

            if (_excelIndexToCodeList == null)
            {
                DataFile excelTables;
                if (!DataFiles.TryGetValue(ExcelTablesStringId, out excelTables)) return 0;

                _excelIndexToCodeList = new List<Xls.TableCodes>();
                foreach (ExcelTablesRow excelTable in excelTables.Rows)
                {
                    _excelIndexToCodeList.Add((Xls.TableCodes)excelTable.code);
                }
            }

            if (tableIndex >= _excelIndexToCodeList.Count) return Xls.TableCodes.Null;
            return _excelIndexToCodeList[tableIndex];
        }

        public int GetExcelRowIndexFromStringId(String stringId, int value, String colName)
        {
            if (DataFiles == null || DataFiles[stringId] == null) return -1;

            ExcelFile excelFile = (ExcelFile)DataFiles[stringId];
            ObjectDelegator excelDelegator = DataFileDelegators[stringId];
            ObjectDelegator.FieldGetValueDelegate getValue = excelDelegator[colName];

            int rowIndex = -1;
            bool foundRow = false;
            foreach (Object row in excelFile.Rows)
            {
                rowIndex++;

                int intVal = 0;
                Object val = getValue(row);
                if (val.GetType() == typeof(Int16))
                {
                    intVal = (int)(short)val;
                }
                else
                {
                    intVal = (int)val;
                }

                if (intVal != value) continue;

                foundRow = true;
                break;
            }

            return (foundRow) ? rowIndex : -1;
        }



        public int GetExcelRowIndex(String stringId, String value)
        {
            if (DataFiles == null) return -1;

            ExcelFile excelFile = DataFiles[stringId] as ExcelFile;
            if (excelFile == null) return -1;

            return _GetExcelRowIndex(excelFile, value);
        }

        public int GetExcelRowIndex(uint code, String value)
        {
            if (DataFiles == null || DataFiles[ExcelTablesStringId] == null) return -1;

            return _GetExcelRowIndex(GetExcelTableFromCode((Xls.TableCodes)code), value);
        }

        public int _GetExcelRowIndex(ExcelFile excelTable, String value)
        {
            FieldInfo field = excelTable.Attributes.RowType.GetFields()[0];
            bool isStringField = (field.FieldType == typeof(String));

            ObjectDelegator excelDelegator = DataFileDelegators[excelTable.StringId];
            ObjectDelegator.FieldGetValueDelegate getValue = excelDelegator[field.Name];

            int i = 0;
            foreach (Object row in excelTable.Rows)
            {
                if (isStringField)
                {
                    String val = (String)getValue(row);
                    if (val == value) return i;
                }
                else // string offset
                {
                    int offset = (int)getValue(row);
                    String stringVal = excelTable.ReadStringTable(offset);
                    if (stringVal == value) return i;
                }
                i++;
            }

            return -1;
        }



        public String GetExcelRowStringFromStringId(String stringId, int rowIndex, int colIndex = 0)
        {
            if (DataFiles == null || String.IsNullOrEmpty(stringId) || DataFiles.ContainsKey(stringId) == false) return null;

            ExcelFile excelTable = DataFiles[stringId] as ExcelFile;
            if (excelTable == null) return null;

            String stringVal = _GetExcelRowStringFromExcelFile(excelTable, rowIndex, colIndex);
            return stringVal;
        }

        public String GetExcelRowStringFromRowIndex(Xls.TableCodes code, int rowIndex, int colIndex = 0)
        {
            if (DataFiles == null || DataFiles.ContainsKey(ExcelTablesStringId) == false || rowIndex < 0) return null;

            ExcelFile excelTable = GetExcelTableFromCode(code);

            String stringVal = _GetExcelRowStringFromExcelFile(excelTable, rowIndex, colIndex);
            return stringVal;
        }

        private String _GetExcelRowStringFromExcelFile(ExcelFile excelTable, int rowIndex, int colIndex)
        {
            if (excelTable == null || rowIndex >= excelTable.Rows.Count) return null;

            ObjectDelegator tableDelegator = DataFileDelegators[excelTable.StringId];
            ObjectDelegator.FieldDelegate fieldDelegate = tableDelegator.GetPublicFieldDelegate(colIndex);

            bool isStringField = (fieldDelegate.FieldType == typeof(String));
            Object row = excelTable.Rows[rowIndex];

            if (isStringField) return (String)fieldDelegate.GetValue(row);

            int offset = (int)fieldDelegate.GetValue(row);
            String stringVal = excelTable.ReadStringTable(offset);
            return stringVal;
        }

        /// <summary>
        /// Gets file byte data from most principle location; considering filetimes and backup status.
        /// The user must manually call EndAllDatAccess to close access to any opened .dat files during the process.
        /// </summary>
        /// <param name="relativeFilePath">The file path relative to HGL installation directory.</param>
        /// <param name="ignorePatchedOut">If true, will ignore the files patched out state effectivly forcing file reading from .dats as if it was never patched out.</param>
        /// <returns>The file byte array, or null on error.</returns>
        public byte[] GetFileBytes(String relativeFilePath, bool ignorePatchedOut = false)
        {
            String directoryString = Path.GetDirectoryName(relativeFilePath).ToLower() + "\\";
            String fileName = Path.GetFileName(relativeFilePath).ToLower();
            UInt64 filePathHash = Crypt.GetStringsSHA1UInt64(directoryString, fileName);

            PackFileEntry fileEntry;
            return FileEntries.TryGetValue(filePathHash, out fileEntry) ? GetFileBytes(fileEntry, ignorePatchedOut) : null;
        }

        /// <summary>
        /// Gets file byte data from most principle location; considering filetimes and backup status.
        /// The user must manually call EndAllDatAccess to close access to any opened .dat files during the process.
        /// </summary>
        /// <param name="fileEntry">The file entry details to read.</param>
        /// <param name="ignorePatchedOut">If true, will ignore the files patched out state effectivly forcing file reading from .dats as if it was never patched out.</param>
        /// <returns>The file byte array, or null on error.</returns>
        public byte[] GetFileBytes(PackFileEntry fileEntry, bool ignorePatchedOut = false)
        {
            if (fileEntry == null) return null;
            byte[] fileBytes = null;


            // if file is backed up, check for unpacked copy
            String filePath = fileEntry.Path;
            if (fileEntry.IsPatchedOut && !ignorePatchedOut)
            {
                filePath = filePath.Replace(@"backup\", "");

                String fullPath = Path.Combine(HellgatePath, filePath);
                if (File.Exists(fullPath))
                {
                    try
                    {
                        fileBytes = File.ReadAllBytes(fullPath);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Warning: Reading from Backup - Failed to read from file: " + fullPath);
                    }
                }
            }


            // if not backed up or if backed up but file not found/readable, then read from .dat
            if (fileBytes == null)
            {
                try
                {
                    fileBytes = fileEntry.Pack.GetFileBytes(fileEntry);
                }
                catch (Exception e)
                {
                    ExceptionLogger.LogException(e);
                    Console.WriteLine("Warning: Failed to read file from .dat: " + fileEntry.Name);
                    return null;
                }
            }

            return fileBytes;
        }

        /// <summary>
        /// Close any opened .dat files during file reading process'
        /// </summary>
        public void EndAllDatAccess()
        {
            // close any/all open dats
            foreach (PackFile indexFile in IndexFiles)
            {
                indexFile.EndDatAccess();
            }
        }

        /// <summary>
        /// Attempts to open all .dat files for reading.
        /// </summary>
        /// <returns>True if .dat files were opened, false upon failure.</returns>
        public void BeginAllDatReadAccess()
        {
            foreach (PackFile packFile in IndexFiles)
            {
                packFile.BeginDatReading();
            }
        }

        /// <summary>
        /// Extracts all Excel files to their \data\ locations.
        /// Primarly a debug function.
        /// </summary>
        public void ExtractAllExcel(String root = null, bool doCSVAsWell = false)
        {
            if (root == null) root = HellgatePath;

            foreach (PackFileEntry fileEntry in FileEntries.Values)
            {
                if (!fileEntry.Name.EndsWith(ExcelFile.Extension)) continue;

                PackFileEntry extractFileEntry = fileEntry;
                //if (fileEntry.Index.ToString().Contains("4580") && fileEntry.Siblings != null)
                //{
                //    extractFileEntry = (from fi in fileEntry.Siblings
                //                        where fi.Index.ToString().Contains("4256")
                //                        select fi).FirstOrDefault();

                //    if (extractFileEntry == null)
                //    {
                //        extractFileEntry = (from fi in fileEntry.Siblings
                //                            where fi.Index.ToString().Contains("000")
                //                            select fi).FirstOrDefault();
                //    }

                //    Debug.Assert(extractFileEntry != null);
                //}

                byte[] fileBytes = GetFileBytes(extractFileEntry, true);
                String filePath = Path.Combine(root, extractFileEntry.Path);
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                File.WriteAllBytes(filePath, fileBytes);


                if (!doCSVAsWell) continue;
                byte[] csvBytes;

                try
                {
                    ExcelFile excelFile = new ExcelFile(fileBytes, filePath, ClientVersion);
                    csvBytes = excelFile.ExportCSV(this);
                }
                catch (Exception)
                {
                    continue;
                }

                File.WriteAllBytes(filePath.Replace(ExcelFile.Extension, ExcelFile.ExtensionDeserialised), csvBytes);
            }
        }

        #region IDisposable Members
        /// <summary>
        /// Disposes the Excel DataSet if created.
        /// </summary>
        public void Dispose()
        {
            if (XlsDataSet != null) XlsDataSet.Dispose();
        }
        #endregion

        public string[] GetLanguages()
        {
            var stringDirs = (from PackFileEntry fileEntry in FileEntries.Values
             where fileEntry.Directory.Contains(@"\data\excel\strings\")
             select Path.GetDirectoryName(fileEntry.Directory));
            return stringDirs.ToArray();
        }


        /// <summary>
        /// Allows pre-loading of the specified table(s) to decrease loading times during execution.
        /// If the table is already loaded nothing will happen.
        /// </summary>
        /// <param name="tableIds">A list of tables that should be pre-loaded</param>
        public void PreLoadTable(List<string> tableIds)
        {
            foreach (string tableId in tableIds)
            {
                PreLoadTable(tableId);
            }
        }

        /// <summary>
        /// Allows pre-loading of the specified table(s) to decrease loading times during execution.
        /// If the table is already loaded nothing will happen.
        /// </summary>
        /// <param name="tableId">A table that should be pre-loaded</param>
        public void PreLoadTable(string tableId)
        {
            this.GetDataTable(tableId);
        }
    }
}
