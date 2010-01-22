using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reanimator
{
    // pretty sure this is populated based on a bit field... should check...
    public struct UnitStat_OtherAttribute
    {
        // if (otherAttributeFlag & 0x01)
        public int unknown1;										// 4 bits
        // if (otherAttributeFlag & 0x02)
        public int unknown2;										// 12 bits
        // if (otherAttributeFlag & 0x04)
        public int unknown3;										// 1 bit		// possibly another reasource flag or something - if not 0x01 alert
    };

    public struct UnitStat_ExtraAttribute
    {
        public int exists;											// 1 bit
        public int bitCount;										// 6 bits
        public int unknown1;										// 2 bits
        public int unknown1_1;										// 1 bit		// if unknown1 == 2
        public int skipResource;									// 1 bit		// if this is set, then don't read the resource below
        public int resource;										// 16 bits		// i think this is a resource thingy anyways...
    };

    public struct StatValues
    {
        public int[] extraAttributeValues;
        public int value;
    };

    public struct UnitStat
    {
        public int statId;											// 16 bits
        public int extraAttributesCount;							// 2 bits
        public UnitStat_ExtraAttribute[] extraAttributes;			                // no idea what they do though
        public int bitCount;									    // 6 bits		// size in bits of extra attribute
        public int otherAttributeFlag;								// 3 bits		// i think that's what this is...		// can be 0x00, 0x01, 0x02, 0x03, 0x04
        public UnitStat_OtherAttribute otherAttribute;
        public int skipResource;									// 2 bits		// resouce flag I think...
        public int resource;										// 16 bits		// like in extra attributes - read if above is 0x00
        public int repeatFlag;										// 1 bit		// if set, check for repeat number
        public int repeatCount;										// 10 bits		// i think this can be something other than 10 bits... I think...

        public StatValues[] values;
    };

    public struct UnitStatAdditional
    {
        public int unknown;											// 16 bits
        public int statCount;										// 16 bits
        public UnitStat[] stats;
    };

    public struct UnitStatName
    {
        public int unknown1;										// 16 bits
        public UnitStatBlock statBlock;								// nameCount * UnitStatBlock stuffs
    };

    public struct UnitStatBlock
    {
        public int statVersion;										// 16 bits
        public int unknown1;										// 3 bits		// untested - alert if != 0
        public int additionalStatCount;								// 6 bits
        public UnitStatAdditional[] additionalStats;

        public int statCount;										// 16 bits
        public UnitStat[] stats;

        public int nameCount;										// 8 bits		// i think this has something to do with item affix/prefix naming
        public UnitStatName[] names;
    };

    public struct UnknownCount1_S
    {
        public int unknown1;										// 16 bits
        public int unknown2;										// 16 bits
    };

    public struct UnknownCount3_S
    {
        public int unknown1;										// 16 bits
        public int unknown2;										// 32 bits
    };

    public struct UnitAppearance
    {
        public struct UnknownCount1_S
        {
            // if (bitTest(bitField1, 0x0F)) // untested
            public int unknown1;									// 32 bits

            public int unknown2;									// 16 bits

            // if (bitTest(bitField1, 0x00)) // if (bitField1 & 0x01)
            public int unknownCount1;								// 3 bits		// alert if != 0 (not encountered yet)
            public int[] unknownCount1s;						    // 32 bits * unknownCount1
        };

        public struct ModelAppearance_S
        {
            public int body;										// 16 bits
            public int head;										// 16 bits
            public int hair;										// 16 bits
            public int faceAccessory;								// 16 bits
        };

        public struct GearAppearance_S
        {
            public int gear;										// 16 bits
            public int unknownBool;									// 1 bit
            public int unknownBoolValue;						// 2 bits
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
        public ModelAppearance_S modelAppearance;						    // 16 bits * modelAppearanceCounter

        public int unknownCount3;									// 4 bits
        public int[] unknownCount3s;						        // 8 bits * unknownCount3

        // if (testBit(pUnit->bitField1, 0x10))
        public int gearCount;										// 16 bits
        public GearAppearance_S[] gears;				        	// 17 bits * gearCount
    };

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

    public struct Unit
    {
        ////// Start of read inside main header check function (in ASM) //////

        public int majorVersion;							    	// 16 bits
        public int minorVersion;							    	// 8

        public int bitFieldCount;									// 8			// must be <= 2
        public int bitField1;										// 32
        public int bitField2;										// 32

        // if (testBit(unit->bitField1, 0x1D))
        public int bitCountEOF;										// 32			// i think...

        // if (testBit(unit->bitField1, 0x00))
        public int beginFlag;										// 32			// must be "Flag" (67616C46h) or Can be "`4R+" ("60 34 52 2B", 2B523460h)

        // if (testBit(unit->bitField1, 0x1C))
        public int timeStamp1;										// 32			// i don't think these are actually time stamps
        public int timeStamp2;										// 32			// but since they change all the time and can be
        public int timeStamp3;										// 32			// set to 00 00 00 00 and it'll still load... it'll do

        // if (testBit(unit->bitField1, 0x1F))
        public int unknownCount1;									// 4
        public UnknownCount1_S[] unknownCount1s;                                    // no idea wtf these do

        // if (testBit(unit->bitField2, 0x00)					                    // char state flags (e.g. "elite")
        public int playerFlagCount1;								// 8
        public int[] playerFlags1;                                  // 16 * playerFlagCount1					

        ////// End of read inside main header check function (in ASM) //////

        // if (testBit(unit->bitField1, 0x1B))
        public int unknownCount3;									// 5
        public UnknownCount3_S[] unknownCount3s;			                        // no idea wtf these do either

        // if (testBit(unit->bitField1, 0x05)) // (bitField1 & 0x20)  (haven't encountered save file with this yet)
        public int unknownFlag;										// 4			// this value > e.g. 0x03 -> (0x3000000...00 & unknownFlagValue) or something like that
        public int unknownFlagValue;								// 16

        // if (testBit(bitField1, 0x17)) // 64 bits read as 8x8 bytes from non-standard bit read function
        public byte[] unknown1;

        // if (testBit(bitField1, 0x03) || testBit(bitField1, 0x01)) // if (bitField1 & 0x08 || bitField1 & 0x02)
        // {
        public int unknownBool2;									// 1 bit
        // {
        // if (testBit(bitField1, 0x02)) // if (bitField & 0x04)
        public int unknown11;										//32 bits			// untested

        public int unknown7;										// 16 bits
        public int unknown8;										// 12 bits
        public int unknown9;										// 12 bits
        // }

        public byte[] unknown10;								    // 8*8 bits (64) - non-standard func
        // }

        // if (testBit(bitField1, 0x06)) // if (bitField1 & 0x40) // does more reading, but can't test
        public int unknownBool3;									// 1 bit					// alert if != 1

        // if (testBit(bitField1, 0x09))
        public int unknown12;										// 8 bits

        // if (testBit(bitField1, 0x07)) // if (bitField1 & 0x80)
        public int jobClass;										// 8 bits		// i think...
        public int unknown2;										// 8 bits		// this appears to be joined with jobClass to form a WORD... I think...

        // if (testBit(bitField1, 0x08))
        public int charCount;										// 8 bits
        public Char[] szName;                                                       // character name - why does name change when change filename though?								

        // if (testBit(bitField1, 0x0A))						                    // char state flags (e.g. "elite")
        public int playerFlagCount2;								// 8 bits
        public int[] playerFlags2;							        // 16 bits * playerFlagCount2

        public int unknownBool1;									// 1 bit		// as above - alert if != 0

        // if (testBit(bitField1, 0x0D))
        public UnitStatBlock statBlock;

        public int hasAppearanceDetails;							// 1
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
        public int itemBitOffset;									// 32 bits		// missed what it did with it
        public int itemCount;										// 10 bits
        public Unit[] items;													    // each item is just a standard data block

        // if (testBit(pUnit->bitField1, 0x1A))
        public uint weaponConfigFlag;                               // 32 bits      // must be 0x91103A74; always present
        public int endFlagBitOffset;                                // 32 bits      // offset to end of file flag
        public int weaponConfigCount;                               // 6 bits       // weapon config count
        public UnitWeaponConfig[] weaponConfigs;                                // i think this has item positions on bottom bar, etc as well

        // if (testBit(unit->bitField1, 0x00))
        public int endFlag;											// 32 bits
    }
}
