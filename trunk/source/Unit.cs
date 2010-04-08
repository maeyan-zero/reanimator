using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Reanimator
{
    // pretty sure this is populated based on a bit field... should check...
    [Serializable]
    public struct UnitStat_OtherAttribute
    {
        // if (otherAttributeFlag & 0x01)
        public int unknown1;										// 4 bits
        // if (otherAttributeFlag & 0x02)
        public int unknown2;										// 12 bits
        // if (otherAttributeFlag & 0x04)
        public int unknown3;										// 1 bit		// possibly another reasource flag or something - if not 0x01 alert
    };

    [Serializable]
    public struct UnitStatAdditional
    {
        public int unknown;											// 16 bits
        public int statCount;										// 16 bits
        public Unit.StatBlock.Stat[] stats;
    };

    [Serializable]
    public struct UnitStatName
    {
        public int unknown1;										// 16 bits
        public Unit.StatBlock statBlock;							// nameCount * UnitStatBlock stuffs
    };

    [Serializable]
    public struct UnknownCount1F_S
    {
        public int unknown1;										// 16 bits
        public int unknown2;										// 16 bits
    };

    [Serializable]
    public struct UnknownCount1B_S
    {
        public int unknown1;										// 16 bits
        public int itemEndBitOffset;								// 32 bits
    };

    [Serializable]
    public struct UnitAppearance
    {
        [Serializable]
        public struct UnknownCount1_S
        {
            // if (bitTest(bitField1, 0x0F)) // untested
            public int unknown1;									// 32 bits

            public int unknown2;									// 16 bits

            // if (bitTest(bitField1, 0x00)) // if (bitField1 & 0x01)
            public int unknownCount1;								// 3 bits		// alert if != 0 (not encountered yet)
            public int[] unknownCount1s;						    // 32 bits * unknownCount1
        };

        [Serializable]
        public struct ModelAppearance_S
        {
            public int body;										// 16 bits
            public int head;										// 16 bits
            public int hair;										// 16 bits
            public int faceAccessory;								// 16 bits
        };

        [Serializable]
        public struct GearAppearance_S
        {
            public int gear;										// 16 bits
            public int unknownBool;									// 1 bit
            public int unknownBoolValue;					    	// 2 bits
        };

        public int unknownCount1;									// 3 bits
        public UnknownCount1_S[] unknownCount1s;

        public int unknown1;										// 16 bits

        // if (bitTest(bitField1, 0x16))
        public byte[] unknown2;										// non-standard read in again

        // if (bitTest(bitField1, 0x11))
        public int unknownCount2;									// 4 bits
        public int[] unknownCount2s;						        // 16 bits * unknownCount2

        public int modelAppearanceCounter;							// 3 bits
        public ModelAppearance_S modelAppearance;					// 16 bits * modelAppearanceCounter

        public int unknownCount3;									// 4 bits
        public int[] unknownCount3s;						        // 8 bits * unknownCount3

        // if (testBit(pUnit->bitField1, 0x10))
        public int gearCount;										// 16 bits
        public GearAppearance_S[] gears;				        	// 17 bits * gearCount
    };

    [Serializable]
    public struct UnitWeaponConfig
    {
        public int id;                                              // 16 bits      // .text:00000001403DB5EA mov     edx, 4       .text:00000001403DB5EF call    ConvertNumber?  ; Call Pr
        public int unknownCount1;                                   // 4 bits       // must be == 0x02
        public int[] exists1;                                       // 1 bit
        public int[] unknownIds1;                                   // 32 bits
        public int unknownCount2;                                   // 4 bits
        public int[] exists2;                                       // 1 bit
        public int[] unknownIds2;                                   // 32 bits
        public int idAnother;                                       // 32 bits
    };

    [Serializable]
    public class Unit
    {
        //Used for XMLSerialization
        public Unit()
        {
        }

        [Serializable]
        public class StatBlock
        {
            public Stat GetStatByName(string name)
            {
                foreach (Stat stat in stats)
                {
                    if (stat.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return stat;
                    }
                }
                return null;
            }

            public Stat GetStatById(int id)
            {
                foreach (Stat stat in stats)
                {
                    if (stat.Id == id)
                    {
                        return stat;
                    }
                }
                return null;
            }

            [Serializable]
            public class Stat
            {
                [Serializable]
                public class Attribute
                {
                    public int exists;										// 1 bit
                    public int BitCount { get; set; }							// 6 bits
                    public int Unknown1 { get; set; }							// 2 bits
                    public int Unknown1_1 { get; set; }							// 1 bit		// if Unknown1 == 2
                    public int skipTableId;            						// 1 bit		// if this is set, then don't read the table id below
                    public int TableId { get; set; }							// 16 bits		// this is the excel table id to use

                    public bool Exists
                    {
                        get { return exists == 1 ? true : false; }
                        set { exists = (value == true) ? 1 : 0; }
                    }

                    public bool SkipTableId
                    {
                        get { return skipTableId == 1 ? true : false; }
                        set { skipTableId = (value == true) ? 1 : 0; }
                    }
                };

                [Serializable]
                public class Values
                {
                    public int Attribute1 { get; set; }
                    public int Attribute2 { get; set; }
                    public int Attribute3 { get; set; }
                    public int Stat { get; set; }

                    public int AttributeAt(int index)
                    {
                        switch (index)
                        {
                            case 0:
                                {
                                    return Attribute1;
                                }
                            case 1:
                                {
                                    return Attribute2;
                                }
                            case 2:
                                {
                                    return Attribute3;
                                }
                            default:
                                {
                                    return -1;
                                }
                        }
                    }
                }


                public Values GetAttributeByAttributeId(int id)
                {
                    foreach (Values value in this.values)
                    {
                        if (value.Attribute1 == id)
                        {
                            return value;
                        }
                    }

                    return null;
                }

                public Stat()
                {
                    attributes = new List<Attribute>();
                }

                public int id;											    // 16 bits
                public int attributesCount;							        // 2 bits
                public List<Attribute> attributes;                                            // tells the game if it's a skill id, or waypoint flag, etc
                public int bitCount;									        // 6 bits		// size in bits of extra attribute
                public int otherAttributeFlag;								// 3 bits		// i think that's what this is...		// can be 0x00, 0x01, 0x02, 0x03, 0x04
                public UnitStat_OtherAttribute otherAttribute;
                public int skipResource;									    // 2 bits		// resouce flag I think...
                public int resource;										    // 16 bits		// like in extra attributes - read if above is 0x00
                public int repeatFlag;										// 1 bit		// if set, check for repeat number
                public int repeatCount;										// 10 bits		// i think this can be something other than 10 bits... I think...

                public Values[] values;

                public int Id
                {
                    get { return id; }
                }

                public Attribute Attribute1
                {
                    get { return AttributeAt(0); }
                }

                public Attribute Attribute2
                {
                    get { return AttributeAt(1); }
                }

                public Attribute Attribute3
                {
                    get { return AttributeAt(2); }
                }

                public Attribute AttributeAt(int index)
                {
                    if (index >= attributes.Count)
                    {
                        return null;
                    }

                    return attributes[index];
                }

                public string Name { get; set; }

                public int Length
                {
                    get { return values.Length; }
                }

                public override string ToString()
                {
                    return Name + " : (0x" + Id.ToString("X") + " : " + Id + ")";
                }

                public Values this[int index]
                {
                    get { return values[index]; }
                }
            };

            public int statVersion;										// 16 bits
            public int unknown1;										    // 3 bits		// untested - alert if != 0
            public int additionalStatCount;								// 6 bits
            public UnitStatAdditional[] additionalStats;

            public int statCount;										    // 16 bits
            public Stat[] stats;

            public int nameCount;										    // 8 bits		// i think this has something to do with item affix/prefix naming
            public UnitStatName[] names;

            public int Length
            {
                get { return stats.Length; }
            }

            public Stat this[int index]
            {
                get { return stats[index]; }
            }
        };

        public Unit(BitBuffer bb)
        {
            bitBuffer = bb;
            PlayerFlags1 = new List<int>();
            PlayerFlags2 = new List<int>();
        }

        public StatBlock Stats
        {
            get { return statBlock; }
        }

        public List<Unit> Items
        {
            get { return items; }
        }

        public string Name
        {
            get { return ToString(); }
            set { characterName = value.ToCharArray(); }
        }

        public override string ToString()
        {
            return new string(characterName);
        }

        ////// Start of read inside main header check function (in ASM) //////

        public int majorVersion;							    	    // 16 bits
        public int minorVersion;							    	    // 8

        public int bitFieldCount;									    // 8			// must be <= 2
        public int bitField1;										    // 32
        public int bitField2;										    // 32

        // if (testBit(unit->bitField1, 0x1D))
        public int bitCount;										    // 32			// of unit block

        // if (testBit(unit->bitField1, 0x00))
        public int beginFlag;										    // 32			// must be "Flag" (67616C46h) or Can be "`4R+" ("60 34 52 2B", 2B523460h)

        // if (testBit(unit->bitField1, 0x1C))
        public int timeStamp1;										// 32			// i don't think these are actually time stamps
        public int timeStamp2;										// 32			// but since they change all the time and can be
        public int timeStamp3;										// 32			// set to 00 00 00 00 and it'll still load... it'll do

        // if (testBit(unit->bitField1, 0x1F))
        public int unknownCount1F;									// 4
        public UnknownCount1F_S[] unknownCount1Fs;                                  // no idea wtf these do

        // if (testBit(unit->bitField2, 0x00)					                    // char state flags (e.g. "elite")
        private int _playerFlagCount1;								    // 8
        public List<int> PlayerFlags1 { get; set; }                     // 16 * _playerFlagCount1					

        ////// End of read inside main header check function (in ASM) //////

        // if (testBit(unit->bitField1, 0x1B))
        public int unknownCount1B;									// 5
        public UnknownCount1B_S[] unknownCount1Bs;			                        // no idea wtf these do either

        // if (testBit(unit->bitField1, 0x05)) // (bitField1 & 0x20)                // haven't encountered file with this yet
        // ALERT

        public int unknownFlag;										// 4		    // this value > e.g. 0x03 -> (0x3000000...00 & unknownFlagValue) or something like that
        public int itemCode;								            // 16

        // if (testBit(bitField1, 0x17)) // 64 bits read as 8x8 chunk bits from non-standard bit read function
        public byte[] unitUniqueId;                   // a unique id identifiying this unit "structure"

        // if (testBit(bitField1, 0x03) || testBit(bitField1, 0x01)) // if (bitField1 & 0x08 || bitField1 & 0x02)
        // {
        public int unknownBool_01_03;								    // 1 bit
        // {
        // if (testBit(bitField1, 0x02)) // if (bitField & 0x04)
        public int unknown_02;										// 32 bits			// untested

        public int inventoryType;        									// 16 bits          // i think...
        //public int unknown_01_03_2;									// 12 bits
        public int inventoryPositionX;									// 12 bits
        //public int unknown_01_03_3;									// 12 bits
        public int inventoryPositionY;									// 12 bits
        // }

        public byte[] unknown_01_03_4;								// 8*8 bits (64) - non-standard func
        // }

        // if (testBit(bitField1, 0x06)) // if (bitField1 & 0x40) // does more reading, but can't test
        public int unknownBool_06;									// 1 bit					// alert if != 1

        // if (testBit(bitField1, 0x09))
        public int unknown_09;										// 8 bits

        // if (testBit(bitField1, 0x07)) // if (bitField1 & 0x80)
        public int jobClass;										    // 8 bits		// i think...
        public int unknown_07;										// 8 bits		// this appears to be joined with jobClass to form a WORD... I think...

        public int JobClass
        {
          get { return jobClass; }
        }

        // if (testBit(bitField1, 0x08))
        public int characterCount;									    // 8 bits
        public Char[] characterName;                                                // character name - why does name change when change filename though?								

        // if (testBit(bitField1, 0x0A))						                    // char state flags (e.g. "elite")
        private int _playerFlagCount2;								    // 8 bits
        public List<int> PlayerFlags2 { get; set; }				        // 16 bits * _playerFlagCount2

        public int unknownBool1;									    // 1 bit		// as above - alert if != 0

        // if (testBit(bitField1, 0x0D))
        public StatBlock statBlock;

        public int hasAppearanceDetails;							    // 1
        public UnitAppearance unitAppearance;
        /*
        // {
        BYTE unknownCount7;										    // 3 bits
        std::vector<UnknownCount7_S> unknownCount7s;

        DWORD unknown5;											    // 16 bits

        //// if (bitTest(bitField1, 0x16))
            BYTE unknown6[8];									                	// non-standard read in again

        //// if (bitTest(bitField1, 0x11))
            BYTE unknownCount8;										// 4 bits
            std::vector<WORD> unknownCount8s;						// 16 bits * unknownCount8

            BYTE modelAppearanceCounter;							// 3 bits
            std::vector<WORD> modelAppearance;						// 16 bits * modelAppearanceCounter

            BYTE unknownCount9;										// 4 bits
            std::vector<BYTE> unknownCount9s;						// 8 bits * unknownCount9

        //// if (140267C48 test    byte ptr [r9+46h], 1) // 0xE7 part
            WORD gearCount;										    // 16 bits
            std::vector<Gear_S> gears;							    // 17 bits * gearCount
        // }*/

        // if (testBit(pUnit->bitField1, 0x12))
        public int itemEndBitOffset;									// 32 bits      // bit offset to end of items block
        public int itemCount;										    // 10 bits
        public List<Unit> items;													    // each item is just a standard data block

        // if (testBit(pUnit->bitField1, 0x1A))
        public uint weaponConfigFlag;                                 // 32 bits      // must be 0x91103A74; always present
        public int endFlagBitOffset;                                  // 32 bits      // offset to end of file flag
        public int weaponConfigCount;                                 // 6 bits       // weapon config count
        public UnitWeaponConfig[] weaponConfigs;                                // i think this has item positions on bottom bar, etc as well

        // if (testBit(unit->bitField1, 0x00))
        public int endFlag;											// 32 bits

        [NonSerialized]
        BitBuffer bitBuffer;

        public bool ReadUnit(ref Unit unit)
        {
            unit.majorVersion = bitBuffer.ReadBits(16);
            if (unit.majorVersion != 0x00BF)
            {
                MessageBox.Show("Error! Invalid Unit Major Version: " + unit.majorVersion.ToString("X4"));
                return false;
            }
            unit.minorVersion = bitBuffer.ReadBits(8);
            if (unit.minorVersion != 0x00)
            {
                MessageBox.Show("Warning! Untested Unit Minor Version: " + unit.minorVersion.ToString("X2"));
            }


            unit.bitFieldCount = bitBuffer.ReadBits(8);
            if (unit.bitFieldCount >= 1)
            {
                unit.bitField1 = bitBuffer.ReadBits(32);
            }
            if (unit.bitFieldCount == 2)
            {
                unit.bitField2 = bitBuffer.ReadBits(32);
            }


            if (TestBit(unit.bitField1, 0x1D))
            {
                unit.bitCount = bitBuffer.ReadBits(32);
            }


            if (TestBit(unit.bitField1, 0x00))
            {
                unit.beginFlag = bitBuffer.ReadBits(32);
            }


            if (TestBit(unit.bitField1, 0x1C))
            {
                unit.timeStamp1 = bitBuffer.ReadBits(32);
                unit.timeStamp2 = bitBuffer.ReadBits(32);
                unit.timeStamp3 = bitBuffer.ReadBits(32);
            }


            if (TestBit(unit.bitField1, 0x1F))
            {
                unit.unknownCount1F = bitBuffer.ReadBits(4) - 8;
                unit.unknownCount1Fs = new UnknownCount1F_S[unit.unknownCount1F];

                for (int i = 0; i < unit.unknownCount1F; i++)
                {
                    UnknownCount1F_S uc1;

                    uc1.unknown1 = bitBuffer.ReadBits(16);
                    uc1.unknown2 = bitBuffer.ReadBits(16);

                    unit.unknownCount1Fs[i] = uc1;
                }
            }


            if (TestBit(unit.bitField2, 0x00))
            {
                unit._playerFlagCount1 = bitBuffer.ReadBits(8);
                for (int i = 0; i < unit._playerFlagCount1; i++)
                {
                    unit.PlayerFlags1.Add(bitBuffer.ReadBits(16));
                }
            }


            if (TestBit(unit.bitField1, 0x1B))
            {
                unit.unknownCount1B = bitBuffer.ReadBits(5);
                unit.unknownCount1Bs = new UnknownCount1B_S[unit.unknownCount1B];

                if (unit.unknownCount1B > 1)
                {
                    MessageBox.Show("Unexpected unknownCount1B > 1!!\nNot-Implmented cases. Please report this error!",
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                for (int i = 0; i < unit.unknownCount1B; i++)
                {
                    UnknownCount1B_S uc3;

                    uc3.unknown1 = bitBuffer.ReadBits(16);
                    uc3.itemEndBitOffset = bitBuffer.ReadBits(32);

                    unit.unknownCount1Bs[i] = uc3;
                }
            }


            if (TestBit(unit.bitField1, 0x05))
            {
                return false;
            }


            unit.unknownFlag = bitBuffer.ReadBits(4);
            unit.itemCode = bitBuffer.ReadBits(16);


            if (TestBit(unit.bitField1, 0x17))
            {
                unit.unitUniqueId = ReadNonStandardFunc();
            }


            if (TestBit(unit.bitField1, 0x03) || TestBit(unit.bitField1, 0x01))
            {
                unit.unknownBool_01_03 = bitBuffer.ReadBits(1);
                if (unit.unknownBool_01_03 > 0)
                {
                    if (TestBit(unit.bitField1, 0x02))
                    {
                        unit.unknown_02 = bitBuffer.ReadBits(32);
                    }

                    unit.inventoryType = bitBuffer.ReadBits(16);
                    unit.inventoryPositionX = bitBuffer.ReadBits(12);
                    unit.inventoryPositionY = bitBuffer.ReadBits(12);
                }

                unit.unknown_01_03_4 = ReadNonStandardFunc();
            }

            if (TestBit(unit.bitField1, 0x06))
            {
                unit.unknownBool_06 = bitBuffer.ReadBits(1);
                if (unit.unknownBool_01_03 != 1)
                {
                    return false;
                }
            }

            // On items only I think - Usually 0x00
            if (TestBit(unit.bitField1, 0x09))
            {
                unit.unknown_09 = bitBuffer.ReadBits(8);
            }

            // On main character unit only
            if (TestBit(unit.bitField1, 0x07))
            {
                //bitBuffer.ReadBits(1);
                unit.jobClass = bitBuffer.ReadBits(8);
                //bitBuffer.ReadBits(3);
                unit.unknown_07 = bitBuffer.ReadBits(8);
            }

            // On main character unit only
            if (TestBit(unit.bitField1, 0x08))
            {
                unit.characterCount = bitBuffer.ReadBits(8);
                if (unit.characterCount > 0)
                {
                    unit.characterName = new Char[unit.characterCount];
                    for (int i = 0; i < unit.characterCount; i++)
                    {
                        unit.characterName[i] = (Char)bitBuffer.ReadBits(16);
                    }
                }
            }

            // On both the main char and on items - appears to be always zero for items
            if (TestBit(unit.bitField1, 0x0A))
            {
                unit._playerFlagCount2 = bitBuffer.ReadBits(8);
                if (unit._playerFlagCount2 > 0)
                {
                    for (int i = 0; i < unit._playerFlagCount2; i++)
                    {
                        unit.PlayerFlags2.Add(bitBuffer.ReadBits(16));
                    }
                }
            }


            unit.unknownBool1 = bitBuffer.ReadBits(1);
            if (unit.unknownBool1 != 0)
            {
                return false;
            }


            if (TestBit(unit.bitField1, 0x0D))
            {
                unit.statBlock = new StatBlock();
                if (!ReadStatBlock(ref unit.statBlock, true))
                    return false;
            }


            unit.hasAppearanceDetails = bitBuffer.ReadBits(1);
            if (unit.hasAppearanceDetails == 1)
            {
                unit.unitAppearance = new UnitAppearance();
                if (!ReadAppearance(ref unit, ref unit.unitAppearance))
                    return false;
            }


            if (TestBit(unit.bitField1, 0x12))
            {
                unit.itemEndBitOffset = bitBuffer.ReadBits(32);
                unit.itemCount = bitBuffer.ReadBits(10);
                unit.items = new List<Unit>();
                for (int i = 0; i < unit.itemCount; i++)
                {
                    Unit item = new Unit(bitBuffer);

                    if (!ReadUnit(ref item))
                        return false;

                    unit.items.Add(item);
                }
            }


            if (TestBit(unit.bitField1, 0x1A))
            {
                unit.weaponConfigFlag = (uint)bitBuffer.ReadBits(32);
                if (unit.weaponConfigFlag != 0x91103A74)
                {
                    MessageBox.Show("Flags not aligned!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                unit.endFlagBitOffset = bitBuffer.ReadBits(32);     // to end flag

                unit.weaponConfigCount = bitBuffer.ReadBits(6);
                unit.weaponConfigs = new UnitWeaponConfig[unit.weaponConfigCount];
                for (int i = 0; i < unit.weaponConfigCount; i++)
                {
                    UnitWeaponConfig weaponConfig = new UnitWeaponConfig();

                    weaponConfig.id = bitBuffer.ReadBits(16);

                    weaponConfig.unknownCount1 = bitBuffer.ReadBits(4);
                    if (weaponConfig.unknownCount1 != 0x02)
                    {
                        MessageBox.Show("if (weaponConfig.unknownCount1 != 0x02)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    weaponConfig.exists1 = new int[2];
                    weaponConfig.unknownIds1 = new int[2];
                    for (int j = 0; j < weaponConfig.unknownCount1; j++)
                    {
                        weaponConfig.exists1[j] = bitBuffer.ReadBits(1);
                        if (weaponConfig.exists1[j] == 1)
                        {
                            weaponConfig.unknownIds1[j] = bitBuffer.ReadBits(32); // under some condition this will be ReadFromOtherFunc thingy
                        }
                    }

                    // yes this chunk looks the same as above - the above chunk though is in a specific function and can differ at 1 point
                    // also, it can be != 2
                    weaponConfig.unknownCount2 = bitBuffer.ReadBits(4);
                    weaponConfig.exists2 = new int[weaponConfig.unknownCount2];
                    weaponConfig.unknownIds2 = new int[weaponConfig.unknownCount2];
                    for (int j = 0; j < weaponConfig.unknownCount2; j++)
                    {
                        weaponConfig.exists2[j] = bitBuffer.ReadBits(1);
                        if (weaponConfig.exists2[j] == 1)
                        {
                            weaponConfig.unknownIds2[j] = bitBuffer.ReadBits(32);
                        }
                    }

                    weaponConfig.idAnother = bitBuffer.ReadBits(32); // read from 0x17 file          // 0x3931 -> 0x4B

                    unit.weaponConfigs[i] = weaponConfig;
                }
            }


            if (TestBit(unit.bitField1, 0x00))
            {
                unit.endFlag = bitBuffer.ReadBits(32);
                if (unit.endFlag != unit.beginFlag && unit.endFlag != 0x2B523460)
                {
                    int bitOffset = unit.bitCount - bitBuffer.DataBitOffset;
                    int byteOffset = (bitBuffer.Length - bitBuffer.DataByteOffset) - (bitBuffer.DataBitOffset >> 3);
                    MessageBox.Show("Flags not aligned!\nBit Offset: " + bitBuffer.DataBitOffset + "\nExpected: " + unit.bitCount + " (+" + bitOffset +
                        ")\nByte Offset: " + (bitBuffer.DataBitOffset >> 3) + "\nExpected: " + (bitBuffer.Length - bitBuffer.DataByteOffset) + " (+" + byteOffset + ")",
                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }


            return true;
        }

        private bool ReadStatBlock(ref StatBlock statBlock, bool readNameCount)
        {
            statBlock.statVersion = bitBuffer.ReadBits(16);
            if (statBlock.statVersion != 0x0A)
            {
                MessageBox.Show("Error! Unknown Stat Block Version: " + statBlock.statVersion.ToString("X2"));
                return false;
            }
            statBlock.unknown1 = bitBuffer.ReadBits(3);
            statBlock.additionalStatCount = bitBuffer.ReadBits(6);
            statBlock.additionalStats = new UnitStatAdditional[statBlock.additionalStatCount];

            for (int i = 0; i < statBlock.additionalStatCount; i++)
            {
                UnitStatAdditional additionalStat = new UnitStatAdditional();

                additionalStat.unknown = bitBuffer.ReadBits(16);
                additionalStat.statCount = bitBuffer.ReadBits(16);
                additionalStat.stats = new StatBlock.Stat[additionalStat.statCount];

                for (int j = 0; j < additionalStat.statCount; j++)
                {
                    StatBlock.Stat unitStat = new StatBlock.Stat();
                    if (!ReadStat(ref unitStat))
                        return false;

                    additionalStat.stats[j] = unitStat;

                }

                statBlock.additionalStats[i] = additionalStat;
            }

            statBlock.statCount = bitBuffer.ReadBits(16);
            statBlock.stats = new StatBlock.Stat[statBlock.statCount];
            for (int i = 0; i < statBlock.statCount; i++)
            {
                StatBlock.Stat unitStat = new StatBlock.Stat();
                if (!ReadStat(ref unitStat))
                    return false;

                statBlock.stats[i] = unitStat;
            }

            if (!readNameCount)
                return true;

            statBlock.nameCount = bitBuffer.ReadBits(8);
            statBlock.names = new UnitStatName[statBlock.nameCount];

            for (int i = 0; i < statBlock.nameCount; i++)
            {
                UnitStatName name = new UnitStatName();

                name.unknown1 = bitBuffer.ReadBits(16);
                name.statBlock = new StatBlock();

                if (!ReadStatBlock(ref name.statBlock, false))
                    return false;

                statBlock.names[i] = name;
            }

            return true;
        }

        private bool ReadStat(ref StatBlock.Stat unitStat)
        {
            unitStat.id = bitBuffer.ReadBits(16);
            unitStat.attributesCount = bitBuffer.ReadBits(2);
            //unitStat.attributes = new StatBlock.Stat.Attribute[unitStat.attributesCount];

            for (int i = 0; i < unitStat.attributesCount; i++)
            {
                StatBlock.Stat.Attribute exAtrib = new StatBlock.Stat.Attribute();

                exAtrib.exists = bitBuffer.ReadBits(1);
                if (exAtrib.exists != 1)
                    break;

                exAtrib.BitCount = bitBuffer.ReadBits(6);

                exAtrib.Unknown1 = bitBuffer.ReadBits(2);
                if (exAtrib.Unknown1 == 0x02)
                {
                    exAtrib.Unknown1_1 = bitBuffer.ReadBits(1);
                }

                exAtrib.skipTableId = bitBuffer.ReadBits(1);
                if (!exAtrib.SkipTableId)
                {
                    exAtrib.TableId = bitBuffer.ReadBits(16);
                }

                unitStat.attributes.Add(exAtrib);
            }

            unitStat.bitCount = bitBuffer.ReadBits(6);

            unitStat.otherAttributeFlag = bitBuffer.ReadBits(3);
            if ((unitStat.otherAttributeFlag & 0x01) >= 1)
            {
                unitStat.otherAttribute.unknown1 = bitBuffer.ReadBits(4);
            }
            if ((unitStat.otherAttributeFlag & 0x02) >= 1)
            {
                unitStat.otherAttribute.unknown2 = bitBuffer.ReadBits(12);
            }
            if ((unitStat.otherAttributeFlag & 0x04) >= 1)
            {
                unitStat.otherAttribute.unknown3 = bitBuffer.ReadBits(1);
                if (unitStat.otherAttribute.unknown3 != 0x01)
                {
                    return false;
                }
            }

            unitStat.skipResource = bitBuffer.ReadBits(2);
            if (unitStat.skipResource == 0)
            {
                unitStat.resource = bitBuffer.ReadBits(16);
            }

            unitStat.repeatFlag = bitBuffer.ReadBits(1);
            unitStat.repeatCount = 1;
            if (unitStat.repeatFlag == 1)
            {
                unitStat.repeatCount = bitBuffer.ReadBits(10);
            }
            unitStat.values = new StatBlock.Stat.Values[unitStat.repeatCount];

            for (int i = 0; i < unitStat.repeatCount; i++)
            {
                unitStat.values[i] = new StatBlock.Stat.Values();
                for (int j = 0; j < unitStat.attributesCount; j++)
                {
                    if (unitStat.attributes[j].exists == 1)
                    {
                        int extraAttribute = bitBuffer.ReadBits(unitStat.attributes[j].BitCount);
                        if (j == 0)
                            unitStat.values[i].Attribute1 = extraAttribute;
                        if (j == 1)
                            unitStat.values[i].Attribute2 = extraAttribute;
                        if (j == 2)
                            unitStat.values[i].Attribute3 = extraAttribute;
                    }
                }

                unitStat.values[i].Stat = bitBuffer.ReadBits(unitStat.bitCount);
            }

            return true;
        }

        private bool ReadAppearance(ref Unit heroUnit, ref UnitAppearance appearance)
        {
            appearance.unknownCount1 = bitBuffer.ReadBits(3);
            appearance.unknownCount1s = new UnitAppearance.UnknownCount1_S[appearance.unknownCount1];
            for (int i = 0; i < appearance.unknownCount1; i++)
            {
                UnitAppearance.UnknownCount1_S uc1 = new UnitAppearance.UnknownCount1_S();

                if (TestBit(heroUnit.bitField1, 0x0F)) // untested
                {
                    uc1.unknown1 = bitBuffer.ReadBits(32);
                }

                uc1.unknown2 = bitBuffer.ReadBits(16);

                if (TestBit(heroUnit.bitField1, 0x00))
                {
                    uc1.unknownCount1 = bitBuffer.ReadBits(3);
                    uc1.unknownCount1s = new int[uc1.unknownCount1];
                    for (int j = 0; j < uc1.unknownCount1; j++)
                    {
                        uc1.unknownCount1s[j] = bitBuffer.ReadBits(32);
                    }
                }

                appearance.unknownCount1s[i] = uc1;
            }


            appearance.unknown1 = bitBuffer.ReadBits(16);


            if (TestBit(heroUnit.bitField1, 0x16))
            {
                appearance.unknown2 = ReadNonStandardFunc();
            }


            if (TestBit(heroUnit.bitField1, 0x11))
            {
                appearance.unknownCount2 = bitBuffer.ReadBits(4);
                appearance.unknownCount2s = new int[appearance.unknownCount2];
                for (int i = 0; i < appearance.unknownCount2; i++)
                {
                    appearance.unknownCount2s[i] = bitBuffer.ReadBits(16);
                }

                appearance.modelAppearanceCounter = bitBuffer.ReadBits(3);
                for (int i = 0; i < appearance.modelAppearanceCounter; i++)
                {
                    int modelAppearance = bitBuffer.ReadBits(16);
                    if (i == 0)
                        appearance.modelAppearance.body = modelAppearance;
                    if (i == 1)
                        appearance.modelAppearance.head = modelAppearance;
                    if (i == 2)
                        appearance.modelAppearance.hair = modelAppearance;
                    if (i == 3)
                        appearance.modelAppearance.faceAccessory = modelAppearance;
                    if (i >= 4)
                    {
                        MessageBox.Show("Warning! appearance.modelAppearanceCounter >= 4");
                    }
                }

                appearance.unknownCount3 = bitBuffer.ReadBits(4);
                appearance.unknownCount3s = new int[appearance.unknownCount3];
                for (int i = 0; i < appearance.unknownCount3; i++)
                {
                    appearance.unknownCount3s[i] = bitBuffer.ReadBits(8);
                }
            }


            if (TestBit(heroUnit.bitField1, 0x10))
            {
                appearance.gearCount = bitBuffer.ReadBits(16);
                appearance.gears = new UnitAppearance.GearAppearance_S[appearance.gearCount];
                for (int i = 0; i < appearance.gearCount; i++)
                {
                    UnitAppearance.GearAppearance_S gear = new UnitAppearance.GearAppearance_S();

                    gear.gear = bitBuffer.ReadBits(16);
                    gear.unknownBool = bitBuffer.ReadBits(1);
                    if (gear.unknownBool == 1)
                    {
                        gear.unknownBoolValue = bitBuffer.ReadBits(2);
                    }

                    appearance.gears[i] = gear;
                }
            }

            return true;
        }

        private byte[] ReadNonStandardFunc()
        {
            byte[] ret = new byte[8];

            for (int i = 0; i < 8; i++)
            {
                ret[i] = (byte)bitBuffer.ReadBits(8);
            }

            return ret;
        }

        public byte[] GenerateSaveData(byte[] charStringBytes)
        {
            BitBuffer saveBuffer = new BitBuffer();

            WriteUnit(saveBuffer, this, false, charStringBytes);

            return saveBuffer.GetData();
        }

        private void WriteUnit(BitBuffer saveBuffer, Unit unit, bool isItem, byte[] charStringBytes)
        {
            /***** Unit Header *****
             * majorVersion                                     16                  Should be 0xBF.
             * minorVersion                                     8                   Should be 0x00 - Not 100% sure that it is a minor version or anything.
             * bitFieldCount                                    8                   Must be <= 2. I haven't tested with it != 2 though.
             * {
             *      bitField                                    32                  Each bit determines if 'x' is read in or not.
             * }
             * 
             * if (TestBit(bitField1, 0x1D))
             * {
             *      bitCount                                    32                  Bit count of unit.
             * }
             * 
             * if (TestBit(bitField1, 0x00))
             * {
             *      beginFlag                                   32                  Flags are used to check file alignment.
             * }
             * 
             * if (TestBit(bitField1, 0x1C))
             * {
             *      timeStamp1                                  32                  I'm not actually sure what these three things are but they can be set to 0x00000000
             *      timeStamp2                                  32                  and it will still be loaded fine. I call them time stamps simply because the first
             *      timeStamp3                                  32                  one changes every time the file is saved.
             * }
             * 
             * if (TestBit(bitField1, 0x1F))
             * {
             *      unknownCount                                4                   count + 8 is what must be written. e.g. If count = 3, then write 11.
             *      {
             *          unknown                                 16                  // TO BE DETERMINED
             *          unknown                                 16                  // TO BE DETERMINED
             *      }
             * }
             * 
             * if (TestBit(bitField2, 0x00))                                        Take note: It's bitField2! This is the only time I've noticed its use.
             * {
             *      characterFlagCount                          8                   Character state flags
             *      {                                                               e.g. Elite, Hardcore, Hardcore Dead, etc.
             *          characterFlag                           16                  However it should be noted that the game doesn't actually
             *      }                                                               appear to use these. It uses the ones located further down.
             * }
             * 
             ***** Unit Body *****                                                  (the header "chunk" is read in its own function in-game - hence I call it a header, lol)
             *
             * if (TestBit(bitField1, 0x1B))
             * {
             *      unknownCount                                5                   Count of following block.
             *      {
             *          Unknown1                                16                  // TO BE DETEREMINED
             *          Unknown2                                32                  // TO BE DETEREMINED
             *      }
             * }
             * 
             * if (TestBit(bitField1, 0x05))
             * {
             *      unknown                                     ??                  Never encountered.
             * }
             * 
             * unknownFlag                                      4                   This value > e.g. 0x03 -> (0x3000000...00 & unknownFlagValue)
             * unknownFlagValue                                 16                  or something like that anyways.
             * 
             * if (TestBit(bitField1, 0x17))
             * {                                                                    This chunk is read in by a secondary non-standard function.
             *      unknown[8]                                  64                  I've not really bothered trying to figure it out yet,
             * }                                                                    But the function is used a few times, but always 64 bits.
             * 
             * if (TestBit(bitField1, 0x03) || TestBit(bitField1, 0x01))
             * {
             *      unknownBool                                 1                   // TO BE DETEREMINED
             *      {
             *          if (TestBit(bitField1, 0x02))
             *          {
             *              unknown                             32                  // UNTESTED
             *          }
             *      
             *          unknown                                 16                  // TO BE DETEREMINED
             *          unknown                                 12                  // TO BE DETEREMINED
             *          unknown                                 12                  // TO BE DETEREMINED
             *      }
             *      
             *      unknown[8]                                  64                  Non-standard function reading (as above).
             * }
             * 
             * if (TestBit(bitField1, 0x06))
             * {
             *      unknownBool                                 1                   // TO BE DETEREMINED
             * }                                                                    // If exists has always been 1.
             * 
             * if (TestBit(bitField1, 0x09))
             * {
             *      unknown                                     8                   // TO BE DETERMINED
             * }
             * 
             * if (TestBit(bitField1, 0x07))
             * {
             *      jobClass                                    8                   I think... Or something to do with it anyways.
             *      unknown                                     8                   // TO BE DETERMINED
             * }
             * 
             * if (TestBit(bitField1, 0x08))
             * {
             *      characterCount                              8                   Number of (unicode) characters in following string.
             *      characterName                               8*2*characterCount  Character's name - doesn't appear to be actually used in-game...
             * }                                                                    (zero end byte not included)
             * 
             * if (TestBit(bitField1, 0x0A))
             * {
             *      characterFlagCount                          8                   Character state flags
             *      {                                                               e.g. Elite, Hardcore, Hardcore Dead, etc.
             *          characterFlag                           16                  These flags actually affect in-game (unlike the previous set which
             *      }                                                               appear to be unused).
             * }
             * 
             * unknownBool                                      1                   // TO BE DETEREMINED
             *                                                                      // always appears to be 0.
             * if (TestBit(bitField1, 0x0D))
             * {
             *      UNIT STAT BLOCK                                                 See WriteStatBlock().
             * }
             * 
             * appearanceBool                                   1                   Bool type.
             * {
             *      unknownCount                                3                   Count of following block.
             *      {
             *          if (TestBit(bitField1, 0x0F))
             *          {
             *              unknown                             32                  // UNTESTED
             *          }
             *          
             *          unknown                                 16                  // TO BE DETERMINED
             *          
             *          if (TestBit(bitField1, 0x00))
             *          {
             *              unknownCount                        3                   Count of following block.
             *              {
             *                  unknown                         32                  // TO BE DETEREMINED
             *              }
             *          }
             *      }
             *      
             *      unknown                                     16                  // TO BE DETEREMINED
             *      
             *      if (TestBit(bitField1, 0x16))
             *      {
             *          unknown[8]                              64                  Non-standard function reading (as above).
             *      }
             *      
             *      if (TestBit(bitField1, 0x11))
             *      {
             *          unknownCount                            4                   Count of following block.
             *          {
             *              unknown                             16                  // TO BE DETEREMINED
             *          }
             *          
             *          modelAppearanceCount                    3                   Count of model appearance parts.
             *          {
             *              modelAppearance                     16                  Order is: body, head, hair, face accessory. 
             *          }
             *          
             *          unknownCount                            4                   Count of following block.
             *          {
             *              unknown                             8                   // TO BE DETERMINED
             *          }
             *      }
             *      
             *      if (TestBit(bitField1, 0x10))
             *      {
             *          gearCount                               16                  Count of equipped gears.
             *          {
             *              gearId                              16                  Refers to ID within applicable excel table.
             *              unknownBool                         1                   Bool type.
             *              {
             *                  unknown                         2                   // TO BE DETEREMINED
             *              }
             *          }
             *      }
             * }
             * 
             * 
             * itemBitOffset                                    32                  Bit offset to end of all item blocks.
             * itemCount                                        10                  Count of items.
             * {
             *      ITEMS
             * }
             * 
             * 
             * if (TestBit(bitField1, 0x1A))
             * {
             *      weaponConfigFlag                            32                  Must be 0x91103A74.
             *      endFlagBitOffset                            32                  Bit offset to end flag.
             *      weaponConfigCount                           6                   Count of weapon configs.
             *      {
             *          unknown                                 16                  // TO BE DETERMINED
             *          unknownCount                            4                   Count of following block - Must be 0x02.
             *          {
             *              exists                              1                   Bool type.
             *              {
             *                  unknown                         32                  // TO BE DETEREMINED
             *              }
             *          }
             *          
             *          unknownCount                            
             *      }
             * }
             */

            // section chunk flags
            // the order is around what they're actually read/checked in loading
            int use_1D_BitCountEOF = (1 << 0x1D);
            int use_00_FlagAlignment = 1;
            int use_1C_TimeStamps = (1 << 0x1C);
            int useUnknown_1F = (1 << 0x1F);
            int use_00_CharacterFlags1 = 1;
            int useUnknown_1B = (1 << 0x1B);
            int useUnknown_17 = (1 << 0x17);
            int useUnknown_03 = 0; // (1 << 0x03);
            int useUnknown_01 = 0; // (1 << 0x01);
            int useUnknown_02 = 0; // (1 << 0x02);
            int useUnknown_06 = 0; // (1 << 0x06);
            int useUnknown_09 = 0; // (1 << 0x09);
            int useUnknown_07 = 0; // (1 << 0x07);
            int useUnknown_08 = 0; // (1 << 0x08);
            int use_0A_CharacterFlags2 = 0; // (1 << 0x0A);
            int use_0D_Stats = 0; // (1 << 0x0D);
            int useUnknown_0F = 0; // (1 << 0x0F);
            int useUnknown_16 = 0; // (1 << 0x16);
            int useUnknown_11 = 0; // (1 << 0x11);
            int useUnknown_10 = 0; // (1 << 0x10);
            int use_12_Items = 0; // (1 << 0x12);
            int use_1A_Unknown = 0; // (1 << 0x1A);


            // temp "fix" until we know what they actually are and how sensitive it is for missing fields
            if (TestBit(unit.bitField1, 0x01))
                useUnknown_01 = (1 << 0x01);
            if (TestBit(unit.bitField1, 0x02))
                useUnknown_02 = (1 << 0x02);
            if (TestBit(unit.bitField1, 0x03))
                useUnknown_03 = (1 << 0x03);
            if (TestBit(unit.bitField1, 0x06))
                useUnknown_06 = (1 << 0x06);
            if (TestBit(unit.bitField1, 0x09))
                useUnknown_09 = (1 << 0x09);
            if (TestBit(unit.bitField1, 0x07))
                useUnknown_07 = (1 << 0x07);
            if (TestBit(unit.bitField1, 0x08))
                useUnknown_08 = (1 << 0x08);
            if (TestBit(unit.bitField1, 0x0A))
                use_0A_CharacterFlags2 = (1 << 0x0A);
            if (TestBit(unit.bitField1, 0x0D))
                use_0D_Stats = (1 << 0x0D);
            if (TestBit(unit.bitField1, 0x0F))
                useUnknown_0F = (1 << 0x0F);
            if (TestBit(unit.bitField1, 0x16))
                useUnknown_16 = (1 << 0x16);
            if (TestBit(unit.bitField1, 0x11))
                useUnknown_11 = (1 << 0x11);
            if (TestBit(unit.bitField1, 0x10))
                useUnknown_10 = (1 << 0x10);
            if (TestBit(unit.bitField1, 0x12))
                use_12_Items = (1 << 0x12);
            if (TestBit(unit.bitField1, 0x1A))
                use_1A_Unknown = (1 << 0x1A);



            // need to keep track of things as we go
            int bitField1 = 0x00000000;
            int bitField1Offset = -1;
            int bitField2 = 0x00000000;
            int bitField2Offset = -1;
            int bitCountEOFOffset = -1;

            /***** Unit Header *****/

            int bitCountStart = saveBuffer.DataBitOffset;

            saveBuffer.WriteBits(0x00BF, 16);
            saveBuffer.WriteBits(0x00, 8);
            saveBuffer.WriteBits(0x02, 8);
            {
                bitField1Offset = saveBuffer.DataBitOffset;
                saveBuffer.WriteBits(bitField1, 32);
                bitField2Offset = saveBuffer.DataBitOffset;
                saveBuffer.WriteBits(bitField2, 32);
            }

            if (use_1D_BitCountEOF > 0)
            {
                bitCountEOFOffset = saveBuffer.DataBitOffset;
                saveBuffer.WriteBits(0x00000000, 32);
                bitField1 |= use_1D_BitCountEOF;
                saveBuffer.WriteBits(bitField1, 32, bitField1Offset);
            }

            if (use_00_FlagAlignment > 0)
            {
                if (isItem)
                {
                    saveBuffer.WriteBits(0x2B523460, 32); // "`4R+"
                }
                else
                {
                    saveBuffer.WriteBits(0x67616C46, 32); // "Flag"
                }
                bitField1 |= use_00_FlagAlignment;
                saveBuffer.WriteBits(bitField1, 32, bitField1Offset);
            }

            if (unit.timeStamp1 != 0)
            {
                saveBuffer.WriteBits(unit.timeStamp1, 32);
                saveBuffer.WriteBits(unit.timeStamp2, 32);
                saveBuffer.WriteBits(unit.timeStamp3, 32);
                bitField1 |= use_1C_TimeStamps;
                saveBuffer.WriteBits(bitField1, 32, bitField1Offset);
            }

            if (unit.unknownCount1F != 0)
            {
                saveBuffer.WriteBits(unit.unknownCount1F + 8, 4);
                for (int i = 0; i < unit.unknownCount1F; i++)
                {
                    UnknownCount1F_S uc1F = unit.unknownCount1Fs[i];
                    saveBuffer.WriteBits(uc1F.unknown1, 16);
                    saveBuffer.WriteBits(uc1F.unknown2, 16);
                }

                bitField1 |= useUnknown_1F;
                saveBuffer.WriteBits(bitField1, 32, bitField1Offset);
            }

            if (use_00_CharacterFlags1 > 0 && !isItem)
            {
                int playerFlagCount = unit.PlayerFlags1.Count;

                saveBuffer.WriteBits(playerFlagCount, 8);
                for (int i = 0; i < playerFlagCount; i++)
                {
                    saveBuffer.WriteBits(unit.PlayerFlags1[i], 16);
                }

                bitField2 |= use_00_CharacterFlags1;
                saveBuffer.WriteBits(bitField2, 32, bitField2Offset);
            }

            /***** Unit Body *****/

            int bitOffsetItemEndBitOffset = 0;
            if (unit.unknownCount1B > 0)
            {
                saveBuffer.WriteBits(unit.unknownCount1B, 5);
                for (int i = 0; i < unit.unknownCount1B; i++)
                {
                    saveBuffer.WriteBits(unit.unknownCount1Bs[i].unknown1, 16);
                    bitOffsetItemEndBitOffset = saveBuffer.DataBitOffset;
                    saveBuffer.WriteBits(0x00000000, 32);
                }

                bitField1 |= useUnknown_1B;
                saveBuffer.WriteBits(bitField1, 32, bitField1Offset);
            }

            saveBuffer.WriteBits(unit.unknownFlag, 4);
            saveBuffer.WriteBits(unit.itemCode, 16);

            if (useUnknown_17 > 0)
            {
                WriteNonStandardFunc(unit.unitUniqueId, saveBuffer);
                bitField1 |= useUnknown_17;
                saveBuffer.WriteBits(bitField1, 32, bitField1Offset);
            }

            if (useUnknown_03 > 0 || useUnknown_01 > 0)
            {
                saveBuffer.WriteBits(unit.unknownBool_01_03, 1);
                if (unit.unknownBool_01_03 == 1)
                {
                    if (useUnknown_02 > 0)
                    {
                        saveBuffer.WriteBits(unit.unknown_02, 32);
                        bitField1 |= useUnknown_02;
                        saveBuffer.WriteBits(bitField1, 32, bitField1Offset);
                    }

                    saveBuffer.WriteBits(unit.inventoryType, 16);
                    saveBuffer.WriteBits(unit.inventoryPositionX, 12);
                    saveBuffer.WriteBits(unit.inventoryPositionY, 12);
                }

                WriteNonStandardFunc(unit.unknown_01_03_4, saveBuffer);

                bitField1 |= useUnknown_01;
                saveBuffer.WriteBits(bitField1, 32, bitField1Offset);
                bitField1 |= useUnknown_03;
                saveBuffer.WriteBits(bitField1, 32, bitField1Offset);
            }

            if (useUnknown_06 > 0)
            {
                saveBuffer.WriteBits(unit.unknownBool_06, 1);
                bitField1 |= useUnknown_06;
                saveBuffer.WriteBits(bitField1, 32, bitField1Offset);
            }

            if (useUnknown_09 > 0)
            {
                saveBuffer.WriteBits(unit.unknown_09, 8);
                bitField1 |= useUnknown_09;
                saveBuffer.WriteBits(bitField1, 32, bitField1Offset);
            }

            if (useUnknown_07 > 0)
            {
                saveBuffer.WriteBits(unit.jobClass, 8);
                saveBuffer.WriteBits(unit.unknown_07, 8);
                bitField1 |= useUnknown_07;
                saveBuffer.WriteBits(bitField1, 32, bitField1Offset);
            }

            if (useUnknown_08 > 0 && charStringBytes != null)
            {
                saveBuffer.WriteBits(charStringBytes.Length / 2, 8);
                for (int i = 0; i < charStringBytes.Length; i++)
                {
                    saveBuffer.WriteBits(charStringBytes[i], 8);
                }

                bitField1 |= useUnknown_08;
                saveBuffer.WriteBits(bitField1, 32, bitField1Offset);
            }

            if (use_0A_CharacterFlags2 > 0)
            {
                int playerFlagCount = unit.PlayerFlags2.Count;

                saveBuffer.WriteBits(playerFlagCount, 8);
                for (int i = 0; i < playerFlagCount; i++)
                {
                    saveBuffer.WriteBits(unit.PlayerFlags2[i], 16);
                }

                bitField1 |= use_0A_CharacterFlags2;
                saveBuffer.WriteBits(bitField1, 32, bitField1Offset);
            }

            saveBuffer.WriteBits(unit.unknownBool1, 1);

            if (use_0D_Stats > 0)
            {
                WriteStatBlock(unit.statBlock, true, saveBuffer);
                bitField1 |= use_0D_Stats;
                saveBuffer.WriteBits(bitField1, 32, bitField1Offset);
            }

            saveBuffer.WriteBits(unit.hasAppearanceDetails, 1);
            if (unit.hasAppearanceDetails > 0)
            {
                saveBuffer.WriteBits(unit.unitAppearance.unknownCount1, 3);
                for (int i = 0; i < unit.unitAppearance.unknownCount1; i++)
                {
                    if (useUnknown_0F > 0)
                    {
                        saveBuffer.WriteBits(unit.unitAppearance.unknownCount1s[i].unknown1, 32); // untested
                    }

                    saveBuffer.WriteBits(unit.unitAppearance.unknownCount1s[i].unknown2, 16);

                    if (use_00_FlagAlignment > 0)
                    {
                        saveBuffer.WriteBits(unit.unitAppearance.unknownCount1s[i].unknownCount1, 3);
                        for (int j = 0; j < unit.unitAppearance.unknownCount1s[i].unknownCount1; j++)
                        {
                            saveBuffer.WriteBits(unit.unitAppearance.unknownCount1s[i].unknownCount1s[j], 32);
                        }
                    }
                }

                saveBuffer.WriteBits(unit.unitAppearance.unknown1, 16);

                if (useUnknown_16 > 0)
                {
                    WriteNonStandardFunc(unit.unitAppearance.unknown2, saveBuffer);
                }

                if (useUnknown_11 > 0)
                {
                    saveBuffer.WriteBits(unit.unitAppearance.unknownCount2, 4);
                    for (int i = 0; i < unit.unitAppearance.unknownCount2; i++)
                    {
                        saveBuffer.WriteBits(unit.unitAppearance.unknownCount2s[i], 16);
                    }

                    saveBuffer.WriteBits(unit.unitAppearance.modelAppearanceCounter, 3);
                    for (int i = 0; i < unit.unitAppearance.modelAppearanceCounter; i++)
                    {
                        if (i == 0)
                            saveBuffer.WriteBits(unit.unitAppearance.modelAppearance.body, 16);
                        if (i == 1)
                            saveBuffer.WriteBits(unit.unitAppearance.modelAppearance.head, 16);
                        if (i == 2)
                            saveBuffer.WriteBits(unit.unitAppearance.modelAppearance.hair, 16);
                        if (i == 3)
                            saveBuffer.WriteBits(unit.unitAppearance.modelAppearance.faceAccessory, 16);
                    }

                    saveBuffer.WriteBits(unit.unitAppearance.unknownCount3, 4);
                    for (int i = 0; i < unit.unitAppearance.unknownCount3; i++)
                    {
                        saveBuffer.WriteBits(unit.unitAppearance.unknownCount3s[i], 8);
                    }
                }

                if (useUnknown_10 > 0)
                {
                    saveBuffer.WriteBits(unit.unitAppearance.gearCount, 16);
                    for (int i = 0; i < unit.unitAppearance.gearCount; i++)
                    {
                        saveBuffer.WriteBits(unit.unitAppearance.gears[i].gear, 16);
                        saveBuffer.WriteBits(unit.unitAppearance.gears[i].unknownBool, 1);
                        if (unit.unitAppearance.gears[i].unknownBool > 0)
                        {
                            saveBuffer.WriteBits(unit.unitAppearance.gears[i].unknownBoolValue, 2);
                        }
                    }
                }
            }


            if (use_12_Items > 0)
            {
                int itemBitOffset = saveBuffer.DataBitOffset;
                saveBuffer.WriteBits(0x00000000, 32);
                saveBuffer.WriteBits(unit.itemCount, 10);
                for (int i = 0; i < unit.itemCount; i++)
                {
                    WriteUnit(saveBuffer, unit.items[i], true, null);
                }

                saveBuffer.WriteBits(saveBuffer.DataBitOffset, 32, itemBitOffset);
                if (bitOffsetItemEndBitOffset > 0)
                {
                    saveBuffer.WriteBits(saveBuffer.DataBitOffset, 32, bitOffsetItemEndBitOffset);
                }
            }


            if (use_1A_Unknown > 0)
            {
                unchecked { saveBuffer.WriteBits((int)0x91103A74, 32); }

                int endFlagBitOffset = saveBuffer.DataBitOffset;
                saveBuffer.WriteBits(0x00000000, 32);

                saveBuffer.WriteBits(unit.weaponConfigCount, 6);
                for (int i = 0; i < unit.weaponConfigCount; i++)
                {
                    UnitWeaponConfig weaponConfig = unit.weaponConfigs[i];

                    saveBuffer.WriteBits(weaponConfig.id, 16);
                    saveBuffer.WriteBits(weaponConfig.unknownCount1, 4);
                    for (int j = 0; j < weaponConfig.unknownCount1; j++)
                    {
                        saveBuffer.WriteBits(weaponConfig.exists1[j], 1);
                        if (weaponConfig.exists1[j] > 0)
                        {
                            saveBuffer.WriteBits(weaponConfig.unknownIds1[j], 32);
                        }
                    }

                    // yes this chunk looks the same as above - the above chunk though is in a specific function and can differ at 1 point
                    // also, it can be != 2
                    saveBuffer.WriteBits(weaponConfig.unknownCount2, 4);
                    for (int j = 0; j < weaponConfig.unknownCount2; j++)
                    {
                        saveBuffer.WriteBits(weaponConfig.exists2[j], 1);
                        if (weaponConfig.exists2[j] > 0)
                        {
                            saveBuffer.WriteBits(weaponConfig.unknownIds2[j], 32);
                        }
                    }

                    saveBuffer.WriteBits(weaponConfig.idAnother, 32);
                }

                saveBuffer.WriteBits(saveBuffer.DataBitOffset, 32, endFlagBitOffset);
            }


            if (use_00_FlagAlignment > 0)
            {
                saveBuffer.WriteBits(0x2B523460, 32); // "`4R+"
            }

            if (use_1D_BitCountEOF > 0)
            {
                saveBuffer.WriteBits(saveBuffer.DataBitOffset - bitCountStart, 32, bitCountEOFOffset);
            }

            // temp
            saveBuffer.WriteBits(unit.bitField1, 32, bitField1Offset);
            saveBuffer.WriteBits(unit.bitField2, 32, bitField2Offset);
        }

        private void WriteNonStandardFunc(byte[] byteArray, BitBuffer saveBuffer)
        {
            for (int i = 0; i < 8; i++)
            {
                saveBuffer.WriteBits(byteArray[i], 8);
            }
        }

        private void WriteStatBlock(StatBlock statBlock, bool writeNameCount, BitBuffer saveBuffer)
        {
            /***** Stat Block Header *****
             * majorVersion                                     16                  Stat block header - Must be 0x000A.
             * minorVersion                                     3                   Must be 0x0.
             * 
             * additionalStatCount                              6                   Additional Stats - Not sure of use yet.
             * {
             *      unknown                                     16                  // TO BE DETEREMINED
             *      statCount                                   16                  Count of following stats.
             *      {
             *          STATS                                                       See WriteStat().
             *      }
             * }
             * 
             * statCount                                        16                  Count of following stats.
             * {
             *      STATS                                                           See WriteStat().
             * }
             * 
             * if (writeNameCount)                                                  This is TRUE by default. Set to FALSE when writing a stat block
             * {                                                                    from the below name stat block chunk.
             *      nameCount                                   8                   I think this has something to do with item names.
             *      {
             *          unknown                                 16                  // TO BE DETEREMINED
             *          STAT BLOCK                                                  See WriteStatBlock().
             *      }
             * }
             */

            saveBuffer.WriteBits(0x000A, 16);
            saveBuffer.WriteBits(0x0, 3);

            saveBuffer.WriteBits(statBlock.additionalStatCount, 6);
            for (int i = 0; i < statBlock.additionalStatCount; i++)
            {
                saveBuffer.WriteBits(statBlock.additionalStats[i].unknown, 16);
                saveBuffer.WriteBits(statBlock.additionalStats[i].stats.Length, 16);

                for (int j = 0; j < statBlock.additionalStats[i].stats.Length; j++)
                {
                    WriteStat(statBlock.additionalStats[i].stats[j], saveBuffer);
                }
            }

            saveBuffer.WriteBits(statBlock.stats.Length, 16);
            for (int i = 0; i < statBlock.stats.Length; i++)
            {
                WriteStat(statBlock.stats[i], saveBuffer);
            }

            if (!writeNameCount)
            {
                return;
            }

            saveBuffer.WriteBits(statBlock.names.Length, 8);
            for (int i = 0; i < statBlock.names.Length; i++)
            {
                saveBuffer.WriteBits(statBlock.names[i].unknown1, 16);
                WriteStatBlock(statBlock.names[i].statBlock, false, saveBuffer);
            }
        }

        private void WriteStat(Unit.StatBlock.Stat stat, BitBuffer saveBuffer)
        {
            /***** Stat Block *****
             * statId                                           16                  Stat ID from applicable excel table.
             * 
             * extraAttributesCount                             2                   Count of following.
             * {
             *      exists                                      1                   Simple bool test.
             *      if (exists)
             *      {
             *          bitCount_EA                             6                   Number of bits used in file.
             *          
             *          unknownFlag                             2                   Only seen 0x00 and 0x02.
             *          if (unknownFlag == 0x02)
             *          {
             *              unknownBool                         1                   // TO BE DETEREMINED
             *          }
             *          
             *          skipResource                            1                   Bool type.
             *          if (!skipResource)
             *          {
             *              resourceId                          16                  Like statId, refers to some value in an excel table.
             *          }
             *      }
             * }
             * 
             * bitCount                                         6                   Number of bits used in file for stat value.
             * 
             * otherAttributeFlag                               3                   // TO BE DETEREMINED
             * if (otherAttributeFlag & 0x01)
             * {
             *      unknown                                     4                   // TO BE DETEREMINED
             * }
             * if (otherAttributeFlag & 0x02)
             * {
             *      unknown                                     12                  // TO BE DETEREMINED
             * }
             * if (otherAttributeFlag & 0x04)
             * {
             *      unknown                                     1                   // TO BE DETEREMINED
             * }
             * 
             * skipResource                                     2                   Bool type. Not sure why it's 2 bits.
             * if (!skipResource)
             * {
             *      resourceId                                  16                  Like statId, refers to some value in an excel table.
             * }
             * 
             * repeatFlag                                       1                   Bool type.
             * {
             *      repeatCount                                 10                  Number of times to read in stat values.
             * }
             * 
             * for (number of repeats)                                              If the repeatFlag is 0, then obviously we still want to read
             * {                                                                    in at least once... So really it's like a do {} while() chunk.
             *      for (number of extra attributes)
             *      {
             *          extraAttributeValue                     bitCount_EA         The extra attribute for the applicable value below.
             *      }
             *      
             *      statValue                                   bitCount            The actual stat value.
             * }
             */

            saveBuffer.WriteBits(stat.id, 16);

            saveBuffer.WriteBits(stat.attributes.Count, 2);
            for (int i = 0; i < stat.attributes.Count; i++)
            {
                saveBuffer.WriteBits(stat.attributes[i].exists, 1);
                if (stat.attributes[i].exists == 0)
                {
                    break;
                }

                saveBuffer.WriteBits(stat.attributes[i].BitCount, 6);

                saveBuffer.WriteBits(stat.attributes[i].Unknown1, 2);
                if (stat.attributes[i].Unknown1 == 0x02)
                {
                    saveBuffer.WriteBits(stat.attributes[i].Unknown1_1, 1);
                }

                saveBuffer.WriteBits(stat.attributes[i].skipTableId, 1);
                if (!stat.attributes[i].SkipTableId)
                {
                    saveBuffer.WriteBits(stat.attributes[i].TableId, 16);
                }
            }

            saveBuffer.WriteBits(stat.bitCount, 6);

            saveBuffer.WriteBits(stat.otherAttributeFlag, 3);
            if ((stat.otherAttributeFlag & 0x01) > 0)
            {
                saveBuffer.WriteBits(stat.otherAttribute.unknown1, 4);
            }
            if ((stat.otherAttributeFlag & 0x02) > 0)
            {
                saveBuffer.WriteBits(stat.otherAttribute.unknown2, 12);
            }
            if ((stat.otherAttributeFlag & 0x04) > 0)
            {
                saveBuffer.WriteBits(stat.otherAttribute.unknown3, 1);
            }

            saveBuffer.WriteBits(stat.skipResource, 2);
            if (stat.skipResource == 0)
            {
                saveBuffer.WriteBits(stat.resource, 16);
            }

            int repeatCount = 1;
            saveBuffer.WriteBits(stat.repeatFlag, 1);
            if (stat.repeatFlag == 1)
            {
                saveBuffer.WriteBits(stat.repeatCount, 10);
                repeatCount = stat.repeatCount;
            }

            for (int i = 0; i < repeatCount; i++)
            {
                for (int j = 0; j < stat.attributes.Count; j++)
                {
                    if (stat.attributes[j].exists == 1)
                    {
                        int extraAttribute = 0;
                        if (j == 0)
                            extraAttribute = stat.values[i].Attribute1;
                        else if (j == 1)
                            extraAttribute = stat.values[i].Attribute2;
                        else if (j == 2)
                            extraAttribute = stat.values[i].Attribute3;

                        saveBuffer.WriteBits(extraAttribute, stat.attributes[j].BitCount);
                    }
                }

                saveBuffer.WriteBits(stat.values[i].Stat, stat.bitCount);
            }
        }

        private static bool TestBit(int bitField, int bitOffset)
        {
            if ((bitField & (1 << bitOffset)) == 0)
                return false;

            return true;
        }
    }
}