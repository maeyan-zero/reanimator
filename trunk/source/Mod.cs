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
    static class Mod
    {
        private static List<Modification> modification;

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
            public Modify[] modify;
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
            public object replace;

            [XmlElement("bitwise")]
            public bool bitwise;
        }
    }
#pragma warning restore 0169
#pragma warning restore 0649
}
