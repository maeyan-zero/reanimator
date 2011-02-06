using System;
using System.Runtime.InteropServices;
using Revival.Common;

namespace Hellgate
{
    public class CharacterFile : HellgateFile
    {
        public new const String Extension = ".hg1";
        public new const String ExtensionDeserialised = ".hg1.xml";
        private const UInt32 RequiredVersion = 0x01; // 1
        private const UInt32 FileMagicWord = 0x484D4752;            // 'RGMH'

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct FileHeader
        {
            public Int32 MagicWord;
            public Int32 Version;
            public Int32 DataOffset1;
            public Int32 DataOffset2;
        };

        public String Path { get; private set; }
        public UnitObject Character { get; private set; }
        public String Name { get { return Character.Name; } }

        public CharacterFile(String filePath)
        {
            Path = filePath;
        }

        public override void ParseFileBytes(byte[] fileBytes)
        {
            ParseFileBytes(fileBytes, false);
        }

        public void ParseFileBytes(byte[] fileBytes, bool debugOutputLoadingProgress)
        {
            // sanity check
            if (fileBytes == null) throw new ArgumentNullException("fileBytes", "File bytes cannot be null!");

            Character = new UnitObject(debugOutputLoadingProgress);
            int byteOffset = 0;

            // main file header
            FileHeader fileHeader = FileTools.ByteArrayToStructure<FileHeader>(fileBytes, ref byteOffset);

            // file header checks
            if (fileHeader.MagicWord != FileMagicWord) throw new Exceptions.UnexpectedMagicWordException();
            if (fileHeader.Version != RequiredVersion) throw new Exceptions.NotSupportedFileVersionException();

            byteOffset = 0x2028;
            Character.ParseUnitObject(fileBytes, byteOffset);
        }

        public void CreateNewCharacter(String name)
        {
            Character = new UnitObject { Name = name };
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