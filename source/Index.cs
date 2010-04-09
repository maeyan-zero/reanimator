using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.ComponentModel;

namespace Reanimator
{
    public class Index : IDisposable
    {
        [DllImport("zlibwapi86.dll")]
        private static extern int uncompress
        (
            [MarshalAs(UnmanagedType.LPArray)]
            Byte[] destinationBuffer,
            [MarshalAs(UnmanagedType.U4)]
            ref UInt32 destinationLength,
            [MarshalAs(UnmanagedType.LPArray)]
            Byte[] sourceBuffer,
            [MarshalAs(UnmanagedType.U4)]
            UInt32 sourceLength
        );

        [DllImport("zlibwapi64.dll")]
        private static extern int uncompress
        (
            [MarshalAs(UnmanagedType.LPArray)]
            Byte[] destinationBuffer,
            [MarshalAs(UnmanagedType.U8)]
            ref UInt64 destinationLength,
            [MarshalAs(UnmanagedType.LPArray)]
            Byte[] sourceBuffer,
            [MarshalAs(UnmanagedType.U8)]
            UInt64 sourceLength
        );

        static public readonly int LatestPatch = 10;
        static public readonly int LatestPatchLocalized = 11;
        static public readonly int ExcelTablesIndex = 2572;
        static public readonly string[] FileNames = { "hellgate_bghigh000",
                                                     "hellgate_graphicshigh000", 
                                                     "hellgate_localized000", 
                                                     "hellgate_movies000",
                                                     "hellgate_movieshigh000",
                                                     "hellgate_movieslow000",
                                                     "hellgate_playershigh000",
                                                     "hellgate_sound000",
                                                     "hellgate_soundmusic000",
                                                     "hellgate000",
                                                     "sp_hellgate_1.10.180.3416_1.18074.70.4256",
                                                     "sp_hellgate_localized_1.10.180.3416_1.18074.70.4256" };
        struct Token
        {
            public static readonly UInt32 head = 0x6867696E; // 'nigh'
            public static readonly UInt32 sect = 0x68677073; // 'spgh'
            public static readonly UInt32 info = 0x6867696F; // 'oigh'
        }

        Int32 structCount;         // offset 4
        Int32 fileCount;           // offset 8
        Int32 stringCount;         // offset 16
        Int32 characterCount;      // offset 20

        int stringDataOffset;
        int stringLengthOffset;
        int fileDataOffset;

        const int stringStructLength = 6;
        const int fileStructLength = 80;

        byte[] buffer;
        string[] stringTable;
        Int32[] stringTableUnknowns;
        FileIndex[] fileTable;

        bool modified;
        const string affix = "backup\\";

        FileStream indexFile;
        FileStream datFile;

        public FileStream DataFile
        {
            get
            {
                return datFile;
            }
        }

        public class FileIndex
        {
            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public class FileIndexStruct
            {
                public UInt32 startToken;
                public Int32 unknown1_1;
                public Int32 unknown1_2;
                public Int32 dataOffset;
                public Int32 null1;
                public Int32 uncompressedSize;
                public Int32 compressedSize;
                public Int32 null2;
                public Int32 directoryArrayPosition;
                public Int32 filenameArrayPosition;
                public Int32 unknown2_1;
                public Int32 unknown2_2;
                public Int32 unknown2_3;
                public Int32 unknown2_4;
                public Int32 null3_1;
                public Int32 null3_2;
                public Int32 null3_3;
                public Int32 first4BytesOfFile;
                public Int32 second4BytesOfFile;
                public UInt32 endToken;
            };

            [Browsable(false)]
            public FileIndexStruct FileStruct { get; set; }

            public int DataOffset
            {
                get { return FileStruct.dataOffset; }
            }

            public int UncompressedSize
            {
                get { return FileStruct.uncompressedSize; }
            }
            public int CompressedSize
            {
                get { return FileStruct.compressedSize; }
            }

            [Browsable(false)]
            public int Directory
            {
                get { return FileStruct.directoryArrayPosition; }
                set { FileStruct.directoryArrayPosition = value; }
            }
            public string DirectoryString { get; set; }

            [Browsable(false)]
            public int FileName
            {
                get { return FileStruct.filenameArrayPosition; }
            }
            public string FileNameString { get; set; }

            public bool Modified
            {
                get
                {
                    return DirectoryString.Contains(affix);
                }
            }
        }

        public FileIndex[] FileTable
        {
            get
            {
                return fileTable;
            }
        }

        public String FileName
        {
            get
            {
                int n = indexFile.Name.LastIndexOfAny("\\".ToCharArray()) + 1;
                return indexFile.Name.Substring(n, indexFile.Name.LastIndexOf('.') - n);
            }
        }

        public String FileDirectory
        {
            get { return indexFile.Name.Substring(0, indexFile.Name.LastIndexOfAny("\\".ToCharArray()) + 1); }
        }

        public bool DatFileOpen
        {
            get { return datFile == null ? false : true; }
        }

        public bool Modified
        {
            get
            {
                return modified;
            }
        }

        public Index(FileStream file)
        {
            indexFile = file;
            buffer = FileTools.StreamToByteArray(file);

            Crypt.Decrypt(buffer);

            structCount = BitConverter.ToInt32(buffer, 4);
            fileCount = BitConverter.ToInt32(buffer, 8);
            stringCount = BitConverter.ToInt32(buffer, 16);
            characterCount = BitConverter.ToInt32(buffer, 20);

            stringDataOffset = 24;
            stringLengthOffset = stringDataOffset + characterCount + sizeof(UInt32);
            fileDataOffset = stringLengthOffset + (stringStructLength * stringCount) + sizeof(UInt32);

            InitializeStringTable();
            InitializeFileTable();

            CheckForModifications();
        }

        void CheckForModifications()
        {
            foreach (string str in stringTable)
            {
                if (str.Contains(affix))
                {
                    modified = true;
                    break;
                }
            }
        }

        void InitializeStringTable()
        {
            stringTable = new String[stringCount];
            stringTableUnknowns = new Int32[stringCount];

            int stringByteOffset = 24;
            short stringLength = 0;

            for (int i = 0; i < stringTable.Length; i++)
            {
                int bufferOffset = stringLengthOffset + (i * stringStructLength);
                stringLength = BitConverter.ToInt16(buffer, bufferOffset);

                stringTable[i] = FileTools.ByteArrayToStringAnsi(buffer, stringByteOffset);
                stringTableUnknowns[i] = BitConverter.ToInt32(buffer, bufferOffset + sizeof(Int16));

                stringByteOffset += stringLength + 1;
            }
        }

        void InitializeFileTable()
        {
            fileTable = new FileIndex[fileCount];

            for (int i = 0; i < fileCount; i++)
            {
                FileIndex fileIndex = new FileIndex();
                fileIndex.FileStruct = (FileIndex.FileIndexStruct)FileTools.ByteArrayToStructure(buffer, typeof(FileIndex.FileIndexStruct),
                                                                                                        fileDataOffset + i * fileStructLength);
                fileIndex.DirectoryString = stringTable[fileIndex.Directory];
                fileIndex.FileNameString = stringTable[fileIndex.FileName];

                fileTable[i] = fileIndex;
            }
        }

        public int Locate(String fileName)
        {
            for (int i = 0; i < fileTable.Length; i++)
            {
                int result = String.Compare(fileName, fileTable[i].FileNameString, true);
                if (result == 0)
                {
                    return i;
                }
            }
            return -1;
        }

        public bool OpenAccompanyingDat()
        {
            if (datFile == null)
            {
                try
                {
                    datFile = new FileStream(this.FileDirectory + this.FileName + ".dat", FileMode.Open);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        public byte[] ReadDataFile(Index.FileIndex file)
        {
            if (OpenAccompanyingDat() == false)
            {
                return null;
            }

            int result = -1;
            byte[] destBuffer = new byte[file.UncompressedSize];
            datFile.Seek(file.DataOffset, SeekOrigin.Begin);

            if (file.CompressedSize > 0)
            {
                byte[] srcBuffer = new byte[file.CompressedSize];
                
                datFile.Read(srcBuffer, 0, srcBuffer.Length);
                if (IntPtr.Size == 4)
                {
                    uint len = (uint)file.UncompressedSize;
                    result = uncompress(destBuffer, ref len, srcBuffer, (uint)file.CompressedSize);
                }
                else
                {
                    ulong len = (uint)file.UncompressedSize;
                    result = uncompress(destBuffer, ref len, srcBuffer, (uint)file.CompressedSize);
                }

                if (result != 0)
                {
                    return null;
                }
            }
            else
            {
                result = datFile.Read(destBuffer, 0, file.UncompressedSize);

                if (result != file.UncompressedSize)
                {
                    return null;
                }
            }

            return destBuffer;
        }

        public byte[] GenerateIndexFile()
        {
            byte[] buffer = new byte[1024];
            int offset = 0;

            // main header
            FileTools.WriteToBuffer(ref buffer, ref offset, Token.head);
            FileTools.WriteToBuffer(ref buffer, ref offset, (Int32)4);
            FileTools.WriteToBuffer(ref buffer, ref offset, this.fileCount);

            // string block
            FileTools.WriteToBuffer(ref buffer, ref offset, Token.sect);
            FileTools.WriteToBuffer(ref buffer, ref offset, this.stringCount);
            int stringByteCountOffset = offset;
            offset += 4;
            foreach (String str in this.stringTable)
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, FileTools.StringToASCIIByteArray(str));
                offset++; // \0
            }
            FileTools.WriteToBuffer(ref buffer, stringByteCountOffset, (UInt32)(offset - stringByteCountOffset - sizeof(UInt32)));

            // string data
            FileTools.WriteToBuffer(ref buffer, ref offset, Token.sect);
            int i = 0;
            foreach (String str in this.stringTable)
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, (Int16)str.Length);
                offset += 4; // unknown  -  not required
                //FileTools.WriteToBuffer(ref buffer, ref offset, stringTableUnknowns[i]);
                i++;
            }

            // file block
            FileTools.WriteToBuffer(ref buffer, ref offset, Token.sect);
            i = 0;
            foreach (FileIndex fileIndex in this.fileTable)
            {
                // this looks gross, but is just for testing
                // final version will be similar to reading - dumping struct using MarshalAs
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.info);
                //offset += 4; // unknown  -  not required
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.FileStruct.unknown1_1);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.FileStruct.unknown1_2); // game freezes if not correct value
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.DataOffset);
                offset += 4; // null
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.UncompressedSize);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.CompressedSize);
                offset += 4; // null
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.Directory);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.FileName);
                //FileTools.WriteToBuffer(ref buffer, ref offset, (UInt32)1); // game clears .idx and .dat if null
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.FileStruct.unknown2_1);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.FileStruct.unknown2_2);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.FileStruct.unknown2_3);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.FileStruct.unknown2_4);
                //offset += 12; // unknown  -  not required
                offset += 12; // null
                //offset += 8; // first 8 bytes  -  not required
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.FileStruct.first4BytesOfFile);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.FileStruct.second4BytesOfFile);
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.info);
                i++;
            }

            byte[] returnBuffer = new byte[offset];
            Buffer.BlockCopy(buffer, 0, returnBuffer, 0, returnBuffer.Length);
            return returnBuffer;
        }

        public void AppendDirectorySuffix(int i)
        {
            string dir = affix + stringTable[fileTable[i].Directory];
            int index = StringExists(dir);
            // If the directory doesn't exist, add it.
            if (index == -1)
            {
                index = stringTable.Length;
                string[] buffer = new string[index + 1];
                stringTable.CopyTo(buffer, 0);
                buffer[index] = affix + stringTable[fileTable[i].Directory];
                stringTable = buffer;
                stringCount++;
            }
            fileTable[i].Directory = index;
            modified = true;
        }

        public void RemoveDirectorySuffix(int i)
        {
            string dir = fileTable[i].DirectoryString.Remove(0, affix.Length);
            fileTable[i].Directory = StringExists(dir);
        }

        public int StringExists(string s)
        {
            for (int i = 0; i < stringTable.Length; i++ )
                if (stringTable[i] == s)
                    return i;

            return -1;
        }

        public bool Restore()
        {
            for (int i = 0; i < FileTable.Length; i++)
            {
                if (FileTable[i].DirectoryString.Contains(affix))
                {
                    string original = FileTable[i].DirectoryString.Remove(0, affix.Length);
                    for (int j = 0; j < stringTable.Length; j++)
                    {
                        if (stringTable[j] == original)
                        {
                            FileTable[i].Directory = j;
                            break;
                        }
                    }
                }
            }

            byte[] buffer = this.GenerateIndexFile();
            Crypt.Encrypt(buffer);

            indexFile.Dispose();

            try
            {
                FileStream fs = new FileStream(this.FileDirectory + "\\" + this.FileName + ".idx", FileMode.OpenOrCreate);
                fs.Flush();
                fs.Write(buffer, 0, buffer.Length);
                fs.Close();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
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
