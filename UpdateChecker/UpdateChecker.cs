using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Reanimator
{
    [XmlRoot("mod")]
    public class Mod
    {
        [XmlElement("name")]
        public string name = null;
        [XmlElement("version")]
        public string version = null;
        [XmlElement("extension")]
        public string extension = null;
        [XmlElement("link")]
        public string link = null;

        public string ToString()
        {
            string content = string.Empty;
            content += name + " - " + version + " - " + extension + " - " + link;

            return content;
        }

        public string ToStringWithMultipleLines()
        {
            string content = string.Empty;
            content += "Name:\t\t" + name + "\n";
            content += "Version:\t" + version + "\n";
            content += "Extension:\t" + extension + "\n";
            content += "Link:\t\t" + link;

            return content;
        }
    }

    public static class UpdateChecker
    {
        public static Mod GetModInfoFromSite(string url)
        {
            string filter = "<div class=\"postbody\"><mod>";
            Mod myMod = null;

            //Console.WriteLine("Opening webpage...");
            List<string> site = GetWebsite(url, filter);

            //Console.WriteLine("Processing data...");
            Trim(site, "<mod>", "</mod>");

            //Console.WriteLine("Deserializing information...");
            foreach (string line in site)
            {
                myMod = Deserialize(line);
            }
            //Console.WriteLine("Finished deserializing information...");

            return myMod;
        }

        private static List<string> GetWebsite(string path, string filter)
        {
            System.Net.WebClient client = new System.Net.WebClient();
            Stream stream = client.OpenRead(path);
            StreamReader reader = new StreamReader(stream);

            List<string> list = new List<string>();

            char[] trim = new char[2];
            trim[0] = ' ';
            trim[1] = '\t';

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine().Replace("&lt;", "<").Replace("&gt;", ">").Replace("<br />", "").Trim(trim);

                if (line.StartsWith(filter))
                {
                    list.Add(line);
                }
            }
            reader.Close();

            return list;
        }

        private static void Trim(List<string> site, string start, string end)
        {
            for (int counter = 0; counter < site.Count; counter++)
            {
                int startIndex = site[counter].IndexOf(start);
                int endIndex = site[counter].IndexOf(end) + end.Length;

                site[counter] = site[counter].Substring(startIndex, endIndex - startIndex);
            }
        }

        private static Stream StringToStream(string site)
        {
            byte[] byteArray = Encoding.ASCII.GetBytes(site);
            MemoryStream stream = new MemoryStream(byteArray);

            return stream;
        }

        private static Mod Deserialize(string text)
        {
            Stream stream = StringToStream(text);

            XmlSerializer serializer = new XmlSerializer(typeof(Mod));
            TextReader tr = new StreamReader(stream);
            Mod info = (Mod)serializer.Deserialize(tr);
            tr.Close();

            return info;
        }

        public static void DownloadFile(Mod mod, string saveFolder, string fileExtension)
        {
            string savePath = saveFolder + mod.name + "_" + mod.version + fileExtension;

            System.Net.WebClient client = new System.Net.WebClient();
            byte[] file = client.DownloadData(mod.link);

            FileStream fstream = new FileStream(savePath, FileMode.CreateNew, FileAccess.Write);
            fstream.Write(file, 0, file.Length);
            fstream.Close();
        }
    }
}
