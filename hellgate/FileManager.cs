using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using FileEntry = Hellgate.Index.FileEntry;

namespace Hellgate
{
    public class FileManager
    {
        public bool IntegrityCheck { get; private set; }

        bool MPVersion { get; set; }
        string HellgatePath { get; set; }
        string HellgateDataPath { get { return Path.Combine(HellgatePath, Common.DataPath); } }
        string HellgateDataCommonPath { get { return Path.Combine(HellgatePath, Common.DataCommonPath); } }
        string[] MPFiles = new string[] { "hellgate*", "mp_hellgate*" };
        string[] SPFiles = new string[] { "hellgate*", "sp_hellgate*" };
        string Language { get; set; }

        public List<Index> IndexFiles { get; set; }
        public Hashtable FileEntries { get; set; }
        public Hashtable ExcelFiles { get; set; }
        public DataSet XlsDataSet { get; set; }


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
            // If the index files didn't load correctly, don't go any further.
            if (!(IntegrityCheck)) return;

            LoadExcelFiles();
        }

        /// <summary>
        /// Generates of a list of all the files inside the .idx .dat files from the Hellgate path.
        /// </summary>
        /// <returns>Result of the initialization. Occurance of an error will return false.</returns>
        bool LoadFileTable()
        {
            IndexFiles = new List<Index>();

            // Get a list of all the existing idx paths
            List<string> idxPaths = new List<string>();
            string[] query = MPVersion ? MPFiles : SPFiles;
            foreach (string fileQuery in query)
            {
                idxPaths.AddRange(Directory.GetFiles(HellgateDataPath, fileQuery).Where(p => p.EndsWith(".idx")));
            }

            // Initialize the file table
            FileEntries = new Hashtable();

            foreach (string idxPath in idxPaths)
            {
                byte[] buffer = File.ReadAllBytes(idxPath);
                Index index = new Index(buffer)
                {
                    FilePath = idxPath
                };
                if (!(index.IntegrityCheck)) return false;
                IndexFiles.Add(index);

                foreach (FileEntry fileEntry in index.Files)
                {
                    // If the file isn't already hashed, add it
                    if (!(FileEntries.Contains(fileEntry.LongHash)))
                    {
                        FileEntries.Add(fileEntry.LongHash, fileEntry);
                    }
                    else
                    {
                        // This file has already been added. If this version is newer, replace it
                        UInt64 longHash = fileEntry.LongHash;
                        FileEntry existingFile = (FileEntry)FileEntries[longHash];
                        long existingFileTime = existingFile.FileTime;

                        if (existingFileTime < fileEntry.FileTime)
                        {
                            FileEntries[longHash] = fileEntry;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Loads all of the available Excel files to a hashtable.
        /// </summary>
        void LoadExcelFiles()
        {
            ExcelFiles = new Hashtable();
            foreach (DictionaryEntry item in FileEntries)
            {
                FileEntry fileEntry = (FileEntry)item.Value;

                // Do not load excel files that have been "backed up"
                if (fileEntry.DirectoryString.Contains(Index.BackupPrefix))
                {
                    continue;
                }
                
                if (fileEntry.FileNameString.Contains(ExcelFile.FileExtention))
                {
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
                    ExcelFile excelFile = new ExcelFile(fileBytes);
                    if (excelFile.IntegrityCheck)
                    {
                        string path = Path.Combine(fileEntry.DirectoryString, fileEntry.FileNameString);
                        ExcelFiles.Add(path, excelFile);
                    }
                }
            }

            // Close any open dats
            foreach (Index indexFile in IndexFiles)
            {
                indexFile.EndDatAccess();
            }
        }

        /// <summary>
        /// Loads the excel table data set.
        /// </summary>
        void LoadTableDataSet()
        {

        }
    }
}
