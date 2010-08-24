using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Windows.Forms;
using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

namespace Reanimator
{
    public class Index : IDisposable
    {
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


        public const int Base000 = 9;
        public const int LatestPatch = 10;
        public const int LatestPatchLocalized = 11;
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
        public const UInt32 TokenHead = 0x6867696E; // 'nigh'
        public const UInt32 TokenSect = 0x68677073; // 'spgh'
        public const UInt32 TokenInfo = 0x6867696F; // 'oigh'

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private class FileHeader
        {
            public UInt32 FileToken;
            public UInt32 FileVersion;
            public Int32 FileCount;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private class StringsHeader
        {
            public UInt32 StringsToken;
            public Int32 StringsCount;
            public Int32 StringByteCount;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private class StringDetailsStruct
        {
            public short StringLength;
            public UInt32 Unknown;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class FileDetailsStruct
        {
            public UInt32 StartToken;
            public Int32 Unknown11;                     // 0    0x00
            public Int32 Unknown12;                     // 4    0x04
            public Int32 DataOffset;                    // 8    0x08
            public Int32 Null1;                         // 12   0x0C
            public Int32 UncompressedSize;              // 16   0x10
            public Int32 CompressedSize;                // 20   0x14
            public Int32 Null2;                         // 24   0x18
            public Int32 DirectoryArrayIndex;           // 28   0x1C
            public Int32 FilenameArrayIndex;            // 32   0x20
            public FILETIME FileTime;                   // 36   0x24            Can't be null             .text:000000014004958B cmp     qword ptr [rdi+10h], 0 -> jz      loc_140049402   ; Jump if Zero
            public Int32 Unknown23;                     // 44   0x2C
            public Int32 Unknown24;                     // 48   0x30
            public Int32 Null31;                        // 52   0x34
            public Int32 Null32;                        // 56   0x38
            public Int32 Null33;                        // 60   0x3C
            public Int32 First4BytesOfFile;             // 64   0x40
            public Int32 Second4BytesOfFile;            // 68   0x44
            public UInt32 EndToken;
        }


        public class FileIndex
        {
            [Browsable(false)]
            public FileDetailsStruct FileStruct { get; set; }

            [Browsable(false)]
            public Index InIndex { get; set; }

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
                get { return FileStruct.DirectoryArrayIndex; }
                set { FileStruct.DirectoryArrayIndex = value; }
            }
            public string DirectoryString { get; set; }

            [Browsable(false)]
            public int FileName
            {
                get { return FileStruct.FilenameArrayIndex; }
            }
            public string FileNameString { get; set; }

            public bool Modified
            {
                get { return DirectoryString.Contains(BackupPrefix); }
            }
        }

        private byte[] _data;
        private FileHeader _indexHeader;
        private StringsHeader _stringsHeader;
        private StringDetailsStruct[] _stringsDetails;
        private String[] _stringTable;
        private FileDetailsStruct[] _filesDetails;
        public FileIndex[] FileTable { get; private set; }

        public String FilePath { get; private set; }
        public String FileDirectory { get { return Path.GetDirectoryName(FilePath); } }
        public String FileNameWithoutExtension { get { return Path.GetFileNameWithoutExtension(FilePath); } }

        public bool DatFileOpen { get { return DataFile == null ? false : true; } }
        public FileStream DataFile { get; set; }

        public const string BackupPrefix = @"backup";
        private bool _checkedForModified;
        public bool Modified
        {
            get
            {
                if (_checkedForModified) return Modified;

                _checkedForModified = true;
                return FileTable.Any(file => file.DirectoryString.Contains(BackupPrefix));
            }

            private set { Modified = value; }
        }

        public bool ParseData(byte[] data, String filePath)
        {
            if (data == null) return false;

            _data = data;
            FilePath = filePath;
            Crypt.Decrypt(_data);



            ////// file header //////
            int offset = 0;
            _indexHeader = (FileHeader)FileTools.ByteArrayToStructure(_data, typeof(FileHeader), offset);
            offset += Marshal.SizeOf(typeof(FileHeader));

            if (_indexHeader.FileToken != TokenHead)
            {
                String msg = String.Format(
                    "Unexpected file type!\nExpected FileToken = 0x{0:X8}\n\nProvided = 0x{1:X8}", TokenHead,
                    _indexHeader.FileToken);
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (_indexHeader.FileVersion != 0x04)
            {
                String msg = String.Format(
                    "Unexpected file version!\nExpected FileVersion = 4\n\nProvided = {0}", _indexHeader.FileVersion);
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }



            ////// strings table //////
            _stringsHeader = (StringsHeader)FileTools.ByteArrayToStructure(_data, typeof(StringsHeader), offset);
            offset += Marshal.SizeOf(typeof(StringsHeader));

            if (_stringsHeader.StringsToken != TokenSect)
            {
                String msg = String.Format(
                    "Unexpected strings token!\nExpected StringsToken = 0x{0:X8}\n\nProvided = 0x{1:X8}", TokenSect,
                    _stringsHeader.StringsToken);
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            _stringTable = new String[_stringsHeader.StringsCount];
            for (int i = 0; i < _stringsHeader.StringsCount; i++)
            {
                _stringTable[i] = FileTools.ByteArrayToStringAnsi(_data, offset);
                offset += _stringTable[i].Length + 1; // +1 for \0
            }



            ////// strings details //////
            UInt32 stringsDetailsToken = FileTools.ByteArrayTo<UInt32>(_data, ref offset);
            if (stringsDetailsToken != TokenSect)
            {
                String msg = String.Format(
                    "Unexpected strings details token!\nExpected stringsDetailsToken = 0x{0:X8}\n\nProvided = 0x{1:X8}", TokenSect,
                    stringsDetailsToken);
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            _stringsDetails = FileTools.ByteArrayToArray<StringDetailsStruct>(_data, ref offset, _stringsHeader.StringsCount);



            ////// files details //////
            UInt32 filesToken = FileTools.ByteArrayTo<UInt32>(_data, ref offset);
            if (filesToken != TokenSect)
            {
                String msg = String.Format(
                    "Unexpected files token!\nExpected filesToken = 0x{0:X8}\n\nProvided = 0x{1:X8}", TokenSect,
                    filesToken);
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            _filesDetails = FileTools.ByteArrayToArray<FileDetailsStruct>(_data, ref offset, _indexHeader.FileCount);



            ////// do files //////
            FileTable = new FileIndex[_indexHeader.FileCount];
            for (int i = 0; i < _indexHeader.FileCount; i++)
            {
                FileIndex fileIndex = new FileIndex
                {
                    FileStruct = _filesDetails[i],
                    InIndex = this
                };

                fileIndex.DirectoryString = _stringTable[fileIndex.Directory];
                fileIndex.FileNameString = _stringTable[fileIndex.FileName];

                FileTable[i] = fileIndex;
            }

            File.WriteAllBytes(@"C:\index1.idx", _data);

            return true;
        }

        public bool OpenDatForReading()
        {
            if (DataFile == null)
            {
                try
                {
                    String filePath = String.Format(@"{0}\{1}.dat", FileDirectory, FileNameWithoutExtension);
                    if (!File.Exists(filePath)) return false;

                    DataFile = new FileStream(filePath, FileMode.Open, FileAccess.Read);
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
            if (!OpenDatForReading()) return null;

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
                if (!OpenDatForReading())
                {
                    MessageBox.Show("Failed to open accompanying dat file!\n" + FileNameWithoutExtension, "Error",
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

            ////// main header //////
            const Int32 version = 4;
            FileTools.WriteToBuffer(ref buffer, ref offset, TokenHead);
            FileTools.WriteToBuffer(ref buffer, ref offset, version);
            FileTools.WriteToBuffer(ref buffer, ref offset, _indexHeader.FileCount);



            ////// string block //////
            FileTools.WriteToBuffer(ref buffer, ref offset, TokenSect);
            FileTools.WriteToBuffer(ref buffer, ref offset, _stringsHeader.StringsCount);
            int stringByteCountOffset = offset;
            offset += 4;
            foreach (String str in _stringTable)
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, FileTools.StringToASCIIByteArray(str));
                offset++; // \0
            }
            FileTools.WriteToBuffer(ref buffer, stringByteCountOffset, (UInt32)(offset - stringByteCountOffset - sizeof(UInt32)));



            ////// string data //////
            FileTools.WriteToBuffer(ref buffer, ref offset, TokenSect);
            //int i = 0;
            foreach (String str in _stringTable)
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, (Int16)str.Length);
                offset += 4; // unknown  -  not required apparently
                //FileTools.WriteToBuffer(ref _buffer, ref offset, _stringTableUnknowns[i]);
                //i++;
            }



            ////// file block //////
            //const UInt64 foo = 0xDEADBEEFDEADBEEF;
            FileTools.WriteToBuffer(ref buffer, ref offset, TokenSect);
            //i = 0;
            foreach (FileIndex fileIndex in FileTable)
            {
                // this looks gross, but is just for testing
                // final version will be similar to reading - dumping struct using MarshalAs
                FileTools.WriteToBuffer(ref buffer, ref offset, TokenInfo);

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
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.FileStruct.FileTime);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.FileStruct.Unknown23);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.FileStruct.Unknown24);
                //offset += 12; // unknown  -  not required
                offset += 12; // null
                //offset += 8; // first 8 bytes  -  not required
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.FileStruct.First4BytesOfFile);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.FileStruct.Second4BytesOfFile);
                FileTools.WriteToBuffer(ref buffer, ref offset, TokenInfo);
                //i++;
            }

            byte[] returnBuffer = new byte[offset];
            Buffer.BlockCopy(buffer, 0, returnBuffer, 0, returnBuffer.Length);
            return returnBuffer;
        }

        public void AddDirectoryPrefix(int i)
        {
            String dir = BackupPrefix + @"\" + _stringTable[FileTable[i].Directory];
            int index = StringExists(dir);

            // if the directory doesn't exist, add it.
            if (index == -1)
            {
                index = _stringTable.Length;

                String[] buffer = new String[index + 1];
                _stringTable.CopyTo(buffer, 0);
                buffer[index] = BackupPrefix + @"\" + _stringTable[FileTable[i].Directory];

                _stringTable = buffer;
                _stringsHeader.StringsCount++;
            }

            FileTable[i].Directory = index;
            Modified = true;
        }

        private int StringExists(String str)
        {
            for (int i = 0; i < _stringTable.Length; i++)
            {
                if (_stringTable[i] == str)
                {
                    return i;
                }
            }

            return -1;
        }

        public override string ToString()
        {
            return FileNameWithoutExtension;
        }

        public void Dispose()
        {
            if (DataFile != null)
            {
                DataFile.Dispose();
            }
        }

        // todo: why are these here?
        /*
        public void RemoveDirectorySuffix(int i)
        {
            string dir = FileTable[i].DirectoryString.Remove(0, Affix.Length);
            FileTable[i].Directory = StringExists(dir);
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
                FileStream fs = new FileStream(FileDirectory + "\\" + GetFileNameWithoutExtension + ".idx", FileMode.OpenOrCreate);
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
         */
    }
}
