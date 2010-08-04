using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using Reanimator.ExcelDefinitions;
using Reanimator.Forms;
using System.Data;

namespace Reanimator
{
    public class Modification : IDisposable
    {
        TableDataSet _tableDataSet;
        List<string> _changedTables;
        Revival _revival;
        Index[] _indexFiles;

        public Modification(TableDataSet tableDataSet)
        {
            _tableDataSet = tableDataSet;
            _changedTables = new List<string>();
            _revival = new Revival();
            _indexFiles = new Index[Index.FileNames.Length];
        }
        public static bool Parse(string path)
        {
            try
            {
                FileStream xsdStream = new FileStream("schema.xsd", FileMode.Open);
                XmlSchema xmlSchema = XmlSchema.Read(xsdStream, null);
                XmlReaderSettings xmlReaderSettings = new XmlReaderSettings()
                {
                    ValidationType = ValidationType.Schema,
                    ProhibitDtd = false
                };
                xmlReaderSettings.Schemas.Add(xmlSchema);
                FileStream xmlStream = new FileStream(path, FileMode.Open);
                XmlReader xmlReader = XmlReader.Create(xmlStream, xmlReaderSettings);

                // Parse the document
                using (xmlReader) while (xmlReader.Read()) { }

                xsdStream.Close();
                xmlStream.Close();

                return true;
            }
            catch
            {
                MessageBox.Show("Either the schema.xsd could not be found, or there is a syntax error in the modification.",
                    "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        public void Open(ProgressForm progress, Object argument)
        {
            int i = ((string)argument).LastIndexOf("\\") + 1;
            string s = ((string)argument).Substring(i);
            progress.SetLoadingText("Parsing modification:");
            progress.SetCurrentItemText(s);

            //_index = Index.LoadIndexFiles(Config.HglDir + "\\data\\");
            XmlSerializer serializer = new XmlSerializer(typeof(Revival));
            TextReader reader = new StreamReader((string)argument);
            _revival = (Revival)serializer.Deserialize(reader);
            reader.Close();
        }
        public void Add(ProgressForm progress, Object argument)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Revival));
            TextReader reader = new StreamReader((string)argument);
            Revival buffer = (Revival)serializer.Deserialize(reader); ;
            Content[] content = buffer.modification;
            int current = _revival.modification.Length;
            int extra = content.Length;
            Array.Resize(ref _revival.modification, current + extra);
            Array.Copy(content, 0, _revival.modification, current, extra);
        }
        public void Apply(ProgressForm progress, Object argument)
        {
            foreach (Content mod in _revival.modification)
            {
                //if (!mod.apply) continue;
                foreach (Pack pack in mod.pack)
                {
                    foreach (File file in pack.file)
                    {
                        progress.SetLoadingText("Modifying: " + file.id);
                        Manipulate(file);
                        _changedTables.Add(file.table_ref);
                    }
                }
            }
        }
        public void Save(ProgressForm progress, Object argument)
        {

        }
        public void Unzip(ProgressForm progress, Object argument)
        {
            int i = ((string)argument).LastIndexOf("\\") + 1;
            string s = ((string)argument).Substring(i);
            progress.SetLoadingText("Uncompressing modification.");
            progress.SetCurrentItemText(s);

            using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile((string)argument))
            {
                zip.ExtractAll(Config.HglDir + "\\modpacks\\",
                    Ionic.Zip.ExtractExistingFileAction.OverwriteSilently);
            }
        }
        public bool Manipulate(File file)
        {
            try
            {
                int p = file.id.IndexOf(ExcelFile.FileExtention) - 1;
                string fileName = file.id.Remove(p);
                string tableName = _tableDataSet.TableFiles.GetStringIdFromFileName(fileName);

                DataTable table = _tableDataSet.XlsDataSet.Tables[tableName];
                ExcelFile excel = _tableDataSet.TableFiles.GetExcelTableFromId(tableName);

                foreach (Entity entity in file.modify.entity)
                {
                    foreach (Attribute attribute in entity)
                    {
                        int min;
                        int max;
                        int last = 0; //for recursive function
                        string function = attribute.GetFunctionString();

                        //determine the range/recursion if any
                        if (entity.id.Equals("*"))
                        {
                            if (entity.value != null)
                            {
                                if (entity.value.Contains("-"))
                                {
                                    int idx = entity.value.IndexOf('-');
                                    int len = entity.value.Length - idx - 1;
                                    min = Convert.ToInt32(entity.value.Substring(0, idx));
                                    max = Convert.ToInt32(entity.value.Substring(idx + 1, len));
                                }
                                else
                                {
                                    min = 0;
                                    max = table.Rows.Count - 1; // TODO fix this up. notice that same use is used twice
                                }
                            }
                            else
                            {
                                min = 0;
                                max = table.Rows.Count - 1;
                            }
                        }
                        else
                        {
                            min = Convert.ToInt32(entity.value);
                            max = min;
                        }

                        //main loop, alters the dataset
                        for (int i = min; i <= max; i++)
                        {
                            object obj = null;
                            string c = attribute.id;
                            Type type = table.Columns[attribute.id].DataType;
                            DataRow dataRow = table.Rows[i];

                            switch (function)
                            {
                                case "replace":
                                    obj = attribute.replace.data;
                                    break;
                                case "multiply":
                                    if (type.Equals(typeof(int)))
                                        obj = (int)dataRow[c] * Convert.ToInt32(attribute.divide.data);
                                    else if (type.Equals(typeof(float)))
                                        obj = (float)dataRow[c] * Convert.ToSingle(attribute.divide.data);
                                    break;
                                case "divide":
                                    if (type.Equals(typeof(int)))
                                        obj = (int)dataRow[c] / Convert.ToInt32(attribute.divide.data);
                                    else if (type.Equals(typeof(float)))
                                        obj = (float)dataRow[c] / Convert.ToSingle(attribute.divide.data);
                                    break;
                                case "bitwise":
                                    foreach (Bitwise bitwise in attribute.bitwise)
                                    {
                                        uint bit = (uint)Enum.Parse(type, bitwise.id, true);
                                        uint mask = (uint)dataRow[c];
                                        bool flick = bitwise.switch_data;
                                        if (flick != ((mask & bit) == 0))
                                            obj = (mask ^= bit);
                                        else
                                            obj = mask;
                                    }
                                    break;
                                case "recursive":
                                    if (i.Equals(min)) //first time only
                                    {
                                        last = Convert.ToInt32(attribute.recursive.data);
                                        if (type.Equals(typeof(int)))
                                            obj = Convert.ToInt32(attribute.recursive.data);
                                        else if (type.Equals(typeof(float)))
                                            obj = Convert.ToSingle(attribute.recursive.data);
                                    }
                                    else
                                    {
                                        if (type.Equals(typeof(int)))
                                            obj = last + Convert.ToInt32(attribute.recursive.step);
                                        else if (type.Equals(typeof(float)))
                                            obj = last + Convert.ToSingle(attribute.recursive.step);
                                    }
                                    break;
                            }

                            dataRow[c] = obj;
                        }
                        //end main loop
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        [XmlRoot("revival")]
        public class Revival
        {
            [XmlElement(typeof(Content))]
            public Content[] modification;

            public MyEnumerator GetEnumerator() { return new MyEnumerator(this); }

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
                public Content Current { get { return (collection.modification[nIndex]); } }
            }
        }
        public class Content
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
            [XmlElement]
            public String image;

            [XmlIgnoreAttribute]
            public Image png;
            [XmlIgnoreAttribute]
            public bool apply;

            public string GetListDescription()
            {
                return "Title: " + title + Environment.NewLine +
                    "Version: " + version + Environment.NewLine +
                    "Url: " + url + Environment.NewLine +
                    "Type: " + type + Environment.NewLine +
                    "Description: " + description;
            }

            public bool GetListEnable()
            {
                if (type == "required")
                    return true;
                else
                    return false;
            }

            public MyEnumerator GetEnumerator()
            { 
                return new MyEnumerator(this);
            }

            public class MyEnumerator
            {
                int nIndex;
                Content collection;
                public MyEnumerator(Content coll)
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
                    get { return (collection.pack[nIndex]); }
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

            public MyEnumerator GetEnumerator() { return new MyEnumerator(this); }

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
                public File Current { get { return (collection.file[nIndex]); } }
            }
        }
        public class File
        {
            [XmlAttribute]
            public string id;
            [XmlAttribute]
            public string dir;
            [XmlAttribute]
            public bool repack;
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
        }
        public class Modify
        {
            [XmlElement(typeof(Entity))]
            public Entity[] entity;
            [XmlAttribute]
            public bool repack;

            public MyEnumerator GetEnumerator() { return new MyEnumerator(this); }

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
                public Entity Current { get { return (collection.entity[nIndex]); } }
            }
        }
        public class Entity
        {
            [XmlAttribute]
            public string id;
            [XmlAttribute]
            public string value;
            [XmlElement(typeof(Attribute))]
            public Attribute[] attribute;

            [XmlIgnoreAttribute]
            public int min;
            [XmlIgnoreAttribute]
            public int max;

            public MyEnumerator GetEnumerator() { return new MyEnumerator(this); }

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
                public Attribute Current { get { return (collection.attribute[nIndex]); } }
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
            [XmlElement(typeof(Multiply))]
            public Multiply multiply;
            [XmlElement(typeof(Bitwise))]
            public Bitwise[] bitwise;
            [XmlElement(typeof(Recursive))]
            public Recursive recursive;

            public Type GetFunction()
            {
                if (replace != null) return typeof(Replace);
                if (divide != null) return typeof(Divide);
                if (multiply != null) return typeof(Multiply);
                if (bitwise != null) return typeof(Bitwise);
                if (recursive != null) return typeof(Recursive);
                else return null;
            }

            public string GetFunctionString()
            {
                if (replace != null) return "replace";
                if (divide != null) return "divide";
                if (multiply != null) return "multiply";
                if (bitwise != null) return "bitwise";
                if (recursive != null) return "recursive";
                else return null;
            }
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
        public class Multiply
        {
            [XmlElement("data")]
            public string data;
        }
        public class Recursive
        {
            [XmlAttribute]
            public int step;
            [XmlElement("data")]
            public string data;
        }
        public void Dispose()
        {

        }
    }
}
