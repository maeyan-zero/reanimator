using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hellgate;
using Revival.Common;
using FileEntry = Hellgate.IndexFile.FileEntry;
using Script = Revival.Modification.Revival.Modification.Script;
using System.Text.RegularExpressions;

namespace Revival
{
    public class Modification
    {
        public Revival Data { get; private set; }
        public bool IntegrityCheck { get { return Data != null ? true : false; } }
        public string DataPath { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public Modification(string path)
        {
            DataPath = Path.GetDirectoryName(path);
            try
            {
                using (TextReader textReader = new StreamReader(path))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Revival));
                    Data = serializer.Deserialize(textReader) as Revival;
                }
            }
            catch(Exception ex)
            {
                ExceptionLogger.LogException(ex);
                Console.WriteLine("Invalid or malformed XML structure.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileManager"></param>
        /// <returns></returns>
        public static bool RemoveModifications(FileManager fileManager)
        {
            bool errorOccurred = false;

            // Step one: Remove any "backup" prefixes from directory strings.
            foreach (IndexFile indexFile in fileManager.IndexFiles)
            {
                bool isModified = false;
                
                foreach (FileEntry fileEntry in indexFile.Files)
                {
                    if (fileEntry.IsBackup)
                    {
                        indexFile.PatchInFile(fileEntry);
                        isModified = true;
                    }
                }

                if (isModified)
                {
                    byte[] ibuffer = indexFile.ToByteArray();
                    Crypt.Encrypt(ibuffer);
                    try
                    {
                        File.WriteAllBytes(indexFile.FilePath, ibuffer);
                    }
                    catch(Exception ex)
                    {
                        ExceptionLogger.LogException(ex);
                        Console.WriteLine(String.Format("Error writing file {0}", indexFile.FilePath));
                        errorOccurred = true;
                    }
                }
            }

            // Step two: Remove any .idx and .dats that arn't FSS original
            string hellgateDatPath = fileManager.HellgateDataPath;
            List<string> hellgateDats = new List<string>();
            hellgateDats.AddRange(Directory.GetFiles(hellgateDatPath, "*.idx"));
            hellgateDats.AddRange(Directory.GetFiles(hellgateDatPath, "*.dat"));
            foreach (string datPath in hellgateDats)
            {
                string fileName = Path.GetFileName(datPath);
                if (Hellgate.Common.OriginalDats.Where(dat => String.Format("{0}.idx", dat) == fileName ||
                                                              String.Format("{0}.dat", dat) == fileName).Any() == false)
                {
                    try
                    {
                        File.Delete(datPath);
                    }
                    catch (Exception ex)
                    {
                        ExceptionLogger.LogException(ex);
                        Console.WriteLine(String.Format("Error deleting file {0}", datPath));
                        errorOccurred = true;
                    }
                }
            }

            return errorOccurred;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="script"></param>
        /// <param name="fileManager"></param>
        /// <returns></returns>
        public static bool ApplyScript(Script script, ref FileManager fileManager)
        {
            #region Table Modifications
            if (script.Tables != null)
            {
                foreach (Script.Table table in script.Tables)
                {
                    string tableID = table.ID.ToUpper();
                    if (fileManager.DataFiles.ContainsKey(tableID) == false)
                    {
                        Console.WriteLine(String.Format("Error: The table id is incorrect or does not exist: {0} ", tableID));
                        continue;
                    }

                    ExcelFile dataTable = fileManager.DataFiles[tableID] as ExcelFile;
                    if (dataTable == null)
                    {
                        Console.WriteLine(String.Format("Error: Could not open {0} due to null reference.", tableID));
                        continue;
                    }

                    foreach (Script.Table.Entity entity in table.Entities)
                    {
                        foreach (Script.Table.Entity.Attribute attribute in entity.Attributes)
                        {
                            int step = 0;
                            int min = 1;
                            int max = 0;
                            int last = 0; //for recursive function
                            string function;
                            int[] list;

                            try
                            {
                                //determine the range
                                if (entity.ID.Contains(","))
                                {
                                    string[] explode = entity.ID.Split(',');
                                    list = new int[explode.Length];

                                    for (int i = 0; i < list.Length; i++)
                                    {
                                        list[i] = Convert.ToInt32(explode[i]);
                                    }
                                }
                                else
                                {
                                    if (entity.ID.Contains("*"))
                                    {
                                        min = 0;
                                        max = dataTable.Count - 1;
                                    }
                                    else if (entity.ID.Contains("-"))
                                    {
                                        int idx = entity.ID.IndexOf('-');
                                        int len = entity.ID.Length - idx - 1;
                                        min = Convert.ToInt32(entity.ID.Substring(0, idx));
                                        max = Convert.ToInt32(entity.ID.Substring(idx + 1, len));
                                    }
                                    else
                                    {
                                        min = Convert.ToInt32(entity.ID);
                                        max = Convert.ToInt32(entity.ID);
                                    }

                                    int listlen = max - min + 1;
                                    list = new int[listlen];
                                    int i = 0;

                                    for (int row = min; row <= max; row++)
                                    {
                                        list[i++] = row;
                                    }
                                }

                                //determine function
                                if (attribute.Bit != null)
                                {
                                    function = "bitwise";
                                }
                                else if (attribute.Operation == null)
                                {
                                    function = "replace";
                                }
                                else if (attribute.Operation.Contains("*"))
                                {
                                    function = "multiply";
                                }
                                else if (attribute.Operation.Contains("/"))
                                {
                                    function = "divide";
                                }
                                else if (attribute.Operation.Contains("+"))
                                {
                                    string s = attribute.Operation.Remove(0);
                                    step = Convert.ToInt32(s);
                                    function = "recursive";
                                }
                                else if (attribute.Operation.Contains("-"))
                                {
                                    step = Convert.ToInt32(attribute.Operation);
                                    function = "recursive";
                                }
                                else
                                {
                                    continue; // syntax error
                                }

                                //main loop, alters the dataset
                                foreach (int row in list)
                                {
                                    object obj = null;
                                    string col = attribute.ID;
                                    Type type = dataTable.DataType.GetField(col).FieldType;//table.Columns[col].DataType;
                                    Object currentValue = dataTable.DataType.GetField(col).GetValue(dataTable.Rows[row]);

                                    switch (function)
                                    {
                                        case "replace":
                                            obj = FileTools.StringToObject(attribute.Data, type);
                                            break;

                                        case "multiply":
                                            if (type.Equals(typeof(int)))
                                                obj = (int)currentValue * Convert.ToInt32(attribute.Data);
                                            else if (type.Equals(typeof(float)))
                                                obj = (float)currentValue * Convert.ToSingle(attribute.Data);
                                            break;

                                        case "divide":
                                            if (type.Equals(typeof(int)))
                                                obj = (int)currentValue / Convert.ToInt32(attribute.Data);
                                            else if (type.Equals(typeof(float)))
                                                obj = (float)currentValue / Convert.ToSingle(attribute.Data);
                                            break;

                                        case "bitwise":
                                            uint bit = (uint)Enum.Parse(type, attribute.Bit, true);
                                            uint mask = (uint)currentValue;
                                            bool flick = Convert.ToBoolean(attribute.Data);
                                            bool current = (mask & bit) > 0;
                                            if (flick != current)
                                                obj = mask ^= bit;
                                            else
                                                obj = mask;
                                            break;

                                        case "recursive":
                                            if (row.Equals(min))//first time only
                                            {
                                                if (type.Equals(typeof(int)))
                                                {
                                                    obj = Convert.ToInt32(attribute.Data);
                                                    last = (int)obj;
                                                }
                                            }
                                            else
                                            {
                                                last += step;
                                                obj = last;
                                            }
                                            break;
                                    }
                                    dataTable.DataType.GetField(col).SetValue(dataTable.Rows[row], obj);
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionLogger.LogException(ex);
                                return false;
                            }
                        }
                    }
                    fileManager.DataFiles[tableID] = dataTable;
                }
            }
            #endregion

            #region Extraction Script
            if (script.Extraction != null)
            {
                string sourcePath = Path.Combine(fileManager.HellgateDataPath, script.Extraction.Source + ".idx");
                string destinationPath = sourcePath.Replace(script.Extraction.Source, script.Extraction.Destination);

                // Check source index exists
                if (File.Exists(sourcePath) == false) return false;

                // Try open the index file
                byte[] sbuffer;
                IndexFile sourceIndex;
                IndexFile destinationIndex;
                try
                {
                    sbuffer = File.ReadAllBytes(sourcePath);
                }
                catch (Exception ex)
                {
                    ExceptionLogger.LogException(ex);
                    return false;
                }
                sourceIndex = new IndexFile(sbuffer) { FilePath = sourcePath };
                if (sourceIndex.HasIntegrity == false)
                {
                    Console.WriteLine("Error: {0} has no integrity. Make sure you have run Hellgate London at least once (MP version for MP files) before installating this modification.", sourceIndex);
                    return false;
                }

                destinationIndex = new IndexFile() { FilePath = destinationPath };
                sourceIndex.BeginDatReading();

                // Extract each path/file
                foreach (FileEntry fileEntry in sourceIndex.Files)
                {
                    if (script.Extraction.paths.Where(p => Regex.IsMatch(fileEntry.RelativeFullPath, p.Replace(@"\", @"\\"))).Any())
                    {
                        byte[] fileBytes = sourceIndex.GetFileBytes(fileEntry);
                        //if (fileEntry.FileNameString.Contains(XmlCookedFile.FileExtention))
                        //{
                        //    try
                        //    {
                        //        XmlCookedFile xmlCookedFile = new XmlCookedFile();
                        //        xmlCookedFile.Uncook(fileBytes);

                        //        XmlCookedFile reCooked = new XmlCookedFile();
                        //        fileBytes = reCooked.CookXmlDocument(xmlCookedFile.XmlDoc);
                        //    }
                        //    catch (Exception) { }
                        //}

                        destinationIndex.AddFile(fileEntry.DirectoryString, fileEntry.FileNameString, fileBytes);
                    }
                }

                // Write the file
                byte[] ibuffer = destinationIndex.ToByteArray();
                Crypt.Encrypt(ibuffer);
                try
                {
                    File.WriteAllBytes(destinationIndex.FilePath, ibuffer);
                    sourceIndex.Dispose();
                    destinationIndex.Dispose();
                }
                catch (Exception ex)
                {
                    ExceptionLogger.LogException(ex);
                    sourceIndex.Dispose();
                    destinationIndex.Dispose();
                    return false;
                }
            }
            #endregion

            return true;
        }

        /// <summary>
        /// The basic root XML node.
        /// </summary>
        [XmlRoot("revival")]
        public class Revival
        {
            [XmlElement("modification", typeof(Modification))]
            public Modification Modifications { get; set; }

            /// <summary>
            /// This contains the entire modification.
            /// </summary>
            public class Modification
            {
                [XmlAttribute("id")]
                public string ID { get; set; }
                [XmlElement("release")]
                public string Release { get; set; }
                [XmlElement("version")]
                public string Version { get; set; }
                [XmlElement("author")]
                public string Author { get; set; }
                [XmlElement("website")]
                public string Website { get; set; }
                [XmlElement("email")]
                public string Email { get; set; }
                [XmlElement("revert")]
                public bool DoRevert { get; set; }
                [XmlElement("dependencies", typeof(Dependency))]
                public Dependency Dependencies { get; set; }
                [XmlElement("description")]
                public string Description { get; set; }
                [XmlElement("script", typeof(Script))]
                public Script[] Scripts { get; set; }

                /// <summary>
                /// List of prequisite dats that must be installed before this patch can be used.
                /// </summary>
                public class Dependency
                {
                    [XmlElement("patch")]
                    public string[] Patch { get; set; }
                }

                /// <summary>
                /// Used for executing commands dynamically, particularly designed for optional modifications.
                /// </summary>
                public class Script
                {
                    [XmlAttribute("type")]
                    public string Type { get; set; }
                    [XmlElement("title")]
                    public string Title { get; set; }
                    [XmlElement("description")]
                    public string Description { get; set; }
                    [XmlElement("table", typeof(Table))]
                    public Table[] Tables { get; set; }
                    [XmlElement("extract", typeof(Extract))]
                    public Extract Extraction { get; set; }

                    /// <summary>
                    /// Used for manipulating Excel tables.
                    /// </summary>
                    public class Table
                    {
                        [XmlAttribute("id")]
                        public string ID { get; set; }
                        [XmlElement("entity", typeof(Entity))]
                        public Entity[] Entities { get; set; }

                        public class Entity
                        {
                            [XmlAttribute("id")]
                            public string ID { get; set; }
                            [XmlElement("attribute", typeof(Attribute))]
                            public Attribute[] Attributes { get; set; }

                            public class Attribute
                            {
                                [XmlAttribute("id")]
                                public string ID { get; set; }
                                [XmlAttribute("operation")]
                                public string Operation { get; set; }
                                [XmlAttribute("bit")]
                                public string Bit { get; set; }
                                [XmlText]
                                public string Data { get; set; }
                            }
                        }
                    }

                    /// <summary>
                    /// Used for extracting and repacking existing dats. Designed for use with the MP archives.
                    /// </summary>
                    public class Extract
                    {
                        [XmlAttribute("source")]
                        public string Source { get; set; }
                        [XmlAttribute("destination")]
                        public string Destination { get; set; }
                        [XmlElement("path")]
                        public string[] paths;
                    }

                    public override string ToString()
                    {
                        return Title;
                    }
                }
            }
        }

    }
}
