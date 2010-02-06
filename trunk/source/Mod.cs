using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.IO;

namespace Reanimator
{
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
                        switch (xmlReader.Name)
                        {
                            case "revival":
                                xmlReader.Read();
                                modification = new List<Modification>();
                                break;
                            case "modification":
                                xmlReader.Read();
                                modification.Add(new Modification());
                                break;
                            case "title":
                                xmlReader.Read();
                                modification.Last().title = xmlReader.Value;
                                break;
                            case "description":
                                xmlReader.Read();
                                modification.Last().description = xmlReader.Value;
                                break;
                            case "version":
                                xmlReader.Read();
                                modification.Last().version = xmlReader.Value;
                                break;
                            case "pack":
                                xmlReader.Read();
                                modification.Last().pack.Add(new Pack(xmlReader["id"]));
                                break;
                            case "file":
                                xmlReader.Read();
                                modification.Last().pack.Last().file.Add(new File(xmlReader["id"]));
                                break;
                            case "modify":
                                xmlReader.Read();
                                break;
                            case "entity":
                                xmlReader.Read();
                                modification.Last().pack.Last().file.Last().modify.entity.Add(new Entity(xmlReader["id"]));
                                break;
                            case "attribute":
                                xmlReader.Read();
                                modification.Last().pack.Last().file.Last().modify.entity.Last().attribute.Add(new Attribute(xmlReader["id"]));
                                break;
                            case "replace":
                                xmlReader.Read();
                                modification.Last().pack.Last().file.Last().modify.entity.Last().attribute.Last().replace = xmlReader.Value;
                                break;
                            case "bitwise":
                                xmlReader.Read();
                                modification.Last().pack.Last().file.Last().modify.entity.Last().attribute.Last().bit = xmlReader["id"];
                                modification.Last().pack.Last().file.Last().modify.entity.Last().attribute.Last().flip = xmlReader.Value;
                                break;
                        }
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

        class Modification
        {
            public Modification()
            {
                pack = new List<Pack>();
            }
            public string title;
            public string version;
            public string description;
            public List<Pack> pack;
        }

        public class Pack
        {
            public Pack(String id)
            {
                file = new List<File>();
                pack = id;
            }
            public string getPackDat()
            {
                return pack + ".dat";
            }
            public string getPackIdx()
            {
                return pack + ".idx";
            }
            public string pack;
            public List<File> file;
        }

        public class File
        {
            public File(String id)
            {
                modify = new Modify();
                file = id;
            }
            public string file;
            public string replace;
            public Modify modify;
        }

        public class Modify
        {
            public Modify()
            {
                entity = new List<Entity>();
            }
            public List<Entity> entity;
        }

        public class Entity
        {
            public Entity(String id)
            {
                attribute = new List<Attribute>();
                entity = id;
            }
            public string entity;
            public List<Attribute> attribute;
        }

        public class Attribute
        {
            public Attribute(String id)
            {
                attribute = id;
            }
            public string attribute;
            public object replace;
            public string bit;
            public string flip;
        }
    }
}
