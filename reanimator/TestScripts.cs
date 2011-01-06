using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using Hellgate;
using Revival.Common;

namespace Reanimator
{
    public static class TestScripts
    {
        public static void ExtractAllCSV()
        {
            const String root = @"C:\test_mod\";
            FileManager fileManager = new FileManager(Config.HglDir);
            fileManager.LoadTableFiles();

            foreach (DataFile dataFile in fileManager.DataFiles.Values)
            {
                if (!dataFile.IsExcelFile) continue;

                ExcelFile excelFile = (ExcelFile) dataFile;
                byte[] csvBytes = excelFile.ExportCSV(fileManager);

                String savePath = Path.Combine(root, excelFile.FilePath).Replace(ExcelFile.Extension, ExcelFile.ExtensionDeserialised);
                Directory.CreateDirectory(Directory.GetDirectoryRoot(savePath));
                File.WriteAllBytes(savePath, csvBytes);
            }
        }

        public static void TestAllCodeValues()
        {
            FileManager fileManager = new FileManager(Config.HglDir);
            fileManager.LoadTableFiles();

            int fileCountWithCode = 0;
            foreach (DataFile dataFile in fileManager.DataFiles.Values)
            {
                if (!dataFile.IsExcelFile) continue;

                ExcelFile excelFile = (ExcelFile)dataFile;

                Debug.Write(String.Format("Checking code fields within file {0}... ", excelFile.StringId));

                FieldInfo[] fieldInfos = excelFile.Attributes.RowType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                FieldInfo[] codeFields = (from fi in fieldInfos
                                          where fi.Name == "code"
                                          select fi).ToArray();

                if (codeFields.Length == 0)
                {
                    Debug.WriteLine("None found.");
                    continue;
                }

                //if (excelFile.StringId == "ACT")
                //{
                //    int bp = 0;

                //    ExcelFile.Code codeObj = new ExcelFile.Code();
                //    codeObj = "AAB";

                //    int codeInt = codeObj;
                //}

                Debug.Assert(codeFields.Length == 1);
                Debug.WriteLine(String.Format("{0} fields found.", codeFields.Length));
                fileCountWithCode++;

                ObjectDelegator objectDelegator = new ObjectDelegator(codeFields);

                Debug.WriteLine("Codes found: ");
                foreach (FieldInfo fieldInfo in codeFields)
                {
                    foreach (Object row in excelFile.Rows)
                    {
                        int code = 0;
                        if (fieldInfo.FieldType == typeof(short))
                        {
                            code = (int)(short)objectDelegator["code"](row);
                        }
                        //else if (fieldInfo.FieldType == typeof(ExcelFile.Code))
                        //{
                        //    code = (ExcelFile.Code)objectDelegator["code"](row);
                        //}
                        //else if (fieldInfo.FieldType == typeof(ExcelFile.ShortCode))
                        //{
                        //    code = (ExcelFile.ShortCode)objectDelegator["code"](row);
                        //}
                        else
                        {
                            code = (int)objectDelegator["code"](row);
                        }

                        if (code == 0) continue;

                        char char1 = (char)(code & 0xFF);
                        char char2 = (char)((code & 0xFF00) >> 8);
                        char char3 = (char)((code & 0xFF0000) >> 16);
                        char char4 = (char)((code & 0xFF000000) >> 24);

                        Debug.Assert(_CharCodeCharValue(char1));
                        Debug.Assert(_CharCodeCharValue(char2));
                        Debug.Assert(_CharCodeCharValue(char3));
                        Debug.Assert(_CharCodeCharValue(char4));

                        Debug.Write(String.Format(" {0}{1}{2}{3}", char1, char2, char3, char4));
                    }
                }

                Debug.WriteLine("");
            }

            Debug.WriteLine(String.Format("{0} files with code fields checked.", fileCountWithCode));
        }

        /// <summary>
        /// Tests if a char is within the valid code range.
        /// </summary>
        /// <param name="c">The char to test.</param>
        /// <returns>True if within range, false otherwise.</returns>
        private static bool _CharCodeCharValue(char c)
        {
            return ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9') || c == 0 || (c >= 0xC9 && c <= 0xD2) /* Only Region table has this last chunk */);
        }

        /// <summary>
        /// Function to test all excel files cooking/recooking/etc to/from byte arrays and csv types.
        /// </summary>
        public static void TestExcelCooking(bool doTCv4 = false)
        {
            String root = @"C:\excel_debug";
            if (doTCv4) root = Path.Combine(root, "tcv4");
            root += @"\"; // lazy
            Directory.CreateDirectory(root);

            FileManager fileManager = new FileManager(Config.HglDir, doTCv4);
            fileManager.LoadTableFiles();
            ExcelFile.EnableDebug = true;

            foreach (DataFile dataFile in fileManager.DataFiles.Values)
            {
                ExcelFile excelFile = dataFile as ExcelFile;
                if (excelFile == null) continue;

                //if (excelFile.StringId != "SKILLS") continue;

                Debug.Write(String.Format("Checking {0}... ", dataFile.StringId));

                byte[] fileBytes = fileManager.GetFileBytes(excelFile.FilePath, true);
                try
                {
                    byte[] dataFileBytes = dataFile.ToByteArray();
                    if (dataFile.StringId == "SOUNDS" && false)
                    {
                        byte[] csvBytesSounds = excelFile.ExportCSV(fileManager);
                        ExcelFile soundsCSV = new ExcelFile(excelFile.FilePath);
                        soundsCSV.ParseCSV(csvBytesSounds, fileManager);
                        byte[] soundsBytes = soundsCSV.ToByteArray();
                        //byte[] soundsBytesFromCSV = soundsCSV.ExportCSV();
                        //ExcelFile soundsCSVFromBytesFromCSV = new ExcelFile(soundsBytesFromCSV, fileEntry.RelativeFullPathWithoutPatch);

                        // some brute force ftw
                        byte[][] bytesArrays = new[] { fileBytes, soundsBytes };
                        for (int z = 0; z < bytesArrays.Length; z++)
                        {
                            byte[] bytes = bytesArrays[z];

                            int offset = 0x20;
                            int stringsBytesCount = FileTools.ByteArrayToInt32(bytes, ref offset);

                            StringWriter stringWriterByteStrings = new StringWriter();
                            stringWriterByteStrings.WriteLine(stringsBytesCount + " bytes");
                            List<String> strings = new List<String>();
                            List<int> offsets = new List<int>();

                            while (offset < stringsBytesCount + 0x20)
                            {
                                String str = FileTools.ByteArrayToStringASCII(bytes, offset);
                                strings.Add(str);
                                offsets.Add(offset);

                                offset += str.Length + 1;
                            }

                            String[] sortedStrings = strings.ToArray();
                            int[] sortedOffsets = offsets.ToArray();
                            Array.Sort(sortedStrings, sortedOffsets);
                            stringWriterByteStrings.WriteLine(strings.Count + " strings");
                            for (int i = 0; i < strings.Count; i++)
                            {
                                stringWriterByteStrings.WriteLine(sortedStrings[i] + "\t\t\t" + sortedOffsets[i]);
                            }

                            File.WriteAllText(@"C:\excel_debug\strings" + z + ".txt", stringWriterByteStrings.ToString());
                        }
                    }

                    Debug.Write("ToByteArray... ");
                    if (fileBytes.Length != dataFileBytes.Length)
                    {
                        Debug.WriteLine("ToByteArray() dataFileBytes has differing length: " + dataFile.StringId);
                        File.WriteAllBytes(root + dataFile.StringId + ".orig", fileBytes);
                        File.WriteAllBytes(root + dataFile.StringId + ".toByteArray", dataFileBytes);
                        continue;
                    }

                    ExcelFile fromBytesExcel = new ExcelFile(excelFile.FilePath, fileManager.MPVersion);
                    fromBytesExcel.ParseData(dataFileBytes);
                    Debug.Write("new ExcelFile... ");
                    if (!fromBytesExcel.HasIntegrity)
                    {
                        Debug.WriteLine("fromBytesExcel = new Excel from ToByteArray() failed!");
                        continue;
                    }


                    // more checks
                    Debug.Write("ToByteArray -> ToByteArray... ");
                    byte[] dataFileBytesFromToByteArray = fromBytesExcel.ToByteArray();
                    if (fileBytes.Length != dataFileBytesFromToByteArray.Length)
                    {
                        Debug.WriteLine("ToByteArray() dataFileBytesFromToByteArray has differing length!");
                        continue;
                    }


                    // check generated sort index arrays);
                    Debug.Write("IndexSortArrays... ");
                    if (excelFile.IndexSortArray != null)
                    {
                        if (fromBytesExcel.IndexSortArray == null || excelFile.IndexSortArray.Count != fromBytesExcel.IndexSortArray.Count)
                        {
                            Debug.WriteLine("fromBytesExcel has not-matching IndexSortArray count!");
                            continue;
                        }

                        bool hasError = false;
                        for (int i = 0; i < excelFile.IndexSortArray.Count; i++)
                        {
                            if (excelFile.IndexSortArray[i].SequenceEqual(fromBytesExcel.IndexSortArray[i])) continue;

                            Debug.WriteLine(String.Format("IndexSortArray[{0}] NOT EQUAL to original!", i));
                            hasError = true;
                        }

                        if (hasError)
                        {
                            File.WriteAllBytes(root + dataFile.StringId + ".orig", fileBytes);
                            File.WriteAllBytes(root + dataFile.StringId + ".toByteArrayFromByteArray", dataFileBytesFromToByteArray);
                            continue;
                        }
                    }


                    // some csv stuff
                    Debug.Write("ExportCSV -> ");
                    byte[] csvBytes = fromBytesExcel.ExportCSV(fileManager);
                    Debug.Write("new ExcelFile");
                    ExcelFile csvExcel = new ExcelFile(excelFile.FilePath, fileManager.MPVersion);
                    csvExcel.ParseCSV(csvBytes, fileManager);
                    Debug.Write("... ");
                    if (!csvExcel.HasIntegrity)
                    {
                        Debug.WriteLine("Failed!");
                        File.WriteAllBytes(root + dataFile.StringId + ".orig", fileBytes);
                        File.WriteAllBytes(root + dataFile.StringId + ".csv", csvBytes);
                        File.WriteAllBytes(root + dataFile.StringId + ".toByteArray", dataFileBytes);
                        File.WriteAllBytes(root + dataFile.StringId + ".toByteArrayFromByteArray", dataFileBytesFromToByteArray);
                        continue;
                    }

                    byte[] recookedExcelBytes = csvExcel.ToByteArray();
                    if (!fileManager.MPVersion)
                    {
                        Debug.Write("StructureId... ");
                        UInt32 structureId = BitConverter.ToUInt32(fileBytes, 4);
                        UInt32 fromCSVStructureId = BitConverter.ToUInt32(recookedExcelBytes, 4);
                        if (structureId != fromCSVStructureId)
                        {
                            Debug.WriteLine("Structure Id value do not match: " + structureId + " != " + fromCSVStructureId);
                            continue;
                        }
                    }

                    Debug.Write("Final Checks... ");
                    int recookedLength = recookedExcelBytes.Length;
                    if (excelFile.StringId == "SKILLS") recookedLength += 12; // 12 bytes in int ptr data not used/referenced at all and are removed/lost in bytes -> csv -> bytes
                    if (fileBytes.Length != recookedLength)
                    {
                        ExcelFile finalExcelDump = new ExcelFile(excelFile.FilePath, fileManager.MPVersion);
                        finalExcelDump.ParseData(recookedExcelBytes);
                        byte[] csvDump = finalExcelDump.ExportCSV(fileManager);

                        Debug.WriteLine("Recooked Excel file has differing length!");
                        File.WriteAllBytes(root + dataFile.StringId + ".orig", fileBytes);
                        File.WriteAllBytes(root + dataFile.StringId + ".txt", csvBytes);
                        File.WriteAllBytes(root + dataFile.StringId + ".recooked.txt", csvDump);
                        File.WriteAllBytes(root + dataFile.StringId + ".toByteArray", dataFileBytes);
                        File.WriteAllBytes(root + dataFile.StringId + ".toByteArrayFromByteArray", dataFileBytesFromToByteArray);
                        File.WriteAllBytes(root + dataFile.StringId + ".recookedExcelBytes", recookedExcelBytes);
                        continue;
                    }

                    ExcelFile finalExcel = new ExcelFile(excelFile.FilePath, fileManager.MPVersion);
                    finalExcel.ParseData(recookedExcelBytes);
                    Debug.Assert(finalExcel.HasIntegrity);
                    byte[] finalCheck = finalExcel.ToByteArray();
                    if (excelFile.StringId == "SKILLS") Debug.Assert(finalCheck.Length + 12 == dataFileBytes.Length);
                    else Debug.Assert(finalCheck.Length == dataFileBytes.Length);
                    byte[] csvCheck = finalExcel.ExportCSV(fileManager);

                    if (!csvBytes.SequenceEqual(csvCheck))
                    {
                        Debug.WriteLine("csvBytes.SequenceEqual failed!");
                        File.WriteAllBytes(root + dataFile.StringId + ".txt", csvBytes);
                        File.WriteAllBytes(root + dataFile.StringId + ".final.txt", csvCheck);
                        continue;
                    }

                    Debug.WriteLine("OK");
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Excel file Exception: " + dataFile.StringId + "\n" + e);
                }
            }

            Debug.WriteLine("All excel files checked.");
        }

        /// <summary>
        /// Function to conver the TCv4 excel files to SP client formats.
        /// </summary>
        public static void ConvertTCv4ExcelToSP()
        {
            // init file manager and load excel files
            FileManager fileManager = new FileManager(Config.HglDir);
            fileManager.ExtractAllExcel();
            FileManager fileManagerTCv4 = new FileManager(Config.HglDir, true);

            fileManager.LoadTableFiles();
            fileManagerTCv4.LoadTableFiles(true);

            if (fileManager.DataFiles.Count == 0)
            {
                Debug.WriteLine("Error: No Excel files loaded!");
                return;
            }
            if (fileManagerTCv4.DataFiles.Count == 0)
            {
                Debug.WriteLine("Error: No TCv4 Excel files loaded!");
                return;
            }

            // convert tables
            int converted = -1;
            Dictionary<String, ObjectDelegator> objectDelegators = new Dictionary<String, ObjectDelegator>();
            foreach (ExcelFile excelFile in fileManager.DataFiles.Values.Where(dataFile => dataFile.IsExcelFile))
            {
                //if (++converted >= 5) break;

                Debug.WriteLine("Converting Excel table " + excelFile.StringId + "...");

                // obviously don't want to convert this one
                if (excelFile.StringId == "EXCELTABLES") continue;

                // we can't convert stats as some index entries are hard-coded...
                if (excelFile.StringId == "STATS") continue;

                // game crashes with this converted...
                if (excelFile.StringId == "UNITMODES") continue;

                // don't bother with these two as there's no TCv4 equivalent (they don't matter anyways)
                if (excelFile.StringId == "LANGUAGE" || excelFile.StringId == "REGION") continue;


                // ensure we have a TCv4 version loaded
                String stringIdTCv4 = "_TCv4_" + excelFile.StringId;
                ExcelFile excelFileTCv4 = (ExcelFile)fileManagerTCv4.GetDataFile(stringIdTCv4);
                if (excelFileTCv4 == null)
                {
                    Debug.WriteLine("Error: TCv4 Excel file not found: " + stringIdTCv4);
                    continue;
                }


                // table specialisation stuffs
                bool isAchievements = false;
                bool isAffixes = false;
                bool isCharacterClass = false;
                bool isCharDisplay = false;
                bool isInventory = false;
                switch (excelFile.StringId)
                {
                    case "ACHIEVEMENTS":
                        isAchievements = true;
                        break;

                    case "AFFIXES":
                        isAffixes = true;
                        break;

                    case "CHARACTER_CLASS":
                        isCharacterClass = true;
                        break;

                    case "CHARDISPLAY":
                        isCharDisplay = true;
                        break;

                    case "INVENTORY":
                        isInventory = true;
                        break;
                }


                // genereal type-init stuffs; get our object delegators
                ObjectDelegator excelDelegator;
                ObjectDelegator excelDelegatorTCv41;
                ObjectDelegator excelDelegatorTCv42 = null;
                Type rowType = excelFile.Attributes.RowType;
                Type rowTypeTCv41 = excelFileTCv4.Attributes.RowType;
                Type rowTypeTCv42 = null;
                FieldInfo[] fieldInfos = rowType.GetFields();
                FieldInfo[] fieldInfosTCv41 = rowTypeTCv41.GetFields();
                FieldInfo[] fieldInfosTCv42 = null;


                // need to copy row headers (private field) as well
                FieldInfo rowHeaderField = rowType.GetField("header", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                FieldInfo rowHeaderFieldTCv4 = rowTypeTCv41.GetField("header", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                Array.Resize(ref fieldInfos, fieldInfos.Length + 1);
                Array.Resize(ref fieldInfosTCv41, fieldInfosTCv41.Length + 1);
                fieldInfos[fieldInfos.Length - 1] = rowHeaderField;
                fieldInfosTCv41[fieldInfosTCv41.Length - 1] = rowHeaderFieldTCv4;


                // create delegates
                if (!objectDelegators.TryGetValue(excelFile.StringId, out excelDelegator))
                {
                    excelDelegator = new ObjectDelegator(fieldInfos);
                    objectDelegators.Add(excelFile.StringId, excelDelegator);
                }
                if (!objectDelegators.TryGetValue(stringIdTCv4, out excelDelegatorTCv41))
                {
                    excelDelegatorTCv41 = new ObjectDelegator(fieldInfosTCv41);
                    objectDelegators.Add(stringIdTCv4, excelDelegatorTCv41);
                }

                // table specialisation inititialisation
                //// achievements
                Dictionary<int, int> unitTypeOverflow = null;
                if (isAchievements)
                {
                    unitTypeOverflow = new Dictionary<int, int>();
                }

                //// affixes
                ExcelFile affixGroupsTable = null;
                StringCollection affixGroupsList = null;
                List<Int32> affixGroupWeightScripts = null;
                if (isAffixes)
                {
                    affixGroupsTable = fileManagerTCv4.GetDataFile("_TCv4_AFFIX_GROUPS") as ExcelFile;
                    affixGroupsList = new StringCollection();
                    affixGroupWeightScripts = new List<Int32>();
                    Debug.Assert(affixGroupsTable != null);

                    rowTypeTCv42 = affixGroupsTable.Attributes.RowType;
                    fieldInfosTCv42 = rowTypeTCv42.GetFields();

                    FieldInfo rowHeaderFieldTCv42 = rowTypeTCv42.GetField("header", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    Array.Resize(ref fieldInfosTCv42, fieldInfosTCv42.Length + 1);
                    fieldInfosTCv42[fieldInfosTCv42.Length - 1] = rowHeaderFieldTCv42;

                    excelDelegatorTCv42 = new ObjectDelegator(fieldInfosTCv42);
                }

                // debug: ensure we have same columns
                if (!isAchievements && !isCharacterClass && !isAffixes)
                {
                    bool hasSameFields = true;
                    foreach (FieldInfo fieldInfo in fieldInfos)
                    {
                        FieldInfo fieldInfoTCv4 = (from fi in fieldInfosTCv41
                                                   where fi.Name == fieldInfo.Name
                                                   select fi).FirstOrDefault();
                        if (fieldInfoTCv4 == null)
                        {
                            Debug.WriteLine("Field not found in TCv4 table: " + fieldInfo.Name);
                            hasSameFields = false;
                            continue;
                        }

                        if (fieldInfo.FieldType == fieldInfoTCv4.FieldType || fieldInfo.FieldType.BaseType == typeof(Enum)) continue;
                        Debug.WriteLine(String.Format("FieldInfo '{0}' of type '{1}' does not match TCv4 of type '{2}'", fieldInfo.Name, fieldInfo.FieldType, fieldInfoTCv4.FieldType));
                        hasSameFields = false;
                    }

                    if (!hasSameFields)
                    {
                        Debug.WriteLine("Error: The Excel types do not have the same fields!");
                        continue;
                    }
                }

                // begin conversion process
                Object[] rows = new Object[excelFileTCv4.Rows.Count];
                bool failed = false;
                int col = -1;
                byte[] scriptBuffer = new byte[1024];
                int scriptBufferOffset = 1; // first byte is null
                foreach (FieldInfo fieldInfo in fieldInfos) // loop by column
                {
                    col++;

                    ObjectDelegator.FieldGetValueDelegate getTCv4Value1 = null;
                    ObjectDelegator.FieldGetValueDelegate getTCv4Value2 = null;
                    ObjectDelegator.FieldGetValueDelegate getTCv4Value3 = null;
                    ObjectDelegator.FieldSetValueDelegate setValue1 = excelDelegator.GetFieldSetDelegate(fieldInfo.Name);
                    ObjectDelegator.FieldSetValueDelegate setValue2 = null;
                    ObjectDelegator.FieldGetValueDelegate getValue = excelDelegator.GetFieldGetDelegate(fieldInfo.Name);
                    ExcelFile.OutputAttribute outputAttribute = ExcelFile.GetExcelAttribute(fieldInfo);

                    // table specialisation stuffs
                    bool isUnitTypeField = false;                       // achievements
                    bool isItemField = false;                           // achievements
                    bool isUnitVersionToGetSkillRespec = false;         // character class
                    bool isGroup = false;                               // affixes
                    if (isAchievements && (fieldInfo.Name.StartsWith("unitType") || fieldInfo.Name == "item"))
                    {
                        if (fieldInfo.Name.StartsWith("unitType"))
                        {
                            String monsterFieldName = "monsterUnitType" + fieldInfo.Name.Last();
                            String itemFieldName = "itemUnitType" + fieldInfo.Name.Last();

                            getTCv4Value1 = excelDelegatorTCv41.GetFieldGetDelegate(monsterFieldName);
                            getTCv4Value2 = excelDelegatorTCv41.GetFieldGetDelegate(itemFieldName);
                            isUnitTypeField = true;
                        }
                        else if (fieldInfo.Name == "item")
                        {
                            isItemField = true;
                        }
                    }
                    else if (isCharacterClass && fieldInfo.Name == "unitVersionToGetSkillRespec")
                    {
                        isUnitVersionToGetSkillRespec = true;
                    }
                    else if (isAffixes && (fieldInfo.Name == "group" || fieldInfo.Name == "groupWeight"))
                    {
                        if (fieldInfo.Name == "groupWeight") continue; // we can skip it as we're setting it during group column

                        getTCv4Value1 = excelDelegatorTCv41.GetFieldGetDelegate(fieldInfo.Name);    // to get group affix index
                        getTCv4Value2 = excelDelegatorTCv42.GetFieldGetDelegate("name");            // to get group affix name
                        getTCv4Value3 = excelDelegatorTCv42.GetFieldGetDelegate("weight");          // to get group affix weight
                        setValue2 = excelDelegator.GetFieldSetDelegate("groupWeight");              // to set group weight
                        isGroup = true;
                    }
                    else
                    {
                        getTCv4Value1 = excelDelegatorTCv41.GetFieldGetDelegate(fieldInfo.Name);
                    }


                    // copy/convert field data
                    int row = -1;
                    foreach (Object rowTCv4 in excelFileTCv4.Rows) // loop by row
                    {
                        if (rows[++row] == null) rows[row] = Activator.CreateInstance(excelFile.Attributes.RowType);

                        Object value;

                        // achievements special stuffs
                        if (isAchievements && (isUnitTypeField || isItemField))
                        {
                            if (isUnitTypeField)
                            {
                                int monsterValue = (int)getTCv4Value1(rowTCv4);
                                int itemValue = (int)getTCv4Value2(rowTCv4);

                                if (monsterValue != 0 && itemValue != 0)
                                {
                                    Debug.Assert(!unitTypeOverflow.ContainsKey(row));

                                    unitTypeOverflow.Add(row, itemValue);
                                    itemValue = 0;
                                }
                                else if (monsterValue == 0 && itemValue == 0 && unitTypeOverflow.ContainsKey(row))
                                {
                                    itemValue = unitTypeOverflow[row];
                                    unitTypeOverflow.Remove(row);
                                }

                                value = (itemValue == 0) ? monsterValue : itemValue;
                            }
                            else
                            {
                                value = 0;
                            }
                        }
                        else if (isCharacterClass && isUnitVersionToGetSkillRespec)
                        {
                            value = getValue(excelFile.Rows[row]);
                        }
                        else if (isAffixes && isGroup)
                        {
                            Debug.Assert(getTCv4Value1 != null && getTCv4Value2 != null && getTCv4Value3 != null);

                            int affixGroupRowIndex = (int)getTCv4Value1(rowTCv4);

                            if (affixGroupRowIndex == -1)
                            {
                                setValue2(rows[row], 0);
                                value = -1;
                            }
                            else
                            {
                                int affixGroupStringOffset = (int)getTCv4Value2(affixGroupsTable.Rows[affixGroupRowIndex]);
                                int affixGroupWeight = (int)getTCv4Value3(affixGroupsTable.Rows[affixGroupRowIndex]);

                                setValue2(rows[row], scriptBufferOffset);
                                FileTools.WriteToBuffer(ref scriptBuffer, ref scriptBufferOffset, new[] { (Int32)ExcelScript.ScriptOpCodes.Push, affixGroupWeight, 0 }.ToByteArray());

                                String affixGroupString = affixGroupsTable.ReadStringTable(affixGroupStringOffset);
                                int affixGroupStringIndex = affixGroupsList.IndexOf(affixGroupString);
                                if (affixGroupStringIndex == -1)
                                {
                                    affixGroupStringIndex = affixGroupsList.Count;
                                    affixGroupsList.Add(affixGroupString);
                                }

                                value = affixGroupStringIndex;
                            }
                        }
                        else
                        {
                            Debug.Assert(getTCv4Value1 != null);

                            value = getTCv4Value1(rowTCv4);
                        }


                        if (outputAttribute == null)
                        {
                            setValue1(rows[row], value);
                            continue;
                        }

                        if (outputAttribute.IsBitmask)
                        {
                            if (value.GetType().BaseType != typeof(Enum))
                            {
                                Debug.WriteLine("Error: IsBitmask is not of type Enum: " + fieldInfo.Name);
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
                            //int scriptOffset = (int)value;
                            //if (scriptOffset != 0)
                            //{
                            //    ExcelScript excelScriptTCv4 = new ExcelScript(fileManagerTCv4);

                            //    try
                            //    {
                            //        excelScriptTCv4.Decompile(excelFileTCv4.ScriptBuffer, scriptOffset, null, excelFileTCv4.StringId, row, col, fieldInfo.Name);
                            //    }
                            //    catch (Exception e)
                            //    {
                            //        Debug.WriteLine("TCv4 Decompile Error:\n" + e);
                            //        continue;
                            //    }

                            //    ExcelScript excelScriptCompiler = new ExcelScript(fileManagerTCv4, true, true);

                            //    try
                            //    {
                            //        excelScriptCompiler.Compile(excelScriptTCv4.ScriptString, null, excelFileTCv4.StringId, row, col, fieldInfo.Name);
                            //        value = scriptBufferOffset;
                            //    }
                            //    catch (Exceptions.ScriptUnknownFunctionException e)
                            //    {
                            //        if (isCharDisplay || isInventory)
                            //        {
                            //            value = 0;
                            //        }
                            //        else
                            //        {
                            //            Debug.WriteLine("SP Recompile Error: \n" + excelScriptTCv4.ScriptString + "\n" + e);
                            //            value = 0;
                            //            //continue;
                            //        }
                            //    }
                            //    catch (Exception e)
                            //    {
                            //        Debug.WriteLine("SP Recompile Error: \n" + excelScriptTCv4.ScriptString + "\n" + e);
                            //        value = 0;
                            //        //continue;
                            //    }

                            //    if ((int)value != 0)
                            //    {
                            //        FileTools.WriteToBuffer(ref scriptBuffer, ref scriptBufferOffset, excelScriptCompiler.ScriptCode.ToByteArray());
                            //    }
                            //}
                        }

                        setValue1(rows[row], value);
                    }

                    if (failed) break;
                }
                if (failed)
                {
                    Debug.WriteLine("Error: Excel conversion failed: " + excelFile.StringId);
                    continue;
                }

                // finish conversion process by "changing" the TCv4 type
                excelFileTCv4.ConvertType(excelFile, rows);
                if (scriptBufferOffset != 1)
                {
                    excelFileTCv4.SetScriptCode(scriptBuffer);
                }

                if (isAffixes)
                {
                    excelFileTCv4.SetSecondaryStringsCollection(affixGroupsList);
                }

                byte[] convertedBytes = excelFileTCv4.ToByteArray();
                String writePath = Path.Combine(Config.HglDir, excelFile.FilePath);
                String backupPath = writePath + ".bak";

                if (File.Exists(backupPath)) File.Delete(backupPath);
                File.Move(writePath, backupPath);
                File.WriteAllBytes(writePath, convertedBytes);

                int bp2 = 0;
            }

            int bp1 = 0;
        }

        /// <summary>
        /// Extracts the script call functions list from the client assembly function.
        /// </summary>
        public static void ExtractFunctionList()
        {
            //const String path = @"C:\SP_FunctionNamePtrGeneration.txt";
            const String path = @"C:\MP_FunctionNamePtrGeneration.txt";
            String[] functionCode = File.ReadAllLines(path);

            ExcelScript.ExtractFunctionList(functionCode);
        }

        /// <summary>
        /// This function checks every row/col of every excel file, determining if it needs to be visible/public or not.
        /// Any element non-zero for all rows, needs to be public (or add new OutputAttribute for "const" values?)
        /// </summary>
        public static void ExcelValuesDeepScan()
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
                    excelDelegator = new ObjectDelegator(fieldInfos);
                    objectDelegators.Add(excelFile.StringId, excelDelegator);
                }

                // check by column, by row
                int col = -1;
                foreach (FieldInfo fieldInfo in fieldInfos)
                {
                    if (++col == 0) continue; // row header

                    ObjectDelegator.FieldGetValueDelegate getValue = excelDelegator.GetFieldGetDelegate(fieldInfo.Name);
                    ExcelFile.OutputAttribute outputAttribute = ExcelFile.GetExcelAttribute(fieldInfo);

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
        public static void DoCookTest()
        {
            FileManager fileManager = new FileManager(Config.HglDir);
            fileManager.LoadTableFiles();
            fileManager.ExtractAllExcel();
            ExcelScript.EnableDebug(true);

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

                byte[] csvBytes = excelFile.ExportCSV(fileManager);
                File.WriteAllBytes(filePath.Replace(ExcelFile.Extension, ExcelFile.ExtensionDeserialised), csvBytes);
                ExcelFile excelFileCSV = new ExcelFile(fileEntry.RelativeFullPathWithoutPatch);
                excelFileCSV.ParseCSV(csvBytes, fileManager);

                //byte[] recookedBytes = excelFileCSV.ToByteArray();
                byte[] csvBytes2 = excelFileCSV.ExportCSV(fileManager);

                if (!csvBytes.SequenceEqual(csvBytes2))
                {
                    int b5p = 0;
                }

                //if (excelFile.StringId == "GLOBAL_STRING")
                //{
                //    ExcelFile temp = new ExcelFile(recookedBytes, fileEntry.RelativeFullPathWithoutPatch);
                //    int bp1 = 0;
                //}

                //File.WriteAllBytes(filePath, recookedBytes);
            }

            int bp = 0;
        }


        public static ObjectDelegator GetExcelDelegator(DataFile excelFile, IDictionary<String, ObjectDelegator> objectDelegators)
        {
            ObjectDelegator excelDelegator;

            if (!objectDelegators.TryGetValue(excelFile.StringId, out excelDelegator))
            {
                Type rowType = excelFile.Attributes.RowType;
                FieldInfo[] fieldInfos = rowType.GetFields();

                excelDelegator = new ObjectDelegator(fieldInfos);

                objectDelegators.Add(excelFile.StringId, excelDelegator);
            }

            return excelDelegator;
        }

        public static void LoadAllMLIFiles()
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

        public static void LoadAllRooms()
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

        public static void LoadAllLevelRules()
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

        public static void UncookAllXml()
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
    }
}