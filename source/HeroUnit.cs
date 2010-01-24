using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reanimator.Excel;

namespace Reanimator
{
    public class HeroUnitStat
    {
        public struct ExtraAttribute
        {
            public int unknownBool;
            public int resourceId;

            public string resourceString;

            public ExtraAttribute(UnitStat_ExtraAttribute ex, ExcelTables excelTables)
            {
                unknownBool = ex.unknown1_1;
                resourceId = ex.resource;

                resourceString = excelTables.Stats.GetStringFromId(resourceId);
            }

            public override string ToString()
            {
                return resourceString;
            }
        }

        int statId;
        ExtraAttribute[] extraAttributes;
        int otherAttribute1;
        int otherAttribute2;
        int otherAttribute3;
        int resourceId;
        List<StatValues> values;

        public string statString;

        public HeroUnitStat(UnitStat unitStat, ExcelTables excelTables)
        {
            statId = unitStat.statId;
            extraAttributes = new ExtraAttribute[unitStat.extraAttributesCount];
            for (int i = 0; i < unitStat.extraAttributesCount; i++)
            {
                extraAttributes[i] = new ExtraAttribute(unitStat.extraAttributes[i], excelTables);
            }
            otherAttribute1 = unitStat.otherAttribute.unknown1;
            otherAttribute2 = unitStat.otherAttribute.unknown2;
            otherAttribute3 = unitStat.otherAttribute.unknown3;
            resourceId = unitStat.resource;
            values = unitStat.values.ToList<StatValues>();

            statString = excelTables.Stats.GetStringFromId(statId);
        }

        public string StatString
        {
            get { return statString; }
            set { statString = value; }
        }

        public List<StatValues> Values
        {
            get { return values; }
        }

        public override string ToString()
        {
            return StatString;
        }
    }

    public class HeroUnit
    {
        BitBuffer bitBuffer;
        Unit heroUnit;

        HeroUnitStat[] stats;
   
        public HeroUnit(byte[] heroData, ExcelTables excelTables)
        {
            bitBuffer = new BitBuffer(heroData);
            bitBuffer.DataByteOffset = 0x2028;

            heroUnit = new Unit();
            ReadUnit(ref heroUnit);

            stats = new HeroUnitStat[heroUnit.statBlock.statCount];
            for (int i = 0; i < heroUnit.statBlock.statCount; i++)
            {
                stats[i] = new HeroUnitStat(heroUnit.statBlock.stats[i], excelTables);
            }
        }

        public HeroUnitStat[] Stats
        {
            get { return stats; }
        }

        public Unit[] Items
        {
            get
            {
                return heroUnit.items;
            }
        }

        private bool ReadUnit(ref Unit unit)
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
                unit.bitCountEOF = bitBuffer.ReadBits(32);
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
                unit.playerFlagCount1 = bitBuffer.ReadBits(8);
                unit.playerFlags1 = new int[unit.playerFlagCount1];

                for (int i = 0; i < unit.playerFlagCount1; i++)
                {
                    int flag = bitBuffer.ReadBits(16);

                    unit.playerFlags1[i] = flag;
                }
            }


            if (TestBit(unit.bitField1, 0x1B))
            {
                unit.unknownCount1B = bitBuffer.ReadBits(5);
                unit.unknownCount1Bs = new UnknownCount1B_S[unit.unknownCount1B];

                for (int i = 0; i < unit.unknownCount1B; i++)
                {
                    UnknownCount1B_S uc3;

                    uc3.unknown1 = bitBuffer.ReadBits(16);
                    uc3.unknown2 = bitBuffer.ReadBits(32);

                    unit.unknownCount1Bs[i] = uc3;
                }
            }


            if (TestBit(unit.bitField1, 0x05))
            {
                return false;
            }


            unit.unknownFlag = bitBuffer.ReadBits(4);
            unit.unknownFlagValue = bitBuffer.ReadBits(16);


            if (TestBit(unit.bitField1, 0x17))
            {
                unit.unknown17 = ReadNonStandardFunc();
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

                    unit.unknown_01_03_1 = bitBuffer.ReadBits(16);
                    unit.unknown_01_03_2 = bitBuffer.ReadBits(12);
                    unit.unknown_01_03_3 = bitBuffer.ReadBits(12);
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


            if (TestBit(unit.bitField1, 0x09))
            {
                unit.unknown_09 = bitBuffer.ReadBits(8);
            }


            if (TestBit(unit.bitField1, 0x07))
            {
                unit.jobClass = bitBuffer.ReadBits(8);
                unit.unknown_07 = bitBuffer.ReadBits(8);
            }


            if (TestBit(unit.bitField1, 0x08))
            {
                unit.characteCount = bitBuffer.ReadBits(8);
                if (unit.characteCount > 0)
                {
                    unit.characterName = new Char[unit.characteCount];
                    for (int i = 0; i < unit.characteCount; i++)
                    {
                        unit.characterName[i] = (Char)bitBuffer.ReadBits(16);
                    }
                }
            }

            if (TestBit(unit.bitField1, 0x0A))
            {
                unit.playerFlagCount2 = bitBuffer.ReadBits(8);
                if (unit.playerFlagCount2 > 0)
                {
                    unit.playerFlags2 = new int[unit.playerFlagCount2];
                    for (int i = 0; i < unit.playerFlagCount2; i++)
                    {
                        unit.playerFlags2[i] = bitBuffer.ReadBits(16);
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
                unit.statBlock = new UnitStatBlock();
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
                unit.itemBitOffset = bitBuffer.ReadBits(32);
                unit.itemCount = bitBuffer.ReadBits(10);
                unit.items = new Unit[unit.itemCount];
                for (int i = 0; i < unit.itemCount; i++)
                {
                    Unit item = new Unit();

                    if (!ReadUnit(ref item))
                        return false;

                    unit.items[i] = item;
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
                    UnitWeaponConfig weaponConfig;

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
                    int bitOffset = unit.bitCountEOF - bitBuffer.DataBitOffset;
                    int byteOffset = (bitBuffer.Length - bitBuffer.DataByteOffset) - (bitBuffer.DataBitOffset >> 3);
                    MessageBox.Show("Flags not aligned!\nBit Offset: " + bitBuffer.DataBitOffset + "\nExpected: " + unit.bitCountEOF + " (+" + bitOffset +
                        ")\nByte Offset: " + (bitBuffer.DataBitOffset >> 3) + "\nExpected: " + (bitBuffer.Length-bitBuffer.DataByteOffset) + " (+" + byteOffset + ")",
                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }


            return true;
        }

        private bool ReadStatBlock(ref UnitStatBlock statBlock, bool readNameCount)
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
                additionalStat.stats = new UnitStat[additionalStat.statCount];

                for (int j = 0; j < additionalStat.statCount; j++)
                {
                    UnitStat unitStat = new UnitStat();
                    if (!ReadStat(ref unitStat))
                        return false;

                    additionalStat.stats[j] = unitStat;

                }

                statBlock.additionalStats[i] = additionalStat;
            }

            statBlock.statCount = bitBuffer.ReadBits(16);
            statBlock.stats = new UnitStat[statBlock.statCount];
            for (int i = 0; i < statBlock.statCount; i++)
            {
                UnitStat unitStat = new UnitStat();
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
                name.statBlock = new UnitStatBlock();

                if (!ReadStatBlock(ref name.statBlock, false))
                    return false;

                statBlock.names[i] = name;
            }

            return true;
        }

        private bool ReadStat(ref UnitStat unitStat)
        {
            unitStat.statId = bitBuffer.ReadBits(16);
            unitStat.extraAttributesCount = bitBuffer.ReadBits(2);
            unitStat.extraAttributes = new UnitStat_ExtraAttribute[unitStat.extraAttributesCount];

            for (int i = 0; i < unitStat.extraAttributesCount; i++)
            {
                UnitStat_ExtraAttribute exAtrib = new UnitStat_ExtraAttribute();

                exAtrib.exists = bitBuffer.ReadBits(1);
                if (exAtrib.exists != 1)
                    break;

                exAtrib.bitCount = bitBuffer.ReadBits(6);

                exAtrib.unknown1 = bitBuffer.ReadBits(2);
                if (exAtrib.unknown1 == 0x02)
                {
                    exAtrib.unknown1_1 = bitBuffer.ReadBits(1);
                }

                exAtrib.skipResource = bitBuffer.ReadBits(1);
                if (exAtrib.skipResource == 0)
                {
                    exAtrib.resource = bitBuffer.ReadBits(16);
                }

                unitStat.extraAttributes[i] = exAtrib;
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
            unitStat.values = new StatValues[unitStat.repeatCount];

            for (int i = 0; i < unitStat.repeatCount; i++)
            {
                for (int j = 0; j < unitStat.extraAttributesCount; j++)
                {
                    if (unitStat.extraAttributes[j].exists == 1)
                    {
                        unitStat.values[i].extraAttributeValues = new int[unitStat.extraAttributesCount];
                        unitStat.values[i].extraAttributeValues[j] = bitBuffer.ReadBits(unitStat.extraAttributes[j].bitCount);
                    }
                }

                unitStat.values[i].value = bitBuffer.ReadBits(unitStat.bitCount);
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

        private bool TestBit(int bitField, int bitOffset)
        {
            if ((bitField & (1 << bitOffset)) == 0)
                return false;

            return true;
        }

        public byte[] GenerateSaveData()
        {
            BitBuffer saveBuffer = new BitBuffer();

            WriteUnit(saveBuffer, heroUnit);

            return saveBuffer.GetData();
        }

        private void WriteUnit(BitBuffer saveBuffer, Unit unit)
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
             *      bitCountEOF                                 32                  // TO CONFIRM
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
             *      unknownCount                                5                   // TO BE DETEREMINED
             *      {
             *          unknown1                                16                  // TO BE DETEREMINED
             *          unknown2                                32                  // TO BE DETEREMINED
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
             * }
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
             */

            // section chunk flags
            // the order is around what they're actually read/checked in loading
            int useBitCountEOF = (1 << 0x1D);
            int useFlagAlignment = 1;
            int useTimeStamps = (1 << 0x1C);
            int useUnknown_1F = (1 << 0x1F);
            int useCharacterFlags1 = 1;
            int useUnknown_1B = (1 << 0x1B);
            int useUnknown_17 = (1 << 0x17);
            int useUnknown_03 = 0; // (1 << 0x03);
            int useUnknown_01 = 0; // (1 << 0x01);
            int useUnknown_02 = 0; // (1 << 0x02);
            int useUnknown_06 = 0; // (1 << 0x06);
            int useUnknown_09 = 0; // (1 << 0x09);
            int useUnknown_07 = 0; // (1 << 0x07);
            int useUnknown_08 = 0; // (1 << 0x08);
            int useCharacterFlags2 = 0; // (1 << 0x0A);
            int useStats = 0; // (1 << 0x0D);

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
                useCharacterFlags2 = (1 << 0x0A);
            if (TestBit(unit.bitField1, 0x0D))
                useStats = (1 << 0x0D);

            // need to keep track of things as we go
            int bitField1 = 0x00000000;
            int bitField1Offset = -1;
            int bitField2 = 0x00000000;
            int bitField2Offset = -1;
            int bitCountEOFOffset = -1;

            /***** Unit Header *****/

            saveBuffer.WriteBits(0x00BF, 16);
            saveBuffer.WriteBits(0x00, 8);
            saveBuffer.WriteBits(0x02, 8);
            {
                bitField1Offset = saveBuffer.DataBitOffset;
                saveBuffer.WriteBits(bitField1, 32);
                bitField2Offset = saveBuffer.DataBitOffset;
                saveBuffer.WriteBits(bitField2, 32);
            }

            if (useBitCountEOF > 0)
            {
                bitCountEOFOffset = saveBuffer.DataBitOffset;
                saveBuffer.WriteBits(0x00000000, 32);
                bitField1 |= useBitCountEOF;
                saveBuffer.WriteBits(bitField1, 32, bitField1Offset);
            }

            if (useFlagAlignment > 0)
            {
                saveBuffer.WriteBits(0x67616C46, 32);
                bitField1 |= useFlagAlignment;
                saveBuffer.WriteBits(bitField1, 32, bitField1Offset);
            }

            if (unit.timeStamp1 != 0)
            {
                saveBuffer.WriteBits(0x00000000, 32);
                saveBuffer.WriteBits(0x00000000, 32);
                saveBuffer.WriteBits(0x00000000, 32);
                bitField1 |= useTimeStamps;
                saveBuffer.WriteBits(bitField1, 32, bitField1Offset);
            }

            if (unit.unknownCount1F != 0)
            {
                saveBuffer.WriteBits(unit.unknownCount1F+8, 4);
                for (int i = 0; i < unit.unknownCount1F; i++)
                {
                    UnknownCount1F_S uc1F = unit.unknownCount1Fs[i];
                    saveBuffer.WriteBits(uc1F.unknown1, 16);
                    saveBuffer.WriteBits(uc1F.unknown2, 16);
                }

                bitField1 |= useUnknown_1F;
                saveBuffer.WriteBits(bitField1, 32, bitField1Offset);
            }

            if (useCharacterFlags1 > 0)
            {
                saveBuffer.WriteBits(unit.playerFlagCount1, 8);
                for (int i = 0; i < unit.playerFlagCount1; i++)
                {
                    saveBuffer.WriteBits(unit.playerFlags1[i], 16);
                }

                bitField2 |= useCharacterFlags1;
                saveBuffer.WriteBits(bitField2, 32, bitField2Offset);
            }

            /***** Unit Body *****/

            if (unit.unknownCount1B > 0)
            {
                saveBuffer.WriteBits(unit.unknownCount1B, 5);
                for (int i = 0; i < unit.unknownCount1B; i++)
                {
                    saveBuffer.WriteBits(unit.unknownCount1Bs[i].unknown1, 16);
                    saveBuffer.WriteBits(unit.unknownCount1Bs[i].unknown2, 32);
                }

                bitField1 |= useUnknown_1B;
                saveBuffer.WriteBits(bitField1, 32, bitField1Offset);
            }

            saveBuffer.WriteBits(unit.unknownFlag, 4);
            saveBuffer.WriteBits(unit.unknownFlagValue, 16);

            if (useUnknown_17 > 0)
            {
                WriteNonStandardFunc(unit.unknown17, saveBuffer);
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

                    saveBuffer.WriteBits(unit.unknown_01_03_1, 16);
                    saveBuffer.WriteBits(unit.unknown_01_03_2, 12);
                    saveBuffer.WriteBits(unit.unknown_01_03_3, 12);
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

            if (useUnknown_08 > 0)
            {
                // does it include \00??
                string blag = "Alex";
                byte[] blagBytes = FileTools.StringToUnicodeByteArray(blag);
                saveBuffer.WriteBits(blagBytes.Length/2, 8);
                for (int i = 0; i < blagBytes.Length; i++)
                {
                    saveBuffer.WriteBits(blagBytes[i], 8);
                }

                bitField1 |= useUnknown_08;
                saveBuffer.WriteBits(bitField1, 32, bitField1Offset);
            }

            if (useCharacterFlags2 > 0)
            {
                saveBuffer.WriteBits(unit.playerFlagCount2, 8);
                for (int i = 0; i < unit.playerFlagCount2; i++)
                {
                    saveBuffer.WriteBits(unit.playerFlags2[i], 16);
                }

                bitField1 |= useCharacterFlags2;
                saveBuffer.WriteBits(bitField1, 32, bitField1Offset);
            }

            if (useStats > 0)
            {
                WriteStatBlock(unit.statBlock, true, saveBuffer);
                bitField1 |= useStats;
                saveBuffer.WriteBits(bitField1, 32, bitField1Offset);
            }
        }

        private void WriteNonStandardFunc(byte[] byteArray, BitBuffer saveBuffer)
        {
            for (int i = 0; i < 8; i++)
            {
                saveBuffer.WriteBits(byteArray[i], 8);
            }
        }

        private void WriteStatBlock(UnitStatBlock statBlock, bool writeNameCount, BitBuffer saveBuffer)
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

        private void WriteStat(UnitStat stat, BitBuffer saveBuffer)
        {
            /***** Stat Block Header *****
             * statId                                           16                  Stat ID from applicable excel table.
             * extraAttributesCount                             2                   Count of following.
             * {
             *      exists                                      1                   Simple bool test.
             *      if (exists)
             *      {
             *          bitCount_EA                             6                   Number of bits used in file.
             *          
             *          unknownFlag                             2                   Only seen 0x0 and 0x2.
             *          if (unknownFlag == 0x2)
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
             * bitCount                                         6                   Number of bits used in file.
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
             * if (repeatFlag)
             * {
             *      repeatCount                                 10                  Number of times to read in stat values.
             * }
             * 
             * for (number of repeats)
             * {
             *      for (number of extra attributes)
             *      {
             *          extraAttributeValue                     bitCount_EA         The extra attribute for the applicable value below.
             *      }
             *      
             *      statValue                                   bitCount            The actual stat value.
             * }
             */


        }
    }
}
