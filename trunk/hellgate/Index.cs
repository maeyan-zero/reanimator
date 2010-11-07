using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.ComponentModel;
using Revival.Common;

namespace Hellgate
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

        #region Data Structures
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct DatHeader
        {
            public UInt32 DatHead { get { return 0x68676461; } }
            public Int32 Version { get { return 0x01; } }
            public Int32 Unknown1 { get { return 0x00; } }
            public Int32 Unknown2 { get { return 0x00; } }
            public Int32 Unknown3 { get { return 0x00; } }
            public Int32 Unknown4 { get { return 0x00001154; } }
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 488)]
            public byte[] hashKeys;
        }

        static class Token
        {
            public static UInt32 Head { get { return 0x6867696E; } } // 'nigh'
            public static UInt32 Sect { get { return 0x68677073; } } // 'spgh'
            public static UInt32 Info { get { return 0x6867696F; } } // 'oigh'
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct FileHeader
        {
            public UInt32 FileToken;
            public UInt32 FileVersion;
            public Int32 FileCount;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct StringsHeader
        {
            public UInt32 StringsToken;
            public Int32 StringsCount;
            public Int32 StringByteCount;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct StringDetailsStruct
        {
            public short StringLength;
            public UInt32 StringHash; // this is the Crypt.GetStringHash results
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class FileDetailsStruct
        {
            public UInt32 StartToken;
            public UInt32 FolderPathSHA1Hash;           // 0    0x00        this is the first 4 bytes of an SHA1 crypto hash of the folder path string
            public UInt32 FileNameSHA1Hash;             // 4    0x04        as above, but with file name string
            public Int64 DataOffset;                    // 8    0x08        this is read in as 8 bytes
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
            public Index Parent { get; set; }

            public Int64 DataOffset
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
            public int DirectoryIndex
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
            public int FileNameIndex
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

            public UInt64 LongHash
            {
                get { return (UInt64)FileStruct.FolderPathSHA1Hash | ((UInt64)FileStruct.FileNameSHA1Hash << 32); }
            }

            public long FileTime
            {
                get { return FileStruct.FileTime; }
            }

            [Browsable(false)]
            public int TempDatOffset { get; set; }
        }
        #endregion

        #region Members
        public String FilePath { get; set; }
        public String FileDirectory { get { return Path.GetDirectoryName(FilePath); } }
        public String FileNameWithoutExtension { get { return Path.GetFileNameWithoutExtension(FilePath); } }
        public bool IntegrityCheck { get; private set; }
        public List<FileEntry> Files { get; private set; }

        StringCollection Strings { get; set; }
        List<FileDetailsStruct> FileDetails { get; set; }
        FileStream DatFile { get; set; }
        public bool DatFileOpen { get { return DatFile == null ? false : true; } }
        public UInt32 Version { get { return 0x04; } }
        public String[] DoNotCompress = new String[] { ".bik", ".ogg", ".mp2" };
        public static String BackupPrefix { get { return "backup"; } }
        public bool Modified { get { return Files.Any(file => file.DirectoryString.Contains(BackupPrefix)); } }
        #endregion



        /// <summary>
        /// Creates an empty Index file.
        /// </summary>
        public Index()
        {
            Strings = new StringCollection();
            FileDetails = new List<FileDetailsStruct>();
            Files = new List<FileEntry>();
        }

        /// <summary>
        /// Creates a new index file from the byte array.
        /// </summary>
        /// <param name="buffer">The index file as a byte array.</param>
        public Index(byte[] buffer)
            : this()
        {
            IntegrityCheck = ParseData(buffer);
        }



        /// <summary>
        /// Parses a serialized version of an index file.
        /// </summary>
        /// <param name="buffer">Encrypted or decrypted index byte array.</param>
        /// <returns>Returns false if the file is malformed.</returns>
        bool ParseData(byte[] buffer)
        {
            if ((buffer == null) || (buffer.Length == 0)) return false;
            int offset = 0;

            // Check for encryption,
            UInt32 fileHeadToken = BitConverter.ToUInt32(buffer, 0);
            if (!(fileHeadToken == Token.Head))
            {
                Crypt.Decrypt(buffer);
            }

            // Read the file header, check for errors
            FileHeader fileHeader = FileTools.ByteArrayToStructure<FileHeader>(buffer, ref offset);
            if (!(fileHeader.FileToken == Token.Head)) return false;
            if (!(fileHeader.FileVersion == Version)) return false;

            // Read the strings section
            StringsHeader stringsHeader = FileTools.ByteArrayToStructure<StringsHeader>(buffer, ref offset);
            if (!(stringsHeader.StringsToken == Token.Sect)) return false;
            for (int i = 0; i < stringsHeader.StringsCount; i++)
            {
                Strings.Add(FileTools.ByteArrayToStringASCII(buffer, offset));
                offset += Strings[i].Length + 1; // +1 for \0
            }
            
            // String Details
            UInt32 stringsDetailsToken = FileTools.ByteArrayToUInt32(buffer, ref offset);
            if (!(stringsDetailsToken == Token.Sect)) return false;
            // Skip over the details struct because we don't need it.
            offset += stringsHeader.StringsCount * Marshal.SizeOf(typeof(StringDetailsStruct));

            // Files Structure details
            UInt32 filesToken = FileTools.ByteArrayToUInt32(buffer, ref offset);
            if (!(filesToken == Token.Sect)) return false;
            FileDetails.AddRange(FileTools.ByteArrayToArray<FileDetailsStruct>(buffer, ref offset, fileHeader.FileCount));

            // The Files list is the public interface
            for (int i = 0; i < fileHeader.FileCount; i++)
            {
                FileEntry fileEntry = new FileEntry
                {
                    FileStruct = FileDetails[i],
                    Parent = this,
                    DirectoryString = Strings[FileDetails[i].DirectoryArrayIndex],
                    FileNameString = Strings[FileDetails[i].FilenameArrayIndex]
                };
                Files.Add(fileEntry);
            }

            return true;
        }

        /// <summary>
        /// Generates a serialized version of the index file.
        /// </summary>
        /// <returns>Returns the index file as an encrypted byte array.</returns>
        public byte[] GenerateIndexFile()
        {
            byte[] buffer = new byte[1024];
            int offset = 0;

            // Header
            FileTools.WriteToBuffer(ref buffer, ref offset, Token.Head);
            FileTools.WriteToBuffer(ref buffer, ref offset, Version);
            FileTools.WriteToBuffer(ref buffer, ref offset, Files.Count);

            // Strings
            FileTools.WriteToBuffer(ref buffer, ref offset, Token.Sect);
            FileTools.WriteToBuffer(ref buffer, ref offset, Strings.Count);
            int stringByteCountOffset = offset;
            offset += 4;
            foreach (String str in Strings)
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, FileTools.StringToASCIIByteArray(str));
                offset++; // \0
            }
            UInt32 strByteArrayLen = (UInt32)(offset - stringByteCountOffset - sizeof(UInt32));
            FileTools.WriteToBuffer(ref buffer, stringByteCountOffset, strByteArrayLen);

            // String Details Struct
            FileTools.WriteToBuffer(ref buffer, ref offset, Token.Sect);
            foreach (String str in Strings)
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, (Int16)str.Length);
                FileTools.WriteToBuffer(ref buffer, ref offset, Crypt.GetStringHash(str));
            }

            // File Structure Block
            FileTools.WriteToBuffer(ref buffer, ref offset, Token.Sect);
            foreach (FileDetailsStruct fileIndex in FileDetails)
            {
                FileTools.WriteToBuffer(ref buffer, offset, fileIndex);
            }

            // Resize the buffer and return
            byte[] returnBuffer = new byte[offset];
            Buffer.BlockCopy(buffer, 0, returnBuffer, 0, returnBuffer.Length);
            return returnBuffer;
        }



        /// <summary>
        /// Appends a file to the index and corresponding data file.
        /// </summary>
        /// <param name="directory">The directory that will be stored in the index.</param>
        /// <param name="fileName">The filename that will be stored in the index.</param>
        /// <param name="bytesToWrite">Byte array of the file to add.</param>
        /// <returns>Returns true if the operation was successful.</returns>
        public bool AddFile(string directory, string fileName, byte[] bytesToWrite)
        {
            if (bytesToWrite == null) return false;
            if (bytesToWrite.Length <= 0) return false;

            // Check Data Stream is available.
            if ((DatFile == null))
            {
                if (!(OpenDat(FileAccess.ReadWrite))) return false;
            }

            bool doCompress = true;
            foreach (string extentsion in DoNotCompress)
            {
                if (fileName.EndsWith(extentsion))
                {
                    doCompress = false;
                    break;
                }
            }

            // Create new Index File entry
            byte[] fileBuffer = bytesToWrite;

            FileDetailsStruct fileStruct = new FileDetailsStruct
            {
                CompressedSize = 0,
                UncompressedSize = fileBuffer.Length,
                FileTime = DateTime.Now.ToFileTime(),
                DataOffset = (int)DatFile.Length,
                FileNameSHA1Hash = Crypt.GetStringSHA1UInt32(fileName),
                FolderPathSHA1Hash = Crypt.GetStringSHA1UInt32(directory),
                StartToken = Token.Info,
                EndToken = Token.Info,
                Unknown23 = 1,
                Unknown24 = 1
            };

            // See if the index already contains the FileName string
            int fileNameIndex = Strings.IndexOf(fileName);
            if (fileNameIndex != -1)
            {
                fileStruct.FilenameArrayIndex = fileNameIndex;
            }
            // otherwise add it
            else
            {
                Strings.Add(fileName);
                fileStruct.FilenameArrayIndex = Strings.Count - 1;
            }

            // See if the index already contains the Directory string
            int directoryIndex = Strings.IndexOf(directory);
            if (directoryIndex != -1)
            {
                fileStruct.DirectoryArrayIndex = directoryIndex;
            }
            // otherwise add it
            else
            {
                Strings.Add(directory);
                fileStruct.DirectoryArrayIndex = Strings.Count - 1;
            }


            // Create an entry in the File Index
            FileEntry fileEntry = new FileEntry
            {
                FileStruct = fileStruct,
                Parent = this
            };

            fileEntry.DirectoryString = Strings[fileEntry.DirectoryIndex];
            fileEntry.FileNameString = Strings[fileEntry.FileNameIndex];
            fileEntry.CompressedSize = doCompress ? 1 : 0;

            int pos = 0;
            byte[] first4Bytes = FileTools.ByteArrayToArray<byte>(bytesToWrite, ref pos, 4);
            byte[] second4Bytes = FileTools.ByteArrayToArray<byte>(bytesToWrite, ref pos, 4);

            fileStruct.First4BytesOfFile = BitConverter.ToInt32(first4Bytes, 0);
            fileStruct.Second4BytesOfFile = BitConverter.ToInt32(second4Bytes, 0);
            FileDetails.Add(fileStruct);

            AddFileToDat(bytesToWrite, fileEntry);

            Files.Add(fileEntry);
            

            

            // todo: handle existing files
            //int index = Files.IndexOf(fileEntry);
            //if (index != -1) Files[index] = fileEntry;
            //else Files.Add(fileEntry);

            return true;
        }

        /// <summary>
        /// Appends a FileEntry object to the index collection.
        /// </summary>
        /// <param name="baseEntry">The FileEntry to add.</param>
        /// <returns>The new FileEntry reference.</returns>
        public FileEntry AddFileToIndex(FileEntry baseEntry)
        {
            Debug.Assert(baseEntry != null);

            // easy-clone the file details struct
            byte[] cloneData = FileTools.StructureToByteArray(baseEntry.FileStruct);
            FileDetailsStruct fileDetailsStruct = FileTools.ByteArrayToStructure<FileDetailsStruct>(cloneData, 0);

            FileEntry newFileEntry = new FileEntry { FileStruct = fileDetailsStruct, Parent = this };

            // update directory and file name string index offsets
            // todo: should we even allow these to be null/empty??
            if (!String.IsNullOrEmpty(baseEntry.DirectoryString))
            {
                // remove the backup string if present
                String directoryString = baseEntry.DirectoryString.Replace(BackupPrefix + @"\", "");

                int directoryIndex = GetStringIndex(directoryString);
                if (directoryIndex == -1)
                {
                    directoryIndex = AddString(directoryString);
                }

                newFileEntry.FileStruct.DirectoryArrayIndex = directoryIndex;
                newFileEntry.DirectoryString = directoryString;
            }
            if (!String.IsNullOrEmpty(baseEntry.FileNameString))
            {
                int fileNameIndex = GetStringIndex(baseEntry.FileNameString);
                if (fileNameIndex == -1)
                {
                    fileNameIndex = AddString(baseEntry.FileNameString);
                }

                newFileEntry.FileStruct.FilenameArrayIndex = fileNameIndex;
                newFileEntry.FileNameString = baseEntry.FileNameString;
            }

            // add and return
            Files.Add(newFileEntry);
            return newFileEntry;
        }

        /// <summary>
        /// Appends a file byte array to the corresponding dat and adjusts the FileEntry object.
        /// </summary>
        /// <param name="fileData">The byte array to append.</param>
        /// <param name="fileEntry">The corresponding FileEntry that will be adjusted.</param>
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



        /// <summary>
        /// Gets the corresponding File Entry structure from the index.
        /// </summary>
        /// <param name="filePath">The full path of the file to search.</param>
        /// <returns>Returns the matching FileEntry or null if no match found.</returns>
        public FileEntry GetFileEntry(String filePath)
        {
            return String.IsNullOrEmpty(filePath) ? null : Files.FirstOrDefault(fileIndex => fileIndex.FullPath == filePath);
        }

        public byte[] GetFileBytes(FileEntry file)
        {
            return GetFileBytes(file, true);
        }

        byte[] GetFileBytes(FileEntry file, bool decompress)
        {
            Debug.Assert(DatFile != null);

            int result;
            // we shouldn't load huge files into memory like this
            // todo: write progressive extraction/copy progress (cinematic files aren't compressed anyways)
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



        public bool BeginDatReading()
        {
            return OpenDat(FileAccess.Read);
        }

        public bool BeginDatWriting()
        {
            if (DatFile != null) DatFile.Close();
            return OpenDat(FileAccess.ReadWrite);
        }

        public void EndDatAccess()
        {
            if (DatFile == null) return;

            DatFile.Close();
            DatFile = null;
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
            String dir = BackupPrefix + @"\" + Strings[fileEntry.DirectoryIndex];
            int index = GetStringIndex(dir);

            // if the directory doesn't exist, add it.
            if (index == -1)
            {
                index = Strings.Count;
                Strings.Add(dir);
            }

            fileEntry.DirectoryIndex = index;
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



        private int GetStringIndex(String str)
        {
            if (String.IsNullOrEmpty(str)) return -1;

            return Strings.IndexOf(str);
        }

        private int AddString(String str)
        {
            int index = Strings.Count;
            Strings.Add(str);
            return index;
        }



        public override string ToString()
        {
            return FileNameWithoutExtension;
        }

        #region Dispose Methods
        public void Dispose()
        {
            if (DatFile != null)
            {
                DatFile.Dispose();
            }
        }
        #endregion
    }
}
