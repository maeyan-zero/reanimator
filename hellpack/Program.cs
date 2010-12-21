using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using Hellgate;
using ExceptionLogger = Revival.Common.ExceptionLogger;

namespace Revival
{
    public static class Hellpack
    {
        static string _defaultDat = "sp_hellgate_1337";
        const string SearchPattern = "*{0}";
        const string WelcomeMsg = "Hellpack - the Hellgate London compiler.\nWritten by the Revival Team, 2010\nhttp://www.hellgateaus.net";
        const string NoPathsMsg = "Sorry, no data paths were found. Check error.xml for details.";
        const string BadSyntaxMsg = "Incorrect argument given: {0}";
        const string UsageMsg = "Usage: todo";
        const string HellgateMissingMsg = "Warning: Can not cook XML, Hellgate London directory missing.";


        static void Main(string[] args)
        {
            FileManager fileManager;
            string hellgatePath = Config.HglDir;
            string currentDir = Directory.GetCurrentDirectory();
            string dataDir = Path.Combine(currentDir, Hellgate.Common.DataPath);
            string dataCommonDir = Path.Combine(currentDir, Hellgate.Common.DataCommonPath);

            bool doCookTxt = false;
            bool doCookXml = false;
            bool doPackDat = false;
            bool doSearchCd = false;
            bool doExcludeRaw = false;
            bool doLevelRules = false;
            bool doRoomDefinitions = false;

            List<string> filesToPack = new List<string>();
            List<string> excelFilesToCook = new List<string>();
            List<string> stringFilesToCook = new List<string>();
            List<string> xmlFilesToCook = new List<string>();
            List<string> levelRulesFilesToSerialize = new List<string>();
            List<string> roomDefinitionFilesSerialize = new List<string>();

            #region alexs_stuff
            if (false)
            {

                fileManager = new FileManager(@"D:\Games\Hellgate London");
                fileManager.LoadTableFiles();
                

                byte[] buffer = fileManager.DataFiles["SOUNDS"].ExportCSV();
                return;
                //File.WriteAllBytes(@"D:\levels_rules.txt", buffer);

                //foreach (DataFile dataFile in fileManager.DataFiles.Values)
                //{
                //    if (dataFile.IsStringsFile) continue;

                //    Console.WriteLine(dataFile.FileName);
                //    String dir = Path.GetDirectoryName(dataFile.FilePath);
                //    if (Directory.Exists(dir) == false) Directory.CreateDirectory(dir);

                //    byte[] ebuffer = dataFile.ExportCSV();
                //    //for (int i = 0; i < 10; i++)
                //    //{
                //    //    Stopwatch stopwatch = new Stopwatch();
                //    //    stopwatch.Start();
                //    //    ebuffer = dataFile.ExportCSV();
                //    //    stopwatch.Stop();
                //    //    Console.WriteLine("Elapsed: {0}", stopwatch.Elapsed);
                //    //}
                    
                //    File.WriteAllBytes(dataFile.FilePath.Replace(".cooked", ""), ebuffer);
                //}


                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                const String pathExcel = @"data\excel\items.txt";
                byte[] excelBytes = File.ReadAllBytes(pathExcel);
                ExcelFile excelFile = new ExcelFile(excelBytes, pathExcel);
                byte[] excelCsvBytes = excelFile.ToByteArray();
                stopwatch.Stop();
                Console.WriteLine("Elapsed: {0}", stopwatch.Elapsed);
                

                //const String path = @"data\excel\strings\english\strings_revival.xls.uni2";
                //byte[] cookedBytes = File.ReadAllBytes(path);
                //StringsFile stringsFile = new StringsFile(cookedBytes, path);
                //byte[] csvBytes = stringsFile.ExportCSV();
                //if (!cookedBytes.SequenceEqual(csvBytes))
                //{
                //    File.WriteAllBytes(path + "2", csvBytes);
                //    int bp = 0;
                //}
                

                //doSearchCd = true;
                //doCookTxt = true;
                //if (doSearchCd && doCookTxt)
                //{
                //    excelFilesToCook.AddRange(SearchForExcelFiles(currentDir));
                //    stringFilesToCook.AddRange(SearchForStringFiles(currentDir));
                //}

                //if (doCookTxt)
                //{
                //    CookExcelFiles(excelFilesToCook.ToArray());
                //    CookStringFiles(stringFilesToCook.ToArray());
                //}

                return;
            }
            #endregion

            Console.WriteLine(WelcomeMsg);
            Console.WriteLine(String.Empty);

            if (args.Length == 0)
            {
                // No arguments defined - this is the default program
                doCookTxt = true;
                doCookXml = true;
                doPackDat = true;
                doSearchCd = true;
                doExcludeRaw = true;
                doLevelRules = true;
                doRoomDefinitions = true;
            }
            else
            {
                foreach (string arg in args)
                {
                    switch (arg)
                    {
                        case "/t":
                            doCookTxt = true;
                            break;
                        case "/x":
                            doCookXml = true;
                            break;
                        case "/p":
                            doPackDat = true;
                            break;
                        case "/s":
                            doSearchCd = true;
                            break;
                        case "/e":
                            doExcludeRaw = true;
                            break;
                        case "/lr":
                            doLevelRules = true;
                            break;
                        case "/rd":
                            doRoomDefinitions = true;
                            break;
                        default:
                            if (arg.StartsWith("/p:"))
                            {
                                _defaultDat = arg.Replace("/p:", "");
                                // Trim in case someone has appended the extention
                                if (_defaultDat.EndsWith(".idx"))
                                    _defaultDat = _defaultDat.Replace(".idx", "");
                                if (_defaultDat.EndsWith(".dat"))
                                    _defaultDat = _defaultDat.Replace(".dat", "");
                                break;
                            }
                            if (arg.StartsWith("/h:"))
                            {
                                hellgatePath = arg.Replace("/h:", "");
                                break;
                            }
                            if (arg.EndsWith(ExcelFile.ExtensionDeserialised))
                            {
                                excelFilesToCook.Add(arg);
                                doCookTxt = true;
                                break;
                            }
                            if (arg.EndsWith(StringsFile.FileExtentionClean))
                            {
                                stringFilesToCook.Add(arg);
                                doCookTxt = true;
                                break;
                            }
                            if (arg.EndsWith(LevelRulesFile.ExtensionDeserialised))
                            {
                                levelRulesFilesToSerialize.Add(arg);
                                doLevelRules = true;
                                break;
                            }
                            if (arg.EndsWith(RoomDefinitionFile.ExtensionDeserialised))
                            {
                                roomDefinitionFilesSerialize.Add(arg);
                                doRoomDefinitions = true;
                                break;
                            }
                            if (arg.EndsWith(XmlCookedFile.ExtensionDeserialised))
                            {
                                xmlFilesToCook.Add(arg);
                                doCookXml = true;
                                break;
                            }
                            else
                            {
                                Console.WriteLine(String.Format(BadSyntaxMsg, arg));
                                break;
                            }
                    }
                }
            }

            // Search for Txt files to cook
            if (doSearchCd && doCookTxt)
            {
                excelFilesToCook.AddRange(SearchForExcelFiles(currentDir));
                stringFilesToCook.AddRange(SearchForStringFiles(currentDir));
            }

            // Search for Xml files to cook
            if (doSearchCd && doCookXml)
            {
                xmlFilesToCook.AddRange(SearchForXmlFiles(currentDir));
            }

            // Search for .drl Level Rules files to cook
            if (doSearchCd && doLevelRules)
            {
                // todo
            }

            // Search for .rom Level Rules files to cook
            if (doSearchCd && doRoomDefinitions)
            {
                // todo
            }

            // Cook Txt files
            if (doCookTxt)
            {
                CookExcelFiles(excelFilesToCook.ToArray());
                CookStringFiles(stringFilesToCook.ToArray());
            }

            // Cook Xml files
            if (doCookXml)
            {
                // ensure we have the correct hellgate installation path
                if (Directory.Exists(hellgatePath) == true)
                {
                    fileManager = new FileManager(hellgatePath);
                    if (fileManager.HasIntegrity == false)
                    {
                        Console.WriteLine("Warning: XML could not be cooked - fileManager.Integrity = false");
                    }
                    else
                    {
                        CookXmlFiles(xmlFilesToCook.ToArray(), fileManager);
                    }
                }
                else
                {
                    Console.WriteLine(HellgateMissingMsg);
                }
            }

            // cook .drl Level Rules files
            if (doLevelRules)
            {
                CookLevelRulesFiles(levelRulesFilesToSerialize);
            }

            // cook .rom Room Definition files
            if (doRoomDefinitions)
            {
                CookRoomDefinitionFiles(roomDefinitionFilesSerialize);
            }

            // Files to pack
            if (doPackDat)
            {
                filesToPack.AddRange(SearchForFilesToPack(currentDir, doExcludeRaw));
                PackDatFile(filesToPack.ToArray(), Path.Combine(currentDir, _defaultDat + ".idx"));
            }

            return;
        }

        /// <summary>
        /// Searches the path for raw excel files to cook.
        /// </summary>
        /// <param name="hellgatePath">The path the search.</param>
        /// <returns>Paths found as an array.</returns>
        public static string[] SearchForExcelFiles(string hellgatePath)
        {
            List<string> excelFilesToCook = new List<string>();
            string dataDir = Path.Combine(hellgatePath, "data");
            string dataCommonDir = Path.Combine(hellgatePath, "data_common");
            string excelDataDir = Path.Combine(dataDir, ExcelFile.FolderPath);
            string excelDataCommonDir = Path.Combine(dataCommonDir, ExcelFile.FolderPath);
            string excelWildCard = String.Format(SearchPattern, ExcelFile.ExtensionDeserialised);

            if (Directory.Exists(excelDataDir))
            {
                string[] result = Directory.GetFiles(excelDataDir, excelWildCard, SearchOption.TopDirectoryOnly);
                excelFilesToCook.AddRange(result);
            }

            if (Directory.Exists(excelDataCommonDir))
            {
                string[] result = Directory.GetFiles(excelDataCommonDir, excelWildCard, SearchOption.TopDirectoryOnly);
                excelFilesToCook.AddRange(result);
            }

            return excelFilesToCook.ToArray();
        }

        public static string[] SearchForStringFiles(string hellgatePath)
        {
            string[] stringPaths = null;
            string dataDir = Path.Combine(hellgatePath, "data");
            string dataCommonDir = Path.Combine(hellgatePath, "data_common");
            string stringDataDir = Path.Combine(dataDir, StringsFile.FolderPath);
            string stringWildCard = String.Format(SearchPattern, StringsFile.FileExtentionClean);

            if (Directory.Exists(stringDataDir))
            {
                stringPaths = Directory.GetFiles(stringDataDir, stringWildCard, SearchOption.AllDirectories);
            }

            return stringPaths;
        }


        public static string[] SearchForXmlFiles(string hellgatePath)
        {
            string[] xmlPaths = null;
            string dataDir = Path.Combine(hellgatePath, "data");

            if (Directory.Exists(dataDir))
            {
                string xmlWildCard = String.Format(SearchPattern, XmlCookedFile.ExtensionDeserialised);
                xmlPaths = Directory.GetFiles(dataDir, xmlWildCard, SearchOption.AllDirectories);
                xmlPaths = xmlPaths.Where(str => !str.Contains("uix")).ToArray(); // remove uix xml files
            }

            return xmlPaths;
        }

        public static string[] SearchForFilesToPack(string hellgatePath, bool excludeRaw)
        {
            List<string> filesToPack = new List<string>();
            string dataDir = Path.Combine(hellgatePath, "data");
            string dataCommonDir = Path.Combine(hellgatePath, "data_common");

            if (Directory.Exists(dataDir))
            {
                string[] result = Directory.GetFiles(dataDir, "*", SearchOption.AllDirectories);
                filesToPack.AddRange(result);
            }

            if (Directory.Exists(dataCommonDir))
            {
                string[] result = Directory.GetFiles(dataCommonDir, "*", SearchOption.AllDirectories);
                filesToPack.AddRange(result);
            }

            if (excludeRaw == true)
            {
                // todo
            }

            return filesToPack.ToArray();
        }

        public static bool PackDatFile(string[] filesToPack, string outputPath)
        {
            IndexFile newPack = new IndexFile() { FilePath = outputPath };
            foreach (String filePath in filesToPack)
            {
                String fileName = Path.GetFileName(filePath);
                String directory = Path.GetDirectoryName(filePath);
                int dataCursor = directory.IndexOf("data");
                directory = directory.Remove(0, dataCursor) + "\\";

                byte[] buffer;
                try
                {
                    buffer = File.ReadAllBytes(filePath);
                }
                catch (Exception ex)
                {
                    ExceptionLogger.LogException(ex);
                    Console.WriteLine(String.Format("Warning: Could not read file {0}", filePath));
                    continue;
                }

                Console.WriteLine("Packing " + directory + fileName);
                if (newPack.AddFile(directory, fileName, buffer) == false)
                {
                    Console.WriteLine("Warning: Failed to add file to index...");
                }
            }

            string thisPack = Path.GetFileNameWithoutExtension(outputPath);
            byte[] indexBytes = newPack.ToByteArray();
            Crypt.Encrypt(indexBytes);
            Console.WriteLine("Writing " + thisPack);
            try
            {
                File.WriteAllBytes(outputPath, indexBytes);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
                Console.WriteLine(String.Format("Fatal error: Could not write index {0}", thisPack));
                return false;
            }

            Console.WriteLine(String.Format("{0} generation complete.", thisPack));
            return true;
        }

        // really should make a base for .drl and .rom
        public static void CookLevelRulesFiles(IEnumerable<String> levelRulesFiles)
        {
            foreach (String filePath in levelRulesFiles)
            {
                try
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(filePath);

                    LevelRulesFile levelRulesFile = new LevelRulesFile();
                    levelRulesFile.ParseXmlDocument(xmlDocument);
                    byte[] fileBytes = levelRulesFile.ToByteArray();

                    File.WriteAllBytes(filePath.Replace(LevelRulesFile.ExtensionDeserialised, LevelRulesFile.Extension), fileBytes);
                }
                catch (Exception e)
                {
                    ExceptionLogger.LogException(e);
                    Console.WriteLine(String.Format("Error: Failed to serialize file {0}", filePath));
                }
            }
        }

        public static void CookRoomDefinitionFiles(IEnumerable<String> levelRulesFiles)
        {
            foreach (String filePath in levelRulesFiles)
            {
                try
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(filePath);

                    RoomDefinitionFile roomDefinitionFile = new RoomDefinitionFile();
                    roomDefinitionFile.ParseXmlDocument(xmlDocument);
                    byte[] fileBytes = roomDefinitionFile.ToByteArray();

                    File.WriteAllBytes(filePath.Replace(RoomDefinitionFile.ExtensionDeserialised, RoomDefinitionFile.Extension), fileBytes);
                }
                catch (Exception e)
                {
                    ExceptionLogger.LogException(e);
                    Console.WriteLine(String.Format("Error: Failed to serialize file {0}", filePath));
                }
            }
        }



        public static void CookExcelFiles(string[] excelFilesToCook)
        {
            foreach (String excelPath in excelFilesToCook)
            {
                CookExcelFile(excelPath);
            }
        }

        public static bool CookExcelFile(string excelPath)
        {
            byte[] excelBuffer = null;
            try
            {
                excelBuffer = File.ReadAllBytes(excelPath);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
                Console.WriteLine(String.Format("Error reading file {0}", excelPath));
                return false;
            }

            ExcelFile excelFile;
            try
            {
                excelFile = new ExcelFile(excelBuffer, excelPath);
            }
            catch (Exception e)
            {
                Console.WriteLine("Critical Error:\n" + e);
                return false;
            }
            if (excelFile.HasIntegrity == false)
            {
                Console.WriteLine(String.Format("Failed to parse excel file {0}", excelPath));
                return false;
            }

            Console.WriteLine(String.Format("Cooking {0}", Path.GetFileNameWithoutExtension(excelPath)));
            excelBuffer = excelFile.ToByteArray();
            if (excelBuffer == null)
            {
                Console.WriteLine(String.Format("Failed to serialize excel file {0}", excelFile.StringId));
                return false;
            }

            String writeToPath = excelPath + ".cooked";
            try
            {
                File.WriteAllBytes(writeToPath, excelBuffer);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
                Console.WriteLine(String.Format("Failed to write cooked file {0} ", writeToPath));
                return false;
            }

            return true;
        }

        public static void CookStringFiles(string[] stringFilesToCook)
        {
            foreach (String stringPath in stringFilesToCook)
            {
                CookStringFile(stringPath);
            }
        }

        public static bool CookStringFile(string stringPath)
        {
            byte[] stringsBuffer = null;
            try
            {
                stringsBuffer = File.ReadAllBytes(stringPath);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
                Console.WriteLine(String.Format("Error reading file {0}", stringPath));
                return false;
            }

            StringsFile stringsFile = new StringsFile(stringsBuffer, Path.GetFileName(stringPath).ToUpper());
            if (stringsFile.HasIntegrity == false)
            {
                Console.WriteLine(String.Format("Failed to parse strings file {0}", stringPath));
                return false;
            }

            Console.WriteLine(String.Format("Cooking {0}", Path.GetFileNameWithoutExtension(stringPath)));
            stringsBuffer = stringsFile.ToByteArray();
            if (stringsBuffer == null)
            {
                Console.WriteLine(String.Format("Failed to serialize strings file {0}", stringsFile.StringId));
                return false;
            }

            String writeToPath = stringPath + ".cooked";
            try
            {
                File.WriteAllBytes(writeToPath, stringsBuffer);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
                Console.WriteLine(String.Format("Failed to write cooked file {0} ", writeToPath));
                return false;
            }

            return true;
        }

        public static void CookXmlFiles(string[] xmlFilesToCook, FileManager fileManager)
        {
            if (xmlFilesToCook.Length > 0)
            {
                if (XmlCookedFile.IsInitialized == false) XmlCookedFile.Initialize(fileManager);

                Console.WriteLine("Cooking XML Files... Loading Data Tables...");

                foreach (String xmlPath in xmlFilesToCook)
                {
                    CookXmlFile(xmlPath, fileManager);
                }
            }
        }

        public static bool CookXmlFile(string xmlPath, FileManager fileManager)
        {
            try
            {
                if (XmlCookedFile.IsInitialized == false)
                    XmlCookedFile.Initialize(fileManager);

                Console.WriteLine(String.Format("Cooking {0}", Path.GetFileNameWithoutExtension(xmlPath)));

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(xmlPath);

                XmlCookedFile cookedXmlFile = new XmlCookedFile();
                byte[] xmlCookedData = cookedXmlFile.CookXmlDocument(xmlDocument);
                File.WriteAllBytes(xmlPath + ".cooked", xmlCookedData);
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
                Console.WriteLine("Error: Failed to cook XML file: " + ex.Message);
                return false;
            }
        }
    }
}
