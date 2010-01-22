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
                unit.unknownCount1 = bitBuffer.ReadBits(4) - 8;
                unit.unknownCount1s = new UnknownCount1_S[unit.unknownCount1];

                for (int i = 0; i < unit.unknownCount1; i++)
                {
                    UnknownCount1_S uc1;

                    uc1.unknown1 = bitBuffer.ReadBits(16);
                    uc1.unknown2 = bitBuffer.ReadBits(16);

                    unit.unknownCount1s[i] = uc1;
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
                unit.unknownCount3 = bitBuffer.ReadBits(5);
                unit.unknownCount3s = new UnknownCount3_S[unit.unknownCount3];

                for (int i = 0; i < unit.unknownCount3; i++)
                {
                    UnknownCount3_S uc3;

                    uc3.unknown1 = bitBuffer.ReadBits(16);
                    uc3.unknown2 = bitBuffer.ReadBits(32);

                    unit.unknownCount3s[i] = uc3;
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
                unit.unknown1 = ReadNonStandardFunc();
            }


            if (TestBit(unit.bitField1, 0x03) || TestBit(unit.bitField1, 0x01))
            {
                unit.unknownBool2 = bitBuffer.ReadBits(1);
                if (unit.unknownBool2 > 0)
                {
                    if (TestBit(unit.bitField1, 0x02))
                    {
                        unit.unknown11 = bitBuffer.ReadBits(32);
                    }

                    unit.unknown7 = bitBuffer.ReadBits(16);
                    unit.unknown8 = bitBuffer.ReadBits(12);
                    unit.unknown9 = bitBuffer.ReadBits(12);
                }

                unit.unknown10 = ReadNonStandardFunc();
            }


            if (TestBit(unit.bitField1, 0x06))
            {
                unit.unknownBool3 = bitBuffer.ReadBits(1);
                if (unit.unknownBool2 != 1)
                {
                    return false;
                }
            }


            if (TestBit(unit.bitField1, 0x09))
            {
                unit.unknown12 = bitBuffer.ReadBits(8);
            }


            if (TestBit(unit.bitField1, 0x07))
            {
                unit.jobClass = bitBuffer.ReadBits(8);
                unit.unknown2 = bitBuffer.ReadBits(8);
            }


            if (TestBit(unit.bitField1, 0x08))
            {
                unit.charCount = bitBuffer.ReadBits(8);
                if (unit.charCount > 0)
                {
                    unit.szName = new Char[unit.charCount];
                    for (int i = 0; i < unit.charCount; i++)
                    {
                        unit.szName[i] = (Char)bitBuffer.ReadBits(16);
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
    }
}
