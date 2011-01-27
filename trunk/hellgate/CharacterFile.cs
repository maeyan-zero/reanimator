using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Revival.Common;

namespace Hellgate
{
    public class CharacterFile : HellgateFile
    {
        public new const String Extension = ".hg1";
        public new const String ExtensionDeserialised = ".hg1.xml";
        private const UInt32 RequiredVersion = 0x01; // 1
        private const UInt32 FileMagicWord = 0x484D4752;            // 'RGMH'
        private const UInt32 StartObjectMagicWord = 0x67616C46;     // 'Flag'
        private const UInt32 EndObjectMagicWord = 0x2B523460;       // '+R4`'
        private const UInt32 WeaponConfigMagicWord = 0x91103A74;    // '`.:t'

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct FileHeader
        {
            public Int32 MagicWord;
            public Int32 Version;
            public Int32 DataOffset1;
            public Int32 DataOffset2;
        };

        private readonly bool _debugOutputLoadingProgress;

        public byte[] FileBytes;
        private int _byteOffset;
        private int _bitOffset;

        public String Path { get; private set; }
        public UnitObject Character { get; private set; }
        public String Name { get { return Character.Name; } }

        public CharacterFile(String filePath, bool debugOutputLoadingProgress = false)
        {
            Path = filePath;
            _debugOutputLoadingProgress = debugOutputLoadingProgress;
        }

        public override void ParseFileBytes(byte[] fileBytes)
        {
            // sanity check
            if (fileBytes == null) throw new ArgumentNullException("fileBytes", "File bytes cannot be null!");

            Character = new UnitObject();
            _byteOffset = 0;
            FileBytes = fileBytes;

            try
            {
                // main file header
                FileHeader fileHeader = FileTools.ByteArrayToStructure<FileHeader>(fileBytes, ref _byteOffset);

                // file header checks
                if (fileHeader.MagicWord != FileMagicWord) throw new Exceptions.UnexpectedMagicWordException();
                if (fileHeader.Version != RequiredVersion) throw new Exceptions.NotSupportedFileVersionException();

                _byteOffset = 0x2028;
                _ReadUnit(Character);
            }
            finally
            {
                //_fileBytes = null; // we should null this as we don't want to hold them in memory, but for testing purposes easier with direct access
            }
        }

        /// <summary>
        /// Reads a Unit structure from the local BitBuffer.
        /// </summary>
        /// <param name="unit">UnitObject to be read into.</param>
        private void _ReadUnit(UnitObject unit)
        {
            // unit object versions
            unit.MajorVersion = _ReadBits(16);
            unit.MinorVersion = _ReadBits(8);
            if (unit.MajorVersion != 0x00BF && unit.MajorVersion != 0x00CD) throw new Exceptions.NotSupportedVersionException("0x00BF or 0x00CD", "0x" + unit.MajorVersion.ToString("X4"));
            if (unit.MinorVersion != 0x00 && unit.MinorVersion != 0x02) throw new Exceptions.NotSupportedVersionException("0x00 or 0x02", "0x" + unit.MinorVersion.ToString("X2"));
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("UnitObject MajorVersion = {0} (0x{0:X4}), MinorVersion = {1} (0x{1:X2})", unit.MajorVersion, unit.MinorVersion));
            }


            // content bit fields
            unit.BitFieldCount = _ReadBits(8);
            if (unit.BitFieldCount >= 1) unit.BitField1 = _ReadBits(32);
            if (unit.BitFieldCount == 2) unit.BitField2 = _ReadBits(32);
            unit.BitField = (ulong)unit.BitField2 << 32 | (uint)unit.BitField1;
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("BitField1 = {0}\nBitField2 = {1}", _DebugBinaryFormat(unit.BitField1), _DebugBinaryFormat(unit.BitField2)));
            }


            // total bit count
            if (_TestBit(unit.BitField, 0x1D))
            {
                unit.BitCount = _ReadBits(32);
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("Total BitCount = {0}", unit.BitCount));
                }
            }



            // begin data magic word
            if (_TestBit(unit.BitField, 0x00))
            {
                unit.BeginFlag = (uint)_ReadBits(32);
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("BeginFlag = 0x{0}", unit.BeginFlag.ToString("X8")));
                }
                if (unit.BeginFlag != StartObjectMagicWord && unit.BeginFlag != EndObjectMagicWord) throw new Exceptions.UnexpectedTokenException(StartObjectMagicWord, unit.BeginFlag);
            }



            // dunno what these are exactly
            if (_TestBit(unit.BitField, 0x1C))
            {
                unit.TimeStamp1 = _ReadBits(32);
                unit.TimeStamp2 = _ReadBits(32);
                unit.TimeStamp3 = _ReadBits(32);
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("TimeStamp1 = {0}, TimeStamp2 = {1}, TimeStamp3 = {2}", unit.TimeStamp1, unit.TimeStamp2, unit.TimeStamp3));
                }
            }


            // dunno...
            if (_TestBit(unit.BitField, 0x1F))
            {
                unit.unknownCount1F = _ReadBits(4) - 8;
                unit.unknownCount1Fs = new UnitObject.UnknownCount1F[unit.unknownCount1F];
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("unit.unknownCount1F = {0}", unit.unknownCount1F));
                }

                for (int i = 0; i < unit.unknownCount1F; i++)
                {
                    unit.unknownCount1Fs[i] = new UnitObject.UnknownCount1F
                    {
                        Unknown1 = (short)_ReadBits(16), // table 0x6D ??
                        Unknown2 = (short)_ReadBits(16)  // table 0xB2 ??
                    };

                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("unit.unknownCount1Fs[{0}].Unknown1 = {1} (0x{1:X4}), unit.unknownCount1Fs[{0}].Unknown1 = {2} (0x{2:X4})",
                            i, unit.unknownCount1Fs[i].Unknown1, unit.unknownCount1Fs[i].Unknown2));
                    }
                }
            }


            // character flags
            if (_TestBit(unit.BitField, 0x20))
            {
                unit._playerFlagCount1 = _ReadBits(8);
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("unit._playerFlagCount1 = {0}", unit._playerFlagCount1));
                }

                for (int i = 0; i < unit._playerFlagCount1; i++)
                {
                    unit.PlayerFlags1.Add(_ReadBits(16)); // table 0x4B??
                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("unit.PlayerFlags1[{0}] = {1} (0x{1:X4})", i, unit.PlayerFlags1[i]));
                    }
                }
            }


            // dunno...
            if (_TestBit(unit.BitField1, 0x1B))
            {
                unit._bitOffsetCount = _ReadBits(5);
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("unit._bitOffsetCount = {0}", unit._bitOffsetCount));
                }
                if (unit._bitOffsetCount > 1)
                {
                    throw new Exceptions.CharacterFileNotImplementedException("Unexpected unit._bitOffsetCount (> 1)!\nNot-Implemented cases. Please report this error and supply the offending file.");
                }

                for (int i = 0; i < unit._bitOffsetCount; i++)
                {
                    UnitObject.UnitBitOffsets bitOffsets = new UnitObject.UnitBitOffsets
                    {
                        Code = _ReadBits(16),
                        Offset = _ReadBits(32)
                    };
                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("bitOffsets[{0}].Code = {1} (0x{1:X4}), bitOffsets[{0}].Offset = {2}", i, bitOffsets.Code, bitOffsets.Offset));
                    }

                    if (bitOffsets.Code != 0x3030)
                    {
                        throw new Exceptions.CharacterFileNotImplementedException("Unexpected value for bitOffsets.index (!= 0x3030)!\nNot-Implemented cases. Please report this error and supply the offending file.");
                    }
                    unit._bitOffsets.Add(bitOffsets);
                }
            }


            // dunno...
            if (_TestBit(unit.BitField1, 0x05))
            {
                throw new Exceptions.CharacterFileNotImplementedException("Unexpected bit case for unit._bitField1 (0x05 = true)!\nNot-Implemented cases. Please report this error and supply the offending file.");
            }


            // unit type/code
            unit.unitType = _ReadBits(4);
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("unit.unitType = {0}", unit.unitType));
            }
            if (unit.unitType != 1 && unit.unitType != 2 && unit.unitType != 4)
            {
                throw new Exceptions.CharacterFileNotImplementedException("Unexpected value for unit.unitType (!= 1, 2, 4)!\nNot-Implemented cases. Please report this warning and supply the offending file.");
            }
            unit.unitCode = _ReadBits(16);
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("unit.unitCode = {0} (0x{0:X4})", unit.unitCode));
            }
            // if unit type == 1, table = 0x91
            //                 2, table = 0x77
            //                 4, table = 0x67
            //                 5, table = 0x7B


            // unit object id
            if (_TestBit(unit.BitField1, 0x17))
            {
                if (unit.MajorVersion > 0xB2)
                {
                    unit.ObjectId = _ReadInt64();
                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("unit.ObjectId = {0} (0x{0:X16})", unit.ObjectId));
                    }
                }
            }


            // dunno
            if (_TestBit(unit.BitField1, 0x03) || _TestBit(unit.BitField1, 0x01))
            {
                unit.unknownBool_01_03 = (_ReadBits(1) != 0);
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("unit.unknownBool_01_03 = {0}", unit.unknownBool_01_03));
                }

                if (unit.unknownBool_01_03)
                {
                    if (_TestBit(unit.BitField1, 0x02))
                    {
                        unit.unknown_02 = _ReadBits(32);
                        if (_debugOutputLoadingProgress)
                        {
                            Debug.WriteLine(String.Format("unit.unknown_02 = {0}", unit.unknown_02));
                        }
                    }

                    unit.inventoryType = _ReadBits(16);
                    unit.inventoryPositionX = _ReadBits(12);
                    unit.inventoryPositionY = _ReadBits(12);
                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("inventoryType = {0}, inventoryPositionX = {1}, inventoryPositionY = {2}",
                            unit.inventoryType, unit.inventoryPositionX, unit.inventoryPositionY));
                    }
                }

                unit.unknown_01_03_4 = _ReadInt64();
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("unit.unknown_01_03_4 = {0} (0x{0:X16})", unit.unknown_01_03_4));
                }
            }


            // dunno
            if (_TestBit(unit.BitField1, 0x06))
            {
                unit.unknownBool_06 = (_ReadBits(1) != 0);
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("unit.unknownBool_06 = {0}", unit.unknownBool_06));
                }
                if (!unit.unknownBool_06)
                {
                    throw new Exceptions.CharacterFileNotImplementedException("if (unit.unknownBool_06 != 1");
                }
            }


            // on items only I think - Usually 0x00
            if (_TestBit(unit.BitField1, 0x09))
            {
                unit.unknown_09 = _ReadBits(8);
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("unit.unknown_09 = {0} (0x{0:X2})", unit.unknown_09));
                }
            }


            // on character only
            if (_TestBit(unit.BitField1, 0x07))
            {
                unit.CharacterHeight = _ReadBits(8);
                unit.CharacterWidth = _ReadBits(8);
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("CharacterHeight = {0}, CharacterWidth = {1}", unit.CharacterHeight, unit.CharacterWidth));
                }
            }


            // on character only
            if (_TestBit(unit.BitField1, 0x08))
            {
                unit._charNameCount = _ReadBits(8);
                if (unit._charNameCount > 0)
                {
                    unit.characterName = new Char[unit._charNameCount];
                    for (int i = 0; i < unit._charNameCount; i++)
                    {
                        unit.characterName[i] = (char)_ReadBits(16);
                    }
                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("CharacterName = {0}", new String(unit.characterName)));
                    }
                }
            }


            // on both character and items - appears to be always zero for items
            if (_TestBit(unit.BitField1, 0x0A))
            {
                unit._playerFlagCount2 = _ReadBits(8);
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("unit._playerFlagCount2 = {0}", unit._playerFlagCount1));
                }

                if (unit._playerFlagCount2 > 0)
                {
                    for (int i = 0; i < unit._playerFlagCount2; i++)
                    {
                        unit.PlayerFlags2.Add(_ReadBits(16));
                        if (_debugOutputLoadingProgress)
                        {
                            Debug.WriteLine(String.Format("unit.PlayerFlags2[{0}] = {1} (0x{1:X4})", i, unit.PlayerFlags2[i]));
                        }
                    }
                }
            }


            // is the player dead
            unit.IsDead = (_ReadBits(1) != 0);
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("IsDead = {0}", unit.IsDead));
            }


            // unit stats
            if (_TestBit(unit.BitField1, 0x0D))
            {
                if (_debugOutputLoadingProgress) Debug.WriteLine("==Has Stat Block==");

                unit.statBlock = new UnitObject.StatBlock();
                _ReadStatBlock(unit.statBlock, true);
            }


            unit.HasAppearanceDetails = (_ReadBits(1) != 0);
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("HasAppearanceDetails = {0}", unit.HasAppearanceDetails));
            }
            if (unit.HasAppearanceDetails)
            {
                unit.Appearance = new UnitObject.UnitAppearance();
                ReadAppearance(unit, unit.Appearance);
            }


            if (_TestBit(unit.BitField1, 0x12))
            {
                unit.ItemEndBitOffset = _ReadBits(32);
                unit.ItemCount = _ReadBits(10);
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("ItemEndBitOffset = {0}, ItemCount = {1}", unit.ItemEndBitOffset, unit.ItemCount));
                }

                for (int i = 0; i < unit.ItemCount; i++)
                {
                    UnitObject item = new UnitObject();
                    _ReadUnit(item);
                    unit.Items.Add(item);
                }
            }


            if (_TestBit(unit.BitField1, 0x1A))
            {
                unit.WeaponConfigFlag = (uint)_ReadBits(32);
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("WeaponConfigFlag = {0} (0x{0:X8})", unit.ItemEndBitOffset));
                }
                if (unit.WeaponConfigFlag != WeaponConfigMagicWord)
                {
                    throw new Exceptions.UnexpectedTokenException(WeaponConfigMagicWord, unit.WeaponConfigFlag);
                }

                unit.EndFlagBitOffset = _ReadBits(32);     // to end flag
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("EndFlagBitOffset = {0}", unit.EndFlagBitOffset));
                }

                unit.WeaponConfigCount = _ReadBits(6);
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("WeaponConfigCount = {0}", unit.WeaponConfigCount));
                }
                unit.WeaponConfigs = new UnitObject.UnitWeaponConfig[unit.WeaponConfigCount];
                for (int i = 0; i < unit.WeaponConfigCount; i++)
                {
                    UnitObject.UnitWeaponConfig weaponConfig = new UnitObject.UnitWeaponConfig
                    {
                        Id = (short)_ReadBits(16),
                        UnknownCount1 = _ReadBits(4)
                    };
                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("weaponConfig.Id = {0} (0x{0:X4}), weaponConfig.UnknownCount1 = {1}", weaponConfig.Id, weaponConfig.UnknownCount1));
                    }

                    if (weaponConfig.UnknownCount1 != 0x02)
                    {
                        throw new Exceptions.CharacterFileNotImplementedException("if (weaponConfig.unknownCount1 != 0x02)");
                    }
                    weaponConfig.Exists1 = new bool[2];
                    weaponConfig.UnknownIds1 = new int[2];
                    for (int j = 0; j < weaponConfig.UnknownCount1; j++)
                    {
                        weaponConfig.Exists1[j] = (_ReadBits(1) != 0);
                        if (_debugOutputLoadingProgress)
                        {
                            Debug.WriteLine(String.Format("weaponConfig.Exists1[{0}] = {1}", j, weaponConfig.Exists1[j]));
                        }
                        if (weaponConfig.Exists1[j])
                        {
                            weaponConfig.UnknownIds1[j] = _ReadBits(32); // under some condition this will be ReadFromOtherFunc thingy
                        }
                    }

                    // yes this chunk looks the same as above - the above chunk though is in a specific function and can differ at 1 point
                    // also, it can be != 2
                    weaponConfig.UnknownCount2 = _ReadBits(4);
                    weaponConfig.Exists2 = new bool[weaponConfig.UnknownCount2];
                    weaponConfig.UnknownIds2 = new int[weaponConfig.UnknownCount2];
                    for (int j = 0; j < weaponConfig.UnknownCount2; j++)
                    {
                        weaponConfig.Exists2[j] = (_ReadBits(1) != 0);
                        if (weaponConfig.Exists2[j])
                        {
                            weaponConfig.UnknownIds2[j] = _ReadBits(32);
                        }
                    }

                    weaponConfig.IdAnother = _ReadBits(32); // read from 0x17 file          // 0x3931 -> 0x4B

                    unit.WeaponConfigs[i] = weaponConfig;
                }
            }


            // end flag
            if (!_TestBit(unit.BitField1, 0x00)) return;
            unit.EndFlag = _ReadBits(32);
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("EndFlag = {0} (0x{0:X8})", unit.EndFlag));
            }

            if (unit.EndFlag == unit.BeginFlag || unit.EndFlag == EndObjectMagicWord) return;
            int bitOffset = unit.BitCount - _bitOffset;
            int byteOffset = (FileBytes.Length - _byteOffset) - (_bitOffset >> 3);
            //throw new Exceptions.InvalidFileException("Flags not aligned!\nBit Offset: " + _bitOffset + "\nExpected: " + unit.BitCount + " (+" + bitOffset +
            //                  ")\nByte Offset: " + (_bitOffset >> 3) + "\nExpected: " + (FileBytes.Length - _byteOffset) + " (+" + byteOffset + ")");
        }

        public override byte[] ToByteArray()
        {
            throw new NotImplementedException();
        }

        public override byte[] ExportAsDocument()
        {
            throw new NotImplementedException();
        }

        private void _ReadStatBlock(UnitObject.StatBlock statBlock, bool readNameCount)
        {
            statBlock.statVersion = _ReadBits(16);
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine("StatBlock Version = " + statBlock.statVersion);
            }
            if (statBlock.statVersion != 0x0A)
            {
                throw new Exceptions.NotSupportedVersionException("0x0A", "0x" + statBlock.statVersion.ToString("X2"));
            }

            statBlock.unknown1 = _ReadBits(3);
            statBlock.additionalStats = new List<UnitObject.UnitStatAdditional>();
            statBlock.stats = new List<UnitObject.StatBlock.Stat>();

            int additionalStatCount = _ReadBits(6);
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("statBlock.unknown1 = {0}, additionalStatCount = {1}", statBlock.unknown1, additionalStatCount));
            }
            for (int i = 0; i < additionalStatCount; i++)
            {
                UnitObject.UnitStatAdditional additionalStat = new UnitObject.UnitStatAdditional
                {
                    Unknown = (short)_ReadBits(16),
                    StatCount = (short)_ReadBits(16),
                    Stats = new List<UnitObject.StatBlock.Stat>()
                };
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("additionalStat.Unknown = {0}, additionalStat.StatCount = {1}", additionalStat.Unknown, additionalStat.StatCount));
                }

                for (int j = 0; j < additionalStat.StatCount; j++)
                {
                    UnitObject.StatBlock.Stat unitStat = new UnitObject.StatBlock.Stat();
                    _ReadStat(unitStat);

                    additionalStat.Stats.Add(unitStat);

                }

                statBlock.additionalStats.Add(additionalStat);
            }

            int statsCount = _ReadBits(16);
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("statsCount = {0}", statsCount));
            }
            for (int i = 0; i < statsCount; i++)
            {
                UnitObject.StatBlock.Stat unitStat = new UnitObject.StatBlock.Stat();
                _ReadStat(unitStat);

                //statBlock.stats[i] = unitStat;
                statBlock.stats.Add(unitStat);
            }


            if (!readNameCount) return;
            statBlock.nameCount = _ReadBits(8);
            statBlock.names = new UnitObject.UnitStatName[statBlock.nameCount];
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("statBlock.nameCount = {0}", statBlock.nameCount));
            }

            for (int i = 0; i < statBlock.nameCount; i++)
            {
                UnitObject.UnitStatName name = new UnitObject.UnitStatName
                {
                    Unknown1 = (short)_ReadBits(16),
                    StatBlock = new UnitObject.StatBlock()
                };
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("name.Unknown1 = {0}", name.Unknown1));
                }

                _ReadStatBlock(name.StatBlock, false);
                statBlock.names[i] = name;
            }
        }

        private void _ReadStat(UnitObject.StatBlock.Stat unitStat)
        {
            unitStat.Code = (short)_ReadBits(16);
            unitStat.Attributes = new List<UnitObject.StatBlock.Stat.Attribute>();

            int statAttributeCount = _ReadBits(2);
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("unitStat.Code = {0} (0x{0:X4}), unitStat.statAttributeCount = {1}", unitStat.Code, statAttributeCount));
            }
            for (int i = 0; i < statAttributeCount; i++)
            {
                UnitObject.StatBlock.Stat.Attribute exAtrib = new UnitObject.StatBlock.Stat.Attribute { exists = _ReadBits(1) };
                if (!exAtrib.Exists) break;

                exAtrib.BitCount = _ReadBits(6);

                exAtrib.Unknown1 = _ReadBits(2);
                if (exAtrib.Unknown1 == 0x02)
                {
                    exAtrib.Unknown1_1 = _ReadBits(1);
                }

                exAtrib.skipTableId = _ReadBits(1);
                if (!exAtrib.SkipTableId)
                {
                    exAtrib.TableId = _ReadBits(16);
                }

                unitStat.Attributes.Add(exAtrib);

                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("exAtrib.BitCount = {0}, exAtrib.Unknown1 = {1}, exAtrib.Unknown1_1 = {2}, exAtrib.SkipTableId = {3}, exAtrib.TableId = {4}",
                        exAtrib.BitCount, exAtrib.Unknown1, exAtrib.Unknown1_1, exAtrib.SkipTableId, exAtrib.TableId));
                }
            }

            unitStat.bitCount = _ReadBits(6);

            unitStat.OtherAttributeFlag = _ReadBits(3);
            unitStat.OtherAttribute = new UnitObject.UnitStatOtherAttribute();
            if ((unitStat.OtherAttributeFlag & 0x01) >= 1)
            {
                unitStat.OtherAttribute.Unknown1 = _ReadBits(4);
            }
            if ((unitStat.OtherAttributeFlag & 0x02) >= 1)
            {
                unitStat.OtherAttribute.Unknown2 = _ReadBits(12);
            }
            if ((unitStat.OtherAttributeFlag & 0x04) >= 1)
            {
                unitStat.OtherAttribute.Unknown3 = _ReadBits(1);
                if (unitStat.OtherAttribute.Unknown3 != 0x01)
                {
                    throw new Exceptions.CharacterFileNotImplementedException("if (unitStat.otherAttribute.unknown3 != 0x01)");
                }
            }
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("unitStat.bitCount = {0}, unitStat.OtherAttributeFlag = {1}, unitStat.OtherAttribute.Unknown1 = {2}, unitStat.OtherAttribute.Unknown2 = {3}, unitStat.OtherAttribute.Unknown3 = {4}",
                    unitStat.bitCount, unitStat.OtherAttributeFlag, unitStat.OtherAttribute.Unknown1, unitStat.OtherAttribute.Unknown2, unitStat.OtherAttribute.Unknown3));
            }

            unitStat.SkipResource = _ReadBits(2);
            if (unitStat.SkipResource == 0)
            {
                unitStat.Resource = (short)_ReadBits(16);
            }

            unitStat.RepeatFlag = (_ReadBits(1) != 0);
            unitStat.RepeatCount = 1;
            if (unitStat.RepeatFlag)
            {
                unitStat.RepeatCount = _ReadBits(10);
            }
            unitStat.values = new List<UnitObject.StatBlock.Stat.Values>();
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("unitStat.SkipResource = {0}, unitStat.Resource = {1}, unitStat.RepeatFlag = {2}, unitStat.RepeatCount = {3}",
                    unitStat.SkipResource, unitStat.Resource, unitStat.RepeatFlag, unitStat.RepeatCount));
            }

            for (int i = 0; i < unitStat.RepeatCount; i++)
            {
                UnitObject.StatBlock.Stat.Values statValues = new UnitObject.StatBlock.Stat.Values();
                
                for (int j = 0; j < statAttributeCount; j++)
                {
                    if (unitStat.Attributes[j].exists != 1) continue;

                    int extraAttribute = _ReadBits(unitStat.Attributes[j].BitCount);
                    switch (j)
                    {
                        case 0:
                            statValues.Attribute1 = extraAttribute;
                            break;
                        case 1:
                            statValues.Attribute2 = extraAttribute;
                            break;
                        case 2:
                            statValues.Attribute3 = extraAttribute;
                            break;
                    }
                }

                statValues.Stat = _ReadBits(unitStat.bitCount);
                unitStat.values.Add(statValues);

                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("statValues.Attribute1 = {0}, statValues.Attribute2 = {1}, statValues.Attribute3 = {2}, statValues.Stat = {3}",
                        statValues.Attribute1, statValues.Attribute2, statValues.Attribute3, statValues.Stat));
                }
            }
        }

        private void ReadAppearance(UnitObject heroUnit, UnitObject.UnitAppearance appearance)
        {
            appearance.equippedItemCount = _ReadBits(3);
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("appearance.equippedItemCount = {0}", appearance.equippedItemCount));
            }
            for (int i = 0; i < appearance.equippedItemCount; i++)
            {
                UnitObject.UnitAppearance.EquippedItem equippedItem = new UnitObject.UnitAppearance.EquippedItem();

                if (_TestBit(heroUnit.BitField1, 0x0F))
                {
                    throw new Exceptions.CharacterFileNotImplementedException("Unexpected bitField1 value! (0x0F == true)\nNot-Implemented cases. Please report this warning and supply the offending file.");
                }

                equippedItem.ItemCode = _ReadBits(16);
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("equippedItem.ItemCode = {0} (0x{0:X4})", equippedItem.ItemCode));
                }

                if (_TestBit(heroUnit.BitField1, 0x00))
                {
                    equippedItem.affixCount = _ReadBits(3);
                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("equippedItem.affixCount = {0}", equippedItem.affixCount));
                    }
                    for (int j = 0; j < equippedItem.affixCount; j++)
                    {
                        equippedItem.AffixCodes.Add(_ReadBits(32));
                        if (_debugOutputLoadingProgress)
                        {
                            Debug.WriteLine(String.Format("equippedItem.AffixCodes[{0}] = {1} (0x{1:X4})", j, equippedItem.AffixCodes[j]));
                        }
                    }
                }

                appearance.EquippedItems.Add(equippedItem);
            }


            appearance.unknown1 = _ReadBits(16);
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("appearance.unknown1 = {0} (0x{0:X4})", appearance.unknown1));
            }

            if (_TestBit(heroUnit.BitField1, 0x16))
            {
                appearance.unknown2 = _ReadInt64();
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("appearance.unknown2 = {0} (0x{0:X16})", appearance.unknown2));
                }
            }


            if (_TestBit(heroUnit.BitField1, 0x11))
            {
                appearance.WardrobeLayerHeadCount = _ReadBits(4);
                for (int i = 0; i < appearance.WardrobeLayerHeadCount; i++)
                {
                    appearance.WardrobeLayersHead.Add(_ReadBits(16));
                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("appearance.WardrobeLayersHead[{0}] = {1} (0x{1:X4})", i, appearance.WardrobeLayersHead[i]));
                    }
                }

                appearance.WardrobeAppearanceGroupCount = _ReadBits(3);
                for (int i = 0; i < appearance.WardrobeAppearanceGroupCount; i++)
                {
                    appearance.WardrobeAppearanceGroups.Add(_ReadBits(16));
                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("appearance.WardrobeAppearanceGroups[{0}] = {1} (0x{1:X4})", i, appearance.WardrobeAppearanceGroups[i]));
                    }
                }

                appearance.ColorCount = _ReadBits(4);
                for (int i = 0; i < appearance.ColorCount; i++)
                {
                    appearance.ColorPaletteIndicies.Add(_ReadBits(8));
                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("appearance.ColorPaletteIndicies[{0}] = {1} (0x{1:X2})", i, appearance.ColorPaletteIndicies[i]));
                    }
                }
            }


            if (_TestBit(heroUnit.BitField1, 0x10))
            {
                appearance.WardrobeLayerCount = _ReadBits(16);
                for (int i = 0; i < appearance.WardrobeLayerCount; i++)
                {
                    UnitObject.UnitAppearance.ModelWardrobeLayer gear = new UnitObject.UnitAppearance.ModelWardrobeLayer
                    {
                        ItemCode = _ReadUInt16(),
                        UnknownBool = _ReadBool()
                    };

                    if (gear.UnknownBool)
                    {
                        gear.UnknownBoolValue = _ReadBits(2);
                    }

                    appearance.WardrobeLayers.Add(gear);
                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("appearance.WardrobeLayers[{0}].ItemCode = {1} (0x{1:X4}), .UnknownBool = {2}, .UnknownBoolValue = {3}", i, appearance.WardrobeLayers[i].ItemCode, appearance.WardrobeLayers[i].UnknownBool, appearance.WardrobeLayers[i].UnknownBoolValue));
                    }
                }
            }
        }

        private UInt16 _ReadUInt16()
        {
            return (UInt16)_ReadBits(16);
        }

        private bool _ReadBool()
        {
            return (_ReadBits(1) != 0);
        }

        private int _ReadBits(int bitCount)
        {
            int bitsToRead = bitCount;
            int byteOffset = _bitOffset >> 3;
            int b = FileBytes[_byteOffset + byteOffset];

            int offsetBitsInThisByte = _bitOffset & 0x07;
            int bitsToUseFromByte = 0x08 - offsetBitsInThisByte;

            int bitOffset = bitCount;
            if (bitsToUseFromByte < bitCount)
                bitOffset = bitsToUseFromByte;

            b >>= offsetBitsInThisByte;
            bitsToRead -= bitOffset;

            // clean any excess bits we don't want
            b &= ((0x01 << bitOffset) - 1);

            int bytesStillToRead = bitsToRead + 0x07;
            bytesStillToRead >>= 3;

            int ret = b;
            for (int i = bytesStillToRead; i > 0; i--)
            {
                int bitLevel = (i - 1) * 8;

                b = FileBytes[_byteOffset + byteOffset + i];
                int bitsRead = 0x08;

                if (i == bytesStillToRead)
                {
                    int cleanBits = bitsToRead - bitLevel;
                    bitsRead = cleanBits;
                    cleanBits = 0x01 << cleanBits;
                    cleanBits--;
                    b &= (byte)cleanBits;
                }

                b <<= bitOffset + bitLevel;
                ret |= b;
                bitsToRead -= bitsRead;
            }

            _bitOffset += bitCount;

            return ret;
        }

        private Int64 _ReadInt64() // todo: this might also be reading in a double value...
        {
            byte[] ret = new byte[8];

            for (int i = 0; i < 8; i++) // todo: this could easily be done as two int32 reads and just OR them
            {
                ret[i] = (byte)_ReadBits(8);
            }

            return BitConverter.ToInt64(ret, 0);
        }

        private static bool _TestBit(Int32 bitField, int bitOffset)
        {
            return (bitField & (1 << bitOffset)) != 0;
        }

        private static bool _TestBit(ulong bitField, int bitOffset)
        {
            const ulong bitMask = 1;
            return ((bitField & (bitMask << bitOffset)) != 0);
        }

        private static String _DebugBinaryFormat(long input)
        {
            String bitString = null;

            for (int i = 31; i >= 0; i--)
            {
                String result = (((input >> i) & 1) == 1) ? "1" : "0";

                bitString += result;

                if ((i > 0) && ((i) % 4) == 0) bitString += " ";
            }

            return bitString;
        }

    }
}