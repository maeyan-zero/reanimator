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

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private class DatHeader
        {
            public UInt32 DatHead = 0x68676461;
            public Int32 Version = 4;
            public Int32 Unknown1 = 1;
            public Int32 Unknown2 = 0;
            public Int32 Unknown3 = 0;
            public Int32 Unknown4 = 0x00001154;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 488)]
            public byte[] hashKeys;
        }

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

            [Browsable(false)]
            public int TempDatOffset { get; set; }
        }

        private byte[] _data;
        private FileHeader _indexHeader;
        private StringsHeader _stringsHeader;
        private StringDetailsStruct[] _stringsDetails;
        private readonly List<String> _stringTable = new List<String>();
        private FileDetailsStruct[] _filesDetails;
        //public FileEntry[] FileTable { get; private set; }
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
                _modified = Files.Any(file => file.DirectoryString.Contains(BackupPrefix));
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
            if (data == null || data.Length == 0) return false;

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
            //FileTable = new FileEntry[_indexHeader.FileCount];
            for (int i = 0; i < _indexHeader.FileCount; i++)
            {
                FileEntry fileEntry = new FileEntry
                {
                    FileStruct = _filesDetails[i],
                    InIndex = this
                };

                fileEntry.DirectoryString = _stringTable[fileEntry.Directory];
                fileEntry.FileNameString = _stringTable[fileEntry.FileName];

                //FileTable[i] = fileEntry;
                Files.Add(fileEntry);
            }

            return true;
        }

        public FileEntry GetFileFromIndex(String filePath)
        {
            return String.IsNullOrEmpty(filePath) ? null : Files.FirstOrDefault(fileIndex => fileIndex.FullPath == filePath);
        }

        public FileEntry AddFileToIndex(FileEntry baseEntry)
        {
            Debug.Assert(baseEntry != null);

            // easy-clone the file details struct
            byte[] cloneData = FileTools.StructureToByteArray(baseEntry.FileStruct);
            FileDetailsStruct fileDetailsStruct = FileTools.ByteArrayTo<FileDetailsStruct>(cloneData, 0);

            FileEntry newFileEntry = new FileEntry { FileStruct = fileDetailsStruct, InIndex = this };

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
                newFileEntry.DirectoryString = directoryString;
            }
            if (!String.IsNullOrEmpty(baseEntry.FileNameString))
            {
                int fileNameIndex = _GetStringIndex(baseEntry.FileNameString);
                if (fileNameIndex == -1)
                {
                    fileNameIndex = _AddString(baseEntry.FileNameString);
                }

                newFileEntry.FileStruct.FilenameArrayIndex = fileNameIndex;
                newFileEntry.FileNameString = baseEntry.FileNameString;
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

                    // open if exists
                    if (File.Exists(filePath))
                    {
                        DatFile = new FileStream(filePath, FileMode.Open, fileAccess);
                        return true;
                    }

                    // can we write to it?
                    if (fileAccess == FileAccess.Read) return false;

                    // create new dat and add header
                    DatFile = new FileStream(filePath, FileMode.Create, fileAccess);
                    DatHeader datHeader = new DatHeader();
                    DatFile.Write(FileTools.StructureToByteArray(datHeader), (int)DatFile.Length, Marshal.SizeOf(datHeader));
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

        public void AddFileToDat(byte[] fileData, FileEntry fileEntry)
        {
            Debug.Assert(DatFile != null && fileData != null && fileEntry != null);

            DatFile.Seek(0, SeekOrigin.End);

            byte[] writeBuffer;
            int writeLength;
            if (fileEntry.CompressedSize > 0)
            {
                writeBuffer = new byte[fileData.Length];

                if (IntPtr.Size == 4) // x86
                {
                    UInt32 destinationLength = (UInt32)writeBuffer.Length;
                    compress(writeBuffer, ref destinationLength, fileData, (UInt32)fileData.Length);
                    writeLength = (int)destinationLength;
                }
                else // x64
                {
                    UInt64 destinationLength = (UInt64)writeBuffer.Length;
                    compress(writeBuffer, ref destinationLength, fileData, (UInt64)fileData.Length);
                    writeLength = (int)destinationLength;
                }

                fileEntry.CompressedSize = writeLength;
            }
            else
            {
                writeBuffer = fileData;
                fileEntry.CompressedSize = 0;
                writeLength = fileData.Length;
            }

            fileEntry.DataOffset = (int)DatFile.Position;
            fileEntry.UncompressedSize = fileData.Length;
            DatFile.Write(writeBuffer, 0, writeLength);
        }

        // note: this function does shit all exception checking
        // todo: "complete" me
        public void RebuildDatFile()
        {
            Debug.Assert(DatFile != null);

            // easiest way to remove orphan data blocks to just extract
            // all known files then delete and remake it
            const String tempDir = "temp";

            // extract/save each file in temp dir (no point decompressing them)
            try
            {
                foreach (FileEntry fileEntry in Files)
                {
                    byte[] fileData = _ReadDatFile(fileEntry, false);
                    String filePath = Path.Combine(tempDir, fileEntry.FullPath);
                    Directory.CreateDirectory(filePath);
                    File.WriteAllBytes(filePath, fileData);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to write temp files!\n\n" + e, "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                _DeleteTempDir(tempDir);
                return;
            }

            // save our current index,  close our dat, and make a backup just in case
            String datPath = DatFile.Name;
            DatFile.Close();
            if (!_MoveFile(datPath, datPath + ".bak"))
            {
                _DeleteTempDir(tempDir);
                return;
            }


            // create new dat and add our files
            if (!BeginDatWriting())
            {
                MessageBox.Show("Failed to create dat for writing!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _MoveFile(datPath + ".bak", datPath);
                _DeleteTempDir(tempDir);
                return;
            }

            try
            {
                foreach (FileEntry fileEntry in Files)
                {
                    String filePath = Path.Combine(tempDir, fileEntry.FullPath);
                    byte[] fileBytes = File.ReadAllBytes(filePath);

                    fileEntry.TempDatOffset = (int)DatFile.Length;
                    DatFile.Write(fileBytes, (int)DatFile.Length, fileBytes.Length);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occured while merging dat files!\n\n" + e, "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                DatFile.Close();
                _DeleteTempFile(datPath);
                _MoveFile(datPath + ".bak", datPath);
                _DeleteTempDir(tempDir);
                return;
            }

            // dat has been rebuilt - apply the DatOffset values
            foreach (FileEntry fileEntry in Files)
            {
                fileEntry.DataOffset = fileEntry.TempDatOffset;
            }
        }

        // these 3 functions look a little exccessive, but they're:
        // "let the user know" if something has failed functions
        // as well as "clean-up" functions
        private static void _DeleteTempDir(String tempDir)
        {
            try
            {
                Directory.Delete(tempDir, true);
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to delete temp dir!\n\n" + e, "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private static bool _MoveFile(String moveFrom, String moveTo)
        {
            try
            {
                File.Move(moveFrom, moveTo);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to move temp files!\n\n" + e, "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }
        }

        private static void _DeleteTempFile(String tempFilePath)
        {
            try
            {
                File.Delete(tempFilePath);
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to delete temp idx!\n\n" + e, "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        // todo: finish me... haven't needed it yet
        //public bool RemoveFileFromDat(FileEntry fileEntry)
        //{
        //    Debug.Assert(DatFile != null && fileEntry != null && DatFile.CanWrite);

        //    if (fileEntry.DataOffset < 0) return true;

        //    int byteCount = fileEntry.CompressedSize > 0 ? fileEntry.CompressedSize : fileEntry.UncompressedSize;
        //    if (byteCount <= 0) return true;

        //    try
        //    {
        //        // create new temp .dat file
        //        String tempPath = fileEntry.FullPath + ".temp";
        //        FileStream tempDat = new FileStream(tempPath, FileMode.Create, FileAccess.ReadWrite);
        //        tempDat.W

        //        DatFile.Seek(fileEntry.DataOffset, SeekOrigin.Begin);

        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        String errorMsg = String.Format("Failed to delete file to .dat!\nFile: {0}\n\n{1}", fileEntry.FileName, e);
        //        MessageBox.Show(errorMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        //        return false;
        //    }
        //}

        public byte[] ReadDatFile(FileEntry file)
        {
            return _ReadDatFile(file, true);
        }

        private byte[] _ReadDatFile(FileEntry file, bool decompress)
        {
            Debug.Assert(DatFile != null);

            int result;
            byte[] destBuffer = new byte[file.UncompressedSize];
            DatFile.Seek(file.DataOffset, SeekOrigin.Begin);

            if (file.CompressedSize > 0 && decompress)
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
                // if NOT decompressing, and file IS compressed (CompressedSize > 0), then read the compressed size
                int readLength = !decompress && file.CompressedSize > 0 ? file.CompressedSize : file.UncompressedSize;

                result = DatFile.Read(destBuffer, 0, readLength);
                if (result != file.UncompressedSize) return null;
            }

            return destBuffer;
        }

        public byte[] GenerateIndexFile()
        {
            byte[] buffer = new byte[1024];
            int offset = 0;

            ////// main header //////
            const Int32 version = 4;
            FileTools.WriteToBuffer(ref buffer, ref offset, TokenHead);
            FileTools.WriteToBuffer(ref buffer, ref offset, version);
            FileTools.WriteToBuffer(ref buffer, ref offset, Files.Count);



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
            int i = 0;
            foreach (String str in _stringTable)
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, (Int16)str.Length);

                // while the stringsDetails.Uknown isn't required by the game - let's not just zero it out
                if (_stringsDetails == null || i >= _stringsDetails.Length)
                {
                    offset += 4; // leave new strings as null - doesn't really matter though
                    continue;
                }

                FileTools.WriteToBuffer(ref buffer, ref offset, _stringsDetails[i].Unknown);
                i++;
            }



            ////// file block //////
            const UInt32 foo = 0;
            //i = 0;
            FileTools.WriteToBuffer(ref buffer, ref offset, TokenSect);
            foreach (FileEntry fileIndex in Files)
            {
                // todo: this looks gross, but is just for testing
                // final version will be similar to reading - dumping struct using MarshalAs
                FileTools.WriteToBuffer(ref buffer, ref offset, TokenInfo);
                //FileTools.WriteToBuffer(ref buffer, ref offset, foo);
                //FileTools.WriteToBuffer(ref buffer, ref offset, foo);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.FileStruct.FolderPathHash); // tested with 0xDEADBEEF and 0x00000000, game didn't care
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

            // get final buffer of correct size
            byte[] returnBuffer = new byte[offset];
            Buffer.BlockCopy(buffer, 0, returnBuffer, 0, returnBuffer.Length);
            return returnBuffer;
        }

        public bool PatchOutFile(int i)
        {
            if (i < 0 || i > Files.Count) return false;

            FileEntry fileIndex = Files[i];
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
