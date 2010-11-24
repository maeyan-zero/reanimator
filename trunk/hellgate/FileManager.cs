using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using FileEntry = Hellgate.Index.FileEntry;

namespace Hellgate
{
    public partial class FileManager : IDisposable
    {
        public bool IntegrityCheck { get; private set; }
        public bool MPVersion { get; private set; }
        public string HellgatePath { get; private set; }
        public string HellgateDataPath { get { return Path.Combine(HellgatePath, Common.DataPath); } }
        public string HellgateDataCommonPath { get { return Path.Combine(HellgatePath, Common.DataCommonPath); } }
        public string Language { get; private set; } // determines which folder to check for the strings files
        public List<Index> IndexFiles { get; private set; }
        public Dictionary<ulong, FileEntry> FileEntries { get; private set; }
        public SortedDictionary<String, DataFile> DataFiles { get; private set; }
        public DataSet XlsDataSet { get; private set; }

        /// <summary>
        /// Initialize the File Manager by the given Hellgate path.
        /// </summary>
        /// <param name="hellgatePath">Path to the Hellgate installation.</param>
        /// <param name="mpVersion">Set true to initialize MP data.</param>
        public FileManager(String hellgatePath, bool mpVersion = false)
        {
            HellgatePath = hellgatePath;
            MPVersion = mpVersion;
            Language = "english"; // do we need to bother with anything other than english?

            DataFiles = new SortedDictionary<String, DataFile>();
            IndexFiles = new List<Index>();
            FileEntries = new Dictionary<ulong, FileEntry>();
            XlsDataSet = new DataSet("xlsDataSet")
            {
                Locale = new CultureInfo("en-us", true),
                RemotingFormat = SerializationFormat.Binary
            };

            IntegrityCheck = LoadFileTable();
        }

        /// <summary>
        /// Generates of a list of all the files inside the .idx .dat files from the Hellgate path.
        /// </summary>
        /// <returns>Result of the initialization. Occurance of an error will return false.</returns>
        private bool LoadFileTable()
        {
            List<String> idxPaths = new List<String>();
            string[] query = MPVersion ? Common.MPFiles : Common.SPFiles;
            foreach (String fileQuery in query)
            {
                idxPaths.AddRange(Directory.GetFiles(HellgateDataPath, fileQuery).Where(p => p.EndsWith(Index.FileExtension)));
            }
            if (idxPaths.Count == 0)
            {
                Console.WriteLine("Error: No index files found at parth: " + HellgateDataPath);
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
            String datFullPath = fullPath.Replace(Index.FileExtension, Index.DatFileExtension);
            if (!File.Exists(datFullPath)) return;


            // read in and parse index
            Index index;
            try
            {
                byte[] idxBytes = File.ReadAllBytes(fullPath);
                index = new Index(idxBytes) { FilePath = fullPath };
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
                ulong hash = currFileEntry.LongPathHash;

                // have we added the file yet
                if (!FileEntries.ContainsKey(hash))
                {
                    FileEntries.Add(hash, currFileEntry);
                    continue;
                }

                // we haven't added the file, so we need to compare file times and backup states
                FileEntry origFileEntry = FileEntries[hash];

                // do backup checks first as they'll "override" the FileTime values (i.e. file not found causes game to go to older version)
                // if currFile IS a backup, and orig is NOT, then add to Siblings as game will be loading orig over "backup" anyways
                if (currFileEntry.IsBackup && !origFileEntry.IsBackup)
                {
                    if (origFileEntry.Siblings == null) origFileEntry.Siblings = new List<FileEntry>();
                    origFileEntry.Siblings.Add(currFileEntry);

                    continue;
                }

                // if curr is NOT a backup, but orig IS, then we want to update (i.e. don't care about FileTime; as above)
                // OR if orig is older than curr, we also want to update/re-arrange Siblings, etc
                if ((!currFileEntry.IsBackup && origFileEntry.IsBackup) ||
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
                byte[] fileBytes = GetFileBytes(fileEntry);

                // parse file data
                DataFile dataFile;
                if (fileEntry.FileNameString.EndsWith(ExcelFile.FileExtention))
                {
                    dataFile = new ExcelFile(fileBytes) { FilePath = fileEntry.RelativeFullPathWithoutBackup };
                }
                else
                {
                    String stringsStringId = fileEntry.FileNameString.Replace(StringsFile.FileExtention, "").ToUpper();
                    dataFile = new StringsFile(fileBytes, stringsStringId) { FilePath = fileEntry.RelativeFullPathWithoutBackup };
                }
                if (dataFile.IntegrityCheck) DataFiles.Add(dataFile.StringId, dataFile);
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
        public DataTable GetExcelTableFromCode(uint code)
        {
            if (DataFiles == null || DataFiles["EXCEL_TABLES"] == null) return null;

            ExcelFile excelTables = (ExcelFile)DataFiles["EXCEL_TABLES"];
            Excel.ExcelTables tableRow = (Excel.ExcelTables)excelTables.Rows[(int)code];
            String stringId = tableRow.StringId;
            if (DataFiles[stringId] == null) return null;

            ExcelFile excelFile = (ExcelFile)DataFiles[stringId];
            DataTable dataTable = LoadExcelTable(excelFile, false);
            return dataTable;
        }

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
            if (fileEntry.DirectoryString.Contains(Index.BackupPrefix))
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
                if (!fileEntry.Parent.DatFileOpen && !fileEntry.Parent.OpenDat(FileAccess.Read))
                {
                    Console.WriteLine("Warning: Failed to open .dat for reading: " + fileEntry.Parent.FileNameWithoutExtension);
                    return null;
                }

                fileBytes = fileEntry.Parent.GetFileBytes(fileEntry);
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
            foreach (Index indexFile in IndexFiles)
            {
                indexFile.EndDatAccess();
            }
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
