using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using Revival.Common;

namespace Hellgate
{
    public class LevelRulesFile : IDisposable
    {
        public const String FileExtension = ".drl";
        public const String FileExtensionXml = ".drl.xml";
        private const UInt32 FileMagicWord = 0xD2D21D25; // '%.ÒÒ'
        private const UInt32 RequiredVersion = 0x1E; // 30

        // total size = 408 bytes (0x198)
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private class LevelRulesRandomFooter
        {
            public Int64 ConnectorRoomCount;                            // I *think* this is the connector rule - rule_pmt01.drl ends with bldg_E_PedMall which is what rule_pmt02.drl starts with
            public Int64 ConnectorRuleOffset;                           // offset to connector rule
            public Int32 RuleCount;                                     // below rule counts
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public Int32[] RoomCounts;                                  // counts of LevelRooms in the rule
            public Int32 Null;                                          // tested on all files - always 0
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public Int64[] RuleOffsets;                                 // offset to rules
        }

        // total size = 16 bytes (0x10)
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private class LevelRulesStaticFooter
        {
            public Int64 RoomCount;                                     // room count in rule
            public Int64 RuleOffset;                                    // offset to rule
        }

        // total size = 328 bytes (0x148)
        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class Room
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public String RoomName;             // has zero terminated string

            public Int32 Unknown;               // not seen used, but is used - haven't finished going through entire function
            private Int32 _internalUnknown1;    // not seen used, but tested on all files - always 0
            private Int32 _internalUnknown2;    // not seen used, but tested on all files - always 0
            private Int32 _internalUnknown3;    // not seen used, but tested on all files - always 0

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
            public float xPosition;              // x offset of room                                     // ParseLevelRuleRooms+EA   10A8 movss   xmm0, dword ptr [rsi+110h]
            public float yPosition;             // y offset of room                                     // ParseLevelRuleRooms+103  10A8 addss   xmm1, dword ptr [rsi+114h]
            public float zPosition;             // z offset of room                                     // ParseLevelRuleRooms+113  10A8 movss   xmm0, dword ptr [rsi+118h]
            public float rotation;              // rotation of room (about as yet unknown axis)         // ParseLevelRuleRooms+2AB  10A8 movss   xmm0, dword ptr [rsi+11Ch]
            // must be in radians - e.g. 90 degrees = pi/2 = 1.57079633

            private int _internalEAXValue;      // not seen used, but tested on all files - always 0    // see above float calcs - has value being assigned

            // only seen these six floats as 0x00 in file, but are used/set further down - all internal so doesn't matter
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
            private float _internalFloat1;      // not seen used, but tested on all files - always 0
            private float _internalFloat2;      // not seen used, but tested on all files - always 0
            private float _internalFloat3;      // not seen used, but tested on all files - always 0
            private float _internalFloat4;      // not seen used, but tested on all files - always 0
            private float _internalFloat5;      // not seen used, but tested on all files - always 0
            private float _internalFloat6;      // not seen used, but tested on all files - always 0

            private Int32 _internalUnknown4;    // not seen used, but tested on all files - always 0
            private Int32 _internalRAXValue;    // not seen used, but tested on all files - always 0    // ParseLevelRuleRooms+386  10A8 mov     [rsi+140h], rax (rax or 0x00)
            private Int32 _internalUnknown5;    // not seen used, but tested on all files - always 0
        }

        [XmlRootAttribute("LevelRule")]
        public class LevelRule
        {
            [XmlArray("ConnectorRule")]
            public Room[] ConnectorRooms;

            [XmlArrayItem("Rule")]
            public Room[][] Rules;

            [XmlElement("Room")]
            public Room[] StaticRooms;
        }

        [XmlRootAttribute("LevelRules")]
        public class XmlLevelRules
        {
            public String LevelRulesName;

            [XmlElement("LevelRule")]
            public LevelRule[] LevelRules;
        }

        private XmlLevelRules _xmlLevelRules;
        public LevelRule[] LevelRules { get { return _xmlLevelRules.LevelRules; } }
        public String Name { get { return _xmlLevelRules.LevelRulesName; } }

        private byte[] _fileBytes;

        private XmlDocument _xmlDocument;
        private XmlWriter _xmlWriter;

        public LevelRulesFile()
        {

        }

        /// <summary>
        /// Parses and XML document and returns the serialized byte array.
        /// </summary>
        /// <param name="xmlDocument">The XML Document to parse.</param>
        /// <returns>The serialized byte array.</returns>
        public byte[] ParseXmlDocument(XmlDocument xmlDocument)
        {
            XmlNodeReader xmlNodeReader = new XmlNodeReader(xmlDocument);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(XmlLevelRules));
            XmlLevelRules xmlLevelRules = (XmlLevelRules)xmlSerializer.Deserialize(xmlNodeReader);

            int offset = 0;
            _fileBytes = new byte[1024];


            // some sanity checks
            if (String.IsNullOrEmpty(xmlLevelRules.LevelRulesName)) throw new Exceptions.InvalidXmlElement("LevelRulesName", "Cannot be empty.");
            if (xmlLevelRules.LevelRules == null) throw new Exceptions.InvalidXmlElement("LevelRules", "Cannot be empty.");
            int ruleCount = xmlLevelRules.LevelRules.Length;
            if (ruleCount == 0) throw new Exceptions.InvalidXmlElement("LevelRules", "Cannot be empty.");


            // write header
            FileTools.WriteToBuffer(ref _fileBytes, ref offset, FileMagicWord);
            FileTools.WriteToBuffer(ref _fileBytes, ref offset, RequiredVersion);


            // level rules name
            FileTools.WriteToBuffer(ref _fileBytes, offset, xmlLevelRules.LevelRulesName);
            offset += 256;
            FileTools.WriteToBuffer(ref _fileBytes, ref offset, -1); // has 0xFFFFFFFF at end of 256 bytes for some reason


            // level rules
            offset = 0x238; // 568
            LevelRulesStaticFooter[] levelRulesStaticFooters = null;
            LevelRulesRandomFooter[] levelRulesRandomFooters = null;
            for (int i = 0; i < ruleCount; i++)
            {
                LevelRule levelRule = xmlLevelRules.LevelRules[i];

                // static rules
                if (xmlLevelRules.LevelRules[i].StaticRooms != null)
                {
                    if (levelRule.StaticRooms.Length == 0) throw new Exceptions.InvalidXmlElement("StaticRooms", "Cannot be empty");

                    if (levelRulesStaticFooters == null) levelRulesStaticFooters = new LevelRulesStaticFooter[ruleCount];
                    levelRulesStaticFooters[i] = new LevelRulesStaticFooter
                    {
                        RoomCount = levelRule.StaticRooms.Length,
                        RuleOffset = offset
                    };

                    foreach (Room room in levelRule.StaticRooms) FileTools.WriteToBuffer(ref _fileBytes, ref offset, room);
                }
                else if (xmlLevelRules.LevelRules[i].Rules != null) // random rules
                {
                    if (levelRule.Rules.Length == 0) throw new Exceptions.InvalidXmlElement("Rules", "Cannot be empty");

                    if (levelRulesRandomFooters == null) levelRulesRandomFooters = new LevelRulesRandomFooter[ruleCount];
                    levelRulesRandomFooters[i] = new LevelRulesRandomFooter();

                    if (levelRule.ConnectorRooms != null && levelRule.ConnectorRooms.Length > 0)
                    {
                        levelRulesRandomFooters[i].ConnectorRoomCount = levelRule.ConnectorRooms.Length;
                        levelRulesRandomFooters[i].ConnectorRuleOffset = offset;
                        foreach (Room room in levelRule.ConnectorRooms) FileTools.WriteToBuffer(ref _fileBytes, ref offset, room);
                    }

                    levelRulesRandomFooters[i].RuleCount = levelRule.Rules.Length;
                    levelRulesRandomFooters[i].RoomCounts = new Int32[32];
                    levelRulesRandomFooters[i].RuleOffsets = new Int64[32];
                    for (int j = 0; j < levelRule.Rules.Length; j++)
                    {
                        levelRulesRandomFooters[i].RoomCounts[j] = levelRule.Rules[j].Length;
                        levelRulesRandomFooters[i].RuleOffsets[j] = offset;
                        foreach (Room room in levelRule.Rules[j]) FileTools.WriteToBuffer(ref _fileBytes, ref offset, room);
                    }
                }
                else
                {
                    throw new Exceptions.InvalidXmlDocument("No StaticRooms or Rules elements found!");
                }
            }


            // footer details and initial offsets
            if (levelRulesStaticFooters != null)
            {
                FileTools.WriteToBuffer(ref _fileBytes, 0x218, ruleCount); // 536
                FileTools.WriteToBuffer(ref _fileBytes, 0x220, offset); // 544
                FileTools.WriteToBuffer(ref _fileBytes, ref offset, levelRulesStaticFooters.ToByteArray());
            }
            if (levelRulesRandomFooters != null)
            {
                FileTools.WriteToBuffer(ref _fileBytes, 0x228, ruleCount); // 552
                FileTools.WriteToBuffer(ref _fileBytes, 0x230, offset); // 560
                FileTools.WriteToBuffer(ref _fileBytes, ref offset, levelRulesRandomFooters.ToByteArray());
            }


            // and we're done
            Array.Resize(ref _fileBytes, offset);
            return _fileBytes;
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
            UInt32 fileMagicWord = FileTools.ByteArrayToUInt32(fileBytes, 0x00);
            if (fileMagicWord != FileMagicWord) throw new Exceptions.UnexpectedMagicWordException();

            UInt32 fileVersion = FileTools.ByteArrayToUInt32(fileBytes, 0x04);
            if (fileVersion != RequiredVersion) throw new Exceptions.NotSupportedFileVersionException();


            // get rule name
            _xmlLevelRules = new XmlLevelRules
            {
                LevelRulesName = FileTools.ByteArrayToStringASCII(_fileBytes, 0x08)
            };


            // static rules
            int staticRulesCount = FileTools.ByteArrayToInt32(fileBytes, 0x218); // 536
            int staticRulesDetailsOffset = FileTools.ByteArrayToInt32(fileBytes, 0x220); // 544
            if (staticRulesCount != 0 && staticRulesDetailsOffset != 0)
            {
                _xmlLevelRules.LevelRules = new LevelRule[staticRulesCount];
                _ParseStaticRooms(staticRulesCount, staticRulesDetailsOffset);
            }


            // random rules
            int randomRulesCount = FileTools.ByteArrayToInt32(fileBytes, 0x228); // 552
            int randomRulesDetailsOffset = FileTools.ByteArrayToInt32(fileBytes, 0x230); // 560
            if (randomRulesCount != 0 && randomRulesDetailsOffset != 0)
            {
                _xmlLevelRules.LevelRules = new LevelRule[randomRulesCount]; // checked all files an no files have both static and random, so no chance of overwriting static above
                _ParseRandomRules(randomRulesCount, randomRulesDetailsOffset);
            }


            // write xml object
            XmlSerializer xmlSerializer = new XmlSerializer(_xmlLevelRules.GetType());
            xmlSerializer.Serialize(_xmlWriter, _xmlLevelRules);

            _xmlWriter.Close();
        }

        public void SaveXmlDocument(String filePath)
        {
            if (_xmlDocument == null) return;

            _xmlDocument.Save(filePath);
        }

        private void _ParseStaticRooms(int count, int offset)
        {
            for (int i = 0; i < count; i++)
            {
                int roomsCount = FileTools.ByteArrayToInt32(_fileBytes, offset);
                offset += 8;
                int roomsOffset = FileTools.ByteArrayToInt32(_fileBytes, offset);
                offset += 8;

                _xmlLevelRules.LevelRules[i] = new LevelRule
                {
                    StaticRooms = FileTools.ByteArrayToArray<Room>(_fileBytes, roomsOffset, roomsCount)
                };
            }
        }

        private void _ParseRandomRules(int count, int offset)
        {
            LevelRulesRandomFooter[] levelRulesFooters = FileTools.ByteArrayToArray<LevelRulesRandomFooter>(_fileBytes, offset, count);

            for (int i = 0; i < count; i++)
            {
                _xmlLevelRules.LevelRules[i] = new LevelRule
                {
                    // get first "connector" (?) rule
                    ConnectorRooms = FileTools.ByteArrayToArray<Room>(_fileBytes, (Int32)levelRulesFooters[i].ConnectorRuleOffset, (Int32)levelRulesFooters[i].ConnectorRoomCount),

                    // then the actual level rules
                    Rules = new Room[levelRulesFooters[i].RuleCount][]
                };

                for (int j = 0; j < levelRulesFooters[i].RuleCount; j++)
                {
                    _xmlLevelRules.LevelRules[i].Rules[j] = FileTools.ByteArrayToArray<Room>(_fileBytes, (Int32)levelRulesFooters[i].RuleOffsets[j], levelRulesFooters[i].RoomCounts[j]);

                    //foreach (Room room in _xmlLevelRules.LevelRules[i].Rules[j])
                    //{
                    //    Console.WriteLine(room.Unknown);
                    //}
                }
            }
        }

        public void Dispose()
        {
            if (_xmlWriter != null) _xmlWriter.Close();
        }
    }
}
