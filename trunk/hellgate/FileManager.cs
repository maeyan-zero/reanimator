using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using FileEntry = Hellgate.IndexFile.FileEntry;

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
        public List<IndexFile> IndexFiles { get; private set; }
        public Dictionary<ulong, FileEntry> FileEntries { get; private set; }
        public SortedDictionary<String, DataFile> DataFiles { get; private set; }
        public DataSet XlsDataSet { get; private set; }

        /// <summary>
        /// Initialize the File Manager by the given Hellgate path.
        /// </summary>
        /// <param name="hellgatePath">Path to the Hellgate installation.</param>
        /// <param name="mpVersion">Set true to initialize only the MP files data.</param>
        public FileManager(String hellgatePath, bool mpVersion = false)
        {
            HellgatePath = hellgatePath;
            MPVersion = mpVersion;
            Language = "english"; // do we need to bother with anything other than english?
            Reload();
        }

        /// <summary>
        /// Reinitializes the FileManager. This is useful after a modification has been installed.
        /// </summary>
        public void Reload()
        {
            DataFiles = new SortedDictionary<String, DataFile>();
            IndexFiles = new List<IndexFile>();
            FileEntries = new Dictionary<ulong, FileEntry>();
            XlsDataSet = new DataSet("xlsDataSet")
            {
                Locale = new CultureInfo("en-us", true),
                RemotingFormat = SerializationFormat.Binary
            };

            HasIntegrity = _LoadFileTable();
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
                idxPaths.AddRange(Directory.GetFiles(HellgateDataPath, fileQuery).Where(p => p.EndsWith(IndexFile.FileExtension)));
            }
            if (idxPaths.Count == 0)
            {
                Console.WriteLine("Error: No index files found at path: " + HellgateDataPath);
                return false;
            }

            foreach (String idxPath in idxPaths)
            {
                _LoadIndexFile(idxPath);
            }

            return FileEntries.Count != 0;
        }

        /// <summary>
        /// Parses a single index file on the specified path. Checking for accompanying dat file and populating file index.
        /// </summary>
        /// <param name="fullPath">The full path of the index file to parse.</param>
        private void _LoadIndexFile(String fullPath)
        {
            // if there is no accompanying .dat at all, then ignore .idx
            String datFullPath = fullPath.Replace(IndexFile.FileExtension, IndexFile.DatFileExtension);
            if (!File.Exists(datFullPath)) return;


            // read in and parse index
            IndexFile index;
            try
            {
                byte[] idxBytes = File.ReadAllBytes(fullPath);
                index = new IndexFile(idxBytes) { FilePath = fullPath };
            }
            catch (Exception)
            {
                Console.WriteLine("Warning: Failed to read in index file: " + fullPath);
                return;
            }
            if (!index.HasIntegrity) return;
            IndexFiles.Add(index);

            // loop through index files
            foreach (FileEntry currFileEntry in index.Files)
            {
                //if (currFileEntry.FileNameString.Contains("recipes.txt.cooked"))
                //{
                //    int bp = 0;
                //}

                ulong pathHash = currFileEntry.LongPathHash;

                // have we added the file yet
                if (!FileEntries.ContainsKey(pathHash))
                {
                    FileEntries.Add(pathHash, currFileEntry);
                    continue;
                }

                // we haven't added the file, so we need to compare file times and backup states
                FileEntry origFileEntry = FileEntries[pathHash];

                // do backup checks first as they'll "override" the FileTime values (i.e. file not found causes game to go to older version)
                // if currFile IS a backup, and orig is NOT, then add to Siblings as game will be loading orig over "backup" anyways
                if (currFileEntry.IsPatchedOut && !origFileEntry.IsPatchedOut)
                {
                    if (origFileEntry.Siblings == null) origFileEntry.Siblings = new List<FileEntry>();
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
                    if (currFileEntry.Siblings == null) currFileEntry.Siblings = new List<FileEntry>();
                    currFileEntry.Siblings.Add(origFileEntry);
                    FileEntries[pathHash] = currFileEntry;

                    continue;
                }

                // if curr is older (or equal to; hellgate000 has duplicates) than the orig, then add this to the Siblings list (i.e. orig is newer)
                if (origFileEntry.FileTime >= currFileEntry.FileTime)
                {
                    if (origFileEntry.Siblings == null) origFileEntry.Siblings = new List<FileEntry>();
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
        public bool LoadTableFiles()
        {
            // want excel files and strings files
            foreach (FileEntry fileEntry in
                FileEntries.Values.Where(fileEntry => fileEntry.FileNameString.EndsWith(ExcelFile.FileExtention) ||
                    (fileEntry.FileNameString.EndsWith(StringsFile.FileExtention) && fileEntry.RelativeFullPath.Contains(Language))))
            {
                if (MPVersion &&
                    // todo: crashing
                    (fileEntry.FileNameString.Contains("items.txt.cooked") ||
                    fileEntry.FileNameString.Contains("monsters.txt.cooked") ||
                    fileEntry.FileNameString.Contains("objects.txt.cooked") ||
                    fileEntry.FileNameString.Contains("players.txt.cooked") ||
                    fileEntry.FileNameString.Contains("missiles.txt.cooked") ||
                    fileEntry.FileNameString.Contains("sounds.txt.cooked") ||
                    fileEntry.FileNameString.Contains("skills.txt.cooked"))) continue;

                byte[] fileBytes = GetFileBytes(fileEntry);

                //if (fileEntry.FileNameString.Contains("strings_cin"))
                //{
                //    int bp = 0;
                //}

                // parse file data
                DataFile dataFile;
                if (fileEntry.FileNameString.EndsWith(ExcelFile.FileExtention))
                {
                    dataFile = new ExcelFile(fileBytes, fileEntry.RelativeFullPathWithoutPatch, MPVersion);
                    if (dataFile.Attributes.IsEmpty) continue;
                }
                else
                {
                    if (MPVersion) continue; // todo: need to add _TCv4_ stringId keys to DataFileMap before we can load them 
                    dataFile = new StringsFile(fileBytes, fileEntry.RelativeFullPathWithoutPatch);
                }

                if (!dataFile.HasIntegrity)
                {
                    Console.WriteLine("Error: Failed to load data file: " + dataFile.StringId);
                    continue;
                }
                if (dataFile.StringId == null)
                {
                    Console.WriteLine(String.Format("Error: StringId is null. {0} was not indexed.", fileEntry.FileNameString));
                    continue;
                }

                try
                {
                    DataFiles.Add(dataFile.StringId, dataFile);
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
        /// Retrieves an Excel Table from its Code value. If it's not loaded, then it will be loaded and returned.
        /// </summary>
        /// <param name="code">The code of the Excel Table to retrieve.</param>
        /// <returns>The Excel Table as a DataTable, or null if not found.</returns>
        public ExcelFile GetExcelTableFromCode(uint code)
        {
            if (DataFiles == null || DataFiles["EXCELTABLES"] == null) return null;

            ExcelFile excelTables = (ExcelFile)DataFiles["EXCELTABLES"];
            for (int i = 0; i < excelTables.Count; i++)
            {
                Excel.ExcelTables excelTable = (Excel.ExcelTables)excelTables.Rows[i];

                if (excelTable.code == code) return (ExcelFile)DataFiles[excelTable.name];
            }

            return null;
        }

        public int GetExcelRowIndex(uint code, String value)
        {
            if (DataFiles == null || DataFiles["EXCELTABLES"] == null) return -1;

            ExcelFile excelTable = GetExcelTableFromCode(code);
            if (excelTable == null) return -1;

            Type type = excelTable.Rows[0].GetType();
            FieldInfo[] fields = type.GetFields();
            FieldInfo field = fields[0];

            bool isStringField = (field.FieldType == typeof(String));
            int i = 0;
            foreach (Object row in excelTable.Rows)
            {
                if (isStringField)
                {
                    String val = (String)field.GetValue(row);
                    if (val == value) return i;
                }
                else // string offset
                {
                    int offset = (int)field.GetValue(row);
                    String stringVal = excelTable.ReadStringTable(offset);
                    if (stringVal == value) return i;
                }
                i++;
            }


            return -1;
        }

        public String GetExcelRowStringFromIndex(uint code, int index)
        {
            if (DataFiles == null || DataFiles["EXCELTABLES"] == null || index < 0) return null;

            ExcelFile excelTable = GetExcelTableFromCode(code);
            if (excelTable == null || index >= excelTable.Rows.Count) return null;

            Type type = excelTable.Rows[0].GetType();
            FieldInfo[] fields = type.GetFields();
            FieldInfo field = fields[0];

            bool isStringField = (field.FieldType == typeof(String));
            Object row = excelTable.Rows[index];

            if (isStringField) return (String)field.GetValue(row);

            int offset = (int)field.GetValue(row);
            String stringVal = excelTable.ReadStringTable(offset);
            return stringVal;
        }

        //public String GetExcelTableStringIdFromCode(UInt32 code)
        //{
        //    DataTable excelTables = GetExcelTableFromStringId("EXCELTABLES");
        //    if (excelTables == null) return null;

        //    DataRow[] rows = excelTables.Select(String.Format("code = '{0}'", code));
        //    return rows.Length == 0 ? null : rows[0][1].ToString();
        //}

        /// <summary>
        /// Gets file byte data from most principle location; considering filetimes and backup status.
        /// The user must manually call EndAllDatAccess to close access to any opened .dat files during the process.
        /// </summary>
        /// <param name="fileEntry">The file entry details to read.</param>
        /// <returns>The file byte array, or null on error.</returns>
        public byte[] GetFileBytes(FileEntry fileEntry)
        {
            byte[] fileBytes = null;


            // if file is backed up, check for unpacked copy
            String filePath = fileEntry.RelativeFullPath;
            if (fileEntry.IsPatchedOut)
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
                // open .dat
                if (!fileEntry.Index.DatFileOpen && !fileEntry.Index.OpenDat(FileAccess.Read))
                {
                    Console.WriteLine("Warning: Failed to open .dat for reading: " + fileEntry.Index.FileNameWithoutExtension);
                    return null;
                }

                fileBytes = fileEntry.Index.GetFileBytes(fileEntry);
                if (fileBytes == null)
                {
                    Console.WriteLine("Warning: Failed to read file from .dat: " + fileEntry.FileNameString);
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
            foreach (IndexFile indexFile in IndexFiles)
            {
                indexFile.EndDatAccess();
            }
        }

        /// <summary>
        /// Attempts to open all .dat files for reading.
        /// </summary>
        /// <returns>True if .dat files were opened, false upon failure.</returns>
        public bool BeginAllDatReadAccess()
        {
            // open accompanying dat files
            if (IndexFiles.Any(indexFile => !indexFile.BeginDatReading()))
            {
                EndAllDatAccess();
                return false;
            }

            return true;
        }

        #region IDisposable Members
        public void Dispose()
        {
            if (!(XlsDataSet == null))
                XlsDataSet.Dispose();
        }
        #endregion
    }
}
