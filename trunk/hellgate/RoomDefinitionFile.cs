using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using Revival.Common;

namespace Hellgate
{
    public class RoomDefinitionFile
    {
        public const String FileExtension = ".rom";
        public const String FileExtensionXml = ".rom.xml";
        private const UInt32 FileMagicWord = 0xEA7A7ABE; // '¾zzê'
        private const UInt32 RequiredVersion = 0x49; // 73

        // total size = 728 bytes (0x2D8)
        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class RoomDefinitionHeader
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            private String InternalRoomDefinitionName;              // 0x00     0       // is internally set - is null in-file              LoadRoomDefinition+262  898 call    strncpy_s_0 (0x100; 256 bytes)
            private Int32 InternalInt321;                           // 0x100    256     // is internally set to -1                          LoadRoomDefinition+24F  898 or      dword ptr [rbx+100h], 0FFFFFFFFh
            public Int32 Unknown1;                                  // 0x104    260     // not seen used
            private Int32 InternalInt322;                           // 0x108    264     // is internally set to ? shortly after Excel_GetRoomIndex (was 22 (or 0x22? - wasn't paying attention) for character creation) LoadRoomDefinition+28D  898 mov     [rbx+108h], eax
            public Int32 UnknownCount1;                             // 0x10C    268     // probably a count - not seen used
            public Int64 Offset110;                                 // 0x110    272
            public Int32 Count118;                                  // 0x118    280
            public Int32 Unknown2;                                  // 0x11C    284     // not seen used
            public Int64 Offset120;                                 // 0x120    288
            public float UnknownFloat1;                             // 0x128    296     // not seen used
            public Int32 UnknownCount2;                             // 0x12C    300     // probably a count - not seen used
            public Int64 Offset130;                                 // 0x130    304     // is offset, but not seen used

            // not seen used/guessing
            public Int32 Unknown3;                                  // 0x138    312     // unknown int32?
            public Int32 Unknown4;                                  // 0x13C    316     // unknown int32?
            public Int32 Unknown5;                                  // 0x140    320     // unknown int32?
            public Int32 Unknown6;                                  // 0x144    324     // unknown int32?
            public float UnknownFloat2;                             // 0x148    328     // is float
            public float UnknownFloat3;                             // 0x14C    332     // is float
            public float UnknownFloat4;                             // 0x150    336     // is float
            public float UnknownFloat5;                             // 0x154    340     // is float
            public float UnknownFloat6;                             // 0x158    344     // is float
            public float UnknownFloat7;                             // 0x15C    348     // is float
            public Int32 Unknown7;                                  // 0x160    352     // is int32
            public Int32 Unknown8;                                  // 0x164    356     // unknown int32?

            public Int64 Offset167;                                 // 0x168    360     // is offset, but not seen used
            public Int32 UnknownValue1;                             // 0x170    368     // is read in before next big function call         LoadRoomDefinition+4CA  898 mov     r9d, [rbx+170h]
            public Int32 Count174;                                  // 0x174    372     // i think this is the count...
            public Int64 Offset178;                                 // 0x178    376
            public Int32 UnknownValue3;                             // 0x180    384     // is read in before next big function call         LoadRoomDefinition+4E0  898 mov     eax, [rbx+180h]
            public Int32 Count184;                                  // 0x184    388     // i think this is the count...
            public Int64 Offset188;                                 // 0x188    392     // is offset to (int32?) value near end of file
            public Int64 Unknown9;                                  // 0x190    400     // is read in before next big function call         LoadRoomDefinition+4E6  898 lea     rcx, [rbx+190h]
            public Int64 Unknown10;                                 // 0x198    408     // unknown
            public Int64 Unknown11;                                 // 0x1A0    416     // not seen used, but is 0x01 in file
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 47)]
            public Int32[] Unknowns;                                // 0x198    406     // not seen used - all zeros in file that was tested

            public Int32 RoomVersion;                               // 0x264    612     // room version from excel table ROOM_INDEX - file must equal excel value
            public Int64 Offset268;                                 // 0x268    616
            public Int32 Count270;                                  // 0x270    624
            public Int32 Unknown13;                                 // 0x274    628     // unknown int32?
            public Int64 Offset278;                                 // 0x278    632
            public Int32 Count280;                                  // 0x280    640
            public Int32 Unknown15;                                 // 0x284    644     // unknown int32?
            public Int64 Offset288;                                 // 0x288    648
            public float UnknownFloat8;                             // 0x28C    656     // is float
            public float UnknownFloat9;                             // 0x290    660     // is float
            public Int32 Count298;                                  // 0x298    664
            public Int32 Count29C;                                  // 0x29C    668
            public Int64 Offset2A0;                                 // 0x2A0    672
            public Int32 Count2A8;                                  // 0x2A8    680
            public Int32 Unknown17;                                 // 0x2AC    684     // is int32?
            public Int32 Unknown18;                                 // 0x2B0    688     // is int32?
            public Int32 Unknown19;                                 // 0x2B4    692     // is int32?
            public Int32 Unknown20;                                 // 0x2B8    696     // is int32?
            public Int32 Unknown21;                                 // 0x2BC    700     // is int32?
            private Int32 InternalInt323;                           // 0x2C0    704     // internally set to -1                             LoadRoomDefinition+286  898 or      dword ptr [rbx+2C0h], 0FFFFFFFFh
            public Int32 Unknown22;                                 // 0x2C4    708     // is int32?
            private Int64 InternalInt641;                           // 0x2C8    712     // internally set to ptr to fileBytes               LoadRoomDefinition+274  898 mov     [rbx+2C8h], r11
            private Int32 InternalInt324;                           // 0x2D0    720     // internally set to fileSize                       LoadRoomDefinition+201  898 mov     [rbx+2D0h], edx
            public Int32 Unknown23;                                 // 0x2D4    724     // is int32?
            // end of struct                                        // 0x2D8    728
        }

        // total size = 40 bytes (0x28)
        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class UnknownStruct1
        {
            public Int64 Offset1;               // 0x00     0       // note: these offset values are file bytes offsets to Int32 arrays in _unknownStruct1Int32Array
            public Int64 Offset2;               // 0x08     8
            public Int64 Offset3;               // 0x10     16
            public Int32 Unknown1;              // 0x18     24      // is int32?
            public Int32 CountOfOffset2Int32s;  // 0x1C     28      // is int32
            public Int32 Unknown3;              // 0x20     32      // is int32?
            public Int32 Unknown4;              // 0x24     36      // is int32?
            // end of struct                    // 0x28     40
        }

        // total size = 64 bytes (0x40)
        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class UnknownStruct2
        {
            public Int64 Offset1;               // 0x00     0       // note: these offset values are file byte offsets to UnknownStruct3 in _unknownStruct3Array
            public Int64 Offset2;               // 0x08     8       // todo: check if original "offset" (before transform) is the index of the _unknownStruct3Array
            public Int64 Offset3;               // 0x10     16
            public float Float1;                // 0x18     24
            public float Float2;                // 0x1C     28
            public float Float3;                // 0x20     32
            public float Float4;                // 0x24     36
            public float Float5;                // 0x28     40
            public float Float6;                // 0x2C     44
            public float Float7;                // 0x30     48
            public float Float8;                // 0x34     52
            public float Float9;                // 0x38     56
            public Int32 Unknown;               // 0x3C     60      // is int32? or float?
            // end of struct                    // 0x40     64
        }

        // total size = 16 bytes (0x10)
        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class UnknownStruct3
        {
            public float Float1;                // 0x00     0
            public float Float2;                // 0x04     4
            public float Float3;                // 0x08     8
            public Int32 Unknown;               // 0x0C     12
            // end of struct                    // 0x10     16
        }

        // total size = 12 bytes (0x0C)
        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class UnknownStruct4
        {
            public float Float1;                // 0x00     0
            public float Float2;                // 0x04     4
            public float Float3;                // 0x08     8
            // end of struct                    // 0x0C     12
        }

        // total size = 12 bytes (0x0C)
        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class UnknownStruct5
        {
            public Int32 Int321;                // 0x00     0
            public Int32 Int322;                // 0x04     4
            public Int32 Int323;                // 0x08     8
            // end of struct                    // 0x0C     12
        }

        // total size = 136 bytes (0x88)
        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class UnknownStruct6
        {
            // most of these are guesses - I think some of the Int32s are float, and possible some of the floats are doubles
            // todo: keep an eye on outputs and check for huge weird numbers
            public Int32 Int321;                // 0x00     0
            public float Float1;                // 0x04     4
            public float Float2;                // 0x08     8
            public float Float3;                // 0x0C     12
            public float Float4;                // 0x10     16
            public float Float5;                // 0x14     20
            public float Float6;                // 0x18     24
            public float Float7;                // 0x1C     28
            public float Float8;                // 0x20     32
            public float Float9;                // 0x24     36
            public float Float10;               // 0x28     40
            public float Float11;               // 0x2C     44
            public float Float12;               // 0x30     48
            public Int32 Int322;                // 0x34     52
            public Int32 Int323;                // 0x38     56
            public Int32 Int324;                // 0x3C     60
            public Int32 Int325;                // 0x40     64
            public Int32 Int326;                // 0x44     68
            public Int32 Int327;                // 0x48     72
            public float Float13;               // 0x4C     76
            public float Float14;               // 0x50     80
            public float Float15;               // 0x54     84
            public float Float16;               // 0x58     88
            public float Float17;               // 0x5C     92
            public float Float18;               // 0x60     96
            public float Float19;               // 0x64     100
            public float Float20;               // 0x68     104
            public float Float21;               // 0x6C     108
            public float Float22;               // 0x70     112
            public Int32 Int328;                // 0x74     116
            public Int32 Int329;                // 0x78     120
            public Int32 Int3210;               // 0x7C     124
            public Int32 Int3211;               // 0x80     128
            public Int32 Int3212;               // 0x84     132
            // end of struct                    // 0x88     136
        }



        private byte[] _fileBytes;

        private XmlDocument _xmlDocument;
        private XmlWriter _xmlWriter;

        private RoomDefinitionHeader _fileHeader;
        private UnknownStruct1[][] _unknownStruct1Arrays;
        private Int32[] _unknownStruct1Int32Array;
        private UnknownStruct2[] _unknownStruct2Array;
        private UnknownStruct3[] _unknownStruct3Array;
        private UnknownStruct4[] _unknownStruct4Array;
        private UnknownStruct5[] _unknownStruct5Array;
        private UnknownStruct6[] _unknownStruct6Array;
        private UnknownStruct4[] _unknownStruct7Array;
        private UnknownStruct5[] _unknownStruct8Array;
        

        public RoomDefinitionFile()
        {

        }

        /// <summary>
        /// Parses a level rules file bytes.
        /// </summary>
        /// <param name="fileBytes">The bytes of the level rules to parse.</param>
        public void ParseFileBytes(byte[] fileBytes)
        {
            Debug.Assert(fileBytes != null);
            _fileBytes = fileBytes;


            // our XML document stuffs
            _xmlDocument = new XmlDocument();
            XPathNavigator xPathNavigator = _xmlDocument.CreateNavigator();
            Debug.Assert(xPathNavigator != null);
            _xmlWriter = xPathNavigator.AppendChild();
            Debug.Assert(_xmlWriter != null);


            // file header checks
            int offset = 0;
            UInt32 fileMagicWord = FileTools.ByteArrayToUInt32(fileBytes, ref offset);
            if (fileMagicWord != FileMagicWord) throw new Exceptions.UnexpectedMagicWordException();

            UInt32 fileVersion = FileTools.ByteArrayToUInt32(fileBytes, ref offset);
            if (fileVersion != RequiredVersion) throw new Exceptions.NotSupportedFileVersionException();


            // main file header
            _fileHeader = FileTools.ByteArrayToStructure<RoomDefinitionHeader>(fileBytes, ref offset);
            Console.WriteLine(String.Format("Bytes used: 0 to {0}", offset));


            // read UnknownStruct1 arrays (not seen read like this - but it works)
            offset = (int)_fileHeader.Offset288;
            int count29C = _fileHeader.Count29C;
            int count298 = _fileHeader.Count298;
            if (offset > 0 && count29C > 0 && count298 > 0)
            {
                _unknownStruct1Arrays = new UnknownStruct1[count29C][];
                for (int i = 0; i < count29C; i++)
                {
                    _unknownStruct1Arrays[i] = FileTools.ByteArrayToArray<UnknownStruct1>(fileBytes, ref offset, count298);
                }
            }
            Console.WriteLine(String.Format("Bytes used: {0} to {1}", _fileHeader.Offset288, offset));


            // read UnknownStruct1 Int32 array
            offset = (int) _fileHeader.Offset2A0;
            int count2A8 = _fileHeader.Count2A8;
            if (offset > 0 && count2A8 > 0)
            {
                _unknownStruct1Int32Array = FileTools.ByteArrayToInt32Array(fileBytes, ref offset, count2A8);
            }
            Console.WriteLine(String.Format("Bytes used: {0} to {1}", _fileHeader.Offset2A0, offset));


            // read UnknownStruct2 array
            offset = (int)_fileHeader.Offset120;
            int count118 = _fileHeader.Count118;
            if (offset > 0 && count118 > 0)
            {
                _unknownStruct2Array = FileTools.ByteArrayToArray<UnknownStruct2>(fileBytes, ref offset, count118);

                // generate offsets
                for (int i = 0; i < count118; i++)
                {
                    _unknownStruct2Array[i].Offset1 = (_unknownStruct2Array[i].Offset1 << 4) + _fileHeader.Offset110;
                    _unknownStruct2Array[i].Offset2 = (_unknownStruct2Array[i].Offset2 << 4) + _fileHeader.Offset110;
                    _unknownStruct2Array[i].Offset3 = (_unknownStruct2Array[i].Offset3 << 4) + _fileHeader.Offset110;
                }
            }
            Console.WriteLine(String.Format("Bytes used: {0} to {1}", _fileHeader.Offset120, offset));


            // read UnknownStruct3 array (not seen read like this - but it works)
            offset = (int)_fileHeader.Offset110;
            int unknownCount1 = _fileHeader.UnknownCount1; // pretty sure UnknownCount1 is for UnknownStruct3, but let's make sure
            int unknownStruct3Bytes = (int)_fileHeader.Offset120 - (int)_fileHeader.Offset110;
            int actualCount = unknownStruct3Bytes / Marshal.SizeOf(typeof(UnknownStruct3));
            Debug.Assert(actualCount == unknownCount1);
            if (offset > 0 && unknownCount1 > 0)
            {
                _unknownStruct3Array = FileTools.ByteArrayToArray<UnknownStruct3>(fileBytes, ref offset, unknownCount1);
            }
            Console.WriteLine(String.Format("Bytes used: {0} to {1}", _fileHeader.Offset110, offset));


            // read UnknownStruct4 array (not seen read like this - but it works)
            offset = (int) _fileHeader.Offset268;
            int count270 = _fileHeader.Count270;
            if (offset > 0 && count270 > 0)
            {
                _unknownStruct4Array = FileTools.ByteArrayToArray<UnknownStruct4>(fileBytes, ref offset, count270);
            }
            Console.WriteLine(String.Format("Bytes used: {0} to {1}", _fileHeader.Offset268, offset));


            // read UnknownStruct5 array (not seen read like this - but it works)
            offset = (int)_fileHeader.Offset278;
            int count280 = _fileHeader.Count280;
            if (offset > 0 && count280 > 0)
            {
                _unknownStruct5Array = FileTools.ByteArrayToArray<UnknownStruct5>(fileBytes, ref offset, count280);
            }
            Console.WriteLine(String.Format("Bytes used: {0} to {1}", _fileHeader.Offset278, offset));


            // read UnknownStruct6 array (not seen read like this - but it works)
            offset = (int)_fileHeader.Offset130;
            int unknownCount2 = _fileHeader.UnknownCount2;
            if (offset > 0 && unknownCount2 > 0)
            {
                _unknownStruct6Array = FileTools.ByteArrayToArray<UnknownStruct6>(fileBytes, ref offset, unknownCount2);
            }
            Console.WriteLine(String.Format("Bytes used: {0} to {1}", _fileHeader.Offset130, offset));


            // read UnknownStruct7 array (not seen read like this - but it works) - has same structure (3xfloat) as UnknownStruct4
            offset = (int)_fileHeader.Offset178;
            int count174 = _fileHeader.Count174;
            if (offset > 0 && count174 > 0)
            {
                _unknownStruct7Array = FileTools.ByteArrayToArray<UnknownStruct4>(fileBytes, ref offset, count174);
            }
            Console.WriteLine(String.Format("Bytes used: {0} to {1}", _fileHeader.Offset178, offset));


            // read UnknownStruct8 array (not seen read like this - but it works) - has same structure (3xint32) as UnknownStruct5 - I think
            // however we also have an offset (Offset170) that points to a value within it... e.o.f. struct needs checking
            offset = (int)_fileHeader.Offset188;
            int count184 = _fileHeader.Count184;
            if (offset > 0 && count184 > 0)
            {
                _unknownStruct8Array = FileTools.ByteArrayToArray<UnknownStruct5>(fileBytes, ref offset, count184);
            }
            Console.WriteLine(String.Format("Bytes used: {0} to {1}", _fileHeader.Offset188, offset));


            // create XmlDocument
            _xmlWriter.WriteStartElement("RoomDefinition");

            XmlSerializer xmlSerializerHeader = new XmlSerializer(_fileHeader.GetType());
            xmlSerializerHeader.Serialize(_xmlWriter, _fileHeader);

            XmlSerializer xmlSerializerStruct1Arrays = new XmlSerializer(_unknownStruct1Arrays.GetType());
            xmlSerializerStruct1Arrays.Serialize(_xmlWriter, _unknownStruct1Arrays);

            XmlSerializer xmlSerializerStruct1Int32Array = new XmlSerializer(_unknownStruct1Int32Array.GetType());
            xmlSerializerStruct1Int32Array.Serialize(_xmlWriter, _unknownStruct1Int32Array);

            XmlSerializer xmlSerializerStruct2Array = new XmlSerializer(_unknownStruct2Array.GetType());
            xmlSerializerStruct2Array.Serialize(_xmlWriter, _unknownStruct2Array);

            XmlSerializer xmlSerializerStruct3Array = new XmlSerializer(_unknownStruct3Array.GetType());
            xmlSerializerStruct3Array.Serialize(_xmlWriter, _unknownStruct3Array);

            XmlSerializer xmlSerializerStruct4Array = new XmlSerializer(_unknownStruct4Array.GetType());
            xmlSerializerStruct4Array.Serialize(_xmlWriter, _unknownStruct4Array);

            XmlSerializer xmlSerializerStruct5Array = new XmlSerializer(_unknownStruct5Array.GetType());
            xmlSerializerStruct5Array.Serialize(_xmlWriter, _unknownStruct5Array);

            XmlSerializer xmlSerializerStruct6Array = new XmlSerializer(_unknownStruct6Array.GetType());
            xmlSerializerStruct6Array.Serialize(_xmlWriter, _unknownStruct6Array);

            xmlSerializerStruct4Array.Serialize(_xmlWriter, _unknownStruct7Array);
            xmlSerializerStruct5Array.Serialize(_xmlWriter, _unknownStruct8Array);

            _xmlWriter.WriteEndElement();
            _xmlWriter.Close();
        }

        public void SaveXmlDocument(String filePath)
        {
            if (_xmlDocument == null) return;

            _xmlDocument.Save(filePath);
        }

    }
}
