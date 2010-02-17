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

namespace Reanimator
{
#pragma warning disable 0169
#pragma warning disable 0649
    public class Mod
    {
        List<Modification> modification;

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
            public Modification[] modification;

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
        }

        public class Modification
        {
            [XmlElement("title")]
            public string title;

            [XmlElement("version")]
            public string version;

            [XmlElement("description")]
            public string description;

            [XmlElement("url")]
            public string url;

            [XmlElement("type")]
            public string type;

            [XmlElement("pack")]
            public Pack[] pack;

            public Pack[] Pack
            {
                get { return pack; }
            }

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

        public class Replace
        {
            public object replace;
        }

        public static void DemoMod()
        {
            Revival revival1 = new Revival();
            Modification mod = new Modification();
            Pack pack = new Pack();
            File file = new File();
            Modify modify = new Modify();
            Entity entity = new Entity();
            Attribute attribute = new Attribute();

            revival1.AddModification(mod);
            mod.AddPack(pack);
            pack.AddFile(file);
            file.SetModify(modify);
            modify.AddEntity(entity);
            entity.AddAttribute(attribute);
            attribute.AddReplace("1123");


            mod.title = "Revival SP modificaton";
            mod.description = "This is the description";
            mod.version = "1.1.0";
            mod.url = "www.hellgateaus.net";
            mod.type = "required";

            pack.id = "hellgate000";

            file.id = "gameglobals.txt.cooked";

            entity.id = "vendorRefresh";

            attribute.id = "intData";


            Revival revival = new Revival();

            revival.modification = new Modification[1];
            revival.modification[0] = new Modification();
            revival.modification[0].title = "Revival SP modificaton";
            revival.modification[0].description = "This is the description";
            revival.modification[0].version = "1.1.0";
            revival.modification[0].pack = new Pack[1];
            revival.modification[0].pack[0] = new Pack();
            revival.modification[0].pack[0].id = "hellgate000";
            revival.modification[0].pack[0].file = new File[1];
            revival.modification[0].pack[0].file[0] = new File();
            revival.modification[0].pack[0].file[0].id = "gameglobals.txt.cooked";
            revival.modification[0].pack[0].file[0].modify = new Modify();
            revival.modification[0].pack[0].file[0].modify.entity = new Entity[1];
            revival.modification[0].pack[0].file[0].modify.entity[0] = new Entity();
            revival.modification[0].pack[0].file[0].modify.entity[0].id = "vendorRefresh";
            revival.modification[0].pack[0].file[0].modify.entity[0].attribute = new Attribute[1];
            revival.modification[0].pack[0].file[0].modify.entity[0].attribute[0] = new Attribute();
            revival.modification[0].pack[0].file[0].modify.entity[0].attribute[0].id = "intData";
            revival.modification[0].pack[0].file[0].modify.entity[0].attribute[0].replace = new String[1];
            revival.modification[0].pack[0].file[0].modify.entity[0].attribute[0].replace[0] = "1123";
            //revival.modification[0].pack[0].file[0].modify.entity[0].attribute[1] = new Attribute();
            //revival.modification[0].pack[0].file[0].modify.entity[0].attribute[1].id = "outData";
            //revival.modification[0].pack[0].file[0].modify.entity[0].attribute[1].replace = new String[1];
            //revival.modification[0].pack[0].file[0].modify.entity[0].attribute[1].replace[0] = "1123";

            //revival.modification[0].pack[0].file[0].modify.entity = new Entity[2];
            //revival.modification[0].pack[0].file[0].modify.entity[1] = new Entity();
            //revival.modification[0].pack[0].file[0].modify.entity[1].id = "vendorRefresh";
            //revival.modification[0].pack[0].file[0].modify.entity[1].attribute = new Attribute[2];
            //revival.modification[0].pack[0].file[0].modify.entity[1].attribute[0] = new Attribute();
            //revival.modification[0].pack[0].file[0].modify.entity[1].attribute[0].id = "intData";
            //revival.modification[0].pack[0].file[0].modify.entity[1].attribute[0].replace = new String[1];
            //revival.modification[0].pack[0].file[0].modify.entity[1].attribute[0].replace[0] = "1123";
            //revival.modification[0].pack[0].file[0].modify.entity[1].attribute[1] = new Attribute();
            //revival.modification[0].pack[0].file[0].modify.entity[1].attribute[1].id = "outData";
            //revival.modification[0].pack[0].file[0].modify.entity[1].attribute[1].replace = new String[1];
            //revival.modification[0].pack[0].file[0].modify.entity[1].attribute[1].replace[0] = "1123";

            // Serialization
            Serialize(revival1, @"c:\list.xml");
        }

        public static void Serialize(Revival revival, string path)
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
    }

#pragma warning restore 0169
#pragma warning restore 0649
}