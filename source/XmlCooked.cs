using System;
using System.Diagnostics;
using System.Xml;

namespace Reanimator
{
    class XmlCooked
    {
        XmlDocument XmlDoc { get; set; }

        public XmlCooked()
        {
            XmlDoc = new XmlDocument();
        }

        public void SaveXml(String path)
        {
            if (XmlDoc == null || String.IsNullOrEmpty(path)) return;

            XmlDoc.Save(path);
        }

        public bool ParseData(byte[] data)
        {
            if (data == null) return false;

            int offset = 0;

            int fileHeadToken = FileTools.ByteArrayTo<Int32>(data, ref offset);
            if (fileHeadToken != 0x6B304F43) return false;  // 'CO0k'

            int version = FileTools.ByteArrayTo<Int32>(data, ref offset);
            if (version != 0x08) return false;

            XmlElement mainElement = XmlDoc.CreateElement("CO0k");
            XmlDoc.AppendChild(mainElement);

            XmlElement versionElement = XmlDoc.CreateElement("version");
            versionElement.InnerText = "8";
            mainElement.AppendChild(versionElement);


            /* ==Strange Array==
             * 4 bytes		unknown
             * 2 bytes		Type Token
             * *remaining based on token*
             * 
             * =Token=		=Followed By=
             *  00 00       4 bytes (Int32?  Or UInt32 because 00 07 appears to have -1?)
             *  00 01       4 bytes (Float)
             *  00 02       1 byte  (Str Length (NOT inc \0) - if != 0, string WITH \0)
             *  00 06       4 bytes (Float)
             *  00 07       4 bytes (Int32?  Or possibly an array with -1 = end?)
             *  01 0B       8 bytes	(Null Int32?,  32 bit Bitmask)
             *  02 0C       8 bytes	(Int32 index?,  Int32 value?)
             *  03 00       2 bytes	(ShortInt?)
             *  03 09       4 bytes	(Int32?)
             *  05 00       2 bytes (??)
             *  08 03       8 bytes	(Int32??,  Int32??)
             *  09 03       4 bytes (Int32)
             *  0A 03       8 bytes	(Int32??,  Int32??)
             *  C0 00       2 bytes (??)
             *  
             *  // extras found in particle effects
             *  8D 00       2 bytes
             *  00 0A       4 bytes (Int32?)
             *  06 01       8 bytes (Int32?, Int32?)
             *  00 05       4 bytes (Float)
             */

            XmlElement unknownArray = XmlDoc.CreateElement("unknownArray");
            mainElement.AppendChild(unknownArray);

            while (true)
            {
                uint unknown = FileTools.ByteArrayTo<UInt32>(data, ref offset);

                if (unknown == 0x41544144) break;     // 'DATA'

                ushort token = FileTools.ByteArrayTo<ushort>(data, ref offset);

                XmlElement arrayElement = XmlDoc.CreateElement("0x" + unknown.ToString("X8"));
                arrayElement.SetAttribute("token", "0x" + token.ToString("X4"));
                unknownArray.AppendChild(arrayElement);

                XmlElement element;
                switch (token)
                {
                    case 0x0200:    // string
                        String str = _DoByteString(data, ref offset, arrayElement, "str");
                        if (!String.IsNullOrEmpty(str) && str.Length != 0)
                        {
                            offset++; // need to include \0 as it isn't included in the strLen byte for some reason
                        }

                        break;

                    case 0x0003:
                    case 0x0005:
                    case 0x00C0:
                    case 0x008D: // found in particle effects
                        short s1 = FileTools.ByteArrayTo<short>(data, ref offset);

                        element = XmlDoc.CreateElement("short");
                        element.InnerText = "0x" + s1.ToString("X4");
                        arrayElement.AppendChild(element);

                        break;

                    case 0x0000:
                    case 0x0309:
                    case 0x0700:
                    case 0x0903:
                    case 0x0A00: // found in particle effects
                        _DoInt(data, ref offset, arrayElement, "int");
                        break;

                    case 0x0100:
                    case 0x0600:
                    case 0x0500: // found in particle effects
                        _DoFloat(data, ref offset, arrayElement, "float");
                        break;

                    case 0x0B01:
                        _DoInt(data, ref offset, arrayElement, "int");
                        _DoBitmask(data, ref offset, arrayElement, "bitmask");
                        break;

                    case 0x0308:
                    case 0x030A:
                    case 0x0106: // found in particle effects
                        _DoInt(data, ref offset, arrayElement, "int1");
                        _DoInt(data, ref offset, arrayElement, "int2");
                        break;

                    case 0x0C02:
                        _DoInt(data, ref offset, arrayElement, "index");
                        _DoInt(data, ref offset, arrayElement, "int");
                        break;

                    default:
                        Debug.Assert(false, "Unknown Token: 0x" + token.ToString("X"));
                        break;
                }

            }

            XmlElement dataElement = XmlDoc.CreateElement("DATA");
            mainElement.AppendChild(dataElement);

            Debug.Assert(_ParseData(data, ref offset, dataElement));

            Debug.Assert(offset == data.Length);

            return true;
        }

        private bool _ParseData(byte[] data, ref int offset, XmlElement parentElement)
        {
            byte dataToken = FileTools.ByteArrayTo<byte>(data, ref offset);

            /* Seen as 0x04 and 0x05
             * If 0x05, then has a _DoZeroString() part first, then like 0x04 as per normal.
             */
            if (dataToken == 0x05)
            {
                _DoZeroString(data, ref offset, parentElement, "StringHeader");
            }

            int branchCount = FileTools.ByteArrayTo<Int32>(data, ref offset);
            for (int i = 0; i < branchCount; i++)
            {
                if (!_ParseBranch(data, ref offset, parentElement)) return false;
            }

            return true;
        }

        private bool _ParseBranch(byte[] data, ref int offset, XmlElement parentElement)
        {
            byte branchToken = FileTools.ByteArrayTo<byte>(data, ref offset);

            byte strLen = FileTools.ByteArrayTo<byte>(data, ref offset);
            String elementName = strLen != 0xFF ? FileTools.ByteArrayToStringAnsi(data, ref offset, strLen) : "NoName";

            XmlElement xmlElement = XmlDoc.CreateElement(elementName);
            parentElement.AppendChild(xmlElement);

            /* Seen as 0x05 and 0x07
             * If 0x07, then has a float part with it (seen usually with loop-type sections - I'm assuming a delay time between loops), then like 0x05 as per normal.
             */
            if (branchToken == 0x07)
            {
                float f = FileTools.ByteArrayTo<float>(data, ref offset);
                xmlElement.SetAttribute("fDelay", f.ToString("F5"));
            }

            int elementCount = FileTools.ByteArrayTo<Int32>(data, ref offset);
            for (int i = 0; i < elementCount; i++)
            {
                if (!_ParseElements(data, ref offset, xmlElement)) return false;
            }

            return true;
        }

        [FlagsAttribute]
        private enum BitField1 : uint
        {
            TypeString100 = (1 << 0),
            // is this entire thing a bit mask???  - todo: test me
            Bitmask101 = (1 << 1),              // found in hellgatedeath.xml.cooked (Fire Lasers to All) with Bitmask115 as well - note: so far, is *assumed* with same bitmask as rest
            Bitmask102 = (1 << 2),              // found in orbilezapcorpse.xml.cooked (State - Remove Target and Set State: orbile_angry) - note: assumed to be with bitmask as with rest
            Bitmask103 = (1 << 3),
            Bitmask104 = (1 << 4),              // found in spectralzap.xml.cooked (Fire Laser) - 104,5,6 all found set in furylightningzap.xml.cooked (Fire Laser)
            Bitmask105 = (1 << 5),
            Bitmask106 = (1 << 6),
            Bitmask107 = (1 << 7),
            Bitmask108 = (1 << 8),              // found in fragblaster.xml.cooked (Start Mode Weapon: item_equipped_idle) - note: assumed to be with bitmask as with rest
            Bitmask109 = (1 << 9),              // found in fellhordediggerball.xml.cooked (Fire Missile: Fell Horde Digger Ball)
            Bitmask111 = (1 << 11),             // found in minionmeleeattack2.xml.cooked (Add Attachment: weapons\HolyFire\Grenade impact.xml) - note: assumed to be with bitmask as with rest
            Bitmask112 = (1 << 12),
            Bitmask114 = (1 << 14),             // found in explodingbarrelexplode.xml.cooked (Fire Missile Gibs)
            Bitmask115 = (1 << 15),
            Bitmask116 = (1 << 16),
            Bitmask117 = (1 << 17),             // found in nanobotsdeath.xml.cooked (State - Clear: nanobots_attack) - note: assumed to be with bitmask as with rest
            Bitmask118 = (1 << 18),             // found in masterfiendmeleespin.xml.cooked - note: assumed to be with bitmask as with rest
            Bitmask119 = (1 << 19),             // found in blink.xml.cooked, same with cansexplode.xml.cooked            doesn't appear to do anything in file though...
            Bitmask120 = (1 << 20),
            IntValue121 = (1 << 21),            // found in physicalmirvmissile.xml.cooked (Fire Missile Nova = 1048576 (0x100000); is it a bitmask with above??)
            Bitmask123 = (1 << 23),             // found in throwsword.xml.cooked (State - Set: dont_draw_quick) - note: assumed to be with bitmask as with rest
            Bitmask124 = (1 << 24),             // found in firebeetles.xml (Add Attachment: FireBeetlesGunFire w\Condition1 = "Stat In Range") - note: assumed to be with bitmask as with rest
            Bitmask125 = (1 << 25),             // found in fiendpriesthellgatelaser.xml.cooked (Fire Laser: Fiend Priest Hellgate Heal) - seen with 104,106 i.e. is bitmask with rest
            IntValue126 = (1 << 26),            // found in chocolatefogdamagefield.xml.cooked (State - Set on Targets in Range: on_chocolate_fog; is it a bitmask with above??)
            Bitmask129 = (1 << 29),             // found in airmeleeattack.xml.cooked (Do Melee Item Events) - note: assumed to be with bitmask as with rest
            Bitmask130 = (1 << 30)              // found in meteor.xml.cooked (Fire Laser)
        }

        [FlagsAttribute]
        private enum BitField2 : uint
        {
            // note: assumed these are all the same bitfield (they appear to be)
            BitField200 = (1 << 0),             // found in cannonguns.xml.cooked (Fire Missile: Cannon Gun)
            BitField201 = (1 << 1),             // found in townportal.xml.cooked
            BitField202 = (1 << 2),
            BitField203 = (1 << 3),
            BitField204 = (1 << 4),             // found in summonreapoer.xml.cooked (Spawn Minion: reaper_pet)
            _Undefined205 = (1 << 5),
            BitField206 = (1 << 6),             // found in vampirepistolheal.xml.cooked (Heal Partially)
            BitField207 = (1 << 7),              // found in blink.xml.cooked (PlayerTeleportToSafety) and corpseheal.xml.cooked (State - Set on Targets in Range)
            BitField208 = (1 << 8),             // found in townportal.xml.cooked
            IntValue209 = (1 << 9),   
            FloatValue211 = (1 << 11),          // found in arc lasher.xml.cooked (Fire Laser: Arc Lasher) - note: assumed to *not* be with above bitfield values...
            FloatValue210 = (1 << 10),
            FloatValue212 = (1 << 12),
            FloatValue213 = (1 << 13),          // found in bloodlink.xml.cooked (GiveLifeToCompanion)
            FloatValue214 = (1 << 14),          // found in bloodlink.xml.cooked (GiveLifeToCompanion)
            FloatValue215 = (1 << 15),          // found in drainlife.xml.cooked (CabDrainLifeLoopGroup)
            IntValue216 = (1 << 16),
            IntValue217 = (1 << 17),
            PrimaryString218 = (1 << 18),
            _Undefined219 = (1 << 19),
            _Undefined220 = (1 << 20),
            SecondaryString221 = (1 << 21),
            _Undefined222 = (1 << 22),
            FloatValue223 = (1 << 23),          // found in paperdollevoker.xml.cooked (Fire Missile = -0.330912)
            FloatValue224 = (1 << 24),
            FloatValue225 = (1 << 25),
            FloatValue226 = (1 << 26),          // almost always 0 - first non-zero found in paperdollevoker.xml.cooked (Fire Missile = -0.00833071)
            FloatValue227 = (1 << 27),
            FloatValue228 = (1 << 28),          // found in explodingbarrelexplode.xml.cooked (Fire Missile Gibs)
            Conditions229 = (1 << 29),
            _Undefined230 = (1 << 30),
            _Undefined231 = ((uint)1 << 31),
        }

        private bool _ParseElements(byte[] data, ref int offset, XmlElement parentElement)
        {
            //Int64 elementMask = FileTools.ByteArrayTo<Int64>(data, ref offset);
            BitField1 bitField1 = (BitField1)BitConverter.ToUInt32(data, offset);
            offset += 4;
            BitField2 bitField2 = (BitField2)BitConverter.ToUInt32(data, offset);
            offset += 4;

            XmlElement dataElement = XmlDoc.CreateElement("data");
            parentElement.AppendChild(dataElement);


            if (_TestField1(bitField1, BitField1.TypeString100))
            {
                _DoByteString(data, ref offset, dataElement, "TypeString100");
            }
            // yes, this is gross, but it was the slow accumulation throughout testing
            // todo: once testing finished it'll probably be easier to mask it out with TypeString100 and do a single compare or something
            if (_TestField1(bitField1, BitField1.Bitmask101) ||
                _TestField1(bitField1, BitField1.Bitmask102) ||
                _TestField1(bitField1, BitField1.Bitmask103) ||
                _TestField1(bitField1, BitField1.Bitmask104) ||
                _TestField1(bitField1, BitField1.Bitmask105) ||
                _TestField1(bitField1, BitField1.Bitmask106) ||
                _TestField1(bitField1, BitField1.Bitmask107) ||
                _TestField1(bitField1, BitField1.Bitmask108) ||
                _TestField1(bitField1, BitField1.Bitmask109) ||
                _TestField1(bitField1, BitField1.Bitmask111) ||
                _TestField1(bitField1, BitField1.Bitmask112) ||
                _TestField1(bitField1, BitField1.Bitmask114) ||
                _TestField1(bitField1, BitField1.Bitmask115) ||
                _TestField1(bitField1, BitField1.Bitmask116) ||
                _TestField1(bitField1, BitField1.Bitmask117) ||
                _TestField1(bitField1, BitField1.Bitmask118) ||
                _TestField1(bitField1, BitField1.Bitmask119) ||
                _TestField1(bitField1, BitField1.Bitmask120) ||
                _TestField1(bitField1, BitField1.Bitmask123) ||
                _TestField1(bitField1, BitField1.Bitmask124) ||
                _TestField1(bitField1, BitField1.Bitmask125) ||
                _TestField1(bitField1, BitField1.Bitmask129) ||
                _TestField1(bitField1, BitField1.Bitmask130)
                )
            {
                _DoBitmask(data, ref offset, dataElement, "Bitfield1");
            }
            //if (_TestField1(bitField1, BitField1.IntValue104) || _TestField1(bitField1, BitField1.IntValue130))
            //{
                //_DoInt(data, ref offset, dataElement, "IntValue104");
            //}
            if (_TestField1(bitField1, BitField1.Bitmask107))
            {
                //_DoInt(data, ref offset, dataElement, "IntValue107");
            }
            if (_TestField1(bitField1, BitField1.Bitmask109))
            {
                //_DoInt(data, ref offset, dataElement, "IntValue109");
            }
            if (_TestField1(bitField1, BitField1.Bitmask112))
            {
                //_DoShort(data, ref offset, dataElement, "ShortValue112");
               // dataElement.SetAttribute("Unknown112", "true");
            }
            if (_TestField1(bitField1, BitField1.Bitmask114))
            {
                //_DoInt(data, ref offset, dataElement, "IntValue114");
            }
            if (_TestField1(bitField1, BitField1.Bitmask116))
            {
               // _DoInt(data, ref offset, dataElement, "IntValue116");
            }
            if (_TestField1(bitField1, BitField1.IntValue121))
            {
                _DoInt(data, ref offset, dataElement, "IntValue121");
            }
          //  if (_TestField1(bitField1, BitField1.IntValue125))  // is this with the bitfield above??
          //  {
                //_DoInt(data, ref offset, dataElement, "IntValue125");
          //  }
            if (_TestField1(bitField1, BitField1.IntValue126))  // is this with the bitfield above??
            {
                _DoInt(data, ref offset, dataElement, "IntValue126");
            }
            //if (_TestField1(bitField1, BitField1.IntValue130))
          // {
                //_DoInt(data, ref offset, dataElement, "IntValue130");
            //}


            if (_TestField2(bitField2, BitField2._Undefined205) ||
                _TestField2(bitField2, BitField2._Undefined219) ||
                _TestField2(bitField2, BitField2._Undefined220) ||
                _TestField2(bitField2, BitField2._Undefined222) ||
                _TestField2(bitField2, BitField2._Undefined230) ||
                _TestField2(bitField2, BitField2._Undefined231))
            {
                _DoInt(data, ref offset, dataElement, "IntValue202");
            }

            if (_TestField2(bitField2, BitField2.BitField200) ||
                _TestField2(bitField2, BitField2.BitField201) ||
                _TestField2(bitField2, BitField2.BitField202) ||
                _TestField2(bitField2, BitField2.BitField203) ||
                _TestField2(bitField2, BitField2.BitField204) ||
                _TestField2(bitField2, BitField2.BitField206) ||
                _TestField2(bitField2, BitField2.BitField207) ||
                _TestField2(bitField2, BitField2.BitField208))
            {
                _DoInt(data, ref offset, dataElement, "Bitfield2");
            }
            if (_TestField2(bitField2, BitField2.BitField202))      // is this with the above bitmask values?
            {
                //_DoInt(data, ref offset, dataElement, "IntValue202");
            }
            if (_TestField2(bitField2, BitField2.BitField204))      // is this with the above bitmask values?
            {
                //_DoInt(data, ref offset, dataElement, "IntValue204");
            }

            if (_TestField2(bitField2, BitField2.IntValue209))
            {
                _DoInt(data, ref offset, dataElement, "IntValue209");
            }
            if (_TestField2(bitField2, BitField2.FloatValue210))
            {
                _DoFloat(data, ref offset, dataElement, "FloatValue210");
            }
            if (_TestField2(bitField2, BitField2.FloatValue211))
            {
                _DoFloat(data, ref offset, dataElement, "FloatValue211");
            }
            if (_TestField2(bitField2, BitField2.FloatValue212))
            {
                _DoFloat(data, ref offset, dataElement, "FloatValue212");
            }
            if (_TestField2(bitField2, BitField2.FloatValue213))
            {
                _DoFloat(data, ref offset, dataElement, "FloatValue213");
            }
            if (_TestField2(bitField2, BitField2.FloatValue214))
            {
                _DoFloat(data, ref offset, dataElement, "FloatValue214");
            }
            if (_TestField2(bitField2, BitField2.FloatValue215))
            {
                _DoInt(data, ref offset, dataElement, "FloatValue215");
            }
            if (_TestField2(bitField2, BitField2.IntValue216))
            {
                _DoInt(data, ref offset, dataElement, "IntValue216");
            }
            if (_TestField2(bitField2, BitField2.IntValue217))
            {
                _DoInt(data, ref offset, dataElement, "IntValue217");
            }

            if (_TestField2(bitField2, BitField2.PrimaryString218))
            {
                _DoZeroString(data, ref offset, dataElement, "PrimaryString218");
            }

            if (_TestField2(bitField2, BitField2.SecondaryString221))
            {
                _DoZeroString(data, ref offset, dataElement, "SecondaryString221");
            }

            if (_TestField2(bitField2, BitField2.FloatValue223))
            {
                _DoFloat(data, ref offset, dataElement, "FloatValue223");
            }
            if (_TestField2(bitField2, BitField2.FloatValue224))
            {
                _DoFloat(data, ref offset, dataElement, "FloatValue224");
            }
            if (_TestField2(bitField2, BitField2.FloatValue225))
            {
                _DoFloat(data, ref offset, dataElement, "FloatValue225");
            }
            if (_TestField2(bitField2, BitField2.FloatValue226))
            {
                _DoFloat(data, ref offset, dataElement, "FloatValue226");
            }
            if (_TestField2(bitField2, BitField2.FloatValue227))
            {
                _DoFloat(data, ref offset, dataElement, "FloatValue227");
            }
            if (_TestField2(bitField2, BitField2.FloatValue228))
            {
                _DoFloat(data, ref offset, dataElement, "FloatValue228");
            }
            if (_TestField2(bitField2, BitField2.Conditions229))
            {
                XmlElement footerElement = XmlDoc.CreateElement("Conditions229");
                dataElement.AppendChild(footerElement);

                byte footerToken = FileTools.ByteArrayTo<byte>(data, ref offset);
                if (footerToken != 0x7F && footerToken != 0xFF)
                {
                    Debug.Assert(false, "unexpected footerToken (!= 0x7F, != 0xFF) == 0x" + footerToken.ToString("X2"));
                }

                byte unknownToken0 = FileTools.ByteArrayTo<byte>(data, ref offset);
                /*if (footerToken == 0xFF && unknownToken0 != 0x00)
                { 
                    Debug.Assert(false, "if (footerToken == 0xFF && unknownToken0 != 0x00)");
                }*/

                /* conditions 1, 3, 5 were seen semi-reguarly within the consumable skills, 2, 4, 7 not at all
                 * the reset are as mentioned below (1, 3, 5 were still seen here and there with them though)
                 */

                _DoByteString(data, ref offset, footerElement, "Condition1");

                // first seen as "sfx_electrical" in drainlife.xml.cooked
                // seen in fiendpriesthellgatelaser.xml.cooked: Condition1 = "Does Not Have State", Condition2State = "hellball_hidden"   i.e. is a state check?
                _DoByteString(data, ref offset, footerElement, "Condition2State");

                _DoByteString(data, ref offset, footerElement, "Condition3");

                // first seen as "Spectral_Nova" in spectralzap.xml.cooked
                // also in fiendpriestheal.xml.cooked (w\Condition1 = "Has Skill Level", Condition2SFX = "increase_healing_source", Condition4DoSkill = "Increase_Healing")
                _DoByteString(data, ref offset, footerElement, "Condition4DoSkill");        // I think this is the "if - *do this*" part

                _DoByteString(data, ref offset, footerElement, "Condition5");

                // first seen as "zombiefood" in enragecompanion.xml.cooked
                _DoByteString(data, ref offset, footerElement, "Condition6State");

                // first seen as "level" in summonblaster.xml.cooked
                _DoByteString(data, ref offset, footerElement, "Condition7");

                /*
                 * Is this a bit mask???
                 * 0x007F              1111111      7
                 * 0x027F           1001111111      8
                 * 0x067F          11001111111      9
                 * 0x107F        1000001111111      8
                 * 0x167F        1011001111111      10
                 * 0x447F      100010001111111      9
                 * 
                 * 0x00FF             11111111      8
                 * 0x06FF          11011111111      10
                 */

                // todo: figure out this section properly
                if (footerToken == 0xFF)       // if 0xFF, seems to have 1.0f             // seen in spectralzap.xml.cooked (Skill - Start When Target Killed: Spectral_Nova) & seen in beacon.xml.cooked (Skill - Do: Beacon_Mastery)
                {
                    _DoFloat(data, ref offset, footerElement, "footerToken_floatValue0x00FF");
                }
                else if (footerToken == 0x7F && unknownToken0 == 0x01)
                {
                    _DoFloat(data, ref offset, footerElement, "footerToken_floatValue0x01FF");
                }

                // 0x02 + 0x04 = 0x06      --->       0x01 + 0x04 = 0x5             // i.e. it's a bitfield     // todo: fix me
                if (unknownToken0 == 0x02 ||        // if 0x02, seems to be 0x00000001
                    unknownToken0 == 0x04 ||        //    0x04, seems to be 0x00000004          // seen in turretrepair.xml.cooked (Run Script on Targets in Range w/Condition1="Is Alive"??)
                    unknownToken0 == 0x06 ||        //    0x06, seems to be 0x00000005          // seen in scatter.xml.cooked (Add Attachment: ScatterCabalist)
                    unknownToken0 == 0x12 ||        //    0x12, seems to be 0x00000009          // seen in fiendpriestheal.xml.cooked (Fire Laser: Fiend Priest Heal)
                    unknownToken0 == 0x14 ||        //    0x14, seems to be 0x0000000C          // seen in vampirecurseheal.xml.cooked (State - Set: vampire_curse_healing w/Condition1="Is Alive" & Run Script on Self  w/Condition1="Is Alive")
                    unknownToken0 == 0x16 ||        //    0x16, seems to be 0x0000000D
                    unknownToken0 == 0x20 ||        //    0x16, seems to be 0x00000010          // seen in playeronhithealthhalf.xml.cooked (Add Attachment: StateLowHeal)
                    unknownToken0 == 0x36 ||        //    0x16, seems to be 0x0000001D          // seen in jumpslam.xml.cooked (Add Attachment: JumpAttackStartup)
                    unknownToken0 == 0x44 ||        //    0x44, seems to be 0x00000024
                    unknownToken0 == 0x10)          //    0x10, seems to be 0x00000008          // seen in meteor.xml.cooked (Add Attachment: SkillMeteorMiddle)
                {
                    XmlElement element = _DoInt(data, ref offset, footerElement, "unknownToken0_intValue");
                    element.SetAttribute("unknownToken0", unknownToken0.ToString());
                }

                else if (unknownToken0 != 0x00 && unknownToken0 != 0x01)
                {
                    Debug.Assert(false, "unexpected footer unknownToken0 == 0x" + unknownToken0.ToString("X2"));
                }
            }

            return true;
        }

        private static bool _TestField1(BitField1 testField, BitField1 testAgainst)
        {
            return ((testField & testAgainst) > 0);
        }

        private static bool _TestField2(BitField2 testField, BitField2 testAgainst)
        {
            return ((testField & testAgainst) > 0);
        }

        private String _DoByteString(byte[] data, ref int offset, XmlElement parentElement, String elementName)
        {
            byte strLen = FileTools.ByteArrayTo<byte>(data, ref offset);
            if (strLen == 0xFF) return null;

            String str = String.Empty;
            if (strLen != 0x00)
            {
                str = FileTools.ByteArrayToStringAnsi(data, ref offset, strLen);
            }

            XmlElement element = XmlDoc.CreateElement(elementName);
            element.InnerText = str;
            parentElement.AppendChild(element);

            return str;
        }

        private void _DoZeroString(byte[] data, ref int offset, XmlElement parentElement, String elementName)
        {
            Int32 strLen = FileTools.ByteArrayTo<Int32>(data, ref offset);  // strLen includes \0
            String str = FileTools.ByteArrayToStringAnsi(data, offset);
            offset += strLen;

            XmlElement element = XmlDoc.CreateElement(elementName);
            element.InnerText = str;
            parentElement.AppendChild(element);
        }

        private void _DoShort(byte[] data, ref int offset, XmlElement parentElement, String elementName)
        {
            short value = FileTools.ByteArrayTo<short>(data, ref offset);

            XmlElement element = XmlDoc.CreateElement(elementName);
            element.InnerText = value.ToString();
            parentElement.AppendChild(element);
        }

        private XmlElement _DoInt(byte[] data, ref int offset, XmlElement parentElement, String elementName)
        {
            Int32 int32 = FileTools.ByteArrayTo<Int32>(data, ref offset);

            XmlElement element = XmlDoc.CreateElement(elementName);
            element.InnerText = int32.ToString();
            parentElement.AppendChild(element);

            return element;
        }

        private void _DoFloat(byte[] data, ref int offset, XmlElement parentElement, String elementName)
        {
            float f = FileTools.ByteArrayTo<float>(data, ref offset);

            XmlElement element = XmlDoc.CreateElement(elementName);
            element.InnerText = f.ToString("F5");
            parentElement.AppendChild(element);
        }

        private void _DoBitmask(byte[] data, ref int offset, XmlElement parentElement, String elementName)
        {
            UInt32 uint32 = FileTools.ByteArrayTo<UInt32>(data, ref offset);

            XmlElement element = XmlDoc.CreateElement(elementName);
            element.InnerText = Convert.ToString(uint32, 2).PadLeft(32, '0');
            parentElement.AppendChild(element);
        }
    }
}
