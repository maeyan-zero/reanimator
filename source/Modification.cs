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

namespace Reanimator
{
    public class Modification : IDisposable
    {
        private Index[] _index;                                                 // The Hellgate London index & data files.
        private Revival _revival;                                               // The base modifications class. Contains many mods.
        private TableFiles _tableFiles;                                         // The "Excel" relational data sets.
        private readonly TableDataSet _tableDataSet = new TableDataSet();
        private readonly List<ExcelFile> _excelList = new List<ExcelFile>();    // Excel files.
        private readonly List<String> _loadedExcelList = new List<String>();    // Loaded Excel files.
        private readonly List<String> _savedExcelList = new List<String>();     // Saved Excel files.

        public Content[] content { get { return _revival.modification; } }
        public int Length { get { return (_revival.modification == null) ? 0 : _revival.modification.Length; } }
        public bool InstallIsClean { get { foreach (Index i in _index) { if (i.Modified) return false; } return true; } }

        public Modification()
        {
            _revival = new Revival();
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

            _index = Index.LoadIndexFiles(Config.HglDir + "\\data\\");
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
            foreach (Content modification in _revival.modification)
            {
                if (!modification.apply) continue;

                foreach (Pack pack in modification)
                {
                    // Find the index indice.
                    for (int i = 0; i < Index.FileNames.Length; i++)
                    {
                        if (pack.id != Index.FileNames[i]) continue;
                        pack.list_id = i;
                        break;
                    }

                    foreach (File file in pack)
                    {
                        try
                        {
                            if (file.id.Contains(".txt.cooked"))
                            {
                                // Has ExcelTables interface been initialized?
                                if (_tableFiles == null)
                                {
                                    // Excel Table FileIndex
                                    Index.FileIndex fileIndex =
                                        _index[Index.LatestPatch].FileTable[Index.ExcelTablesIndex];
                                    if (String.IsNullOrEmpty(fileIndex.DirectoryString) ||
                                        !fileIndex.FileNameString.Contains("exceltables"))
                                    {
                                        MessageBox.Show("File not found in data file!\nFile: exceltables.txt.cooked", "Error",
                                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        continue;
                                    }

                                    // Extract and initialize.
                                    _tableFiles = new TableFiles();
                                    if (!_tableFiles.ParseSingleExcelData("EXCELTABLES", _index[Index.LatestPatch].ReadDataFile(fileIndex)))
                                    {
                                        throw new Exception("Failed to parse exceltables data!");
                                    }
                                }

                                // Has file been extracted yet?
                                if (_loadedExcelList.Contains(file.id) == false)
                                {
                                    // Find the index indice.
                                    file.index_id_patch = _index[Index.LatestPatch].Locate(file.id, file.dir);

                                    if (file.index_id_patch == -1)
                                    {
                                        throw new Exception("Could not locate file: " + file.id);
                                    }

                                    // Resolve the correct table reference.
                                    file.table_ref = _tableFiles.GetStringIdFromFileName(file.id.Replace(".txt.cooked", ""));

                                    // Reference the file index
                                    Index.FileIndex fileIndex = _index[Index.LatestPatch].FileTable[file.index_id_patch];
                                    // Extract the file
                                    if (!_tableFiles.ParseSingleExcelData(file.table_ref, _index[Index.LatestPatch].ReadDataFile(fileIndex)))
                                    {
                                        throw new Exception("Failed to parse excel table data!\n\nString ID: " + file.table_ref);
                                    }
                                    _excelList.Add(_tableFiles[file.table_ref] as ExcelFile);

                                    // Record the file has been loaded
                                    _loadedExcelList.Add(file.id);
                                    // Record the indice
                                    file.list_id = _loadedExcelList.Count - 1;
                                    // Add the table to the data set
                                    _tableDataSet.LoadTable(progress, _excelList[file.list_id]);
                                }
                                else
                                {
                                    if (file.table_ref == null)
                                    {
                                        file.table_ref = _tableFiles.GetStringIdFromFileName(file.id.Replace(".txt.cooked", ""));
                                    }
                                }
                            }

                            //////
                            // Strings file.
                            //

                            //if (file.id.Contains("xls.uni.cooked") == true)
                            //{
                            //    if (loaded_string_list.Contains(file.id) == false)
                            //    {
                            //        // Find the index indice.
                            //        file.index_id_patch = index[Index.LatestPatchLocalized].Locate(file.id, file.dir);

                            //        if (file.index_id_patch != -1)
                            //        {
                            //            // Reference the file index
                            //            Index.FileIndex file_index = index[Index.LatestPatchLocalized].FileTable[file.index_id_patch];
                            //            // Extract the strings file
                            //            string_list.Add(new StringsFile(index[Index.LatestPatchLocalized].ReadDataFile(file_index)));
                            //            // Record the file has been loaded
                            //            loaded_string_list.Add(file.id);
                            //            // Record the indice
                            //            file.list_id = loaded_string_list.Count - 1;
                            //            // Add the table to the data set
                            //            data_set.LoadTable(progress, string_list[file.list_id]);
                            //        }
                            //        else
                            //        {
                            //            throw new Exception("Could not locate file: " + file.id);
                            //        }
                            //    }
                            //}

                            //////
                            // Modify the files.
                            //
                            if (file.modify == null) continue;

                            // Only Excel files can be modified
                            if (!file.id.Contains(".txt.cooked"))
                            {
                                throw new Exception("Only excel files can be 'modified'");
                            }

                            foreach (Entity entity in file.modify)
                            {
                                foreach (Attribute attribute in entity)
                                {
                                    // It's a wildcard
                                    if (entity.id == "*")
                                    {
                                        // If a value is specified, then a range has been defined
                                        if (entity.value != null)
                                        {
                                            // Feed the values through the Logic function
                                            if (entity.value.Contains("-"))
                                            {
                                                int indexVal = entity.value.IndexOf('-');
                                                int startVal =
                                                    Convert.ToInt32(entity.value.Substring(0, indexVal));
                                                int lastVal =
                                                    Convert.ToInt32(entity.value.Substring(indexVal + 1,
                                                                                           entity.value.
                                                                                               Length -
                                                                                           (indexVal + 1)));

                                                for (int i = startVal; i <= lastVal; i++)
                                                {
                                                    Manipulate(_tableDataSet, file.table_ref, attribute, i);
                                                }
                                            }
                                            else
                                            {
                                                throw new Exception(
                                                    "Syntax Error. Value must be null or a range when using a wildcard.");
                                            }
                                        }
                                        // Modify all rows.
                                        else
                                        {
                                            for (int i = 0;
                                                 i < _tableDataSet.XlsDataSet.Tables[file.table_ref].Rows.Count;
                                                 i++)
                                            {
                                                Manipulate(_tableDataSet, file.table_ref, attribute, i);
                                            }
                                        }
                                    }
                                    // It's a specific row. ie. 3
                                    else
                                    {
                                        Manipulate(_tableDataSet, file.table_ref, attribute,
                                              Convert.ToInt32(entity.value));
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.ToString());
                        }
                    }
                }
            }
        }

        public void Save(ProgressForm progress, Object argument)
        {
            progress.SetLoadingText("Modifying and Saving all files.");

            foreach (Content modification in _revival.modification)
            {
                if (!modification.apply) continue;

                foreach (Pack pack in modification)
                {
                    foreach (File file in pack)
                    {
                        try
                        {
                            // Get the file's index relative to its base index class. Another index may be required
                            // for its location in the patch file. This index is stored in file.index_id_patch if it
                            // exists.
                            file.index_id = _index[pack.list_id].Locate(file.id, file.dir);

                            if (file.id.Contains(".xls.uni.cooked") == false)
                            {
                                file.index_id_patch = _index[Index.LatestPatch].Locate(file.id, file.dir);
                            }
                            else
                            {
                                file.index_id_patch = _index[Index.LatestPatchLocalized].Locate(file.id, file.dir);
                            }

                            // Update the progress bar.
                            progress.SetCurrentItemText(file.id);

                            // Modified files are dumped from memory.
                            if (file.modify != null)
                            {
                                // Excel Files.
                                if (file.id.Contains("txt.cooked"))
                                {
                                    // Been saved yet?
                                    if (_savedExcelList.Contains(file.id) == false && file.table_ref != null)
                                    {
                                        string dir = Config.HglDir + "\\" +
                                                     _index[Index.LatestPatch].FileTable[file.index_id_patch].
                                                         DirectoryString;

                                        string filename = _tableFiles.GetFileNameFromStringId(file.id);

                                        byte[] excelBytes = _excelList[file.list_id].GenerateFile(_tableDataSet.XlsDataSet.Tables[file.id]);

                                        if (Directory.Exists(dir) == false) Directory.CreateDirectory(dir);

                                        using (
                                            FileStream fs = new FileStream(dir + filename, FileMode.OpenOrCreate,
                                                                           FileAccess.ReadWrite))
                                        {
                                            fs.Write(excelBytes, 0, excelBytes.Length);
                                            _savedExcelList.Add(file.id);
                                        }
                                    }
                                }

                                //
                                // String Files.
                                //

                                //if (file.id.Contains("xls.uni.cooked") == true)
                                //{
                                //    // Been saved yet?
                                //    if (saved_excel_list.Contains(file.id) == false && file.table_ref != null)
                                //    {
                                //        string dir = Config.HglDir + "\\" + index[Index.LatestPatch].FileTable[file.index_id_patch].DirectoryString;
                                //        string filename = excel_tables.TableManager.GetReplacement(file.id);

                                //        byte[] excel_bytes = excel_list[file.list_id].GenerateExcelFile(data_set.XlsDataSet);

                                //        if (Directory.Exists(dir) == false) Directory.CreateDirectory(dir);

                                //        using (FileStream fs = new FileStream(dir + filename, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                                //        {
                                //            fs.Write(excel_bytes, 0, excel_bytes.Length);
                                //            saved_excel_list.Add(file.id);
                                //        }
                                //    }
                                //}
                            }


                            if (file.replace != null)
                            {
                                // If its a strings file, repack it
                                if (file.repack)
                                {
                                    FileStream buffer = new FileStream(
                                        Config.HglDir + "\\data" + "\\" + file.replace.data, FileMode.Open);

                                    byte[] byteBuffer = new byte[buffer.Length];

                                    buffer.Read(byteBuffer, 0, (int)buffer.Length);

                                    if (file.index_id_patch >= 0)
                                    {
                                        if (file.id.Contains("xls.uni.cooked"))
                                        {
                                            _index[Index.LatestPatchLocalized].AppendToDat(byteBuffer, true,
                                                                                          file.index_id_patch, true);
                                        }
                                        else
                                        {
                                            _index[Index.LatestPatch].AppendToDat(byteBuffer, true,
                                                                                 file.index_id_patch, true);
                                        }
                                    }
                                    else
                                    {
                                        _index[pack.list_id].AppendToDat(byteBuffer, true, file.index_id, true);
                                    }

                                    buffer.Dispose();
                                }
                                // Replace copies the replacing file to the HGL dir.
                                else
                                {
                                    string source = Directory.GetCurrentDirectory() + "\\" + file.replace.data;
                                    string destination = Config.HglDir + "\\" +
                                                         _index[pack.list_id].FileTable[file.index_id].
                                                             DirectoryString;

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
                            }

                            // Modify the index if it isn't already
                            if (_index[pack.list_id].FileTable[file.index_id].Modified == false &&
                                file.repack == false)
                            {
                                _index[pack.list_id].AppendDirectorySuffix(file.index_id);

                                // Modify the patch index.
                                if (file.index_id_patch >= 0)
                                {
                                    _index[Index.LatestPatch].AppendDirectorySuffix(file.index_id_patch);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            if (e.ToString() == "Source file not found")
                                MessageBox.Show("File not found: " + file.id, "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            else
                                MessageBox.Show(e.ToString());
                        }
                    }
                }

            }

            // Save the modified index files
            foreach (Index indx in _index)
            {
                if (!indx.Modified) continue;

                indx.WriteIndex();
            }
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
        void Manipulate(TableDataSet dataSet, String t, Attribute a, int r)
        {
            try
            {
                if (a.replace != null)
                {
                    Type type = dataSet.XlsDataSet.Tables[t].Rows[r][a.id].GetType();

                    if (type == typeof(Int32))
                        dataSet.XlsDataSet.Tables[t].Rows[r][a.id] =
                            Convert.ToInt32(a.replace.data);
                    else if (type == typeof(UInt32))
                        dataSet.XlsDataSet.Tables[t].Rows[r][a.id] =
                            Convert.ToUInt32(a.replace.data);
                    else if (type == typeof(float))
                        dataSet.XlsDataSet.Tables[t].Rows[r][a.id] =
                            Convert.ToSingle(a.replace.data);
                    else if (type == typeof(string))
                        dataSet.XlsDataSet.Tables[t].Rows[r][a.id] =
                            a.replace.data;
                }

                if (a.divide != null)
                {
                    Type type = dataSet.XlsDataSet.Tables[t].Rows[r][a.id].GetType();

                    if (type == typeof(Int32))
                        dataSet.XlsDataSet.Tables[t].Rows[r][a.id] =
                            Convert.ToInt32(dataSet.XlsDataSet.Tables[t].Rows[r][a.id]) /
                            Convert.ToInt32(a.divide.data);
                    else if (type == typeof(float))
                        dataSet.XlsDataSet.Tables[t].Rows[r][a.id] =
                            Convert.ToSingle(dataSet.XlsDataSet.Tables[t].Rows[r][a.id]) /
                            Convert.ToSingle(a.divide.data);
                }

                if (a.multiply != null)
                {
                    Type type = dataSet.XlsDataSet.Tables[t].Rows[r][a.id].GetType();

                    if (type == typeof(Int32))
                        dataSet.XlsDataSet.Tables[t].Rows[r][a.id] =
                            Convert.ToInt32(dataSet.XlsDataSet.Tables[t].Rows[r][a.id]) /
                            Convert.ToInt32(a.divide.data);
                    else if (type == typeof(float))
                        dataSet.XlsDataSet.Tables[t].Rows[r][a.id] =
                            Convert.ToSingle(dataSet.XlsDataSet.Tables[t].Rows[r][a.id]) /
                            Convert.ToSingle(a.divide.data);
                }

                if (a.bitwise != null)
                {
                    uint arg = 0;
                    bool current = false;
                    uint value = Convert.ToUInt32(dataSet.XlsDataSet.Tables[t].Rows[r][a.id]);

                    for (int j = 0; j < a.bitwise.Length; j++)
                    {
                        foreach (Bitwise bitmask in a.bitwise)
                        {
                            switch (a.id)
                            {
                                case "bitmask01":
                                    arg = (uint)Enum.Parse(typeof(Items.BitMask01), bitmask.id, true);
                                    break;
                                case "bitmask02":
                                    arg = (uint)Enum.Parse(typeof(Items.BitMask02), bitmask.id, true);
                                    break;
                                case "bitmask03":
                                    arg = (uint)Enum.Parse(typeof(Items.BitMask03), bitmask.id, true);
                                    break;
                                case "bitmask04":
                                    arg = (uint)Enum.Parse(typeof(Items.BitMask04), bitmask.id, true);
                                    break;
                                case "bitmask05":
                                    arg = (uint)Enum.Parse(typeof(Items.BitMask05), bitmask.id, true);
                                    break;
                                case "bitmask06":
                                    arg = (uint)Enum.Parse(typeof(Items.BitMask06), bitmask.id, true);
                                    break;
                                case "bitmask07":
                                    arg = (uint)Enum.Parse(typeof(Items.BitMask07), bitmask.id, true);
                                    break;
                            }

                            // Is the current value on or off?
                            if ((value & arg) > 0)
                                current = true;
                            // Does the current value need to be inversed?
                            if (current != bitmask.switch_data)
                                dataSet.XlsDataSet.Tables[t].Rows[r][a.id] = (value ^= arg);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("There was a problem modifying the form: " + t + "\n" +
                    "Column: " + a.id + ". Check syntax and try again.\n\n" + e,
                    "Problem", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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

            public MyEnumerator GetEnumerator() { return new MyEnumerator(this); }

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
                public Pack Current { get { return (collection.pack[nIndex]); } }
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

        public void Dispose()
        {
            foreach (Index i in _index)
                i.Dispose();
        }
    }
}
