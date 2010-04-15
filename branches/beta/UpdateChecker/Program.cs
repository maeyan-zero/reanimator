//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.IO;
//using System.Xml.Serialization;
//using System.Threading;

//namespace Reanimator
//{
//    class Program
//    {
//        static UpdateCheckerParams currentVersionInfo;

//        static void Main(string[] args)
//        {
//            try
//            {
//                currentVersionInfo = new UpdateCheckerParams("Test", "0_1_0", ".mod.zip", "http://www.hellgateaus.net/forum/viewtopic.php?f=47&t=1279&p=18796#p18796", @"C:\");

//                UpdateChecker checker = currentVersionInfo.updateChecker;

//                checker.GetWebsiteCompleteEvent += new UpdateChecker.GetWebsiteComplete(checker_GetWebsiteCompleteEvent);
//                checker.FileDownloadCompleteEvent += new UpdateChecker.FileDownloadComplete(checker_FileDownloadCompleteEvent);

//                checker.GetWebsiteByUrl(currentVersionInfo.installedVersion.link);

//                Console.Read();
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//                Console.Read();
//            }
//        }

//        static void checker_GetWebsiteCompleteEvent(List<NewMod> mods)
//        {
//            foreach (NewMod mod in mods)
//            {
//                // if the mod file defines its own extension use that one)
//                string extension = mod.extension == null ? currentVersionInfo.installedVersion.extension : mod.extension;

//                // might also want to check if the mod is the most up-to-date one (compared to possible other mods)
//                if (!File.Exists(currentVersionInfo.saveFolder + mod.name + "_" + mod.version.CurrentVersion + extension) && !currentVersionInfo.installedVersion.IsNewestVersion(mod.version))
//                {
//                    Console.WriteLine("Newer version found! Downloading file to " + currentVersionInfo.saveFolder + "...");
//                    currentVersionInfo.updateChecker.GetFile(mod, currentVersionInfo.saveFolder, extension);
//                }
//                else
//                {
//                    Console.WriteLine("You already have the newest version installed or downloaded!");
//                }

//                ConsoleColor tmp = Console.ForegroundColor;
//                Console.ForegroundColor = ConsoleColor.Yellow;
//                Console.WriteLine(mod.ToStringWithMultipleLines());
//                Console.ForegroundColor = tmp;
//            }

//            Console.WriteLine("Done!");
//            Console.Read();
//        }

//        static void checker_FileDownloadCompleteEvent(string name)
//        {
//            Console.WriteLine("DONE!");
//        }
//    }
//}
