using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Hellgate;
using FileEntry = Hellgate.IndexFile.FileEntry;

namespace Hellpack
{
    static class Program
    {
        static String currentDir = Directory.GetCurrentDirectory();
        static String dataDir = Path.Combine(currentDir, Common.DataPath);
        static String dataCommonDir = Path.Combine(currentDir, Common.DataCommonPath);
        static String defaultDat = "sp_hellgate_1337";
        static String searchPattern = "*{0}";

        // Messages
        static string welcomeMsg = "Hellpack - the Hellgate London compiler.\nWritten by the Revival Team, 2010\nhttp://www.hellgateaus.net";
        static string noPathsMsg = "Sorry, no data paths were found. Check error.xml for details.";
        static string badSyntaxMsg = "Incorrect argument given: {0}";
        static string usageMsg = "Usage: todo";

        // Program options / switches
        static bool doCookTxt = false;
        static bool doCookXml = false;
        static bool doPackDat = false;
        static bool doSearchCd = false;
        static bool doExcludeRaw = false;

        // Queues
        static List<String> filesToPack = new List<String>();
        static List<String> excelFilesToCook = new List<String>();
        static List<String> stringFilesToCook = new List<String>();
        static List<String> xmlFilesToCook = new List<String>();

        static void Main(string[] args)
        {
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
                SearchForTxtFiles();
            }

            // Search for Xml files to cook
            if (doSearchCd && doCookXml)
            {
                SearchForXmlFiles();
            }

            // Cook Txt files
            if (doCookTxt)
            {
                CookExcelFiles();
                CookStringFiles();
            }

            // Cook Xml files
            if (doCookXml)
            {
                CookXmlFiles();
            }

            // Files to pack
            if (doPackDat)
            {
                SearchForFilesToPack();
                PackDatFile();
            }

            return;
        }

        static void SearchForTxtFiles()
        {
            String excelDataDir = Path.Combine(dataDir, ExcelFile.FolderPath);
            String excelDataCommonDir = Path.Combine(dataCommonDir, ExcelFile.FolderPath);
            String stringDataDir = Path.Combine(dataDir, StringsFile.FolderPath);
            String excelWildCard = String.Format(searchPattern, ExcelFile.FileExtentionClean);
            String stringWildCard = String.Format(searchPattern, StringsFile.FileExtentionClean);

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

            if (Directory.Exists(stringDataDir))
            {
                string[] result = Directory.GetFiles(stringDataDir, stringWildCard, SearchOption.AllDirectories);
                stringFilesToCook.AddRange(result);
            }
        }

        static void SearchForXmlFiles()
        {
            if (Directory.Exists(dataDir))
            {
                string xmlWildCard = String.Format(searchPattern, XmlCookedFile.FileExtentionClean);
                string[] result = Directory.GetFiles(dataDir, xmlWildCard, SearchOption.AllDirectories);
                xmlFilesToCook.AddRange(result);
                xmlFilesToCook = xmlFilesToCook.Where(str => !str.Contains("uix")).ToList(); // remove uix xml files
            }
        }

        static void SearchForFilesToPack()
        {
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

            if (doExcludeRaw)
            {
                // todo
            }
        }

        static void PackDatFile()
        {
            String packName = defaultDat;
            if (!(packName.EndsWith(".idx"))) packName += ".idx";
            packName = Path.Combine(currentDir, packName);
            IndexFile newPack = new IndexFile()
            {
                FilePath = packName
            };

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

            string thisPack = packName.Replace(currentDir + "\\", "");
            byte[] indexBytes = newPack.ToByteArray();
            Crypt.Encrypt(indexBytes);
            Console.WriteLine("Writing " + thisPack);
            File.WriteAllBytes(packName, indexBytes);
            Console.WriteLine(thisPack + " generation complete.");
        }

        static void CookExcelFiles()
        {
            foreach (String excelPath in excelFilesToCook)
            {
                byte[] excelBuffer = File.ReadAllBytes(excelPath);
                ExcelFile excelFile = new ExcelFile(excelBuffer, excelPath);
                if (!excelFile.IntegrityCheck)
                {
                    Console.WriteLine("Failed to parse excel file: " + excelPath);
                    continue;
                }

                Console.WriteLine("Cooking " + excelPath.Replace(currentDir + "\\", ""));
                excelBuffer = excelFile.ToByteArray();
                if (excelBuffer == null)
                {
                    Console.WriteLine("Failed to cook excel file: " + excelFile.StringId);
                    continue;
                }

                String writeToPath = excelPath + ".cooked";
                try
                {
                    File.WriteAllBytes(writeToPath, excelBuffer);
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to write cooked file: " + writeToPath);
                    continue;
                }
            }
        }

        static void CookStringFiles()
        {
            foreach (String stringPath in stringFilesToCook)
            {
                byte[] stringsBuffer = File.ReadAllBytes(stringPath);
                StringsFile stringsFile = new StringsFile(stringsBuffer, Path.GetFileName(stringPath).ToUpper());
                if (!(stringsFile.IntegrityCheck == true)) continue;
                Console.WriteLine("Cooking " + stringPath.Replace(currentDir + "\\", ""));
                stringsBuffer = stringsFile.ToByteArray();
                if (stringsBuffer == null) continue;
                File.WriteAllBytes(stringPath + ".cooked", stringsBuffer);
            }
        }

        static void CookXmlFiles()
        {
            // Cook XML Files
            if (xmlFilesToCook.Count > 0)
            {
                Console.WriteLine("Cooking XML Files... Loading Data Tables...");
                FileManager fileManager = new FileManager(@"D:\Games\Hellgate London");
                fileManager.LoadTableFiles();
                XmlCookedFile.Initialize(fileManager);
                foreach (String xmlPath in xmlFilesToCook)
                {
                    try
                    {
                        Console.WriteLine("Cooking " + xmlPath.Replace(currentDir + "\\", ""));

                        XmlDocument xmlDocument = new XmlDocument();
                        xmlDocument.Load(xmlPath);

                        XmlCookedFile cookedXmlFile = new XmlCookedFile();
                        byte[] xmlCookedData = cookedXmlFile.CookXmlDocument(xmlDocument);
                        File.WriteAllBytes(xmlPath + ".cooked", xmlCookedData);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error: Failed to cook XML file: " + e);
                        continue;
                    }
                }
            }
        }
    }
}
