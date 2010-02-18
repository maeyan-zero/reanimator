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
                Mod mod = UpdateChecker.GetModInfoFromSite(url);

                extension = mod.extension == null ? extension : mod.extension;

                if (!File.Exists(folder + mod.name + "_" + mod.version + extension))
                {
                    Console.WriteLine("Newer version found! Downloading file...");
                    // if the mod file defines its own extension use that one)
                    UpdateChecker.DownloadFile(mod, folder, extension);
                }
                else
                {
                    Console.WriteLine("You already have the newest version!");
                }

                ConsoleColor tmp = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(mod.ToStringWithMultipleLines());
                Console.ForegroundColor = tmp;

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
