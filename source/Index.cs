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

        static readonly string affix = "backup\\";

        struct Token
        {
            public static readonly UInt32 head = 0x6867696E; // 'nigh'
            public static readonly UInt32 sect = 0x68677073; // 'spgh'
            public static readonly UInt32 info = 0x6867696F; // 'oigh'
        }

        /* Index File Structure Layout
         * 
         ***** Main Header *****
         * token                                Int32                                   // Must be 0x6867696E ('nigh').
         * structCount                          Int32                                   // Number of Structs in Index(?) - count tokens.
         * fileCount                            Int32                                   // File count.
         * 
         ***** String Block *****
         * token                                Int32                                   // Must be 0x68677073 ('spgh').
         * stringCount                          Int32                                   // String count.
         * blockSize                            Int32                                   // Number of bytes in following block.
         * stringBytes                          blockSize                               // The strings (each one is \0) lumped together as one big block.
         * 
         ***** String Data ***** 
         * token                                Int32                                   // Must be 0x68677073 ('spgh').
         * for (stringCount)
         * {
         *      stringSize                      Int16                                   // Count of chars in string (not including \0).
         *      unknown                         Int32                                   // CRC perhaps?  -  Not required for valid game loading.
         * }
         * 
         ***** File Block *****
         * token                                Int32                                   // Must be 0x68677073 ('spgh').
         * for (fileCount)
         * {
         *      token                           Int32                                   // Must be 0x6867696F ('oigh').
         *      unknown                         Int32                                   // Not required for valid game loading (can be null).
         *      unknown                         Int32                                   // REQUIRED for valid game loading! // Must be a specific value... What?
         *      dataOffset                      Int32                                   // Offset in bytes within accompanying .dat file.
         *      null                            Int32
         *      uncompressedSize                Int32
         *      compressedSize                  Int32
         *      null                            Int32
         *      directoryArrayPosition          Int32
         *      filenameArrayPosition           Int32
         *      unknown                         Int32                                   // REQUIRED for valid game loading! // Game clears .idx and .dat if null (can be anything but null).
         *      unknown                         Int32                                   // Not required for valid game loading (can be null).
         *      unknown                         Int32                                   // Not required for valid game loading (can be null).
         *      unknown                         Int32                                   // Not required for valid game loading (can be null).
         *      null                            Int32
         *      null                            Int32
         *      null                            Int32
         *      startOfFile                     8 Bytes                                 // First 8 bytes of said file.  -  Not required for valid game loading (can be null).
         *      token                           Int32                                   // Must be 0x6867696F ('oigh').
         * }
         */

        // Quick overview of a typical index file. This is the movies_low.idx
        //0000 0003		110, 105, 103, 104	// Start File Token
        //0004 0007		4					// Number of Structs in Index(?) - count tokens.
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

           // just ignore me, I was curious
            FileStream fOut = new FileStream("out.idx", FileMode.Create);
            fOut.Write(buffer, 0, buffer.Length);
            fOut.Dispose();

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

                /*
                // crc test
                if (this.DatFileOpen)
                {
                    byte[] buff = this.ReadDataFile(fileIndex);
                    uint crc = Crc32.Compute(buff);

                }
                else
                {
                    OpenAccompanyingDat();
                }*/
            }
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
