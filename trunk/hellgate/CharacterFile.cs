using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using Revival.Common;

namespace Hellgate
{
    public class CharacterFile : HellgateFile
    {
        public new const String Extension = ".hg1";
        public new const String ExtensionDeserialised = ".hg1.xml";
        public const int UnitObjectOffset = 0x2028;
        private const UInt32 RequiredVersion = 0x01; // 1
        private const UInt32 FileMagicWord = 0x484D4752;            // 'RGMH'

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct FileHeader
        {
            public UInt32 MagicWord;
            public UInt32 Version;
            public UInt32 DataOffset1;
            public UInt32 DataOffset2;
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

            byteOffset = UnitObjectOffset;
            Character.ParseUnitObject(fileBytes, byteOffset, fileBytes.Length - byteOffset);
        }

        public void CreateNewCharacter(String name)
        {
            Character = new UnitObject { Name = name };
        }

        public override byte[] ToByteArray()
        {
            byte[] characterBytes = Character.ToByteArray();
            byte[] buffer = new byte[UnitObjectOffset + characterBytes.Length];

            FileHeader fileHeader = new FileHeader
            {
                MagicWord = FileMagicWord,
                Version = RequiredVersion,
                DataOffset1 = UnitObjectOffset,
                DataOffset2 = UnitObjectOffset
            };

            FileTools.WriteToBuffer(ref buffer, 0, fileHeader);
            Buffer.BlockCopy(characterBytes, 0, buffer, UnitObjectOffset, characterBytes.Length);

            return buffer;
        }

        public override byte[] ExportAsDocument()
        {
            if (Character == null) throw new Exceptions.NotInitializedException();

            MemoryStream memoryStream = new MemoryStream();
            XmlSerializer xmlSerializer = new XmlSerializer(Character.GetType());
            xmlSerializer.Serialize(memoryStream, Character);

            return memoryStream.ToArray();
        }
    }
}