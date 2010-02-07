using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;

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
                xsdStream = new FileStream("..//..//source//ModSchema.xsd", FileMode.Open);
                
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
                return false;
            }
        }

        [XmlRoot("revival")]
        public class Revival
        {
            [XmlElement("modification")]
            public Modification[] modification;
        }

        public class Modification
        {
            [XmlElement("title")]
            public string title;

            [XmlElement("version")]
            public string version;

            [XmlElement("description")]
            public string description;

            [XmlElement("pack")]
            public Pack[] pack;

            public Modification()
            {

            }
        }

        public class Pack
        {
            [XmlAttribute("id")]
            public string id;

            [XmlElement("file")]
            public File[] file;
        }

        public class File
        {
            [XmlAttribute("id")]
            public string id;

            [XmlElement("modify")]
            public Modify modify;
        }

        public class Modify
        {
            [XmlElement("entity")]
            public Entity[] entity;
        }

        public class Entity
        {
            [XmlAttribute("id")]
            public string id;

            [XmlElement("attribute")]
            public Attribute[] attribute;
        }

        public class Attribute
        {
            [XmlAttribute("id")]
            public string id;

            [XmlElement("replace")]
            public object[] replace;

            [XmlElement("bitmask")]
            public bool[] bitmask;
        }

        public class Replace
        {
            public object replace;
        }

        public void DemoMod()
        {
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

            // Serialization
            XmlSerializer s = new XmlSerializer(typeof(Revival));
            TextWriter w = new StreamWriter(@"c:\list.xml");
            s.Serialize(w, revival);
            w.Close();
        }
    }

#pragma warning restore 0169
#pragma warning restore 0649
}
