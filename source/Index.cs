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

        FileStream indexFile;
        FileStream datFile;
        byte[] buffer;
        string[] stringTable;
        Int32[] stringTableUnknowns;
        FileIndex[] fileTable;

        const string affix = "backup\\"; // used for modifications

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
            }
            public string DirectoryString { get; set; }

            [Browsable(false)]
            public int FileName
            {
                get { return FileStruct.filenameArrayPosition; }
            }
            public string FileNameString { get; set; }
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
                if (fileName.ToLower().Contains(fileTable[i].FileNameString))
                {
                    return i;
                }
            }
            return -1;
        }

        public void SetFileTable(FileIndex[] fileIndex)
        {
            fileTable = fileIndex;
        }

        public FileIndex[] GetFileTable()
        {
            return fileTable;
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
                offset += 4; // unknown  -  not required
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.FileStruct.unknown1_2); // game freezes if not correct value
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.DataOffset);
                offset += 4; // null
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.UncompressedSize);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.CompressedSize);
                offset += 4; // null
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.Directory);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.FileName);
                FileTools.WriteToBuffer(ref buffer, ref offset, (UInt32)1); // game clears .idx and .dat if null
                offset += 12; // unknown  -  not required
                offset += 12; // null
                offset += 8; // first 8 bytes  -  not required
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.info);

                i++;
            }

            byte[] returnBuffer = new byte[offset];
            Buffer.BlockCopy(buffer, 0, returnBuffer, 0, returnBuffer.Length);
            return returnBuffer;
        }

        public bool ApplyDirectoryAffix(int id)
        {
            Index.FileIndex[] fileIndex = this.GetFileTable();

            if (fileIndex[id].DirectoryString.Contains(affix))
            {
                return false;
            }
            else
            {
                fileIndex[id].DirectoryString = fileIndex[id].DirectoryString.Insert(0, affix);
            }

            this.SetFileTable(fileIndex);

            return true;
        }

        public bool IsModified()
        {
            Index.FileIndex[] fileIndex = this.GetFileTable();

            foreach (Index.FileIndex file in fileIndex)
            {
                if (file.DirectoryString.Contains(affix))
                {
                    return true;
                }
            }

            return false;
        }

        public bool RestoreIndex(string path)
        {
            Index.FileIndex[] fileIndex = this.GetFileTable();

            foreach (Index.FileIndex file in fileIndex)
            {
                if (file.DirectoryString.Contains(affix))
                {
                    file.DirectoryString = file.DirectoryString.Remove(0, affix.Length);
                }
            }
            this.SetFileTable(fileIndex);

            byte[] buffer = this.GenerateIndexFile();
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
