using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using Revival.Common;

namespace Hellgate
{
    public class RoomDefinitionFile
    {
        public const String FileExtension = ".rom";
        public const String FileExtensionXml = ".rom.xml";
        private const UInt32 FileMagicWord = 0xEA7A7ABE; // '%.ÒÒ'
        private const UInt32 RequiredVersion = 0x49; // 73


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class RoomDefinitionHeader
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public String InternalRoomDefinitionName;               // 0x00     0       // is internally set - is null in-file              LoadRoomDefinition+262  898 call    strncpy_s_0 (0x100; 256 bytes)
            public Int32 InternalInt321;                            // 0x100    256     // is internally set to -1                          LoadRoomDefinition+24F  898 or      dword ptr [rbx+100h], 0FFFFFFFFh
            public Int32 Unknown1;                                  // 0x104    260     // not seen used
            public Int32 InternalInt322;                            // 0x108    264     // is internally set to ? shortly after Excel_GetRoomIndex (was 22 (or 0x22? - wasn't paying attention) for character creation) LoadRoomDefinition+28D  898 mov     [rbx+108h], eax
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
            public Int32 UnknownValue2;                             // 0x174    372     // is read in before next big function call         LoadRoomDefinition+4F1  898 mov     eax, [rbx+174h]
            public Int64 Offset178;                                 // 0x178    376
            public Int32 UnknownValue3;                             // 0x180    384     // is read in before next big function call         LoadRoomDefinition+4E0  898 mov     eax, [rbx+180h]
            public Int32 UnknownValue4;                             // 0x184    388     // is read in before next big function call         LoadRoomDefinition+4D6  898 mov     eax, [rbx+184h]
            public Int64 Offset188;                                 // 0x188    392     // is offset to (int32?) value near end of file
            public Int64 Unknown9;                                  // 0x190    400     // is read in before next big function call         LoadRoomDefinition+4E6  898 lea     rcx, [rbx+190h]
            public Int64 Unknown10;                                 // 0x198    408     // unknown
            public Int64 Unknown11;                                 // 0x1A0    416     // not seen used, but is 0x01 in file
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 47)]
            public Int32[] Unknowns;                                // 0x198    406     // not seen used - all zeros in file that was tested

            public Int32 RoomVersion;                               // 0x264    612     // room version from excel table ROOM_INDEX - file must equal excel value
            public Int64 Offset268;                                 // 0x268    616
            public Int32 Unknown12;                                 // 0x270    624     // is int32
            public Int32 Unknown13;                                 // 0x274    628     // unknown int32?
            public Int64 Offset278;                                 // 0x278    632
            public Int32 Unknown14;                                 // 0x280    640     // is int32
            public Int32 Unknown15;                                 // 0x284    644     // unknown int32?
            public Int64 Offset288;                                 // 0x288    648
            public float UnknownFloat8;                             // 0x28C    656     // is float
            public float UnknownFloat9;                             // 0x290    660     // is float
            public Int32 Count298;                                  // 0x298    664
            public Int32 Count29C;                                  // 0x29C    668
            public Int64 Offset2A0;                                 // 0x2A0    672
            public Int32 Unknown16;                                 // 0x2A8    680     // is int32
            public Int32 Unknown17;                                 // 0x2AC    684     // is int32?
            public Int32 Unknown18;                                 // 0x2B0    688     // is int32?
            public Int32 Unknown19;                                 // 0x2B4    692     // is int32?
            public Int32 Unknown20;                                 // 0x2B8    696     // is int32?
            public Int32 Unknown21;                                 // 0x2BC    700     // is int32?
            public Int32 InternalInt323;                            // 0x2C0    704     // internally set to -1                             LoadRoomDefinition+286  898 or      dword ptr [rbx+2C0h], 0FFFFFFFFh
            public Int32 Unknown22;                                 // 0x2C4    708     // is int32?
            public Int64 InternalInt641;                            // 0x2C8    712     // internally set to ptr to fileBytes               LoadRoomDefinition+274  898 mov     [rbx+2C8h], r11
            public Int32 InternalInt324;                            // 0x2D0    720     // internally set to fileSize                       LoadRoomDefinition+201  898 mov     [rbx+2D0h], edx
            public Int32 Unknown23;                                 // 0x2D4    724     // is int32?
            // end of struct                                        // 0x2D8    728
        }

        private byte[] _fileBytes;

        private XmlDocument _xmlDocument;
        private XmlWriter _xmlWriter;
        private RoomDefinitionHeader _fileHeader;

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

            int bp = 0;
        }
    }
}
