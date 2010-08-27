using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Windows.Forms;

namespace Reanimator
{
    public class Index : IDisposable
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
            public Int32 FolderPathHash;                // 0    0x00        this value is the same for all files that have the same folder path
            public Int32 FileNameHash;                  // 4    0x04        as above, but with file name
            public Int32 DataOffset;                    // 8    0x08
            public Int32 Null1;                         // 12   0x0C
            public Int32 UncompressedSize;              // 16   0x10
            public Int32 CompressedSize;                // 20   0x14
            public Int32 Null2;                         // 24   0x18
            public Int32 DirectoryArrayIndex;           // 28   0x1C
            public Int32 FilenameArrayIndex;            // 32   0x20
            public Int64 FileTime;                      // 36   0x24        can't be null - is a FILETIME (using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME)
            public Int32 Unknown23;                     // 44   0x2C        this and following unknown are the same for files that have the same compressed and uncompressed sizes
            public Int32 Unknown24;                     // 48   0x30        however they're different for different files with the same sizes...
            public Int32 Null31;                        // 52   0x34
            public Int32 Null32;                        // 56   0x38
            public Int32 Null33;                        // 60   0x3C
            public Int32 First4BytesOfFile;             // 64   0x40
            public Int32 Second4BytesOfFile;            // 68   0x44
            public UInt32 EndToken;
        }


        public class FileEntry
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

            public String FolderPathHash
            {
                get { return FileStruct.FolderPathHash.ToString("X8"); }
            }
            public String FileNameHash
            {
                get { return FileStruct.FileNameHash.ToString("X8"); }
            }

            public String Unknown23
            {
                get { return FileStruct.Unknown23.ToString("X8"); }
            }
            public String Unknown24
            {
                get { return FileStruct.Unknown24.ToString("X8"); }
            }

            [Browsable(false)]
            public int Directory
            {
                get { return FileStruct.DirectoryArrayIndex; }
                set { FileStruct.DirectoryArrayIndex = value; }
            }

            private String _directoryString;
            public String DirectoryString
            {
                get { return _directoryString; }

                set
                {
                    _directoryString = value;
                    GenerateFullPath();
                }
            }

            [Browsable(false)]
            public int FileName
            {
                get { return FileStruct.FilenameArrayIndex; }
            }

            private String _fileNameString;
            public String FileNameString
            {
                get { return _fileNameString; }

                set
                {
                    _fileNameString = value;
                    GenerateFullPath();
                }
            }

            public String FullPath;
            private void GenerateFullPath()
            {
                if (_directoryString == null || _fileNameString == null) return;

                FullPath = _directoryString + _fileNameString;
            }

            public bool Modified
            {
                get { return DirectoryString != null && DirectoryString.Contains(BackupPrefix); }
            }
        }

        private byte[] _data;
        private FileHeader _indexHeader;
        private StringsHeader _stringsHeader;
        private StringDetailsStruct[] _stringsDetails;
        private readonly List<String> _stringTable = new List<String>();
        private FileDetailsStruct[] _filesDetails;
        public FileEntry[] FileTable { get; private set; }
        public List<FileEntry> Files { get; private set; }

        public String FilePath { get; private set; }
        public String FileDirectory { get { return Path.GetDirectoryName(FilePath); } }
        public String FileNameWithoutExtension { get { return Path.GetFileNameWithoutExtension(FilePath); } }

        public bool DatFileOpen { get { return DatFile == null ? false : true; } }
        public FileStream DatFile { get; set; }

        public const string BackupPrefix = @"backup";
        private bool _checkedForModified;
        private bool _modified;
        public bool Modified
        {
            get
            {
                if (_checkedForModified) return _modified;

                _checkedForModified = true;
                _modified = FileTable.Any(file => file.DirectoryString.Contains(BackupPrefix));
                return _modified;
            }

            private set
            {
                _modified = value;
            }
        }

        public Index()
        {
            FilePath = null;
            Files = new List<FileEntry>();
        }

        public Index(String filePath)
        {
            FilePath = filePath;
            Files = new List<FileEntry>();
        }

        public bool ParseData(byte[] data, String filePath)
        {
            if (data == null) return false;

            _data = data;
            FilePath = filePath;



            ////// check if encrypted //////
            UInt32 fileHeadToken = BitConverter.ToUInt32(_data, 0);
            if (fileHeadToken != TokenHead)
            {
                Crypt.Decrypt(_data);
            }



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

            String[] stringTable = new String[_stringsHeader.StringsCount];
            for (int i = 0; i < _stringsHeader.StringsCount; i++)
            {
                stringTable[i] = FileTools.ByteArrayToStringAnsi(_data, offset);
                offset += stringTable[i].Length + 1; // +1 for \0
            }
            _stringTable.AddRange(stringTable);


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
            FileTable = new FileEntry[_indexHeader.FileCount];
            for (int i = 0; i < _indexHeader.FileCount; i++)
            {
                FileEntry fileEntry = new FileEntry
                {
                    FileStruct = _filesDetails[i],
                    InIndex = this
                };

                fileEntry.DirectoryString = _stringTable[fileEntry.Directory];
                fileEntry.FileNameString = _stringTable[fileEntry.FileName];

                FileTable[i] = fileEntry;
                Files.Add(fileEntry);
            }

            return true;
        }

        public FileEntry GetFileFromIndex(String filePath)
        {
            return String.IsNullOrEmpty(filePath) ? null : FileTable.FirstOrDefault(fileIndex => fileIndex.FullPath == filePath);
        }

        public FileEntry AddFileToIndex(FileEntry baseEntry)
        {
            Debug.Assert(baseEntry != null);

            // easy-clone the file details struct
            byte[] cloneData = FileTools.StructureToByteArray(baseEntry.FileStruct);
            FileDetailsStruct fileDetailsStruct = FileTools.ByteArrayTo<FileDetailsStruct>(cloneData, 0);

            FileEntry newFileEntry = new FileEntry {FileStruct = fileDetailsStruct, InIndex = this };

            // update directory and file name string index offsets
            // todo: should we even allow these to be null/empty??
            if (!String.IsNullOrEmpty(baseEntry.DirectoryString))
            {
                // remove the backup string if present
                String directoryString = baseEntry.DirectoryString.Replace(BackupPrefix + @"\", "");

                int directoryIndex = _GetStringIndex(directoryString);
                if (directoryIndex == -1)
                {
                    directoryIndex = _AddString(directoryString);
                }

                newFileEntry.FileStruct.DirectoryArrayIndex = directoryIndex;
                // todo: update DirectoryString as well? Do we really need to?
            }
            if (!String.IsNullOrEmpty(baseEntry.FileNameString))
            {
                int fileNameIndex = _GetStringIndex(baseEntry.FileNameString);
                if (fileNameIndex == -1)
                {
                    fileNameIndex = _AddString(baseEntry.FileNameString);
                }

                newFileEntry.FileStruct.FilenameArrayIndex = fileNameIndex;
                // todo: update FileNameString as well? Do we really need to?
            }

            // add and return
            Files.Add(newFileEntry);
            return newFileEntry;
        }

        public bool BeginDatReading()
        {
            return OpenDat(FileAccess.Read);
        }

        public bool BeginDatWriting()
        {
            if (DatFile != null) DatFile.Close();

            return OpenDat(FileAccess.ReadWrite);
        }

        public bool OpenDat(FileAccess fileAccess)
        {
            if (DatFile == null)
            {
                try
                {
                    String filePath = String.Format(@"{0}\{1}.dat", FileDirectory, FileNameWithoutExtension);
                    if (!File.Exists(filePath)) return false;

                    DatFile = new FileStream(filePath, FileMode.Open, fileAccess);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        public void EndDatAccess()
        {
            if (DatFile != null)
            {
                DatFile.Close();
            }
        }

        public bool AddFileToDat(byte[] fileData, FileEntry fileEntry, bool removeOld)
        {
            Debug.Assert(fileData != null);
            Debug.Assert(fileEntry != null);
            if (DatFile == null) return false;



            return true;
        }

        public byte[] ReadDataFile(FileEntry file)
        {
            if (!BeginDatReading()) return null;

            int result;
            byte[] destBuffer = new byte[file.UncompressedSize];
            DatFile.Seek(file.DataOffset, SeekOrigin.Begin);

            if (file.CompressedSize > 0)
            {
                byte[] srcBuffer = new byte[file.CompressedSize];

                DatFile.Read(srcBuffer, 0, srcBuffer.Length);
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
                result = DatFile.Read(destBuffer, 0, file.UncompressedSize);

                if (result != file.UncompressedSize)
                {
                    return null;
                }
            }

            return destBuffer;
        }

        public void AppendToDat(byte[] uncompressedBuffer, bool doCompress, FileEntry file, bool writeIndex)
        {
            if (DatFile == null) return;

            DatFile.Seek(0, SeekOrigin.End);

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
            file.DataOffset = (int)DatFile.Position;
            file.UncompressedSize = uncompressedBuffer.Length;
            //int i = 1;
            DatFile.Write(compressedBuffer, 0, len);
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
            FileTools.WriteToBuffer(ref buffer, ref offset, _stringTable.Count);
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
            foreach (FileEntry fileIndex in FileTable)
            {
                // this looks gross, but is just for testing
                // final version will be similar to reading - dumping struct using MarshalAs
                FileTools.WriteToBuffer(ref buffer, ref offset, TokenInfo);

                //FileTools.WriteToBuffer(ref buffer, ref offset, foo);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.FileStruct.FolderPathHash);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.FileStruct.FileNameHash); // game freezes if not correct value
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.DataOffset);
                offset += 4; // null
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.UncompressedSize);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.CompressedSize);
                offset += 4; // null
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.Directory);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.FileName);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.FileStruct.FileTime);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.FileStruct.Unknown23);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.FileStruct.Unknown24);
                offset += 12; // null
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.FileStruct.First4BytesOfFile);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.FileStruct.Second4BytesOfFile);
                FileTools.WriteToBuffer(ref buffer, ref offset, TokenInfo);
            }

            byte[] returnBuffer = new byte[offset];
            Buffer.BlockCopy(buffer, 0, returnBuffer, 0, returnBuffer.Length);
            return returnBuffer;
        }

        public bool PatchOutFile(int i)
        {
            if (i < 0 || i > FileTable.Length) return false;

            FileEntry fileIndex = FileTable[i];
            return PatchOutFile(fileIndex);
        }

        public bool PatchOutFile(FileEntry fileEntry)
        {
            Debug.Assert(fileEntry != null);

            // has it already be patched out?
            if (fileEntry.DirectoryString.Contains(BackupPrefix)) return false;

            // does the backup dir exist?
            String dir = BackupPrefix + @"\" + _stringTable[fileEntry.Directory];
            int index = _GetStringIndex(dir);

            // if the directory doesn't exist, add it.
            if (index == -1)
            {
                index = _stringTable.Count;
                _stringTable.Add(dir);
            }

            fileEntry.Directory = index;
            Modified = true;
            return true;
        }

        public bool PatchInFile(FileEntry fileEntry)
        {
            Debug.Assert(fileEntry != null);

            // does it even need to be patched in?
            if (!fileEntry.DirectoryString.Contains(BackupPrefix)) return false;

            // does the original dir exist?
            Debug.Assert(false, "todo");
            //String dir = BackupPrefix + @"\" + _stringTable[fileEntry.Directory];
            //int index = _GetStringIndex(dir);

            //// if the directory doesn't exist, add it.
            //if (index == -1)
            //{
            //    index = _stringTable.Count;
            //    _stringTable.Add(dir);
            //}

            //fileEntry.Directory = index;
            //Modified = true;);
            return true;
        }

        private int _GetStringIndex(String str)
        {
            if (String.IsNullOrEmpty(str)) return -1;

            return _stringTable.IndexOf(str);
        }

        private int _AddString(String str)
        {
            int index = _stringTable.Count;
            _stringTable.Add(str);
            return index;
        }

        public override string ToString()
        {
            return FileNameWithoutExtension;
        }

        public void Dispose()
        {
            if (DatFile != null)
            {
                DatFile.Dispose();
            }
        }
    }
}
