using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Reanimator
{
    public class Index : IDisposable
    {
        struct Token
        {
            public static readonly byte[] head = new byte[] { 110, 105, 103, 104 };
            public static readonly byte[] sect = new byte[] { 115, 112, 103, 104 };
            public static readonly byte[] info = new byte[] { 111, 105, 103, 104 };
        }

        // Quick overview of a typical index file. This is the movies_low.idx

        //0000 0003		110, 105, 103, 104	// Start File Token
        //0004 0007		4					// No Structs in Index(?) - count tokens
        //0008 0011		9					// File Count
        //0012 0015		115, 112, 103, 104  // Start of Struct Token
        //0016 0019		18					// String Count
        //0020 0023		44, 1				// No Bytes
        //0024 0323		****				// Char Data
        //0324 0327		115, 112, 103, 104  // Start of Struct Token
        //START REPEAT (string count) len:6
        //0328 0329		15, 0				// String size
        //0330 0333		X, X, X, X			// Unknown 4 bytes
        //END REPEAT
        //0436 0439		115, 112, 103, 104  // Start of Struct Token
        //START REPEAT (file count) len:80
        //0000 0003		111, 105, 103, 104  // TOKEN
        //0004 0007       x, x, x, x
        //0008 0011		x, x, x, x
        //0012 0015							// Offset in Data file (Long)
        //0016 0019		Null?
        //0020 0023		xxxx				// Uncompressed Size
        //0024 0027		xxxx				// Compressed Size
        //0028 0032       Null
        //0032 0035							// Directory array position
        //0036 0039 							// Filename array position
        //0040 0055		16 bytes?
        //0056 0067		NULLS 12 bytes
        //0068 0071		66, 73, 75, 105		// Unknown1 CONST
        //0072 0075		184, 4, 168, 0		// Unknown2
        //0076 0079		111, 105, 103, 104  // TOKEN
        //END REPEAT

        int structCount;         // offset 4
        int fileCount;           // offset 8
        int stringCount;         // offset 16
        int characterCount;      // offset 20

        int stringDataOffset;
        int stringLengthOffset;
        int fileDataOffset;

        const int stringStructLength = 6;
        const int fileStructLength = 80;

        FileStream indexFile;
        FileStream datFile;
        static byte[] buffer;
        static string[] stringTable;
        FileIndex[] fileTable;

        public struct FileIndex
        {
            int offset;
            public int Offset
            {
                set { offset = value; }
                get { return BitConverter.ToInt32(buffer, offset); }
            }

            int uncompressedSize;
            public int UncompressedSize
            {
                set { uncompressedSize = value; }
                get { return BitConverter.ToInt32(buffer, uncompressedSize); }
            }

            int compressedSize;
            public int CompressedSize
            {
                set { compressedSize = value; }
                get { return BitConverter.ToInt32(buffer, compressedSize); }
            }

            int directory;
            public int Directory
            {
                set { directory = value; }
            }
            public string DirectoryString
            {
                get { return stringTable[BitConverter.ToInt32(buffer, directory)]; }
            }

            int filename;
            public int Filename
            {
                set { filename = value; }
            }
            public string FilenameString
            {
                get { return stringTable[BitConverter.ToInt32(buffer, filename)]; }
            }
        }

        public Index(FileStream file)
        {
            indexFile = file;
            buffer = FileTools.StreamToByteArray(file);

            Crypt.Decrypt(buffer);

            //just ignore me, I was curious
            //FileStream fOut = new FileStream("out.idx", FileMode.Create);
            //fOut.Write(buffer, 0, buffer.Length);

            structCount = BitConverter.ToInt32(buffer, 4);
            fileCount = BitConverter.ToInt32(buffer, 8);
            stringCount = BitConverter.ToInt32(buffer, 16);
            characterCount = BitConverter.ToInt32(buffer, 20);

            stringDataOffset = 24;
            stringLengthOffset = stringDataOffset + characterCount + Token.sect.Length;
            fileDataOffset = stringLengthOffset + (stringStructLength * stringCount) + Token.sect.Length;

            InitializeStringTable();
            InitializeFileTable();
        }

        void InitializeStringTable()
        {
            stringTable = new string[stringCount];

            int position = 24;
            short stringLength;
            char[] characterArray;

            for (int i = 0; i < stringTable.Length; i++)
            {
                stringLength = BitConverter.ToInt16(buffer, stringLengthOffset + (i * stringStructLength));
                characterArray = new char[stringLength];
                Array.Copy(buffer, position, characterArray, 0, stringLength);
                stringTable[i] = new string(characterArray);
                position += stringLength + 1;
            }
        }

        void InitializeFileTable()
        {
            fileTable = new FileIndex[fileCount];

            for (int i = 0; i < fileCount; i++)
            {
                fileTable[i].Offset = fileDataOffset + (i * fileStructLength) + 12;
                fileTable[i].UncompressedSize = fileDataOffset + (i * fileStructLength) + 20;
                fileTable[i].CompressedSize = fileDataOffset + (i * fileStructLength) + 24;
                fileTable[i].Directory = fileDataOffset + (i * fileStructLength) + 32;
                fileTable[i].Filename = fileDataOffset + (i * fileStructLength) + 36;
            }
        }

        public FileIndex[] GetFileTable()
        {
            return fileTable;
        }

        public String FileName
        {
            get { return indexFile.Name.Substring(0, indexFile.Name.LastIndexOf('.')); }
        }

        public String FileDirectory
        {
            get { return indexFile.Name.Substring(0, indexFile.Name.LastIndexOfAny("\\".ToCharArray())); }
        }

        public bool OpenAccompanyingDat()
        {
            if (datFile == null)
            {
                try
                {
                    datFile = new FileStream(this.FileName + ".dat", FileMode.Open);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        public bool DatFileOpen
        {
            get { return datFile == null ? false : true; }
        }

        public byte[] ReadDataFile(Index.FileIndex file)
        {
            if (datFile == null)
            {
                return null;
            }

            // Yes, this needs to be recreated for each file - it was having a fit when I didn't
            ManagedZLib.Decompress decompress = new ManagedZLib.Decompress(datFile);
            byte[] buffer = new byte[file.UncompressedSize];
            datFile.Seek(file.Offset, SeekOrigin.Begin);
            if (file.CompressedSize == 0)
            {
                decompress.Read(buffer, 0, 0);
            }
            else
            {
                decompress.Read(buffer, 0, file.UncompressedSize);
            }

            return buffer;
        }

        public void Dispose()
        {
            if (indexFile != null)
            {
                indexFile.Dispose();
            }
            if (datFile != null)
            {
                datFile.Dispose();
            }
        }
    }
}
