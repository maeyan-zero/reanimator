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
            String currentDir = Directory.GetCurrentDirectory();
            String dataDir = Path.Combine(currentDir, Common.DataPath);
            String dataCommonDir = Path.Combine(currentDir, Common.DataCommonPath);
            String excelDir = ExcelFile.FolderPath;
            String stringDir = StringsFile.FolderPath;
            String defaultDat = "sp_hellgate_1337";

            Boolean hasDataDir = Directory.Exists(dataDir);
            Boolean hasDataCommonDir = Directory.Exists(dataCommonDir);

            // Any files to pack? If no, exit
            if (!(hasDataDir) && !(hasDataCommonDir))
            {
                Console.WriteLine("Sorry, no buffer paths were found. Check error.xml for details.");
                Console.ReadKey();
                return;
            }

            // Get a list of all the files to add.
            List<String> filesToPack = new List<String>();
            List<String> excelFilesToCook = new List<String>();

            if (Directory.Exists(dataDir + excelDir))
                excelFilesToCook.AddRange(Directory.GetFiles(dataDir + excelDir, "*.txt", SearchOption.TopDirectoryOnly));
            if (Directory.Exists(dataCommonDir + excelDir))
                excelFilesToCook.AddRange(Directory.GetFiles(dataCommonDir + excelDir, "*.txt", SearchOption.TopDirectoryOnly));

            // Cook all the excel files.
            foreach (String excelPath in excelFilesToCook)
            {
                byte[] excelBuffer = File.ReadAllBytes(excelPath);
                ExcelFile excelFile = new ExcelFile(excelBuffer);
                if (!(excelFile.IntegrityCheck == true)) continue;
                excelBuffer = excelFile.ToByteArray();
                if (excelBuffer == null) continue;
                File.WriteAllBytes(excelPath + ".cooked", excelBuffer);
            }


            // Cook String files


            // Cook XML files


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
                
                byte[] buffer = File.ReadAllBytes(filePath);
                if (buffer == null) continue;

                newPack.AddFile(directory, fileName, buffer);
            }

            byte[] indexBytes = newPack.GenerateIndexFile();
            File.WriteAllBytes(packName, indexBytes);

            // All done, save the idx and finish.
            //indexFile.CloseStream();
            //Boolean saveResult = indexFile.Save();
            //String operationResult = (!(saveResult)) ? "Sorry, there was a problem saving the file." : "Modification compilation success.";
            //Console.WriteLine(operationResult);
            return;
        }
    }
}
