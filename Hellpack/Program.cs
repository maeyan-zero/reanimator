using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Hellgate;

namespace Revival
{
    class Program
    {
        static void Main(string[] args)
        {
            String currentDir = Directory.GetCurrentDirectory();
            String dataDir = Path.Combine(currentDir, "data\\");
            String dataCommonDir = Path.Combine(dataDir, "data_common\\");
            Boolean hasDataDir = Directory.Exists(Path.Combine(currentDir, "data"));
            Boolean hasDataCommonDir = Directory.Exists(Path.Combine(currentDir, "data_common"));

            // Check for the data and data common paths
            if (!(hasDataDir) && !(hasDataCommonDir))
            {
                Console.WriteLine("Sorry, no data paths were found. Check error.xml for details.");
                ExceptionLogger.LogException(new FileLoadException("data and data_common directories could not be found. Make sure Hellpack is in the directory that contains these directories."), true);
                return;
            }


            // Get a list of all the files to add.
            List<String> filesToPack = new List<string>();
            if (hasDataDir)
            {
                String[] dataDirFiles = Directory.GetFiles(dataDir, "*", SearchOption.AllDirectories);
                filesToPack.AddRange(dataDirFiles);
            }
            if (hasDataCommonDir)
            {
                String[] dataDirFiles = Directory.GetFiles(dataCommonDir, "*", SearchOption.AllDirectories);
                filesToPack.AddRange(dataDirFiles);
            }


            String packName = args.Length == 0 ? "sp_hellgate_1337" : args[1];
            if (!(packName.EndsWith(".idx"))) packName += ".idx";
            packName = Path.Combine(currentDir, packName);

            IndexFile indexFile = new IndexFile()
            {
                FilePath = packName
            };

            foreach (String filePath in filesToPack)
            {
                Boolean doCompress = (filePath.Contains("sounds") || filePath.Contains("movies")) ? false : true;
                String directory = Path.GetDirectoryName(filePath).Remove(0, filePath.IndexOf("data"));
                directory = (!(directory.EndsWith("\\"))) ? directory += "\\" : directory;
                String fileName = Path.GetFileName(filePath);
                Byte[] fileBytes = File.ReadAllBytes(filePath);
                indexFile.Add(directory, fileName, fileBytes, doCompress);
            }

            indexFile.CloseStream();
            if (!(indexFile.Save()))
            {
                Console.WriteLine("Problem saving idx fileStructure.");
                return;
            }

            Console.WriteLine(packName.Replace(".idx", "") + " written.");
            return;
        }
    }
}
