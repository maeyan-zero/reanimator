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
    public class IndexFile : IDisposable
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

        abstract class Token
        {
            public const UInt32 Head = 0x6867696E;      // 'nigh'
            public const UInt32 Sect = 0x68677073;      // 'spgh'
            public const UInt32 Info = 0x6867696F;      // 'oigh'
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
        struct StringDetailsStruct                      // this is a structure definition only - not used
        {
            private readonly short StringLength;        // the length of the string, not including \0 i.e. String.Length
            private readonly UInt32 StringHash;         // string hash generated from Crypt.GetStringHash
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class FileDetailsStruct
        {
            public UInt32 StartToken;                   //                  must be Token.Sect
            public UInt32 FolderPathSHA1Hash;           // 0    0x00        this is the first 4 bytes of an SHA1 crypto hash of the folder path string
            public UInt32 FileNameSHA1Hash;             // 4    0x04        as above, but with file name string -  !!!do *not* change these values when patching out a file!!!
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
            public UInt32 EndToken;                     //                  must be Token.Sect
        }

        public class FileEntry
        {
            [Browsable(false)]
            public FileDetailsStruct FileStruct { get; set; }

            [Browsable(false)]
            public IndexFile Index { get; set; }

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
                    _GenerateRelativeFullPath();
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
                    _GenerateRelativeFullPath();
                }
            }

            public String RelativeFullPath { get; private set; }
            private void _GenerateRelativeFullPath()
            {
                if (_directoryString == null || _fileNameString == null) return;
                RelativeFullPath = _directoryString + _fileNameString;
                RelativeFullPathWithoutPatch = RelativeFullPath.Replace(PatchPrefix, "");
                DirectoryStringWithoutPatch = _directoryString.Replace(PatchPrefix, "");
            }

            public String DirectoryStringWithoutPatch { get; private set; }
            public String RelativeFullPathWithoutPatch { get; private set; }

            public bool IsPatchedOut
            {
                get { return DirectoryString != null && DirectoryString.Contains(PatchPrefix); }
            }

            public UInt64 LongPathHash
            {
                get { return FileStruct.FolderPathSHA1Hash | ((UInt64)FileStruct.FileNameSHA1Hash << 32); }
            }

            public long FileTime
            {
                get { return FileStruct.FileTime; }
            }

            [Browsable(false)]
            public List<FileEntry> Siblings;
        }
        #endregion

        #region Members
        public const String FileExtension = ".idx";
        public const String DatFileExtension = ".dat";
        private const String PatchPrefix = @"backup\";
        private const UInt32 RequiredVersion = 0x04;
        private readonly static String[] NoCompressionExt = new[] { ".bik", ".ogg", ".mp2", ".wav" };
        public readonly static String[] NoPatchExt = new[] { ".bik", ".ogg", ".mp2", ".wav", StringsFile.Extention, "lightingmap.dds" };

        public String FilePath { get; set; }
        public String FileDirectory { get { return Path.GetDirectoryName(FilePath); } }
        public String FileNameWithoutExtension { get { return Path.GetFileNameWithoutExtension(FilePath); } }
        public bool HasIntegrity { get; private set; }
        public List<FileEntry> Files { get; private set; }
        private StringCollection Strings { get; set; }
        private List<FileDetailsStruct> FileDetails { get; set; }
        private FileStream DatFile { get; set; }
        public bool DatFileOpen { get { return DatFile == null ? false : true; } }
        public bool Modified { get { return Files.Any(file => file.DirectoryString.Contains(PatchPrefix)); } }
        public int Count { get { return FileDetails.Count; } }
        #endregion



        /// <summary>
        /// Creates an empty Index file.
        /// </summary>
        public IndexFile()
        {
            Strings = new StringCollection();
            FileDetails = new List<FileDetailsStruct>();
            Files = new List<FileEntry>();
        }

        /// <summary>
        /// Creates a new index file from the byte array.
        /// </summary>
        /// <param name="buffer">The index file as a byte array.</param>
        public IndexFile(byte[] buffer) : this()
        {
            HasIntegrity = ParseData(buffer);
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

            // Check for encryption
            UInt32 fileHeadToken = BitConverter.ToUInt32(buffer, 0);
            if (fileHeadToken != Token.Head)
            {
                Crypt.Decrypt(buffer);
            }

            // Read the file header, check for errors
            FileHeader fileHeader = FileTools.ByteArrayToStructure<FileHeader>(buffer, ref offset);
            if (fileHeader.FileToken != Token.Head) return false;
            if (fileHeader.FileVersion != RequiredVersion) return false;

            // Read the strings section
            StringsHeader stringsHeader = FileTools.ByteArrayToStructure<StringsHeader>(buffer, ref offset);
            if (stringsHeader.StringsToken != Token.Sect) return false;
            for (int i = 0; i < stringsHeader.StringsCount; i++)
            {
                Strings.Add(FileTools.ByteArrayToStringASCII(buffer, offset));
                offset += Strings[i].Length + 1; // +1 for \0
            }

            // String Details
            UInt32 stringsDetailsToken = FileTools.ByteArrayToUInt32(buffer, ref offset);
            if (stringsDetailsToken != Token.Sect) return false;
            // Skip over the details struct because we don't need it.
            offset += stringsHeader.StringsCount * Marshal.SizeOf(typeof(StringDetailsStruct));

            // Files Structure details
            UInt32 filesToken = FileTools.ByteArrayToUInt32(buffer, ref offset);
            if (filesToken != Token.Sect) return false;
            FileDetails.AddRange(FileTools.ByteArrayToArray<FileDetailsStruct>(buffer, ref offset, fileHeader.FileCount));

            // The Files list is the public interface
            for (int i = 0; i < fileHeader.FileCount; i++)
            {
                FileEntry fileEntry = new FileEntry
                {
                    FileStruct = FileDetails[i],
                    Index = this,
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
        public byte[] ToByteArray()
        {
            byte[] buffer = new byte[1024];
            int offset = 0;

            // Header
            FileTools.WriteToBuffer(ref buffer, ref offset, Token.Head);
            FileTools.WriteToBuffer(ref buffer, ref offset, RequiredVersion);
            FileTools.WriteToBuffer(ref buffer, ref offset, FileDetails.Count);

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
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.Info);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.FolderPathSHA1Hash);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.FileNameSHA1Hash);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.DataOffset);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.UncompressedSize);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.CompressedSize);
                offset += 4; // null
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.DirectoryArrayIndex);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.FilenameArrayIndex);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.FileTime);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.Unknown23);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.Unknown24);
                offset += 12; // null
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.First4BytesOfFile);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileIndex.Second4BytesOfFile);
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.Info);
            }

            // Resize the buffer and return
            Array.Resize(ref buffer, offset);
            return buffer;
        }

        /// <summary>
        /// Appends a file to the index and accompanying data file.
        /// </summary>
        /// <param name="directory">The directory that will be stored in the index.</param>
        /// <param name="fileName">The filename that will be stored in the index.</param>
        /// <param name="bytesToWrite">Byte array of the file to add.</param>
        /// <param name="fileTime">File time to set to the file.</param>
        /// <returns>Returns true if the operation was successful.</returns>
        public bool AddFile(String directory, String fileName, byte[] bytesToWrite, DateTime fileTime)
        {
            if (bytesToWrite == null) return false;
            if (bytesToWrite.Length <= 0) return false;

            // ensure .dat file open
            if (!OpenDat(FileAccess.ReadWrite)) return false;

            bool doCompress = NoCompressionExt.All(extentsion => !fileName.EndsWith(extentsion));

            // Create new Index File Details Struct
            FileDetailsStruct fileStruct = new FileDetailsStruct
            {
                UncompressedSize = bytesToWrite.Length,
                FileTime = fileTime.ToFileTime(),
                FileNameSHA1Hash = Crypt.GetStringSHA1UInt32(fileName),
                FolderPathSHA1Hash = Crypt.GetStringSHA1UInt32(directory),
                StartToken = Token.Info,
                EndToken = Token.Info
            };


            // See if the index already contains the Directory string
            int directoryIndex = Strings.IndexOf(directory);
            if (directoryIndex != -1)
            {
                fileStruct.DirectoryArrayIndex = directoryIndex;
            }
            else // otherwise add it
            {
                Strings.Add(directory);
                fileStruct.DirectoryArrayIndex = Strings.Count - 1;
            }

            // See if the index already contains the FileName string
            int fileNameIndex = Strings.IndexOf(fileName);
            if (fileNameIndex != -1)
            {
                fileStruct.FilenameArrayIndex = fileNameIndex;
            }
            else
            {
                Strings.Add(fileName);
                fileStruct.FilenameArrayIndex = Strings.Count - 1;
            }

            fileStruct.CompressedSize = doCompress ? 1 : 0;
            fileStruct.First4BytesOfFile = FileTools.ByteArrayToInt32(bytesToWrite, 0);
            fileStruct.Second4BytesOfFile = FileTools.ByteArrayToInt32(bytesToWrite, 4);

            _AddFileToDat(bytesToWrite, fileStruct);
            FileDetails.Add(fileStruct);

            // todo: create new applicable file entry object as well?
            // todo: remove existing file bytes if applicable...

            return true;
        }

        /// <summary>
        /// Adds file bytes to the accompanying .dat file.
        /// Does not remove old/duplicate file bytes from duplicate or new version additions.
        /// </summary>
        /// <param name="fileData">The file byte array to add.</param>
        /// <param name="fileStruct">The file structure details.</param>
        private void _AddFileToDat(byte[] fileData, FileDetailsStruct fileStruct)
        {
            Debug.Assert(DatFile != null && fileData != null && fileStruct != null);

            DatFile.Seek(0, SeekOrigin.End);

            byte[] writeBuffer;
            int writeLength;
            if (fileStruct.CompressedSize > 0)
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

                fileStruct.CompressedSize = writeLength;
            }
            else
            {
                writeBuffer = fileData;
                fileStruct.CompressedSize = 0;
                writeLength = fileData.Length;
            }

            fileStruct.DataOffset = (int)DatFile.Position;
            fileStruct.UncompressedSize = fileData.Length;
            DatFile.Write(writeBuffer, 0, writeLength);
        }

        /// <summary>
        /// Reads the accompanying .dat file for the file.
        /// </summary>
        /// <param name="file">The file to be read.</param>
        /// <returns>A byte array of the files bytes, or null on error.</returns>
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

        /// <summary>
        /// Close the .dat file if open.
        /// </summary>
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
                    DatFile.Write(FileTools.StructureToByteArray(datHeader), 0, Marshal.SizeOf(datHeader));
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        public FileEntry GetFileEntry(String relativePath)
        {
            return Files.FirstOrDefault(fileEntry => fileEntry.RelativeFullPath == relativePath ||
                                                     fileEntry.RelativeFullPathWithoutPatch == relativePath ||
                                                     relativePath.EndsWith(fileEntry.RelativeFullPath) ||
                                                     relativePath.EndsWith(fileEntry.RelativeFullPathWithoutPatch));
        }

        /// <summary>
        /// Patch out a file in the index by prepending a junk string to the entry.
        /// This function does not modify the file details string hashes.
        /// </summary>
        /// <param name="i">The index to the file entry to be patched out.</param>
        /// <returns>True upon success, of false if file is already patched out or on invalid index.</returns>
        public bool PatchOutFile(int i)
        {
            if (i < 0 || i > Files.Count) return false;

            FileEntry fileIndex = Files[i];
            return PatchOutFile(fileIndex);
        }

        /// <summary>
        /// Patch out a file in the index by prepending a junk string to the entry.
        /// This function does not modify the file details string hashes.
        /// </summary>
        /// <param name="fileEntry">The file entry to patch out.</param>
        /// <returns>True upon success, of false if file is already patched out.</returns>
        public bool PatchOutFile(FileEntry fileEntry)
        {
            Debug.Assert(fileEntry != null);

            // has it already be patched out?
            if (fileEntry.DirectoryString.Contains(PatchPrefix)) return false;

            // does the backup dir exist?
            String dir = PatchPrefix + Strings[fileEntry.DirectoryIndex];
            int index = Strings.IndexOf(dir);

            // if the directory doesn't exist, add it.
            if (index == -1)
            {
                index = Strings.Count;
                Strings.Add(dir);
            }

            fileEntry.DirectoryIndex = index;
            fileEntry.DirectoryString = dir;
            return true;
        }

        /// <summary>
        /// Patch in a file in the index by removing the prepended junk string on the entry.
        /// The function does not modify the file details string hashes (they shouldn't need to be changed).
        /// </summary>
        /// <param name="fileEntry">The file entry to be patched back in.</param>
        /// <returns>True if file has been patched back in, false if file was never patched out.</returns>
        public bool PatchInFile(FileEntry fileEntry)
        {
            Debug.Assert(fileEntry != null);

            // does it even need to be patched in?
            if (!fileEntry.DirectoryString.Contains(PatchPrefix)) return false;

            // does the original dir exist?
            int index = Strings.IndexOf(fileEntry.DirectoryStringWithoutPatch);

            // if the directory doesn't exist, add it.
            if (index == -1)
            {
                index = Strings.Count;
                Strings.Add(fileEntry.DirectoryStringWithoutPatch);
            }

            fileEntry.DirectoryIndex = index;
            fileEntry.DirectoryString = fileEntry.DirectoryStringWithoutPatch;
            return true;
        }

        /// <summary>
        /// Performs a deep file entry check, patching in all files and correcting all path hashes.
        /// </summary
        /// <returns>True upon repair modifications. False if no modifications were made.</returns>
        public bool Repair()
        {
            bool modified = false;

            // check all files entries
            foreach (FileEntry fileEntry in Files)
            {
                if (fileEntry.IsPatchedOut)
                {
                    PatchInFile(fileEntry);
                    modified = true;
                }

                UInt32 folderPathHash = Crypt.GetStringSHA1UInt32(fileEntry.DirectoryString);
                UInt32 fileNameHash = Crypt.GetStringSHA1UInt32(fileEntry.FileNameString);

                UInt64 pathHash = folderPathHash | (UInt64)fileNameHash << 32;
                if (fileEntry.LongPathHash == pathHash) continue;

                fileEntry.FileStruct.FolderPathSHA1Hash = folderPathHash;
                fileEntry.FileStruct.FileNameSHA1Hash = fileNameHash;
                modified = true;
            }

            return modified;
        }

        /// <summary>
        /// Override of ToString() function.
        /// </summary>
        /// <returns>The index filename without the index file extension.</returns>
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
