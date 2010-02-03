using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace Reanimator
{
    /// <summary>
    /// Describes the variables associated with a modification.
    /// </summary>
#pragma warning disable 0169
    struct Modification
    {
        /// <summary>
        /// Full qualifying path of the .idx file that contains the following filename.
        /// </summary>
        string index;
        /// <summary>
        /// Filename of the file that is going to be modified.
        /// </summary>
        string filename;
        /// <summary>
        /// Describes what modification to make on the referenced file.
        /// </summary>
        int action;
        /// <summary>
        /// Column of the object that is being modified (this is the descriptor).
        /// </summary>
        int column;
        /// <summary>
        /// Row of the object that is being modified (this is the entity).
        /// </summary>
        int row;

        Type type;
        int bit;
        Object data;
    }
#pragma warning restore 0169

    struct FileIndex
    {
        public FileIndex(string filename, uint checksum)
        {
            _filename = filename;
            _checksum = checksum;
        }

        string _filename;

        public string filename
        {
            get { return _filename; }
        }

        uint _checksum;

        public uint checksum
        {
            get { return _checksum; }
        }
    }

    class ModTools
    {
        static FileIndex[] file = new FileIndex[]
        {
            new FileIndex("data\\hellgate_bghigh000.dat", 0),
            new FileIndex("data\\hellgate_bghigh000.idx", 2358709037),
            new FileIndex("data\\hellgate_graphicshigh000.dat", 0),
            new FileIndex("data\\hellgate_graphicshigh000.idx", 2085585647),
            new FileIndex("data\\hellgate_localized000.dat", 0),
            new FileIndex("data\\hellgate_localized000.idx", 4166602695),
            new FileIndex("data\\hellgate_movies000.dat", 0),
            new FileIndex("data\\hellgate_movies000.idx", 3397556088),
            new FileIndex("data\\hellgate_movieshigh000.dat", 0),
            new FileIndex("data\\hellgate_movieshigh000.idx", 2832275718),
            new FileIndex("data\\hellgate_movieslow000.dat", 0),
            new FileIndex("data\\hellgate_movieslow000.idx", 674109462),
            new FileIndex("data\\hellgate_playershigh000.dat", 0),
            new FileIndex("data\\hellgate_playershigh000.idx", 1174187501),
            new FileIndex("data\\hellgate_sound000.dat", 0),
            new FileIndex("data\\hellgate_sound000.idx", 19311727),
            new FileIndex("data\\hellgate_soundmusic000.dat", 0),
            new FileIndex("data\\hellgate_soundmusic000.idx", 3608803675),
            new FileIndex("data\\hellgate000.dat", 0),
            new FileIndex("data\\hellgate000.idx", 2931504621),
            new FileIndex("data\\sp_hellgate_1.10.180.3416_1.18074.70.4256.dat", 0),
            new FileIndex("data\\sp_hellgate_1.10.180.3416_1.18074.70.4256.idx", 3689463640),
            new FileIndex("data\\sp_hellgate_localized_1.10.180.3416_1.18074.70.4256.dat", 0),
            new FileIndex("data\\sp_hellgate_localized_1.10.180.3416_1.18074.70.4256.idx", 224562155)
        };

        public static void GenerateChecksums()
        {
            // FileStream stream;
            // Crc32 crcTool = new Crc32();
            // //foreach (FileIndex file in fileIndex)
            //// {
            // for (int i = 0; i < fileIndex.Length / 2; i++)
            // {
            //     stream = new FileStream(@Properties.Settings.Default.HellgateDirectory + fileIndex[i * 2 + 1].Filename, FileMode.Open);
            //     byte[] bytes = FileTools.StreamToByteArray(stream);
            //     uint checksum = Crc32.Compute(bytes);
            //     Console.WriteLine("Checksum for file " + fileIndex[i * 2 + 1].Filename + ": " + checksum);
            // }
            // //}
        }

        public static void Check(XmlReader xmlModification)
        {

        }

        public static List<Modification> Read(XmlReader xmlModification)
        {
            List<Modification> modification = new List<Modification>();

            while (xmlModification.Read())
            {
                // Read <Revival>
                if (xmlModification.Name == "Revival")
                {
                    xmlModification.Read();

                }
            }

            return modification;
        }

        public static void Apply(Modification[] modification)
        {
            // Reads the index;

            // Extracts/Compresses the files

            // Applies the modifications

            // Recompresses the archieve
        }

        public static void GenerateIndexKeys(Index index)
        {

        }
    }
}
