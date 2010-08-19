using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Reanimator
{
    class XmlCookedSkill : XmlCookedBase
    {
        class XmlDataSkills
        {
            public class DataConditionsElement
            {
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

            public string TypeString;
            public uint BitField1;
            public uint BitField2;
            public int Int209;
            public float Float211;
            public float Float210;
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

        protected override bool ParseDataSegment(XmlElement dataElement)
        {
            byte dataToken = FileTools.ByteArrayTo<byte>(Data, ref Offset);

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
                ReadZeroString(dataElement, "StringHeader");
            }

            int branchCount = FileTools.ByteArrayTo<Int32>(Data, ref Offset);
            for (int i = 0; i < branchCount; i++)
            {
                if (!_ParseBranch(Data, ref Offset, dataElement)) return false;
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
                XmlDataSkills xmlDataSkills = new XmlDataSkills();
                DataElements.Add(xmlDataSkills);

                if (!_ParseElements(data, ref offset, xmlElement, xmlDataSkills)) return false;
            }

            return true;
        }

        private bool _ParseElements(byte[] data, ref int offset, XmlElement parentElement, XmlDataSkills xmlDataSkills)
        {
            //Int64 elementMask = FileTools.ByteArrayTo<Int64>(data, ref offset);
            SkillsBitField1 bitField1 = (SkillsBitField1)BitConverter.ToUInt32(data, offset);
            offset += 4;
            SkillsBitField2 bitField2 = (SkillsBitField2)BitConverter.ToUInt32(data, offset);
            offset += 4;

            XmlElement dataElement = XmlDoc.CreateElement("data");
            parentElement.AppendChild(dataElement);


            // ================BitField1================
            if (_TestField1(bitField1, SkillsBitField1.TypeString100))
            {
                xmlDataSkills.TypeString = ReadByteString(dataElement, "szType", true);
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
                xmlDataSkills.BitField1 = ReadBitField(dataElement, "BitField1");
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
                xmlDataSkills.BitField2 = ReadBitField(dataElement, "BitField2");
            }


            if (_TestField2(bitField2, SkillsBitField2.FloatValue210))
            {
                xmlDataSkills.Float210 = ReadFloat(dataElement, "FloatValue210");
            }
            if (_TestField2(bitField2, SkillsBitField2.FloatValue211))
            {
                xmlDataSkills.Float211 = ReadFloat(dataElement, "FloatValue211");
            }
            if (_TestField2(bitField2, SkillsBitField2.FloatValue212))
            {
                xmlDataSkills.Float212 = ReadFloat(dataElement, "FloatValue212");
            }
            if (_TestField2(bitField2, SkillsBitField2.FloatValue213))
            {
                xmlDataSkills.Float213 = ReadFloat(dataElement, "FloatValue213");
            }
            if (_TestField2(bitField2, SkillsBitField2.FloatValue214))
            {
                xmlDataSkills.Float214 = ReadFloat(dataElement, "FloatValue214");
            }
            if (_TestField2(bitField2, SkillsBitField2.FloatValue215))
            {
                xmlDataSkills.Float215 = ReadFloat(dataElement, "FloatValue215");
            }
            if (_TestField2(bitField2, SkillsBitField2.IntValue216))
            {
                xmlDataSkills.Int216 = ReadInt32(dataElement, "IntValue216");
            }
            if (_TestField2(bitField2, SkillsBitField2.IntValue217))
            {
                xmlDataSkills.Int217 = ReadInt32(dataElement, "IntValue217");
            }
            if (_TestField2(bitField2, SkillsBitField2.DataString218))
            {
                xmlDataSkills.DataString = ReadZeroString(dataElement, "szData");
            }
            if (_TestField2(bitField2, SkillsBitField2.BoneString221))
            {
                xmlDataSkills.BoneString = ReadZeroString(dataElement, "szBone");
            }
            if (_TestField2(bitField2, SkillsBitField2.FloatValue223))
            {
                xmlDataSkills.Float223 = ReadFloat(dataElement, "FloatValue223");
            }
            if (_TestField2(bitField2, SkillsBitField2.FloatValue224))
            {
                xmlDataSkills.Float224 = ReadFloat(dataElement, "FloatValue224");
            }
            if (_TestField2(bitField2, SkillsBitField2.FloatValue225))
            {
                xmlDataSkills.Float225 = ReadFloat(dataElement, "FloatValue225");
            }
            if (_TestField2(bitField2, SkillsBitField2.FloatValue226))
            {
                xmlDataSkills.Float226 = ReadFloat(dataElement, "FloatValue226");
            }
            if (_TestField2(bitField2, SkillsBitField2.FloatValue227))
            {
                xmlDataSkills.Float227 = ReadFloat(dataElement, "FloatValue227");
            }
            if (_TestField2(bitField2, SkillsBitField2.FloatValue228))
            {
                xmlDataSkills.Float228 = ReadFloat(dataElement, "FloatValue228");
            }
            if (_TestField2(bitField2, SkillsBitField2.Conditions229))
            {
                xmlDataSkills.Conditions = new XmlDataSkills.DataConditionsElement();

                XmlElement footerElement = XmlDoc.CreateElement("Conditions229");
                dataElement.AppendChild(footerElement);

                byte footerToken = FileTools.ByteArrayTo<byte>(data, ref offset);
                if (footerToken != 0x7F && footerToken != 0xFF)
                {
                    MessageBox.Show("Unexpected footerToken value!\n\nfooterToken = 0x" + footerToken.ToString("X2"),
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Debug.Assert(false);
                    return false;
                }

                byte unknownToken0 = FileTools.ByteArrayTo<byte>(data, ref offset);


                /* conditions 1, 3, 5 were seen semi-reguarly within the consumable skills, 2, 4, 7 not at all
                 * the reset are as mentioned below (1, 3, 5 were still seen here and there with them though)
                 */
                // seen quite frequently
                xmlDataSkills.Conditions.Condition1 = ReadByteString(footerElement, "Condition1", true);

                // first seen as "sfx_electrical" in drainlife.xml.cooked
                // seen in fiendpriesthellgatelaser.xml.cooked: Condition1 = "Does Not Have State", Condition2State = "hellball_hidden"   i.e. is a state check?
                xmlDataSkills.Conditions.Coniditon2State = ReadByteString(footerElement, "Condition2State", true);

                // seen quite frequently
                xmlDataSkills.Conditions.Condition3 = ReadByteString(footerElement, "Condition3", true);

                // first seen as "Spectral_Nova" in spectralzap.xml.cooked
                // also in fiendpriestheal.xml.cooked (w\Condition1 = "Has Skill Level", Condition2SFX = "increase_healing_source", Condition4DoSkill = "Increase_Healing")
                xmlDataSkills.Conditions.Condition4DoSkill = ReadByteString(footerElement, "Condition4DoSkill", true);        // I think this is the "if - *do this*" part

                // seen quite frequently
                xmlDataSkills.Conditions.Condition5 = ReadByteString(footerElement, "Condition5", true);

                // first seen as "zombiefood" in enragecompanion.xml.cooked
                xmlDataSkills.Conditions.Condition6State = ReadByteString(footerElement, "Condition6State", true);

                // first seen as "level" in summonblaster.xml.cooked
                xmlDataSkills.Conditions.Condition7 = ReadByteString(footerElement, "Condition7", true);

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
                    xmlDataSkills.Conditions.Float0 = ReadFloat(footerElement, "footerToken_floatValue0x00FF");
                }
                else if (footerToken == 0x7F && unknownToken0 == 0x01)
                {
                    xmlDataSkills.Conditions.Float1 = ReadFloat(footerElement, "footerToken_floatValue0x01FF");
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
                    xmlDataSkills.Conditions.BitField = ReadBitField(footerElement, "BitField");

                    XmlElement bitFieldElement = footerElement.LastChild as XmlElement;
                    if (bitFieldElement != null)
                    {
                        bitFieldElement.SetAttribute("unknownToken0", unknownToken0.ToString());
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
