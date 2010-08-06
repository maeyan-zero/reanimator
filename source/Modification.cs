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
    public class Modification
    {
        TableDataSet _tableDataSet;
        List<string> _changedTables;
        Root _revival;
        Index[] _indexFiles;

        public Modification(TableDataSet tableDataSet)
        {
            _tableDataSet = tableDataSet;
            _changedTables = new List<string>();
            _revival = new Root();
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
            XmlSerializer serializer = new XmlSerializer(typeof(Root));
            TextReader reader = new StreamReader((string)argument);
            _revival = (Root)serializer.Deserialize(reader);
            reader.Close();
        }
        public void Apply(ProgressForm progress, Object argument)
        {
            foreach (File file in _revival.files)
            {
                progress.SetLoadingText("Modifying: " + file.ID);
                Manipulate(file);
                _changedTables.Add(file.ID);
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
                DataTable table = _tableDataSet.XlsDataSet.Tables[file.ID];
                ExcelFile excel = _tableDataSet.TableFiles.GetExcelTableFromId(file.ID);

                foreach (Entity entity in file.Entities)
                {
                    foreach (Attribute attribute in entity.Attributes)
                    {
                        int step = 0;
                        int min = 1;
                        int max = 0;
                        int last = 0; //for recursive function
                        string function;
                        int[] list;

                        //determine the range
                        if (entity.ID.Contains(","))
                        {
                            string[] explode = entity.ID.Split(',');
                            list = new int[explode.Length];

                            for (int i = 0; i < list.Length; i++)
                            {
                                list[i] = Convert.ToInt32(explode[i]);
                            }
                        }
                        else
                        {
                            if (entity.ID.Contains("*"))
                            {
                                min = 0;
                                max = table.Rows.Count - 1;
                            }
                            else if (entity.ID.Contains("-"))
                            {
                                int idx = entity.ID.IndexOf('-');
                                int len = entity.ID.Length - idx - 1;
                                min = Convert.ToInt32(entity.ID.Substring(0, idx));
                                max = Convert.ToInt32(entity.ID.Substring(idx + 1, len));
                            }
                            else
                            {
                                min = Convert.ToInt32(entity.ID);
                                max = Convert.ToInt32(entity.ID);
                            }

                            int listlen = max - min + 1;
                            list = new int[listlen];
                            int i = 0;

                            for (int row = min; row <= max; row++)
                            {
                                list[i++] = row;
                            }
                        }

                        //determine function
                        if (attribute.Bit != null)
                        {
                            function = "bitwise";
                        }
                        else if (attribute.Operation == null)
                        {
                            function = "replace";
                        }
                        else if (attribute.Operation.Contains("*"))
                        {
                            function = "multiply";
                        }
                        else if (attribute.Operation.Contains("/"))
                        {
                            function = "divide";
                        }
                        else if (attribute.Operation.Contains("+"))
                        {
                            string s = attribute.Operation.Remove(0);
                            step = Convert.ToInt32(s);
                            function = "recursive";
                        }
                        else if (attribute.Operation.Contains("-"))
                        {
                            step = Convert.ToInt32(attribute.Operation);
                            function = "recursive";
                        }
                        else
                        {
                            continue; // syntax error
                        }

                        //main loop, alters the dataset
                        foreach (int row in list)
                        {
                            object obj = null;
                            string col = attribute.ID;
                            Type type = table.Columns[col].DataType;
                            DataRow dataRow;

                            //add blank/reserved rows if required
                            while (row >= table.Rows.Count)
                            {
                                dataRow = table.NewRow();
                                table.Rows.Add(dataRow);
                            }

                            dataRow = table.Rows[row];

                            switch (function)
                            {
                                case "replace":
                                    obj = attribute.Value;
                                    break;

                                case "multiply":
                                    if (type.Equals(typeof(int)))
                                        obj = (int)dataRow[col] * Convert.ToInt32(attribute.Value);
                                    else if (type.Equals(typeof(float)))
                                        obj = (float)dataRow[col] * Convert.ToSingle(attribute.Value);
                                    break;

                                case "divide":
                                    if (type.Equals(typeof(int)))
                                        obj = (int)dataRow[col] / Convert.ToInt32(attribute.Value);
                                    else if (type.Equals(typeof(float)))
                                        obj = (float)dataRow[col] / Convert.ToSingle(attribute.Value);
                                    break;

                                case "bitwise":
                                    uint bit = (uint)Enum.Parse(type, attribute.Bit, true);
                                    uint mask = (uint)dataRow[col];
                                    bool flick = Convert.ToBoolean(attribute.Value);
                                    if (flick != ((mask & bit) == 1))
                                        obj = mask ^= bit;
                                    else
                                        obj = mask;
                                    break;

                                case "recursive":
                                    if (row.Equals(min))//first time only
                                    {
                                        if (type.Equals(typeof(int)))
                                        {
                                            obj = Convert.ToInt32(attribute.Value);
                                            last = (int)obj;
                                        }
                                    }
                                    else 
                                    {
                                        last += step;
                                        obj = last;
                                    }
                                    break;
                            }
                            dataRow[col] = obj;
                        } //end main loop
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        [XmlRoot("modification")]
        public class Root
        {
            [XmlElement("file", typeof(File))]
            public File[] files { get; set; }
        }

        public class File
        {
            [XmlAttribute("id")]
            public string ID { get; set; }

            [XmlElement("entity", typeof(Entity))]
            public Entity[] Entities { get; set; }
        }

        public class Entity
        {
            [XmlAttribute("id")]
            public string ID { get; set; }

            [XmlElement("attribute", typeof(Attribute))]
            public Attribute[] Attributes { get; set; }
        }

        public class Attribute
        {
            [XmlAttribute("id")]
            public string ID { get; set; }

            [XmlAttribute("bit")]
            public string Bit { get; set; }

            [XmlAttribute("param")]
            public string Operation { get; set; }

            [XmlText]
            public string Value { get; set; }
        }
    }
}
