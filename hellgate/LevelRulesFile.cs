﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Serialization;
using Revival.Common;

namespace Hellgate
{
    public class LevelRulesFile : HellgateFile
    {
        public new const String Extension = ".drl";
        public new const String ExtensionDeserialised = ".drl.xml";
        private const UInt32 FileMagicWord = 0xD2D21D25; // '%.ÒÒ'
        private const UInt32 RequiredVersion = 0x1E; // 30

        #region Structure Definitions
#pragma warning disable 169

        // total size = 556 bytes (0x22C)
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class LevelRulesHeader
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public String RuleName;                                     // 0x00     0       // rule name is "RuleName[QuestScript].CODE, where "[QuestScript]" and ".CODE" are optional (CODE from LEVELS_FILES_PATHS)
            internal Int32 _internalInt321;                             // 0x100    256     // is always -1
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 67)]
            private Int32[] _internalUnknowns;                          // 0x104    260     // is all 0's or garbage in every file; probably internal/reserved
            internal Int64 StaticRulesCount;                            // 0x20C    524
            internal Int64 StaticRulesFooterOffset;                     // 0x214    532
            internal Int64 RandomRulesCount;                            // 0x21C    540
            internal Int64 RandomRulesFooterOffset;                     // 0x224    548
            // end of struct                                            // 0x22C    556
        }

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
            public float xPosition;             // x offset of room                                     // ParseLevelRuleRooms+EA   10A8 movss   xmm0, dword ptr [rsi+110h]
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
            public LevelRulesHeader FileHeader;

            [XmlElement("LevelRule")]
            public LevelRule[] LevelRules;
        }

#pragma warning restore 169
        #endregion

        private XmlLevelRules _xmlLevelRules;
        public IEnumerable<LevelRule> LevelRules { get { return _xmlLevelRules.LevelRules; } }
        public String Name { get { return _xmlLevelRules.FileHeader.RuleName; } }

        /// <summary>
        /// Parses a level rules file bytes.
        /// </summary>
        /// <param name="fileBytes">The bytes of the level rules to parse.</param>
        public override void ParseFileBytes(byte[] fileBytes)
        {
            // sanity check
            if (fileBytes == null) throw new ArgumentNullException("fileBytes", "File bytes cannot be null!");
            _xmlLevelRules = new XmlLevelRules();
            int offset = 0;


            // file header checks
            UInt32 fileMagicWord = FileTools.ByteArrayToUInt32(fileBytes, ref offset);
            if (fileMagicWord != FileMagicWord) throw new Exceptions.UnexpectedMagicWordException();

            UInt32 fileVersion = FileTools.ByteArrayToUInt32(fileBytes, ref offset);
            if (fileVersion != RequiredVersion) throw new Exceptions.NotSupportedFileVersionException();


            // main file header
            _xmlLevelRules.FileHeader = FileTools.ByteArrayToStructure<LevelRulesHeader>(fileBytes, ref offset);
            LevelRulesHeader header = _xmlLevelRules.FileHeader;


            // static rules
            if (header.StaticRulesCount != 0 && header.StaticRulesFooterOffset != 0)
            {
                _xmlLevelRules.LevelRules = new LevelRule[header.StaticRulesCount];
                int staticRulesFooterOffset = (int)header.StaticRulesFooterOffset;
                for (int i = 0; i < header.StaticRulesCount; i++)
                {
                    int roomsCount = FileTools.ByteArrayToInt32(fileBytes, staticRulesFooterOffset);
                    staticRulesFooterOffset += 8;
                    int roomsOffset = FileTools.ByteArrayToInt32(fileBytes, staticRulesFooterOffset);
                    staticRulesFooterOffset += 8;

                    _xmlLevelRules.LevelRules[i] = new LevelRule
                    {
                        StaticRooms = FileTools.ByteArrayToArray<Room>(fileBytes, roomsOffset, roomsCount)
                    };
                    offset += Marshal.SizeOf(typeof(Room)) * roomsCount + 16;
                }
            }


            // random rules
            if (header.RandomRulesCount != 0 && header.RandomRulesFooterOffset != 0)
            {
                _xmlLevelRules.LevelRules = new LevelRule[header.RandomRulesCount]; // checked all files an no files have both static and random, so no chance of overwriting static above
                LevelRulesRandomFooter[] levelRulesFooters = FileTools.ByteArrayToArray<LevelRulesRandomFooter>(fileBytes, (int)header.RandomRulesFooterOffset, (int)header.RandomRulesCount);
                offset += Marshal.SizeOf(typeof(LevelRulesRandomFooter)) * (int)header.RandomRulesCount;

                for (int i = 0; i < header.RandomRulesCount; i++)
                {
                    _xmlLevelRules.LevelRules[i] = new LevelRule
                    {
                        // get first "connector" (?) rule
                        ConnectorRooms = FileTools.ByteArrayToArray<Room>(fileBytes, (Int32)levelRulesFooters[i].ConnectorRuleOffset, (Int32)levelRulesFooters[i].ConnectorRoomCount),

                        // then the actual level rules
                        Rules = new Room[levelRulesFooters[i].RuleCount][]
                    };
                    offset += Marshal.SizeOf(typeof(Room)) * (int)levelRulesFooters[i].ConnectorRoomCount;

                    for (int j = 0; j < levelRulesFooters[i].RuleCount; j++)
                    {
                        _xmlLevelRules.LevelRules[i].Rules[j] = FileTools.ByteArrayToArray<Room>(fileBytes, (Int32)levelRulesFooters[i].RuleOffsets[j], levelRulesFooters[i].RoomCounts[j]);
                        offset += Marshal.SizeOf(typeof(Room)) * levelRulesFooters[i].RoomCounts[j];
                    }
                }
            }


            // final debug check
            Debug.Assert(offset == fileBytes.Length);
        }

        /// <summary>
        /// Parses and XML document and initialises the class.
        /// </summary>
        /// <param name="xmlDocument">The XML Document to parse.</param>
        public void ParseXmlDocument(XmlDocument xmlDocument)
        {
            XmlNodeReader xmlNodeReader = new XmlNodeReader(xmlDocument);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof (XmlLevelRules));
            _xmlLevelRules = (XmlLevelRules) xmlSerializer.Deserialize(xmlNodeReader);
        }

        /// <summary>
        /// Generates a native file byte array of the object.
        /// </summary>
        /// <returns>The byte array to save.</returns>
        public override byte[] ToByteArray()
        {
            if (_xmlLevelRules == null) throw new Exceptions.NotInitializedException();
            if (String.IsNullOrEmpty(_xmlLevelRules.FileHeader.RuleName)) throw new Exceptions.InvalidXmlElement("LevelRulesName", "Cannot be empty.");
            if (_xmlLevelRules.LevelRules == null) throw new Exceptions.InvalidXmlElement("LevelRules", "Cannot be empty.");
            int ruleCount = _xmlLevelRules.LevelRules.Length;
            if (ruleCount == 0) throw new Exceptions.InvalidXmlElement("LevelRules", "Cannot be empty.");

            int offset = 0;
            byte[] fileBytes = new byte[1024];


            // write header
            FileTools.WriteToBuffer(ref fileBytes, ref offset, FileMagicWord);
            FileTools.WriteToBuffer(ref fileBytes, ref offset, RequiredVersion);


            // level rules header
            offset += Marshal.SizeOf(typeof (LevelRulesHeader));


            // level rules
            LevelRulesStaticFooter[] levelRulesStaticFooters = null;
            LevelRulesRandomFooter[] levelRulesRandomFooters = null;
            for (int i = 0; i < ruleCount; i++)
            {
                LevelRule levelRule = _xmlLevelRules.LevelRules[i];

                // static rules
                if (_xmlLevelRules.LevelRules[i].StaticRooms != null)
                {
                    if (levelRule.StaticRooms.Length == 0) throw new Exceptions.InvalidXmlElement("StaticRooms", "Cannot be empty");

                    if (levelRulesStaticFooters == null) levelRulesStaticFooters = new LevelRulesStaticFooter[ruleCount];
                    levelRulesStaticFooters[i] = new LevelRulesStaticFooter
                    {
                        RoomCount = levelRule.StaticRooms.Length,
                        RuleOffset = offset
                    };

                    foreach (Room room in levelRule.StaticRooms) FileTools.WriteToBuffer(ref fileBytes, ref offset, room);
                }
                else if (_xmlLevelRules.LevelRules[i].Rules != null) // random rules
                {
                    if (levelRule.Rules.Length == 0) throw new Exceptions.InvalidXmlElement("Rules", "Cannot be empty");

                    if (levelRulesRandomFooters == null) levelRulesRandomFooters = new LevelRulesRandomFooter[ruleCount];
                    levelRulesRandomFooters[i] = new LevelRulesRandomFooter();

                    if (levelRule.ConnectorRooms != null && levelRule.ConnectorRooms.Length > 0)
                    {
                        levelRulesRandomFooters[i].ConnectorRoomCount = levelRule.ConnectorRooms.Length;
                        levelRulesRandomFooters[i].ConnectorRuleOffset = offset;
                        foreach (Room room in levelRule.ConnectorRooms) FileTools.WriteToBuffer(ref fileBytes, ref offset, room);
                    }

                    levelRulesRandomFooters[i].RuleCount = levelRule.Rules.Length;
                    levelRulesRandomFooters[i].RoomCounts = new Int32[32];
                    levelRulesRandomFooters[i].RuleOffsets = new Int64[32];
                    for (int j = 0; j < levelRule.Rules.Length; j++)
                    {
                        levelRulesRandomFooters[i].RoomCounts[j] = levelRule.Rules[j].Length;
                        levelRulesRandomFooters[i].RuleOffsets[j] = offset;
                        foreach (Room room in levelRule.Rules[j]) FileTools.WriteToBuffer(ref fileBytes, ref offset, room);
                    }
                }
                else
                {
                    throw new Exceptions.InvalidXmlDocument("No StaticRooms or Rules elements found!");
                }
            }


            // footer details and initial offsets
            _xmlLevelRules.FileHeader._internalInt321 = -1;
            if (levelRulesStaticFooters != null)
            {
                _xmlLevelRules.FileHeader.StaticRulesCount = ruleCount;
                _xmlLevelRules.FileHeader.StaticRulesFooterOffset = offset;
                FileTools.WriteToBuffer(ref fileBytes, ref offset, levelRulesStaticFooters.ToByteArray());
            }
            if (levelRulesRandomFooters != null)
            {
                _xmlLevelRules.FileHeader.RandomRulesCount = ruleCount;
                _xmlLevelRules.FileHeader.RandomRulesFooterOffset = offset;
                FileTools.WriteToBuffer(ref fileBytes, ref offset, levelRulesRandomFooters.ToByteArray());
            }


            // write file header
            FileTools.WriteToBuffer(ref fileBytes, 0x08, _xmlLevelRules.FileHeader);


            // and we're done
            Array.Resize(ref fileBytes, offset);
            return fileBytes;
        }

        public override byte[] ExportAsDocument()
        {
            if (_xmlLevelRules == null) throw new Exceptions.NotInitializedException();

            MemoryStream memoryStream = new MemoryStream();
            XmlSerializer xmlSerializerHeader = new XmlSerializer(_xmlLevelRules.GetType());
            xmlSerializerHeader.Serialize(memoryStream, _xmlLevelRules);

            return memoryStream.ToArray();
        }
    }
}