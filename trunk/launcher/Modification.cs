using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Data;
using System.Linq;
using Hellgate;

namespace Revival
{
    public class Modification
    {
        public Revival Data { get; private set; }
        FileManager HellgateFileManager { get; set; }
        public bool IntegrityCheck { get { return Data != null ? true : false; } }

        public Modification(string path)
        {
            try
            {
                using (TextReader textReader = new StreamReader(path))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Revival));
                    Data = serializer.Deserialize(textReader) as Revival;
                }
            }
            catch
            {
                Console.WriteLine("Invalid or malformed XML structure.");
            }
        }

        //public void Open(string path)
        //{
        //    string modDir;

        //    if (path.EndsWith("zip"))
        //    {
        //        bool unzipResult = Unzip(path);
        //        if (!unzipResult) return;
        //        modDir = PathTools.FileNameFromPath(path, true);
        //        path = ModPackPath + modDir;
        //    }
        //    else
        //    {
        //        modDir = PathTools.FileNameFromPath(path, false);
        //    }

        //    string configPath = path + "\\Config.xml";

        //    // Open the Config file
        //    try
        //    {
        //        using (TextReader textReader = new StreamReader(configPath))
        //        {
        //            XmlSerializer serializer = new XmlSerializer(typeof(Package));
        //            Package package = serializer.Deserialize(textReader) as Package;
        //            ModPackage.Add(package);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLogger.LogException(ex, "Modification.Open", false);
        //        return;
        //    }

        //    Package casePackage = ModPackage.Last();
        //    if (casePackage == null) return;
        //    casePackage.Title = modDir;
        //    casePackage.Path = path;

        //    // Parse each pack
        //    foreach (Pack pack in casePackage.Pack)
        //    {
        //        pack.Apply = (pack.Type == "required" || pack.Type == "recommended") ? true : false;
        //        pack.Path = casePackage.Path + "\\" + pack.Dir + "\\";
        //        string imagePath = pack.Path + "preview.png";

        //        if (System.IO.File.Exists(imagePath))
        //        {
        //            pack.Image = Image.FromFile(imagePath);
        //        }

        //        string repackDir = pack.Path + "repack\\";
        //        if (Directory.Exists(repackDir))
        //        {
        //            pack.Repack = Directory.GetFiles(repackDir, "*", SearchOption.AllDirectories);
        //            for (int i = 0; i < pack.Repack.Length; i++)
        //            {
        //                pack.Repack[i] = pack.Repack[i].Remove(0, repackDir.Length);
        //            }
        //        }

        //        string unpackDir = pack.Path + "unpack\\";
        //        if (Directory.Exists(unpackDir))
        //        {
        //            pack.Unpack = Directory.GetFiles(unpackDir, "*", SearchOption.AllDirectories);
        //            for (int i = 0; i < pack.Unpack.Length; i++)
        //            {
        //                pack.Unpack[i] = pack.Unpack[i].Remove(0, unpackDir.Length);
        //            }
        //        }

        //        string scriptDir = pack.Path + "Scripts\\";
        //        if (!Directory.Exists(scriptDir)) continue; // no scripts? continue
        //        string[] scripts = Directory.GetFiles(scriptDir, "*.xml");
        //        pack.Scripts = new Script[scripts.Length];

        //        for (int i = 0; i < scripts.Length; i++)
        //        {
        //            // Open the script file
        //            try
        //            {
        //                using (TextReader textReader = new StreamReader(scripts[i]))
        //                {
        //                    XmlSerializer serializer = new XmlSerializer(typeof(Script));
        //                    Script script = serializer.Deserialize(textReader) as Script;
        //                    script.Title = PathTools.FileNameFromPath(scripts[i], false);
        //                    pack.Scripts[i] = script;
        //                    script.Path = scripts[i];
        //                }
        //            }
        //            catch (Exception exception)
        //            {
        //                ExceptionLogger.LogException(exception, "Modification.Open", false);
        //                return;
        //            }
        //        }
        //    }
        //}
        //public void Apply(ProgressForm progress, Object argument)
        //{
        //    Script script = argument as Script;
        //    if (script == null) return;

        //    foreach (File file in script.Files)
        //    {
        //        Manipulate(file);
        //    }

        //    //foreach (Package package in ModPackage)
        //    //{
        //    //    foreach (Pack pack in package.Pack)
        //    //    {
        //    //        if (!pack.Apply) continue;
                    
        //    //        //repack files

        //    //        //unpack files

        //    //        //manipulate files

        //    //    }
        //    //}
        //}

        //private bool Manipulate(File file)
        //{
        //    try
        //    {

        //        ExcelFile excel = null;// todo: rewrite  _tableDataSet.TableFiles.GetExcelTableFromId(file.ID);
        //        DataTable table = null;// todo: rewrite  _tableDataSet.XlsDataSet.Tables[file.ID];

        //        foreach (Entity entity in file.Entities)
        //        {
        //            foreach (Attribute attribute in entity.Attributes)
        //            {
        //                int step = 0;
        //                int min = 1;
        //                int max = 0;
        //                int last = 0; //for recursive function
        //                string function;
        //                int[] list;

        //                //determine the range
        //                if (entity.ID.Contains(","))
        //                {
        //                    string[] explode = entity.ID.Split(',');
        //                    list = new int[explode.Length];

        //                    for (int i = 0; i < list.Length; i++)
        //                    {
        //                        list[i] = Convert.ToInt32(explode[i]);
        //                    }
        //                }
        //                else
        //                {
        //                    if (entity.ID.Contains("*"))
        //                    {
        //                        min = 0;
        //                        max = table.Rows.Count - 1;
        //                    }
        //                    else if (entity.ID.Contains("-"))
        //                    {
        //                        int idx = entity.ID.IndexOf('-');
        //                        int len = entity.ID.Length - idx - 1;
        //                        min = Convert.ToInt32(entity.ID.Substring(0, idx));
        //                        max = Convert.ToInt32(entity.ID.Substring(idx + 1, len));
        //                    }
        //                    else
        //                    {
        //                        min = Convert.ToInt32(entity.ID);
        //                        max = Convert.ToInt32(entity.ID);
        //                    }

        //                    int listlen = max - min + 1;
        //                    list = new int[listlen];
        //                    int i = 0;

        //                    for (int row = min; row <= max; row++)
        //                    {
        //                        list[i++] = row;
        //                    }
        //                }

        //                //determine function
        //                if (attribute.Bit != null)
        //                {
        //                    function = "bitwise";
        //                }
        //                else if (attribute.Operation == null)
        //                {
        //                    function = "replace";
        //                }
        //                else if (attribute.Operation.Contains("*"))
        //                {
        //                    function = "multiply";
        //                }
        //                else if (attribute.Operation.Contains("/"))
        //                {
        //                    function = "divide";
        //                }
        //                else if (attribute.Operation.Contains("+"))
        //                {
        //                    string s = attribute.Operation.Remove(0);
        //                    step = Convert.ToInt32(s);
        //                    function = "recursive";
        //                }
        //                else if (attribute.Operation.Contains("-"))
        //                {
        //                    step = Convert.ToInt32(attribute.Operation);
        //                    function = "recursive";
        //                }
        //                else
        //                {
        //                    continue; // syntax error
        //                }

        //                //main loop, alters the dataset
        //                foreach (int row in list)
        //                {
        //                    object obj = null;
        //                    string col = attribute.ID;
        //                    Type type = table.Columns[col].DataType;
        //                    DataRow dataRow;

        //                    //add blank/reserved rows if required
        //                    while (row >= table.Rows.Count)
        //                    {
        //                        dataRow = table.NewRow();
        //                        table.Rows.Add(dataRow);
        //                    }

        //                    dataRow = table.Rows[row];

        //                    switch (function)
        //                    {
        //                        case "replace":
        //                            obj = attribute.Value;
        //                            break;

        //                        case "multiply":
        //                            if (type.Equals(typeof(int)))
        //                                obj = (int)dataRow[col] * Convert.ToInt32(attribute.Value);
        //                            else if (type.Equals(typeof(float)))
        //                                obj = (float)dataRow[col] * Convert.ToSingle(attribute.Value);
        //                            break;

        //                        case "divide":
        //                            if (type.Equals(typeof(int)))
        //                                obj = (int)dataRow[col] / Convert.ToInt32(attribute.Value);
        //                            else if (type.Equals(typeof(float)))
        //                                obj = (float)dataRow[col] / Convert.ToSingle(attribute.Value);
        //                            break;

        //                        case "bitwise":
        //                            uint bit = (uint)Enum.Parse(type, attribute.Bit, true);
        //                            uint mask = (uint)dataRow[col];
        //                            bool flick = Convert.ToBoolean(attribute.Value);
        //                            bool current = (mask & bit) > 0;
        //                            if (flick != current)
        //                                obj = mask ^= bit;
        //                            else
        //                                obj = mask;
        //                            break;

        //                        case "recursive":
        //                            if (row.Equals(min))//first time only
        //                            {
        //                                if (type.Equals(typeof(int)))
        //                                {
        //                                    obj = Convert.ToInt32(attribute.Value);
        //                                    last = (int)obj;
        //                                }
        //                            }
        //                            else 
        //                            {
        //                                last += step;
        //                                obj = last;
        //                            }
        //                            break;
        //                    }
        //                    dataRow[col] = obj ?? String.Empty;
        //                } //end main loop
        //            } //attribute
        //        } //entity

        //        return true;
        //    }
        //    catch (Exception exception)
        //    {
        //        ExceptionLogger.LogException(exception, "Modification.Manipulate", false);
        //        return false;
        //    }
        //}


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
