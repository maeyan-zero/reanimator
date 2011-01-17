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
    public class IndexFile : PackFile
    {
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
        public class FileEntryStruct
        {
            public UInt32 StartToken;                   //                  must be Token.Sect
            public UInt32 DirectoryHash;                // 0    0x00        this is the first 4 bytes of an SHA1 crypto hash of the folder path string
            public UInt32 NameHash;                     // 4    0x04        as above, but with file name string -  !!!do *not* change these values when patching out a file!!!
            public Int64 Offset;                        // 8    0x08        this is read in as 8 bytes
            public Int32 SizeUncompressed;              // 16   0x10
            public Int32 SizeCompressed;                // 20   0x14
            public Int32 Null1;                         // 24   0x18
            public Int32 DirectoryIndex;                // 28   0x1C
            public Int32 NameIndex;                     // 32   0x20
            public Int64 FileTime;                      // 36   0x24        can't be null - is a FILETIME (using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME)
            public Int32 Unknown23;                     // 44   0x2C        this and following unknown are the same for files that have the same compressed and uncompressed sizes
            public Int32 Unknown24;                     // 48   0x30        however they're different for different files with the same sizes...
            public Int32 Null21;                        // 52   0x34
            public Int32 Null22;                        // 56   0x38
            public Int32 Null23;                        // 60   0x3C
            public Int32 First4BytesOfFile;             // 64   0x40
            public Int32 Second4BytesOfFile;            // 68   0x44
            public UInt32 EndToken;                     //                  must be Token.Sect
        }

        public class FileEntry : PackFileEntry
        {
            private readonly FileEntryStruct _fileEntryStruct;

            public FileEntry(FileEntryStruct fileEntryStruct)
            {
                _fileEntryStruct = fileEntryStruct;
            }

            private String _directory;
            public override String Directory
            {
                get { return _directory; }
                set
                {
                    _directory = value;
                    DirectoryPatch = value;
                }
            }
            public override UInt32 DirectoryHash
            {
                get { return _fileEntryStruct.DirectoryHash; }
                set { _fileEntryStruct.DirectoryHash = value; }
            }

            [Browsable(false)]
            public int DirectoryIndex
            {
                get { return _fileEntryStruct.DirectoryIndex; }
                set { _fileEntryStruct.DirectoryIndex = value; }
            }

            internal String DirectoryPatch { get; private set; }

            public override Int64 FileTime
            {
                get { return _fileEntryStruct.FileTime; }
                set { _fileEntryStruct.FileTime = value; }
            }

            public override bool IsPatchedOut
            {
                get { return DirectoryPatch.Contains(PatchPrefix); }
                set
                {
                    if (value == IsPatchedOut) return;

                    DirectoryPatch = (value) ? System.IO.Path.Combine(PatchPrefix, DirectoryPatch) : DirectoryPatch.Replace(PatchPrefix, "");
                }
            }

            public override String Name { get; set; }
            public override UInt32 NameHash
            {
                get { return _fileEntryStruct.NameHash; }
                set { _fileEntryStruct.NameHash = value; }
            }

            [Browsable(false)]
            public Int32 NameIndex
            {
                get { return _fileEntryStruct.NameIndex; }
                set { _fileEntryStruct.NameIndex = value; }
            }

            public override Int64 Offset
            {
                get { return _fileEntryStruct.Offset; }
                set { _fileEntryStruct.Offset = value; }
            }

            internal Int32 Unknown23
            {
                get { return _fileEntryStruct.Unknown23; }
                set { _fileEntryStruct.Unknown23 = value; }
            }

            internal Int32 Unknown24
            {
                get { return _fileEntryStruct.Unknown24; }
                set { _fileEntryStruct.Unknown24 = value; }
            }

            internal Int32 First4BytesOfFile
            {
                get { return _fileEntryStruct.First4BytesOfFile; }
                set { _fileEntryStruct.First4BytesOfFile = value; }
            }

            internal Int32 Second4BytesOfFile
            {
                get { return _fileEntryStruct.Second4BytesOfFile; }
                set { _fileEntryStruct.Second4BytesOfFile = value; }
            }

            private String _path;
            public override String Path
            {
                get
                {
                    if (!String.IsNullOrEmpty(_path)) return _path;

                    return _path = System.IO.Path.Combine(Directory, Name);
                }

                set
                {
                    _path = value;
                    Name = System.IO.Path.GetFileName(value);
                    Directory = System.IO.Path.GetDirectoryName(value);
                }
            }

            public override Int32 SizeCompressed
            {
                get { return _fileEntryStruct.SizeCompressed; }
                set { _fileEntryStruct.SizeCompressed = value; }
            }

            public override Int32 SizeUncompressed
            {
                get { return _fileEntryStruct.SizeUncompressed; }
                set { _fileEntryStruct.SizeUncompressed = value; }
            }

        }
        #endregion

        #region Members
        public new const String Extension = ".idx";
        public new const String ExtensionDeserialised = ".idx.xml";
        public override String DatExtension { get { return ".dat"; } }
        private const UInt32 RequiredVersion = 0x04;
        private readonly static String[] NoCompressionExt = new[] { ".bik", ".ogg", ".mp2", ".wav" };
        public readonly static String[] NoPatchExt = new[] { ".bik", ".ogg", ".mp2", ".wav", StringsFile.Extention, "lightingmap.dds" };

        public String FilePath { get; set; }
        public bool HasIntegrity { get; private set; }

        private List<FileEntryStruct> FileDetails { get; set; }
        
        public bool DatFileOpen { get { return DatFile == null ? false : true; } }
        public int Count { get { return FileDetails.Count; } }
        #endregion

        /// <summary>
        /// Creates an empty Index file.
        /// </summary>
        public IndexFile(String filePath) : base (filePath)
        {
            FileDetails = new List<FileEntryStruct>();
        }

        /// <summary>
        /// Creates a new index file from the byte array.
        /// </summary>
        /// <param name="filePath">The full path of the index.</param>
        /// <param name="buffer">The index file as a byte array.</param>
        public IndexFile(String filePath, byte[] buffer) : this(filePath)
        {
            ParseFileBytes(buffer);
        }

        /// <summary>
        /// Parses a serialized version of an index file.
        /// </summary>
        /// <param name="buffer">Encrypted or decrypted index byte array.</param>
        /// <returns>Returns false if the file is malformed.</returns>
        public override sealed void ParseFileBytes(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0) throw new ArgumentNullException("buffer", "buffer cannot be empty!");

            // Check for encryption
            int offset = 0;
            UInt32 fileHeadToken = BitConverter.ToUInt32(buffer, 0);
            if (fileHeadToken != Token.Head)
            {
                Crypt.Decrypt(buffer);
            }

            // Read the file header, check for errors
            FileHeader fileHeader = FileTools.ByteArrayToStructure<FileHeader>(buffer, ref offset);
            if (fileHeader.FileToken != Token.Head) throw new Exceptions.UnexpectedTokenException("Expected head token but got " + fileHeader.FileToken.ToString("X"));
            if (fileHeader.FileVersion != RequiredVersion) throw new Exceptions.NotSupportedFileVersionException(RequiredVersion, fileHeader.FileVersion);

            // Read the strings section
            StringsHeader stringsHeader = FileTools.ByteArrayToStructure<StringsHeader>(buffer, ref offset);
            if (stringsHeader.StringsToken != Token.Sect) throw new Exceptions.UnexpectedTokenException(Token.Sect, stringsHeader.StringsToken);
            List<String> strings = new List<String>();
            for (int i = 0; i < stringsHeader.StringsCount; i++)
            {
                strings.Add(FileTools.ByteArrayToStringASCII(buffer, offset));
                offset += strings[i].Length + 1; // +1 for \0
            }

            // String Details
            UInt32 stringsDetailsToken = FileTools.ByteArrayToUInt32(buffer, ref offset);
            if (stringsDetailsToken != Token.Sect) throw new Exceptions.UnexpectedTokenException(Token.Sect, stringsDetailsToken);
            // Skip over the details struct because we don't need it.
            offset += stringsHeader.StringsCount * Marshal.SizeOf(typeof(StringDetailsStruct));

            // Files Structure details
            UInt32 filesToken = FileTools.ByteArrayToUInt32(buffer, ref offset);
            if (filesToken != Token.Sect) throw new Exceptions.UnexpectedTokenException(Token.Sect, stringsDetailsToken);
            FileDetails.AddRange(FileTools.ByteArrayToArray<FileEntryStruct>(buffer, ref offset, fileHeader.FileCount));

            // The Files list is the public interface
            for (int i = 0; i < fileHeader.FileCount; i++)
            {
                PackFileEntry fileEntry = new FileEntry(FileDetails[i])
                {
                    Pack = this,
                    Path = System.IO.Path.Combine(strings[FileDetails[i].DirectoryIndex], strings[FileDetails[i].NameIndex]),
                };

                Files.Add(fileEntry);
            }

            HasIntegrity = true;
        }

        /// <summary>
        /// Generates a serialized version of the index file.
        /// </summary>
        /// <returns>Returns the index file as an encrypted byte array.</returns>
        public override byte[] ToByteArray()
        {
            byte[] buffer = new byte[1024];
            int offset = 0;

            // file header
            FileTools.WriteToBuffer(ref buffer, ref offset, Token.Head);
            FileTools.WriteToBuffer(ref buffer, ref offset, RequiredVersion);
            FileTools.WriteToBuffer(ref buffer, ref offset, Files.Count);


            // update strings
            List<String> strings = new List<String>();
            foreach (FileEntry fileEntry in Files)
            {
                fileEntry.NameIndex = _GetAddStringIndex(strings, fileEntry.Name);
                fileEntry.DirectoryIndex = _GetAddStringIndex(strings, fileEntry.DirectoryPatch);
            }


            // strings
            FileTools.WriteToBuffer(ref buffer, ref offset, Token.Sect);
            FileTools.WriteToBuffer(ref buffer, ref offset, strings.Count);
            int stringByteCountOffset = offset;
            offset += 4;
            foreach (String str in strings)
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, FileTools.StringToASCIIByteArray(str));
                offset++; // \0
            }
            UInt32 strByteArrayLen = (UInt32)(offset - stringByteCountOffset - sizeof(UInt32));
            FileTools.WriteToBuffer(ref buffer, stringByteCountOffset, strByteArrayLen);

            // String Details Struct
            FileTools.WriteToBuffer(ref buffer, ref offset, Token.Sect);
            foreach (String str in strings)
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, (Int16)str.Length);
                FileTools.WriteToBuffer(ref buffer, ref offset, Crypt.GetStringHash(str));
            }

            // File Structure Block
            FileTools.WriteToBuffer(ref buffer, ref offset, Token.Sect);


            foreach (FileEntry fileEntry in Files)
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.Info);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileEntry.DirectoryHash);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileEntry.NameHash);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileEntry.Offset);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileEntry.SizeUncompressed);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileEntry.SizeCompressed);
                offset += 4; // null
                FileTools.WriteToBuffer(ref buffer, ref offset, fileEntry.DirectoryIndex);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileEntry.NameIndex);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileEntry.FileTime);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileEntry.Unknown23);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileEntry.Unknown24);
                offset += 12; // null
                FileTools.WriteToBuffer(ref buffer, ref offset, fileEntry.First4BytesOfFile);
                FileTools.WriteToBuffer(ref buffer, ref offset, fileEntry.Second4BytesOfFile);
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.Info);
            }


            // resize, encrypt, and return
            Array.Resize(ref buffer, offset);
            Crypt.Encrypt(buffer);
            return buffer;
        }

        private static int _GetAddStringIndex(IList<String> strings, String str)
        {
            int index = strings.IndexOf(str);
            if (index != -1) return index;

            index = strings.Count;
            strings.Add(str);
            return index;
        }

        /// <summary>
        /// Appends a file to the index and accompanying data file.
        /// </summary>
        /// <param name="directory">The directory that will be stored in the index.</param>
        /// <param name="fileName">The filename that will be stored in the index.</param>
        /// <param name="bytesToWrite">Byte array of the file to add.</param>
        /// <param name="fileTime">File time to set to the file.</param>
        /// <returns>Returns true if the operation was successful.</returns>
        public override bool AddFile(String directory, String fileName, byte[] bytesToWrite, DateTime ?fileTime = null)
        {
            if (bytesToWrite == null || bytesToWrite.Length == 0) throw new ArgumentNullException("bytesToWrite", "Bytes to write cannot be empty!");

            // ensure .dat file open
            BeginDatWriting();

            bool doCompress = NoCompressionExt.All(extentsion => !fileName.EndsWith(extentsion));
            if (fileTime == null) fileTime = DateTime.Now;

            FileEntryStruct fileStruct = new FileEntryStruct
            {
                StartToken = Token.Info,
                SizeUncompressed = bytesToWrite.Length,
                SizeCompressed = doCompress ? 1 : 0,
                FileTime = fileTime.Value.ToFileTime(),
                NameHash = Crypt.GetStringSHA1UInt32(fileName),
                DirectoryHash = Crypt.GetStringSHA1UInt32(directory),
                First4BytesOfFile = FileTools.ByteArrayToInt32(bytesToWrite, 0),
                Second4BytesOfFile = FileTools.ByteArrayToInt32(bytesToWrite, 4),
                EndToken = Token.Info,
            };

            PackFileEntry fileEntry = new FileEntry(fileStruct);
            Files.Add(fileEntry);

            _AddFileToDat(bytesToWrite, fileEntry);

            // todo: create new applicable file entry object as well?
            // todo: remove existing file bytes if applicable...

            return true;
        }

        protected override void WriteDatHeader()
        {
            DatHeader datHeader = new DatHeader();
            DatFile.Write(FileTools.StructureToByteArray(datHeader), 0, Marshal.SizeOf(datHeader));
        }

         //<summary>
         //Patch out a file in the index by prepending a junk string to the entry.
         //This function does not modify the file details string hashes.
         //</summary>
         //<param name="i">The index to the file entry to be patched out.</param>
         //<returns>True upon success, of false if file is already patched out or on invalid index.</returns>
        //public bool PatchOutFile(int i)
        //{
        //    if (i < 0 || i > Files.Count) return false;

        //    FileEntry fileIndex = Files[i];
        //    return PatchOutFile(fileIndex);
        //}

         //<summary>
         //Patch out a file in the index by prepending a junk string to the entry.
         //This function does not modify the file details string hashes.
         //</summary>
         //<param name="fileEntry">The file entry to patch out.</param>
         //<returns>True upon success, of false if file is already patched out.</returns>
        //public bool PatchOutFile(FileEntry fileEntry)
        //{
        //    Debug.Assert(fileEntry != null);

        //    // has it already be patched out?
        //    if (fileEntry.DirectoryString.Contains(PatchPrefix)) return false;

        //    // does the backup dir exist?
        //    String dir = PatchPrefix + Strings[fileEntry.DirectoryIndex];
        //    int index = Strings.IndexOf(dir);

        //    // if the directory doesn't exist, add it.
        //    if (index == -1)
        //    {
        //        index = Strings.Count;
        //        Strings.Add(dir);
        //    }

        //    fileEntry.DirectoryIndex = index;
        //    fileEntry.DirectoryString = dir;
        //    return true;
        //}

         //<summary>
         //Patch in a file in the index by removing the prepended junk string on the entry.
         //The function does not modify the file details string hashes (they shouldn't need to be changed).
         //</summary>
         //<param name="fileEntry">The file entry to be patched back in.</param>
         //<returns>True if file has been patched back in, false if file was never patched out.</returns>
        //public override bool PatchInFile(PackFileEntry fileEntry)
        //{
        //    Debug.Assert(fileEntry != null);

        //    // does it even need to be patched in?
        //    if (!fileEntry.DirectoryString.Contains(PatchPrefix)) return false;

        //    // does the original dir exist?
        //    int index = Strings.IndexOf(fileEntry.DirectoryStringWithoutPatch);

        //    // if the directory doesn't exist, add it.
        //    if (index == -1)
        //    {
        //        index = Strings.Count;
        //        Strings.Add(fileEntry.DirectoryStringWithoutPatch);
        //    }

        //    fileEntry.DirectoryIndex = index;
        //    fileEntry.DirectoryString = fileEntry.DirectoryStringWithoutPatch;
        //    return true;
        //}

        /// <summary>
        /// Performs a deep file entry check, patching in all files and correcting all path hashes.
        /// </summary>
        /// <returns>True upon repair modifications. False if no modifications were made.</returns>
        public bool Repair()
        {
            bool modified = false;

            // check all files entries
            foreach (PackFileEntry fileEntry in Files)
            {
                if (fileEntry.IsPatchedOut)
                {
                    fileEntry.IsPatchedOut = false;
                    modified = true;
                }

                UInt32 directoryHash = Crypt.GetStringSHA1UInt32(fileEntry.Directory);
                UInt32 nameHash = Crypt.GetStringSHA1UInt32(fileEntry.Name);

                UInt64 pathHash = directoryHash | (UInt64)nameHash << 32;
                if (fileEntry.PathHash == pathHash) continue;

                fileEntry.DirectoryHash = directoryHash;
                fileEntry.NameHash = nameHash;
                modified = true;
            }

            return modified;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override byte[] ExportAsDocument()
        {
            throw new NotImplementedException();
        }
    }
}
