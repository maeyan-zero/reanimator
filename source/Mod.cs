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
using Reanimator.Excel;

namespace Reanimator
{
#pragma warning disable 0169
#pragma warning disable 0649
    public class Mod
    {
        ExcelTables excelTables;

        public Mod(ExcelTables excel)
        {
            excelTables = excel;
        }

        public static bool Parse(string xml)
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
                    ValidationType = ValidationType.Schema
                };

                // Append the Schema above
                xmlReaderSettings.Schemas.Add(xmlSchema);

                // Add the XML FileStream
                FileStream xmlStream;
                xmlStream = new FileStream(xml, FileMode.Open);

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

                // Validation Success
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

        [XmlRoot("revival")]
        public class Revival
        {
            [XmlElement("modification")]
            Modification[] modification;

            public Modification[] Modification
            {
                get { return modification; }
            }

            public Revival()
            {
            }

            public Revival(Modification modification)
            {
                AddModification(modification);
            }

            public Revival(Modification[] modification)
            {
                AddModification(modification);
            }

            public void AddModification(Modification modification)
            {
                if (this.modification == null)
                {
                    this.modification = new Modification[0];
                }

                List<Modification> mod = new List<Modification>();

                mod.AddRange(this.modification);
                mod.Add(modification);

                this.modification = mod.ToArray();
            }

            public void AddModification(Modification[] modification)
            {
                if (this.modification == null)
                {
                    this.modification = new Modification[0];
                }

                List<Modification> mod = new List<Modification>();

                mod.AddRange(this.modification);
                mod.AddRange(modification);

                this.modification = mod.ToArray();
            }

            public bool HasModification()
            {
                return this.modification == null;
            }

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
            [XmlElement("title")]
            string title;
            [XmlElement("version")]
            string version;
            [XmlElement("description")]
            string description;
            [XmlElement("url")]
            string url;
            [XmlElement("type")]
            string type;
            [XmlElement("pack")]
            Pack[] pack;

            [NonSerialized]
            public bool apply;

            public Modification()
            {
            }

            public Modification(string title, string version, string description, string url, string type, Pack[] pack)
                : this(title, version, description, url, type)
            {
                AddPack(pack);
            }

            public Modification(string title, string version, string description, string url, string type, Pack pack)
                : this(title, version, description, url, type)
            {
                AddPack(pack);
            }

            public Modification(string title, string version, string description, string url, string type)
            {
                this.title = title;
                this.version = version;
                this.description = description;
                this.url = url;
                this.type = type;
            }

            public Modification(Pack pack)
            {
                AddPack(pack);
            }

            public Modification(Pack[] pack)
            {
                AddPack(pack);
            }

            public Pack[] Pack
            {
                get { return pack; }
            }

            public void AddPack(Pack pack)
            {
                if (this.pack == null)
                {
                    this.pack = new Pack[0];
                }

                List<Pack> pck = new List<Pack>();

                pck.AddRange(this.pack);
                pck.Add(pack);

                this.pack = pck.ToArray();
            }

            public void AddPack(Pack[] pack)
            {
                if (this.pack == null)
                {
                    pack = new Pack[0];
                }

                List<Pack> pck = new List<Pack>();

                pck.AddRange(this.pack);
                pck.AddRange(pack);

                this.pack = pck.ToArray();
            }

            public bool HasPack()
            {
                return this.pack == null;
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
            [XmlAttribute("id")]
            public string id;

            [XmlElement("file")]
            public File[] file;

            public File[] File
            {
                get { return file; }
            }

            public Pack()
            {
            }

            public Pack(string id)
            {
                this.id = id;
            }

            public Pack(string id, File file)
                : this(id)
            {
                AddFile(file);
            }

            public Pack(string id, File[] file)
                : this(id)
            {
                AddFile(file);
            }

            public void AddFile(File file)
            {
                if (this.file == null)
                {
                    this.file = new File[0];
                }

                List<File> fle = new List<File>();

                fle.AddRange(this.file);
                fle.Add(file);

                this.file = fle.ToArray();
            }

            public void AddFile(File[] file)
            {
                if (this.file == null)
                {
                    this.file = new File[0];
                }

                List<File> fle = new List<File>();

                fle.AddRange(this.file);
                fle.AddRange(file);

                this.file = fle.ToArray();
            }

            public bool HasFile()
            {
                return file == null;
            }

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
            [XmlAttribute("id")]
            public string id;

            [XmlElement("modify")]
            public Modify modify;

            public File()
            {
            }

            public File(string id)
            {
                this.id = id;
            }

            public File(string id, Modify modify)
                : this(id)
            {
                SetModify(modify);
            }

            public void SetModify(Modify modify)
            {
                this.modify = modify;
            }
        }

        public class Modify
        {
            [XmlElement("entity")]
            public Entity[] entity;

            public Entity[] Entity
            {
                get { return entity; }
            }
            
            public Modify()
            {
            }

            public Modify(Entity entity)
            {
                AddEntity(entity);
            }

            public Modify(Entity[] entity)
            {
                AddEntity(entity);
            }

            public void AddEntity(Entity entity)
            {
                if (this.entity == null)
                {
                    this.entity = new Entity[0];
                }

                List<Entity> ent = new List<Entity>();

                ent.AddRange(this.entity);
                ent.Add(entity);

                this.entity = ent.ToArray();
            }

            public void AddEntity(Entity[] entity)
            {
                if (this.entity == null)
                {
                    this.entity = new Entity[0];
                }

                List<Entity> ent = new List<Entity>();

                ent.AddRange(this.entity);
                ent.AddRange(entity);

                this.entity = ent.ToArray();
            }

            public bool HasEntity()
            {
                return this.entity != null;
            }

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
            [XmlAttribute("id")]
            public string id;

            [XmlElement("attribute")]
            public Attribute[] attribute;

            public Attribute[] Attribute
            {
                get { return attribute; }
            }

            public Entity()
            {
            }

            public Entity(string id)
            {
                this.id = id;
            }

            public Entity(string id, Attribute attribute)
                : this(id)
            {
                AddAttribute(attribute);
            }

            public Entity(string id, Attribute[] attribute)
                : this(id)
            {
                AddAttribute(attribute);
            }

            public void AddAttribute(Attribute attribute)
            {
                if (this.attribute == null)
                {
                    this.attribute = new Attribute[0];
                }

                List<Attribute> attr = new List<Attribute>();
                attr.AddRange(this.attribute);
                attr.Add(attribute);

                this.attribute = attr.ToArray();
            }

            public void AddAttribute(Attribute[] attribute)
            {
                if (this.attribute == null)
                {
                    this.attribute = new Attribute[0];
                }

                List<Attribute> attr = new List<Attribute>();
                attr.AddRange(this.attribute);
                attr.AddRange(attribute);

                this.attribute = attr.ToArray();
            }

            public bool HasAttribute()
            {
                return attribute == null;
            }

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
            [XmlAttribute("id")]
            public string id;

            [XmlElement("replace")]
            public string[] replace;

            [XmlElement("bitmask")]
            public bool[] bitmask;

            public object[] Replace
            {
                get { return replace; }
            }

            public bool[] Bitmask
            {
                get { return bitmask; }
            }

            public Attribute()
            {
            }

            public Attribute(string id)
            {
                this.id = id;
            }

            public Attribute(string id, string replace)
                : this(id)
            {
                AddReplace(replace);
            }

            public Attribute(string id, bool[] bitmask)
                : this(id)
            {
                AddBitmask(bitmask);
            }

            public Attribute(string id, bool bitmask)
                : this(id)
            {
                AddBitmask(bitmask);
            }

            public void AddReplace(string replace)
            {
                this.replace = new string[1];
                this.replace[0] = replace;
                this.bitmask = null;
            }

            public void AddBitmask(bool bitmask)
            {
                if (this.bitmask == null)
                {
                    this.bitmask = new bool[0];
                }

                List<bool> bmsk = new List<bool>();

                bmsk.AddRange(this.bitmask);
                bmsk.Add(bitmask);

                this.bitmask = bmsk.ToArray();

                this.replace = null;
            }

            public void AddBitmask(bool[] bitmask)
            {
                if (this.bitmask == null)
                {
                    this.bitmask = new bool[0];
                }

                List<bool> bmsk = new List<bool>();

                bmsk.AddRange(this.bitmask);
                bmsk.AddRange(bitmask);

                this.bitmask = bmsk.ToArray();

                this.replace = null;
            }

            public bool HasReplaceValue()
            {
                return this.replace != null;
            }

            public bool HasBitmaskValue()
            {
                return this.bitmask != null;
            }
        }

        public static void Serialize(Revival revival, string path)
        {
            XmlSerializer s = new XmlSerializer(typeof(Revival));
            TextWriter w = new StreamWriter(path);
            s.Serialize(w, revival);
            w.Close();
        }

        public static Revival Deserialize(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Revival));
            TextReader tr = new StreamReader(path);
            Revival revival = (Revival)serializer.Deserialize(tr);
            tr.Close();

            return revival;
        }

        public List<ExcelTable> Apply(Revival revival)
        {
            List<ExcelTable> excelTable = new List<ExcelTable>();
            List<String> loadedTables = new List<string>();

            try
            {
                foreach (Modification modification in revival)
                {
                    if (modification.apply == true)
                    {
                        foreach (Pack pack in modification)
                        {
                            // Generally only the one pack will be modified
                            // Logic here is skipped at the moment
                            foreach (File file in pack)
                            {
                                if (loadedTables.Contains(file.id) == false)
                                {
                                    excelTable.Add(excelTables.GetTable(file.id));
                                    loadedTables.Add(file.id);
                                }

                                if (file.modify != null)
                                {
                                    foreach (Entity entity in file.modify)
                                    {
                                        foreach (Attribute attribute in entity)
                                        {
                                            if (attribute.replace != null)
                                            {
                                                // Replace
                                            }
                                            else if (attribute.bitmask != null)
                                            {
                                                //foreach (bool bit in attribute.bitmask)
                                                //{
                                                //    // Perform Bitwise operation
                                                //}
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return excelTable;
            }
            catch
            {
                return null;
            }
        }
    }

#pragma warning restore 0169
#pragma warning restore 0649
}