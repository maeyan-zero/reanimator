using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using Revival.Common;

namespace Hellgate
{
    public class MLIFile
    {
        public const String FileExtension = ".mli";
        public const String FileExtensionXml = ".mli.xml";
        private const UInt32 FileMagicWord = 0x1515CAFE; // 'þÊ..'
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
        private XmlDocument _xmlDocument;
        private XmlWriter _xmlWriter;

        /// <summary>
        /// Parses a level rules file bytes.
        /// </summary>
        /// <param name="fileBytes">The bytes of the level rules to parse.</param>
        public void ParseFileBytes(byte[] fileBytes)
        {
            Debug.Assert(fileBytes != null);

            // our XML document stuffs
            _xmlDocument = new XmlDocument();
            XPathNavigator xPathNavigator = _xmlDocument.CreateNavigator();
            Debug.Assert(xPathNavigator != null);
            _xmlWriter = xPathNavigator.AppendChild();
            Debug.Assert(_xmlWriter != null);
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

            int count1 = FileTools.ByteArrayToInt32(fileBytes, fileBytes.Length - 8);
            if (count1 > 0)
            {
                _mliStruct.UnknownStruct3Array1 = FileTools.ByteArrayToArray<UnknownStruct3>(fileBytes, ref offset, count1);
            }

            int count2 = FileTools.ByteArrayToInt32(fileBytes, fileBytes.Length - 4);
            if (count2 > 0)
            {
                _mliStruct.UnknownStruct3Array2 = FileTools.ByteArrayToArray<UnknownStruct3>(fileBytes, ref offset, count2);
            }

            offset += 8; // for 2x count fields

            Debug.Assert(offset == fileBytes.Length);


            // create XmlDocument
            XmlSerializer xmlSerializerHeader = new XmlSerializer(_mliStruct.GetType());
            xmlSerializerHeader.Serialize(_xmlWriter, _mliStruct);
            _xmlWriter.Close();
        }

        /// <summary>
        /// Parses and XML document and returns the serialized byte array.
        /// </summary>
        /// <param name="xmlDocument">The XML Document to parse.</param>
        /// <returns>The serialized byte array.</returns>
        public byte[] ParseXmlDocument(XmlDocument xmlDocument)
        {
            XmlNodeReader xmlNodeReader = new XmlNodeReader(xmlDocument);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(MLIStruct));
            MLIStruct mliStruct = (MLIStruct)xmlSerializer.Deserialize(xmlNodeReader);

            int offset = 0;
            byte[] fileBytes = new byte[1024];

            // write header
            FileTools.WriteToBuffer(ref fileBytes, ref offset, FileMagicWord);
            FileTools.WriteToBuffer(ref fileBytes, ref offset, RequiredVersion);


            // and we're done
            Array.Resize(ref fileBytes, offset);
            return fileBytes;
        }

        public void SaveXmlDocument(String filePath)
        {
            if (_xmlDocument == null) return;

            _xmlDocument.Save(filePath);
        }
    }
}
