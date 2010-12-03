using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using Hellgate;

namespace Revival
{
    public static class Hellpack
    {
        static string defaultDat = "sp_hellgate_1337";
        static string searchPattern = "*{0}";
        static string welcomeMsg = "Hellpack - the Hellgate London compiler.\nWritten by the Revival Team, 2010\nhttp://www.hellgateaus.net";
        static string noPathsMsg = "Sorry, no data paths were found. Check error.xml for details.";
        static string badSyntaxMsg = "Incorrect argument given: {0}";
        static string usageMsg = "Usage: todo";

        static void Main(string[] args)
        {
            FileManager fileManager;
            string currentDir = Directory.GetCurrentDirectory();
            string dataDir = Path.Combine(currentDir, Common.DataPath);
            string dataCommonDir = Path.Combine(currentDir, Common.DataCommonPath);
            string hellgatePath = String.Empty;

            bool doCookTxt = false;
            bool doCookXml = false;
            bool doPackDat = false;
            bool doSearchCd = false;
            bool doExcludeRaw = false;

            List<string> filesToPack = new List<string>();
            List<string> excelFilesToCook = new List<string>();
            List<string> stringFilesToCook = new List<string>();
            List<string> xmlFilesToCook = new List<string>();

            #region alexs_stuff
            if (false)
            {

                fileManager = new FileManager(@"D:\Games\Hellgate London");
                fileManager.LoadTableFiles();
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



                //const String pathExcel = @"data\excel\affixes.txt";
                //byte[] excelBytes = File.ReadAllBytes(pathExcel);
                //ExcelFile excelFile = new ExcelFile(excelBytes, pathExcel);
                //byte[] excelCsvBytes = excelFile.ExportCSV();



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

                //return;
            }
            #endregion

            Console.WriteLine(welcomeMsg);
            Console.WriteLine(String.Empty);

            if (args.Length == 0)
            {
                // No arguments defined - this is the default program
                doCookTxt = true;
                doCookXml = true;
                doPackDat = true;
                doSearchCd = true;
                doExcludeRaw = true;
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
                        default:
                            if (arg.StartsWith("/p:"))
                            {
                                defaultDat = arg.Replace("/p:", "");
                                // Trim in case someone has appended the extention
                                if (defaultDat.EndsWith(".idx"))
                                    defaultDat = defaultDat.Replace(".idx", "");
                                if (defaultDat.EndsWith(".dat"))
                                    defaultDat = defaultDat.Replace(".dat", "");
                                break;
                            }
                            if (arg.EndsWith(ExcelFile.FileExtentionClean))
                            {
                                excelFilesToCook.Add(arg);
                                break;
                            }
                            if (arg.EndsWith(StringsFile.FileExtentionClean))
                            {
                                stringFilesToCook.Add(arg);
                                break;
                            }
                            if (arg.EndsWith(XmlCookedFile.FileExtentionClean))
                            {
                                xmlFilesToCook.Add(arg);
                                break;
                            }
                            else
                            {
                                Console.WriteLine(String.Format(badSyntaxMsg, arg));
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

            // Cook Txt files
            if (doCookTxt)
            {
                CookExcelFiles(excelFilesToCook.ToArray());
                CookStringFiles(stringFilesToCook.ToArray());
            }

            // Cook Xml files
            if (doCookXml)
            {
                // This requires the Hellgate London directory, be sure to set via the switch
                if (hellgatePath != String.Empty)
                {
                    fileManager = new FileManager(hellgatePath);
                    if (fileManager.HasIntegrity == false)
                    {
                        Console.WriteLine("Warning: XML could not be cooked.");
                    }
                    else
                    {
                        CookXmlFiles(xmlFilesToCook.ToArray(), fileManager);
                    }
                }
            }

            // Files to pack
            if (doPackDat)
            {
                filesToPack.AddRange(SearchForFilesToPack(currentDir, doExcludeRaw));
                PackDatFile(filesToPack.ToArray(), Path.Combine(currentDir, defaultDat + ".idx"));
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
            string excelWildCard = String.Format(searchPattern, ExcelFile.FileExtentionClean);

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
            string stringWildCard = String.Format(searchPattern, StringsFile.FileExtentionClean);

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
                string xmlWildCard = String.Format(searchPattern, XmlCookedFile.FileExtentionClean);
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

            // Pack the files!
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
                catch (Exception)
                {
                    continue;
                }

                Console.WriteLine("Packing " + directory + fileName);

                if (!newPack.AddFile(directory, fileName, buffer))
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
            catch
            {
                return false;
            }
            Console.WriteLine(thisPack + " generation complete.");
            return true;
        }

        public static void CookExcelFiles(string[] excelFilesToCook)
        {
            foreach (String excelPath in excelFilesToCook)
            {
                CookExcelFile(excelPath);
            }
        }

        public static void CookExcelFile(string excelPath)
        {
            byte[] excelBuffer = File.ReadAllBytes(excelPath);
            ExcelFile excelFile = new ExcelFile(excelBuffer, excelPath);
            if (!excelFile.HasIntegrity)
            {
                Console.WriteLine("Failed to parse excel file: " + excelPath);
                return;
            }

            Console.WriteLine(String.Format("Cooking {0}", Path.GetFileNameWithoutExtension(excelPath)));
            excelBuffer = excelFile.ToByteArray();
            if (excelBuffer == null)
            {
                Console.WriteLine("Failed to cook excel file: " + excelFile.StringId);
                return;
            }

            String writeToPath = excelPath + ".cooked";
            try
            {
                File.WriteAllBytes(writeToPath, excelBuffer);
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to write cooked file: " + writeToPath);
                return;
            }
        }

        public static void CookStringFiles(string[] stringFilesToCook)
        {
            foreach (String stringPath in stringFilesToCook)
            {
                CookStringFile(stringPath);
            }
        }

        public static void CookStringFile(string stringPath)
        {
            byte[] stringsBuffer = File.ReadAllBytes(stringPath);
            StringsFile stringsFile = new StringsFile(stringsBuffer, Path.GetFileName(stringPath).ToUpper());
            if (!(stringsFile.HasIntegrity == true)) return;
            Console.WriteLine(String.Format("Cooking {0}", Path.GetFileNameWithoutExtension(stringPath)));
            stringsBuffer = stringsFile.ToByteArray();
            if (stringsBuffer == null) return;
            File.WriteAllBytes(stringPath + ".cooked", stringsBuffer);
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

        public static void CookXmlFile(string xmlPath, FileManager fileManager)
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
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: Failed to cook XML file: " + e);
            }
        }
    }
}
