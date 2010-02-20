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
        public string name;
        [XmlElement("extension")]
        public string extension;
        [XmlElement("link")]
        public string link;
        [XmlElement("version")]
        public Version version;

        public Mod()
        {
            this.name = null;
            this.extension = null;
            this.link = null;
            this.version = new Version();
        }

        public override string ToString()
        {
            string content = string.Empty;
            content += name + " - " + version.CurrentVersion + " - " + extension + " - " + link;

            return content;
        }

        public string ToStringWithMultipleLines()
        {
            string content = string.Empty;
            content += "Name:\t\t" + name + "\n";
            content += "Version:\t" + version.CurrentVersion + "\n";
            content += "Extension:\t" + extension + "\n";
            content += "Link:\t\t" + link;

            return content;
        }

        public bool IsNewestVersion(Mod mod)
        {
            return this.version.IsNewestVersion(mod);
        }
    }

    [XmlRoot("version")]
    public class Version
    {
        [XmlElement("majorVersion")]
        public int majorVersion = 0;
        [XmlElement("minorVersion")]
        public int minorVersion = 0;
        [XmlElement("subVersion")]
        public int subVersion = 0;

        public bool IsNewestVersion(Mod mod)
        {
            if (this.majorVersion >= mod.version.majorVersion)
            {
                if (this.majorVersion > mod.version.majorVersion)
                {
                    return true;
                }
                else
                {
                    if (this.minorVersion >= mod.version.minorVersion)
                    {
                        if (this.minorVersion > mod.version.minorVersion)
                        {
                            return true;
                        }
                        else
                        {
                            if (this.subVersion >= mod.version.subVersion)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public string CurrentVersion
        {
            get
            {
                return majorVersion + "_" + minorVersion + "_" + subVersion;
            }
            set
            {
                string[] ver = value.Split(new char[] { '_' });
                majorVersion = Int32.Parse(ver[0]);
                minorVersion = Int32.Parse(ver[1]);
                subVersion = Int32.Parse(ver[2]);
            }
        }
    }

    public static class UpdateChecker
    {
        public static List<Mod> GetModInfoFromSite(string url)
        {
            string filter = "<div class=\"postbody\"><mod>";
            List<Mod> myMod = new List<Mod>();

            //Console.WriteLine("Opening webpage...");
            List<string> site = GetWebsite(url, filter);

            //Console.WriteLine("Processing data...");
            Trim(site, "<mod>", "</mod>");

            //Console.WriteLine("Deserializing information...");
            foreach (string line in site)
            {
                myMod.Add(Deserialize(line));
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
            string savePath = saveFolder + mod.name + "_" + mod.version.CurrentVersion + fileExtension;

            System.Net.WebClient client = new System.Net.WebClient();
            byte[] file = client.DownloadData(mod.link);

            FileStream fstream = new FileStream(savePath, FileMode.CreateNew, FileAccess.Write);
            fstream.Write(file, 0, file.Length);
            fstream.Close();
        }
    }
}
