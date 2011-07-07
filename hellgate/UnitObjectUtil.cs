using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Hellgate.Excel;
using Hellgate.Excel.JapaneseBeta;

namespace Hellgate
{
    partial class UnitObject
    {
        public enum ObjectContext
        {
            None = -1,
            Save = 0,
            CharSelect = 2,
            CharStats = 3,
            ItemDrop = 4,
            Monster = 4,
            Unknown6 = 6,
            Unknown7 = 7
        }

        private enum Bits
        {
            // the order is around what they're actually read/checked in loading
            Bit1DBitCountEof = 0x1D,
            Bit00FlagAlignment = 0x00,
            Bit1CTimeStamps = 0x1C,
            Bit1FSaveLocations = 0x1F,
            Bit20States1 = 0x20,
            Bit1BBookmarks = 0x1B,
            Bit05Unknown = 0x05,
            Bit17ObjectId = 0x17,
            Bit03Unknown = 0x03,
            Bit01Unknown = 0x01,
            Bit02Unknown = 0x02,
            Bit06Unknown = 0x06,
            Bit09ItemLookGroup = 0x09,
            Bit07CharacterShape = 0x07,
            Bit08CharacterName = 0x08,
            Bit0AStates2 = 0x0A,
            Bit0DStats = 0x0D,
            Bit14CharSelectStats = 0x14,
            Bit1ECharSelectStatsMaxDifficulty = 0x1E,
            Bit0FEquippedItemUnknown = 0x0F,
            Bit22EquippedItemRowIndex = 0x22, // I think (from ASM)... This was seen as 0 though with ItemCode being used, so not sure what it really is
            Bit18EquippedItemAffix = 0x18,
            Bit16AppearanceUnknown64Bits = 0x16,
            Bit23AppearanceUnknownColorSetCode = 0x23,
            Bit11WardrobeLayers = 0x11,
            Bit10LayerAppearances = 0x10,
            Bit12Items = 0x12,
            Bit1AHotkeys = 0x1A,

            Bit0EUnknown = 0x0E // is set for char select screen characters, but didn't even find usage position in ASM
        }

        public enum UnitTypes
        {
            Player = 1,
            Monster = 2,
            Missile,
            Item,
            Object
        }

        [Serializable]
        public class UnitAppearance
        {
            [Serializable]
            public class EquippedItem
            {
                // if (bitTest(bitField1, 0x0F))                                
                public int Unknown0F;							        // 32                   Haven't encountered - saw in ASM though.


                public UInt16 ItemCode;									// 16
                public int ItemRowIndex;                                // 8


                // if (bitTest(bitField1, 0x00)) // if (bitField1 & 0x01)
                public int AffixCount;								    // 3
                public readonly List<int> AffixCodes;                   // 32 * AffixCount


                public EquippedItem()
                {
                    AffixCodes = new List<int>();
                }
            };

            [Serializable]
            public class LayerAppearance
            {
                public UInt16 WardrobeLayerCode;					    // 16                   From table WARDROBE_LAYERS
                public bool UnknownBool;							    // 1
                public int UnknownBoolValue;					    	// 2
            };

            internal int EquippedItemCount;								// 3                    This only affects viewing from the select char screen
            public readonly List<EquippedItem> EquippedItems;

            public int ArmorColorSetCode;							    // 16                   From table COLORSETS


            // if (bitTest(bitField1, 0x16))
            public Int64 Unknown16;										// 64 (non-standard read in again)


            // if (bitTest(bitField1, 0x23))
            public Int16 Unknown23ColorSetsCode;					    // 16                   From table COLORSETS


            // if (bitTest(bitField1, 0x11))
            public int WardrobeLayerBaseCount;						    // 4                    Max = 0x0A
            public readonly List<int> WardrobeLayerBases;				// 16 * Count           These are the base WardrobeLayers e.g. player_base_3p, base1_hair, male_base_face_accessory_3


            public int WardrobeAppearanceGroupCount;					// 3                    Max = 0x06
            public readonly List<int> WardrobeAppearanceGroups;			// 16 * Count           e.g. male_body_1, male_head_7, male_hair_1, male_face_hair_3

            public int ColorCount;									    // 4                    Max = 0x03
            public readonly List<int> ColorPaletteIndicies;				// 8 * Count            Order: Body, Hair, ??


            // if (testBit(pUnit->bitField1, 0x10))
            public int LayerAppearanceCount;				            // 16                   This is the model appearance e.g. player_base_3p, base1_hair, male_base_face_accessory_3,
            public readonly List<LayerAppearance> LayerAppearances;     // (17 (+2)) * Count       starter_suit_pants, starter_suit_belt, street_apocalyptic01_torso, starter_suit_boots


            public UnitAppearance()
            {
                EquippedItems = new List<EquippedItem>();
                WardrobeLayerBases = new List<int>();
                WardrobeAppearanceGroups = new List<int>();
                ColorPaletteIndicies = new List<int>();
                LayerAppearances = new List<LayerAppearance>();
            }
        };

        //[Serializable]
        //public class StatBlock
        //{
        //    //public const int MaxParams = 4;

            //[Serializable]
            //public class StatSelector
            //{
            //    public ushort Code;									    // 16               code from table STATS_SELECTOR  (if Context != 1)
            //    public int RowIndex;                                    // 5                row index from same table       (if Context == 1) // I think....
            //    //public short StatCount;                               // 16
            //    public readonly List<Stat> Stats = new List<Stat>();

            //    public ExcelTableRow StatSelectorRow;

            //    public String Name { get { return (StatSelectorRow != null) ? StatSelectorRow.name : null; } }
            //};

            //[Serializable]
            //public class StatName // this isn't a "StatName" - it's another StatSelector like above... not sure what difference is... leaving like this until determined
            //{
            //    public short Unknown1;								    // 16 bits
            //    public StatBlock StatBlock;						        // nameCount * UnitStatBlock stuffs
            //};

            //[Serializable]
            //public class Stat
            //{
            //    //[Serializable]
            //    //public class Param
            //    //{
            //    //    public bool Exists;                               // 1
            //    //    public int BitCount;                              // 6
            //    //    public int ParamOperationsFlags;                  // 2
            //    //    public int ParamShift;                            // 3
            //    //    public bool ParamIsOffset;                        // 1
            //    //    public bool NoTableCode;                          // 1		    if this is set, then don't read the table code below
            //    //    public UInt16 TableCode;                          // 16		    this is the excel table code to use
            //    //};

            //    [Serializable]
            //    public class StatValue
            //    {
            //        public int Param1;
            //        public Object Param1Row;
            //        public int Param2;
            //        public Object Param2Row;
            //        public int Param3;
            //        public Object Param3Row;
            //        public int Param4;
            //        public Object Param4Row;
            //        public int Value;
            //        public Object ValueRow;

            //        public void SetParamAt(int index, int value)
            //        {
            //            if (index < 0 || index >= MaxParams) throw new IndexOutOfRangeException("if (index < 0 || index >= MaxParams (=4))");

            //            switch (index)
            //            {
            //                case 0: Param1 = value; return;
            //                case 1: Param2 = value; return;
            //                case 2: Param3 = value; return;
            //                case 3: Param4 = value; return;
            //            }
            //        }

            //        public int GetParamAt(int index)
            //        {
            //            if (index < 0 || index >= MaxParams) throw new IndexOutOfRangeException("if (index < 0 || index >= MaxParams (=4))");

            //            switch (index)
            //            {
            //                case 0: return Param1;
            //                case 1: return Param2;
            //                case 2: return Param3;
            //                case 3: return Param4;
            //            }

            //            return -1;
            //        }
            //    }

            //    // stat types = 0 = int, 1 = double?, 2 = float, 3+ = ?

            //    //public int RowIndex;                                  // 11
            //    //public UInt16 Code;                                   // 16
            //    //public int ParamCount;							    // 2            see Stats table - paramX columns
            //    //public List<Param> Params;
            //    //public int BitCount;                                  // 6		    bits to read in for stat value

            //    //public int ValueOperationsFlags;                      // 3		    operations upon the stat value (shift/offset/?)
            //    // if (OperationsFlags & 0x01)
            //    //public int ValueShift;                                // 4            valShift column in Stats
            //    // if (OperationsFlags & 0x02)
            //    //public int ValueOffset;                               // 12           valOffset column in Stats
            //    // if (OperationsFlags & 0x04)
            //    //public bool IsOffset;                                 // 1            offset column in Stats (unknown usage)

            //    //public int NoValueTableCode;                          // 2		    this is only tested for >= 3 (fail) and == 0 (get table code) - not sure what 1/2 do
            //    //public UInt16 ValueTableCode;                         // 16		    like in params - read if above is 0x00 - code value of ExcelTable

            //    //public bool HasMultipleValues;                        // 1	        if set, check for repeat number
            //    //public int ValueCount;                                // 7/10		    if Version <= 7, 7 bits, if version > 7, 10 bits

            //    public List<StatValue> Values;
            //    public StatRow StatData;

            //    public ExcelFile ValueTable { get { return (StatData != null) ? StatData.ValueExcelTable : null; } }
            //    public ExcelFile Param1Table { get { return (StatData != null) ? StatData.Param1ExcelTable : null; } }
            //    public ExcelFile Param2Table { get { return (StatData != null) ? StatData.Param2ExcelTable : null; } }
            //    public ExcelFile Param3Table { get { return (StatData != null) ? StatData.Param3ExcelTable : null; } }
            //    public ExcelFile Param4Table { get { return (StatData != null) ? StatData.Param4ExcelTable : null; } }
            //    public Xls.TableCodes ValueTableCode { get { return (StatData != null && StatData.ValueExcelTable != null) ? StatData.ValueExcelTable.Code : Xls.TableCodes.Null; } }
            //    public Xls.TableCodes Param1TableCode { get { return (StatData != null && StatData.Param1ExcelTable != null) ? StatData.Param1ExcelTable.Code : Xls.TableCodes.Null; } }
            //    public Xls.TableCodes Param2TableCode { get { return (StatData != null && StatData.Param2ExcelTable != null) ? StatData.Param2ExcelTable.Code : Xls.TableCodes.Null; } }
            //    public Xls.TableCodes Param3TableCode { get { return (StatData != null && StatData.Param3ExcelTable != null) ? StatData.Param3ExcelTable.Code : Xls.TableCodes.Null; } }
            //    public Xls.TableCodes Param4TableCode { get { return (StatData != null && StatData.Param4ExcelTable != null) ? StatData.Param4ExcelTable.Code : Xls.TableCodes.Null; } }

            //    public string Name { get { return (StatData != null) ? StatData.stat : null; } }
            //    public Xls.StatCodes Code { get { return (StatData != null) ? StatData.code : 0; } }
            //    public int Length { get { return Values.Count; } }

            //    public Stat()
            //    {
            //        //Params = new List<Param>(MaxParams);
            //        Values = new List<StatValue>();
            //    }

            //    public override string ToString()
            //    {
            //        return String.Format("{0} : {1} (0x{2:X4})", Name, Code, (uint)Code);
            //    }

            //    public StatValue this[int index]
            //    {
            //        get { return Values[index]; }
            //    }
            //};

            //public int Version;                                         // 16
            //public StatContext Context;                                 // 3            0 = use code (character file), 1 = use row index (MP item drop or monster etc), 2 = use code (UnitData excel row)

            ////public int StatSelectorCount;                             // 6
            //public List<StatSelector> StatSelectors;                    // 21/32

            ////public int StatCount;                                     // 16
            //public readonly ConcurrentDictionary<Xls.StatCodes, Stat> Stats;

            //public int NameCount;                                       // 8            i think this has something to do with item affix/prefix naming
            //public List<StatName> Names;

            //public StatBlock()
            //{
            //    StatSelectors = new List<StatSelector>();
            //    Stats = new ConcurrentDictionary<Xls.StatCodes, Stat>(2, 50);
            //    //Stats = new Dictionary<Xls.StatCodes, Stat>();
            //    Names = new List<StatName>();
            //}

            //public void SetStat(Xls.StatCodes statCode, int value)
            //{
            //    SetStat(FileManager.GetStatRowFromCode(statCode), value);
            //}

            //public void SetStat(StatRow statData, int value)
            //{
            //    Stats.AddOrUpdate(statData.code,
            //    (code) => _AddStat(statData, value),
            //    (code, stat) =>
            //    {
            //        lock (stat.Values)
            //        {
            //            if (stat.Values.Count == 0)
            //            {
            //                stat.Values.Add(new Stat.StatValue { Value = value });
            //            }
            //            else
            //            {
            //                stat.Values[0].Value = value;
            //            }
            //        }

            //        return stat;
            //    }
            //    );
            //}

            //public Stat.StatValue GetStatValue(Xls.StatCodes statCode)
            //{
            //    Stat stat = Stats.GetOrAdd(statCode, (code) => _AddStat(FileManager.GetStatRowFromCode(statCode), 0));

            //    if (stat.Values.Count == 0) return null;

            //    lock (stat.Values)
            //    {
            //        if (stat.Values.Count > 0) return stat.Values[0];
            //    }

            //    return null;
            //}

            //public int GetStatValueOrDefault(Xls.StatCodes statCode, int defaultValue = 0)
            //{
            //    Stat.StatValue statValue = GetStatValue(statCode);

            //    return (statValue == null) ? defaultValue : statValue.Value;
            //}

            //private Stat _AddStat(StatRow statData, int value)
            //{
            //    Stat stat;
            //    lock (Stats)
            //    {
            //        if (Stats.TryGetValue(statData.code, out stat)) return stat;

            //        stat = new Stat { StatData = statData };
            //        stat.Values.Add(new Stat.StatValue { Value = value });
            //    }

            //    return stat;
      //      //}
       // };

        [Serializable]
        public class Bookmark
        {
            public UInt16 Code;                                         // 16           code value from table "BOOKMARKS"
            public int Offset;                                          // 32           bit offset to bookmark (only 1 entry in Bookmarks table though - "hotkeys")
        }

        [Serializable]
        public class SaveLocation
        {
            //public UInt16 LevelCode;                                  // 16
            //public UInt16 DifficultyCode;                             // 16

            public LevelRow Level;
            public DifficultyRow Difficulty;
        };

        [Serializable]
        public class Hotkey
        {
            public UInt16 Code;                                         // 16           code value from table "TAG"

            public int UnknownCount;                                    // 4            must be <= 0x02
            public bool[] UnknownExists;                                // 1
            public int[] UnknownValues;                                 // 32           not code values - not sure what these are

            public int SkillCount;                                      // 4
            public bool[] SkillExists;                                  // 1
            public int[] SkillCode;                                     // 32           code from SKILLS table

            public int UnitTypeCode;                                    // 32           code from UNITTYPES table
        };
    }
}