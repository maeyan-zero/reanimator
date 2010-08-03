using System;
using System.IO;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Windows.Forms;

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

        /*
             Decompresses the source buffer into the destination buffer.  sourceLen is
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

        /*
         Compresses the source buffer into the destination buffer.  sourceLen is
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

        public const int Base000 = 9;
        public const int LatestPatch = 10;
        public const int LatestPatchLocalized = 11;
        public const int ExcelTablesIndex = 2572;
        static public readonly string[] FileNames = { "hellgate_bghigh000",                                     // 0
                                                     "hellgate_graphicshigh000",                                // 1
                                                     "hellgate_localized000",                                   // 2
                                                     "hellgate_movies000",                                      // 3
                                                     "hellgate_movieshigh000",                                  // 4
                                                     "hellgate_movieslow000",                                   // 5
                                                     "hellgate_playershigh000",                                 // 6
                                                     "hellgate_sound000",                                       // 7
                                                     "hellgate_soundmusic000",                                  // 8
                                                     "hellgate000",                                             // 9
                                                     "sp_hellgate_1.10.180.3416_1.18074.70.4256",               // 10
                                                     "sp_hellgate_localized_1.10.180.3416_1.18074.70.4256" };   // 11
        struct Token
        {
            public const UInt32 Head = 0x6867696E; // 'nigh'
            public const UInt32 Sect = 0x68677073; // 'spgh'
            public const UInt32 Info = 0x6867696F; // 'oigh'
        }

        Int32 _structCount;                     // offset 4
        readonly Int32 _fileCount;              // offset 8
        Int32 _stringCount;                     // offset 16
        readonly Int32 _characterCount;         // offset 20

        readonly int _stringDataOffset;
        readonly int _stringLengthOffset;
        readonly int _fileDataOffset;

        const int StringStructLength = 6;
        const int FileStructLength = 80;

        readonly byte[] _buffer;
        string[] _stringTable;
        Int32[] _stringTableUnknowns;

        const string Affix = "backup\\";

        FileStream _indexFile;

        public FileStream DataFile { get; set; }

        public void WriteIndex()
        {
            byte[] buffer = this.GenerateIndexFile();
            Crypt.Encrypt(buffer);
            _indexFile.Seek(0, SeekOrigin.Begin);
            _indexFile.Write(buffer, 0, buffer.Length);
        }



        public class FileIndex
        {
            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public class FileIndexStruct
            {
                public UInt32 StartToken;
                public Int32 Unknown11;                     // 0    0x00
                public Int32 Unknown12;                     // 4    0x04
                public Int32 DataOffset;                    // 8    0x08
                public Int32 Null1;                         // 12   0x0C
                public Int32 UncompressedSize;              // 16   0x10
                public Int32 CompressedSize;                // 20   0x14
                public Int32 Null2;                         // 24   0x18
                public Int32 DirectoryArrayPosition;        // 28   0x1C
                public Int32 FilenameArrayPosition;         // 32   0x20
                public Int32 Unknown21;                     // 36   0x24            Can't be null             .text:000000014004958B cmp     qword ptr [rdi+10h], 0 -> jz      loc_140049402   ; Jump if Zero
                public Int32 Unknown22;                     // 40   0x28
                public Int32 Unknown23;                     // 44   0x2C
                public Int32 Unknown24;                     // 48   0x30
                public Int32 Null31;                        // 52   0x34
                public Int32 Null32;                        // 56   0x38
                public Int32 Null33;                        // 60   0x3C
                public Int32 First4BytesOfFile;             // 64   0x40
                public Int32 Second4BytesOfFile;            // 68   0x44
                public UInt32 EndToken;
            };

            [Browsable(false)]
            public FileIndexStruct FileStruct { get; set; }

            public int DataOffset
            {
                get { return FileStruct.DataOffset; }
                set { FileStruct.DataOffset = value; }
            }

            public int UncompressedSize
            {
                get { return FileStruct.UncompressedSize; }
                set { FileStruct.UncompressedSize = value; }
            }
            public int CompressedSize
            {
                get { return FileStruct.CompressedSize; }
                set { FileStruct.CompressedSize = value; }
            }

            [Browsable(false)]
            public int Directory
            {
                get { return FileStruct.DirectoryArrayPosition; }
                set { FileStruct.DirectoryArrayPosition = value; }
            }
            public string DirectoryString { get; set; }

            [Browsable(false)]
            public int FileName
            {
                get { return FileStruct.FilenameArrayPosition; }
            }
            public string FileNameString { get; set; }

            public bool Modified
            {
                get
                {
                    return DirectoryString.Contains(Affix);
                }
            }
        }

        public FileIndex[] FileTable { get; private set; }

        static public Index[] LoadIndexFiles(String path)
        {
            Index[] index = new Index[FileNames.Length];
            for (int i = 0; i < index.Length; i++)
            {
                String filePath = String.Format("{0}{1}.idx", path, FileNames[i]);
                if (!File.Exists(filePath))
                {
                    MessageBox.Show("Index file not found!\n\n" + filePath, "Warning", MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    continue;
                }

                try
                {
                    index[i] = new Index(filePath);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Unknown IO Error!\n\n" + e, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            return index;
        }

        public String FileName
        {
            get
            {
                int n = _indexFile.Name.LastIndexOfAny("\\".ToCharArray()) + 1;
                return _indexFile.Name.Substring(n, _indexFile.Name.LastIndexOf('.') - n);
            }
        }

        public String FileDirectory
        {
            get { return _indexFile.Name.Substring(0, _indexFile.Name.LastIndexOfAny("\\".ToCharArray()) + 1); }
        }

        public bool DatFileOpen
        {
            get { return DataFile == null ? false : true; }
        }

        public bool Modified { get; private set; }

        public Index(String filePath)
        {
            _indexFile = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
            _buffer = FileTools.StreamToByteArray(_indexFile);
            Crypt.Decrypt(_buffer);
            _structCount = BitConverter.ToInt32(_buffer, 4);
            _fileCount = BitConverter.ToInt32(_buffer, 8);
            _stringCount = BitConverter.ToInt32(_buffer, 16);
            _characterCount = BitConverter.ToInt32(_buffer, 20);
            _stringDataOffset = 24;
            _stringLengthOffset = _stringDataOffset + _characterCount + sizeof(UInt32);
            _fileDataOffset = _stringLengthOffset + (StringStructLength * _stringCount) + sizeof(UInt32);

            InitializeStringTable();
            InitializeFileTable();
            CheckForModifications();
        }

        void CheckForModifications()
        {
            foreach (FileIndex file in FileTable)
            {
                if (!file.DirectoryString.Contains(Affix)) continue;

                Modified = true;
                break;
            }
        }

        void InitializeStringTable()
        {
            _stringTable = new String[_stringCount];
            _stringTableUnknowns = new Int32[_stringCount];

            int stringByteOffset = 24;

            for (int i = 0; i < _stringTable.Length; i++)
            {
                int bufferOffset = _stringLengthOffset + (i * StringStructLength);
                short stringLength = BitConverter.ToInt16(_buffer, bufferOffset);

                _stringTable[i] = FileTools.ByteArrayToStringAnsi(_buffer, stringByteOffset);
                _stringTableUnknowns[i] = BitConverter.ToInt32(_buffer, bufferOffset + sizeof(Int16));

                stringByteOffset += stringLength + 1;
            }
        }

        void InitializeFileTable()
        {
            FileTable = new FileIndex[_fileCount];

            for (int i = 0; i < _fileCount; i++)
            {
                FileIndex fileIndex = new FileIndex
                                          {
                                              FileStruct =
                                                  (FileIndex.FileIndexStruct)
                                                  FileTools.ByteArrayToStructure(_buffer,
                                                                                 typeof(FileIndex.FileIndexStruct),
                                                                                 _fileDataOffset + i * FileStructLength)
                                          };
                fileIndex.DirectoryString = _stringTable[fileIndex.Directory];
                fileIndex.FileNameString = _stringTable[fileIndex.FileName];

                FileTable[i] = fileIndex;
            }
        }

        public int Locate(String file, String dir)
        {
            for (int i = 0; i < FileTable.Length; i++)
            {
                int result1 = String.Compare(file, FileTable[i].FileNameString, true);
                int result2 = String.Compare(dir, FileTable[i].DirectoryString, true);
                if (result1 == 0 && result2 == 0)
                {
                    return i;
                }
            }
            return -1;
        }

        public bool OpenAccompanyingDat()
        {
            if (DataFile == null)
            {
                try
                {
                    String filePath = String.Format("{0}{1}.dat", FileDirectory, FileName);
                    if (!File.Exists(filePath)) return false;

                    DataFile = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        public byte[] ReadDataFile(FileIndex file)
        {
            if (!OpenAccompanyingDat()) return null;

            int result;
            byte[] destBuffer = new byte[file.UncompressedSize];
            DataFile.Seek(file.DataOffset, SeekOrigin.Begin);

            if (file.CompressedSize > 0)
            {
                byte[] srcBuffer = new byte[file.CompressedSize];

                DataFile.Read(srcBuffer, 0, srcBuffer.Length);
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
                result = DataFile.Read(destBuffer, 0, file.UncompressedSize);

                if (result != file.UncompressedSize)
                {
                    return null;
                }
            }

            return destBuffer;
        }

        public void AppendToDat(byte[] uncompressedBuffer, bool doCompress, FileIndex file, bool writeIndex)
        {
            // New Entry
            //FileIndex newIndex = index;
            // Move pointer to the end of the stream.
            if (!DatFileOpen)
            {
                if (!OpenAccompanyingDat())
                {
                    MessageBox.Show("Failed to open accompanying dat file!\n" + FileName, "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            DataFile.Seek(0, SeekOrigin.End);

            if (!doCompress) return;

            byte[] compressedBuffer = new byte[uncompressedBuffer.Length];

            int len;
            if (IntPtr.Size == 4) // x86
            {
                UInt32 destinationLength = (UInt32)compressedBuffer.Length;
                compress(compressedBuffer, ref destinationLength, uncompressedBuffer, (UInt32)uncompressedBuffer.Length);
                len = (int)destinationLength;
            }
            else // x64
            {
                UInt64 destinationLength = (UInt64)compressedBuffer.Length;
                compress(compressedBuffer, ref destinationLength, uncompressedBuffer, (UInt64)uncompressedBuffer.Length);
                len = (int)destinationLength;
            }

            file.CompressedSize = len;
            file.DataOffset = (int)DataFile.Position;
            file.UncompressedSize = uncompressedBuffer.Length;
            //int i = 1;
            DataFile.Write(compressedBuffer, 0, len);
            DataFile.Close();
            Modified = true;
        }

        public byte[] GenerateIndexFile()
        {
            byte[] buffer = new byte[1024];
            int offset = 0;

            // main header
            const Int32 version = 4;
            FileTools.WriteToBuffer(ref buffer, ref offset, Token.Head);
            FileTools.WriteToBuffer(ref buffer, ref offset, version);
            FileTools.WriteToBuffer(ref buffer, ref offset, _fileCount);

            // string block
            FileTools.WriteToBuffer(ref buffer, ref offset, Token.Sect);
            FileTools.WriteToBuffer(ref buffer, ref offset, _stringCount);
            int stringByteCountOffset = offset;
            offset += 4;
            foreach (String str in _stringTable)
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, FileTools.StringToASCIIByteArray(str));
                offset++; // \0
            }
            FileTools.WriteToBuffer(ref buffer, stringByteCountOffset, (UInt32)(offset - stringByteCountOffset - sizeof(UInt32)));

            // string data
            FileTools.WriteToBuffer(ref buffer, ref offset, Token.Sect);
            //int i = 0;
            foreach (String str in _stringTable)
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, (Int16)str.Length);
                offset += 4; // unknown  -  not required
                //FileTools.WriteToBuffer(ref _buffer, ref offset, _stringTableUnknowns[i]);
                //i++;
            }

            // file block
            //const UInt64 foo = 0xDEADBEEFDEADBEEF;
            FileTools.WriteToBuffer(ref buffer, ref offset, Token.Sect);
            //i = 0;
            foreach (FileIndex fileIndex in FileTable)
            {
                // this looks gross, but is just for testing
                // final version will be similar to reading - dumping struct using MarshalAs
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.Info);

                //FileTools.WriteToBuffer(ref buffer, ref offset, foo);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.FileStruct.Unknown11);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.FileStruct.Unknown12); // game freezes if not correct value
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.DataOffset);
                offset += 4; // null
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.UncompressedSize);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.CompressedSize);
                offset += 4; // null
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.Directory);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.FileName);
                //FileTools.WriteToBuffer(ref _buffer, ref offset, (UInt32)1); // game clears .idx and .dat if null
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.FileStruct.Unknown21);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.FileStruct.Unknown22);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.FileStruct.Unknown23);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.FileStruct.Unknown24);
                //offset += 12; // unknown  -  not required
                offset += 12; // null
                //offset += 8; // first 8 bytes  -  not required
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.FileStruct.First4BytesOfFile);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.FileStruct.Second4BytesOfFile);
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.Info);
                //i++;
            }

            byte[] returnBuffer = new byte[offset];
            Buffer.BlockCopy(buffer, 0, returnBuffer, 0, returnBuffer.Length);
            return returnBuffer;
        }

        public void AppendDirectorySuffix(int i)
        {
            string dir = Affix + _stringTable[FileTable[i].Directory];
            int index = StringExists(dir);
            // If the directory doesn't exist, add it.
            if (index == -1)
            {
                index = _stringTable.Length;
                string[] buffer = new string[index + 1];
                _stringTable.CopyTo(buffer, 0);
                buffer[index] = Affix + _stringTable[FileTable[i].Directory];
                _stringTable = buffer;
                _stringCount++;
            }
            FileTable[i].Directory = index;
            Modified = true;
        }

        public void RemoveDirectorySuffix(int i)
        {
            string dir = FileTable[i].DirectoryString.Remove(0, Affix.Length);
            FileTable[i].Directory = StringExists(dir);
        }

        public int StringExists(String s)
        {
            for (int i = 0; i < _stringTable.Length; i++)
                if (_stringTable[i] == s)
                    return i;

            return -1;
        }

        public bool Restore()
        {
            for (int i = 0; i < FileTable.Length; i++)
            {
                if (!FileTable[i].DirectoryString.Contains(Affix)) continue;

                string original = FileTable[i].DirectoryString.Remove(0, Affix.Length);
                for (int j = 0; j < _stringTable.Length; j++)
                {
                    if (_stringTable[j] != original) continue;

                    FileTable[i].Directory = j;
                    break;
                }
            }

            byte[] buffer = GenerateIndexFile();
            Crypt.Encrypt(buffer);

            _indexFile.Dispose();

            try
            {
                FileStream fs = new FileStream(FileDirectory + "\\" + FileName + ".idx", FileMode.OpenOrCreate);
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
            if (_indexFile != null)
            {
                _indexFile.Dispose();
            }
            if (DataFile != null)
            {
                DataFile.Dispose();
            }
        }
    }
}
