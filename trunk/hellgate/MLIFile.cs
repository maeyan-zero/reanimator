using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using Revival.Common;

namespace Hellgate
{
    public class MLIFile : HellgateFile
    {
        public new const String Extension = ".mli";
        public new const String ExtensionDeserialised = ".mli.xml";
        private const UInt32 FileMagicWord = 0x1515CAFE; // 'þÊ'
        private const UInt32 RequiredVersion = 0x3; // 3

        // total size = 12 bytes (0x0C)
        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class UnknownStruct1
        {
            public Int32 UnknownInt321;         // 0x00     0
            public Int32 UnknownInt322;         // 0x04     4
            public Int32 UnknownInt323;         // 0x08     8
            // end of struct                    // 0x0C     12
        }

        // total size = 12 bytes (0x0C)
        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class UnknownStruct2
        {
            public float UnknownFloat1;         // 0x00     0
            public float UnknownFloat2;         // 0x04     4
            public float UnknownFloat3;         // 0x08     8
            // end of struct                    // 0x0C     12
        }

        // total size = 44 bytes (0x2C)
        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class UnknownStruct3
        {
            public Int32 UnknownInt32;          // 0x00     0
            public float UnknownFloat1;         // 0x04     4
            public float UnknownFloat2;         // 0x08     8
            public float UnknownFloat3;         // 0x0C     12
            public float UnknownFloat4;         // 0x10     16
            public float UnknownFloat5;         // 0x14     20
            public float UnknownFloat6;         // 0x18     24
            public float UnknownFloat7;         // 0x1C     28
            public float UnknownFloat8;         // 0x20     32
            public float UnknownFloat9;         // 0x24     36
            public float UnknownFloat10;        // 0x28     40
            // end of struct                    // 0x2C     44
        }

        public class MLIStruct
        {
            public UnknownStruct1[] UnknownStruct1Array;
            public short[] UnknownShortArray;
            public UnknownStruct2[] UnknownStruct2Array;
            public UnknownStruct3[] UnknownStruct3Array1;
            public UnknownStruct3[] UnknownStruct3Array2;
        }

        private MLIStruct _mliStruct;

        /// <summary>
        /// Parses a level rules file bytes.
        /// </summary>
        /// <param name="fileBytes">The bytes of the level rules to parse.</param>
        public override void ParseFileBytes(byte[] fileBytes)
        {
            // sanity check
            if (fileBytes == null) throw new ArgumentNullException("fileBytes", "File bytes cannot be null!");
            _mliStruct = new MLIStruct();

            // file header checks
            int offset = 0;
            UInt32 fileMagicWord = FileTools.ByteArrayToUInt32(fileBytes, ref offset);
            if (fileMagicWord != FileMagicWord) throw new Exceptions.UnexpectedMagicWordException();

            UInt32 fileVersion = FileTools.ByteArrayToUInt32(fileBytes, ref offset);
            if (fileVersion != RequiredVersion) throw new Exceptions.NotSupportedFileVersionException();

            // first block
            int flag = FileTools.ByteArrayToInt32(fileBytes, ref offset);
            int count = FileTools.ByteArrayToInt32(fileBytes, ref offset);
            if (flag == 1)
            {
                _mliStruct.UnknownStruct1Array = FileTools.ByteArrayToArray<UnknownStruct1>(fileBytes, ref offset, count);
            }

            if (flag == 1 && _mliStruct.UnknownStruct1Array[0].UnknownInt321 == 0x0A)
            {
                // shorts block
                int shortCount = FileTools.ByteArrayToInt32(fileBytes, ref offset);
                if (shortCount > 0)
                {
                    _mliStruct.UnknownShortArray = FileTools.ByteArrayToShortArray(fileBytes, ref offset, shortCount);
                }

                // floats block
                int floatTripletCount = FileTools.ByteArrayToInt32(fileBytes, ref offset);
                if (floatTripletCount > 0)
                {
                    _mliStruct.UnknownStruct2Array = FileTools.ByteArrayToArray<UnknownStruct2>(fileBytes, ref offset, floatTripletCount);
                }
            }

            // last two unknown blocks
            int count1 = FileTools.ByteArrayToInt32(fileBytes, fileBytes.Length - 8);
            int count2 = FileTools.ByteArrayToInt32(fileBytes, fileBytes.Length - 4);
            if (count1 > 0)
            {
                _mliStruct.UnknownStruct3Array1 = FileTools.ByteArrayToArray<UnknownStruct3>(fileBytes, ref offset, count1);
            }
            if (count2 > 0)
            {
                _mliStruct.UnknownStruct3Array2 = FileTools.ByteArrayToArray<UnknownStruct3>(fileBytes, ref offset, count2);
            }
            offset += 8; // for 2x count fields

            // final debug check
            Debug.Assert(offset == fileBytes.Length);
        }



        /// <summary>
        /// Parses and XML document and returns the serialized byte array.
        /// </summary>
        /// <param name="xmlDocument">The XML Document to parse.</param>
        /// <returns>The serialized byte array.</returns>
        public void ParseXmlDocument(XmlDocument xmlDocument)
        {
            if (xmlDocument == null) throw new ArgumentNullException("xmlDocument", "XML Document cannot be null!");

            XmlNodeReader xmlNodeReader = new XmlNodeReader(xmlDocument);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof (MLIStruct));
            _mliStruct = (MLIStruct) xmlSerializer.Deserialize(xmlNodeReader);
        }

        public override byte[] ToByteArray()
        {
            if (_mliStruct == null) throw new Exceptions.NotInitializedException();

            int offset = 0;
            byte[] fileBytes = new byte[1024];

            // write header
            FileTools.WriteToBuffer(ref fileBytes, ref offset, FileMagicWord);
            FileTools.WriteToBuffer(ref fileBytes, ref offset, RequiredVersion);

            // first block
            if (_mliStruct.UnknownStruct1Array != null)
            {
                FileTools.WriteToBuffer(ref fileBytes, ref offset, 1); // flag
                FileTools.WriteToBuffer(ref fileBytes, ref offset, _mliStruct.UnknownStruct1Array.Length);
                FileTools.WriteToBuffer(ref fileBytes, ref offset, _mliStruct.UnknownStruct1Array);

                // shorts block
                if (_mliStruct.UnknownShortArray != null && _mliStruct.UnknownShortArray.Length > 0)
                {
                    FileTools.WriteToBuffer(ref fileBytes, ref offset, _mliStruct.UnknownShortArray.Length);
                    FileTools.WriteToBuffer(ref fileBytes, ref offset, _mliStruct.UnknownShortArray.ToByteArray());
                    _mliStruct.UnknownStruct1Array[0].UnknownInt321 = 0x0A;
                }

                // floats block
                if (_mliStruct.UnknownStruct2Array != null && _mliStruct.UnknownStruct2Array.Length > 0)
                {
                    FileTools.WriteToBuffer(ref fileBytes, ref offset, _mliStruct.UnknownStruct2Array.Length);
                    FileTools.WriteToBuffer(ref fileBytes, ref offset, _mliStruct.UnknownStruct2Array);
                    _mliStruct.UnknownStruct1Array[0].UnknownInt321 = 0x0A;
                }
            }

            // last two unknown blocks
            Int32 count1 = 0;
            Int32 count2 = 0;
            if (_mliStruct.UnknownStruct3Array1 != null && _mliStruct.UnknownStruct3Array1.Length > 0)
            {
                FileTools.WriteToBuffer(ref fileBytes, ref offset, _mliStruct.UnknownStruct3Array1);
                count1 = _mliStruct.UnknownStruct3Array1.Length;
            }
            if (_mliStruct.UnknownStruct3Array2 != null && _mliStruct.UnknownStruct3Array2.Length > 0)
            {
                FileTools.WriteToBuffer(ref fileBytes, ref offset, _mliStruct.UnknownStruct3Array2);
                count2 = _mliStruct.UnknownStruct3Array2.Length;
            }
            FileTools.WriteToBuffer(ref fileBytes, ref offset, count1);
            FileTools.WriteToBuffer(ref fileBytes, ref offset, count2);

            // and we're done
            Array.Resize(ref fileBytes, offset);
            return fileBytes;
        }

        public override byte[] ExportAsDocument()
        {
            if (_mliStruct == null) throw new Exceptions.NotInitializedException();

            MemoryStream memoryStream = new MemoryStream();
            XmlSerializer xmlSerializer = new XmlSerializer(_mliStruct.GetType());
            xmlSerializer.Serialize(memoryStream, _mliStruct);

            return memoryStream.ToArray();
        }
    }
}
