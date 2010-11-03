using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using FileEntry = Reanimator.Index.FileEntry;

namespace Reanimator
{
    public class FileManager
    {
        public Hashtable FileEntries { get; private set; }

        bool MPVersion { get; set; }
        string HellgatePath { get; set; }
        string HellgateDataPath { get { return Path.Combine(HellgatePath, "data"); } }

        string[] MPQuery = new string[] { "hellgate*", "mp_hellgate*" };
        string[] SPQuery = new string[] { "hellgate*", "sp_hellgate*" };

        public FileManager(string hellgatePath)
            : this(hellgatePath, false)
        {
        
        }

        public FileManager(string hellgatePath, bool mpVersion)
        {
            HellgatePath = hellgatePath;
            MPVersion = mpVersion;

            List<string> idxPaths = new List<string>();
            string[] query = mpVersion ? MPQuery : SPQuery;
            foreach (string fileQuery in query)
            {
                idxPaths.AddRange(Directory.GetFiles(HellgateDataPath, fileQuery).Where(p => p.EndsWith(".idx")));
            }

            FileEntries = new Hashtable();

            foreach (string idxPath in idxPaths)
            {
                byte[] indexBuffer = File.ReadAllBytes(idxPath);
                Index index = new Index(idxPath);
                index.ParseData(indexBuffer, idxPath);

                foreach (FileEntry fileEntry in index.Files)
                {
                    if (!(FileEntries.Contains(fileEntry.LongHash)))
                    {
                        FileEntries.Add(fileEntry.LongHash, fileEntry);
                    }
                    else
                    {
                        uint longHash = fileEntry.LongHash;
                        FileEntry existingFile = (FileEntry)FileEntries[longHash];
                        long existingFileTime = existingFile.FileTime;

                        if (existingFileTime < fileEntry.FileTime)
                        {
                            FileEntries[longHash] = fileEntry;
                        }
                    }
                }
            }
        }
    }
}
