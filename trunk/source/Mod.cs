using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Data;
using Reanimator.Excel;

namespace Reanimator
{
    public class Mod : IDisposable
    {
        Revival revival;
        String directory;
        Index[] index;
        TableDataSet data_set;
        ExcelTables excel_tables;

        List<ExcelTable> excel_list;
        List<String> loaded_excel_list;
        List<String> saved_excel_list;

        public Mod(string path, Index[] index)
        {
            this.revival = Deserialize(path);
            this.directory = path.Substring(0, path.LastIndexOf("\\"));
            this.index = index;
            this.data_set = new TableDataSet();

            this.excel_list = new List<ExcelTable>();
            this.loaded_excel_list = new List<String>();
            this.saved_excel_list = new List<String>();
        }

        public void Add(Mod mod)
        {
            foreach (Modification newMod in mod.revival)
            {
                if (revival.modification.Contains(newMod) == false)
                {
                    Modification[] newModSet = new Modification[revival.modification.Length + 1];
                    revival.modification.CopyTo(newModSet, 0);
                    newModSet[newModSet.Length - 1] = newMod;
                    revival.modification = newModSet;
                }
            }
        }

        public static bool Parse(string path)
        {
            try
            {
                FileStream xsdStream;
                xsdStream = new FileStream("Schema.xsd", FileMode.Open);

                XmlSchema xmlSchema;
                xmlSchema = XmlSchema.Read(xsdStream, null);

                XmlReaderSettings xmlReaderSettings;
                xmlReaderSettings = new XmlReaderSettings()
                {
                    ValidationType = ValidationType.Schema,
                    ProhibitDtd = false
                };

                xmlReaderSettings.Schemas.Add(xmlSchema);

                FileStream xmlStream;
                xmlStream = new FileStream(path, FileMode.Open);

                XmlReader xmlReader;
                xmlReader = XmlReader.Create(xmlStream, xmlReaderSettings);

                // Parse the document
                using (xmlReader)
                {
                    while (xmlReader.Read()) { }
                }
    
                xsdStream.Close();
                xmlStream.Close();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                MessageBox.Show(e.Message);

                return false;
            }
        }

        public void Serialize(string path)
        {
            XmlSerializer s = new XmlSerializer(typeof(Revival));
            TextWriter w = new StreamWriter(path);
            s.Serialize(w, revival);
            w.Close();
        }

        public Revival Deserialize(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Revival));
            TextReader tr = new StreamReader(path);
            Revival revival = (Revival)serializer.Deserialize(tr);
            tr.Close();

            return revival;
        }

        // To be used with a progress form only.
        public void Apply(Forms.ProgressForm progress, Object argument)
        {
            foreach (Modification modification in revival)
            {
                // Only continue if the modification has been enabled through the GUI.
                if (modification.apply == true)
                {
                    foreach (Pack pack in modification)
                    {
                        // Find the packs index files indice
                        for (int i = 0; i < Index.FileNames.Length; i++)
                        {
                            if (pack.id == Index.FileNames[i])
                            {
                                pack.list_id = i;
                                break;
                            }
                        }

                        foreach (File file in pack)
                        {
                            try
                            {
                                // If the file is an Excel type, extract it.
                                if (file.id.Contains(".txt.cooked"))
                                {
                                    // Check if the ExcelTables class has been initialized yet.
                                    if (excel_tables == null)
                                    {
                                        Index.FileIndex file_index = index[Index.LatestPatch].FileTable[Index.ExcelTablesIndex];
                                        // Get the latest version of this file from the patch file.
                                        excel_tables = new ExcelTables(index[Index.LatestPatch].ReadDataFile(file_index));
                                    }

                                    // Extract the file if it hasn't been already.
                                    if (loaded_excel_list.Contains(file.id) == false)
                                    {
                                        // Find the excel file inside the latest patch.
                                        file.index_id = index[Index.LatestPatch].Locate(file.id);

                                        if (file.index_id != -1)
                                        {
                                            // Resolve the correct table reference
                                            file.table_ref = excel_tables.TableManager.ResolveTableId(file.id.Replace(".txt.cooked", ""));
                                            file.index_id_patch = index[Index.LatestPatch].Locate(file.id);
                                            // Reference the file index
                                            Index.FileIndex file_index = index[Index.LatestPatch].FileTable[file.index_id_patch];
                                            // Extract the file
                                            excel_list.Add(excel_tables.TableManager.CreateTable(file.table_ref, index[Index.LatestPatch].ReadDataFile(file_index)));
                                            // Record the file has been loaded
                                            loaded_excel_list.Add(file.id);
                                            // Record the indice
                                            file.list_id = loaded_excel_list.Count - 1;
                                            // Add the table to the data set
                                            data_set.LoadTable(progress, excel_list[file.list_id]);
                                        }
                                        else
                                        {
                                            throw new Exception("Could not locate file: " + file.id);
                                        }
                                    }
                                }

                                // If a modification script has been wrote, apply it.
                                if (file.modify != null)
                                {
                                    // Only Excel files can be modified
                                    if (file.id.Contains(".txt.cooked"))
                                    {
                                        foreach (Entity entity in file.modify)
                                        {
                                            foreach (Attribute attribute in entity)
                                            {
                                                // It's a wildcard
                                                if (entity.id == "*")
                                                {
                                                    // If a value is specified, then a range has been defined
                                                    if (entity.value != null)
                                                    {
                                                        // Feed the values through the Logic function
                                                        if (entity.value.Contains("-"))
                                                        {
                                                            int index_val = entity.value.IndexOf('-');
                                                            int start_val = Convert.ToInt32(entity.value.Substring(0, index_val));
                                                            int last_val = Convert.ToInt32(entity.value.Substring(index_val + 1, entity.value.Length - (index_val + 1)));

                                                            for (int i = start_val; i <= last_val; i++)
                                                            {
                                                                Logic(data_set, file.table_ref, attribute, i);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            throw new Exception("Syntax Error. Value must be null or a range when using a wildcard.");
                                                        }
                                                    }
                                                    // Modify all rows.
                                                    else
                                                    {
                                                        for (int i = 0; i < data_set.XlsDataSet.Tables[file.table_ref].Rows.Count; i++)
                                                        {
                                                            Logic(data_set, file.table_ref, attribute, i);
                                                        }
                                                    }
                                                }
                                                // It's a specific row. ie. 3
                                                else
                                                {
                                                    Logic(data_set, file.table_ref, attribute, Convert.ToInt32(entity.value));
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("Only excel files can be 'modified'");
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show(e.ToString());
                            }
                        }
                    }
                }
            }
        }

        public void Save(Forms.ProgressForm progress, Object argument)
        {
            progress.SetLoadingText("Modifying and Saving all files.");

            foreach (Modification modification in revival)
            {
                if (modification.apply == true)
                {
                    foreach (Pack pack in modification)
                    {
                        foreach (File file in pack)
                        {
                            try
                            {
                                file.index_id = index[pack.list_id].Locate(file.id);

                                if (index[Index.LatestPatch].Locate(file.id) != -1)
                                {
                                    file.patched = true;
                                    file.index_id_patch = index[Index.LatestPatch].Locate(file.id);
                                }

                                progress.SetCurrentItemText(file.id);

                                // Its an Excel file
                                if (file.modify != null)
                                {
                                    // Check if its already saved
                                    if (saved_excel_list.Contains(file.id) == false && file.table_ref != null)
                                    {
                                        byte[] excelBytes = excel_list[file.list_id].GenerateExcelFile(data_set.XlsDataSet);
                                        string dir = Config.HglDir + "\\" + index[Index.LatestPatch].FileTable[file.index_id_patch].DirectoryString;
                                        string filename = excel_tables.TableManager.GetReplacement(file.id);

                                        if (Directory.Exists(dir) == false) Directory.CreateDirectory(dir);

                                        using (FileStream fs = new FileStream(dir + filename, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                                        {
                                            fs.Write(excelBytes, 0, excelBytes.Length);
                                            saved_excel_list.Add(file.id);
                                        }
                                    }
                                }

                                // Its not an Excel file
                                if (file.replace != null)
                                {
                                    string source = this.directory + "\\" + file.replace.data;
                                    string destination = Config.HglDir + "\\" + index[pack.list_id].FileTable[file.index_id].DirectoryString;

                                    if (System.IO.File.Exists(source) == false)
                                    {
                                        throw new Exception("Source file not found");
                                    }
                                    if (Directory.Exists(destination) == false)
                                    {
                                        Directory.CreateDirectory(destination);
                                    }
                                    System.IO.File.Copy(source, destination + file.id, true);
                                }

                                // Modify the index if it isn't already
                                if (index[pack.list_id].FileTable[file.index_id].Modified == false)
                                {
                                    index[pack.list_id].AppendDirectorySuffix(file.index_id);

                                    // If the file is inside the patch file, modify this too.
                                    if (file.patched == true)
                                    {
                                        index[Index.LatestPatch].AppendDirectorySuffix(file.index_id_patch);
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show(e.ToString());
                            }
                        }
                    }
                }
            }

            // Save the modified index files
            foreach (Index indx in index)
            {
                if (indx.Modified == true)
                {
                    using (FileStream fs = new FileStream(Config.HglDir + "\\data\\" + indx.FileName + ".idx", FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        byte[] buffer = indx.GenerateIndexFile();
                        Crypt.Encrypt(buffer);
                        fs.Write(buffer, 0, buffer.Length);
                    }
                }
            }
        }

        private void Logic(TableDataSet dataSet, String file, Attribute attribute, int row)
        {
            try
            {
                if (attribute.replace != null)
                {
                    if (dataSet.XlsDataSet.Tables[file].Rows[row][attribute.id].GetType() == typeof(Int32))
                    {
                        dataSet.XlsDataSet.Tables[file].Rows[row][attribute.id] = Convert.ToUInt32(attribute.replace.data);
                    }
                    else if (dataSet.XlsDataSet.Tables[file].Rows[row][attribute.id].GetType() == typeof(float))
                    {
                        dataSet.XlsDataSet.Tables[file].Rows[row][attribute.id] = Convert.ToSingle(attribute.replace.data);
                    }
                    else if (dataSet.XlsDataSet.Tables[file].Rows[row][attribute.id].GetType() == typeof(string))
                    {
                        dataSet.XlsDataSet.Tables[file].Rows[row][attribute.id] = attribute.replace.data;
                    }
                }
                else if (attribute.bitwise != null)
                {
                    //for (int j = 0; j < attribute.bitwise.Length; j++)
                    //{
                    //    string[] bitmask;

                    //    switch (attribute.id)
                    //    {
                    //        case "Bitmask01":
                    //            bitmask = Enum.GetNames(Items.BitMask01);
                    //            break;
                    //        case "Bitmask02":
                    //            //bitmask = Enum.GetNames(Items.BitMask02);
                    //            break;
                    //        case "Bitmask03":
                    //            //bitmask = Enum.GetNames(Items.BitMask03);
                    //            break;
                    //        case "Bitmask04":
                    //            //bitmask = Enum.GetNames(Items.BitMask04);
                    //            break;
                    //        case "Bitmask05":
                    //            //bitmask = Enum.GetNames(Items.BitMask05);
                    //            break;
                    //    }

                    //}
                    //for (int i = 0; i < bitmask.Length; i++)
                    //{
                    //    if (string.Compare(attribute.bitwise[j].id, bitmask[j], true))
                    //    {
                    //        if (dataSet.XlsDataSet.Tables[file].Rows[row][attribute.id] & (j ^ 32))
                    //        {

                    //        }
                    //    }
                    //}
                }
                else if (attribute.divide != null)
                {
                    if (dataSet.XlsDataSet.Tables[file].Rows[row][attribute.id].GetType() == typeof(Int32))
                    {
                        dataSet.XlsDataSet.Tables[file].Rows[row][attribute.id] = Convert.ToInt32(dataSet.XlsDataSet.Tables[file].Rows[row][attribute.id]) / Convert.ToInt32(attribute.divide.data);
                    }
                    else if (dataSet.XlsDataSet.Tables[file].Rows[row][attribute.id].GetType() == typeof(float))
                    {
                        dataSet.XlsDataSet.Tables[file].Rows[row][attribute.id] = Convert.ToSingle(dataSet.XlsDataSet.Tables[file].Rows[row][attribute.id]) / Convert.ToSingle(attribute.divide.data);
                    }
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public int Length
        {
            get
            {
                return revival.modification.Count();
            }
        }

        public string getTitle(int i)
        {
            return revival.modification[i].title;
        }

        public string getDescription(int i)
        {
             return revival.modification[i].GetListDescription();
        }

        public bool getEnabled(int i)
        {
            return revival.modification[i].type == "required" ? true : false;
        }

        public void setApply(int i, bool use)
        {
            revival.modification[i].apply = use;
        }

        [XmlRoot("revival")]
        public class Revival
        {
            [XmlElement(typeof(Modification))]
            public Modification[] modification;

            public MyEnumerator GetEnumerator()
            {
                return new MyEnumerator(this);
            }

            public class MyEnumerator
            {
                int nIndex;
                Revival collection;
                public MyEnumerator(Revival coll)
                {
                    collection = coll;
                    nIndex = -1;
                }

                public bool MoveNext()
                {
                    nIndex++;
                    return (nIndex < collection.modification.GetLength(0));
                }

                public Modification Current
                {
                    get
                    {
                        return (collection.modification[nIndex]);
                    }
                }
            }
        }

        public class Modification
        {
            [XmlElement]
            public string title;
            [XmlElement]
            public string version;
            [XmlElement]
            public string author;
            [XmlElement]
            public string description;
            [XmlElement]
            public string url;
            [XmlElement]
            public string type;
            [XmlElement(typeof(Pack))]
            public Pack[] pack;

            [XmlIgnoreAttribute]
            public bool apply;

            public string GetListDescription()
            {
                return
                    "Title: " + title + Environment.NewLine +
                    "Version: " + version + Environment.NewLine +
                    "Url: " + url + Environment.NewLine +
                    "Type: " + type + Environment.NewLine +
                    "Description: " + description;
            }

            public bool GetListEnable()
            {
                if (type == "required")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public MyEnumerator GetEnumerator()
            {
                return new MyEnumerator(this);
            }

            public class MyEnumerator
            {
                int nIndex;
                Modification collection;
                public MyEnumerator(Modification coll)
                {
                    collection = coll;
                    nIndex = -1;
                }

                public bool MoveNext()
                {
                    nIndex++;
                    return (nIndex < collection.pack.GetLength(0));
                }

                public Pack Current
                {
                    get
                    {
                        return (collection.pack[nIndex]);
                    }
                }
            }
        }

        public class Pack
        {
            [XmlAttribute]
            public string id;

            [XmlElement(typeof(File))]
            public File[] file;

            [XmlIgnoreAttribute]
            public int list_id;

            public MyEnumerator GetEnumerator()
            {
                return new MyEnumerator(this);
            }

            public class MyEnumerator
            {
                int nIndex;
                Pack collection;
                public MyEnumerator(Pack coll)
                {
                    collection = coll;
                    nIndex = -1;
                }

                public bool MoveNext()
                {
                    nIndex++;
                    return (nIndex < collection.file.GetLength(0));
                }

                public File Current
                {
                    get
                    {
                        return (collection.file[nIndex]);
                    }
                }
            }
        }

        public class File
        {
            [XmlAttribute]
            public string id;

            [XmlElement(typeof(Modify))]
            public Modify modify;

            [XmlElement(typeof(Replace))]
            public Replace replace;

            [XmlIgnoreAttribute]
            public int list_id;

            [XmlIgnoreAttribute]
            public int index_id;

            [XmlIgnoreAttribute]
            public int index_id_patch;

            [XmlIgnoreAttribute]
            public string table_ref;

            [XmlIgnoreAttribute]
            public bool patched;
        }

        public class Modify
        {
            [XmlElement(typeof(Entity))]
            public Entity[] entity;

            public MyEnumerator GetEnumerator()
            {
                return new MyEnumerator(this);
            }

            public class MyEnumerator
            {
                int nIndex;
                Modify collection;
                public MyEnumerator(Modify coll)
                {
                    collection = coll;
                    nIndex = -1;
                }

                public bool MoveNext()
                {
                    nIndex++;
                    return (nIndex < collection.entity.GetLength(0));
                }

                public Entity Current
                {
                    get
                    {
                        return (collection.entity[nIndex]);
                    }
                }
            }
        }

        public class Entity
        {
            [XmlAttribute]
            public string id;

            [XmlAttribute]
            public string value;

            [XmlIgnoreAttribute]
            public int min;

            [XmlIgnoreAttribute]
            public int max;

            [XmlElement(typeof(Attribute))]
            public Attribute[] attribute;

            public MyEnumerator GetEnumerator()
            {
                return new MyEnumerator(this);
            }

            public class MyEnumerator
            {
                int nIndex;
                Entity collection;
                public MyEnumerator(Entity coll)
                {
                    collection = coll;
                    nIndex = -1;
                }

                public bool MoveNext()
                {
                    nIndex++;
                    return (nIndex < collection.attribute.GetLength(0));
                }

                public Attribute Current
                {
                    get
                    {
                        return (collection.attribute[nIndex]);
                    }
                }
            }
        }

        public class Attribute
        {
            [XmlAttribute]
            public string id;

            [XmlElement(typeof(Replace))]
            public Replace replace;

            [XmlElement(typeof(Divide))]
            public Divide divide;

            [XmlElement(typeof(Bitwise))]
            public Bitwise[] bitwise;
        }

        public class Bitwise
        {
            [XmlAttribute]
            public string id;

            [XmlElement("switch")]
            public bool switch_data;
        }

        public class Replace
        {
            [XmlElement("data")]
            public string data;
        }
        
        public class Divide
        {
            [XmlElement("data")]
            public string data;
        }

        public void Dispose()
        {

        }
    }
}
