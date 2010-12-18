using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
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

            public Int64 Offset168;                                 // 0x168    360     // is offset, but not seen used
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

        // total size = 6 bytes (0x06)
        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class UnknownStruct7
        {
            public short Short1;                // 0x00     0
            public short Short2;                // 0x02     2
            public short Short3;                // 0x04     4
            // end of struct                    // 0x06     6
        }

        public class RoomDefinition
        {
            public RoomDefinitionHeader FileHeader;
            public UnknownStruct1[][] UnknownStruct1Arrays;
            public Int32[] UnknownStruct1Int32Array;
            public UnknownStruct2[] UnknownStruct2Array;
            public UnknownStruct3[] UnknownStruct3Array;
            public UnknownStruct4[] UnknownStruct4Array;
            public UnknownStruct5[] UnknownStruct5Array;
            public UnknownStruct6[] UnknownStruct6Array;
            public UnknownStruct4[] UnknownStruct7Array;
            public UnknownStruct7[] UnknownStruct8Array;
            public UnknownStruct5[] UnknownStructFooter;
        }

        private byte[] _fileBytes;

        private XmlDocument _xmlDocument;
        private XmlWriter _xmlWriter;
        private RoomDefinition _roomDefinition;

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
            _roomDefinition = new RoomDefinition();


            // file header checks
            int offset = 0;
            UInt32 fileMagicWord = FileTools.ByteArrayToUInt32(fileBytes, ref offset);
            if (fileMagicWord != FileMagicWord) throw new Exceptions.UnexpectedMagicWordException();

            UInt32 fileVersion = FileTools.ByteArrayToUInt32(fileBytes, ref offset);
            if (fileVersion != RequiredVersion) throw new Exceptions.NotSupportedFileVersionException();


            // main file header
            _roomDefinition.FileHeader = FileTools.ByteArrayToStructure<RoomDefinitionHeader>(fileBytes, ref offset);
            RoomDefinitionHeader header = _roomDefinition.FileHeader;
            //Console.WriteLine(String.Format("Bytes used: 0 to {0}", offset));


            // read UnknownStruct1 arrays
            offset = (int)header.Offset288;
            int count29C = header.Count29C;
            int count298 = header.Count298;
            if (offset > 0 && count29C > 0 && count298 > 0)
            {
                _roomDefinition.UnknownStruct1Arrays = new UnknownStruct1[count29C][];
                for (int i = 0; i < count29C; i++)
                {
                    _roomDefinition.UnknownStruct1Arrays[i] = FileTools.ByteArrayToArray<UnknownStruct1>(fileBytes, ref offset, count298);
                }
            }
            //Console.WriteLine(String.Format("Bytes used: {0} to {1}", header.Offset288, offset));


            // read UnknownStruct1 Int32 array
            offset = (int) header.Offset2A0;
            int count2A8 = header.Count2A8;
            if (offset > 0 && count2A8 > 0)
            {
                _roomDefinition.UnknownStruct1Int32Array = FileTools.ByteArrayToInt32Array(fileBytes, ref offset, count2A8);
            }
            //Console.WriteLine(String.Format("Bytes used: {0} to {1}", header.Offset2A0, offset));


            // read UnknownStruct2 array
            offset = (int)header.Offset120;
            int count118 = header.Count118;
            if (offset > 0 && count118 > 0)
            {
                _roomDefinition.UnknownStruct2Array = FileTools.ByteArrayToArray<UnknownStruct2>(fileBytes, ref offset, count118);

                // generate offsets
                //for (int i = 0; i < count118; i++)
                //{
                //    _roomDefinition._unknownStruct2Array[i].Offset1 = (_roomDefinition._unknownStruct2Array[i].Offset1 << 4) + header.Offset110;
                //    _roomDefinition._unknownStruct2Array[i].Offset2 = (_roomDefinition._unknownStruct2Array[i].Offset2 << 4) + header.Offset110;
                //    _roomDefinition._unknownStruct2Array[i].Offset3 = (_roomDefinition._unknownStruct2Array[i].Offset3 << 4) + header.Offset110;
                //}
            }
            //Console.WriteLine(String.Format("Bytes used: {0} to {1}", header.Offset120, offset));


            // read UnknownStruct3 array (not seen read like this - but it works)
            offset = (int)header.Offset110;
            int unknownCount1 = header.UnknownCount1;
            if (offset > 0 && unknownCount1 > 0)
            {
                _roomDefinition.UnknownStruct3Array = FileTools.ByteArrayToArray<UnknownStruct3>(fileBytes, ref offset, unknownCount1);
            }
            //Console.WriteLine(String.Format("Bytes used: {0} to {1}", header.Offset110, offset));


            // read UnknownStruct4 array (not seen read like this - but it works)
            offset = (int) header.Offset268;
            int count270 = header.Count270;
            if (offset > 0 && count270 > 0)
            {
                _roomDefinition.UnknownStruct4Array = FileTools.ByteArrayToArray<UnknownStruct4>(fileBytes, ref offset, count270);
            }
            //Console.WriteLine(String.Format("Bytes used: {0} to {1}", header.Offset268, offset));


            // read UnknownStruct5 array (not seen read like this - but it works)
            offset = (int)header.Offset278;
            int count280 = header.Count280;
            if (offset > 0 && count280 > 0)
            {
                _roomDefinition.UnknownStruct5Array = FileTools.ByteArrayToArray<UnknownStruct5>(fileBytes, ref offset, count280);
            }
            //Console.WriteLine(String.Format("Bytes used: {0} to {1}", header.Offset278, offset));


            // read UnknownStruct6 array (not seen read like this - but it works)
            offset = (int)header.Offset130;
            int unknownCount2 = header.UnknownCount2;
            if (offset > 0 && unknownCount2 > 0)
            {
                _roomDefinition.UnknownStruct6Array = FileTools.ByteArrayToArray<UnknownStruct6>(fileBytes, ref offset, unknownCount2);
            }
            //Console.WriteLine(String.Format("Bytes used: {0} to {1}", header.Offset130, offset));


            // read UnknownStruct7 array (not seen read like this - but it works) - has same structure (3xfloat) as UnknownStruct4
            offset = (int)header.Offset178;
            int count174 = header.Count174;
            if (offset > 0 && count174 > 0)
            {
                _roomDefinition.UnknownStruct7Array = FileTools.ByteArrayToArray<UnknownStruct4>(fileBytes, ref offset, count174);
            }
            //Console.WriteLine(String.Format("Bytes used: {0} to {1}", header.Offset178, offset));


            // read UnknownStruct8 array (not seen read like this - but it works)
            offset = (int)header.Offset188;
            int count184 = header.Count184;
            if (offset > 0 && count184 > 0)
            {
                _roomDefinition.UnknownStruct8Array = FileTools.ByteArrayToArray<UnknownStruct7>(fileBytes, ref offset, count184);
            }
            //Console.WriteLine(String.Format("Bytes used: {0} to {1}", header.Offset188, offset));


            // read UnknownStruct9 array (not seen read like this - but it works)
            offset = (int)header.Offset168;
            int countUnknown7 = header.Unknown7;
            if (offset > 0 && countUnknown7 > 0)
            {
                _roomDefinition.UnknownStructFooter = FileTools.ByteArrayToArray<UnknownStruct5>(fileBytes, ref offset, countUnknown7);
            }
            //Console.WriteLine(String.Format("Bytes used: {0} to {1}", header.Offset168, offset));


            // create XmlDocument
            XmlSerializer xmlSerializerHeader = new XmlSerializer(_roomDefinition.GetType());
            xmlSerializerHeader.Serialize(_xmlWriter, _roomDefinition);
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
            XmlSerializer xmlSerializer = new XmlSerializer(typeof (RoomDefinition));
            RoomDefinition roomDefinition = (RoomDefinition)xmlSerializer.Deserialize(xmlNodeReader);

            int offset = 0;
            _fileBytes = new byte[1024];

            // write header
            FileTools.WriteToBuffer(ref _fileBytes, ref offset, FileMagicWord);
            FileTools.WriteToBuffer(ref _fileBytes, ref offset, RequiredVersion);
            offset += Marshal.SizeOf(roomDefinition.FileHeader); // want to update offsets and counts first

            // write unknown struct 3
            roomDefinition.FileHeader.Offset110 = offset;
            roomDefinition.FileHeader.UnknownCount1 = roomDefinition.UnknownStruct3Array.Length;
            FileTools.WriteToBuffer(ref _fileBytes, ref offset, roomDefinition.UnknownStruct3Array);

            // write unknown struct 2
            roomDefinition.FileHeader.Offset120 = offset;
            roomDefinition.FileHeader.Count118 = roomDefinition.UnknownStruct2Array.Length;
            FileTools.WriteToBuffer(ref _fileBytes, ref offset, roomDefinition.UnknownStruct2Array);

            // write int arrays
            roomDefinition.FileHeader.Offset2A0 = offset;
            roomDefinition.FileHeader.Count2A8 = roomDefinition.UnknownStruct1Int32Array.Length;
            FileTools.WriteToBuffer(ref _fileBytes, ref offset, roomDefinition.UnknownStruct1Int32Array.ToByteArray());

            // write unknown struct 1
            roomDefinition.FileHeader.Offset288 = offset;
            roomDefinition.FileHeader.Count29C = roomDefinition.UnknownStruct1Arrays.Length;
            roomDefinition.FileHeader.Count298 = roomDefinition.UnknownStruct1Arrays[0].Length;
            FileTools.WriteToBuffer(ref _fileBytes, ref offset, roomDefinition.UnknownStruct1Arrays);

            // write unknown struct 4
            roomDefinition.FileHeader.Offset268 = 0;
            roomDefinition.FileHeader.Count270 = 0;
            if (roomDefinition.UnknownStruct4Array != null)
            {
                roomDefinition.FileHeader.Offset268 = offset;
                roomDefinition.FileHeader.Count270 = roomDefinition.UnknownStruct4Array.Length;
                FileTools.WriteToBuffer(ref _fileBytes, ref offset, roomDefinition.UnknownStruct4Array);
            }

            // write unknown struct 5
            roomDefinition.FileHeader.Offset278 = 0;
            roomDefinition.FileHeader.Count280 = 0;
            if (roomDefinition.UnknownStruct5Array != null)
            {
                roomDefinition.FileHeader.Offset278 = offset;
                roomDefinition.FileHeader.Count280 = roomDefinition.UnknownStruct5Array.Length;
                FileTools.WriteToBuffer(ref _fileBytes, ref offset, roomDefinition.UnknownStruct5Array);
            }

            // write unknown struct 6
            roomDefinition.FileHeader.Offset130 = 0;
            roomDefinition.FileHeader.UnknownCount2 = 0;
            if (roomDefinition.UnknownStruct6Array != null)
            {
                roomDefinition.FileHeader.Offset130 = offset;
                roomDefinition.FileHeader.UnknownCount2 = roomDefinition.UnknownStruct6Array.Length;
                FileTools.WriteToBuffer(ref _fileBytes, ref offset, roomDefinition.UnknownStruct6Array);
            }

            // write unknown struct 7
            roomDefinition.FileHeader.Offset178 = offset;
            roomDefinition.FileHeader.Count174 = roomDefinition.UnknownStruct7Array.Length;
            FileTools.WriteToBuffer(ref _fileBytes, ref offset, roomDefinition.UnknownStruct7Array);

            // write unknown struct 8
            roomDefinition.FileHeader.Offset188 = offset;
            roomDefinition.FileHeader.Count184 = roomDefinition.UnknownStruct8Array.Length;
            FileTools.WriteToBuffer(ref _fileBytes, ref offset, roomDefinition.UnknownStruct8Array);

            // write unknown struct footer
            roomDefinition.FileHeader.Offset168 = offset;
            roomDefinition.FileHeader.Unknown7 = roomDefinition.UnknownStructFooter.Length;
            FileTools.WriteToBuffer(ref _fileBytes, ref offset, roomDefinition.UnknownStructFooter);

            // write our updated header
            FileTools.WriteToBuffer(ref _fileBytes, 8, roomDefinition.FileHeader);

            // and we're done
            Array.Resize(ref _fileBytes, offset);
            return _fileBytes;
        }

        public void SaveXmlDocument(String filePath)
        {
            if (_xmlDocument == null) return;

            _xmlDocument.Save(filePath);
        }

    }
}
