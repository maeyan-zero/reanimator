using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace Reanimator
{
    class XmlCookedSkill : XmlCookedBase
    {
        public const String RootFolder = "skills";
        private const Byte SkillDataTokenBasic = 0x04;
        private const Byte SkillDataTokenString = 0x05;

        private const String StringDataHeader = "DataHeader";

        private String _dataString;
        private Int32 _branchCount;

        private readonly List<XmlBranchSkills> _xmlBranches;

        public XmlCookedSkill()
        {
            _xmlBranches = new List<XmlBranchSkills>();
        }

        class XmlBranchSkills
        {
            public XmlNode XmlNode;

            public String Name;

            public float Delay;
            public bool HasDelay;

            public readonly List<XmlDataSkills> DataElements;

            public XmlBranchSkills()
            {
                DataElements = new List<XmlDataSkills>();
            }
        }

        class XmlDataSkills
        {
            public const Byte DataTokenStandard = 0x05;
            public const Byte DataTokenDelay = 0x07;

            public const String StringType = "szType";
            public const String StringBitField1 = "BitField1";
            public const String StringBitField2 = "BitField2";
            public const String StringFloat210 = "fValue220";
            public const String StringFloat211 = "fValue221";
            public const String StringFloat212 = "fValue222";
            public const String StringFloat213 = "fValue223";
            public const String StringFloat214 = "fValue224";
            public const String StringFloat215 = "fValue225";
            public const String StringInt216 = "iValue216";
            public const String StringInt217 = "iValue217";
            public const String StringData = "szData";
            public const String StringBone = "szBone";
            public const String StringFloat223 = "fValue223";
            public const String StringFloat224 = "fValue224";
            public const String StringFloat225 = "fValue225";
            public const String StringFloat226 = "fValue226";
            public const String StringFloat227 = "fValue227";
            public const String StringFloat228 = "fValue228";
            public const String StringConditions = "Conditions";

            /*
            public class BitField01
            {
                public const String StringBit101 = "Bit101";
                public const String StringBit102 = "Bit102";
                public const String StringBit103 = "Bit103";
            }
            */

            public class DataConditionsElement
            {
                public const String StringCondition1 = "Condition1";
                public const String StringCondition2State = "Condition2State";
                public const String StringCondition3 = "Condition3";
                public const String StringCondition4DoSkill = "Condition4DoSkill";
                public const String StringCondition5 = "Condition5";
                public const String StringCondition6State = "Condition6State";
                public const String StringCondition7 = "Condition7";
                public const String StringFloat0 = "fValue0";
                public const String StringFloat1 = "fValue1";
                public const String StringBitField = "BitField";
                public const String StringUnknownTokenAttribute = "unknownToken0";

                public XmlNode XmlNode;

                public byte Token1;
                public byte Token2;
                public string Condition1;
                public string Coniditon2State;
                public string Condition3;
                public string Condition4DoSkill;
                public string Condition5;
                public string Condition6State;
                public string Condition7;
                public float Float0;
                public float Float1;
                public uint BitField;
            }

            public XmlNode XmlNode;
            public SkillsBitField1 ContentsBitField1;       // these two shouldn't be read from
            public SkillsBitField2 ContentsBitField2;       // they're just for debugging

            public string TypeString;
            public uint BitField1;
            public uint BitField2;
            public float Float210;
            public float Float211;
            public float Float212;
            public float Float213;
            public float Float214;
            public float Float215;
            public int Int216;
            public int Int217;
            public string DataString;
            public string BoneString;
            public float Float223;
            public float Float224;
            public float Float225;
            public float Float226;
            public float Float227;
            public float Float228;
            public DataConditionsElement Conditions;
        }

        [FlagsAttribute]
        private enum SkillsBitField1 : uint
        {
            TypeString100 = (1 << 0),
            // pretty sure entire thing is a bitfield
            BitField101 = (1 << 1),              // found in hellgatedeath.xml.cooked (Fire Lasers to All) with BitField115 as well - note: so far, is *assumed* with same bitmask as rest
            BitField102 = (1 << 2),              // found in orbilezapcorpse.xml.cooked (State - Remove Target and Set State: orbile_angry) - note: assumed to be with bitmask as with rest
            BitField103 = (1 << 3),
            BitField104 = (1 << 4),              // found in spectralzap.xml.cooked (Fire Laser) - 104,5,6 all found set in furylightningzap.xml.cooked (Fire Laser)
            BitField105 = (1 << 5),
            BitField106 = (1 << 6),
            BitField107 = (1 << 7),
            BitField108 = (1 << 8),              // found in fragblaster.xml.cooked (Start Mode Weapon: item_equipped_idle) - note: assumed to be with bitmask as with rest
            BitField109 = (1 << 9),              // found in fellhordediggerball.xml.cooked (Fire Missile: Fell Horde Digger Ball)
            BitField110 = (1 << 10),             // found in furynova.xml.cooked (Fire Missile Nova: fury spectral nova)
            BitField111 = (1 << 11),             // found in minionmeleeattack2.xml.cooked (Add Attachment: weapons\HolyFire\Grenade impact.xml) - note: assumed to be with bitmask as with rest
            BitField112 = (1 << 12),
            Unknown113 = (1 << 13),         // not seen
            BitField114 = (1 << 14),             // found in explodingbarrelexplode.xml.cooked (Fire Missile Gibs)
            BitField115 = (1 << 15),
            BitField116 = (1 << 16),
            BitField117 = (1 << 17),             // found in nanobotsdeath.xml.cooked (State - Clear: nanobots_attack) - note: assumed to be with bitmask as with rest
            BitField118 = (1 << 18),             // found in masterfiendmeleespin.xml.cooked - note: assumed to be with bitmask as with rest
            BitField119 = (1 << 19),             // found in blink.xml.cooked, same with cansexplode.xml.cooked            doesn't appear to do anything in file though...
            BitField120 = (1 << 20),
            BitField121 = (1 << 21),             // found in physicalmirvmissile.xml.cooked (Fire Missile Nova)
            BitField122 = (1 << 22),             // found in toxicarmorattack.xml.cooked (Fire Missile: toxic armor)
            BitField123 = (1 << 23),             // found in throwsword.xml.cooked (State - Set: dont_draw_quick) - note: assumed to be with bitmask as with rest
            BitField124 = (1 << 24),             // found in firebeetles.xml (Add Attachment: FireBeetlesGunFire w\Condition1 = "Stat In Range") - note: assumed to be with bitmask as with rest
            BitField125 = (1 << 25),             // found in fiendpriesthellgatelaser.xml.cooked (Fire Laser: Fiend Priest Hellgate Heal) - seen with 104,106 i.e. is bitmask with rest
            BitField126 = (1 << 26),             // found in chocolatefogdamagefield.xml.cooked (State - Set on Targets in Range: on_chocolate_fog; is it a bitmask with above??)
            Unknown127 = (1 << 27),         // not seen
            Unknown128 = (1 << 28),             // found in playermelee.xml.cooked (Skill - Do: PlayerMelee_BreakDestructibles)
            BitField129 = (1 << 29),            // found in airmeleeattack.xml.cooked (Do Melee Item Events) - note: assumed to be with bitmask as with rest
            BitField130 = (1 << 30),            // found in meteor.xml.cooked (Fire Laser)
            Unknown131 = ((uint)1 << 31)    // not seen
        }

        [FlagsAttribute]
        private enum SkillsBitField2 : uint
        {
            // note: assumed these are all the same bitfield (they appear to be)
            BitField200 = (1 << 0),             // found in cannonguns.xml.cooked (Fire Missile: Cannon Gun)
            BitField201 = (1 << 1),             // found in townportal.xml.cooked
            BitField202 = (1 << 2),
            BitField203 = (1 << 3),
            BitField204 = (1 << 4),             // found in summonreapoer.xml.cooked (Spawn Minion: reaper_pet)
            Unknown205 = (1 << 5),
            BitField206 = (1 << 6),             // found in vampirepistolheal.xml.cooked (Heal Partially)
            BitField207 = (1 << 7),              // found in blink.xml.cooked (PlayerTeleportToSafety) and corpseheal.xml.cooked (State - Set on Targets in Range)
            BitField208 = (1 << 8),             // found in townportal.xml.cooked
            BitField209 = (1 << 9),
            FloatValue211 = (1 << 11),          // found in arc lasher.xml.cooked (Fire Laser: Arc Lasher) - note: assumed to be with bitmask as with rest
            FloatValue210 = (1 << 10),
            FloatValue212 = (1 << 12),
            FloatValue213 = (1 << 13),          // found in bloodlink.xml.cooked (GiveLifeToCompanion)
            FloatValue214 = (1 << 14),          // found in bloodlink.xml.cooked (GiveLifeToCompanion)
            FloatValue215 = (1 << 15),          // found in drainlife.xml.cooked (CabDrainLifeLoopGroup)
            IntValue216 = (1 << 16),
            IntValue217 = (1 << 17),
            DataString218 = (1 << 18),
            Unknown219 = (1 << 19),
            Unknown220 = (1 << 20),
            BoneString221 = (1 << 21),
            Unknown222 = (1 << 22),
            FloatValue223 = (1 << 23),          // found in paperdollevoker.xml.cooked (Fire Missile = -0.330912)
            FloatValue224 = (1 << 24),
            FloatValue225 = (1 << 25),
            FloatValue226 = (1 << 26),          // almost always 0 - first non-zero found in paperdollevoker.xml.cooked (Fire Missile = -0.00833071)
            FloatValue227 = (1 << 27),          // always 1.0?
            FloatValue228 = (1 << 28),          // found in explodingbarrelexplode.xml.cooked (Fire Missile Gibs)
            Conditions229 = (1 << 29),
            Unknown230 = (1 << 30),
            Unknown231 = ((uint)1 << 31),
        }

        protected override bool CookDataSegment(byte[] buffer)
        {
            // first token, and if applicable, data string
            if (_dataString == null)
            {
                FileTools.WriteToBuffer(ref buffer, ref WriteOffset, SkillDataTokenBasic);
            }
            else
            {
                FileTools.WriteToBuffer(ref buffer, ref WriteOffset, SkillDataTokenString);
                FileTools.WriteToBuffer(ref buffer, ref WriteOffset, (UInt32)_dataString.Length);
                FileTools.WriteToBuffer(ref buffer, ref WriteOffset, _dataString);
                WriteOffset++; // for \0
            }

            // branch count
            FileTools.WriteToBuffer(ref buffer, ref WriteOffset, _branchCount);
            return _xmlBranches.All(xmlBranchSkills => _CookBranch(ref buffer, xmlBranchSkills));
        }

        private bool _CookBranch(ref byte[] buffer, XmlBranchSkills xmlBranchSkills)
        {
            FileTools.WriteToBuffer(ref buffer, ref WriteOffset,
                    xmlBranchSkills.HasDelay ? XmlDataSkills.DataTokenDelay : XmlDataSkills.DataTokenStandard);

            if (xmlBranchSkills.Name != null)
            {
                WriteByteString(ref buffer, xmlBranchSkills.Name, false);
            }

            if (xmlBranchSkills.HasDelay)
            {
                FileTools.WriteToBuffer(ref buffer, ref WriteOffset, xmlBranchSkills.Delay);
            }

            Int32 dataElementCount = xmlBranchSkills.DataElements.Count;
            FileTools.WriteToBuffer(ref buffer, ref WriteOffset, dataElementCount);

            foreach (XmlDataSkills xmlDataSkills in xmlBranchSkills.DataElements)
            {
                if (!_CookDataElement(ref buffer, xmlDataSkills)) return false;
            }

            return true;
        }

        private bool _CookDataElement(ref byte[] buffer, XmlDataSkills xmlDataSkills)
        {
            Int32 bitField1Offset = WriteOffset;
            SkillsBitField1 bitField1 = 0;
            FileTools.WriteToBuffer(ref buffer, ref WriteOffset, (uint)bitField1);

            Int32 bitField2Offset = WriteOffset;
            SkillsBitField2 bitField2 = 0;
            FileTools.WriteToBuffer(ref buffer, ref WriteOffset, (uint)bitField2);

            // type string
            if (HasChildNode(xmlDataSkills.XmlNode, XmlDataSkills.StringType))
            {
                bitField1 |= SkillsBitField1.TypeString100;
                WriteByteString(ref buffer, xmlDataSkills.TypeString, true);
            }

            // bitfield 1       // todo: FIXME
            if (HasChildNode(xmlDataSkills.XmlNode, XmlDataSkills.StringBitField1))
            {
                //bitField1 |= SkillsBitField1.BitField101;
                FileTools.WriteToBuffer(ref buffer, ref WriteOffset, xmlDataSkills.BitField1);
            }

            // bitfield 2       // todo: FIXME
            if (HasChildNode(xmlDataSkills.XmlNode, XmlDataSkills.StringBitField2))
            {
                //bitField2 |= SkillsBitField2.BitField201;
                FileTools.WriteToBuffer(ref buffer, ref WriteOffset, xmlDataSkills.BitField2);
            }

            if (HasChildNode(xmlDataSkills.XmlNode, XmlDataSkills.StringFloat210))
            {
                bitField2 |= SkillsBitField2.FloatValue210;
                FileTools.WriteToBuffer(ref buffer, ref WriteOffset, xmlDataSkills.Float210);
            }
            if (HasChildNode(xmlDataSkills.XmlNode, XmlDataSkills.StringFloat211))
            {
                bitField2 |= SkillsBitField2.FloatValue211;
                FileTools.WriteToBuffer(ref buffer, ref WriteOffset, xmlDataSkills.Float211);
            }
            if (HasChildNode(xmlDataSkills.XmlNode, XmlDataSkills.StringFloat212))
            {
                bitField2 |= SkillsBitField2.FloatValue212;
                FileTools.WriteToBuffer(ref buffer, ref WriteOffset, xmlDataSkills.Float212);
            }
            if (HasChildNode(xmlDataSkills.XmlNode, XmlDataSkills.StringFloat213))
            {
                bitField2 |= SkillsBitField2.FloatValue213;
                FileTools.WriteToBuffer(ref buffer, ref WriteOffset, xmlDataSkills.Float213);
            }
            if (HasChildNode(xmlDataSkills.XmlNode, XmlDataSkills.StringFloat214))
            {
                bitField2 |= SkillsBitField2.FloatValue214;
                FileTools.WriteToBuffer(ref buffer, ref WriteOffset, xmlDataSkills.Float214);
            }
            if (HasChildNode(xmlDataSkills.XmlNode, XmlDataSkills.StringFloat215))
            {
                bitField2 |= SkillsBitField2.FloatValue215;
                FileTools.WriteToBuffer(ref buffer, ref WriteOffset, xmlDataSkills.Float215);
            }

            if (HasChildNode(xmlDataSkills.XmlNode, XmlDataSkills.StringInt216))
            {
                bitField2 |= SkillsBitField2.IntValue216;
                FileTools.WriteToBuffer(ref buffer, ref WriteOffset, xmlDataSkills.Int216);
            }
            if (HasChildNode(xmlDataSkills.XmlNode, XmlDataSkills.StringInt217))
            {
                bitField2 |= SkillsBitField2.IntValue217;
                FileTools.WriteToBuffer(ref buffer, ref WriteOffset, xmlDataSkills.Int217);
            }

            if (HasChildNode(xmlDataSkills.XmlNode, XmlDataSkills.StringData))
            {
                bitField2 |= SkillsBitField2.DataString218;
                WriteZeroString(ref buffer, xmlDataSkills.DataString);
            }
            if (HasChildNode(xmlDataSkills.XmlNode, XmlDataSkills.StringBone))
            {
                bitField2 |= SkillsBitField2.BoneString221;
                WriteZeroString(ref buffer, xmlDataSkills.BoneString);
            }

            if (HasChildNode(xmlDataSkills.XmlNode, XmlDataSkills.StringFloat223))
            {
                bitField2 |= SkillsBitField2.FloatValue223;
                FileTools.WriteToBuffer(ref buffer, ref WriteOffset, xmlDataSkills.Float223);
            }
            if (HasChildNode(xmlDataSkills.XmlNode, XmlDataSkills.StringFloat224))
            {
                bitField2 |= SkillsBitField2.FloatValue224;
                FileTools.WriteToBuffer(ref buffer, ref WriteOffset, xmlDataSkills.Float224);
            }
            if (HasChildNode(xmlDataSkills.XmlNode, XmlDataSkills.StringFloat225))
            {
                bitField2 |= SkillsBitField2.FloatValue225;
                FileTools.WriteToBuffer(ref buffer, ref WriteOffset, xmlDataSkills.Float225);
            }
            if (HasChildNode(xmlDataSkills.XmlNode, XmlDataSkills.StringFloat226))
            {
                bitField2 |= SkillsBitField2.FloatValue226;
                FileTools.WriteToBuffer(ref buffer, ref WriteOffset, xmlDataSkills.Float226);
            }
            if (HasChildNode(xmlDataSkills.XmlNode, XmlDataSkills.StringFloat227))
            {
                bitField2 |= SkillsBitField2.FloatValue227;
                FileTools.WriteToBuffer(ref buffer, ref WriteOffset, xmlDataSkills.Float227);
            }
            if (HasChildNode(xmlDataSkills.XmlNode, XmlDataSkills.StringFloat228))
            {
                bitField2 |= SkillsBitField2.FloatValue228;
                FileTools.WriteToBuffer(ref buffer, ref WriteOffset, xmlDataSkills.Float228);
            }

            if (HasChildNode(xmlDataSkills.XmlNode, XmlDataSkills.StringConditions))
            {
                bitField2 |= SkillsBitField2.Conditions229;

                Byte footerToken = xmlDataSkills.Conditions.Token1;
                Byte unknownToken = xmlDataSkills.Conditions.Token2;

                XmlNode xmlNode = xmlDataSkills.Conditions.XmlNode;
                // this chunk is completely guess work - I think the two tokens are a bitfield
                // todo: fix/test me
                if (HasChildNode(xmlNode, XmlDataSkills.DataConditionsElement.StringFloat0))
                {
                    footerToken = 0xFF;
                }
                else if (HasChildNode(xmlNode, XmlDataSkills.DataConditionsElement.StringFloat1))
                {
                    unknownToken = 0x01;
                }

                FileTools.WriteToBuffer(ref buffer, ref WriteOffset, footerToken);
                FileTools.WriteToBuffer(ref buffer, ref WriteOffset, unknownToken);

                // want 0xFF even if no string - never seen it without them
                WriteByteString(ref buffer, xmlDataSkills.Conditions.Condition1, true, true);
                WriteByteString(ref buffer, xmlDataSkills.Conditions.Coniditon2State, true, true);
                WriteByteString(ref buffer, xmlDataSkills.Conditions.Condition3, true, true);
                WriteByteString(ref buffer, xmlDataSkills.Conditions.Condition4DoSkill, true, true);
                WriteByteString(ref buffer, xmlDataSkills.Conditions.Condition5, true, true);
                WriteByteString(ref buffer, xmlDataSkills.Conditions.Condition6State, true, true);
                WriteByteString(ref buffer, xmlDataSkills.Conditions.Condition7, true, true);

                if (HasChildNode(xmlNode, XmlDataSkills.DataConditionsElement.StringFloat0))
                {
                    FileTools.WriteToBuffer(ref buffer, ref WriteOffset, xmlDataSkills.Conditions.Float0);
                }
                if (HasChildNode(xmlNode, XmlDataSkills.DataConditionsElement.StringFloat1))
                {
                    FileTools.WriteToBuffer(ref buffer, ref WriteOffset, xmlDataSkills.Conditions.Float1);
                }
                else if (HasChildNode(xmlNode, XmlDataSkills.DataConditionsElement.StringBitField))
                {
                    FileTools.WriteToBuffer(ref buffer, ref WriteOffset, xmlDataSkills.Conditions.BitField);
                }
            }


            // write completed bitfields
            FileTools.WriteToBuffer(ref buffer, bitField1Offset, (uint)bitField1);
            FileTools.WriteToBuffer(ref buffer, bitField2Offset, (uint)bitField2);

            return true;
        }

        protected override bool ParseDataSegment(XmlElement dataElement)
        {
            byte dataToken = FileTools.ByteArrayTo<byte>(Data, ref ReadOffset);

            if (dataToken != 0x04 && dataToken != 0x05)
            {
                MessageBox.Show("Unexpected dataToken value!\n\ndataToken = 0x" + dataToken.ToString("X2"));
                return false;
            }

            /* Seen as 0x04 and 0x05
             * If 0x05, then has a _DoZeroString() part first, then like 0x04 as per normal.
             */
            if (dataToken == 0x05)
            {
                _dataString = ReadZeroString(dataElement, StringDataHeader);
            }

            _branchCount = FileTools.ByteArrayTo<Int32>(Data, ref ReadOffset);
            for (int i = 0; i < _branchCount; i++)
            {
                if (!_ParseBranch(Data, ref ReadOffset, dataElement)) return false;
            }

            return true;
        }

        private bool _ParseBranch(byte[] data, ref int offset, XmlNode parentElement)
        {
            XmlBranchSkills xmlBranchSkills = new XmlBranchSkills();
            _xmlBranches.Add(xmlBranchSkills);

            byte branchToken = FileTools.ByteArrayTo<byte>(data, ref offset);

            byte strLen = FileTools.ByteArrayTo<byte>(data, ref offset);
            Debug.Assert(strLen != 0x00);
            xmlBranchSkills.Name = strLen == 0xFF ? "NONAME" : FileTools.ByteArrayToStringAnsi(data, ref offset, strLen);

            XmlElement xmlBranch = XmlDoc.CreateElement(xmlBranchSkills.Name);
            parentElement.AppendChild(xmlBranch);
            xmlBranchSkills.XmlNode = xmlBranch;

            /* Seen as 0x05 and 0x07
             * If 0x07, then has a float part with it (seen usually with loop-type sections - I'm assuming a delay time between loops), then like 0x05 as per normal.
             */
            if (branchToken == 0x07)
            {
                float f = FileTools.ByteArrayTo<float>(data, ref offset);
                xmlBranch.SetAttribute("fDelay", f.ToString("F5"));
                xmlBranchSkills.Delay = f;
                xmlBranchSkills.HasDelay = true;
            }

            int elementCount = FileTools.ByteArrayTo<Int32>(data, ref offset);
            for (int i = 0; i < elementCount; i++)
            {
                XmlDataSkills xmlDataSkills = new XmlDataSkills();
                xmlBranchSkills.DataElements.Add(xmlDataSkills);

                if (!_ParseElements(data, ref offset, xmlBranch, xmlDataSkills)) return false;
            }

            return true;
        }

        private bool _ParseElements(byte[] data, ref int offset, XmlElement parentElement, XmlDataSkills xmlDataSkills)
        {
            XmlElement dataElement = XmlDoc.CreateElement("data");
            parentElement.AppendChild(dataElement);
            xmlDataSkills.XmlNode = dataElement;


            SkillsBitField1 bitField1 = (SkillsBitField1)BitConverter.ToUInt32(data, offset);
            offset += 4;
            SkillsBitField2 bitField2 = (SkillsBitField2)BitConverter.ToUInt32(data, offset);
            offset += 4;


            // ================BitField1================
            if (_TestField1(bitField1, SkillsBitField1.TypeString100))
            {
                xmlDataSkills.TypeString = ReadByteString(dataElement, XmlDataSkills.StringType, true);
            }

            // all unknowns
            const SkillsBitField1 skillsBitField1Unknowns = (SkillsBitField1)0x88002000;
            if (((uint)skillsBitField1Unknowns & (uint)bitField1) > 0)
            {
                MessageBox.Show("Unknowns detected as true for skillsBitfield1:\n " + bitField1, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // all but unknowns at type string
            SkillsBitField1 skillsBitField1Only =
                (SkillsBitField1)((0xFFFFFFFF ^ ((uint)skillsBitField1Unknowns | 0x01)) & (uint)bitField1);
            if (skillsBitField1Only > 0)
            {
                xmlDataSkills.BitField1 = ReadBitField(dataElement, XmlDataSkills.StringBitField1);
            }

            if ((bitField1 & SkillsBitField1.BitField112) > 0)
            {
                if (Blah == null) Blah = String.Empty;


                Blah += xmlDataSkills.TypeString + ": " + bitField1 + "\n";
                Blah += Convert.ToString(xmlDataSkills.BitField1, 2).PadLeft(32, '0') +"\n";
            }


            // ================BitField2================
            // all unknowns
            const SkillsBitField2 skillsBitField2Unknowns = (SkillsBitField2)0xC0580020;
            if (((uint)skillsBitField2Unknowns & (uint)bitField2) > 0)
            {
                MessageBox.Show("Unknowns detected as true for skillsBitfield2:\n " + bitField2, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // all but unknowns at type string
            SkillsBitField2 skillsBitField2Only =
                (SkillsBitField2)((0x03DF ^ ((uint)skillsBitField2Unknowns)) & (uint)bitField2);
            if (skillsBitField2Only > 0)
            {
                xmlDataSkills.BitField2 = ReadBitField(dataElement, XmlDataSkills.StringBitField2);
            }


            if (_TestField2(bitField2, SkillsBitField2.FloatValue210))
            {
                xmlDataSkills.Float210 = ReadFloat(dataElement, XmlDataSkills.StringFloat210);
            }
            if (_TestField2(bitField2, SkillsBitField2.FloatValue211))
            {
                xmlDataSkills.Float211 = ReadFloat(dataElement, XmlDataSkills.StringFloat211);
            }
            if (_TestField2(bitField2, SkillsBitField2.FloatValue212))
            {
                xmlDataSkills.Float212 = ReadFloat(dataElement, XmlDataSkills.StringFloat212);
            }
            if (_TestField2(bitField2, SkillsBitField2.FloatValue213))
            {
                xmlDataSkills.Float213 = ReadFloat(dataElement, XmlDataSkills.StringFloat213);
            }
            if (_TestField2(bitField2, SkillsBitField2.FloatValue214))
            {
                xmlDataSkills.Float214 = ReadFloat(dataElement, XmlDataSkills.StringFloat214);
            }
            if (_TestField2(bitField2, SkillsBitField2.FloatValue215))
            {
                xmlDataSkills.Float215 = ReadFloat(dataElement, XmlDataSkills.StringFloat215);
            }
            if (_TestField2(bitField2, SkillsBitField2.IntValue216))
            {
                xmlDataSkills.Int216 = ReadInt32(dataElement, XmlDataSkills.StringInt216);
            }
            if (_TestField2(bitField2, SkillsBitField2.IntValue217))
            {
                xmlDataSkills.Int217 = ReadInt32(dataElement, XmlDataSkills.StringInt217);
            }
            if (_TestField2(bitField2, SkillsBitField2.DataString218))
            {
                xmlDataSkills.DataString = ReadZeroString(dataElement, XmlDataSkills.StringData);
            }
            if (_TestField2(bitField2, SkillsBitField2.BoneString221))
            {
                xmlDataSkills.BoneString = ReadZeroString(dataElement, XmlDataSkills.StringBone);
            }
            if (_TestField2(bitField2, SkillsBitField2.FloatValue223))
            {
                xmlDataSkills.Float223 = ReadFloat(dataElement, XmlDataSkills.StringFloat223);
            }
            if (_TestField2(bitField2, SkillsBitField2.FloatValue224))
            {
                xmlDataSkills.Float224 = ReadFloat(dataElement, XmlDataSkills.StringFloat224);
            }
            if (_TestField2(bitField2, SkillsBitField2.FloatValue225))
            {
                xmlDataSkills.Float225 = ReadFloat(dataElement, XmlDataSkills.StringFloat225);
            }
            if (_TestField2(bitField2, SkillsBitField2.FloatValue226))
            {
                xmlDataSkills.Float226 = ReadFloat(dataElement, XmlDataSkills.StringFloat226);
            }
            if (_TestField2(bitField2, SkillsBitField2.FloatValue227))
            {
                xmlDataSkills.Float227 = ReadFloat(dataElement, XmlDataSkills.StringFloat227);
            }
            if (_TestField2(bitField2, SkillsBitField2.FloatValue228))
            {
                xmlDataSkills.Float228 = ReadFloat(dataElement, XmlDataSkills.StringFloat228);
            }
            if (_TestField2(bitField2, SkillsBitField2.Conditions229))
            {
                xmlDataSkills.Conditions = new XmlDataSkills.DataConditionsElement();

                XmlElement footerElement = XmlDoc.CreateElement(XmlDataSkills.StringConditions);
                dataElement.AppendChild(footerElement);
                xmlDataSkills.Conditions.XmlNode = footerElement;

                byte footerToken = FileTools.ByteArrayTo<byte>(data, ref offset);
                xmlDataSkills.Conditions.Token1 = footerToken;
                if (footerToken != 0x7F && footerToken != 0xFF)
                {
                    MessageBox.Show("Unexpected footerToken value!\n\nfooterToken = 0x" + footerToken.ToString("X2"),
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Debug.Assert(false);
                    return false;
                }

                byte unknownToken0 = FileTools.ByteArrayTo<byte>(data, ref offset);
                xmlDataSkills.Conditions.Token2 = unknownToken0;


                /* conditions 1, 3, 5 were seen semi-reguarly within the consumable skills, 2, 4, 7 not at all
                 * the reset are as mentioned below (1, 3, 5 were still seen here and there with them though)
                 */
                // seen quite frequently
                xmlDataSkills.Conditions.Condition1 = ReadByteString(footerElement, XmlDataSkills.DataConditionsElement.StringCondition1, true);

                // first seen as "sfx_electrical" in drainlife.xml.cooked
                // seen in fiendpriesthellgatelaser.xml.cooked: Condition1 = "Does Not Have State", Condition2State = "hellball_hidden"   i.e. is a state check?
                xmlDataSkills.Conditions.Coniditon2State = ReadByteString(footerElement, XmlDataSkills.DataConditionsElement.StringCondition2State, true);

                // seen quite frequently
                xmlDataSkills.Conditions.Condition3 = ReadByteString(footerElement, XmlDataSkills.DataConditionsElement.StringCondition3, true);

                // first seen as "Spectral_Nova" in spectralzap.xml.cooked
                // also in fiendpriestheal.xml.cooked (w\Condition1 = "Has Skill Level", Condition2SFX = "increase_healing_source", Condition4DoSkill = "Increase_Healing")
                xmlDataSkills.Conditions.Condition4DoSkill = ReadByteString(footerElement, XmlDataSkills.DataConditionsElement.StringCondition4DoSkill, true);        // I think this is the "if - *do this*" part

                // seen quite frequently
                xmlDataSkills.Conditions.Condition5 = ReadByteString(footerElement, XmlDataSkills.DataConditionsElement.StringCondition5, true);

                // first seen as "zombiefood" in enragecompanion.xml.cooked
                xmlDataSkills.Conditions.Condition6State = ReadByteString(footerElement, XmlDataSkills.DataConditionsElement.StringCondition6State, true);

                // first seen as "level" in summonblaster.xml.cooked
                xmlDataSkills.Conditions.Condition7 = ReadByteString(footerElement, XmlDataSkills.DataConditionsElement.StringCondition7, true);

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
                    xmlDataSkills.Conditions.Float0 = ReadFloat(footerElement, XmlDataSkills.DataConditionsElement.StringFloat0);
                }
                else if (footerToken == 0x7F && unknownToken0 == 0x01)
                {
                    xmlDataSkills.Conditions.Float1 = ReadFloat(footerElement, XmlDataSkills.DataConditionsElement.StringFloat1);
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
                    xmlDataSkills.Conditions.BitField = ReadBitField(footerElement, XmlDataSkills.DataConditionsElement.StringBitField);

                    XmlElement bitFieldElement = footerElement.LastChild as XmlElement;
                    if (bitFieldElement != null)
                    {
                        bitFieldElement.SetAttribute(XmlDataSkills.DataConditionsElement.StringUnknownTokenAttribute, unknownToken0.ToString());
                    }
                }
                else if (unknownToken0 != 0x00 && unknownToken0 != 0x01)
                {
                    MessageBox.Show(
                        "Unexpected footer unknownToken0!\n\nunknownToken0 = 0x" + unknownToken0.ToString("X2"), "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Debug.Assert(false);
                    return false;
                }
            }

            return true;
        }

        private static bool _TestField1(SkillsBitField1 testField, SkillsBitField1 testAgainst)
        {
            return ((testField & testAgainst) > 0);
        }

        private static bool _TestField2(SkillsBitField2 testField, SkillsBitField2 testAgainst)
        {
            return ((testField & testAgainst) > 0);
        }
    }
}