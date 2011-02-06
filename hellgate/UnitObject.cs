using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;
using Hellgate.Excel.JapaneseBeta;
using Revival.Common;

namespace Hellgate
{
    [Serializable]
    public partial class UnitObject
    {
        private const UInt32 ObjectMagicWord = 0x67616C46;          // 'Flag'
        private const UInt32 ItemMagicWord = 0x2B523460;            // '`4R+'
        private const UInt32 WeaponConfigMagicWord = 0x91103A74;    // 't:.`'

        ///////////////////// General Structure Definition /////////////////////
        /////// Start of read inside main header check function (in ASM) ///////

        public int Version;							    	                    // 16 bits      // 0x00BF for SP client, 0x00CD for current Resurrection client
        public int Usage;							    	                    // 8

        public int BitFieldCount;									            // 8			// must be <= 2
        public int BitField1;										            // 32
        public int BitField2;										            // 32
        public UInt64 BitField;


        // if (testBit(bitField, 0x1D))
        public int BitCount;										            // 32			// of entire unit object


        // if (testBit(bitField, 0x00))
        public uint BeginFlag;										            // 32			// must be "Flag" (0x67616C46) or "`4R+" (0x2B523460)


        // if (testBit(bitField, 0x1C))
        public int TimeStamp1;										            // 32			// i don't think these are actually time stamps
        public int TimeStamp2;										            // 32			// but since they change all the time and can be
        public int TimeStamp3;										            // 32			// set to 00 00 00 00 and it'll still load... it'll do


        // if (testBit(bitField, 0x1F))
        public int UnknownCount1F;									            // 4
        public UnknownCount1FClass[] UnknownCount1Fs;                                           // no idea wtf these do


        // if (testBit(bitField, 0x20)					                                        // char state flags (e.g. "elite")
        public int PlayerFlagCount1;								            // 8
        public List<short> PlayerFlags1;                                        // 16 * PlayerFlagCount1		


        /////// End of read inside main header check function (in ASM) ///////


        // if (testBit(bitField, 0x1B))
        public int BitOffsetCount;                                              // 5            // only ever seen as 0 or 1 - alert if otherwise
        public readonly List<UnitBitOffsets> BitOffsets;                                        // only seen with item end bit offset in it


        // if (testBit(bitField, 0x05))
        public int Unknown05;                                                                   // used in MP


        public int UnitType;										            // 4		    // 1 = character, 2 = monster (e.g. engineer drone), 3 = ?, 4 = item, 5 = ?
        public short UnitCode;								                    // 16           // the code identifier of the UnitObject (e.g. Job Class, or Item Id etc)


        // if (testBit(bitField, 0x17)) // 64 bits read as 8x8 chunk bits from non-standard bit read function
        public Int64 ObjectId;                                                  // 64           // a unique id identifiying this unit "structure"


        // if (testBit(bitField, 0x01) || testBit(bitField, 0x03))
        // {
        public bool IsInventory;								                // 1 bit
        // {
        //// if (testBit(bitField, 0x02))
        public int Unknown02;										            // 32			// untested

        public int InventoryType;        								        // 16           // i think...
        public int InventoryPositionX;									        // 12           // x-position of inventory item
        public int InventoryPositionY;									        // 12           // y-position of inventory item
        // }
        // else
        // {
        public int Unknown0103Int1;                                             // 32

        public float Unknown0103Float11;                                        // 32
        public float Unknown0103Float12;                                        // 32
        public float Unknown0103Float13;                                        // 32

        public float Unknown0103Float21;                                        // 32
        public float Unknown0103Float22;                                        // 32
        public float Unknown0103Float23;                                        // 32

        public float Unknown0103Float31;                                        // 32
        public float Unknown0103Float32;                                        // 32
        public float Unknown0103Float33;                                        // 32

        public int Unknown0103Int2;                                             // 32

        public float Unknown0103Float4;                                         // 32

        public float Unknown0103Float5;                                         // 32
        // } // end IsInventory

        public Int64 Unknown0103Int64;								            // 8*8 bits (64) - non-standard func
        // } // end 0x01 || 0x03


        // if (testBit(bitField, 0x06)) // does more reading, but not seen
        public bool UnknownBool06;									            // 1            // alert if != 1


        // if (testBit(bitField, 0x09))
        public int Unknown09;										            // 8


        // if (testBit(bitField, 0x07)) // if (bitField1 & 0x80)
        public int CharacterHeight;									            // 8
        public int CharacterWidth;									            // 8


        // if (testBit(bitField, 0x08))
        public int CharNameCount;									            // 8
        public char[] CharacterName;                                            // 8 * CharNameCount


        // if (testBit(bitField, 0x0A))						                                    // char state flags (e.g. "elite")
        public int PlayerFlagCount2;								            // 8
        public List<int> PlayerFlags2;			                                // 16  * PlayerFlagCount2


        public bool UsageBool;                                                  // 1
        public int UsageBoolValue;                                              // 4


        public bool IsDead;									                    // 1            // does NOT influence HC dead! If the character died right before saving this flag is set to 1


        // if (testBit(bitField1, 0x0D))
        public StatBlock Stats;
        public byte StatUnknownByte1;
        public byte StatUnknownByte2;
        public int StatUnknownInt1;


        public bool HasAppearanceDetails;						                // 1
        public UnitAppearance Appearance;
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
        public int ItemEndBitOffset;									        // 32           // bit offset to end of items
        public int ItemCount;										            // 10 
        public List<UnitObject> Items;                                                          // each item is another UnitObject


        // if (testBit(pUnit->bitField1, 0x1A))
        public uint WeaponConfigFlag;                                           // 32           // must be 0x91103A74; always present
        public int EndFlagBitOffset;                                            // 32           // offset to end of file flag
        public int WeaponConfigCount;                                           // 6            // weapon config count
        public UnitWeaponConfig[] WeaponConfigs;                                                // i think this has item positions on bottom bar, etc as well


        // if (testBit(unit->bitField1, 0x00))
        public int EndFlag;											            // 32           // end of unit flag


        ///////////////////// Function Definitions /////////////////////

        public static FileManager FileManager;
        private readonly bool _debugOutputLoadingProgress;

        private bool _usingExternalBuffer;
        private byte[] _buffer;
        private int _byteOffset;
        private int _bitOffset;

        public UnitObject() : this(false) { } // for XML serialisation

        public UnitObject(bool debugOutputLoadingProgress)
        {
            _debugOutputLoadingProgress = debugOutputLoadingProgress;

            PlayerFlags1 = new List<short>();
            PlayerFlags2 = new List<int>();
            BitOffsets = new List<UnitBitOffsets>();
            Items = new List<UnitObject>();
        }

        [XmlAttribute("Name")]
        public string Name
        {
            get { return ToString(); }
            set { CharacterName = value.ToCharArray(); }
        }

        public override string ToString()
        {
            return new string(CharacterName);
        }

        public void ParseUnitObject(byte[] buffer, int byteOffset = 0)
        {
            _buffer = buffer;
            _byteOffset = byteOffset;
            _ReadUnit(this);
            _buffer = null;
        }

        /// <summary>
        /// Reads a UnitObject from the internal serialised byte array.
        /// </summary>
        private void _ReadUnit(UnitObject unit)
        {
            //// start of header
            // unit object versions
            unit.Version = _ReadBits(16);
            unit.Usage = _ReadBits(8);
            if (unit.Version != 0x00BF && unit.Version != 0x00CD) throw new Exceptions.NotSupportedVersionException("0x00BF or 0x00CD", "0x" + unit.Version.ToString("X4"));
            if (unit.Usage != 0x00 && unit.Usage != 0x02 && unit.Usage != 0x04) throw new Exceptions.NotSupportedVersionException("0x00 or 0x02", "0x" + unit.Usage.ToString("X2"));
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("UnitObject Version = {0} (0x{0:X4}), Usage = {1} (0x{1:X2})", unit.Version, unit.Usage));
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
                if (unit.BeginFlag != ObjectMagicWord && unit.BeginFlag != ItemMagicWord) throw new Exceptions.UnexpectedTokenException(ObjectMagicWord, unit.BeginFlag);
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
                unit.UnknownCount1F = _ReadBits(4) - 8;
                unit.UnknownCount1Fs = new UnknownCount1FClass[unit.UnknownCount1F];
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("unit.unknownCount1F = {0}", unit.UnknownCount1F));
                }

                for (int i = 0; i < unit.UnknownCount1F; i++)
                {
                    unit.UnknownCount1Fs[i] = new UnknownCount1FClass
                    {
                        Unknown1 = (short)_ReadBits(16), // table 0x6D ??
                        Unknown2 = (short)_ReadBits(16)  // table 0xB2 ??
                    };

                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("unit.unknownCount1Fs[{0}].Unknown1 = {1} (0x{1:X4}), unit.unknownCount1Fs[{0}].Unknown1 = {2} (0x{2:X4})",
                            i, unit.UnknownCount1Fs[i].Unknown1, unit.UnknownCount1Fs[i].Unknown2));
                    }
                }
            }


            // character flags
            if (_TestBit(unit.BitField, 0x20))
            {
                unit.PlayerFlagCount1 = _ReadBits(8);
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("unit.PlayerFlagCount1 = {0}", unit.PlayerFlagCount1));
                }

                for (int i = 0; i < unit.PlayerFlagCount1; i++)
                {
                    unit.PlayerFlags1.Add(_ReadInt16()); // table 0x4B??
                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("unit.PlayerFlags1[{0}] = {1} (0x{1:X4})", i, unit.PlayerFlags1[i]));
                    }
                }
            }
            //// end of header


            // dunno...
            if (_TestBit(unit.BitField, 0x1B))
            {
                unit.BitOffsetCount = _ReadBits(5);
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("unit._bitOffsetCount = {0}", unit.BitOffsetCount));
                }
                if (unit.BitOffsetCount > 1)
                {
                    throw new Exceptions.CharacterFileNotImplementedException("Unexpected unit._bitOffsetCount (> 1)!\nNot-Implemented cases. Please report this error and supply the offending file.");
                }

                for (int i = 0; i < unit.BitOffsetCount; i++)
                {
                    UnitBitOffsets bitOffsets = new UnitBitOffsets
                    {
                        Code = _ReadInt16(),
                        Offset = _ReadInt32()
                    };
                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("bitOffsets[{0}].Code = {1} (0x{1:X4}), bitOffsets[{0}].Offset = {2}", i, bitOffsets.Code, bitOffsets.Offset));
                    }

                    if (bitOffsets.Code != 0x3030)
                    {
                        throw new Exceptions.CharacterFileNotImplementedException("Unexpected value for bitOffsets.index (!= 0x3030)!\nNot-Implemented cases. Please report this error and supply the offending file.");
                    }
                    unit.BitOffsets.Add(bitOffsets);
                }
            }


            // dunno...
            if (_TestBit(unit.BitField, 0x05))
            {
                unit.Unknown05 = _ReadInt32();
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("unit.Unknown05 = {0} (0x{0:X4})", unit.Unknown05));
                }
            }


            // unit type/code
            unit.UnitType = _ReadBits(4);
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("unit.unitType = {0}", unit.UnitType));
            }
            if (unit.UnitType != 1 && unit.UnitType != 2 && unit.UnitType != 4 && unit.UnitType != 5)
            {
                throw new Exceptions.CharacterFileNotImplementedException("Unexpected value for unit.unitType (!= 1, 2, 4, 5)!\nNot-Implemented cases. Please report this warning and supply the offending file.");
            }
            unit.UnitCode = _ReadInt16();
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("unit.unitCode = {0} (0x{0:X4})", unit.UnitCode));
            }
            // if unit type == 1, table = 0x91
            //                 2, table = 0x77
            //                 4, table = 0x67
            //                 5, table = 0x7B


            // unit object id
            if (_TestBit(unit.BitField, 0x17))
            {
                if (unit.Version > 0xB2)
                {
                    unit.ObjectId = _ReadInt64();
                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("unit.ObjectId = {0} (0x{0:X16})", unit.ObjectId));
                    }

                    if (unit.ObjectId == 0)
                    {
                        throw new Exceptions.CharacterFileNotImplementedException("if (unit.ObjectId == 0)");
                    }
                }
            }


            // dunno
            if (_TestBit(unit.BitField, 0x03) || _TestBit(unit.BitField, 0x01))
            {
                unit.IsInventory = (_ReadBits(1) != 0);
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("unit.IsInventory = {0}", unit.IsInventory));
                }

                if (unit.IsInventory) // item is in inventory
                {
                    if (_TestBit(unit.BitField, 0x02))
                    {
                        unit.Unknown02 = _ReadBits(32);
                        if (_debugOutputLoadingProgress)
                        {
                            Debug.WriteLine(String.Format("unit.unknown_02 = {0}", unit.Unknown02));
                        }
                    }

                    unit.InventoryType = _ReadBits(16);
                    unit.InventoryPositionX = _ReadBits(12);
                    unit.InventoryPositionY = _ReadBits(12);
                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("inventoryType = {0}, inventoryPositionX = {1}, inventoryPositionY = {2}",
                            unit.InventoryType, unit.InventoryPositionX, unit.InventoryPositionY));
                    }

                    unit.Unknown0103Int64 = _ReadInt64();
                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("unit.unknown_01_03_4 = {0} (0x{0:X16})", unit.Unknown0103Int64));
                    }
                }
                else // item is a "world drop"
                {
                    unit.Unknown0103Int1 = _ReadInt32();

                    unit.Unknown0103Float11 = _ReadFloat();
                    unit.Unknown0103Float12 = _ReadFloat();
                    unit.Unknown0103Float13 = _ReadFloat();

                    unit.Unknown0103Float21 = _ReadFloat();
                    unit.Unknown0103Float22 = _ReadFloat();
                    unit.Unknown0103Float23 = _ReadFloat();

                    unit.Unknown0103Float31 = _ReadFloat();
                    unit.Unknown0103Float32 = _ReadFloat();
                    unit.Unknown0103Float33 = _ReadFloat();

                    unit.Unknown0103Int2 = _ReadBits(10);

                    unit.Unknown0103Float4 = _ReadFloat();

                    unit.Unknown0103Float5 = _ReadFloat();

                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("Unknown0103Int1 = {0}", unit.Unknown0103Int1));
                        Debug.WriteLine(String.Format("Unknown0103Float11 = {0}, Unknown0103Float12 = {1}, Unknown0103Float13 = {2}", unit.Unknown0103Float11, unit.Unknown0103Float12, unit.Unknown0103Float13));
                        Debug.WriteLine(String.Format("Unknown0103Float21 = {0}, Unknown0103Float22 = {1}, Unknown0103Float23 = {2}", unit.Unknown0103Float21, unit.Unknown0103Float22, unit.Unknown0103Float23));
                        Debug.WriteLine(String.Format("Unknown0103Float31 = {0}, Unknown0103Float32 = {1}, Unknown0103Float33 = {2}", unit.Unknown0103Float31, unit.Unknown0103Float32, unit.Unknown0103Float33));
                        Debug.WriteLine(String.Format("Unknown0103Int2 = {0}", unit.Unknown0103Int2));
                        Debug.WriteLine(String.Format("Unknown0103Float4 = {0}", unit.Unknown0103Float4));
                        Debug.WriteLine(String.Format("Unknown0103Float5 = {0}", unit.Unknown0103Float5));
                    }
                }
            }


            // dunno
            if (_TestBit(unit.BitField, 0x06))
            {
                unit.UnknownBool06 = (_ReadBits(1) != 0);
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("unit.unknownBool_06 = {0}", unit.UnknownBool06));
                }
                if (!unit.UnknownBool06)
                {
                    throw new Exceptions.CharacterFileNotImplementedException("if (unit.unknownBool_06 != 1");
                }
            }


            // on items only I think - Usually 0x00
            if (_TestBit(unit.BitField, 0x09))
            {
                unit.Unknown09 = _ReadBits(8);
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("unit.unknown_09 = {0} (0x{0:X2})", unit.Unknown09));
                }
            }


            // on character only
            if (_TestBit(unit.BitField, 0x07))
            {
                unit.CharacterHeight = _ReadByte();
                unit.CharacterWidth = _ReadByte();
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("CharacterHeight = {0}, CharacterWidth = {1}", unit.CharacterHeight, unit.CharacterWidth));
                }
            }


            // unknown - older versions only
            if (_TestBit(unit.BitField, 0x17))
            {
                if (unit.Version <= 0xB2)
                {
                    throw new Exceptions.CharacterFileNotImplementedException("if (_TestBit(unit.BitField, 0x17) && unit.MajorVersion <= 0xB2)");
                }
            }


            // on character only
            if (_TestBit(unit.BitField, 0x08))
            {
                unit.CharNameCount = _ReadBits(8);
                if (unit.CharNameCount > 0)
                {
                    int byteCount = unit.CharNameCount * 2; // is Unicode string without \0
                    unit.CharacterName = new char[byteCount];
                    for (int i = 0; i < byteCount; i++)
                    {
                        unit.CharacterName[i] = _ReadChar();
                    }
                    unit.Name = new String(unit.CharacterName);
                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("unit.Name = {0}", unit.Name));
                    }
                }
            }


            // on both character and items - appears to be always zero for items
            if (_TestBit(unit.BitField, 0x0A))
            {
                unit.PlayerFlagCount2 = _ReadBits(8);
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("unit._playerFlagCount2 = {0}", unit.PlayerFlagCount1));
                }

                if (unit.PlayerFlagCount2 > 0)
                {
                    for (int i = 0; i < unit.PlayerFlagCount2; i++)
                    {
                        unit.PlayerFlags2.Add(_ReadBits(16));
                        if (_debugOutputLoadingProgress)
                        {
                            Debug.WriteLine(String.Format("unit.PlayerFlags2[{0}] = {1} (0x{1:X4})", i, unit.PlayerFlags2[i]));
                        }
                    }
                }
            }



            if (unit.Usage > 2 && (unit.Usage <= 6 || unit.Usage != 7)) // so if == 0, 1, 2, 7, then *don't* do this
            {
                unit.UsageBool = _ReadBool();
                if (unit.UsageBool)
                {
                    unit.UsageBoolValue = _ReadBits(4);
                }

                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("unit.UsageBool = {0}, UsageBoolValue = {1}", unit.UsageBool, unit.UsageBoolValue));
                }
            }

            // <unknown bitfield 0x11th bit> - only seen as false anyways

            unit.IsDead = _ReadBool();
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("IsDead = {0}", unit.IsDead));
            }


            // unit stats
            if (_TestBit(unit.BitField, 0x0D))
            {
                if (_debugOutputLoadingProgress) Debug.WriteLine("==Has Stat Block==");

                unit.Stats = new StatBlock();
                _ReadStatBlock(unit.Stats, true);
            }
            else if (_TestBit(unit.BitField, 0x14))
            {
                unit.StatUnknownByte1 = _ReadByte(); // stats row 0x3C0?
                unit.StatUnknownByte2 = _ReadByte(); // stats row 0x347?
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("unit.StatUnknownByte1 = {0}, unit.StatUnknownByte2 = {1}", unit.StatUnknownByte1, unit.StatUnknownByte2));
                }

                if (_TestBit(unit.BitField, 0x1E))
                {
                    unit.StatUnknownInt1 = _ReadBits(3); // stats row 0x347? or 0x33A?
                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("unit.StatUnknownInt1 = {0}, ", unit.StatUnknownInt1));
                    }
                }
            }


            unit.HasAppearanceDetails = _ReadBool();
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("HasAppearanceDetails = {0}", unit.HasAppearanceDetails));
            }
            if (unit.HasAppearanceDetails)
            {
                _ReadAppearance(unit);
            }


            if (_TestBit(unit.BitField, 0x12))
            {
                unit.ItemEndBitOffset = _ReadInt32();
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


            if (_TestBit(unit.BitField, 0x1A))
            {
                unit.WeaponConfigFlag = _ReadUInt32();
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
                unit.WeaponConfigs = new UnitWeaponConfig[unit.WeaponConfigCount];
                for (int i = 0; i < unit.WeaponConfigCount; i++)
                {
                    UnitWeaponConfig weaponConfig = new UnitWeaponConfig
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
            //if (!_TestBit(unit.BitField, 0x00))
            //{
            unit.EndFlag = _ReadBits(32);
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("EndFlag = {0} (0x{0:X8})", unit.EndFlag));
            }

            if (unit.EndFlag != unit.BeginFlag && unit.EndFlag != ItemMagicWord)
            {
                int bitOffset = unit.BitCount - _bitOffset;
                int byteOffset = (_buffer.Length - _byteOffset) - (_bitOffset >> 3);
                throw new Exceptions.InvalidFileException("Flags not aligned!\nBit Offset: " + _bitOffset + "\nExpected: " + unit.BitCount + " (+" + bitOffset +
                                                          ")\nByte Offset: " + (_bitOffset >> 3) + "\nExpected: " + (_buffer.Length - _byteOffset) + " (+" + byteOffset + ")");
            }
            //}

            if (_TestBit(unit.BitField, 0x1D)) // no reading is done in here
            {
                //throw new Exceptions.CharacterFileNotImplementedException("if (!_TestBit(unit.BitField, 0x1D))");
            }

        }
        private void _ReadStatBlock(StatBlock statBlock, bool readNameCount)
        {
            statBlock.Version = _ReadBits(16);
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine("StatBlock Version = " + statBlock.Version);
            }
            if (statBlock.Version != 0x0A)
            {
                throw new Exceptions.NotSupportedVersionException("0x0A", "0x" + statBlock.Version.ToString("X2"));
            }

            statBlock.Usage = _ReadBits(3);
            statBlock.AdditionalStats = new List<UnitStatAdditional>();
            statBlock.stats = new List<StatBlock.Stat>();

            int additionalStatCount = _ReadBits(6);
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("statBlock.unknown1 = {0}, additionalStatCount = {1}", statBlock.Usage, additionalStatCount));
            }
            for (int i = 0; i < additionalStatCount; i++)
            {
                UnitStatAdditional additionalStat = new UnitStatAdditional
                {
                    Unknown = (short)_ReadBits(16),
                    StatCount = (short)_ReadBits(16),
                    Stats = new List<StatBlock.Stat>()
                };
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("additionalStat.Unknown = {0}, additionalStat.StatCount = {1}", additionalStat.Unknown, additionalStat.StatCount));
                }

                for (int j = 0; j < additionalStat.StatCount; j++)
                {
                    StatBlock.Stat unitStat = new StatBlock.Stat();
                    _ReadStat(statBlock, unitStat);

                    additionalStat.Stats.Add(unitStat);

                }

                statBlock.AdditionalStats.Add(additionalStat);
            }

            int statsCount = _ReadBits(16);
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("statsCount = {0}", statsCount));
            }
            for (int i = 0; i < statsCount; i++)
            {
                StatBlock.Stat unitStat = new StatBlock.Stat();
                _ReadStat(statBlock, unitStat);
                statBlock.stats.Add(unitStat);
            }


            if (!readNameCount) return;
            statBlock.NameCount = _ReadBits(8);
            statBlock.Names = new UnitStatName[statBlock.NameCount];
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("statBlock.nameCount = {0}", statBlock.NameCount));
            }

            for (int i = 0; i < statBlock.NameCount; i++)
            {
                UnitStatName name = new UnitStatName
                {
                    Unknown1 = (short)_ReadBits(16),
                    StatBlock = new StatBlock()
                };
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("name.Unknown1 = {0}", name.Unknown1));
                }

                _ReadStatBlock(name.StatBlock, false);
                statBlock.Names[i] = name;
            }
        }

        private void _ReadStat(StatBlock statBlock, StatBlock.Stat unitStat)
        {
            int statAttributeCount = 0;

            if (statBlock.Usage == 1)
            {
                unitStat.Row = _ReadBits(11); // not sure what this is... row? (pretty sure it's the row index)
                StatsBeta stat = FileManager.GetExcelStatsRowFromIndex(unitStat.Row);
                unitStat.BitCount = stat.valbits;
            }
            else
            {
                unitStat.Code = (short)_ReadBits(16);
                unitStat.Attributes = new List<StatBlock.Stat.Attribute>();

                statAttributeCount = _ReadBits(2);
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("unitStat.Code = {0} (0x{0:X4}), unitStat.statAttributeCount = {1}", unitStat.Code, statAttributeCount));
                }
                for (int i = 0; i < statAttributeCount; i++)
                {
                    StatBlock.Stat.Attribute exAtrib = new StatBlock.Stat.Attribute { Exists = _ReadBool() };
                    if (!exAtrib.Exists) break;

                    exAtrib.BitCount = _ReadBits(6);

                    exAtrib.Unknown1 = _ReadBits(2);
                    if (exAtrib.Unknown1 == 0x02)
                    {
                        exAtrib.Unknown11 = _ReadBool();
                    }

                    exAtrib.HasTableCode = _ReadBool();
                    if (!exAtrib.HasTableCode)
                    {
                        exAtrib.TableCode = _ReadInt16();
                    }

                    unitStat.Attributes.Add(exAtrib);

                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("exAtrib.BitCount = {0}, exAtrib.Unknown1 = {1}, exAtrib.Unknown11 = {2}, exAtrib.HasTableCode = {3}, exAtrib.TableCode = {4}",
                            exAtrib.BitCount, exAtrib.Unknown1, exAtrib.Unknown11, exAtrib.HasTableCode, exAtrib.TableCode));
                    }
                }

                unitStat.BitCount = _ReadBits(6);

                unitStat.OtherAttributeFlag = _ReadBits(3);
                unitStat.OtherAttribute = new UnitStatOtherAttribute();
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
                        unitStat.BitCount, unitStat.OtherAttributeFlag, unitStat.OtherAttribute.Unknown1, unitStat.OtherAttribute.Unknown2, unitStat.OtherAttribute.Unknown3));
                }

                unitStat.SkipResource = _ReadBits(2);
                if (unitStat.SkipResource == 0)
                {
                    unitStat.Resource = (short)_ReadBits(16);
                }

            }

            unitStat.RepeatFlag = (_ReadBits(1) != 0);
            unitStat.RepeatCount = 1;
            if (unitStat.RepeatFlag)
            {
                unitStat.RepeatCount = _ReadBits(10);
            }
            unitStat.values = new List<StatBlock.Stat.Values>();
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("unitStat.SkipResource = {0}, unitStat.Resource = {1}, unitStat.RepeatFlag = {2}, unitStat.RepeatCount = {3}",
                    unitStat.SkipResource, unitStat.Resource, unitStat.RepeatFlag, unitStat.RepeatCount));
            }

            for (int i = 0; i < unitStat.RepeatCount; i++)
            {
                StatBlock.Stat.Values statValues = new StatBlock.Stat.Values();

                for (int j = 0; j < statAttributeCount; j++)
                {
                    if (!unitStat.Attributes[j].Exists) continue;

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

                statValues.StatValue = _ReadBits(unitStat.BitCount);
                unitStat.values.Add(statValues);

                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("statValues.Attribute1 = {0}, statValues.Attribute2 = {1}, statValues.Attribute3 = {2}, statValues.Stat = {3}",
                        statValues.Attribute1, statValues.Attribute2, statValues.Attribute3, statValues.StatValue));
                }
            }
        }

        private void _ReadAppearance(UnitObject unit)
        {
            UnitAppearance appearance = new UnitAppearance();
            unit.Appearance = appearance;

            appearance.EquippedItemCount = _ReadBits(3);
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("appearance.equippedItemCount = {0}", appearance.EquippedItemCount));
            }
            for (int i = 0; i < appearance.EquippedItemCount; i++)
            {
                UnitAppearance.EquippedItem equippedItem = new UnitAppearance.EquippedItem();

                if (_TestBit(unit.BitField, 0x0F)) // reads in Int32
                {
                    throw new Exceptions.CharacterFileNotImplementedException("Unexpected bitField1 value! (0x0F == true)\nNot-Implemented cases. Please report this warning and supply the offending file.");
                }

                equippedItem.ItemCode = _ReadBits(16); // table 0x67?
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("equippedItem.ItemCode = {0} (0x{0:X4})", equippedItem.ItemCode));
                }

                if (_TestBit(unit.BitField, 0x22))
                {
                    equippedItem.Unknown = (byte)_ReadBitsShift(8);
                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("equippedItem.Unknown = {0}", equippedItem.Unknown));
                    }
                }

                if (_TestBit(unit.BitField, 0x18))
                {
                    int bitCount = (unit.Version > 0xC0) ? 4 : 3;
                    equippedItem.AffixCount = _ReadBits(bitCount);
                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("equippedItem.affixCount = {0}", equippedItem.AffixCount));
                    }
                    for (int j = 0; j < equippedItem.AffixCount; j++)
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


            appearance.Unknown1 = _ReadBits(16); // some sort of color Code 0x3430 = Cooper White LBlue or something
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("appearance.unknown1 = {0} (0x{0:X4})", appearance.Unknown1));
            }

            if (_TestBit(unit.BitField, 0x16))
            {
                appearance.Unknown16 = _ReadInt64();
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("appearance.Unknown16 = {0} (0x{0:X16})", appearance.Unknown16));
                }
            }

            if (_TestBit(unit.BitField, 0x23))
            {
                appearance.Unknown23 = _ReadInt16();
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("appearance.Unknown23 = {0} (0x{0:X16})", appearance.Unknown23));
                }
            }

            if (_TestBit(unit.BitField1, 0x11))
            {
                appearance.WardrobeLayerHeadCount = _ReadBits(4);
                for (int i = 0; i < appearance.WardrobeLayerHeadCount; i++)
                {
                    appearance.WardrobeLayersHead.Add(_ReadBits(16)); // table 0x62
                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("appearance.WardrobeLayersHead[{0}] = {1} (0x{1:X4})", i, appearance.WardrobeLayersHead[i]));
                    }
                }

                appearance.WardrobeAppearanceGroupCount = _ReadBits(3); // max of 0x0A
                for (int i = 0; i < appearance.WardrobeAppearanceGroupCount; i++)
                {
                    appearance.WardrobeAppearanceGroups.Add(_ReadBits(16)); // table 0x5A
                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("appearance.WardrobeAppearanceGroups[{0}] = {1} (0x{1:X4})", i, appearance.WardrobeAppearanceGroups[i]));
                    }
                }

                appearance.ColorCount = _ReadBits(4); // max of 0x03
                for (int i = 0; i < appearance.ColorCount; i++)
                {
                    appearance.ColorPaletteIndicies.Add(_ReadBits(8));
                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("appearance.ColorPaletteIndicies[{0}] = {1} (0x{1:X2})", i, appearance.ColorPaletteIndicies[i]));
                    }
                }
            }


            if (_TestBit(unit.BitField1, 0x10))
            {
                appearance.WardrobeLayerCount = _ReadBits(16);
                for (int i = 0; i < appearance.WardrobeLayerCount; i++)
                {
                    UnitAppearance.ModelWardrobeLayer gear = new UnitAppearance.ModelWardrobeLayer
                    {
                        ItemCode = _ReadUInt16(), // table 0x62
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

        private unsafe float _ReadFloat()
        {
            int val = _ReadBits(32);
            return (*(float*)&val);
        }

        private Int32 _ReadInt32()
        {
            return _ReadBits(32);
        }

        private UInt32 _ReadUInt32()
        {
            return (UInt32)_ReadBits(32);
        }

        private Int16 _ReadInt16()
        {
            return (Int16)_ReadBits(16);
        }

        private UInt16 _ReadUInt16()
        {
            return (UInt16)_ReadBits(16);
        }

        private char _ReadChar()
        {
            return (char)_ReadBits(8);
        }

        private byte _ReadByte()
        {
            return (byte)_ReadBits(8);
        }

        private bool _ReadBool()
        {
            return (_ReadBits(1) != 0);
        }

        private int _ReadBitsShift(int bitCount)
        {
            int val = _ReadBits(bitCount);
            int shift = (1 << (bitCount - 1));
            return val - shift;
        }

        private int _ReadBits(int bitCount)
        {
            int bitsToRead = bitCount;
            int byteOffset = _bitOffset >> 3;
            int b = _buffer[_byteOffset + byteOffset];

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

                b = _buffer[_byteOffset + byteOffset + i];
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

        private static bool _TestBit(ulong bitField, Bits bit)
        {
            const ulong bitMask = 1;
            return ((bitField & (bitMask << (int)bit)) != 0);
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

        public byte[] GenerateItemDrop()
        {
            _usingExternalBuffer = false;
            _buffer = new byte[1024];
            _byteOffset = 0;
            _bitOffset = 0;

            _WriteUnit(this);

            int byteCount = (_bitOffset >> 3) + 1;
            byte[] returnBuffer = new byte[byteCount];
            Buffer.BlockCopy(_buffer, 0, returnBuffer, 0, byteCount);
            return returnBuffer;
        }

        public void GenerateItemDrop(byte[] buffer, int offset)
        {
            _usingExternalBuffer = true;
            _buffer = buffer;
            _byteOffset = offset;
            _bitOffset = 0;

            _WriteUnit(this);
        }

        private enum Bits
        {
            // the order is around what they're actually read/checked in loading
            Bit1DBitCountEof = 0x1D,
            Bit00FlagAlignment = 0x00,
            Bit1CTimeStamps = 0x1C,
            Bit1FUnknown = 0x1F,
            Bit20CharacterFlags1 = 0x20,
            Bit1BBitOffsets = 0x1B,
            Bit05Unknown = 0x05,
            Bit17Unknown = 0x17,
            Bit03Unknown = 0x03,
            Bit01Unknown = 0x01,
            Bit02Unknown = 0x02,
            Bit06Unknown = 0x06,
            Bit09Unknown = 0x09,
            Bit07Unknown = 0x07,
            Bit08Unknown = 0x08,
            Bit0ACharacterFlags2 = 0x0A,
            Bit0DStats = 0x0D,
            Bit14Stats = 0x14, // i think it's another stats things
            Bit1EInsideStats14 = 0x1E,
            Bit0FUnknown = 0x0F,
            Bit22Unknown = 0x22,
            Bit18Unknown = 0x18,
            Bit16Unknown = 0x16,
            Bit23Unknown = 0x23,
            Bit11Unknown = 0x11,
            Bit10Unknown = 0x10,
            Bit12Items = 0x12,
            Bit1AWeaponConfig = 0x1A,
        }

        private void _WriteUnit(UnitObject unit)
        {
            /***** Unit Header *****
             * Version                                          16                  Should be 0x00BF for SP client, 0x00CD for Resurrection client.
             * Usage                                            8                   0 for SP. For Resurrection client; 2 for char select, 4 for item drop
             * bitFieldCount                                    8                   Must be <= 2. I haven't tested with it != 2 though.
             * {
             *      bitField                                    32                  Each bit determines if 'x' is read in or not.
             * }
             * 
             * if (TestBit(bitField, 0x1D))
             * {
             *      bitCount                                    32                  Bit count of entire unit object.
             * }
             * 
             * if (TestBit(bitField, 0x00))
             * {
             *      BeginFlag                                   32                  Flags used to check data/position alignment.
             * }
             * 
             * if (TestBit(bitField, 0x1C))
             * {
             *      timeStamp1                                  32                  I'm not actually sure what these three things are but they can be set to 0x00000000
             *      timeStamp2                                  32                  and it will still be loaded fine. I call them time stamps simply because the first
             *      timeStamp3                                  32                  one changes every time the file is saved.
             * }
             * 
             * if (TestBit(bitField, 0x1F))
             * {
             *      unknownCount                                4                   count + 8 is what must be written. e.g. If count = 3, then write 11.
             *      {
             *          unknown                                 16                  // TO BE DETERMINED
             *          unknown                                 16                  // TO BE DETERMINED
             *      }
             * }
             * 
             * if (TestBit(bitField, 0x20))
             * {
             *      characterFlagCount                          8                   Character state flags
             *      {                                                               e.g. Elite, Hardcore, Hardcore Dead, etc.
             *          characterFlag                           16                  However it should be noted that the game doesn't actually appear to use these.
             *      }                                                               It uses the ones located further down.
             * }
             * 
             ***** Unit Body *****                                                  (the header "chunk" is read in its own function in-game - hence I call it a header, lol)
             *
             * if (TestBit(bitField, 0x1B))
             * {
             *      BitOffsetsCount                             5                   Count of following block.
             *      {
             *          Code                                    16                  Code value of offset - code is from unknown excel table.
             *          Offset                                  32                  Bit Offset value
             *      }
             * }
             * 
             * if (TestBit(bitField, 0x05))
             * {
             *      Unknown                                     32                  Seen in MP only - unknown usage.
             * }
             * 
             * UnitType                                         4                   The type of unit... Is this from UnitType table?
             * UnitCode                                         16                  The code value of the unit.
             * 
             * if (TestBit(bitField, 0x17))
             * {                                                                    This chunk is read in by a secondary non-standard function.
             *      ObjectId                                    64                  Contains a unique id for this unit object (e.g. item hash id to stop duping, etc).
             * }
             * 
             * if (TestBit(bitField, 0x03) || TestBit(bitField, 0x01))
             * {
             *      IsInventory                                 1                   Is true if UnitObject is item in inventory. False if item is a "world drop" (i.e. drop from monster)
             *      {
             *          if (TestBit(bitField, 0x02))
             *          {
             *              Unknown                             32                  // UNTESTED
             *          }
             *      
             *          InventoryType                           16                  Inventory item is within.
             *          InventoryPositionX                      12                  X Position of item in inventory.
             *          InventoryPositionY                      12                  Y Position of item in inventory.
             *          
             *          Unknown                                 64                  Non-standard function reading - unknown usage.
             *      }
             *      else
             *      {
             *          Unknown0103Int1                         32                  These values have something to do with an "item dropped" world positioning etc
             *                                                                      i.e. only applicable to MP clients.
             *          Unknown0103Float11                      32
             *          Unknown0103Float12                      32
             *          Unknown0103Float13                      32
             *          
             *          Unknown0103Float21                      32
             *          Unknown0103Float22                      32
             *          Unknown0103Float23                      32
             *          
             *          Unknown0103Float31                      32
             *          Unknown0103Float32                      32
             *          Unknown0103Float33                      32
             *          
             *          Unknown0103Int2                         10
             *          
             *          Unknown0103Float5                       32
             *          
             *          Unknown0103Float6                       32
             *      }
             * }
             * 
             * if (TestBit(bitField, 0x06))
             * {
             *      unknownBool                                 1                   // TO BE DETEREMINED
             * }                                                                    // If exists has always been 1.
             * 
             * if (TestBit(bitField, 0x09))
             * {
             *      unknown                                     8                   // TO BE DETERMINED
             * }
             * 
             * if (TestBit(bitField, 0x07))
             * {
             *      CharacterHeight                             8                   Height of character.
             *      CharacterWidth                              8                   Width of character.
             * }
             * 
             * if (TestBit(bitField, 0x08))
             * {
             *      characterCount                              8                   Number of (unicode) characters in following string.
             *      characterName                               8*2*count           Character's name in Unicode (no \0) - doesn't appear to be actually used in-game...
             * }
             * 
             * if (TestBit(bitField, 0x0A))
             * {
             *      characterFlagCount                          8                   Character state flags
             *      {                                                               e.g. Elite, Hardcore, Hardcore Dead, etc.
             *          characterFlag                           16                  These flags actually affect in-game (unlike the previous set which
             *      }                                                               appear to be unused).
             * }
             * 
             * if (Usage > 2 && (Usage <= 6 || Usage != 7))                         These would be applicable to MP only - SP has Usage == 0
             * {
             *      UsageBool                                   1                   True if following value is present.
             *      {
             *          Unknown                                 4                   // TO BE DETERMINED
             *      }
             * }
             * 
             * IsDead                                           1                   Does NOT influence HC dead! If the character died right before saving this flag is set to 1
             * 
             * if (TestBit(bitField, 0x0D))
             * {
             *      UNIT STAT BLOCK                                                 See WriteStatBlock().
             * }
             * 
             * HasAppearanceDetails                             1                   Bool type.
             * {
             *      EquippedItemCount                           3                   Count of equipped items.
             *      {
             *          if (TestBit(bitField, 0x0F))
             *          {
             *              Unknown                             32                  // UNTESTED
             *          }
             *          
             *          ItemCode                                16                  Code of item equipped.
             *          
             *          if (TestBit(bitField, 0x22))
             *          {
             *              Unknown                             8                   Read from ReadBitsShift()
             *          }
             *          
             *          if (TestBit(bitField, 0x18))
             *          {
             *              AffixCount                          3 (+1 Ver > 0xC0)   Count of affix codes.
             *              {
             *                  Code                            32                  Code of affix.
             *              }
             *          }
             *      }
             *      
             *      Unknown                                     16                  // TO BE DETEREMINED
             *      
             *      if (TestBit(bitField, 0x16))
             *      {
             *          Unknown                                 64                  Non-standard function reading (as above).
             *      }
             *      
             *      if (TestBit(bitField, 0x23))
             *      {
             *          Unknown                                 16                  // TO BE DETEREMINED
             *      }
             *      
             *      if (TestBit(bitField, 0x11))
             *      {
             *          WardrobeLayerHeadCount                  4                   Count of WardrobeLayers for Head
             *          {
             *              Code                                16                  Code of WardrobeLayer.
             *          }
             *          
             *          WardrobeAppearanceGroupCount            3                   Count of model appearance parts.
             *          {
             *              Code                                16                  Code of WardrobeAppearanceGroup - Order is: body, head, hair, face accessory. 
             *          }
             *          
             *          ColorCount                              4                   Count of following block.
             *          {
             *              ColorPaletteIndicies                8                   Not sure... Is this row index? Code?
             *          }
             *      }
             *      
             *      if (TestBit(bitField, 0x10))
             *      {
             *          WardrobeLayerCount                      16                  Count of equipped gears.
             *          {
             *              Code                                16                  Code of equipped item wardrobe layer.
             *              UnknownBool                         1                   Bool type.
             *              {
             *                  Unknown                         2                   // TO BE DETEREMINED
             *              }
             *          }
             *      }
             * }
             * 
             * 
             * ItemBitOffset                                    32                  Bit offset to end of all item blocks.
             * ItemCount                                        10                  Count of items.
             * {
             *      ITEMS
             * }
             * 
             * 
             * if (TestBit(bitField1, 0x1A))
             * {
             *      WeaponConfigFlag                            32                  Must be 0x91103A74.
             *      
             *      EndFlagBitOffset                            32                  Bit offset to end flag.
             *      WeaponConfigCount                           6                   Count of weapon configs.
             *      {
             *          Code                                    16                  Code of weapon.
             *          UnknownCount                            4                   Count of following block - Must be 0x02.
             *          {
             *              Exists                              1                   Bool type.
             *              {
             *                  Unknown                         32                  // TO BE DETEREMINED
             *              }
             *          }
             *          
             *          UnknownCount                            4                   Count of...?
             *          {
             *              Exists                              1                   Bool type.
             *              {
             *                  Unknown                         32                  // TO BE DETEREMINED
             *              }
             *          }
             *          
             *          Code                                    32                  Code of something...?
             *      }
             * }
             * 
             * if (TestBit(bitField, 0x00))
             * {
             *      EndFlag                                     32                  Flags used to check data/position alignment.
             * }
             */

            int bitCountEofOffset = -1;
            bool isItem = (unit.Usage == 4);

            /***** Unit Header *****/

            int bitCountStart = _bitOffset;

            _WriteBits(unit.Version, 16);
            _WriteBits(unit.Usage, 8);
            _WriteBits(0x02, 8);
            {
                _WriteNonStandardFunc(unit.BitField);
            }

            if (_TestBit(unit.BitField, Bits.Bit1DBitCountEof))
            {
                bitCountEofOffset = _bitOffset;
                _WriteInt32(0x00000000);
            }

            if (_TestBit(unit.BitField, Bits.Bit00FlagAlignment))
            {
                _WriteUInt32(isItem ? ItemMagicWord : ObjectMagicWord);
            }

            if (_TestBit(unit.BitField, Bits.Bit1CTimeStamps))
            {
                _WriteInt32(unit.TimeStamp1);
                _WriteInt32(unit.TimeStamp2);
                _WriteInt32(unit.TimeStamp3);
            }

            if (_TestBit(unit.BitField, Bits.Bit1FUnknown))
            {
                _WriteBits(unit.UnknownCount1F + 8, 4);
                for (int i = 0; i < unit.UnknownCount1F; i++)
                {
                    UnknownCount1FClass uc1F = unit.UnknownCount1Fs[i];
                    _WriteInt16(uc1F.Unknown1);
                    _WriteInt16(uc1F.Unknown2);
                }
            }

            if (_TestBit(unit.BitField, Bits.Bit20CharacterFlags1) && !isItem)
            {
                int playerFlagCount = unit.PlayerFlags1.Count;

                _WriteBits(playerFlagCount, 8);
                for (int i = 0; i < playerFlagCount; i++)
                {
                    _WriteInt16(unit.PlayerFlags1[i]);
                }
            }

            /***** Unit Body *****/

            int bitOffsetItemEndBitOffset = 0;
            if (_TestBit(unit.BitField, Bits.Bit1BBitOffsets))
            {
                int bitOffsetsCount = unit.BitOffsets.Count;

                _WriteBits(bitOffsetsCount, 5);
                for (int i = 0; i < bitOffsetsCount; i++)
                {
                    _WriteInt16(unit.BitOffsets[i].Code);
                    bitOffsetItemEndBitOffset = _bitOffset;
                    _WriteInt32(0x00000000);
                }
            }

            if (_TestBit(unit.BitField, Bits.Bit05Unknown))
            {
                _WriteInt32(unit.Unknown05);
            }

            _WriteBits(unit.UnitType, 4);
            _WriteInt16(unit.UnitCode);

            if (_TestBit(unit.BitField, Bits.Bit17Unknown))
            {
                _WriteNonStandardFunc(unit.ObjectId);
            }

            if (_TestBit(unit.BitField, Bits.Bit01Unknown) || _TestBit(unit.BitField, Bits.Bit03Unknown))
            {
                _WriteBool(unit.IsInventory);
                if (unit.IsInventory) // item is in inventory
                {
                    if (_TestBit(unit.BitField, Bits.Bit02Unknown))
                    {
                        _WriteInt32(unit.Unknown02);
                    }

                    _WriteBits(unit.InventoryType, 16);
                    _WriteBits(unit.InventoryPositionX, 12);
                    _WriteBits(unit.InventoryPositionY, 12);

                    _WriteNonStandardFunc(unit.Unknown0103Int64);
                }
                else // item is a "world drop"
                {
                    _WriteInt32(unit.Unknown0103Int1);

                    _WriteFloat(unit.Unknown0103Float11);
                    _WriteFloat(unit.Unknown0103Float12);
                    _WriteFloat(unit.Unknown0103Float13);

                    _WriteFloat(unit.Unknown0103Float21);
                    _WriteFloat(unit.Unknown0103Float22);
                    _WriteFloat(unit.Unknown0103Float23);

                    _WriteFloat(unit.Unknown0103Float31);
                    _WriteFloat(unit.Unknown0103Float32);
                    _WriteFloat(unit.Unknown0103Float33);

                    _WriteBits(unit.Unknown0103Int2, 10);

                    _WriteFloat(unit.Unknown0103Float4);

                    _WriteFloat(unit.Unknown0103Float5);
                }
            }

            if (_TestBit(unit.BitField, Bits.Bit06Unknown))
            {
                _WriteBool(unit.UnknownBool06);
                if (!unit.UnknownBool06)
                {
                    throw new Exceptions.CharacterFileNotImplementedException("_WriteUnit : if (!unit.UnknownBool06)");
                }
            }

            if (_TestBit(unit.BitField, Bits.Bit09Unknown))
            {
                _WriteByte(unit.Unknown09);
            }

            if (_TestBit(unit.BitField, Bits.Bit07Unknown))
            {
                _WriteByte(unit.CharacterHeight);
                _WriteByte(unit.CharacterWidth);
            }

            if (_TestBit(unit.BitField, Bits.Bit08Unknown) && !String.IsNullOrEmpty(unit.Name))
            {
                _WriteBits(unit.Name.Length / 2, 8); // is Unicode string without \0
                foreach (char c in unit.Name) // warning: must be a Unicode string
                {
                    _WriteBits(c, 8);
                }
            }

            if (_TestBit(unit.BitField, Bits.Bit0ACharacterFlags2))
            {
                int playerFlagCount = unit.PlayerFlags2.Count;

                _WriteBits(playerFlagCount, 8);
                for (int i = 0; i < playerFlagCount; i++)
                {
                    _WriteBits(unit.PlayerFlags2[i], 16);
                }
            }

            if (unit.Usage > 2 && (unit.Usage <= 6 || unit.Usage != 7)) // so if == 0, 1, 2, 7, then *don't* do this
            {
                _WriteBool(unit.UsageBool);
                if (unit.UsageBool)
                {
                    _WriteBits(unit.UsageBoolValue, 4);
                }
            }

            _WriteBool(unit.IsDead);

            if (_TestBit(unit.BitField, Bits.Bit0DStats))
            {
                _WriteStatBlock(unit.Stats, true);
            }
            else if (_TestBit(unit.BitField, Bits.Bit14Stats))
            {
                _WriteByte(unit.StatUnknownByte1);
                _WriteByte(unit.StatUnknownByte2);

                if (_TestBit(unit.BitField, Bits.Bit1EInsideStats14))
                {
                    _WriteBits(unit.StatUnknownInt1, 3);
                }
            }

            _WriteBool(unit.HasAppearanceDetails);
            if (unit.HasAppearanceDetails)
            {
                int equippedItemCount = unit.Appearance.EquippedItems.Count;
                _WriteBits(equippedItemCount, 3);
                for (int i = 0; i < equippedItemCount; i++)
                {
                    UnitAppearance.EquippedItem equippedItem = unit.Appearance.EquippedItems[i];

                    if (_TestBit(unit.BitField, Bits.Bit0FUnknown))
                    {
                        throw new Exceptions.CharacterFileNotImplementedException("Unexpected bitField1 value! (0x0F == true)\nNot-Implemented cases. Please report this warning and supply the offending file.");
                    }

                    _WriteBits(equippedItem.ItemCode, 16);

                    if (_TestBit(unit.BitField, Bits.Bit22Unknown))
                    {
                        _WriteBitsShift(equippedItem.Unknown, 8);
                    }

                    if (_TestBit(unit.BitField, Bits.Bit18Unknown))
                    {
                        int bitCount = (unit.Version > 0xC0) ? 4 : 3;
                        int affixCount = equippedItem.AffixCodes.Count;
                        _WriteBits(affixCount, bitCount);
                        for (int j = 0; j < affixCount; j++)
                        {
                            _WriteBits(equippedItem.AffixCodes[j], 32);
                        }
                    }
                }

                _WriteBits(unit.Appearance.Unknown1, 16);

                if (_TestBit(unit.BitField, Bits.Bit16Unknown))
                {
                    _WriteNonStandardFunc(unit.Appearance.Unknown16);
                }

                if (_TestBit(unit.BitField, Bits.Bit23Unknown))
                {
                    _WriteInt16(unit.Appearance.Unknown23);
                }

                if (_TestBit(unit.BitField, Bits.Bit11Unknown))
                {
                    int wardrobeLayerCount = unit.Appearance.WardrobeLayersHead.Count;
                    _WriteBits(wardrobeLayerCount, 4);
                    for (int i = 0; i < wardrobeLayerCount; i++)
                    {
                        _WriteBits(unit.Appearance.WardrobeLayersHead[i], 16);
                    }

                    int wardrobeAppearanceGroupCount = unit.Appearance.WardrobeAppearanceGroups.Count;
                    _WriteBits(wardrobeAppearanceGroupCount, 3);
                    for (int i = 0; i < wardrobeAppearanceGroupCount; i++)
                    {
                        _WriteBits(unit.Appearance.WardrobeAppearanceGroups[i], 16);
                    }

                    int colorPaletteCount = unit.Appearance.ColorPaletteIndicies.Count;
                    _WriteBits(colorPaletteCount, 4);
                    for (int i = 0; i < colorPaletteCount; i++)
                    {
                        _WriteBits(unit.Appearance.ColorPaletteIndicies[i], 8);
                    }
                }

                if (_TestBit(unit.BitField, Bits.Bit10Unknown))
                {
                    _WriteBits(unit.Appearance.WardrobeLayerCount, 16);
                    for (int i = 0; i < unit.Appearance.WardrobeLayerCount; i++)
                    {
                        _WriteBits(unit.Appearance.WardrobeLayers[i].ItemCode, 16);
                        _WriteBits(unit.Appearance.WardrobeLayers[i].UnknownBool ? 1 : 0, 1);
                        if (unit.Appearance.WardrobeLayers[i].UnknownBool)
                        {
                            _WriteBits(unit.Appearance.WardrobeLayers[i].UnknownBoolValue, 2);
                        }
                    }
                }
            }


            if (_TestBit(unit.BitField, Bits.Bit12Items))
            {
                int itemBitOffset = _bitOffset;
                _WriteBits(0x00000000, 32);

                int itemCount = unit.Items.Count;
                _WriteBits(itemCount, 10);
                for (int i = 0; i < itemCount; i++)
                {
                    _WriteUnit(unit.Items[i]);
                }

                _WriteBits(_bitOffset, 32, itemBitOffset);
                if (bitOffsetItemEndBitOffset > 0)
                {
                    _WriteBits(_bitOffset, 32, bitOffsetItemEndBitOffset);
                }
            }


            if (_TestBit(unit.BitField, Bits.Bit1AWeaponConfig))
            {
                _WriteUInt32(WeaponConfigMagicWord);

                int endFlagBitOffset = _bitOffset;
                _WriteBits(0x00000000, 32);

                int weaponConfigCount = unit.WeaponConfigs.Length;
                _WriteBits(weaponConfigCount, 6);
                for (int i = 0; i < weaponConfigCount; i++)
                {
                    UnitWeaponConfig weaponConfig = unit.WeaponConfigs[i];

                    _WriteBits(weaponConfig.Id, 16);
                    _WriteBits(weaponConfig.UnknownCount1, 4);
                    for (int j = 0; j < weaponConfig.UnknownCount1; j++)
                    {
                        _WriteBits(weaponConfig.Exists1[j] ? 1 : 0, 1);
                        if (weaponConfig.Exists1[j])
                        {
                            _WriteBits(weaponConfig.UnknownIds1[j], 32);
                        }
                    }

                    // yes this chunk looks the same as above - the above chunk though is in a specific function and can differ at 1 point
                    // also, it can be != 2
                    _WriteBits(weaponConfig.UnknownCount2, 4);
                    for (int j = 0; j < weaponConfig.UnknownCount2; j++)
                    {
                        _WriteBits(weaponConfig.Exists2[j] ? 1 : 0, 1);
                        if (weaponConfig.Exists2[j])
                        {
                            _WriteBits(weaponConfig.UnknownIds2[j], 32);
                        }
                    }

                    _WriteBits(weaponConfig.IdAnother, 32);
                }

                _WriteBits(_bitOffset, 32, endFlagBitOffset);
            }


            if (_TestBit(unit.BitField, Bits.Bit00FlagAlignment))
            {
                _WriteBits(0x2B523460, 32); // "`4R+"
            }

            if (_TestBit(unit.BitField, Bits.Bit1DBitCountEof))
            {
                _WriteBits(_bitOffset - bitCountStart, 32, bitCountEofOffset);
            }
        }

        private void _WriteNonStandardFunc(UInt64 val)
        {
            byte[] byteArray = BitConverter.GetBytes(val); // todo: this could easily be done as two UInt32 writes
            for (int i = 0; i < 8; i++)
            {
                _WriteBits(byteArray[i], 8);
            }
        }

        private void _WriteNonStandardFunc(Int64 val)
        {
            byte[] byteArray = BitConverter.GetBytes(val); // todo: this could easily be done as two Int32 writes
            for (int i = 0; i < 8; i++)
            {
                _WriteBits(byteArray[i], 8);
            }
        }

        private void _WriteStatBlock(StatBlock statBlock, bool writeNameCount)
        {
            /***** Stat Block Header *****
             * Version                                          16                  Stat block header - Must be 0x000A.
             * Usage                                            3                   Use as 0 for SP, 1 for MP item drops
             * 
             * AdditionalStatCount                              6                   Additional Stats - Not sure of use yet.
             * {
             *      Unknown                                     16                  // TO BE DETEREMINED
             *      StatCount                                   16                  Count of following stats.
             *      {
             *          STATS                                                       See WriteStat().
             *      }
             * }
             * 
             * StatCount                                        16                  Count of following stats.
             * {
             *      STATS                                                           See WriteStat().
             * }
             * 
             * if (WriteNameCount)                                                  This is TRUE by default. Set to FALSE when writing a stat block
             * {                                                                    from the below name stat block chunk.
             *      NameCount                                   8                   I think this has something to do with item names.
             *      {
             *          Unknown                                 16                  // TO BE DETEREMINED
             *          STAT BLOCK                                                  See WriteStatBlock().
             *      }
             * }
             */

            _WriteBits(statBlock.Version, 16);
            _WriteBits(statBlock.Usage, 3);

            _WriteBits(statBlock.AdditionalStats.Count, 6);
            foreach (UnitStatAdditional additionalStat in statBlock.AdditionalStats)
            {
                _WriteBits(additionalStat.Unknown, 16);
                _WriteBits(additionalStat.Stats.Count, 16);

                foreach (StatBlock.Stat stat in additionalStat.Stats)
                {
                    _WriteStat(statBlock, stat);
                }
            }

            _WriteBits(statBlock.stats.Count, 16);
            foreach (StatBlock.Stat stat in statBlock.stats)
            {
                _WriteStat(statBlock, stat);
            }

            if (!writeNameCount) return;

            _WriteBits(statBlock.Names.Length, 8);
            foreach (UnitStatName statName in statBlock.Names)
            {
                _WriteBits(statName.Unknown1, 16);
                _WriteStatBlock(statName.StatBlock, false);
            }
        }

        private void _WriteStat(StatBlock statBlock, StatBlock.Stat stat)
        {
            /***** Stat Block *****
             * Code                                             16                  Code from Stats excel table.
             * 
             * extraAttributesCount                             2                   Count of following.
             * {
             *      Exists                                      1                   Simple bool test.
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
             *              ResourceCode                        16                  Like StatCode, refers to some value in an excel table.
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
             * RepeatFlag                                       1                   Bool type.
             * {
             *      RepeatCount                                 10                  Number of times to read in stat values.
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

            if (statBlock.Usage == 1)
            {
                _WriteBits(stat.Row, 11);
                StatsBeta statRow = FileManager.GetExcelStatsRowFromIndex(stat.Row);
                stat.BitCount = statRow.valbits;
            }
            else
            {
                _WriteBits(stat.Code, 16);
                _WriteBits(stat.Attributes.Count, 2);
                foreach (StatBlock.Stat.Attribute attribute in stat.Attributes)
                {
                    _WriteBool(attribute.Exists);
                    if (!attribute.Exists) break;

                    _WriteBits(attribute.BitCount, 6);

                    _WriteBits(attribute.Unknown1, 2);
                    if (attribute.Unknown1 == 0x02)
                    {
                        _WriteBool(attribute.Unknown11);
                    }

                    _WriteBool(attribute.HasTableCode);
                    if (!attribute.HasTableCode)
                    {
                        _WriteBits(attribute.TableCode, 16);
                    }
                }

                _WriteBits(stat.BitCount, 6);

                _WriteBits(stat.OtherAttributeFlag, 3);
                if ((stat.OtherAttributeFlag & 0x01) > 0)
                {
                    _WriteBits(stat.OtherAttribute.Unknown1, 4);
                }
                if ((stat.OtherAttributeFlag & 0x02) > 0)
                {
                    _WriteBits(stat.OtherAttribute.Unknown2, 12);
                }
                if ((stat.OtherAttributeFlag & 0x04) > 0)
                {
                    _WriteBits(stat.OtherAttribute.Unknown3, 1);
                }

                _WriteBits(stat.SkipResource, 2);
                if (stat.SkipResource == 0)
                {
                    _WriteBits(stat.Resource, 16);
                }
            }

            int repeatCount = stat.values.Count;
            stat.RepeatFlag = (repeatCount > 1);

            _WriteBool(stat.RepeatFlag);
            if (stat.RepeatFlag)
            {
                _WriteBits(repeatCount, 10);
            }

            for (int i = 0; i < repeatCount; i++)
            {
                for (int j = 0; j < stat.Attributes.Count; j++)
                {
                    if (!stat.Attributes[j].Exists) continue;

                    int extraAttribute = 0;
                    switch (j)
                    {
                        case 0:
                            extraAttribute = stat.values[i].Attribute1;
                            break;
                        case 1:
                            extraAttribute = stat.values[i].Attribute2;
                            break;
                        case 2:
                            extraAttribute = stat.values[i].Attribute3;
                            break;
                    }

                    _WriteBits(extraAttribute, stat.Attributes[j].BitCount);
                }

                _WriteBits(stat.values[i].StatValue, stat.BitCount);
            }
        }

        private void _WriteBool(bool value)
        {
            _WriteBits(value ? 1 : 0, 1);
        }

        private void _WriteByte(int value)
        {
            _WriteBits(value, 8);
        }

        private void _WriteByte(byte value)
        {
            _WriteBits(value, 8);
        }

        private void _WriteInt16(Int16 value)
        {
            _WriteBits(value, 16);
        }

        private unsafe void _WriteFloat(float value)
        {
            int intVal = *(int*)&value;
            _WriteBits(intVal, 32);
        }

        private void _WriteInt32(Int32 value)
        {
            _WriteBits(value, 32);
        }

        private void _WriteUInt32(UInt32 value)
        {
            _WriteBits((Int32)value, 32);
        }

        private void _WriteBitsShift(int value, int bitCount)
        {
            int shift = (1 << (bitCount - 1));
            value += shift;
            _WriteBits(value, bitCount);
        }

        private void _WriteBits(int value, int bitCount)
        {
            _WriteBits(value, bitCount, _bitOffset, true);
        }

        private void _WriteBits(int value, int bitCount, int bitOffset, bool setIncrementOffset = false)
        {
            int byteOffset = bitOffset >> 3;
            if (!_usingExternalBuffer && byteOffset > _buffer.Length - 10)
            {
                byte[] newData = new byte[_buffer.Length + 1024];
                Buffer.BlockCopy(_buffer, 0, newData, 0, _buffer.Length);
                _buffer = newData;
            }

            int bitsToWrite = bitCount;
            int offsetBitsInFirstByte = bitOffset & 0x07;
            int bitByteOffset = 0x08 - offsetBitsInFirstByte;

            int bitsInFirstByte = bitCount;
            if (bitByteOffset < bitCount) bitsInFirstByte = bitByteOffset;

            int bytesToWriteTo = (bitsToWrite + 0x07 + offsetBitsInFirstByte) >> 3;

            for (int i = 0; i < bytesToWriteTo; i++, byteOffset++)
            {
                int bitLevel = 0;
                if (offsetBitsInFirstByte > 0 && i > 0)
                {
                    bitLevel = 8 - offsetBitsInFirstByte;
                }
                if (offsetBitsInFirstByte > 0 && i >= 2)
                {
                    bitLevel += (i - 1) * 8;
                }
                else if (offsetBitsInFirstByte == 0 && i >= 1)
                {
                    bitLevel += i * 8;
                }

                int toWrite = (value >> bitLevel);
                if (i == 0)
                {
                    toWrite &= ((1 << bitsInFirstByte) - 1);
                    toWrite <<= offsetBitsInFirstByte;
                    bitsToWrite -= bitsInFirstByte;
                }
                else if (i == bytesToWriteTo - 1 && offsetBitsInFirstByte > 0)
                {
                    toWrite &= ((1 << bitsToWrite) - 1);
                }
                else
                {
                    bitsToWrite -= 8;
                }

                _buffer[_byteOffset + byteOffset] |= (byte)toWrite;
            }

            if (setIncrementOffset)
            {
                _bitOffset += bitCount;
            }
        }
    }
}