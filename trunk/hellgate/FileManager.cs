using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Revival.Common;

namespace Hellgate
{
    public partial class FileManager : IDisposable
    {
        public bool HasIntegrity { get; private set; }
        public bool MPVersion { get; private set; }
        public string HellgatePath { get; private set; }
        public string HellgateDataPath { get { return Path.Combine(HellgatePath, Common.DataPath); } }
        public string HellgateDataCommonPath { get { return Path.Combine(HellgatePath, Common.DataCommonPath); } }
        public string Language { get; private set; } // determines which folder to check for the strings files
        public List<PackFile> IndexFiles { get; private set; }
        public Dictionary<ulong, PackFileEntry> FileEntries { get; private set; }
        public SortedDictionary<String, DataFile> DataFiles { get; private set; }
        public DataSet XlsDataSet { get; private set; }
        private List<uint> _excelIndexToCodeList;
        private Dictionary<uint, ExcelFile> _excelCodeToTable;
        private readonly String _excelTablesStringId;
        public readonly Dictionary<String, ObjectDelegator> DataFileDelegators = new Dictionary<String, ObjectDelegator>();

        /// <summary>
        /// Initialize the File Manager by the given Hellgate path.
        /// </summary>
        /// <param name="hellgatePath">Path to the Hellgate installation.</param>
        /// <param name="mpVersion">Set true to initialize only the MP files data.</param>
        public FileManager(String hellgatePath, bool mpVersion = false)
        {
            HellgatePath = hellgatePath;
            MPVersion = mpVersion;
            _excelTablesStringId = MPVersion ? "_TCv4_EXCELTABLES" : "EXCELTABLES";
            Language = "english"; // do we need to bother with anything other than english?
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
            List<String> idxPaths = new List<String>();
            string[] query = MPVersion ? Common.MPFiles : Common.SPFiles;
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
                catch (Exception)
                {
                    Console.WriteLine("Warning: Failed to read in index file: " + idxPath);
                    continue;
                }

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
                if (currFileEntry.Name.Contains("bldg_c_station_warp_next_layout.xml.cooked") || currFileEntry.Name.Contains("sku."))
                {
                    int bp = 0;
                }

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
            BeginAllDatReadAccess();

            // want excel files and strings files
            foreach (PackFileEntry fileEntry in
                FileEntries.Values.Where(fileEntry => fileEntry.Name.EndsWith(ExcelFile.Extension) ||
                    (fileEntry.Name.EndsWith(StringsFile.Extention) && fileEntry.Path.Contains(Language))))
            {
                byte[] fileBytes = GetFileBytes(fileEntry, ignorePatchedOut);

                //if (!fileEntry.FileNameString.Contains("sounds")) continue;
                //if (!MPVersion && fileEntry.FileNameString.Contains("act.txt.cooked"))
                //{
                //    int bp = 0;
                //}

                // parse file data
                DataFile dataFile;
                if (fileEntry.Name.EndsWith(ExcelFile.Extension))
                {
                    try
                    {
                        dataFile = new ExcelFile(fileBytes, fileEntry.Path, MPVersion);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.ToString());
                        ExceptionLogger.LogException(e);
                        Console.WriteLine(String.Format("Critical Error: Failed to load excel file {0} (MPVersion={1})", fileEntry.Name, MPVersion));
                        continue;
                    }
                    if (dataFile.Attributes.IsEmpty) continue;
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

                    FieldInfo[] dataFileFields = dataFile.Attributes.RowType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    ObjectDelegator dataFileDelegator = new ObjectDelegator(dataFileFields);
                    DataFileDelegators.Add(dataFile.StringId, dataFileDelegator);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Critical Error: Cannot add table data file to dictionary: " + dataFile + "\n" + e);
                }
            }

            EndAllDatAccess();

            return true;
        }

        /// <summary>
        /// Retrieves a DataFile from the DataFiles list.
        /// </summary>
        /// <param name="stringId">The stringID of the DataFile.</param>
        /// <returns>Matching DataFile if it exists.</returns>
        public DataFile GetDataFile(String stringId)
        {
            if (DataFiles == null) return null;

            var query = from dt in DataFiles
                        where dt.Value.StringId == stringId
                        select dt.Value;

            return (query.Count() != 0) ? query.First() : null;
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
            if (value.GetType() == typeof(int)) return (int) value;

            return (int)(short)value;
        }

        /// <summary>
        /// Retrieves an Excel Table from its Code value. If it's not loaded, then it will be loaded and returned.
        /// </summary>
        /// <param name="code">The code of the Excel Table to retrieve.</param>
        /// <returns>The Excel Table as a DataTable, or null if not found.</returns>
        public ExcelFile GetExcelTableFromCode(uint code)
        {
            if (DataFiles == null || DataFiles.ContainsKey(_excelTablesStringId) == false) return null;

            if (_excelCodeToTable == null)
            {
                ExcelFile excelTables = (ExcelFile)DataFiles[_excelTablesStringId];
                _excelCodeToTable = new Dictionary<uint, ExcelFile>();

                foreach (Excel.ExcelTables excelTablesRow in excelTables.Rows)
                {
                    String excelName = (MPVersion) ? "_TCv4_" + excelTablesRow.name : excelTablesRow.name;
                    DataFile dataFile;
                    if (DataFiles.TryGetValue(excelName, out dataFile))
                    {
                        _excelCodeToTable[(uint)excelTablesRow.code] = (ExcelFile)dataFile;
                    }
                }
            }

            ExcelFile excelTable;
            return _excelCodeToTable.TryGetValue(code, out excelTable) ? excelTable : null;
        }

        public int GetExcelRowIndexFromTableIndex(int tableIndex, String value)
        {
            uint tableCode = _GetTableCodeFromTableIndex(tableIndex);
            return GetExcelRowIndex(tableCode, value);
        }

        public String GetExcelRowStringFromTableIndex(int tableIndex, int rowIndex)
        {
            uint tableCode = _GetTableCodeFromTableIndex(tableIndex);
            return GetExcelRowStringFromRowIndex(tableCode, rowIndex);
        }

        private uint _GetTableCodeFromTableIndex(int tableIndex)
        {
            if (_excelIndexToCodeList == null)
            {
                DataFile excelTables;
                if (!DataFiles.TryGetValue(_excelTablesStringId, out excelTables)) return 0;

                _excelIndexToCodeList = new List<uint>();
                foreach (Excel.ExcelTables excelTable in excelTables.Rows)
                {
                    _excelIndexToCodeList.Add((uint)excelTable.code);
                }
            }

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
                    intVal = (int) (short) val;
                }
                else
                {
                    intVal = (int) val;
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
            if (DataFiles == null || DataFiles[_excelTablesStringId] == null) return -1;

            return _GetExcelRowIndex(GetExcelTableFromCode(code), value);
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

        public String GetExcelRowStringFromRowIndex(uint code, int rowIndex, int colIndex = 0)
        {
            if (DataFiles == null || DataFiles.ContainsKey(_excelTablesStringId) == false || rowIndex < 0) return null;

            ExcelFile excelTable = GetExcelTableFromCode(code);

            String stringVal = _GetExcelRowStringFromExcelFile(excelTable, rowIndex, colIndex);
            return stringVal;
        }

        private static String _GetExcelRowStringFromExcelFile(ExcelFile excelTable, int rowIndex, int colIndex)
        {
            if (excelTable == null || rowIndex >= excelTable.Rows.Count) return null;

            Type type = excelTable.Attributes.RowType;
            FieldInfo[] fields = type.GetFields();
            FieldInfo field = fields[colIndex];

            bool isStringField = (field.FieldType == typeof(String));
            Object row = excelTable.Rows[rowIndex];

            if (isStringField) return (String)field.GetValue(row);

            int offset = (int)field.GetValue(row);
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
            String directoryString = Path.GetDirectoryName(relativeFilePath) + "\\";
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
                    ExcelFile excelFile = new ExcelFile(fileBytes, filePath, MPVersion);
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
    }
}
