using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using Revival;

namespace Hellgate
{
    public class IndexFile
    {
        #region ZLibImports
        /* Decompresses the source buffer into the destination buffer.  sourceLen is
           the byte length of the source buffer. Upon entry, destLen is the total
           size of the destination buffer, which must be large enough to hold the
           entire uncompressed data. (The size of the uncompressed data must have
           been saved previously by the compressor and transmitted to the decompressor
           by some mechanism outside the scope of this compression library.)
           Upon exit, destLen is the actual size of the compressed buffer.
             This function can be used to decompress a whole file at once if the
           input file is mmap'ed.

             uncompress returns Z_OK if success, Z_MEM_ERROR if there was not
           enough memory, Z_BUF_ERROR if there was not enough room in the output
           buffer, or Z_DATA_ERROR if the input data was corrupted or incomplete.
        */

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


        /* Compresses the source buffer into the destination buffer.  sourceLen is
           the byte length of the source buffer. Upon entry, destLen is the total
           size of the destination buffer, which must be at least the value returned
           by compressBound(sourceLen). Upon exit, destLen is the actual size of the
           compressed buffer.
             This function can be used to compress a whole file at once if the
           input file is mmap'ed.
             compress returns Z_OK if success, Z_MEM_ERROR if there was not
           enough memory, Z_BUF_ERROR if there was not enough room in the output
           buffer.
        */

        [DllImport("zlibwapi86.dll")]
        private static extern int compress
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
        private static extern int compress
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
        #endregion

        #region IndexClassTypes
        private class Token
        {
            public const UInt32 Head = 0x6867696E; // 'nigh'
            public const UInt32 Sect = 0x68677073; // 'spgh'
            public const UInt32 Info = 0x6867696F; // 'oigh'
            public const UInt32 Data = 0x68676461; // 'adgh'
        }

        #region Dat Header Bytes
        private static Byte[] DatHeader = new Byte[]
        {
            0x61, 0x64, 0x67, 0x68, 0x04, 0x00, 0x00, 0x00,
            0x01, 0x00, 0x00, 0x00, 0x0A, 0x00, 0x00, 0x00,
            0xB4, 0x00, 0x00, 0x00, 0x58, 0x0D, 0x00, 0x00,
            0x78, 0x61, 0x33, 0x37, 0x64, 0x64, 0x34, 0x35,
            0x66, 0x66, 0x65, 0x31, 0x30, 0x30, 0x62, 0x66,
            0x66, 0x66, 0x63, 0x63, 0x39, 0x37, 0x35, 0x33,
            0x61, 0x61, 0x62, 0x61, 0x63, 0x33, 0x32, 0x35,
            0x66, 0x30, 0x37, 0x63, 0x62, 0x33, 0x66, 0x61,
            0x32, 0x33, 0x31, 0x31, 0x34, 0x34, 0x66, 0x65,
            0x32, 0x65, 0x33, 0x33, 0x61, 0x65, 0x34, 0x37,
            0x38, 0x33, 0x66, 0x65, 0x65, 0x61, 0x64, 0x32,
            0x62, 0x38, 0x61, 0x37, 0x33, 0x66, 0x66, 0x30,
            0x32, 0x31, 0x66, 0x61, 0x63, 0x33, 0x32, 0x36,
            0x64, 0x66, 0x30, 0x65, 0x66, 0x39, 0x37, 0x35,
            0x33, 0x61, 0x62, 0x39, 0x63, 0x64, 0x66, 0x36,
            0x35, 0x37, 0x33, 0x64, 0x64, 0x66, 0x66, 0x30,
            0x33, 0x31, 0x32, 0x66, 0x61, 0x62, 0x30, 0x62,
            0x30, 0x66, 0x66, 0x33, 0x39, 0x37, 0x37, 0x39,
            0x65, 0x61, 0x66, 0x66, 0x33, 0x31, 0x32, 0x61,
            0x34, 0x66, 0x35, 0x64, 0x65, 0x36, 0x35, 0x38,
            0x39, 0x32, 0x66, 0x66, 0x65, 0x65, 0x33, 0x33,
            0x61, 0x34, 0x34, 0x35, 0x36, 0x39, 0x62, 0x65,
            0x62, 0x66, 0x32, 0x31, 0x66, 0x36, 0x36, 0x64,
            0x32, 0x32, 0x65, 0x35, 0x34, 0x61, 0x32, 0x32,
            0x33, 0x34, 0x37, 0x65, 0x66, 0x64, 0x33, 0x37,
            0x35, 0x39, 0x38, 0x31, 0x31, 0x38, 0x38, 0x37,
            0x34, 0x33, 0x61, 0x66, 0x64, 0x39, 0x39, 0x62,
            0x61, 0x61, 0x63, 0x63, 0x33, 0x34, 0x32, 0x64,
            0x38, 0x38, 0x61, 0x39, 0x39, 0x33, 0x32, 0x31,
            0x32, 0x33, 0x35, 0x37, 0x39, 0x38, 0x37, 0x32,
            0x35, 0x66, 0x65, 0x64, 0x63, 0x62, 0x66, 0x34,
            0x33, 0x32, 0x35, 0x32, 0x36, 0x36, 0x39, 0x64,
            0x61, 0x64, 0x65, 0x33, 0x32, 0x34, 0x31, 0x35,
            0x66, 0x65, 0x65, 0x38, 0x39, 0x64, 0x61, 0x35,
            0x34, 0x33, 0x62, 0x66, 0x32, 0x33, 0x64, 0x34,
            0x65, 0x78, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 
        };
        #endregion

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private class IndexHeader
        {
            public UInt32 Token;
            public UInt32 Version;
            public Int32 FileCount;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private class DataHeader
        {
            public UInt32 DatHead = 0x68676461;
            public Int32 Version = 0x04;
            public Int32 Unknown1 = 0x01;
            public Int32 Unknown2 = 0x00;
            public Int32 Unknown3 = 0x00;
            public Int32 Unknown4 = 0x00001154;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 488)]
            public byte[] HashKeys;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private class IndexStringsHeader
        {
            public UInt32 Token;
            public Int32 Count;
            public Int32 Length;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private class IndexStringStruct
        {
            public Int16 Length;
            public UInt32 Unknown;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class FileStructure
        {
            public UInt32 StartToken;
            public UInt32 FolderPathHash;
            public UInt32 FileNameHash;
            public Int32 DataOffset;
            public Int32 Null1;
            public Int32 UncompressedSize;
            public Int32 CompressedSize;
            public Int32 Null2;
            public Int32 DirectoryArrayIndex;
            public Int32 FilenameArrayIndex;
            public Int64 FileTime;
            public Int32 Unknown23;
            public Int32 Unknown24;
            public Int32 Null31;
            public Int32 Null32;
            public Int32 Null33;
            public Int32 First4BytesOfFile;
            public Int32 Second4BytesOfFile;
            public UInt32 EndToken;
        }

        public class FileDefinition : IComparable, IEquatable<FileDefinition>
        {
            public String FileName { get; set; }
            public String Directory { get; set; }
            public FileStructure FileStruct { get; private set; }
            public UInt64 Hash
            {
                get
                {
                    UInt64 directoryHash = (UInt64)Crypt.GetHash(Directory);
                    UInt64 fileNameHash = (UInt64)Crypt.GetHash(FileName);
                    return directoryHash + (fileNameHash << 32);
                }
            }

            public FileDefinition()
            {
                if (FileStruct == null)
                {
                    FileStruct = new FileStructure();
                    FileStruct.FileNameHash = Crypt.GetHash(FileName);
                    FileStruct.FolderPathHash = Crypt.GetHash(Directory);
                }
            }

            public FileDefinition(FileStructure fileStruct)
            {
                FileStruct = fileStruct;
            }

            public int CompareTo(Object obj)
            {
                FileDefinition otherFile = obj as FileDefinition;
                if (otherFile == null) return -1;

                if (String.IsNullOrEmpty(FileName)) return -1;
                if (String.IsNullOrEmpty(Directory)) return -1;

                int result;

                result = FileStruct.FileNameHash.CompareTo(otherFile.FileStruct.FileNameHash);
                if (result != 0) return result;

                result = FileStruct.FolderPathHash.CompareTo(otherFile.FileStruct.FolderPathHash);
                return result;
            }

            public bool Equals(FileDefinition otherFile)
            {
                if (String.IsNullOrEmpty(FileName)) return false;
                if (String.IsNullOrEmpty(Directory)) return false;

                return (FileStruct.FileNameHash == otherFile.FileStruct.FileNameHash &&
                        FileStruct.FolderPathHash == otherFile.FileStruct.FolderPathHash);
            }
        }
        #endregion

        #region Members
        private Int32 Version { get { return 4; } }
        private List<String> StringsList { get; set; }
        private List<FileStructure> FileStructureList { get; set; }
        private FileStream DataFile { get; set; }

        public String FilePath { get; set; }
        public String DataFilePath { get { return FilePath.Replace(".idx", ".dat"); } }
        public List<FileDefinition> FileList { get; private set; }
        public Boolean IntegrityCheck { get; private set; }
        #endregion

        public IndexFile() { }

        public IndexFile(byte[] buffer)
        {
            IntegrityCheck = Read(buffer);
        }

        public bool OpenStream()
        {
            if (!File.Exists(DataFilePath)) return false;
            try
            {
                DataFile = new FileStream(@DataFilePath, FileMode.Open);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool CloseStream()
        {
            if (DataFile == null) return false;
            DataFile.Close();

            return true;
        }

        public bool Read(byte[] buffer)
        {
            if (buffer == null) return false;
            if (buffer.Length == 0) return false;

            Crypt.Decrypt(buffer);

            int offset = 0;
            IndexHeader Header = Tools.ByteArrayToStructure<IndexHeader>(buffer, ref offset);
            if (Header.Token != Token.Head) return false;
            if (Header.Version != Version) return false;

            IndexStringsHeader StringsHeader = Tools.ByteArrayToStructure<IndexStringsHeader>(buffer, ref offset);
            if (StringsHeader.Token != Token.Sect) return false;

            StringsList = new List<String>();
            for (int i = 0; i < StringsHeader.Count; i++)
            {
                StringsList.Add(Tools.ByteArrayToStringASCII(buffer, offset));
                offset += StringsList[i].Length + 1; // \0
            }

            if (Tools.ByteArrayToUInt32(buffer, ref offset) != Token.Sect) return false;
            IndexStringStruct[] stringStruct = Tools.ByteArrayToArray<IndexStringStruct>(buffer, ref offset, StringsHeader.Count);

            if (Tools.ByteArrayToUInt32(buffer, ref offset) != Token.Sect) return false;
            FileStructureList = new List<FileStructure>(Tools.ByteArrayToArray<FileStructure>(buffer, ref offset, Header.FileCount));

            FileList = new List<FileDefinition>();
            foreach (FileStructure fileStruct in FileStructureList)
            {
                FileDefinition fileEntry = new FileDefinition(fileStruct)
                {
                    FileName = StringsList[fileStruct.FilenameArrayIndex],
                    Directory = StringsList[fileStruct.DirectoryArrayIndex]
                };
                FileList.Add(fileEntry);
            }
            FileList.Sort();

            return true;
        }

        public byte[] Create()
        {
            byte[] buffer = new byte[1024];
            int offset = 0;


            // Index file header
            Tools.WriteToBuffer(ref buffer, ref offset, Token.Head);
            Tools.WriteToBuffer(ref buffer, ref offset, Version);
            Tools.WriteToBuffer(ref buffer, ref offset, FileList.Count);

            // Strings header
            Tools.WriteToBuffer(ref buffer, ref offset, Token.Sect);
            Tools.WriteToBuffer(ref buffer, ref offset, StringsList.Count);

            // String byte array
            int stringByteCountOffset = offset;
            offset += 4;
            foreach (String str in StringsList)
            {
                Tools.WriteToBuffer(ref buffer, ref offset, Tools.StringToASCIIByteArray(str));
                offset++; // \0
            }
            int arraySize = offset - stringByteCountOffset - sizeof(Int32);
            Tools.WriteToBuffer(ref buffer, stringByteCountOffset, arraySize);

            // String struct
            Tools.WriteToBuffer(ref buffer, ref offset, Token.Sect);
            foreach (String str in StringsList)
            {
                Tools.WriteToBuffer(ref buffer, ref offset, (Int16)str.Length);
                offset += 4; // null this value, game doesn't need it
            }

            // File block
            Tools.WriteToBuffer(ref buffer, ref offset, Token.Sect);
            foreach (FileStructure fileStruct in FileStructureList)
            {
                byte[] fileIndexBytes = Tools.StructureToByteArray(fileStruct);
                Tools.WriteToBuffer(ref buffer, ref offset, fileIndexBytes);
            }


            Crypt.Encrypt(buffer);


            // Correct the buffer size and return
            byte[] returnBuffer = new byte[offset];
            Buffer.BlockCopy(buffer, 0, returnBuffer, 0, returnBuffer.Length);

            return returnBuffer;
        }

        public bool Save()
        {
            byte[] buffer = Create();

            try
            {
                File.WriteAllBytes(FilePath, buffer);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool CreateDatFile(bool overrideExiting)
        {
            if ((!(File.Exists(DataFilePath)) || (overrideExiting)))
            {
                try
                {
                    File.WriteAllBytes(DataFilePath, DatHeader);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool CreateDatFile()
        {
            return CreateDatFile(true);
        }

        public bool Add(string directory, string fileName, byte[] bytesToWrite, bool doCompress)
        {
            if (bytesToWrite == null) return false;
            if (bytesToWrite.Length <= 0) return false;

            // Check Data Stream is available.
            if ((DataFile == null))
            {
                if (!(File.Exists(DataFilePath)))
                    if (!(CreateDatFile())) return false;
                if (!(OpenStream())) return false;
            }

            // Create new Index File entry
            byte[] fileBuffer = bytesToWrite;
            if (FileStructureList == null) FileStructureList = new List<FileStructure>();
            FileStructure fileStruct = new FileStructure()
            {
                CompressedSize = 0,
                UncompressedSize = fileBuffer.Length,
                FileTime = DateTime.Now.ToFileTime(),
                DataOffset = (int)DataFile.Length,
                FileNameHash = Crypt.GetHash(fileName),
                FolderPathHash = Crypt.GetHash(directory),
                StartToken = Token.Info,
                EndToken = Token.Info
            };

            // See if the index already contains the FileName string
            if (StringsList == null) StringsList = new List<String>();
            int fileNameIndex = StringsList.IndexOf(fileName);
            if (fileNameIndex != -1)
            {
                fileStruct.FilenameArrayIndex = fileNameIndex;
            }
            // otherwise add it
            else
            {
                StringsList.Add(fileName);
                fileStruct.FilenameArrayIndex = StringsList.Count - 1;
            }

            // See if the index already contains the Directory string
            int directoryIndex = StringsList.IndexOf(directory);
            if (directoryIndex != -1)
            {
                fileStruct.DirectoryArrayIndex = directoryIndex;
            }
            // otherwise add it
            else
            {
                StringsList.Add(directory);
                fileStruct.DirectoryArrayIndex = StringsList.Count - 1;
            }

            // Compress the buffer if toggled.
            if (doCompress == true)
            {
                if (IntPtr.Size == 4) // x86
                {
                    UInt32 destinationLength = (UInt32)fileBuffer.Length;
                    compress(fileBuffer, ref destinationLength, fileBuffer, (UInt32)fileBuffer.Length);
                    fileStruct.CompressedSize = (int)destinationLength;
                }
                else // x64
                {
                    UInt64 destinationLength = (UInt64)fileBuffer.Length;
                    compress(fileBuffer, ref destinationLength, fileBuffer, (UInt64)fileBuffer.Length);
                    fileStruct.CompressedSize = (int)destinationLength;
                }
            }

            // Always append to the end.
            try
            {
                DataFile.Seek(0, SeekOrigin.End);
                DataFile.Write(fileBuffer, 0, fileBuffer.Length);
            }
            catch
            {
                return false;
            }

            fileStruct.First4BytesOfFile = Tools.ByteArrayToInt32(fileBuffer, 0);
            fileStruct.Second4BytesOfFile = Tools.ByteArrayToInt32(fileBuffer, 4); 
            FileStructureList.Add(fileStruct);


            // Create an entry in the File Index
            FileDefinition fileEntry = new FileDefinition(fileStruct);

            // check if it exists, if so replace
            if (FileList == null) FileList = new List<FileDefinition>();

            int index = FileList.IndexOf(fileEntry);
            if (index != -1) FileList[index] = fileEntry;
            else FileList.Add(fileEntry);

            return true;
        }

        public byte[] GetFileBytes(string directory, string fileName)
        {       
            IEnumerable<FileDefinition> entryVar =
                FileList.Where(f => f.Directory == directory && f.FileName == fileName);
            if (entryVar.Count() == 0) return null;

            FileStructure file = entryVar.First().FileStruct;
            return GetFileBytes(file);
        }

        public byte[] GetFileBytes(FileStructure fileStructure)
        {
            // Check the data file is available
            if (DataFile == null)
                // If not try open it, otherwise fail.
                if (!(OpenStream()))
                    return null;


            Byte[] buffer;
            
            // file not compressed
            if (fileStructure.CompressedSize == 0)
            {
                buffer = new byte[fileStructure.UncompressedSize];
                DataFile.Position = fileStructure.DataOffset;
                DataFile.Read(buffer, 0, fileStructure.UncompressedSize);
            }
            else // file is compressed
            {
                byte[] compressedBuffer = new byte[fileStructure.CompressedSize];
                DataFile.Position = fileStructure.DataOffset;
                DataFile.Read(compressedBuffer, 0, fileStructure.CompressedSize);
                buffer = new byte[fileStructure.UncompressedSize];

                int result;
                if (IntPtr.Size == 4) //x86
                {
                    uint len = (uint)fileStructure.UncompressedSize;
                    result = uncompress(buffer, ref len, compressedBuffer, (uint)fileStructure.CompressedSize);
                }
                else //x64
                {
                    ulong len = (ulong)fileStructure.UncompressedSize;
                    result = uncompress(buffer, ref len, compressedBuffer, (uint)fileStructure.CompressedSize);
                }
            }

            return buffer;
        }

        public FileDefinition GetFileDefinition(UInt64 definitionHash)
        {
            var match = from file in FileList
                        where file.Hash == definitionHash
                        select file;

            return match.Count() > 0 ? (FileDefinition)match.First() : null;
        }
    }
}
