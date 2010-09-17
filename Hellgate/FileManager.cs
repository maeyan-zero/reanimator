using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using Hellgate;
using FileEntry = Hellgate.IndexFile.FileDefinition;
using ColumnKeys = Hellgate.ExcelFile.ColumnKeys;
using ExcelAttribute = Hellgate.ExcelFile.ExcelAttribute;

namespace Reanimator
{
    public class FileManager
    {
        public Hashtable HellgateFiles { get; private set; } // Hashed by fileEntryHash.
        public bool Initialized { get  { return HellgateFiles == null ? false : true; } }
        public void Reload() { LoadIndexFiles(); }


        private String BasePacks { get { return "hellgate*.idx"; } }
        private String CustomPacks { get; set; } // ie. "sp_hellgate_*"
        private String HellgatePath { get; set; } // ie. c:\Program Files\Hellgate London
        private Hashtable IndexFiles { get; set; } // Hashed by fileName.
        private Hashtable ExcelFiles { get; set; } // Hashed by stringID.
        private Hashtable StringFiles { get; set; } // Hashed by fileName.
        private DataSet XlsDataSet { get; set; }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="hellgatePath"></param>
        /// <param name="mpVersion"></param>
        public FileManager(string hellgatePath, bool mpVersion)
        {
            HellgatePath = hellgatePath;
            CustomPacks = mpVersion ? "mp_hellgate*.idx" : "sp_hellgate*.idx";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hellgatePath"></param>
        public FileManager(string hellgatePath)
            : this(hellgatePath, false) { }



        /// <summary>
        /// Loads all the index files found in the Hellgate directory specified in the constructor.
        /// </summary>
        /// <returns>Returns true if the load was successful.</returns>
        public bool LoadIndexFiles()
        {
            // Check if the Hellgate path exists.
            String HellgateDataPath = Path.Combine(HellgatePath, "data\\");
            if (!(Directory.Exists(HellgateDataPath))) return false;

            // Obtain the paths of all idx files.
            List<String> packPaths = new List<string>();
            packPaths.AddRange(Directory.GetFiles(HellgateDataPath, BasePacks));
            packPaths.AddRange(Directory.GetFiles(HellgateDataPath, CustomPacks));
            if (packPaths.Count == 0) return false;

            // Load the idx files.
            IndexFiles = new Hashtable();
            foreach(String packPath in packPaths)
            {
                try
                {
                    Byte[] buffer = File.ReadAllBytes(packPath);
                    IndexFile indexFile = new IndexFile(buffer)
                    {
                        FilePath = packPath
                    };
                    String fileName = Path.GetFileNameWithoutExtension(packPath);
                    if (indexFile.IntegrityCheck) IndexFiles.Add(fileName, indexFile);
                }
                catch
                {
                    continue;
                }
            }

            // Create a hashtable of all the files contained in the index files,
            // disgarding any duplicates for the newest versions.
            HellgateFiles = new Hashtable();
            foreach (DictionaryEntry dictionaryEntry in IndexFiles)
            {
                IndexFile indexFile = (IndexFile)dictionaryEntry.Value;
                foreach (FileEntry fileEntry in indexFile.FileList)
                {
                    if (HellgateFiles.ContainsKey(fileEntry.Hash))
                    {
                        Managed existing = (Managed)HellgateFiles[fileEntry.Hash];
                        
                        // Replace existing if fileEntry is newer.
                        if (fileEntry.FileStruct.FileTime > existing.Entry.FileStruct.FileTime)
                            HellgateFiles[fileEntry.Hash] = new Managed(fileEntry, indexFile);
                    }
                    else
                    {
                        Managed managedIndex = new Managed(fileEntry, indexFile);
                        HellgateFiles.Add(fileEntry.Hash, managedIndex);
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Loads all the Excel files into the FileManager collection.
        /// </summary>
        /// <returns>Returns true if the load was successful.</returns>
        public bool LoadExcelFiles()
        {
            IEnumerable<String> fileNames = ExcelFile.DataTables.Select( fn => fn.Key );

            ExcelFiles = new Hashtable();
            foreach (String stringID in fileNames)
            {
                if (!(LoadExcelFile(stringID))) return false;
            }

            return true;
        }




        /// <summary>
        /// Extracts a requested file by its given directory and file name.<br/>
        /// If the file is not found in the FileManagers hashtable, it will look inside the Hellgate directory.
        /// </summary>
        /// <param name="directory">The directory of the file. ie "data_common\\excel\\"</param>
        /// <param name="fileName">The file name of the file. ie "exceltables.txt.cooked"</param>
        /// <returns>Returns a byte array of the requested file.</returns>
        public byte[] GetFileBytes(string directory, string fileName)
        {
            //if (!(IndexFilesAreLoaded)) return null;

            UInt64 directoryHash = (UInt64)Crypt.GetHash(directory);
            UInt64 fileNameHash = (UInt64)Crypt.GetHash(fileName);
            UInt64 fileEntryHash = directoryHash + (fileNameHash << 32);

            // Extract the file from the dat.
            if (HellgateFiles.ContainsKey(fileEntryHash))
            {
                Managed file = (Managed)HellgateFiles[fileEntryHash];
                IndexFile indexFile = (IndexFile)file.Parent;
                Byte[] buffer = indexFile.GetFileBytes(directory, fileName);
                return buffer;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public byte[] GetFileBytes(Managed file)
        {
            IndexFile indexFile = (IndexFile)file.Parent;
            return indexFile.GetFileBytes(file.Entry.FileStruct);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="excelStringID"></param>
        /// <returns></returns>
        public bool LoadExcelFile(string excelStringID)
        {
            String fileName = excelStringID.ToLower() + ExcelFile.FileExtension;
            String directory = Path.Combine("data_common\\", ExcelFile.FilePath);

            Byte[] buffer = GetFileBytes(directory, fileName);
            if (buffer == null)
            {
                directory = directory.Replace("data_common", "data");
                buffer = GetFileBytes(directory, fileName);
                if (buffer == null) return false;
            }

            ExcelFile excelFile = new ExcelFile(buffer);
            if (!(excelFile.IntegrityCheck)) return false;
            ExcelFiles.Add(excelStringID, excelFile);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="excelStringID"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string excelStringID)
        {
            if (XlsDataSet == null) XlsDataSet = new DataSet();

            // If the table is already generated, great! Return it
            if (XlsDataSet.Tables.Contains(excelStringID)) return XlsDataSet.Tables[excelStringID];

            // Initialize the hashtable if need be
            if (ExcelFiles == null) ExcelFiles = new Hashtable();

            // Initialize the excel table
            if (!(ExcelFiles.Contains(excelStringID)))
            {
                // Need to load it. Check all files are loaded
                //if (!(IndexFilesAreLoaded)) return null;

                // Load the excel file.

                if (!(LoadExcelFile(excelStringID))) return null;
            }

            ExcelFile excelFile = (ExcelFile)ExcelFiles[excelStringID];
            DataTable dataTable = excelFile.GetDataTable();
            dataTable.TableName = excelStringID;
            XlsDataSet.Tables.Add(dataTable);

            return dataTable;
        }

        /// <summary>
        /// Loads the related data tables of the given excel table.<br />
        /// The method assumes the DataSet is already initialized and the given excel table is loaded.
        /// </summary>
        /// <param name="excelTable">The StringID of the excel table.</param>
        /// <returns>A boolean if the method was successful.</returns>
        public bool GenerateRelations(string excelStringID)
        {
            if (!(XlsDataSet.Tables.Contains(excelStringID))) return false;

            DataTable dt = XlsDataSet.Tables[excelStringID];
            ExcelFile ef = (ExcelFile)ExcelFiles[excelStringID];

            int col = 1;

            foreach (FieldInfo fieldInfo in ef.StructureType.GetFields())
            {
                DataColumn dcChild = dt.Columns[col++];

                ExcelAttribute excelAttribute = ExcelFile.GetExcelAttribute(fieldInfo);
                if (excelAttribute == null) continue;

                if (dcChild.ExtendedProperties.Contains(ColumnKeys.IsTableIndex))
                {
                    String relatedTableID = (String)dcChild.ExtendedProperties[ColumnKeys.TableStringID];
                    DataTable relatedTable;

                    // Check if the table if already loaded
                    if (!(XlsDataSet.Tables.Contains(relatedTableID)))
                    {
                        ExcelFile excelFile = (ExcelFile)ExcelFiles[relatedTableID];
                        if (excelFile == null) continue;

                        DataTable dataTable = excelFile.GetDataTable();
                        dataTable.TableName = excelFile.StringID;
                        XlsDataSet.Tables.Add(dataTable);
                    }

                    relatedTable = XlsDataSet.Tables[relatedTableID];
                    DataColumn dcParent = relatedTable.Columns["Index"];

                    // Create the relation
                    String relationName = excelStringID + dcChild.ColumnName + ColumnKeys.IsTableIndex;
                    DataRelation relation = new DataRelation(relationName, dcParent, dcChild, false);
                    XlsDataSet.Relations.Add(relation);

                    // Add the column
                    DataColumn dcString = dt.Columns.Add(dcChild.ColumnName + "_string", typeof(String), String.Format("Parent({0}).{1}", relationName, relatedTable.Columns[1].ColumnName));
                    dcString.SetOrdinal(dcChild.Ordinal + 1);
                    dcString.ExtendedProperties.Add(ColumnKeys.IsRelationGenerated, true);
                    col++;
                }
            }

            return true;
        }

        public struct Managed
        {
            public String Directory { get { return Entry.Directory; } }
            public String FileName { get { return Entry.FileName; } }
            public FileEntry Entry { get; private set; }
            public IndexFile Parent { get; private set; }

            public Managed(FileEntry entry, IndexFile parent)
                : this()
            {
                Entry = entry;
                Parent = parent;
            }
        }


    }
}