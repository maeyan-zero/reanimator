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
        Revival revival;
        TableDataSet dataSet;
        List<ExcelTable> modifiedTables;

        public Mod(TableDataSet dataSet, String revivalModPath)
        {
            this.dataSet = dataSet;
            this.revival = Deserialize(revivalModPath);
            modifiedTables = new List<ExcelTable>();
        }

        public static bool Parse(string xmlPath)
        {
            try
            {
                // Add the XSD FileStream
                FileStream xsdStream;
                xsdStream = new FileStream("..//..//..//source//Mods//ModSchema.xsd", FileMode.Open);
                //xsdStream = new FileStream("..//..//source//ModSchema.xsd", FileMode.Open);

                
                // Add the XML Schema using the above stream
                XmlSchema xmlSchema;
                xmlSchema = XmlSchema.Read(xsdStream, null);

                // Add XML Reader Settings
                XmlReaderSettings xmlReaderSettings;
                xmlReaderSettings = new XmlReaderSettings()
                {
                    ValidationType = ValidationType.Schema,
                    ProhibitDtd = false
                };

                // Append the Schema above
                xmlReaderSettings.Schemas.Add(xmlSchema);

                // Add the XML FileStream
                FileStream xmlStream;
                xmlStream = new FileStream(xmlPath, FileMode.Open);

                // Add the XML Reader using the stream and setting above
                XmlReader xmlReader;
                xmlReader = XmlReader.Create(xmlStream, xmlReaderSettings);

                // Parse the document
                using (xmlReader)
                {
                    while (xmlReader.Read())
                    {

                    }
                }
    
                xsdStream.Close();
                xmlStream.Close();

                return true;
            }
            catch (Exception e)
            {
                // Validation Failed
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

        public void Apply()
        {
            try
            {
                foreach (Modification modification in revival)
                {
                    if (modification.apply == true)
                    {
                        foreach (Pack pack in modification)
                        {
                            foreach (File file in pack)
                            {
                                if (file.modify != null)
                                {
                                    foreach (Entity entity in file.modify)
                                    {
                                        foreach (Attribute attribute in entity)
                                        {
                                            // Wildcard. Modify all entries
                                            if (entity.id == "*")
                                            {
                                                for (int i = 0; i < dataSet.XlsDataSet.Tables[file.id].Rows.Count; i++)
                                                {
                                                    ModLogic(file.id, attribute, i);
                                                }
                                            }
                                            // Modify a single entry
                                            else
                                            {
                                                ModLogic(file.id, attribute, entity.value);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // Add each modified Excel file to a list
                foreach (Modification modification in revival)
                {
                    if (modification.apply == true)
                    {
                        foreach (Pack pack in modification)
                        {
                            foreach (File file in pack)
                            {
                                // Don't save the same file twice
                                if (modifiedTables.Contains(dataSet.ExcelTables.GetTable(file.id)) == false)
                                {
                                    byte[] excelFileData = dataSet.ExcelTables.GetTable(file.id).GenerateExcelFile(dataSet.XlsDataSet.Tables[file.id].DataSet);

                                    using (FileStream fs = new FileStream("test.txt.cooked", FileMode.Create, FileAccess.ReadWrite))
                                    {
                                        fs.Write(excelFileData, 0, excelFileData.Length);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void Save(string path)
        {
            foreach (ExcelTable table in modifiedTables)
            {
                
            }
        }

        private void ModLogic(String file, Attribute attribute, int row)
        {
            try
            {
                if (attribute.replace != null)
                {
                    if (attribute.replace.int_data != null)
                    {
                        dataSet.XlsDataSet.Tables[file].Rows[row][attribute.id] = attribute.replace.int_data;
                    }
                    if (attribute.replace.float_data != null)
                    {
                        dataSet.XlsDataSet.Tables[file].Rows[row][attribute.id] = attribute.replace.float_data;
                    }
                    if (attribute.replace.string_data != null)
                    {
                        dataSet.XlsDataSet.Tables[file].Rows[row][attribute.id] = attribute.replace.string_data;
                    }
                }
                if (attribute.bitwise != null)
                {
                    //foreach (bool bit in attribute.bitmask)
                    //{
                    //    // Perform Bitwise operation
                    //}
                }
                if (attribute.divide != null)
                {
                    if (attribute.divide.int_data != null)
                    {
                        dataSet.XlsDataSet.Tables[file].Rows[row][attribute.id] = Convert.ToInt32(dataSet.XlsDataSet.Tables[file].Rows[row][attribute.id]) / attribute.divide.int_data;
                    }
                    if (attribute.divide.float_data != null)
                    {
                        //dataSet.XlsDataSet.Tables[file].Rows[row][attribute.id] = Convert.ToInt32(dataSet.XlsDataSet.Tables[file].Rows[row][attribute.id]) / attribute.divide.float_data;
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

        public string Title(int i)
        {
            return revival.modification[i].title;
        }

        public string Description(int i)
        {
             return revival.modification[i].GetListDescription();
        }

        public bool Enabled(int i)
        {
            return revival.modification[i].type == "required" ? true : false;
        }

        public void Apply(int i, bool use)
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
            [XmlElement("integer")]
            public int int_data;

            [XmlElement("float")]
            public float float_data;

            [XmlElement("string")]
            public string string_data;
        }
        
        public class Divide
        {
            [XmlElement("integer")]
            public int int_data;

            [XmlElement("float")]
            public float float_data;
        }
    }
}