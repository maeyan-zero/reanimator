using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Revival.Common;

namespace Hellgate
{
    public class HellgatePackFile : PackFile
    {
        public new const String Extension = ".hpt";
        public new const String ExtensionDeserialised = ".hpt.xml";
        public override String DatExtension { get { return ".hpd"; } }
        private const UInt32 FileMagicWord = 0xF0F0F0F0; // 'ðððð'
        private const UInt32 DatMagicWord = 0x0F0F0F0F; // ''

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private class DatHeader
        {
            public UInt32 MagicWord = DatMagicWord;
            public Int32 Version = 0;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private class Header
        {
            public UInt32 MagicWord;
            public UInt32 Unknown1;
            public UInt32 Unknown2;
            public UInt32 Unknown3;
            public UInt32 Unknown4;
            public UInt32 Unknown5;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class FileEntryStruct
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public String Path;
            public Int32 Size;
            public Int64 FileTime; // is a FILETIME (using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME)
            public UInt32 Unknown1;
            public UInt32 Unknown2;
            public UInt32 Unknown3;
            public Int64 Offset;
            public UInt32 Unknown4;
            public UInt32 Unknown5;
            public UInt32 Unknown6;
            public UInt32 Unknown7;
        }

        public class FileEntry : PackFileEntry
        {
            private readonly FileEntryStruct _fileEntryStruct;

            public FileEntry(FileEntryStruct fileEntryStruct)
            {
                _fileEntryStruct = fileEntryStruct;
            }

            public override Int64 Offset
            {
                get { return _fileEntryStruct.Offset; }
                set { _fileEntryStruct.Offset = value; }
            }

            public override Int32 SizeCompressed
            {
                get { return 0; }
                set { }
            }

            public override Int32 SizeUncompressed
            {
                get { return _fileEntryStruct.Size; }
                set { _fileEntryStruct.Size = value; }
            }

            public override String Directory
            {
                get { return System.IO.Path.GetDirectoryName(_fileEntryStruct.Path).Replace(PatchPrefix, ""); }
                set { _fileEntryStruct.Path = System.IO.Path.Combine(value, Name); }
            }

            public override UInt32 DirectoryHash
            {
                get { return Crypt.GetStringSHA1UInt32(Directory + @"\"); }
                set { }
            }

            public override String Name
            {
                get { return System.IO.Path.GetFileName(_fileEntryStruct.Path); }
                set { _fileEntryStruct.Path = System.IO.Path.Combine(Directory, value); }
            }

            public override uint NameHash
            {
                get { return Crypt.GetStringSHA1UInt32(Name); }
                set { }
            }

            public override String Path
            {
                get { return _fileEntryStruct.Path.Replace(PatchPrefix, ""); }
                set { _fileEntryStruct.Path = value; }
            }

            public String TruePath
            {
                get { return _fileEntryStruct.Path; }
                set { _fileEntryStruct.Path = value; }
            }

            public override bool IsPatchedOut
            {
                get { return _fileEntryStruct.Path.Contains(PatchPrefix); }
                set
                {
                    if (value == IsPatchedOut) return;

                    _fileEntryStruct.Path = (value) ? System.IO.Path.Combine(PatchPrefix, Path) : Path;
                }
            }

            public override Int64 FileTime
            {
                get { return  _fileEntryStruct.FileTime; }
                set { _fileEntryStruct.FileTime = value; }
            }
        }

        private Header _header;
        private FileEntryStruct[] _fileEntryStructs;
        private FileStream _datFile;

        public HellgatePackFile(String filePath) : base (filePath) { }

        public override bool AddFile(string directory, string fileName, byte[] bytes, DateTime? fileTime = null)
        {
            throw new NotImplementedException();
        }

        protected override void WriteDatHeader()
        {
            DatHeader datHeader = new DatHeader();
            DatFile.Write(FileTools.StructureToByteArray(datHeader), 0, Marshal.SizeOf(datHeader));
        }

        public override void ParseFileBytes(byte[] fileBytes)
        {
            int offset = 0;
            FileInfo fileInfo = new FileInfo(Path);


            // read header
            _header = FileTools.ByteArrayToStructure<Header>(fileBytes, ref offset);
            if (_header.MagicWord != FileMagicWord) return; // todo add exception


            // read file entries
            int bytesRemaining = (int) (fileInfo.Length - offset);
            int structSize = Marshal.SizeOf(typeof (FileEntryStruct));
            if (bytesRemaining % structSize != 0)
            {
                // todo: add fail/warning?
            }
            int fileEntryCount = bytesRemaining / structSize;
            _fileEntryStructs = FileTools.ByteArrayToArray<FileEntryStruct>(fileBytes, ref offset, fileEntryCount);


            // process file entries
            foreach (FileEntryStruct fileEntryStruct in _fileEntryStructs)
            {
                Files.Add(new FileEntry(fileEntryStruct) { Pack = this });
            }
        }

        public override byte[] ToByteArray()
        {
            throw new NotImplementedException();
        }

        public override byte[] ExportAsDocument()
        {
            throw new NotImplementedException();
        }
    }
}