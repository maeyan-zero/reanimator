using System;
using System.Collections.Generic;
using System.Data;
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
        public string Language { get; private set; }
        public List<Index> IndexFiles { get; private set; }
        public Dictionary<ulong,FileEntry> FileEntries { get; private set; }
        public Dictionary<string,DataFile> DataFiles { get; private set; }
        public DataSet XlsDataSet { get; private set; }

        /// <summary>
        /// Initialize the File Manager by the given Hellgate path.
        /// </summary>
        /// <param name="hellgatePath">Path to the Hellgate installation.</param>
        public FileManager(string hellgatePath)
            : this(hellgatePath, false)
        {
            //no body
        }

        /// <summary>
        /// Initialize the File Manager by the given Hellgate path.
        /// </summary>
        /// <param name="hellgatePath">Path to the Hellgate installation.</param>
        /// <param name="mpVersion">Set true to initialize MP data.</param>
        public FileManager(string hellgatePath, bool mpVersion)
        {
            HellgatePath = hellgatePath;
            MPVersion = mpVersion;
            IntegrityCheck = LoadFileTable();
        }

        /// <summary>
        /// Generates of a list of all the files inside the .idx .dat files from the Hellgate path.
        /// </summary>
        /// <returns>Result of the initialization. Occurance of an error will return false.</returns>
        private bool LoadFileTable()
        {
            IndexFiles = new List<Index>();

            List<string> idxPaths = new List<string>();
            string[] query = MPVersion ? Common.MPFiles : Common.SPFiles;
            foreach (string fileQuery in query)
            {
                idxPaths.AddRange(Directory.GetFiles(HellgateDataPath, fileQuery).Where(p => p.EndsWith(".idx")));
            }

            FileEntries = new Dictionary<ulong,FileEntry>();

            foreach (string idxPath in idxPaths)
            {
                byte[] buffer = File.ReadAllBytes(idxPath);
                Index index = new Index(buffer)
                {
                    FilePath = idxPath
                };
                if (!(index.IntegrityCheck)) return false;
                IndexFiles.Add(index);

                foreach (FileEntry current in index.Files)
                {
                    ulong hash = current.LongHash;

                    if (!(FileEntries.ContainsKey(hash)))
                    {
                        FileEntries.Add(hash, current);
                        continue;
                    }

                    FileEntry existing = FileEntries[hash];
                    if (existing.FileTime < current.FileTime)
                    {
                        FileEntries[hash] = current;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Loads all of the available Excel files to a hashtable.
        /// </summary>
        public bool LoadExcelFiles()
        {
            DataFiles = new Dictionary<string,DataFile>();
            foreach (FileEntry fileEntry in FileEntries.Values)
            {
                // Do not load excel files that have been "backed up"
                if ((fileEntry.DirectoryString.Contains(Index.BackupPrefix)))
                {
                    continue;
                }

                // Check if its a excel file by its file extentsion
                if (!(fileEntry.FileNameString.Contains(ExcelFile.FileExtention)))
                {
                    continue;
                }

                // Create a hashtable key from the filename
                string fileName = fileEntry.FileNameString;
                fileName = fileName.Replace(ExcelFile.FileExtention, "");
                fileName = fileName.ToUpper();

                // If the accompanying dat isn't open, open it
                if (!(fileEntry.Parent.DatFileOpen))
                {
                    fileEntry.Parent.OpenDat(FileAccess.Read);
                }

                // Parse the excel file and if it reads okay add it to the table
                byte[] fileBytes = fileEntry.Parent.GetFileBytes(fileEntry);
                ExcelFile excelFile = new ExcelFile(fileBytes)
                {
                    FilePath = Path.Combine(fileEntry.DirectoryString, fileEntry.FileNameString)
                };

                if (excelFile.IntegrityCheck)
                {
                    DataFiles.Add(excelFile.GetStringID(), excelFile);
                }
            }

            // Close any open dats
            foreach (Index indexFile in IndexFiles)
            {
                indexFile.EndDatAccess();
            }
            return true;
        }

        /// <summary>
        /// Retrieves a DataFile from the DataFiles list.
        /// </summary>
        /// <param name="stringID">The stringID of the DataFile.</param>
        /// <returns>Matching DataFile if it exists.</returns>
        public DataFile GetDataFile(string stringID)
        {
            if ((DataFiles == null)) return null;
            var query = from dt in DataFiles
                        where dt.Value.StringID == stringID
                        select dt.Value;
            return (!(query.Count() == 0)) ? query.First() : null;
        }

        public DataTable GetExcelTableFromCode(uint code)
        {
            if ((DataFiles == null)) return null;
            if ((DataFiles["EXCEL_TABLES"] == null)) return null;
            ExcelFile excelTables = (ExcelFile)DataFiles["EXCEL_TABLES"];
            Excel.ExcelTables tableRow = (Excel.ExcelTables)excelTables.Rows[(int)code];
            string stringID = tableRow.StringId;
            if ((DataFiles[stringID] == null)) return null;
            ExcelFile excelFile = (ExcelFile)DataFiles[stringID];
            DataTable dataTable = LoadExcelTable(excelFile, false);
            return dataTable;
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
