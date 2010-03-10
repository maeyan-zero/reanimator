using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Reanimator
{
    static public class Utility
    {
        static readonly string affix = "backup\\";

        static public bool ApplyDirectoryAffix(Index index, int id)
        {
            Index.FileIndex[] fileIndex = index.GetFileTable();

            if (fileIndex[id].DirectoryString.Contains(affix))
            {
                return false;
            }
            else
            {
                fileIndex[id].DirectoryString = fileIndex[id].DirectoryString.Insert(0, affix);
            }

            index.SetFileTable(fileIndex);

            return true;
        }

        static public bool IndexIsModified(Index index)
        {
            Index.FileIndex[] fileIndex = index.GetFileTable();

            foreach (Index.FileIndex file in fileIndex)
            {
                if (file.DirectoryString.Contains(affix))
                {
                    return true;
                }
            }

            return false;
        }

        static public bool RestoreIndex(Index index, string path)
        {
            Index.FileIndex[] fileIndex = index.GetFileTable();

            foreach (Index.FileIndex file in fileIndex)
            {
                if (file.DirectoryString.Contains(affix))
                {
                    file.DirectoryString = file.DirectoryString.Remove(0, affix.Length);
                }
            }
            index.SetFileTable(fileIndex);

            byte[] buffer = index.GenerateIndexFile();
            Crypt.Encrypt(buffer);

            try
            {
                FileStream stream = new FileStream(path, FileMode.CreateNew);
                stream.Write(buffer, 0, buffer.Length);
                stream.Close();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }
    }
}
