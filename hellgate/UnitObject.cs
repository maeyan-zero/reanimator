using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Hellgate.Excel;
using Hellgate.Excel.JapaneseBeta;
using Revival.Common;

namespace Hellgate
{
    /*
    == Monster Stats ==
    level
    hp_cur
    hp_max
    hp_cur_otherplayer
    hp_max_otherplayer
    attack_rating
    base_dmg_min
    base_dmg_max
    damage_min
    damage_max
    critical_mult
    interrupt_defense
    armor
    sfx_defense_bonus
    experience
    can_run
    stealth_defense
    ai_change_defense
    name_label_adj
    corpse_explode_points

    == NPC Stats ==
    level
    hp_cur
    hp_max
    hp_cur_otherplayer
    hp_max_otherplayer
    sfx_defense_bonus
    can_run
    */

    [Serializable]
    public partial class UnitObject
    {
        private const UInt32 ObjectMagicWord = 0x67616C46;          // 'Flag'
        private const UInt32 ItemMagicWord = 0x2B523460;            // '`4R+'
        private const UInt32 HotkeysMagicWord = 0x91103A74;         // 't:.`'

        public const int SPVersion = 0x00BF;
        public const int CurrentVersion = 0x00CD;

        ///////////////////// General Structure Definition /////////////////////
        /////// Start of read inside main header check function (in ASM) ///////

        private int _version;                                                   // 16 bits      0x00BF for SP client, 0x00CD for current Resurrection client
        private ObjectContext _context;                                         // 8            Usage context - 0 = Character, 2 = Character Select, 4 = Item Drop

        private int _bitFieldCount;									            // 8            must be <= 2
        private UInt64 _bitField;                                               // 32 * Count


        // if (testBit(bitField, 0x1D))
        private int _bitCount;                                                  // 32           of entire unit object


        // if (testBit(bitField, 0x00))
        private uint _beginFlag;                                                // 32           must be "Flag" (0x67616C46) or "`4R+" (0x2B523460)


        // if (testBit(bitField, 0x1C))
        public int _timeStamp1;                                                 // 32           i don't think these are actually time stamps
        public int _timeStamp2;                                                 // 32           but since they change all the time and can be
        public int _timeStamp3;                                                 // 32           set to 00 00 00 00 and it'll still load... it'll do


        // if (testBit(bitField, 0x1F))
        //public int SaveLocationsCount;									    // 4
        public List<SaveLocation> SaveLocations;                                // 32 * Count   contains Level and Difficulty code values of last(?) station save location


        // if (testBit(bitField, 0x20)
        //public int StateCode1Count;                                           // 8            state code values (e.g. "elite") from STATES
        public List<Int32> StateCodes1;                                         // 16 * Count	not sure what difference is with this and StateCodes2


        /////// End of read inside main header check function (in ASM) ///////


        // if (testBit(bitField, 0x1B))
        public int BookmarkCount;                                              // 5             (only 1 bookmark row exists though - "hotkeys")
        public List<Bookmark> Bookmarks;                                       // 48 * Count    


        // if (testBit(bitField, 0x05))
        public int UnitObjectId;                                                   // 32           used in MP, used in item (same as UnitObjectID)


        public UnitTypes UnitType;										        // 4		    1 = character, 2 = monster (e.g. engineer drone), 3 = ?, 4 = item, 5 = ? (these look like row index values from UNITTYPES)
        public ushort UnitCode;                                                 // 16           the code identifier of the UnitObject (e.g. Job Class, or Item Id etc)
        public UnitDataRow UnitData;


        // if (testBit(bitField, 0x17))
        public UInt64 ObjectId;                                                 // 64           a unique id identifiying this unit "structure"


        // if (testBit(bitField, 0x01) || testBit(bitField, 0x03))
        // {
        public bool IsInventory;								                // 1 bit
        // {
        //// if (testBit(bitField, 0x02))
        public int Unknown02;										            // 32			untested

        public int InventoryLocationIndex;     								    // 12           i think...
        public int InventoryPositionX;									        // 12           x-position of inventory item
        public int InventoryPositionY;									        // 12           y-position of inventory item
        public int Unknown04;                                                   // 4 bits       not sure
        // }
        // else
        // {
        public int RoomId;                                                      // 32

        public Vector3 Position;                                                // 32 * 3

        public float Unknown0103Float21;                                        // 32
        public float Unknown0103Float22;                                        // 32
        public float Unknown0103Float23;                                        // 32

        public Vector3 Normal;                                                  // 32 * 3

        public int Unknown0103Int2;                                             // 32

        public float Unknown0103Float4;                                         // 32

        public float Unknown0103Float5;                                         // 32
        // } // end IsInventory

        public Int64 Unknown0103Int64;								            // 8*8 bits (64) - non-standard func
        // } // end 0x01 || 0x03


        // if (testBit(bitField, 0x06)) // does more reading, but not seen
        public bool UnknownBool06;									            // 1            // alert if != 1


        // if (testBit(bitField, 0x09))
        public int ItemLookGroupCode;									        // 8            from table ITEM_LOOK_GROUPS


        // if (testBit(bitField, 0x07)) // if (bitField1 & 0x80)
        public int CharacterHeight;									            // 8
        public int CharacterBulk;									            // 8


        // if (testBit(bitField, 0x08))
        //private int _charCount;									            // 8            number of *Unicode* characters
        private byte[] _charNameBytes;                                          // 8 * Count * 2


        // if (testBit(bitField, 0x0A))
        //public int StateCode2Count;                                          // 8            state code values (e.g. "elite") from STATES table
        public List<Int32> StateCodes2;                                        // 16 * Count   not sure what difference is with this and StateCodes1


        public bool ContextBool;                                                // 1
        public int ContextBoolValue;                                            // 4


        public bool IsDead;									                    // 1            does NOT influence HC dead! If the character died right before saving this bool is true


        // if (_TestBit(Bits.Bit0DStats))
        public UnitObjectStats Stats;
        // else if (_TestBit(Bits.Bit14CharSelectStats))                                        these values are used in the character select screen when playing MP
        //public int CharacterLevel;                                            // 8
        //public int CharacterPvpRankRowIndex;                                  // 8
        // if (_TestBit(Bits.Bit1ECharSelectStatsMaxDifficulty))
        //public int MaxDifficultyRowIndex;                                     // 3


        public bool HasAppearanceDetails;						                // 1
        public UnitAppearance Appearance;                                       //              see UnitAppearance structure for layout and counts


        // if (testBit(pUnit->bitField1, 0x12))
        public int ItemEndBitOffset;									        // 32           bit offset to end of items
        public int ItemCount;										            // 10           each item is another UnitObject
        public List<UnitObject> Items;


        // if (testBit(pUnit->bitField1, 0x1A))
        public uint HotkeyFlag;                                                 // 32           must be 0x91103A74 (always present)
        public int EndFlagBitOffset;                                            // 32           offset to end of file flag
        public int HotkeyCount;                                                 // 6            hotkey count
        public List<Hotkey> Hotkeys;                                            //              positions on bottom bar, etc as well


        public int EndFlag;											            // 32           end of unit flag (always present)


        ///////////////////// Function Definitions /////////////////////

        public static FileManager FileManager;

        private readonly bool _debugOutputLoadingProgress;

        private BitBuffer _bitBuffer;

        public UnitObject(int version = CurrentVersion) : this(false)
        {
            _version = version;
            _Init();
        }

        public UnitObject(bool debugOutputLoadingProgress, int version = CurrentVersion)
        {
            _version = version;
            _debugOutputLoadingProgress = debugOutputLoadingProgress;
            _bitBuffer = new BitBuffer();

            _Init();
        }

        private UnitObject(BitBuffer bitBuffer, bool debugOutputLoadingProgress = false)
            : this(debugOutputLoadingProgress)
        {
            _bitBuffer = bitBuffer;
        }

        private void _Init()
        {
            SaveLocations = new List<SaveLocation>(3); // always seen as 3 (1 for each difficulty - even if not applicable on the char yet)
            StateCodes1 = new List<Int32>();
            StateCodes2 = new List<Int32>();
            Bookmarks = new List<Bookmark>();
            Stats = new UnitObjectStats(FileManager, _debugOutputLoadingProgress);
            Items = new List<UnitObject>();
            Appearance = new UnitAppearance();
            Hotkeys = new List<Hotkey>();
            Position = new Vector3();
            Normal = new Vector3();
        }

        private String _name;
        public String Name
        {
            get { return _name; }
            set
            {
                _name = value;
                _charNameBytes = Encoding.Unicode.GetBytes(value);
            }
        }

        public override String ToString()
        {
            return _name;
        }

        /// <summary>
        /// Parses a unit object byte array and populates the current UnitObject.
        /// </summary>
        /// <param name="buffer">The buffer to read from.</param>
        /// <param name="offset">The offset within the buffer to start from.</param>
        /// <param name="maxBytes">A maxmimum byte count to use. Set to 0 to ignore (not recommended).</param>
        public void ParseUnitObject(byte[] buffer, int offset, int maxBytes)
        {
            lock (_bitBuffer)
            {
                _bitBuffer.SetBuffer(buffer, offset, maxBytes);
                _ReadUnit();
                _bitBuffer.FreeBuffer();
            }
        }

        /// <summary>
        /// Parses the wardrobe appearance segment of a unit object byte array and populates the current UnitObject.
        /// </summary>
        /// <param name="buffer">The buffer to read</param>
        /// <param name="offset">The offset within the buffer to start from.</param>
        /// <param name="maxBytes">A maximum byte count to use. Set to 0 to ignore (not recommended).</param>
        public void ParseWardrobeAppearance(byte[] buffer, int offset, int maxBytes)
        {
            lock (_bitBuffer)
            {
                _bitBuffer.SetBuffer(buffer, offset, maxBytes);
                _ReadWardrobeAppearance(true);
            }
        }

        /// <summary>
        /// Parses the stats segment of a unit object byte array and populates the current UnitObject.
        /// </summary>
        /// <param name="buffer">The buffer to read</param>
        /// <param name="offset">The offset within the buffer to start from.</param>
        /// <param name="maxBytes">A maximum byte count to use. Set to 0 to ignore (not recommended).</param>
        /// <param name="readNameCount">Whether or not to include reading the "name count" (actual usage not entirely accurate).</param>
        public void ParseStats(byte[] buffer, int offset, int maxBytes, bool readNameCount)
        {
            lock (_bitBuffer)
            {
                _bitBuffer.SetBuffer(buffer, offset, maxBytes);
                Stats.ReadStats(_bitBuffer, readNameCount);
            }
        }

        public void ParseUnknown(byte[] buffer, int offset)
        {
            // code based on sub_4AC1CC

            _bitBuffer.SetBuffer(buffer, offset, 0);

            int roomRowIndexOffset = _bitBuffer.ReadBits(12); // 0x30 for our Tutorial_Russel Square
            int bitCount = _bitBuffer.ReadBits(12); // 0x73 bits... is this always 0x73? Where is it from? (possibly largest number of rooms any level can have?)
            Debug.WriteLine(String.Format("RoomRowIndexOffset = {0} (0x{0:X4}), BitCount = {1} (0x{1:X4})", roomRowIndexOffset, bitCount));

            int int32Count = (bitCount >> 5);
            int remainingBitCount = (bitCount & 0x1F);
            int remainingByteCount = (remainingBitCount >> 3) + (remainingBitCount > 0 ? 1 : 0);
            Debug.WriteLine(String.Format("Int32Count = {0}, RemainingBitCount = {1} -> RemainingByteCount = {2}", int32Count, remainingBitCount, remainingByteCount));

            int bitFieldCount = int32Count + (remainingByteCount > 0 ? 1 : 0);
            int[] bitField = new int[bitFieldCount];
            for (int i = 0; i < int32Count; i++)
            {
                bitField[i] = _bitBuffer.ReadInt32();
                Debug.WriteLine(String.Format("bitField[{0}] = {2} = {1} (0x{1:X8})", i, bitField[i], _DebugBinaryFormat(bitField[i])));
            }
            if (remainingBitCount > 0)
            {
                int lastIndex = bitField.Length - 1;
                bitField[bitField.Length - 1] = _bitBuffer.ReadBits(remainingBitCount);
                Debug.WriteLine(String.Format("bitField[{0}] = {2} = {1} (0x{1:X8})", lastIndex, bitField[lastIndex], _DebugBinaryFormat((ulong)bitField[lastIndex], remainingBitCount)));
            }

            int[] roomOrder = new int[bitCount]; // not yet sure why these are needed client-side, or what they are, or how they're generated server-side
            int[] roomIndexes = new int[bitCount];
            for (int i = 0; i < bitCount; i++)
            {
                int int32Index = (i >> 5);
                int bitIndex = (i % 32);

                if ((bitField[int32Index] & (1 << bitIndex)) > 0)
                {
                    roomOrder[i] = _bitBuffer.ReadByte(); // 0x50, 0x50, 0x3C, -, 0x1E, 0x5A, -, 0, 0, 0x14, 0xA, 0x46, -, 0x32, -, 0, 0x46, -, -, 0x3C, 0x28, ---..., 0x50 (last bit)
                    roomIndexes[i] = roomRowIndexOffset + i;
                    Debug.WriteLine(String.Format("RoomOrder = {0}, RoomIndex = {1} (0x{1:X2})", roomOrder[i], roomIndexes[i]));
                }
            }
            Array.Sort(roomOrder, roomIndexes); // proper sort method is ordering by roomOrder desc, roomIndexes desc
                                                // Array.Sort default like this is roomOrder desc, roomIndexes asc - does it matter?

            foreach (int index in roomIndexes)
            {
                if (index == 0) continue;

                RoomIndexRow roomIndex = (RoomIndexRow)FileManager.GetRowFromIndex(Xls.TableCodes.ROOM_INDEX, index);
                Debug.WriteLine(String.Format("RoomIndex[{0}] = {1}", index, roomIndex.Name));
            }

            int monsterCount = _bitBuffer.ReadUInt16();
            Debug.WriteLine(String.Format("MonsterCount = {0}", monsterCount));

            ExcelFile monstersTable = FileManager.GetExcelTableFromCode(Xls.TableCodes.MONSTERS);
            for (int i = 0; i < monsterCount; i++)
            {
                int monsterIndex = _bitBuffer.ReadUInt16();

                UnitDataRow monster = FileManager.GetUnitDataRowFromIndex(Xls.TableCodes.MONSTERS, monsterIndex);
                String monsterName = monstersTable.ReadStringTable(monster.name);

                Debug.WriteLine(String.Format("Monster[{0}] = {1} (rowIndex = {2})", i, monsterName, monsterIndex));
            }
        }

        // writing bit fields
        private const UInt64 ItemDrop = 0x0000000C0381E827;
        private const UInt64 SaveCharacter = 0x00000001BDE77D81;
        private const UInt64 SaveCharacterItems = 0x0000000021A76E09;
        private const UInt64 CharStats = 0x0000000E0381E9E7;
        private const UInt64 Monster = 0x0000000C0301E867;

        public byte[] ToByteArray()
        {
            byte[] buffer = null;
            _GenerateUnitObject(ref buffer, 0, 0, SaveCharacter, ObjectContext.Save, SaveCharacterItems, ObjectContext.Save, UnitObjectStats.StatContext.UseCodes);
            return buffer;
        }

        public byte[] GenerateMonster()
        {
            byte[] buffer = null;
            _GenerateUnitObject(ref buffer, 0, 0, Monster, ObjectContext.Monster, 0, ObjectContext.None, UnitObjectStats.StatContext.UseRows);
            return buffer;
        }

        /// <summary>
        /// Generate a byte array of this object as an item drop.
        /// </summary>
        /// <returns>A new byte array of the item drop.</returns>
        public byte[] GenerateItemDrop()
        {
            byte[] buffer = null;
            _GenerateUnitObject(ref buffer, 0, 0, ItemDrop, ObjectContext.ItemDrop, 0, ObjectContext.None, UnitObjectStats.StatContext.UseRows);
            return buffer;
        }

        /// <summary>
        /// Writes an item drop byte array to the supplied buffer at the specified offset up to maxBytes length.
        /// </summary>
        /// <param name="buffer">The buffer to write to.</param>
        /// <param name="offset">The offset to begin writing at.</param>
        /// <param name="maxBytes">A maximum byte count to write - an exception will be throw in exceeded.</param>
        /// <returns>The bytes written.</returns>
        public int GenerateItemDrop(byte[] buffer, int offset, int maxBytes)
        {
            // todo: not sure if items can be within items for item drops
            return _GenerateUnitObject(ref buffer, offset, maxBytes, ItemDrop, ObjectContext.ItemDrop, 0, ObjectContext.None, UnitObjectStats.StatContext.UseRows);
        }

        //private void _FlagBit(Bits bit)
        //{
        //    BitField |= ((ulong)1 << (int)bit);
        //}

        public int GenerateCharacterSelect(byte[] buffer, int offset, int maxBytes)
        {
            /* Character Select Contents
             * Bits.Bit00FlagAlignment
             * Bits.Bit1CTimeStamps
             * Bits.Bit1FSaveLocations
             * Bits.Bit20States1 (was zero though)
             * Bits.Bit17ObjectId
             * Bits.Bit07CharacterShape
             * Bits.Bit08CharacterName
             * Bits.Bit0AStates2 (was zero though)
             * Bits.Bit14CharSelectStats
             * Bits.Bit1ECharSelectStatsMaxDifficulty
             * Bits.Bit22EquippedItemRowIndex (was zero though - ItemCode was used instead - not sure what exactly this relates to)
             * Bits.Bit18EquippedItemAffix (even when all items have no affixes)
             * Bits.Bit16AppearanceUnknown64Bits (was zero though)
             * Bits.Bit23AppearanceUnknownColorSetCode
             * Bits.Bit11WardrobeLayers
             * Bits.Bit10LayerAppearances
             * Bits.Bit0EUnknown // is set for char select screen characters, but didn't even find any usage in ASM
             */
            //BitField = 0;
            //_FlagBit(Bits.Bit00FlagAlignment);
            //_FlagBit(Bits.Bit1CTimeStamps);
            //_FlagBit(Bits.Bit1FSaveLocations);
            //_FlagBit(Bits.Bit20States1);
            //_FlagBit(Bits.Bit17ObjectId);
            //_FlagBit(Bits.Bit07CharacterShape);
            //_FlagBit(Bits.Bit08CharacterName);
            //_FlagBit(Bits.Bit0AStates2);
            //_FlagBit(Bits.Bit14CharSelectStats);
            //_FlagBit(Bits.Bit1ECharSelectStatsMaxDifficulty);
            //_FlagBit(Bits.Bit22EquippedItemRowIndex);
            //_FlagBit(Bits.Bit18EquippedItemAffix);
            //_FlagBit(Bits.Bit16AppearanceUnknown64Bits);
            //_FlagBit(Bits.Bit23AppearanceUnknownColorSetCode);
            //_FlagBit(Bits.Bit11WardrobeLayers);
            //_FlagBit(Bits.Bit10LayerAppearances);
            //_FlagBit(Bits.Bit0EUnknown);

            const UInt64 charSelectCharacter = 0x0000000DD1D34581; // 59354858881 =  110111010001110100110100010110000001

            return _GenerateUnitObject(ref buffer, offset, maxBytes, charSelectCharacter, ObjectContext.CharSelect, 0, ObjectContext.None, UnitObjectStats.StatContext.UseRows);
        }

        /// <summary>
        /// Generate a byte array of this object as an item drop.
        /// </summary>
        /// <returns>A new byte array of the item drop.</returns>
        public byte[] GenerateCharacterStats()
        {
            byte[] buffer = null;
            _GenerateUnitObject(ref buffer, 0, 0, CharStats, ObjectContext.CharStats, 0, ObjectContext.None, UnitObjectStats.StatContext.UseRows);
            return buffer;
        }

        public int GenerateCharacterStats(byte[] buffer, int offset, int maxBytes)
        {
            return _GenerateUnitObject(ref buffer, offset, maxBytes, CharStats, ObjectContext.CharStats, 0, ObjectContext.None, UnitObjectStats.StatContext.UseRows);
        }

        private int _GenerateUnitObject(ref byte[] buffer, int offset, int maxBytes, UInt64 bitField, ObjectContext context, UInt64 itemsBitField, ObjectContext itemsContext, UnitObjectStats.StatContext statsContext)
        {
            int byteCount;

            lock (_bitBuffer)
            {
                if (buffer == null)
                {
                    _bitBuffer.CreateBuffer();
                }
                else
                {
                    _bitBuffer.SetBuffer(buffer, offset, maxBytes);
                }

                // BitField and Context could possibly be done "automatically", but it'd require changing  every _TestBit()
                //            to a set of TestContext ranges and such; simplier like this - and essentially the same result
                _WriteUnit(bitField, context, itemsBitField, itemsContext, statsContext); // no items supplied

                byteCount = _bitBuffer.BytesUsed;

                if (buffer == null)
                {
                    buffer = _bitBuffer.GetBuffer();
                }
                _bitBuffer.FreeBuffer();
            }

            return byteCount;
        }

        /// <summary>
        /// Reads a UnitObject from the internal serialised byte array.
        /// </summary>
        private void _ReadUnit()
        {
            //// start of header
            // unit object versions
            _version = _bitBuffer.ReadInt16();
            _context = (ObjectContext)_bitBuffer.ReadByte();
            if (_version != 0x00BF && _version != 0x00CD) throw new Exceptions.NotSupportedVersionException("0x00BF or 0x00CD", "0x" + _version.ToString("X4"));
            if (_context != ObjectContext.Save && _context != ObjectContext.CharSelect &&
                _context != ObjectContext.CharStats && _context != ObjectContext.ItemDrop)
            {
                throw new Exceptions.NotSupportedVersionException("0x00 or 0x02 or 0x03 or 0x04", "0x" + _context.ToString("X2"));
            }
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("Version = {0} (0x{0:X4}), Context = {1} (0x{2:X2})", _version, _context, (int)_context));
            }


            // content bit fields
            _bitFieldCount = _bitBuffer.ReadBits(8);
            if (_bitFieldCount == 1) _bitField = _bitBuffer.ReadUInt32();
            if (_bitFieldCount == 2) _bitField = _bitBuffer.ReadUInt64();
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("BitField = {0} (0x{1:X16})", _DebugBinaryFormat(_bitField), _bitField));
            }


            // total bit count
            if (_TestBit(Bits.Bit1DBitCountEof))
            {
                _bitCount = _bitBuffer.ReadBits(32);
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("Total BitCount = {0}", _bitCount));
                }
            }


            // begin data magic word
            if (_TestBit(Bits.Bit00FlagAlignment))
            {
                _beginFlag = (uint)_bitBuffer.ReadBits(32);
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("BeginFlag = 0x{0}", _beginFlag.ToString("X8")));
                }
                if (_beginFlag != ObjectMagicWord && _beginFlag != ItemMagicWord) throw new Exceptions.UnexpectedTokenException(ObjectMagicWord, _beginFlag);
            }


            // dunno what these are exactly
            if (_TestBit(Bits.Bit1CTimeStamps))
            {
                _timeStamp1 = _bitBuffer.ReadBits(32);
                _timeStamp2 = _bitBuffer.ReadBits(32);
                _timeStamp3 = _bitBuffer.ReadBits(32);
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("TimeStamp1 = {0}, TimeStamp2 = {1}, TimeStamp3 = {2}", _timeStamp1, _timeStamp2, _timeStamp3));
                }
            }


            // last station visited save/respawn location
            if (_TestBit(Bits.Bit1FSaveLocations))
            {
                int saveLocationsCount = _bitBuffer.ReadBitsShift(4);
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("SaveLocationsCount = {0}", saveLocationsCount));
                }

                for (int i = 0; i < saveLocationsCount; i++)
                {
                    ushort levelCode = _bitBuffer.ReadUInt16(); // table 0x6D (LEVEL)
                    ushort difficultyCode = _bitBuffer.ReadUInt16();  // table 0xB2 (DIFFICULTY)

                    SaveLocation saveLocation = new SaveLocation
                    {
                        Level = (LevelRow)FileManager.GetRowFromCode(Xls.TableCodes.LEVEL, (short)levelCode),
                        Difficulty = (DifficultyRow)FileManager.GetRowFromCode(Xls.TableCodes.DIFFICULTY, (short)difficultyCode)
                    };
                    SaveLocations.Add(saveLocation);

                    if ((SaveLocations[i].Level == null && SaveLocations[i].Difficulty != null) || (SaveLocations[i].Level != null && SaveLocations[i].Difficulty == null))
                    {
                        throw new Exceptions.UnitObjectException(String.Format("Invalid SaveLocation encountered. Level = {0:X4}, Difficulty = {1:X4}", levelCode, difficultyCode));
                    }

                    if (!_debugOutputLoadingProgress) continue;
                    if (SaveLocations[i].Level == null || SaveLocations[i].Difficulty == null)
                    {
                        Debug.WriteLine(String.Format("SaveLocations[{0}].LevelCode = {1} (0x{1:X4}), SaveLocations[{0}].DifficultyCode = {2} (0x{2:X4})",
                                                      i, levelCode, difficultyCode));
                    }
                    else
                    {
                        Debug.WriteLine(String.Format("SaveLocations[{0}].Level = {1}, SaveLocations[{0}].Difficulty = {2}",
                                                      i, SaveLocations[i].Level.levelName, SaveLocations[i].Difficulty.name));
                    }
                }
            }


            // character flags
            if (_TestBit(Bits.Bit20States1))
            {
                int statCount = _bitBuffer.ReadBits(8);
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("StateCode1Count = {0}", statCount));
                }

                for (int i = 0; i < statCount; i++)
                {
                    int state = _bitBuffer.ReadInt16();
                    AddState1(state); // table 0x4B (STATES)
                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("StateCodes1[{0}] = {1}({2:X})", i, StateCodes1[i], (short)(StateCodes1[i])));
                    }
                }
            }
            //// end of header


            // bit offsets to bookmarks (only 1 bookmark though - "hotkeys")
            if (_TestBit(Bits.Bit1BBookmarks))
            {
                BookmarkCount = _bitBuffer.ReadBits(5);
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("BookmarkCount = {0}", BookmarkCount));
                }
                if (BookmarkCount > 1)
                {
                    throw new Exceptions.UnitObjectNotImplementedException("Unexpected BookmarkCount (> 1)!\nNot-Implemented cases. Please report this error and supply the offending file.");
                }

                for (int i = 0; i < BookmarkCount; i++)
                {
                    Bookmark bookmark = new Bookmark
                    {
                        Code = _bitBuffer.ReadUInt16(),
                        Offset = _bitBuffer.ReadInt32()
                    };
                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("Bookmarks[{0}].Code = {1} (0x{1:X4}), Bookmarks[{0}].Offset = {2}", i, bookmark.Code, bookmark.Offset));
                    }

                    Bookmarks.Add(bookmark);
                }
            }


            // dunno...
            if (_TestBit(Bits.Bit05Unknown))
            {
                UnitObjectId = _bitBuffer.ReadInt32();
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("UnitObjectId = {0} (0x{0:X4})", UnitObjectId));
                }
            }


            // unit type/code
            // if unit type == 1, table = 0x91 (PLAYERS)
            //                 2, table = 0x77 (MONSTERS)
            //                 3? (table = 0x72; MISSILES at a guess. For memory, MISSILES doesn't use code values - probably why not seen in ASM)
            //                 4, table = 0x67 (ITEMS)
            //                 5, table = 0x7B (OBJECTS)
            UnitType = (UnitTypes)_bitBuffer.ReadBits(4);
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("UnitType = {0}", UnitType));
            }
            UnitCode = _bitBuffer.ReadUInt16();
            if (_debugOutputLoadingProgress)
            {
                Debug.Write(String.Format("UnitCode = {0} (0x{0:X4}), ", UnitCode));
            }
            Xls.TableCodes tableCode = Xls.TableCodes.Null;
            switch (UnitType)
            {
                case UnitTypes.Player:  tableCode = Xls.TableCodes.PLAYERS;  break;
                case UnitTypes.Monster: tableCode = Xls.TableCodes.MONSTERS; break;
                case UnitTypes.Missile: tableCode = Xls.TableCodes.MISSILES; break;
                case UnitTypes.Item:    tableCode = Xls.TableCodes.ITEMS;    break;
                case UnitTypes.Object:  tableCode = Xls.TableCodes.OBJECTS;  break;
            }
            if (tableCode == Xls.TableCodes.Null) throw new Exceptions.UnitObjectException("The unit object data has an unknown UnitType.");
            UnitData = FileManager.GetUnitDataRowFromCode(tableCode, (short)UnitCode);
            if (UnitData == null) Debug.WriteLine(String.Format("Warning: UnitCode {0} (0x{0:X4}) not found!", UnitCode));
            if (_debugOutputLoadingProgress && UnitData != null)
            {
                ExcelFile unitDataTable = FileManager.GetExcelTableFromCode(tableCode);
                String rowName = unitDataTable.ReadStringTable(UnitData.name);
                Debug.WriteLine(String.Format("UnitDataName = " + rowName));
            }


            // unit object id
            if (_TestBit(Bits.Bit17ObjectId))
            {
                if (_version > 0xB2)
                {
                    ObjectId = _bitBuffer.ReadUInt64();
                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("ObjectId = {0} (0x{0:X16})", ObjectId));
                    }

                    if (ObjectId == 0)
                    {
                        throw new Exceptions.UnitObjectNotImplementedException("if (ObjectId == 0)");
                    }
                }
            }


            // item positioning stuff
            if (_TestBit(Bits.Bit01Unknown) || _TestBit(Bits.Bit03Unknown))
            {
                IsInventory = _bitBuffer.ReadBool();
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("IsInventory = {0}, Bits.Bit01Unknown = {1}, Bits.Bit03Unknown = {2}", IsInventory, _TestBit(Bits.Bit01Unknown), _TestBit(Bits.Bit03Unknown)));
                }

                if (IsInventory) // item is in inventory
                {
                    if (_TestBit(Bits.Bit02Unknown))
                    {
                        Unknown02 = _bitBuffer.ReadBits(32);
                        if (_debugOutputLoadingProgress)
                        {
                            Debug.WriteLine(String.Format("Unknown02 = {0}", Unknown02));
                        }
                    }

                    InventoryLocationIndex = _bitBuffer.ReadBits(12);
                    InventoryPositionX = _bitBuffer.ReadBits(12);
                    InventoryPositionY = _bitBuffer.ReadBits(12);
                    Unknown04 = _bitBuffer.ReadBits(4);
                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("InventoryLocationIndex = {0}, InventoryPositionX = {1}, InventoryPositionY = {2}, Unknown04 = {3}",
                            InventoryLocationIndex, InventoryPositionX, InventoryPositionY, Unknown04));
                    }

                    Unknown0103Int64 = _bitBuffer.ReadNonStandardFunc();
                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("Unknown0103Int64 = {0} (0x{0:X16})", Unknown0103Int64));
                    }
                }
                else // item is a "world drop"
                {
                    RoomId = _bitBuffer.ReadInt32();

                    Position.X = _bitBuffer.ReadFloat();
                    Position.Y = _bitBuffer.ReadFloat();
                    Position.Z = _bitBuffer.ReadFloat();

                    Unknown0103Float21 = _bitBuffer.ReadFloat();
                    Unknown0103Float22 = _bitBuffer.ReadFloat();
                    Unknown0103Float23 = _bitBuffer.ReadFloat();

                    Normal.X = _bitBuffer.ReadFloat();
                    Normal.Y = _bitBuffer.ReadFloat();
                    Normal.Z = _bitBuffer.ReadFloat();

                    Unknown0103Int2 = _bitBuffer.ReadBits(10);

                    Unknown0103Float4 = _bitBuffer.ReadFloat();

                    Unknown0103Float5 = _bitBuffer.ReadFloat();

                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("RoomId = {0}", RoomId));
                        Debug.WriteLine(String.Format("Position.X = {0}, Position.Y = {1}, Position.Z = {2}", Position.X, Position.Y, Position.Z));
                        Debug.WriteLine(String.Format("Unknown0103Float21 = {0}, Unknown0103Float22 = {1}, Unknown0103Float23 = {2}", Unknown0103Float21, Unknown0103Float22, Unknown0103Float23));
                        Debug.WriteLine(String.Format("NormalX = {0}, NormalY = {1}, NormalZ = {2}", Normal.X, Normal.Y, Normal.Z));
                        Debug.WriteLine(String.Format("Unknown0103Int2 = {0}", Unknown0103Int2));
                        Debug.WriteLine(String.Format("Unknown0103Float4 = {0}", Unknown0103Float4));
                        Debug.WriteLine(String.Format("Unknown0103Float5 = {0}", Unknown0103Float5));
                    }
                }
            }


            // I think this has something to do with the Monsters table +46Ch, bit 0x55 = 4 bits or bit 0x47 = 2 bits. Or Objects table +46Ch, bit 0x55 = 2 bits... Something like that
            if (_TestBit(Bits.Bit06Unknown))
            {
                UnknownBool06 = _bitBuffer.ReadBool();
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("UnknownBool06 = {0}", UnknownBool06));
                }
                if (!UnknownBool06)
                {
                    throw new Exceptions.UnitObjectNotImplementedException("if (UnknownBool06 != 1)");
                }
            }


            if (_TestBit(Bits.Bit09ItemLookGroup))
            {
                ItemLookGroupCode = _bitBuffer.ReadBits(8);
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("ItemLookGroupCode = {0} (0x{0:X2})", ItemLookGroupCode));
                }
            }


            // on character only
            if (_TestBit(Bits.Bit07CharacterShape))
            {
                CharacterHeight = _bitBuffer.ReadByte();
                CharacterBulk = _bitBuffer.ReadByte();
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("CharacterHeight = {0}, CharacterBulk = {1}", CharacterHeight, CharacterBulk));
                }
            }


            // object id for older versions - they moved it?
            if (_TestBit(Bits.Bit17ObjectId))
            {
                if (_version <= 0xB2)
                {
                    throw new Exceptions.UnitObjectNotImplementedException("if (_TestBit(0x17) && Version <= 0xB2)");
                }
            }


            // on character only
            if (_TestBit(Bits.Bit08CharacterName))
            {
                int unicodeCharCount = _bitBuffer.ReadBits(8);
                if (unicodeCharCount > 0)
                {
                    int byteCount = unicodeCharCount * 2; // is Unicode string without \0
                    _charNameBytes = new byte[byteCount];
                    for (int i = 0; i < byteCount; i++)
                    {
                        _charNameBytes[i] = _bitBuffer.ReadByte();
                    }
                    Name = Encoding.Unicode.GetString(_charNameBytes, 0, byteCount);
                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("Name = {0}", Name));
                    }
                }
            }


            // on both character and items - appears to be always zero for items
            if (_TestBit(Bits.Bit0AStates2))
            {
                int stateCount = _bitBuffer.ReadBits(8);
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("StateCode2Count = {0}", stateCount));
                }

                for (int i = 0; i < stateCount; i++)
                {
                    int state = _bitBuffer.ReadInt16();
                    AddState2(state);
                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("StateCodes2[{0}] = {1}({2:X})", i, StateCodes2[i], (short)(StateCodes2[i])));
                    }

                    // this section looks like it has more reading if Bit14 is flagged (CharSelectStats)
                }
            }


            if (_context > ObjectContext.CharSelect && (_context <= ObjectContext.Unknown6 || _context != ObjectContext.Unknown7)) // so if == 0, 1, 2, 7, then *don't* do this
            {
                ContextBool = _bitBuffer.ReadBool();
                if (ContextBool)
                {
                    ContextBoolValue = _bitBuffer.ReadBits(4); // invlocidx??
                }

                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("UsageBool = {0}, UsageBoolValue = {1}", ContextBool, ContextBoolValue));
                }
            }

            // <unknown bitfield 0x11th bit> - only seen as false anyways

            IsDead = _bitBuffer.ReadBool();
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("IsDead = {0}", IsDead));
            }


            // unit stats
            if (_TestBit(Bits.Bit0DStats))
            {
                Stats.ReadStats(_bitBuffer, true);
            }
            else if (_TestBit(Bits.Bit14CharSelectStats))
            {
                int characterLevel = _bitBuffer.ReadByte(); // stats row 0x000 (level)
                Stats.SetStat("level", characterLevel);

                int characterPvpRankRowIndex = _bitBuffer.ReadByte(); // stats row 0x347 (player_rank)
                Stats.SetStat("player_rank", characterPvpRankRowIndex);

                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("LevelRowIndex = {0}, PlayerRankRowIndex = {1}", characterLevel, characterPvpRankRowIndex));
                }

                if (_TestBit(Bits.Bit1ECharSelectStatsMaxDifficulty))
                {
                    int maxDifficultyRowIndex = _bitBuffer.ReadBits(3); // stats row 0x347 (difficulty_max)
                    Stats.SetStat("difficulty_max", maxDifficultyRowIndex);

                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("MaxDifficultyRowIndex = {0}, ", maxDifficultyRowIndex));
                    }
                }
            }


            HasAppearanceDetails = _bitBuffer.ReadBool();
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("HasAppearanceDetails = {0}", HasAppearanceDetails));
            }
            if (HasAppearanceDetails)
            {
                _ReadAppearance();
            }


            if (_TestBit(Bits.Bit12Items))
            {
                ItemEndBitOffset = _bitBuffer.ReadInt32();
                ItemCount = _bitBuffer.ReadBits(10);
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("ItemEndBitOffset = {0}, ItemCount = {1}", ItemEndBitOffset, ItemCount));
                }

                for (int i = 0; i < ItemCount; i++)
                {
                    UnitObject item = new UnitObject(_bitBuffer, _debugOutputLoadingProgress);
                    item._ReadUnit();
                    Items.Add(item);
                }
            }


            if (_TestBit(Bits.Bit1AHotkeys))
            {
                HotkeyFlag = _bitBuffer.ReadUInt32();
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("HotkeyFlag = {0} (0x{0:X8})", HotkeyFlag));
                }
                if (HotkeyFlag != HotkeysMagicWord)
                {
                    throw new Exceptions.UnexpectedTokenException(HotkeysMagicWord, HotkeyFlag);
                }

                EndFlagBitOffset = _bitBuffer.ReadBits(32);     // to end flag
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("EndFlagBitOffset = {0}", EndFlagBitOffset));
                }

                HotkeyCount = _bitBuffer.ReadBits(6);
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("HotkeyCount = {0}", HotkeyCount));
                }
                for (int i = 0; i < HotkeyCount; i++)
                {
                    Hotkey hotkey = new Hotkey
                    {
                        Code = _bitBuffer.ReadUInt16(), // code from TAG table
                    };
                    Hotkeys.Add(hotkey);
                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("hotkey.Code = 0x{0:X4}", hotkey.Code));
                    }


                    hotkey.UnknownCount = _bitBuffer.ReadBits(4);
                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("hotkey.UnknownCount = " + hotkey.UnknownCount));
                    }
                    if (hotkey.UnknownCount > 0x02)
                    {
                        throw new Exceptions.UnitObjectNotImplementedException("if (hotkey.UnknownCount > 0x02)");
                    }

                    hotkey.UnknownExists = new bool[hotkey.UnknownCount];
                    hotkey.UnknownValues = new int[hotkey.UnknownCount];
                    for (int j = 0; j < hotkey.UnknownCount; j++)
                    {
                        hotkey.UnknownExists[j] = _bitBuffer.ReadBool();
                        if (hotkey.UnknownExists[j])
                        {
                            hotkey.UnknownValues[j] = _bitBuffer.ReadBits(32); // under some condition this will be ReadFromOtherFunc thingy
                        }
                        if (_debugOutputLoadingProgress)
                        {
                            Debug.WriteLine(String.Format("hotkey.UnknownExists[{0}] = {1}, hotkey.UnknownValues[{0}] = 0x{2:X8}", j, hotkey.UnknownExists[j], hotkey.UnknownValues[j]));
                        }
                    }


                    hotkey.SkillCount = _bitBuffer.ReadBits(4);
                    hotkey.SkillExists = new bool[hotkey.SkillCount];
                    hotkey.SkillCode = new int[hotkey.SkillCount];
                    for (int j = 0; j < hotkey.SkillCount; j++)
                    {
                        hotkey.SkillExists[j] = _bitBuffer.ReadBool();
                        if (hotkey.SkillExists[j])
                        {
                            hotkey.SkillCode[j] = _bitBuffer.ReadBits(32); // code from SKILLS table
                        }
                        if (_debugOutputLoadingProgress)
                        {
                            Debug.WriteLine(String.Format("hotkey.SkillExists[{0}] = {1}, hotkey.SkillCode[{0}] = 0x{2:X8}", j, hotkey.SkillExists[j], hotkey.SkillCode[j]));
                        }
                    }


                    hotkey.UnitTypeCode = _bitBuffer.ReadBits(32); // code from UNITTYPES table
                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("hotkey.UnitTypeCode = 0x{0:X8}", hotkey.UnitTypeCode));
                    }
                }
            }


            // end flag
            EndFlag = _bitBuffer.ReadBits(32);
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("EndFlag = {0} (0x{0:X8})", EndFlag));
            }

            if (EndFlag != _beginFlag && EndFlag != ItemMagicWord)
            {
                int bitOffset = _bitCount - _bitBuffer.BitOffset;
                int byteOffset = (_bitBuffer.Length - _bitBuffer.Offset) - (_bitBuffer.BytesUsed);
                throw new Exceptions.InvalidFileException("Flags not aligned!\nBit Offset: " + _bitBuffer.BitOffset + "\nExpected: " + _bitCount + " (+" + bitOffset +
                                                          ")\nBytes Used: " + (_bitBuffer.BytesUsed) + "\nExpected: " + (_bitBuffer.Length - _bitBuffer.Offset) + " (+" + byteOffset + ")");
            }


            if (_TestBit(Bits.Bit1DBitCountEof)) // no reading is done in here
            {
                // todo: do check that we're at the EoF bit count etc
            }

        }

        private void _ReadAppearance()
        {
            //// ReadEquippedItems ////
            Appearance.EquippedItemCount = _bitBuffer.ReadBits(3);
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("Appearance.EquippedItemCount = {0}", Appearance.EquippedItemCount));
            }
            for (int i = 0; i < Appearance.EquippedItemCount; i++)
            {
                UnitAppearance.EquippedItem equippedItem = new UnitAppearance.EquippedItem();

                if (_TestBit(Bits.Bit0FEquippedItemUnknown)) // unknown
                {
                    equippedItem.Unknown0F = _bitBuffer.ReadInt32();
                }

                equippedItem.ItemCode = _bitBuffer.ReadUInt16(); // table 0x67 (ITEMS)
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("EquippedItem.ItemCode = {0} (0x{0:X4})", equippedItem.ItemCode));
                }

                if (_TestBit(Bits.Bit22EquippedItemRowIndex))
                {
                    equippedItem.ItemRowIndex = (byte)_bitBuffer.ReadBitsShift(8); // table 0x67  (ITEMS) row index (not used? or is it in relation to something else?)
                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("EquippedItem.ItemRowIndex = {0}", equippedItem.ItemRowIndex));
                    }
                }

                if (_TestBit(Bits.Bit18EquippedItemAffix))
                {
                    int bitCount = (_version > 0xC0) ? 4 : 3;
                    equippedItem.AffixCount = _bitBuffer.ReadBits(bitCount);
                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("EquippedItem.AffixCount = {0}", equippedItem.AffixCount));
                    }
                    for (int j = 0; j < equippedItem.AffixCount; j++)
                    {
                        equippedItem.AffixCodes.Add(_bitBuffer.ReadBits(32)); // table 0x35 (AFFIXES)
                        if (_debugOutputLoadingProgress)
                        {
                            Debug.WriteLine(String.Format("EquippedItem.AffixCodes[{0}] = {1} (0x{1:X4})", j, equippedItem.AffixCodes[j]));
                        }
                    }

                }

                Appearance.EquippedItems.Add(equippedItem);
            }


            //// ReadAppearanceColors ////
            Appearance.ArmorColorSetCode = _bitBuffer.ReadInt16(); // table 0x08 (COLORSETS)
            if (_debugOutputLoadingProgress)
            {
                Debug.WriteLine(String.Format("Appearance.ArmorColorSetCode = {0} (0x{0:X4})", Appearance.ArmorColorSetCode));
            }

            if (_TestBit(Bits.Bit16AppearanceUnknown64Bits))
            {
                Appearance.Unknown16 = _bitBuffer.ReadNonStandardFunc();
                Debug.Assert(Appearance.Unknown16 == 0); // only seen as zero - need non-zero to test for type/usage
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("Appearance.Unknown16 = {0} (0x{0:X16})", Appearance.Unknown16));
                }
            }

            if (_TestBit(Bits.Bit23AppearanceUnknownColorSetCode))
            {
                Appearance.Unknown23ColorSetsCode = _bitBuffer.ReadInt16(); // table 0x08 (COLORSETS)
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("Appearance.Unknown23ColorSetsCode = {0} (0x{0:X4})", Appearance.Unknown23ColorSetsCode));
                }
            }


            //// ReadWardrobeAppearance ////
            _ReadWardrobeAppearance(false);


            //// ReadAppearanceWardrobeLayers ////
            if (_TestBit(Bits.Bit10LayerAppearances))
            {
                Appearance.LayerAppearanceCount = _bitBuffer.ReadInt16();
                for (int i = 0; i < Appearance.LayerAppearanceCount; i++)
                {
                    UnitAppearance.LayerAppearance gear = new UnitAppearance.LayerAppearance
                    {
                        WardrobeLayerCode = _bitBuffer.ReadUInt16(), // table 0x62 (WARDROBE_LAYER)
                        UnknownBool = _bitBuffer.ReadBool()
                    };

                    if (gear.UnknownBool)
                    {
                        gear.UnknownBoolValue = _bitBuffer.ReadBits(2);
                    }

                    Appearance.LayerAppearances.Add(gear);
                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("appearance.WardrobeLayerModel[{0}].ItemCode = {1} (0x{1:X4}), .UnknownBool = {2}, .UnknownBoolValue = {3}", i, Appearance.LayerAppearances[i].WardrobeLayerCode, Appearance.LayerAppearances[i].UnknownBool, Appearance.LayerAppearances[i].UnknownBoolValue));
                    }
                }
            }
        }

        private void _ReadWardrobeAppearance(bool ignoreBitfields)
        {
            if (ignoreBitfields || _TestBit(Bits.Bit11WardrobeLayers))
            {
                Appearance.WardrobeLayerBaseCount = _bitBuffer.ReadBits(4); // client max of 0x0A
                for (int i = 0; i < Appearance.WardrobeLayerBaseCount; i++)
                {
                    Appearance.WardrobeLayerBases.Add(_bitBuffer.ReadBits(16)); // Code values from table 0x62 (WARDROBE_LAYER)
                    if (_debugOutputLoadingProgress)
                    {
                        Debug.WriteLine(String.Format("appearance.WardrobeLayers[{0}] = {1} (0x{1:X4})", i, Appearance.WardrobeLayerBases[i]));
                    }
                }
            }

            Appearance.WardrobeAppearanceGroupCount = _bitBuffer.ReadBits(3); // client max of 0x06
            for (int i = 0; i < Appearance.WardrobeAppearanceGroupCount; i++)
            {
                Appearance.WardrobeAppearanceGroups.Add(_bitBuffer.ReadBits(16)); // Code values from table 0x5A (WARDROBE_APPEARANCE_GROUP)
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("appearance.WardrobeAppearanceGroups[{0}] = {1} (0x{1:X4})", i, Appearance.WardrobeAppearanceGroups[i]));
                }
            }

            Appearance.ColorCount = _bitBuffer.ReadBits(4); // client max of 0x03
            for (int i = 0; i < Appearance.ColorCount; i++)
            {
                Appearance.ColorPaletteIndicies.Add(_bitBuffer.ReadBits(8));
                if (_debugOutputLoadingProgress)
                {
                    Debug.WriteLine(String.Format("appearance.ColorPaletteIndicies[{0}] = {1} (0x{1:X2})", i, Appearance.ColorPaletteIndicies[i]));
                }
            }
        }

        private bool _TestBit(Bits bit)
        {
            const ulong bitMask = 1;
            return ((_bitField & (bitMask << (int)bit)) != 0);
        }

        private static String _DebugBinaryFormat(int input)
        {
            return _DebugBinaryFormat((ulong)input, 32);
        }

        private static String _DebugBinaryFormat(ulong input)
        {
            return _DebugBinaryFormat(input, 64);
        }

        private static String _DebugBinaryFormat(ulong input, int bitCount)
        {
            String bitString = String.Empty;

            for (int i = bitCount - 1; i >= 0; i--)
            {
                String result = (((input >> i) & 1) == 1) ? "1" : "0";

                bitString += result;

                if ((i > 0) && ((i) % 4) == 0) bitString += " ";
            }

            return bitString;
        }

        private void _WriteUnit(UInt64 bitField, ObjectContext context, UInt64 itemsBitField, ObjectContext itemsContext, UnitObjectStats.StatContext statContext, BitBuffer bitBuffer = null)
        {
            _bitField = bitField;
            _context = context;
            Stats.Context = statContext;
            if (bitBuffer != null) _bitBuffer = bitBuffer;

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
             *      BookmarkCount                               5                   Count of following block - only seen as 1 (only 1 row in Bookmarks table - "hotkeys")
             *      {
             *          Code                                    16                  Code value of offset - code is from Bookmarks table.
             *          Offset                                  32                  Bit Offset value
             *      }
             * }
             * 
             * if (TestBit(bitField, 0x05))
             * {
             *      Unknown                                     32                  Seen in MP only - unknown usage.
             * }
             * 
             * UnitType                                         4                   The type of .. Is this from UnitType table?
             * UnitCode                                         16                  The code value of the 
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
             *      UnicodeCharCount                            8                   Number of (unicode) characters in following string.
             *      CharacterName                               8*2*count           Character's name in Unicode (no \0) - doesn't appear to be actually used in-game...
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
             * if (Context > 2 && (Context <= 6 || Context != 7))                   These would be applicable to MP only - SP has Usage == 0
             * {
             *      ContextBool                                 1                   True if following value is present.
             *      {
             *          ContextBoolValu                         4                   // TO BE DETERMINED
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
            bool isItem = (UnitType == UnitTypes.Item);
            bool isMonster = (UnitType == UnitTypes.Monster);

            /***** Unit Header *****/

            int bitCountStart = _bitBuffer.BitOffset;

            _version = CurrentVersion;
            _bitBuffer.WriteBits(_version, 16);
            _bitBuffer.WriteBits((int)_context, 8);
            _bitBuffer.WriteBits(0x02, 8);
            {
                _bitBuffer.WriteUInt64(_bitField);
            }

            if (_TestBit(Bits.Bit1DBitCountEof))
            {
                bitCountEofOffset = _bitBuffer.BitOffset;
                _bitBuffer.WriteInt32(0x00000000);
            }

            if (_TestBit(Bits.Bit00FlagAlignment))
            {
                _bitBuffer.WriteUInt32((isItem || isMonster) ? ItemMagicWord : ObjectMagicWord);
            }

            if (_TestBit(Bits.Bit1CTimeStamps))
            {
                _bitBuffer.WriteInt32(_timeStamp1);
                _bitBuffer.WriteInt32(_timeStamp2);
                _bitBuffer.WriteInt32(_timeStamp3);
            }

            if (_TestBit(Bits.Bit1FSaveLocations))
            {
                int saveLocationsCount = SaveLocations.Count;
                _bitBuffer.WriteBitsShift(saveLocationsCount, 4);
                foreach (SaveLocation saveLocation in SaveLocations)
                {
                    if (saveLocation.Level == null || saveLocation.Difficulty == null)
                    {
                        const ushort noCode = 0xFFFF;
                        _bitBuffer.WriteUInt16(noCode);
                        _bitBuffer.WriteUInt16(noCode);
                    }
                    else
                    {
                        _bitBuffer.WriteUInt16((ushort)saveLocation.Level.code);
                        _bitBuffer.WriteUInt16((ushort)saveLocation.Difficulty.code);
                    }
                }
            }

            if (_TestBit(Bits.Bit20States1) && (!isItem || !isMonster))
            {
                int stateCount = StateCodes1.Count;
                _bitBuffer.WriteBits(stateCount, 8);

                foreach (short stateCode in StateCodes1)
                {
                    _bitBuffer.WriteInt16(stateCode);
                }
            }

            /***** Unit Body *****/

            int bitOffsetHotkeys = 0;
            if (_TestBit(Bits.Bit1BBookmarks))
            {
                int bitOffsetsCount = Bookmarks.Count;

                _bitBuffer.WriteBits(bitOffsetsCount, 5);
                for (int i = 0; i < bitOffsetsCount; i++)
                {
                    _bitBuffer.WriteUInt16(Bookmarks[i].Code);
                    bitOffsetHotkeys = _bitBuffer.BitOffset;
                    _bitBuffer.WriteInt32(0x00000000);
                }
            }

            if (_TestBit(Bits.Bit05Unknown))
            {
                _bitBuffer.WriteInt32(UnitObjectId);
            }

            _bitBuffer.WriteBits((int)UnitType, 4);
            if (UnitData == null && UnitCode == 0) throw new Exceptions.UnitObjectException("Unexpected null UnitData object.");
            _bitBuffer.WriteInt16((UnitData == null) ? UnitCode : UnitData.code); // characters from SP or mods may have items not found - we don't want to delete them (todo: or do we?)


            if (_TestBit(Bits.Bit17ObjectId))
            {
                _bitBuffer.WriteUInt64(ObjectId);
            }

            if (_TestBit(Bits.Bit01Unknown) || _TestBit(Bits.Bit03Unknown))
            {
                _bitBuffer.WriteBool(IsInventory);
                if (IsInventory) // item is in inventory
                {
                    if (_TestBit(Bits.Bit02Unknown))
                    {
                        _bitBuffer.WriteInt32(Unknown02);
                    }

                    _bitBuffer.WriteBits(InventoryLocationIndex, 12);
                    _bitBuffer.WriteBits(InventoryPositionX, 12);
                    _bitBuffer.WriteBits(InventoryPositionY, 12);
                    _bitBuffer.WriteBits(Unknown04, 4);

                    _bitBuffer.WriteNonStandardFunc(Unknown0103Int64);
                }
                else // item is a "world drop"
                {
                    _bitBuffer.WriteInt32(RoomId);

                    _bitBuffer.WriteFloat(Position.X);
                    _bitBuffer.WriteFloat(Position.Y);
                    _bitBuffer.WriteFloat(Position.Z);

                    _bitBuffer.WriteFloat(Unknown0103Float21);
                    _bitBuffer.WriteFloat(Unknown0103Float22);
                    _bitBuffer.WriteFloat(Unknown0103Float23);

                    _bitBuffer.WriteFloat(Normal.X);
                    _bitBuffer.WriteFloat(Normal.Y);
                    _bitBuffer.WriteFloat(Normal.Z);

                    _bitBuffer.WriteBits(Unknown0103Int2, 10);

                    _bitBuffer.WriteFloat(Unknown0103Float4);

                    _bitBuffer.WriteFloat(Unknown0103Float5);
                }
            }

            if (_TestBit(Bits.Bit06Unknown))
            {
                _bitBuffer.WriteBool(UnknownBool06);
                if (!UnknownBool06)
                {
                    throw new Exceptions.UnitObjectNotImplementedException("_WriteUnit : if (!UnknownBool06)");
                }
            }

            if (_TestBit(Bits.Bit09ItemLookGroup))
            {
                _bitBuffer.WriteByte(ItemLookGroupCode);
            }

            if (_TestBit(Bits.Bit07CharacterShape))
            {
                _bitBuffer.WriteByte(CharacterHeight);
                _bitBuffer.WriteByte(CharacterBulk);
            }

            if (_TestBit(Bits.Bit08CharacterName) && !String.IsNullOrEmpty(Name))
            {
                _bitBuffer.WriteBits(_charNameBytes.Length / 2, 8); // is Unicode string without \0
                foreach (byte b in _charNameBytes)
                {
                    _bitBuffer.WriteByte(b);
                }
            }

            if (_TestBit(Bits.Bit0AStates2))
            {
                int stateCount = StateCodes2.Count;
                _bitBuffer.WriteBits(stateCount, 8);

                foreach (short stateCode in StateCodes2)
                {
                    _bitBuffer.WriteInt16(stateCode);
                }
            }

            if (_context > ObjectContext.CharSelect && (_context <= ObjectContext.Unknown6 || _context != ObjectContext.Unknown7)) // so if == 0, 1, 2, 7, then *don't* do this
            {
                _bitBuffer.WriteBool(ContextBool);
                if (ContextBool)
                {
                    _bitBuffer.WriteBits(ContextBoolValue, 4);
                }
            }

            _bitBuffer.WriteBool(IsDead);

            if (_TestBit(Bits.Bit0DStats))
            {
                Stats.WriteStats(_bitBuffer, true);
            }
            else if (_TestBit(Bits.Bit14CharSelectStats))
            {
                int charLevel = Stats.GetStatValueOrDefault("level");
                int charPvpRank = Stats.GetStatValueOrDefault("pvp_rank");
                _bitBuffer.WriteByte(charLevel);
                _bitBuffer.WriteByte(charPvpRank);

                if (_TestBit(Bits.Bit1ECharSelectStatsMaxDifficulty))
                {
                    int maxDifficulty = Stats.GetStatValueOrDefault("difficulty_max");
                    _bitBuffer.WriteBits(maxDifficulty, 3);
                }
            }

            _bitBuffer.WriteBool(HasAppearanceDetails);
            if (HasAppearanceDetails)
            {
                int equippedItemCount = Appearance.EquippedItems.Count;
                _bitBuffer.WriteBits(equippedItemCount, 3);
                for (int i = 0; i < equippedItemCount; i++)
                {
                    UnitAppearance.EquippedItem equippedItem = Appearance.EquippedItems[i];

                    if (_TestBit(Bits.Bit0FEquippedItemUnknown))
                    {
                        _bitBuffer.WriteInt32(equippedItem.Unknown0F);
                    }

                    _bitBuffer.WriteUInt16(equippedItem.ItemCode);

                    if (_TestBit(Bits.Bit22EquippedItemRowIndex))
                    {
                        _bitBuffer.WriteBitsShift(equippedItem.ItemRowIndex, 8);
                    }

                    if (_TestBit(Bits.Bit18EquippedItemAffix))
                    {
                        int bitCount = (_version > 0xC0) ? 4 : 3;
                        int affixCount = equippedItem.AffixCodes.Count;
                        _bitBuffer.WriteBits(affixCount, bitCount);
                        for (int j = 0; j < affixCount; j++)
                        {
                            _bitBuffer.WriteBits(equippedItem.AffixCodes[j], 32);
                        }
                    }
                }

                _bitBuffer.WriteBits(Appearance.ArmorColorSetCode, 16);

                if (_TestBit(Bits.Bit16AppearanceUnknown64Bits))
                {
                    _bitBuffer.WriteNonStandardFunc(Appearance.Unknown16);
                }

                if (_TestBit(Bits.Bit23AppearanceUnknownColorSetCode))
                {
                    _bitBuffer.WriteInt16(Appearance.Unknown23ColorSetsCode);
                }

                if (_TestBit(Bits.Bit11WardrobeLayers))
                {
                    int wardrobeLayerCount = Appearance.WardrobeLayerBases.Count;
                    _bitBuffer.WriteBits(wardrobeLayerCount, 4);
                    for (int i = 0; i < wardrobeLayerCount; i++)
                    {
                        _bitBuffer.WriteBits(Appearance.WardrobeLayerBases[i], 16);
                    }
                }

                int wardrobeAppearanceGroupCount = Appearance.WardrobeAppearanceGroups.Count;
                _bitBuffer.WriteBits(wardrobeAppearanceGroupCount, 3);
                for (int i = 0; i < wardrobeAppearanceGroupCount; i++)
                {
                    _bitBuffer.WriteBits(Appearance.WardrobeAppearanceGroups[i], 16);
                }

                int colorPaletteCount = Appearance.ColorPaletteIndicies.Count;
                _bitBuffer.WriteBits(colorPaletteCount, 4);
                for (int i = 0; i < colorPaletteCount; i++)
                {
                    _bitBuffer.WriteBits(Appearance.ColorPaletteIndicies[i], 8);
                }

                if (_TestBit(Bits.Bit10LayerAppearances))
                {
                    _bitBuffer.WriteBits(Appearance.LayerAppearanceCount, 16);
                    for (int i = 0; i < Appearance.LayerAppearanceCount; i++)
                    {
                        _bitBuffer.WriteBits(Appearance.LayerAppearances[i].WardrobeLayerCode, 16);
                        _bitBuffer.WriteBits(Appearance.LayerAppearances[i].UnknownBool ? 1 : 0, 1);
                        if (Appearance.LayerAppearances[i].UnknownBool)
                        {
                            _bitBuffer.WriteBits(Appearance.LayerAppearances[i].UnknownBoolValue, 2);
                        }
                    }
                }
            }


            if (_TestBit(Bits.Bit12Items))
            {
                int itemBitOffset = _bitBuffer.BitOffset;
                _bitBuffer.WriteBits(0x00000000, 32);

                int itemCount = Items.Count;
                _bitBuffer.WriteBits(itemCount, 10);
                for (int i = 0; i < itemCount; i++)
                {
                    Items[i]._WriteUnit(itemsBitField, itemsContext, itemsBitField, itemsContext, statContext, _bitBuffer);
                }

                _bitBuffer.WriteBits(_bitBuffer.BitOffset, 32, itemBitOffset);
            }


            if (_TestBit(Bits.Bit1AHotkeys))
            {
                if (bitOffsetHotkeys > 0)
                {
                    _bitBuffer.WriteBits(_bitBuffer.BitOffset, 32, bitOffsetHotkeys);
                }

                _bitBuffer.WriteUInt32(HotkeysMagicWord);

                int endFlagBitOffset = _bitBuffer.BitOffset;
                _bitBuffer.WriteBits(0x00000000, 32);

                int weaponConfigCount = Hotkeys.Count;
                _bitBuffer.WriteBits(weaponConfigCount, 6);
                foreach (Hotkey hotkey in Hotkeys)
                {
                    _bitBuffer.WriteBits(hotkey.Code, 16);
                    _bitBuffer.WriteBits(hotkey.UnknownCount, 4);
                    for (int i = 0; i < hotkey.UnknownCount; i++)
                    {
                        _bitBuffer.WriteBits(hotkey.UnknownExists[i] ? 1 : 0, 1);
                        if (hotkey.UnknownExists[i])
                        {
                            _bitBuffer.WriteBits(hotkey.UnknownValues[i], 32);
                        }
                    }

                    // yes this chunk looks the same as above - the above chunk though is in a specific function and can differ at 1 point
                    // also, it can be != 2
                    _bitBuffer.WriteBits(hotkey.SkillCount, 4);
                    for (int i = 0; i < hotkey.SkillCount; i++)
                    {
                        _bitBuffer.WriteBits(hotkey.SkillExists[i] ? 1 : 0, 1);
                        if (hotkey.SkillExists[i])
                        {
                            _bitBuffer.WriteBits(hotkey.SkillCode[i], 32);
                        }
                    }

                    _bitBuffer.WriteBits(hotkey.UnitTypeCode, 32);
                }

                _bitBuffer.WriteBits(_bitBuffer.BitOffset, 32, endFlagBitOffset);
            }


            _bitBuffer.WriteUInt32(ItemMagicWord); // "`4R+"

            if (_TestBit(Bits.Bit1DBitCountEof))
            {
                _bitBuffer.WriteBits(_bitBuffer.BitOffset - bitCountStart, 32, bitCountEofOffset);
            }
        }

        public void AddState2(Int32 state)
        {
            if (!(StateCodes2.Contains(state)))
            {
                StateCodes2.Add(state);
            }
        }

        public void RemoveState2(Int32 state)
        {
            StateCodes2.Remove(state);
        }

        public void AddState1(Int32 state)
        {
            if (!(StateCodes1.Contains(state)))
            {
                StateCodes1.Add(state);
            }
        }

        public void RemoveState1(Int32 state)
        {
            StateCodes1.Remove(state);
        }
    }
}