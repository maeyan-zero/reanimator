using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

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
         *      unknown                         Int32                                   // CRC perhaps?
         * }
         * 
         ***** File Block *****
         * token                                Int32                                   // Must be 0x68677073 ('spgh').
         * for (fileCount)
         * {
         *      token                           Int32                                   // Must be 0x6867696F ('oigh').
         *      unknown                         Int32
         *      unknown                         Int32
         *      dataOffset                      Int32                                   // Offset in bytes within accompanying .dat file.
         *      null                            Int32                                   // Always 0x00?
         *      uncompressedSize                Int32
         *      compressedSize                  Int32
         *      null                            Int32                                   // Always 0x00?
         *      directoryArrayPosition          Int32
         *      filenameArrayPosition           Int32
         *      unknown                         16 Bytes
         *      null                            12 Bytes
         *      startOfFile                     8 Bytes                                 // Appears to be the first 8 bytes of said file.
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
        FileIndex[] fileTable;

        public class FileIndex
        {
            int offset;
            public int Offset { get; set; }/*
            {
                set { offset = value; }
                get { return BitConverter.ToInt32(buffer, offset); }
            }*/

            int uncompressedSize;
            public int UncompressedSize { get; set; }/*
            {
                set { uncompressedSize = value; }
                get { return BitConverter.ToInt32(buffer, uncompressedSize); }
            }*/

            int compressedSize;
            public int CompressedSize { get; set; }/*
            {
                set { compressedSize = value; }
                get { return BitConverter.ToInt32(buffer, compressedSize); }
            }*/

            int directory;
            public int Directory { get; set; }/*
            {
                set { directory = value; }
            }*/
            public string DirectoryString { get; set; }/*
            {
                get { return stringTable[BitConverter.ToInt32(buffer, directory)]; }
            }*/

            int filename;
            public int Filename { get; set; } /*
            {
                set { filename = value; }
            }*/
            public string FilenameString { get; set; } /*
            {
                get { return stringTable[BitConverter.ToInt32(buffer, filename)]; }
            }*/
        }

        public Index(FileStream file)
        {
            indexFile = file;
            buffer = FileTools.StreamToByteArray(file);

            Crypt.Decrypt(buffer);

           // just ignore me, I was curious
           // FileStream fOut = new FileStream("out.idx", FileMode.Create);
            //fOut.Write(buffer, 0, buffer.Length);

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
            stringTable = new string[stringCount];

            int position = 24;
            short stringLength;
            char[] characterArray;

            for (int i = 0; i < stringTable.Length; i++)
            {
                int offset = stringLengthOffset + (i * stringStructLength);
                stringLength = BitConverter.ToInt16(buffer, offset);
                int unknownValue = BitConverter.ToInt32(buffer, offset + sizeof(Int16));

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
                fileTable[i] = new FileIndex();
                fileTable[i].Offset = fileDataOffset + (i * fileStructLength) + 12;
                fileTable[i].UncompressedSize = fileDataOffset + (i * fileStructLength) + 20;
                fileTable[i].CompressedSize = fileDataOffset + (i * fileStructLength) + 24;
                fileTable[i].Directory = fileDataOffset + (i * fileStructLength) + 32;
                fileTable[i].Filename = fileDataOffset + (i * fileStructLength) + 36;
                fileTable[i].DirectoryString = stringTable[BitConverter.ToInt32(buffer, fileTable[i].Directory)];
                fileTable[i].FilenameString = stringTable[BitConverter.ToInt32(buffer, fileTable[i].Filename)];
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

            int result = -1;
            byte[] destBuffer = new byte[file.UncompressedSize];
            byte[] srcBuffer = new byte[file.CompressedSize];
            datFile.Seek(file.Offset, SeekOrigin.Begin);
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

            return destBuffer;
        }

        public byte[] GenerateIndexFile()
        {
            return null;
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
            FileTools.WriteToBuffer(ref buffer, stringByteCountOffset, (UInt32)offset - stringByteCountOffset);


            // string data
            FileTools.WriteToBuffer(ref buffer, ref offset, Token.sect);
            foreach (String str in this.stringTable)
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, (Int16)str.Length);
                offset += 4; // unknown - leaving as null for now
            }


            // file block

            return null;
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
