using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Threading;

namespace Reanimator
{
    [XmlRoot("mod")]
    public class NewMod
    {
        [XmlElement("name")]
        public string name;
        [XmlElement("extension")]
        public string extension;
        [XmlElement("link")]
        public string link;
        [XmlElement("version")]
        public Version version;
        [XmlIgnore]
        public bool alreadyPresent;

        public NewMod()
        {
            this.name = null;
            this.extension = null;
            this.link = null;
            this.version = new Version();
        }

        public override string ToString()
        {
            string content = string.Empty;
            content += name + "_";
            content += version.CurrentVersion;
            content += extension;

            return content;
        }

        public string ToStringCompleteInfo()
        {
            string content = string.Empty;
            content += name + " - ";
            content += version.CurrentVersion + " - ";
            content += extension;
            content += link;

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

        public bool IsNewestVersion(Version version)
        {
            return this.version.IsNewestVersion(version);
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

        public bool IsNewestVersion(Version version)
        {
            if (this.majorVersion >= version.majorVersion)
            {
                if (this.majorVersion > version.majorVersion)
                {
                    return true;
                }
                else
                {
                    if (this.minorVersion >= version.minorVersion)
                    {
                        if (this.minorVersion > version.minorVersion)
                        {
                            return true;
                        }
                        else
                        {
                            if (this.subVersion >= version.subVersion)
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

    public class UpdateCheckerParams
    {
        public UpdateChecker updateChecker;
        public NewMod installedVersion;
        public string saveFolder;

        public UpdateCheckerParams()
        {
            updateChecker = new UpdateChecker();
            installedVersion = new NewMod();
        }

        public UpdateCheckerParams(string name, string version, string extension, string url, string saveFolder)
        {
            updateChecker = new UpdateChecker();
            installedVersion = new NewMod();
            installedVersion.name = name;
            installedVersion.extension = extension;
            installedVersion.link = url;
            installedVersion.version.CurrentVersion = version;
            this.saveFolder = saveFolder;
        }
    }

    public class UpdateChecker
    {
        public delegate void GetWebsiteComplete(List<NewMod> mods);
        public event GetWebsiteComplete GetWebsiteCompleteEvent = delegate { };

        public delegate void FileDownloadComplete(NewMod mod);
        public event FileDownloadComplete FileDownloadCompleteEvent = delegate { };

        public void GetWebsiteByUrl(string url)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(GetModInfoFromSite), (object)url);
        }

        private void GetModInfoFromSite(object o)
        {
            string url = (string)o;

            string filter = "<div class=\"postbody\"><mod>";
            List<NewMod> myMods = new List<NewMod>();

            //Console.WriteLine("Opening webpage...");
            List<string> site = DownloadWebsite(url, filter);

            //Console.WriteLine("Processing data...");
            Trim(site, "<mod>", "</mod>");

            //Console.WriteLine("Deserializing information...");
            foreach (string line in site)
            {
                myMods.Add(Deserialize(line));
            }
            //Console.WriteLine("Finished deserializing information...");

            //return myMod;
            GetWebsiteCompleteEvent(myMods);
        }

        private List<string> DownloadWebsite(string path, string filter)
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

        private void Trim(List<string> site, string start, string end)
        {
            for (int counter = 0; counter < site.Count; counter++)
            {
                int startIndex = site[counter].IndexOf(start);
                int endIndex = site[counter].IndexOf(end) + end.Length;

                site[counter] = site[counter].Substring(startIndex, endIndex - startIndex);
            }
        }

        private Stream StringToStream(string site)
        {
            byte[] byteArray = Encoding.ASCII.GetBytes(site);
            MemoryStream stream = new MemoryStream(byteArray);

            return stream;
        }

        private NewMod Deserialize(string text)
        {
            Stream stream = StringToStream(text);

            XmlSerializer serializer = new XmlSerializer(typeof(NewMod));
            TextReader tr = new StreamReader(stream);
            NewMod info = (NewMod)serializer.Deserialize(tr);
            tr.Close();

            return info;
        }

        public void GetFile(NewMod mod, string saveFolder)
        {
            ThreadParam param = new ThreadParam();
            param.mod = mod;
            param.saveFolder = saveFolder;

            ThreadPool.QueueUserWorkItem(new WaitCallback(DownloadFile), (object)param);
        }

        private void DownloadFile(object o)
        {
            ThreadParam param = (ThreadParam)o;
            string savePath = param.saveFolder + param.mod.ToString();

            System.Net.WebClient client = new System.Net.WebClient();
            byte[] file = client.DownloadData(param.mod.link);

            FileStream fstream = new FileStream(savePath, FileMode.Create, FileAccess.Write);
            fstream.Write(file, 0, file.Length);
            fstream.Close();

            FileDownloadCompleteEvent(param.mod);
        }

        private struct ThreadParam
        {
            public NewMod mod;
            public string saveFolder;
        }
    }
}
