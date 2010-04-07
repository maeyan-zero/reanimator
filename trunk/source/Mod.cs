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
    public class Mod
    {
        static public readonly string defaultPack = "sp_hellgate_1.10.180.3416_1.18074.70.4256";
        static private readonly int excelTablesIndex = 2572;


        public enum Method { EXTRACT, REPACK }

        Revival revival;
        String directory;

        TableDataSet dataSet;
        ExcelTables excelTables;

        List<ExcelTable> excelList;
        List<String> loadedExcelList;
        List<String> savedExcelList;

        List<Index> indexList;
        List<String> loadedIndexList;
        List<String> savedIndexList;

        public Mod(String revivalModPath)
        {
            this.revival = Deserialize(revivalModPath);
            this.directory = revivalModPath.Substring(0, revivalModPath.LastIndexOf("\\"));
            this.dataSet = new TableDataSet();

            this.excelList = new List<ExcelTable>();
            this.loadedExcelList = new List<String>();
            this.savedExcelList = new List<String>();

            this.indexList = new List<Index>();
            this.loadedIndexList = new List<String>();
            this.savedIndexList = new List<String>();
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

        public static bool Parse(string xmlPath)
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
                xmlStream = new FileStream(xmlPath, FileMode.Open);

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

        public void Apply(Forms.ProgressForm progress, Object argument)
        {
            foreach (Modification modification in revival)
            {
                if (modification.apply == true)
                {
                    foreach (Pack pack in modification)
                    {
                        try
                        {
                            if (pack.id == "Default" || pack.id == "default")
                            {
                                pack.id = defaultPack;
                            }
                            if (loadedIndexList.Contains(pack.id) == false)
                            {
                                String path = Config.HglDir + "\\data\\" + pack.id + ".idx";
                                FileStream stream = new FileStream(@path, FileMode.Open);
                                indexList.Add(new Index(stream));
                                stream.Close();
                                loadedIndexList.Add(pack.id);
                                pack.listId = loadedIndexList.Count - 1;
                            }
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Error reading IDX file. Details: " + Environment.NewLine + e.ToString(), "IDX Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        Index.FileIndex[] fileIndex = indexList[pack.listId].GetFileTable();

                        foreach (File file in pack)
                        {
                            try
                            {
                                if (excelTables == null)
                                {
                                    if (indexList[pack.listId].DatFileOpen == false)
                                    {
                                        if (indexList[pack.listId].OpenAccompanyingDat() == false)
                                        {
                                            MessageBox.Show("Could not open accompying DAT.");
                                        }
                                    }

                                    excelTables = new ExcelTables(indexList[pack.listId].ReadDataFile(fileIndex[excelTablesIndex]));
                                }
                                if (loadedExcelList.Contains(file.id) == false)
                                {
                                    file.indexId = indexList[pack.listId].Locate(file.id + ".txt.cooked");
                                    if (file.indexId == -1)
                                    {
                                        file.indexId = indexList[pack.listId].Locate(file.id.Replace("_", "") + ".txt.cooked");
                                    }
                                    if (file.indexId != -1)
                                    {
                                        file.tableRef = excelTables.TableManager.ResolveTableId(file.id.Replace(".txt.cooked", ""));
                                        excelList.Add(excelTables.TableManager.CreateTable(file.tableRef, indexList[pack.listId].ReadDataFile(fileIndex[file.indexId])));
                                        loadedExcelList.Add(file.id);
                                        file.listId = loadedExcelList.Count - 1;
                                        dataSet.LoadTable(progress, excelList[file.listId]);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Could not locate file: " + file.id);
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("Error reading DAT file. Details: " + Environment.NewLine + e.ToString(), "DAT Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                            if (file.modify != null)
                            {
                                foreach (Entity entity in file.modify)
                                {
                                    foreach (Attribute attribute in entity)
                                    {
                                        try
                                        {
                                            if (entity.id == "*")
                                            {
                                                if (entity.value != null)
                                                {
                                                    int indexVal = entity.value.IndexOf('-');
                                                    int startVal = Convert.ToInt32(entity.value.Substring(0, indexVal));
                                                    int lastVal = Convert.ToInt32(entity.value.Substring(indexVal + 1, entity.value.Length - (indexVal + 1)));

                                                    for (int i = startVal; i <= lastVal; i++)
                                                    {
                                                        ModLogic(dataSet, file.tableRef, attribute, i);
                                                    }
                                                }
                                                else
                                                {
                                                    for (int i = 0; i < dataSet.XlsDataSet.Tables[file.tableRef].Rows.Count; i++)
                                                    {
                                                        ModLogic(dataSet, file.tableRef, attribute, i);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                ModLogic(dataSet, file.tableRef, attribute, Convert.ToInt32(entity.value));
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            MessageBox.Show("There seems to be an error in the entity syntax. Details: " + e.ToString());
                                        }
                                    }
                                }
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
                            progress.SetCurrentItemText(file.id);

                            // Its an Excel File
                            if (file.modify != null)
                            {
                                // Check if its already saved
                                if (savedExcelList.Contains(file.id) == false && file.tableRef != null)
                                {
                                    byte[] excelBytes = excelList[file.listId].GenerateExcelFile(dataSet.XlsDataSet);
                                    string dir = Config.HglDir + "\\" + indexList[pack.listId].FileTable[file.indexId].DirectoryString;
                                    string filename = excelTables.TableManager.GetReplacement(file.id);

                                    if (Directory.Exists(dir) == false) Directory.CreateDirectory(dir);

                                    using (FileStream fs = new FileStream(dir + filename, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                                    {
                                        fs.Write(excelBytes, 0, excelBytes.Length);
                                        savedExcelList.Add(file.id);
                                    }
                                }
                            }
                            // Its not an Excel file
                            else if (file.replace != null)
                            {
                                string source = this.directory + "\\" + file.replace.data;
                                string destination = Config.HglDir + "\\" + indexList[pack.listId].FileTable[file.indexId].DirectoryString;

                                try
                                {
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
                                catch (Exception e)
                                {
                                    MessageBox.Show(e.ToString());
                                }
                            }

                            // Modify the index if it isn't already
                            if (indexList[pack.listId].FileTable[file.indexId].IsModified() == false)
                            {
                                indexList[pack.listId].RemoveFromIndex(file.indexId);
                            }
                        }
                    }
                }
            }

            // Save the modified index files
            foreach (Index indx in indexList)
            {
                using (FileStream fs = new FileStream(Config.HglDir + "\\data\\" + indx.FileName + ".idx", FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    byte[] buffer = indx.GenerateIndexFile();
                    Crypt.Encrypt(buffer);
                    fs.Write(buffer, 0, buffer.Length);
                }
            }
        }

        private void ModLogic(TableDataSet dataSet, String file, Attribute attribute, int row)
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
            public int listId;

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
            public int listId;

            [XmlIgnoreAttribute]
            public int indexId;

            [XmlIgnoreAttribute]
            public string tableRef;
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
    }
}