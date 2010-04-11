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
                    return index >= attributes.Count ? null : attributes[index];
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

            public int statVersion;										    // 16 bits
            public int unknown1;										    // 3 bits		// untested - alert if != 0
            public int additionalStatCount;								    // 6 bits
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

        [Serializable]
        private class UnitBitOffsets
        {
            internal int index;                                         // 16           // only seen as 0x3030 ('00' in ascii - assuming as "0th" index) - alert if otherwise
            internal int offset;                                        // 32           // only seen as offset to end of items bit offset
        }

        public Unit(BitBuffer bb)
        {
            _bitBuffer = bb;
            PlayerFlags1 = new List<int>();
            PlayerFlags2 = new List<int>();
            _bitOffsets = new List<UnitBitOffsets>();
            Items = new List<Unit>();
        }

        public StatBlock Stats
        {
            get { return statBlock; }
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

        int _majorVersion;							    	            // 16 bits
        int _minorVersion;							    	            // 8

        int _bitFieldCount;									            // 8			// must be <= 2
        public int bitField1;										    // 32
        public int bitField2;										    // 32

        // if (testBit(unit->bitField1, 0x1D))
        public int bitCount;										    // 32			// of unit block

        // if (testBit(unit->bitField1, 0x00))
        int _beginFlag;										            // 32			// must be "Flag" (67616C46h) or Can be "`4R+" ("60 34 52 2B", 2B523460h)

        // if (testBit(unit->bitField1, 0x1C))
        public int timeStamp1;										    // 32			// i don't think these are actually time stamps
        public int timeStamp2;										    // 32			// but since they change all the time and can be
        public int timeStamp3;										    // 32			// set to 00 00 00 00 and it'll still load... it'll do

        // if (testBit(unit->bitField1, 0x1F))
        public int unknownCount1F;									    // 4
        public UnknownCount1F_S[] unknownCount1Fs;                                      // no idea wtf these do

        // if (testBit(unit->bitField2, 0x00)					                        // char state flags (e.g. "elite")
        int _playerFlagCount1;								            // 8
        public List<int> PlayerFlags1 { get; set; }             // 16 * _playerFlagCount1					

        ////// End of read inside main header check function (in ASM) //////

        // if (testBit(unit->bitField1, 0x1B))
        private int _bitOffsetCount;                                    // 5            // only ever seen as 0 or 1 - alert if otherwise
        private readonly List<UnitBitOffsets> _bitOffsets;                              // only seen with item end bit offset in it

        // if (testBit(unit->bitField1, 0x05)) // (bitField1 & 0x20)                    // haven't encountered file with this yet
        // ALERT

        public int unitType;										    // 4		    // 1 = character, 2 = monster (e.g. engineer drone), 3 = ?, 4 = item
        public int unitCode;								            // 16           // the code identifier of the unit (e.g. Job Class, or Item Id)

        // if (testBit(bitField1, 0x17)) // 64 bits read as 8x8 chunk bits from non-standard bit read function
        public byte[] unitUniqueId;                                     // 64           // a unique id identifiying this unit "structure"

        // if (testBit(bitField1, 0x03) || testBit(bitField1, 0x01)) // if (bitField1 & 0x08 || bitField1 & 0x02)
        // {
        public int unknownBool_01_03;								    // 1 bit
        // {
        // if (testBit(bitField1, 0x02)) // if (bitField & 0x04)
        public int unknown_02;										    // 32			// untested

        public int inventoryType;        								// 16           // i think...
        public int inventoryPositionX;									// 12           // x-position of inventory item
        public int inventoryPositionY;									// 12           // y-position of inventory item
        // }

        public byte[] unknown_01_03_4;								    // 8*8 bits (64) - non-standard func
        // }

        // if (testBit(bitField1, 0x06)) // if (bitField1 & 0x40) // does more reading, but can't test
        public int unknownBool_06;									    // 1            // alert if != 1

        // if (testBit(bitField1, 0x09))
        public int unknown_09;										    // 8

        // if (testBit(bitField1, 0x07)) // if (bitField1 & 0x80)
        // {
        int _characterHeight;									        // 8		    // Unsigned - Ranges from 1-255
        public int CharacterHeight
        {
            get { return _characterHeight; }
            set
            {
                if (value < 0 || value > 255) _characterHeight = 125;
                else  _characterHeight = value;
            }
        }

        int _characterWidth;									        // 8 	    	// Unsigned - Ranges from 1-255
        public int CharacterWidth
        {
            get { return _characterWidth; }
            set
            {
                if (value < 0 || value > 255) _characterWidth = 125;
                else _characterWidth = value;
            }

        }
        // }

        // if (testBit(bitField1, 0x08))
        // {
        int _charNameCount;									            // 8            // count of characters of following character name var
        char[] characterName;                                                           // character name - why does name change when change filename though?
        // }

        // if (testBit(bitField1, 0x0A))						                        // char state flags (e.g. "elite")
        int _playerFlagCount2;								            // 8            // count of flags PlayerFlags2
        public List<int> PlayerFlags2 { get; set; }				        // 16  * _playerFlagCount2

        public int unknownBool1;									    // 1    		// as above - alert if != 0

        // if (testBit(bitField1, 0x0D))
        public StatBlock statBlock;

        public int hasAppearanceDetails;							    // 1
        public UnitAppearance unitAppearance;
        /*
        // {
        BYTE unknownCount7;										        // 3 
        std::vector<UnknownCount7_S> unknownCount7s;

        DWORD unknown5;											        // 16 

        //// if (bitTest(bitField1, 0x16))
            BYTE unknown6[8];                                                           // non-standard read in again

        //// if (bitTest(bitField1, 0x11))
            BYTE unknownCount8;										    // 4 
            std::vector<WORD> unknownCount8s;						    // 16  * unknownCount8

            BYTE modelAppearanceCounter;							    // 3 
            std::vector<WORD> modelAppearance;						    // 16  * modelAppearanceCounter

            BYTE unknownCount9;										    // 4 
            std::vector<BYTE> unknownCount9s;						    // 8  * unknownCount9

        //// if (140267C48 test    byte ptr [r9+46h], 1) // 0xE7 part
            WORD gearCount;										        // 16 
            std::vector<Gear_S> gears;							        // 17  * gearCount
        // }*/

        // if (testBit(pUnit->bitField1, 0x12))
        int _itemEndBitOffset;									        // 32           // bit offset to end of items block
        int _itemCount;										            // 10 
        public List<Unit> Items;                                                        // each item is just a standard data block

        // if (testBit(pUnit->bitField1, 0x1A))
        uint _weaponConfigFlag;                                         // 32           // must be 0x91103A74; always present
        int _endFlagBitOffset;                                          // 32           // offset to end of file flag
        int _weaponConfigCount;                                         // 6            // weapon config count
        public UnitWeaponConfig[] weaponConfigs;                                        // i think this has item positions on bottom bar, etc as well

        // if (testBit(unit->bitField1, 0x00))
        int _endFlag;											        // 32           // end of unit flag

        [NonSerialized] readonly BitBuffer _bitBuffer;

        ///////////////////// Function Definitions /////////////////////

        /// <summary>
        /// Reads a the Unit structure from the local BitBuffer.
        /// </summary>
        /// <param name="unit">Unit to be read into.</param>
        /// <returns>True on success.</returns>
        public bool ReadUnit(ref Unit unit)
        {
            unit._majorVersion = _bitBuffer.ReadBits(16);
            if (unit._majorVersion != 0x00BF)
            {
                MessageBox.Show("Error! Invalid Unit Major Version: " + unit._majorVersion.ToString("X4"));
                return false;
            }
            unit._minorVersion = _bitBuffer.ReadBits(8);
            if (unit._minorVersion != 0x00)
            {
                MessageBox.Show("Warning! Untested Unit Minor Version: " + unit._minorVersion.ToString("X2"));
            }


            unit._bitFieldCount = _bitBuffer.ReadBits(8);
            if (unit._bitFieldCount >= 1)
            {
                unit.bitField1 = _bitBuffer.ReadBits(32);
            }
            if (unit._bitFieldCount == 2)
            {
                unit.bitField2 = _bitBuffer.ReadBits(32);
            }


            if (TestBit(unit.bitField1, 0x1D))
            {
                unit.bitCount = _bitBuffer.ReadBits(32);
            }


            if (TestBit(unit.bitField1, 0x00))
            {
                unit._beginFlag = _bitBuffer.ReadBits(32);
            }


            if (TestBit(unit.bitField1, 0x1C))
            {
                unit.timeStamp1 = _bitBuffer.ReadBits(32);
                unit.timeStamp2 = _bitBuffer.ReadBits(32);
                unit.timeStamp3 = _bitBuffer.ReadBits(32);
            }


            if (TestBit(unit.bitField1, 0x1F))
            {
                unit.unknownCount1F = _bitBuffer.ReadBits(4) - 8;
                unit.unknownCount1Fs = new UnknownCount1F_S[unit.unknownCount1F];

                for (int i = 0; i < unit.unknownCount1F; i++)
                {
                    UnknownCount1F_S uc1;

                    uc1.unknown1 = _bitBuffer.ReadBits(16);
                    uc1.unknown2 = _bitBuffer.ReadBits(16);

                    unit.unknownCount1Fs[i] = uc1;
                }
            }


            if (TestBit(unit.bitField2, 0x00))
            {
                unit._playerFlagCount1 = _bitBuffer.ReadBits(8);
                for (int i = 0; i < unit._playerFlagCount1; i++)
                {
                    unit.PlayerFlags1.Add(_bitBuffer.ReadBits(16));
                }
            }


            if (TestBit(unit.bitField1, 0x1B))
            {
                unit._bitOffsetCount = _bitBuffer.ReadBits(5);
                if (unit._bitOffsetCount > 1)
                {
                    MessageBox.Show("Unexpected unit._bitOffsetCount (> 1)!\nNot-Implemented cases. Please report this error and supply the offending file.",
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                for (int i = 0; i < unit._bitOffsetCount; i++)
                {
                    UnitBitOffsets bitOffsets = new UnitBitOffsets
                                                    {
                                                        index = _bitBuffer.ReadBits(16),
                                                        offset = _bitBuffer.ReadBits(32)
                                                    };

                    if (bitOffsets.index != 0x3030) // '00'
                    {
                        MessageBox.Show("Unexpected value for bitOffsets.index (!= 0x3030)!\nNot-Implemented cases. Please report this error and supply the offending file.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    unit._bitOffsets.Add(bitOffsets);
                }
            }


            if (TestBit(unit.bitField1, 0x05))
            {
                MessageBox.Show(
                    "Unexpected bit case for unit._bitField1 (0x05 = true)!\nNot-Implemented cases. Please report this error and supply the offending file.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


            unit.unitType = _bitBuffer.ReadBits(4);
            if (unit.unitType != 1 && unit.unitType != 2 && unit.unitType != 4)
            {
                MessageBox.Show(
                    "Unexpected value for unit.unitType (!= 1, 2, 4)!\nNot-Implemented cases. Please report this warning and supply the offending file.",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            unit.unitCode = _bitBuffer.ReadBits(16);


            if (TestBit(unit.bitField1, 0x17))
            {
                unit.unitUniqueId = ReadNonStandardFunc();
            }


            if (TestBit(unit.bitField1, 0x03) || TestBit(unit.bitField1, 0x01))
            {
                unit.unknownBool_01_03 = _bitBuffer.ReadBits(1);
                if (unit.unknownBool_01_03 > 0)
                {
                    if (TestBit(unit.bitField1, 0x02))
                    {
                        unit.unknown_02 = _bitBuffer.ReadBits(32);
                    }

                    unit.inventoryType = _bitBuffer.ReadBits(16);
                    unit.inventoryPositionX = _bitBuffer.ReadBits(12);
                    unit.inventoryPositionY = _bitBuffer.ReadBits(12);
                }

                unit.unknown_01_03_4 = ReadNonStandardFunc();
            }

            if (TestBit(unit.bitField1, 0x06))
            {
                unit.unknownBool_06 = _bitBuffer.ReadBits(1);
                if (unit.unknownBool_01_03 != 1)
                {
                    return false;
                }
            }

            // On items only I think - Usually 0x00
            if (TestBit(unit.bitField1, 0x09))
            {
                unit.unknown_09 = _bitBuffer.ReadBits(8);
            }

            // On main character unit only
            if (TestBit(unit.bitField1, 0x07))
            {
                unit.CharacterHeight = _bitBuffer.ReadBits(8);
                unit.CharacterWidth = _bitBuffer.ReadBits(8);
            }

            // On main character unit only
            if (TestBit(unit.bitField1, 0x08))
            {
                unit._charNameCount = _bitBuffer.ReadBits(8);
                if (unit._charNameCount > 0)
                {
                    unit.characterName = new Char[unit._charNameCount];
                    for (int i = 0; i < unit._charNameCount; i++)
                    {
                        unit.characterName[i] = (Char)_bitBuffer.ReadBits(16);
                    }
                }
            }

            // On both the main char and on items - appears to be always zero for items
            if (TestBit(unit.bitField1, 0x0A))
            {
                unit._playerFlagCount2 = _bitBuffer.ReadBits(8);
                if (unit._playerFlagCount2 > 0)
                {
                    for (int i = 0; i < unit._playerFlagCount2; i++)
                    {
                        unit.PlayerFlags2.Add(_bitBuffer.ReadBits(16));
                    }
                }
            }


            unit.unknownBool1 = _bitBuffer.ReadBits(1);
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


            unit.hasAppearanceDetails = _bitBuffer.ReadBits(1);
            if (unit.hasAppearanceDetails == 1)
            {
                unit.unitAppearance = new UnitAppearance();
                if (!ReadAppearance(ref unit, ref unit.unitAppearance))
                    return false;
            }


            if (TestBit(unit.bitField1, 0x12))
            {
                unit._itemEndBitOffset = _bitBuffer.ReadBits(32);
                unit._itemCount = _bitBuffer.ReadBits(10);
                for (int i = 0; i < unit._itemCount; i++)
                {
                    Unit item = new Unit(_bitBuffer);

                    if (!ReadUnit(ref item))
                        return false;

                    unit.Items.Add(item);
                }
            }


            if (TestBit(unit.bitField1, 0x1A))
            {
                unit._weaponConfigFlag = (uint)_bitBuffer.ReadBits(32);
                if (unit._weaponConfigFlag != 0x91103A74)
                {
                    MessageBox.Show("Flags not aligned!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                unit._endFlagBitOffset = _bitBuffer.ReadBits(32);     // to end flag

                unit._weaponConfigCount = _bitBuffer.ReadBits(6);
                unit.weaponConfigs = new UnitWeaponConfig[unit._weaponConfigCount];
                for (int i = 0; i < unit._weaponConfigCount; i++)
                {
                    UnitWeaponConfig weaponConfig = new UnitWeaponConfig
                                                        {
                                                            id = _bitBuffer.ReadBits(16),
                                                            unknownCount1 = _bitBuffer.ReadBits(4)
                                                        };

                    if (weaponConfig.unknownCount1 != 0x02)
                    {
                        MessageBox.Show("if (weaponConfig.unknownCount1 != 0x02)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    weaponConfig.exists1 = new int[2];
                    weaponConfig.unknownIds1 = new int[2];
                    for (int j = 0; j < weaponConfig.unknownCount1; j++)
                    {
                        weaponConfig.exists1[j] = _bitBuffer.ReadBits(1);
                        if (weaponConfig.exists1[j] == 1)
                        {
                            weaponConfig.unknownIds1[j] = _bitBuffer.ReadBits(32); // under some condition this will be ReadFromOtherFunc thingy
                        }
                    }

                    // yes this chunk looks the same as above - the above chunk though is in a specific function and can differ at 1 point
                    // also, it can be != 2
                    weaponConfig.unknownCount2 = _bitBuffer.ReadBits(4);
                    weaponConfig.exists2 = new int[weaponConfig.unknownCount2];
                    weaponConfig.unknownIds2 = new int[weaponConfig.unknownCount2];
                    for (int j = 0; j < weaponConfig.unknownCount2; j++)
                    {
                        weaponConfig.exists2[j] = _bitBuffer.ReadBits(1);
                        if (weaponConfig.exists2[j] == 1)
                        {
                            weaponConfig.unknownIds2[j] = _bitBuffer.ReadBits(32);
                        }
                    }

                    weaponConfig.idAnother = _bitBuffer.ReadBits(32); // read from 0x17 file          // 0x3931 -> 0x4B

                    unit.weaponConfigs[i] = weaponConfig;
                }
            }


            if (TestBit(unit.bitField1, 0x00))
            {
                unit._endFlag = _bitBuffer.ReadBits(32);
                if (unit._endFlag != unit._beginFlag && unit._endFlag != 0x2B523460)
                {
                    int bitOffset = unit.bitCount - _bitBuffer.DataBitOffset;
                    int byteOffset = (_bitBuffer.Length - _bitBuffer.DataByteOffset) - (_bitBuffer.DataBitOffset >> 3);
                    MessageBox.Show("Flags not aligned!\nBit Offset: " + _bitBuffer.DataBitOffset + "\nExpected: " + unit.bitCount + " (+" + bitOffset +
                        ")\nByte Offset: " + (_bitBuffer.DataBitOffset >> 3) + "\nExpected: " + (_bitBuffer.Length - _bitBuffer.DataByteOffset) + " (+" + byteOffset + ")",
                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }


            return true;
        }

        private bool ReadStatBlock(ref StatBlock statBlock, bool readNameCount)
        {
            statBlock.statVersion = _bitBuffer.ReadBits(16);
            if (statBlock.statVersion != 0x0A)
            {
                MessageBox.Show("Error! Unknown Stat Block Version: " + statBlock.statVersion.ToString("X2"));
                return false;
            }
            statBlock.unknown1 = _bitBuffer.ReadBits(3);
            statBlock.additionalStatCount = _bitBuffer.ReadBits(6);
            statBlock.additionalStats = new UnitStatAdditional[statBlock.additionalStatCount];

            for (int i = 0; i < statBlock.additionalStatCount; i++)
            {
                UnitStatAdditional additionalStat = new UnitStatAdditional();

                additionalStat.unknown = _bitBuffer.ReadBits(16);
                additionalStat.statCount = _bitBuffer.ReadBits(16);
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

            statBlock.statCount = _bitBuffer.ReadBits(16);
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

            statBlock.nameCount = _bitBuffer.ReadBits(8);
            statBlock.names = new UnitStatName[statBlock.nameCount];

            for (int i = 0; i < statBlock.nameCount; i++)
            {
                UnitStatName name = new UnitStatName();

                name.unknown1 = _bitBuffer.ReadBits(16);
                name.statBlock = new StatBlock();

                if (!ReadStatBlock(ref name.statBlock, false))
                    return false;

                statBlock.names[i] = name;
            }

            return true;
        }

        private bool ReadStat(ref StatBlock.Stat unitStat)
        {
            unitStat.id = _bitBuffer.ReadBits(16);
            unitStat.attributesCount = _bitBuffer.ReadBits(2);
            //unitStat.attributes = new StatBlock.Stat.Attribute[unitStat.attributesCount];

            for (int i = 0; i < unitStat.attributesCount; i++)
            {
                StatBlock.Stat.Attribute exAtrib = new StatBlock.Stat.Attribute();

                exAtrib.exists = _bitBuffer.ReadBits(1);
                if (exAtrib.exists != 1)
                    break;

                exAtrib.BitCount = _bitBuffer.ReadBits(6);

                exAtrib.Unknown1 = _bitBuffer.ReadBits(2);
                if (exAtrib.Unknown1 == 0x02)
                {
                    exAtrib.Unknown1_1 = _bitBuffer.ReadBits(1);
                }

                exAtrib.skipTableId = _bitBuffer.ReadBits(1);
                if (!exAtrib.SkipTableId)
                {
                    exAtrib.TableId = _bitBuffer.ReadBits(16);
                }

                unitStat.attributes.Add(exAtrib);
            }

            unitStat.bitCount = _bitBuffer.ReadBits(6);

            unitStat.otherAttributeFlag = _bitBuffer.ReadBits(3);
            if ((unitStat.otherAttributeFlag & 0x01) >= 1)
            {
                unitStat.otherAttribute.unknown1 = _bitBuffer.ReadBits(4);
            }
            if ((unitStat.otherAttributeFlag & 0x02) >= 1)
            {
                unitStat.otherAttribute.unknown2 = _bitBuffer.ReadBits(12);
            }
            if ((unitStat.otherAttributeFlag & 0x04) >= 1)
            {
                unitStat.otherAttribute.unknown3 = _bitBuffer.ReadBits(1);
                if (unitStat.otherAttribute.unknown3 != 0x01)
                {
                    return false;
                }
            }

            unitStat.skipResource = _bitBuffer.ReadBits(2);
            if (unitStat.skipResource == 0)
            {
                unitStat.resource = _bitBuffer.ReadBits(16);
            }

            unitStat.repeatFlag = _bitBuffer.ReadBits(1);
            unitStat.repeatCount = 1;
            if (unitStat.repeatFlag == 1)
            {
                unitStat.repeatCount = _bitBuffer.ReadBits(10);
            }
            unitStat.values = new StatBlock.Stat.Values[unitStat.repeatCount];

            for (int i = 0; i < unitStat.repeatCount; i++)
            {
                unitStat.values[i] = new StatBlock.Stat.Values();
                for (int j = 0; j < unitStat.attributesCount; j++)
                {
                    if (unitStat.attributes[j].exists == 1)
                    {
                        int extraAttribute = _bitBuffer.ReadBits(unitStat.attributes[j].BitCount);
                        if (j == 0)
                            unitStat.values[i].Attribute1 = extraAttribute;
                        if (j == 1)
                            unitStat.values[i].Attribute2 = extraAttribute;
                        if (j == 2)
                            unitStat.values[i].Attribute3 = extraAttribute;
                    }
                }

                unitStat.values[i].Stat = _bitBuffer.ReadBits(unitStat.bitCount);
            }

            return true;
        }

        private bool ReadAppearance(ref Unit heroUnit, ref UnitAppearance appearance)
        {
            appearance.unknownCount1 = _bitBuffer.ReadBits(3);
            appearance.unknownCount1s = new UnitAppearance.UnknownCount1_S[appearance.unknownCount1];
            for (int i = 0; i < appearance.unknownCount1; i++)
            {
                UnitAppearance.UnknownCount1_S uc1 = new UnitAppearance.UnknownCount1_S();

                if (TestBit(heroUnit.bitField1, 0x0F)) // untested
                {
                    uc1.unknown1 = _bitBuffer.ReadBits(32);
                }

                uc1.unknown2 = _bitBuffer.ReadBits(16);

                if (TestBit(heroUnit.bitField1, 0x00))
                {
                    uc1.unknownCount1 = _bitBuffer.ReadBits(3);
                    uc1.unknownCount1s = new int[uc1.unknownCount1];
                    for (int j = 0; j < uc1.unknownCount1; j++)
                    {
                        uc1.unknownCount1s[j] = _bitBuffer.ReadBits(32);
                    }
                }

                appearance.unknownCount1s[i] = uc1;
            }


            appearance.unknown1 = _bitBuffer.ReadBits(16);


            if (TestBit(heroUnit.bitField1, 0x16))
            {
                appearance.unknown2 = ReadNonStandardFunc();
            }


            if (TestBit(heroUnit.bitField1, 0x11))
            {
                appearance.unknownCount2 = _bitBuffer.ReadBits(4);
                appearance.unknownCount2s = new int[appearance.unknownCount2];
                for (int i = 0; i < appearance.unknownCount2; i++)
                {
                    appearance.unknownCount2s[i] = _bitBuffer.ReadBits(16);
                }

                appearance.modelAppearanceCounter = _bitBuffer.ReadBits(3);
                for (int i = 0; i < appearance.modelAppearanceCounter; i++)
                {
                    int modelAppearance = _bitBuffer.ReadBits(16);
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

                appearance.unknownCount3 = _bitBuffer.ReadBits(4);
                appearance.unknownCount3s = new int[appearance.unknownCount3];
                for (int i = 0; i < appearance.unknownCount3; i++)
                {
                    appearance.unknownCount3s[i] = _bitBuffer.ReadBits(8);
                }
            }


            if (TestBit(heroUnit.bitField1, 0x10))
            {
                appearance.gearCount = _bitBuffer.ReadBits(16);
                appearance.gears = new UnitAppearance.GearAppearance_S[appearance.gearCount];
                for (int i = 0; i < appearance.gearCount; i++)
                {
                    UnitAppearance.GearAppearance_S gear = new UnitAppearance.GearAppearance_S();

                    gear.gear = _bitBuffer.ReadBits(16);
                    gear.unknownBool = _bitBuffer.ReadBits(1);
                    if (gear.unknownBool == 1)
                    {
                        gear.unknownBoolValue = _bitBuffer.ReadBits(2);
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
                ret[i] = (byte)_bitBuffer.ReadBits(8);
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
             *      bitOffsetsCount                             5                   Count of following block.
             *      {
             *          index                                   16                  Index value of offset (e.g. 0x3030 = item end bit offset)
             *          offset                                  32                  Bit Offset value
             *      }
             * }
             * 
             * if (TestBit(bitField1, 0x05))
             * {
             *      --                                          ??                  Never encountered.
             * }
             * 
             * unknownFlag                                      4                   This value > e.g. 0x03 -> (0x3000000...00 & unknownFlagValue)
             * unknownFlagValue                                 16                  or something like that anyways.
             * 
             * if (TestBit(bitField1, 0x17))
             * {                                                                    This chunk is read in by a secondary non-standard function.
             *      unitId[8]                                   64                  Contains a unique id for this unit object (e.g. item hash id to stop duping, etc).
             * }
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
             *      unknown[8]                                  64                  Non-standard function reading - unknown usage.
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
             *      unknown                                     8                   // TO BE DETERMINED
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
            int use_1B_BitOffsets = (1 << 0x1B);
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
            if (unit._bitOffsetCount > 0)
            {
                int bitOffsetsCount = unit._bitOffsets.Count;

                saveBuffer.WriteBits(bitOffsetsCount, 5);
                for (int i = 0; i < bitOffsetsCount; i++)
                {
                    saveBuffer.WriteBits(unit._bitOffsets[i].index, 16);
                    bitOffsetItemEndBitOffset = saveBuffer.DataBitOffset;
                    saveBuffer.WriteBits(0x00000000, 32);
                }

                bitField1 |= use_1B_BitOffsets;
                saveBuffer.WriteBits(bitField1, 32, bitField1Offset);
            }

            saveBuffer.WriteBits(unit.unitType, 4);
            saveBuffer.WriteBits(unit.unitCode, 16);

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
                saveBuffer.WriteBits(unit.CharacterHeight, 8);
                saveBuffer.WriteBits(unit.CharacterWidth, 8);
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

                int itemCount = unit.Items.Count;
                saveBuffer.WriteBits(itemCount, 10);
                for (int i = 0; i < itemCount; i++)
                {
                    WriteUnit(saveBuffer, unit.Items[i], true, null);
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

                int weaponConfigCount = unit.weaponConfigs.Length;
                saveBuffer.WriteBits(weaponConfigCount, 6);
                for (int i = 0; i < weaponConfigCount; i++)
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
             * _majorVersion                                     16                  Stat block header - Must be 0x000A.
             * _minorVersion                                     3                   Must be 0x0.
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

            if (stat.id == 25667) // difficulty_max
            {
                int bp = 1;
            }
            if (stat.id == 25155) // difficulty_current
            {
                int bp = 1;
            }

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