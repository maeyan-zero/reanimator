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

        TableDataSet dataSet;
        ExcelTables excelTables;

        List<ExcelTable> excelList;
        List<String> loadedExcelList;

        List<Index> indexList;
        List<String> loadedIndexList;

        public Mod(String revivalModPath)
        {
            this.revival = Deserialize(revivalModPath);

            this.excelList = new List<ExcelTable>();
            this.loadedExcelList = new List<String>();

            this.indexList = new List<Index>();
            this.loadedIndexList = new List<String>();
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
                xsdStream = new FileStream("ModSchema.xsd", FileMode.Open);

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

        public void Apply(Forms.ProgressForm progress)
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
                                    int fileIndexNo = indexList[pack.listId].Locate(file.id + ".txt.cooked");
                                    if (fileIndexNo == -1)
                                    {
                                        fileIndexNo = indexList[pack.listId].Locate(file.id.Replace("_", "") + ".txt.cooked");
                                    }
                                    if (fileIndexNo != -1)
                                    {
                                        excelList.Add(excelTables._excelTables.CreateTable(file.id, indexList[pack.listId].ReadDataFile(fileIndex[fileIndexNo])));
                                        loadedExcelList.Add(file.id);
                                        file.listId = loadedExcelList.Count - 1;
                                        //dataSet.LoadTable(progressForm, excelList[file.listId]);
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
                                        if (entity.id == "*")
                                        {
                                            for (int i = 0; i < dataSet.XlsDataSet.Tables[file.id].Rows.Count; i++)
                                            {
                                                ModLogic(dataSet, file.id, attribute, i);
                                            }
                                        }
                                        else
                                        {
                                            ModLogic(dataSet, file.id, attribute, entity.value);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Save(string path, Method method)
        {
            foreach (ExcelTable excelFile in excelList)
            {
                //byte[] excelFileData = dataSet.ExcelTables.GetTable(file.id).GenerateExcelFile(dataSet.XlsDataSet);

                //using (FileStream fs = new FileStream("test.txt.cooked", FileMode.Create, FileAccess.ReadWrite))
                //{
                //    fs.Write(excelFileData, 0, excelFileData.Length);
                //}
            }

            foreach (Index indexFile in indexList)
            {
                switch (method)
                {
                    case Method.EXTRACT:
                        
                        break;
                    case Method.REPACK:

                        break;
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
                    //TODO
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

            [XmlIgnoreAttribute]
            public int listId;
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
            public int value;

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