using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Revival.Common;

namespace Hellgate
{
    public class LevelRulesFile
    {
        public const String FileExtention = ".drl";
        private const UInt32 FileMagicWord = 0xD2D21D25; // '%.ÒÒ'
        private const UInt32 RequiredVersion = 0x1E; // 30

        // total size = 328 bytes (0x148)
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class LevelRoom
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public String RoomName;         // has zero terminated string

            public Int32 Uknown1;           // not seen used, but is used - haven't finished going through entire function
            private Int32 Uknown2;           // not seen used
            private Int32 Uknown3;           // not seen used
            private Int32 Uknown4;           // not seen used

            // the three position floats have some weird calcs done to them, however they always seem to just be set to their same value. todo: look into.
            /*
                ParseLevelRuleRooms+DA   10A8 mov     rax, [rsp+10A8h+arg_20]
                ...
                ParseLevelRuleRooms+FA   10A8 addss   xmm0, dword ptr [rax]
                ParseLevelRuleRooms+FE   10A8 movss   xmm1, dword ptr [rax+4]
                ParseLevelRuleRooms+103  10A8 addss   xmm1, dword ptr [rsi+114h]
                ParseLevelRuleRooms+10B  10A8 movss   dword ptr [rsi+110h], xmm0
                ParseLevelRuleRooms+113  10A8 movss   xmm0, dword ptr [rsi+118h]
                ParseLevelRuleRooms+11B  10A8 addss   xmm0, dword ptr [rax+8]
                ParseLevelRuleRooms+120  10A8 movss   dword ptr [rsi+114h], xmm1
                ParseLevelRuleRooms+128  10A8 mov     eax, [rsp+10A8h+arg_28]
                ParseLevelRuleRooms+12F  10A8 movss   dword ptr [rsi+118h], xmm0
                ParseLevelRuleRooms+137  10A8 mov     [rsi+120h], eax
             */
            public float xPostion;          // x offset of room                                 // ParseLevelRuleRooms+EA   10A8 movss   xmm0, dword ptr [rsi+110h]
            public float yPosition;         // y offset of room                                 // ParseLevelRuleRooms+103  10A8 addss   xmm1, dword ptr [rsi+114h]
            public float zPosition;         // z offset of room                                 // ParseLevelRuleRooms+113  10A8 movss   xmm0, dword ptr [rsi+118h]
            public float rotation;          // rotation of room (about as yet unknown axis)     // ParseLevelRuleRooms+2AB  10A8 movss   xmm0, dword ptr [rsi+11Ch]
            // must be in radians - e.g. 90 degrees = pi/2 = 1.57079633

            private int unknownEAXValue;     // see above float calcs - has value being assigned

            // only seen these six floats as 0x00 in file, but are used/set further down. todo: look into
            /* RSI = ptr to start of room
                ParseLevelRuleRooms+392      loc_140340FE2:          ; Logical AND
                ParseLevelRuleRooms+392  10A8 and     qword ptr [rsi+140h], 0
                ParseLevelRuleRooms+39A  10A8 mov     eax, [rax+148h]
                ParseLevelRuleRooms+3A0  10A8 lea     rcx, [rsi+124h]
                ParseLevelRuleRooms+3A7  10A8 mov     [rsi+124h], eax
                ParseLevelRuleRooms+3AD  10A8 mov     eax, [r12+14Ch]
                ParseLevelRuleRooms+3B5  10A8 mov     [rsi+128h], eax
                ParseLevelRuleRooms+3BB  10A8 mov     eax, [r12+150h]
                ParseLevelRuleRooms+3C3  10A8 mov     [rsi+12Ch], eax
                ParseLevelRuleRooms+3C9  10A8 mov     eax, [r12+154h]
                ParseLevelRuleRooms+3D1  10A8 mov     [rsi+130h], eax
                ParseLevelRuleRooms+3D7  10A8 mov     eax, [r12+158h]
                ParseLevelRuleRooms+3DF  10A8 mov     [rsi+134h], eax
                ParseLevelRuleRooms+3E5  10A8 mov     eax, [r12+15Ch]
                ParseLevelRuleRooms+3ED  10A8 mov     [rsi+138h], eax
                ParseLevelRuleRooms+3F3  10A8 movss   xmm1, dword ptr [rsi+11Ch]
                ParseLevelRuleRooms+3FB  10A8 call    sub_14033DF84  ; does float stuffs - has sin() and cos() etc
                ParseLevelRuleRooms+3FB
                ParseLevelRuleRooms+400  10A8 movss   xmm0, dword ptr [rsi+110h]
                ParseLevelRuleRooms+408  10A8 addss   xmm0, dword ptr [rsi+124h]
                ParseLevelRuleRooms+410  10A8 movss   dword ptr [rsi+124h], xmm0
                ParseLevelRuleRooms+418  10A8 movss   xmm1, dword ptr [rsi+114h]
                ParseLevelRuleRooms+420  10A8 addss   xmm1, dword ptr [rsi+128h]
                ParseLevelRuleRooms+428  10A8 movss   dword ptr [rsi+128h], xmm1
                ParseLevelRuleRooms+430  10A8 movss   xmm0, dword ptr [rsi+118h]
                ParseLevelRuleRooms+438  10A8 addss   xmm0, dword ptr [rsi+12Ch]
                ParseLevelRuleRooms+440  10A8 movss   dword ptr [rsi+12Ch], xmm0
                ParseLevelRuleRooms+448  10A8 movss   xmm0, dword ptr [rsi+110h]
                ParseLevelRuleRooms+450  10A8 addss   xmm0, dword ptr [rsi+130h]
                ParseLevelRuleRooms+458  10A8 movss   dword ptr [rsi+130h], xmm0
                ParseLevelRuleRooms+460  10A8 movss   xmm1, dword ptr [rsi+114h]
                ParseLevelRuleRooms+468  10A8 addss   xmm1, dword ptr [rsi+134h]
                ParseLevelRuleRooms+470  10A8 movss   dword ptr [rsi+134h], xmm1
                ParseLevelRuleRooms+478  10A8 movss   xmm0, dword ptr [rsi+118h]
                ParseLevelRuleRooms+480  10A8 addss   xmm0, dword ptr [rsi+138h]
                ParseLevelRuleRooms+488  10A8 movss   dword ptr [rsi+138h], xmm0
             */
            private float UnknownFloat1;
            private float UnknownFloat2;
            private float UnknownFloat3;
            private float UnknownFloat4;
            private float UnknownFloat5;
            private float UnknownFloat6;

            private Int32 Unknown5;          // not seen used
            private int UnknownRAXValue;     // not read in, but assigned as rax or 0            // ParseLevelRuleRooms+386  10A8 mov     [rsi+140h], rax
            private Int32 Unknown6;          // not seen used
        }

        private byte[] _fileBytes;
        private String _filePath;

        private XmlDocument _xmlDocument;
        private XmlWriter _xmlWriter;
        private LevelRoom[][] _levelRules;

        public LevelRulesFile()
        {
        }

        public bool ParseFileBytes(byte[] fileBytes, String filePath)
        {
            Debug.Assert(fileBytes != null);
            _fileBytes = fileBytes;
            _filePath = filePath;

            _xmlDocument = new XmlDocument();
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineOnAttributes = true
            };
            _xmlWriter = XmlWriter.Create(_filePath.Replace(FileExtention, ".xml"), settings);
            Debug.Assert(_xmlWriter != null);

            // file header checks
            UInt32 fileMagicWord = FileTools.ByteArrayToUInt32(fileBytes, 0);
            if (fileMagicWord != FileMagicWord) throw new Exceptions.UnexpectedMagicWordException();

            UInt32 fileVersion = FileTools.ByteArrayToUInt32(fileBytes, 4);
            if (fileVersion != RequiredVersion) throw new Exceptions.NotSupportedFileVersionException();


            // static rules
            int staticRulesCount = FileTools.ByteArrayToInt32(fileBytes, 0x218); // 536
            int staticRulesDetailsOffset = FileTools.ByteArrayToInt32(fileBytes, 0x220); // 544
            if (staticRulesCount != 0 && staticRulesDetailsOffset != 0)
            {
                if (staticRulesCount > 1)
                {
                    int bp = 0;
                }

                _ParseStaticRooms(staticRulesCount, staticRulesDetailsOffset);
            }


            // random rules
            int randomRulesCount = FileTools.ByteArrayToInt32(fileBytes, 0x228); // 552
            int randomRulesDetailsOffset = FileTools.ByteArrayToInt32(fileBytes, 0x230); // 560
            if (randomRulesCount != 0 && randomRulesDetailsOffset != 0)
            {
                _ParseRandomRules(randomRulesCount, randomRulesDetailsOffset);
            }


            _xmlWriter.Close();

            return true;
        }

        private void _ParseStaticRooms(int count, int offset)
        {
            _xmlWriter.WriteStartElement("LevelRules");

            _levelRules = new LevelRoom[count][];
            for (int i = 0; i < count; i++)
            {
                int roomsCount = FileTools.ByteArrayToInt32(_fileBytes, offset);
                offset += 8;
                int roomsOffset = FileTools.ByteArrayToInt32(_fileBytes, offset);
                offset += 8;
                
                _levelRules[i] = FileTools.ByteArrayToArray<LevelRoom>(_fileBytes, roomsOffset, roomsCount);
            }

            XmlSerializer xmlSerializer = new XmlSerializer(_levelRules.GetType());
            xmlSerializer.Serialize(_xmlWriter, _levelRules);
            _xmlWriter.WriteEndElement();
        }

        private void _ParseRandomRules(int count, int offset)
        {
            _xmlWriter.WriteStartElement("LevelRules");
            for (int i = 0; i < count; i++, offset += 248)
            {
                int connectorRoomsCount = FileTools.ByteArrayToInt32(_fileBytes, offset);
                offset += 8;
                int connectorRoomsOffset = FileTools.ByteArrayToInt32(_fileBytes, offset);
                offset += 8;
                int ruleCount = FileTools.ByteArrayToInt32(_fileBytes, offset);
                offset += 4;


                // get first "connector" (?) rule
                LevelRoom[] connectorRooms = FileTools.ByteArrayToArray<LevelRoom>(_fileBytes, connectorRoomsOffset, connectorRoomsCount);
                XmlSerializer xmlSerializer1 = new XmlSerializer(connectorRooms.GetType());
                xmlSerializer1.Serialize(_xmlWriter, connectorRooms);


                // then the actual level rules
                _levelRules = new LevelRoom[ruleCount][];
                for (int j = 0; j < ruleCount; j++, offset += 4)
                {
                    int roomCount = FileTools.ByteArrayToInt32(_fileBytes, offset);
                    int ruleOffset = FileTools.ByteArrayToInt32(_fileBytes, offset + 132 + j * 4);

                    _levelRules[j] = FileTools.ByteArrayToArray<LevelRoom>(_fileBytes, ruleOffset, roomCount);
                }
                offset += 128 + ruleCount * 8;

                XmlSerializer xmlSerializer2 = new XmlSerializer(_levelRules.GetType());
                xmlSerializer2.Serialize(_xmlWriter, _levelRules);
            }
            _xmlWriter.WriteEndElement();
        }
    }
}
