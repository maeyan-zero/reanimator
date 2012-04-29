using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml;
using Hellgate;
using Revival.Common;
using FieldDelegate = Revival.Common.ObjectDelegator.FieldDelegate;
using ExcelAttributes = Hellgate.ExcelFile.OutputAttribute;

namespace Reanimator
{
    public static class TestScripts
    {
        /// <summary>
        /// Function to test saving/importing/exported/etc relating to DataTables (i.e. actions from WITHIN Reanimator etc.)
        /// </summary>
        public static void TestDataTableExportAndImport(bool doTCv4 = false)
        {
            /*
            bytesXls						Bytes->Xls
            bytesXlsBytes					Bytes->Xls->Bytes
            bytesXlsCsv						Bytes->Xls->CSV
            bytesXlsCsvXls					Bytes->Xls->CSV->Xls
            bytesXlsCsvXlsBytes				Bytes->Xls->CSV->Xls->Bytes					== bytesXlsTableXlsBytes as single byte is stripped ->CSV

            bytesXlsTable					Bytes->Xls->Table
            bytesXlsTableXls				Bytes->Xls->Table->Xls
            bytesXlsTableXlsBytes			Bytes->Xls->Table->Xls->Bytes				!= bytesXlsBytes because of single script byte thingy lost to table
            bytesXlsTableXlsCsv				Bytes->Xls->Table->Xls->CSV
            bytesXlsTableXlsCsvXls			Bytes->Xls->Table->Xls->CSV->Xls
            bytesXlsTableXlsCsvXlsBytes		Bytes->Xls->Table->Xls->CSV->Xls->Bytes


            byte[] bytesXlsBytes				= bytesXls.ToByteArray();
            byte[] bytesXlsTableXlsCsvXlsBytes	= bytesXlsTableXlsCsvXls.ToByteArray();
             */

            String root = @"C:\excel_datatable_debug";
            FileManager.ClientVersions clientVersion = FileManager.ClientVersions.SinglePlayer;
            if (doTCv4)
            {
                root = Path.Combine(root, "tcv4");
                clientVersion = FileManager.ClientVersions.TestCenter;
            }
            root += @"\"; // lazy
            Directory.CreateDirectory(root);


            FileManager fileManager = new FileManager(Config.HglDir, clientVersion);
            fileManager.BeginAllDatReadAccess();
            fileManager.LoadTableFiles();

            List<String> sequenceFailed = new List<String>();
            List<String> sequenceChecked = new List<String>();
            foreach (KeyValuePair<String, DataFile> keyValuePair in fileManager.DataFiles)
            {
                String stringId = keyValuePair.Key;
                DataFile dataFile = keyValuePair.Value;
                if (dataFile.IsStringsFile) continue;

                //if (stringId != "WARDROBE_LAYER") continue;
                //if (stringId == "WARDROBE_LAYER")
                //{
                //    int bp = 0;
                //}


                Debug.Write("Checking " + stringId + "... ");
                ExcelFile bytesXls = (ExcelFile)dataFile;
                byte[] bytes = fileManager.GetFileBytes(bytesXls.FilePath, true);
                sequenceChecked.Add(stringId);
                sequenceFailed.Add(stringId);

                if (stringId == "PROPERTIES") continue; // can't do this atm - need to export Properties script table to DataTable etc.

                Debug.Write("DataTable... ");
                DataTable bytesXlsTable = fileManager.GetDataTable(stringId);
                if (bytesXlsTable == null)
                {
                    Debug.WriteLine("FAILED!");
                    continue;
                }

                // parse as DataTable
                Debug.Write("new Excel... ");
                DataFile bytesXlsTableXls = new ExcelFile(bytesXls.FilePath, fileManager.ClientVersion);
                if (!bytesXlsTableXls.ParseDataTable(bytesXlsTable, fileManager))
                {
                    Debug.WriteLine("FAILED!");
                    continue;
                }


                // check just data table import/export
                byte[] bytesXlsBytes = bytesXls.ToByteArray();
                byte[] bytesXlsTableXlsBytes = bytesXlsTableXls.ToByteArray();
                if (bytesXls.ScriptBuffer == null && // can't check this as ScriptBuffer[0] is NOT always 0x00 (even though it's unused and means no script normally when pointed to)
                    !bytesXls.HasStringBuffer) // as above - gets re-arranged etc (only consistant comparint CSV->XLS->DATA
                {
                    Debug.Write("bytesXlsBytes==bytesXlsTableXlsBytes... ");
                    if (!bytesXlsBytes.SequenceEqual(bytesXlsTableXlsBytes))
                    {
                        Debug.WriteLine("FALSE!");
                        File.WriteAllBytes(root + bytesXls.StringId + "0.bytes", bytes);
                        File.WriteAllBytes(root + bytesXls.StringId + "1.bytesXlsBytes", bytesXlsBytes);
                        File.WriteAllBytes(root + bytesXls.StringId + "2.bytesXlsTableXlsBytes", bytesXlsTableXlsBytes);
                        continue;
                    }
                }


                // export CSV
                Debug.Write("ExportCSV... ");
                byte[] bytesXlsTableXlsCsv = bytesXlsTableXls.ExportCSV(fileManager);
                if (bytesXlsTableXlsCsv == null)
                {
                    Debug.WriteLine("FAILED!");
                    continue;
                }

                Debug.Write("bytesXlsCsv==bytesXlsTableXlsCsv... ");
                byte[] bytesXlsCsv = bytesXls.ExportCSV(fileManager);
                if (!bytesXlsCsv.SequenceEqual(bytesXlsTableXlsCsv))
                {
                    Debug.WriteLine("FALSE!");
                    File.WriteAllBytes(root + bytesXls.StringId + "0.bytes", bytes);
                    File.WriteAllBytes(root + bytesXls.StringId + "1.bytesXlsBytes", bytesXlsBytes);
                    File.WriteAllBytes(root + bytesXls.StringId + "1a.bytesXlsCsv.csv", bytesXlsCsv);
                    File.WriteAllBytes(root + bytesXls.StringId + "2.bytesXlsTableXlsBytes", bytesXlsTableXlsBytes);
                    File.WriteAllBytes(root + bytesXls.StringId + "3.bytesXlsTableXlsCsv.csv", bytesXlsTableXlsCsv);
                    continue;
                }

                Debug.Write("bytesXlsCsvXlsBytes==bytesXlsTableXlsBytes... ");
                DataFile bytesXlsCsvXls = new ExcelFile(bytesXls.FilePath, fileManager.ClientVersion);
                bytesXlsCsvXls.ParseCSV(bytesXlsCsv, fileManager);
                byte[] bytesXlsCsvXlsBytes = bytesXlsCsvXls.ToByteArray();
                if (!bytesXlsCsvXlsBytes.SequenceEqual(bytesXlsTableXlsBytes) &&
                    stringId != "ITEMDISPLAY" && stringId != "LEVEL") // ITEMDISPLAY has weird single non-standard ASCII char that is lost (from using MS Work on their part lol)
                {                                                       // and LEVEL references blank TREASURE rows and hence rowId references differ on recooks
                    
                    Debug.WriteLine("FALSE!");
                    File.WriteAllBytes(root + bytesXls.StringId + "0.bytes", bytes);
                    File.WriteAllBytes(root + bytesXls.StringId + "1.bytesXlsBytes", bytesXlsBytes);
                    File.WriteAllBytes(root + bytesXls.StringId + "1a.bytesXlsCsv.csv", bytesXlsCsv);
                    File.WriteAllBytes(root + bytesXls.StringId + "1b.bytesXlsCsvXlsBytes", bytesXlsCsvXlsBytes);
                    File.WriteAllBytes(root + bytesXls.StringId + "2.bytesXlsTableXlsBytes", bytesXlsTableXlsBytes);
                    File.WriteAllBytes(root + bytesXls.StringId + "3.bytesXlsTableXlsCsv.csv", bytesXlsTableXlsCsv);
                    continue;
                }


                // import CSV
                Debug.Write("ParseCSV... ");
                DataFile bytesXlsTableXlsCsvXls = new ExcelFile(bytesXls.FilePath, fileManager.ClientVersion);
                if (!bytesXlsTableXlsCsvXls.ParseCSV(bytesXlsTableXlsCsv, fileManager))
                {
                    Debug.WriteLine("FAILED!");
                    continue;
                }


                // export imported as cooked and do byte compare
                Debug.Write("ToByteArray... ");
                byte[] bytesXlsTableXlsCsvXlsBytes = bytesXlsTableXlsCsvXls.ToByteArray();
                Debug.Write("bytesXlsCsvXlsBytes==bytesXlsTableXlsCsvXlsBytes... ");
                if (!bytesXlsCsvXlsBytes.SequenceEqual(bytesXlsTableXlsCsvXlsBytes))
                {
                    Debug.WriteLine("FALSE!");
                    File.WriteAllBytes(root + bytesXls.StringId + "0.bytes", bytes);
                    File.WriteAllBytes(root + bytesXls.StringId + "1.bytesXlsBytes", bytesXlsBytes);
                    File.WriteAllBytes(root + bytesXls.StringId + "2.bytesXlsTableXlsBytes", bytesXlsTableXlsBytes);
                    File.WriteAllBytes(root + bytesXls.StringId + "3.bytesXlsTableXlsCsv", bytesXlsTableXlsCsv);
                    File.WriteAllBytes(root + bytesXls.StringId + "4.bytesXlsTableXlsCsvXlsBytes", bytesXlsTableXlsCsvXlsBytes);
                    continue;
                }


                // yay
                Debug.WriteLine("OK!");
                sequenceFailed.RemoveAt(sequenceFailed.Count - 1);
            }

            Debug.WriteLine("Totals: {0} sequence checks, {1} fails. ({2}% failed!)", sequenceChecked.Count, sequenceFailed.Count, (float)sequenceFailed.Count*100f / sequenceChecked.Count);
            Debug.Write("Failed Tables: ");
            foreach (String stringIdFailed in sequenceFailed)
            {
                Debug.Write(stringIdFailed + ", ");
            }
        }

        public static void TestAllExcelScripts()
        {
            ExcelScript.GlobalDebug(true);
            FileManager fileManager = new FileManager(Config.HglDir);
            fileManager.BeginAllDatReadAccess();
            fileManager.LoadTableFiles();
            fileManager.EndAllDatAccess();

            StringWriter results = new StringWriter();
            Dictionary<String, int> excelScriptDecompileFails = new Dictionary<String, int>();
            Dictionary<String, int> excelScriptRecompileFails = new Dictionary<String, int>();
            Dictionary<String, int> excelScriptComparisonFails = new Dictionary<String, int>();
            int grandTotalScripts = 0;
            int grandTotalScriptsDecompiled = 0;
            int grandTotalScriptsFailedDecompilation = 0;
            int grandTotalScriptsRecompiled = 0;
            int grandTotalScriptsFailedRecompilation = 0;
            int grandTotalScriptsCompared = 0;
            int grandTotalScriptsFailedComparison = 0;

            foreach (ExcelFile excelFile in fileManager.DataFiles.Values.Where(dataFile => dataFile.IsExcelFile))
            {
                String excelStringId = String.Format("{0}...", excelFile.StringId);
                Debug.Write(excelStringId);
                results.Write(excelStringId);

                //if (excelFile.StringId != "SKILLS") continue;

                if (excelFile.Delegator == null || excelFile.ScriptCode == null || excelFile.ScriptCode.Length == 0)
                {
                    Debug.WriteLine(" No script data");
                    results.WriteLine(" No script data");
                    continue;
                }

                String scriptIntValues = String.Format(" {0} script int values...", excelFile.ScriptCode.Length);
                Debug.WriteLine(scriptIntValues);
                results.WriteLine(scriptIntValues);

                StringWriter excelFileResults = new StringWriter();
                int colIndex = -1;
                int totalScriptsFound = 0;
                int totalScriptsDecompiled = 0;
                int totalScriptsFailedDecompilation = 0;
                int totalScriptsRecompiled = 0;
                int totalScriptsFailedRecompilation = 0;
                int totalScriptsCompared = 0;
                int totalScriptsFailedComparison = 0;

                foreach (FieldDelegate fieldDelegate in excelFile.Delegator)
                {
                    colIndex++;
                    ExcelAttributes excelAtributes = (ExcelAttributes)fieldDelegate.Info.GetCustomAttributes(typeof (ExcelAttributes), true).FirstOrDefault();
                    if (excelAtributes == null || !excelAtributes.IsScript) continue;

                    String currentColumn = String.Format("\tColumn[{0}] = {1}", colIndex, fieldDelegate.Name);
                    Debug.WriteLine(currentColumn);
                    results.WriteLine(currentColumn);
                    excelFileResults.WriteLine(currentColumn);

                    int rowIndex = -1;
                    int prevOffset = 0;
                    int scriptCount = 0;
                    int scriptsDecompiled = 0;
                    int scriptsFailedDecompilation = 0;
                    int scriptsRecompiled = 0;
                    int scriptsFailedRecompilation = 0;
                    int scriptsCompared = 0;
                    int scriptsFailedComparison = 0;
                    foreach (Object row in excelFile.Rows)
                    {
                        rowIndex++;
                        int byteOffset = (int)fieldDelegate.GetValue(row);
                        if (byteOffset == 0) continue;
                        Debug.Assert(byteOffset > prevOffset);
                        prevOffset = byteOffset;

                        if (byteOffset == 9649 && excelFile.StringId == "SKILLS") // todo: not sure what's with this script...
                        {
                            continue;
                        }

                        excelFileResults.WriteLine(String.Format("\t\tColumn({0}) = {3}, Row({1}), ByteOffset({2}):", colIndex, rowIndex, byteOffset, fieldDelegate.Name));
                        ExcelScript excelScript = new ExcelScript(fileManager);

                        scriptCount++;
                        String script;
                        int[] scriptCodes = excelFile.ReadScriptTable(byteOffset);
                        String scriptCodesStr = scriptCodes.ToString(",");
                        String semiDecompiledScript = _ReadScriptTable(excelScript, scriptCodes);

                        //if (scriptCodesStr == "707,0,714,666,0,3,201,673,603979779,26,5,2,284,0,0")
                        //{
                        //    int bp = 0;
                        //}

                        // test decompiling
                        try
                        {
                            script = excelScript.Decompile(excelFile.ScriptBuffer, byteOffset, scriptCodesStr, excelFile.StringId, rowIndex, colIndex, fieldDelegate.Name);
                            scriptsDecompiled++;
                        }
                        catch (Exception e)
                        {
                            if (!excelScriptDecompileFails.ContainsKey(excelFile.StringId))
                            {
                                excelScriptDecompileFails.Add(excelFile.StringId, 1);
                            }
                            else
                            {
                                excelScriptDecompileFails[excelFile.StringId]++;
                            }
                            scriptsFailedDecompilation++;
                            excelFileResults.WriteLine("Script Decompile Failed:\n" + semiDecompiledScript + "\n" + e + "\n");
                            continue;
                        }

                        // test compiling of decompiled script
                        int[] recompiledScriptCode;
                        try
                        {
                            ExcelScript excelScriptCompile = new ExcelScript(fileManager);
                            recompiledScriptCode = excelScriptCompile.Compile(script, scriptCodesStr, excelFile.StringId, rowIndex, colIndex, fieldDelegate.Name);
                            scriptsRecompiled++;
                        }
                        catch (Exception e)
                        {
                            if (!excelScriptRecompileFails.ContainsKey(excelFile.StringId))
                            {
                                excelScriptRecompileFails.Add(excelFile.StringId, 1);
                            }
                            else
                            {
                                excelScriptRecompileFails[excelFile.StringId]++;
                            }
                            scriptsFailedRecompilation++;
                            excelFileResults.WriteLine("Script Recompile Failed:\n" + semiDecompiledScript + "\n" + e + "\n");
                            continue;
                        }

                        // check recompiled fidelity
                        scriptsCompared++;
                        if (!scriptCodes.SequenceEqual(recompiledScriptCode))
                        {
                            if (!excelScriptComparisonFails.ContainsKey(excelFile.StringId))
                            {
                                excelScriptComparisonFails.Add(excelFile.StringId, 1);
                            }
                            else
                            {
                                excelScriptComparisonFails[excelFile.StringId]++;
                            }
                            scriptsFailedComparison++;
                            String compareFailed = String.Format("Script Comparison Failed:\nOriginal Script: {0} = {1}\nRecompiled Script: {2}\nSemi-Decompiled:\n{3}", scriptCodesStr, script, recompiledScriptCode.ToString(","), semiDecompiledScript);
                            excelFileResults.WriteLine(compareFailed);
                            continue;
                        }

                        excelFileResults.WriteLine(script + "\n");
                    }

                    totalScriptsFound += scriptCount;
                    totalScriptsDecompiled += scriptsDecompiled;
                    totalScriptsFailedDecompilation += scriptsFailedDecompilation;
                    totalScriptsRecompiled += scriptsRecompiled;
                    totalScriptsFailedRecompilation += scriptsFailedRecompilation;
                    totalScriptsCompared += scriptsCompared;
                    totalScriptsFailedComparison += scriptsFailedComparison;

                    String columnStats = String.Format("\t\t{0} scripts found, {1} scripts decompiled, {2} scripts failed to decompile.\n", scriptCount, scriptsDecompiled, scriptsFailedDecompilation);
                    columnStats += String.Format("\t\t\t{0} scripts recompiled, {1} scripts failed to recompile, {2} scripts compared, {3} scripts failed comparison",
                                                 scriptsRecompiled, scriptsFailedRecompilation, scriptsCompared, scriptsFailedComparison);
                    Debug.WriteLine(columnStats);
                    excelFileResults.WriteLine(columnStats);
                }

                grandTotalScripts += totalScriptsFound;
                grandTotalScriptsDecompiled += totalScriptsDecompiled;
                grandTotalScriptsFailedDecompilation += totalScriptsFailedDecompilation;
                grandTotalScriptsRecompiled += totalScriptsRecompiled;
                grandTotalScriptsFailedRecompilation += totalScriptsFailedRecompilation;
                grandTotalScriptsCompared += totalScriptsCompared;
                grandTotalScriptsFailedComparison += totalScriptsFailedComparison;

                String totalStats = String.Format("Totals: {0} scripts found, {1} scripts decompiled, {2} scripts failed to decompile.\n", totalScriptsFound, totalScriptsDecompiled, totalScriptsFailedDecompilation);
                totalStats += String.Format("\t{0} scripts recompiled, {1} scripts failed to recompile, {2} scripts compared, {3} scripts failed comparison",
                                            totalScriptsRecompiled, totalScriptsFailedRecompilation, totalScriptsCompared, totalScriptsFailedComparison);
                Debug.WriteLine(totalStats);
                results.WriteLine(totalStats);
                excelFileResults.WriteLine(totalStats);

                File.WriteAllText(@"C:\TestScripts\excelScripts_" + excelFile.StringId + ".txt", excelFileResults.ToString());
                excelFileResults.Close();
            }

            foreach (KeyValuePair<String, int> keyValuePair in excelScriptDecompileFails)
            {
                String excelFileFails = String.Format("{0} had {1} failed decompilation.", keyValuePair.Key, keyValuePair.Value);
                Debug.WriteLine(excelFileFails);
                results.WriteLine(excelFileFails);
            }

            String grandTotalStats = String.Format("Grand Totals: {0} scripts found, {1} scripts decompiled, {2} scripts failed to decompile.\n", grandTotalScripts, grandTotalScriptsDecompiled, grandTotalScriptsFailedDecompilation);
            grandTotalStats += String.Format("\t{0} scripts recompiled, {1} scripts failed to recompile, {2} scripts compared, {3} scripts failed comparison",
                                        grandTotalScriptsRecompiled, grandTotalScriptsFailedRecompilation, grandTotalScriptsCompared, grandTotalScriptsFailedComparison);
            Debug.WriteLine(grandTotalStats);
            results.WriteLine(grandTotalStats);

            File.WriteAllText(@"C:\TestScripts\excelScripts_Results.txt", results.ToString());
        }

        private static String _ReadScriptTable(ExcelScript script, IList<int> scriptCodes)
        {
            String semiParsedCode = String.Empty;
            for (int i = 0; i < scriptCodes.Count; i++)
            {
                int offset = i * 4;
                ExcelScript.ScriptOpCodes opCode = (ExcelScript.ScriptOpCodes)scriptCodes[i];
                semiParsedCode += String.Format("{0}\t\t{1}\t\t{2}", offset, (int)opCode, opCode);

                if (opCode == ExcelScript.ScriptOpCodes.Call)
                {
                    int funcIndex = scriptCodes[++i];
                    semiParsedCode += String.Format("({0}) = {1}\n", funcIndex, script.CallFunctions[funcIndex]);
                }
                else if (opCode == ExcelScript.ScriptOpCodes.Push)
                {
                    semiParsedCode += String.Format(" {0}\n", scriptCodes[++i]);
                }
                else if (ExcelFile.ScriptOpCodeSizes.Case01.Contains((int)opCode))
                {
                    semiParsedCode += String.Format("({0}, {1})\n", scriptCodes[++i], scriptCodes[++i]);
                }
                else if (ExcelFile.ScriptOpCodeSizes.Case02.Contains((int)opCode) || ExcelFile.ScriptOpCodeSizes.BitField.Contains((int)opCode))
                {
                    semiParsedCode += String.Format("({0})\n", scriptCodes[++i]);
                }
                else if (ExcelFile.ScriptOpCodeSizes.Case03.Contains((int)opCode))
                {
                    semiParsedCode += "()\n";
                }
                else if (opCode == ExcelScript.ScriptOpCodes.Return)
                {
                    // nothing
                }
                else
                {
                    throw new NotImplementedException(opCode + " not implemented!");
                }
            }

            return semiParsedCode;
        }


        public static void CheckIdenticalFieldsToTCv4()
        {
            FileManager fileManager = new FileManager(Config.HglDir);
            fileManager.ExtractAllExcel();
            FileManager fileManagerTCv4 = new FileManager(Config.HglDir, FileManager.ClientVersions.TestCenter);

            fileManager.LoadTableFiles();
            fileManagerTCv4.LoadTableFiles(true);

            foreach (ExcelFile excelFile in fileManager.DataFiles.Values.Where(dataFile => dataFile.IsExcelFile))
            {
                Debug.WriteLine("Checking: " + excelFile.StringId);

                if (excelFile.StringId == "LANGUAGE" || excelFile.StringId == "REGION") continue; ;

                ObjectDelegator objectDelegator = fileManager.DataFileDelegators[excelFile.StringId];
                ObjectDelegator objectDelegatorTCv4 = fileManagerTCv4.DataFileDelegators["_TCv4_" + excelFile.StringId];

                foreach (ObjectDelegator.FieldDelegate fieldDelegate in objectDelegator)
                {
                    ObjectDelegator.FieldDelegate fieldDelegateTCv4 = objectDelegatorTCv4.GetFieldDelegate(fieldDelegate.Name);
                    if (fieldDelegateTCv4 == null)
                    {
                        if (fieldDelegate.Name == "blendOpAdventurer") continue; // field removed in TCv4 from WARDROBE_LAYER
                        if (fieldDelegate.Name == "unitVersionToGetSkillRespec") continue; // field removed in TCv4 from CHAR_DISPLAY
                        if (fieldDelegate.Name == "String") continue; // field removed in TCv4 from RECIPIES
                        if (fieldDelegate.Name == "unknown5") continue; // field removed in TCv4 from LEVEL
                        if (fieldDelegate.Name == "undefined11a") continue; // field removed in TCv4 from STATS

                        Debug.WriteLine(String.Format("Field '{0}' not found in TCv4 table.", fieldDelegate.Name));
                        continue;
                    }

                    // check excel attributes
                    ExcelFile.OutputAttribute outputAttribute = ExcelFile.GetExcelAttribute(fieldDelegate.Info);
                    ExcelFile.OutputAttribute outputAttributeTCv4 = ExcelFile.GetExcelAttribute(fieldDelegateTCv4.Info);
                    if (!Equals(outputAttribute, outputAttributeTCv4))
                    {
                        Debug.WriteLine(String.Format("Field '{0}' doesn't have matching excel attributes.", fieldDelegate.Name));
                    }

                    // check field types
                    if (fieldDelegate.FieldType.BaseType == typeof(Array))
                    {
                        if (fieldDelegateTCv4.FieldType.BaseType != typeof(Array))
                        {
                            Debug.WriteLine(String.Format("Field '{0}' is of type array, but TCv4 field is not.", fieldDelegate.Name));
                            continue;
                        }

                        MarshalAsAttribute arrayMarshal = (MarshalAsAttribute)fieldDelegate.Info.GetCustomAttributes(typeof(MarshalAsAttribute), false).First();
                        MarshalAsAttribute arrayMarshalTCv4 = (MarshalAsAttribute)fieldDelegateTCv4.Info.GetCustomAttributes(typeof(MarshalAsAttribute), false).First();

                        if (arrayMarshal.SizeConst == arrayMarshalTCv4.SizeConst) continue;

                        Debug.WriteLine(String.Format("Array fields '{0}' have lengths '{1}' != '{2}'", fieldDelegate.Name, arrayMarshal.SizeConst, arrayMarshalTCv4.SizeConst));
                    }

                    // check field accessor
                    if (fieldDelegate.IsPublic && !fieldDelegateTCv4.IsPublic)
                    {
                        Debug.WriteLine(String.Format("Field '{0}' is public, but not so in TCv4 class.", fieldDelegate.Name));
                    }
                    else if (fieldDelegate.IsPrivate && !fieldDelegateTCv4.IsPrivate)
                    {
                        Debug.WriteLine(String.Format("Field '{0}' is private, but not so in TCv4 class.", fieldDelegate.Name));
                    }

                    if (fieldDelegate.FieldType == fieldDelegateTCv4.FieldType || fieldDelegate.FieldType.BaseType == typeof(Enum)) continue;
                    Debug.WriteLine(String.Format("Field '{0}' of type '{1}' does not match TCv4 of type '{2}'", fieldDelegate.Name, fieldDelegate.FieldType, fieldDelegateTCv4.FieldType));
                }
            }
        }

        public static void RepackMPDats()
        {
            //String filePath1 = Config.HglDataDir + @"\data\hellgate000.idx";
            //IndexFile spHellgate4256 = new IndexFile(File.ReadAllBytes(filePath1));
            //spHellgate4256.FilePath = filePath1;

            String filePath1 = Config.HglDataDir + @"\data\mp_hellgate_1.10.180.3416_1.0.86.4580.idx";
            String filePath2 = Config.HglDataDir + @"\data\mp_hellgate_localized_1.10.180.3416_1.0.86.4580.idx";

            IndexFile mpHellgate4580 = new IndexFile(filePath1, File.ReadAllBytes(filePath1));
            IndexFile mpHellgateLocal4580 = new IndexFile(filePath2, File.ReadAllBytes(filePath2));

            IndexFile[] indexFiles = new[] { mpHellgate4580, mpHellgateLocal4580 };
            //IndexFile[] indexFiles = new[] { spHellgate4256 };

            foreach (IndexFile indexFile in indexFiles)
            {
                String filePath = indexFile.FilePath.Replace("mp_hellgate", "sp_hellgate");
                IndexFile newIndexFile = new IndexFile(filePath);
                newIndexFile.BeginDatWriting();

                indexFile.BeginDatReading();
                foreach (PackFileEntry fileEntry in indexFile.Files)
                {
                    byte[] fileBytes = indexFile.GetFileBytes(fileEntry);

                    newIndexFile.AddFile(fileEntry.Directory, fileEntry.Name, fileBytes);
                }
                indexFile.EndDatAccess();



                byte[] indexFileBytes = newIndexFile.ToByteArray();
                Crypt.Encrypt(indexFileBytes);
                File.WriteAllBytes(newIndexFile.FilePath, indexFileBytes);
                newIndexFile.EndDatAccess();
            }
        }

        public static void ExtractAllCSV()
        {
            const String root = @"C:\test_mod\";
            FileManager fileManager = new FileManager(Config.HglDir);
            fileManager.LoadTableFiles();

            foreach (DataFile dataFile in fileManager.DataFiles.Values)
            {
                if (!dataFile.IsExcelFile) continue;

                ExcelFile excelFile = (ExcelFile)dataFile;
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
            /*																0
0   byte[] fileBytes = fileManager.GetFileBytes(excelFile.FilePath, true);	Bytes													0-gen bytes

                                                                            0      1           2        3            4
    foreach (DataFile dataFile in fileManager.DataFiles.Values)				Bytes->Xls												1-gen Xls file
1   byte[] dataFileBytes = dataFile.ToByteArray();							Bytes->Xls->Bytes										1-gen bytes
    fromBytesExcel.ParseData(dataFileBytes);								Bytes->Xls->Bytes->Xls									2-gen Xls file
1a  byte[] bytesXlsBytesXlsBytes = fromBytesExcel.ToByteArray();			Bytes->Xls->Bytes->Xls->Bytes							2-gen bytes
1ba byte[] bytesXlsCsv = dataFile.ExportCSV(fileManager);					Bytes->Xls->CSV											1-gen CSV-bytes
    bytesXlsCsvXls.ParseCSV(bytesXlsCsv, fileManager);						Bytes->Xls->CSV  ->Xls									2-gen Xls file
1bb byte[] bytesXlsCsvXlsBytes = bytesXlsCsvXls.ToByteArray();				Bytes->Xls->CSV  ->Xls->Bytes							2-gen CSV-bytes
1b  byte[] csvBytes = fromBytesExcel.ExportCSV(fileManager);				Bytes->Xls->Bytes->Xls->CSV								2-gen CSV file
    csvExcel.ParseCSV(csvBytes, fileManager);								Bytes->Xls->Bytes->Xls->CSV->Xls						3-gen Xls file
1c  byte[] bytesXlsBytesXlsCsvXlsBytes = csvExcel.ToByteArray();			Bytes->Xls->Bytes->Xls->CSV->Xls->Bytes					3-gen bytes
    finalExcel.ParseData(recookedExcelBytes);								Bytes->Xls->Bytes->Xls->CSV->Xls->Bytes->Xls			4-gen Xls file
1d  byte[] csvCheck = finalExcel.ExportCSV(fileManager);					Bytes->Xls->Bytes->Xls->CSV->Xls->Bytes->Xls->CSV		4-gen CSV file
1e  byte[] finalCheck = finalExcel.ToByteArray();							Bytes->Xls->Bytes->Xls->CSV->Xls->Bytes->Xls->Bytes		4-gen bytes
             */

            String root = @"C:\excel_debug";
            FileManager.ClientVersions clientVersion = FileManager.ClientVersions.SinglePlayer;
            if (doTCv4)
            {
                root = Path.Combine(root, "tcv4");
                clientVersion = FileManager.ClientVersions.TestCenter;
            }
            root += @"\"; // lazy
            Directory.CreateDirectory(root);

            FileManager fileManager = new FileManager(Config.HglDir, clientVersion);
            fileManager.BeginAllDatReadAccess();
            fileManager.LoadTableFiles();
            ExcelFile.EnableDebug = true;

            int checkedCount = 0;
            List<String> excelFailed = new List<String>();
            foreach (DataFile bytesXls in fileManager.DataFiles.Values)
            {
                ExcelFile excelFile = bytesXls as ExcelFile;
                if (excelFile == null) continue;

                const String debugStr = "SKILLS";
                //if (excelFile.StringId != debugStr) continue;
                //if (excelFile.StringId == debugStr)
                //{
                //    int bp = 0;
                //}

                checkedCount++;
                Debug.Write(String.Format("Checking {0}... ", bytesXls.StringId));

                byte[] bytes = fileManager.GetFileBytes(excelFile.FilePath, true);
                try
                {
                    byte[] bytesXlsBytes = bytesXls.ToByteArray();
                    if (bytesXls.StringId == "SOUNDS" && false)
                    {
                        byte[] csvBytesSounds = excelFile.ExportCSV(fileManager);
                        ExcelFile soundsCSV = new ExcelFile(excelFile.FilePath);
                        soundsCSV.ParseCSV(csvBytesSounds, fileManager);
                        byte[] soundsBytes = soundsCSV.ToByteArray();
                        //byte[] soundsBytesFromCSV = soundsCSV.ExportCSV();
                        //ExcelFile soundsCSVFromBytesFromCSV = new ExcelFile(soundsBytesFromCSV, fileEntry.RelativeFullPathWithoutPatch);

                        // some brute force ftw
                        byte[][] bytesArrays = new[] { bytes, soundsBytes };
                        for (int z = 0; z < bytesArrays.Length; z++)
                        {
                            byte[] bytesBrute = bytesArrays[z];

                            int offset = 0x20;
                            int stringsBytesCount = FileTools.ByteArrayToInt32(bytesBrute, ref offset);

                            StringWriter stringWriterByteStrings = new StringWriter();
                            stringWriterByteStrings.WriteLine(stringsBytesCount + " bytes");
                            List<String> strings = new List<String>();
                            List<int> offsets = new List<int>();

                            while (offset < stringsBytesCount + 0x20)
                            {
                                String str = FileTools.ByteArrayToStringASCII(bytesBrute, offset);
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
                    if (bytes.Length != bytesXlsBytes.Length && !doTCv4) // some TCv4 tables don't have their sort columns yet
                    {
                        Debug.WriteLine("ToByteArray() dataFileBytes has differing length: " + bytesXls.StringId);
                        File.WriteAllBytes(root + bytesXls.StringId + "0.bytes", bytes);
                        File.WriteAllBytes(root + bytesXls.StringId + "1.bytesXlsBytes", bytesXlsBytes);
                        excelFailed.Add(bytesXls.StringId);
                        continue;
                    }

                    ExcelFile bytesXlsBytesXls = new ExcelFile(excelFile.FilePath, fileManager.ClientVersion);
                    bytesXlsBytesXls.ParseData(bytesXlsBytes);
                    Debug.Write("new ExcelFile... ");
                    if (!bytesXlsBytesXls.HasIntegrity)
                    {
                        Debug.WriteLine("fromBytesExcel = new Excel from ToByteArray() failed!");
                        File.WriteAllBytes(root + bytesXls.StringId + "0.bytes", bytes);
                        File.WriteAllBytes(root + bytesXls.StringId + "1.bytesXlsBytes", bytesXlsBytes);
                        excelFailed.Add(bytesXls.StringId);
                        continue;
                    }


                    // more checks
                    Debug.Write("ToByteArray->ToByteArray... ");
                    byte[] bytesXlsBytesXlsBytes = bytesXlsBytesXls.ToByteArray(); // bytesXlsBytesXlsBytes
                    if (bytes.Length != bytesXlsBytesXlsBytes.Length && !doTCv4) // some TCv4 tables don't have their sort columns yet
                    {
                        Debug.WriteLine("ToByteArray() dataFileBytesFromToByteArray has differing length!");
                        File.WriteAllBytes(root + bytesXls.StringId + "0.bytes", bytes);
                        File.WriteAllBytes(root + bytesXls.StringId + "1.bytesXlsBytes", bytesXlsBytes);
                        File.WriteAllBytes(root + bytesXls.StringId + "1a.toByteArrayFromByteArray", bytesXlsBytesXlsBytes);
                        excelFailed.Add(bytesXls.StringId);
                        continue;
                    }


                    // check generated sort index arrays);
                    Debug.Write("IndexSortArrays... ");
                    if (excelFile.IndexSortArray != null)
                    {
                        if (bytesXlsBytesXls.IndexSortArray == null || excelFile.IndexSortArray.Count != bytesXlsBytesXls.IndexSortArray.Count)
                        {
                            Debug.WriteLine("fromBytesExcel has not-matching IndexSortArray count!");
                            excelFailed.Add(bytesXls.StringId);
                            continue;
                        }

                        bool hasError = false;
                        for (int i = 0; i < excelFile.IndexSortArray.Count; i++)
                        {
                            if (excelFile.IndexSortArray[i].SequenceEqual(bytesXlsBytesXls.IndexSortArray[i])) continue;

                            Debug.WriteLine(String.Format("IndexSortArray[{0}] NOT EQUAL to original!", i));
                            hasError = true;
                        }

                        if (hasError)
                        {
                            File.WriteAllBytes(root + bytesXls.StringId + "0.bytes", bytes);
                            File.WriteAllBytes(root + bytesXls.StringId + "1.bytesXlsBytes", bytesXlsBytes);
                            File.WriteAllBytes(root + bytesXls.StringId + "1a.bytesXlsBytesXlsBytes", bytesXlsBytesXlsBytes);
                            excelFailed.Add(bytesXls.StringId);
                            continue;
                        }
                    }


                    // some csv stuff
                    Debug.Write("BaseXls->BaseCSV==");
                    byte[] bytesXlsCsv = bytesXls.ExportCSV(fileManager); // Bytes->Xls->CSV
                    Debug.Write("ExportCSV... ");
                    byte[] bytesXlsBytesXlsCsv = bytesXlsBytesXls.ExportCSV(fileManager);
                    if (!bytesXlsCsv.SequenceEqual(bytesXlsBytesXlsCsv))
                    {
                        Debug.WriteLine("FALSE!");
                        File.WriteAllBytes(root + bytesXls.StringId + "0.bytes", bytes);
                        File.WriteAllBytes(root + bytesXls.StringId + "1.bytesXlsBytes", bytesXlsBytes);
                        File.WriteAllBytes(root + bytesXls.StringId + "1a.bytesXlsBytesXlsBytes", bytesXlsBytesXlsBytes);
                        File.WriteAllBytes(root + bytesXls.StringId + "1b.bytesXlsBytesXlsCsv.csv", bytesXlsBytesXlsCsv);
                        File.WriteAllBytes(root + bytesXls.StringId + "1ba.bytesXlsCsv.csv", bytesXlsCsv);
                        excelFailed.Add(bytesXls.StringId);
                        continue;
                    }


                    ExcelFile bytesXlsCsvXls = new ExcelFile(excelFile.FilePath, fileManager.ClientVersion);
                    bytesXlsCsvXls.ParseCSV(bytesXlsCsv, fileManager);
                    byte[] bytesXlsCsvXlsBytes = bytesXlsCsvXls.ToByteArray(); // used later as well
                    if (excelFile.ScriptBuffer == null && // can only compare here for no-script tables - can't compare to pure-base xls-bytes as xls->csv->xls rearranges script code
                        !excelFile.HasStringBuffer) // xls->csv->xls also rearranges the strings buffer
                    {
                        Debug.Write("BytesXlsBytesXlsBytes==BytesXlsCsvXlsBytes... ");
                        if (!bytesXlsBytesXlsBytes.SequenceEqual(bytesXlsCsvXlsBytes))
                        {
                            Debug.WriteLine("FALSE!");
                            File.WriteAllBytes(root + bytesXls.StringId + "0.bytes", bytes);
                            File.WriteAllBytes(root + bytesXls.StringId + "1.bytesXlsBytes", bytesXlsBytes);
                            File.WriteAllBytes(root + bytesXls.StringId + "1a.bytesXlsBytesXlsBytes", bytesXlsBytesXlsBytes);
                            File.WriteAllBytes(root + bytesXls.StringId + "1b.bytesXlsBytesXlsCsv.csv", bytesXlsBytesXlsCsv);
                            File.WriteAllBytes(root + bytesXls.StringId + "1ba.bytesXlsCsv.csv", bytesXlsCsv);
                            File.WriteAllBytes(root + bytesXls.StringId + "1bb.bytesXlsCsvXlsBytes", bytesXlsCsvXlsBytes);
                            excelFailed.Add(bytesXls.StringId);
                            continue;
                        }
                    
                        Debug.Write("BytesXlsBytes==BytesXlsCsvXlsBytes... ");
                        if (!bytesXlsBytes.SequenceEqual(bytesXlsCsvXlsBytes))
                        {
                            Debug.WriteLine("FALSE!");
                            File.WriteAllBytes(root + bytesXls.StringId + "0.bytes", bytes);
                            File.WriteAllBytes(root + bytesXls.StringId + "1.bytesXlsBytes", bytesXlsBytes);
                            File.WriteAllBytes(root + bytesXls.StringId + "1a.bytesXlsBytesXlsBytes", bytesXlsBytesXlsBytes);
                            File.WriteAllBytes(root + bytesXls.StringId + "1b.bytesXlsBytesXlsCsv.csv", bytesXlsBytesXlsCsv);
                            File.WriteAllBytes(root + bytesXls.StringId + "1ba.bytesXlsCsv.csv", bytesXlsCsv);
                            File.WriteAllBytes(root + bytesXls.StringId + "1bb.bytesXlsCsvXlsBytes", bytesXlsCsvXlsBytes);
                            excelFailed.Add(bytesXls.StringId);
                            continue;
                        }
                    }


                    Debug.Write("BytesXlsBytesXlsCsv -> new ExcelFile");
                    ExcelFile bytesXlsBytesXlsCsvXls = new ExcelFile(excelFile.FilePath, fileManager.ClientVersion);
                    bytesXlsBytesXlsCsvXls.ParseCSV(bytesXlsBytesXlsCsv, fileManager);
                    Debug.Write("... ");
                    if (!bytesXlsBytesXlsCsvXls.HasIntegrity)
                    {
                        Debug.WriteLine("Failed!");
                        File.WriteAllBytes(root + bytesXls.StringId + "0.bytes", bytes);
                        File.WriteAllBytes(root + bytesXls.StringId + "1.bytesXlsBytes", bytesXlsBytes);
                        File.WriteAllBytes(root + bytesXls.StringId + "1a.bytesXlsBytesXlsBytes", bytesXlsBytesXlsBytes);
                        File.WriteAllBytes(root + bytesXls.StringId + "1b.bytesXlsBytesXlsCsv.csv", bytesXlsBytesXlsCsv);
                        File.WriteAllBytes(root + bytesXls.StringId + "1ba.bytesXlsCsv.csv", bytesXlsCsv);
                        excelFailed.Add(bytesXls.StringId);
                        continue;
                    }

                    byte[] bytesXlsBytesXlsCsvXlsBytes = bytesXlsBytesXlsCsvXls.ToByteArray();
                    Debug.Write("BytesXlsCsvXlsBytes==BytesXlsBytesXlsCsvXlsBytes... ");
                    if (!bytesXlsCsvXlsBytes.SequenceEqual(bytesXlsBytesXlsCsvXlsBytes))
                    {
                        Debug.WriteLine("FALSE!");
                        File.WriteAllBytes(root + bytesXls.StringId + "0.bytes", bytes);
                        File.WriteAllBytes(root + bytesXls.StringId + "1.bytesXlsBytes", bytesXlsBytes);
                        File.WriteAllBytes(root + bytesXls.StringId + "1a.bytesXlsBytesXlsBytes", bytesXlsBytesXlsBytes);
                        File.WriteAllBytes(root + bytesXls.StringId + "1b.bytesXlsBytesXlsCsv.csv", bytesXlsBytesXlsCsv);
                        File.WriteAllBytes(root + bytesXls.StringId + "1ba.bytesXlsCsv.csv", bytesXlsCsv);
                        File.WriteAllBytes(root + bytesXls.StringId + "1bb.bytesXlsCsvXlsBytes", bytesXlsCsvXlsBytes);
                        File.WriteAllBytes(root + bytesXls.StringId + "1c.bytesXlsBytesXlsCsvXlsBytes", bytesXlsBytesXlsCsvXlsBytes);
                        excelFailed.Add(bytesXls.StringId);
                        continue;
                    }

                    if (!fileManager.IsVersionTestCenter)
                    {
                        Debug.Write("StructureId... ");
                        UInt32 structureId = BitConverter.ToUInt32(bytes, 4);
                        UInt32 fromCSVStructureId = BitConverter.ToUInt32(bytesXlsBytesXlsCsvXlsBytes, 4);
                        if (structureId != fromCSVStructureId)
                        {
                            Debug.WriteLine("Structure Id value do not match: " + structureId + " != " + fromCSVStructureId);
                            excelFailed.Add(bytesXls.StringId);
                            continue;
                        }
                    }

                    Debug.Write("Almost Done... ");
                    ExcelFile bytesXlsBytesXlsCsvXlsBytesXls = new ExcelFile(excelFile.FilePath, fileManager.ClientVersion);
                    bytesXlsBytesXlsCsvXlsBytesXls.ParseData(bytesXlsBytesXlsCsvXlsBytes);
                    byte[] bytesXlsBytesXlsCsvXlsBytesXlsCsv = bytesXlsBytesXlsCsvXlsBytesXls.ExportCSV(fileManager);

                    int recookedLength = bytesXlsBytesXlsCsvXlsBytes.Length;
                    if (excelFile.StringId == "SKILLS") recookedLength += 12; // 12 bytes in int ptr data not used/referenced at all and are removed/lost in bytes -> csv -> bytes
                    if (bytes.Length != recookedLength && !doTCv4) // some TCv4 tables don't have their sort columns yet
                    {
                        Debug.WriteLine("Recooked Excel file has differing length!");
                        File.WriteAllBytes(root + bytesXls.StringId + "0.bytes", bytes);
                        File.WriteAllBytes(root + bytesXls.StringId + "1.bytesXlsBytes", bytesXlsBytes);
                        File.WriteAllBytes(root + bytesXls.StringId + "1a.bytesXlsBytesXlsBytes", bytesXlsBytesXlsBytes);
                        File.WriteAllBytes(root + bytesXls.StringId + "1b.bytesXlsBytesXlsCsv.csv", bytesXlsBytesXlsCsv);
                        File.WriteAllBytes(root + bytesXls.StringId + "1ba.bytesXlsCsv.csv", bytesXlsCsv);
                        File.WriteAllBytes(root + bytesXls.StringId + "1c.bytesXlsBytesXlsCsvXlsBytes", bytesXlsBytesXlsCsvXlsBytes);
                        File.WriteAllBytes(root + bytesXls.StringId + "1d.bytesXlsBytesXlsCsvXlsBytesXlsCsv.csv", bytesXlsBytesXlsCsvXlsBytesXlsCsv);
                        excelFailed.Add(bytesXls.StringId);
                        continue;
                    }

                    Debug.Assert(bytesXlsBytesXlsCsvXlsBytesXls.HasIntegrity);
                    byte[] bytesXlsBytesXlsCsvXlsBytesXlsBytes = bytesXlsBytesXlsCsvXlsBytesXls.ToByteArray();
                    if (excelFile.StringId == "SKILLS") Debug.Assert(bytesXlsBytesXlsCsvXlsBytesXlsBytes.Length + 12 == bytesXlsBytes.Length);
                    else Debug.Assert(bytesXlsBytesXlsCsvXlsBytesXlsBytes.Length == bytesXlsBytes.Length || doTCv4);
                    
                    if (!bytesXlsBytesXlsCsv.SequenceEqual(bytesXlsBytesXlsCsvXlsBytesXlsCsv))
                    {
                        Debug.WriteLine("csvBytes.SequenceEqual failed!");
                        File.WriteAllBytes(root + bytesXls.StringId + "0.bytes", bytes);
                        File.WriteAllBytes(root + bytesXls.StringId + "1.bytesXlsBytes", bytesXlsBytes);
                        File.WriteAllBytes(root + bytesXls.StringId + "1a.bytesXlsBytesXlsBytes", bytesXlsBytesXlsBytes);
                        File.WriteAllBytes(root + bytesXls.StringId + "1b.bytesXlsBytesXlsCsv.csv", bytesXlsBytesXlsCsv);
                        File.WriteAllBytes(root + bytesXls.StringId + "1ba.bytesXlsCsv.csv", bytesXlsCsv);
                        File.WriteAllBytes(root + bytesXls.StringId + "1c.bytesXlsBytesXlsCsvXlsBytes", bytesXlsBytesXlsCsvXlsBytes);
                        File.WriteAllBytes(root + bytesXls.StringId + "1d.bytesXlsBytesXlsCsvXlsBytesXlsCsv.csv", bytesXlsBytesXlsCsvXlsBytesXlsCsv);
                        File.WriteAllBytes(root + bytesXls.StringId + "1e.bytesXlsBytesXlsCsvXlsBytesXlsBytes", bytesXlsBytesXlsCsvXlsBytesXlsBytes);
                        excelFailed.Add(bytesXls.StringId);
                        continue;
                    }

                    Debug.Write("\nPerforming deep scan: ");
                    ObjectDelegator objectDelegator = fileManager.DataFileDelegators[excelFile.StringId];
                    int lastPercent = 0;
                    int col = 0;
                    bool failed = false;
                    foreach (FieldDelegate fieldDelegate in objectDelegator)
                    {
                        int percent = col * 100 / objectDelegator.FieldCount - 1;
                        int dotCount = percent - lastPercent;
                        for (int i = 0; i < dotCount; i++) Debug.Write(".");

                        lastPercent = percent;

                        ExcelFile.OutputAttribute excelAttribute = ExcelFile.GetExcelAttribute(fieldDelegate.Info);
                        bool isArray = (fieldDelegate.FieldType.BaseType == typeof(Array));

                        for (int row = 0; row < excelFile.Rows.Count; row++)
                        {
                            Object obj1 = fieldDelegate.GetValue(excelFile.Rows[row]);
                            Object obj2 = fieldDelegate.GetValue(bytesXlsBytesXlsCsvXlsBytesXls.Rows[row]);

                            if (isArray)
                            {
                                //if (excelFile.StringId == "TREASURE")
                                //{
                                //    int bp = 0;
                                //}
                                IEnumerator arrayEnumator1 = ((Array)obj1).GetEnumerator();
                                IEnumerator arrayEnumator2 = ((Array)obj2).GetEnumerator();
                                int index = -1;
                                while (arrayEnumator1.MoveNext() && arrayEnumator2.MoveNext())
                                {
                                    index++;
                                    Object elementVal1 = arrayEnumator1.Current;
                                    Object elementVal2 = arrayEnumator2.Current;

                                    if (elementVal1.Equals(elementVal2)) continue;

                                    Debug.WriteLine(String.Format("Array Element '{0}' != '{1}' on col({2}) = '{3}', row({4}), index({5})", elementVal1, elementVal2, col, fieldDelegate.Name, row, index));
                                    failed = true;
                                    break;
                                }
                                if (failed) break;

                                continue;
                            }

                            if (obj1.Equals(obj2)) continue;

                            if (excelAttribute != null)
                            {
                                if (excelAttribute.IsStringOffset)
                                {
                                    int offset1 = (int)obj1;
                                    int offset2 = (int)obj2;

                                    String str1 = excelFile.ReadStringTable(offset1);
                                    String str2 = bytesXlsBytesXlsCsvXlsBytesXls.ReadStringTable(offset2);

                                    if (str1 == str2) continue;

                                    obj1 = str1;
                                    obj2 = str2;
                                }

                                if (excelAttribute.IsScript)
                                {
                                    int offset1 = (int)obj1;
                                    int offset2 = (int)obj2;

                                    Int32[] script1 = excelFile.ReadScriptTable(offset1);
                                    Int32[] script2 = bytesXlsBytesXlsCsvXlsBytesXls.ReadScriptTable(offset2);

                                    if (script1.SequenceEqual(script2)) continue;

                                    obj1 = script1.ToString(",");
                                    obj2 = script2.ToString(",");
                                }
                            }

                            String str = obj1 as String;
                            if ((str != null && str.StartsWith("+N%-N% base damage as [elem]")) ||  // "+N%-N% base damage as [elem]…" != "+N%-N% base damage as [elem]?" on col(2) = 'exampleDescription', row(132) // the '?' at end
                                fieldDelegate.Name == "bonusStartingTreasure")                      // refernces multiple different blank rows
                                continue;

                            Debug.WriteLine(String.Format("'{0}' != '{1}' on col({2}) = '{3}', row({4})", obj1, obj2, col, fieldDelegate.Name, row));
                            failed = true;
                            break;
                        }
                        if (failed) break;

                        col++;
                    }

                    if (failed)
                    {
                        File.WriteAllBytes(root + bytesXls.StringId + ".bytesXlsBytesXlsCsv.csv", bytesXlsBytesXlsCsv);
                        File.WriteAllBytes(root + bytesXls.StringId + ".bytesXlsBytesXlsCsvXlsBytesXlsCsv.csv", bytesXlsBytesXlsCsvXlsBytesXlsCsv);
                        excelFailed.Add(bytesXls.StringId);
                        continue;
                    }

                    Debug.WriteLine("OK\n");
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Excel file Exception: " + bytesXls.StringId + "\n" + e);
                }
            }

            Debug.WriteLine(checkedCount + " excel files checked.");

            if (excelFailed.Count <= 0) return;
            Debug.WriteLine(excelFailed.Count + " failed excel checks! (" + ((float)excelFailed.Count / checkedCount * 100) + "%)");
            foreach (String str in excelFailed) Debug.Write(str + ", ");
        }

        /// <summary>
        /// Function to conver the TCv4 excel files to SP client formats.
        /// </summary>
        public static void ConvertTCv4ExcelToSP()
        {
            // init file manager and load excel files
            FileManager fileManager = new FileManager(Config.HglDir);
            fileManager.ExtractAllExcel();
            FileManager fileManagerTCv4 = new FileManager(Config.HglDir, FileManager.ClientVersions.TestCenter);

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
            //int converted = -1;
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
                //bool isCharDisplay = false;
                //bool isInventory = false;
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

                    //case "CHARDISPLAY":
                    //    isCharDisplay = true;
                    //    break;

                    //case "INVENTORY":
                    //    isInventory = true;
                    //    break;
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
                List<String> affixGroupsList = null;
                List<Int32> affixGroupWeightScripts = null;
                if (isAffixes)
                {
                    affixGroupsTable = fileManagerTCv4.GetDataFile("_TCv4_AFFIX_GROUPS") as ExcelFile;
                    affixGroupsList = new List<String>();
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

                //int bp2 = 0;
            }

            //int bp1 = 0;
        }

        /// <summary>
        /// Extracts the script call functions list from the client assembly function.
        /// </summary>
        public static void ExtractFunctionList()
        {
            //const String path = @"C:\SP_FunctionNamePtrGeneration.txt";
            //const String path = @"C:\MP_FunctionNamePtrGeneration.txt";
            //const String path = @"C:\FunctionNamePtrGeneration.txt";
            //const String path = @"C:\test.txt";
            const String path = @"C:\FunctionNamePtrGeneration_resurrection.2011.07.06.txt";
            String[] functionCode = File.ReadAllLines(path);

            ExcelScript.ExtractFunctionListCStyle(functionCode);
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
            foreach (PackFileEntry fileEntry in fileManager.FileEntries.Values)
            {
                if (!fileEntry.Name.EndsWith(ExcelFile.Extension)) continue;

                byte[] fileBytes = fileManager.GetFileBytes(fileEntry, true);
                Debug.Assert(fileBytes != null);

                ExcelFile excelFile = new ExcelFile(fileBytes, fileEntry.Path);
                if (excelFile.Attributes.IsEmpty) continue;

                Debug.WriteLine("Checking file: " + fileEntry.Path);

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

                //int bp3 = 0;
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

                String[] stringIds1 = (from dataTableAttribute in DataFile.DataFileMap
                                       where dataTableAttribute.Value.StructureId == forStructureId
                                       select dataTableAttribute.Key).ToArray();
                String[] stringIds2 = (from dataTableAttribute in DataFile.DataFileMapTestCenter
                                       where dataTableAttribute.Value.StructureId == forStructureId
                                       select dataTableAttribute.Key).ToArray();
                String[] stringIds3 = (from dataTableAttribute in DataFile.DataFileMapResurrection
                                       where dataTableAttribute.Value.StructureId == forStructureId
                                       select dataTableAttribute.Key).ToArray();
                String[] stringIds4 = (from dataTableAttribute in DataFile.DataFileMapMod
                                       where dataTableAttribute.Value.StructureId == forStructureId
                                       select dataTableAttribute.Key).ToArray();
                String stringIdPrepend = String.Join(",", stringIds1) + String.Join(",", stringIds2) + String.Join(",", stringIds3) + String.Join(",", stringIds4);

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
            ExcelScript.GlobalDebug(true);

            foreach (PackFileEntry fileEntry in fileManager.FileEntries.Values)
            {
                if (!fileEntry.Name.EndsWith(ExcelFile.Extension)) continue;

                byte[] fileBytes = fileManager.GetFileBytes(fileEntry, true);
                Debug.Assert(fileBytes != null);
                String filePath = Path.Combine(Config.HglDir, fileEntry.Path);

                ExcelFile excelFile = new ExcelFile(fileBytes, fileEntry.Path);
                if (excelFile.Attributes.IsEmpty) continue;

                Console.WriteLine("Cooking file: " + fileEntry.Path);

                //if (!fileEntry.RelativeFullPathWithoutPatch.Contains("display_item")) continue;

                byte[] csvBytes = excelFile.ExportCSV(fileManager);
                File.WriteAllBytes(filePath.Replace(ExcelFile.Extension, ExcelFile.ExtensionDeserialised), csvBytes);
                ExcelFile excelFileCSV = new ExcelFile(fileEntry.Path);
                excelFileCSV.ParseCSV(csvBytes, fileManager);

                //byte[] recookedBytes = excelFileCSV.ToByteArray();
                byte[] csvBytes2 = excelFileCSV.ExportCSV(fileManager);

                //if (!csvBytes.SequenceEqual(csvBytes2))
                //{
                //    int b5p = 0;
                //}

                //if (excelFile.StringId == "GLOBAL_STRING")
                //{
                //    ExcelFile temp = new ExcelFile(recookedBytes, fileEntry.RelativeFullPathWithoutPatch);
                //    int bp1 = 0;
                //}

                //File.WriteAllBytes(filePath, recookedBytes);
            }

            //int bp = 0;
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
                RoomDefinitionFile roomDefinitionFile = new RoomDefinitionFile(path, null, null);

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
                    RoomDefinitionFile roomDefinitionFile2 = new RoomDefinitionFile(xmlPath);
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
                LevelRulesFile levelRulesFile = new LevelRulesFile(path, null);

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
                    LevelRulesFile levelRulesFile2 = new LevelRulesFile(xmlPath, null);
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

        public static void TestAllXml()
        {
            //System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            //const String root = @"D:\Games\Hellgate\Data\";
            //const String root = @"D:\Games\Hellgate London\data\mp_hellgate_1.10.180.3416_1.0.86.4580\";
            //const String root = @"D:\Games\Hellgate London\data\mp_hellgate_1.10.180.3416_1.0.86.4580\data\background\";
            //List<String> xmlFiles = new List<String>(Directory.GetFiles(root, "*.xml.cooked", SearchOption.AllDirectories));

            FileManager fileManager = new FileManager(Config.HglDir);
            fileManager.BeginAllDatReadAccess();
            fileManager.LoadTableFiles();
            //fileManager.EndAllDatAccess();
            //XmlCookedFile.Initialize(fileManager);

            String debugPath = @"C:\xml_debug\";
            Directory.CreateDirectory(debugPath);
            int count = 0;
            List<XmlCookedFile> excelStringWarnings = new List<XmlCookedFile>();
            List<String> testCentreWarnings = new List<String>();
            List<String> resurrectionWarnings = new List<String>();
            int i = 0;
            foreach (PackFileEntry fileEntry in fileManager.FileEntries.Values)
            {
                if (!fileEntry.Path.EndsWith(XmlCookedFile.Extension)) continue;

                String xmlFilePath = fileEntry.Path;

                //const String debugStr = "shield09_appearance";
                //if (!xmlFilePath.Contains(debugStr)) continue;
                //if (xmlFilePath.Contains(debugStr))
                //{
                //    int bp = 0;
                //}
                

            //foreach (String xmlFilePath in xmlFiles)
            //{
                //bool skip = ((i++ % 23) > 0);
                //if (skip)
                //{
                //    if (xmlFilePath.Contains("\\Data\\colorsets.xml")) continue;
                //    if (xmlFilePath.Contains("\\Data\\ai\\")) continue;
                //    if (xmlFilePath.Contains("\\Data\\background\\")) continue;
                //    if (xmlFilePath.Contains("\\Data\\demolevel\\")) continue;
                //    if (xmlFilePath.Contains("\\Data\\lights\\")) continue;
                //    if (xmlFilePath.Contains("\\Data\\materials\\")) continue;
                //    if (xmlFilePath.Contains("\\Data\\particles\\")) continue;
                //    if (xmlFilePath.Contains("\\Data\\screenfx\\")) continue;
                //    if (xmlFilePath.Contains("\\Data\\skills\\")) continue;
                //    if (xmlFilePath.Contains("\\Data\\sounds\\")) continue;
                //    if (xmlFilePath.Contains("\\Data\\states\\")) continue;
                //    if (xmlFilePath.Contains("\\Data\\units\\items\\")) continue;
                //    if (xmlFilePath.Contains("\\Data\\units\\missiles\\")) continue;
                //    if (xmlFilePath.Contains("\\Data\\units\\monsters\\")) continue;
                //    if (xmlFilePath.Contains("\\Data\\units\\npc\\")) continue;
                //    if (xmlFilePath.Contains("\\Data\\units\\objects\\")) continue;
                //}

                String path = xmlFilePath;
                String fileName = Path.GetFileName(path);
                Debug.Assert(!String.IsNullOrEmpty(fileName));
                //path = @"D:\Games\Hellgate London\data\mp_hellgate_1.10.180.3416_1.0.86.4580\data\background\city\treasury\cap_path.xml.cooked";
                //path = "D:\\Games\\Hellgate\\Data\\background\\_environments\\outdoor_redhellcow_env.xml.cooked";
                //path = "D:\\Games\\Hellgate\\Data\\particles\\background\\t_background\\tokyo_dark_smoke_c_large_01.xml.cooked";
                //if (path == @"D:\Games\Hellgate\Data\background\cans and boxes.xml.cooked")
                //{
                //    int bp = 0;
                //}

                XmlCookedFile xmlCookedFile = new XmlCookedFile(fileManager, fileName);
                byte[] data = fileManager.GetFileBytes(fileEntry);
                Debug.Assert(data != null && data.Length > 0);

                //Console.WriteLine("Uncooking: " + fileName);

                try
                {
                    xmlCookedFile.ParseFileBytes(data, true);
                }
                catch (Exception e)
                {
                    File.WriteAllBytes(debugPath + fileName, data);
                    Console.WriteLine("Failed to uncooked file \"" + path + "\"\n" + e + "\n");
                    continue;
                }

                byte[] newXmlBytes = xmlCookedFile.ExportAsDocument();
                count++;

                if (xmlCookedFile.HasExcelStringsMissing) excelStringWarnings.Add(xmlCookedFile);
                if (xmlCookedFile.HasTestCentreElements) testCentreWarnings.Add(Path.GetFileName(fileName));
                if (xmlCookedFile.HasResurrectionElements) resurrectionWarnings.Add(Path.GetFileName(fileName));
                if (xmlCookedFile.HasExcelStringsMissing || xmlCookedFile.HasTestCentreElements || xmlCookedFile.HasResurrectionElements) continue;

                XmlCookedFile recookedXmlFile = new XmlCookedFile(fileManager, fileName);// { CookExcludeResurrection = false };
                byte[] newXmlCookedBytes = recookedXmlFile.ParseFileBytes(newXmlBytes);

                // check if *cooking method* is working. i.e. is the cook from the *new* XML format == the original cooked
                bool identicalNew = data.SequenceEqual(newXmlCookedBytes);

                // if file passes byte-byte test, then continue
                if (identicalNew)
                {
                    Console.WriteLine("{0} passed checks...", path);
                    continue;
                }

                if (
                path.Contains("female_3p_appearance.xml") ||            // this file has some weird bytes in a string element
                path.Contains("focus_item12_mesh_appearance.xml") ||    // this file has non-zeroed flag base masks (all differing)
                path.Contains("focus_item10_mesh_appearance.xml") ||    // as above     // (all 3 probably from not zeroing a ptr at original cooking)
                path.Contains("dof_test.xml") ||                        // as above
                path.Contains("motionblur.xml") ||                      // as above
                    path.Contains("player dead.xml")
                 )
                {
                    Console.WriteLine("{0} passed checks...", path);
                    continue;
                }

                File.WriteAllBytes(debugPath + fileName, data);
                File.WriteAllBytes(debugPath + fileName + "recooked", newXmlCookedBytes);
                File.WriteAllBytes(debugPath + fileName.Replace(".cooked", ""), newXmlBytes);

                Console.WriteLine("{0} failed!", path);
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
            if (testCentreWarnings.Count > 0)
            {
                Console.WriteLine("Warning: " + testCentreWarnings.Count + " files had TestCentre-specific elements:");
                foreach (String str in testCentreWarnings) Console.WriteLine("\t" + str);
            }
            if (resurrectionWarnings.Count > 0)
            {
                Console.WriteLine("Warning: " + resurrectionWarnings.Count + " files had Resurrection-specific elements:");
                foreach (String str in resurrectionWarnings) Console.WriteLine("\t" + str);
            }
            textWriter.Close();
            Console.SetOut(consoleOut);
        }
    }
}