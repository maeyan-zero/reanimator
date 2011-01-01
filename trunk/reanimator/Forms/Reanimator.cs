using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using Hellgate;
using Revival.Common;

namespace Reanimator.Forms
{
    public partial class Reanimator : Form
    {
        private FileExplorer _fileExplorer;
        private TablesLoaded _tablesLoaded;
        private TablesLoaded _tablesLoadedTCv4;
        private FileManager _fileManager;
        private FileManager _fileManagerTCv4;
        private readonly Options _optionsForm = new Options();
        private readonly List<TableForm> _openTableForms = new List<TableForm>();
        private readonly List<ExcelTableForm> _openExcelTableForms = new List<ExcelTableForm>();
        private List<HeroEditor> _openHeroEditorForms = new List<HeroEditor>();
        private const bool AlexInstaLoad = false;

        public Reanimator()
        {
            InitializeComponent();

            #region alexs_stuff

            if (true) return;

            //_ExtractFunctionList();
            //_ExcelValuesDeepScan();
            _DoCookTest();
            //_ConvertTCv4ExcelToSP();
            //_LoadAllMLIFiles();
            //_LoadAllRooms();
            //_LoadAllLevelRules();
            //_UncookAllXml();
            //return;

            //LevelRulesEditor levelRulesEditor = new LevelRulesEditor(@"D:\Games\Hellgate London\data\background\city\rule_pmt02.drl")
            //{
            //    MdiParent = this
            //};
            //levelRulesEditor.Show(););
            return;

            //
            //return;


            //const String hashStr1 = @"data\background\catacombs\";
            //const String hashStr2 = "ct_connb_path.xml.cooked";
            //byte[] data1 = FileTools.StringToASCIIByteArray(hashStr1);
            //byte[] data2 = FileTools.StringToASCIIByteArray(hashStr2);

            //SHA1 sha = new SHA1CryptoServiceProvider();
            //byte[] result1 = sha.ComputeHash(data1);
            //byte[] result2 = sha.ComputeHash(data2);

            //byte[] cryptoBytes = Crypt.GetStringsSHA1Bytes(hashStr1, hashStr2);
            //UInt64 cryptoValue = Crypt.GetStringsSHA1UInt64(hashStr1, hashStr2);


            //const String filePath = @"D:\Games\Hellgate London\MP_x64\hellgate_mp_dx9_x64.txt";
            //String[] strings = File.ReadAllLines(filePath);
            //foreach (String str in strings)
            //{
            //    if (str.Length <= 37) continue;

            //    String subStr = str.Substring(37);
            //    if (subStr == "pszSpineSidesTop")
            //    {
            //        int bp = 0;
            //    }
            //}
            #endregion
        }

        #region alexs_stuff

        private static void _ExtractFunctionList()
        {
            const String path = @"C:\SP_FunctionNamePtrGeneration.txt";
            String[] functionCode = File.ReadAllLines(path);

            ExcelScript.ExtractFunctionList(functionCode);
        }

        /// <summary>
        /// This function checks every row/col of every excel file, determining if it needs to be visible/public or not.
        /// Any element non-zero for all rows, needs to be public (or add new OutputAttribute for "const" values?)
        /// </summary>
        private static void _ExcelValuesDeepScan()
        {
            FileManager fileManager = new FileManager(Config.HglDir);
            fileManager.ExtractAllExcel();

            Dictionary<uint, uint> rowTypeCounts = new Dictionary<uint, uint>();
            Dictionary<String, uint[]> outputMessages = new Dictionary<String, uint[]>();
            Dictionary<String, ObjectDelegator> objectDelegators = new Dictionary<String, ObjectDelegator>();
            foreach (IndexFile.FileEntry fileEntry in fileManager.FileEntries.Values)
            {
                if (!fileEntry.FileNameString.EndsWith(ExcelFile.Extension)) continue;

                byte[] fileBytes = fileManager.GetFileBytes(fileEntry, true);
                Debug.Assert(fileBytes != null);

                ExcelFile excelFile = new ExcelFile(fileBytes, fileEntry.RelativeFullPathWithoutPatch);
                if (excelFile.Attributes.IsEmpty) continue;

                Debug.WriteLine("Checking file: " + fileEntry.RelativeFullPathWithoutPatch);

                uint structureId = excelFile.Attributes.StructureId;
                uint structureUseCount = 0;
                if (rowTypeCounts.TryGetValue(structureId, out structureUseCount))
                {
                    rowTypeCounts[structureId] = ++structureUseCount;
                }
                else
                {
                    rowTypeCounts.Add(structureId, 1);
                }

                ObjectDelegator excelDelegator;
                Type rowType = excelFile.Attributes.RowType;
                FieldInfo[] fieldInfos = rowType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                int rowCount = excelFile.Rows.Count;

                // create delegates
                if (!objectDelegators.TryGetValue(excelFile.StringId, out excelDelegator))
                {
                    excelDelegator = new ObjectDelegator(fieldInfos, ObjectDelegator.SupportedFields.GetValue);
                    objectDelegators.Add(excelFile.StringId, excelDelegator);
                }

                // check by column, by row
                int col = -1;
                foreach (FieldInfo fieldInfo in fieldInfos)
                {
                    if (++col == 0) continue; // row header

                    ObjectDelegator.FieldGetValueDelegate getValue = excelDelegator.GetFieldGetDelegate(fieldInfo.Name);
                    ExcelFile.OutputAttribute outputAttribute = ExcelFile.GetExcelOutputAttribute(fieldInfo);

                    bool isArray = false;
                    bool allEqual = true;
                    Object firstValue = null;
                    Array firstValueArray = null;
                    int arrayIndexFirstDifferent = -1;
                    int row = -1;
                    String message = null;
                    foreach (Object value in excelFile.Rows.Select(rowObject => getValue(rowObject)))
                    {
                        arrayIndexFirstDifferent = -1;
                        if (++row == 0)
                        {
                            if (fieldInfo.FieldType.BaseType == typeof(Array))
                            {
                                firstValueArray = (Array)value;
                                isArray = true;
                                IEnumerator enumerator = firstValueArray.GetEnumerator();
                                enumerator.MoveNext();
                                firstValue = enumerator.Current;
                            }
                            else
                            {
                                firstValue = value;
                            }
                            continue;
                        }

                        if (isArray)
                        {
                            Array objArray = (Array)value;
                            Debug.Assert(firstValueArray != null);

                            bool arrayEqual = true;
                            IEnumerator enumeratorFirst = firstValueArray.GetEnumerator();
                            IEnumerator enumeratorCurrent = objArray.GetEnumerator();
                            while (enumeratorFirst.MoveNext())
                            {
                                enumeratorCurrent.MoveNext();
                                arrayIndexFirstDifferent++;

                                Object objFirst = enumeratorFirst.Current;
                                Object objCurrent = enumeratorCurrent.Current;

                                if (firstValue == null) firstValue = objFirst;
                                if (objFirst.Equals(objCurrent)) continue;

                                arrayEqual = false;
                                break;
                            }

                            if (arrayEqual) continue;
                            allEqual = false;
                            break;
                        }

                        Debug.Assert(firstValue != null);
                        if (firstValue.Equals(value)) continue;
                        allEqual = false;
                        break;
                    }
                    Debug.Assert(firstValue != null);

                    Object constValue = null;
                    bool ignoreConstCheck = false;
                    if (outputAttribute != null)
                    {
                        constValue = outputAttribute.ConstantValue;
                        ignoreConstCheck = outputAttribute.DebugIgnoreConstantCheck;
                    }

                    if (fieldInfo.IsPrivate)
                    {
                        if (!allEqual)
                        {
                            message = String.Format("Warning: Private column (Type = {1}, StructureId = 0x{2}) \"{0}\" has per-row differing values",
                                fieldInfo.Name, fieldInfo.FieldType, structureId.ToString("X8")); // need to add structureId to ensure message is unique per structure type

                            if (isArray)
                            {
                                message += ", first index differing: " + arrayIndexFirstDifferent;
                            }
                        }
                        else // allEqual && IsPrivate
                        {
                            if (isArray)
                            {
                                if (fieldInfo.FieldType == typeof(Int32[]))
                                {
                                    if (firstValue.Equals((Int32)0)) continue;
                                }
                                else if (fieldInfo.FieldType == typeof(byte[]))
                                {
                                    if (firstValue.Equals((byte)0x00)) continue;
                                }
                                else
                                {
                                    Debug.Assert(false, "Unexpected Array Type: " + fieldInfo.FieldType);
                                }

                                if (outputAttribute != null && firstValue.Equals(outputAttribute.ConstantValue)) continue;

                                message = String.Format("Warning: Private column (Type = {2}, StructureId = 0x{3}) \"{0}\" has constant value not zero \"{1}\"",
                                    fieldInfo.Name, firstValue, fieldInfo.FieldType, structureId.ToString("X8"));
                            }
                            else // not array
                            {
                                if (firstValue.Equals(constValue)) continue;
                                if (fieldInfo.FieldType == typeof(Int32) &&
                                    ((Int32)firstValue == 0 && (firstValue.Equals(constValue) || constValue == null))) continue;
                                if (fieldInfo.FieldType == typeof(Int16) &&
                                    ((Int16)firstValue == 0 && (firstValue.Equals(constValue) || constValue == null))) continue;

                                message = String.Format("Warning: Private column (Type = {2}, StructureId = 0x{3}) \"{0}\" has constant value not zero \"{1}\"",
                                    fieldInfo.Name, firstValue, fieldInfo.FieldType, structureId.ToString("X8"));

                                if (outputAttribute != null && outputAttribute.ConstantValue != null) message += " (outputAttribute.ConstantValue is set)";
                            }
                        }
                    }
                    else // IsPublic
                    {
                        if (ignoreConstCheck) continue;

                        if (allEqual && fieldInfo.IsPublic && rowCount != 1 && (outputAttribute == null || outputAttribute.ConstantValue == null))
                        {
                            message = String.Format("Notice: Public column (Type = {2}, StructureId = 0x{3}) \"{0}\" has constant value \"{1}\"",
                                fieldInfo.Name, firstValue, fieldInfo.FieldType, structureId.ToString("X8"));
                        }
                    }

                    if (message == null) continue;

                    uint[] messageCounts;
                    if (outputMessages.TryGetValue(message, out messageCounts))
                    {
                        outputMessages[message] = new[] { messageCounts[0] + 1, structureId };
                    }
                    else
                    {
                        outputMessages.Add(message, new uint[] { 1, structureId });
                    }
                }

                int bp3 = 0;
            }

            String previousStringId = null;
            StringWriter stringWriter = new StringWriter();
            foreach (KeyValuePair<String, uint[]> message in outputMessages)
            {
                String msg = message.Key;
                uint msgCount = message.Value[0];
                uint forStructureId = message.Value[1];
                uint structureCount = rowTypeCounts[forStructureId];

                if (structureCount != msgCount) continue; // if not equal, then we have a message in one table, but in another table it's not applicable

                String[] stringIds = (from dataTableAttribute in DataFile.DataFileMap
                                      where dataTableAttribute.Value.StructureId == forStructureId
                                      select dataTableAttribute.Key).ToArray();
                String stringIdPrepend = String.Join(",", stringIds);

                if (previousStringId != stringIdPrepend)
                {
                    //Console.WriteLine(stringIdPrepend);
                    stringWriter.WriteLine(stringIdPrepend);
                    previousStringId = stringIdPrepend;
                }

                //Console.WriteLine(msg);
                stringWriter.WriteLine(msg);
            }
            File.WriteAllText(@"C:\asdf.txt", stringWriter.ToString());
        }

        /// <summary>
        /// This function is to test excel CSV cooking.
        /// </summary>
        private static void _DoCookTest()
        {
            FileManager fileManager = new FileManager(Config.HglDir);
            fileManager.LoadTableFiles();
            fileManager.ExtractAllExcel();
            ExcelScript.EnableDebug(true);
            ExcelScript.SetFileManager(fileManager);

            foreach (IndexFile.FileEntry fileEntry in fileManager.FileEntries.Values)
            {
                if (!fileEntry.FileNameString.EndsWith(ExcelFile.Extension)) continue;

                byte[] fileBytes = fileManager.GetFileBytes(fileEntry, true);
                Debug.Assert(fileBytes != null);
                String filePath = Path.Combine(Config.HglDir, fileEntry.RelativeFullPathWithoutPatch);

                ExcelFile excelFile = new ExcelFile(fileBytes, fileEntry.RelativeFullPathWithoutPatch);
                if (excelFile.Attributes.IsEmpty) continue;

                Console.WriteLine("Cooking file: " + fileEntry.RelativeFullPathWithoutPatch);

                //if (!fileEntry.RelativeFullPathWithoutPatch.Contains("display_item")) continue;

                byte[] csvBytes = excelFile.ExportCSV();
                File.WriteAllBytes(filePath.Replace(ExcelFile.Extension, ExcelFile.ExtensionDeserialised), csvBytes);
                //ExcelFile excelFileCSV = new ExcelFile(csvBytes, fileEntry.RelativeFullPathWithoutPatch);

                //byte[] recookedBytes = excelFileCSV.ToByteArray();

                //if (excelFile.StringId == "GLOBAL_STRING")
                //{
                //    ExcelFile temp = new ExcelFile(recookedBytes, fileEntry.RelativeFullPathWithoutPatch);
                //    int bp1 = 0;
                //}

                //File.WriteAllBytes(filePath, recookedBytes);
            }

            int bp = 0;
        }

        private static void _ConvertTCv4ExcelToSP()
        {
            return;

            FileManager fileManager = new FileManager(Config.HglDir);
            fileManager.ExtractAllExcel();
            FileManager fileManagerTCv4 = new FileManager(Config.HglDir, true);

            fileManager.LoadTableFiles();
            fileManagerTCv4.LoadTableFiles();

            if (fileManager.DataFiles.Count == 0)
            {
                Console.WriteLine("Error: No Excel files loaded!");
                return;
            }
            if (fileManagerTCv4.DataFiles.Count == 0)
            {
                Console.WriteLine("Error: No TCv4 Excel files loaded!");
                return;
            }

            Dictionary<String, ObjectDelegator> objectDelegators = new Dictionary<String, ObjectDelegator>();
            foreach (ExcelFile excelFile in fileManager.DataFiles.Values.Where(dataFile => dataFile.IsExcelFile))
            {
                if (excelFile.Attributes.RowType == typeof(Hellgate.Excel.Items) ||
                    excelFile.Attributes.RowType == typeof(Hellgate.Excel.Properties))
                {
                    continue;
                }

                Console.WriteLine("Converting Excel table " + excelFile.StringId + "...");

                // ensure we have a TCv4 version loaded
                String stringIdTCv4 = "_TCv4_" + excelFile.StringId;
                ExcelFile excelFileTCv4 = (ExcelFile)fileManagerTCv4.GetDataFile(stringIdTCv4);
                if (excelFileTCv4 == null)
                {
                    Console.WriteLine("Error: TCv4 Excel file not found: " + stringIdTCv4);
                    continue;
                }


                // genereal type-init stuffs; get our object delegators
                ObjectDelegator excelDelegator;
                ObjectDelegator excelDelegatorTCv4;
                Type rowType = excelFile.Attributes.RowType;
                Type rowTypeTCv4 = excelFileTCv4.Attributes.RowType;
                FieldInfo[] fieldInfos = rowType.GetFields();
                FieldInfo[] fieldInfosTCv4 = rowTypeTCv4.GetFields();


                // need to copy row headers (private field) as well
                FieldInfo rowHeaderField = rowType.GetField("header", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                FieldInfo rowHeaderFieldTCv4 = rowTypeTCv4.GetField("header", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                Array.Resize(ref fieldInfos, fieldInfos.Length + 1);
                Array.Resize(ref fieldInfosTCv4, fieldInfosTCv4.Length + 1);
                fieldInfos[fieldInfos.Length - 1] = rowHeaderField;
                fieldInfosTCv4[fieldInfosTCv4.Length - 1] = rowHeaderFieldTCv4;


                // create delegates
                if (!objectDelegators.TryGetValue(excelFile.StringId, out excelDelegator))
                {
                    excelDelegator = new ObjectDelegator(fieldInfos, new[] { ObjectDelegator.SupportedFields.GetValue, ObjectDelegator.SupportedFields.SetValue });
                    objectDelegators.Add(excelFile.StringId, excelDelegator);
                }
                if (!objectDelegators.TryGetValue(stringIdTCv4, out excelDelegatorTCv4))
                {
                    excelDelegatorTCv4 = new ObjectDelegator(fieldInfosTCv4, ObjectDelegator.SupportedFields.GetValue);
                    objectDelegators.Add(stringIdTCv4, excelDelegatorTCv4);
                }


                // debug: ensure we have same columns
                bool hasSameFields = fieldInfos.All(fieldInfo => excelDelegatorTCv4.ContainsGetFieldDelegate(fieldInfo.Name));
                if (!hasSameFields)
                {
                    Console.WriteLine("Error: The Excel types do not have the same fields!");
                    continue;
                }


                // todo: _TCv4_AFFIX removed SecondaryStrings and put them into a table called AFFIX_GROUPS
                // todo: Ensure BitRelations for TCv4 UNITTYPES and STATES are correct
                // todo: check _extendedBuffer AI stuff

                // begin conversion process
                Object[] rows = new Object[excelFileTCv4.Rows.Count];
                bool failed = false;
                foreach (FieldInfo fieldInfo in fieldInfos)
                {
                    ObjectDelegator.FieldGetValueDelegate getTCv4Value = excelDelegatorTCv4.GetFieldGetDelegate(fieldInfo.Name);
                    ObjectDelegator.FieldSetValueDelegate setValue = excelDelegator.GetFieldSetDelegate(fieldInfo.Name);
                    ExcelFile.OutputAttribute outputAttribute = ExcelFile.GetExcelOutputAttribute(fieldInfo);

                    int col = -1;
                    foreach (Object rowTCv4 in excelFileTCv4.Rows)
                    {
                        if (rows[++col] == null) rows[col] = Activator.CreateInstance(excelFile.Attributes.RowType);
                        Object value = getTCv4Value(rowTCv4);

                        if (outputAttribute == null)
                        {
                            setValue(rows[col], value);
                            continue;
                        }

                        if (outputAttribute.IsBitmask)
                        {
                            if (value.GetType().BaseType != typeof(Enum))
                            {
                                Console.WriteLine("Error: IsBitmask is not of type Enum: " + fieldInfo.Name);
                                failed = true;
                                break;
                            }

                            Type spBitMask = fieldInfo.FieldType;
                            Type tcBitMask = value.GetType();
                            uint currentMask = (uint)value;
                            uint convertedMask = 0;

                            for (int i = 0; i < 32; i++)
                            {
                                uint testBit = (uint)1 << i;
                                if ((currentMask & testBit) == 0) continue;

                                String bitString = Enum.GetName(tcBitMask, testBit);
                                if (bitString == null) continue;

                                if (Enum.IsDefined(spBitMask, bitString))
                                {
                                    convertedMask += (uint)Enum.Parse(spBitMask, bitString);
                                }
                            }

                            value = convertedMask;
                        }
                        else if (outputAttribute.IsScript)
                        {
                            int bp = 0;
                        }

                        setValue(rows[col], value);
                    }

                    if (failed) break;
                }
                if (failed)
                {
                    Console.WriteLine("Error: Excel conversion failed: " + excelFile.StringId);
                    continue;
                }


                // finish conversion process by "changing" the TCv4 type
                excelFileTCv4.ConvertType(excelFile, rows);
                byte[] convertedBytes = excelFileTCv4.ToByteArray();
                String writePath = Path.Combine(Config.HglDir, excelFile.FilePath);
                File.WriteAllBytes(writePath, convertedBytes);

                int bp2 = 0;
            }

            int bp1 = 0;
        }

        private static ObjectDelegator _GetExcelDelegator(DataFile excelFile, IDictionary<String, ObjectDelegator> objectDelegators)
        {
            ObjectDelegator excelDelegator;

            if (!objectDelegators.TryGetValue(excelFile.StringId, out excelDelegator))
            {
                Type rowType = excelFile.Attributes.RowType;
                FieldInfo[] fieldInfos = rowType.GetFields();

                excelDelegator = new ObjectDelegator(fieldInfos, new[] { ObjectDelegator.SupportedFields.GetValue, ObjectDelegator.SupportedFields.SetValue });

                objectDelegators.Add(excelFile.StringId, excelDelegator);
            }

            return excelDelegator;
        }

        private static void _LoadAllMLIFiles()
        {
            const String root = @"D:\Games\Hellgate London\data\background\";
            List<String> mliFiles = new List<String>(Directory.GetFiles(root, "*.mli", SearchOption.AllDirectories));

            foreach (String mliFilePath in mliFiles)
            {
                String path = mliFilePath;
                //path = @"D:\Games\Hellgate London\data\background\city\charactercreate.rom";
                //path = "D:\\Games\\Hellgate London\\data\\background\\props\\vehicles\\ambulance_a.rom";

                byte[] mliFileBytes = File.ReadAllBytes(path);
                MLIFile roomDefinitionFile = new MLIFile();

                String fileName = path.Replace(@"D:\Games\Hellgate London\data\background\", "");
                String xmlPath = path.Replace(MLIFile.Extension, MLIFile.ExtensionDeserialised);
                Console.WriteLine("Loading: " + fileName);
                try
                {
                    roomDefinitionFile.ParseFileBytes(mliFileBytes);
                    byte[] xmlBytes = roomDefinitionFile.ExportAsDocument();
                    File.WriteAllBytes(xmlPath, xmlBytes);

                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(xmlPath);
                    MLIFile mliFile2 = new MLIFile();
                    mliFile2.ParseXmlDocument(xmlDocument);
                    byte[] bytes = mliFile2.ToByteArray();

                    if (!mliFileBytes.SequenceEqual(bytes))
                    {
                        File.WriteAllBytes(path + "2", bytes);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to load file!\n" + e);
                    continue;
                }
            }
        }

        private static void _LoadAllRooms()
        {
            const String root = @"D:\Games\Hellgate London\data\background\";
            List<String> roomFiles = new List<String>(Directory.GetFiles(root, "*.rom", SearchOption.AllDirectories));

            foreach (String roomFilePath in roomFiles)
            {
                String path = roomFilePath;
                //path = @"D:\Games\Hellgate London\data\background\city\charactercreate.rom";
                //path = "D:\\Games\\Hellgate London\\data\\background\\props\\vehicles\\ambulance_a.rom";

                byte[] roomFileBytes = File.ReadAllBytes(path);
                RoomDefinitionFile roomDefinitionFile = new RoomDefinitionFile();

                String fileName = path.Replace(@"D:\Games\Hellgate London\data\background\", "");
                String xmlPath = path.Replace(RoomDefinitionFile.Extension, RoomDefinitionFile.ExtensionDeserialised);
                Console.WriteLine("Loading: " + fileName);
                try
                {
                    roomDefinitionFile.ParseFileBytes(roomFileBytes);
                    byte[] xmlBytes = roomDefinitionFile.ExportAsDocument();
                    File.WriteAllBytes(xmlPath, xmlBytes);

                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(xmlPath);
                    RoomDefinitionFile roomDefinitionFile2 = new RoomDefinitionFile();
                    roomDefinitionFile2.ParseXmlDocument(xmlDocument);
                    byte[] bytes = roomDefinitionFile2.ToByteArray();

                    if (!roomFileBytes.SequenceEqual(bytes))
                    {
                        File.WriteAllBytes(path + "2", bytes);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to load file!\n" + e);
                    continue;
                }
            }
        }

        private static void _LoadAllLevelRules()
        {
            const String root = @"D:\Games\Hellgate London\data\background\";
            List<String> drlFiles = new List<String>(Directory.GetFiles(root, "*.drl", SearchOption.AllDirectories));

            foreach (String drlFilePath in drlFiles)
            {
                String path = drlFilePath;
                //path = @"D:\Games\Hellgate London\data\background\catacombs\ct_rule_100.drl";
                //path = @"D:\Games\Hellgate London\data\background\city\rule_pmt02.drl";

                byte[] levelRulesBytes = File.ReadAllBytes(path);
                LevelRulesFile levelRulesFile = new LevelRulesFile();

                String fileName = path.Replace(@"D:\Games\Hellgate London\data\background\", "");
                String xmlPath = path.Replace(LevelRulesFile.Extension, LevelRulesFile.ExtensionDeserialised);
                Console.WriteLine("Loading: " + fileName);
                try
                {
                    levelRulesFile.ParseFileBytes(levelRulesBytes);
                    byte[] xmlBytes = levelRulesFile.ExportAsDocument();
                    File.WriteAllBytes(xmlPath, xmlBytes);

                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(xmlPath);
                    LevelRulesFile levelRulesFile2 = new LevelRulesFile();
                    levelRulesFile2.ParseXmlDocument(xmlDocument);
                    byte[] bytes = levelRulesFile2.ToByteArray();

                    // note: ct_rule_100.drl has unreferenced rules (only one found out of all files)
                    if (levelRulesBytes.Length != bytes.Length)
                    {
                        File.WriteAllBytes(path + "2", bytes);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to load file!\n" + e);
                    continue;
                }

            }
        }

        private static void _UncookAllXml()
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            //const String root = @"D:\Games\Hellgate London\data\";
            const String root = @"D:\Games\Hellgate London\data\mp_hellgate_1.10.180.3416_1.0.86.4580\";
            //const String root = @"D:\Games\Hellgate London\data\mp_hellgate_1.10.180.3416_1.0.86.4580\data\background\";
            DirectoryInfo directoryInfo = new DirectoryInfo(root);
            List<String> xmlFiles = new List<String>(Directory.GetFiles(root, "*.xml.cooked", SearchOption.AllDirectories));

            int count = 0;
            List<XmlCookedFile> excelStringWarnings = new List<XmlCookedFile>();
            List<String> tcv4Warnings = new List<String>();
            foreach (String xmlFilePath in xmlFiles)
            {
                String path = xmlFilePath;
                //path = @"D:\Games\Hellgate London\data\mp_hellgate_1.10.180.3416_1.0.86.4580\data\background\city\treasury\cap_path.xml.cooked";

                //if (path.Contains("mp_hellgate")) continue;                 // todo: can't do these yet
                //if (path.Contains("path")) continue;                 // todo: can't do these yet

                XmlCookedFile xmlCookedFile = new XmlCookedFile(Path.GetFileName(path));
                byte[] data = File.ReadAllBytes(path);

                String fileName = path.Replace(@"D:\Games\Hellgate London\", "");
                //Console.WriteLine("Uncooking: " + fileName);

                try
                {
                    xmlCookedFile.ParseFileBytes(data);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to uncooked!\n" + e);
                    continue;
                }

                xmlCookedFile.SaveXml(path.Replace(".cooked", ""));
                count++;

                if (xmlCookedFile.HasExcelStringsMissing) excelStringWarnings.Add(xmlCookedFile);
                if (xmlCookedFile.HasTCv4Elements) tcv4Warnings.Add(Path.GetFileName(fileName));
                if (xmlCookedFile.HasExcelStringsMissing || xmlCookedFile.HasTCv4Elements) continue;

                XmlCookedFile recookedXmlFile = new XmlCookedFile();
                byte[] recookedData = recookedXmlFile.CookXmlDocument(xmlCookedFile.XmlDoc);

                // if file passes byte-byte test, then continue
                if (data.SequenceEqual(recookedData) ||
                    path.Contains("sevenbranchsword_mesh_appearance.xml") ||    // this file has some weird bytes in a string element
                    path.Contains("focus_item11_mesh_appearance.xml") ||        // this file has non-zeroed flag base masks (all differing)
                    path.Contains("focus_item10_mesh_appearance.xml") ||        // as above     // (all 3 probably from not zeroing a ptr at original cooking)
                    path.Contains("thirdpersononly.xml.cooked") ||              // this file has 3xBitFlag Elements (from ConditionsDefinition) exceeding the file buffer
                    path.Contains("dizzy_reverb.xml.cooked")                    // as above, but in SoundReverbDefinition
                   ) continue;

                File.WriteAllBytes(path + "2", recookedData);
            }

            TextWriter consoleOut = Console.Out;
            TextWriter textWriter = new StreamWriter("uncook_results.txt");
            Console.SetOut(textWriter);
            Console.WriteLine("XML Files Uncooked: " + count);
            if (excelStringWarnings.Count > 0)
            {
                Console.WriteLine("Warning: " + excelStringWarnings.Count + " files had excel strings missing:");
                foreach (XmlCookedFile xmlCookedFile in excelStringWarnings)
                {
                    Console.WriteLine("\t" + xmlCookedFile.FileName);
                    foreach (String str in xmlCookedFile.ExcelStringsMissing) Console.WriteLine("\t\t- \"" + str + "\"");
                }
            }
            if (tcv4Warnings.Count > 0)
            {
                Console.WriteLine("Warning: " + tcv4Warnings.Count + " files had TCv4-specific elements:");
                foreach (String str in tcv4Warnings) Console.WriteLine("\t" + str);
            }
            textWriter.Close();
            Console.SetOut(consoleOut);
        }

        #endregion

        /// <summary>
        /// Checks the registry for the Hellgate path, if it doesn't exist prompt the user to find it.
        /// </summary>
        /// <returns>True if the installation is okay.</returns>
        private static bool CheckInstallation()
        {
            if (Directory.Exists(Config.HglDir)) return true;

            string caption = "Reanimator Installation";
            string message = "Please locate your Hellgate London installation directory.\n" +
                             "For this program to work correctly, please ensure the latest Single Player patch is installed.\n" +
                             "For more information, please visit our website: http://www.hellgateaus.net";
            MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);

            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            DialogResult installResult;

            do
            {
                DialogResult selectPathResult = folderBrowser.ShowDialog();
                if ((selectPathResult == DialogResult.OK))
                {
                    Config.HglDir = folderBrowser.SelectedPath;
                    Config.HglDataDir = Path.Combine(Config.HglDir, "\\data");
                    return true;
                }

                caption = "Installation Error";
                message = "You must have Hellgate: London installed and the directory set to use Reanimator.";
                installResult = MessageBox.Show(message, caption, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }
            while (installResult == DialogResult.Retry);

            return false;
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = (!(String.IsNullOrEmpty(Config.LastDirectory))) ? Config.LastDirectory : Config.HglDataDir,
                Filter = "Hellgate London Files (*.*)|*.idx;*.txt.cooked;*.xls.uni.cooked;*.xml.cooked;*.hg1|" +
                         "Index Files|*.idx|" +
                         "Excel Files|*.txt.cooked|" +
                         "String Files|*.xls.uni.cooked|" +
                         "XML Files|*.xml.cooked|" +
                         "Save Files|*.hg1"
            };

            if ((openFileDialog.ShowDialog(this) != DialogResult.OK)) return;

            string fileName = openFileDialog.FileName;
            Config.LastDirectory = Path.GetDirectoryName(fileName);

            if ((fileName.EndsWith(".idx")))
            {
                OpenIndexFile(fileName);
                return;
            }

            if ((fileName.EndsWith(".txt.cooked")))
            {
                OpenExcelFile(fileName);
                return;
            }

            if ((fileName.EndsWith(".xls.uni.cooked")))
            {
                // Open String File
                return;
            }

            if ((fileName.EndsWith(".xml.cooked")))
            {
                // Open Xml File
                return;
            }

            if ((fileName.EndsWith(".hg1")))
            {
                // Open Save File
                return;
            }
        }

        /// <summary>
        /// Opens a TableForm based on the path to a Index or StringsFile.
        /// </summary>
        /// <param name="filePath">Path to the Index or StringsFile.</param>
        private void OpenIndexFile(String filePath)
        {
            byte[] buffer;
            Hellgate.IndexFile indexFile;
            TableForm tableForm;

            // Check if the form is already open.
            // If true, then activate the form.
            bool isOpen = _openTableForms.Where(tf => tf.FilePath == filePath).Any();
            if (isOpen)
            {
                tableForm = _openTableForms.Where(tf => tf.FilePath == filePath).First();
                if ((tableForm.Created))
                {
                    tableForm.Select();
                    return;
                }
            }

            // Try read the file.
            // If an exception is caught, log the error and inform the user.
            try
            {
                buffer = File.ReadAllBytes(filePath);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, false);
                return;
            }

            // Initialize the indexFile.
            indexFile = new Hellgate.IndexFile(buffer)
            {
                FilePath = filePath
            };

            // If the Index file is initialized without error, load the form.
            // Otherwise, show a message box.
            if ((indexFile.HasIntegrity == true))
            {
                tableForm = new TableForm(indexFile)
                {
                    MdiParent = this
                };
                if (!(_openTableForms.Contains(tableForm)))
                    _openTableForms.Add(tableForm);
                tableForm.Show();
            }
            else
            {
                string message = String.Format("The index file {0} appears invalid or malformed.", filePath);
                string caption = "Bad File Format";
                MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Opens a ExcelTableForm based on the given path of a Excel/Strings file.
        /// </summary>
        /// <param name="filePath">Path to the file to open.</param>
        private void OpenExcelFile(String filePath)
        {
            //byte[] buffer;
            //Hellgate.ExcelFile excelFile;
            //ExcelTableForm excelTableForm;

            //// Check if the form is already open.
            //// If true, then activate the form.
            //bool isOpen = _openExcelTableForms.Where(etf => etf.FilePath == filePath).Any();
            //if (isOpen)
            //{
            //    excelTableForm = _openExcelTableForms.Where(etf => etf.FilePath == filePath).First();
            //    if ((excelTableForm.Created))
            //    {
            //        excelTableForm.Select();
            //        return;
            //    }
            //}

            //// Try read the file.
            //// If an exception is caught, log the error and inform the user.
            //try
            //{
            //    buffer = File.ReadAllBytes(filePath);
            //}
            //catch (Exception ex)
            //{
            //    ExceptionLogger.LogException(ex, false);
            //    return;
            //}

            //// Initialize the ExcelFile.
            //excelFile = new Hellgate.ExcelFile(buffer)
            //{
            //    FilePath = filePath
            //};

            //// If the Excel file is initialized without error, load the form.
            //// Otherwise, show a message box.
            //if ((excelFile.IntegrityCheck == true))
            //{
            //    excelTableForm = new ExcelTableForm(excelFile)
            //    {
            //        MdiParent = this
            //    };
            //    if (!(_openExcelTableForms.Contains(excelTableForm)))
            //        _openExcelTableForms.Add(excelTableForm);
            //    excelTableForm.Show();
            //}
            //else
            //{
            //    string message = String.Format("The excel file {0} appears invalid or malformed.", filePath);
            //    string caption = "Bad File Format";
            //    MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }


        private void OpemXmlFile(String filePath)
        {
            byte[] xmlCookedBytes;
            try
            {
                xmlCookedBytes = File.ReadAllBytes(filePath);
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to read in file!\n\n" + e, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            XmlCookedFile xmlCookedFile = new XmlCookedFile();
            try
            {
                xmlCookedFile.ParseFileBytes(xmlCookedBytes);
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to uncook xml file!\n" + e, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            String xmlPath = filePath.Replace(".cooked", "");
            try
            {
                xmlCookedFile.SaveXml(xmlPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save uncooked xml file!\n\n" + ex, "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            // this is a bit dodgy using exceptions as if-else, but meh
            // todo: add check for file name existence etc
            try
            {
                Process notePad = new Process { StartInfo = { FileName = "notepad++.exe", Arguments = xmlPath } };
                notePad.Start();
            }
            catch (Exception)
            {
                Process notePad = new Process { StartInfo = { FileName = "notepad.exe", Arguments = xmlPath } };
                notePad.Start();
            }
        }


        //private void _OpenFileHg1(String fileName)
        //{
        //    try
        //    {
        //        Unit heroUnit = UnitHelpFunctions.OpenCharacterFile(_tableDataSet, fileName);

        //        //Unit wrapper test
        //        //UnitWrapper wrapper = new UnitWrapper(heroUnit);
        //        //wrapper.Mode.IsElite = true;
        //        ////wrapper.Mode.IsElite = true;
        //        ////UnitWrapper w = new UnitWrapper(wrapper.Items.Items[2]);

        //        ////UnitWrapper drone = new UnitWrapper(wrapper.Drone.Drone);
        //        ////CharacterValues values = drone.Values;

        //        //UnitHelpFunctions.SaveCharacterFile(heroUnit, @"F:\test.hg1");

        //        //Comment me when testing the unit wrapper!!!

        //        HeroEditor2 heroEditor = new HeroEditor2(fileName, _tableDataSet)
        //        {
        //            Text = "Hero Editor: " + fileName,
        //            MdiParent = this
        //        };
        //        heroEditor.Show();
        //        //if (heroUnit.IsGood)
        //        //{
        //        //    HeroEditor heroEditor = new HeroEditor(heroUnit, _tableDataSet, fileName)
        //        //    {
        //        //        Text = "Hero Editor: " + fileName,
        //        //        MdiParent = this
        //        //    };
        //        //    heroEditor.Show();
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLogger.LogException(ex, "OpenFileHg1", false);
        //    }
        //}


        //private void _OpenFileStrings(String fileName)
        //{
        //    // todo: make me neater etc - i.e. merge with cooked file above
        //    // copy-paste for most part
        //    try
        //    {
        //        String name = Path.GetFileNameWithoutExtension(fileName);


        //        // todo: this doesn't work 100% as string IDs are stored with each first letter capitalized
        //        DataFile excelTable = _tableFiles.GetTableFromFileName(name);
        //        // todo: Add check for file differing from what's in dataset, and open as new file if different etc
        //        if (excelTable == null)
        //        {
        //            MessageBox.Show("TODO");
        //            return;
        //        }

        //        ExcelTableForm excelTableForm = new ExcelTableForm(excelTable, _tableDataSet)
        //        {
        //            Text = "Excel Table: " + fileName,
        //            MdiParent = this
        //        };
        //        excelTableForm.Show();
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLogger.LogException(ex, "OpenFileCooked", false);
        //    }
        //}

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /* Doesn't appear to do anything...
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal)
                };

                if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    string fileName = saveFileDialog.FileName;
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "SaveAsToolStripMenuItem_Click");
            }
             */
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }


        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                if (childForm.Name != "ExcelTablesLoaded")
                {
                    childForm.Close();
                }
            }
        }

        private void _OptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _optionsForm.ShowDialog(this);
        }

        private void _ClientPatcherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "EXE Files (*.exe)|*.exe|All Files (*.*)|*.*";
            openFileDialog.InitialDirectory = Config.HglDir + "\\SP_x64";
            if (openFileDialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            FileStream clientFile;
            try
            {
                clientFile = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.ReadWrite);
            }
            catch (Exception)
            {
                return;
            }

            Patches clientPatcher = new Patches(FileTools.StreamToByteArray(clientFile));
            if (clientPatcher.ApplyHardcorePatch())
            {
                FileStream fileOut = new FileStream(openFileDialog.FileName + ".patched.exe", FileMode.Create);
                fileOut.Write(clientPatcher.Buffer, 0, clientPatcher.Buffer.Length);
                fileOut.Dispose();
                MessageBox.Show("Hardcore patch applied!");
            }
            else
            {
                MessageBox.Show("Failed to apply Hardcore patch!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            clientFile.Dispose();
        }

        private void _Reanimator_ResizeEnd(object sender, EventArgs e)
        {
            Config.ClientHeight = Height;
            Config.ClientWidth = Width;
        }

        private void _Reanimator_Load(object sender, EventArgs e)
        {
            if (AlexInstaLoad) return;

            try
            {
                Height = Config.ClientHeight;
                Width = Config.ClientWidth;
                Show();
                Refresh();

                if (CheckInstallation())
                {
                    ProgressForm progressForm = new ProgressForm(_DoLoading, null);
                    progressForm.SetStyle(ProgressBarStyle.Marquee);
                    progressForm.SetLoadingText("Initializing Reanimator subsystems...");
                    progressForm.Disposed += delegate
                    {
                        _fileExplorer.MdiParent = this;
                        _fileExplorer.Show();

                        _tablesLoaded.MdiParent = this;
                        _tablesLoaded.Bounds = new Rectangle(_fileExplorer.Size.Width + 10, 0, 300, 350);
                        _tablesLoaded.Text = "Hellgate Tables Loaded [" + _fileManager.DataFiles.Count + "]";
                        _tablesLoaded.Show();

                        if (_tablesLoadedTCv4 != null)
                        {
                            _tablesLoadedTCv4.MdiParent = this;
                            _tablesLoadedTCv4.Bounds = new Rectangle(_fileExplorer.Size.Width + 10, 350 + 10, 300, 350);
                            _tablesLoadedTCv4.Text = "Hellgate TCv4 Tables Loaded [" + _fileManagerTCv4.DataFiles.Count + "]";
                            _tablesLoadedTCv4.Show();
                        }

                        XmlCookedFile.Initialize(_fileManager);
                    };
                    progressForm.Show(this);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "Reanimator_Load", false);
                MessageBox.Show(ex.Message, "Reanimator_Load");
            }
        }

        private void _DoLoading(ProgressForm progressForm, Object var)
        {
            progressForm.SetCurrentItemText("Loading File Manager...");
            _fileManager = new FileManager(Config.HglDir, false);

            progressForm.SetCurrentItemText("Loading Excel and Strings Tables...");
            if (!_fileManager.LoadTableFiles())
            {
                MessageBox.Show("Failed to load excel and strings files!", "Data Table Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            ExcelScript.SetFileManager(_fileManager);

            progressForm.SetCurrentItemText("Loading File Explorer...");
            _fileExplorer = new FileExplorer(_fileManager);

            progressForm.SetCurrentItemText("Loading Table View...");
            _tablesLoaded = new TablesLoaded(_fileManager);

            if (!Config.LoadTCv4DataFiles) return;

            progressForm.SetCurrentItemText("Loading TCv4 File Manager...");
            _fileManagerTCv4 = new FileManager(Config.HglDir, true);

            progressForm.SetCurrentItemText("Loading TCv4 Excel and Strings Tables...");
            if (!_fileManagerTCv4.LoadTableFiles())
            {
                MessageBox.Show("Failed to load TCv4 excel and strings files!", "Data Table Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }

            progressForm.SetCurrentItemText("Loading TCv4 Table View...");
            _tablesLoadedTCv4 = new TablesLoaded(_fileManagerTCv4);
        }

        private void _SaveToolStripButton_Click(object sender, EventArgs e)
        {
            IMdiChildBase mdiChildBase = ActiveMdiChild as IMdiChildBase;
            if (mdiChildBase == null) return;

            mdiChildBase.SaveButton();
        }

        private void _HardcoreModex64DX9ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Exectuable Files (*.exe)|*.exe|All Files (*.*)|*.*",
                InitialDirectory = Config.HglDir
            };
            if (openFileDialog.ShowDialog(this) != DialogResult.OK || !openFileDialog.FileName.EndsWith("exe")) return;


            Patches hglexe = new Patches(File.ReadAllBytes(openFileDialog.FileName));
            try
            {
                hglexe.ApplyHardcorePatch();
                File.WriteAllBytes(openFileDialog.FileName.Insert(openFileDialog.FileName.Length - 4, "-patched"),
                                   hglexe.Buffer);
                MessageBox.Show("Patch successfully applied!");
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "_HardcoreModex64DX9ToolStripMenuItem_Click", false);
                MessageBox.Show("Problem Applying Patch. :(");
            }
        }

        private void _ShowExcelTablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (showExcelTablesToolStripMenuItem.Checked)
                {
                    _tablesLoaded.Location = new Point(0, 0);
                    _tablesLoaded.Show();
                }
                else
                {
                    _tablesLoaded.Hide();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "_ShowExcelTablesToolStripMenuItem_Click", false);
            }
        }

        private void _ApplyModificationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    ModificationForm modificationForm = new ModificationForm(_tableDataSet);
            //    modificationForm.ShowDialog();
            //}
            //catch (Exception ex)
            //{
            //    ExceptionLogger.LogException(ex, "_ApplyModificationsToolStripMenuItem_Click", false);
            //}
        }

        private void _TradeItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    //MessageBox.Show("Tables are being loaded. This may take a few seconds!");

            //    //ItemTransferForm transfer = new ItemTransferForm(_tableDataSet, _tableFiles);
            //    ItemTransferForm transfer = new ItemTransferForm(_tableDataSet, _fileExplorer);
            //    //Displays a warning message before opening the item trading window.
            //    transfer.DisplayWarningMessage(null, null);
            //    transfer.ShowDialog(this);
            //    transfer.Dispose();
            //}
            //catch (Exception ex)
            //{
            //    ExceptionLogger.LogException(ex, "_TradeItemsToolStripMenuItem_Click", false);
            //}
        }

        private void _SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void _ItemShopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    CharacterShop shop = new CharacterShop(_tableDataSet, _tableFiles);
            //    shop.ShowDialog(this);
            //}
            //catch (Exception ex)
            //{
            //    ExceptionLogger.LogException(ex, "_ItemShopToolStripMenuItem_Click", false);
            //}
        }

        private void _SearchTablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    TableSearch search = new TableSearch(_tableDataSet, _tableFiles);
            //    search.ShowDialog(this);
            //}
            //catch (Exception ex)
            //{
            //    ExceptionLogger.LogException(ex, "_SearchTablesToolStripMenuItem_Click", false);
            //}
        }

        private void _AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Reanimator by the Revival Team (c) 2009-2010" + Environment.NewLine
                + "Credits: Maeyan, Alex2069, Kite & Malachor" + Environment.NewLine
                + "For more info visit us at: http://www.hellgateaus.net", "Credits", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void _ScriptEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    ScriptEditor scriptEditor = new ScriptEditor(_tableDataSet);
            //    scriptEditor.MdiParent = this;
            //    scriptEditor.Show();
            //}
            //catch (Exception ex)
            //{
            //    ExceptionLogger.LogException(ex, "_ScriptEditorToolStripMenuItem_Click", false);
            //}
        }

        private void _PatchToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                PatchForm patchForm = new PatchForm { MdiParent = this };
                patchForm.Show();
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "_PatchToolToolStripMenuItem_Click", false);
            }
        }

        private void _ConvertTestCenterFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //// Select Dump location
            //FolderBrowserDialog folderBrower = new FolderBrowserDialog();
            //folderBrower.SelectedPath = Config.HglDataDir;
            //DialogResult dialogResult = folderBrower.ShowDialog();
            //if (dialogResult == DialogResult.Cancel) return;

            //// cache all tables
            //ProgressForm cacheTableProgress = new ProgressForm(_LoadAllExcelTables, null);
            //cacheTableProgress.SetLoadingText("Caching all tables.");
            //cacheTableProgress.ShowDialog();

            //// generate all tables
            //ProgressForm generateTableProgress = new ProgressForm(_ConvertTestCenterFiles, folderBrower.SelectedPath);
            //generateTableProgress.SetLoadingText("Generating all converted tables, this will take a while.");
            //generateTableProgress.SetStyle(ProgressBarStyle.Marquee);
            //generateTableProgress.ShowDialog();

            //MessageBox.Show("Complete");
        }

        private void _SaveSinglePlayerFiles(ProgressForm progress, object obj)
        {
            //    string savePath = (string)obj;

            //    foreach (DataTable spDataTable in _tableDataSet.XlsDataSet.Tables)
            //    {
            //        if (spDataTable.TableName.Contains("_TCv4_")) continue;
            //        if (spDataTable.TableName.Contains("Strings_")) continue;

            //        progress.SetCurrentItemText("Current table... " + spDataTable.TableName);

            //        ExcelFile spExcelFile = _tableDataSet.TableFiles.GetExcelTableFromId(spDataTable.TableName);

            //        byte[] buffer = spExcelFile.GenerateFile(spDataTable);
            //        string path = Path.Combine(savePath, spExcelFile.FilePath);
            //        string filename = spExcelFile.FileName + "." + spExcelFile.FileExtension;

            //        if (!Directory.Exists(path))
            //        {
            //            Directory.CreateDirectory(path);
            //        }

            //        File.WriteAllBytes(path + filename, buffer);
            //    }
        }

        //private void _ConvertTestCenterFiles(ProgressForm progress, object obj)
        //{
        //    string savePath = (string)obj;

        //    foreach (DataTable tcDataTable in _tableDataSet.XlsDataSet.Tables)
        //    {
        //        if (!tcDataTable.TableName.Contains("_TCv4_")) continue;
        //        string spVersion = tcDataTable.TableName.Replace("_TCv4_", "");

        //        progress.SetCurrentItemText("Current table... " + spVersion);

        //        DataTable spDataTable = _tableDataSet.XlsDataSet.Tables[spVersion];
        //        DataTable convertedDataTable = ExcelFile.ConvertToSinglePlayerVersion(spDataTable, tcDataTable);
        //        ExcelFile tcExcelFile = _tableDataSet.TableFiles.GetExcelTableFromId(tcDataTable.TableName);
        //        ExcelFile spExcelFile = _tableDataSet.TableFiles.GetExcelTableFromId(spVersion);

        //        spExcelFile.MyshBytes = tcExcelFile.MyshBytes;

        //        byte[] buffer = spExcelFile.GenerateFile(convertedDataTable);
        //        string path = Path.Combine(savePath, spExcelFile.FilePath);
        //        string filename = spExcelFile.FileName + "." + spExcelFile.FileExtension;

        //        if (!Directory.Exists(path))
        //        {
        //            Directory.CreateDirectory(path);
        //        }

        //        File.WriteAllBytes(path + filename, buffer);
        //    }
        //}

        //private void saveSinglePlayerFilesToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    // Select Dump location
        //    FolderBrowserDialog folderBrower = new FolderBrowserDialog();
        //    folderBrower.SelectedPath = Config.HglDataDir;
        //    DialogResult dialogResult = folderBrower.ShowDialog();
        //    if (dialogResult == DialogResult.Cancel) return;

        //    // cache all tables
        //    ProgressForm cacheTableProgress = new ProgressForm(_LoadAllExcelTables, null);
        //    cacheTableProgress.SetLoadingText("Caching all tables.");
        //    cacheTableProgress.ShowDialog();

        //    // generate all tables
        //    ProgressForm generateTableProgress = new ProgressForm(_SaveSinglePlayerFiles, folderBrower.SelectedPath);
        //    generateTableProgress.SetLoadingText("Saving all single player files, this will take a while.");
        //    generateTableProgress.SetStyle(ProgressBarStyle.Marquee);
        //    generateTableProgress.ShowDialog();

        //    MessageBox.Show("Complete");
        //}
    }
}