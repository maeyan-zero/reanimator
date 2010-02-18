using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace Reanimator
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string url = "http://www.hellgateaus.net/forum/viewtopic.php?f=47&t=1279&p=18796#p18796";
                string folder = @"F:\";
                string extension = ".mod.zip";
                
                Mod installedVersion = new Mod();
                installedVersion.name = "Test";
                installedVersion.extension = extension;
                installedVersion.Version = "0_1_0";

                List<Mod> mods = UpdateChecker.GetModInfoFromSite(url);

                foreach (Mod mod in mods)
                {
                    // if the mod file defines its own extension use that one)
                    extension = mod.extension == null ? extension : mod.extension;

                    // might also want to check if the mod is the most up-to-date one (compared to possible other mods)
                    if (!File.Exists(folder + mod.name + "_" + mod.Version + extension) && !installedVersion.IsNewestVersion(mod))
                    {
                        Console.WriteLine("Newer version found! Downloading file...");
                        UpdateChecker.DownloadFile(mod, folder, extension);
                    }
                    else
                    {
                        Console.WriteLine("You already have the newest version installed or downloaded!");
                    }

                    ConsoleColor tmp = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(mod.ToStringWithMultipleLines());
                    Console.ForegroundColor = tmp;
                }

                Console.WriteLine("Done!");
                Console.Read();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Read();
            }
        }
    }
}
