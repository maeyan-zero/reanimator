using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reanimator
{
    class PathTools
    {
        public static string[] FileNameFromPath(string[] paths)
        {
            string[] filenames = new string[paths.Length];
            for (int i = 0; i < filenames.Length; i++)
            {
                int j = paths[i].LastIndexOf("\\");
                string filename = paths[i].Remove(0, j + 1);
                j = filename.LastIndexOf(".");
                filenames[i] = filename.Remove(j, filename.Length - j);
            }
            return filenames;
        }

        public static string[] DirectoriesFromPath(string[] paths)
        {
            string[] directories = new string[paths.Length];
            for (int i = 0; i < directories.Length; i++)
            {
                //if (directories[i].Contains(".")) continue;
                int j = paths[i].LastIndexOf("\\");
                string dir = paths[i].Remove(0, j + 1);
                directories[i] = dir;
            }
            return directories;
        }

        public static string FileNameFromPath(string path)
        {
            int j = path.LastIndexOf("\\");
            string filename = path.Remove(0, j + 1);
            //j = filename.LastIndexOf(".");
            //filename = filename.Remove(j, filename.Length - j);
            return filename;
        }

        public static string DirectoriesFromPath(string path)
        {
            int j = path.LastIndexOf("\\");
            string filename = path.Remove(0, j + 1);
            return filename;
        }
    }
}
