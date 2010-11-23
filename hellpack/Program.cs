using System;
using System.Collections.Generic;
using System.IO;
using Hellgate;
using FileEntry = Hellgate.Index.FileEntry;

namespace Hellpack
{
    class Program
    {
        static void Main(string[] args)
        {
            if (false)
            {
                FileManager fileManager = new FileManager(@"D:\Games\Hellgate London");
                fileManager.LoadExcelFiles();
                byte[] data = fileManager.DataFiles["SKILLS"].ExportCSV();
                File.WriteAllBytes(@"D:\Projects\Hellgate London\Reanimator\trunk\bin\Hellpack\x64\Debug\data_common\excel\SKILLS.orig.txt", data);
            }




            String currentDir = Directory.GetCurrentDirectory();
            String dataDir = Path.Combine(currentDir, Common.DataPath);
            String dataCommonDir = Path.Combine(currentDir, Common.DataCommonPath);
            String excelDir = ExcelFile.FolderPath;
            String stringDir = "excel\\strings\\";
            String defaultDat = "sp_hellgate_1337";
            Boolean hasDataDir = Directory.Exists(dataDir);
            Boolean hasDataCommonDir = Directory.Exists(dataCommonDir);
            String welcomeMsg = "Hellpack - the Hellgate London compiler.\nWritten by the Revival Team, 2010\nhttp://www.hellgateaus.net\n";
            String noPathsMsg = "Sorry, no data paths were found. Check error.xml for details.";

            Console.WriteLine(welcomeMsg);
            if (!(hasDataDir) && !(hasDataCommonDir))
            {
                Console.WriteLine(noPathsMsg);
                Console.ReadKey();
                return;
            }

            // Get a list of all the files to add.
            List<String> filesToPack = new List<String>();
            List<String> excelFilesToCook = new List<String>();
            List<String> stringFilesToCook = new List<String>();

            // Query Excel
            if (Directory.Exists(dataDir + excelDir))
                excelFilesToCook.AddRange(Directory.GetFiles(dataDir + excelDir, "*.txt", SearchOption.TopDirectoryOnly));
            if (Directory.Exists(dataCommonDir + excelDir))
                excelFilesToCook.AddRange(Directory.GetFiles(dataCommonDir + excelDir, "*.txt", SearchOption.TopDirectoryOnly));

            // Query Strings
            if (Directory.Exists(dataDir + stringDir))
                stringFilesToCook.AddRange(Directory.GetFiles(dataDir + stringDir, "*.xls.uni", SearchOption.AllDirectories));
            if (Directory.Exists(dataCommonDir + stringDir))
                stringFilesToCook.AddRange(Directory.GetFiles(dataCommonDir + stringDir, "*.xls.uni", SearchOption.AllDirectories));

            // todo: Query XML


            // cook all the excel files
            foreach (String excelPath in excelFilesToCook)
            {
                byte[] excelBuffer = File.ReadAllBytes(excelPath);
                ExcelFile excelFile = new ExcelFile(excelBuffer);
                if (!excelFile.IntegrityCheck)
                {
                    Console.WriteLine("Failed to uncook excel file: " + excelPath);
                    continue;
                }

                Console.WriteLine("Cooking " + excelPath.Replace(currentDir + "\\", ""));
                excelBuffer = excelFile.ToByteArray();
                if (excelBuffer == null)
                {
                    Console.WriteLine("Failed to cook excel file: " + excelFile.StringID);
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


            // Cook String files
            foreach (String stringPath in stringFilesToCook)
            {
                byte[] stringsBuffer = File.ReadAllBytes(stringPath);
                StringsFile stringsFile = new StringsFile(stringsBuffer);
                if (!(stringsFile.IntegrityCheck == true)) continue;
                Console.WriteLine("Cooking " + stringPath.Replace(currentDir + "\\", ""));
                stringsBuffer = stringsFile.ToByteArray();
                if (stringsBuffer == null) continue;
                File.WriteAllBytes(stringPath + ".cooked", stringsBuffer);
            }

            // todo: Cook XML files


            // Files to pack
            if (hasDataDir)
                filesToPack.AddRange(Directory.GetFiles(dataDir, "*", SearchOption.AllDirectories));
            if (hasDataCommonDir)
                filesToPack.AddRange(Directory.GetFiles(dataCommonDir, "*", SearchOption.AllDirectories));


            String packName = args.Length == 0 ? defaultDat : args[1];
            if (!(packName.EndsWith(".idx"))) packName += ".idx";
            packName = Path.Combine(currentDir, packName);
            Index newPack = new Index()
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
                    buffer = File.ReadAllBytes(filePath); // buffer is never null - only exceptions are thrown
                }
                catch (Exception)
                {
                    continue;
                }

                Console.WriteLine("Packing " + directory + fileName);
                newPack.AddFile(directory, fileName, buffer);
            }

            string thisPack = packName.Replace(currentDir + "\\", "");
            byte[] indexBytes = newPack.GenerateIndexFile();
            Crypt.Encrypt(indexBytes);
            Console.WriteLine("Writing " + thisPack);
            File.WriteAllBytes(packName, indexBytes);
            Console.WriteLine(thisPack + " generation complete.");
            Console.ReadKey();
            return;
        }
    }
}
